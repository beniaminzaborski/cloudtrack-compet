using CloudTrack.Competitions.Application;
using CloudTrack.Competitions.Application.Competitions.Commands;
using CloudTrack.Competitions.Application.Competitions.Queries;
using CloudTrack.Competitions.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Azure.Monitor.OpenTelemetry.Exporter;
using Npgsql;
using CloudTrack.Competitions.Application.Competitions;

internal class Program
{
    private const string ServiceName = "CloudTrack-Competitions-Job";
    private const string ServiceVersion = "1.0.0";

    private static readonly ActivitySource RootActivitySource = new(ServiceName, ServiceVersion);

    private static async Task Main(string[] args)
    {
        var config = CreateConfiguration();
        var tracerProvider = CreateTracerProvider(config);
        var logger = CreateLogger();
        var services = CreateServices(config);

        IMediator mediator = services.GetRequiredService<IMediator>();

        await ProcessCompetitions(mediator, logger);

        tracerProvider.Dispose();
    }

    private static async Task ProcessCompetitions(IMediator mediator, ILogger logger)
    {
        using var activity = RootActivitySource.StartActivity("GetOpenedForRegistrationCompetitionListQuery");
        activity?.SetStartTime(DateTime.UtcNow);
        activity?.AddEvent(new ActivityEvent("Fetching competitions"));

        var competitions = await FetchCompetitions(mediator);

        activity?.AddEvent(new ActivityEvent("Competitions fetched"));
        await CompleteRegistrations(mediator, logger, competitions);

        activity?.SetEndTime(DateTime.UtcNow);
        activity?.SetStatus(ActivityStatusCode.Ok);
    }

    private static async Task<IEnumerable<CompetitionDto>> FetchCompetitions(IMediator mediator)
    {
        var query = new GetOpenedForRegistrationCompetitionListQuery();
        return await mediator.Send(query);
    }

    private static async Task CompleteRegistrations(IMediator mediator, ILogger logger, IEnumerable<CompetitionDto> competitions)
    {
        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        foreach (var competition in competitions)
        {
            using var nestedActivity = StartNestedActivity("CompleteRegistrationCommand", competition);
            if (competition.StartAt.Date == tomorrow)
            {
                await CompleteRegistration(mediator, logger, competition, nestedActivity);
            }
            else
            {
                logger.LogInformation("Nothing to do with {Name}.", competition.Name);
            }

            EndActivity(nestedActivity);
        }
    }

    private static async Task CompleteRegistration(IMediator mediator, ILogger logger, CompetitionDto competition, Activity? activity)
    {
        activity?.AddEvent(new ActivityEvent($"Completing registration for {competition.Name}"));
        logger.LogInformation("Completing registration for {Name}.", competition.Name);

        var command = new CompleteRegistrationCommand(competition.Id);
        await mediator.Send(command);

        logger.LogInformation("Registration completed for {Name}.", competition.Name);
        activity?.AddEvent(new ActivityEvent($"Registration completed for {competition.Name}"));
    }

    private static Activity? StartNestedActivity(string name, CompetitionDto competition)
    {
        var nestedActivity = RootActivitySource.StartActivity(name, ActivityKind.Internal);
        nestedActivity?.SetStartTime(DateTime.UtcNow);
        nestedActivity?.SetTag("Id", competition.Id);
        nestedActivity?.SetTag("Name", competition.Name);
        nestedActivity?.SetTag("Status", competition.Status);
        return nestedActivity;
    }

    private static void EndActivity(Activity? activity)
    {
        if (activity == null) return;
        activity.SetEndTime(DateTime.UtcNow);
        activity.SetStatus(ActivityStatusCode.Ok);
    }

    private static IConfiguration CreateConfiguration() => 
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

    private static ILogger CreateLogger() => 
        LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger("Job");

    private static TracerProvider CreateTracerProvider(IConfiguration config) => 
        Sdk.CreateTracerProviderBuilder()
            .AddSource(ServiceName)
            .AddNpgsql()
            .AddMassTransitInstrumentation().AddSource("MassTransit")
            .AddConsoleExporter()
            .AddAzureMonitorTraceExporter(cfg => cfg.ConnectionString = config.GetConnectionString("ApplicationInsights"))
            .Build();

    private static ServiceProvider CreateServices(IConfiguration config) => 
        new ServiceCollection()
            .AddApplication()
            .AddInfrastructure(config)
            .BuildServiceProvider();
}
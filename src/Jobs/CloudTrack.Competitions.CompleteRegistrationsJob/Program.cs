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

        using (var activity = RootActivitySource.StartActivity("GetOpenedForRegistrationCompetitionListQuery"))
        {
            activity?.SetStartTime(DateTime.UtcNow);

            activity?.AddEvent(new ActivityEvent("Fetching competitions"));
            
            var query = new GetOpenedForRegistrationCompetitionListQuery();
            var onlyOpenedCompetitions = await mediator.Send(query);
            
            activity?.AddEvent(new ActivityEvent("Competitions fetched"));

            var tomorrow = DateTime.UtcNow.Date.AddDays(1);

            foreach (var competition in onlyOpenedCompetitions)
            {
                using var nestedActivity = RootActivitySource.StartActivity("CompleteRegistrationCommand", ActivityKind.Internal, activity.Context);
                nestedActivity?.SetStartTime(DateTime.UtcNow);

                if (competition.StartAt.Date == tomorrow)
                {
                    nestedActivity?.SetTag("Id", competition.Id);
                    nestedActivity?.SetTag("Name", competition.Name);
                    nestedActivity?.SetTag("Status", competition.Status);
                    nestedActivity?.AddEvent(new ActivityEvent($"Completing registration for {competition.Name}"));

                    logger.LogInformation("Completing registration for {Name}.", competition.Name);
                    var command = new CompleteRegistrationCommand(competition.Id);
                    await mediator.Send(command);
                    logger.LogInformation("Registration completed for {Name}.", competition.Name);
                    nestedActivity?.AddEvent(new ActivityEvent($"Registration completed for {competition.Name}"));
                }
                else
                {
                    logger.LogInformation("Nothing to do with {Name}.", competition.Name);
                }

                nestedActivity?.SetEndTime(DateTime.UtcNow);
                nestedActivity?.SetStatus(ActivityStatusCode.Ok);
            }

            activity?.SetEndTime(DateTime.UtcNow);
            activity?.SetStatus(ActivityStatusCode.Ok);
        }

        tracerProvider.Dispose();
    }

    private static IConfiguration CreateConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
    }

    private static ILogger CreateLogger()
    {
        return LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger("Job");
    }

    private static TracerProvider CreateTracerProvider(IConfiguration config)
    {
        var appInsightsConnectionString = config.GetConnectionString("ApplicationInsights");

        return Sdk.CreateTracerProviderBuilder()
            .AddSource(ServiceName)
            .AddNpgsql()
            .AddMassTransitInstrumentation().AddSource("MassTransit")
            .AddConsoleExporter()
            .AddAzureMonitorTraceExporter(cfg => cfg.ConnectionString = appInsightsConnectionString)
            .Build();
    }

    private static ServiceProvider CreateServices(IConfiguration config)
    {
        IServiceCollection services = new ServiceCollection();
        services
            .AddApplication()
            .AddInfrastructure(config);

        return services
            .BuildServiceProvider();
    }
}
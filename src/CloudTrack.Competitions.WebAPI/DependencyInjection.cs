using CloudTrack.Competitions.WebAPI.ExceptionsHandling;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Npgsql;
using Azure.Monitor.OpenTelemetry.Exporter;

namespace CloudTrack.Competitions.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(GlobalExceptionFilter));
            options.SuppressAsyncSuffixInActionNames = false;
        });

        return services;
    }

    public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration config, string serviceName, string serviceVersion)
    {
        return services
            .AddOpenTelemetry()
            .WithTracing(builder => builder
                .AddSource(serviceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: serviceVersion))
                .AddAspNetCoreInstrumentation()
                .AddNpgsql()
                .AddMassTransitInstrumentation().AddSource("MassTransit")
                //.AddConsoleExporter()
                .AddTraceExporter(config))
            .WithMetrics(builder => builder
                .AddMeter(serviceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: serviceVersion))
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                //.AddConsoleExporter()
                .AddMetricsExporter(config))
            .Services;
    }

    private static TracerProviderBuilder AddTraceExporter(this TracerProviderBuilder tracerProviderBuilder, IConfiguration config)
    {
        var useOtlpExporter = config.GetValue<bool>("OpenTelemetry:UseOtlpExporter");
        if(useOtlpExporter)
        {
            return tracerProviderBuilder.AddOtlpExporter();
        }
        else
        {
            var appInsightsConnectionString = GetApplicationInsightsConnectionString(config);
            return tracerProviderBuilder.AddAzureMonitorTraceExporter(cfg => cfg.ConnectionString = appInsightsConnectionString);
        }
    }

    private static MeterProviderBuilder AddMetricsExporter(this MeterProviderBuilder meterProviderBuilder, IConfiguration config)
    {
        var useOtlpExporter = config.GetValue<bool>("OpenTelemetry:UseOtlpExporter");
        if (useOtlpExporter)
        {
            return meterProviderBuilder.AddOtlpExporter();
        }
        else
        {
            var appInsightsConnectionString = GetApplicationInsightsConnectionString(config);
            return meterProviderBuilder.AddAzureMonitorMetricExporter(cfg => cfg.ConnectionString = appInsightsConnectionString);
        }
    }

    private static string? GetApplicationInsightsConnectionString(IConfiguration config) => config.GetConnectionString("ApplicationInsights");
}

namespace API.Configurations;

internal static class LogConfiguration
{
    public static void AddLogConfiguration(this WebApplicationBuilder builder)
    {
        builder.AddOpenTelemetryConfiguration();
        builder.AddSerilogConfiguration();
    }

    public static IApplicationBuilder UseApiRequestLogging(this IApplicationBuilder app) =>
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diag, httpContext) =>
            {
                var req = httpContext.Request;

                diag.Set("RequestHost", req.Host.Value);
                diag.Set("RequestScheme", req.Scheme);
                diag.Set("RequestProtocol", req.Protocol);
                diag.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());

                var endpoint = httpContext.GetEndpoint();
                if (endpoint is not null)
                    diag.Set("EndpointName", endpoint.DisplayName);
            };

            options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex is not null)
                    return LogEventLevel.Error;

                var statusCode = httpContext.Response.StatusCode;

                if (statusCode >= 500)
                    return LogEventLevel.Error;
                if (statusCode >= 400)
                    return LogEventLevel.Warning;

                var path = httpContext.Request.Path;
                if (path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWithSegments("/ready", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase))
                    return LogEventLevel.Debug;

                return LogEventLevel.Information;

            };
        });

    private static void AddOpenTelemetryConfiguration(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration["AzureMonitor:ConnectionString"]
            ?? builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

        if (!string.IsNullOrWhiteSpace(connectionString))
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(r => r.AddService(builder.Environment.ApplicationName))
                .UseAzureMonitor(options => options.ConnectionString = connectionString)
                .WithTracing(tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation();
                    tracing.AddHttpClientInstrumentation();
                    tracing.AddSqlClientInstrumentation(options => options.RecordException = true);
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation();
                    metrics.AddHttpClientInstrumentation();
                });
    }

    private static void AddSerilogConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, config) =>
        {
            config
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);
        });
    }
}
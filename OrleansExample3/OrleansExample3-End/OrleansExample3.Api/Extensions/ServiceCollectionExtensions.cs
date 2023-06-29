using Microsoft.ApplicationInsights.Extensibility;
using OrleansExample3.Api.Telemetry;

namespace OrleansExample3.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddApplicationInsights(this IServiceCollection services, string applicationName)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ITelemetryInitializer>(_ => new ApplicationMapNodeNameInitializer(applicationName));
        }
    }
}

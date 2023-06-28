using Microsoft.ApplicationInsights.Extensibility;
using OrleansExample2.Api.Telemetry;

namespace OrleansExample2.Api.Extensions
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

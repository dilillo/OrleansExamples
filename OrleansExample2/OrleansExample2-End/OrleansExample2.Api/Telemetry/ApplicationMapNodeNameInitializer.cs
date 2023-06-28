using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace OrleansExample2.Api.Telemetry
{
    public class ApplicationMapNodeNameInitializer : ITelemetryInitializer
    {
        private readonly string _name;

        internal ApplicationMapNodeNameInitializer(string name) => _name = name;

        public void Initialize(ITelemetry telemetry) =>
            telemetry.Context.Cloud.RoleName = _name;
    }
}

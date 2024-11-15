using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.Api.Infrastructure.HealthChecks
{
    public class HealthCheckReportEntry
    {
        public IReadOnlyDictionary<string, object> Data { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public string Exception { get; set; }

        public HealthStatus Status { get; set; }
    }
}

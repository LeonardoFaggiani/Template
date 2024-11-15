using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.Api.Infrastructure.HealthChecks
{
    public class HealthCheckReport
    {
        public HealthStatus Status { get; set; }

        public TimeSpan TotalDuration { get; set; }

        public Dictionary<string, HealthCheckReportEntry> Entries { get; }

        public HealthCheckReport(HealthReport report)
        {
            Entries = new Dictionary<string, HealthCheckReportEntry>();
            TotalDuration = report.TotalDuration;
            Status = report.Status;
            foreach (KeyValuePair<string, HealthReportEntry> entry in report.Entries)
            {
                HealthCheckReportEntry healthCheckReportEntry = new HealthCheckReportEntry
                {
                    Data = entry.Value.Data,
                    Description = entry.Value.Description,
                    Duration = entry.Value.Duration,
                    Status = entry.Value.Status
                };
                if (entry.Value.Exception != null)
                {
                    string text2 = (healthCheckReportEntry.Exception = entry.Value.Exception.Message.ToString());
                    healthCheckReportEntry.Description = entry.Value.Description ?? text2;
                }

                Entries.Add(entry.Key, healthCheckReportEntry);
            }
        }
    }
}

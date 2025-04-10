using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.Api.Infrastructure.HealthChecks
{
    public static class HealthCheckResponseWriter
    {
        private static readonly Lazy<JsonSerializerOptions> options = new Lazy<JsonSerializerOptions>(() => CreateJsonOptions());

        public static async Task Write(HttpContext httpContext, HealthReport report)
        {
            if (report != null)
            {
                httpContext.Response.ContentType = "application/json";
                HealthCheckReport value = new HealthCheckReport(report);
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(value, options.Value));
            }
            else
            {
                await httpContext.Response.WriteAsync("{}");
            }
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
        }
    }
}

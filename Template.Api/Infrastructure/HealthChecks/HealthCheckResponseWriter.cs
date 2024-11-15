using Microsoft.Extensions.Diagnostics.HealthChecks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Template.Api.Infrastructure.HealthChecks
{
    public static class HealthCheckResponseWriter
    {
        private static readonly Lazy<JsonSerializerSettings> options = new Lazy<JsonSerializerSettings>(() => CreateJsonOptions());

        public static async Task Write(HttpContext httpContext, HealthReport report)
        {
            if (report != null)
            {
                httpContext.Response.ContentType = "application/json";
                HealthCheckReport value = new HealthCheckReport(report);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(value, options.Value));
            }
            else
            {
                await httpContext.Response.WriteAsync("{}");
            }
        }

        private static JsonSerializerSettings CreateJsonOptions()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { (JsonConverter)new StringEnumConverter() }
            };
        }
    }
}

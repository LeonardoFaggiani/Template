using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Template.Api.Sdk.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> GetAsync<TResponse>(this HttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                httpResponseMessage = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }

            using (httpResponseMessage)
            {
                return await httpResponseMessage.Content.ReadAsAsync<TResponse>();
            }
        }

        private static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            using Stream stream = await httpContent.ReadAsStreamAsync();
            using StreamReader reader = new StreamReader(stream);
            using JsonTextReader reader2 = new JsonTextReader(reader);
            return new JsonSerializer().Deserialize<T>(reader2);
        }
    }
}

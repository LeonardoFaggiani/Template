using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Template.Api.Dto.Samples;
using Template.Api.Sdk.Extensions;
using Template.Api.Sdk.Interfaces;

namespace Template.Api.Sdk
{
    public class SamplesClient : ISamplesClient
    {
        private readonly HttpClient httpClient;

        public SamplesClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<SampleQueryResponse> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await httpClient.GetAsync<SampleQueryResponse>($"Sample?id={id}", cancellationToken);
        }
    }
}

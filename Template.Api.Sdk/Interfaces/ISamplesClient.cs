using System.Threading;
using System.Threading.Tasks;

using Template.Api.Dto.Samples;

namespace Template.Api.Sdk.Interfaces
{
    public interface ISamplesClient
    {
        Task<SampleQueryResponse> GetAsync(int id, CancellationToken cancellationToken = default);
    }
}

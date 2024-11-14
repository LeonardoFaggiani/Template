using System.Threading;
using System.Threading.Tasks;

using Template.Api.Dto.Samples;

namespace Template.Api.Sdk.Interfaces
{
    public interface ISamplesClient
    {
        /// <summary>
        /// This is a sample
        /// </summary>        
        Task<SampleQueryResponse> GetAsync(int id, CancellationToken cancellationToken = default);
    }
}

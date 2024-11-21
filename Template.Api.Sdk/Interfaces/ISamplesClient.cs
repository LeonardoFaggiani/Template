using System;
using System.Threading;
using System.Threading.Tasks;

using Template.Api.Dto.Samples;

namespace Template.Api.Sdk.Interfaces
{
    public interface ISamplesClient
    {
        /// <summary>
        /// Get all samples
        /// </summary>                
        /// <param name="cancellationToken"></param>
        Task<SampleQueryResponse> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Sample by Id
        /// </summary>        
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        Task<SampleByIdQueryResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a sample
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        Task PostSampleAsync(CancellationToken cancellationToken = default);
    }
}

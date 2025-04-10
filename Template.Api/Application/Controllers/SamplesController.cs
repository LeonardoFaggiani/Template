using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Template.Api.Application.Controllers.Base;
using Template.Api.Application.Samples.Commands;
using Template.Api.Application.Samples.Queries;
using Template.Api.Dto.Samples;
using Template.Api.Dto.Samples.Request;

namespace Template.Api.Application.Controllers
{
    public class SamplesController : ApiController
    {
        public SamplesController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Get all samples
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json", Type = typeof(SampleQueryResponse))]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var query = new SampleQuery();

            return Ok(await mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Get Sample by Id
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("by-id")]
        [AllowAnonymous]
        [Produces("application/json", Type = typeof(SampleByIdQueryResponse))]
        public async Task<IActionResult> GetByIdAsync([FromQuery] SampleByIdRequest request, CancellationToken cancellationToken)
        {
            var query = new SampleByIdQuery(request);

            return Ok(await mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Create a sample
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [AllowAnonymous]
        [Route("new")]
        public async Task<IActionResult> PostSampleAsync([FromBody] CreateSampleRequest request, CancellationToken cancellationToken)
        {
            var query = new NewSampleCommand(request);

            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}
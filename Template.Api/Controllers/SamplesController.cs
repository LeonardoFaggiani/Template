using MediatR;

using Microsoft.AspNetCore.Mvc;

using Template.Api.Application.Samples.Queries;
using Template.Api.Controllers.Base;
using Template.Api.Dto.Samples;
using Template.Api.Dto.Samples.Request;

namespace Template.Api.Controllers
{
    public class SamplesController : ApiController
    {
        public SamplesController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// This is a sample
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Produces("application/json", Type = typeof(SampleQueryResponse))]
        public async Task<IActionResult> GetAsync([FromQuery] SampleRequest request, CancellationToken cancellationToken)
        {
            var query = new SampleQuery(request);

            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}
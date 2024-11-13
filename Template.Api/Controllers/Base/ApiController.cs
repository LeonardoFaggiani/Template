using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class ApiController : ControllerBase
    {
        protected readonly IMediator mediator;

        protected ApiController(IMediator mediator)
        {
            Guard.IsNotNull(mediator, nameof(mediator));

            this.mediator = mediator;
        }
    }
}

using MediatR;

using Template.Api.Dto.Samples.Request;

namespace Template.Api.Application.Samples.Commands
{
    public record NewSampleCommand(CreateSampleRequest request) : IRequest
    { }
}

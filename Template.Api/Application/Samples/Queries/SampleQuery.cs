using MediatR;

using Template.Api.Dto.Samples;
using Template.Api.Dto.Samples.Request;

namespace Template.Api.Application.Samples.Queries
{
    public record SampleQuery(SampleRequest request) : IRequest<SampleQueryResponse>
    { }
}

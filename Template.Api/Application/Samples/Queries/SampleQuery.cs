using MediatR;

using Template.Api.Dto.Samples;

namespace Template.Api.Application.Samples.Queries
{
    public class SampleQuery : IRequest<SampleQueryResponse>
    { }
}

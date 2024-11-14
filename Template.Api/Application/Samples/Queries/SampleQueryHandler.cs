using MediatR;

using Template.Api.Dto.Samples;

namespace Template.Api.Application.Samples.Queries
{
    public class SampleQueryHandler : IRequestHandler<SampleQuery, SampleQueryResponse>
    {
        public async Task<SampleQueryResponse> Handle(SampleQuery query, CancellationToken cancellationToken)
        {
            return new SampleQueryResponse(Random.Shared.Next(0, 100));
        }
    }
}

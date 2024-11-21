using AutoMapper;

using CommunityToolkit.Diagnostics;

using MediatR;

using Template.Api.Domian;
using Template.Api.Dto.Samples;
using Template.Api.Infrastructure.Repositories;

namespace Template.Api.Application.Samples.Queries
{
    public class SampleByIdQueryHandler : IRequestHandler<SampleByIdQuery, SampleByIdQueryResponse>
    {
        private readonly ISampleRepository sampleRepository;
        private readonly IMapper mapper;

        public SampleByIdQueryHandler(ISampleRepository sampleRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(sampleRepository, nameof(sampleRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.sampleRepository = sampleRepository;
            this.sampleRepository = sampleRepository;
        }

        public async Task<SampleByIdQueryResponse> Handle(SampleByIdQuery query, CancellationToken cancellationToken)
        {
            Sample sample = await this.sampleRepository.GetByIdAsync(query.request.Id, cancellationToken);

            return this.mapper.Map<Sample, SampleByIdQueryResponse>(sample);
        }
    }
}
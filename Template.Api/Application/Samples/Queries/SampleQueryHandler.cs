using AutoMapper;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Template.Api.Domian;
using Template.Api.Dto;
using Template.Api.Dto.Samples;
using Template.Api.Infrastructure.Repositories;

namespace Template.Api.Application.Samples.Queries
{
    public class SampleQueryHandler : IRequestHandler<SampleQuery, SampleQueryResponse>
    {
        private readonly ISampleRepository sampleRepository;
        private readonly IMapper mapper;

        public SampleQueryHandler(ISampleRepository sampleRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(sampleRepository, nameof(sampleRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.sampleRepository = sampleRepository;
            this.mapper = mapper;
        }

        public async Task<SampleQueryResponse> Handle(SampleQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Sample> samples = await this.sampleRepository.All().ToListAsync(cancellationToken);

            var samplesDto = this.mapper.Map<IEnumerable<Sample>, IEnumerable<SampleDto>>(samples);

            return new SampleQueryResponse(samplesDto);
        }
    }
}
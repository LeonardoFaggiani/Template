using System.Collections.Generic;

namespace Template.Api.Dto.Samples
{
    public class SampleQueryResponse
    {
        public SampleQueryResponse(IEnumerable<SampleDto> samples)
        {
            this.Samples = samples;
        }

        public IEnumerable<SampleDto> Samples { get; }
    }
}
using System;

namespace Template.Api.Dto.Samples
{
    public class SampleByIdQueryResponse
    {
        public SampleByIdQueryResponse(Guid id, string description)
        {
            this.Id = id;
            this.Description = description;
        }

        public Guid Id { get; }
        public string Description { get; }
    }
}

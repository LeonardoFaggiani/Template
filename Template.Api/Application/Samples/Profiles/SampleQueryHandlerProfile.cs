using AutoMapper;

using Template.Api.Domian;
using Template.Api.Dto;
using Template.Api.Dto.Samples;

namespace Template.Api.Application.Samples.Profiles
{
    public class SampleQueryHandlerProfile : Profile
    {
        public SampleQueryHandlerProfile()
        {
            CreateMap<Sample, SampleByIdQueryResponse>(MemberList.None);

            CreateMap<Sample, SampleDto>(MemberList.None);
        }
    }
}

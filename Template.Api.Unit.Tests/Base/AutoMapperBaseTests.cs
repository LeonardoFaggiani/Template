using AutoMapper;

namespace Template.Api.Unit.Tests.Base
{
    public class AutoMapperBaseTests<TProfile> where TProfile : Profile, new()
    {
        public IMapper Mapper;
        public MapperConfiguration MapperConfiguration;

        public AutoMapperBaseTests()
        {
            MapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<TProfile>());
            Mapper = MapperConfiguration.CreateMapper();
        }
    }
}

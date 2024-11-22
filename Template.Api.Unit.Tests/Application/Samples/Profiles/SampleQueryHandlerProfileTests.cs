using Template.Api.Application.Samples.Profiles;
using Template.Api.Unit.Tests.Base;

namespace Template.Api.Unit.Tests.Application.Samples.Profiles
{
    public class SampleQueryHandlerProfileTests : AutoMapperBaseTests<SampleQueryHandlerProfile>
    {
        [Fact]
        public void Should_be_correctly_configured()
        {
            // Assert
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
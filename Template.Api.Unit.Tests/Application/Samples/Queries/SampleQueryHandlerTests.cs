using AutoMapper;

using MockQueryable;

using Moq;

using Template.Api.Application.Samples.Queries;
using Template.Api.Domian;
using Template.Api.Dto;
using Template.Api.Infrastructure.Repositories;
using Template.Api.Unit.Tests.Base;

namespace Template.Api.Unit.Tests.Application.Samples.Queries
{
    public class SampleQueryHandlerTests : BaseTestClass<SampleQueryHandler>
    {
        private readonly ISampleRepository SampleRepository;
        private readonly IMapper Mapper;

        public SampleQueryHandlerTests()
        {
            this.SampleRepository = Mock.Of<ISampleRepository>();
            this.Mapper = Mock.Of<IMapper>();

            var samples = new List<Sample> { new Sample("Testing") };

            Mock.Get(this.SampleRepository).Setup(x => x.All()).Returns(samples.AsQueryable().BuildMock());
            Mock.Get(this.Mapper).Setup(x => x.Map<IEnumerable<Sample>, IEnumerable<SampleDto>>(It.IsAny<IEnumerable<Sample>>())).Returns(new List<SampleDto> { new SampleDto() { Id = Guid.NewGuid(), Description = "Testing" } });

            this.Sut = new SampleQueryHandler(this.SampleRepository, this.Mapper);
        }

        public class The_Constructor : SampleQueryHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_sampleRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new SampleQueryHandler(null, this.Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_mapper_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new SampleQueryHandler(this.SampleRepository, null));
            }
        }

        public class The_Method_Handle : SampleQueryHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(this.SampleRepository).Setup(x => x.Add(It.IsAny<Sample>()));
            }

            [Fact]
            public async Task Should_verify_if_all_service_is_called()
            {
                //Arrange
                var query = new SampleQuery();

                //Act
                await this.Sut.Handle(query, CancellationToken);

                //Assert
                Mock.Get(this.SampleRepository).Verify(x => x.All(), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_map_sample_to_sampleByIdQueryResponse_is_called()
            {
                //Arrange
                var query = new SampleQuery();

                //Act
                await this.Sut.Handle(query, CancellationToken);

                //Assert
                Mock.Get(this.Mapper).Verify(x => x.Map<IEnumerable<Sample>, IEnumerable<SampleDto>>(It.IsAny<IEnumerable<Sample>>()), Times.Once);
            }
        }
    }
}
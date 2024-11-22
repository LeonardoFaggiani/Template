using AutoMapper;

using Moq;

using Template.Api.Application.Samples.Queries;
using Template.Api.Domian;
using Template.Api.Dto.Samples;
using Template.Api.Dto.Samples.Request;
using Template.Api.Infrastructure.Repositories;
using Template.Api.Unit.Tests.Base;

namespace Template.Api.Unit.Tests.Application.Samples.Queries
{
    public class SampleByIdQueryHandlerTests : BaseTestClass<SampleByIdQueryHandler>
    {
        private readonly ISampleRepository SampleRepository;
        private readonly IMapper Mapper;

        public SampleByIdQueryHandlerTests()
        {
            this.SampleRepository = Mock.Of<ISampleRepository>();
            this.Mapper = Mock.Of<IMapper>();

            Mock.Get(this.SampleRepository).Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken)).ReturnsAsync(new Sample("Testing"));
            Mock.Get(this.Mapper).Setup(x => x.Map<Sample, SampleByIdQueryResponse>(It.IsAny<Sample>())).Returns(new SampleByIdQueryResponse(Guid.NewGuid(), "Testing"));

            this.Sut = new SampleByIdQueryHandler(this.SampleRepository, this.Mapper);
        }

        public class The_Constructor : SampleByIdQueryHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_sampleRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new SampleByIdQueryHandler(null, this.Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_mapper_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new SampleByIdQueryHandler(this.SampleRepository, null));
            }
        }

        public class The_Method_Handle : SampleByIdQueryHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(this.SampleRepository).Setup(x => x.Add(It.IsAny<Sample>()));
            }

            [Fact]
            public async Task Should_verify_if_getByIdAsync_service_is_called()
            {
                //Arrange
                var request = new SampleByIdRequest();
                var query = new SampleByIdQuery(request);

                //Act
                await this.Sut.Handle(query, CancellationToken);

                //Assert
                Mock.Get(this.SampleRepository).Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_map_sample_to_sampleByIdQueryResponse_is_called()
            {
                //Arrange
                var request = new SampleByIdRequest();
                var query = new SampleByIdQuery(request);

                //Act
                await this.Sut.Handle(query, CancellationToken);

                //Assert
                Mock.Get(this.Mapper).Verify(x => x.Map<Sample, SampleByIdQueryResponse>(It.IsAny<Sample>()), Times.Once);
            }
        }
    }
}
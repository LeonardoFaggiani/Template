using MediatR;

using Moq;

using NetDevPack.Data;

using Template.Api.Application.Samples.Commands;
using Template.Api.Domian;
using Template.Api.Dto.Samples.Request;
using Template.Api.Infrastructure.Repositories;
using Template.Api.Unit.Tests.Base;

namespace Template.Api.Unit.Tests.Application.Samples.Commands
{
    public class NewSampleCommandHandlerTests : BaseTestClass<IRequestHandler<NewSampleCommand>>
    {
        private readonly ISampleRepository SampleRepository;
        private readonly IUnitOfWork UnitOfWork;

        public NewSampleCommandHandlerTests()
        {
            this.SampleRepository = Mock.Of<ISampleRepository>();
            this.UnitOfWork = Mock.Of<IUnitOfWork>();

            Mock.Get(this.SampleRepository).Setup(x => x.UnitOfWork).Returns(this.UnitOfWork);

            this.Sut = new NewSampleCommandHandler(this.SampleRepository);
        }

        public class The_Constructor : NewSampleCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_sampleRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new NewSampleCommandHandler(null));
            }
        }

        public class The_Method_Handle : NewSampleCommandHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(this.SampleRepository).Setup(x => x.Add(It.IsAny<Sample>()));
            }

            [Fact]
            public async Task Should_verify_if_sample_is_added_to_repository()
            {
                //Arrange
                var request = new CreateSampleRequest();
                var command = new NewSampleCommand(request);

                //Act
                await this.Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(this.SampleRepository).Verify(x => x.Add(It.IsAny<Sample>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_sample_is_committed()
            {
                //Arrange
                var request = new CreateSampleRequest();
                var command = new NewSampleCommand(request);

                //Act
                await this.Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(this.SampleRepository.UnitOfWork).Verify(x => x.Commit(), Times.Once);
            }
        }
    }
}
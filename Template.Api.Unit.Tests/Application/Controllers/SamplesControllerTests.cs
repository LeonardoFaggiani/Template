using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;
using Template.Api.Application.Controllers;
using Template.Api.Application.Samples.Commands;
using Template.Api.Application.Samples.Queries;
using Template.Api.Dto;
using Template.Api.Dto.Samples;
using Template.Api.Dto.Samples.Request;
using Template.Api.Unit.Tests.Base;

namespace Template.Api.Unit.Tests.Application.Controllers
{
    public class SamplesControllerTests : BaseTestClass<SamplesController>
    {
        public SamplesControllerTests()
        {
            var samplesDto = new List<SampleDto>()
            {
                new SampleDto()
                {
                    Id = new Guid("5e832cf5-6934-485f-bd8a-207eb3eadc4c"),
                    Description = "Tests"
                }
            };

            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<SampleQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SampleQueryResponse(samplesDto));
            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<SampleByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SampleByIdQueryResponse(new Guid("5e832cf5-6934-485f-bd8a-207eb3eadc4c"), "Tests"));
            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<NewSampleCommand>(), It.IsAny<CancellationToken>()));

            this.Sut = new SamplesController(Mediator);
        }

        public class The_Constructor : SamplesControllerTests
        {
            [Fact]
            public void Should_throw_an_argumentNullException_when_mediator_is_null()
            {
                // act & assert
                Assert.Throws<ArgumentNullException>(() => new SamplesController(null));
            }
        }

        public class The_Method_GetAsync : SamplesControllerTests
        {
            [Fact]
            public async Task Should_verify_if_mediator_was_called()
            {
                //Act                
                await this.Sut.GetAsync(CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<SampleQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await this.Sut.GetAsync(CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<SampleQueryResponse>().Samples.First().Description.Should().Be("Tests");
            }
        }

        public class The_Method_GetByIdAsync : SamplesControllerTests
        {
            private SampleByIdRequest Request;

            public The_Method_GetByIdAsync()
            {
                this.Request = new SampleByIdRequest()
                {
                    Id = new Guid("5e832cf5-6934-485f-bd8a-207eb3eadc4c"),
                };
            }

            [Fact]
            public async Task Should_verify_if_mediator_was_called()
            {
                //Act                
                await this.Sut.GetByIdAsync(this.Request, CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<SampleByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await this.Sut.GetByIdAsync(this.Request, CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<SampleByIdQueryResponse>().Id.Should().Be("5e832cf5-6934-485f-bd8a-207eb3eadc4c");
            }
        }

        public class The_Method_PostSampleAsync : SamplesControllerTests
        {
            private CreateSampleRequest Request;

            public The_Method_PostSampleAsync()
            {
                this.Request = new CreateSampleRequest()
                {
                    Description = "Tests"
                };
            }

            [Fact]
            public async Task Should_verify_if_mediator_was_called()
            {
                //Act                
                await this.Sut.PostSampleAsync(this.Request, CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<NewSampleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await this.Sut.PostSampleAsync(this.Request, CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<IActionResult>();
            }
        }
    }
}
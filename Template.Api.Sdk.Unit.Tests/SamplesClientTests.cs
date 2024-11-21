using Moq;

using Template.Api.Dto.Samples;
using Template.Api.Sdk.Interfaces;
using Template.Api.Sdk.Unit.Tests.Extensions;

namespace Template.Api.Sdk.Unit.Tests
{
    public class SamplesClientTests
    {
        private static string BaseAddress => "http://test.com/api/";
        private static CancellationToken CancellationToken => CancellationToken.None;
        private Mock<HttpMessageHandler> HandlerMock { get; set; }
        private HttpClient HttpClient { get; set; }
        private ISamplesClient Sut { get; set; }

        public SamplesClientTests()
        {
            HandlerMock = new Mock<HttpMessageHandler>();

            HttpClient = new HttpClient(HandlerMock.Object)
            {
                BaseAddress = new Uri(BaseAddress)
            };

            Sut = new SamplesClient(HttpClient);
        }

        public class TheConstructor : SamplesClientTests
        {
            [Fact]
            public void Should_throw_argument_null_exception_when_http_client_is_null()
            {
                //Arrange
                HttpClient = null;

                //Act & Assert
                Assert.Throws<ArgumentNullException>("httpClient", () => new SamplesClient(HttpClient));
            }
        }

        public class TheMethod_GetByIdAsync : SamplesClientTests
        {
            [Fact]
            public async Task Should_call_its_httpClient_with_proper_url()
            {
                // Arrange
                var expected = new SampleByIdQueryResponse(Guid.NewGuid(), "test");

                HandlerMock.SetupGetReturnValue(expected);

                var expectedUri = $"{BaseAddress}Samples/by-id?id=5e832cf5-6934-485f-bd8a-207eb3eadc4c";

                // Act
                var result = await Sut.GetByIdAsync(new Guid("5e832cf5-6934-485f-bd8a-207eb3eadc4c"), CancellationToken);

                // Assert
                HandlerMock.VerifyCalledWithGetMethod(result, expected, expectedUri);
            }
        }
    }
}

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

        public class TheMethod_GetAsync : SamplesClientTests
        {
            [Fact]
            public async Task Should_call_its_httpClient_with_proper_url()
            {
                // Arrange
                var expected = new SampleQueryResponse(2);

                HandlerMock.SetupGetReturnValue(expected);

                var expectedUri = $"{BaseAddress}Samples?id=1";

                // Act
                var result = await Sut.GetAsync(1, CancellationToken);

                // Assert
                HandlerMock.VerifyCalledWithGetMethod(result, expected, expectedUri);
            }
        }
    }
}

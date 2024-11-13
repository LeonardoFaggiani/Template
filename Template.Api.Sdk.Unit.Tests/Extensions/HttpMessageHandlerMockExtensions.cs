using System.Net;

using FluentAssertions;

using Moq;
using Moq.Language.Flow;
using Moq.Protected;

using Newtonsoft.Json;

namespace Template.Api.Sdk.Unit.Tests.Extensions
{
    public static class HttpMessageHandlerMockExtensions
    {
        public static IReturnsResult<HttpMessageHandler> SetupGetReturnValue(this Mock<HttpMessageHandler> mock, object expected)
        {
            return SetupReturnValueWithStatusCode(mock, expected, HttpStatusCode.OK);
        }
        public static void VerifyCalledWithGetMethod<TExpectation>(this Mock<HttpMessageHandler> mock, object actual, TExpectation expected, string expectedUri, int times = 1)
        {
            actual.Should().BeEquivalentTo<TExpectation>(expected, "", Array.Empty<object>());
            mock.VerifySendAsyncWasCalledWithMethod(HttpMethod.Get, expectedUri, times);
        }
        private static void VerifySendAsyncWasCalledWithMethod(this Mock<HttpMessageHandler> mock, HttpMethod expectedMethod, string expectedUri, int times = 1)
        {
            Uri expectedRequestUri = new Uri(expectedUri);
            mock.Protected().Verify("SendAsync", Times.Exactly(times), ItExpr.Is((HttpRequestMessage req) => req.Method == expectedMethod && req.RequestUri == expectedRequestUri), ItExpr.IsAny<CancellationToken>());
        }

        private static IReturnsResult<HttpMessageHandler> SetupReturnValueWithStatusCode(Mock<HttpMessageHandler> mock, object expected, HttpStatusCode httpStatusCode)
        {
            return mock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", new object[2]
            {
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            }).ReturnsAsync(new HttpResponseMessage(httpStatusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expected))
            });
        }
    }
}

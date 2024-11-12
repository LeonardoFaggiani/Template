using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using Moq;

using Template.Api.Infrastructure.Exceptions;
using Template.Api.Infrastructure.Exceptions.Builder;
using Template.Api.Infrastructure.Filters;

namespace Template.Api.Unit.Tests.Infrastructure.Filters
{
    public class CustomExceptionFilterTests
    {
        private IServiceProvider ServiceProvider;
        private IExceptionMessageBuilder ExceptionMessageBuilder;

        public CustomExceptionFilterTests()
        {
            this.ServiceProvider = Mock.Of<IServiceProvider>();
            this.ExceptionMessageBuilder = Mock.Of<IExceptionMessageBuilder>();

            Mock.Get(this.ExceptionMessageBuilder).Setup(x => x.WithContext(It.IsAny<ExceptionContext>(), It.IsAny<HttpStatusCode>())).Returns(this.ExceptionMessageBuilder);
            Mock.Get(this.ExceptionMessageBuilder).Setup(x => x.WithExceptionMessage()).Returns(this.ExceptionMessageBuilder);
            Mock.Get(this.ExceptionMessageBuilder).Setup(x => x.WithStackTrace()).Returns(this.ExceptionMessageBuilder);
            Mock.Get(this.ExceptionMessageBuilder).Setup(x => x.WithSupportId()).Returns(this.ExceptionMessageBuilder);
            Mock.Get(this.ExceptionMessageBuilder).Setup(x => x.Build()).Returns(new ExceptionMessage("Error", "trace", "id"));
        }

        public class The_Constructor : CustomExceptionFilterTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_exceptionMessageBuilder_is_null()
            {
                // act & assert
                Assert.Throws<ArgumentNullException>(() => new CustomExceptionFilter(null));
            }
        }

        public class The_Method_OnException : CustomExceptionFilterTests
        {

            [Fact]
            public void OnExceptionTests_badRequest_shouldWork()
            {
                //Arrange
                var context = GetActionContext();

                context.HttpContext.RequestServices = this.ServiceProvider;

                var exContext = new ExceptionContext(context, new List<IFilterMetadata>())
                {
                    Exception = new Exception("Error")
                };

                var result = new CustomExceptionFilter(this.ExceptionMessageBuilder);

                //Act
                result.OnException(exContext);

                //Assert
                Assert.Equal((int)HttpStatusCode.InternalServerError, exContext.HttpContext.Response.StatusCode);
            }

            private static ActionContext GetActionContext()
                   => new ActionContext
                   {
                       HttpContext = new DefaultHttpContext(),
                       RouteData = new RouteData(),
                       ActionDescriptor = new ActionDescriptor(),
                   };

        }
    }
}

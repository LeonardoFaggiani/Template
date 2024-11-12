using System.Net;

using FluentAssertions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using Moq;

using Template.Api.CrossCutting.Resources;
using Template.Api.Infrastructure.Exceptions.Builder;
using Template.Api.Unit.Tests.Base;

namespace Template.Api.Unit.Tests.Infrastructure.Exceptions.Builder
{
    public class ExceptionMessageBuilderTests : BaseTestClass<ExceptionMessageBuilder>
    {
        private IWebHostEnvironment Environment;

        public ExceptionMessageBuilderTests()
        {
            this.Environment = Mock.Of<IWebHostEnvironment>();

            Sut = new ExceptionMessageBuilder(this.Environment);
        }

        public class The_Constructor : ExceptionMessageBuilderTests
        {
            [Fact]
            public void Should_throw_argumentNullException_when_Environment_is_null()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new ExceptionMessageBuilder(null));
            }
        }

        public class TheMethod_WithExceptionMessage : ExceptionMessageBuilderTests
        {
            public TheMethod_WithExceptionMessage()
            {
                var actionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                };

                var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
                {
                    Exception = new Exception()
                };

                Sut.WithContext(exceptionContext, HttpStatusCode.InternalServerError);
            }

            [Fact]
            public void Should_return_the_same_instance_with_exception()
            {
                // Act
                var actual = Sut.WithExceptionMessage();

                // Assert
                actual.Should().BeSameAs(Sut);
            }

            [Fact]
            public void Should_set_message_as_offlineServer_if_environment_is_production_and_statusCode_is_internalServerError()
            {
                // Arrange
                Mock.Get(this.Environment).Setup(e => e.EnvironmentName).Returns("Production");
                Sut.WithContext(It.IsAny<ExceptionContext>(), HttpStatusCode.InternalServerError);

                // Act
                Sut.WithExceptionMessage();

                // Assert
                Sut.Message.Should().Be(Messages.OfflineServer);
            }

            [Fact]
            public void Should_set_exceptions_message_if_environment_is_not_production()
            {
                // Arrange
                var expected = "a-message";
                Mock.Get(this.Environment).Setup(e => e.EnvironmentName).Returns("UAT");

                var actionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                };
                var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
                {
                    Exception = new Exception(expected)
                };
                Sut.WithContext(exceptionContext, It.IsAny<HttpStatusCode>());

                // Act
                Sut.WithExceptionMessage();

                // Assert
                Sut.Message.Should().Be(expected);
            }
        }

        public class TheMethod_WithStackTrace : ExceptionMessageBuilderTests
        {
            public class TestExceptionWithStackTrace : Exception
            {
                public static string StackTraceMessage => "a stack trace";
                public override string StackTrace => StackTraceMessage;
            }

            public TheMethod_WithStackTrace()
            {
                var actionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                };
                var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
                {
                    Exception = new Exception()
                };

                Sut.WithContext(exceptionContext, HttpStatusCode.InternalServerError);
            }

            [Fact]
            public void Should_return_the_same_instance_with_stacktrace()
            {
                // Act
                var actual = Sut.WithStackTrace();

                // Assert
                actual.Should().BeSameAs(Sut);
            }

            [Fact]
            public void Should_set_stackTrace_when_environment_is_not_production()
            {
                // Arrange
                Mock.Get(this.Environment).Setup(e => e.EnvironmentName).Returns("UAT");

                var actionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                };
                var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
                {
                    Exception = new TestExceptionWithStackTrace()
                };

                Sut.WithContext(exceptionContext, HttpStatusCode.InternalServerError);

                // Act
                Sut.WithStackTrace();

                // Assert
                Sut.StackTrace.Should().Be(TestExceptionWithStackTrace.StackTraceMessage);
            }

            [Fact]
            public void Should_not_set_stackTrace_when_environment_is_production()
            {
                // Arrange
                Mock.Get(this.Environment).Setup(e => e.EnvironmentName).Returns("Production");

                var actionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                };
                var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
                {
                    Exception = new TestExceptionWithStackTrace()
                };

                Sut.WithContext(exceptionContext, HttpStatusCode.InternalServerError);

                // Act
                Sut.WithStackTrace();

                // Assert
                Sut.StackTrace.Should().BeNull();
            }
        }

        public class TheMethod_WithSupportId : ExceptionMessageBuilderTests
        {
            [Fact]
            public void Should_return_the_same_instance_with_support_id()
            {
                // Act
                var actual = Sut.WithSupportId();

                // Assert
                actual.Should().BeSameAs(Sut);
            }

            [Fact]
            public void Should_set_supportId_with_a_guid()
            {
                // Act
                Sut.WithSupportId();

                // Assert
                Sut.SupportId.Should().NotBeNullOrEmpty();
            }
        }

        public class TheMethod_Build : ExceptionMessageBuilderTests
        {
            [Fact]
            public void Should_return_an_exceptionMessage()
            {
                // Act
                var actual = Sut.Build();

                // Assert
                actual.Should().NotBeNull();
            }
        }
    }
}

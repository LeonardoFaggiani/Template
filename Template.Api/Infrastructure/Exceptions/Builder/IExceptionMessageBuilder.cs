using System.Net;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Template.Api.Infrastructure.Exceptions.Builder
{
    public interface IExceptionMessageBuilder
    {
        IExceptionMessageBuilder WithContext(ExceptionContext context, HttpStatusCode httpStatusCode);
        IExceptionMessageBuilder WithExceptionMessage();
        IExceptionMessageBuilder WithStackTrace();
        IExceptionMessageBuilder WithSupportId();
        ExceptionMessage Build();
    }
}

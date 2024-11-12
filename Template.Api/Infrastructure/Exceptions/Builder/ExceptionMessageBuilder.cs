using System.Net;

using Microsoft.AspNetCore.Mvc.Filters;

using Template.Api.CrossCutting.Resources;

namespace Template.Api.Infrastructure.Exceptions.Builder
{
    public class ExceptionMessageBuilder : IExceptionMessageBuilder
    {
        private readonly IWebHostEnvironment environment;
        private ExceptionContext Context { get; set; }
        private HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        public string SupportId { get; private set; }

        public ExceptionMessageBuilder(IWebHostEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public ExceptionMessage Build()
        {
            return new ExceptionMessage(Message, StackTrace, SupportId);
        }

        public IExceptionMessageBuilder WithContext(ExceptionContext context, HttpStatusCode httpStatusCode)
        {
            Context = context;
            HttpStatusCode = httpStatusCode;
            return this;
        }

        public IExceptionMessageBuilder WithExceptionMessage()
        {
            if (environment.IsProduction() && HttpStatusCode == HttpStatusCode.InternalServerError)
            {
                Message = Messages.OfflineServer;
            }
            else
            {
                Message = Context.Exception.Message;
            }
            return this;
        }

        public IExceptionMessageBuilder WithStackTrace()
        {
            if (!environment.IsProduction())
            {
                StackTrace = Context?.Exception?.StackTrace ?? string.Empty;
            }
            return this;
        }

        public IExceptionMessageBuilder WithSupportId()
        {
            SupportId = Guid.NewGuid().ToString();
            return this;
        }
    }
}

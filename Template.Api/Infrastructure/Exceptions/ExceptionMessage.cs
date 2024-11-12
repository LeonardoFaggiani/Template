namespace Template.Api.Infrastructure.Exceptions
{
    public class ExceptionMessage
    {
        public virtual string Message { get; protected set; }
        public virtual string StackTrace { get; protected set; }
        public virtual string SupportId { get; protected set; }

        protected ExceptionMessage()
        {
        }
        public ExceptionMessage(string message, string stackTrace, string supportId)
        {
            Message = message;
            StackTrace = stackTrace;
            SupportId = supportId;
        }
    }
}

using MediatR;

namespace Template.Api.Domian.Events
{
    public class SampleEventHandler :
        INotificationHandler<SampleHasBeenInserted>
    {
        public Task Handle(SampleHasBeenInserted notification, CancellationToken cancellationToken)
        {
            //You do something when the Sample is created.
            //I highly recommend adding a messaging system like RabbitMQ, Azure Service Bus, or Amazon EventBridge.
            return Task.CompletedTask;
        }
    }
}

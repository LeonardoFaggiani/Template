using CommunityToolkit.Diagnostics;

using FluentValidation.Results;

using MediatR;

using NetDevPack.Mediator;
using NetDevPack.Messaging;

namespace Template.Api.CrossCutting.Bus
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator mediator;

        public InMemoryBus(IMediator mediator)
        {
            Guard.IsNotNull(mediator, nameof(mediator));

            this.mediator = mediator;
        }

        public async Task PublishEvent<T>(T @event) where T : Event
        {
            await this.mediator.Publish(@event);
        }

        public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
        {
            return await mediator.Send(command);
        }
    }
}

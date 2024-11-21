using CommunityToolkit.Diagnostics;

using Template.Api.Application.Base;
using Template.Api.Domian;
using Template.Api.Domian.Events;
using Template.Api.Infrastructure.Repositories;

namespace Template.Api.Application.Samples.Commands
{
    public class NewSampleCommandHandler : CommandHandler<NewSampleCommand>
    {
        private readonly ISampleRepository sampleRepository;

        public NewSampleCommandHandler(ISampleRepository sampleRepository)
        {
            Guard.IsNotNull(sampleRepository, nameof(sampleRepository));

            this.sampleRepository = sampleRepository;
        }

        protected override async Task Handle(NewSampleCommand command, CancellationToken cancellationToken)
        {
            var sample = new Sample(command.request.Description);

            sample.AddDomainEvent(new SampleHasBeenInserted(sample.Id));

            this.sampleRepository.Add(sample);

            await Commit(this.sampleRepository.UnitOfWork);
        }
    }
}
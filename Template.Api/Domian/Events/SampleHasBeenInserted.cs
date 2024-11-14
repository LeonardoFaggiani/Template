using NetDevPack.Messaging;

namespace Template.Api.Domian.Events
{
    public class SampleHasBeenInserted : Event
    {
        public SampleHasBeenInserted(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}

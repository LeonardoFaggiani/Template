using NetDevPack.Domain;

namespace Template.Api.Domian
{
    public class Sample : Entity
    {        
        public Sample(Guid id, string description)
        {
            Id = id;
            this.Description = description;
        }

        public string Description { get; set; }
    }
}

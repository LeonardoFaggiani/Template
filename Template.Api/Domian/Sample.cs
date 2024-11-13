using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

using NetDevPack.Domain;

namespace Template.Api.Domian
{
    public class Sample : Entity
    {
        public Sample(string desciption)
        {
            this.Description = desciption;
        }

        public string Description { get; }
    }
}

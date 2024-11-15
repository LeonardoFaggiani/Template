using Template.Api.Domian;
using Template.Api.Infrastructure.Data;
using Template.Api.Infrastructure.Repositories.Base;

namespace Template.Api.Infrastructure.Repositories
{
    public interface ISampleRepository : IRepository<Sample>
    {
        Task<Sample> GetByIdAsync(int id);
    }

    public class SampleRepository : Repository<Sample>, ISampleRepository
    {   

        public SampleRepository(TemplateContext context) : base(context)
        { }

        public async Task<Sample> GetByIdAsync(int id)
        {
            return await Context.Samples.FindAsync(id);
        }
    }
}

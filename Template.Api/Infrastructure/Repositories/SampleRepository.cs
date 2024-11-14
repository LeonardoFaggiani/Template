using Microsoft.EntityFrameworkCore;


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
        protected readonly DbSet<Sample> DbSet;        

        public SampleRepository(TemplateContext context) : base(context)
        {
            DbSet = Context.Set<Sample>();
        }

        public async Task<Sample> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }
    }
}

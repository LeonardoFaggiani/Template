using Microsoft.EntityFrameworkCore;

using NetDevPack.Data;

using Template.Api.Domian;
using Template.Api.Infrastructure.Data;
using Template.Api.Infrastructure.Repositories.Base;

namespace Template.Api.Infrastructure.Repositories
{
    public interface ISampleRepository : Base.IRepository<Sample>
    {
        Task<Sample> GetByIdAsync(int id);
    }

    public class SampleRepository : Repository<Sample>, ISampleRepository
    {
        protected readonly DbSet<Sample> DbSet;
        public IUnitOfWork UnitOfWork => Context;

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

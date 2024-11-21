using Template.Api.Domian;
using Template.Api.Infrastructure.Data;
using Template.Api.Infrastructure.Repositories.Base;

namespace Template.Api.Infrastructure.Repositories
{
    public interface ISampleRepository : IRepository<Sample>
    {
        Task<Sample> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }

    public class SampleRepository : Repository<Sample>, ISampleRepository
    {   

        public SampleRepository(TemplateContext context) : base(context)
        { }

        public async Task<Sample> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Context.Samples.FindAsync(id, cancellationToken);
        }
    }
}

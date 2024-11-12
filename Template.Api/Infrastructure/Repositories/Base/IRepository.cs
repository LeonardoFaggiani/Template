using NetDevPack.Domain;

namespace Template.Api.Infrastructure.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task SaveAsync(TEntity entity);
        IQueryable<TEntity> All();
        Task UpdateAsync(TEntity entity);
    }
}

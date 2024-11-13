using NetDevPack.Domain;

namespace Template.Api.Infrastructure.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);
        IQueryable<TEntity> All();
        void Update(TEntity entity);
    }
}

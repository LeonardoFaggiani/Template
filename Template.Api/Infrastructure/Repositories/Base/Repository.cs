using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

using NetDevPack.Domain;

using Template.Api.Infrastructure.Data;

namespace Template.Api.Infrastructure.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly TemplateContext Context;

        public Repository(TemplateContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            this.Context = context;
        }

        public IQueryable<TEntity> All()
        {
            DbSet<TEntity> dbEntity = this.Context.Set<TEntity>();

            return dbEntity.AsNoTracking();
        }

        public Task UpdateAsync(TEntity entity)
        {
            this.Context.Update(entity);

            return this.Context.SaveChangesAsync();
        }

        public Task SaveAsync(TEntity entity)
        {
            if (this.Context.Entry(entity).State.Equals(EntityState.Detached))
            {
                this.Context.Set<TEntity>().Add(entity);
            }

            return this.Context.SaveChangesAsync();
        }
    }
}

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

using NetDevPack.Data;
using NetDevPack.Mediator;

using Template.Api.Domian;
using Template.Api.Infrastructure.Extensions;

namespace Template.Api.Infrastructure.Data
{
    public class TemplateContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler mediatorHandler;

        public TemplateContext()
        {
        }

        public TemplateContext(DbContextOptions<TemplateContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            Guard.IsNotNull(mediatorHandler, nameof(mediatorHandler));

            this.mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Commit()
        {
            var success = await SaveChangesAsync() > 0;

            await this.mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

            return success;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // in memory database used for simplicity, change to a real db for production applications
            options.UseInMemoryDatabase("templateApiBD");
        }

        public DbSet<Sample> Samples { get; set; }
    }
}
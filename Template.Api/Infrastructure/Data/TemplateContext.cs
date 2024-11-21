using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

using NetDevPack.Data;
using NetDevPack.Domain;
using NetDevPack.Mediator;
using NetDevPack.Messaging;

using Template.Api.Domian;
using Template.Api.Infrastructure.Extensions;

namespace Template.Api.Infrastructure.Data
{
    public class TemplateContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler mediatorHandler;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.Entity<Entity>().HasKey(e => e.Id);

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.Property(c => c.Id)
                .HasColumnName("Id");

                entity.ToTable("Sample");

                entity.HasIndex(e => e.Description, "UK_Sample_Description").IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }

        public DbSet<Sample> Samples { get; set; }
    }
}
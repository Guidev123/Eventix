﻿using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Domain.TicketTypes.Entities;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Infrastructure.Inbox.Mappings;
using Eventix.Shared.Infrastructure.Outbox.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Events.Infrastructure.Database
{
    public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<TicketType> TicketTypes { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Events);

            modelBuilder.ApplyConfiguration(new OutboxMessageMapping());
            modelBuilder.ApplyConfiguration(new OutboxMessageConsumerMapping());
            modelBuilder.ApplyConfiguration(new InboxMessageMapping());
            modelBuilder.ApplyConfiguration(new InboxMessageConsumerMapping());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
            => await SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}
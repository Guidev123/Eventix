﻿using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Infrastructure.Inbox.Mappings;
using Eventix.Shared.Infrastructure.Outbox.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Eventix.Modules.Users.Infrastructure.Database
{
    public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Users);

            modelBuilder.ApplyConfiguration(new OutboxMessageMapping());
            modelBuilder.ApplyConfiguration(new OutboxMessageConsumerMapping());
            modelBuilder.ApplyConfiguration(new InboxMessageMapping());
            modelBuilder.ApplyConfiguration(new InboxMessageConsumerMapping());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
            => await SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}
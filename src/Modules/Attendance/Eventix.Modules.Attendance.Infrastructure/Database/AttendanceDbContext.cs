﻿using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using Eventix.Modules.Attendance.Domain.Events.Entities;
using Eventix.Modules.Attendance.Domain.Events.Models;
using Eventix.Modules.Attendance.Domain.Tickets.Entities;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Infrastructure.Inbox.Mappings;
using Eventix.Shared.Infrastructure.Outbox.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Modules.Attendance.Infrastructure.Database
{
    public sealed class AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Attendee> Attendees { get; set; } = null!;
        internal DbSet<Ticket> Tickets { get; set; } = null!;
        internal DbSet<Event> Events { get; set; } = null!;
        internal DbSet<EventStatistic> EventStatistics { get; set; } = null!;
        internal DbSet<InvalidCheckInTicket> InvalidCheckInTickets { get; set; } = null!;
        internal DbSet<DuplicateCheckInTicket> DuplicateCheckInTickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Attendance);

            modelBuilder.ApplyConfiguration(new OutboxMessageMapping());
            modelBuilder.ApplyConfiguration(new OutboxMessageConsumerMapping());
            modelBuilder.ApplyConfiguration(new InboxMessageMapping());
            modelBuilder.ApplyConfiguration(new InboxMessageConsumerMapping());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AttendanceDbContext).Assembly);
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
            => await SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}
﻿using Eventix.Shared.Domain.DomainEvents;

namespace Eventix.Modules.Ticketing.Domain.Events.DomainEvents
{
    public sealed record EventRescheduledDomainEvent(Guid EventId) : DomainEvent(EventId);
}
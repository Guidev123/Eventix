﻿using Eventix.Modules.Events.IntegrationEvents.Events;
using Eventix.Modules.Ticketing.Application.Events.UseCases.Reschedule;
using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Exceptions;

namespace Eventix.Modules.Ticketing.Infrastructure.Events.IntegrationEvents
{
    internal sealed class EventRescheduledIntegrationEventHandler(IMediatorHandler mediator) : IntegrationEventHandler<EventRescheduledIntegrationEvent>
    {
        public override async Task ExecuteAsync(EventRescheduledIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await mediator.DispatchAsync(new RescheduleEventCommand(
                integrationEvent.EventId,
                integrationEvent.StartsAtUtc,
                integrationEvent.EndsAtUtc), cancellationToken);

            if (result.IsFailure)
                throw new EventixException(nameof(RescheduleEventCommand), result.Error);
        }
    }
}
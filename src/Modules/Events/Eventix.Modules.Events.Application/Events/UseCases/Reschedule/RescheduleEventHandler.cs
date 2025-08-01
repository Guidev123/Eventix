﻿using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.Events.UseCases.Reschedule
{
    internal sealed class RescheduleEventHandler(IEventRepository eventRepository,
                                               IDateTimeProvider dateTimeProvider) : ICommandHandler<RescheduleEventCommand>
    {
        public async Task<Result> ExecuteAsync(RescheduleEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId!.Value, cancellationToken);

            if (@event is null)
                return Result.Failure(EventErrors.NotFound(request.EventId.Value));

            if (request.StartsAtUtc < dateTimeProvider.UtcNow)
                return Result.Failure(EventErrors.StartDateInPast);

            @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);
            eventRepository.Update(@event);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges ? Result.Success() : Result.Failure(EventErrors.UnableToCancelEvent(request.EventId.Value));
        }
    }
}
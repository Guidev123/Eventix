﻿using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Ticketing.Application.Events.UseCases.Cancel
{
    internal sealed class CancelEventHandler(IEventRepository eventRepository) : ICommandHandler<CancelEventCommand>
    {
        public async Task<Result> ExecuteAsync(CancelEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
                return Result.Failure(EventErrors.NotFound(request.EventId));

            @event.Cancel();
            eventRepository.Update(@event);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken);
            return saveChanges
                ? Result.Success()
                : Result.Failure(EventErrors.FailToCancelEvent);
        }
    }
}
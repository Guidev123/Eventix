﻿using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Attendance.Application.Events.UseCases.Cancel
{
    internal sealed class CancelEventHandler(IEventRepository eventRepository) : ICommandHandler<CancelEventCommand>
    {
        public async Task<Result> ExecuteAsync(CancelEventCommand request, CancellationToken cancellationToken = default)
        {
            var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
                return Result.Failure(EventErrors.NotFound(request.EventId));

            eventRepository.Delete(@event);

            var saveChanges = await eventRepository.UnitOfWork.CommitAsync(cancellationToken);
            return saveChanges
                ? Result.Success()
                : Result.Failure(EventErrors.FailToCancelEvent(request.EventId));
        }
    }
}
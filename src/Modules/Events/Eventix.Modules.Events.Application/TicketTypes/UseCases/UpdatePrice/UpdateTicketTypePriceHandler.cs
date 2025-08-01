﻿using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.UpdatePrice
{
    internal sealed class UpdateTicketTypePriceHandler(ITicketTypeRepository ticketTypeRepository) : ICommandHandler<UpdateTicketTypePriceCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateTicketTypePriceCommand request, CancellationToken cancellationToken = default)
        {
            var ticketType = await ticketTypeRepository.GetByIdAsync(request.TicketTypeId!.Value, cancellationToken);
            if (ticketType is null)
                return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId.Value));

            ticketType.UpdatePrice(request.Price);
            ticketTypeRepository.Update(ticketType);

            var saveChanges = await ticketTypeRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return saveChanges
                ? Result.Success()
                : Result.Failure(TicketTypeErrors.UnableToUpdate(ticketType.GetType().Name, ticketType.Id));
        }
    }
}
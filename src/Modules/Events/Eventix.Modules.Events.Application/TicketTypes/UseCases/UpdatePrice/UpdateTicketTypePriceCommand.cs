﻿using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.TicketTypes.UseCases.UpdatePrice
{
    public sealed record UpdateTicketTypePriceCommand : ICommand
    {
        public UpdateTicketTypePriceCommand(decimal price)
        {
            Price = price;
        }

        public Guid? TicketTypeId { get; private set; }
        public decimal Price { get; }
        public void SetTicketTypeId(Guid ticketTypeId) => TicketTypeId = ticketTypeId;
    }
}
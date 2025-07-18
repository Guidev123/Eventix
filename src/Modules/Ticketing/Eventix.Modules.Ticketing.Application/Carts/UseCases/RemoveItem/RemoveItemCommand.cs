﻿using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Ticketing.Application.Carts.UseCases.RemoveItem
{
    public sealed record RemoveItemCommand : ICommand
    {
        public RemoveItemCommand(Guid ticketTypeId)
        {
            TicketTypeId = ticketTypeId;
        }

        public Guid CustomerId { get; private set; }
        public Guid TicketTypeId { get; }
        public void SetCustomerId(Guid customerId) => CustomerId = customerId;
    }
}
﻿using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetById
{
    public record GetEventByIdQuery(Guid EventId) : IQuery<GetEventResponse>
    {
    }
}
﻿using Eventix.Modules.Events.Domain.TicketTypes.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Events.Domain.TicketTypes.ValueObjects
{
    public record TicketTypeSpecification : ValueObject
    {
        public const int MIN_NAME_LENGTH = 3;
        public const int MAX_NAME_LENGTH = 100;
        public TicketTypeSpecification(string name, decimal quantity)
        {
            Name = name;
            Quantity = quantity;
            Validate();
        }

        private TicketTypeSpecification()
        { }

        public string Name { get; } = string.Empty;
        public decimal Quantity { get; }

        public static implicit operator TicketTypeSpecification((string name, decimal quantity) value)
            => new(value.name, value.quantity);

        public override string ToString() => $"{Name} ({Quantity})";

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, TicketTypeErrors.NameIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Name, MIN_NAME_LENGTH, MAX_NAME_LENGTH, TicketTypeErrors.NameLengthInvalid.Description);
            AssertionConcern.EnsureTrue(Quantity > 0, TicketTypeErrors.QuantityMustBeGreaterThanZero.Description);
        }
    }
}
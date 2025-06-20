﻿using Eventix.Modules.Users.Domain.Users.DomainEvents;
using Eventix.Modules.Users.Domain.Users.Errors;
using Eventix.Modules.Users.Domain.Users.ValueObjects;
using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects;

namespace Eventix.Modules.Users.Domain.Users.Entities
{
    public sealed class User : Entity, IAggregateRoot
    {
        private User(string email, string firstName, string lastName)
        {
            Email = email;
            Name = (firstName, lastName);
            AuditInfo = new(DateTime.UtcNow);
            Validate();
        }

        private User()
        { }

        public Email Email { get; private set; } = null!;
        public Name Name { get; private set; } = null!;
        public UserAuditInfo AuditInfo { get; private set; } = null!;

        public static User Create(string email, string firstName, string lastName)
        {
            var user = new User(email, firstName, lastName);

            user.Raise(new UserCreatedDomainEvent(user.Id));

            return user;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (Name.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
                && Name.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)) return;

            var name = (firstName, lastName);
            Name = name;

            Raise(new UserNameUpdatedDomainEvent(Id, Name.FirstName, Name.LastName));
        }

        public void UpdateEmail(string email)
        {
            if (Email.Address.Equals(email,
                StringComparison.OrdinalIgnoreCase)) return;

            Email = email;

            Raise(new UserEmailUpdatedDomainEvent(Id, Email.Address));
        }

        public void Delete()
        {
            if (AuditInfo.IsDeleted) return;

            AuditInfo = new UserAuditInfo(AuditInfo.CreatedAtUtc, true, DateTime.UtcNow);

            Raise(new UserDeletedDomainEvent(Id));
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Email, UserErrors.EmailMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotNull(Name, UserErrors.NameMustBeNotEmpty.Description);
            AssertionConcern.EnsureNotNull(AuditInfo, UserErrors.AuditInfoMustBeNotEmpty.Description);
        }
    }
}
﻿using ErrorConsole.Core.DomainEvents;
using System.Collections.Generic;

namespace ErrorConsole.Core.Common
{
    public abstract class AggregateRoot
    {
        public AggregateRoot() => _domainEvents = new List<DomainEvent>();
        private List<DomainEvent> _domainEvents;
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(DomainEvent @event) => _domainEvents.Add(@event);
        public void ClearEvents() => _domainEvents.Clear();
        public void Apply(DomainEvent @event)
        {
            When(@event);
            EnsureValidState();
            RaiseDomainEvent(@event);
        }
        protected abstract void When(DomainEvent @event);
        protected abstract void EnsureValidState();
    }
}

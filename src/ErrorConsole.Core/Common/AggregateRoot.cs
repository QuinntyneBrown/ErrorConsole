using ErrorConsole.Core.DomainEvents;
using System.Collections.Generic;

namespace ErrorConsole.Core.Common
{
    public abstract class AggregateRoot
    {
        private List<DomainEvent> _domainEvents;
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(DomainEvent @event) {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            _domainEvents.Add(@event);
        }
        public void ClearEvents() => _domainEvents.Clear();
        public AggregateRoot Apply(DomainEvent @event)
        {
            When(@event);
            EnsureValidState();
            RaiseDomainEvent(@event);
            return this;
        }
        protected abstract void When(DomainEvent @event);
        protected abstract void EnsureValidState();
    }
}

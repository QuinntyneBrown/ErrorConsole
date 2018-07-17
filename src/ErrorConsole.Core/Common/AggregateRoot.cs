using MediatR;
using System;
using System.Collections.Generic;

namespace ErrorConsole.Core.Common
{
    public abstract class AggregateRoot
    {
        public AggregateRoot() => _domainEvents = new List<INotification>();
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(INotification @event) => _domainEvents.Add(@event);
        public void ClearEvents() => _domainEvents.Clear();        
    }
}

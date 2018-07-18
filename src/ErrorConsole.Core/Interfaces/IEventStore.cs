using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrorConsole.Core.Interfaces
{
    public interface IEventStore : IDisposable
    {
        void Save(AggregateRoot aggregateRoot);
        List<(Guid, DomainEvent[])> GetAllEventsForAggregate<T>()
            where T : AggregateRoot;
        IList<StoredEvent> All(Guid aggregateId);        

        DomainEvent[] GetAllEventsByAggregateId(Guid aggregateId);
        IQueryable<StoredEvent> GetAllByEventProperyValue<T>(string property, string value);

        T GetEventByEventProperyValue<T>(string property, string value);

        T Load<T>(Guid id)
            where T : AggregateRoot;

        TAggregateRoot[] Query<TAggregateRoot>()
            where TAggregateRoot : AggregateRoot;
    }
}

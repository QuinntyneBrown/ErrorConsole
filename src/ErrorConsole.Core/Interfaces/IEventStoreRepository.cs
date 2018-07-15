using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrorConsole.Core.Interfaces
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store<TEvent>(Guid aggregateId, TEvent @event)
            where TEvent : INotification;
        IList<StoredEvent> All(Guid aggregateId);        
        IList<StoredEvent> GetAllByEvent<T>();
        T[] GetAllEventsOfType<T>();
        object[] GetAllEvents(Guid aggregateId);
        IQueryable<StoredEvent> GetAllByEventProperyValue<T>(string property, string value);

        T GetEventByEventProperyValue<T>(string property, string value);
    }
}

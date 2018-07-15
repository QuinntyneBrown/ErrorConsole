using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.Core.Interfaces
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store<TEvent>(Guid aggregateId, TEvent @event)
            where TEvent : INotification;
        IList<StoredEvent> All(Guid aggregateId);
        IList<StoredEvent> GetAllByEvent<T>();
        IQueryable<StoredEvent> GetAllByEventProperyValue<T>(string property, string value);
    }
}

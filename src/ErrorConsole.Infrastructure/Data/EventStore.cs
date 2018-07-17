using ErrorConsole.Core.Common;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.Infrastructure.Data
{
    public class EventStore : IEventStore
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;

        public EventStore(IAppDbContext context, IMediator mediator = default(IMediator)) {
            _context = context;
            _mediator = mediator;
        }
        public IList<StoredEvent> All(Guid aggregateId)
            => _context.StoredEvents.Where(x => x.StreamId == aggregateId).ToList();

        public IList<StoredEvent> GetAllByEvent<T>()
            => _context.StoredEvents.FromSql($"SELECT * FROM StoredEvents WHERE DotNetType ={typeof(T).AssemblyQualifiedName}").ToList();

        public IQueryable<StoredEvent> GetAllByEventProperyValue<T>(string property, string value)
        {
            var propertyParameter = new SqlParameter("property", value);
            var dotNetTypeParameter = new SqlParameter("dotNetType", typeof(T).AssemblyQualifiedName);
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"SELECT * from StoredEvents WHERE JSON_VALUE([Data],'$.{property}')= @property AND DotNetType = @dotnetType");
            
            return _context.StoredEvents.FromSql(stringBuilder.ToString(),propertyParameter, dotNetTypeParameter);
        }

        public void Dispose() => _context.Dispose();

        public T[] GetAllEventsOfType<T>()
            => GetAllByEvent<T>().Select(x => JsonConvert.DeserializeObject(x.Data, typeof(T))).ToArray() as T[];

        public INotification[] GetAllEvents(Guid aggregateId) {
            var list = new List<INotification>();

            foreach(var storedEvent in All(aggregateId))
                list.Add(JsonConvert.DeserializeObject(storedEvent.Data, Type.GetType(storedEvent.DotNetType)) as INotification);

            return list.ToArray();
        }

        public IDictionary<Guid, INotification[]> GetAllEventsForAggregate<T>()
            where T: AggregateRoot
        {
            var result = new Dictionary<Guid, INotification[]>();

            foreach (var grouping in _context.StoredEvents.Where(x => x.Aggregate == typeof(T).Name).GroupBy(x => x.StreamId))
            {
                var events = new List<INotification>();
                foreach(var storedEvent in grouping)
                    events.Add(JsonConvert.DeserializeObject(storedEvent.Data, Type.GetType(storedEvent.DotNetType)) as INotification);
                
                result.Add(grouping.Key, events.ToArray());
            }

            return result;
        }

        public T GetEventByEventProperyValue<T>(string property, string value)
        {
            var domainEvent = GetAllByEventProperyValue<T>(property, value).Single();
            return JsonConvert.DeserializeObject<T>(domainEvent.Data);
        }

        public void Save(Guid aggregateId, AggregateRoot aggregateRoot)
        {
            foreach (var @event in aggregateRoot.DomainEvents) {
                _context.StoredEvents.Add(new StoredEvent()
                {
                    StoredEventId = Guid.NewGuid(),
                    Aggregate = aggregateRoot.GetType().Name,
                    Data = JsonConvert.SerializeObject(@event),
                    StreamId = aggregateId,
                    DotNetType = @event.GetType().AssemblyQualifiedName,
                    Type = @event.GetType().Name,
                    CreatedOn = DateTime.UtcNow
                });

                if (_mediator != null) _mediator.Publish(@event).GetAwaiter().GetResult();

                _context.SaveChanges();
            }

            aggregateRoot.ClearEvents();
        }

        public T Load<T>(Guid id)
            where T : AggregateRoot
        {
            var aggregate = Activator.CreateInstance<Company>();

            foreach (var @event in GetAllEvents(id))
                aggregate.Apply(@event);

            return aggregate as T;
        }
    }
}

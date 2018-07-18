using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
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
using static Newtonsoft.Json.JsonConvert;
using static System.Type;

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

        public IQueryable<StoredEvent> GetAllByEventProperyValue<T>(string property, string value)
        {
            var propertyParameter = new SqlParameter("property", value);
            var dotNetTypeParameter = new SqlParameter("dotNetType", typeof(T).AssemblyQualifiedName);
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"SELECT * from StoredEvents WHERE JSON_VALUE([Data],'$.{property}')= @property AND DotNetType = @dotnetType");
            
            return _context.StoredEvents.FromSql(stringBuilder.ToString(),propertyParameter, dotNetTypeParameter);
        }

        public void Dispose() => _context.Dispose();

        public DomainEvent[] GetAllEventsByAggregateId(Guid aggregateId) {
            var list = new List<DomainEvent>();

            foreach(var storedEvent in All(aggregateId))
                list.Add(JsonConvert.DeserializeObject(storedEvent.Data, Type.GetType(storedEvent.DotNetType)) as DomainEvent);

            return list.ToArray();
        }

        public List<(Guid, DomainEvent[])> GetAllEventsForAggregate<T>()
            where T : AggregateRoot
        {
            var result = new List<(Guid, DomainEvent[])>();

            foreach (var grouping in _context.StoredEvents
                .Where(x => x.Aggregate == typeof(T).Name).GroupBy(x => x.StreamId))
            {
                var events = new List<DomainEvent>();
                foreach (var storedEvent in grouping)
                    events.Add(DeserializeObject(storedEvent.Data, Type.GetType(storedEvent.DotNetType)) as DomainEvent);

                result.Add((grouping.Key, events.ToArray()));
            }

            return result;
        }

        public T GetEventByEventProperyValue<T>(string property, string value)
            => DeserializeObject<T>(GetAllByEventProperyValue<T>(property, value).Single().Data);

        public void Save(AggregateRoot aggregateRoot)
        {
            try
            {
                foreach (var @event in aggregateRoot.DomainEvents)
                {
                    var type = aggregateRoot.GetType();

                    _context.StoredEvents.Add(new StoredEvent()
                    {
                        StoredEventId = Guid.NewGuid(),
                        Aggregate = aggregateRoot.GetType().Name,
                        Data = SerializeObject(@event),
                        StreamId = (Guid)type.GetProperty($"{type.Name}Id").GetValue(aggregateRoot, null),
                        DotNetType = @event.GetType().AssemblyQualifiedName,
                        Type = @event.GetType().Name,
                        CreatedOn = DateTime.UtcNow
                    });

                    if (_mediator != null) _mediator.Publish(@event).GetAwaiter().GetResult();

                    _context.SaveChanges();
                }

                aggregateRoot.ClearEvents();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public T Load<T>(Guid id)
            where T : AggregateRoot
            => Load<T>(id, GetAllEventsByAggregateId(id));

        public T Load<T>(Guid id, DomainEvent[] events)
            where T : AggregateRoot
        {
            var aggregate = Activator.CreateInstance<T>();

            foreach (var @event in events)
                aggregate.Apply(@event);

            aggregate.ClearEvents();

            return aggregate;
        }

        public TAggregateRoot[] Query<TAggregateRoot>()
            where TAggregateRoot : AggregateRoot
        {
            var result = new List<TAggregateRoot>();

            foreach (var ( aggregateId, events ) in GetAllEventsForAggregate<TAggregateRoot>())
                result.Add(Load<TAggregateRoot>(aggregateId, events));

            return result.ToArray();
        }
    }
}

using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static Newtonsoft.Json.JsonConvert;

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
        
        public void Dispose() => _context.Dispose();

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

        public T Query<T>(Guid id)
            where T : AggregateRoot
        {
            var list = new List<DomainEvent>();

            foreach (var storedEvent in _context.StoredEvents.Where(x => x.StreamId == id))
                list.Add(DeserializeObject(storedEvent.Data, Type.GetType(storedEvent.DotNetType)) as DomainEvent);
            
            return Load<T>(list.ToArray());
        }

        private T Load<T>(DomainEvent[] events)
            where T : AggregateRoot
        {
            var aggregate = Activator.CreateInstance<T>();

            foreach (var @event in events)
                aggregate.Apply(@event);

            aggregate.ClearEvents();

            return aggregate;
        }

        public TAggregateRoot Query<TAggregateRoot>(string propertyName, string value)
            where TAggregateRoot : AggregateRoot
        {
            var propertyParameter = new SqlParameter("property", value);
            var dotNetTypeParameter = new SqlParameter("dotNetType", typeof(TAggregateRoot).AssemblyQualifiedName);
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"SELECT * from StoredEvents WHERE JSON_VALUE([Data],'$.{propertyName}')= @property AND DotNetType = @dotnetType");

            var events = _context.StoredEvents.FromSql(stringBuilder.ToString(), propertyParameter, dotNetTypeParameter)
                .Select(x => DeserializeObject(x.Data, Type.GetType(x.DotNetType)) as DomainEvent)
                .ToArray();

            if (events.Length < 1) return null;

            return Load<TAggregateRoot>(events) as TAggregateRoot;
        }

        public TAggregateRoot[] Query<TAggregateRoot>()
            where TAggregateRoot : AggregateRoot
        {
            var result = new List<TAggregateRoot>();

            var list = new List<(Guid, DomainEvent[])>();

            foreach (var grouping in _context.StoredEvents
                .Where(x => x.Aggregate == typeof(TAggregateRoot).Name).GroupBy(x => x.StreamId))
            {
                var events = new List<DomainEvent>();
                foreach (var storedEvent in grouping)
                    events.Add(DeserializeObject(storedEvent.Data, Type.GetType(storedEvent.DotNetType)) as DomainEvent);

                list.Add((grouping.Key, events.ToArray()));
            }
            
            foreach (var ( aggregateId, events ) in list)
                result.Add(Load<TAggregateRoot>(events));

            return result.ToArray();
        }        
    }
}

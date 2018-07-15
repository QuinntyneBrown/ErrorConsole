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
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;

        public EventStoreRepository(IAppDbContext context, IMediator mediator = default(IMediator)) {
            _context = context;
            _mediator = mediator;
        }
        public IList<StoredEvent> All(Guid aggregateId)
            => _context.StoredEvents.Where(x => x.StreamId == aggregateId).ToList();

        public IList<StoredEvent> GetAllByEvent<T>()
            => _context.StoredEvents.FromSql($"SELECT * FROM StoredEvents WHERE DotNetType ={typeof(T).AssemblyQualifiedName}").ToList();

        public IQueryable<StoredEvent> GetAllByEventProperyValue<T>(string property, string value)
        {
            var propertyValue = new SqlParameter("property", value);
            var dotNetType = new SqlParameter("dotNetType", typeof(T).AssemblyQualifiedName);
            var sb = new StringBuilder();

            sb.Append($"SELECT * from StoredEvents WHERE JSON_VALUE([Data],'$.{property}')= @property AND DotNetType = @dotnetType");
            
            return _context.StoredEvents.FromSql(sb.ToString(),propertyValue, dotNetType);
        }

        public void Dispose() => _context.Dispose();

        public void Store<TEvent>(Guid aggregateId, TEvent @event)
            where TEvent : INotification
        {
            _context.StoredEvents.Add(new StoredEvent()
            {
                StoredEventId = Guid.NewGuid(),
                Data = JsonConvert.SerializeObject(@event),
                StreamId = aggregateId,
                DotNetType = typeof(TEvent).AssemblyQualifiedName,
                Type = typeof(TEvent).Name,
                CreatedOn = DateTime.UtcNow
            });

            if(_mediator != null) _mediator.Publish(@event).GetAwaiter().GetResult();

            _context.SaveChanges();
        }

        public T[] GetAllEventsOfType<T>()
            => GetAllByEvent<T>().Select(x => JsonConvert.DeserializeObject(x.Data, typeof(T))).ToArray() as T[];

        public object[] GetAllEvents(Guid aggregateId)
            => All(aggregateId).Select(x => JsonConvert.DeserializeObject(x.Data, Type.GetType(x.DotNetType))).ToArray();

        public T GetEventByEventProperyValue<T>(string property, string value)
        {
            var domainEvent = GetAllByEventProperyValue<T>(property, value).Single();
            return JsonConvert.DeserializeObject<T>(domainEvent.Data);
        }
    }
}

using System;

namespace ErrorConsole.Core.Models
{
    public class DomainEvent
    {
        public Guid DomainEventId { get; set; }
        public Guid AggregateId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public string DotNetType { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorConsole.Core.Models
{
    public class DomainEvent
    {
        public Guid DomainEventId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public string DotNetType { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

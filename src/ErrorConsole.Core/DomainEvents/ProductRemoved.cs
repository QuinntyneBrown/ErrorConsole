using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class ProductRemoved: DomainEvent
    {
        public Guid ProductId { get; set; }
    }
}

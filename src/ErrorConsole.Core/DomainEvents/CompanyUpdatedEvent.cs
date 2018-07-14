using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class CompanyUpdatedEvent
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
    }
}

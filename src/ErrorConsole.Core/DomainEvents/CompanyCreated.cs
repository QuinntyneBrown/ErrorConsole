using MediatR;
using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class CompanyCreated: DomainEvent
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
    }
}

using MediatR;
using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class CompanyCreated: INotification
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
    }
}

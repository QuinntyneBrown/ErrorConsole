using MediatR;
using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class CompanyRemovedEvent: INotification
    {
        public Guid CompanyId { get; set; }
    }
}

using MediatR;
using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class CompanyRemoved: INotification
    {
        public CompanyRemoved(Guid companyId) => CompanyId = companyId;
        public Guid CompanyId { get; set; }
    }
}

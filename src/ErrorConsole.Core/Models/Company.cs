using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using MediatR;
using System;

namespace ErrorConsole.Core.Models
{
    public class Company: AggregateRoot
    {
        public Company()
        {

        }

        public Company(Guid id, string name)
        {            
            Apply(new CompanyCreated()
            {
                CompanyId = id,
                Name = name
            });
        }

        public Guid CompanyId { get; set; }           
		public string Name { get; set; }
        public CompanyStatus Status { get; set; } = CompanyStatus.Active;
        public void Apply(INotification @event)
        {
            switch (@event)
            {
                case CompanyRemoved data:
                    Status = CompanyStatus.Deleted;
                    break;

                case CompanyNameChanged data:
                    Name = data.Name;
                    break;

                case CompanyCreated data:
                    CompanyId = data.CompanyId;
                    Name = data.Name;
                    break;
            }

            RaiseDomainEvent(@event);
        }

        public void ChangeName(string name)
            => Apply(new CompanyNameChanged(name));

        public void Delete() 
            => Apply(new CompanyRemoved());        
    }
}

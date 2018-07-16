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

                case CompanyChanged data:
                    Name = data.Name;
                    break;

                case CompanyCreated data:
                    CompanyId = data.CompanyId;
                    Name = data.Name;
                    break;
            }

            RaiseDomainEvent(@event);
        }

        public void ChangeName(string name) {
            Apply(new CompanyChanged()
            {
                Name = name
            });
        }

        public void Delete()
        {
            Apply(new CompanyRemoved());
        }
        public static Company Load(Guid id, INotification[] events)
        {
            Company company = new Company() {
                CompanyId = id
            };

            foreach (var @event in events)
                company.Apply(@event);

            return company;
        }        
    }
}

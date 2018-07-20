using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using System;
using System.Collections.Generic;

namespace ErrorConsole.Core.Models
{
    public class Company: AggregateRoot
    {
        public Company()
        {

        }

        public Company(Guid id, string name)
            => Apply(new CompanyCreated()
            {
                CompanyId = id,
                Name = name
            });

        public Guid CompanyId { get; set; }           
		public string Name { get; set; }
        public CompanyStatus Status { get; set; } = CompanyStatus.Active;
        public ICollection<Guid> ProductIds { get; set; }  = new HashSet<Guid>();
        protected override void When(DomainEvent @event)
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
        }

        public void ChangeName(string name)
            => Apply(new CompanyNameChanged(name));

        public void Delete() 
            => Apply(new CompanyRemoved());

        protected override void EnsureValidState()
        {

        }
    }
}

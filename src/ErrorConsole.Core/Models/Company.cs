using ErrorConsole.Core.DomainEvents;
using System;
//https://www.youtube.com/watch?v=0RGuoRRHWf8

namespace ErrorConsole.Core.Models
{
    public class Company
    {
        public Guid CompanyId { get; set; }           
		public string Name { get; set; }
        public bool IsDeleted { get; set; }   
        public static Company Create(Guid id, object[] events)
        {
            Company company = new Company() {
                CompanyId = id
            };

            foreach (var @event in events)
            {
                switch (@event)
                {
                    case CompanyCreatedEvent data:
                        company.Name = data.Name;
                        break;

                    case CompanyChangedEvent data:
                        company.Name = data.Name;
                        break;

                    case CompanyRemovedEvent data:
                        company.IsDeleted = true;
                        break;
                }
            }

            return company;
        }
    }
}

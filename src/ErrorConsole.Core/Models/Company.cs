using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using System;

namespace ErrorConsole.Core.Models
{
    public class Company: Aggregate
    {
        public Guid CompanyId { get; set; }           
		public string Name { get; set; }
        public bool IsDeleted { get; set; }   
        public static Company Load(Guid id, object[] events)
        {
            Company company = new Company() {
                CompanyId = id
            };

            foreach (var @event in events)
            {
                switch (@event)
                {
                    case CompanyCreated data:
                        company.Name = data.Name;
                        break;

                    case CompanyChanged data:
                        company.Name = data.Name;
                        break;

                    case CompanyRemoved data:
                        company.IsDeleted = true;
                        break;
                }
            }

            return company;
        }
    }
}

using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using System;
using System.Collections.Generic;

namespace ErrorConsole.Core.Models
{
    public class Dashboard: AggregateRoot
    {
        public Dashboard(string name)
            => Apply(new DashboardCreated(name));

        public Guid DashboardId { get; set; } = Guid.NewGuid();     
        public ICollection<Guid> DashboardCardIds { get; set; }
		public string Name { get; set; }        
		public bool IsDeleted { get; set; }

        protected override void EnsureValidState()
        {
            
        }

        protected override void When(DomainEvent @event)
        {
            switch (@event)
            {
                case DashboardCreated dashboardCreated:
                    Name = dashboardCreated.Name;
                    DashboardCardIds = new HashSet<Guid>();
                    break;

                case DashboardNameChanged dashboardNameChanged:
                    Name = dashboardNameChanged.Name;
                    break;

                case DashboardRemoved dashboardRemoved:
                    IsDeleted = true;
                    break;

                case DashboardCardAddedToDashboard dashboardCardAddedToDashboard:
                    DashboardCardIds.Add(dashboardCardAddedToDashboard.DashboardCardId);
                    break;
            }
        }

        public void ChangeName(string name)
            => Apply(new DashboardNameChanged(name));

        public void Remove()
            => Apply(new DashboardRemoved());

        public void AddDashboardCard(Guid dashboardCardId)
            => Apply(new DashboardCardAddedToDashboard(dashboardCardId));
    }
}

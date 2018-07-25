using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using Newtonsoft.Json;
using System;

namespace ErrorConsole.Core.Models
{
    public class DashboardCard: AggregateRoot
    {
        public DashboardCard(Guid dashoardId, Guid cardId)
            => Apply(new DashboardCardCreated(dashoardId,cardId));

        public Guid DashboardCardId { get; set; } = Guid.NewGuid();
        public Guid DashboardId { get; set; }
        public Guid CardId { get; set; }
        public string Options { get; set; }
        public bool IsDeleted { get; set; }

        protected override void EnsureValidState()
        {
            
        }

        protected override void When(DomainEvent @event)
        {
            switch (@event)
            {
                case DashboardCardCreated dashboardCardCreated:
                    DashboardId = dashboardCardCreated.DashboardId;
                    CardId = dashboardCardCreated.CardId;
                    break;

                case DashboardCardRemoved dashboardCardRemoved:
                    IsDeleted = true;
                    break;

                case DashboardCardOptionsUpdated dashboardCardOptionsUpdated:
                    Options = JsonConvert.SerializeObject(new {
                        dashboardCardOptionsUpdated.Top,
                        dashboardCardOptionsUpdated.Height,
                        dashboardCardOptionsUpdated.Width,
                        dashboardCardOptionsUpdated.Left
                    });
                    break;
            }
        }
        
        public void Remove()
            => Apply(new DashboardCardRemoved());

        public void UpdateOptions(int top, int width, int left, int height)
            => Apply(new DashboardCardOptionsUpdated(top, width, height, width));
    }
}

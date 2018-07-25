using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class DashboardCardCreated: DomainEvent
    {
        public DashboardCardCreated(Guid dashboardId, Guid cardId) {
            DashboardId = dashboardId;
            CardId = cardId;
        }
        public Guid DashboardId { get; set; }
        public Guid CardId { get; set; }
    }
}

using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.DashboardCards
{
    public class DashboardCardDto
    {        
        public Guid DashboardCardId { get; set; }
        public Guid DashboardId { get; set; }
        public Guid CardId { get; set; }

        public static DashboardCardDto FromDashboardCard(DashboardCard dashboardCard)
            => new DashboardCardDto
            {
                DashboardCardId = dashboardCard.DashboardCardId,
                DashboardId = dashboardCard.DashboardId,
                CardId = dashboardCard.CardId
            };
    }
}

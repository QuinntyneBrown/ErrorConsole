using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Dashboards
{
    public class DashboardDto
    {        
        public Guid DashboardId { get; set; }
        public string Name { get; set; }

        public static DashboardDto FromDashboard(Dashboard dashboard)
            => new DashboardDto
            {
                DashboardId = dashboard.DashboardId,
                Name = dashboard.Name
            };
    }
}

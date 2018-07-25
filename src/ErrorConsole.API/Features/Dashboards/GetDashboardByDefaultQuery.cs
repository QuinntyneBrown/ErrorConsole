using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using ErrorConsole.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ErrorConsole.Core.Models;
using ErrorConsole.API.Features.DashboardCards;
using ErrorConsole.API.Features.Cards;

namespace ErrorConsole.API.Features.Dashboards
{
    public class GetDashboardByDefaultQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public DashboardDto  Dashboard { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _eventStore;
            public Handler(IEventStore eventStore)
            {
                _eventStore = eventStore;
            }
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dashboard = _eventStore.Query<Dashboard>("Name", "Default");

                var dashboardDto = DashboardDto.FromDashboard(dashboard);

                var dashboardCardDtos = new List<DashboardCardDto>();

                foreach(var dashboardCardId in dashboard.DashboardCardIds)
                {
                    var dashboardCardDto = DashboardCardDto.FromDashboardCard(_eventStore.Query<DashboardCard>(dashboardCardId));
                    dashboardCardDto.Card = CardDto.FromCard(_eventStore.Query<Card>(dashboardCardDto.CardId));                    
                    dashboardCardDtos.Add(dashboardCardDto);
                }
                       
                return Task.FromResult(new Response()
                {
                    Dashboard = DashboardDto.FromDashboard(dashboard, dashboardCardDtos)
                });
            }
        }
    }
}

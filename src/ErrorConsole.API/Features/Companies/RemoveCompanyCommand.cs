using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Companies
{
    public class RemoveCompanyCommand
    {
        public class Request : IRequest
        {
            public Guid CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly IEventStore _eventStore;
            
            public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public Task Handle(Request request, CancellationToken cancellationToken)
            {
                var company = _eventStore.Query<Company>(request.CompanyId);

                company.Delete();
                
                _eventStore.Save(company);

                return Task.CompletedTask;
            }
        }
    }
}

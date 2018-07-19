using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Companies
{
    public class MaybeSaveCompanyCommand
    {
        public class Request : IRequest<Response> {
            public CompanyDto Company { get; set; }
        }

        public class Response
        {			
            public Guid CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _eventStore;

            public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (RandomNumberFactory.Create() > 14) throw new Exception();

                var company = _eventStore.Query<Company>(request.Company.CompanyId);

                company.ChangeName(request.Company.Name);

                _eventStore.Save(company);

                return Task.FromResult(new Response() { CompanyId = request.Company.CompanyId });                
            }
        }
    }
}

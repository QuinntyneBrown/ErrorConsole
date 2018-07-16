using ErrorConsole.Core.Common;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Companies
{
    public class MaybeCreateCompanyCommand
    {
        public class Request : IRequest<Response>
        {
            public CompanyApiModel Company { get; set; }
        }

        public class Response
        {
            public Guid CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IEventStore _eventStore { get; set; }

            public Handler(IEventStore repository) => _eventStore = repository;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (RandomNumberFactory.Create() > 5) throw new Exception();
                
                var company = new Company(Guid.NewGuid(),request.Company.Name);

                _eventStore.Save(company.CompanyId,company);
                
                return Task.FromResult(new Response() { CompanyId = company.CompanyId });
            }
        }
    }
}

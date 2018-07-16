using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ErrorConsole.Core.Common;
using System;
using ErrorConsole.Core.DomainEvents;
using Newtonsoft.Json;

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
            public IEventStoreRepository _repository { get; set; }

            public Handler(IEventStoreRepository repository) => _repository = repository;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (RandomNumberFactory.Create() > 5) throw new Exception();
                
                var company = new Company(Guid.NewGuid(),request.Company.Name);

                _repository.Save(company.CompanyId,company);
                
                return Task.FromResult(new Response() { CompanyId = company.CompanyId });
            }
        }
    }
}

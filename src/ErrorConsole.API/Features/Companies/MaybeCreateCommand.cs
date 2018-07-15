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
                if (RandomNumberFactory.Create() > 14) throw new Exception();

                var companyId = Guid.NewGuid();

                _repository.Store(companyId, new CompanyCreated()
                {
                    CompanyId = companyId,
                    Name = request.Company.Name
                });
                
                return Task.FromResult(new Response() { CompanyId = companyId });
            }
        }
    }
}

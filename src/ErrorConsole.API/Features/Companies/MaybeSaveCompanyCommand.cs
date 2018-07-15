using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Companies
{
    public class MaybeSaveCompanyCommand
    {
        public class Request : IRequest<Response> {
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
                
                _repository.Store(request.Company.CompanyId, new CompanyChanged()
                {
                    CompanyId = request.Company.CompanyId,
                    Name = request.Company.Name
                });

                return Task.FromResult(new Response() { CompanyId = request.Company.CompanyId });                
            }
        }
    }
}

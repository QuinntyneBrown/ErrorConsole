using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Interfaces;
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
            private IEventStoreRepository _repository;
            
            public Handler(IEventStoreRepository repository) => _repository = repository;

            public Task Handle(Request request, CancellationToken cancellationToken)
            {
                _repository.Store(request.CompanyId, new CompanyRemoved(request.CompanyId));

                return Task.CompletedTask;
            }
        }
    }
}

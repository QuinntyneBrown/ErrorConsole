using ErrorConsole.Core.DomainEvents;
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
            private IEventStore _repository;
            
            public Handler(IEventStore repository) => _repository = repository;

            public Task Handle(Request request, CancellationToken cancellationToken)
            {
                var company = Company.Load(request.CompanyId, _repository.GetAllEvents(request.CompanyId));

                company.Delete();
                
                _repository.Save(company.CompanyId,company);

                return Task.CompletedTask;
            }
        }
    }
}

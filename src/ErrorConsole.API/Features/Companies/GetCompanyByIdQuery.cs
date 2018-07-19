using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ErrorConsole.Core.Interfaces;
using FluentValidation;
using ErrorConsole.Core.Models;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace ErrorConsole.API.Features.Companies
{
    public class GetCompanyByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CompanyId).NotEqual(default(Guid));
            }
        }

        public class Request : IRequest<Response> {
            public Guid CompanyId { get; set; }
        }

        public class Response
        {
            public CompanyDto Company { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _eventStore;
            
			public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {                
                return Task.FromResult(new Response()
                {
                    Company = CompanyDto.FromCompany(_eventStore.Query<Company>(request.CompanyId))
                });
            }
        }
    }
}

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ErrorConsole.Core.Interfaces;
using FluentValidation;
using ErrorConsole.Core.Models;
using System;
using System.Linq;

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
            public CompanyApiModel Company { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IEventStoreRepository _repository { get; set; }
            
			public Handler(IEventStoreRepository repository) => _repository = repository;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var company = Company.Create(request.CompanyId, _repository.All(request.CompanyId).ToArray());

                return new Response()
                {
                    Company = CompanyApiModel.FromCompany(company)
                };
            }
        }
    }
}

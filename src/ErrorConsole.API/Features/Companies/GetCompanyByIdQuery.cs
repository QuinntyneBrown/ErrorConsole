using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ErrorConsole.Core.Interfaces;
using FluentValidation;

namespace ErrorConsole.API.Features.Companies
{
    public class GetCompanyByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CompanyId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int CompanyId { get; set; }
        }

        public class Response
        {
            public CompanyApiModel Company { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Company = CompanyApiModel.FromCompany(await _context.Companies.FindAsync(request.CompanyId))
                };
        }
    }
}

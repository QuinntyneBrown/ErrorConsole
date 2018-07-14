using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ErrorConsole.Core.Common;
using System;

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
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (RandomNumberFactory.Create() > 14) throw new Exception();

                var company = await _context.Companies.FindAsync(request.Company.CompanyId);

                if (company == null) _context.Companies.Add(company = new Company());

                company.Name = request.Company.Name;
                
                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { CompanyId = company.CompanyId };
            }
        }
    }
}

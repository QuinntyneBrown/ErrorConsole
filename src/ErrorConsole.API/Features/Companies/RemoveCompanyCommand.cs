using ErrorConsole.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Companies
{
    public class RemoveCompanyCommand
    {
        public class Request : IRequest
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var company = await _context.Companies.FindAsync(request.CompanyId);
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

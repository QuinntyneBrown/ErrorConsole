using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorConsole.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrorConsole.Core.Common;

namespace ErrorConsole.API.Features.Companies
{
    public class MaybeGetCompaniesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CompanyApiModel> Companies { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {                
                if (RandomNumberFactory.Create() > 14) throw new Exception();

                return new Response()
                {
                    Companies = await _context.Companies.Select(x => CompanyApiModel.FromCompany(x)).ToListAsync()
                };
            }
        }
    }
}

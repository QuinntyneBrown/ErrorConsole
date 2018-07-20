using ErrorConsole.Core.Common;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Companies
{
    public class MaybeGetCompaniesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CompanyDto> Companies { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _eventStore;

            public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                //if (RandomNumberFactory.Create() > 14) throw new Exception();
                
                return Task.FromResult(new Response()
                {
                    Companies = _eventStore.Query<Company>()
                    .Where(x => x.Status == CompanyStatus.Active)
                    .Select( x=> CompanyDto.FromCompany(x)).ToList()
                });
            }
        }
    }
}

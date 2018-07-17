using ErrorConsole.Core.Common;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            private readonly IEventStore _repository;

            public Handler(IEventStore repository) => _repository = repository;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {                
                if (RandomNumberFactory.Create() > 14) throw new Exception();

                List<CompanyApiModel> companies = new List<CompanyApiModel>();

                foreach (var result in _repository.GetAllEventsForAggregate<Company>()) {                    
                    var model = Company.Load(result.Key, result.Value);

                    if(model.Status == CompanyStatus.Active)
                        companies.Add(CompanyApiModel.FromCompany(model));
                }

                return Task.FromResult(new Response()
                {
                    Companies = companies
                });
            }
        }
    }
}

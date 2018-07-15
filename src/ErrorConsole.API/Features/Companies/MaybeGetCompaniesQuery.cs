using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorConsole.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Models;
using Newtonsoft.Json;

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
            private readonly IEventStoreRepository _repository;

            public Handler(IEventStoreRepository repository) => _repository = repository;

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {                
                if (RandomNumberFactory.Create() > 14) throw new Exception();

                List<CompanyApiModel> companies = new List<CompanyApiModel>();

                foreach (var id in _repository.GetAllByEvent<CompanyCreated>().Select(x => x.StreamId)) {
                    var model = Company.Load(id, _repository.GetAllEvents(id));

                    if(model.IsDeleted == false)
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

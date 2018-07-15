using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Companies
{
    [Authorize]
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<MaybeSaveCompanyCommand.Response>> Update(MaybeSaveCompanyCommand.Request request)
            => await _mediator.Send(request);

        [HttpPost("create")]
        public async Task<ActionResult<MaybeCreateCompanyCommand.Response>> Create(MaybeCreateCompanyCommand.Request request)
            => await _mediator.Send(request);

        [HttpDelete("{companyId}")]
        public async Task Remove([FromRoute]RemoveCompanyCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{companyId}")]
        public async Task<ActionResult<GetCompanyByIdQuery.Response>> GetById([FromRoute]GetCompanyByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<MaybeGetCompaniesQuery.Response>> Get()
            => await _mediator.Send(new MaybeGetCompaniesQuery.Request());
    }
}

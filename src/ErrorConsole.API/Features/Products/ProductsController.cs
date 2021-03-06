using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Products
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductsController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveProductCommand.Response>> Save(SaveProductCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{productId}")]
        public async Task Remove([FromRoute]RemoveProductCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{productId}")]
        public async Task<ActionResult<GetProductByIdQuery.Response>> GetById([FromRoute]GetProductByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetProductsQuery.Response>> Get()
            => await _mediator.Send(new GetProductsQuery.Request());
    }
}

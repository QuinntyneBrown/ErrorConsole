using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using ErrorConsole.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ErrorConsole.Core.Models;

namespace ErrorConsole.API.Features.Products
{
    public class GetProductsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<ProductDto> Products { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IEventStore _eventStore { get; set; }
            
			public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Products = _eventStore.Query<Product>().Select(x => ProductDto.FromProduct(x)).ToList()
                };
        }
    }
}

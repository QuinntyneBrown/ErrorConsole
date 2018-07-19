using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ErrorConsole.Core.Interfaces;
using FluentValidation;
using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Products
{
    public class GetProductByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.ProductId).NotEqual(default(Guid));
            }
        }

        public class Request : IRequest<Response> {
            public Guid ProductId { get; set; }
        }

        public class Response
        {
            public ProductDto Product { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IEventStore _eventStore { get; set; }
            
			public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Product = ProductDto.FromProduct(_eventStore.Query<Product>(request.ProductId))
                };
        }
    }
}

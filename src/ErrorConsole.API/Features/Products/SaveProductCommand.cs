using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.API.Features.Products
{
    public class SaveProductCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Product.ProductId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public ProductDto Product { get; set; }
        }

        public class Response
        {			
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IEventStore _eventStore { get; set; }
            
			public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var product = _eventStore.Query<Product>(request.Product.ProductId);

                if (product == null) product = new Product(
                    Guid.NewGuid(), 
                    request.Product.CompanyId,
                    request.Product.ImageUrl, 
                    request.Product.Price,
                    request.Product.Name,
                    request.Product.Description);

                product.Name = request.Product.Name;
                
                _eventStore.Save(product);

                return new Response() { ProductId = product.ProductId };
            }
        }
    }
}

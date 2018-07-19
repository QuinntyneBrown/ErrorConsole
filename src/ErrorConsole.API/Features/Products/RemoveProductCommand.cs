using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace ErrorConsole.API.Features.Products
{
    public class RemoveProductCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.ProductId).NotEqual(default(Guid));
            }
        }

        public class Request : IRequest
        {
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IEventStore _eventStore { get; set; }
            
			public Handler(IEventStore eventStore) => _eventStore = eventStore;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var product = _eventStore.Query<Product>(request.ProductId);

                product.Remove();

                _eventStore.Save(product);                            }
        }
    }
}

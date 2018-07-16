using ErrorConsole.Core.DomainEvents;
using ErrorConsole.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.Infrastructure.Data
{
    public class QueryService: INotificationHandler<INotification>
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public QueryService(IMediator mediator, IEventStore eventStoreRepository)
        {
            _mediator = mediator;
            _eventStore = eventStoreRepository;
        }

        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            switch (notification)
            {
                case CompanyCreated companyCreated:
                    break;

                default:
                    Console.WriteLine("Works");
                    break;
            }

            return Task.CompletedTask;
        }

        public void StartAggregate() {

        }
    }
}

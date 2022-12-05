using MediatR;
using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;
using MicroserviceSample.Services.Reports.API.Application.Events;
using MicroserviceSample.Services.Reports.API.Infrastructure.Domain.Events;

namespace MicroserviceSample.Services.Reports.API.Application
{
    public class ReportCreatedDomainEventHandler : INotificationHandler<ReportCreatedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public ReportCreatedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(ReportCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            return _eventBus.Publish(new ReportCreatedIntegrationEvent(Guid.NewGuid(), DateTimeExtensions.LocalNow, notification.ReportId), cancellationToken);
        }
    }
}

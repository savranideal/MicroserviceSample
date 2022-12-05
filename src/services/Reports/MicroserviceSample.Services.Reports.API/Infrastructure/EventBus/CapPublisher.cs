using DotNetCore.CAP;

using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;

namespace MicroserviceSample.Services.Reports.API.Infrastructure.EventBus
{
    internal class CapPublisher : IEventBus
    {
        private readonly ICapPublisher _publisher;

        public CapPublisher(ICapPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Publish<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken) where TIntegrationEvent : IntegrationEvent
        {
            await _publisher.PublishAsync(typeof(TIntegrationEvent).Name, @event, cancellationToken: cancellationToken);
        }
    }
}

namespace MicroserviceSample.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventBus
    {
        Task Publish<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default) where TIntegrationEvent : IntegrationEvent;
    }
}
using Microsoft.EntityFrameworkCore.Storage;

namespace MicroserviceSample.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventBus
    {
        Task Publish<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default) where TIntegrationEvent : IntegrationEvent;
    }


    public interface ITransactionableEventBus : IEventBus
    {

        void BeginTransaction(IDbContextTransaction transaction);
    }
}
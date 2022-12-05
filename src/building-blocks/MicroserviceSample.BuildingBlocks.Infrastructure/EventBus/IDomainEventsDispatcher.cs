using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.BuildingBlocks.Infrastructure.EventBus
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync(DbContext context, CancellationToken cancellationToken);
    }
}

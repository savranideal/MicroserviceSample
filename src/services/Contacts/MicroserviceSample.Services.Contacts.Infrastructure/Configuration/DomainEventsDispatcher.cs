using MediatR;

using MicroserviceSample.BuildingBlocks.Domain;
using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Configuration
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IPublisher _publisher;

        public DomainEventsDispatcher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task DispatchEventsAsync(DbContext context, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<IDomainEvent> domainEvents = GetAllDomainEvents(context);

            ClearAllDomainEvents(context);

            foreach (IDomainEvent domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }

        private static IReadOnlyCollection<IDomainEvent> GetAllDomainEvents(DbContext context)
        {
            List<EntityEntry<Entity>> domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            return domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
        }

        private static void ClearAllDomainEvents(DbContext context)
        {
            List<EntityEntry<Entity>> domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());
        }
    }
}
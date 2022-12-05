using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;
using MicroserviceSample.BuildingBlocks.Infrastructure.Persistence;

using Microsoft.Extensions.Logging;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Persistence
{
    internal class ContactUnitOfWork : EfUnitOfWork<ContactManagementDbContext>
    {
        public ContactUnitOfWork(ContactManagementDbContext context, IDomainEventsDispatcher domainEventsDispatcher, ILogger<EfUnitOfWork<ContactManagementDbContext>> logger) : base(context, domainEventsDispatcher, logger)
        {
        }
    }
}

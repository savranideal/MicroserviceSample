using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;
using MicroserviceSample.BuildingBlocks.Infrastructure.Persistence;

namespace MicroserviceSample.Services.Reports.API.Infrastructure.Persistence
{
    internal class ReportUnitOfWork : EfUnitOfWork<ReportManagementDbContext>
    {
        public ReportUnitOfWork(ReportManagementDbContext context, IDomainEventsDispatcher domainEventsDispatcher, ILogger<EfUnitOfWork<ReportManagementDbContext>> logger) : base(context, domainEventsDispatcher, logger)
        {
        }
    }
}

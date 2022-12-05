using MicroserviceSample.BuildingBlocks.Domain;

namespace MicroserviceSample.Services.Reports.API.Infrastructure.Domain.Events
{
    public class ReportCreatedDomainEvent : DomainEventBase
    {
        public Guid ReportId { get; }

        public ReportCreatedDomainEvent(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}

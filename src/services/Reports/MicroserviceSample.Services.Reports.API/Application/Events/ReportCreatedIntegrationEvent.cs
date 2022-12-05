using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;

namespace MicroserviceSample.Services.Reports.API.Application.Events
{
    public record ReportCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid ReportId { get; init; }

        public ReportCreatedIntegrationEvent()
        {
        }

        public ReportCreatedIntegrationEvent(Guid id, DateTime createDate,Guid reportId) : base(id, createDate)
        {
            ReportId=reportId;
        }
    }
}

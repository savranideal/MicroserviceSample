using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;

namespace MicroserviceSample.Services.Reports.API.Application.Events
{
    public record ReportCreatedIntegrationEvent(Guid ReportId) : IntegrationEvent;
}

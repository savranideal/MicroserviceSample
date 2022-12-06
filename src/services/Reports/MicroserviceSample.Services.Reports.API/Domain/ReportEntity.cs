
using MicroserviceSample.BuildingBlocks.Domain;
using MicroserviceSample.Services.Reports.API.Application;
using MicroserviceSample.Services.Reports.API.Infrastructure.Domain.Events;

namespace MicroserviceSample.Services.Reports.API.Domain
{
    public class ReportEntity : Entity
    {
        private ReportEntity()
        {
            // For EF
        }

        private ReportEntity(DateTime requestDate, ReportStatus status, string path)
        {
            Id = Guid.NewGuid();
            RequestDate = requestDate;
            Status = status;
            Path = path;
            AddDomainEvent(new ReportCreatedDomainEvent(Id));
        }

        public Guid Id { get; set; }

        public DateTime RequestDate { get; set; }

        public ReportStatus Status { get; set; }

        public string Path { get; set; }

        public static ReportEntity Create()
        {
            return new ReportEntity(DateTimeExtensions.Now, ReportStatus.InProgress, string.Empty);
        }
    }
}
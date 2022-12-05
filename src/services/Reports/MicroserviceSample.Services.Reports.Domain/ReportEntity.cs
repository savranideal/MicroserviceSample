using MicroserviceSample.BuildingBlocks.Domain;

namespace MicroserviceSample.Services.Reports.Domain
{
    public class ReportEntity : Entity
    {
        public Guid Id { get; set; }

        public DateTime RequestDate { get; set; }

        public ReportStatus Status { get; set; }
    }
}
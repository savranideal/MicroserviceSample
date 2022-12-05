namespace MicroserviceSample.Services.Reports.API.Application
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Path { get; set; }
    }
}

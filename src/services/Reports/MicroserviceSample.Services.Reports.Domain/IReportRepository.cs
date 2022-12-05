namespace MicroserviceSample.Services.Reports.Domain
{
    public interface IReportRepository
    {
        void Add(ReportEntity entity);
        Task<IEnumerable<ReportEntity>> AllAsync(CancellationToken cancellationToken);
        Task<ReportEntity> GetById(Guid id,CancellationToken cancellationToken);
    }
}
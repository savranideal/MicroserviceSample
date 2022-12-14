namespace MicroserviceSample.Services.Reports.API.Domain;

public interface IReportRepository
{
    void Add(ReportEntity entity);
    Task<IEnumerable<ReportEntity>> AllAsync(CancellationToken cancellationToken);
    Task<ReportEntity> GetById(Guid id,CancellationToken cancellationToken);
    Task<bool> HasActiveReportAsync(CancellationToken cancellationToken);
}
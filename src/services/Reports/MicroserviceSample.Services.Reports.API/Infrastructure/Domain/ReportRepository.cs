using MicroserviceSample.Services.Reports.API.Domain;
using MicroserviceSample.Services.Reports.API.Infrastructure.Persistence;
using MicroserviceSample.Services.Reports.Domain;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.Services.Reports.API.Infrastructure.Domain
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportManagementDbContext _dbContext;

        public ReportRepository(ReportManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(ReportEntity entity)
        {
            _dbContext.Add(entity);
        }

        public async Task<IEnumerable<ReportEntity>> AllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Reports.ToArrayAsync(cancellationToken);
        }

        public async Task<ReportEntity> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Reports.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<bool> HasActiveReportAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Reports.AnyAsync(c => c.Status == ReportStatus.InProgress, cancellationToken);
        }
    }
}

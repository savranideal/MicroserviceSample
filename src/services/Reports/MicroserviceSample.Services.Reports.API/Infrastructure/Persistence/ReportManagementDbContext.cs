using MicroserviceSample.Services.Reports.API.Domain;
using MicroserviceSample.Services.Reports.Infrastructure.Domain.Report;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.Services.Reports.API.Infrastructure.Persistence
{
    public class ReportManagementDbContext : DbContext
    {
        public ReportManagementDbContext(DbContextOptions<ReportManagementDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<ReportEntity> Reports { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ReportEntityConfiguration());
        }
    }
}

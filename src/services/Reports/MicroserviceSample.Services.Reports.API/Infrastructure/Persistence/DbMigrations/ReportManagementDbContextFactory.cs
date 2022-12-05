using MicroserviceSample.Services.Reports.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RoadRunner.Dock.Infrastructure.Context.DbMigrations
{
    public class ReportManagementDbContextFactory : IDesignTimeDbContextFactory<ReportManagementDbContext>
    {
        // dotnet ef migrations add ReportManagementDbContextFactory --context ReportManagementDbContext --project src/services/Reports/MicroserviceSample.Services.Reports.API --output-dir Infrastructure/Persistence/DbMigrations

        public ReportManagementDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ReportManagementDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=microservicesample.Report;User Id=Reportsa;Password=SplArmonsMAZONTINGEriCi", o => o.MigrationsAssembly("MicroserviceSample.Services.Reports.API"));
            return new ReportManagementDbContext(optionsBuilder.Options);
        }
    }
}

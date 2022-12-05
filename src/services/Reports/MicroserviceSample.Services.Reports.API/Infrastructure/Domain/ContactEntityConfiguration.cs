using MicroserviceSample.Services.Reports.API.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceSample.Services.Reports.Infrastructure.Domain.Report
{
    public class ReportEntityConfiguration : IEntityTypeConfiguration<ReportEntity>
    {
        public void Configure(EntityTypeBuilder<ReportEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RequestDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Path).IsRequired(false);

            builder.ToTable("Report");
        }
    }
}

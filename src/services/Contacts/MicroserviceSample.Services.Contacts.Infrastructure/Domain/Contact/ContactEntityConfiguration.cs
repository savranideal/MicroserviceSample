using MicroserviceSample.Services.Contacts.Domain.Contact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Domain.Contact
{
    public class ContactEntityConfiguration : IEntityTypeConfiguration<ContactEntity>
    {
        public void Configure(EntityTypeBuilder<ContactEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired(false);
            builder.Property(x => x.CompanyName).IsRequired(false);
             
             builder.HasMany(x => x.Communications)
                .WithOne(p => p.Contact)
                .HasForeignKey(x => x.ContactId)
                .IsRequired(false);

             builder.ToTable("Contact");
        }
    }
}

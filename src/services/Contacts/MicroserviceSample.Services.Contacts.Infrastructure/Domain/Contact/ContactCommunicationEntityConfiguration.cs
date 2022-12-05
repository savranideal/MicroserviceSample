using MicroserviceSample.Services.Contacts.Domain.Contact;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Domain.Contact
{
    public class ContactCommunicationEntityConfiguration : IEntityTypeConfiguration<ContactCommunicationEntity>
    {
        public void Configure(EntityTypeBuilder<ContactCommunicationEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Value).IsRequired(false);

            builder.HasOne(x => x.Contact)
               .WithMany(p => p.Communications)
               .HasForeignKey(x => x.ContactId)
               .IsRequired(false);

            builder.ToTable("ContactCommunication");
        }
    }
}

using MicroserviceSample.Services.Contacts.Domain.Person;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Domain.Person
{
    public class PersonContactEntityConfiguration : IEntityTypeConfiguration<PersonContactEntity>
    {
        public void Configure(EntityTypeBuilder<PersonContactEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Value).IsRequired(false); 
             
             builder.HasOne(x => x.Person)
                .WithMany(p => p.Contacts)
                .HasForeignKey(x => x.PersonId)
                .IsRequired(false); 
        }
    }
}

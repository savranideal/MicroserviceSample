using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroserviceSample.Services.Contacts.Domain.Person;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Domain.Person
{
    public class PersonEntityConfiguration : IEntityTypeConfiguration<PersonEntity>
    {
        public void Configure(EntityTypeBuilder<PersonEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired(false);
            builder.Property(x => x.CompanyName).IsRequired(false);
             
             builder.HasMany(x => x.Contacts)
                .WithOne(p => p.Person)
                .HasForeignKey(x => x.PersonId)
                .IsRequired(false);
        }
    }
}

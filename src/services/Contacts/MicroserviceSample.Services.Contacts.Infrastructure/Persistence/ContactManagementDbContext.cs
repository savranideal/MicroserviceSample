using MicroserviceSample.Services.Contacts.Domain.Contact;
using MicroserviceSample.Services.Contacts.Infrastructure.Domain.Contact;

using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Persistence
{
    public class ContactManagementDbContext : DbContext
    {
        public ContactManagementDbContext(DbContextOptions<ContactManagementDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<ContactCommunicationEntity> ContactContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ContactEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ContactCommunicationEntityConfiguration());

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MicroserviceSample.Services.Contacts.Domain.Person;
using MicroserviceSample.Services.Contacts.Infrastructure.Domain.Person;

using Microsoft.EntityFrameworkCore;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Persistence
{
    public class ContactManagementDbContext : DbContext
    {
        public ContactManagementDbContext(DbContextOptions<ContactManagementDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<PersonContactEntity> PersonContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PersonContactEntityConfiguration());

        }
    }
}

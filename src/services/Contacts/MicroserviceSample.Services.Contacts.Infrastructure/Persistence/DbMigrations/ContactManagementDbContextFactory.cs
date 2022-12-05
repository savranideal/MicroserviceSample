using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Persistence.DbMigrations
{
    public class ContactManagementDbContextFactory : IDesignTimeDbContextFactory<ContactManagementDbContext>
    {
        // dotnet ef migrations add ContactManagementDbContextFactory --context ContactManagementDbContext --project src/services/Contacts/MicroserviceSample.Services.Contacts.Infrastructure --output-dir Persistence/DbMigrations

        public ContactManagementDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ContactManagementDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=microservicesample.contact;User Id=contactsa;Password=SplArmonsMAZONTINGEriCi", o => o.MigrationsAssembly("MicroserviceSample.Services.Contacts.Infrastructure"));
            return new ContactManagementDbContext(optionsBuilder.Options);
        }
    }
}

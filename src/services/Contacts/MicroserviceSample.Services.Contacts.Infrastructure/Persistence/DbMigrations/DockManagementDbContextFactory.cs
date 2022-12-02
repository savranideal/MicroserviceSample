using MicroserviceSample.Services.Contacts.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RoadRunner.Dock.Infrastructure.Context.DbMigrations
{
    public class ContactManagementDbContextFactory : IDesignTimeDbContextFactory<ContactManagementDbContext>
    {
        // dotnet ef migrations add ContactManagementDbContextFactory --context ContactManagementDbContext --project src/services/Contacts/MicroserviceSample.Services.Contacts.Infrastructure --output-dir Persistence/DbMigrations

        public ContactManagementDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ContactManagementDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=roadrunner.dockmanagement;User Id=roadrunner;Password=*__scotty__*", o => o.MigrationsAssembly("MicroserviceSample.Services.Contacts.Infrastructure"));
            return new ContactManagementDbContext(optionsBuilder.Options);
        }
    }
}

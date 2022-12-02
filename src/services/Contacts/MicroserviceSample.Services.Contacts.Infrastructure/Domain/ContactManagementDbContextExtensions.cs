using System.Reflection;

using MicroserviceSample.Services.Contacts.Domain.Person;
using MicroserviceSample.Services.Contacts.Infrastructure.Domain.Person;
using MicroserviceSample.Services.Contacts.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Domain
{
    public static class ContactManagementDbContextExtensions
    {
        private const string DockManagement = "DockManagement";

        public static void RegisterDbContext(IServiceCollection services,IConfiguration configuration,bool isProduction)
        { 
            services.AddDbContextPool<ContactManagementDbContext>((dbContextOptions) =>
            {
                dbContextOptions
                    .UseNpgsql(configuration.GetConnectionString(DockManagement), opts =>
                    {
                        opts.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        opts.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    });
                if (!isProduction)
                {
                    dbContextOptions.EnableDetailedErrors();
                    dbContextOptions.EnableSensitiveDataLogging();
                }
            });

            services.AddScoped<ContactManagementDbContext>();  
            services.AddScoped<IPersonRepository,PersonRepository>();
        }
    }
}

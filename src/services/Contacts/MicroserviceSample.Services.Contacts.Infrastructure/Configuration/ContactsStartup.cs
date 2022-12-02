using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroserviceSample.Services.Contacts.Infrastructure.Domain;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceSample.Services.Contacts.Infrastructure.Configuration
{
    public class ContactsStartup
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IConfiguration _configuration;

        public ContactsStartup(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _serviceCollection = serviceCollection;
            _configuration = configuration;
        }

        public void Configure(bool isProduction)
        {
            ContactManagementDbContextExtensions.RegisterDbContext(_serviceCollection, _configuration, isProduction);

        }
    }
}

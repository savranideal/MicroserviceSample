using System.Reflection;

using FluentValidation;

using MediatR;
using MicroserviceSample.BuildingBlocks.Application.CORS.Commands;
using MicroserviceSample.BuildingBlocks.Application.CORS.Queries;
using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;
using MicroserviceSample.Services.Contacts.Application.Contacts.CreateContact;
using MicroserviceSample.Services.Contacts.Infrastructure.Configuration.Behavior;
using MicroserviceSample.Services.Contacts.Infrastructure.Configuration.Command;
using MicroserviceSample.Services.Contacts.Infrastructure.Domain;
using MicroserviceSample.Services.Contacts.Infrastructure.Persistence; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace MicroserviceSample.Services.Contacts.Infrastructure.Configuration
{
    public class ContactsStartup
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public ContactsStartup(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _services = serviceCollection;
            _configuration = configuration;
        }

        public void ConfigureService(bool isProduction)
        {
            ContactManagementDbContextExtensions.RegisterDbContext(_services, _configuration, isProduction);
            AddCqrs(new[]
                    {
                        typeof(ValidationBehaviour<,>),
                        typeof(TransactionBehaviour<,>),
                    });

            _services.AddTransient<IDomainEventsDispatcher,DomainEventsDispatcher>();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            ContactManagementDbContext context = scope.ServiceProvider.GetRequiredService<ContactManagementDbContext>();
            
            context.Database.Migrate();
        }

        public IServiceCollection AddCqrs(params Type[] pipelines)
        {
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient;
            var assemblies = new[] { typeof(CreateContactCommand).Assembly, Assembly.GetCallingAssembly() };
            _services.AddMediatR(assemblies,
                x =>
                {
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Transient:
                            x.AsTransient();
                            break;
                        case ServiceLifetime.Scoped:
                            x.AsScoped();
                            break;
                        case ServiceLifetime.Singleton:
                            x.AsSingleton();
                            break;
                    }
                });

            foreach (var pipeline in pipelines)
            {
                _services.AddScoped(typeof(IPipelineBehavior<,>), pipeline);
            }

            _services.AddTransient<ICommandProcessor, CommandProcessor>();
            _services.AddTransient<IQueryProcessor, QueryProcessor>();
            
            _services.AddValidatorsFromAssemblies(assemblies);
            return _services;
        }
    }
}

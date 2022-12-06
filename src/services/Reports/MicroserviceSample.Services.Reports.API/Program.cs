
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using Serilog.Core;
using Serilog.Events;
using Serilog;
using MicroserviceSample.BuildingBlocks.Application.Exception.Types;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Hellang.Middleware.ProblemDetails;
using MicroserviceSample.BuildingBlocks.Infrastructure.EventBus;
using MicroserviceSample.BuildingBlocks.Infrastructure.Persistence;
using MicroserviceSample.Services.Reports.API.Infrastructure.Domain;
using MicroserviceSample.Services.Reports.API.Infrastructure.EventBus;
using System.Reflection;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MicroserviceSample.Services.Reports.API.Application;
using MicroserviceSample.Services.Reports.API.Application.Services;
using MicroserviceSample.Services.Reports.API.Domain;
using MicroserviceSample.Services.Reports.API.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Extensions.Http;

namespace MicroserviceSample.Services.Reports.API
{
    public class Program
    {
        private const string GroupNameFormat = "'v'VVV";
        internal static readonly LoggingLevelSwitch MinimumLoggingLevel = new();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = ApiVersion.Default;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = GroupNameFormat;
                options.SubstituteApiVersionInUrl = true;
            });
            builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

            builder.Services.AddEndpointsApiExplorer();

            AddSwagger(builder);

            builder.Services.AddDbContextPool<ReportManagementDbContext>((dbContextOptions) =>
          {
              dbContextOptions
                  .UseNpgsql(builder.Configuration.GetConnectionString("Report"), opts =>
                  {
                      opts.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                      opts.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                  });
              if (!builder.Environment.IsProduction())
              {
                  dbContextOptions.EnableDetailedErrors();
                  dbContextOptions.EnableSensitiveDataLogging();
              }
          });
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            builder.Services.AddMediatR(assemblies);
            builder.Services.AddScoped<ReportManagementDbContext>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IUnitOfWork, ReportUnitOfWork>();
            builder.Services.AddScoped<IEventBus, CapPublisher>();
            builder.Services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
            builder.Services.AddTransient<CreateReportEventHandler>();

            builder.Logging.ClearProviders();
            MinimumLoggingLevel.MinimumLevel = LogEventLevel.Information;
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.ControlledBy(MinimumLoggingLevel)
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .Enrich.WithProperty("Application", "Report-api")
                .WriteTo.Console()
                .CreateLogger();

            builder.Logging.AddSerilog(logger);

            builder = AddCustomProblemDetails(builder);

            builder = AddContactApi(builder);
            
            builder = AddCap(builder);

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
            {
                app.UseSwagger();
                IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                app.UseSwaggerUI(c =>
                 {
                     c.DefaultModelsExpandDepth(-1);
                     foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                     {
                         c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                     }
                 });
            }

            app.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/liveness", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseAuthorization();
            app.MapControllers();

            using IServiceScope scope = app.Services.CreateScope();
            ReportManagementDbContext context = scope.ServiceProvider.GetRequiredService<ReportManagementDbContext>();
            context.Database.Migrate();

            app.Run();
        }

        public static WebApplicationBuilder AddCustomProblemDetails(WebApplicationBuilder builder)
        {
            builder.Services.AddProblemDetails(x =>
            {
                x.ShouldLogUnhandledException = (httpContext, exception, problemDetails) =>
                {
                    IHostEnvironment env = httpContext.RequestServices.GetRequiredService<IHostEnvironment>();
                    return env.IsDevelopment() || env.IsStaging() || env.IsEnvironment("Docker");
                };

                x.IncludeExceptionDetails = (ctx, _) =>
                {
                    IHostEnvironment env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                    return env.IsDevelopment() || env.IsStaging();
                };

                x.Map<ConflictException>(ex => new ProblemDetails
                {
                    Title = "Application rule broken",
                    Status = StatusCodes.Status409Conflict,
                    Detail = ex.Message,
                    Type = "https://Reportapidomain/application-rule-validation-error"
                });

                x.Map<BadRequestException>(ex => new ProblemDetails
                {
                    Title = "bad request exception",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Type = "https://Reportapidomain/bad-request-error"
                });

                x.Map<ArgumentException>(ex => new ProblemDetails
                {
                    Title = "argument is invalid",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Type = "https://Reportapidomain/argument-error"
                });

                x.Map<ResourceNotFoundException>(ex => new ProblemDetails
                {
                    Title = "not found exception",
                    Status = (int)ex.StatusCode,
                    Detail = ex.Message,
                    Type = "https://Reportapidomain/not-found-error"
                });

                x.Map<ApiException>(ex => new ProblemDetails
                {
                    Title = "api server exception",
                    Status = StatusCodes.Status404NotFound,
                    Detail = ex.Message,
                    Type = "https://Reportapidomain/api-server-error"
                });

                x.MapToStatusCode<ArgumentNullException>(StatusCodes.Status400BadRequest);

                x.MapStatusCode = context => new StatusCodeProblemDetails(context.Response.StatusCode);
            });

            return builder;
        }

        public static WebApplicationBuilder AddSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(swaggerGenOptions =>
          {
              swaggerGenOptions.EnableAnnotations();
              swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
              {
                  Title = "Reports HTTP API",
                  Version = "v1",
                  Description = "Reports HTTP API"
              });

              swaggerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MicroserviceSample.Services.Reports.API.xml"));
              swaggerGenOptions.TagActionsBy(api =>
              {
                  bool hasGroupName = api.ActionDescriptor.EndpointMetadata.Any(c => c is ApiExplorerSettingsAttribute);
                  if (!hasGroupName)
                  {
                      if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor2)
                      {
                          return new[] { controllerActionDescriptor2.ControllerName };
                      }
                  }

                  if (api.GroupName != null)
                  {
                      return new[] { api.GroupName };
                  }

                  if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                  {
                      return new[] { controllerActionDescriptor.ControllerName };
                  }

                  throw new InvalidOperationException("Unable to determine tag for endpoint.");
              });
              swaggerGenOptions.DocInclusionPredicate((_, _) => true);
          });
            return builder;
        }

        private static WebApplicationBuilder AddContactApi(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<HttpClientCorrelationIdDelegatingHandler>();

            builder.Services.AddHttpClient<IContactService, ContactService>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(builder.Configuration.GetSection("Services").GetSection("Contact").Value);
                    c.Timeout = TimeSpan.FromSeconds(30);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (req, cert, chain, sslPolicy) => true,
                    };
                })
                .AddHttpMessageHandler<HttpClientCorrelationIdDelegatingHandler>();
                //.AddPolicyHandler(GetRetryPolicy())
                //.AddPolicyHandler(GetCircuitBreakerPolicy());

            return builder;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        private static WebApplicationBuilder AddCap(WebApplicationBuilder builder)
        {
            builder.Services.AddCap(capOptions =>
            {
                capOptions.UseDashboard(c =>
               {
                   c.PathMatch = "/events";
               });
                capOptions.DefaultGroupName = "contact-report-services";

                IConfigurationSection rabbitMQSection = builder.Configuration.GetSection("Cap").GetSection("RabbitMq");
                capOptions.UseRabbitMQ(conf =>
                 {
                     conf.HostName = rabbitMQSection.GetSection("Hostname").Value!;
                     conf.UserName = rabbitMQSection.GetSection("Username").Value!;
                     conf.Password = rabbitMQSection.GetSection("Password").Value!;
                     conf.Port = 5672;
                 });

                capOptions.UsePostgreSql(c =>
                {
                    c.Schema = "EventBus";
                    c.ConnectionString = builder.Configuration.GetConnectionString("Report")!;
                });

                capOptions.FailedRetryCount = 1;
                capOptions.FailedRetryInterval = 30;
                capOptions.DefaultGroupName = "ReportApi";
                capOptions.Version = "v1";
            });

            return builder;
        }
    }
}

using MicroserviceSample.BuildingBlocks.Application.Exception.Types;
using MicroserviceSample.Services.Contacts.Infrastructure.Configuration;

using Microsoft.AspNetCore.Mvc;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Hellang.Middleware.ProblemDetails;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MicroserviceSample.Services.Contacts.API
{
    public class Program
    {
        private const string GroupNameFormat = "'v'VVV";
        internal static readonly LoggingLevelSwitch MinimumLoggingLevel = new();
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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

            builder.Services.AddEndpointsApiExplorer();
            AddSwagger(builder);

            ContactsStartup contactModule = new(builder.Services, builder.Configuration);

            contactModule.ConfigureService(builder.Environment.EnvironmentName == "Production");

            builder.Logging.ClearProviders();
            MinimumLoggingLevel.MinimumLevel = LogEventLevel.Information;
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.ControlledBy(MinimumLoggingLevel)
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .Enrich.WithProperty("Application", "contact-api")
                .WriteTo.Console()
                .CreateLogger();

            builder.Logging.AddSerilog(logger);

            builder = AddCustomProblemDetails(builder);

            var app = builder.Build();


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
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            contactModule.Configure(app.Services);

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
                    Type = "https://contactapidomain/application-rule-validation-error"
                });

                x.Map<ValidationException>(ex => new ProblemDetails
                {
                    Title = "input validation rules broken",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = System.Text.Json.JsonSerializer.Serialize(ex.Errors),
                    Type = "https://contactapidomain/input-validation-rules-error"
                });
                x.Map<BadRequestException>(ex => new ProblemDetails
                {
                    Title = "bad request exception",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Type = "https://contactapidomain/bad-request-error"
                });
                x.Map<ArgumentException>(ex => new ProblemDetails
                {
                    Title = "argument is invalid",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Type = "https://contactapidomain/argument-error"
                });
                x.Map<ResourceNotFoundException>(ex => new ProblemDetails
                {
                    Title = "not found exception",
                    Status = (int)ex.StatusCode,
                    Detail = ex.Message,
                    Type = "https://contactapidomain/not-found-error"
                });
                x.Map<ApiException>(ex => new ProblemDetails
                {
                    Title = "api server exception",
                    Status = StatusCodes.Status404NotFound,
                    Detail = ex.Message,
                    Type = "https://contactapidomain/api-server-error"
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
                  Title = "Contacts HTTP API",
                  Version = "v1",
                  Description = "Contacts HTTP API"
              });

              swaggerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MicroserviceSample.Services.Contacts.API.xml"));
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
    }
}
using Autofac.Core;
using Daniel.Common.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using System.Reflection;
using TUF.Api.Middleware;
using TUF.Database.DbContexts;
using TUF.Database.Identity.Models;
using TUF.Infrastructure.Notifications;
using TUF.Infrastructure.OpenApi;
using TUF.Shared.Services;
using ZymLabs.NSwag.FluentValidation;

namespace TUF.Api.Configurations
{
    public static partial class Startup
    {
        private const string CorsPolicy = nameof(CorsPolicy);

        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {  
            builder.MapControllers();//.RequireAuthorization();            
            //builder.MapHealthCheck();
            //builder.MapNotifications();
            return builder;
        }

        private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health").RequireAuthorization();

        internal static IEndpointRouteBuilder MapNotifications(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<NotificationHub>("/notifications", options =>
            {
                options.CloseOnAuthenticationExpiration = true;
            });
            return endpoints;
        }
         

        internal static ConfigureHostBuilder AddConfigurations(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                const string configurationsDirectory = "Configurations";
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/logger.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/logger.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/hangfire.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/hangfire.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cache.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cache.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cors.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cors.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/database.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/mail.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/mail.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/middleware.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/middleware.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/security.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/security.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/openapi.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/openapi.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/signalr.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/signalr.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/securityheaders.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/securityheaders.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/localization.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/localization.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });
            return host;
        }

         
        internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
        app.UseCors(CorsPolicy);


        public static IServiceCollection AddVersionedApiExplorer(this IServiceCollection services, Action<ApiExplorerOptions> setupAction)
        {
            AddApiExplorerServices(services);
            services.Configure(setupAction);
            return services;
        }
        static void AddApiExplorerServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddMvcCore().AddApiExplorer();            
        }
        internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
        {            
            var settings = config.GetSection(nameof(TUF.Infrastructure.OpenApi.SwaggerSettings)).Get<TUF.Infrastructure.OpenApi.SwaggerSettings>();
            if (settings.Enable)
            {
                services.AddVersionedApiExplorer(o => o.SubstituteApiVersionInUrl = true);
                services.AddEndpointsApiExplorer();
                services.AddOpenApiDocument((document, serviceProvider) =>
                {
                    document.PostProcess = doc =>
                    {
                        doc.Info.Title = settings.Title;
                        doc.Info.Version = settings.Version;
                        doc.Info.Description = settings.Description;
                        doc.Info.Contact = new()
                        {
                            Name = settings.ContactName,
                            Email = settings.ContactEmail,
                            Url = settings.ContactUrl
                        };
                        doc.Info.License = new()
                        {
                            Name = settings.LicenseName,
                            Url = settings.LicenseUrl
                        };
                    };

                    document.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "Input your Bearer token to access this API",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT",
                    });

                    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                    document.OperationProcessors.Add(new SwaggerGlobalAuthProcessor());

                    document.TypeMappers.Add(new PrimitiveTypeMapper(typeof(TimeSpan), schema =>
                    {
                        schema.Type = NJsonSchema.JsonObjectType.String;
                        schema.IsNullableRaw = true;
                        schema.Pattern = @"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$";
                        schema.Example = "02:00:00";
                    }));

                    document.OperationProcessors.Add(new SwaggerHeaderAttributeProcessor());

                    var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetService<FluentValidationSchemaProcessor>();
                    document.SchemaProcessors.Add(fluentValidationSchemaProcessor);
                });
                services.AddScoped<FluentValidationSchemaProcessor>();
            }

            return services;
        }


        internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
        {
            if (config.GetValue<bool>("SwaggerSettings:Enable"))
            {    
                app.UseOpenApi();
                app.UseSwaggerUi3(options =>
                {
                    options.DefaultModelsExpandDepth = -1;
                    options.DocExpansion = "none";
                    options.TagsSorter = "alpha";
                    //if (config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
                    //{
                    options.OAuth2Client = new OAuth2ClientSettings
                    {
                        AppName = "Full Stack Hero Api Client",
                        ClientId = config["SecuritySettings:Swagger:OpenIdClientId"],
                        ClientSecret = string.Empty,
                        UsePkceWithAuthorizationCodeGrant = true,
                        ScopeSeparator = " "
                    };
                    options.OAuth2Client.Scopes.Add(config["SecuritySettings:Swagger:ApiScope"]);
                    //}
                });
            }
            return app;
        }
         
    }


   

}

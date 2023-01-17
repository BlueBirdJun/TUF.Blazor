using Autofac.Core;
using Daniel.Common.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TUF.Api.Middleware;
using TUF.Database.DbContexts;
using TUF.Database.Identity.Models;
using TUF.Domains.Infrastructure;
using TUF.Shared.Services;

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

        internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
        {
            if (config.GetValue<bool>("SwaggerSettings:Enable"))
            {
                
                //app.UseOpenApi();
                //app.UseSwaggerUi3(options =>
                //{
                //    options.DefaultModelsExpandDepth = -1;
                //    options.DocExpansion = "none";
                //    options.TagsSorter = "alpha";
                //    //if (config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
                //    //{
                //    //    options.OAuth2Client = new OAuth2ClientSettings
                //    //    {
                //    //        AppName = "Full Stack Hero Api Client",
                //    //        ClientId = config["SecuritySettings:Swagger:OpenIdClientId"],
                //    //        ClientSecret = string.Empty,
                //    //        UsePkceWithAuthorizationCodeGrant = true,
                //    //        ScopeSeparator = " "
                //    //    };
                //    //    options.OAuth2Client.Scopes.Add(config["SecuritySettings:Swagger:ApiScope"]);
                //    //}
                //});
            }
            return app;
        }


    }


   

}

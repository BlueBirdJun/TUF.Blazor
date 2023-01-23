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

namespace TUF.Api.Configurations;

internal static partial class Startup
{
    private const string CorsPolicy = nameof(CorsPolicy);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {  
        builder.MapControllers().RequireAuthorization();            
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
                //.AddJsonFile($"{configurationsDirectory}/securityheaders.json", optional: true, reloadOnChange: true)
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
    
   
}




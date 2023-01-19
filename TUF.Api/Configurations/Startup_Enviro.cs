using Autofac.Core;
using Daniel.Common.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using TUF.Api.Middleware;
using TUF.Database.DbContexts;
using TUF.Database.Identity.Models;

using TUF.Infrastructure.Auth.Jwt;
using TUF.Infrastructure.Auth.Permissions;
using TUF.Shared.Services;

namespace TUF.Api.Configurations;

public static partial class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(assembly);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
    builder
    .UseRequestLocalization()
    .UseStaticFiles()
    //.UseSecurityHeaders(config)
    //.UseFileStorage()
    //.UseExceptionMiddleware()
    .UseHttpsRedirection()
    .UseRouting()
    .UseCorsPolicy()
    .UseAuthentication()
    //.UseCurrentUser()
    //.UseMultiTenancy()
    .UseAuthorization()
    .UseRequestLogging(config)
    //.UseHangfireDashboard(config)
    .UseOpenApiDocumentation(config);

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<IDateTimeService, SystemDateTimeService>();
        services.AddTransient<ICrytoManager, CrytoManager>();
        services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        //MapsterSettings.Configure();
        return services
            .AddApiVersioning()
            .ServiceResist(config)
            .AddAuth(config)
            //.AddBackgroundJobs(config)
            //.AddCaching(config)
            //.AddCorsPolicy(config)
            //.AddExceptionMiddleware()
            //.AddHealthChecks()
            //.AddPOLocalization(config)
            //.AddMailing(config)                
            //.AddMediatR(Assembly.GetExecutingAssembly())
            //.AddMultitenancy(config)
            //.AddNotifications(config)
            .AddOpenApiDocumentation(config)
            //.AddPersistence(config)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true);
        //.AddServices();
    }

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
    app.UseMiddleware<CurrentUserMiddleware>();

    internal static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpsLogging)
        {
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddScoped<ResponseLoggingMiddleware>();
        }
        return services;
    }

    internal static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpsLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
        }
        return app;
    }
    private static MiddlewareSettings GetMiddlewareSettings(IConfiguration config) =>
   config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>();

    public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetSection("DatabaseSettings:IdentityConnection").Value), ServiceLifetime.Transient);
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            configuration.GetSection("DatabaseSettings:ApplicationConnection").Value), ServiceLifetime.Transient);
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            //options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 1;
            options.Password.RequireDigit = false;
            //options.SignIn.RequireConfirmedAccount = false;
            //options.Password.RequireNonAlphanumeric = true;
            //options.Password.RequiredLength = 6;                
            //options.Password.RequireUppercase = false;
            //options.Password.RequireLowercase = false;
            ////options.Password.RequireNonAlphanumeric = true;
            //options.Password.RequiredUniqueChars = 1;
            //options.Password.RequireDigit = false;
            //options.Password.req = false;
            options.User.AllowedUserNameCharacters = null;
        }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
    }

    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
   services
       .AddScoped<CurrentUserMiddleware>()
       .AddScoped<ICurrentUser, CurrentUser>()
       .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());

    internal static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<JwtSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(JwtSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        return services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!)
            .Services;
        
        
    }

    private static IServiceCollection AddPermissions(this IServiceCollection services) =>
    services
        .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
        .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

}

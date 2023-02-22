using Daniel.Common.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using TUF.Database.DbContexts;
using TUF.Database.Identity.Models;
using TUF.HostApi.Authentication;
using TUF.HostApi.Middleware;
using TUF.Infrastructure.Auth;
using TUF.Infrastructure.Auth.Jwt;
using TUF.Infrastructure.Auth.Permissions;
using TUF.Infrastructure.Caching;
using TUF.Infrastructure.OpenApi;
using TUF.Infrastructure.SecurityHeaders;
using TUF.Infrastructure.Cors;

using TUF.Shared.Authorization;
using TUF.Shared.Services;
using TUF.Infrastructure.Notifications;
using Microsoft.Extensions.DependencyInjection;
using TUF.Database.TUFDB;
using TUF.Application.Multitenancy;
using Microsoft.Extensions.Primitives;
using TUF.Infrastructure.Multitenancy;
using Autofac.Core;
using Finbuckle.MultiTenant;

namespace TUF.HostApi.Configurations;

internal static partial class Startup
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(assembly);
            //.AddAutoMapper(assembly);
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<IDateTimeService, SystemDateTimeService>();
        services.AddTransient<ICrytoManager, CrytoManager>();
        services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();     
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        return services
            .AddApiVersioning()
            .ServiceResist(config)
            .AddCaching(config)
            .AddHealthCheck()
            .AddCorsPolicy(config)
            .AddOpenApiDocumentation(config)
            .AddAuth(config)
            .AddNotifications(config)
            .AddRouting(options => options.LowercaseUrls = true);
    }

    public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
        options.UseSqlServer(configuration.GetSection("DatabaseSettings:IdentityConnection").Value)
        , ServiceLifetime.Transient);

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            configuration.GetSection("DatabaseSettings:ApplicationConnection").Value)
        , ServiceLifetime.Transient);

        services.AddDbContext<TenantDbContext>(options =>
        options.UseSqlServer(
            configuration.GetSection("DatabaseSettings:ApplicationConnection").Value)
        , ServiceLifetime.Transient);
        //.AddMultiTenant<TUFTenantInfo>()
        //.WithBasePathStrategy(options => options.RebaseAspNetCorePathBase = true)
        //.WithConfigurationStore();
        //   .WithClaimStrategy(TUFClaims.Tenant)
        //   .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
        //  .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
        // .WithEFCoreStore<TenantDbContext, TUFTenantInfo>()
        //  .Services
        //.AddScoped<ITenantService, TenantService>();   

        services.AddMultiTenant<TenantInfo>()
            .WithBasePathStrategy(options => options.RebaseAspNetCorePathBase = true)
            .WithConfigurationStore();
    }

    private static FinbuckleMultiTenantBuilder<TUFTenantInfo> WithQueryStringStrategy(this FinbuckleMultiTenantBuilder<TUFTenantInfo> builder, string queryStringKey) =>
        builder.WithDelegateStrategy(context =>
        {
            if (context is not HttpContext httpContext)
            {
                return Task.FromResult((string?)null);
            }
            httpContext.Request.Query.TryGetValue(queryStringKey, out StringValues tenantIdParam);
            return Task.FromResult((string?)tenantIdParam.ToString());
        });


    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
   builder
   .UseRequestLocalization()
   .UseStaticFiles()
   .UseSecurityHeaders(config)
   //.UseFileStorage()
   //.UseExceptionMiddleware()
   .UseHttpsRedirection()
   .UseRouting()
   .UseCorsPolicy()
   .UseAuthentication()
   .UseAuthorization()       
   .UseCurrentUser()
   //.UseMultiTenancy() 
   //.UseRequestLogging(config)
   //.UseHangfireDashboard(config)
   .UseOpenApiDocumentation(config);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        //builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
       endpoints.MapHealthChecks("/api/health").RequireAuthorization();

    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        return services
        .AddCurrentUser()
        .AddJwtAuth(config)
        .AddIdentity();
        //.AddPermissions(); 
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
        var c = config.GetSection("SecuritySettings:JwtSettings:key").Value;
        byte[] key = Encoding.ASCII.GetBytes(c);

        //return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
        //    AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            //ValidAudience = "domain.com",
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            //ValidIssuer = "domain.com",
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ClockSkew = TimeSpan.Zero,
        //            IssuerSigningKey = new SymmetricSecurityKey(key) // NOTE: THIS SHOULD BE A SECRET KEY NOT TO BE SHARED; A GUID IS RECOMMENDED
        //        };
        //    }).Services;

        return services.AddAuthentication(authentication =>
          {
              authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
           .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!)
          .Services;
    }
     
    internal static IServiceCollection AddIdentity(this IServiceCollection services) =>
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders()
           //.AddDefaultTokenProviders()
            .Services;



    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
   app.UseMiddleware<CurrentUserMiddleware>();

    private static IServiceCollection AddPermissions(this IServiceCollection services) =>
    services
      .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
      .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
       services.AddHealthChecks().Services;

}

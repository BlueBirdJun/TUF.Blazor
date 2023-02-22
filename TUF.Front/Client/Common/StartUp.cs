using Blazored.LocalStorage;
using Daniel.Common.Interfaces;
using MediatR;
using MediatR.Courier;
using MediatR.Courier.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using TUF.Front.Client.Infrastructure.Notifications;
using TUF.Front.Client.Services;
using TUF.Front.Client.Shared;
using TUF.Shared.Authorization;

namespace TUF.Front.Client.Common;

public static class StartUp
{
    public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration config) =>
        services.AddBlazoredLocalStorage()
        .AddMudServices(configuration =>
        {
            configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            configuration.SnackbarConfiguration.HideTransitionDuration = 100;
            configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
            configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
            configuration.SnackbarConfiguration.ShowCloseIcon = false;
        })
        //.AddServices(typeof(ITransient), ServiceLifetime.Transient)
        //.AutoRegisterInterfaces<ITransient>()
        .AddTransient<ITokenService, TokenService>()
        .AddTransient<IApiHelper, ApiHelper>()        
        .AddAuthentication(config)
        .AddNotifications()
        .AddAuthorizationCore(RegisterPermissionClaims);

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<JwtAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationService>());
        return services;
    }
    private static void RegisterPermissionClaims(AuthorizationOptions options)
    {
        foreach (var permission in TUFPermissions.All)
        {
            options.AddPolicy(permission.Name, policy => policy.RequireClaim(TUFClaims.Permission, permission.Name));
        }
    }

    private static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t)
                            && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                })
                .Where(t => t.Service is not null
                            && interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
        {
            services.AddService(type.Service!, type.Implementation, lifetime);
        }

        return services;
    }
    private static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
   lifetime switch
   {
       ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
       ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
       ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
       _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
   };

    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services
           .AddMediatR(assemblies)
           .AddCourier(assemblies)
           .AddTransient<INotificationPublisher, NotificationPublisher>();
        foreach (var eventType in assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces().Any(i => i == typeof(INotificationMessage))))
        {
            services.AddSingleton(
                typeof(INotificationHandler<>).MakeGenericType(
                    typeof(NotificationWrapper<>).MakeGenericType(eventType)),
                serviceProvider => serviceProvider.GetRequiredService(typeof(MediatRCourier)));
        }
        return services;
    }


}

using Daniel.Common.Interfaces;

namespace TUF.Api.Configurations;

internal static partial class Startup
{
    internal static IServiceCollection ServiceResist(this IServiceCollection services, IConfiguration config)
    {
        return services
                 .AddServices(typeof(ITransient), ServiceLifetime.Transient)
                 .AddServices(typeof(IScoped), ServiceLifetime.Scoped);
                 //.AddSingleton(typeof(IScoped), ServiceLifetime.Singleton);


    }
    internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
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
    internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
   lifetime switch
   {
       ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
       ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
       ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
       _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
   };
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        return services
        .AddCurrentUser()        
        .AddJwtAuth(config)
        .AddPermissions();
        
        // Must add identity before adding auth!
        //.AddIdentity();
        //services.Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));
        //return config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase)
        //    ? services.AddAzureAdAuth(config)
        //   : services.AddJwtAuth(config);
    }
}

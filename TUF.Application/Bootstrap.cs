using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TUF.Application
{
    public static class Bootstrap
    {
        public static IServiceCollection AddApplicationInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            return services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
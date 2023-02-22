using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Shared.Notification;

namespace TUF.Infrastructure.Notifications;

public static class Startup
{
    public static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration config)
    {
        ILogger logger = Log.ForContext(typeof(Startup));
        var signalRSettings = config.GetSection(nameof(SignalRSettings)).Get<SignalRSettings>();
        services.AddSignalR();
        //if (!signalRSettings.UseBackplane)
        //{
        //    services.AddSignalR();
        //}
        //else
        //{
        //    var backplaneSettings = config.GetSection("SignalRSettings:Backplane").Get<SignalRSettings.Backplane>();
        //    if (backplaneSettings is null) throw new InvalidOperationException("Backplane enabled, but no backplane settings in config.");
        //    switch (backplaneSettings.Provider)
        //    {
        //        //case "redis":
        //        //    if (backplaneSettings.StringConnection is null) throw new InvalidOperationException("Redis backplane provider: No connectionString configured.");
        //        //    services.AddSignalR().AddStackExchangeRedis(backplaneSettings.StringConnection, options =>
        //        //    {
        //        //        options.Configuration.AbortOnConnectFail = false;
        //        //    });
        //        //    break;
        //        default:
        //            throw new InvalidOperationException($"SignalR backplane Provider {backplaneSettings.Provider} is not supported.");
        //    } 
        //    logger.Information($"SignalR Backplane Current Provider: {backplaneSettings.Provider}.");
        //} 
        return services;
    }

    public static IEndpointRouteBuilder MapNotifications(this IEndpointRouteBuilder endpoints)
    {    
        endpoints.MapHub<SignalHub>(SignalClient.HUBURL, options =>
        {        
        });
        endpoints.MapHub<NotificationsHub2>("/noti", options =>
        {
            options.CloseOnAuthenticationExpiration = true;
        });
        return endpoints;
    }
}

namespace TUF.Api.Configurations
{
    public static partial class Startup
    {
        internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
        {
            return services
            .AddCurrentUser();
            //.AddPermissions()
            // Must add identity before adding auth!
            //.AddIdentity();
            //services.Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));
            //return config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase)
            //    ? services.AddAzureAdAuth(config)
             //   : services.AddJwtAuth(config);
        }
    }
}

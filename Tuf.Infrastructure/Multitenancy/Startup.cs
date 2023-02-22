using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Application.Multitenancy;
using TUF.Database.TUFDB;
using TUF.Shared.Authorization;

namespace TUF.Infrastructure.Multitenancy;

public static class Startup
{
    public static IServiceCollection AddMultitenancy(this IServiceCollection services, IConfiguration config)
    { 
        return services
            .AddMultiTenant<TUFTenantInfo>()
            .WithClaimStrategy(TUFClaims.Tenant)
            //.WithHeaderStrategy(MultitenancyConstants.TenantIdName)
            //.WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
           // .WithEFCoreStore<TenantDbContext, TUFTenantInfo>()
               .Services
            .AddScoped<ITenantService, TenantService>(); ;
    }
    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
       app.UseMultiTenant();

}

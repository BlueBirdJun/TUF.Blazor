using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Application.Common.Caching;

namespace TUF.Infrastructure.Caching;
public class CacheKeyService : ICacheKeyService
{
    private readonly ITenantInfo? _currentTenant;

    //public CacheKeyService(ITenantInfo currentTenant) => _currentTenant = currentTenant;
    public CacheKeyService()
    {

    }

    public string GetCacheKey(string name, object id, bool includeTenantId = true)
    {
        //string tenantId = includeTenantId
        //    ? _currentTenant?.Id ?? throw new InvalidOperationException("GetCacheKey: includeTenantId set to true and no ITenantInfo available.")
        //    : "GLOBAL";
        return $"{name}-{id}";
    }
}
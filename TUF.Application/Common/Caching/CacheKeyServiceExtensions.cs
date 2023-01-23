using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Domains.Common.Contracts;

namespace TUF.Application.Common.Caching;

public static class CacheKeyServiceExtensions
{
    public static string GetCacheKey<TEntity>(this ICacheKeyService cacheKeyService, object id, bool includeTenantId = true)
    where TEntity : IEntity =>
        cacheKeyService.GetCacheKey(typeof(TEntity).Name, id, includeTenantId);
}
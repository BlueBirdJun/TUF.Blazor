using Daniel.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Application.Common.Caching;

public interface ICacheKeyService : IScoped
{
    public string GetCacheKey(string name, object id, bool includeTenantId = true);
}

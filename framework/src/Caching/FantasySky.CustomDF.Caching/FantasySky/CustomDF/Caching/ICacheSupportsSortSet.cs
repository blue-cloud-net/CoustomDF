using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

namespace FantasySky.CustomDF.Caching;

public interface ICacheSupportsSortSet
{
    bool SortedSetAdd(string key, double order, byte[] value, DistributedCacheEntryOptions options);
}

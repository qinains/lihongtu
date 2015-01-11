using System;
using System.Collections.Generic;
using System.Text;

using System.Web.Caching;

namespace CacheDependency
{
    public interface ICacheDependency
    {
        AggregateCacheDependency GetDependency();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;

using System.Web.Caching;

namespace CacheDependency
{
    public static class DependencyFacatory
    {
        private static readonly string path = ConfigurationManager.AppSettings["CacheDependencyAssembly"];

        public static AggregateCacheDependency GetWebServiceUrlDependency()
        {
            if (!String.IsNullOrEmpty(path))
                return DependencyAccess.CreateWebServiceUrlDependency().GetDependency();
            else
                return null;
        }
    }
}

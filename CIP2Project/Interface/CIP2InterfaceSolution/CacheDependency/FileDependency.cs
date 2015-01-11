using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using System.Web.Caching;

namespace CacheDependency
{
    public class FileDependency:ICacheDependency
    {
        private char[] configSeparator = new char[] { ',' };
        private AggregateCacheDependency dependency = new AggregateCacheDependency();

        public FileDependency(String configKey) {
            String fileConfig = ConfigurationManager.AppSettings[configKey];
            String[] files = fileConfig.Split(configSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (String fi in files)
                dependency.Add(new System.Web.Caching.CacheDependency(fi));
        }

        public AggregateCacheDependency GetDependency()
        {
            return dependency;
        }

    }
}

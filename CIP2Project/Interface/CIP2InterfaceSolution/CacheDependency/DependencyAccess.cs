using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Configuration;
using System.Web.Caching;

namespace CacheDependency
{
    public static class DependencyAccess
    {
        #region FileCache

        public static ICacheDependency CreateWebServiceUrlDependency()
        {
            return LoadFileInstance("");
        }

        #endregion

        private static ICacheDependency LoadFileInstance(String className)
        {
            String path = ConfigurationManager.AppSettings["CacheDependencyAssembly"];
            String fullname = path + "." + className + ".cs";

            return (ICacheDependency)Assembly.Load(path).CreateInstance(fullname);
        }
    }
}

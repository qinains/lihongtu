using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Data;


namespace Linkage.BestTone.Interface.Rule
{
    public abstract class AbstractInfoManage
    {
        protected abstract String DataCacheName
        {
            get;
        }

        protected abstract String DataCacheExpireTime
        {
            get;
        }

        protected abstract DataSet ObjectData
        { 
            get;
        }

        public AbstractInfoManage()
        { }

        protected abstract object GetDataFromDB();
        public virtual object GetDataFromCache(HttpContext context)
        {
            if (context.Cache[DataCacheName] != null)
                return context.Cache[DataCacheName];

            Object obj = GetDataFromDB();
            context.Cache[DataCacheName] = obj;
            return obj;
        }
        public virtual object GetDataFromCache(WebService webservice)
        {
            if (webservice.Context.Cache[DataCacheName] != null)
                return webservice.Context.Cache[DataCacheName];

            Object obj = GetDataFromDB();
            webservice.Context.Cache[DataCacheName] = obj;
            return obj;
        }
    }
}

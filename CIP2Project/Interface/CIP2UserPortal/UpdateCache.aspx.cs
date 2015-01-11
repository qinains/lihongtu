using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UpdateCache : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        for (int i = 0; i < Cache.Count; i++)
        {
            IDictionaryEnumerator CacheEnum = Cache.GetEnumerator();
            string name = CacheEnum.Key.ToString();
            if (Cache[name] != null)
            {
                Cache.Remove(name);
            }
        }   


    }

    protected void RemoveAllCache()
    {
        //System.Web.Caching.Cache _cache = HttpRuntime.Cache;
        //IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
        //ArrayList al = new ArrayList();
        //while (CacheEnum.MoveNext())
        //{
        //    al.Add(CacheEnum.Key);
        //}
        //foreach (string key in al)
        //{
        //    _cache.Remove(key);
        //}
        //show();
    } 

}

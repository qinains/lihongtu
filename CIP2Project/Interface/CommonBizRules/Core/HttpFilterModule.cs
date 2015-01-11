using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// HttpFilterModule 的摘要说明
    /// </summary>
    public class HttpFilterModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            try
            {
                HttpApplication application = (HttpApplication)sender;

                ConstHelper constInstance = ConstHelper.DefaultInstance;
                Boolean isOpen = constInstance.IsOpenHttpFilter;
                String filterMethods = constInstance.HttpFilterMethods.ToLower();
                String requestUrl = application.Context.Request.Url.AbsoluteUri;
                msg.Append("context_AcquireRequestState\r\n");
                msg.Append(requestUrl + "\r\n");
                msg.Append(application.Context.Request.QueryString+"\r\n");
                msg.Append("\r\n");
                if (isOpen)
                {
                    if (filterMethods.IndexOf("get") >= 0)
                        FilterGetArgs(application);
                    if (filterMethods.IndexOf("post") >= 0)
                        FilterPostArgs(application);
                }
            }
            catch { }
            finally {
                log(msg.ToString());
            }
        }

        /// <summary>
        /// 过滤get方式参数(url中参数)
        /// </summary>
        protected void FilterGetArgs(HttpApplication application)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            if (application != null)
            {
                String requestUrl = application.Context.Request.Url.AbsoluteUri;
                msg.Append("FilterGetArgs\r\n");
                msg.Append(requestUrl+"\r\n");
                if (requestUrl.IndexOf("?") > 0)
                {
                    NameValueCollection queryCollection = application.Context.Request.QueryString;
                    String queryString = application.Context.Request.Url.Query;
                    msg.Append(queryString+"\r\n");
                    queryString = HttpUtility.UrlDecode(queryString);
                    msg.Append(queryString+"\r\n");
                    if (FilterUtility.IsHasExceptionTags(queryString))
                    {
                        String redirectUrl, queryParams = String.Empty;
                        redirectUrl = requestUrl.Substring(0, requestUrl.IndexOf("?"));

                        foreach (String key in queryCollection.Keys)
                        {
                            if (String.IsNullOrEmpty(key))
                                continue;
                            if (FilterUtility.IsHasExceptionTags(key))
                                continue;
                            String keyValue = queryCollection[key];
                            keyValue = FilterUtility.FilterExceptionTags(keyValue);

                            queryParams += String.Format("{0}={1}&", key, HttpUtility.UrlEncode(keyValue));
                        }
                        if (!String.IsNullOrEmpty(queryParams))
                            redirectUrl += "?" + queryParams.TrimEnd(new char[] { '&' });
                        application.Context.Response.Redirect(redirectUrl);
                    }
                }
            }
            log(msg.ToString());
        }

        /// <summary>
        /// 过滤post表单中参数
        /// </summary>
        protected void FilterPostArgs(HttpApplication application)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append("FilterPostArgs\r\n");
            if (application != null)
            {
                NameValueCollection formCollection = application.Context.Request.Form;
                foreach (String key in formCollection.Keys)
                {
                    if (FilterUtility.IsHasExceptionTags(key))
                        application.Context.Response.Redirect("ErrorInfo.aspx?ErroInfo=" + HttpUtility.UrlEncode("提交表单中含有非法字符"));
                    
                    String keyValue = formCollection[key];
                    if (FilterUtility.IsHasExceptionTags(keyValue))
                        application.Context.Response.Redirect("ErrorInfo.aspx?ErroInfo=" + HttpUtility.UrlEncode("提交表单中含有非法字符"));
                }
            }
            log(msg.ToString());
        }

        public void Dispose()
        {

        }


        protected void log(string str)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("HttpFilter", msg);
        }


    }
}

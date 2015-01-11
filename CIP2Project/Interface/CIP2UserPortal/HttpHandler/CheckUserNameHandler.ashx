<%@ WebHandler Language="C#" Class="CheckUserNameHandler" %>

using System;
using System.Web;

using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class CheckUserNameHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.ContentType = "text/plain";

        StringBuilder strMsg = new StringBuilder();
        try
        {
            String username = context.Request["username"] == null ? String.Empty : context.Request["username"];
            if (String.IsNullOrEmpty(username))
                strMsg.Append("[{result:\"false\",info:\"用户名不能为空\"}]");
            else
            {
                Int32 result = CustBasicInfo.IsExistUser(username);
                if (result == 0)
                    strMsg.Append("[{result:\"true\",info:\"恭喜,用户名可以使用\"}]");
                else
                    strMsg.Append("[{result:\"false\",info:\"用户名重复\"}]");
            }
        }
        catch (Exception ex)
        {
            strMsg = new StringBuilder();
            strMsg.Append("[{result:\"false\",info:\"异常\"}]");
        }
        
        context.Response.Write(strMsg.ToString());
        context.Response.Flush();
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
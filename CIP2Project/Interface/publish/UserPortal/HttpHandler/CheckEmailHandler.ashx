<%@ WebHandler Language="C#" Class="CheckEmailHandler" %>

using System;
using System.Web;

using System.Text;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class CheckEmailHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        context.Response.ContentType = "text/plain";
        context.Response.ContentEncoding = Encoding.UTF8;


        Int32 Result;
        String ErrMsg = String.Empty;
        
        StringBuilder returnMsg = new StringBuilder();
        
        try
        {
            String custID = context.Request["CustID"] == null ? String.Empty : context.Request["CustID"];
            String email = context.Request["Email"] == null ? String.Empty : context.Request["Email"];
            Result = SetMail.EmailSel(custID, email, ConstHelper.DefaultInstance.BesttoneSPID, out ErrMsg);
            if (Result == 0)
            {
                returnMsg.Append("[{result:\"true\",info:\"邮箱可以使用\"}]");
            }
            else
            {
                returnMsg.Append("[{result:\"false\",info:\"" + ErrMsg + "\"}]");
            }
        }
        catch (Exception ex)
        {
            returnMsg = new StringBuilder();
            returnMsg.Append("[{result:\"false\",info:\"异常:" + ex.Message + "\"}]");
        }

        context.Response.Write(returnMsg.ToString());
        context.Response.Flush();
        context.Response.End();
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
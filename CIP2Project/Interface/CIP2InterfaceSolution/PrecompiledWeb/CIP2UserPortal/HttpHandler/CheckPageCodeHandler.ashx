<%@ WebHandler Language="C#" Class="CheckPageCodeHandler" %>

using System;
using System.Web;

using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class CheckPageCodeHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.ContentType = "text/plain";

        StringBuilder returnMsg = new StringBuilder();

        try
        {
            String pageCode = context.Request["PageCode"] == null ? String.Empty : context.Request["PageCode"];
            if (CommonUtility.ValidateValidateCode(pageCode, context))
            {
                
            }
            else
            { 
                
            }
        }
        catch (Exception ex)
        { 
            
        }
        
        //context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
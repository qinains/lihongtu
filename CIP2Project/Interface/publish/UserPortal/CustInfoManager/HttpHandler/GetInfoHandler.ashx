<%@ WebHandler Language="C#" Class="GetInfoHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

public class GetInfoHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        StringBuilder returnMsg = new StringBuilder();
        try
        {
            String type = context.Request["Type"];
            switch (type)
            {
                case "GetAreaList":
                    returnMsg.Append(GetAreaList(context));
                    break;
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("[{result:\"false\",info:\"异常:" + ex.Message + "\"}]");
        }
        context.Response.Write(returnMsg.ToString());
        context.Response.Flush();
        context.Response.End();
    }
    
    protected String GetAreaList(HttpContext context)
    {        
        String provinceid = context.Request["ProvinceID"];
        StringBuilder returnMsg = new StringBuilder();

        //根据省获取市列表
        PhoneAreaInfoManager areaInfo = new PhoneAreaInfoManager();
        object areaData = areaInfo.GetPhoneAreaData(HttpContext.Current);
        DataSet pad = (DataSet)areaData;
        DataRow[] rows = pad.Tables["Area"].Select("ProvinceID='" + provinceid + "'");

        foreach (DataRow row in rows)
        {
            returnMsg.Append("{");
            returnMsg.AppendFormat("\"AreaID\":\"{0}\",", row["AreaID"].ToString());
            returnMsg.AppendFormat("\"AreaName\":\"{0}\"", row["AreaName"].ToString());
            returnMsg.Append("},");
        }

        returnMsg.Insert(0, "{\"result\":\"true\",\"areainfo\":[");
        returnMsg = returnMsg.Remove(returnMsg.Length - 1, 1);
        returnMsg.Append("]}");

        return returnMsg.ToString();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
<%@ WebHandler Language="C#" Class="CheckPhoneCodeHandler" %>

using System;
using System.Web;
using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class CheckPhoneCodeHandler : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {

        //----------------------------------------
        context.Response.Clear();
        context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        context.Response.ContentType = "text/plain";

        Int32 result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        StringBuilder returnResult = new StringBuilder();
        
        try
        {
            System.Threading.Thread.Sleep(1000);
            returnResult.Append("[");
            //获取参数
            String CustID = context.Request["CustID"];
            String PhoneNum = context.Request["PhoneNum"];
            String Code = context.Request["Code"];

            //手机验证码验证
            result = PhoneBO.SelSendSMSMassage(CustID, PhoneNum, Code, out ErrMsg);
            if (result == 0)
            {
                returnResult.Append("{result:\"true\",msg:\"验证通过\"}");
            }
            else
            {
                returnResult.Append("{result:\"false\",msg:\"验证码验证失败," + ErrMsg + "\"}");
            }
            
            returnResult.Append("]");
        }
        catch (Exception ex)
        {
            returnResult = new StringBuilder();
            returnResult.Append("[{result:\"false\",msg:\"发生异常，" + ex.Message + "\"}]");
        }

        context.Response.Write(returnResult.ToString());
        context.Response.Flush();
        context.Response.End();
    }

    
    public bool IsReusable {
        get {
            return false;
        }
    }

}
<%@ WebHandler Language="C#" Class="SaveInfoHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

public class SaveInfoHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        StringBuilder returnMsg = new StringBuilder();
        try
        {
            String type = context.Request["Type"];
            switch (type)
            {
                case "SaveCustInfo":
                    returnMsg.Append(SaveCustInfo(context));
                    break;
                case "SetAuthenPhone":
                    returnMsg.Append(SetAuthenPhone(context));
                    break;
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("[{\"result\":\"false\",\"info\":\"异常:" + ex.Message + "\"}]");
        }
        context.Response.Write(returnMsg.ToString());
        context.Response.Flush();
        context.Response.End();
    }

    protected String SaveCustInfo(HttpContext context)
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        
        String custid = context.Request["CustID"];
        StringBuilder returnMsg = new StringBuilder();

        string spid = HttpUtility.HtmlDecode(context.Request["SPID"]);
        string username = HttpUtility.HtmlDecode(context.Request["UserName"].ToString());
        string realname = HttpUtility.HtmlDecode(context.Request["RealName"].ToString());
        string nickname = HttpUtility.HtmlDecode(context.Request["NickName"].ToString());
        string sex = HttpUtility.HtmlDecode(context.Request["Sex"].ToString());
        string birthday = HttpUtility.HtmlDecode(context.Request["Birthday"].ToString());
        string provinceid = HttpUtility.HtmlDecode(context.Request["ProvinceID"].ToString());
        string areaid = HttpUtility.HtmlDecode(context.Request["AreaID"].ToString());
        string certificatecode = HttpUtility.HtmlDecode(context.Request["CertificateCode"].ToString());
        string certificatetype = HttpUtility.HtmlDecode(context.Request["CertificateType"].ToString());
        if (String.IsNullOrEmpty(certificatetype))
        {
            certificatecode = "";
        }
        string edulevel = HttpUtility.HtmlDecode(context.Request["EduLevel"].ToString());
        string incomelevel = HttpUtility.HtmlDecode(context.Request["IncomeLevel"].ToString());

        Result = CustBasicInfo.UpdateCustInfoById(custid, provinceid, areaid, certificatetype, certificatecode, realname, sex, nickname, DateTime.Now, birthday, edulevel, incomelevel, out ErrMsg);
        if (Result == 0)
        {
            CIP2BizRules.InsertCustInfoNotify(custid, "2", spid, "", "0", out ErrMsg);
            returnMsg.Append("[{\"result\":\"true\",\"info\":\"保存成功\"}]");
        }
        else
            returnMsg.Append("[{\"result\":\"false\",\"info\":\"保存失败\"}]");

        return returnMsg.ToString();
    }

    /// <summary>
    /// 设置认证手机
    /// </summary>
    public String SetAuthenPhone(HttpContext context)
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        String custid = context.Request["CustID"];
        String phoneNum = context.Request["PhoneNum"];
        String phoneCode = context.Request["PhoneCode"];
        StringBuilder returnMsg = new StringBuilder();
        //检查手机验证码是否有误
        //手机验证码验证
        Result = PhoneBO.SelSendSMSMassage(custid, phoneNum, phoneCode, out ErrMsg);
        if (Result != 0)
        {
            return "{result:\"CodeError\",msg:\"验证码验证失败\"}";
        }

        //设置认证手机
        Result = PhoneBO.PhoneSet(ConstHelper.DefaultInstance.BesttoneSPID, custid, phoneNum, "2", "2", out ErrMsg);
        if (Result == 0)
        {
            CIP2BizRules.InsertCustInfoNotify(custid, "2", ConstHelper.DefaultInstance.BesttoneSPID, "", "0", out ErrMsg);
            returnMsg.Append("[{\"result\":\"true\",\"info\":\"设置成功\"}]");
        }
        else
            returnMsg.Append("[{\"result\":\"false\",\"info\":\"设置失败\"}]");

        return returnMsg.ToString();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
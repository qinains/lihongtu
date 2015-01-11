<%@ WebHandler Language="C#" Class="GetPhoneAuthenCodeHandler" %>

using System;
using System.Web;
using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class GetPhoneAuthenCodeHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        context.Response.Clear();
        context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        context.Response.ContentType = "text/plain";

        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        String returnMsg = String.Empty;
        
        try
        {
            ////获取手机号码
            //String PhoneNum = context.Request["PhoneNum"] == null ? String.Empty : context.Request["PhoneNum"];

            //string custid = PhoneBO.IsAuthenPhone(PhoneNum, ConstHelper.DefaultInstance.BesttoneSPID, out ErrMsg);
            //if (String.IsNullOrEmpty(custid))
            //{
            //    returnMsg = "[{result:\"false\",msg:\"认证手机不正确\"}]";
            //}
            //else
            //{
            //    //生成手机随机验证码
            //    String AuthenCode = RandNum(6);
            //    CommonBizRules.SendMessage(PhoneNum, "您的手机验证码是:" + AuthenCode, "35000000");
            //    PhoneBO.InsertPhoneSendMassage(custid, "验证码信息内容", AuthenCode, PhoneNum, DateTime.Now, "描述未知", 1, 0, "1", out ErrMsg);
                
            //    ErrMsg = "";
            //    returnMsg = "[{result:\"true\",authencode:\"" + AuthenCode + "\",custid:\"" + custid + "\"}]";
            //}
            
            String type = context.Request["type"] == null ? String.Empty : context.Request["type"];
            switch (type)
            { 
                case "authen":
                    returnMsg = SendPhoneCodeForAuthen(context, out ErrMsg);
                    break;
                default:
                    returnMsg = SendPhoneCode(context, out ErrMsg);
                    break;
            }
        }
        catch (Exception ex)
        {
            ErrMsg += ex.Message;
            returnMsg = "[{result:\"false\",msg:\"验证码发送失败\"}]";
        }

        context.Response.Write(returnMsg);
        context.Response.Flush();
        context.Response.End();
        
    }

    protected String SendPhoneCodeForAuthen(HttpContext context, out String ErrMsg)
    {
        String ReturnMsg = String.Empty;
        ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        //获取手机号码
        String PhoneNum = context.Request["PhoneNum"] == null ? String.Empty : context.Request["PhoneNum"];
        String CustID_old = context.Request["CustID"] == null ? String.Empty : context.Request["CustID"];

        String custid = PhoneBO.IsAuthenPhone(PhoneNum, ConstHelper.DefaultInstance.BesttoneSPID, out ErrMsg);
        if (String.IsNullOrEmpty(custid))
        {
            //生成手机随机验证码
            String AuthenCode = RandNum(6);
     
            CommonBizRules.SendMessageV3(PhoneNum, " 您正在重置号码百事通会员密码，验证码为" + AuthenCode + "，有效期2分钟；如需帮助，请联系：4008-118114。", "35433333");
            PhoneBO.InsertPhoneSendMassage(CustID_old, "验证码信息内容", AuthenCode, PhoneNum, DateTime.Now, "描述未知", 1, 0, "1", out ErrMsg);

            ErrMsg = "";
            ReturnMsg = "[{result:\"true\",authencode:\"" + AuthenCode + "\",custid:\"" + CustID_old + "\"}]";
        }
        else if (CustID_old.Equals(custid))
        {
            ReturnMsg = "[{result:\"IsYours\",msg:\"该手机已是您的认证手机\"}]";
        }else{
            ReturnMsg = "[{result:\"false\",msg:\"该手机已经被认证\"}]";
        }

        return ReturnMsg;
    }

    protected String SendPhoneCode(HttpContext context,out String ErrMsg)
    {
        String ReturnMsg = String.Empty;
        ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        
        //获取手机号码
        String PhoneNum = context.Request["PhoneNum"] == null ? String.Empty : context.Request["PhoneNum"];

        string custid = PhoneBO.IsAuthenPhone(PhoneNum, ConstHelper.DefaultInstance.BesttoneSPID, out ErrMsg);
        if (String.IsNullOrEmpty(custid))
        {
            ReturnMsg = "[{result:\"false\",msg:\"认证手机不正确\"}]";
        }
        else
        {
            //生成手机随机验证码
            String AuthenCode = RandNum(6);
            //CommonBizRules.SendMessage(PhoneNum, "您的手机验证码是:" + AuthenCode, "35000000");
            //CommonBizRules.SendMessageV3(PhoneNum, "您的手机验证码是:" + AuthenCode, "35000000");
            CommonBizRules.SendMessageV3(PhoneNum, " 您正在重置号码百事通会员密码，验证码为" + AuthenCode + "，有效期2分钟；如需帮助请联系，请联系：4008-118114。", "35433333");

            PhoneBO.InsertPhoneSendMassage(custid, "验证码信息内容", AuthenCode, PhoneNum, DateTime.Now, "描述未知", 1, 0, "1", out ErrMsg);

            ErrMsg = "";
            ReturnMsg = "[{result:\"true\",authencode:\"" + AuthenCode + "\",custid:\"" + custid + "\"}]";
        }
        
        return ReturnMsg;

    }

    protected string RandNum(int n)
    {
        char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        StringBuilder num = new StringBuilder();
        Random rnd = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < n; i++)
        {
            num.Append(arrChar[rnd.Next(0, 9)].ToString());
        }
        return num.ToString();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
<%@ WebHandler Language="C#" Class="FindPwdByEmailHandler" %>

using System;
using System.Web;
using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class FindPwdByEmailHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        //----------------------------------------
        context.Response.Clear();
        context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        context.Response.ContentType = "text/plain";

        
        StringBuilder returnResult = new StringBuilder();
        try
        {
            String code = context.Request["Code"];
            //校验验证码
            if (!CommonUtility.ValidateValidateCode(code, context))
            {
                returnResult.Append("[{result:\"CodeError\",msg:\"验证码错误\"}]");
            }
            else
            {
                String type = context.Request["Type"] == null ? String.Empty : context.Request["Type"];
                switch (type)
                {
                    case "AuthenEmail":
                        returnResult.Append(AuthenEmail(context));
                        break;
                    default:
                        returnResult.Append(FindPwdByEmail(context));
                        break;
                }
            }
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

    /// <summary>
    /// 设置认证邮箱
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public String AuthenEmail(HttpContext context)
    {
        Int32 result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        //获取参数
        String email = context.Request["Email"];
        String returnUrl = context.Request["ReturnUrl"];
        String custid = context.Request["CustID"];

        String SPID = ConstHelper.DefaultInstance.BesttoneSPID;
        Int32 expiredHour = ConstHelper.DefaultInstance.ResetPwdExpiredHour;
        
        String custid_temp;
        result = SetMail.EmailSel(email, out custid_temp, out ErrMsg);
        if (result == 0)
        {
            if (custid == custid_temp)
                return "[{\"result\":\"IsYours\",\"msg\":\"邮箱已是您的认证邮箱\"}]";
            else
                return "[{\"result\":\"IsExist\",\"msg\":\"邮箱已被注册为认证邮箱\"}]";
        }
        else
        {
            Random random = new Random();
            String authenCode = random.Next(111111, 999999).ToString();
            
            //发送邮件
            String encryptUrl = CommonBizRules.EncryptEmailURl(SPID, custid, email, returnUrl, authenCode, context);
            //设置邮件中重置密码页面
            String ResetPwdEmailUrl = ConstHelper.DefaultInstance.BesttoneResetPwdByEmail + "?UrlParam=" + HttpUtility.UrlEncode(encryptUrl);
            String link = "点击重置密码:<a href='" + ResetPwdEmailUrl + "'>" + ResetPwdEmailUrl + "</a>";

            result = SetMail.InsertEmailByResetPwd(custid, "2", link, authenCode, 1, email, DateTime.Now, "描述", "中国电信号码百事通：找回密码", 1, expiredHour, out ErrMsg);
            if (result == 0)
            {
                return "[{result:\"true\",msg:\"邮件发生成功\"}]";
            }
            else
            {
                return "[{result:\"false\",msg:\"邮件发送失败，" + ErrMsg + "\"}]";
            }
        }
    }

    /// <summary>
    /// 通过邮箱找回密码
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public String FindPwdByEmail(HttpContext context)
    {
        Int32 result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        //获取参数
        String email = context.Request["Email"];
        String returnUrl = context.Request["ReturnUrl"];

        String SPID = ConstHelper.DefaultInstance.BesttoneSPID;
        Int32 expiredHour = ConstHelper.DefaultInstance.ResetPwdExpiredHour;
        String CustID = String.Empty;
        
        result = SetMail.EmailSel(email, out CustID, out ErrMsg);
        if (result != 0)
        {
            return "[{result:\"EmailError\",msg:\"认证邮箱有误\"}]";
        }
        else
        {
            Random random = new Random();
            String authenCode = random.Next(111111, 999999).ToString();

            //发送邮件
            String encryptUrl = CommonBizRules.EncryptEmailURl(SPID, CustID, email, returnUrl, authenCode, context);
            //设置邮件中重置密码页面
            String ResetPwdEmailUrl = ConstHelper.DefaultInstance.BesttoneResetPwdByEmail + "?UrlParam=" + HttpUtility.UrlEncode(encryptUrl);
            String link = "点击重置密码:<a href='" + ResetPwdEmailUrl + "'>" + ResetPwdEmailUrl + "</a>";

            result = SetMail.InsertEmailByResetPwd(CustID, "2", link, authenCode, 1, email, DateTime.Now, "描述", "中国电信号码百事通：找回密码", 1, expiredHour, out ErrMsg);
            if (result == 0)
            {
                return "[{result:\"true\",msg:\"邮件发生成功\"}]";
            }
            else
            {
                return "[{result:\"false\",msg:\"邮件发送失败，" + ErrMsg + "\"}]";
            }
        }
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}
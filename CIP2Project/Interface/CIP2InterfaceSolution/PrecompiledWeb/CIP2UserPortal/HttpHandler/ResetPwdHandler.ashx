<%@ WebHandler Language="C#" Class="ResetPwdHandler" %>

using System;
using System.Web;
using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class ResetPwdHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        context.Response.Clear();
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentType = "text/plain";

        String type = context.Request["Type"] == null ? String.Empty : context.Request["Type"];
        String code = context.Request["Code"] == null ? String.Empty : context.Request["Code"];            //验证码
        
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        StringBuilder strMsg = new StringBuilder();

        try
        {
            //校验验证码
            if (!CommonUtility.ValidateValidateCode(code, context))
            {
                strMsg.Append("[{result:\"CodeError\",msg:\"验证码错误\"}]");
            }
            else
            {
                switch (type)
                {
                    case "ResetPwdByEmail":
                        strMsg.Append(ResetPwdByEmail(context));
                        break;
                    case "ResetPwdByPhone":
                        strMsg.Append(ResetPwdByPhone(context));
                        break;
                    case "ResetPayPwd":
                        break;
                    default:
                        strMsg.Append(ResetPwd(context));
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ErrMsg += ex.Message;
            strMsg.Append("[{result:\"false\",msg:\"密码重置出错\"}]");
        }

        context.Response.Write(strMsg.ToString());
        context.Response.Flush();
        context.Response.End();
    }

    protected String ResetPwd(HttpContext context)
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        String SPID = ConstHelper.DefaultInstance.BesttoneSPID;
        String CustID = context.Request["CustID"];
        String PwdOld = context.Request["PasswordOld"];
        String PwdNew = context.Request["PasswordNew"];
        //String AuthenCode = context.Request["AuthenCode"];

        //旧密码验证
        if (!PassWordBO.OldPwdIsRight(CustID, PwdOld, "2", out ErrMsg))
        {
            return "[{result:\"OldPwdError\",msg:\"旧密码输入有误\"}]";
        }
        else
        {
            Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            //新密码设置
            Result = PassWordBO.SetPassword(SPID, CustID, PwdNew, "2", "", out ErrMsg);
            if (Result == 0)
            {
                return "[{result:\"true\",msg:\"新密码设置成功\"}]";
            }
            else
            {
                return "[{result:\"false\",msg:\"新密码设置有误\"}]";
            }
        }
    }

    /// <summary>
    /// 支付密码的修改
    /// </summary>
    protected String ResetPayPwd(HttpContext context)
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        String SPID = ConstHelper.DefaultInstance.BesttoneSPID;
        String CustID = context.Request["CustID"];
        String PwdOld = context.Request["PasswordOld"];
        String PwdNew = context.Request["PasswordNew"];         //1为语音密码，2为web密码
        String AuthenCode = context.Request["AuthenCode"];

        //查询账户信息
        BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
        BesttoneAccount account_entity = _besttoneAccount_dao.QueryByCustID(CustID);

        Result = BesttoneAccountHelper.ModifyPayPassword(account_entity.BestPayAccount, PwdOld, PwdNew, PwdNew, out ErrMsg);
        if (Result == 0)
        {
            return "[{result:\"true\",msg:\"新密码设置成功\"}]";
        }
        else
        {
            return "[{result:\"false\",msg:\"新密码设置有误\"}]";
            
            //return "[{result:\"OldPwdError\",msg:\"旧密码输入有误\"}]";
        }
    }

    protected String ResetPwdByEmail(HttpContext context)
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        
        String SPID = ConstHelper.DefaultInstance.BesttoneSPID;
        String CustID = context.Request["CustID"];
        String Email = context.Request["Email"];
        String AuthenCode = context.Request["AuthenCode"];
        String pwd = context.Request["PassWord"];   //1为语音密码，2为web密码

        Result = PassWordBO.SetPassword(SPID, CustID, pwd, "2", "", out ErrMsg);

        if (Result == 0)
        {
            SetMail.InsertEmailSendHistory2(CustID, Email, AuthenCode, out ErrMsg);
            return "[{result:\"true\",msg:\"新密码设置成功\"}]";
        }
        else
        {
            return "[{result:\"false\",msg:\"新密码设置有误\"}]";
        }
    }

    protected String ResetPwdByPhone(HttpContext context)
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        String SPID = ConstHelper.DefaultInstance.BesttoneSPID;
        String CustID = context.Request["CustID"];
        String pwd = context.Request["PassWord"];
        //1为语音密码，2为web密码
        String pwdType = context.Request["PwdType"] == null ? "2" : context.Request["PwdType"];

        Result = PassWordBO.SetPassword(SPID, CustID, pwd, pwdType, "", out ErrMsg);
        if (Result == 0)
            return "[{result:\"true\",msg:\"新密码设置成功\"}]";
        else
            return "[{result:\"false\",msg:\"新密码设置有误\"}]";
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
<%@ WebHandler Language="C#" Class="SaveRegisterInfoHandler" %>

using System;
using System.Web;

using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public class SaveRegisterInfoHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        context.Response.ContentType = "text/plain";
        context.Response.ContentEncoding = Encoding.UTF8;

        Int32 Result;
        String ErrMsg;
        StringBuilder returnMsg = new StringBuilder();
        String SPID = ConstHelper.DefaultInstance.BesttoneSPID;
        String CustID;
        try
        {
            //获取参数
            String userName = context.Request["UserName"] == null ? String.Empty : context.Request["UserName"];
            String password = context.Request["PassWord"] == null ? String.Empty : context.Request["PassWord"];
            String provinceID = context.Request["ProvinceID"] == null ? String.Empty : context.Request["ProvinceID"];
            String areaID = context.Request["AreaID"] == null ? String.Empty : context.Request["AreaID"];
            String email = context.Request["Email"] == null ? String.Empty : context.Request["Email"];
            String emailState = context.Request["EmailState"] == null ? String.Empty : context.Request["EmailState"];
            String phoneNum = context.Request["PhoneNum"] == null ? String.Empty : context.Request["PhoneNum"];
            String phoneState = context.Request["PhoneState"] == null ? String.Empty : context.Request["PhoneState"];
            String phoneCode = context.Request["PhoneCode"] == null ? String.Empty : context.Request["PhoneCode"];
            String pageCode = context.Request["PageCode"] == null ? String.Empty : context.Request["PageCode"];

            //之所以这样转，是因为存储过程中只判断空和0这两种值
            if (emailState != "0")
                emailState = "";
            if (phoneState != "0")
                phoneState = "";

            //认证邮箱验证
            if (emailState == "0")
            {
                Result = SetMail.EmailSel("", email, "", out ErrMsg);
                if (Result != 0)
                {
                    returnMsg.Append("[{result:\"false\",info:\"" + ErrMsg + "\"}]");
                    goto OutPut;
                }
            }

            //手机验证码验证
            if (phoneState == "0")
            {
                Result = PhoneBO.SelSendSMSMassage("", phoneNum, phoneCode, out ErrMsg);
                if (Result != 0)
                {
                    returnMsg.Append("[{result:\"false\",info:\"验证码验证失败," + ErrMsg + "\"}]");
                    goto OutPut;
                }
            }

            //验证页面验证码
            if (!CommonUtility.ValidateValidateCode(pageCode, context))
            {
                returnMsg.Append("[{result:\"false\",info:\"验证码有误\"}]");
            }
            else
            {
                Result = UserRegistry.getUserRegistryWeb(SPID, userName, "", password, phoneNum, phoneState, email, emailState,
                                        "", "", "", "", "", "", "", provinceID, areaID, out CustID, out ErrMsg);

                if (Result != 0)
                {
                    returnMsg.Append("[{result:\"false\",info:\"用户注册失败:" + ErrMsg + "\"}]");
                }
                else
                {
                    if (emailState == "0")
                    {
                        //给客户认证邮箱发邮件
                        String encryptUrl = CommonBizRules.EncryptEmailURl(CustID, email, context);
                        String url = "点击完成认证:<a href='" + encryptUrl + "'>" + encryptUrl + "</a>";
                        SetMail.InsertEmailSendMassage(CustID, "1", url, "", 1, email, DateTime.Now, "", "中国电信号码百事通：激活邮箱", 0, out ErrMsg);
                    }

                    returnMsg.Append("[{result:\"true\",info:\"恭喜您,注册成功!\"}]");
                }
            }
            
        }
        catch (Exception ex)
        {
            returnMsg = new StringBuilder();
            returnMsg.Append("[{result:\"false\",info:\"异常:" + ex.Message + "\"}]");
        }

    OutPut:
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
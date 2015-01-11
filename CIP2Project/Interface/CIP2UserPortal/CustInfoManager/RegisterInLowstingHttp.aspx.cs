using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using log4net;

public partial class CustInfoManager_RegisterInLowstingHttp : System.Web.UI.Page
{
    private Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
    private String SPID = "";
    private String CustID = "";
    private String UserName;
    private String PassWord;
    private String PassWord2;
    public String wt;
    public String Device = "android";
    public String ShareCode = "";
    public String AuthenCode = "";
    public String RegisterInLowstingHttp(String SPID,String UserName,String PassWord,String PassWord2,String Device,String wt)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        #region 数据校验
        if (CommonUtility.IsEmpty(SPID))
        {
           // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(UserName))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "UserName不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "UserName不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(AuthenCode))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AuthenCode不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AuthenCode不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        String t_CustID = String.Empty;
        String msg = String.Empty;
        int k = PhoneBO.SelSendSMSMassage(t_CustID, UserName, AuthenCode, out msg); 
        if(k!=0)
        {
          // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "验证码不正确！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "验证码不正确！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        String regMobile = @"^1[345678]\d{9}$";
        String regEmail = @"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$";
        String RegularUserName = @"^[a-zA-Z][a-zA-Z0-9]*([-._]?[a-zA-Z0-9]+)*$";

        if (!ValidateUserName(UserName, regMobile))
        //if (!ValidateUserName(UserName, RegularUserName))
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "991");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "UserName只能是手机号码！");
                //ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "UserName不合乎规范！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "991");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "UserName只能是手机号码！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(PassWord))
        {
   
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "PassWord不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "PassWord不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(PassWord2))
        {
    
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "PassWord2不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "PassWord2不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (!PassWord.Equals(PassWord2))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1001");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码不一致!");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1001");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码不一致！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        //Result = CustBasicInfo.IsExistUser(UserName);

        //if (Result != 0)
        //{
        //    // 返回错误信息
        //    ResponseMsg.Length = 0;
        //    if ("json".Equals(wt))
        //    {
        //        ResponseMsg.Append("{");
        //        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1000");
        //        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "用户名已经存在！");
        //        ResponseMsg.Append("}");
        //    }
        //    else
        //    {
        //        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //        ResponseMsg.Append("<PayPlatRequestParameter>");
        //        ResponseMsg.Append("<PARAMETERS>");
        //        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1000");
        //        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "用户名已经存在！");
        //        ResponseMsg.Append("</PARAMETERS>");
        //        ResponseMsg.Append("</PayPlatRequestParameter>");
        //    }
        //    return ResponseMsg.ToString();
        //}



        //验证码校验

        //if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(checkCode), this.Context))
        //{
            //hintError提示错误验证码校验未通过
            //errorHint.InnerHtml = "验证码校验未通过!";
            //return;
        //}


        #endregion



        String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
        String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
        String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
        String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
        String clientIp = HttpContext.Current.Request.UserHostAddress;
        String clientAgent = HttpContext.Current.Request.UserAgent;
        String sendSms = UDBConstDefinition.DefaultInstance.UnifyPlatformRegisterAccountSendSms;
        if (String.IsNullOrEmpty(sendSms))
        {
            sendSms = "false";
        }

        //综合平台隐式注册只支持手机，也就是说，用户名模式，放弃注册为天翼账号，仅注册为号百用户
        //既有用户名又有手机的，放弃注册天翼账号
        String Unify_ErrMsg = String.Empty;
        String userId = String.Empty;
        String o_userName = String.Empty;
        String accessToken = String.Empty;
        //msg.AppendFormat("注册天翼账号:\r\n");
        int Unify_Result = CIP2BizRules.RegisterUnifyPlatformAccount(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, PassWord, sendSms, out userId, out o_userName, out accessToken, out Unify_ErrMsg);
        //msg.AppendFormat("注册天翼账号,Result:{0},accessToken:{1},userId:{2},usrName:{3},ErrMsg:{4}\r\n", Unify_Result, accessToken, userId, o_userName, Unify_ErrMsg);

        if (Unify_Result == 0 && !String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(accessToken))
        {
            Result = UserRegistry.UserRegisterWebLowStintV2(SPID, UserName, PassWord, Device, out CustID, out ErrMsg);
            if (Result == 0)
            {

                String IPAddress = Request.UserHostAddress.ToString();
                CommonBizRules.WriteTraceIpLog(CustID, UserName, SPID, IPAddress, "client_zc");

                String youhuiquan_url = "http://www.114yg.cn/facadeHome.do?actions=facadeHome&method=sendCouponToRegist&wt=json&from=" + Device + "&custId=" + CustID;
                String jsonmsg = HttpMethods.HttpGet(youhuiquan_url);
                System.Collections.Generic.Dictionary<string, string> resuzt = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(jsonmsg);
                //{"returnCode":"00000"}
                string youhuiquan = "";
                resuzt.TryGetValue("returnCode", out youhuiquan);

                //建立绑定关系  （待完成）
                //因暂时不支持lognum

                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                    ResponseMsg.AppendFormat("\"returnCode\":\"{0}\",", youhuiquan);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "注册成功！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<CustID>{0}</CustID>", CustID);
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "注册成功！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
            else
            { 
                // 账号注册失败
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-1");
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", "");
                    ResponseMsg.AppendFormat("\"returnCode\":\"{0}\",", "");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "注册失败！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<CustID>{0}</CustID>", "");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-1");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "注册失败！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();

            }
        }
        else
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-11");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "天翼账号注册失败！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-11");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "天翼账号注册失败！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        return ResponseMsg.ToString();
    }

    public static bool ValidateUserName(string UserName, string Regular)
    {
        if (null == UserName)
        {
            return false;
        }
        if (UserName.IndexOf(" ")>=0)
        {
            return false;
        }

        if (UserName.Length < 5 || UserName.Length > 25)
        {
            return false;
        }

        return Regex.IsMatch(UserName, Regular);
    }


    public String RegisterInLowstingHttp(String SPID, String UserName, String PassWord, String PassWord2, String Device, String ShareCode,String wt)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        #region 数据校验
        if (CommonUtility.IsEmpty(SPID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(UserName))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "UserName不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "UserName不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(AuthenCode))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AuthenCode不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AuthenCode不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        String t_CustID = String.Empty;
        String msg = String.Empty;
        int k = PhoneBO.SelSendSMSMassage(t_CustID, UserName, AuthenCode, out msg);
        if (k != 0)
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "验证码不正确！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "验证码不正确！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        String regMobile = @"^1[345678]\d{9}$";
        //String regEmail = @"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$";
        //String RegularUserName = @"^[a-zA-Z][a-zA-Z0-9]*([-._]?[a-zA-Z0-9]+)*$";
        if (!ValidateUserName(UserName, regMobile))
        //if (!ValidateUserName(UserName, RegularUserName))
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "991");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "UserName只能是手机号码！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "991");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "UserName只能是手机号码！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(PassWord))
        {

            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "PassWord不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "PassWord不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(PassWord2))
        {

            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "PassWord2不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "PassWord2不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (!PassWord.Equals(PassWord2))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1001");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码不一致!");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1001");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码不一致！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

         #endregion

        String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
        String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
        String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
        String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
        String clientIp = HttpContext.Current.Request.UserHostAddress;
        String clientAgent = HttpContext.Current.Request.UserAgent;
        String sendSms = UDBConstDefinition.DefaultInstance.UnifyPlatformRegisterAccountSendSms;
        if (String.IsNullOrEmpty(sendSms))
        {
            sendSms = "false";
        }

        //综合平台隐式注册只支持手机，也就是说，用户名模式，放弃注册为天翼账号，仅注册为号百用户
        //既有用户名又有手机的，放弃注册天翼账号
        String Unify_ErrMsg = String.Empty;
        String userId = String.Empty;
        String o_userName = String.Empty;
        String accessToken = String.Empty;
        //msg.AppendFormat("注册天翼账号:\r\n");
        int Unify_Result = CIP2BizRules.RegisterUnifyPlatformAccount(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, PassWord, sendSms, out userId, out o_userName, out accessToken, out Unify_ErrMsg);
        //msg.AppendFormat("注册天翼账号,Result:{0},accessToken:{1},userId:{2},usrName:{3},ErrMsg:{4}\r\n", Unify_Result, accessToken, userId, o_userName, Unify_ErrMsg);


        if (Unify_Result == 0 && !String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(accessToken))
        {
            //Result = UserRegistry.UserRegisterWebLowStintV3(SPID, UserName, PassWord, Device, ShareCode, out CustID, out ErrMsg);
            UDBMBOSS _UDBMBoss = new UDBMBOSS();
            UnifyAccountInfo accountInfo = new UnifyAccountInfo();
            Unify_Result = _UDBMBoss.UnifyPlatformGetUserInfo(appId, appSecret, version, clientType, accessToken, clientIp, clientAgent, out accountInfo, out Unify_ErrMsg);
            String OuterID, Status, CustType, CustLevel, NickName, Email, CertificateCode, CertificateType, Sex, RealName, EnterpriseID, ProvinceID, AreaID, RegistrationSource;

            if (Unify_Result == 0 && !String.IsNullOrEmpty(Convert.ToString(accountInfo.userId)))
            {

                String MobileName = String.Empty;
                String EmailName = String.Empty;
                if (!String.IsNullOrEmpty(accountInfo.nickName))
                {
                    RealName = accountInfo.nickName;
                }
                else if (!String.IsNullOrEmpty(accountInfo.userName))
                {
                    RealName = accountInfo.userName;
                }
                else if (!String.IsNullOrEmpty(accountInfo.mobileName))
                {
                    RealName = accountInfo.mobileName;
                }
                else if (!String.IsNullOrEmpty(accountInfo.emailName))
                {
                    RealName = accountInfo.emailName;
                }
                else
                {
                    RealName = "";
                }
                if (!String.IsNullOrEmpty(accountInfo.mobileName))
                {
                    MobileName = accountInfo.mobileName;
                }
                if (!String.IsNullOrEmpty(accountInfo.emailName))
                {
                    EmailName = accountInfo.emailName;
                }
                String EncrytpPassWord = CryptographyUtil.Encrypt(PassWord);
                String OperType = "2";  // 注册 ,

                if (!String.IsNullOrEmpty(MobileName) || !String.IsNullOrEmpty(EmailName))
                {

                    CustID = String.Empty;

                    Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", MobileName, EmailName, RealName, EncrytpPassWord, accountInfo.userId, SPID, OperType, out CustID, out ErrMsg);
              
                    if (Result == 0 && !String.IsNullOrEmpty(CustID))
                    {
                        Result = CustBasicInfo.getCustInfo(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                            out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                            out AreaID, out RegistrationSource);
                        CommonBizRules.WriteTraceIpLog(CustID, UserName, SPID, Request.UserHostAddress.ToString(), "client_zc");

                        String youhuiquan_url = "http://www.114yg.cn/facadeHome.do?actions=facadeHome&method=sendCouponToShare&wt=json&from=" + Device + "&registerCustId=" + CustID;
                        String jsonmsg = HttpMethods.HttpGet(youhuiquan_url);
                        System.Collections.Generic.Dictionary<string, string> resuzt = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(jsonmsg);
                        //{"returnCode":"00000"}
                        string youhuiquan = "";
                        resuzt.TryGetValue("returnCode", out youhuiquan);
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                            ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                            ResponseMsg.AppendFormat("\"returnCode\":\"{0}\",", youhuiquan);
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "注册成功！");
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<CustID>{0}</CustID>", CustID);
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                            ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "注册成功！");
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                        return ResponseMsg.ToString();
                    }
                }
                else
                {
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-11");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "MobileName或EmailName为空不能注册天翼账号所以号百注册也失败！");
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-11");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "MobileName或EmailName为空不能注册天翼账号所以号百注册也失败！！");
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                }
                //http://www.114yg.cn/facadeHome.do?actions=facadeHome&method=sendCouponToShare&wt=json&from=ios&registerCustId=134664179
                //"http://116.228.55.13:8113/facadeHome.do?actions=facadeHome&method=sendCouponToShare&wt=json&from=ios&registerCustId=
            }
            else
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-10");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "号百账号注册失败！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-10");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "号百账号注册失败！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
        }
        else
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-10");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "天翼账号注册失败！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-10");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "天翼账号注册失败！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        return ResponseMsg.ToString();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        UserName = Request["UserName"];
        PassWord = Request["PassWord"];
        PassWord2 = Request["PassWord2"];
        AuthenCode = Request["AuthenCode"];
        Device = Request["Device"];
        wt = Request["wt"];
        ShareCode = Request["ShareCode"];
        if (String.IsNullOrEmpty(Device))
        {
            Device = "android";
        }

        if (!Device.Equals("android") && !Device.Equals("ios") && !Device.Equals("wap"))
        {
            Device = "android";
        }

        String ResponseText = "";
        if (String.IsNullOrEmpty(ShareCode))   //不带分享码注册
        {
            ResponseText = RegisterInLowstingHttp(SPID, UserName, PassWord, PassWord2, Device, wt);  //QueryCustBasicInfo(SPID, CustID);

        }
        else  //带分享码注册
        {
            ResponseText = RegisterInLowstingHttp(SPID, UserName, PassWord, PassWord2, Device,ShareCode, wt);  //QueryCustBasicInfo(SPID, CustID);

        }
       
        
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Text;

public partial class UnifyAccountCheck : System.Web.UI.Page
{
    public String ReturnUrl = "CustInfo.aspx";
    private String SPID = String.Empty;
    private UDBMBOSS _UDBMBoss = new UDBMBOSS();
    private Dictionary<String, String> splitParameters(string paraStr)
    {
        StringBuilder sbLog = new StringBuilder();
        sbLog.AppendFormat("splitParameters:{0}\r\n", paraStr);
        Dictionary<String, String> parameters = new Dictionary<string, string>();

        if (!String.Empty.Equals(paraStr))
        {
            string[] array = paraStr.Trim().Split('&');

            foreach (string temp in array)
            {
                if (!String.Empty.Equals(temp))
                {
                    string ttemp = temp.Trim();
                    int index = ttemp.IndexOf("=");
                    if (index > 0)
                    {
                        String key = ttemp.Substring(0, index);
                        String value = ttemp.Substring(index + 1);
                        if (String.Empty.Equals(key) || String.Empty.Equals(value)) continue;
                        if (!parameters.ContainsKey(key))
                        {
                            parameters.Add(key.Trim(), value.Trim());
                        }
                        
                    }
                }
            }
        }

        return parameters;
    }
    /// <summary>
    /// 页面跳转
    /// </summary>
    protected void Redirect(String argsName, String argsValue)
    {
        if (ReturnUrl.IndexOf('?') > 0)
        {
            ReturnUrl = ReturnUrl + "&" + argsName + "=" + argsValue;
        }
        else
        {
            ReturnUrl = ReturnUrl + "?" + argsName + "=" + argsValue;
        }
        StringBuilder sbLog = new StringBuilder();
        sbLog.AppendFormat("Redirect to URL:{0}\r\n", ReturnUrl);
        WriteLog(sbLog.ToString());
        Response.Redirect(ReturnUrl);
    }
    /// <summary>
    /// 写日志功能
    /// </summary>
    protected void WriteLog(String str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyAccountCheck", msg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        StringBuilder strMsg = new StringBuilder();

        SPID = Request["SPID"];
        string appId = Request["appId"];
        string paras = Request["paras"];
        string sign = Request["sign"];

        string unifyPlatform_appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
        string unifyPlatform_appSecretKey = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];

        strMsg.AppendFormat("返回参数:appId:{0},paras:{1},sign:{2},SPID:{3}\r\n",appId,paras,sign,SPID);

        string unifyPlatformResponse = CryptographyUtil.XXTeaDecrypt(paras, unifyPlatform_appSecretKey);
        strMsg.AppendFormat("unifyPlatformResponse:{0}\r\n", unifyPlatformResponse);
        string newsign = CryptographyUtil.HMAC_SHA1(unifyPlatform_appId + paras, unifyPlatform_appSecretKey);
        strMsg.AppendFormat("newsign:{0},sign:{1}\r\n", newsign, sign);
   
        if (!newsign.Equals(sign))
        {
            Redirect("ErrMsg", "签名不正确");
        }

        string result = "";
        string accessToken = "";
        string timeStamp = "";
        string userId = "";
        string productUid = "";
        string loginNum = "";
        string nickName = "";
        string userIconUrl = "";
        string userIconUrl2 = "";
        string userIconUrl3 = "";
        string isThirdAccount = "";

        Dictionary<String, String> parames = new Dictionary<string, string>();
        strMsg.Append("开始解析unifyPlatformResponse\r\n");
        try
        {
            parames = splitParameters(unifyPlatformResponse);
            strMsg.AppendFormat("params:{0}\r\n",parames);
        }
        catch (Exception exp)
        {
            strMsg.AppendFormat(exp.ToString());
        }
        strMsg.Append("解析unifyPlatformResponse完毕\r\n");

        foreach (KeyValuePair<String, String> p in parames)
        {
            if (p.Key.Equals("result"))
            {
                result = p.Value;
                strMsg.AppendFormat("result:{0}\r\n",result);
            }
            if (p.Key.Equals("accessToken"))
            {
                accessToken = p.Value;
                strMsg.AppendFormat("accessToken:{0}\r\n", accessToken);
            }
            if (p.Key.Equals("timeStamp"))
            {
                timeStamp = p.Value;
                strMsg.AppendFormat("timeStamp:{0}\r\n", timeStamp);
            }
            if (p.Key.Equals("userId"))
            {
                userId = p.Value;
                strMsg.AppendFormat("userId:{0}\r\n", userId);
            }
            if (p.Key.Equals("productUid"))
            {
                productUid = p.Value;
                strMsg.AppendFormat("productUid:{0}\r\n", productUid);
            }
            if (p.Key.Equals("loginNum"))
            {
                loginNum = p.Value;
                strMsg.AppendFormat("loginNum:{0}\r\n", loginNum);
            }
            if (p.Key.Equals("nickName"))
            {
                nickName = p.Value;
                strMsg.AppendFormat("nickName:{0}\r\n", nickName);
            }
            if (p.Key.Equals("userIconUrl"))
            {
                userIconUrl = p.Value;
                strMsg.AppendFormat("userIconUrl:{0}\r\n", userIconUrl);
            }
            if (p.Key.Equals("userIconUrl2"))
            {
                userIconUrl2 = p.Value;
                strMsg.AppendFormat("userIconUrl2:{0}\r\n", userIconUrl2);
            }
            if (p.Key.Equals("userIconUrl3"))
            {
                userIconUrl3 = p.Value;
                strMsg.AppendFormat("userIconUrl3:{0}\r\n", userIconUrl3);
            }
            if (p.Key.Equals("isThirdAccount"))
            {
                isThirdAccount = p.Value;
                strMsg.AppendFormat("isThirdAccount:{0}\r\n", isThirdAccount);
            }

        }

        strMsg.Append("【综合平台 getUserInfo.do】:");

        UnifyAccountInfo accountInfo = new UnifyAccountInfo();
        String clientIp = System.Configuration.ConfigurationManager.AppSettings["CIP2_clientIp"];//? 通过f5出去的，这样获得地址不对
        if (String.IsNullOrEmpty(clientIp))
        {
            clientIp = Request.UserHostAddress;
        }

        try
        {
            String clientAgent = Request.UserAgent;
            if ("0".Equals(result) && !String.IsNullOrEmpty(accessToken))   // result = 0 说明已经处于登录状态 result = 1 说明处于未登录状态
            {
                string p_version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
                string p_clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
                Result = _UDBMBoss.UnifyPlatformGetUserInfo(unifyPlatform_appId, unifyPlatform_appSecretKey, p_version, p_clientType, accessToken, clientIp, clientAgent, out accountInfo, out ErrMsg);

                if (Result == 0)  // 综合平台查询客户信息成功
                {

                    String CustID, OuterID, Status, CustType, CustLevel, RealName, UserName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                    //检测对应用户是否在号百系统，不在，则注册进来
                    strMsg.Append("【开始注册到号百】:");
                    CustID = String.Empty;
                    System.Text.RegularExpressions.Regex regMobile = new System.Text.RegularExpressions.Regex(@"^1[345678]\d{9}$");
                    System.Text.RegularExpressions.Regex regEmail = new System.Text.RegularExpressions.Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
                    System.Text.RegularExpressions.Regex regCard = new System.Text.RegularExpressions.Regex(@"^(\d{9}|\d{16})$");

                    string AuthenType = "1";
                    strMsg.AppendFormat("accountInfo.username:{0}\r\n", accountInfo.userName);
                    strMsg.AppendFormat("acountInfo.userId:{0},accountInfo.pUserId:{1}\r\n", accountInfo.userId, accountInfo.pUserId);
                    if (regMobile.IsMatch(accountInfo.userName))
                    {
                        AuthenType = "2";
                    }
                    if (regEmail.IsMatch(accountInfo.userName))
                    {
                        AuthenType = "4";
                    }
                    if (regCard.IsMatch(accountInfo.userName))
                    {
                        AuthenType = "3";
                    }

                    if ("2".Equals(AuthenType))
                    {
                        //Result = UserRegistry.getUserRegistryUnifyPlatform(accountInfo, out CustID, out ErrMsg);
                        String OperType = "3";  // 注册
                        String Password = "";  // 从综合平台注册过来，密码是不知道的
                        RealName = "";
                        Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", accountInfo.mobileName,
                            accountInfo.emailName, RealName, Password, accountInfo.userId, SPID, OperType, out CustID, out ErrMsg);

                    }
                    else
                    {
                        Result = -7766;
                    }
                    //Result = UserRegistry.getUserRegistryUnifyPlatform(accountInfo, out CustID, out ErrMsg);



                    strMsg.AppendFormat("Result:{0},CustID:{1}\r\n", Result, CustID);
                    strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                    //注册成功
                    if (Result == 0)
                    {
                        Result = CustBasicInfo.getCustInfo(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                            out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                            out AreaID, out RegistrationSource);
                        strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                        if (Result != 0)
                        {
                            strMsg.Append(",ErrMsg:客户不存在" + CustID);
                            //客户不存在
                            Redirect("ErrMsg", "客户不存在");
                        }

                        strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl + "?UnifyAccountCheckResult=0&SPID=35000000");

                        //埋号百token
                        string AuthenName = UserName;
                        AuthenType = "2";
                        SPInfoManager spInfo = new SPInfoManager();
                        Object SPData = spInfo.GetSPData(this.Context, "SPData");
                        string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
                        UserToken UT = new UserToken();
                        if (accountInfo.userId != null && accountInfo.userId != 0)
                        {
                            OuterID = "123456";
                        }
                        string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OuterID, CustType, AuthenName, AuthenType, key, out ErrMsg);
                        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                        PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
                        //埋综合平台token   6.1 add
                        //String UnifyPlatformCookieName = ConfigurationManager.AppSettings["UnifyPlatformCookieName"];
                        //PageUtility.SetCookie(UserTokenValue, UnifyPlatformCookieName, this.Page);
                        //埋综合平台token   6.1 end
                        //ReturnUrl = Request["ReturnUrl"] ;
                        strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl + "?UnifyAccountCheckResult=0");
                        strMsg.AppendFormat("Response.Redirect to {0}\r\n", ReturnUrl + "?UnifyAccountCheckResult=0&SPID=35000000");
                        Response.Redirect(ReturnUrl + "?UnifyAccountCheckResult=0&SPID=35000000", false);
                    }
                    else
                    {
                        strMsg.Append(",ErrMsg:用户注册到号百失败");
                        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                        PageUtility.ExpireCookie(CookieName, this.Page);
                        //清综合平台token   6.1 add
                        //String UnifyPlatformCookieName = ConfigurationManager.AppSettings["UnifyPlatformCookieName"];
                        //PageUtility.ExpireCookie(UnifyPlatformCookieName, this.Page);
                        //清综合平台token   6.1 end
                        Response.Redirect(ReturnUrl + "?UnifyAccountCheckResult=0&SPID=35000000", false);
                    }
                }

            }
            else  // 未登录
            { 
                //清楚cookie (登录状态)
                string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                PageUtility.ExpireCookie(CookieName, this.Page);
                //清综合平台token   6.1 add
                //String UnifyPlatformCookieName = ConfigurationManager.AppSettings["UnifyPlatformCookieName"];
                //PageUtility.ExpireCookie(UnifyPlatformCookieName, this.Page);
                //清综合平台token   6.1 end
                Response.Redirect(ReturnUrl + "?UnifyAccountCheckResult=1&SPID=35000000", false);
            
            }

        }
        catch (Exception excp)
        {
            strMsg.AppendFormat("异常:{0}\r\n",excp.ToString());
        }
        finally
        {
            WriteLog(strMsg.ToString());
        }
        //WriteLog(strMsg.ToString());

    }
}

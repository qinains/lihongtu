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
using System.Text.RegularExpressions;

public partial class UnifyPlatformWebRegisterCallback : System.Web.UI.Page
{
    private UDBMBOSS _UDBMBoss = new UDBMBOSS();
    public String ticket = String.Empty;
    private String SPID = String.Empty;
    private String LSID = String.Empty;
    private String appId = String.Empty;
    private String paras = String.Empty;
    private String sign = String.Empty;
    public String ReturnUrl = String.Empty;
    public String AccessToken = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if ( CommonUtility.IsParameterExist("accessToken",this.Page) && CommonUtility.IsParameterExist("SPID", this.Page))
        {
            UnifyAccountCheck();
        }
        else
        {
            Redirect("ErrMsg", "SPID或accessToken缺失");
        }
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyPlatformWebRegisterCallback", msg);
    }


    protected void UnifyAccountCheck()
    {
        StringBuilder strLog = new StringBuilder();
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        try
        {
            SPID = Request["SPID"];
            ReturnUrl = Request["ReturnUrl"] == null ? String.Empty : Request["ReturnUrl"];
            AccessToken = Request["accessToken"];
            appId = Request["appId"];
            paras = Request["paras"];
            sign = Request["sign"];
            strLog.AppendFormat("SPID:{0},ReturnUrl:{1},appId:{2},paras:{3},sign:{4},AccessToken:{5}\r\n",SPID,ReturnUrl,appId,paras,sign,AccessToken);
            //查综合平台客户信息
            strLog.Append("查询综合平台客户信息\r\n");
            if (!String.IsNullOrEmpty(AccessToken))
            {
                UnifyAccountInfo accountInfo = new UnifyAccountInfo();
                String clientIp = System.Configuration.ConfigurationManager.AppSettings["CIP2_clientIp"];//? 通过f5出去的，这样获得地址不对
                if (String.IsNullOrEmpty(clientIp))
                {
                    clientIp = Request.UserHostAddress;
                }

                String clientAgent = Request.UserAgent;
                String unifyPlatform_appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
                String unifyPlatform_appSecretKey = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
                String p_version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
                String p_clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
                Result = _UDBMBoss.UnifyPlatformGetUserInfo(unifyPlatform_appId, unifyPlatform_appSecretKey, p_version, p_clientType, AccessToken, clientIp, clientAgent, out accountInfo, out ErrMsg);
                strLog.AppendFormat("查询综合平台返回:Result:{0},ErrMsg:{1},UserID:{2}\r\n",Result,ErrMsg,Convert.ToString(accountInfo.userId));
                if (Result == 0 && !String.IsNullOrEmpty(Convert.ToString(accountInfo.userId)))  //这个地方跟登录回来不一致，登录回来是根据loginnum去匹配
                {
                   
                    ///////////////
                    #region 开始注册到号百
                    String CustID, OuterID, Status, CustType, CustLevel, RealName, UserName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                    CustID = String.Empty;
                    Regex regMobile = new Regex(@"^1[345678]\d{9}$");
                    Regex regEmail = new Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
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

                    String EncrytpPassWord = CryptographyUtil.Encrypt("123456");   //通过页面注册进来的，不知道密码，给一个默认密码
                    //通过统一注册页面过来的，注册为号百的 “非认证用户”,通过语音注册进来的，注册为号百的 “认证用户”    
                    String OperType = "1";  // 注册 ,
                    if (!String.IsNullOrEmpty(MobileName) || !String.IsNullOrEmpty(EmailName) )
                    {
                        strLog.Append("【开始注册或者绑定到号百】:\r\n");
                        Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", MobileName, EmailName, RealName, EncrytpPassWord, accountInfo.userId, SPID, OperType, out CustID, out ErrMsg);
                        strLog.Append("【开始注册或者绑定到号百的结果】:\r\n");
                        strLog.AppendFormat("Result:{0},CustID:{1},ErrMsg:{2}\r\n", Result, CustID, ErrMsg);
                        //注册成功
                        if (Result == 0)
                        {
                            Result = CustBasicInfo.getCustInfo(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                                out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                                out AreaID, out RegistrationSource);
                            strLog.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                            if (Result != 0)
                            {
                                strLog.Append(",ErrMsg:客户不存在" + CustID);
                                Redirect("ErrMsg", "客户不存在");
                            }
                            strLog.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                            //生成Ticket
                            ticket = CommonBizRules.CreateTicket();
                            Result = CIPTicketManager.insertCIPTicket(ticket, SPID, CustID, RealName, UserName, NickName, OuterID, "UDBTicket", Convert.ToString(accountInfo.userId), "42", out ErrMsg);
                            //insertAccessToken
                            strLog.AppendFormat("【生成ticket】:Result:{0},Ticket:{1}", Result, ticket);
                            strLog.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                            if (Result != 0)
                            {
                                strLog.Append(",ErrMsg:Ticket生成失败" + ticket);
                                Redirect("ErrMsg", "Ticket生成失败");
                            }
                            strLog.Append(",Message:生成ticket成功，返回业务系统\r\n");
                            ReturnUrl = Request["ReturnUrl"];
                            strLog.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                            strLog.AppendFormat("Response.Redirect to {0}\r\n", ReturnUrl);
                            Response.Redirect(ReturnUrl,false);
                        }
                    }
                    else
                    {
                        Result = -7766;
                        ErrMsg = "MobileName,或者EmailName为空,所以不注册号百客户";
                        strLog.Append("MobileName,或者EmailName为空,所以不注册号百客户\r\n");
                        Redirect("ErrMsg", "MobileName,或者EmailName为空,所以不注册号百客户");
                    }
                    strLog.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                    //下面删掉一大段
                   #endregion
                    //////////////
                }
                else
                { //查询综合平台客户信息失败,或者account.userid为空
                    strLog.Append("查询综合平台客户信息失败,或者account.userid为空\r\n");
                    Redirect("ErrMsg", "查询综合平台客户信息失败,或者account.userid为空");
                }
            }
            else
            { //accesstoken没有返回
                strLog.Append("综合平台accesstoken没有返回\r\n");
                Redirect("ErrMsg", "综合平台accesstoken没有返回户");
            }
        }
        catch (Exception e)
        {
            strLog.AppendFormat("异常:{0}\r\n",e.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }
    }


    /// <summary>
    /// 开始UDBSSO功能
    /// </summary>
    protected void ProcessUnifyPlatformReturn()
    {
        StringBuilder strMsg = new StringBuilder();
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        try
        {
            #region 获取参数并验证

            SPID = Request["SPID"];
            ReturnUrl = Request["ReturnUrl"] == null ? String.Empty : Request["ReturnUrl"];
            LSID = Request["LSID"];
            appId = Request["appId"];
            paras = Request["paras"];
            sign = Request["sign"];

            string unifyPlatform_appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
            string unifyPlatform_appSecretKey = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];


            strMsg.AppendFormat("【验证参数，DateTime：{0}】:SPID:{1},LSID:{2},ReturnUrl:{3},appId:{4},paras:{5},sign:{6}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), SPID, LSID, ReturnUrl, appId, paras, sign);

            string unifyPlatformResponse = CryptographyUtil.XXTeaDecrypt(paras, unifyPlatform_appSecretKey);
            strMsg.AppendFormat("unifyPlatformResponse:{0}\r\n", unifyPlatformResponse);
            string newsign = CryptographyUtil.HMAC_SHA1(unifyPlatform_appId + paras, unifyPlatform_appSecretKey);
            strMsg.AppendFormat("newsign:{0},sign:{1}\r\n", newsign, sign);
            strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
            if (!newsign.Equals(sign))
            {
                Redirect("ErrMsg", "签名不正确");
            }

            //paras {result,accessToken,timeStamp,userId,productUid,loginNum,nickName,userIconUrl,userIconUrl2,userIconUrl3,isThirdAccount}
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
                strMsg.AppendFormat("params:{0}\r\n", parames);
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
                    strMsg.AppendFormat("result:{0}\r\n", result);
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
            strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);

            #endregion

            #region 根据UDBTkcket到UDB查询用户信息

            strMsg.Append("【开始查询信息】:");

            UnifyAccountInfo accountInfo = new UnifyAccountInfo();
            String clientIp = System.Configuration.ConfigurationManager.AppSettings["CIP2_clientIp"];//? 通过f5出去的，这样获得地址不对
            if (String.IsNullOrEmpty(clientIp))
            {
                clientIp = Request.UserHostAddress;
            }

            String clientAgent = Request.UserAgent;
            ////根据UDBTicket到UDB查询用户信息
            //Result = _UDBMBoss.AccountInfoQuery(UDBSPID, UDBSPID, UDBTicket, UDBKey, out accountInfo, out ErrMsg);
            if ("0".Equals(result) && !String.IsNullOrEmpty(accessToken))
            {

                string p_version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
                string p_clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
                Result = _UDBMBoss.UnifyPlatformGetUserInfo(unifyPlatform_appId, unifyPlatform_appSecretKey, p_version, p_clientType, accessToken, clientIp, clientAgent, out accountInfo, out ErrMsg);
            }

            strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);

            if ("0".Equals(result) && Result == 0)   // 认证成功 并且根据accesstoken查客户信息成功   
            {
                String CustID, OuterID, Status, CustType, CustLevel, RealName, UserName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                //检测对应用户是否在号百系统，不在，则注册进来
                strMsg.Append("【开始注册到号百】:");
                CustID = String.Empty;

                Regex regMobile = new Regex(@"^1[345678]\d{9}$");
                Regex regEmail = new Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
                Regex regCard = new Regex(@"^(\d{9}|\d{16})$");

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
                    String OperType = "1";  // 注册
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

                    ////登录tab写入cookie
                    //PageUtility.SetCookie("LoginTabCookie", "UDBTab", 8760);

                    strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                    //生成Ticket
                    ticket = CommonBizRules.CreateTicket();
                    Result = CIPTicketManager.insertCIPTicket(ticket, SPID, CustID, RealName, UserName, NickName, OuterID, "UDBTicket", Convert.ToString(accountInfo.userId), UDBBusiness.ConvertAuthenType(Convert.ToString(accountInfo.userType)), out ErrMsg);
                    strMsg.AppendFormat("【生成ticket】:Result:{0},Ticket:{1}", Result, ticket);
                    strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                    if (Result != 0)
                    {
                        strMsg.Append(",ErrMsg:Ticket生成失败" + ticket);
                        Redirect("ErrMsg", "Ticket生成失败");
                    }
                    strMsg.Append(",Message:生成ticket成功，返回业务系统\r\n");
                    ReturnUrl = Request["ReturnUrl"];
                    strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                    strMsg.AppendFormat("Response.Redirect to {0}\r\n", ReturnUrl);

                    //埋综合平台token 6.1 add
                    //String UnifyPlatformCookieName = ConfigurationManager.AppSettings["UnifyPlatformCookieName"];
                    //string AuthenName = UserName;
                    //AuthenType = "2";
                    //SPInfoManager spInfo = new SPInfoManager();
                    //Object SPData = spInfo.GetSPData(this.Context, "SPData");
                    //string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
                    //UserToken UT = new UserToken();
                    //string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OuterID, CustType, AuthenName, AuthenType, key, out ErrMsg);
                    //string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                    //PageUtility.SetCookie(UserTokenValue, UnifyPlatformCookieName, this.Page);
                    //埋综合平台token 6.1 end


                }
                else
                {
                    strMsg.Append(",ErrMsg:用户注册到号百失败");
                    Redirect("ErrMsg", "用户注册到号百失败" + ErrMsg);
                }
            }
            else
            {
                strMsg.Append(",ErrMsg:查询用户信息失败");
                Redirect("ErrMsg", "查询用户信息失败");
            }

            #endregion
        }
        catch (Exception ex)
        {
            strMsg.AppendFormat(",ErrMsg:{0}", ex.Message);
        }
        finally
        {
            WriteLog(strMsg.ToString());
        }
    }


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
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyPlatformWebRegisterCallBack", msg);
    }
}

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
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using BTUCenter.Proxy;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
public partial class SSO_BesttoneLoginForm : System.Web.UI.Page
{

    #region 单点登录全局变量
    string SPTokenRequest = "";
    public string SPID = "35433333";
    string ReturnURL = "http://www.118114.cn/";
    string UAProvinceID = "";
    string SourceType = "";

    string ErrMsg = "";
    int Result = 0;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 判断并解析SPTokenRequest参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            String reg_url = System.Configuration.ConfigurationManager.AppSettings["YgRegisterTargetURL"];

            if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
            {
                SPTokenRequest = Request["SPTokenRequest"];
                //日志
                strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
                //解析请求参数
                Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnURL, out ErrMsg);
                //日志
                strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnURL));

                if (Result != 0)
                {
                    errorHint.InnerHtml = "请求参数SPTokenRequest不正确";
                }

                if (!CommonUtility.ValidateUrl(ReturnURL.Trim()))
                {
                    errorHint.InnerHtml = "请求参数ReturnURL不正确";
                }


                string SignUpReturnUrl = HttpUtility.UrlEncode(SPTokenRequest);
                SignUpReturnUrl = "SPTokenRequest=" + SignUpReturnUrl;

                if (CommonUtility.IsParameterExist("NeedLogin", this.Page))
                {
                    SignUpReturnUrl = SignUpReturnUrl + "&NeedLogin=" + HttpUtility.UrlEncode(Request["NeedLogin"]);
                }
                String _url = "";
                string tmp_url = "";
                if ("35433333".Equals(SPID))
                {
                    SignUpReturnUrl = HttpUtility.UrlEncode(this.Context.Request.Url.ToString().Substring(0, this.Context.Request.Url.ToString().IndexOf("?") + 1) + SignUpReturnUrl);
                    int startIndex = ReturnURL.IndexOf("url=");
                    tmp_url = ReturnURL.Substring(startIndex + 4);  //  tmp_url = aHR0cDovL3d3dy5iZXN0dG9uZS5jbg==
                    strLog.AppendFormat(String.Format("url={0}", tmp_url));
                    _url = System.Text.UTF8Encoding.Default.GetString(FromBase64String(tmp_url));   //  _url = http://www.besttone.cn
                }

                strLog.AppendFormat(String.Format("url={0}", tmp_url));
                strLog.AppendFormat(String.Format("_url={0}", _url));
                if ("35433334".Equals(SPID))
                {
                    reg_url = System.Configuration.ConfigurationManager.AppSettings["YgRegisterTargetURL_YG"];
                }
                else
                {
                    reg_url = reg_url + "?returnUrl=" + _url;     // reg_url = http://sso.besttone.cn/SSO/registerV2.action?returnUrl=http://www.besttone.cn   http://sso.besttone.cn/SSO/registerV2.action  从配置文件中
                }
                strLog.AppendFormat("reg_url=" + reg_url);
                //this.linkU1.NavigateUrl = reg_url;

            }
            else
            {
                //this.linkU1.NavigateUrl = reg_url + "?SPID=35000000&ReturnUrl=http://www.118114.cn";
            }
            //this.linkU1.Target = "_top";
        }
        catch (System.Exception ex)
        {
            log(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneLoginForm", msg);
    }

    /// <summary>
    /// Base64反编码
    /// </summary>
    public static byte[] FromBase64String(string source)
    {
        return Convert.FromBase64String(source);
    }

    protected void ssoFunc()
    {
        string Url = "";
        try
        {
            string Ticket = CommonBizRules.CreateTicket();

            string CustID = TokenValidate.CustID;
            string RealName = TokenValidate.RealName;
            string NickName = TokenValidate.NickName;
            string UserName = TokenValidate.UserName;
            string OutID = TokenValidate.OuterID;
            OutID = "99999";
            string LoginAuthenName = TokenValidate.LoginAuthenName;
            string LoginAuthenType = TokenValidate.LoginAuthenType;
            log(String.Format("ssoFunc: TokenValidate.RealName:{0},TokenValidate.NickName:{1},TokenValidate.UserName:{2},TokenValidate.LoginAuthenName:{3},TokenValidate.LoginAuthenType:{4}",
                TokenValidate.RealName, TokenValidate.NickName, TokenValidate.UserName, TokenValidate.LoginAuthenName, TokenValidate.LoginAuthenType));
            String er = "";
            Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, UserName, NickName, OutID, "", LoginAuthenName, LoginAuthenType, out er);

            if (Result != 0)
            {
                errorHint.InnerHtml = er;
                return;
            }

            if (ReturnURL.IndexOf("?") > 0)
            {
                Url = ReturnURL + "&Ticket=" + Ticket;
            }
            else
            {
                Url = ReturnURL + "?Ticket=" + Ticket;
            }

            if (CommonUtility.IsParameterExist("NeedLogin", this.Page))
            {
                Url = Url + "&NeedLogin=" + Request["NeedLogin"];
            }
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            Response.Redirect(Url, false);
        }

        catch (Exception e)
        {
            errorHint.InnerHtml = e.Message + ">>ReturnURL:" + Url;
        }
    }
    protected void login_Click(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");
        StringBuilder strLog = new StringBuilder();
        //string AuthenType = HttpUtility.HtmlDecode(Request.Form["AuthenType"].ToString().Trim().ToUpper());         //获取认证类型

        string AuthenName = username.Text;
        string Password = password.Text;
        string AuthenType = "1";  // 默认是用户名

        Regex regMobile = new Regex(@"^1[3458]\d{9}$");
        Regex regEmail = new Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
        Regex regCard = new Regex(@"^(\d{9}|\d{16})$");

        if (regMobile.IsMatch(AuthenName))
        {
            AuthenType = "2";
        }
        if (regEmail.IsMatch(AuthenName))
        {
            AuthenType = "4";
        }
        if (regCard.IsMatch(AuthenName))
        {
            AuthenType = "3";
        }


        PageUtility.SetCookie("AuthenType", AuthenType, 168);           //168个小时，即一个礼拜
        PageUtility.SetCookie("LoginTabCookie", "BestToneTab", 8760);
        string CustID = "";
        string RealName = "";
        string NickName = "";
        string UserName = "";
        string OutID = "";
        string UserAccount = "";
        string ErrMsg = "";
        string CustType = "";
        string ProvinceID = "";
        int Result = 1;
        try
        {
            strLog.AppendFormat("checkCode={0}", Request.Form["checkCode"]);

            String VoidCheckCodeSPID = System.Configuration.ConfigurationManager.AppSettings["VoidCheckCodeSPID"];
            if (String.IsNullOrEmpty(VoidCheckCodeSPID))
            {
                if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["checkCode"].ToString().Trim()), this.Context))
                {
                    errorHint.InnerHtml = "验证码错误，请重新输入";
                    return;
                }
            }
            else
            {
                if (!VoidCheckCodeSPID.Equals("35433334"))
                {
                    if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["checkCode"].ToString().Trim()), this.Context))
                    {
                        errorHint.InnerHtml = "验证码错误，请重新输入";
                        return;
                    }
                }
            }

            if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["checkCode"].ToString().Trim()), this.Context))
            {
                errorHint.InnerHtml = "验证码错误，请重新输入";
                return;
            }

            //日志
            strLog.AppendFormat("【开始验证】:SPID:{0},ProvinceID:{1},AuthenName:{2},AuthenType:{3}", SPID, ProvinceID, AuthenName, AuthenType);

            Result = BTForBusinessSystemInterfaceRules.UserAuthV2(SPID, AuthenName, AuthenType, Password, Context, ProvinceID, "", "",
                out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  ProvinceID, out  RealName, out  UserName, out  NickName);
            //验证结果日志
            strLog.AppendFormat("【认证结果】:CustID:{0},UserAcount:{1},CustType:{2},OutID:{3},ProvinceID:{4},RealName:{5},UserName:{6},NickName:{7},Result:{8},ErrMsg:{9}\r\n",
                CustID, UserAccount, CustType, OutID, ProvinceID, RealName, UserName, NickName, Result, ErrMsg);
            CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, ProvinceID, AuthenType, AuthenName, "2", Result, ErrMsg);
            if (Result != 0)
            {
                if (Result == 1001 || Result == -20504 || Result == -21553)
                {
                    errorHint.InnerHtml = ErrMsg;
                    //hint_Username.InnerHtml = "";
                    return;
                }

                if (Result == -21501)
                {
                    errorHint.InnerHtml = ErrMsg;
                    return;
                }
                Response.Write(ErrMsg);
                return;
            }

            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);

            //生成token并保存
            UserToken UT = new UserToken();
            string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OutID, CustType, AuthenName, AuthenType, key, out ErrMsg);
            string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            PageUtility.SetCookie(CookieName, UserTokenValue);

            TokenValidate.IsRedircet = false;
            TokenValidate.Validate();

            this.ssoFunc();
        }
        catch (System.Exception ex)
        {
            log(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }

    }
}

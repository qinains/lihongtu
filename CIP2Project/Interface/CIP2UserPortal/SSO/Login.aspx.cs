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


using NetDimension.Web;
using NetDimension.Weibo;


using Newtonsoft.Json;
using BTUCenter.Proxy;
using System.Text.RegularExpressions;

public partial class SSO_NewLogin2 : System.Web.UI.Page
{



    Cookie cookie = new Cookie("SinaWeibo", 24, TimeUnit.Hour);
    Client Sina = null;

    private readonly string client_id = Utils.GetAppSeting("qzone_AppID");
    private readonly string client_secret = Utils.GetAppSeting("qzone_AppKey");
    private readonly string redirect_uri = Utils.GetAppSeting("qzone_Redirect_uri");
    private string state = "";

    private String UDBReturnURL = String.Empty;
    private String UdbSrcSsDeviceNo = String.Empty;
    private String UdbKey = String.Empty;

    private string passportLoginRequestValue;
    public string PassportLoginRequestValue
    {
        get { return passportLoginRequestValue; }
        set
        {
            passportLoginRequestValue = value;
        }
    }
    //url参数=SPID+ProvinceID+SourceType+ReturnURL
    string SPTokenRequest = "";
    string SPID = "35000000";
    string ReturnURL = "http://www.118114.cn/";
    string UAProvinceID = "";
    string SourceType = "";

    string ErrMsg = "";
    int Result = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

   
        //Response.AddHeader("P3P", "CP=CAO PSA OUR");
       
        err_Username.InnerHtml = "";
        err_Password.InnerHtml = "";
        err_code.InnerHtml = "";
        backCount.Value = Convert.ToString((Convert.ToInt32(backCount.Value) - 1));

        //判断并解析SPTokenRequest参数
        ParseSPTokenRequest();
        //生成udb请求参数,注意CreateUdbPassportLoginRequest()方法必须放在ParseSPTokenRequst()后面
        CreateUdbPassportLoginRequest();
        //新浪微博认证请求
        SinaOAuthRequest();
        //QQ认证
        QqOAuthRequest();
        //省网厅单点登陆入口
        if (SourceType == "1")
        {
            log(String.Format("【省网厅登录】:SourceType:{0},ProvinceID:{1}", SourceType, UAProvinceID));
            string Url = "login1.aspx?ProvinceID=" + UAProvinceID;
            PageUtility.SetCookie(ReturnURL, "ReturnURL", this.Page);
            PageUtility.SetCookie(SPID, "SPID", this.Page);
            Response.Redirect(Url, true);
            return;
        }

        //已登录流程
        TokenValidate.IsRedircet = false;
        TokenValidate.Validate();
        if (TokenValidate.Result == 0)
        {
            this.ssoFunc();
        }
        else if (CommonUtility.IsParameterExist("NeedLogin", this.Page))
        {
            //不需要客户认证平台登陆
            if (!"0".Equals(Request["NeedLogin"]))
            {
                Response.Redirect(ReturnURL + "?NeedLogin=1");
            }
        }

        if (!Page.IsPostBack)
        {
            BindAuthenType();
        }
        else
        {
            foreach (ListItem item in ddlAuthenTypeList.Items)
            {
                item.Attributes.Add("onclick", "javascript:selvalue()");
            }
        }
    }

    /// <summary>
    /// 绑定认证类型
    /// </summary>
    protected void BindAuthenType()
    {
        this.ddlAuthenTypeList.Items.Clear();
        ddlAuthenTypeList.Items.Add(new ListItem("用户名", "1"));
        ddlAuthenTypeList.Items.Add(new ListItem("手机号", "2"));
        ddlAuthenTypeList.Items.Add(new ListItem("商旅卡", "3"));
        ddlAuthenTypeList.Items.Add(new ListItem("邮箱", "4"));

        foreach (ListItem item in ddlAuthenTypeList.Items)
        {
            item.Attributes.Add("onclick", "javascript:selvalue('" + item.Value + "')");
        }

        //检测用户上一次登录类型
        String authenType = PageUtility.GetCookie("AuthenType");
        if (!String.IsNullOrEmpty(authenType))
            ddlAuthenTypeList.SelectedValue = authenType;
        else
            ddlAuthenTypeList.SelectedValue = "2";
    }


    /// <summary>
    /// 生成PassportLoginRequest参数
    /// </summary>
    protected void CreateUdbPassportLoginRequest()
    {
        UDBReturnURL = System.Configuration.ConfigurationManager.AppSettings["UDBReturnURL"];
        UDBReturnURL = UDBReturnURL + "&ReturnUrl=" + HttpUtility.UrlEncode(ReturnURL);
        UdbSrcSsDeviceNo = System.Configuration.ConfigurationManager.AppSettings["UdbSrcSsDeviceNo"];
        UdbKey = System.Configuration.ConfigurationManager.AppSettings["UdbKey"];
        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String Digest = CryptographyUtil.ToBase64String(CryptographyUtil.Hash(UdbSrcSsDeviceNo + TimeStamp + UDBReturnURL));
        passportLoginRequestValue = System.Web.HttpUtility.UrlEncode(UdbSrcSsDeviceNo + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + UDBReturnURL + "$" + Digest, UdbKey));

  
    }

    protected void SinaOAuthRequest()
    {

        string appKey = ConfigurationManager.AppSettings["AppKey"];
        string appSecret = ConfigurationManager.AppSettings["AppSecret"];
        string callbackUrl = ConfigurationManager.AppSettings["CallbackUrl"] ;
        OAuth oauth = new OAuth(appKey, appSecret, callbackUrl);

        Sina = new Client(oauth);
        if (!IsPostBack)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                AccessToken token = oauth.GetAccessTokenByAuthorizationCode(Request.QueryString["code"]);
                string accessToken = token.Token;

                cookie["AccessToken"] = accessToken;

                Response.Redirect("SinaDefaultV2.aspx"+Utils.UrlEncode("?ReturnUrl="+ReturnURL) );
            }
            else
            {
                string url = oauth.GetAuthorizeURL(ResponseType.Code, null, DisplayType.Default);
                authUrl.NavigateUrl = url;
            }

        }
    }

     protected void QqOAuthRequest()
     {

         state = Guid.NewGuid().ToString().Replace("-", "");
         //Utils.WriteCookie("state", state, 60);
         HttpContext.Current.Session["state"] = state;

         string login_url = "https://open.t.qq.com/cgi-bin/oauth2/authorize?response_type=code&client_id=" + client_id + "&state=" + state + "&redirect_uri=" + Utils.UrlEncode(redirect_uri) + "&scope=get_user_info"; //,get_info,get_other_info
         login_url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + client_id + "&state=" + state + "&redirect_uri=" + Utils.UrlEncode(redirect_uri+"?ReturnUrl="+ReturnURL) + "&scope=get_user_info"; //,get_info,get_other_info
         //grant_type=authoriaztion_code
         
         //写日志
         //Logs.logSave("===========================================分割线=============================================");
         //写日志
         //Logs.logSave("第一步：开始跳转至QQ登陆URL：" + login_url);

         //开始发送
         this.qqUrl.NavigateUrl = login_url;
     }

   

  

    /// <summary>
    /// 判断并解析SPTokenRequest参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //日志
            log("【SPTokenRequest参数】:" + SPTokenRequest);
            //解析请求参数
            Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnURL, out ErrMsg);
            //日志
            log(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnURL));

            if (Result != 0)
            {
                err_code.InnerHtml = "请求参数SPTokenRequest不正确";
            }

            if (!CommonUtility.ValidateUrl(ReturnURL.Trim()))
                err_code.InnerHtml = "请求参数ReturnURL不正确";

            string SignUpReturnUrl = HttpUtility.UrlEncode(SPTokenRequest);
            SignUpReturnUrl = "SPTokenRequest=" + SignUpReturnUrl;

            if (CommonUtility.IsParameterExist("NeedLogin", this.Page))
            {
                SignUpReturnUrl = SignUpReturnUrl + "&NeedLogin=" + HttpUtility.UrlEncode(Request["NeedLogin"]);
            }

            SignUpReturnUrl = HttpUtility.UrlEncode(this.Context.Request.Url.ToString().Substring(0, this.Context.Request.Url.ToString().IndexOf("?") + 1) + SignUpReturnUrl);

            this.linkU1.HRef = "../signup.aspx?SPID=" + SPID + "&ReturnUrl=" + SignUpReturnUrl;
            this.linkU2.HRef = "../signup.aspx?SPID=" + SPID + "&ReturnUrl=" + SignUpReturnUrl;

        }
    }

    protected void ssoFunc()
    {
        string Url = "";
        try
        {
            //生成ticket
            //string sDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //Random r = new Random();
            //string Ticket = sDate + r.Next(10000, 99999).ToString();
            string Ticket = CommonBizRules.CreateTicket();

            string CustID = TokenValidate.CustID;
            string RealName = TokenValidate.RealName;
            string NickName = TokenValidate.NickName;
            string UserName = TokenValidate.UserName;
            string OutID = TokenValidate.OuterID;
            string LoginAuthenName = TokenValidate.LoginAuthenName;
            string LoginAuthenType = TokenValidate.LoginAuthenType;

            String er = "";

            Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, NickName, UserName, OutID, "", LoginAuthenName, LoginAuthenType, out er);

            if (Result != 0)
            {
                err_code.InnerHtml = er;
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

            Response.Redirect(Url);
        }

        catch (Exception e)
        {
            err_code.InnerHtml = e.Message + ">>ReturnURL:" + Url;
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        string AuthenType = ddlAuthenTypeList.SelectedValue;
        PageUtility.SetCookie("AuthenType", AuthenType, 168);           //168个小时，即一个礼拜
        string AuthenName = txtUsername.Text;
        string Password = txtPassword.Text;
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

        if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["code"].ToString().Trim().ToUpper()), this.Context))
        {
            err_code.InnerHtml = "验证码错误，请重新输入";
            return;
        }

        //日志
        log(String.Format("【开始验证】:SPID:{0},ProvinceID:{1},AuthenName:{2},AuthenType:{3}", SPID, ProvinceID, AuthenName, AuthenType));

        Result = BTForBusinessSystemInterfaceRules.UserAuthV2(SPID, AuthenName, AuthenType, Password, Context, ProvinceID, "", "",
            out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  ProvinceID, out  RealName, out  UserName, out  NickName);
        //验证结果日志
        log(String.Format("【验证结果】:CustID:{0},UserAcount:{1},CustType:{2},OutID:{3},ProvinceID:{4},RealName:{5},UserName:{6},NickName:{7}",
            CustID, UserAccount, CustType, OutID, ProvinceID, RealName, UserName, NickName));
        CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, ProvinceID, AuthenType, AuthenName, "2", Result, ErrMsg);
        if (Result != 0)
        {
            if (Result == 1001 || Result == -20504 || Result == -21553)
            {
                err_Username.InnerHtml = ErrMsg;
                hint_Username.InnerHtml = "";
                return;
            }

            if (Result == -21501)
            {
                err_Password.InnerHtml = ErrMsg;
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
        //PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
        PageUtility.SetCookie(CookieName, UserTokenValue);

        TokenValidate.IsRedircet = false;

        TokenValidate.Validate();

        this.ssoFunc();
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("tb", msg);
    }

}

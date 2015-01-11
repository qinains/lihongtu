using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetDimension.Web;
using NetDimension.Weibo;
using Newtonsoft.Json;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

using Linkage.BestTone.Interface.Rule.ThirdPartyAuthen;

public partial class SSO_AuthBindLogin : System.Web.UI.Page
{
    public string SPTokenRequest = "";
    public string SPID = "35000000";
    public string ReturnUrl = "";
    public string CustID = "";
    public string ErrMsg = "";
    public string TimeStamp = "";
    public int Result=0;
    public string code = "";
    public string oauthtype = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (CommonUtility.IsParameterExist("code", this.Page))
        {
            code = Request["code"];
        }

        if (CommonUtility.IsParameterExist("oauthtype",this.Page))
        {
            oauthtype = Request["oauthtype"];
        }
        ParseSPTokenRequest();

    }


    /// <summary>
    /// 判断并解析SPTokenRequest参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
       
        string UAProvinceID = "";
        string SourceType = "";

        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //日志
            log("【SPTokenRequest参数】:" + SPTokenRequest);
            //解析请求参数
            Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnUrl, out ErrMsg);
            //日志
            log(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnUrl));
            if (Result != 0)
            {
                errorHint.InnerHtml = "请求参数SPTokenRequest不正确";
                return;
            }

            if (!CommonUtility.ValidateUrl(ReturnUrl.Trim()))
            {
                errorHint.InnerHtml = "请求参数ReturnURL不正确";
                return;
            }
        }

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

            if (ReturnUrl.IndexOf("?") > 0)
            {
                Url = ReturnUrl + "&Ticket=" + Ticket;
            }
            else
            {
                Url = ReturnUrl + "?Ticket=" + Ticket;
            }

            if (CommonUtility.IsParameterExist("NeedLogin", this.Page))
            {
                Url = Url + "&NeedLogin=" + Request["NeedLogin"];
            }
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            Response.Redirect(Url);
        }

        catch (Exception e)
        {
            errorHint.InnerHtml = e.Message + ">>ReturnURL:" + Url;
        }
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("+++++++++++++++++"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("+++++++++++++++++" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("AuthBindLogin", msg);
    }

    protected void auth_Click(object sender, EventArgs e)
    {
        string AuthenType = HttpUtility.HtmlDecode(Request.Form["AuthenType"].ToString().Trim().ToUpper());         //获取认证类型
        string password = Request.Form["password"].ToString().Trim();
        string username = Request.Form["username"].ToString().Trim();
        // 按照模式匹配出,认证模式 （手机，用户名，商旅卡，邮箱）认证
        // 如果认证通过，返回custid 
        // 绑定 custid和openid

        PageUtility.SetCookie("AuthenType", AuthenType, 168);           //168个小时，即一个礼拜
        string AuthenName = username;
        string Password = password;
        string CustID = "";
        string RealName = "";
        string NickName = "";
        string UserName = "";
        string OutID = "";
        string UserAccount = "";
        string CustType = "";
        string ProvinceID = "";
        string ErrMsg = "";
        int Result;

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
                errorHint.InnerHtml = ErrMsg;
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

        // 绑定custid 和code关系   begin
        string _connectionString = WebConfigurationManager.ConnectionStrings["BestToneCenterConStr"].ConnectionString;
        int result = -1;
        SqlConnection con = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand("insert into oauthaccount (openid,custid,createtime,status) values (@code,@CustID,getdate(),@oauthtype)", con);
        cmd.Parameters.Add("@code",SqlDbType.NVarChar,50).Value=code;
        cmd.Parameters.Add("@CustID", SqlDbType.NVarChar, 16).Value = CustID;
        cmd.Parameters.Add("@oauthtype", SqlDbType.NVarChar, 1).Value = oauthtype;

        using(con)
        {
            con.Open();
            result = cmd.ExecuteNonQuery();
        }
       //end
        if(result!= 0)
        {
            errorHint.InnerHtml = "绑定关系建立失败";
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
}

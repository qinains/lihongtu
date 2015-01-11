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

public partial class SSO_AuthRegister : System.Web.UI.Page
{
    public string SPTokenRequest = "";
    public string SPID = "35000000";
    public string ReturnUrl = "";
    public string CustID = "";
    public string ErrMsg = "";
    public string TimeStamp = "";
    public int Result = 0;
    public string code = "";
    public string oauthtype = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CommonUtility.IsParameterExist("code", this.Page))
        {
            code = Request["code"];
        }

        if (CommonUtility.IsParameterExist("oauthtype", this.Page))
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

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("AuthRegister", msg);
    }

    protected void register_Click(object sender, EventArgs e)
    {

        string mobile = this.mobile.Text;
        string checkCode = this.checkCode.Text;
        string password = this.password.Text;
        string password2 = this.password2.Text;

        //判断手机验证码
        if (checkCode != null && !"".Equals(checkCode))
        {
            Result = PhoneBO.SelSendSMSMassage("", mobile, checkCode, out ErrMsg);
            if (Result != 0)
            {
                errorHint.InnerHtml = "手机验证码错误，请重新输入";  
                return;
            }
        }

        Result = UserRegistry.quickUserRegistryWeb(SPID, password, mobile, "2", out CustID, out ErrMsg);
        if (Result != 0)
        {
            CommonBizRules.ErrorHappenedRedircet(Result, ErrMsg, "用户注册", this.Context);
            return;
        }

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

        string Digest = CryptographyUtil.GenerateAuthenticator(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg, key);
        string temp = SPID + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg + "$" + Digest, key);
        string RegistryResponseValue = HttpUtility.UrlEncode(temp);
        log(String.Format("key:{0},Digest:{1},temp:{2},RegistryResponseValue:{3}", key, Digest, temp, RegistryResponseValue));
        //给用户写cookie
        UserToken UT = new UserToken();
        string key2 = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);

        string RealName = mobile;
        string UserName = mobile;
        string NickName = mobile;

        //                                           CustID, RealName, UserName, NickName, OuterID, CustType, string LoginAuthenName, string LoginAuthenType,string key, out string ErrMsg
        string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, "", "42", UserName, "1", key2, out ErrMsg);
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);

        //通知积分平台
        CIP2BizRules.InsertCustInfoNotify(CustID, "2", SPID, "", "0", out ErrMsg);
        //记登录日志
        CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, "35", "0", "", "2", Result, ErrMsg);

        Response.Redirect(ReturnUrl + "?RegistryResponse=" + RegistryResponseValue, true);







    }
}

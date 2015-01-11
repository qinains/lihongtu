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

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public partial class Registration : System.Web.UI.Page
{

    public string SPID = "";
    public string ReturnUrl = "";
    public string CustID = "";
    public string ErrMsg = "";
    public string TimeStamp = "";
    public int Result;
    public string YiYouRegisterFrameURL = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");
        SPID = Request["SPID"] == null ? ConstHelper.DefaultInstance.BesttoneSPID : HttpUtility.HtmlDecode(Request["SPID"]);
        ReturnUrl = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.HtmlDecode(Request["ReturnUrl"]);
        HiddenField_SPID.Value = SPID;
        HiddenField_URL.Value = ReturnUrl;
    }



    protected void register_Click(object sender, EventArgs e)
    {

        Response.AddHeader("P3P", "CP=CAO PSA OUR");

        SPID = Request["SPID"] == null ? ConstHelper.DefaultInstance.BesttoneSPID : HttpUtility.HtmlDecode(Request["SPID"]);
        ReturnUrl = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.HtmlDecode(Request["ReturnUrl"]);

        log(String.Format("【Web注册】:SPID:{0},ReturnUrl:{1}", SPID, ReturnUrl));
        string password = Request.Form["password"].ToString().Trim();
        string telephone = Request.Form["mobile"].ToString().Trim();
        string phonecode = Request.Form["checkCode"].ToString().Trim();
        log(String.Format("mobile:{0},password:{1},phonecode:{2}", telephone, password, phonecode));
        string ErrMsg = "";
        int Result;

   
        if (CommonUtility.IsEmpty(password))
        {
            hintPassword.InnerHtml = "密码不能为空格"; // 这里如何控制样式
            return;
        }

        if (ViewState["phonestate"] == null)
        {
            ViewState["phonestate"] = Request.Form["phonestate"].ToString();
            string a = (string)ViewState["phonestate"];
        }

        if (((string)ViewState["phonestate"]).Equals("0"))
        {
            //判断手机验证码
            Result = PhoneBO.SelSendSMSMassage("", telephone, phonecode, out ErrMsg);
            if (Result != 0)
            {
                hintCode.InnerHtml = "手机验证码错误，请重新输入";  // 这里如何控制样式
                return;
            }

           
        }

        TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Result = UserRegistry.quickUserRegistryWeb(SPID, password, telephone, (string)ViewState["phonestate"], out CustID, out ErrMsg);
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

        string RealName = telephone;
        string UserName = telephone;
        string NickName = telephone;

        //                                           CustID, RealName, UserName, NickName, OuterID, CustType, string LoginAuthenName, string LoginAuthenType,string key, out string ErrMsg
        string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, "", "42", UserName, "1", key2, out ErrMsg);
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);

        //通知积分平台
        CIP2BizRules.InsertCustInfoNotify(CustID, "2", SPID, "", "0", out ErrMsg);
        //记登录日志
        CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, "35", "0", "", "2", Result, ErrMsg);

        log(String.Format("【返回之前】:SPID:{0},ReturnUrl:{1}", SPID, ReturnUrl));


        string hid_openAccount = Request.Form["hid_openAccount"].ToString().Trim();
        log(String.Format("开户选项:{0}", hid_openAccount));
     


        if (ReturnUrl.IndexOf("?") > 0)
        {
            if ("1".Equals(hid_openAccount))
            {
                // 这里要 跳回 调用方，并告知 CreateBesttoneAccount.aspx,让其重定向到该地址
                //Response.Write(" <A   id= 'kh '   href= 'CreateBesttoneAccount.aspx?mobile=" + telephone + "&ReturnUrl=" + ReturnUrl + "'   target= '_top '> </A> <script language='javascript' type='text/javascript'> document.getElementById('kh').click(); </script> ");
                //return;
                //Response.Redirect("CreateBesttoneAccount.aspx?mobile=" + telephone + "&ReturnUrl=" + ReturnUrl);
                Response.Redirect(ReturnUrl + "&RegistryResponse=" + RegistryResponseValue + "&registBesttoneAccount=true", true);
            }
            else {
                Response.Redirect(ReturnUrl + "&RegistryResponse=" + RegistryResponseValue, true);
            }


            //Response.Redirect(ReturnUrl  + "&RegistryResponse=" + RegistryResponseValue, true);
            //CommonBizRules.SuccessRedirect(ReturnUrl + "&RegistryResponse=" + RegistryResponseValue, "成功注册", this.Context);
        }
        else
        {
            if ("1".Equals(hid_openAccount))
            {
                // 这里要 跳回 调用方，并告知 CreateBesttoneAccount.aspx,让其重定向到该地址
                //Response.Write(" <A   id= 'kh '   href= 'CreateBesttoneAccount.aspx?mobile=" + telephone + "&ReturnUrl=" + ReturnUrl + "'   target= '_top '> </A> <script language='javascript' type='text/javascript'> document.getElementById('kh').click(); </script> ");
                //return;
                //Response.Redirect("CreateBesttoneAccount.aspx?mobile=" + telephone + "&ReturnUrl=" + ReturnUrl);
                Response.Redirect(ReturnUrl + "?RegistryResponse=" + RegistryResponseValue + "&registBesttoneAccount=true", true);
            }
            else
            {
                Response.Redirect(ReturnUrl + "?RegistryResponse=" + RegistryResponseValue, true);
            }
           
            //CommonBizRules.SuccessRedirect(ReturnUrl + "?RegistryResponse=" + RegistryResponseValue, "成功注册", this.Context);
        }

    }

    /// <summary>
    /// Base64反编码
    /// </summary>
    public static byte[] FromBase64String(string source)
    {
        return Convert.FromBase64String(source);
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("webregister", msg);
    }



}

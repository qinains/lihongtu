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
using Linkage.BestTone.Interface.Utility;
using System.Net;//网络功能 
using System.IO;//流支持
using System.Threading;//线程支持
using System.Text;

using Linkage.BestTone.Interface.Rule;

public partial class SSO_PassportLogout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        log(String.Format("CookieName:{0}",CookieName));
        PageUtility.ExpireCookie(CookieName, this.Page);
        string logOut = System.Configuration.ConfigurationManager.AppSettings["LogOut"];
        log(String.Format("logOut:{0}", logOut));
        string[] alLogOut = logOut.Split(';');
        int i = alLogOut.Length;

        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
        string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
        string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
        string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_clientType"];
        string format = "redirect";
        //string userId = Convert.ToString(accountInfo.userId);
        string parameters = "userId=&timeStamp=" + TimeStamp + "&udbUserId=&productUid=&returnURL=" + HttpUtility.UrlEncode(Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneHomePage : Request["ReturnUrl"].ToString());

        string paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);

        string sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);

        String UnifyPlatformLogoutUrl = UDBConstDefinition.DefaultInstance.UnifyPlatformLogoutUrl;
        UnifyPlatformLogoutUrl = UnifyPlatformLogoutUrl + "?appId=" + appId + "&version=" + version + "&clientType=" + clientType + "&paras=" + paras + "&sign=" + sign + "&format=redirect";

        Response.Write("<iframe frameborder='1'  width='100'   height='100' src='" + UnifyPlatformLogoutUrl + "'  style='display:none'></iframe>");


        for (i = 0; i < alLogOut.Length; i++)
        {
            Response.Write("<iframe frameborder='1'  width='100'   height='100'src='" + alLogOut[i] + "'  style='display  :none'></iframe>");
        }
        log(String.Format("ReturnUrl:{0}",Request["ReturnUrl"]));
        this.txtHid.Value = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneHomePage : Request["ReturnUrl"].ToString();
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("PassportLogout", msg);
    }

}

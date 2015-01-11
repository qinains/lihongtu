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
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Data.SqlClient;


public partial class SSO_mobile_PassportLogout : System.Web.UI.Page
{
    public String ReturnUrl = "http://www.114yg.cn";
    public String PUserID = String.Empty;
    public String UserID = String.Empty;

    private string passportLogoutRequestValue;
    public string PassportLogoutRequestValue
    {
        get { return passportLogoutRequestValue; }
        set
        {
            passportLogoutRequestValue = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //PUserID = Request["PUserID"];
        //UserID = Request["UserID"];
        //CreateUdbPassportLoginRequest(UserID, PUserID);
        //string UdbLogoutWapUrl = System.Configuration.ConfigurationManager.AppSettings["UdbLogoutWapUrl"];
        //Response.Redirect(UdbLogoutWapUrl + "?PassportLogoutRequest=" + passportLogoutRequestValue);
        StringBuilder strLog = new StringBuilder();
        try
        {
            string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            PageUtility.ExpireCookie(CookieName, this.Page);
        }
        catch (Exception ep)
        {
            strLog.AppendFormat("异常:{0}\r\n", ep.ToString());
        }

        try
        {
            string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
            string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
            string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
            string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_clientType"];
            string format = "redirect";
            //string userId = Convert.ToString(accountInfo.userId);
            string parameters = "userId=&timeStamp=" + TimeStamp + "&udbUserId=&productUid=&returnURL=" + HttpUtility.UrlEncode(Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneHomePage : Request["ReturnUrl"].ToString());
            //string parameters = "userId=&timeStamp=" + TimeStamp + "&udbUserId=&productUid=&returnURL="+HttpUtility.UrlEncode("m.114yg.cn") ;
            strLog.AppendFormat("parameters:{0}\r\n", parameters);
            string paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
            strLog.AppendFormat("paras:{0}\r\n", paras);
            string sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
            strLog.AppendFormat("sign:{0}\r\n", sign);
            String UnifyPlatformLogoutUrl = UDBConstDefinition.DefaultInstance.UnifyPlatformLogoutUrl;
            strLog.AppendFormat("UnifyPlatformLogoutUrl:{0}\r\n", UnifyPlatformLogoutUrl);
            UnifyPlatformLogoutUrl = UnifyPlatformLogoutUrl + "?appId=" + appId + "&version=" + version + "&clientType=" + clientType + "&paras=" + paras + "&sign=" + sign + "&format=redirect";
            strLog.AppendFormat("UnifyPlatformLogoutUrl:{0}\r\n", UnifyPlatformLogoutUrl);

            Response.Redirect(UnifyPlatformLogoutUrl, false);

        }
        catch (Exception ecp)
        {
            strLog.AppendFormat("异常:{0}\r\n", ecp.ToString());
        }
        finally {
            WriteLog(strLog.ToString());
        }


        //Response.Write("<iframe frameborder='1'  width='100'   height='100' src='" + UnifyPlatformLogoutUrl + "'  style='display:none'></iframe>");

  
    }
    protected void WriteLog(string str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("PassportLoginOutFromUnifyPlatform", msg);
    }
    protected void CreateUdbPassportLoginRequest(String UserID,String PUserID )
    {
        
        String UdbSrcSsDeviceNo = System.Configuration.ConfigurationManager.AppSettings["UdbSrcSsDeviceNo"];
        String UdbKey = System.Configuration.ConfigurationManager.AppSettings["UdbKey"];
        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // Digest =   SrcSsDeviceNo + UserID + PUserID＋TimeStamp+ReturnURL
        //PassportLogoutRequestValue = URLEncoding(SrcSsDeviceNo + “$” +Base64(Encrypt(UserID＋“$” + PUserID＋“$”＋TimeStamp+ “$”+ ReturnURL+ “$”+ Digest)))

        String Digest = CryptographyUtil.ToBase64String(CryptographyUtil.Hash(UdbSrcSsDeviceNo + UserID + PUserID + TimeStamp + ReturnUrl));
        passportLogoutRequestValue = System.Web.HttpUtility.UrlEncode(UdbSrcSsDeviceNo + "$" + CryptographyUtil.Encrypt(UserID + "$" + PUserID + "$" + TimeStamp + "$" + ReturnUrl + "$" + Digest, UdbKey));
        
        
    }

}

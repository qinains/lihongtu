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

public partial class SSO_Logout : System.Web.UI.Page
{
    public int Result = -1;
    public string ErrMsg = String.Empty;
    public StringBuilder sbLog = new StringBuilder();
    private UDBMBOSS _UDBMBoss = new UDBMBOSS();
    protected void Page_Load(object sender, EventArgs e)
    {
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        PageUtility.ExpireCookie(CookieName, this.Page);
        string logOut = System.Configuration.ConfigurationManager.AppSettings["LogOut"];
        string[] alLogOut = logOut.Split(';');
        int i = alLogOut.Length;

        sbLog.AppendFormat("logOut:{0}\r\n",logOut);
        //String UnifyPlatformCookieName = ConfigurationManager.AppSettings["UnifyPlatformCookieName"];
        //sbLog.AppendFormat("UnifyPlatformCookieName:{0}\r\n", UnifyPlatformCookieName);
        //string accessToken = String.Empty;
        //sbLog.Append("存在综合平台的accessToken存在cookie中吗\r\n");

        try
        {
            //if (PageUtility.IsCookieExist(UnifyPlatformCookieName, this.Context))
            //{
            //    sbLog.Append("存在\r\n");
            //    accessToken = Request.Cookies.Get(UnifyPlatformCookieName).Value;
            //    sbLog.AppendFormat("accesstoken:{0}\r\n", accessToken);
                //string unifyPlatform_appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
                //string unifyPlatform_appSecretKey = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
                //sbLog.AppendFormat("appid:{0}\r\n", unifyPlatform_appId);
                //sbLog.AppendFormat("appsecretkey:{0}\r\n", unifyPlatform_appSecretKey);
                //UnifyAccountInfo accountInfo = new UnifyAccountInfo();
                //String clientIp = System.Configuration.ConfigurationManager.AppSettings["CIP2_clientIp"];//? 通过f5出去的，这样获得地址不对
                //if (String.IsNullOrEmpty(clientIp))
                //{
                //    clientIp = Request.UserHostAddress;
                //}

                //String clientAgent = Request.UserAgent;
                //sbLog.Append("清除cookie中的accessToken\r\n");
                //PageUtility.ExpireCookie(UnifyPlatformCookieName, this.Page);  // 清掉综合平台留下的access token
                //if (!String.IsNullOrEmpty(accessToken))
                //{
                    //string p_version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
                    //string p_clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
                    //Result = _UDBMBoss.UnifyPlatformGetUserInfo(unifyPlatform_appId, unifyPlatform_appSecretKey, p_version, p_clientType, accessToken, clientIp, clientAgent, out accountInfo, out ErrMsg);
                    //sbLog.AppendFormat("getUserInfo.do:result:{0},Errmsg:{1},userId:{2}\r\n", Result, ErrMsg, accountInfo.userId);

                    //PageUtility.ExpireCookie(UnifyPlatformCookieName, this.Page);  // 清掉综合平台留下的access token
                    //if (Result == 0)
                    //{
                        sbLog.Append("调综合平台退出接口(重定向)\r\n");
                        //调综合平台退出接口(重定向)
                        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
                        string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
                        string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
                        string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_clientType"];
                        string format = "redirect";
                        //string userId = Convert.ToString(accountInfo.userId);
                        string parameters = "userId=&timeStamp=" + TimeStamp + "&udbUserId=&productUid=&returnURL=" + HttpUtility.UrlEncode(Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneHomePage : Request["ReturnUrl"].ToString());
                        sbLog.AppendFormat("参数:{0}\r\n", parameters);
                        string paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                        sbLog.AppendFormat("参数:{0},paras:{1}\r\n", parameters, paras);
                        string sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                        sbLog.AppendFormat("sign:{0}\r\n", sign);
                        String UnifyPlatformLogoutUrl = UDBConstDefinition.DefaultInstance.UnifyPlatformLogoutUrl;
                        UnifyPlatformLogoutUrl = UnifyPlatformLogoutUrl + "?appId=" + appId + "&version=" + version + "&clientType=" + clientType + "&paras=" + paras + "&sign=" + sign + "&format=redirect";
                        sbLog.AppendFormat("UnifyPlatformLogoutUrl:{0}\r\n", UnifyPlatformLogoutUrl);
                        Response.Write("<iframe frameborder='1'  width='100'   height='100' src='" + UnifyPlatformLogoutUrl + "'  style='display:none'></iframe>");
                    //}

                //}
            //}
            //else
            //{
            //    sbLog.Append("不存在\r\n");
            //}

            //Response.Write("<iframe frameborder='1'  width='100'   height='100' src='http://service.passport.189.cn/logon/UDBCommon/PassportLogout.aspx'  style='display:none'></iframe>");

            for (i = 0; i < alLogOut.Length; i++)
            {
                Response.Write("<iframe frameborder='1'  width='100'   height='100' src='" + alLogOut[i] + "'  style='display  :none'></iframe>");
            }

            this.txtHid.Value = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneHomePage : Request["ReturnUrl"].ToString();




        }
        catch (Exception ecp)
        {
            sbLog.AppendFormat("异常:{0}\r\n",ecp.ToString());
        }
        finally
        {
            WriteLog(sbLog.ToString());
        }
 
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
        BTUCenterInterfaceLog.CenterForBizTourLog("Logout", msg);
    }


    /*
        public class RunGetXmlFile{
            public int threadh;
            public  RunGetXmlFile(int i)
            {
                threadh=i;
            }

            public void getXmlFile()
            {
                HttpWebResponse res = null;
                string strResult = "";

                try
                {
                    string url = urlList[threadh];
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "GET";
                    req.KeepAlive = true;
                    req.Referer = "";
                    // CookieContainer   cookieCon   =   new   CookieContainer();   
                    // req.CookieContainer   =   cookieCon;   
                    //req.CookieContainer.SetCookies(new   Uri(url),cookieheader);   

                    StringBuilder UrlEncoded = new StringBuilder();
                    res = (HttpWebResponse)req.GetResponse();
                    Stream ReceiveStream = res.GetResponseStream();
                    Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
                    StreamReader sr = new StreamReader(ReceiveStream, encode);
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);
                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        strResult += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
                catch (Exception e)
                {
                    strResult = e.ToString();
                }
                finally
                {
                    if (res != null)
                    {
                        res.Close();
                    }
                }

                BTUCenterInterfaceLog.CenterForBizTourLog("RunGetXmlFile",new StringBuilder(strResult));
                //return strResult;
            }   

            }*/

}

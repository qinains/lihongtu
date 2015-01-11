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
using Linkage.BestTone.Interface.Rule;
using System.Data.SqlClient;
using System.Text;

public partial class SSO_CustInfo1 : System.Web.UI.Page
{
    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("custinfo1", msg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        string isLogin = "1";
        string welcomeName = "0";
        string encryptCustIDValue = "0";
        if (PageUtility.IsCookieExist(CookieName, this.Context))
        {
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
            UserToken UT = new UserToken();
            string strCIPToken = Request.Cookies.Get(CookieName).Value;
            string custID;
            string realName;
            string userName;
            string nickName;
            string outerID;
            string custType;
            string loginAuthenName;
            string loginAuthenType;
            string TimeStamp = "";
            string SPID = "";
            string errMsg = "";
            int result = UT.ParseUserToken(strCIPToken, key, out custID, out realName, out userName, out nickName, out outerID, out custType, out loginAuthenName, out loginAuthenType, out errMsg);
            log("result="+result+";custID="+custID+";outerID="+outerID+"\r\n");
            string json_custinfo = "";
            json_custinfo = json_custinfo + "{";

            if (result == 0)
            {
                isLogin = "0";

                if (realName != null && !"".Equals(realName))
                {
                    welcomeName = realName;
                }
                else if (nickName != null && !"".Equals(nickName))
                {
                    welcomeName = nickName;
                }
                else if (userName != null && !"".Equals(userName))
                {
                    welcomeName = userName;
                }

                json_custinfo = json_custinfo + "isLogin" + ":" + "'" + isLogin + "',";

                json_custinfo = json_custinfo + "welcomeName" + ":" + "'" + realName + "',";

                json_custinfo = json_custinfo + "outerID" + ":" + "'" + outerID + "',";
                //json_custinfo = json_custinfo + "encryptCustIDValue"+":"+"'"+


            }

            if (CommonUtility.IsParameterExist("SPID", this.Page))
            {

                TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SPID = Request["SPID"];
                spInfo = new SPInfoManager();
                SPData = spInfo.GetSPData(this.Context, "SPData");
                key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                string Digest = CryptographyUtil.GenerateAuthenticator(TimeStamp + "$" + custID + "$" + result + "$" + errMsg, key);
                encryptCustIDValue = SPID + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + custID + "$" + result + "$" + errMsg + "$" + Digest, key);
                //string RegistryResponseValue = HttpUtility.UrlEncode(temp);
                json_custinfo = json_custinfo + "encryptCustIDValue" + ":" + "'" + encryptCustIDValue + "'";
            }
            json_custinfo = json_custinfo + "}";

            Response.Write("var o ="+json_custinfo);
        }
        else
        {
            //综合平台渠道udb渠道控制
            String UDBorUnifyPlatform = String.Empty;
            try
            {
                SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                StringBuilder sql = new StringBuilder();
                sql.Append("select platform_name from udb_authen_platform where flag=1 ");   // 1生效  0 失效
                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
                using (conn)
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UDBorUnifyPlatform = (String)reader["platform_name"];
                    }
                }

            }
            catch (Exception ex)
            {
                UDBorUnifyPlatform = System.Configuration.ConfigurationManager.AppSettings["UDBorUnifyPlatform"];
                strLog.AppendFormat("UDBorUnifyPlatform异常:{0}\r\n", ex.ToString());
            }
            strLog.AppendFormat("UDBorUnifyPlatform:{0}\r\n", UDBorUnifyPlatform);

            //单双向sso控制
            String ssoway = String.Empty;
            try
            {
                SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                StringBuilder sql = new StringBuilder();
                sql.Append("select ssoway from unifyAuthen  ");   // 1生效  0 失效
                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
                using (conn)
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ssoway = (String)reader["ssoway"];
                    }
                }

            }
            catch (Exception ex)
            {
                strLog.AppendFormat("ssoway异常:{0}\r\n", ex.ToString());
                ssoway = "1";
            }
            strLog.AppendFormat("ssoway:{0}\r\n", ssoway);
            log(strLog.ToString());

            if (!String.IsNullOrEmpty(UDBorUnifyPlatform))
            {
                if (UDBorUnifyPlatform.ToLower().Equals("unifyplatform") && ssoway.Equals("2"))  //双向sso
                {
                    //String UnifyAccountCheckResult = String.Empty;
                    //if (CommonUtility.IsParameterExist("UnifyAccountCheckResult", this.Page))
                    //{
                    //     UnifyAccountCheckResult = Request["UnifyAccountCheckResult"];
                    //}
                    //strLog.AppendFormat("UnifyAccountCheckResult:{0}\r\n", UnifyAccountCheckResult);
                    //if ("1".Equals(UnifyAccountCheckResult) || String.IsNullOrEmpty(UnifyAccountCheckResult))
                    //{
                    //检查登录状态
                    if (!CommonUtility.IsParameterExist("UnifyAccountCheckResult", this.Page))
                    {
                        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
                        string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
                        string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
                        string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_clientType"];
                        string accountType = UDBConstDefinition.DefaultInstance.UnifyPlatformAccountType;
                        string format = "redirect";
                        String returnURL = HttpUtility.UrlEncode(UDBConstDefinition.DefaultInstance.UnifyAccountCheckCallBackUrlYY + "?SPID=35000000");
                        string parameters = "&timeStamp=" + TimeStamp + "&accoutType=" + accountType + "&returnURL=" + returnURL;
                        strLog.AppendFormat("参数:{0}\r\n", parameters);
                        string paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                        strLog.AppendFormat("参数:{0},paras:{1}\r\n", parameters, paras);
                        string sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                        strLog.AppendFormat("sign:{0}\r\n", sign);
                        String UnifyAccountCheckUrl = UDBConstDefinition.DefaultInstance.UnifyAccountCheckUrl;
                        UnifyAccountCheckUrl = UnifyAccountCheckUrl + "?appId=" + appId + "&version=" + version + "&clientType=" + clientType + "&paras=" + paras + "&sign=" + sign + "&format=redirect";
                        strLog.AppendFormat(" Redirect to UnifyAccountCheckUrl:{0}\r\n", UnifyAccountCheckUrl);
                        log(strLog.ToString());
                        Response.Redirect(UnifyAccountCheckUrl, false);
                    }

                    //}

                }

            }
            else
            {

            }



        }

    }
}
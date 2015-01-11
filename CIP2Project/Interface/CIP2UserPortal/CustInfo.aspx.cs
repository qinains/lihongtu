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
public partial class CustInfo : System.Web.UI.Page
{

    private string phone;

    public string Phone
    {
        get { return phone; }
        set { phone = value; }
    }

    private string isLogin;
    public string IsLogin
    {
        get { return isLogin; }
        set
        {
            isLogin = value;
        }
    }

    private string welcomeName;
    public string WelcomeName
    {
        get { return welcomeName; }
        set
        {
            welcomeName = value;

        }
    }


    public  string outerID=String.Empty;
  

    private string encryptCustIDValue;
    public string EncryptCustIDValue
    {
        get { return encryptCustIDValue; }
        set
        {
            encryptCustIDValue = value;

        }
    }

    

    protected void ParseToken(String CookieName)
    {
        StringBuilder strLog = new StringBuilder();
        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
        UserToken UT = new UserToken();
        string strCIPToken = Request.Cookies.Get(CookieName).Value;
        string custID;
        string realName;
        string userName;
        string nickName;
        //string outerID;
        string custType;
        string loginAuthenName;
        string loginAuthenType;
        string TimeStamp = "";
        string SPID = "";
        string errMsg = "";
        int result = 0;

        try
        {
            log("custinfo 解:" + String.Format("token:{0}", strCIPToken));
            outerID = String.Empty;
            result = UT.ParseUserToken(strCIPToken, key, out custID, out realName, out userName, out nickName, out outerID, out custType, out loginAuthenName, out loginAuthenType, out errMsg);
            log("result="+result+";custID="+custID+";outerID="+outerID+"\r\n");
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
            }
        }
        catch (System.Exception ex)
        {
            strLog.AppendFormat("异常:{0}\r\n", ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }
    }

    protected void CheckToken()
    {
        StringBuilder strLog = new StringBuilder();
        String LocalCookie = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        String UnifyPlatformCookie = ConfigurationManager.AppSettings["UnifyPlatformCookieName"];
        isLogin = "1";
        welcomeName = "0";
        encryptCustIDValue = "0";

        if (PageUtility.IsCookieExist(UnifyPlatformCookie, this.Context))  // unifyplatform token
        {
            if (PageUtility.IsCookieExist(LocalCookie, this.Context))  // local token
            {
                ParseToken(Request.Cookies.Get(LocalCookie).Value);
            }
            else  //建立localtoken
            {
                string UnifyPlatformToken = Request.Cookies.Get(UnifyPlatformCookie).Value;
                PageUtility.SetCookie(UnifyPlatformToken, LocalCookie, this.Page);
                ParseToken(Request.Cookies.Get(LocalCookie).Value);
            }
        }
        else  // 全局token不存在  unifyAccountCheck 检查登录状态
        {
            if (IsUnifyPlatformChannel() && Bidirectional())
            {
                //检查登录状态
                if (!CommonUtility.IsParameterExist("UnifyAccountCheckResult", this.Page))
                {
                    string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; 
                    string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  
                    string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  
                    string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;  
                    string accountType = UDBConstDefinition.DefaultInstance.UnifyPlatformAccountType;
                    string format = "redirect";
                    String returnURL = HttpUtility.UrlEncode(UDBConstDefinition.DefaultInstance.UnifyAccountCheckCallBackUrl + "?SPID=35000000");
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
                if (PageUtility.IsCookieExist(LocalCookie, this.Context))  // 局部token 存在
                {
                    PageUtility.ExpireCookie(LocalCookie, this.Page);
                }
            }
 
        }
    }



    protected bool Bidirectional()
    {
        bool flag = false;
        StringBuilder strLog = new StringBuilder();
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
                    if ("2".Equals(ssoway))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            strLog.AppendFormat("ssoway:{0}\r\n", ssoway);
        }
        catch (Exception ex)
        {
            strLog.AppendFormat("ssoway异常:{0}\r\n", ex.ToString());
            ssoway = "1";
            flag = false;
        }
        finally
        {
            log(strLog.ToString());
        }
        return flag;
    }

    /// <summary>
    /// 判断配置是否指向综合平台
    /// </summary>
    /// <returns></returns>
    protected bool IsUnifyPlatformChannel()
    {
        bool flag = false;
        StringBuilder strLog = new StringBuilder();
        try
        {
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
            if (!String.IsNullOrEmpty(UDBorUnifyPlatform))
            {
                if (UDBorUnifyPlatform.ToLower().Equals("unifyplatform") )
                {
                    flag = true;
                }else
                {
                    flag = false;
                }
            }else
            {
                flag = false;
            }
        }
        catch (Exception e)
        {
            flag = false;
            strLog.AppendFormat("异常:{0}\r\n",e.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }
        return flag;

    }

    protected bool IsUnifyPlatformCookieExist(String CookieName, HttpContext context)
    {
        bool flag = false;
        StringBuilder strLog = new StringBuilder();
        try
        {
            if (IsUnifyPlatformChannel())
            {
                if (!CommonUtility.IsParameterExist("UnifyAccountCheckResult", this.Page))
                {
                    string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
                    string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
                    string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
                    string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_clientType"];
                    string accountType = UDBConstDefinition.DefaultInstance.UnifyPlatformAccountType;
                    string format = "redirect";
                    String returnURL = HttpUtility.UrlEncode(UDBConstDefinition.DefaultInstance.UnifyAccountCheckCallBackUrl + "?SPID=35000000");
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
                else
                {
                    String UnifyAccountCheckResult = Request["UnifyAccountCheckResult"];
                    flag = "0".Equals(UnifyAccountCheckResult) ? true : false;
                }
            }
            else
            {
                flag = false;
            }
            
        }
        catch (Exception e)
        {
            flag = false;
            strLog.AppendFormat("异常:{0\r\n}",e.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }
        return flag;
    }

    //6.1
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    CheckToken();
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        isLogin = "1";
        welcomeName = "0";
        encryptCustIDValue = "0";
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
            //string outerID;
            string custType;
            string loginAuthenName;
            string loginAuthenType;
            string TimeStamp = "";
            string SPID = "";
            string errMsg = "";
            int result = 0;
            try
            {
                log("custinfo 解:" + String.Format("token:{0}", strCIPToken));

                result = UT.ParseUserToken(strCIPToken, key, out custID, out realName, out userName, out nickName, out outerID, out custType, out loginAuthenName, out loginAuthenType, out errMsg);
                log("result="+result+";custID="+custID+";outerID="+outerID+"\r\n");
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
                    else if (!String.IsNullOrEmpty(loginAuthenName))
                    {
                        welcomeName = loginAuthenName;
                       
                    }
                }

                try
                {
                    if (!String.IsNullOrEmpty(custID))
                    {
                        int _result = 0;
                        string _errMsg = "";
                        PhoneRecord[] prs = this.getPhoneRecord(custID, out _result, out _errMsg);
                        if (prs != null)
                        {
                            if (prs.Length > 0)
                            {
                                Phone = prs[0].Phone;
                                
                            }
                        }
                    }
                }
                catch (Exception pe)
                {
                    log(pe.Message);
                }

                log("SPID 解:" +Request["SPID"]);

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
                }
            }
            catch (System.Exception ex)
            {
                log(ex.ToString());
            }
        }
        else
        {   // 不是从登陆入口进入，而是从别的平台（比如189.cn）单点登录过来的
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
                        String returnURL = HttpUtility.UrlEncode(UDBConstDefinition.DefaultInstance.UnifyAccountCheckCallBackUrl + "?SPID=35000000");
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

                }

            }
            else
            {

            }
        }
    }



    private  PhoneRecord[] getPhoneRecord(string CustID, out int Result, out string ErrMsg)
    {
        PhoneRecord[] PhoneRecords = null;
        Result = -19999;
        ErrMsg = "";

        SqlConnection myCon = null;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = null;

        try
        {
            myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_CustPhoneQuery";

            SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
            parCustID.Value = CustID;
            cmd.Parameters.Add(parCustID);

            ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        int RowCount = ds.Tables[0].Rows.Count;
                        PhoneRecords = new PhoneRecord[RowCount];
                        PhoneRecord phoneRecord = new PhoneRecord();
                        for (int i = 0; i < RowCount; i++)
                        {
                            phoneRecord = new PhoneRecord();
                            phoneRecord.Phone = ds.Tables[0].Rows[i]["Phone"].ToString().Trim();
                            phoneRecord.PhoneClass = ds.Tables[0].Rows[i]["PhoneClass"].ToString().Trim();
                            if (!"1".Equals(phoneRecord.PhoneClass))
                            {
                                PhoneRecords[i] = phoneRecord;
                            }
                            
                        }

                    }
                    Result = 0;

                }
            }


        }
        catch (Exception e)
        {
            Result = -22500;
            ErrMsg = "获取客户电话号码出错，" + e.Message;
        }

        return PhoneRecords;

    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("custinfo", msg);
    }

}

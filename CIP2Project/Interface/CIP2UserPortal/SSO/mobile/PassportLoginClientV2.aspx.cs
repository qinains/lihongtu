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
using System.Text.RegularExpressions;
using log4net;

public partial class SSO_mobile_PassportLoginClientV2 : System.Web.UI.Page
{

    #region 天翼账号登录变量
    private String UDBLoginURL = String.Empty;
    private String UDBReturnURL = String.Empty;
    private String UdbSrcSsDeviceNo = String.Empty;
    private String UdbKey = String.Empty;
    private static readonly ILog logger = LogManager.GetLogger(typeof(SSO_mobile_PassportLoginClientV2));
    public string login189Url;
    public string Login189Url
    {
        get { return login189Url; }
        set
        {
            login189Url = value;
        }
    }


    private string passportLoginRequestValue;
    public string PassportLoginRequestValue
    {
        get { return passportLoginRequestValue; }
        set
        {
            passportLoginRequestValue = value;
        }
    }
    #endregion

    //url参数=SPID+ProvinceID+SourceType+ReturnURL
    string SPTokenRequest = "";
    string SPID = "";
    string ReturnURL = "http://114yg.cn/";

    string UAProvinceID = "";
    string SourceType = "";
    string welcomeName = "";
    string ErrMsg = "";
    int Result = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");
        //获取ReturnUrl
        //ParseSPTokenRequest();
        //CreateUdbPassportLoginRequest();
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
        }
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("UDBorUnifyPlatform:{0}", UDBorUnifyPlatform);
        log(strLog.ToString());
        if (!String.IsNullOrEmpty(UDBorUnifyPlatform))
        {
            if (UDBorUnifyPlatform.ToLower().Equals("unifyplatform"))
            {
                CreateUnifyPlatformLoginRequest();
            }
            else
            {
                CreateUdbPassportLoginRequest();
            }
        }
        else
        {
            CreateUdbPassportLoginRequest();
        }


    }


    /// <summary>
    /// 判断并解析SPTokenRequest参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            String reg_url = System.Configuration.ConfigurationManager.AppSettings["YgMobileReturnURL"];
            if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
            {
                SPTokenRequest = Request["SPTokenRequest"];
                strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
                Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnURL, out ErrMsg);
                strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnURL));
            }
            else
            {
                //this.errorHint.InnerText = "SPTokenRequest参数缺失";
                return;
            }

        }
        catch (System.Exception ex)
        {
            strLog.Append(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }

    }



    protected void CreateUdbPassportLoginRequest()
    {
        UDBReturnURL = System.Configuration.ConfigurationManager.AppSettings["UDBClientCallBackReturnURL"];   // udb 回调客户信息平台地址
        UDBReturnURL = UDBReturnURL + "&ReturnUrl=" + HttpUtility.UrlEncode(ReturnURL);
        UdbSrcSsDeviceNo = System.Configuration.ConfigurationManager.AppSettings["UdbSrcSsDeviceNo"];
        UdbKey = System.Configuration.ConfigurationManager.AppSettings["UdbKey"];
        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String Digest = CryptographyUtil.ToBase64String(CryptographyUtil.Hash(UdbSrcSsDeviceNo + TimeStamp + UDBReturnURL));
        passportLoginRequestValue = System.Web.HttpUtility.UrlEncode(UdbSrcSsDeviceNo + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + UDBReturnURL + "$" + Digest, UdbKey));
        UDBLoginURL = System.Configuration.ConfigurationManager.AppSettings["UDBWAPLoginURL"];  //http://passport.wap.189.cn/WapLogin.aspx
        login189Url = UDBLoginURL + "?PassportLoginRequest=" + passportLoginRequestValue;// +"&ReturnUrl=" + ReturnURL;
        logger.Info("UDBReturnURL=" + UDBReturnURL);
        logger.Info("UdbSrcSsDeviceNo=" + UdbSrcSsDeviceNo);
        logger.Info("UdbKey=" + UdbKey);
        logger.Info("Digest=" + Digest);
        logger.Info("passportLoginRequestValue=" + passportLoginRequestValue);
        logger.Info("login189Url=" + login189Url);
        Response.Redirect(login189Url);
        //login189.NavigateUrl = UDBLoginURL + "?PassportLoginRequest=" + passportLoginRequestValue;
        //login189.Target = System.Configuration.ConfigurationManager.AppSettings["YgLoginTargetURL"];
    }



    protected void CreateUnifyPlatformLoginRequest()
    {

        string unifyPlatformLogonUrl = UDBConstDefinition.DefaultInstance.UnifyPlatformLogonUrl;   //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_LogonUrl"];  // 综合平台回调客户信息平台地址
        //unifyPlatformLogonUrl = unifyPlatformLogonUrl + "&ReturnUrl=" + HttpUtility.UrlEncode(ReturnURL);
        string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
        string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];
        string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
        string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_clientType"];
        string accountType = UDBConstDefinition.DefaultInstance.UnifyPlatformAccountType;   //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_accountType"];
        string pageKey = UDBConstDefinition.DefaultInstance.UnifyPlatformPageKey;   //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_pageKey"];
        string businessPage = UDBConstDefinition.DefaultInstance.UnifyPlatformBusinessPage;   //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_businessPage"];
        string thirdAccount = UDBConstDefinition.DefaultInstance.UnifyPlatformThirdAccount;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_thirdAccount"];
        string mustBind = UDBConstDefinition.DefaultInstance.UnifyPlatformMustBind;   //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_mustBind"];
        string quicklogin = UDBConstDefinition.DefaultInstance.UnifyPlatformQuicklogin;    //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_quicklogin"];
        string returnURL = UDBConstDefinition.DefaultInstance.UnifyPlatformCallBackForClientUrl;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatformCallBackUrl"];
        //returnURL = returnURL + "?SPID=" + SPID + "&ReturnUrl="+ HttpUtility.UrlEncode(ReturnURL);
        returnURL = HttpUtility.UrlEncode(returnURL + "?SPID=" + SPID + "&ReturnUrl=" + HttpUtility.UrlEncode(ReturnURL));
        string format = "redirect";
        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (String.IsNullOrEmpty(accountType))
        {
            accountType = "01";  //accountType  01(手机，邮箱，别名)   ,02 所有账号包括互联网账号？
        }
        if (String.IsNullOrEmpty(pageKey))
        {
            pageKey = "default";
        }
        if (String.IsNullOrEmpty(thirdAccount))
        {
            thirdAccount = "yes";
        }
        if (String.IsNullOrEmpty(mustBind))
        {
            mustBind = "yes";
        }
        if (String.IsNullOrEmpty(quicklogin))
        {
            quicklogin = "yes";
        }


        string parameters = "timeStamp=" + TimeStamp + "&returnURL=" + returnURL + "&accoutType=" + accountType + "&zhUserName=&pageKey=" + pageKey + "&businessPage=" + businessPage + "&thirdAccount=" + thirdAccount + "&mustBind=" + mustBind + "&quicklogin=" + quicklogin;
        string paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
        string sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);

        login189Url = unifyPlatformLogonUrl + "?appId=" + appId + "&version=" + version + "&clientType=" + clientType + "&paras=" + paras + "&sign=" + sign + "&format=redirect";
        if ("35433333".Equals(SPID))
        {
            login189Url = unifyPlatformLogonUrl + "?appId=" + appId + "&version=" + version + "&clientType=" + clientType + "&paras=" + paras + "&sign=" + sign + "&format=redirect&btnC=blue";

        }
        log("login189Url=" + login189Url);
        Response.Redirect(login189Url);
    }
    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("mobile-passportlogin", msg);
    }


    protected int UserAuthV2(string SPID, string AuthenName, string AuthenType, string Password, HttpContext Context, string ProvinceID, string IsQuery, string PwdType, out string ErrMsg, out string CustID, out string UserAccount, out string CustType, out string outerid, out string ProvinceID1, out  string RealName, out string UserName, out string NickName)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("UserAuthV3-AuthenType:{0},AuthenName:{1},Password:{2}\r\n", AuthenType, AuthenName, Password);
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = "";
        CustID = "";
        UserAccount = "";
        CustType = "";
        RealName = "";
        UserName = "";
        NickName = "";
        outerid = "";
        ProvinceID1 = "";
        string UProvinceID = "";
        string SysID = "";
        string AreaID = "";

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 15;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_Customer_OV3_Interface_UserAuthV2";


            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
            parAuthenName.Value = AuthenName;
            cmd.Parameters.Add(parAuthenName);

            SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
            parAuthenType.Value = AuthenType;
            cmd.Parameters.Add(parAuthenType);

            SqlParameter parPwd = new SqlParameter("@Pwd", SqlDbType.VarChar, 100);
            parPwd.Value = CryptographyUtil.Encrypt(Password);
            cmd.Parameters.Add(parPwd);


            SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 100);
            parPwdType.Value = PwdType;
            cmd.Parameters.Add(parPwdType);


            SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
            parResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parResult);

            SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
            parErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parErrMsg);

            SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
            parCustID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parCustID);

            SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
            parUserAccount.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parUserAccount);

            SqlParameter parCustType = new SqlParameter("@CustType ", SqlDbType.VarChar, 2);
            parCustType.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parCustType);

            SqlParameter parUProvinceID = new SqlParameter("@UProvinceID ", SqlDbType.VarChar, 2);
            parUProvinceID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parUProvinceID);

            SqlParameter parSysID = new SqlParameter("@SysID ", SqlDbType.VarChar, 8);
            parSysID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parSysID);

            SqlParameter parAreaID = new SqlParameter("@AreaID ", SqlDbType.VarChar, 3);
            parAreaID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parAreaID);

            SqlParameter parOuterID = new SqlParameter("@outerid ", SqlDbType.VarChar, 30);
            parOuterID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parOuterID);


            SqlParameter parUserName = new SqlParameter("@UserName ", SqlDbType.VarChar, 30);
            parUserName.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parUserName);


            SqlParameter parRealName = new SqlParameter("@RealName ", SqlDbType.VarChar, 30);
            parRealName.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parRealName);


            SqlParameter parNickName = new SqlParameter("@NickName ", SqlDbType.VarChar, 30);
            parNickName.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parNickName);

            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            Result = int.Parse(parResult.Value.ToString());
            ErrMsg = parErrMsg.Value.ToString();
            CustID = parCustID.Value.ToString();
            UserAccount = parUserAccount.Value.ToString();
            CustType = parCustType.Value.ToString();
            UProvinceID = parUProvinceID.Value.ToString();
            SysID = parSysID.Value.ToString();
            AreaID = parAreaID.Value.ToString();

            outerid = parOuterID.Value.ToString();
            ProvinceID1 = UProvinceID;

            RealName = parRealName.Value.ToString();
            UserName = parUserName.Value.ToString();
            NickName = parNickName.Value.ToString();

            strLog.AppendFormat("UserAuthV3认证结果：Result:{0};ErrMsg:{1};CustID:{2};CustType:{3};UProvinceID:{4};RealName:{5};UserName:{6}", Result, ErrMsg, CustID, CustType, UAProvinceID, RealName, UserName);
        }
        catch (Exception ex)
        {
            strLog.AppendFormat(ex.ToString());

        }
        finally
        {
            log(strLog.ToString());
        }
        return Result;
    }

}

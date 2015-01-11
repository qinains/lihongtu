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

public partial class SSO_mobile_PassportLoginClient : System.Web.UI.Page
{
    #region 天翼账号登录变量
    private String UDBLoginURL = String.Empty;
    private String UDBReturnURL = String.Empty;
    private String UdbSrcSsDeviceNo = String.Empty;
    private String UdbKey = String.Empty;
    private static readonly ILog logger = LogManager.GetLogger(typeof(SSO_mobile_PassportLoginClient));
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
        ParseSPTokenRequest();
        CreateUdbPassportLoginRequest();
        //重定向到wap登录页面
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
        //login189.NavigateUrl = UDBLoginURL + "?PassportLoginRequest=" + passportLoginRequestValue;
        //login189.Target = System.Configuration.ConfigurationManager.AppSettings["YgLoginTargetURL"];
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

    protected void ImageBtnLogin_Click(object sender, ImageClickEventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        string AuthenType = "1";  // 默认是用户名
        string AuthenName = this.username.Text;
        string Password = this.password.Text;

        string CustID = "";
        string RealName = "";
        string NickName = "";
        string UserName = "";
        string OutID = "";
        string UserAccount = "";
        string ErrMsg = "";
        string CustType = "";
        string ProvinceID = "";

        if (String.IsNullOrEmpty(AuthenName))
        {
            this.errorHint.InnerText = "用户名不能为空!" + ErrMsg;
            return;
        }

        if (String.IsNullOrEmpty(Password))
        {
            this.errorHint.InnerText = "密码不能为空!" + ErrMsg;
            return;
        }

        Regex regMobile = new Regex(@"^1[3458]\d{9}$");
        Regex regEmail = new Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
        Regex regCard = new Regex(@"^(\d{9}|\d{16})$");

        if (regMobile.IsMatch(AuthenName))
        {
            AuthenType = "2";
        }
        if (regEmail.IsMatch(AuthenName))
        {
            AuthenType = "4";
        }
        if (regCard.IsMatch(AuthenName))
        {
            AuthenType = "3";
        }




        Result = UserAuthV2(SPID, AuthenName, AuthenType, Password, Context, ProvinceID, "", "",
         out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  ProvinceID, out  RealName, out  UserName, out  NickName);
        //验证结果日志


        if (Result == 0)
        {

            if (RealName != null && !"".Equals(RealName))
            {
                welcomeName = RealName;
            }
            else if (NickName != null && !"".Equals(NickName))
            {
                welcomeName = NickName;
            }
            else if (UserName != null && !"".Equals(UserName))
            {
                welcomeName = UserName;
            }

            if (ReturnURL.IndexOf("?") > 0)
            {
                ReturnURL = ReturnURL + "&CustID=" + CustID + "&welcomeName=" + welcomeName;

            }
            else
            {
                ReturnURL = ReturnURL + "?CustID=" + CustID + "&welcomeName=" + welcomeName;
            }
            strLog.AppendFormat("重定向地址:{0}\r\n", ReturnURL);
            log(strLog.ToString());
            Response.Redirect(ReturnURL, true);
            return;
        }
        else
        {
            strLog.AppendFormat("认证失败!" + ErrMsg);
            this.errorHint.InnerText = "认证失败" + ErrMsg;
            log(strLog.ToString());
            return;
        }
    }
    
}

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
using Linkage.BestTone.Interface.BTException;
using System.Data.SqlClient;
public partial class yiqigou_login : System.Web.UI.Page
{

    public string SPTokenRequest="";
    public string SPID ="";
    public string UAProvinceID ="";
    public string SourceType = "";
    public string ReturnURL = "";
    public string ErrMsg="";
    public int Result = 0;

    public StringBuilder strLog = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");

        ParseSPTokenRequest();

        string NeedLogin = Request["NeedLogin"];
        if ("0".Equals(NeedLogin))
        {
            //已登录流程
            TokenValidate.IsRedircet = false;
            TokenValidate.Validate();
            if (TokenValidate.Result == 0)
            {
                this.ssoFunc();
            }
            else if (CommonUtility.IsParameterExist("NeedLogin", this.Page))
            {
                //不需要客户认证平台登陆
                if (!"0".Equals(Request["NeedLogin"]))
                {
                    Response.Redirect(ReturnURL + "?NeedLogin=1");
                    //Response.Redirect("yiqigou_login.aspx");
                }
            }
        }
        else
        { 
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
            log(String.Format("ssoFunc: TokenValidate.RealName:{0},TokenValidate.NickName:{1},TokenValidate.UserName:{2},TokenValidate.LoginAuthenName:{3},TokenValidate.LoginAuthenType:{4}", TokenValidate.RealName, TokenValidate.NickName, TokenValidate.UserName, TokenValidate.LoginAuthenName, TokenValidate.LoginAuthenType));
            String er = "";
            Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, UserName, NickName, OutID, "", LoginAuthenName, LoginAuthenType, out er);

            if (Result != 0)
            {
                errorHint.InnerHtml = er;
                return;
            }

            if (ReturnURL.IndexOf("?") > 0)
            {
                Url = ReturnURL + "&Ticket=" + Ticket;
            }
            else
            {
                Url = ReturnURL + "?Ticket=" + Ticket;
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


    /// <summary>
    /// 判断并解析SPTokenRequest参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            
            if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
            {
                SPTokenRequest = Request["SPTokenRequest"];
                //日志
                strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
                //解析请求参数
                Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnURL, out ErrMsg);
                //日志
                strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}\r\n", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnURL));

                if (Result != 0)
                {
                    errorHint.InnerHtml = "请求参数SPTokenRequest不正确";
                }

                if (!CommonUtility.ValidateUrl(ReturnURL.Trim()))
                {
                    errorHint.InnerHtml = "请求参数ReturnURL不正确";
                }
            }
        }
        catch (System.Exception ex)
        {
            strLog.AppendFormat(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }

    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("YiqiGou_login", msg);
    }


    protected int UserAuthV2(string SPID, string AuthenName, string AuthenType, string Password, HttpContext Context, string ProvinceID, string IsQuery, string PwdType, out string ErrMsg, out string CustID, out string UserAccount, out string CustType, out string outerid, out string ProvinceID1, out  string RealName, out string UserName, out string NickName)
    {
        strLog.AppendFormat("UserAuthV2-AuthenType:{0},AuthenName:{1},Password:{2}\r\n", AuthenType,AuthenName,Password);
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

        }
        catch (Exception ex)
        {

        }
        finally
        { 
        
        }
        return Result;
    }

    protected void Submit1_Click(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");
        string AuthenType = HttpUtility.HtmlDecode(Request.Form["AuthenType"].ToString().Trim().ToUpper());
        strLog.AppendFormat("AuthenType:{0}\r\n",AuthenType);
        PageUtility.SetCookie("AuthenType", AuthenType, 168);           //168个小时，即一个礼拜
        string AuthenName = Request.Form["username"];
        string Password = Request.Form["password"];
        string CustID = "";
        string RealName = "";
        string NickName = "";
        string UserName = "";
        string OutID = "";
        string UserAccount = "";
        string CustType = "";
        string ProvinceID = UAProvinceID;

        try {
            strLog.AppendFormat("checkCode={0}", Request.Form["checkCode"]);
            if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["checkCode"].ToString().Trim()), this.Context))
            {
                errorHint.InnerHtml = "验证码错误，请重新输入";
                return;
            }
            strLog.Append("验证码校验通过\r\n");
            strLog.AppendFormat("【开始验证】:SPID:{0},ProvinceID:{1},AuthenName:{2},AuthenType:{3}\r\n", SPID, ProvinceID, AuthenName, AuthenType);
            Result = UserAuthV2(SPID, AuthenName, AuthenType, Password, Context, ProvinceID, "", "",
                out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  ProvinceID, out  RealName, out  UserName, out  NickName);
            //验证结果日志
            strLog.AppendFormat("【验证结果】:CustID:{0},UserAcount:{1},CustType:{2},OutID:{3},ProvinceID:{4},RealName:{5},UserName:{6},NickName:{7},Result:{8},ErrMsg:{9}\r\n",
                CustID, UserAccount, CustType, OutID, ProvinceID, RealName, UserName, NickName, Result, ErrMsg);
            CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, ProvinceID, AuthenType, AuthenName, "2", Result, ErrMsg);
            if (Result != 0)
            {
                if (Result == 1001 || Result == -20504 || Result == -21553)
                {
                    errorHint.InnerHtml = ErrMsg;
                    //hint_Username.InnerHtml = "";
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
        catch (System.Exception ex)
        {
            strLog.AppendFormat(ex.ToString()+"\r\n");
        }finally{
            log(strLog.ToString());
        }
    }
}

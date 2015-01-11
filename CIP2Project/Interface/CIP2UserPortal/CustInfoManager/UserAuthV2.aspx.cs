using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Data.SqlClient;
using log4net;


public partial class CustInfoManager_UserAuthV2 : System.Web.UI.Page
{
    public String SPID;
    /// <summary>
    /// parameters in 
    /// </summary>
    public String AuthenName;
    public String AuthenType;
    public String Password;
    public String PwdType;
    
    /// <summary>
    /// parameters out
    /// </summary>
    public Int32 Result;
    public String ErrMsg;
    public String CustID;
    public String UserAccount;
    public String CustType;
    public String UProvinceID;
    public String SysID;
    public String AreaID;
    public String outerid;
    public String UserName;
    public String RealName;
    public String NickName;
    public String CertificateCode;
    public String Email;
    public String wt;

    private static readonly ILog logger = LogManager.GetLogger(typeof(CustInfoManager_UserAuthV2));

    /// <summary>
    /// AuthenType 
    /// 1.用户名
    /// 2.认证手机
    /// 3.商旅卡号
    /// 4. Email（商旅卡用户）
    /// </summary>
    /// <returns></returns>
    protected String UserAuthV2()
    { 
        StringBuilder ResponseMsg = new StringBuilder();
        SPID = Request["SPID"];
        AuthenName = Request["AuthenName"];
        AuthenType = Request["AuthenType"];
        Password = Request["Password"];
        PwdType = Request["PwdType"];

        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;

        wt = Request["wt"];   // json or xml

        #region 数据校验
        if (CommonUtility.IsEmpty(SPID))
        {
            Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";

            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(AuthenName))
        {
            Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";

            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AuthenName不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AuthenName不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(AuthenType))
        {
            Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";

            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AuthenType不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AuthenType不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();

        }

        if (CommonUtility.IsEmpty(Password))
        {
            Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";

            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "Password不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "Password不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        #endregion

        #region    认证
        try
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 15;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_Customer_OV3_Interface_UserAuthForClient";

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
            parPwdType.Value = "1";
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


            SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
            parCertificateCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parCertificateCode);

            SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 20);
            parEmail.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parEmail);

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
            RealName = parRealName.Value.ToString();
            UserName = parUserName.Value.ToString();
            NickName = parNickName.Value.ToString();
            CertificateCode = parCertificateCode.Value.ToString();
            Email = parEmail.Value.ToString();

            if (Result == 0)
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                    ResponseMsg.AppendFormat("\"UserAccount\":\"{0}\",", UserAccount);
                    ResponseMsg.AppendFormat("\"CustType\":\"{0}\",", CustType);
                    ResponseMsg.AppendFormat("\"UProvinceID\":\"{0}\",", UProvinceID);
                    ResponseMsg.AppendFormat("\"SysID\":\"{0}\",", SysID);
                    ResponseMsg.AppendFormat("\"AreaID\":\"{0}\",", AreaID);
                    ResponseMsg.AppendFormat("\"outerid\":\"{0}\",", outerid);
                    ResponseMsg.AppendFormat("\"RealName\":\"{0}\",", RealName);
                    ResponseMsg.AppendFormat("\"UserName\":\"{0}\",", UserName);
                    ResponseMsg.AppendFormat("\"CertificateCode\":\"{0}\",", CertificateCode);
                    ResponseMsg.AppendFormat("\"Email\":\"{0}\",", Email);
                    ResponseMsg.AppendFormat("\"NickName\":\"{0}\"", NickName);
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");

                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
            else
            {
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ErrMsg);
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", Result);
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ErrMsg);
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
            }

        }
        catch (Exception exp)
        {
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-2508");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", exp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-2508");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", exp.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }

        }
        finally
        {
            
        }
        #endregion
        
        return ResponseMsg.ToString();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        String ResponseText = UserAuthV2();
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("mobile-UserAuthV2", msg);
    }


    protected int UserAuthV2(string SPID, string AuthenName, string AuthenType, string Password, HttpContext Context, string ProvinceID, string IsQuery, string PwdType, out string ErrMsg, out string CustID, out string UserAccount, out string CustType, out string outerid, out string ProvinceID1, out  string RealName, out string UserName, out string NickName)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("UserAuthV2-AuthenType:{0},AuthenName:{1},Password:{2}\r\n", AuthenType, AuthenName, Password);
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
            strLog.AppendFormat(ex.ToString());

        }
        finally
        {
            log(strLog.ToString());
        }
        return Result;
    }

}

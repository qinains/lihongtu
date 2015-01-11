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

public partial class CustInfoManager_UserRegisterV2 : System.Web.UI.Page
{
    public String SPID;
    public String CheckPhoneCode;
    /// <summary>
    /// parameters in 
    /// </summary>
 	public String UserType ;
 	public String Password ;
 	public String ProvinceID;
 	public String AreaCode ;
 	public String RealName ;
 	public String UserName ;
 	public String AuthenPhone ;
    public String Email;
 	public String CertificateCode ;
 	public String CertificateType ;
 	public String Sex ;
    public String wt;
    public String Device;
    /// <summary>
    /// parameters out
    /// </summary>
    public Int32 Result;
    public String ErrMsg;
    public String CustID;



    private static readonly ILog logger = LogManager.GetLogger(typeof(CustInfoManager_UserRegisterV2));


    public String UserRegisterClient(String SPID, String AuthenPhone, String Password, String Email, String Sex, String RealName,String Device)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        #region 数据校验

        if (CommonUtility.IsEmpty(SPID))
        {
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

        if (CommonUtility.IsEmpty(AuthenPhone))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AuthenPhone不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AuthenPhone不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        Result = PhoneBO.SelSendSMSMassage("", AuthenPhone, CheckPhoneCode, out ErrMsg);
        if (Result != 0)
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "手机验证码验证错误！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "手机验证码验证错误！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();

        }

        String T_CustID = String.Empty;

        if (!CommonBizRules.HasBesttoneAccount(this.Context, AuthenPhone, out T_CustID, out ErrMsg))
        {
            if (!String.IsNullOrEmpty(T_CustID))
            {

                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "990");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", AuthenPhone+"该手机号码已经被别的客户作为号码百事通账户！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "990");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", AuthenPhone+"该手机号码已经被别的客户作为号码百事通账户！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();

            }

        }
        #endregion

        #region   开始注册

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 15;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_Customer_V3_Interface_UserRegistryClient";   // 不带RegistrationSouce参数
            //cmd.CommandText = "dbo.up_Customer_V3_Interface_UserRegistryClientV2";   // 带RegistrationSouce参数  2.0后做

            int RegistrationSouce = 2; // 默认是web注册

            if ("android".Equals(Device))
            {
                RegistrationSouce = 11;  // 客户端 且 android
            }

            if ("ios".Equals(Device))
            {
                RegistrationSouce = 12;  // 客户端 且 ios
            }

            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
            parUserType.Value = "42";
            cmd.Parameters.Add(parUserType);

            SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(Password))
            {
                parPassword.Value = CryptographyUtil.Encrypt("111111");
            }
            else
            {
                parPassword.Value = CryptographyUtil.Encrypt(Password);           
            }
            cmd.Parameters.Add(parPassword);

           
            SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
            parUProvinceID.Value = "02";
            cmd.Parameters.Add(parUProvinceID);

            SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
            parAreaCode.Value = "021";
            cmd.Parameters.Add(parAreaCode);

            SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty("RealName"))
            {
                parRealName.Value = "";
            }
            else
            {
                parRealName.Value = RealName;
            }
            cmd.Parameters.Add(parRealName);

            SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(UserName))
            {
                parUserName.Value = "";
            }
            else
            {
                parUserName.Value = UserName;
            }
            cmd.Parameters.Add(parUserName);

            SqlParameter parAuthenPhone = new SqlParameter("@AuthenPhone", SqlDbType.VarChar, 15);
            parAuthenPhone.Value = AuthenPhone;
            cmd.Parameters.Add(parAuthenPhone);

            SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 20);
            if (String.IsNullOrEmpty(Email))
            {
                parEmail.Value = Email;
            }
            else
            {
                parEmail.Value = "";
            
            }
            cmd.Parameters.Add(parEmail);


            //SqlParameter parRegistrationSource = new SqlParameter("@RegistrationSource", SqlDbType.Int);
            //parRegistrationSource.Value = RegistrationSouce;
            //cmd.Parameters.Add(parRegistrationSource);

            SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
            if (String.IsNullOrEmpty(Sex))
            {
                parSex.Value = "2";
            }
            else
            {
                parSex.Value = Sex;
            }
           cmd.Parameters.Add(parSex);

            SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
            parResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parResult);

            SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
            parErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parErrMsg);

            SqlParameter paroCustID = new SqlParameter("@oCustID ", SqlDbType.VarChar, 16);
            paroCustID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paroCustID);

            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            Result = int.Parse(parResult.Value.ToString());
            ErrMsg = parErrMsg.Value.ToString();
            CustID = paroCustID.Value.ToString();

            ResponseMsg.Length = 0;
            if (Result == 0)
            {
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "注册成功！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "注册成功！");
                    ResponseMsg.AppendFormat("<CustID>{0}</CustID>", CustID);
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
            else
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-2508");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ErrMsg);
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-2508");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ErrMsg);
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
            }

        }
        catch (Exception exp)
        {
            ResponseMsg.Length = 0;
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
        #endregion



        return ResponseMsg.ToString();
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        AuthenPhone = Request["AuthenPhone"];
        CheckPhoneCode = Request["CheckPhoneCode"];
        Password = Request["PassWord"];
        Email = Request["Email"];
        Sex = Request["Sex"];
        RealName = Request["RealName"];
        wt = Request["wt"];
        Device = Request["Device"];
        if (String.IsNullOrEmpty(Device))
        {
            Device = "android";
        }
        String ResponseText = UserRegisterClient(SPID, AuthenPhone, Password, Email, Sex, RealName,Device);

        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }
}

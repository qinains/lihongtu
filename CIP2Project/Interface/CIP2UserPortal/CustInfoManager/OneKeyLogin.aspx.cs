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


public partial class CustInfoManager_OneKeyLogin : System.Web.UI.Page
{
    public String SPID;
    public String imsi;
    public String Phone;
    public String wt;


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

    private static readonly ILog logger = LogManager.GetLogger(typeof(CustInfoManager_OneKeyLogin));
    
    public String OneKeyLogin(String imsi)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;

        wt = Request["wt"];   // json or xml
        

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

        Phone = getMobileSelfReg(imsi);

        if (CommonUtility.IsEmpty(Phone))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", imsi+":根据imsi号查手机号码失败！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", imsi+":根据imsi号查手机号码失败！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

       


        #endregion

        #region  开始认证
        try
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 15;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_Customer_V3_Interface_IsAuthenPhone";

            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 20);
            parPhone.Value = Phone;
            cmd.Parameters.Add(parPhone);

            SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
            parResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parResult);

            SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
            parErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parErrMsg);

            SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
            parCustID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parCustID);

            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            Result = int.Parse(parResult.Value.ToString());
            ErrMsg = parErrMsg.Value.ToString();
            CustID = parCustID.Value.ToString();


            if (!String.IsNullOrEmpty(CustID))  // 代表已经注册过，并且有认证手机 
            {
                // 根据CustID查询客户信息并返回
                String OuterID = "";
                String Status = "";
                String CustLevel = "";
                String CertificateType = "";
                String Sex = "";
                String EnterpriseID = "";
                String ProvinceID = "";
                String Registration = "";
                try
                {
                    Result = CustBasicInfo.getCustInfo(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType,
                     out CustLevel, out RealName, out UserName, out NickName, out CertificateCode,
                     out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID, out AreaID, out Registration);

                    int QueryResult = -1;
                    PhoneRecord[] PhoneRecords = CustBasicInfo.getPhoneRecord(CustID, out QueryResult, out ErrMsg);
                    ResponseMsg.Length = 0;
                    if (Result == 0)
                    {
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                            ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                            ResponseMsg.AppendFormat("\"Status\":\"{0}\",", Status);
                            ResponseMsg.AppendFormat("\"CustType\":\"{0}\",", CustType);
                            ResponseMsg.AppendFormat("\"CustLevel\":\"{0}\",", CustLevel);
                            ResponseMsg.AppendFormat("\"RealName\":\"{0}\",", RealName);
                            ResponseMsg.AppendFormat("\"UserName\":\"{0}\",", UserName);
                            ResponseMsg.AppendFormat("\"NickName\":\"{0}\",", NickName);
                            ResponseMsg.AppendFormat("\"CertificateCode\":\"{0}\",", CertificateCode);
                            ResponseMsg.AppendFormat("\"CertificateType\":\"{0}\",", CertificateType);
                            ResponseMsg.AppendFormat("\"Sex\":\"{0}\",", Sex);
                            ResponseMsg.AppendFormat("\"Email\":\"{0}\",", Email);
                            ResponseMsg.AppendFormat("\"EnterpriseID\":\"{0}\",", EnterpriseID);
                            ResponseMsg.AppendFormat("\"ProvinceID\":\"{0}\",", ProvinceID);
                            ResponseMsg.AppendFormat("\"AreaID\":\"{0}\",", AreaID);
                            if (QueryResult == 0 && PhoneRecords != null && Registration.Length > 0)
                            {
                                ResponseMsg.AppendFormat("\"Phone\":\"{0}\",", PhoneRecords[0].Phone);
                                ResponseMsg.AppendFormat("\"PhoneClass\":\"{0}\",", PhoneRecords[0].PhoneClass);
                            }
                            ResponseMsg.AppendFormat("\"Registration\":\"{0}\"", Registration);
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
                }
                catch (Exception ept)
                {
                    // 返回错误信息
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "990");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ept.ToString());
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "990");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ept.ToString());
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                }
            }
            else // 没有注册过，或者注册过，但未有认证电话 ，因无法根据电话号码是否为认证电话判断客户是否注册过，因此就帮他注册一个（有可能这个用户有用户名但是没有认证手机）
            {

                String T_CustID = String.Empty;
                if (!CommonBizRules.HasBesttoneAccount(this.Context, Phone, out T_CustID, out ErrMsg))
                {
                    if (!String.IsNullOrEmpty(T_CustID))
                    {

                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "990");
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", Phone + "该手机号码已经被别的客户作为号码百事通账户！");
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "990");
                            ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", Phone + "该手机号码已经被别的客户作为号码百事通账户！");
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                        return ResponseMsg.ToString();
                    }
                }

                #region  开始注册
                try
                {
                    cmd = new SqlCommand();
                    cmd.CommandTimeout = 15;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.up_Customer_V3_Interface_UserRegistryClient";

                    parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                    parSPID.Value = SPID;
                    cmd.Parameters.Add(parSPID);

                    SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                    parUserType.Value = "42";
                    cmd.Parameters.Add(parUserType);

                    SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 50);
                    parPassword.Value = CryptographyUtil.Encrypt("111111");
                    cmd.Parameters.Add(parPassword);

                    SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                    parUProvinceID.Value = "02";
                    cmd.Parameters.Add(parUProvinceID);

                    SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                    parAreaCode.Value = "021";
                    cmd.Parameters.Add(parAreaCode);

                    SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                    parRealName.Value = "";
                    cmd.Parameters.Add(parRealName);

                    SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                    parUserName.Value = "";
                    cmd.Parameters.Add(parUserName);

                    SqlParameter parAuthenPhone = new SqlParameter("@AuthenPhone", SqlDbType.VarChar, 15);
                    parAuthenPhone.Value = Phone;
                    cmd.Parameters.Add(parAuthenPhone);

                    SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 20);
                    parEmail.Value = "";
                    cmd.Parameters.Add(parEmail);


                    SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                    parSex.Value = "2";
                    cmd.Parameters.Add(parSex);

                    parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    SqlParameter paroCustID = new SqlParameter("@oCustID ", SqlDbType.VarChar, 16);
                    paroCustID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paroCustID);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                    Result = int.Parse(parResult.Value.ToString());
                    ErrMsg = parErrMsg.Value.ToString();
                    CustID = paroCustID.Value.ToString();

                    if (Result == 0)    //注册成功
                    {
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                            ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                            ResponseMsg.AppendFormat("\"Status\":\"{0}\",", "00");
                            ResponseMsg.AppendFormat("\"CustType\":\"{0}\",", "42");
                            ResponseMsg.AppendFormat("\"CustLevel\":\"{0}\",", "3");
                            ResponseMsg.AppendFormat("\"RealName\":\"{0}\",", "");
                            ResponseMsg.AppendFormat("\"UserName\":\"{0}\",", "");
                            ResponseMsg.AppendFormat("\"NickName\":\"{0}\",", "");
                            ResponseMsg.AppendFormat("\"CertificateCode\":\"{0}\",", "");
                            ResponseMsg.AppendFormat("\"CertificateType\":\"{0}\",", "");
                            ResponseMsg.AppendFormat("\"Sex\":\"{0}\",", "2");
                            ResponseMsg.AppendFormat("\"Email\":\"{0}\",", "");
                            ResponseMsg.AppendFormat("\"EnterpriseID\":\"{0}\",", "");
                            ResponseMsg.AppendFormat("\"ProvinceID\":\"{0}\",", "02");
                            ResponseMsg.AppendFormat("\"AreaID\":\"{0}\",", "021");
                            ResponseMsg.AppendFormat("\"Phone\":\"{0}\",",Phone);
                            ResponseMsg.AppendFormat("\"Registration\":\"{0}\"", "");
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
                    else    //注册失败
                    {
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "993");
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "注册失败！");
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "993");
                            ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "注册失败!");
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                        return ResponseMsg.ToString();
                    }

                }
                catch (Exception ecp)
                {

                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "993");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ecp.ToString());
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "993");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ecp.ToString());
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                }
                #endregion

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
        #endregion

        return ResponseMsg.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        imsi = Request["imsi"];

        String ResponseText = OneKeyLogin(imsi);
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
        BTUCenterInterfaceLog.CenterForBizTourLog("mobile-client-onekeylogin", msg);
    }


    public String getMD5Str(String source)
    {
        String Md5_digest = String.Empty;
        byte[] md5bytes = CryptographyUtil.MD5Encrypt(source);
        Md5_digest = CryptographyUtil.byteToHexStr(md5bytes);
        return Md5_digest;
    }

    public String getMobileSelfReg(String imsi)
    { 
        String mobile = String.Empty;

        StringBuilder requestXml = new StringBuilder();
        String responseXml = String.Empty;

        #region 拼接请求xml字符串

        String appKey = System.Configuration.ConfigurationManager.AppSettings["BesttoneOpenApi_appKey"];  //  "ED150A183B8DE9A3E040A8C030B452AD";
        String appSecret = System.Configuration.ConfigurationManager.AppSettings["BesttoneOpenApi_appSecret"]; //"ED150A183B8EE9A3E040A8C030B452AD";
        String apiName = System.Configuration.ConfigurationManager.AppSettings["BesttoneOpenApi_apiName"];  //"mobile";
        String apiMethod = System.Configuration.ConfigurationManager.AppSettings["BesttoneOpenApi_apiMethod"];  //"getMobileSelfReg";

        if (String.IsNullOrEmpty(appKey))
        {
            appKey = "ED150A183B8DE9A3E040A8C030B452AD";
        }

        if (String.IsNullOrEmpty(appSecret))
        {
            appSecret = "ED150A183B8EE9A3E040A8C030B452AD";
        }

        if (String.IsNullOrEmpty(apiName))
        {
            apiName = "mobile";
        }

        if (String.IsNullOrEmpty(apiMethod))
        {
            apiMethod = "getMobileSelfReg";
        }

        requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        requestXml.Append("<reqXML version=\"1.0\">");
        //添加参数
        requestXml.AppendFormat("<appKey>{0}</appKey>", appKey);
        requestXml.AppendFormat("<apiName>{0}</apiName>", apiName);
        requestXml.AppendFormat("<apiMethod>{0}</apiMethod>", apiMethod);
        requestXml.AppendFormat("<timestamp>{0}</timestamp>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        requestXml.Append("<params>");
        requestXml.AppendFormat("<param  name=\"imsi\"  value='{0}' />", imsi);           //手机IMSI号
        requestXml.Append("</params>");
        requestXml.Append("</reqXML>");
        #endregion

        #region  提交请求 
        String serverAddress = System.Configuration.ConfigurationManager.AppSettings["BesttoneOpenApiURL"];  //      "http://open.118114.cn/";
        if (String.IsNullOrEmpty(serverAddress))
        {
            serverAddress = "http://open.118114.cn/";
        }
        
        String url = serverAddress + "api?reqXml=" + HttpUtility.UrlEncode(requestXml.ToString(), Encoding.UTF8) + "&sign=" + getMD5Str(requestXml.ToString() + appSecret);
        responseXml = HttpMethods.HttpGet(url);
        #endregion

        #region 解析返回报文并取出手机号码
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(responseXml);
        XmlNode dataNode = xmlDoc.SelectNodes("/data")[0];

        mobile = String.Empty;
        if (dataNode != null)
        {
            mobile = dataNode.Attributes["mobile"].Value;
        }
       

        XmlNode errorCodeNode = xmlDoc.SelectNodes("/error/errorCode")[0];
        XmlNode errorMessageNode = xmlDoc.SelectNodes("/error/errorMessage")[0];

        if (errorCodeNode != null)
        {
            mobile = "";
        }

        #endregion
        return mobile;
    }

}

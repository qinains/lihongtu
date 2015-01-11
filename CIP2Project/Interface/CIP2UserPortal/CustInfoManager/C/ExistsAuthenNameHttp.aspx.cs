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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using System.Data.SqlClient;
using log4net;

public partial class CustInfoManager_C_ExistsAuthenNameHttp : System.Web.UI.Page
{
    public Int32 Result;
    public String ErrMsg;
    public String SPID;
    public String CustID;
    public String AuthenName;
    public String wt;

    protected void Page_Load(object sender, EventArgs e)
    {

        SPID = Request["SPID"];
        AuthenName = Request["AuthenName"];
        wt = Request["wt"];
        String ResponseText = String.Empty;
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        ResponseText = ExistsAuthenName(SPID,AuthenName,wt);
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }

    public String ExistsAuthenName(String SPID, String AuthenName,String wt)
    {
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
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

        if (CommonUtility.IsEmpty(AuthenName))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AuthenName不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AuthenName不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        String AuthenType = "1";
        Regex regUserName = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*([-._]?[a-zA-Z0-9]+)*$");
        Regex regMobile = new Regex(@"^1[3458]\d{9}$");
        Regex regEmail = new Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
        Regex regCard = new Regex(@"^(\d{9}|\d{16})$");

        if (regUserName.IsMatch(AuthenName))
        {
            AuthenType = "1";
        }
    
        if (regMobile.IsMatch(AuthenName))
        {
            AuthenType = "2";
        }
        if (regCard.IsMatch(AuthenName))
        {
            AuthenType = "3";
        }
        if (regEmail.IsMatch(AuthenName))
        {
            AuthenType = "4";
        }

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 15;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_Customer_V3_Interface_IsExistsAuthenName";

            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
            parAuthenName.Value = AuthenName;
            cmd.Parameters.Add(parAuthenName);

            SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 1);
            parAuthenType.Value = AuthenType;
            cmd.Parameters.Add(parAuthenType);

            SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
            parCustID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parCustID);

            SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
            parResult.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parResult);

            SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
            parErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parErrMsg);

            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            Result = int.Parse(parResult.Value.ToString());
            ErrMsg = parErrMsg.Value.ToString();
            CustID = parCustID.Value.ToString();

           
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                ResponseMsg.AppendFormat("\"CustID\":\"{0}\"", CustID);
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<errcode>{0}</errcode>", Result);
                ResponseMsg.AppendFormat("<errmsg>{0}</errmsg>", ErrMsg);
                ResponseMsg.AppendFormat("<CustID>{0}</CustID>", ErrMsg);
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
            


        }
        catch (Exception ecp)
        {

            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-7756");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ecp.ToString());
             
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<errcode>{0}</errcode>", "-7756");
                ResponseMsg.AppendFormat("<errmsg>{0}</errmsg>", ecp.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        return ResponseMsg.ToString();
    }



}

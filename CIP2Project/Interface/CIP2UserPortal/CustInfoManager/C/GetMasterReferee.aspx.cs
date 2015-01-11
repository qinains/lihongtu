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

public partial class CustInfoManager_C_GetMasterReferee : System.Web.UI.Page
{
    public String SPID;
    public String CustID;
    public String Phone;
    public Int32 Result;
    public String ErrMsg;
    public String wt;
    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        CustID = Request["CustID"];
        wt = Request["wt"];
        String ResponseText = GetMasterReferee(SPID, CustID);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }

    public String GetMasterReferee(String SPID, String CustID)
    {
        StringBuilder ResponseMsg = new StringBuilder();
        if (CommonUtility.IsEmpty(SPID))
        {

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
        if (CommonUtility.IsEmpty(CustID))
        {

            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        String m_custid = String.Empty;


        try
        {
            SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select distinct b.custid from set_sharecode a ,get_sharecode b where a.sharecode = b.sharecode and a.t_custid='{0}'",CustID);
            SqlCommand cmd = new SqlCommand(sql.ToString(),conn);
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    m_custid = (String)reader["custid"];                
                }
            }


            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", "");
                ResponseMsg.AppendFormat("\"custid\":\"{0}\"", m_custid);
                ResponseMsg.Append("}");
            }
            else
            {
                
            }
            return ResponseMsg.ToString();


        }
        catch (Exception e)
        {
                ResponseMsg.Length = 0;
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-7756");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", e.ToString());
                ResponseMsg.Append("}");
        }
        return ResponseMsg.ToString();
    }

}

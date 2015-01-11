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

public partial class CustInfoManager_C_GetReferee : System.Web.UI.Page
{

    public String SPID;
    public String CustID;
    public String Phone;
    public Int32 Result;
    public String ErrMsg;
    public String wt;


    class Customer {
        public String ShareCode;
        public String T_CustID;
        public String UserName;
        public String NickName;
        public String RealName;
        public String Phone;
        public String CustID;
        public String CreateTime;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        CustID = Request["CustID"];
        wt = Request["wt"];
        String ResponseText = GetReferee(SPID, CustID);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }

    public String GetReferee(String SPID,String CustID)
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


        System.Collections.Generic.List<Customer> result = new System.Collections.Generic.List<Customer>();

        SqlConnection myCon = null;
        SqlCommand cmd = new SqlCommand();
        try
        {
            //ref_custid 推荐人custid  ,  bef_custid  被推荐人custid
            myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            cmd.Connection = myCon;
            cmd.CommandTimeout = 15;
            StringBuilder sql = new StringBuilder();
            sql.Append("select a.sharecode,a.t_custid,a.username,a.nickname,");
            sql.Append("a.realname,b.phone,a.registerTime as createtime,c.custid from  ");
            sql.Append("(select a.sharecode,a.t_custid, convert(varchar(10),a.registerTime,120) as registerTime, b.username,b.nickname,b.realname ");
            sql.Append("from set_sharecode a left join custinfo b on a.t_custid = b.custid ) a , ");
            sql.Append("(select a.sharecode,a.t_custid,case when b.phone is null then '' else b.phone end phone  ");
            sql.Append("from set_sharecode a left join custphone b on a.t_custid = b.custid ) b ,get_sharecode c ");
            sql.AppendFormat("where a.t_custid = b.t_custid and a.sharecode = c.sharecode and c.custid = '{0}'",CustID);

            
            
            cmd.CommandText =sql.ToString();
            
            using (myCon)
            {
                myCon.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customer c = new Customer();
                    c.T_CustID = (String)reader["t_custid"];
                    c.UserName = (String)reader["username"];
                    c.NickName = (String)reader["nickname"];
                    c.Phone = (String)reader["phone"];
                    c.RealName = (String)reader["realname"];
                    c.CreateTime = (String)reader["createtime"];
                    c.ShareCode = (String)reader["sharecode"];
                    result.Add(c);
                }
            }

            if (result.Count > 0)
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                    ResponseMsg.Append("\"Customers\":");
                    ResponseMsg.Append("[");
                    int x = 0;
                    foreach (Customer c in result)
                    {
                        x++;
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"T_CustID\":\"{0}\",", c.T_CustID);
                        ResponseMsg.AppendFormat("\"UserName\":\"{0}\",", c.UserName);
                        ResponseMsg.AppendFormat("\"NickName\":\"{0}\",", c.NickName);
                        ResponseMsg.AppendFormat("\"Phone\":\"{0}\",", c.Phone);
                        ResponseMsg.AppendFormat("\"RealName\":\"{0}\",", c.RealName);
                        ResponseMsg.AppendFormat("\"CreateTime\":\"{0}\"", c.CreateTime);
                        if (x == result.Count)
                        {
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("},");
                        }

                    }
                    ResponseMsg.Append("]");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<errcode>{0}</errcode>", Result);
                    ResponseMsg.AppendFormat("<errmsg>{0}</errmsg>", ErrMsg);
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
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                    ResponseMsg.Append("\"Customers\":");
                    ResponseMsg.Append("[");
                    ResponseMsg.Append("]");
                    ResponseMsg.Append("}");
                }
            }
        }
        catch (Exception e)
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-7756");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", e.ToString());

                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<errcode>{0}</errcode>", "-7756");
                ResponseMsg.AppendFormat("<errmsg>{0}</errmsg>", e.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        return ResponseMsg.ToString();
    }

}

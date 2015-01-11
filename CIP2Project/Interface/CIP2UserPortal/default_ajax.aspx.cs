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
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Data.SqlClient;
public partial class default_ajax : System.Web.UI.Page
{
    public string k;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString())==1)
            {
                add();
                bdd();
            }
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 2)
            {
             
                bdd();
            }
        }
    }

    public void add()
    {
        string htmlpid = HttpUtility.HtmlDecode(Request.QueryString["pid"].ToString());
        for (int i = 0; i < selCity(htmlpid).Tables[0].Rows.Count; i++)
        {
            string prname = selCity(htmlpid).Tables[0].Rows[i]["areaName"].ToString() ;
            string prid = selCity(htmlpid).Tables[0].Rows[i]["areaid"].ToString() ;
            string[] str = new string[] { prname + "count" + selCity(htmlpid).Tables[0].Rows.Count+"count"};
            Response.Write(str.GetValue(0));
        }
       
    }

    public void bdd()
    {
        string htmlpid = HttpUtility.HtmlDecode(Request.QueryString["pid"].ToString());
        for (int i = 0; i < selCity(htmlpid).Tables[0].Rows.Count; i++)
        {
            string prname = selCity(htmlpid).Tables[0].Rows[i]["areaName"].ToString();
            string prid = selCity(htmlpid).Tables[0].Rows[i]["areaid"].ToString();
            string[] str = new string[] { prid + "count" + selCity(htmlpid).Tables[0].Rows.Count + "count" };
            Response.Write(str.GetValue(0));
        }
    }


    #region 城市查询

    public static DataSet selCity(string provinceid)
    {
        string Msg = "";
        SqlConnection myCon = null;
        SqlCommand cmd = new SqlCommand();
        string SqlResult = "";
        DataSet ds = new DataSet();
        try
        {

            myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_SelCityByPid";

            SqlParameter parProvinceid = new SqlParameter("provinceid", SqlDbType.VarChar, 2);
            parProvinceid.Value = provinceid;
            cmd.Parameters.Add(parProvinceid);

            ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

        }
        catch (Exception e)
        {
            SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code.ToString();
            Msg = e.Message;
        }
        return ds;

        
    }
    #endregion
}

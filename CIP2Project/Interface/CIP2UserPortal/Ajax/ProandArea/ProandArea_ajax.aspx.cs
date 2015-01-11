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
using System.Data.SqlClient;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;

/// <summary>
// *     描述: 省市联动AJAX处理页面
// * 开发平台: Windows XP + Microsoft SQL Server 2005
// * 开发语言: C#
// * 开发工具: Microsoft Visual Studio.Net 2005
// *     作者: 赵锐
// * 联系方式: 
// *     公司: 联创科技(南京)股份有限公司
// * 创建日期: 2009-07-31
/// </summary>

public partial class Ajax_ProandArea_ProandArea_ajax : System.Web.UI.Page
{
    public string k;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 1)
            {
                cityName();    
            }
            else if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 2)
            {

                cityId();
            }
           
           
        }
    }

    public void cityName()
    {

        string htmlpid = HttpUtility.HtmlDecode(Request.QueryString["pid"].ToString());
        string r = null;
        PhoneAreaInfoManager areaInfo = new PhoneAreaInfoManager();
        object areaData = areaInfo.GetPhoneAreaData(HttpContext.Current);
        DataSet pad = (DataSet)areaData;
        DataRow[] rows = pad.Tables["area"].Select("ProvinceID='" + htmlpid + "'");
        for (int i = 0; i < rows.Length; i++)
        {
            r = r + rows[i]["AreaName"].ToString() + "count" + rows.Length.ToString() + "count" + "";
        }
        Response.Write(r);
       

    }

    public void cityId()
    {

        string htmlpid = HttpUtility.HtmlDecode(Request.QueryString["pid"].ToString());
        string r = null;
        PhoneAreaInfoManager areaInfo = new PhoneAreaInfoManager();
        object areaData = areaInfo.GetPhoneAreaData(HttpContext.Current);
        DataSet pad = (DataSet)areaData;
        DataRow[] rows = pad.Tables["area"].Select("ProvinceID='" + htmlpid + "'");
        for (int i = 0; i < rows.Length; i++)
        {
            r = r + rows[i]["AreaId"].ToString() + "count" + rows.Length.ToString() + "count" + "";
        }
        Response.Write(r);
    }

    //public void cityid()
    //{
       
    //}

    //#region 城市查询

    //public static DataSet selCity(string provinceid)
    //{
    //    string Msg = "";
    //    SqlConnection myCon = null;
    //    SqlCommand cmd = new SqlCommand();
    //    string SqlResult = "";
    //    DataSet ds = new DataSet();
    //    try
    //    {

    //        myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandText = "up_Customer_V3_Interface_SelCityByPid";

    //        SqlParameter parProvinceid = new SqlParameter("provinceid", SqlDbType.VarChar, 2);
    //        parProvinceid.Value = provinceid;
    //        cmd.Parameters.Add(parProvinceid);

    //        ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

    //    }
    //    catch (Exception e)
    //    {
    //        SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code.ToString();
    //        Msg = e.Message;
    //    }
    //    return ds;
    //}
    //#endregion
}

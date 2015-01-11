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
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public partial class bizCard2 : System.Web.UI.Page
{
    public string SPID = "35000001";
   // public string ReturnUrl = "";
    public static string CustID = "";
    public string OldPwd = "";
    public string VerifyPwd = "";
    public string ErrMsg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            TokenValidate.Validate();
            if (CommonBizRules.IsUrlParams(HttpContext.Current.Request.Url.OriginalString))
            {
                SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
               
            }
            CustID = TokenValidate.CustID;
           
            proInfoList.Items.Clear();
            DataTable dt = CustProvinceRelation.GetProvince();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem ltem = new ListItem();
                    ltem.Value = dt.Rows[i]["ProvinceID"].ToString();
                    ltem.Text = dt.Rows[i]["ProvinceName"].ToString();
                    proInfoList.Items.Add(ltem);
                }
                proInfoList.Items.Add("请选择省份");
                proInfoList.SelectedIndex = dt.Rows.Count ;
            }
         

        }
    }
    protected void Button1_ServerClick(object sender, EventArgs e)
    {
      
           
        if (proInfoList.SelectedIndex == proInfoList.Items.Count -1)
            return;
       string ProvinceID=proInfoList.Value;
       string AreaID = resulttxt.Value;
       if ( AreaID == null || AreaID == "")
           AreaID = ProvinceID; 
        
        string UserAccount = "";
        string sUserAccount = "";
        string ErrMsg = "";
        string CustLevel = System.Configuration.ConfigurationManager.AppSettings["CustLevel"];
        int Result = UserRegistry.GenerationCard(SPID, CustID, "", ProvinceID, AreaID, 1, CustLevel, "02","1",
            out UserAccount, out sUserAccount, out ErrMsg);
        if(Result !=0)
        {
            Response.Redirect("ErrorInfo.aspx?Result=" + Result + "&ErrorInfo=未分配到商旅卡号&FunctionName=生成商旅卡号"); 
        }
        else
        {           
            Response.Redirect("bizCard3.aspx?SLCarDID=" + sUserAccount);
        }

    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            string ReferrerUrl = Request.Cookies["ReferrerUrl"].Value.ToString();
            Response.Redirect(ReferrerUrl);
        }
        catch (System.Exception ex)
        {

        }
        
    }
}

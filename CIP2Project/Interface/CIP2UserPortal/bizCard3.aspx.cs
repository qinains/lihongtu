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

public partial class bizCard3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!this.IsPostBack )
        {
            if (PageUtility.IsParameterExist("SLCarDID", this.Page))
            {
                this.lbErrorInfo.InnerText  = Request["SLCarDID"];
            }
            
        }
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string ReturnUrl = Request.Cookies["ReferrerUrl"].Value.ToString();
            Response.Redirect(ReturnUrl);            
        }
        catch (System.Exception ex)
        {
        	
        }
        
    }
}

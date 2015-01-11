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
using System.Globalization;
using Linkage.BestTone.Interface.Utility;

public partial class ContactInfo : System.Web.UI.Page
{
    public string Spid = "35000000";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string url = Request.Url.AbsoluteUri;
            if (Request.QueryString["SPID"] != null)
            {
                Spid = Request.QueryString["SPID"].ToString();
                this.spidtxt.Value = Spid;
            }

            TokenValidate.IsRedircet = true;            
            TokenValidate.Validate();                       
            string CustID = TokenValidate.CustID;   
            if (CustID != "")
            {
                this.custidtxt.Value = CustID;
            }

        }
    }
}

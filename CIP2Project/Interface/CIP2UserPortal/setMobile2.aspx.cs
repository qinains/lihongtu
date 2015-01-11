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

public partial class setMobile2 : System.Web.UI.Page
{
    public string Phone;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string Url = "";
            string urlphone = null;
            if (Request.QueryString["Phone"] != null)
            {
                urlphone = Request.QueryString["Phone"].ToString();
                Phone = urlphone;
            }
            if (Request.QueryString["ReturnUrl"] != null)
            {
                Url = Request.QueryString["ReturnUrl"].ToString();
                Response.Redirect(Url);
            }
        }
    }
}

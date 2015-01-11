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

public partial class SSO_Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String ticket = Request["Ticket"] == null ? String.Empty : ("Ticket:" + Request["Ticket"]);
            String errmsg = Request["ErrMsg"] == null ? String.Empty : ("errmsg:" + Request["ErrMsg"]);
            Response.Write(ticket + errmsg);
        }
    }
}

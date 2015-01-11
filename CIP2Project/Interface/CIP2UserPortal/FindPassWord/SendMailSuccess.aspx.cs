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

public partial class FindPassWord_SendMailSuccess : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.linkReSendEmail.NavigateUrl = "FindByEmail.aspx?ReturnUrl=" + (Request["ReturnUrl"] == null ? HttpUtility.UrlEncode(ConstHelper.DefaultInstance.BesttoneLoginPage) : Request["ReturnUrl"]);
        }
    }
}

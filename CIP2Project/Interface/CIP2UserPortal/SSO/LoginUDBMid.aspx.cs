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

public partial class SSO_LoginUDBMid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            String Ticket = Request["Ticket"] == null ? String.Empty : Request["Ticket"];
            String ErrMsg = Request["ErrMsg"] == null ? String.Empty : Request["ErrMsg"];
            String ReturnUrl = Request["ReturnUrl"];
            if (!String.IsNullOrEmpty(ErrMsg))
            {
                Response.Write("<iframe frameborder='1' width='100' height='100'src='" + UDBConstDefinition.DefaultInstance.UDBLogoutUrl + "'  style='display:none'></iframe>");
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (String.IsNullOrEmpty(ReturnUrl))
                    ReturnUrl = "http://www.118114.cn";
                if (ReturnUrl.IndexOf('?') > 0)
                {
                    ReturnUrl += "&Ticket=" + Ticket;
                }
                else
                {
                    ReturnUrl += "?Ticket=" + Ticket;
                }
                this.hdReturnUrl.Value = ReturnUrl;
            }
        }
        catch (Exception ex)
        { }
    }
}

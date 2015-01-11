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
public partial class verifyMobile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.hdReturnUrl.Value = Request["ReturnUrl"] == null ? String.Empty : HttpUtility.UrlDecode(Request["ReturnUrl"]);
            string url = Request.Url.AbsoluteUri;
            string Spid = null;
            if (Request.QueryString["SPID"] != null)
            {
                if (Request.QueryString["SPID"].ToString() != "35000000")
                {
                    Spid = Request.QueryString["SPID"].ToString();
                    this.spidtxt.Value = Spid.ToString();
                }
                else
                {
                    Spid = "35000000";
                    this.spidtxt.Value = Spid.ToString();
                }
            }
            else
            {
                Spid = "";
                this.spidtxt.Value = "";
            }
            string IP = Page.Request.UserHostAddress;
            this.iptxt.Value = IP;
        }
    }


    protected void urlRedirectButton_ServerClick(object sender, EventArgs e)
    {
        String redirectUrl = "Success.aspx?Description=密码已发送至您的手机，请注意查收";
        if (String.IsNullOrEmpty(this.hdReturnUrl.Value))
        {
            redirectUrl += "&ReturnUrl=" + this.hdReturnUrl.Value;
        }
        Response.Redirect(redirectUrl);
        //CommonBizRules.SuccessRedirect("verifyMobile2.aspx", "密码已发送至您的手机，请注意查收并修改", HttpContext.Current);

    }
}

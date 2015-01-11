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

public partial class FindPwdSelect : System.Web.UI.Page
{
    public String SPID = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        if (!IsPostBack)
        {
            this.hdReturnUrl.Value = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.UrlDecode(Request["ReturnUrl"]);
            if (!String.IsNullOrEmpty(this.hdReturnUrl.Value))
            {
                String url = HttpUtility.UrlEncode(this.hdReturnUrl.Value);
                //this.linkPhone.NavigateUrl = "verifyMobile.aspx?ReturnUrl=" + url;
                //this.linkEmail.NavigateUrl = "emailByPwd.aspx?ReturnUrl=" + url;
                //this.linkPwdQuestion.NavigateUrl = "paswdByQnA.aspx?ReturnUrl=" + url;
                this.linkEmail.NavigateUrl = "FindPassWord/FindByEmail.aspx?ReturnUrl=" + url+"&SPID="+SPID;
                this.linkPhone.NavigateUrl = "FindPassWord/FindByPhoneV2.aspx?ReturnUrl=" + url+"&SPID="+SPID;
                //this.linkPwdQuestion.NavigateUrl = "paswdByQnA.aspx?ReturnUrl=" + url;
            }
        }
    }
}

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
using System.Collections.Generic;
public partial class authen : System.Web.UI.Page
{
    public string Msg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string CustID = null;
            string Email = null;
            string url = Request.Url.AbsoluteUri;
            List<string> list = new List<string>();
            list = CommonBizRules.DecryptEmailURL(url, HttpContext.Current);
            CustID = list[0];
            Email = list[1];

            int i = SetMail.SelSendEmailMassage(CustID, Email, out Msg);
            if (i == 0)
            {
                CommonBizRules.SuccessRedirect("", "认证邮箱设置成功", this.Context);
            }
        }
    }
}

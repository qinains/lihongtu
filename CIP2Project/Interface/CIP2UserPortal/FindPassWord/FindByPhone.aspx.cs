﻿using System;
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

public partial class FindByPhone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.hdReturnUrl.Value = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.UrlDecode(Request["ReturnUrl"]);
    }

    protected void urlRedirectButton_ServerClick(object sender, EventArgs e)
    {
        CommonBizRules.SuccessRedirect("", "请查收邮件", HttpContext.Current);
    }
}

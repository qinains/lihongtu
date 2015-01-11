using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

public partial class UserAccount_DetailQuery : AccountBasePage
{
    BesttoneAccountDAO _besttoneAccount_bo = new BesttoneAccountDAO();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.hdHeadFoot.Value = base.IsNeedHeadFoot == "yes" ? "1" : "0";
    }
 
}

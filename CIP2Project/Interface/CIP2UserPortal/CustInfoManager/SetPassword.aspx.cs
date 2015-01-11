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

public partial class CustInfoManager_SetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.hdCustID.Value = "117663768";
        this.hdPasswordType.Value = "2";
    }
}

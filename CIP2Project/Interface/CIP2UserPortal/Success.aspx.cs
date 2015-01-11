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
using System.Threading;
using Linkage.BestTone.Interface.Utility;


public partial class Success : System.Web.UI.Page
{
    public string ReturnUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string Description = "";
        if (PageUtility.IsParameterExist("Description", this.Page))
        {
            Description = Request["Description"];
        }

        if (PageUtility.IsParameterExist("ReturnUrl", this.Page))
        {
            ReturnUrl = Request["ReturnUrl"];
        }

        //if(!CommonUtility.IsEmpty(ReturnUrl))
        //{
        //    lbHint.InnerText = "稍后自动跳转";
        //}
        lbDescription.InnerText = Description;

    }
}

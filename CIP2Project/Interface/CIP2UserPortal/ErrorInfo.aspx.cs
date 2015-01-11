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

using Linkage.BestTone.Interface.Utility;

public partial class ErrorInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int Result = -1;
        string ErrMsg = "";
        string FunctionName = "";
        if (PageUtility.IsParameterExist("Result", this.Page))
        {
            Result = int.Parse(Request["Result"]);
        }

        if (PageUtility.IsParameterExist("ErrorInfo", this.Page))
        {
            ErrMsg = Request["ErrorInfo"];
        }

        if (PageUtility.IsParameterExist("FunctionName", this.Page))
        {
            FunctionName = Request["FunctionName"];
        }

        lbFunctionName.InnerText = FunctionName;
        lbErrorInfo.InnerText = ErrMsg;
        
    }
    protected void Foot1_Load(object sender, EventArgs e)
    {

    }
}

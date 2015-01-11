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

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.Rule;

public partial class FindPassWord_ResetPasswordByPhone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String urlParam = Request["urlParam"] == null ? String.Empty : HttpUtility.UrlDecode(Request["urlParam"]);
        if (String.IsNullOrEmpty(urlParam))
        {
            this.ResetPanel.Visible = false;
            this.MsgPanel.Visible = true;
        }
        else
        {
            this.ResetPanel.Visible = true;
            this.MsgPanel.Visible = false;
            try
            {
                String[] tempArray = urlParam.Split('$');
                this.hdCustID.Value = tempArray[0];
                this.hdReturnUrl.Value = tempArray[1];
                if (String.IsNullOrEmpty(this.hdCustID.Value))
                    throw new Exception("异常");

            }
            catch (Exception ex)
            {
                this.ResetPanel.Visible = false;
                this.MsgPanel.Visible = true;
            }

        }
    }
}

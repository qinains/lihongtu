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

public partial class TellMeAuthenCode : System.Web.UI.Page
{
    public String AuthenCode = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        String t_authenCode = "";
        String ErrMsg = "";
        String Phone = Request["Phone"];
        int Result = PhoneBO.SelSendSMSMessageAuthenCode(Phone, out t_authenCode, out ErrMsg);
        if (Result == 0)
        {
            AuthenCode = t_authenCode;
            Response.Write("您的验证码是:" + AuthenCode);
        }
        else
        {
            Response.Write("没有取得验证码");
        }

    }
}

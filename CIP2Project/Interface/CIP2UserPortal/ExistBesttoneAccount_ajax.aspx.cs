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
public partial class ExistBesttoneAccount_ajax : System.Web.UI.Page
{

    int Result ;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["typeId"].ToString().Equals("1"))
            {
                //判断手机是否可开户
                Result = ExistBesttoneAccount();
                Response.Write(Result.ToString());
 
            }
        }
    }


    public int ExistBesttoneAccount()
    {
        string ErrMsg = "";
        string PhoneNum = HttpUtility.HtmlDecode(Request.QueryString["PhoneNum"].ToString());
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        int Result = PhoneBO.IsBesttoneAccountBind(PhoneNum, out ErrMsg);
        return Result;
    }

}

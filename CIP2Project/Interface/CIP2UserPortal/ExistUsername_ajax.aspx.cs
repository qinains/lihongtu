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

public partial class ExistUsername_ajax : System.Web.UI.Page
{
    public int Result;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            if (Request.QueryString["typeId"].ToString().Equals("1"))
            {
                //判断用户已经存在
                ExistUser();
            }
        }
    }

    public void ExistUser()
    {
        //调用用户验证函数
        string UserName = HttpUtility.HtmlDecode(Request.QueryString["UserName"].ToString().Trim());
        Result = CustBasicInfo.IsExistUser(UserName);
        Response.Write(Result);
    }
}

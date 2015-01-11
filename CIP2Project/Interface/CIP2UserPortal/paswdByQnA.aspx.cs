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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;

public partial class paswdByQnA : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.hdReturnUrl.Value = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.UrlDecode(Request["ReturnUrl"]);
            //取出问题列表
            DataSet ds = PassWordBO.QueryPwdQuestion();

            //给问题一下拉框赋值
            ddlQuestion.DataSource = ds;
            ddlQuestion.DataValueField = "QuestionID";
            ddlQuestion.DataTextField = "Question";
            ddlQuestion.SelectedIndex = 1;
            ddlQuestion.DataBind();

            txtQuestion.Value = ddlQuestion.Value;
        }
    }

    protected void btByQnA_ServerClick(object sender, EventArgs e)
    {

    }
}

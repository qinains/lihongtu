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


public partial class passwordQnA : System.Web.UI.Page
{
    string CustID = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

            TokenValidate.IsRedircet = true;
            TokenValidate.Validate();
            CustID = TokenValidate.CustID;
            //取出问题列表
            DataSet ds = PassWordBO.QueryPwdQuestion();

            //给问题一下拉框赋值
            ddlQuestion1.DataSource = ds;
            ddlQuestion1.DataValueField = "QuestionID";
            ddlQuestion1.DataTextField = "Question";
            ddlQuestion1.DataBind();

            //给问题二下拉框赋值
            ddlQuestion2.DataSource = ds;
            ddlQuestion2.DataValueField = "QuestionID";
            ddlQuestion2.DataTextField = "Question";
            ddlQuestion2.DataBind();

            //给问题三下拉框赋值
            ddlQuestion3.DataSource = ds;
            ddlQuestion3.DataValueField = "QuestionID";
            ddlQuestion3.DataTextField = "Question";
            ddlQuestion3.DataBind();

            //根据当前客户ID取得答案列表
            DataSet dsAnswer = PassWordBO.QueryPwdQuestionAnswer(CustID);

            int j = 0;

            foreach (DataRow row in dsAnswer.Tables[0].Rows)
            {

                if (j == 0)
                {
                    txtHidSq1.Text = row[0].ToString();
                    ddlQuestion1.SelectedIndex = int.Parse(row[1].ToString());
                    txtAnswer1.Text = row[2].ToString();
                }
                else if (j == 1)
                {
                    txtHidSq2.Text = row[0].ToString();
                    ddlQuestion2.SelectedIndex = int.Parse(row[1].ToString());
                    txtAnswer2.Text = row[2].ToString();
                }
                else if (j == 2)
                {
                    txtHidSq3.Text = row[0].ToString();
                    ddlQuestion3.SelectedIndex = int.Parse(row[1].ToString());
                    txtAnswer3.Text = row[2].ToString();
                }

                j++;

            }

        }
    }
    protected void btnlogin_ServerClick(object sender, EventArgs e)
    {

        string ErrMeg = "";
        string er = "";
        TokenValidate.IsRedircet = true;
        TokenValidate.Validate();
        CustID = TokenValidate.CustID;

        if (ddlQuestion1.SelectedIndex != 0 && txtAnswer1.Text.Trim().Length > 0)
        {
            PassWordBO.UpdatePwdQuestionAnswer(txtHidSq1.Text, CustID, ddlQuestion1.SelectedIndex, txtAnswer1.Text, out er);
            ErrMeg = ErrMeg + er;
        }

        if (ddlQuestion2.SelectedIndex != 0 && txtAnswer2.Text.Trim().Length > 0)
        {
            PassWordBO.UpdatePwdQuestionAnswer(txtHidSq2.Text, CustID, ddlQuestion2.SelectedIndex, txtAnswer2.Text, out er);
            ErrMeg = ErrMeg + er;
        }

        if (ddlQuestion3.SelectedIndex != 0 && txtAnswer3.Text.Trim().Length > 0)
        {
            PassWordBO.UpdatePwdQuestionAnswer(txtHidSq3.Text, CustID, ddlQuestion3.SelectedIndex, txtAnswer3.Text, out er);
            ErrMeg = ErrMeg + er;
        }

        CommonBizRules.SuccessRedirect("", "问题设置成功", this.Context);
        //if (ErrMeg == "")
        //{
        //    Context.Server.Transfer("verifyPasswordQnA.aspx");
        //}
        //else {
        //    Context.Server.Transfer("verifyPasswordQnA.aspx");
        //}
    }
}

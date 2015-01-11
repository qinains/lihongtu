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
using System.Collections.Generic;
public partial class verifyEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            string Spid = null;
            string Emali = null;
            string Msg = null;
            if (Request.QueryString["SPID"] != null)
            {
                if (Request.QueryString["SPID"].ToString() != "35000000")
                {
                    Spid = Request.QueryString["SPID"].ToString();
                }
                else
                {
                    Spid = "35000000";
                }
            }
            else
            {
                Spid = "";
            }
            TokenValidate.IsRedircet = true;
            TokenValidate.Validate();
           string CustID = TokenValidate.CustID;
            //  string CustID = "571300100";
            if (CustID != "")
            {
                this.custidtxt.Value = CustID;
            }
            // Emali = SetMail.SelEmailAddress(this.custidtxt.Value, Spid, out Msg);
            DataSet ds = new DataSet();
            ds = SetMail.SelEmailAddress(this.custidtxt.Value, out Msg);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.Emailtxt.Value = ds.Tables[0].Rows[0]["Email"].ToString();

                string Emailclass = ds.Tables[0].Rows[0]["EmailClass"].ToString();
                if (Emailclass == "1")
                {
                    this.EmailClassLab.Text = "一般邮箱";
                }
                else if (Emailclass == "2")
                {
                    this.EmailClassLab.Text = "认证邮箱";
                }
            }


        }


    }

    protected void urlRedirectButton_ServerClick(object sender, EventArgs e)
    {
        CommonBizRules.SuccessRedirect("", "设置认证邮箱成功", HttpContext.Current);
    }
}

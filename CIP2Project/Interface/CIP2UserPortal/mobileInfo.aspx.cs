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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
public partial class mobileInfo : System.Web.UI.Page
{
    public string Msg;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string Spid = null;

            TokenValidate.IsRedircet = true;
            TokenValidate.Validate();
            string CustID = TokenValidate.CustID;

            Spid = Request["SPID"] == null ? String.Empty : Request["SPID"];
            //if (Request.QueryString["SPID"] != null)
            //{
            //    if (Request.QueryString["SPID"].ToString() != "35000000")
            //    {
            //        Spid = Request.QueryString["SPID"].ToString();
            //    }
            //    else
            //    {
            //        Spid = "35000000";
            //    }
            //}
            //else
            //{
            //    Spid = "";
            //}

            DataSet ds = PhoneBO.GetAllPhone(CustID, out Msg);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.Repeater1.DataSource = ds;
                this.Repeater1.DataBind();
            }
            else
            {
                Response.Redirect("setMobile.aspx?id=4&SPID=35000000");
            }
        }
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlInputText typetxt = e.Item.FindControl("typeTxt") as HtmlInputText;
            Label typeLab = e.Item.FindControl("typelable") as Label;
            HtmlInputButton rzbutton = e.Item.FindControl("rzbutton") as HtmlInputButton;
            HtmlInputButton delbutton = e.Item.FindControl("delbutton") as HtmlInputButton;
            if (typetxt.Value == "1")
            {
                typeLab.Text = "未认证";
                delbutton.Visible = true;
                rzbutton.Visible = true;
            }
            else if (typetxt.Value == "2")
            {
                typeLab.Text = "已认证";
                delbutton.Visible = true;
                rzbutton.Visible = false;
            }
        }
    }
}

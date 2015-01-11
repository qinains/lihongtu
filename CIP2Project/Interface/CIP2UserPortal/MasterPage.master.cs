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
public partial class MasterPage : System.Web.UI.MasterPage
{
    public string a = null;
    public string ReturnUrl = "";
    public String OuterID = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (PageUtility.IsParameterExist("ReturnUrl", this.Page))
        {
            ReturnUrl = Request["ReturnUrl"];
        }

        if (!Page.IsPostBack)
        {
            //Page.RegisterStartupScript("", "<script>csstype();</script>");
            this.Page.Title = "号码百事通客户信息平台";
            string Spid = null;
            if (Request.QueryString["SPID"] != null)
            {
                if (Request.QueryString["SPID"].ToString() != "35000000")
                {
                    Spid = Request.QueryString["SPID"].ToString();
                    this.spidtxt.Value = Spid;
                }
                else
                {
                    Spid = "35000000";
                    this.spidtxt.Value = Spid;
                }
            }
            else
            {
                Spid = "";
                this.spidtxt.Value = Spid;
            }


            string PageName = Request.Url.AbsolutePath;
            int LastLine = PageName.LastIndexOf('/');
            int LocationASPX = PageName.LastIndexOf(".aspx");
            PageName = PageName.Substring(LastLine + 1, LocationASPX - LastLine-1);
            //获取不需要登录的页面列表，若为不需要登录的页面则不校验Cookie
            string NoLoginPageList = System.Configuration.ConfigurationManager.AppSettings["NoLoginPageList"];
            if (NoLoginPageList.IndexOf(PageName) < 0)
            {
                TokenValidate.IsRedircet = true;
            }
            TokenValidate.IsRedircet = false;

            TokenValidate.Validate();
            string CustID = TokenValidate.CustID;
            OuterID = TokenValidate.OuterID;
            if (CustID != "")
            {
                this.custidtxt.Value = CustID;
            }
            if (!String.IsNullOrEmpty(OuterID))
            {
                this.outeridtxt.Value = OuterID;
            }
        }
    }
    public void setTopWelcome(string welcome)
    {
        Top1.Welcome = welcome;
    }
    
}

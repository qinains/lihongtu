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
using System.Data.SqlClient;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;


public partial class bizCard : System.Web.UI.Page
{
    public string SPID = "35000001";
    public string ReturnUrl = "";
    public string CustID = "";
    public string OldPwd = "";
    public string VerifyPwd = "";
    public string ErrMsg = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            TokenValidate.Validate();
            if (CommonBizRules.IsUrlParams(HttpContext.Current.Request.Url.OriginalString))
            {
                if (Request.QueryString["SPID"] != null)
                {
                    SPID = Request.QueryString["SPID"].ToString();
                    //  ReturnUrl = Request.QueryString["ReturnUrl"].ToString();
                }
                else
                    SPID = "35000000";
                //SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
                //ReturnUrl = HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"].ToString());

            }

            CustID = TokenValidate.CustID;
            string UserAccount = "";

            try
            {
                string ReturnUrl = HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"].ToString());
                PageUtility.SetCookie(ReturnUrl, "ReferrerUrl", this.Page);
                //string url = Page.Request.UrlReferrer.ToString();5
                //PageUtility.SetCookie(url, "ReferrerUrl", this.Page);
            }
            catch (System.Exception ex)
            { }
            int Result = CustBasicInfo.GetUserAccount(CustID,out UserAccount,out ErrMsg);
            if (Result == 0)
            {
                //判断此人时候有商旅卡号!
                this.Label1.Text = "您已经申请过商旅卡了,你的商旅卡号为:" + UserAccount;
                this.Label1.Visible = true;               
                btnlogin2.Visible = true;
              
            }
            else
            {
                Response.Redirect("bizCard2.aspx?id=6&SPID=" + SPID);
               
            }

           
                
        }
    }
      protected void btnlogin_ServerClick(object sender, EventArgs e)
      {
         
      }

    protected void btnlogin2_ServerClick(object sender, EventArgs e)
    {
       
        //string ReturnUrl = Request.Cookies["ReturnUrl"].Value.ToString();
        //Response.Redirect(ReturnUrl);    
        try
        {
            string ReferrerUrl = Request.Cookies["ReferrerUrl"].Value.ToString();
            Response.Redirect(ReferrerUrl);
        }
        catch (System.Exception ex)
        {

        }
       
    }
}

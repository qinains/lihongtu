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

public partial class IpTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)

    {
        //string IPAddress = "";
        //if (Context.Request.ServerVariables["HTTP_VIA"] != null) // using proxy
        //{
        //    IPAddress = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();  // Return real client IP.
        //}
        //else// not using proxy or can't get the Client IP
        //{
        //    IPAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
        //}
        
        //txtIP.Text = "["+IPAddress+"]["+HttpContext.Current.Request.UserHostAddress+"]";


        Response.Write("<iframe frameborder='1'  width='100'   height='100'src='http://zyj.vnet.cn/CIP2UserPortal/sso/QuickLogin.aspx?AuthenType=1&AuthenName=csdn23&Password=123456'  style='display  :block'></iframe>");

        txtUrl.Text = "[" + this.Context.Request.Url.AbsoluteUri.ToString() + "][" + this.Context.Request.Url.Port.ToString() + "]";
        
    }
}

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

using NetDimension.Web;
using NetDimension.Weibo;

public partial class SSO_SinaLogin : System.Web.UI.Page
{
    Cookie cookie = new Cookie("WeiboDemo", 24, TimeUnit.Hour);

    Client Sina = null;
    OAuth oauth = new OAuth(ConfigurationManager.AppSettings["AppKey"], ConfigurationManager.AppSettings["AppSecret"], ConfigurationManager.AppSettings["CallbackUrl"]);


    private void LoadPublicTimeline()
    {

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Sina = new Client(oauth);
        if (!IsPostBack)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                AccessToken token = oauth.GetAccessTokenByAuthorizationCode(Request.QueryString["code"]);
                string accessToken = token.Token;

                cookie["AccessToken"] = accessToken;

                Response.Redirect("SinaDefaultV2.aspx");
            }
            else
            {
                string url = oauth.GetAuthorizeURL(ResponseType.Code, null, DisplayType.Default);
                authUrl.NavigateUrl = url;
            }

        }

       
    }
}

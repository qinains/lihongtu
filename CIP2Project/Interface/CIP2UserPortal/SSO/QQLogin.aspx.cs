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

using System.Collections.Generic;
using QWeiboSDK;

public partial class SSO_QQLogin : System.Web.UI.Page
{
    private string strAppKey = ConfigurationManager.AppSettings["AppKey_QQ"];　　 //应用KEY        
    private string strAppSecret = ConfigurationManager.AppSettings["AppSecret_QQ"]; //应用密钥



    //授权确认码        
    private string oauthVerify = null;
    public string OauthVerify
    {
        get { return oauthVerify; }
        set { oauthVerify = value; }
    }

    public string tokenKey = null;
    public string TokenKey
    {
        get { return tokenKey; }
    }

    private string tokenSecret = null;
    public string TokenSecret
    {
        get { return tokenSecret; }
    }

    private string accessSecret = null;
    public string AccessSecret
    {
        get { return accessSecret; }
        set { accessSecret = value; }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request["oauth_verifier"] != null)
            {
                OauthVerify = Request["oauth_verifier"].ToString();


                Session["oauthVerify"] = OauthVerify;
            }
            else
            {
                if (GetRequestToken(strAppKey, strAppSecret))
                {
                    Session["tokenKey"] = tokenKey;
                    Session["tokenSecret"] = tokenSecret;

                    Response.Redirect("http://open.t.qq.com/cgi-bin/authorize?oauth_token=" + tokenKey);
                }
            }

            if (Session["accessToken"] == null && Session["accessSecret"] == null)
            {
                if (OauthVerify != null && Session["tokenKey"] != null && Session["tokenSecret"] != null)
                {
                    bool answer = GetAccessToken(strAppKey, strAppSecret, Session["tokenKey"].ToString(), Session["tokenSecret"].ToString(), OauthVerify);

                    if (answer)
                    {
                        Session["accessToken"] = tokenKey;

                        Session["accessSecret"] = tokenSecret;

                        Response.Write("accesstoken=" + tokenKey);
                        Response.Write("accesssecret=" + TokenSecret);
                    }

                }
            }
        }
    }

    private bool GetRequestToken(string customerKey, string customerSecret)
    {
        string url = "https://open.t.qq.com/cgi-bin/request_token";
        List<QWeiboSDK.Parameter> parameters = new List<QWeiboSDK.Parameter>();
        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = customerKey;
        oauthKey.customSecret = customerSecret;
        oauthKey.callbackUrl = ConfigurationManager.AppSettings["CallbackUrl_QQ"];       //"http://localhost:20595";
        QWeiboRequest request = new QWeiboRequest();

        return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));

    }


    private bool GetAccessToken(string customerKey, string customerSecret, string requestToken, string requestTokenSecrect, string verify)
    {
        string url = "https://open.t.qq.com/cgi-bin/access_token";
        List<QWeiboSDK.Parameter> parameters = new List<QWeiboSDK.Parameter>();

        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = customerKey;

        oauthKey.customSecret = customerSecret;

        oauthKey.tokenKey = requestToken;

        oauthKey.tokenSecret = requestTokenSecrect;

        oauthKey.verify = verify;

        QWeiboRequest request = new QWeiboRequest();

        return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));

    }


    private void Send()
    {
        string strRet = "";
        List<QWeiboSDK.Parameter> parameters = new List<QWeiboSDK.Parameter>();
        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = strAppKey;
        oauthKey.customSecret = strAppSecret;
        oauthKey.tokenKey = Session["accessToken"].ToString();
        oauthKey.tokenSecret = Session["accessSecret"].ToString();


    }

    private bool ParseToken(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            return false;
        }

        string[] tokenArray = response.Split('&');

        if (tokenArray.Length < 2)
        {
            return false;
        }

        string strTokenKey = tokenArray[0];
        string strTokenSecret = tokenArray[1];

        string[] token1 = strTokenKey.Split('=');
        if (token1.Length < 2)
        {
            return false;
        }

        tokenKey = token1[1];

        string[] token2 = strTokenSecret.Split('=');
        if (token2.Length < 2)
        {
            return false;
        }

        tokenSecret = token2[1];

        return true;

    }
}

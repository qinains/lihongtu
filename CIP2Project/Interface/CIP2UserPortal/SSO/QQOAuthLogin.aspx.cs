using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;


public partial class SSO_QQOAuthLogin : System.Web.UI.Page
{

    private readonly string client_id = Utils.GetAppSeting("qzone_AppID");
    private readonly string client_secret = Utils.GetAppSeting("qzone_AppKey");
    private readonly string redirect_uri = Utils.GetAppSeting("qzone_Redirect_uri");
    private string state = "";

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnLogin_Click(object sender, EventArgs e)
    {

        state = Guid.NewGuid().ToString().Replace("-", "");
        //Utils.WriteCookie("state", state, 60);
        HttpContext.Current.Session["state"] = state;

        string login_url = "https://open.t.qq.com/cgi-bin/oauth2/authorize?response_type=code&client_id=" + client_id + "&state=" + state + "&redirect_uri=" + Utils.UrlEncode(redirect_uri) + "&scope=get_user_info"; //,get_info,get_other_info
        //写日志
        Logs.logSave("===========================================分割线=============================================");
        //写日志
        Logs.logSave("第一步：开始跳转至QQ登陆URL：" + login_url);

        //开始发送
        HttpContext.Current.Response.Redirect(login_url);

    }
    
}

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
public partial class SSO_SinaDefault : System.Web.UI.Page
{
    Cookie cookie = new Cookie("WeiboDemo", 24, TimeUnit.Hour);

    Client Sina = null;
    string UserID = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(cookie["AccessToken"]))
        {
            Response.Redirect("SinaLogin.aspx");
        }
        else
        {
            Sina = new Client(new OAuth(ConfigurationManager.AppSettings["AppKey"], ConfigurationManager.AppSettings["AppSecret"], cookie["AccessToken"], null)); //用cookie里的accesstoken来实例化OAuth，这样OAuth就有操作权限了
        }

        UserID = Sina.API.Account.GetUID();
        this.UID.Text = UserID;
    }

    public string LoadUserInfo()
    {
        NetDimension.Weibo.Entities.user.Entity user = Sina.API.Users.Show(UserID, null);
       
        string result = user.ToString();
       
        return string.Format("{0}", result);
    }


    protected void btnSend_Click(object sender, EventArgs e)
    {
        string status = string.Empty;
        if (txtStatusBody.Text.Length == 0)
        {
            status = "我很懒，所以我直接点了发布按钮。";
        }
        else
        {
            status = txtStatusBody.Text;
        }


        if (fileUpload1.HasFile)
        {
            Sina.API.Statuses.Upload(status, fileUpload1.FileBytes, 0, 0, null);
        }
        else
        {
            Sina.API.Statuses.Update(status, 0, 0, null);
        }

        Response.Redirect("SinaDefault.aspx");
    }


}

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

public partial class EmailAuth_ajax : System.Web.UI.Page
{
    public int Result;
    public string SPID = "";
    public string Mail = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["typeId"].ToString().Equals("1"))
            {
                SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
                Mail = HttpUtility.HtmlDecode(Request.QueryString["Mail"].ToString());
                //判断邮箱是否被绑定
                Result = EmailAuth();
                if (Result != 0)
                {
                    Response.Write(Result.ToString());
                }
                else
                {
                    //发送验证码
                    //SendCode();
                    Result = 0;
                    Response.Write(Result.ToString());
                }

            }
        }
    }

    public int EmailAuth()
    {
        string ErrMsg="";
        Result = SetMail.EmailSel("", Mail, SPID, out ErrMsg);
        //Result = 1;
        return Result;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendCode()
    { 
        DateTime datetime = DateTime.Now;
        string ErrMsg = "";

        //给客户认证邮箱发EMAIL
        string m = CommonBizRules.EncryptEmailURl("", Mail, this.Context);
        string url = "点击完成认证:<a href='" + m + "'>" + m + "</a>";
        SetMail.InsertEmailSendMassage("", "1", url, "", 1, Mail, datetime, "", "中国电信号码百事通：激活邮箱", 0, out ErrMsg);
       //SetMail.InsertEmailSendMassage("", "1", "请点击下面链接", "", 0, Mail, datetime, "", "中国电信号码百事通：激活邮箱", 0, out ErrMsg);
    }
}

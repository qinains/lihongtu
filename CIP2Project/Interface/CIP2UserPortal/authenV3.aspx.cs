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
using System.Text;
public partial class authenV3 : System.Web.UI.Page
{
    public string Msg;
    /// <summary>
    /// author lihongtu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string SPID = null;
            string CustID = null;
            string Email = null;
            //string ReturnUrl = null;
            string url = Request.Url.AbsoluteUri;      //  这个就是邮箱中用户点的那串完整的链接
            List<string> list = new List<string>();
            //String Digest = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + Email + "$" + ReturnUrl + "$" + timeTamp, key);
            //String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(SPID + "$" + CustID + "$" +
            //    Email + "$" + ReturnUrl + "$" + timeTamp + "$" + Digest)));
            //list = CommonBizRules.DecryptEmailURLV2(SPID, url, HttpContext.Current);
            list = CommonBizRules.DecryptEmailWithNoReturnUrl(SPID,url,HttpContext.Current);
            SPID = list[0];
            CustID = list[1];
            Email = list[2];
            //ReturnUrl = list[3];
            int i = SetMail.SelSendEmailMassage(CustID, Email, out Msg);  // update custinfo set email=@Email,emailclass='2' where custid=@CustID
            StringBuilder ResponseMsg = new StringBuilder();
            if (i == 0)
            {
                //CommonBizRules.SuccessRedirect(ReturnUrl, "认证邮箱设置成功", this.Context);
                ResponseMsg.Length = 0;
                ResponseMsg.Append("<script type='text/javascript'>");
                ResponseMsg.AppendFormat("alert('{0}');",Msg);
                ResponseMsg.Append("</script>");
            }
            else
            {
                ResponseMsg.Length = 0;
                ResponseMsg.Append("<script type='text/javascript'>");
                ResponseMsg.AppendFormat("alert('{0}');", Msg);
                ResponseMsg.Append("</script>");
            }
        }
    }
}

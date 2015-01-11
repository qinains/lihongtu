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
using Linkage.BestTone.Interface.Utility;
using System.Text;
/// <summary>
// * 描述: 新版本注册流程
// * 开发平台: Windows XP + Microsoft SQL Server 2005
// * 开发语言: C#
// * 开发工具: Microsoft Visual Studio.Net 2005
// * 作者: 李宏图
// * 联系方式: 
// * 公司: 号百信息股份有限公司
// * 创建日期: 2013-12-20
/// </summary>
public partial class Ajax_Email_setEmailV2_ajax : System.Web.UI.Page
{
    public int k, y, j;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request["typeId"].ToString()) == 1)
            {
                String ajsxData = selEmail();
                Response.Write(ajsxData);
            }
        }
    }


    public String selEmail()
    {
        StringBuilder ResponseMsg = new StringBuilder();
        string msg = "";
        string Email = HttpUtility.HtmlDecode(Request["email"].ToString());
        string CustId = HttpUtility.HtmlDecode(Request["custid"].ToString());
        string ReturnUrl = HttpUtility.HtmlDecode(Request["returnUrl"].ToString());
        string Spid = HttpUtility.HtmlDecode(Request["SPID"].ToString());
  
        k = SetMail.EmailSel(CustId, Email, Spid, out msg);  // 查看该邮箱是否已经被别人注册   k =0 代表此邮箱尚未被用
        if (k == 0)
        {
            string a = CommonBizRules.EncryptEmailURlV2(Spid, CustId, Email, ReturnUrl, HttpContext.Current);  //这里跟老版本不一样的地方是加了返回地址
            string url = "点击完成认证:<a href='" + a + "'>" + a + "</a>";
            Random random = new Random();
            string AuthenCode = random.Next(111111, 999999).ToString();
            y = SetMail.InsertEmailSendMassage(CustId, "2", url, AuthenCode, 1, Email, DateTime.Now, "描述", "中国电信号码百事通：激活邮箱", 0, out msg);
            //Response.Write(y);
            ResponseMsg.Append("[{");
            ResponseMsg.AppendFormat("\"result\":\"{0}\",", "true");
            ResponseMsg.AppendFormat("\"info\":\"{0}\"", y);
            ResponseMsg.Append("}]");
            return ResponseMsg.ToString();
        }
        else
        {
            //Response.Write(msg);
            ResponseMsg.Append("[{");
            ResponseMsg.AppendFormat("\"result\":\"{0}\",", "false");
            ResponseMsg.AppendFormat("\"info\":\"{0}\"", msg);
            ResponseMsg.Append("}]");
            return ResponseMsg.ToString();
                
        }
        



    


    }



}

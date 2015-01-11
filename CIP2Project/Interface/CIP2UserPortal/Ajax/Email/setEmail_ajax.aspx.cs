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

/// <summary>
// *     描述: 修改客户信息AJAX处理页面
// * 开发平台: Windows XP + Microsoft SQL Server 2005
// * 开发语言: C#
// * 开发工具: Microsoft Visual Studio.Net 2005
// *     作者: 赵锐
// * 联系方式: 
// *     公司: 联创科技(南京)股份有限公司
// * 创建日期: 2009-07-31
/// </summary>

public partial class Ajax_Email_setEmail_ajax : System.Web.UI.Page
{
    public int k,y,j;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 1)
            {
                selEmail();
            }
        }
    }

    public void selEmail()
    {
        string msg="";
        string Email = HttpUtility.HtmlDecode(Request.QueryString["email"].ToString());
        string CustId = HttpUtility.HtmlDecode(Request.QueryString["custid"].ToString());
        string Spid = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        string Pwd = HttpUtility.HtmlDecode(Request.QueryString["pwd"].ToString());
        string webpwd = CryptographyUtil.Encrypt(Pwd);
        int i = FindPwd.SelState(CustId, webpwd, out msg);



        if (!ValidateValidateCode())
        {
            Response.Write("验证码错误，请重新输入");
            return;
        }
        else
        {

            if (i != 0)
            {
                Response.Write("登录密码输入错误，请重新输入");
                return;
            }
            else
            {
                k = SetMail.EmailSel(CustId, Email, Spid, out msg);
                if (k == 0)
                {
                    string a = CommonBizRules.EncryptEmailURl(CustId, Email, HttpContext.Current);
                    string url = "点击完成认证:<a href='" + a + "'>" + a + "</a>";
                    Random random = new Random();
                    string AuthenCode = random.Next(111111, 999999).ToString();
                    y = SetMail.InsertEmailSendMassage(CustId, "2", url, AuthenCode, 1, Email, DateTime.Now, "描述", "中国电信号码百事通：激活邮箱", 0, out msg);
                    Response.Write(y);
                }
                else
                {
                    Response.Write(msg);
                }
            }


           
        }

      
    }


    #region 验证校验
    /// <summary>
    /// 验证验证码是否正确
    /// </summary>
    /// <returns></returns>

    private bool ValidateValidateCode()
    {
        bool result = false;
        try
        {
            string validStr = HttpUtility.HtmlDecode(Request.QueryString["pageyz"].ToString().Trim().ToUpper());
            validStr = CryptographyUtil.Encrypt(validStr);
            if (Request.Cookies["PASSPORT_USER_VALIDATOR"] == null)
            {
                result = false;
            }
            else
            {
                if (Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"] == null || Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"] == null)
                {
                    result = false;
                }
                if (validStr == Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"].ToString())
                {
                    if (DateTime.Now > DateTime.Parse(CryptographyUtil.Decrypt(Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"].ToString())))
                        result = false;
                    else
                        result = true;
                }
                else
                {
                    result = false;
                }
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }

    #endregion
}

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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

/// <summary>
// *     描述: 认证邮箱找回密码AJAX处理页面
// * 开发平台: Windows XP + Microsoft SQL Server 2005
// * 开发语言: C#
// * 开发工具: Microsoft Visual Studio.Net 2005
// *     作者: 赵锐
// * 联系方式: 
// *     公司: 联创科技(南京)股份有限公司
// * 创建日期: 2009-07-31
/// </summary>

public partial class Ajax_Email_findPwd_ajax : System.Web.UI.Page
{
    public string Msg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString())==1)
            {
                SelEmail();
            }
        }
    }

    public void SelEmail()
    {
        string username = HttpUtility.HtmlDecode(Request.QueryString["name"].ToString());
        string email = HttpUtility.HtmlDecode(Request.QueryString["email"].ToString());
        if (!ValidateValidateCode())
        {
            Response.Write("验证码错误，请重新输入");
            return;
        }
        else
        {
            int i = SetMail.FindPwdByEmail(username, email, out Msg);
            if (i == 0)
            {
                string[] str = FindPwd.SelPwdByEmailandName(username, email, out Msg);
                string Pwd = CryptographyUtil.Decrypt(str[1].ToString());
                string CustId = str[0].ToString();
                int y = SetMail.InsertEmailSendMassage(CustId, "2", "您的密码是:" + Pwd, "", 1, email, DateTime.Now, "找回密码", "中国电信号码百事通：找回密码", 0, out Msg);
                Response.Write(y);
            }
            else
            {
                Response.Write(Msg);
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

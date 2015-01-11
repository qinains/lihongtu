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

public partial class Ajax_Mobile_setMobile_ajaxV2 : System.Web.UI.Page
{
    public int k;
    public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 1)
            {
                selMobile();
            }
            else if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 2)
            {
                setMobile();
            }
            else if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 3)
            {
                delMobile();
            }
        }
    }

    #region 校验手机号是否可以绑定
    /// <summary>
    /// 作者：赵锐
    /// 日期：2009年8月15日
    /// </summary>
    public void selMobile()
    {

        string custid = HttpUtility.HtmlDecode(Request.QueryString["custid"].ToString());
        string mnum = HttpUtility.HtmlDecode(Request.QueryString["mnum"].ToString());
        string spid = HttpUtility.HtmlDecode(Request.QueryString["spid"].ToString());
        int count = Convert.ToInt32(HttpUtility.HtmlDecode(Request.QueryString["count"].ToString()));

        k = PhoneBO.PhoneSel(custid, mnum, out msg);
        if (k == 0)
        {
            Random random = new Random();
            string AuthenCode = random.Next(111111, 999999).ToString();
            //CommonBizRules.SendMessage(mnum, "您的验证码是:"+AuthenCode, spid);
            CommonBizRules.SendMessageV3(mnum, "您在设置认证手机，验证码是:" + AuthenCode, spid);
            int y = PhoneBO.InsertPhoneSendMassage(custid, "您在设置认证手机，验证码信息内容", AuthenCode, mnum, DateTime.Now, "描述未知", count, 0, "1", out msg);
            Response.Write(y);
        }
        else
        {
            Response.Write(msg);
        }
    }
    #endregion

    #region 手机绑定
    /// <summary>
    /// 作者：赵锐
    /// 日期：2009年8月15日
    /// </summary>
    public void setMobile()
    {
        string ErrMsg = "";
        string mnum = HttpUtility.HtmlDecode(Request.QueryString["mnum"].ToString());
        string auth = HttpUtility.HtmlDecode(Request.QueryString["auth"].ToString());
        string custid = HttpUtility.HtmlDecode(Request.QueryString["custid"].ToString());
        string spid = HttpUtility.HtmlDecode(Request.QueryString["spid"].ToString());
        string pwd = HttpUtility.HtmlDecode(Request.QueryString["pwd"].ToString());
        string webpwd = CryptographyUtil.Encrypt(pwd);
        int i = FindPwd.SelState(custid, webpwd, out ErrMsg);  // 根据custid和webpwd 校验客户身份是否合法性

        if (!ValidateValidateCode())    //校验页面验证码
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
                k = PhoneBO.SelSendSMSMassage(custid, mnum, auth, out msg);   // 校验手机验证码
                if (k == 0)
                {
                    //int y = PhoneBO.PhoneSet(spid, custid, mnum, "2", "2", out msg);
                    int y = PhoneBO.PhoneSetV2(spid, custid, mnum, "2", "2", out msg);
                    Response.Write(k);
                }
                else
                {
                    Response.Write(msg);
                }
            }
        }
    }
    #endregion

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

    #region 删除手机
    /// <summary>
    /// 作者：赵锐
    /// 日期：2009年10月12日
    /// </summary>
    public void delMobile()
    {
        string id = HttpUtility.HtmlDecode(Request.QueryString["a"].ToString());
        string phone = HttpUtility.HtmlDecode(Request.QueryString["b"].ToString());
        string phoneclass = HttpUtility.HtmlDecode(Request.QueryString["c"].ToString());
        int i = PhoneBO.DelPhoneV2(phone, id, out msg);
        Response.Write(i);
    }
    #endregion


}

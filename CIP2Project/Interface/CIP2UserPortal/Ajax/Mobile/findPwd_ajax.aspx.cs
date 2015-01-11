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
using System.Collections.Generic;
/// <summary>
// *     描述: 认证手机找回密码AJAX处理页面
// * 开发平台: Windows XP + Microsoft SQL Server 2005
// * 开发语言: C#
// * 开发工具: Microsoft Visual Studio.Net 2005
// *     作者: 赵锐
// * 联系方式: 
// *     公司: 联创科技(南京)股份有限公司
// * 创建日期: 2009-07-31
/// </summary>

public partial class Ajax_Mobile_findPwd_ajax : System.Web.UI.Page
{
    public string Msg = "";
    public int k;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString())==1)
            {
                findpwd();
            }
        }
    }


    public void findpwd()
    {
        List<string> list = new List<string>();
        string massage=null;
        int type = Convert.ToInt32(HttpUtility.HtmlDecode(Request.QueryString["type"].ToString()));
        string phone = HttpUtility.HtmlDecode(Request.QueryString["phone"].ToString());
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["spid"].ToString());
        string IP = HttpUtility.HtmlDecode(Request.QueryString["ip"].ToString());

       
        if (!ValidateValidateCode())
        {
            Response.Write("验证码错误，请重新输入");
            return;
        }
        else
        {
            list = FindPwd.SelTypeFindPassWord(type, phone, out Msg);
            if (list[0].ToString()=="-30009")
            {
                Response.Write("voicePassword.aspx");
                return;
            }

            if (list[0].ToString() == "0")
            {
                string y = list[2].ToString();
                if (type == 1)
                {
                    massage = "您的语音密码为：" + y;
                    return;
                }
                else if (type == 2)
                {
                        massage = "您的Web密码为：" + y;
                }
                FindPwd.InsertFindPwdLog(list[1].ToString(), list[3].ToString(), Convert.ToString(type), "2", phone, 0, SPID, IP, "...", out Msg);
                //CommonBizRules.SendMessage(phone, massage, SPID);
                CommonBizRules.SendMessageV3(phone, massage, SPID);
                Response.Write("0");
            }
            else
            {
                Response.Write(list[4].ToString());
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

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

public partial class WebPassword : System.Web.UI.Page
{
    public string SPID = "35000000";
    public string ReturnUrl = "";
    public string CustID = "";
    public string OldPwd = "";
    public string VerifyPwd = "";
    public string ErrMsg = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        error.InnerHtml = "";
        err_code.InnerHtml = "";
        TokenValidate.Validate();
        CustID = TokenValidate.CustID;

        if(!Page.IsPostBack)
        {
            //this.CustID = "117663768";

            btn_OK.Attributes.Add("onclick", "return CheckInput('1')");
            if (CommonBizRules.IsUrlParams(HttpContext.Current.Request.Url.OriginalString))
            {
                SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
                ReturnUrl = HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"].ToString());
            }
        }
    }
    protected void btn_OK_Click(object sender, EventArgs e)
    {
        if (CommonBizRules.IsUrlParams(HttpContext.Current.Request.Url.OriginalString))
        {
            SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
            ReturnUrl = HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"].ToString());
        }

        if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["code"].ToString().Trim().ToUpper()),this.Context))
        {
            err_code.InnerHtml = "验证码错误，请重新输入";
            return;
        }
        OldPwd = Request.Form["oldPasswd"].ToString().Trim();
        VerifyPwd = Request.Form["verifyPasswd"].ToString().Trim();
        if (!PassWordBO.OldPwdIsRight(CustID, OldPwd, "2", out ErrMsg))
        {
            //CommonBizRules.ErrorHappenedRedircet(-1, ErrMsg, "修改登录密码", this.Context);
            error.InnerHtml = "原始密码错误";
            return;
        }
        else
        {
            string ErrMsgSetPwd = "";
            int Result = PassWordBO.SetPassword(SPID, CustID, VerifyPwd, "2", "", out ErrMsgSetPwd);
            if (Result != 0)
            {
                CommonBizRules.ErrorHappenedRedircet(Result, ErrMsgSetPwd, "修改登录密码", this.Context);
            }
            CommonBizRules.SuccessRedirect(ReturnUrl, "修改登录成功", this.Context);
        }
    }

    
}

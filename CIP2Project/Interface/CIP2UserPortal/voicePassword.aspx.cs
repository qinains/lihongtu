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

public partial class voicePassword : System.Web.UI.Page
{
    public string SPID = "35000000";
    public string ReturnUrl = "";
    public string CustID = "";
    public string OldPwd = "";
    public string VerifyPwd = "";
    public string ErrMsg = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        err_code.InnerHtml = "";
        error.InnerHtml = "";
        if(!Page.IsPostBack)
        {
            btn_OK.Attributes.Add("onclick", "return CheckInput('0')");
            TokenValidate.IsRedircet = true;
            TokenValidate.Validate();
            if (CommonBizRules.IsUrlParams(HttpContext.Current.Request.Url.OriginalString))
            {
                SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
                ReturnUrl = HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"].ToString());
            }
            CustID = TokenValidate.CustID;

            //语言密码为空
            if(PassWordBO.VoicePwdIsNull(CustID,out ErrMsg))
            {
                Label1.Text = "输入登录密码";
                error.InnerHtml = "提示：您未设置过语音密码 请输入登录密码";
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

        if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["code"].ToString().Trim().ToUpper()), this.Context))
        {
            err_code.InnerHtml = "验证码错误，请重新输入";
            return;
        }
        //TokenValidate.IsRedircet = false;
        TokenValidate.Validate();
        CustID = TokenValidate.CustID;

        OldPwd = Request.Form["oldPasswd"].ToString().Trim();
        VerifyPwd = Request.Form["verifyPasswd"].ToString().Trim();

        //语言密码为空
        if (PassWordBO.VoicePwdIsNull(CustID, out ErrMsg))
        {
            Label1.Text = "输入登录密码";
            if (!PassWordBO.OldPwdIsRight(CustID, OldPwd, "2", out ErrMsg))
            {
                //CommonBizRules.ErrorHappenedRedircet(-1, ErrMsg, "修改语音密码", this.Context);
                err_code.InnerHtml = "";
                error.InnerHtml = "原始密码错误";
                return;
            }
            else
            {
                string ErrMsgSetPwd = "";
                int Result = PassWordBO.SetPassword(SPID, CustID, VerifyPwd, "1", "", out ErrMsgSetPwd);
                if (Result != 0)
                {
                    CommonBizRules.ErrorHappenedRedircet(Result, ErrMsgSetPwd, "修改语音密码", this.Context);
                }
                CommonBizRules.SuccessRedirect(ReturnUrl, "修改语音密码成功", this.Context);
            }
        }
        else
        {
            if (!PassWordBO.OldPwdIsRight(CustID, OldPwd, "1", out ErrMsg))
            {
                //CommonBizRules.ErrorHappenedRedircet(-1, ErrMsg, "修改语音密码", this.Context);
                error.InnerHtml = "原始密码错误";
                return;
            }
            else
            {
                string ErrMsgSetPwd = "";
                int Result = PassWordBO.SetPassword(SPID, CustID, VerifyPwd, "1", "", out ErrMsgSetPwd);
                if (Result != 0)
                {
                    CommonBizRules.ErrorHappenedRedircet(Result, ErrMsgSetPwd, "修改语音密码", this.Context);
                }
                CommonBizRules.SuccessRedirect(ReturnUrl, "修改语音密码成功", this.Context);
            }
        }


    }

    

}

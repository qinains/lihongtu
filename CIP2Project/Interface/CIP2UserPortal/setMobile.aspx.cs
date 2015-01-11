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
public partial class setMobile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string mobile = null;
            string Spid = null;
            string Msg = "";

            TokenValidate.IsRedircet = true;
            TokenValidate.Validate();
            string CustID = TokenValidate.CustID;
            if (CustID != "")
            {
                this.custidtxt.Value = CustID;
            }
            Spid = Request["SPID"] == null ? String.Empty : Request["SPID"].ToString();
            //if (Request["SPID"] != null)
            //{
            //    if (Request["SPID"].ToString() != "35000000")
            //    {
            //        Spid = Request["SPID"].ToString();
            //    }
            //    else
            //    {
            //        Spid = "35000000";
            //    }
            //}
            //else
            //{
            //    Spid = "";
            //}
            if (Request["Phone"] != null)
            {
                mobile = Request["Phone"].ToString();
                this.verifyMobile.Value = mobile;
                return;
            }

            mobile = PhoneBO.SelPhoneNumV2(this.custidtxt.Value, Spid, out Msg);
            this.verifyMobile.Value = mobile;

        }
    }
    protected void urlRedirectButton_ServerClick(object sender, EventArgs e)
    {
        string returnurl = "";
        if (Request.QueryString["ReturnUrl"] != null)
        {
            returnurl = Request.QueryString["ReturnUrl"].ToString();
        }
        string Phone = this.verifyMobile.Value;
        CommonBizRules.SuccessRedirect(returnurl, "您已成功设置手机", HttpContext.Current);
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
            if (Request["codeId"] == null)
            {
                result = false;
            }
            else
            {
                string validStr = HttpUtility.HtmlDecode(Request.Form["code"].ToString().Trim().ToUpper());
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
                        if (DateTime.Now > DateTime.Parse(Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"].ToString()))
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
        }
        catch
        {
            result = false;
        }

        return result;
    }
    #endregion
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.Rule;

/// <summary>
/// 号码百事通账户页面基类(开通注册页面不用集成此类)
/// </summary>
//public class AccountBasePage:BasePage
public class AccountBasePage : BasePage
{
    Int32 result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    private String _bestPayAccount;
    public String BestPayAccount
    {
        get { return _bestPayAccount; }
    }

    private String _isNeedHeadFoot = "1";
    public String IsNeedHeadFoot
    {
        get { return _isNeedHeadFoot; }
        set { _isNeedHeadFoot = value; }
    }

    private String _spid = "35000000";
    public String SPID
    {
        get { return _spid; }
        set { _spid = value; }
    }

    private String _returnUrl;
    public String ReturnUrl
    {
        get { return _returnUrl; }
        set { _returnUrl = value; }
    }

    public AccountBasePage()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
        BesttoneAccount _besttoneAccount_entity = _besttoneAccount_dao.QueryByCustID(base.CustID);
        if (_besttoneAccount_entity == null)
        {
            String SPTokenRequestValue = HttpContext.Current.Request["SPTokenRequest"] == null ? String.Empty : HttpContext.Current.Request["SPTokenRequest"];
            if (!String.IsNullOrEmpty(SPTokenRequestValue))
                HttpContext.Current.Response.Redirect("AccountNotBind.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequestValue));
            else
                HttpContext.Current.Response.Redirect("AccountNotBind.aspx");

            return;
        }
        else
        {
            this._bestPayAccount = _besttoneAccount_entity.BestPayAccount;
        }
        //解析参数
        ParseSPTokenRequest();
    }

    /// <summary>
    /// 解析请求参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
        try
        {
            if (HttpContext.Current.Request["SPTokenRequest"]!=null)
            {
                String SPTokenRequest = HttpContext.Current.Request["SPTokenRequest"];

                String ErrMsg = String.Empty;
                String tempCustID = String.Empty;
                Int32 Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out _spid, out tempCustID, out _isNeedHeadFoot, out _returnUrl, out ErrMsg);
                if (Result != 0)
                {
                    CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "请求参数有误", this.Context);
                    return;
                }
                if (tempCustID != base.CustID)
                {
                    CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "登录信息有误，请重新登录", this.Context);
                    return;
                }
            }
        }
        catch { }
    }

}

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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

public partial class UserAccount_AccountRechargeMobile : System.Web.UI.Page
{
    Int32 result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
    BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();

    protected void Page_Load(object sender, EventArgs e)
    {

        CustID = Request["CustID"];
        SPID = Request["SPID"];
        ReturnUrl = Request["ReturnUrl"];

        this.hdCustID.Value = CustID;
        this.hdSPID.Value = SPID;
        this.hdReturnUrl.Value = ReturnUrl;

        if (String.IsNullOrEmpty(CustID))
        {
            CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "您尚未登录，请登录", this.Context);
            return;
        }

        BesttoneAccount _besttoneAccount_entity = _besttoneAccount_dao.QueryByCustID(CustID);
        if (_besttoneAccount_entity == null)
        {
           HttpContext.Current.Response.Redirect("AccountNotBind.aspx");
            return;
        }
        else
        {
            _bestPayAccount = _besttoneAccount_entity.BestPayAccount;
        }

        this.OnceRechargeLimited = BesttoneAccountHelper.ConvertAmountToYuan(BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedOne);
        this.DayRechargeLimited = BesttoneAccountHelper.ConvertAmountToYuan(BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedDay);
        this.AccountBalanceLimited = BesttoneAccountHelper.ConvertAmountToYuan(BesttoneAccountConstDefinition.DefaultInstance.AccountBalanceLimited);

    }

    public String CustID = String.Empty;
    public String SPID = String.Empty;
    public String ReturnUrl = String.Empty;

    private String _bestPayAccount;
    public String BestPayAccount
    {
        get { return _bestPayAccount; }
    }


    private string _onceRechargeLimited;
    /// <summary>
    /// 单笔充值上限
    /// </summary>
    public String OnceRechargeLimited
    {
        get { return _onceRechargeLimited; }
        set { _onceRechargeLimited = value; }
    }

    private string _dayRechargeLimited;
    /// <summary>
    /// 当日累计充值上限
    /// </summary>
    public String DayRechargeLimited
    {
        get { return _dayRechargeLimited; }
        set { _dayRechargeLimited = value; }
    }

    private string _accountBalanceLimited;
    /// <summary>
    /// 账户余额上限
    /// </summary>
    public String AccountBalanceLimited
    {
        get { return _accountBalanceLimited; }
        set { _accountBalanceLimited = value; }
    }


}

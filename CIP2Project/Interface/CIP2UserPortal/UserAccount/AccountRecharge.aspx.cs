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

public partial class UserAccount_AccountRecharge : AccountBasePage
{
    BesttoneAccountDAO _besttoneAccount_bo = new BesttoneAccountDAO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //测试用117663797
        //this.hdCustID.Value = "117663797";//116694907
        //this.hdSPID.Value = "35000000";
        //this.hdReturnUrl.Value = "http://www.118114.cn";

        this.hdCustID.Value = base.CustID;
        this.hdSPID.Value = base.SPID;
        this.hdReturnUrl.Value = base.ReturnUrl;
        this.hdHeadFoot.Value = base.IsNeedHeadFoot == "yes" ? "1" : "0";

        this.OnceRechargeLimited = BesttoneAccountHelper.ConvertAmountToYuan(BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedOne);
        this.DayRechargeLimited = BesttoneAccountHelper.ConvertAmountToYuan(BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedDay);
        this.AccountBalanceLimited = BesttoneAccountHelper.ConvertAmountToYuan(BesttoneAccountConstDefinition.DefaultInstance.AccountBalanceLimited);
    }

    /// <summary>
    /// 测试用
    /// </summary>
    //public String BestPayAccount
    //{
    //    get { return "18918790558"; }
    //}

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

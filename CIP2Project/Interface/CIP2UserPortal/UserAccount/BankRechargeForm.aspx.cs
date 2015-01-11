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

public partial class UserAccount_BankRechargeForm : System.Web.UI.Page
{
    private int result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
    BankRechargeRecordDAO _bankRechargeRecord_dao = new BankRechargeRecordDAO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                String custid = Request["hiddenCustID"];
                String spid = Request["hiddenSPID"];
                String InvoiceType = Request["InvoiceType"];
                String InvoiceTitle = Request["InvoiceTitle"];
                String InvoiceContent = Request["InvoiceContent"];
                String ContactPerson = Request["ContactPerson"];
                String ContactPhone = Request["ContactPhone"];
                String Address = Request["Address"];
                String Zip = Request["Zip"];
                String Mem = Request["Mem"];
                String NeedInvoice = Request["NeedInvoice"];


                long balance = Convert.ToInt64(Convert.ToDouble(Request["TranAmount"]) * 100);
                DateTime reqTime = DateTime.Now;

                //查询账户信息
                BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
                BesttoneAccount account_entity = _besttoneAccount_dao.QueryByCustID(custid);
                if (account_entity == null)
                {
                    CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "账户信息异常", this.Context);
                    return;
                }

                #region 账户充值金额上限校验

                long OnceRechargeLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedOne;              //单笔充值金额上限
                long RechargeAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedDay;            //账户单日充值额度上限
                long CurrentAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountBalanceLimited;                 //账户余额上限
                if (OnceRechargeLimit > 0)
                {
                    //检测用户单笔充值金额是否超限(10000元)
                    if (balance > OnceRechargeLimit)
                    {
                        result = 100003;
                        ErrMsg = String.Format("单笔充值金额最多不能超过{0}元", BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit));
                        CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "充值金额超过上限", this.Context);
                        return;
                    }
                }
                if (RechargeAmountLimit > 0)
                {
                    //检测用户当日充值是否超限(当日充值金额不能超过50000元)
                    long hadRechargeAmount = _rechargeOrder_dao.QueryCurrentRechargeAmount(account_entity.BestPayAccount);
                    if ((hadRechargeAmount + balance) > RechargeAmountLimit)
                    {
                        result = 100001;
                        ErrMsg = String.Format("您今日累计充值金额:{0}元，本次充值将超过您的当日累计充值限额:{1}元，请改日再进行充值操作!",
                            BesttoneAccountHelper.ConvertAmountToYuan(hadRechargeAmount), BesttoneAccountHelper.ConvertAmountToYuan(RechargeAmountLimit));
                        CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "充值金额超过上限", this.Context);
                        return;
                    }
                }

                if (CurrentAmountLimit > 0)
                {
                    long accountBalance = 0;
                    //检测用户帐户余额(个人账户余额不能超过100000元)
                    result = BesttoneAccountHelper.QueryAccountBalance(account_entity.BestPayAccount, out accountBalance, out ErrMsg);
                    if (result != 0)
                    {
                        CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "账户信息异常", this.Context);
                        return;
                    }

                    if ((accountBalance + balance) > CurrentAmountLimit)
                    {
                        result = 100002;
                        ErrMsg = String.Format("您的账户余额为:{0}元，本次充值将超过您的账户余额上限:{1}元，请消费后再进行充值操作!",
                            BesttoneAccountHelper.ConvertAmountToYuan(accountBalance), BesttoneAccountHelper.ConvertAmountToYuan(CurrentAmountLimit));
                        CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "充值金额超过上限", this.Context);
                        return;
                    }
                }

                #endregion

                bool Result = false;

                #region 生成订单信息

                /***************************************************************************/
                String orderSeq = BesttoneAccountHelper.CreateOrderSeq();                   //订单号
                String transactionID = BesttoneAccountHelper.CreateTransactionID();         //流水号
                // 在发起网银扣款请求，主表状态为1，扣款子表状态为0
                //初始化充值订单
                //2013-04-13 add -start 
                if (String.IsNullOrEmpty(NeedInvoice))
                {
                    NeedInvoice = "0";
                }
                //2013-04-13 add -end 
                RechargeOrder _recharge_order = new RechargeOrder(orderSeq, transactionID, reqTime.ToString("yyyyMMdd"), "RMB",
                    balance, balance, 0, "网银充值", custid, account_entity.BestPayAccount, "0", spid, reqTime, new DateTime(1900, 1, 1),
                    new DateTime(1900, 1, 1), 1, 0, "", "", "", NeedInvoice);  //2013-04-13 add NeedInvoice 字段

                //初始化网银扣款流水记录
                BankRechargeRecord _bankRecharge_record = new BankRechargeRecord(orderSeq, transactionID, reqTime.ToString("yyyyMMdd"), "RMB",
                    balance, balance, 0, "网银充值", account_entity.BestPayAccount, 0, reqTime, new DateTime(1900, 1, 1), "", "", "", "", "");

                Result = _rechargeOrder_dao.Insert(_recharge_order);
                if (!Result)
                {
                    CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "网银充值失败1", this.Context);
                    return;
                }

                //如果需要开发票 -- 插入发票表  2013-04-14 add-start
                if ("1".Equals(NeedInvoice))
                {
                    System.Text.StringBuilder datalog = new System.Text.StringBuilder();
                    //Result = _rechargeOrder_dao.InsertInvoice(orderSeq, InvoiceType, "", InvoiceTitle, ContactPerson, ContactPhone, Address, Zip, Mem, out datalog);
                    if ("0".Equals("InvoiceType"))
                    {
                        InvoiceTitle = "个人";
                    }
                    if (String.IsNullOrEmpty(InvoiceTitle))
                    {
                        InvoiceTitle = "个人";
                    }
                    if (String.IsNullOrEmpty(InvoiceContent))
                    {
                        InvoiceContent = "日用品";
                    }
                    Result = _rechargeOrder_dao.InsertInvoice(orderSeq, InvoiceType, InvoiceContent, InvoiceTitle, ContactPerson, ContactPhone, Address, Zip, Mem,"9", out datalog);  // "9" 代表需要开票
                    if (!Result)
                    {
                        CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "网银充值失败-发票生成失败", this.Context);
                        return;
                    }
                }
                //2013-04-14 add-end

                Result = _bankRechargeRecord_dao.Insert(_bankRecharge_record);
                if (!Result)
                {
                    CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "网银充值失败2", this.Context);
                    return;
                }
                /***************************************************************************/

                #endregion

                this._ORDERSEQ = orderSeq;
                this._ORDERREQTRANSEQ = transactionID;
                this._ORDERDATE = reqTime.ToString("yyyyMMddHHmmss");
                this._ORDERAMOUNT = balance.ToString();
                this._PRODUCTAMOUNT = balance.ToString();

                //MAC签名
                String mac = String.Format("MERCHANTID={0}&ORDERSEQ={1}&ORDERDATE={2}&ORDERAMOUNT={3}", this.MERCHANTID, this._ORDERSEQ, this._ORDERDATE, this._ORDERAMOUNT);
                mac = BesttoneAccountHelper.MACSign(mac);

                this._MAC = mac;
            }
            catch (Exception ex)
            {
                CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "网银充值失败", this.Context);
                return;
            }
        }
    }

    #region Properties

    public String MERCHANTID
    {
        get
        {
            return BesttoneAccountConstDefinition.DefaultInstance.MERCHANTID;
        }
    }

    public String SUBMERCHANTID
    {
        get
        {
            return BesttoneAccountConstDefinition.DefaultInstance.SUBMERCHANTID;
        }
    }

    private string _ORDERSEQ;
    /// <summary>
    /// 订单号
    /// </summary>
    public String ORDERSEQ
    {
        get { return _ORDERSEQ; }
        set { _ORDERSEQ = value; }
    }

    private string _ORDERREQTRANSEQ;
    /// <summary>
    /// 订单流水号
    /// </summary>
    public String ORDERREQTRANSEQ
    {
        get { return _ORDERREQTRANSEQ; }
        set { _ORDERREQTRANSEQ = value; }
    }

    private string _ORDERDATE;
    /// <summary>
    /// /订单日期
    /// </summary>
    public String ORDERDATE
    {
        get { return _ORDERDATE; }
        set { _ORDERDATE = value; }
    }

    private string _ORDERAMOUNT;
    /// <summary>
    /// 订单总额
    /// </summary>
    public string ORDERAMOUNT
    {
        get { return _ORDERAMOUNT; }
        set { _ORDERAMOUNT = value; }
    }

    private string _PRODUCTAMOUNT;
    /// <summary>
    /// 产品价格
    /// </summary>
    public String PRODUCTAMOUNT
    {
        get { return _PRODUCTAMOUNT; }
        set { _PRODUCTAMOUNT = value; }
    }

    private string _ATTACHAMOUNT = "0";
    /// <summary>
    /// 附加金额
    /// </summary>
    public String ATTACHAMOUNT
    {
        get { return _ATTACHAMOUNT; }
        set { _ATTACHAMOUNT = value; }
    }

    private string _ENCODETYPE = "1";
    /// <summary>
    /// 加密类型
    /// </summary>
    public String ENCODETYPE
    {
        get { return _ENCODETYPE; }
        set { _ENCODETYPE = value; }
    }

    /// <summary>
    /// 前台返回url
    /// </summary>
    public String MERCHANTURL
    {
        get
        {
            return BesttoneAccountConstDefinition.DefaultInstance.BesttoneMerchantURL;
        }
    }

    /// <summary>
    /// 后台返回url
    /// </summary>
    public String BACKMERCHANTURL
    {
        get
        {
            return BesttoneAccountConstDefinition.DefaultInstance.BesttoneBackMerchantURL;
        }
    }

    private string _MAC;
    public String MAC
    {
        get { return _MAC; }
        set { _MAC = value; }
    }

    #endregion


}

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
using System.Text;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

public partial class UserAccount_ajax_RechargeAjax : System.Web.UI.Page
{
    //订单充值操作类
    RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
    //卡扣款记录操作类
    CardRechargeRecordDAO _cardRechargeRecord_dao = new CardRechargeRecordDAO();
    //充值操作类
    AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();


    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder returnMsg = new StringBuilder();
        StringBuilder strLog = new StringBuilder();
        String logName = String.Empty;
        try
        {
            String type = Request["Type"];
            switch (type)
            {
                //查询卡余额
                case "QueryCardBalance":
                    returnMsg.Append(CardBalanceQuery());
                    break;
                //卡充值
                case "AccountRechargeByCard":
                    logName = "RechargeByCard";
                    returnMsg.Append(AccountRechargeByCard(out strLog));
                    break;
                case "QueryRechargeInfo":

                    break;
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("[{result:\"false\",info:\"异常:" + ex.Message + "\"}]");
            strLog.AppendFormat("，异常out：{0}", ex.Message);
        }
        finally
        {
            log(logName, strLog.ToString());
        }
        Response.Write(returnMsg.ToString());
        Response.Flush();
        Response.End();
    }


    /// <summary>
    /// 卡充值
    /// </summary>
    protected String AccountRechargeByCard(out StringBuilder strLog)
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        strLog = new StringBuilder();
        strLog.AppendFormat("【消费卡充值，DateTime:{0}】\r\n[参数]:", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //获取参数
        String spid = Request["SPID"];
        String custid = Request["CustID"];
        String cardNo = Request["CardNo"];
        String cardPwd = Request["CardPassword"];
        String cardType = Request["CardType"];
        strLog.AppendFormat("SPID:{0},CustID:{1},CardNo:{2},CardPwd:{3},CardType:{4}\r\n", spid, custid, cardNo, cardPwd, cardType);

        //String CheckCardErrMsg = "";
        //Int32 CheckCardResult = BesttoneAccountHelper.VerifyCardNo(cardNo, out CheckCardErrMsg);
        //if(CheckCardResult!=0)
        //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + CheckCardResult + "\",\"info\":\"" + CheckCardErrMsg + "}]";

        long accountBalance = 0;        //账户余额
        long cardBalance = 0;           //卡余额
        String accountType = BesttoneAccountHelper.ConvertAccountType(cardType);            //转换卡类型
        /*********************************************查询账户信息*****************************************************/
        BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
        BesttoneAccount account_entity = _besttoneAccount_dao.QueryByCustID(custid);


        #region 卡余额查询
        
        //查询卡余额
        Result = BesttoneAccountHelper.QueryCardBalance(cardNo, accountType, out cardBalance, out ErrMsg);
        strLog.AppendFormat("[查询卡余额]:Result:{0},Balance:{1}\r\n", Result, cardBalance);

        //查询失败
        if (Result != 0)
            return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + Result + "\",\"info\":\"查询余额失败\"}]";
        
        //卡余额为0
        if (cardBalance == 0)
            return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"200020\",\"info\":\"卡内余额为0\"}]";

        #endregion

        #region 账户充值金额上限校验

        long OnceRechargeLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedOne;              //单笔充值金额上限
        long RechargeAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedDay;            //账户单日充值额度上限
        long CurrentAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountBalanceLimited;                 //账户余额上限
        if (OnceRechargeLimit > 0)
        { 
            //检测用户单笔充值金额是否超限(10000元)
            if (cardBalance > OnceRechargeLimit)
                return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100003\",\"rechargeamount\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "\",\"rechargeamountlimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit) + "\",\"info\":\"卡内余额为0\"}]";
        }
        if (RechargeAmountLimit > 0)
        {
            //检测用户当日充值是否超限(当日充值金额不能超过50000元)
            long hadRechargeAmount = _rechargeOrder_dao.QueryCurrentRechargeAmount(account_entity.BestPayAccount);
            if ((hadRechargeAmount + cardBalance) > RechargeAmountLimit)
                return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100001\",\"rechargeamount\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(hadRechargeAmount) + "\",\"rechargeamountlimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(RechargeAmountLimit) + "\",\"info\":\"卡内余额为0\"}]";
        }

        if (CurrentAmountLimit > 0)
        {
            //检测用户帐户余额(个人账户余额不能超过100000元)
            Result = BesttoneAccountHelper.QueryAccountBalance(account_entity.BestPayAccount, out accountBalance, out ErrMsg);
            if (Result != 0)
                return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + Result + "\",\"info\":\"账户信息查询失败\"}]";

            if ((accountBalance + cardBalance) > CurrentAmountLimit)
                return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100002\",\"accountbalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\",\"CurrentAmountLimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(CurrentAmountLimit) + "\",\"info\":\"卡内余额为0\"}]";
            accountBalance = 0;
        }

        #endregion

        #region 卡扣款

        String transactionID = BesttoneAccountHelper.CreateTransactionID();
        String orderSeq = BesttoneAccountHelper.CreateOrderSeq();
        DateTime reqTime = DateTime.Now;

        RechargeOrder _recharge_order;              //充值订单
        CardRechargeRecord cardrecharge_entity;     //卡扣款流水记录

        //初始化充值订单
        _recharge_order = new RechargeOrder(orderSeq, transactionID, reqTime.ToString("yyyyMMdd"), "RMB", cardBalance, cardBalance, 0, "消费卡向账户充值扣款",
            custid, account_entity.BestPayAccount, cardType, spid, reqTime, new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), 1, 0, "", "", "","0");  //2013-04-13 add 最后一个字段 0 代表是否需要开票
        //初始化充值订单—测试
        //_recharge_order = new RechargeOrder(orderSeq, transactionID, reqTime.ToString("yyyyMMdd"), "RMB", 1, 1, 0, "消费卡向账户充值扣款",
        //    custid, account_entity.BestPayAccount, cardType, spid, reqTime, new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), 1, 0, "", "", "");


        //初始化卡扣款流水
        cardrecharge_entity = new CardRechargeRecord(transactionID, orderSeq, reqTime.ToString("yyyyMMdd"), "RMB", cardBalance, "消费卡向账户充值扣款", cardNo, cardPwd, cardType,
            account_entity.BestPayAccount, 0, reqTime, new DateTime(1900, 1, 1), "", "", "", "", "");
        //初始化卡扣款流水—测试
        //cardrecharge_entity = new CardRechargeRecord(transactionID, orderSeq, reqTime.ToString("yyyyMMdd"), "RMB", 1, "消费卡向账户充值扣款", cardNo, cardPwd, cardType,
        //    account_entity.BestPayAccount, 0, reqTime, new DateTime(1900, 1, 1), "", "", "", "", "");



        strLog.AppendFormat("[订单信息]：TransactionID:{0},OrderSeq:{1},ReqTime:{2}\r\n", transactionID, orderSeq, reqTime.ToString("yyyy-MM-dd HH:mm:ss"));
        
        /***********************************************************开始扣款*******************************************************/
        String uptranSeq = String.Empty;                        //交易流水号，支付平台返回的，后期对账用
        Result = BesttoneAccountHelper.CardDeductionBalance(transactionID, orderSeq, cardNo, cardPwd, cardType, cardBalance, reqTime, "", out uptranSeq, out ErrMsg);
        //扣款—测试
        //Result = BesttoneAccountHelper.CardDeductionBalance(transactionID, orderSeq, cardNo, cardPwd, cardType, 1, reqTime, "", out uptranSeq, out ErrMsg);
        strLog.AppendFormat("[卡扣款]:Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
        _recharge_order.UptranSeq = uptranSeq;
        cardrecharge_entity.UptranSeq = uptranSeq;
        if (Result != 0)
        {
            //修改订单信息
            _recharge_order.Status = 4;     // 这里要对调网关发生异常做分别处理 ，定位6 
            if (Result == -3024)
            {
                _recharge_order.Status = 6;
            }

            _recharge_order.PayTime = DateTime.Now;
            _recharge_order.ReturnCode = Result.ToString();
            _recharge_order.ReturnDesc = ErrMsg;
            _rechargeOrder_dao.Insert(_recharge_order);

            //修改卡扣款记录信息
            //cardrecharge_entity.Status = 2;   //为统一rechargeorder 和 cardrechargerecord 的状态值，这里做修改 2013-05-15
            cardrecharge_entity.Status = 4;
            if (Result == -3024)
            {
                cardrecharge_entity.Status = 6;
            }
            //以上和rechargeorder 的状态处理方式统一

            cardrecharge_entity.PayTime = DateTime.Now;
            cardrecharge_entity.ReturnCode = Result.ToString();
            cardrecharge_entity.ReturnDesc = ErrMsg;
            _cardRechargeRecord_dao.Insert(cardrecharge_entity);
            return "[{\"result\":\"false\",\"step\":\"deduction\",\"errorcode\":\"" + Result + "\",\"info\":\"卡扣款失败\"}]";
        }
        else
        {
            //修改订单信息
            _recharge_order.Status = 2;
            _recharge_order.PayTime = DateTime.Now;
            _recharge_order.ReturnCode = "0000";
            _recharge_order.ReturnDesc = "已扣款待充值";   // 原：扣款成功
            _rechargeOrder_dao.Insert(_recharge_order);

            //修改卡充值记录信息
            //cardrecharge_entity.Status = 1;      // 这里为和 rechargeorder 状态统一 ,修改为2
            cardrecharge_entity.Status = 2;
            cardrecharge_entity.PayTime = DateTime.Now;
            cardrecharge_entity.ReturnCode = "0000";
            cardrecharge_entity.ReturnDesc = "已扣款待充值";   // 原：扣款成功
            _cardRechargeRecord_dao.Insert(cardrecharge_entity);
        }

        #endregion

        #region 充值

        transactionID = BesttoneAccountHelper.CreateTransactionID();        //充值流水记录
        DateTime rechargeTime = DateTime.Now;                               //充值请求时间
        String returnMsg = String.Empty;
        bool resultBoolean = false;

        //初始化充值流水类
        AccountRechargeRecord rechargeRecord_entity = new AccountRechargeRecord(transactionID, rechargeTime.ToString("yyyyMMdd"), orderSeq,
            cardBalance, cardType, "消费卡充值", rechargeTime, new DateTime(1900, 1, 1), 0, "", "");

        try
        {
            #region 开始充值

            //调用接口给账户充值
            Result = BesttoneAccountHelper.AccountRecharge(transactionID, account_entity.BestPayAccount, cardBalance, rechargeTime, out accountBalance, out ErrMsg);
            //调用接口给账户充值—测试
            //Result = BesttoneAccountHelper.AccountRecharge(transactionID, account_entity.BestPayAccount, 1, rechargeTime, out accountBalance, out ErrMsg);
            strLog.AppendFormat("[账户充值]:TransactionID:{0},Result:{1},ErrMsg:{2}\r\n", transactionID, Result, ErrMsg);
            if (Result == 0)
            {
                //修改订单信息
                _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                _recharge_order.Status = 3;
                _recharge_order.RechargeCount = 1;
                _recharge_order.CompleteTime = DateTime.Now;
                _recharge_order.ReturnCode = "0000";
                _recharge_order.ReturnDesc = "已充值";    // 原：充值成功
                _recharge_order.RechargeTransactionID = transactionID;  // 回填充值流水号
                resultBoolean = _rechargeOrder_dao.Update(_recharge_order);

                //修改充值流水记录信息
                rechargeRecord_entity.Status = 3;    // 原: 1 ,为和总表rechargeorder 统一改为3
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = "0000";
                rechargeRecord_entity.ReturnDesc = "已充值";  // 原:充值成功

                strLog.AppendFormat("[更新订单状态]ResultBoolean:{0}\r\n", resultBoolean);
                returnMsg = "[{\"result\":\"true\",\"info\":\"账户充值成功\",\"deductionBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "\",\"accountBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\"}]";
            }
            else
            {
                //修改订单信息
                _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                _recharge_order.RechargeCount = 1;
                _recharge_order.Status = 5; // 原：没有这句
                if (Result == -3025)
                {
                    _recharge_order.Status = 7;  //原：没有这句                
                }
                _recharge_order.ReturnCode = Result.ToString();
                _recharge_order.ReturnDesc = ErrMsg;
                _recharge_order.RechargeTransactionID = transactionID;  // 回填充值流水号
                resultBoolean = _rechargeOrder_dao.Update(_recharge_order);

                //修改充值流水记录信息
                rechargeRecord_entity.Status = 5;    //   5 代表充值失败  原：2 

                if (Result == -3025)
                {
                    rechargeRecord_entity.Status = 7;// 充值异常
                }
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = Result.ToString();
                rechargeRecord_entity.ReturnDesc = ErrMsg;

                returnMsg = "[{\"result\":\"false\",\"step\":\"recharge\",\"errorcode\":\"" + Result + "\",\"info\":\"账户充值失败\"}]";
            }

            #endregion
        }
        catch (Exception ex)
        {
            rechargeRecord_entity.ReturnDesc += ex.Message;
            throw ex;
        }
        finally
        {
            _accountRechargeRecord_dao.Insert(rechargeRecord_entity);
        }

        #endregion


        return returnMsg;
    }

    /// <summary>
    /// 卡余额查询
    /// </summary>
    protected String CardBalanceQuery()
    {
        String cardNo = Request["CardNo"];
        String cardPwd = Request["CardPassword"];
        String cardType = Request["CardType"];

        long balance;
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        //查询余额
        Result = BesttoneAccountHelper.CardAuthen(cardNo, cardPwd, cardType, out balance, out ErrMsg);
        if (Result == 0)
            return "[{\"result\":\"true\",\"balance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(balance) + "\"}]";
        else
            return "[{\"result\":\"false\",\"info\":\"查询余额失败\"}]";
    }

    /// <summary>
    /// 记录日志
    /// </summary>
    protected void log(String name, string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog(name, msg);
    }

}

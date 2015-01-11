<%@ WebHandler Language="C#" Class="AccountRechargeHandler" %>

using System;
using System.Web;
using System.Text;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

public class AccountRechargeHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        StringBuilder returnMsg = new StringBuilder();
        StringBuilder strLog = new StringBuilder();
        String logName = String.Empty;
        try
        {
            String type = context.Request["Type"];
            switch (type)
            {
                case "QueryCardBalance":
                    returnMsg.Append(CardBalanceQuery(context));
                    break;
                case "AccountRechargeByCard":
                    logName = "RechargeByCard";
                    returnMsg.Append(AccountRechargeByCard(context, out strLog));
                    break;
                case "AccountRechargeByBank":
                    logName = "RechargeByBank";
                    returnMsg.Append(AccountRechargeByBank(context));
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
        context.Response.Write(returnMsg.ToString());
        context.Response.Flush();
        context.Response.End();
    }

    /// <summary>
    /// 卡充值
    /// </summary>
    public String AccountRechargeByCard(HttpContext context, out StringBuilder strLog)
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        
        strLog = new StringBuilder();
        strLog.AppendFormat("【消费卡充值，DateTime:{0}】\r\n[参数]:", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        
        String spid = context.Request["SPID"];
        String custid = context.Request["CustID"];
        String cardNo = context.Request["CardNo"];
        String cardPwd = context.Request["CardPassword"];
        String cardType = context.Request["CardType"];
        strLog.AppendFormat("SPID:{0},CustID:{1},CardNo:{2},CardPwd:{3},CardType:{4}\r\n", spid, custid, cardNo, cardPwd, cardType);

        /*********************************************查询账户信息*****************************************************/
        BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
        BesttoneAccount account_entity = _besttoneAccount_dao.QueryByCustID(custid);

        
        /***********************************************查询余额*******************************************************/
        long accountBalance = 0;        //账户余额
        long cardBalance = 0;           //卡余额，卡扣款时时全扣款

        String accountType = BesttoneAccountHelper.ConvertAccountType(cardType);
        //查询卡余额
        Result = BesttoneAccountHelper.QueryCardBalance(cardNo, accountType, out cardBalance, out ErrMsg);
        strLog.AppendFormat("[查询卡余额]:Result:{0},Balance:{1}\r\n", Result, cardBalance);

        if (Result != 0)
        {
            return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + Result + "\",\"info\":\"查询余额失败\"}]";
        }
        //卡余额为0
        //if (cardBalance == 0)
        //{
        //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"200020\",\"info\":\"卡内余额为0\"}]";
        //}

        String transactionID = BesttoneAccountHelper.CreateTransactionID();
        String orderSeq = BesttoneAccountHelper.CreateOrderSeq();
        DateTime reqTime = DateTime.Now;
        
        CardRechargeRecordDAO _cardRechargeRecord_dao = new CardRechargeRecordDAO();
        CardRechargeRecord cardrecharge_entity = new CardRechargeRecord();
        cardrecharge_entity.TransactionID = transactionID;
        cardrecharge_entity.OrderSeq = orderSeq;
        cardrecharge_entity.OrderDate = reqTime.ToString("yyyyMMdd");
        cardrecharge_entity.CurType = "RMB";
        cardrecharge_entity.OrderAmount = cardBalance;
        //cardrecharge_entity.OrderAmount = 1;
        cardrecharge_entity.OrderDesc = "消费卡向账户充值扣款";
        cardrecharge_entity.CardNo = cardNo;
        cardrecharge_entity.CardType = cardType;
        cardrecharge_entity.CardPwd = "";
        cardrecharge_entity.CustID = custid;
        cardrecharge_entity.TargetAccount = account_entity.BestPayAccount;
        cardrecharge_entity.SourceSPID = spid;
        cardrecharge_entity.ReqTime = reqTime;
        cardrecharge_entity.DeductionTime = new DateTime(1900, 1, 1);
        cardrecharge_entity.CompleteTime = new DateTime(1900, 1, 1);
        cardrecharge_entity.Status = 1;
        cardrecharge_entity.RechargeCount = 0;
        cardrecharge_entity.ReturnCode = "-42000";
        cardrecharge_entity.ReturnDesc = "未知错误";

        strLog.AppendFormat("[订单信息]：TransactionID:{0},OrderSeq:{1},ReqTime:{2}\r\n", transactionID, orderSeq, reqTime.ToString("yyyy-MM-dd HH:mm:ss"));

        /***********************************************************开始扣款*******************************************************/
        //卡扣款
        Result = BesttoneAccountHelper.CardDeductionBalance(transactionID, orderSeq, cardNo, cardPwd, cardType, cardBalance, reqTime, "", out ErrMsg);
        //Result = BesttoneAccountHelper.CardDeductionBalance(transactionID, orderSeq, cardNo, cardPwd, cardType, 1, reqTime, "", out ErrMsg);
        strLog.AppendFormat("[卡扣款]:Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);

        if (Result != 0)
        {
            //插入一条记录到数据库
            cardrecharge_entity.Status = 4;
            cardrecharge_entity.DeductionTime = DateTime.Now;
            cardrecharge_entity.ReturnCode = Result.ToString();
            cardrecharge_entity.ReturnDesc = ErrMsg;
            _cardRechargeRecord_dao.Insert(cardrecharge_entity);
            return "[{\"result\":\"false\",\"step\":\"deduction\",\"errorcode\":\"" + Result + "\",\"info\":\"卡扣款失败\"}]";
        }
        else
        {
            //插入一条记录到数据库
            cardrecharge_entity.Status = 2;
            cardrecharge_entity.DeductionTime = DateTime.Now;
            cardrecharge_entity.ReturnCode = "0000";
            cardrecharge_entity.ReturnDesc = "扣款成功";
            _cardRechargeRecord_dao.Insert(cardrecharge_entity);
        }

        /*************************************************************开始充值******************************************************/
        transactionID = BesttoneAccountHelper.CreateTransactionID();
        DateTime rechargeTime = DateTime.Now;
        
        AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();
        AccountRechargeRecord rechargeRecord_entity = new AccountRechargeRecord();
        rechargeRecord_entity.RechargeTransactionID = transactionID;
        rechargeRecord_entity.RechargeDate = rechargeTime.ToString("yyyyMMdd");
        rechargeRecord_entity.OrderSeq = orderSeq;
        rechargeRecord_entity.OrderAmount = cardBalance;
        //rechargeRecord_entity.OrderAmount = 1;
        rechargeRecord_entity.RechargeType = cardType;
        rechargeRecord_entity.OrderDesc = "消费卡充值";
        rechargeRecord_entity.ReqTime = rechargeTime;
        rechargeRecord_entity.CompleteTime = new DateTime(1900, 1, 1);
        rechargeRecord_entity.Status = 0;
        rechargeRecord_entity.ReturnCode = "-42000";
        rechargeRecord_entity.ReturnDesc = "未知错误";
        
        String returnMsg = String.Empty;
        try
        {
            //调用接口给账户充值
            Result = BesttoneAccountHelper.AccountRecharge(transactionID,account_entity.BestPayAccount, cardBalance, rechargeTime, out accountBalance, out ErrMsg);
            //Result = BesttoneAccountHelper.AccountRecharge(transactionID, account_entity.BestPayAccount, 1, rechargeTime, out accountBalance, out ErrMsg);
            strLog.AppendFormat("[账户充值]:TransactionID:{0},Result:{1},ErrMsg:{2}\r\n", transactionID, Result, ErrMsg);
            if (Result == 0)
            {
                rechargeRecord_entity.Status = 1;
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = "0000";
                rechargeRecord_entity.ReturnDesc = "充值成功";
                //充值成功
                cardrecharge_entity = _cardRechargeRecord_dao.QueryByOrderSeq(orderSeq);
                cardrecharge_entity.Status = 3;
                cardrecharge_entity.RechargeCount = 1;
                cardrecharge_entity.CompleteTime = DateTime.Now;
                cardrecharge_entity.ReturnCode = "0000";
                cardrecharge_entity.ReturnDesc = "充值成功";
                _cardRechargeRecord_dao.Update(cardrecharge_entity);
                returnMsg = "[{\"result\":\"true\",\"info\":\"账户充值成功\",\"deductionBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "\",\"accountBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\"}]";
                //returnMsg = "[{\"result\":\"true\",\"info\":\"账户充值成功\",\"deductionBalance\":\"0.01\",\"balance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\"}]";
            }
            else
            {
                rechargeRecord_entity.Status = 2;
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = Result.ToString();
                rechargeRecord_entity.ReturnDesc = ErrMsg;
                //充值失败——交由后台程序去继续充值
                cardrecharge_entity = _cardRechargeRecord_dao.QueryByOrderSeq(orderSeq);
                cardrecharge_entity.RechargeCount = 1;
                cardrecharge_entity.ReturnCode = Result.ToString();
                cardrecharge_entity.ReturnDesc = ErrMsg;
                _cardRechargeRecord_dao.Update(cardrecharge_entity);
                returnMsg = "[{\"result\":\"false\",\"step\":\"recharge\",\"errorcode\":\"" + Result + "\",\"info\":\"账户充值失败\"}]";
            }
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

        return returnMsg;
    }

    /// <summary>
    /// 卡余额查询
    /// </summary>
    public String CardBalanceQuery(HttpContext context)
    {
        String cardNo = context.Request["CardNo"];
        String cardPwd = context.Request["CardPassword"];
        String cardType = context.Request["CardType"];

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
    /// 银行卡充值——新增账单
    /// </summary>
    public String AccountRechargeByBank(HttpContext context)
    {
        StringBuilder returnMsg = new StringBuilder();

        String custid = context.Request["CustID"];
        String spid = context.Request["SPID"];
        long balance = Convert.ToInt64(Convert.ToDouble(context.Request["Balance"]) * 100);
        //查询账户信息
        BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
        BesttoneAccount account_entity = _besttoneAccount_dao.QueryByCustID(custid);
        
        bool Result = false;

        //生成账单插入数据库
        BankRechargeOrderDAO _bankRechargeOrder_DAO = new BankRechargeOrderDAO();
        BankRechargeOrder recharge_order = new BankRechargeOrder();
        String orderSeq = BesttoneAccountHelper.CreateOrderSeq();                   //订单号
        String transactionID = BesttoneAccountHelper.CreateTransactionID();         //流水号

        recharge_order.OrderSeq = orderSeq;
        recharge_order.OrderTransactionID = transactionID;
        recharge_order.CustID = custid;
        recharge_order.TargetAccount = account_entity.BestPayAccount;
        recharge_order.SourceSPID = spid;
        recharge_order.OrderDate = DateTime.Now.ToString("yyyyMMdd");
        recharge_order.CurType = "RMB";
        recharge_order.OrderAmount = balance;
        recharge_order.ProductAmount = balance;
        recharge_order.AttachAmount = 0;
        recharge_order.OrderDesc = "描述";
        recharge_order.ReqTime = DateTime.Now;
        recharge_order.DeductionTime = new DateTime(1900, 1, 1);
        recharge_order.CompleteTime = new DateTime(1900, 1, 1);
        recharge_order.Status = 1;                  //1:待扣款，2:已扣款,3:已充值,4:充值失败
        recharge_order.RechargeCount = 0;

        //插入一条银行卡扣款账单
        Result = _bankRechargeOrder_DAO.Insert(recharge_order);
        if (!Result)
            return "[{\"result\":\"false\",\"info\":\"新增账单失败\"}]";

        //MD5加密
        String mac = String.Format("MERCHANTID={0}&ORDERSEQ={1}&ORDERDATE={2}&ORDERAMOUNT={3}", "3100000026", orderSeq, recharge_order.OrderDate, recharge_order.OrderAmount);
        mac = BitConverter.ToString(CryptographyUtil.MD5Encrypt(mac)).Replace("-", "");

        ////返回订单信息
        StringBuilder str = new StringBuilder();
        str.Append("\"result\":\"true\",");
        str.AppendFormat("\"OrderSeq\":\"{0}\",", orderSeq);
        str.AppendFormat("\"OrderTransactionID\":\"{0}\",", transactionID);
        str.AppendFormat("\"OrderDate\":\"{0}\",", recharge_order.OrderDate);
        str.AppendFormat("\"OrderAmount\":\"{0}\",", recharge_order.OrderAmount);
        str.AppendFormat("\"ProductAmount\":\"{0}\",", recharge_order.ProductAmount);
        str.AppendFormat("\"AttachAmount\":\"{0}\",", recharge_order.AttachAmount);
        str.AppendFormat("\"OrderDesc\":\"{0}\",", recharge_order.OrderDesc);
        str.AppendFormat("\"Mac\":\"{0}\"", mac);
        
        return "[{" + str.ToString() + "}]";
    }

    protected void log(String name,string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog(name, msg);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}   
﻿<%@ WebHandler Language="C#" Class="AccountRechargeHandler" %>

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
                //查询卡余额
                case "QueryCardBalance":
                    returnMsg.Append(CardBalanceQuery(context));
                    break;
                //卡充值
                case "AccountRechargeByCard":
                    logName = "RechargeByCard";
                    returnMsg.Append(AccountRechargeByCard(context, out strLog));
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
        long cardBalance = 0;           //卡余额

        String accountType = BesttoneAccountHelper.ConvertAccountType(cardType);
        //查询卡余额
        Result = BesttoneAccountHelper.QueryCardBalance(cardNo, accountType, out cardBalance, out ErrMsg);
        strLog.AppendFormat("[查询卡余额]:Result:{0},Balance:{1}\r\n", Result, cardBalance);

        if (Result != 0)
        {
            return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + Result + "\",\"info\":\"查询余额失败\"}]";
        }
        //卡余额为0
        if (cardBalance == 0)
        {
            return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"200020\",\"info\":\"卡内余额为0\"}]";
        }
        
        /**********************************充值上限验证_开始*************************************/
        RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
        CardRechargeRecordDAO _cardRechargeRecord_dao = new CardRechargeRecordDAO();
        
        //账户单日充值额度上限
        long RechargeAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedDay;
        //账户余额上限
        long CurrentAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountBalanceLimited;
        if (RechargeAmountLimit > 0 && CurrentAmountLimit > 0)
        {
            //检测用户当日充值是否超限(当日充值金额不能超过3000元)
            long hadRechargeAmount = _rechargeOrder_dao.QueryCurrentRechargeAmount(account_entity.BestPayAccount);
            if ((hadRechargeAmount + cardBalance) > RechargeAmountLimit)
                return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100001\",\"rechargeamount\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(hadRechargeAmount) + "\",\"rechargeamountlimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(RechargeAmountLimit) + "\",\"info\":\"卡内余额为0\"}]";

            //检测用户帐户余额(个人账户余额不能超过4999元)
            Result = BesttoneAccountHelper.QueryAccountBalance(account_entity.BestPayAccount, out accountBalance, out ErrMsg);
            if (Result != 0)
                return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + Result + "\",\"info\":\"账户信息查询失败\"}]";
            if ((accountBalance + cardBalance) > CurrentAmountLimit)
                return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100002\",\"accountbalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\",\"CurrentAmountLimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(CurrentAmountLimit) + "\",\"info\":\"卡内余额为0\"}]";

            accountBalance = 0;
        }

        /**********************************充值上限验证_结束************************************/
        
        String transactionID = BesttoneAccountHelper.CreateTransactionID();
        String orderSeq = BesttoneAccountHelper.CreateOrderSeq();
        DateTime reqTime = DateTime.Now;
        
        RechargeOrder _recharge_order;              //充值订单
        CardRechargeRecord cardrecharge_entity;     //卡扣款流水记录

        _recharge_order = new RechargeOrder(orderSeq, transactionID, reqTime.ToString("yyyyMMdd"), "RMB", cardBalance, cardBalance, 0, "消费卡向账户充值扣款",
            custid, account_entity.BestPayAccount, cardType, spid, reqTime, new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), 1, 0, "", "", "","0");
        cardrecharge_entity = new CardRechargeRecord(transactionID, orderSeq, reqTime.ToString("yyyyMMdd"), "RMB", cardBalance, "消费卡向账户充值扣款", cardNo, cardPwd, cardType,
            account_entity.BestPayAccount, 0, reqTime, new DateTime(1900, 1, 1), "", "", "", "", "");

        strLog.AppendFormat("[订单信息]：TransactionID:{0},OrderSeq:{1},ReqTime:{2}\r\n", transactionID, orderSeq, reqTime.ToString("yyyy-MM-dd HH:mm:ss"));

        /***********************************************************开始扣款*******************************************************/
        //卡扣款
        String uptranSeq = String.Empty;
        Result = BesttoneAccountHelper.CardDeductionBalance(transactionID, orderSeq, cardNo, cardPwd, cardType, cardBalance, reqTime, "", out uptranSeq, out ErrMsg);
        //Result = BesttoneAccountHelper.CardDeductionBalance(transactionID, orderSeq, cardNo, cardPwd, cardType, 1, reqTime, "", out uptranSeq, out ErrMsg);
        _recharge_order.UptranSeq = uptranSeq;
        cardrecharge_entity.UptranSeq = uptranSeq;
        strLog.AppendFormat("[卡扣款]:Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
        if (Result != 0)
        {
            //修改订单信息
            _recharge_order.Status = 4;
            _recharge_order.PayTime = DateTime.Now;
            _recharge_order.ReturnCode = Result.ToString();
            _recharge_order.ReturnDesc = ErrMsg;
            _rechargeOrder_dao.Insert(_recharge_order);
            
            //修改卡扣款记录信息
            cardrecharge_entity.Status = 2;
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
            _recharge_order.ReturnDesc = "扣款成功";
            _rechargeOrder_dao.Insert(_recharge_order);

            //修改卡充值记录信息
            cardrecharge_entity.Status = 1;
            cardrecharge_entity.PayTime = DateTime.Now;
            cardrecharge_entity.ReturnCode = "0000";
            cardrecharge_entity.ReturnDesc = "扣款成功";
            _cardRechargeRecord_dao.Insert(cardrecharge_entity);
        }

        /*************************************************************开始充值******************************************************/
        transactionID = BesttoneAccountHelper.CreateTransactionID();
        DateTime rechargeTime = DateTime.Now;
        
        AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();
        AccountRechargeRecord rechargeRecord_entity = new AccountRechargeRecord(transactionID, rechargeTime.ToString("yyyyMMdd"), orderSeq,
            cardBalance, cardType, "消费卡充值", rechargeTime, new DateTime(1900, 1, 1), 0, "", "");

        String returnMsg = String.Empty;
        try
        {
            bool resultBoolean = false;
            //调用接口给账户充值
            Result = BesttoneAccountHelper.AccountRecharge(transactionID,account_entity.BestPayAccount, cardBalance, rechargeTime, out accountBalance, out ErrMsg);
            strLog.AppendFormat("[账户充值]:TransactionID:{0},Result:{1},ErrMsg:{2}\r\n", transactionID, Result, ErrMsg);
            if (Result == 0)
            {
                //修改订单信息
                _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                _recharge_order.Status = 3;
                _recharge_order.RechargeCount = 1;
                _recharge_order.CompleteTime = DateTime.Now;
                _recharge_order.ReturnCode = "0000";
                _recharge_order.ReturnDesc = "充值成功";
                resultBoolean = _rechargeOrder_dao.Update(_recharge_order);
                
                //修改充值流水记录信息
                rechargeRecord_entity.Status = 1;
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = "0000";
                rechargeRecord_entity.ReturnDesc = "充值成功";
                
                strLog.AppendFormat("[更新订单状态]ResultBoolean:{0}\r\n", resultBoolean);
                returnMsg = "[{\"result\":\"true\",\"info\":\"账户充值成功\",\"deductionBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "\",\"accountBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\"}]";
            }
            else
            {
                //修改订单信息
                _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                _recharge_order.RechargeCount = 1;
                _recharge_order.ReturnCode = Result.ToString();
                _recharge_order.ReturnDesc = ErrMsg;
                resultBoolean = _rechargeOrder_dao.Update(_recharge_order);
                
                //修改充值流水记录信息
                rechargeRecord_entity.Status = 2;
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = Result.ToString();
                rechargeRecord_entity.ReturnDesc = ErrMsg;
                
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
            try
            {
                _accountRechargeRecord_dao.Insert(rechargeRecord_entity);
            }
            catch { }
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

    protected String QueryAccountAmount(HttpContext context)
    {
        String account = context.Request["CustID"];

        return String.Empty;
    }
    
    /// <summary>
    /// 查询账户当日已充值金额
    /// </summary>
    protected long QueryCurrentRechargeAmount(String account)
    {
        long amount = 0;
        RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
        amount = _rechargeOrder_dao.QueryCurrentRechargeAmount(account);
        return amount;
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
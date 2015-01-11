using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using log4net;
public partial class UserAccount_ajax_QueryInfoAjax : System.Web.UI.Page
{
    //账户充值流水记录操作类
    AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();

    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder returnMsg = new StringBuilder();
        try
        {
            String type = Request["Type"];
            switch (type)
            {
                    //获取进三月交易明细
                case "GetThreeMonthsDetails":
                    returnMsg.Append(GetThreeMonthsHistoryDetails());
                    break;
                    //获取三月之前交易明细
                case "GetThreeMonthsBeforeDetails":
                    returnMsg.Append(GetThreeMonthsBeforeHistoryDetails());
                    break;
                    //账户余额查询
                case "QueryAcountBalance":
                    returnMsg.Append(QueryAcountBalance());
                    break;
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("[{result:\"false\",info:\"异常:" + ex.Message + "\"}]");
        }
        Response.Write(returnMsg.ToString());
        Response.Flush();
        Response.End();
    }


    /// <summary>
    /// 获取近三个月交易明细
    /// </summary>
    protected String GetThreeMonthsHistoryDetails()
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        StringBuilder returnMsg = new StringBuilder();
        DateTime startDate = new DateTime(DateTime.Now.AddMonths(-2).Year, DateTime.Now.AddMonths(-2).Month, 1);
        DateTime endDate = DateTime.Now;
        
        try
        {
            String bestPayAccount = Request["BestPayAccount"];

            List<TxnItem> list = new List<TxnItem>();
            IList<TxnItem> list1, list2;

            Result = BesttoneAccountHelper.QueryAllTxnCurrentDay(bestPayAccount, out list1, out ErrMsg);
            Result = BesttoneAccountHelper.QueryAllTxnHistory(startDate, endDate, bestPayAccount, 10000, 1, out list2, out ErrMsg);

            if (list1 != null && list1.Count > 0)
            {
                foreach (TxnItem item in list1)
                {
                    list.Add(item);
                }
            }

            if (list2 != null && list2.Count > 0)
            {
                foreach (TxnItem item in list2)
                {
                    list.Add(item);
                }
            }
            //测试数据
            //list = CreateTestList();
            if (list != null && list.Count > 0)
            {
                list.Sort();
                foreach (TxnItem item in list)
                {
                    //如果是交易类型是“退货退还手续费”记录，则过滤掉不显示
                    if (item.TxnType == "261020")
                        continue;

                    String sign = "＋";
                    String txnType = BesttoneAccountHelper.ConvertTxnType(item.TxnType);
                    String flag = item.CancelFlag == "1" ? "已撤销" : "成功";
                    if (item.TxnType == "131090")
                        sign = "－";

                    AccountRechargeRecord _recharge_redcord = _accountRechargeRecord_dao.QueryByTransactionID(item.AcceptSeqNO);

                    returnMsg.Append("{");
                    returnMsg.AppendFormat("\"AcceptSeqNO\":\"{0}\",", item.AcceptSeqNO);
                    returnMsg.AppendFormat("\"AcceptDate\":\"{0}\",", item.AcceptDate);
                    returnMsg.AppendFormat("\"AcceptTime\":\"{0}\",", item.AcceptTime);
                    returnMsg.AppendFormat("\"TxnAmount\":\"{0}\",", sign + BesttoneAccountHelper.ConvertAmountToYuan(item.TxnAmount));
                    returnMsg.AppendFormat("\"TxnType\":\"{0}\",", txnType);
                    returnMsg.AppendFormat("\"TxnChannel\":\"{0}\",", item.TxnChannel);
                    returnMsg.AppendFormat("\"MerchantName\":\"{0}\",", item.MerchantName);
                    returnMsg.AppendFormat("\"TxnDscpt\":\"{0}\",", item.TxnDscpt);
                    returnMsg.AppendFormat("\"CancelFlag\":\"{0}\",", flag);
                    if (_recharge_redcord != null)
                        returnMsg.AppendFormat("\"OrderSeq\":\"{0}\",", _recharge_redcord.OrderSeq);
                    returnMsg.AppendFormat("\"TxnTime\":\"{0}\"", item.TxnTime);

                    returnMsg.Append("},");
                }
                returnMsg.Insert(0, "{\"result\":\"true\",\"txninfo\":[");
                returnMsg = returnMsg.Remove(returnMsg.Length - 1, 1);
                returnMsg.Append("]}");
            }
            else
            {
                returnMsg.Append("{\"result\":\"NoData\"}");
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("{\"result\":\"false\",\"info\":\"异常:" + ex.Message + "\"}");
        }

        return returnMsg.ToString();
    }

    /// <summary>
    /// 获取三个月之前交易明细
    /// </summary>
    protected String GetThreeMonthsBeforeHistoryDetails()
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        StringBuilder returnMsg = new StringBuilder();
        DateTime startDate = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
        DateTime endDate = new DateTime(DateTime.Now.AddMonths(-2).Year, DateTime.Now.AddMonths(-2).Month, 1).AddDays(-1);

        try
        {
            String bestPayAccount = Request["BestPayAccount"];
            IList<TxnItem> list = null;
            Result = BesttoneAccountHelper.QueryAllTxnHistory(startDate, endDate, bestPayAccount, 10000, 1, out list, out ErrMsg);
            //测试
            //List<TxnItem> list = CreateTestList();
            if (list != null && list.Count > 0)
            {
                //list.Sort();
                foreach (TxnItem item in list)
                {
                    //如果是交易类型是“退货退还手续费”记录，则过滤掉不显示
                    if (item.TxnType == "261020")
                        continue;

                    //根据流水号查询订单
                    AccountRechargeRecord _recharge_redcord = _accountRechargeRecord_dao.QueryByTransactionID(item.AcceptSeqNO);

                    String sign = "＋";
                    String txnType = BesttoneAccountHelper.ConvertTxnType(item.TxnType);
                    String flag = item.CancelFlag == "1" ? "已撤销" : "成功";
                    if (item.TxnType == "131090")
                        sign = "－";

                    returnMsg.Append("{");
                    returnMsg.AppendFormat("\"AcceptSeqNO\":\"{0}\",", item.AcceptSeqNO);
                    returnMsg.AppendFormat("\"AcceptDate\":\"{0}\",", item.AcceptDate);
                    returnMsg.AppendFormat("\"AcceptTime\":\"{0}\",", item.AcceptTime);
                    returnMsg.AppendFormat("\"TxnAmount\":\"{0}\",", sign + BesttoneAccountHelper.ConvertAmountToYuan(item.TxnAmount));
                    returnMsg.AppendFormat("\"TxnType\":\"{0}\",", txnType);
                    returnMsg.AppendFormat("\"TxnChannel\":\"{0}\",", item.TxnChannel);
                    returnMsg.AppendFormat("\"MerchantName\":\"{0}\",", item.MerchantName);
                    returnMsg.AppendFormat("\"TxnDscpt\":\"{0}\",", item.TxnDscpt);
                    returnMsg.AppendFormat("\"CancelFlag\":\"{0}\",", flag);
                    //查询订单号拼接上
                    if (_recharge_redcord != null)
                        returnMsg.AppendFormat("\"OrderSeq\":\"{0}\",", _recharge_redcord.OrderSeq);
                    returnMsg.AppendFormat("\"TxnTime\":\"{0}\"", item.TxnTime);
                    returnMsg.Append("},");
                }
                returnMsg.Insert(0, "{\"result\":\"true\",\"txninfo\":[");
                returnMsg = returnMsg.Remove(returnMsg.Length - 1, 1);
                returnMsg.Append("]}");
            }
            else
            {
                returnMsg.Append("{\"result\":\"NoData\"}");
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("{\"result\":\"false\",\"info\":\"异常\"}");
        }

        return returnMsg.ToString();
    }

    /// <summary>
    /// 暂时不用
    /// </summary>
    protected List<TxnItem> QueryCurrentDayDetails()
    {

        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        List<TxnItem> list = new List<TxnItem>();
        IList<TxnItem> txnList_business_currentDay, txnList_recharge_currentDay, txnList_refund_currentDay;

        String bestPayAccount = Request["BestPayAccount"];
        //当日消费记录
        Result = BesttoneAccountHelper.QueryBusinessTxnCurrentDay(bestPayAccount, out txnList_business_currentDay, out ErrMsg);
        //当日充值记录
        Result = BesttoneAccountHelper.QueryRechargeTxnCurrentDay(bestPayAccount, out txnList_recharge_currentDay, out ErrMsg);
        //当日退款记录
        Result = BesttoneAccountHelper.QueryRefundTxnICurrentDay(bestPayAccount, out txnList_refund_currentDay, out ErrMsg);

        if (txnList_business_currentDay != null && txnList_business_currentDay.Count > 0)
        {
            foreach (TxnItem item in txnList_business_currentDay)
            {
                list.Add(item);
            }
        }

        if (txnList_recharge_currentDay != null && txnList_recharge_currentDay.Count > 0)
        {
            foreach (TxnItem item in txnList_recharge_currentDay)
            {
                list.Add(item);
            }
        }

        if (txnList_refund_currentDay != null && txnList_refund_currentDay.Count > 0)
        {
            foreach (TxnItem item in txnList_refund_currentDay)
            {
                list.Add(item);
            }
        }

        return list;
    }

    /// <summary>
    /// 暂时不用
    /// </summary>
    protected List<TxnItem> QueryHistoryDetails(DateTime startDate, DateTime endDate)
    {
        StringBuilder returnMsg = new StringBuilder();

        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        //历史交易记录只查询近2年的数据,所以从去年的1月1日到距离现在三个月前的那一天
        String bestPayAccount = Request["BestPayAccount"];

        List<TxnItem> list = new List<TxnItem>();
        IList<TxnItem> txnList_business, txnList_recharge, txnList_refund;

        //历史消费记录
        Result = BesttoneAccountHelper.QueryBusinessTxnHistory(startDate, endDate, bestPayAccount, 0, 0, out txnList_business, out ErrMsg);
        //历史充值记录
        Result = BesttoneAccountHelper.QueryRechargeTxnHistory(startDate, endDate, bestPayAccount, 0, 0, out txnList_recharge, out ErrMsg);
        //历史退款记录
        Result = BesttoneAccountHelper.QueryRefundTxnHistory(startDate, endDate, bestPayAccount, 0, 0, out txnList_refund, out ErrMsg);

        //历史交易记录
        if (txnList_business != null && txnList_business.Count > 0)
        {
            foreach (TxnItem item in txnList_business)
            {
                list.Add(item);
            }
        }

        if (txnList_recharge != null && txnList_recharge.Count > 0)
        {
            foreach (TxnItem item in txnList_recharge)
            {
                list.Add(item);
            }
        }

        if (txnList_refund != null && txnList_refund.Count > 0)
        {
            foreach (TxnItem item in txnList_refund)
            {
                list.Add(item);
            }
        }

        return list;

    }

    /// <summary>
    /// 测试方法
    /// </summary>
    protected List<TxnItem> CreateTestList()
    {
        List<TxnItem> list = new List<TxnItem>();

        for (Int32 i = 0; i < 20; i++)
        {
            String str = i < 10 ? "0" + i.ToString() : i.ToString();
            TxnItem item = new TxnItem();
            item.AcceptDate = DateTime.Now.ToString("yyyyMM") + str;
            item.TxnAmount = 1;
            item.TxnChannel = "02";
            item.TxnDscpt = "上海营业厅1充值";
            item.TxnType = "01";
            item.MerchantName = "上海营业厅1";

            list.Add(item);
        }

        return list;
    }

    /// <summary>
    /// 账户余额查询
    /// </summary>
    protected string QueryAcountBalance()
    {
        String bestPayAccount = Request["BestPayAccount"];

        long balance;
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        //查询余额
        Result = BesttoneAccountHelper.QueryAccountBalance(bestPayAccount, out balance, out ErrMsg);
        if (Result == 0)
            return "[{\"result\":\"true\",\"balance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(balance) + "\"}]";
        else
            return "[{\"result\":\"false\",\"info\":\"查询余额失败\"}]";
    }
}

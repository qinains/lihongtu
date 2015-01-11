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
public partial class UserAccount_DetailQueryForMobileHttp : System.Web.UI.Page
{

    BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
    //账户充值流水记录操作类
    AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();
    protected void Page_Load(object sender, EventArgs e)
    {

        String SPID = Request["SPID"];
        String CustID = Request["CustID"];
        String QType = Request["QType"];    // 1 代表近3个月，包含当天 2,代表3个月前
        String wt = Request["wt"];   // json or xml

        String ResponseText = String.Empty;

        if ("2".Equals(QType))
        {
            ResponseText = GetThreeMonthsBeforeHistoryDetails(SPID, CustID, wt);
        }
        else
        {
            ResponseText = GetThreeMonthsHistoryDetails(SPID, CustID, wt);
        }


        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

      
    }


    /// <summary>
    /// 获取近三个月交易明细
    /// </summary>
    protected String GetThreeMonthsHistoryDetails(String SPID,String CustID,String wt)
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        StringBuilder returnMsg = new StringBuilder();
        DateTime startDate = new DateTime(DateTime.Now.AddMonths(-2).Year, DateTime.Now.AddMonths(-2).Month, 1);
        DateTime endDate = DateTime.Now;

        wt = "json";    //仅支持json

        if (String.IsNullOrEmpty(SPID))
        {
            returnMsg.Length = 0;
            if ("json".Equals(wt))
            {
                returnMsg.Append("{");
                returnMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                returnMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                returnMsg.Append("}");
            }
            else
            {
                returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                returnMsg.Append("<PayPlatRequestParameter>");
                returnMsg.Append("<PARAMETERS>");
                returnMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                returnMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                returnMsg.Append("</PARAMETERS>");
                returnMsg.Append("</PayPlatRequestParameter>");
            }
            return returnMsg.ToString();
        }


        if (String.IsNullOrEmpty(CustID))
        {
            returnMsg.Length = 0;
            if ("json".Equals(wt))
            {
                returnMsg.Append("{");
                returnMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                returnMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
                returnMsg.Append("}");
            }
            else
            {
                returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                returnMsg.Append("<PayPlatRequestParameter>");
                returnMsg.Append("<PARAMETERS>");
                returnMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                returnMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
                returnMsg.Append("</PARAMETERS>");
                returnMsg.Append("</PayPlatRequestParameter>");
            }
            return returnMsg.ToString();
        }

        String bestPayAccount = String.Empty;
        try
        {


            BesttoneAccount _besttoneAccount_entity = _besttoneAccount_dao.QueryByCustID(CustID);
            if (_besttoneAccount_entity == null)
            {
                returnMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    returnMsg.Append("{");
                    returnMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                    returnMsg.AppendFormat("\"errmsg\":\"{0}\"", "号码百事通账户未开通！");
                    returnMsg.Append("}");
                }
                else
                {
                    returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    returnMsg.Append("<PayPlatRequestParameter>");
                    returnMsg.Append("<PARAMETERS>");
                    returnMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                    returnMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "号码百事通账户未开通！");
                    returnMsg.Append("</PARAMETERS>");
                    returnMsg.Append("</PayPlatRequestParameter>");
                }
                return returnMsg.ToString();
            }
            else
            {
                bestPayAccount = _besttoneAccount_entity.BestPayAccount;
            }

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
                returnMsg.Length = 0;
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

                returnMsg.Insert(0, "{\"errcode\":\"0\",\"errmsg\":\"\",\"txninfo\":[");
                returnMsg = returnMsg.Remove(returnMsg.Length - 1, 1);
                returnMsg.Append("]}");
            }
            else
            {
                returnMsg.Append("{\"errcode\":\"-2012\",\"errmsg\":\"NoData\"}");
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("{\"errcode\":\"-2013\",\"errmsg\":\"异常:" + ex.Message + "\"}");
        }

        return returnMsg.ToString();
    }

    /// <summary>
    /// 获取三个月之前交易明细
    /// </summary>
    protected String GetThreeMonthsBeforeHistoryDetails(String SPID, String CustID, String wt)
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        StringBuilder returnMsg = new StringBuilder();
        DateTime startDate = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
        DateTime endDate = new DateTime(DateTime.Now.AddMonths(-2).Year, DateTime.Now.AddMonths(-2).Month, 1).AddDays(-1);

        wt = "json";    //仅支持json

        if (String.IsNullOrEmpty(SPID))
        {
            returnMsg.Length = 0;
            if ("json".Equals(wt))
            {
                returnMsg.Append("{");
                returnMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                returnMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                returnMsg.Append("}");
            }
            else
            {
                returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                returnMsg.Append("<PayPlatRequestParameter>");
                returnMsg.Append("<PARAMETERS>");
                returnMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                returnMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                returnMsg.Append("</PARAMETERS>");
                returnMsg.Append("</PayPlatRequestParameter>");
            }
            return returnMsg.ToString();
        }


        if (String.IsNullOrEmpty(CustID))
        {
            returnMsg.Length = 0;
            if ("json".Equals(wt))
            {
                returnMsg.Append("{");
                returnMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                returnMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
                returnMsg.Append("}");
            }
            else
            {
                returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                returnMsg.Append("<PayPlatRequestParameter>");
                returnMsg.Append("<PARAMETERS>");
                returnMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                returnMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
                returnMsg.Append("</PARAMETERS>");
                returnMsg.Append("</PayPlatRequestParameter>");
            }
            return returnMsg.ToString();
        }

        String bestPayAccount = String.Empty;
        try
        {

            BesttoneAccount _besttoneAccount_entity = _besttoneAccount_dao.QueryByCustID(CustID);
            if (_besttoneAccount_entity == null)
            {
                returnMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    returnMsg.Append("{");
                    returnMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                    returnMsg.AppendFormat("\"errmsg\":\"{0}\"", "号码百事通账户未开通！");
                    returnMsg.Append("}");
                }
                else
                {
                    returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    returnMsg.Append("<PayPlatRequestParameter>");
                    returnMsg.Append("<PARAMETERS>");
                    returnMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                    returnMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID号码百事通账户未开通！");
                    returnMsg.Append("</PARAMETERS>");
                    returnMsg.Append("</PayPlatRequestParameter>");
                }
                return returnMsg.ToString();
            }
            else
            {
                bestPayAccount = _besttoneAccount_entity.BestPayAccount;
            }


          
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
                returnMsg.Insert(0, "{\"errcode\":\"0\",\"errmsg\":\"\",\"txninfo\":[");
                //returnMsg.Insert(0, "{\"result\":\"true\",\"txninfo\":[");
                returnMsg = returnMsg.Remove(returnMsg.Length - 1, 1);
                returnMsg.Append("]}");
            }
            else
            {
                //returnMsg.Append("{\"result\":\"NoData\"}");
                returnMsg.Append("{\"errcode\":\"-2012\",\"errmsg\":\"NoData\"}");
            }
        }
        catch (Exception ex)
        {
            //returnMsg.Append("{\"result\":\"false\",\"info\":\"异常\"}");
            returnMsg.Append("{\"errcode\":\"-2013\",\"errmsg\":\"异常:" + ex.Message + "\"}");
        }

        return returnMsg.ToString();
    }
}

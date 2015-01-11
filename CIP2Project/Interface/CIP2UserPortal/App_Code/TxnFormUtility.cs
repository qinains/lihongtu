using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
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

/// <summary>
/// TxnFormUtility 的摘要说明
/// </summary>
public class TxnFormUtility
{
    public TxnFormUtility()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //

    }

    public static String CreateForm_TxmList(IList<TxnItem> list)
    {
        StringBuilder strHtml = new StringBuilder();

        //strHtml.Append("<table class=\"resultsA\">");
        //strHtml.Append("<thead><tr><td>日期</td><td>交易摘要</td><td>交易金额</td><td>交易渠道</td><td>商户名称</td><td>状态</td></tr></thead>");
        //strHtml.Append("<tbody>");
        if (list != null && list.Count > 0)
        {
            foreach (TxnItem item in list)
            {
                strHtml.Append("<tr>");

                String txnType = String.Empty;
                if (item.TxnType == "121020")
                    txnType = "充值";
                else if (item.TxnType == "131010")
                    txnType = "消费";
                else if (item.TxnType == "131030")
                    txnType = "退款";
                else
                    txnType = "其他";
                double amount = item.TxnAmount / 100;

                strHtml.AppendFormat("<td>{0}</td>", item.AcceptDate);
                strHtml.AppendFormat("<td>{0}</td>", item.TxnDscpt);
                strHtml.AppendFormat("<td class=\"orange\">-{0}</td>", amount.ToString("F2"));
                strHtml.AppendFormat("<td>{0}</td>", item.TxnChannel);
                strHtml.AppendFormat("<td>{0}</td>", item.MerchantName);
                strHtml.AppendFormat("<td>{0}</td>", txnType);
                strHtml.AppendFormat("<td>{0}</td>", item.CancelFlag == "1" ? "已撤销" : "成功");

                strHtml.Append("</tr>");

                strHtml.Append("<tr class=\"b_l2\"><td colspan=\"8\"></td></tr>");

            }
        }
        else
        {
            strHtml.Append("<tr><td colspan=\"8\">无记录</td></tr>");
        }

        //strHtml.Append("</tbody>");
        //strHtml.Append("</table>");

        return strHtml.ToString();
    }

    public static String CreateForm_Business(IList<TxnItem> list)
    {
        StringBuilder strHtml = new StringBuilder();

        //strHtml.Append("<table class=\"resultsA\">");
        //strHtml.Append("<thead><tr><td>日期</td><td>交易摘要</td><td>交易金额</td><td>交易渠道</td><td>商户名称</td><td>状态</td></tr></thead>");
        //strHtml.Append("<tbody>");
        if (list != null && list.Count > 0)
        {
            foreach (TxnItem item in list)
            {
                strHtml.Append("<tr>");

                strHtml.AppendFormat("<td>{0}</td>", item.AcceptDate);
                strHtml.AppendFormat("<td>{0}</td>", item.TxnDscpt);
                strHtml.AppendFormat("<td class=\"orange\">-{0}</td>", item.TxnAmount);
                strHtml.AppendFormat("<td>{0}</td>", item.TxnChannel);
                strHtml.AppendFormat("<td>{0}</td>", item.MerchantName);
                strHtml.AppendFormat("<td>{0}</td>", item.CancelFlag == "1" ? "已撤销" : "成功");

                strHtml.Append("</tr>");

                strHtml.Append("<tr class=\"b_l2\"><td colspan=\"8\"></td></tr>");

            }
        }
        else
        {
            strHtml.Append("<tr><td colspan=\"8\">无记录</td></tr>");
        }

        //strHtml.Append("</tbody>");
        //strHtml.Append("</table>");

        return strHtml.ToString();
    }

    public static String CreateForm_Recharge(IList<TxnItem> list)
    {
        StringBuilder strHtml = new StringBuilder();

        //strHtml.Append("<table class=\"resultsA\">");
        //strHtml.Append("<thead><tr><td>日期</td><td>交易摘要</td><td>交易金额</td><td>交易渠道</td><td>商户名称</td><td>状态</td></tr></thead>");
        //strHtml.Append("<tbody>");

        if (list != null && list.Count > 0)
        {
            foreach (TxnItem item in list)
            {
                strHtml.Append("<tr>");

                strHtml.AppendFormat("<td>{0}</td>", item.AcceptDate);
                strHtml.AppendFormat("<td>{0}</td>", item.TxnDscpt);
                strHtml.AppendFormat("<td class=\"orange\">-{0}</td>", item.TxnAmount);
                strHtml.AppendFormat("<td>{0}</td>", item.TxnChannel);
                strHtml.AppendFormat("<td>{0}</td>", item.MerchantName);
                strHtml.AppendFormat("<td>{0}</td>", item.CancelFlag == "1" ? "已撤销" : "成功");

                strHtml.Append("</tr>");

                strHtml.Append("<tr class=\"b_l2\"><td colspan=\"8\"></td></tr>");

            }
        }
        else
        {
            strHtml.Append("<tr><td colspan=\"8\">无记录</td></tr>");
        }

        //strHtml.Append("</tbody>");
        //strHtml.Append("</table>");

        return strHtml.ToString();
    }

    public static String CreateForm_Refund(IList<TxnItem> list)
    {
        StringBuilder strHtml = new StringBuilder();

        //strHtml.Append("<table class=\"resultsA\">");
        //strHtml.Append("<thead><tr><td>日期</td><td>交易摘要</td><td>交易金额</td><td>交易渠道</td><td>商户名称</td><td>状态</td></tr></thead>");
        //strHtml.Append("<tbody>");

        if (list != null && list.Count > 0)
        {
            foreach (TxnItem item in list)
            {
                strHtml.Append("<tr>");

                strHtml.AppendFormat("<td>{0}</td>", item.AcceptDate);
                strHtml.AppendFormat("<td>{0}</td>", item.TxnDscpt);
                strHtml.AppendFormat("<td class=\"orange\">-{0}</td>", item.TxnAmount);
                strHtml.AppendFormat("<td>{0}</td>", item.TxnChannel);
                strHtml.AppendFormat("<td>{0}</td>", item.MerchantName);
                strHtml.AppendFormat("<td>{0}</td>", item.CancelFlag == "1" ? "已撤销" : "成功");

                strHtml.Append("</tr>");

                strHtml.Append("<tr class=\"b_l2\"><td colspan=\"8\"></td></tr>");

            }
        }
        else
        {
            strHtml.Append("<tr><td colspan=\"8\">无记录</td></tr>");
        }

        //strHtml.Append("</tbody>");
        //strHtml.Append("</table>");

        return strHtml.ToString();
    }
}

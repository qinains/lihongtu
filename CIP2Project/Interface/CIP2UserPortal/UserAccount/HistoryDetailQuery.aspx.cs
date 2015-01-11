using System;
using System.Data;
using System.Collections.Generic;
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

public partial class UserAccount_HistoryDetailQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnSearch.Click += new EventHandler(btnSearch_Click);
        if (!IsPostBack)
        {
            
        }
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    void LoadDataForm()
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        this.LiteralBusiness.Text = "无记录";
        this.LiteralRecharge.Text = "无记录";
        this.LiteralRefund.Text = "无记录";

        try
        {
            DateTime startDate = Convert.ToDateTime(this.hdStartDate.Value);
            DateTime endDate = Convert.ToDateTime(this.hdEndDate.Value);
            IList<TxnItem> txnList_business, txnList_recharge, txnList_refund;

            //调用接口查询
            Result = BesttoneAccountHelper.QueryBusinessTxnHistory(startDate, endDate, "18918790558", 10, 0, out txnList_business, out ErrMsg);

            Result = BesttoneAccountHelper.QueryRechargeTxnHistory(startDate, endDate, "18918790558", 10, 0, out txnList_recharge, out ErrMsg);

            Result = BesttoneAccountHelper.QueryRefundTxnHistory(startDate, endDate, "18918790558", 10, 0, out txnList_refund, out ErrMsg);

            //生成表单

            this.LiteralBusiness.Text = TxnFormUtility.CreateForm_Business(txnList_business);
            this.LiteralRecharge.Text = TxnFormUtility.CreateForm_Recharge(txnList_recharge);    
            this.LiteralRefund.Text = TxnFormUtility.CreateForm_Refund(txnList_refund);
        }
        catch { }
    }

    #region 事件

    /// <summary>
    /// 查询
    /// </summary>
    void btnSearch_Click(object sender, EventArgs e)
    {
        this.hdStartDate.Value = this.txtStartDate.Text;
        this.hdEndDate.Value = this.txtEndDate.Text;
        LoadDataForm();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {

    }
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {

    }
    protected void AspNetPager3_PageChanged(object sender, EventArgs e)
    {

    }

    #endregion
}

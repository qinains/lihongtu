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
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

public partial class UserAccount_BankRechargeSuccess : System.Web.UI.Page
{
    //账户信息管理操作类
    BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
    //充值订单操作类
    RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
    //账户充值流水号操作类
    AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();

    Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Threading.Thread.Sleep(200);
            //客户ID，账户号，订单号
            String CustID, BestPayAccount, OrderSeq;
            //充值金额，账户余额
            long RechargeBalance, Balance;

            try
            {
                #region 获取支付平台post过来的一些变量

                //获取支付平台post过来的一些变量
                String uptranSeq = Request.Form["UPTRANSEQ"];                   //支付平台交易流水号
                String tranDate = Request.Form["TRANDATE"];                     //支付平台交易日期
                String retnCode = Request.Form["RETNCODE"];                     //处理结果码
                String retnInfo = Request.Form["RETNINFO"];                     //处理结果解释码
                String orderReqtranSeq = Request.Form["ORDERREQTRANSEQ"];       //订单请求交易流水号
                String orderSeq = Request.Form["ORDERSEQ"];                     //订单号
                String orderAmount = Request.Form["ORDERAMOUNT"];               //订单总金额
                String productAmount = Request.Form["PRODUCTAMOUNT"];           //产品金额
                String attachAmount = Request.Form["ATTACHAMOUNT"];             //附加金额
                String curType = Request.Form["CURTYPE"];                       //币种
                String encodeType = Request.Form["ENCODETYPE"];                 //加密方式
                String attach = Request.Form["ATTACH"];                         //SP附加信息
                String sign = Request.Form["SIGN"];                             //数字签名

                //验证签名
                #endregion

                #region 查看订单状态

                //根据订单号查询该订单是否已经支付
                RechargeOrder _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                if (_recharge_order == null)
                    return;

                CustID = _recharge_order.CustID;
                BestPayAccount = _recharge_order.TargetAccount;
                OrderSeq = _recharge_order.OrderSeq;
                RechargeBalance = _recharge_order.OrderAmount;

                //如果不是待付款状态则跳转错误页面
                if (_recharge_order.Status != 3)
                {
                    this.lblMsg.Text = "提示：您的充值申请已提交，系统正在处理中……请耐心等待！";
                }
                else
                {
                    //查询账户余额
                    Result = BesttoneAccountHelper.QueryAccountBalance(BestPayAccount, out Balance, out ErrMsg);
                    this.lblMsg.Text = "您已成功充值<font color=red>&nbsp;" + BesttoneAccountHelper.ConvertAmountToYuan(_recharge_order.OrderAmount) + "&nbsp;</font>元，账户余额&nbsp;<font color=red>" + BesttoneAccountHelper.ConvertAmountToYuan(Balance) + "</font>&nbsp;元";
                }

                #endregion
            }
            catch { }
        }
    }
}

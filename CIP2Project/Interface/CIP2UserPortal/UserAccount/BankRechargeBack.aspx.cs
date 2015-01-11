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
using System.Text;

public partial class UserAccount_BankRechargeBack : System.Web.UI.Page
{
    //账户信息管理操作类
    BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
    //充值订单操作类
    RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
    //网银扣款流水记录
    BankRechargeRecordDAO _bankRechargeRecord_dao = new BankRechargeRecordDAO();
    //账户充值流水号操作类
    AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();

    Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("【网银充值，DateTime:{0}】\r\n", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
        if (!IsPostBack)
        {
            try
            {

                //客户ID，账户号，订单号
                String CustID, BestPayAccount, OrderSeq;
                //充值金额，账户余额
                long RechargeBalance, Balance;

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

                strLog.AppendFormat("[网银返回参数]:uptranSeq:{0},tranDate:{1},retnCode:{2},retnInfo:{3},orderReqtranSeq:{4},orderSeq:{5},orderAmount:{6},productAmount:{7},attachAmount:{8},curType:{9},attach:{10},sign:{11},",
                    uptranSeq, tranDate, retnCode, retnInfo, orderReqtranSeq, orderSeq, orderAmount, productAmount, attachAmount, curType, attach, sign);

                if (retnCode != "0000")
                    return;

                //验证签名:16进制转换(MD5加密)
                String newSign = String.Format("UPTRANSEQ={0}&MERCHANTID={1}&ORDERID={2}&PAYMENT={3}&RETNCODE={4}&RETNINFO={5}&PAYDATE={6}&KEY={7}",
                    uptranSeq, BesttoneAccountConstDefinition.DefaultInstance.MERCHANTID, orderSeq, orderAmount, retnCode, retnInfo, tranDate, BesttoneAccountConstDefinition.DefaultInstance.MERCHANTID_KEY);
                String md5EncodingSign = BesttoneAccountHelper.MACSign(newSign);
                strLog.AppendFormat("newSign:{0},md5EncodingSign:{1}\r\n", newSign, md5EncodingSign);
                if (!md5EncodingSign.Equals(sign))
                {
                    strLog.Append("[签名验证]:签名验证有误");
                    return;
                }

                String responseXml = String.Format("UPTRANSEQ_{0}", uptranSeq);
                strLog.AppendFormat("[返回给网银参数]:ResponseXml:{0}\r\n", responseXml);

                #endregion

                #region 更新银行扣款订单状态

                //查询订单，检查订单状态
                RechargeOrder _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                BankRechargeRecord _bankRecharge_record = _bankRechargeRecord_dao.QueryByOrderTransacntionID(orderReqtranSeq);
                if (_recharge_order == null)
                    return;
                if (_bankRecharge_record == null)
                    return;

                CustID = _recharge_order.CustID;
                BestPayAccount = _recharge_order.TargetAccount;
                OrderSeq = _recharge_order.OrderSeq;
                RechargeBalance = _recharge_order.OrderAmount;

                strLog.AppendFormat("[订单信息]:CustID:{0},BestPayAccount:{1},OrderSeq:{2},RechargeBalance:{3},订单状态:{4}\r\n",
                    CustID, BestPayAccount, OrderSeq, RechargeBalance, _recharge_order.Status);

                //检查订单状态    6 扣款异常  主表
                if (_recharge_order.Status == 6)
                {
                    return;
                }
                else if (_recharge_order.Status == 2 || _recharge_order.Status == 3 || _recharge_order.Status == 5 || _recharge_order.Status == 9)
                {
                    Response.Write(responseXml.ToString());    // 2 扣款成功    3 已充成功    5  充值失败   9 已对账
                }
                // Response.Write 后，不会再往下执行
                // 0 ,1, 4, 7 往下做  先把主表状态改为2,然后将扣款子表状态改为1 意味着扣款成功
                Boolean result = false;
                //修改订单状态
                _recharge_order.Status = 2;
                _recharge_order.PayTime = DateTime.Now;
                _recharge_order.UptranSeq = uptranSeq;
                result = _rechargeOrder_dao.Update(_recharge_order);
                strLog.AppendFormat("[修改订单结果]:{0}\r\n", result);
                if (!result)
                    return;

                //修改扣款记录状态
                _bankRecharge_record.UptranSeq = uptranSeq;
                _bankRecharge_record.Sign = sign;
                _bankRecharge_record.TranDate = tranDate;
                _bankRecharge_record.ReturnCode = retnCode;
                _bankRecharge_record.ReturnDesc = retnInfo;
                _bankRecharge_record.PayTime = DateTime.Now;
                _bankRecharge_record.Status = 1;
                result = _bankRechargeRecord_dao.Update(_bankRecharge_record);
                if (!result)
                    return;

                #endregion

                #region 向账户进行充值

                //生成一条充值流水号记录
                DateTime rechargeTime = DateTime.Now;
                String transactionid = BesttoneAccountHelper.CreateTransactionID();

                AccountRechargeRecord rechargeRecord_entity = new AccountRechargeRecord();
                rechargeRecord_entity.RechargeTransactionID = transactionid;
                rechargeRecord_entity.RechargeDate = rechargeTime.ToString("yyyyMMdd");
                rechargeRecord_entity.OrderSeq = OrderSeq;
                rechargeRecord_entity.OrderAmount = RechargeBalance;
                rechargeRecord_entity.RechargeType = "0";
                rechargeRecord_entity.OrderDesc = "网银充值";
                rechargeRecord_entity.ReqTime = rechargeTime;
                rechargeRecord_entity.CompleteTime = new DateTime(1900, 1, 1);
                rechargeRecord_entity.Status = 0;
                rechargeRecord_entity.ReturnCode = "";
                rechargeRecord_entity.ReturnDesc = "";
                strLog.AppendFormat("[开始给账户充值]:transactionid:{0},", transactionid);
                try
                {
                    //开始调用接口充值
                    Result = BesttoneAccountHelper.AccountRecharge(transactionid, BestPayAccount, RechargeBalance, rechargeTime, out Balance, out ErrMsg);
                    if (Result == 0)
                    {
                        strLog.Append("充值状态：充值成功\r\n");
                        //充值成功，则更新订单状态
                        _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(OrderSeq);
                        _recharge_order.CompleteTime = DateTime.Now;
                        _recharge_order.Status = 3;
                        _recharge_order.RechargeCount = _recharge_order.RechargeCount + 1;
                        _recharge_order.ReturnCode = "0000";
                        _recharge_order.ReturnDesc = "充值成功";
                        _rechargeOrder_dao.Update(_recharge_order);
                        //如果需要开发票 -- 插入发票表  2013-04-14 add-start
                        System.Text.StringBuilder datalog = new System.Text.StringBuilder();
                        _rechargeOrder_dao.UpdateInvoice(orderSeq, "0", out datalog);  //将发票状态改为申请中.
                        strLog.Append("更新发票状态："+datalog.ToString());
                        //2013-04-14 add-end
                        //更新流水号状态
                        rechargeRecord_entity.Status = 1;
                        rechargeRecord_entity.CompleteTime = DateTime.Now;
                        rechargeRecord_entity.ReturnCode = "0000";
                        rechargeRecord_entity.ReturnDesc = "充值成功";

                    }
                    else
                    {
                        strLog.Append("充值状态：充值失败\r\n");
                        //充值失败，则更新订单状态
                        _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(OrderSeq);
                        _recharge_order.RechargeCount = _recharge_order.RechargeCount + 1;
                        _recharge_order.ReturnCode = Result.ToString();
                        _recharge_order.ReturnDesc = ErrMsg;
                        _rechargeOrder_dao.Update(_recharge_order);

                        //更新流水号状态
                        rechargeRecord_entity.Status = 2;
                        rechargeRecord_entity.CompleteTime = DateTime.Now;
                        rechargeRecord_entity.ReturnCode = Result.ToString();
                        rechargeRecord_entity.ReturnDesc = ErrMsg;
                    }
                }
                catch (Exception ex) 
                {
                    strLog.AppendFormat("充值异常1：{0}\r\n", ex.Message); 
                }
                finally
                {
                    //插入充值流水记录
                    _accountRechargeRecord_dao.Insert(rechargeRecord_entity);
                    Response.Write(responseXml.ToString());
                }

                #endregion

            }
            catch (Exception ex) 
            {
                strLog.AppendFormat("异常2：{0}\r\n", ex.Message); 
            }
            finally
            {
                log(strLog.ToString());
            }
        }
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("BankRechargeBack", msg);
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    public class RechargeOrder
    {
        public RechargeOrder()
        {
            
        }

        public RechargeOrder(String in_OrderSeq, String in_PayTransactionID, String in_OrderDate, String in_CurType,
            long in_OrderAmount, long in_ProductAmount, long in_AttachAmount, String in_OrderDesc, String in_CustID, String in_TargetAccount,
            String in_RechargeType, String in_SourceSPID, DateTime in_ReqTime, DateTime in_PayTime, DateTime in_CompleteTime, Int32 in_Status,
            Int32 in_RechargeCount, String in_UptranSeq, String in_ReturnCode, String in_ReturnDesc, String in_NeedInvoice)
        {
            this.OrderSeq = in_OrderSeq;
            this.PayTransactionID = in_PayTransactionID;
            this.OrderDate = in_OrderDate;
            this.CurType = in_CurType;
            this.OrderAmount = in_OrderAmount;
            this.ProductAmount = in_ProductAmount;
            this.AttachAmount = in_AttachAmount;
            this.OrderDesc = in_OrderDesc;
            this.CustID = in_CustID;
            this.TargetAccount = in_TargetAccount;
            this.RechargeType = in_RechargeType;
            this.SourceSPID = in_SourceSPID;
            this.ReqTime = in_ReqTime;
            this.PayTime = in_PayTime;
            this.CompleteTime = in_CompleteTime;
            this.Status = in_Status;
            this.RechargeCount = in_RechargeCount;
            this.UptranSeq = in_UptranSeq;
            this.ReturnCode = in_ReturnCode;
            this.ReturnDesc = in_ReturnDesc;
            this.NeedInvoice = in_NeedInvoice;
        }

        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _orderSeq;
        public String OrderSeq
        {
            get { return _orderSeq; }
            set { _orderSeq = value; }
        }

        private String _payTransactionID;
        public String PayTransactionID
        {
            get { return _payTransactionID; }
            set { _payTransactionID = value; }
        }

        private string _orderDate;
        public string OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        private string _curType;
        public string CurType
        {
            get { return _curType; }
            set { _curType = value; }
        }

        private long _orderAmount;
        public long OrderAmount
        {
            get { return _orderAmount; }
            set { _orderAmount = value; }
        }

        private long _productAmount;
        public long ProductAmount
        {
            get { return _productAmount; }
            set { _productAmount = value; }
        }

        private long _attachAmount;
        public long AttachAmount
        {
            get { return _attachAmount; }
            set { _attachAmount = value; }
        }

        private string _orderDesc;
        public string OrderDesc
        {
            get { return _orderDesc; }
            set { _orderDesc = value; }
        }

        private String _custid;
        public String CustID
        {
            get { return _custid; }
            set { _custid = value; }
        }

        private String _targetAccount;
        public String TargetAccount
        {
            get { return _targetAccount; }
            set { _targetAccount = value; }
        }

        private String _rechargeType;
        public String RechargeType
        {
            get { return _rechargeType; }
            set { _rechargeType = value; }
        }

        private String _sourceSPID;
        public String SourceSPID
        {
            get { return _sourceSPID; }
            set { _sourceSPID = value; }
        }

        private DateTime? _reqTime;
        public DateTime? ReqTime
        {
            get { return _reqTime; }
            set { _reqTime = value; }
        }

        private DateTime _payTime;
        public DateTime PayTime
        {
            get { return _payTime; }
            set { _payTime = value; }
        }

        private DateTime? _completeTime;
        public DateTime? CompleteTime
        {
            get { return _completeTime; }
            set { _completeTime = value; }
        }

        private Int32 _status;
        /// <summary>
        /// 1:待付款,2:已付款待充值,3:已充值(已结束),4:付款失败,5:充值失败,6:订单已撤销,9:已对账
        /// </summary>
        public Int32 Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private Int32 _rechargeCount;
        public Int32 RechargeCount
        {
            get { return _rechargeCount; }
            set { _rechargeCount = value; }
        }

        private String _uptranSeq;
        public String UptranSeq
        {
            get { return _uptranSeq; }
            set { _uptranSeq = value; }
        }

        private String _returnCode;
        public String ReturnCode
        {
            get { return _returnCode; }
            set { _returnCode = value; }
        }

        private String _returnDesc;
        public String ReturnDesc
        {
            get { return _returnDesc; }
            set { _returnDesc = value; }
        }

        private String _needInvoice;
        public String NeedInvoice
        {
            get { return _needInvoice; }
            set { _needInvoice = value; }
        }

        private String _rechargeTransactionID;
        public String RechargeTransactionID
        {
            get { return _rechargeTransactionID; }
            set { _rechargeTransactionID = value; }
        }


    }
}

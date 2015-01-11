using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    public class BankRechargeOrder
    {
        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _orderTransactionID;
        public String OrderTransactionID
        {
            get { return _orderTransactionID; }
            set { _orderTransactionID = value; }
        }

        private string _orderSeq;
        public String OrderSeq
        {
            get { return _orderSeq; }
            set { _orderSeq = value; }
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

        private string _sourceSPID;
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

        private DateTime _deductionTime;
        public DateTime DeductionTime
        {
            get { return _deductionTime; }
            set { _deductionTime = value; }
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
    }
}

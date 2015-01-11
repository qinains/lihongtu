using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    public class AccountRechargeRecord
    {
        public AccountRechargeRecord()
        { 
            
        }

        public AccountRechargeRecord(String in_RechargeTransactionID, String in_RechargeDate, String in_OrderSeq, long in_OrderAmount, String in_RechargeType,
            String in_OrderDesc, DateTime in_ReqTime, DateTime in_CompleteTime, Int32 in_Status, String in_ReturnCode, String in_ReturnDesc)
        {
            this.RechargeTransactionID = in_RechargeTransactionID;
            this.RechargeDate = in_RechargeDate;
            this.OrderSeq = in_OrderSeq;
            this.OrderAmount = in_OrderAmount;
            this.RechargeType = in_RechargeType;
            this.OrderDesc = in_OrderDesc;
            this.ReqTime = in_ReqTime;
            this.CompleteTime = in_CompleteTime;
            this.Status = in_Status;
            this.ReturnCode = in_ReturnCode;
            this.ReturnDesc = in_ReturnDesc;
        }

        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _rechargeTransactionID;
        public String RechargeTransactionID
        {
            get { return _rechargeTransactionID; }
            set { _rechargeTransactionID = value; }
        }

        private string _rechargeDate;
        public string RechargeDate
        {
            get { return _rechargeDate; }
            set { _rechargeDate = value; }
        }

        private string _orderSeq;
        public String OrderSeq
        {
            get { return _orderSeq; }
            set { _orderSeq = value; }
        }

        private long _orderAmount;
        public long OrderAmount
        {
            get { return _orderAmount; }
            set { _orderAmount = value; }
        }

        private string _rechargeType;
        /// <summary>
        /// 【充值类型】0:网银,2:翼游卡(号码百事通卡),3:百事购卡
        /// </summary>
        public string RechargeType
        {
            get { return _rechargeType; }
            set { _rechargeType = value; }
        }

        private string _orderDesc;
        public string OrderDesc
        {
            get { return _orderDesc; }
            set { _orderDesc = value; }
        }

        private DateTime _reqTime;
        public DateTime ReqTime
        {
            get { return _reqTime; }
            set { _reqTime = value; }
        }

        private DateTime _completeTime;
        public DateTime CompleteTime
        {
            get { return _completeTime; }
            set { _completeTime = value; }
        }

        private Int32 _status;
        /// <summary>
        /// 0:待充值,1:充值成功,2:充值失败
        /// </summary>
        public Int32 Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 返回响应码
        /// </summary>
        private string _returnCode;
        public string ReturnCode
        {
            get { return _returnCode; }
            set { _returnCode = value; }
        }

        /// <summary>
        /// 返回描述
        /// </summary>
        private string _returnDesc;
        public string ReturnDesc
        {
            get { return _returnDesc; }
            set { _returnDesc = value; }
        }
    }
}

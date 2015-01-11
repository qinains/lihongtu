using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    public class CardRechargeRecord
    {
        public CardRechargeRecord()
        { 
            
        }

        public CardRechargeRecord(String in_PayTransactionID, String in_OrderSeq, String in_OrderDate, String in_CurType, long in_OrderAmount,
            String in_OrderDesc, String in_CardNo, String in_CardPwd, String in_CardType, String in_TargetAccount, Int32 in_Status, DateTime in_ReqTime,
            DateTime in_PayTime, String in_UptranSeq, String in_TranDate, String in_Sign, String in_ReturnCode, String in_ReturnDesc)
        {
            this.PayTransactionID = in_PayTransactionID;
            this.OrderSeq = in_OrderSeq;
            this.OrderDate = in_OrderDate;
            this.CurType = in_CurType;
            this.OrderAmount = in_OrderAmount;
            this.OrderDesc = in_OrderDesc;
            this.CardNo = in_CardNo;
            this.CardPwd = in_CardPwd;
            this.CardType = in_CardType;
            this.TargetAccount = in_TargetAccount;
            this.Status = in_Status;
            this.ReqTime = in_ReqTime;
            this.PayTime = in_PayTime;
            this.UptranSeq = in_UptranSeq;
            this.TranDate = in_TranDate;
            this.Sign = in_Sign;
            this.ReturnCode = in_ReturnCode;
            this.ReturnDesc = in_ReturnDesc;
        }

        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _payTransactionID;
        public String PayTransactionID
        {
            get { return _payTransactionID; }
            set { _payTransactionID = value; }
        }

        private String _orderSeq;
        public String OrderSeq
        {
            get { return _orderSeq; }
            set { _orderSeq = value; }
        }

        private String _orderDate;
        public String OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        private String _curType;
        public String CurType
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

        private String _orderDesc;
        public String OrderDesc
        {
            get { return _orderDesc; }
            set { _orderDesc = value; }
        }

        private String _cardNo;
        public String CardNo
        {
            get { return _cardNo; }
            set { _cardNo = value; }
        }

        private String _cardType;
        public String CardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }

        private String _cardPwd;
        public String CardPwd
        {
            get { return _cardPwd; }
            set { _cardPwd = value; }
        }

        private String _targetAccount;
        public String TargetAccount
        {
            get { return _targetAccount; }
            set { _targetAccount = value; }
        }

        private Int32 _status;
        /// <summary>
        /// 1£º³É¹¦£¬2£ºÊ§°Ü
        /// </summary>
        public Int32 Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private DateTime _reqTime;
        public DateTime ReqTime
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

        private String _uptranSeq;
        public String UptranSeq
        {
            get { return _uptranSeq; }
            set { _uptranSeq = value; }
        }

        private String _tranDate;
        public String TranDate
        {
            get { return _tranDate; }
            set { _tranDate = value; }
        }

        private String _sign;
        public String Sign
        {
            get { return _sign; }
            set { _sign = value; }
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

    }
}

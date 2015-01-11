using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    public class RechargeBackRecord
    {
        private long _id;
        public long ID
        {
            get { return _id; }
            set { _id = value; }
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

        private String _retnCode;
        public String RetnCode
        {
            get { return _retnCode; }
            set { _retnCode = value; }
        }

        private String _retnInfo;
        public String RetnInfo
        {
            get { return _retnInfo; }
            set { _retnInfo = value; }
        }

        private String _orderTransactionID;
        public String OrderTransactionID
        {
            get { return _orderTransactionID; }
            set { _orderTransactionID = value; }
        }

        private String _orderSeq;
        public String OrderSeq
        {
            get { return _orderSeq; }
            set { _orderSeq = value; }
        }

        private String _encodeType;
        public String EncodeType
        {
            get { return _encodeType; }
            set { _encodeType = value; }
        }

        private String _sign;
        public String Sign
        {
            get { return _sign; }
            set { _sign = value; }
        }

        private String _rechargeType;
        public String RechargeType
        {
            get { return _rechargeType; }
            set { _rechargeType = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    public class AccountBindingRecord
    {
        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _transactionID_SP;
        public String TransactionID_SP
        {
            get { return _transactionID_SP; }
            set { _transactionID_SP = value; }
        }

        private String _transactionID_Ext;
        public String TransactionID_Ext
        {
            get { return _transactionID_Ext; }
            set { _transactionID_Ext = value; }
        }

        private String _custid;
        public String CustID
        {
            get { return _custid; }
            set { _custid = value; }
        }

        private String _spid;
        public String SPID
        {
            get { return _spid; }
            set { _spid = value; }
        }

        private Int32 _optionType;
        public Int32 OptionType
        {
            get { return _optionType; }
            set { _optionType = value; }
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

        private String _bestPayAccount;
        public String BestPayAccount
        {
            get { return _bestPayAccount; }
            set { _bestPayAccount = value; }
        }

        private String _pw;
        public String PW
        {
            get { return _pw; }
            set { _pw = value; }
        }

        private Int32 _status;
        public Int32 Status
        {
            get { return _status; }
            set { _status = value; }
        }
        
        private Int32 _isDeleted;
        public Int32 IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }


    }
}

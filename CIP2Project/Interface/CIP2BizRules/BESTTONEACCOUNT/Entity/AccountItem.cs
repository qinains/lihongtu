using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// ÕË»§¼ÇÂ¼
    /// </summary>
    public class AccountItem
    {
        private string _Account;
        public String Account
        {
            get { return _Account; }
            set { _Account = value; }
        }

        private String _AccountNo;
        public String AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }

        private string _AccountName;
        public String AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        private string _AccountType;
        public String AccountType
        {
            get { return _AccountType; }
            set { _AccountType = value; }
        }

        private string _AccountStatus;
        public String AccountStatus
        {
            get { return _AccountStatus; }
            set { _AccountStatus = value; }
        }

        private long _AccountBalance;
        public long AccountBalance
        {
            get { return _AccountBalance; }
            set { _AccountBalance = value; }
        }

        private long _PredayBalance;
        public long PredayBalance
        {
            get { return _PredayBalance; }
            set { _PredayBalance = value; }
        }

        private long _PreMonthBalance;
        public long PreMonthBalance
        {
            get { return _PreMonthBalance; }
            set { _PreMonthBalance = value; }
        }

        private long _AvailableBalance;
        public long AvailableBalance
        {
            get { return _AvailableBalance; }
            set { _AvailableBalance = value; }
        }

        private long _UnAvailableBalance;
        public long UnAvailableBalance
        {
            get { return _UnAvailableBalance; }
            set { _UnAvailableBalance = value; }
        }

        private long _AvailableLecash;
        public long AvailableLecash
        {
            get { return _AvailableLecash; }
            set { _AvailableLecash = value; }
        }

        private string _CardNum;
        public String CardNum
        {
            get { return _CardNum; }
            set { _CardNum = value; }
        }

        private string _CardType;
        public String CardType
        {
            get { return _CardType; }
            set { _CardType = value; }
        }


    }
}

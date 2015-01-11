using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 号码百事通账户实体
    /// </summary>
    public class BesttoneAccount
    {
        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _bestPayAccount;
        public String BestPayAccount
        {
            get { return _bestPayAccount; }
            set { _bestPayAccount = value; }
        }

        private String _custid;
        public String CustID
        {
            get { return _custid; }
            set { _custid = value; }
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

        private DateTime _createTime;
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    [Serializable]
    public class UDBSPInfo
    {
        private Int32 _id;
        private String _spid;
        private String _udbSPID;
        private String _udbKey;
        private String _redirectUrl;
        private String _extendField_1;
        private String _extenfField_2;
        private DateTime _createTime;

        public Int32 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public String SPID
        {
            get { return _spid; }
            set { _spid = value; }
        }

        public String UDBSPID
        {
            get { return _udbSPID; }
            set { _udbSPID = value; }
        }

        public String UDBKey
        {
            get { return _udbKey; }
            set { _udbKey = value; }
        }

        public String RedirectUrl
        {
            get { return _redirectUrl; }
            set { _redirectUrl = value; }
        }

        public String ExtendField_1
        {
            get { return _extendField_1; }
            set { _extendField_1 = value; }
        }

        public String ExtendField_2
        {
            get { return _extenfField_2; }
            set { _extenfField_2 = value; }
        }

        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
    }
}

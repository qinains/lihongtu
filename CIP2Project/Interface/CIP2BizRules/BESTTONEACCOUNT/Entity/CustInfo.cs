using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    public class CustInfo
    {

        private string _CustomerNo;

        public string CustomerNo
        {
            get { return _CustomerNo; }
            set { _CustomerNo = value; }
        }

        private string _ProductNo;

        public string ProductNo
        {
            get { return _ProductNo; }
            set { _ProductNo = value; }
        }

        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }

        private string _IdType;

        public string IdType
        {
            get { return _IdType; }
            set { _IdType = value; }
        }


        private string _IdNo;

        public string IdNo
        {
            get { return _IdNo; }
            set { _IdNo = value; }
        }


    }
}

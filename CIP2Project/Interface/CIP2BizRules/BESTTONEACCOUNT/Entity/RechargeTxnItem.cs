using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 充值记录
    /// </summary>
    public class RechargeTxnItem
    {

        #region Private Fields

        private String _acceptSeqNO;
        private string _acceptDate;
        private String _acceptTime;
        private string _txnMount;
        private string _txnType;
        private string _txnChannel;
        private string _merchantName;
        private string _txnDscpt;
        private string _cancelFlag;

        #endregion

        #region Public Members

        /// <summary>
        /// 受理流水号
        /// </summary>
        public String AcceptSeqNO
        {
            get { return _acceptSeqNO; }
            set { _acceptSeqNO = value; }
        }
        
        /// <summary>
        /// 受理日期
        /// </summary>
        public String AcceptDate
        {
            get { return _acceptDate; }
            set { _acceptDate = value; }
        }
        
        /// <summary>
        /// 受理时间
        /// </summary>
        public String AcceptTime
        {
            get { return _acceptTime; }
            set { _acceptTime = value; }
        }
        
        /// <summary>
        /// 交易金额
        /// </summary>
        public String TxnAmount
        {
            get { return _txnMount; }
            set { _txnMount = value; }
        }
        
        /// <summary>
        /// 交易类型
        /// </summary>
        public String TxnType
        {
            get { return _txnType; }
            set { _txnType = value; }
        }
        
        /// <summary>
        /// 交易摘要
        /// </summary>
        public String TxnDscpt
        {
            get { return _txnDscpt; }
            set { _txnDscpt = value; }
        }

        #endregion

    }
}

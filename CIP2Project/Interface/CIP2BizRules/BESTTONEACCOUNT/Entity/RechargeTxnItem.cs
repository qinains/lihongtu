using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// ��ֵ��¼
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
        /// ������ˮ��
        /// </summary>
        public String AcceptSeqNO
        {
            get { return _acceptSeqNO; }
            set { _acceptSeqNO = value; }
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        public String AcceptDate
        {
            get { return _acceptDate; }
            set { _acceptDate = value; }
        }
        
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public String AcceptTime
        {
            get { return _acceptTime; }
            set { _acceptTime = value; }
        }
        
        /// <summary>
        /// ���׽��
        /// </summary>
        public String TxnAmount
        {
            get { return _txnMount; }
            set { _txnMount = value; }
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        public String TxnType
        {
            get { return _txnType; }
            set { _txnType = value; }
        }
        
        /// <summary>
        /// ����ժҪ
        /// </summary>
        public String TxnDscpt
        {
            get { return _txnDscpt; }
            set { _txnDscpt = value; }
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// ���׼�¼
    /// </summary>
    public class TxnItem:IComparable<TxnItem>
    {

        #region Private Fields

        private String _acceptSeqNO;
        private string _acceptDate;
        private String _acceptTime;
        private long _txnMount;
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
        public long TxnAmount
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
        /// ��������
        /// </summary>
        public String TxnChannel
        {
            get { return _txnChannel; }
            set { _txnChannel = value; }
        }
        
        /// <summary>
        /// �̻�����
        /// </summary>
        public String MerchantName
        {
            get { return _merchantName; }
            set { _merchantName = value; }
        }
        
        /// <summary>
        /// ����ժҪ
        /// </summary>
        public String TxnDscpt
        {
            get { return _txnDscpt; }
            set { _txnDscpt = value; }
        }

        /// <summary>
        /// ������ʶ
        /// </summary>
        public String CancelFlag
        {
            get { return _cancelFlag; }
            set { _cancelFlag = value; }
        }

        /// <summary>
        /// ����ʱ�䣺����+ʱ��
        /// </summary>
        public String TxnTime
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(this._acceptDate))
                        return String.Empty;
                    if (String.IsNullOrEmpty(this._acceptTime))
                        return String.Empty;

                    String year = this._acceptDate.Substring(0, 4);
                    String month = this._acceptDate.Substring(4, 2);
                    String day = this._acceptDate.Substring(6, 2);

                    String hour = this._acceptTime.Substring(0, 2);
                    String min = this._acceptTime.Substring(2, 2);

                    return String.Format("{0}-{1}-{2} {3}:{4}", year, month, day, hour, min);
                }
                catch { return String.Empty; }
            }
        }
        #endregion


        #region IComparable ��Ա

        public int CompareTo(TxnItem obj)
        {
            Int32 result = 0;
            try
            {
                if (TxnTime.CompareTo(obj.TxnTime) > 0)
                {
                    result = -1;
                }
                else if (TxnTime.CompareTo(obj.TxnTime) < 0)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            catch { }

            return result;
        }

        #endregion
    }

}

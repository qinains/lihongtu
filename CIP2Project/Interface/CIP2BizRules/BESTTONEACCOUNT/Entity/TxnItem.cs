using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 交易记录
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
        public long TxnAmount
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
        /// 交易渠道
        /// </summary>
        public String TxnChannel
        {
            get { return _txnChannel; }
            set { _txnChannel = value; }
        }
        
        /// <summary>
        /// 商户名称
        /// </summary>
        public String MerchantName
        {
            get { return _merchantName; }
            set { _merchantName = value; }
        }
        
        /// <summary>
        /// 交易摘要
        /// </summary>
        public String TxnDscpt
        {
            get { return _txnDscpt; }
            set { _txnDscpt = value; }
        }

        /// <summary>
        /// 撤销标识
        /// </summary>
        public String CancelFlag
        {
            get { return _cancelFlag; }
            set { _cancelFlag = value; }
        }

        /// <summary>
        /// 交易时间：日期+时间
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


        #region IComparable 成员

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

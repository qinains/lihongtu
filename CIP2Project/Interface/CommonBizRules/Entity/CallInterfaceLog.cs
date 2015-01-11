using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// �ӿڵ�����־ʵ����
    /// </summary>
    [Serializable]
    public class CallInterfaceLog
    {
        /// <summary>
        /// Ψһ��ID
        /// </summary>
        private Int32 _sequenceID;
        public Int32 SequenceID
        {
            get { return _sequenceID; }
            set { _sequenceID = value; }
        }

        private String _ip;
        public String IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// ҵ��ϵͳSPID
        /// </summary>
        private String _SPID;
        public String SPID
        {
            get { return _SPID; }
            set { _SPID = value; }
        }

        /// <summary>
        /// �ӿ�����
        /// </summary>
        private String _InterfaceName;
        public String InterfaceName
        {
            get { return _InterfaceName; }
            set { _InterfaceName = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        private String _InParameters;
        public String InParameters
        {
            get { return _InParameters; }
            set { _InParameters = value; }
        }

        /// <summary>
        /// ���ز���
        /// </summary>
        private String _OutParameters;
        public String OutParameters
        {
            get { return _OutParameters; }
            set { _OutParameters = value; }
        }

        /// <summary>
        /// ���ý��
        /// </summary>
        private String _CallResult;
        public String CallResult
        {
            get { return _CallResult; }
            set { _CallResult = value; }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private String _ErrMsg;
        public String ErrMsg
        {
            get { return _ErrMsg; }
            set { _ErrMsg = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        private DateTime _CreateTime;
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
    }
}

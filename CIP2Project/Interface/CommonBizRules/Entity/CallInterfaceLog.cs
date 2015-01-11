using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 接口调用日志实体类
    /// </summary>
    [Serializable]
    public class CallInterfaceLog
    {
        /// <summary>
        /// 唯一键ID
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
        /// 业务系统SPID
        /// </summary>
        private String _SPID;
        public String SPID
        {
            get { return _SPID; }
            set { _SPID = value; }
        }

        /// <summary>
        /// 接口名称
        /// </summary>
        private String _InterfaceName;
        public String InterfaceName
        {
            get { return _InterfaceName; }
            set { _InterfaceName = value; }
        }

        /// <summary>
        /// 传入参数
        /// </summary>
        private String _InParameters;
        public String InParameters
        {
            get { return _InParameters; }
            set { _InParameters = value; }
        }

        /// <summary>
        /// 返回参数
        /// </summary>
        private String _OutParameters;
        public String OutParameters
        {
            get { return _OutParameters; }
            set { _OutParameters = value; }
        }

        /// <summary>
        /// 调用结果
        /// </summary>
        private String _CallResult;
        public String CallResult
        {
            get { return _CallResult; }
            set { _CallResult = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        private String _ErrMsg;
        public String ErrMsg
        {
            get { return _ErrMsg; }
            set { _ErrMsg = value; }
        }

        /// <summary>
        /// 调用时间
        /// </summary>
        private DateTime _CreateTime;
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
    }
}

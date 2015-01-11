using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Author Lihongtu 2013.02.26
    /// </summary>
    public class SmsMessage
    {
        private String _targetNumber;
        public String TargetNumber 
        {
            get { return _targetNumber; }
            set { _targetNumber = value; }
        }// 目标号码

        private String _sendNumber;
        public String SendNumber 
        {
            get { return _sendNumber; }
            set { _sendNumber = value; }
        }// 发送号码

        private String _sequenceId;
        public String SequenceId 
        {
            get { return _sequenceId; }
            set { _sequenceId = value; }
        }//客户端设置

        private String _content;
        public String Content
        {
            get { return _content; }
            set { _content = value; }
        }// 短信内容

        private String _createTime;
        public String CreateTime 
        {
            get { return _createTime; }
            set { _createTime = value; }
        }//客户端服务IP

        private String _smsType;
        public String SmsType 
        {
            get { return _smsType; }
            set { _smsType = value; }
        } // 短信类型

        private String _receiveCode;
        public String ReceiveCode 
        {
            get { return _receiveCode; }
            set { _receiveCode = value; }
        }// 返回的状态码

        private String _receiveId;
        public String ReceiveId 
        {
            get { return _receiveId; }
            set { _receiveId = value; }
        }// 返回的smsId

        private String _receiveInfo;
        public String ReceiveInfo
        {
            get { return _receiveInfo; }
            set { _receiveInfo = value; }
        }// 返回状态描述

    }
}

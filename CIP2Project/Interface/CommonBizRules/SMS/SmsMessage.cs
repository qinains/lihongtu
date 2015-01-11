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
        }// Ŀ�����

        private String _sendNumber;
        public String SendNumber 
        {
            get { return _sendNumber; }
            set { _sendNumber = value; }
        }// ���ͺ���

        private String _sequenceId;
        public String SequenceId 
        {
            get { return _sequenceId; }
            set { _sequenceId = value; }
        }//�ͻ�������

        private String _content;
        public String Content
        {
            get { return _content; }
            set { _content = value; }
        }// ��������

        private String _createTime;
        public String CreateTime 
        {
            get { return _createTime; }
            set { _createTime = value; }
        }//�ͻ��˷���IP

        private String _smsType;
        public String SmsType 
        {
            get { return _smsType; }
            set { _smsType = value; }
        } // ��������

        private String _receiveCode;
        public String ReceiveCode 
        {
            get { return _receiveCode; }
            set { _receiveCode = value; }
        }// ���ص�״̬��

        private String _receiveId;
        public String ReceiveId 
        {
            get { return _receiveId; }
            set { _receiveId = value; }
        }// ���ص�smsId

        private String _receiveInfo;
        public String ReceiveInfo
        {
            get { return _receiveInfo; }
            set { _receiveInfo = value; }
        }// ����״̬����

    }
}

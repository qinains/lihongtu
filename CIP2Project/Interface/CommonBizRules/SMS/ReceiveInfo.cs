using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Author 2013.02.26
    /// </summary>
    public class ReceiveInfo
    {
        public static Dictionary<String,String> map = new Dictionary<string,string>();
        
        public ReceiveInfo()
        {
            map.Add(ReceiveCode.LOGIN_SUCCESS, "��¼�ɹ�");
            map.Add(ReceiveCode.LAWLESS_USER_NAME, "�Ƿ��û���");
            map.Add(ReceiveCode.LAWLESS_USER_PASS, "�������");
            map.Add(ReceiveCode.RECEIVE_SUCCESS, "���ݰ���ȷ�����ճɹ�");
            map.Add(ReceiveCode.LAWLESS_FROMAT, "�Ƿ���ʽ���ݰ�");
            map.Add(ReceiveCode.LAWLESS_AIM_PHONE, "�Ƿ�Ŀ�����");
            map.Add(ReceiveCode.LINK_OVERTIME, "���ӳ�ʱ");
            map.Add(ReceiveCode.INFO_TOO_LONG, "��Ϣ���ݳ���̫��");
            map.Add(ReceiveCode.LAWLESS_SEND_PHONE, "�Ƿ����ͺ���");
            map.Add(ReceiveCode.LAWLESS_SEND_TIME, "�Ƿ����뷢��ʱ��");
            map.Add(ReceiveCode.LAWLESS_MSG_TYPE, "�Ƿ�����ҵ������");
            map.Add(ReceiveCode.USER_NOT_OPEN, "�û�û�п�ͨ");
            map.Add(ReceiveCode.USER_NOT_LOGIN, "�㻹δ��¼���߻Ự�ѹ��ڣ����ȵ�¼���ٷ������ݰ�");
            map.Add(ReceiveCode.LAWLESS_LOGIN_FROMAT, "Login���ݰ���ʽ����");
            map.Add(ReceiveCode.TOO_MANY_REQUESTS, "�������̫��");
            map.Add(ReceiveCode.SEND_LIMIT, "���ŷ����������������޶������ֵ");
            map.Add(ReceiveCode.LAWLESS_LOGIN_IP, "�Ƿ��ĵ�¼ip");
            map.Add(ReceiveCode.TOO_MANY_SAME_MESSAGE, "��ʱ���ڴ�������ͬ��Ϣ����ͬĿ��");
            map.Add(ReceiveCode.TOO_MANY_SMS, "�������");

            map.Add(ReceiveCode.NO_SUPOORT_OPER, "��֧�ִ���������");
            map.Add(ReceiveCode.NO_MORE_LESS, "δ�����쳣");


        }
    }
}

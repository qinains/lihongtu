using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Author 2013.02.26
    /// </summary>
    public class ReceiveCode
    {
	    public static  String LOGIN_SUCCESS = "000";// ��½�ɹ�
	    public static  String LAWLESS_USER_NAME = "002";// �Ƿ��û���
	    public static  String LAWLESS_USER_PASS = "003";// �������
	    public static  String RECEIVE_SUCCESS = "001";// ���ݰ���ȷ�����ճɹ�
	    public static  String LAWLESS_FROMAT = "004";// �Ƿ���ʽ���ݰ�
	    public static  String LAWLESS_AIM_PHONE = "005";// �Ƿ�Ŀ�����
	    public static  String LINK_OVERTIME = "006";// ���ӳ�ʱ
	    public static  String INFO_TOO_LONG = "007";// ��Ϣ���ݳ���̫��
	    public static  String LAWLESS_SEND_PHONE = "008";// �Ƿ����ͺ���
	    public static  String LAWLESS_SEND_TIME = "009";// �Ƿ����뷢��ʱ��
	    public static  String LAWLESS_MSG_TYPE = "010";// �Ƿ�����ҵ������
	    public static  String USER_NOT_OPEN = "011";// �û�û�п�ͨ
	    public static  String USER_NOT_LOGIN = "012";// �㻹δ��¼���߻Ự�ѹ��ڣ����ȵ�¼���ٷ������ݰ�
	    public static  String LAWLESS_LOGIN_FROMAT = "013";// Login���ݰ���ʽ����
	    public static  String TOO_MANY_REQUESTS = "014";// �������̫��
	    public static  String SEND_LIMIT = "015";// ���ŷ����������������޶������ֵ
	    public static  String LAWLESS_LOGIN_IP = "016"; // �Ƿ��ĵ�¼ip
	    public static  String TOO_MANY_SAME_MESSAGE = "017"; // ��ʱ���ڴ�������ͬ��Ϣ����ͬĿ��
	    public static  String TOO_MANY_SMS = "018"; // �������
	    public static  String NO_SUPOORT_OPER = "019"; // ��֧�ִ���������
    	
	    public static  String NO_MORE_LESS = "099"; // δ�����쳣,�ͻ���̫�ɣ��쳣�벻��
    }
}

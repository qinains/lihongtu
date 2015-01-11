//==============================================================================================================
//
// Class Name: ConstDefinition
// Description: ��������
// Author: Է��
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-05
//
//==============================================================================================================
using System;

namespace Linkage.BestTone.Interface.Utility
{
	/// <summary>
	/// Summary description for ConstDefinition.
	/// </summary>
	public class ConstDefinition
	{
        public const long IPAddress_First = 16777216;
        public const long IPAddress_Second = 65536;
        public const long IPAddress_Third = 256;
        public const long IPAddress_Forth = 1; 

        /// <summary>
        /// ͨ�ÿ���������
        ///  1 - ��
        ///  0 - ��
        /// </summary>
        public const string ConfigurationFlag_On = "1";
        public const string ConfigurationFlag_Off = "0";

		/*
		 * ���ò�������
		 */
		/// <summary>
        /// �ͻ�ID����
		/// </summary>
		public const int Length_CustID = 16;

        /// <summary>
        /// CustID Max Length
        /// </summary>
        public const int Length_Max_CustID = 16;
        /// <summary>
        /// �û��ʺų���
        /// </summary>
        public const int Length_UserAccount = 16;
        /// <summary>
        /// UserAccount Max Length
        /// </summary>
        public const int Length_Max_UserAccount = 16;
        /// <summary>
        /// UserAccount Min Length
        /// </summary>
        public const int Length_Min_UserAccount = 0;

        /// <summary>
        /// InnerCardID 16 λȫ����
        /// </summary>
        public const int Length_InnerCardID = 16;
        public const int Length_Min_Password = 6;

        /// <summary>
        /// SYSID����
        /// </summary>
        public const int Length_SYSID = 4;
		/// <summary>
        /// SPID����
		/// </summary>
		public const int Length_SPID = 8;
        /// <summary>
        /// ����id����
        /// </summary>
        public const int Length_ServiceID = 16;
        /// <summary>
        /// �绰���볤��
        /// </summary>
        public const int Length_PhoneNum = 20;
        /// <summary>
        /// ʡ���볤��
        /// </summary>
        public const int Length_ProvinceID = 2;
        /// <summary>
        /// ���׺ų���
        /// </summary>
        public const int Length_TransactionID = 36;
        /// <summary>
        /// �绰���ų���
        /// </summary>
        public const int Length_AreaCode = 3;
        /// <summary>
        /// �û�״̬���� 00-���� 01-���� 02-��ͣ 03-ע��
        /// </summary>
        public const int Length_CustStatus = 2;
        /// <summary>
        /// �ͻ�״̬���֪ͨ�ӿ�Description��󳤶�
        /// </summary>
        public const int MaxLength_CustStatusChangeDescription = 256;
        /// <summary>
        /// �绰������󳤶�
        /// </summary>
        public const int MaxLength_PhoneNum = 20;
        /// <summary>
        /// ʱ�䳤��
        /// </summary>
        public const int Length_TimeStamp = 19;
        /// <summary>
        /// ������ֵ�ַ�������
        /// </summary>
        public const int Length_IntegralInfo = 20;

        /// <summary>
        /// �ͻ����𳤶�
        /// </summary>
        public const int Length_CustLevel = 1;

        public const int Length_Max_EnterpriseID = 30;
        public const int Length_Max_EnterpriseName = 50;
        public const int Length_EnterpriseType = 2;



        /// <summary>
        /// ֤�����ͷ�Χ
        /// </summary>
        public const string Span_CertificateType = "0;1;2;3;4;5;6;7;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;";
        /// <summary>
        /// ע�᷽ʽѡ��
        /// </summary>
        public const string Span_RegistrationStyle = "01;02;03;04;05;06";
        /// <summary>
        /// �û�����ѡ��
        /// </summary>
        public const string Span_UserType = "10;11;12;13;14;20;30;40;41;43;90;42;50";//"00;01;02;03;09;99";
        /// <summary>
        /// �û�״̬ѡ��
        /// </summary>
        public const string Span_UserStatus = "00;01;02;03";
        /// <summary>
        /// �Ա�ѡ��
        /// </summary>
        public const string Span_Sex = "0;1;2";
        /// <summary>
        /// �ͻ�����ѡ��
        /// </summary>
        public const string Span_CustLevel = "0;1;2;3;4;5";
        /// <summary>
        /// ����ˮƽѡ��
        /// </summary>
        public const string Span_EduLevel = "1;2;3;4;5;6";
        /// <summary>
        /// ����ˮƽѡ��
        /// </summary>
        public const string Span_IncomeLevel = "0;1;2;3;4;5";
        /// <summary>
        /// ֧���ʺ�����ѡ��
        /// </summary>
        public const string Span_PaymentAccountType = "00;01";
        /// <summary>
        /// ʹ������ѡ��
        /// </summary>
        public const string Span_SubscribeStype = "1;2;3";
        /// <summary>
        /// ��֤����
        /// </summary>
        public const string Span_AuthenType = "1;2;3;4";

        /// <summary>
        /// �Ƿ��ʼ�ѡ�Χ
        /// </summary>
        public const string Span_IsPost = "0;1";

        /// <summary>
        /// 
        /// </summary>
        public const string Span_GradeOrCredit = "01;02";


    }// End Class
}

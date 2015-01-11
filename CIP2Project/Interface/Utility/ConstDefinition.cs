//==============================================================================================================
//
// Class Name: ConstDefinition
// Description: 常量定义
// Author: 苑峰
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
        /// 通用开关配置项
        ///  1 - 开
        ///  0 - 关
        /// </summary>
        public const string ConfigurationFlag_On = "1";
        public const string ConfigurationFlag_Off = "0";

		/*
		 * 常用参数长度
		 */
		/// <summary>
        /// 客户ID长度
		/// </summary>
		public const int Length_CustID = 16;

        /// <summary>
        /// CustID Max Length
        /// </summary>
        public const int Length_Max_CustID = 16;
        /// <summary>
        /// 用户帐号长度
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
        /// InnerCardID 16 位全卡号
        /// </summary>
        public const int Length_InnerCardID = 16;
        public const int Length_Min_Password = 6;

        /// <summary>
        /// SYSID长度
        /// </summary>
        public const int Length_SYSID = 4;
		/// <summary>
        /// SPID长度
		/// </summary>
		public const int Length_SPID = 8;
        /// <summary>
        /// 服务id长度
        /// </summary>
        public const int Length_ServiceID = 16;
        /// <summary>
        /// 电话号码长度
        /// </summary>
        public const int Length_PhoneNum = 20;
        /// <summary>
        /// 省代码长度
        /// </summary>
        public const int Length_ProvinceID = 2;
        /// <summary>
        /// 交易号长度
        /// </summary>
        public const int Length_TransactionID = 36;
        /// <summary>
        /// 电话区号长度
        /// </summary>
        public const int Length_AreaCode = 3;
        /// <summary>
        /// 用户状态长度 00-正常 01-冻结 02-暂停 03-注销
        /// </summary>
        public const int Length_CustStatus = 2;
        /// <summary>
        /// 客户状态变更通知接口Description最大长度
        /// </summary>
        public const int MaxLength_CustStatusChangeDescription = 256;
        /// <summary>
        /// 电话号码最大长度
        /// </summary>
        public const int MaxLength_PhoneNum = 20;
        /// <summary>
        /// 时间长度
        /// </summary>
        public const int Length_TimeStamp = 19;
        /// <summary>
        /// 积分数值字符串长度
        /// </summary>
        public const int Length_IntegralInfo = 20;

        /// <summary>
        /// 客户级别长度
        /// </summary>
        public const int Length_CustLevel = 1;

        public const int Length_Max_EnterpriseID = 30;
        public const int Length_Max_EnterpriseName = 50;
        public const int Length_EnterpriseType = 2;



        /// <summary>
        /// 证件类型范围
        /// </summary>
        public const string Span_CertificateType = "0;1;2;3;4;5;6;7;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;";
        /// <summary>
        /// 注册方式选项
        /// </summary>
        public const string Span_RegistrationStyle = "01;02;03;04;05;06";
        /// <summary>
        /// 用户类型选项
        /// </summary>
        public const string Span_UserType = "10;11;12;13;14;20;30;40;41;43;90;42;50";//"00;01;02;03;09;99";
        /// <summary>
        /// 用户状态选项
        /// </summary>
        public const string Span_UserStatus = "00;01;02;03";
        /// <summary>
        /// 性别选项
        /// </summary>
        public const string Span_Sex = "0;1;2";
        /// <summary>
        /// 客户级别选项
        /// </summary>
        public const string Span_CustLevel = "0;1;2;3;4;5";
        /// <summary>
        /// 教育水平选项
        /// </summary>
        public const string Span_EduLevel = "1;2;3;4;5;6";
        /// <summary>
        /// 收入水平选项
        /// </summary>
        public const string Span_IncomeLevel = "0;1;2;3;4;5";
        /// <summary>
        /// 支付帐号类型选项
        /// </summary>
        public const string Span_PaymentAccountType = "00;01";
        /// <summary>
        /// 使用类型选项
        /// </summary>
        public const string Span_SubscribeStype = "1;2;3";
        /// <summary>
        /// 认证类型
        /// </summary>
        public const string Span_AuthenType = "1;2;3;4";

        /// <summary>
        /// 是否邮寄选项范围
        /// </summary>
        public const string Span_IsPost = "0;1";

        /// <summary>
        /// 
        /// </summary>
        public const string Span_GradeOrCredit = "01;02";


    }// End Class
}

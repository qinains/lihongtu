using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 用户信息记录列表
    /// </summary>
    public class BasicInfoV2Record
    {
        public string CustID;
        public string CustType;
        public string RealName;
        public string Sex;
        public string PhoneClass;
        public string ExtendField;
    }

    public class BasicInfoV3Record
    {
        public string CustID;
        public string CustType;
        public string RealName;
        public string EnterpriseName;
        public string Sex;
        public string PhoneClass;
        public string ExtendField;
    }

    /// <summary>
    /// 联系电话记录
    /// </summary>
    public class PhoneRecord
    {
        public string Phone;
        public string PhoneClass;
    }

    public class PhoneRecordV2
    {
        public string Phone;
        public string PhoneType;
        public string PhoneClass;
    }

    /// <summary>
    /// 客户密码问题记录
    /// </summary>
    public class PwdQARecord
    {
        public int QuestionID;
        public string Answer;
        public string ExtendField;
    }

    /// <summary>
    /// 密码问题记录
    /// </summary>
    public class PwdQuestionRecord
    {
        public int QuestionID;
        public string Question;
        public string ExtendField;
    }


    public class DictCity
    {
        public string ID;
        public string Name;
        public string Code;
        public int Grade;
        public DictCity dc;
    }



    /// <summary>
    /// 客户地址信息
    /// </summary>
    public class AddressInfo
    {
        public long AddressID;
        public string AreaCode;
        public string Address;
        public string Zipcode;
        public string Type;
        public string OtherType;
        public string RelationPerson;
        public string Mobile;
        public string FixedPhone;
    }

    /// <summary>
    /// 客户地址信息记录
    /// </summary>
    public class AddressInfoRecord
    {
        public long AddressID;
        public string AreaCode;
        public string Address;
        public string Zipcode;
        public string Type;
        public string OtherType;
        public string RelationPerson;
        public string Mobile;
        public string FixedPhone;
        public string DealType;
        public string ExtendField;
    }

    /// <summary>
    /// 常旅客信息
    /// </summary>
    public class FrequentUserInfo
    {
        public long FrequentUserID;
        public string DealType;
        public string RealName;
        public string CertificateCode;
        public string CertificateType;
        public string Phone;
        public string ExtendField;
    }

    /// <summary>
    /// 支付账号列表
    /// </summary>
    public class PaymentAccountRecord
    {
        public string PaymentAccount;
        public string Institution;
        public string Type;
        public string ExtendField;
    }



    /// <summary>
    /// 商旅卡号记录
    /// </summary>
    public class TourCardIDRecord
    {
        public string CardID;
        public string CardType;
        public string InnerCardID;
    }

    /// <summary>
    /// 卡号记录
    /// </summary>
    public class CustAuthenInfoRecord
    {
        public string AuthenName;
        public string AuthenType;
    }

    public class CustInfoNotifyRecord
    {
            public string SequenceID;
            public string CustID;
            public string OPType;
            public string DealType;
            public string PaymentPwd;
            public string ProvinceId;
			public string AreaId;
			public string CustType;
			public string CertificateType;
			public string CertificateCode;
			public string RealName;
			public string CustLevel;
			public string Sex;
			public string RegistrationSource;
			public string UserName;
			public string NickName;
			public string Email;
			public string EmailClass;
			public string SourceSpid;
			public string OuterId;
			public string DealTime;
            public string CreateTime;
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    //用户基本信息
    public class UserBasicInfoRecord
    {
        public string CustID;
        public string UserAccount;
        public string RealName;
        public string CertificateCode;
        public string CertificateType;
        public string Status;
        public BoundPhoneRecord[] BoundPhoneRecords;  
    }

    //用户基本信息
    public class UserInfo
    {
        public string UserType;
        public string UserAccount;
        public string Password;
        public string CustID;
        public string UProvinceID;
        public string AreaCode;
        public string OuterID;
        public string Status;
        public string RealName;
        public string CertificateCode;
        public string CertificateType;
        public string Birthday;
        public string Sex;
        public string CustLevel;
        public string EduLevel;
        public string Favorite; 
        public string IncomeLevel;
        public string Email;
        public string PaymentAccountType; 
        public string PaymentAccount;
        public string PaymentAccountPassword;
        public string CustContactTel;
        public string EnterpriseID;
        public string IsPost;
        public string ExtendField;
        public AssessmentInfoRecord[] AssessmentInfos;
        public AddressRecord[] AddressRecords;
        public BoundPhoneRecord[] BoundPhoneRecords;


    }

    //评估信息记录
    public class AssessmentInfoRecord
    { 
        public long Credit;
        public string CreditLevel;
        public long Score;
        public string TotalScore;
        public long AvailableScore;
    }

    //绑定电话
    public class BoundPhoneRecord
    {
        public string Phone;//用户绑定的主叫电话，固话含区号，手机号不加0
    
    }

    //联系方式
    public class AddressRecord
    { 
        public string LinkMan;
        public string ContactTel;
        public string Address;
        public string Zipcode;
        public string Type;
    
    }

    //订购记录
    public class SubscriptionRecord
    { 
        public string CustID;
        public string UserAccount;
        public string SubscribeStyle;
        public string ServiceID;
        public string ServiceName;
        public string StartTime;
        public string EndTime;
        public string TransactionID;
    }

    //帐号记录
    public class UserAccountRecord
    { 
       public string UserAccount;
       public string CustID;    
    }

    //积分明细记录
    public class ScoreDetailRecord
    { 
        public string SPID;
        public long Score;
        public string ScoreType;
        public string Description;
        public string UploadTime;
        public string EffectiveTime;
        public string ExpireTime;   
    }


    public class AuthenStyleInfoRecord
    {
        public string CustID;
        public string AuthenName;
        public string AuthenType;
    }

    public class AuthenRecord
    {
        public string AuthenType;
        public string AuthenName;
        public string ExtendField;
        public string areaid;
    }
}

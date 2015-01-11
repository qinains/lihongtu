using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    //�û�������Ϣ
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

    //�û�������Ϣ
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

    //������Ϣ��¼
    public class AssessmentInfoRecord
    { 
        public long Credit;
        public string CreditLevel;
        public long Score;
        public string TotalScore;
        public long AvailableScore;
    }

    //�󶨵绰
    public class BoundPhoneRecord
    {
        public string Phone;//�û��󶨵����е绰���̻������ţ��ֻ��Ų���0
    
    }

    //��ϵ��ʽ
    public class AddressRecord
    { 
        public string LinkMan;
        public string ContactTel;
        public string Address;
        public string Zipcode;
        public string Type;
    
    }

    //������¼
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

    //�ʺż�¼
    public class UserAccountRecord
    { 
       public string UserAccount;
       public string CustID;    
    }

    //������ϸ��¼
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

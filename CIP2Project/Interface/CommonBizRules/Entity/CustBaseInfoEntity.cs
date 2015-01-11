using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 客户基本信息实体类
    /// </summary>
    public class CustBaseInfoEntity
    {
        private String _custID;
        public String CustID
        {
            get { return _custID; }
            set { _custID = value; }
        }

        private String _provinceID;
        public String ProvinceID
        {
            get { return _provinceID; }
            set { _provinceID = value; }
        }

        private String _areaID;
        public String AreaID
        {
            get { return _areaID; }
            set { _areaID = value; }
        }

        private String _custType;
        public String CustType
        {
            get { return _custType; }
            set { _custType = value; }
        }

        private String _voicePwd;
        public String VoicePwd
        {
            get { return _voicePwd; }
            set { _voicePwd = value; }
        }

        private String _webPwd;
        public String WebPwd
        {
            get { return _webPwd; }
            set { _webPwd = value; }
        }

        private String _certificateType;
        public String CertificateType
        {
            get { return _certificateType; }
            set { _certificateType = value; }
        }

        private String _certificateCode;
        public String CertificateCode
        {
            get { return _certificateCode; }
            set { _certificateCode = value; }
        }

        private String _realName;
        public String RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }

        private String _custLevel;
        public String CustLevel
        {
            get { return _custLevel; }
            set { _custLevel = value; }
        }

        private String _sex;
        public String Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        private Int32 _registrationSource;
        public Int32 RegistrationSource
        {
            get { return _registrationSource; }
            set { _registrationSource = value; }
        }

        private String _userName;
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private String _nickName;
        public String NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        private String _status;
        public String Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private String _email;
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private Int32 _emailClass;
        public Int32 EmailClass
        {
            get { return _emailClass; }
            set { _emailClass = value; }
        }

        private String _sourceSPID;
        public String SourceSPID
        {
            get { return _sourceSPID; }
            set { _sourceSPID = value; }
        }

        private String _outerID;
        public String OuterID
        {
            get { return _outerID; }
            set { _outerID = value; }
        }

        private DateTime _dealTime;
        public DateTime DealTime
        {
            get { return _dealTime; }
            set { _dealTime = value; }
        }

        private DateTime _createTime;
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        private Int32 _enterpriseID;
        public Int32 EnterpriseID
        {
            get { return _enterpriseID; }
            set { _enterpriseID = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class AccountInfo
    {
        private string _name;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _loginID;
        /// <summary>
        /// 登录号
        /// </summary>
        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }
        }

        private string _productNO;
        /// <summary>
        /// 电信产品号
        /// </summary>
        public string ProductNO
        {
            get { return _productNO; }
            set { _productNO = value; }
        }

        private string _isRealName;
        /// <summary>
        /// 是否已实名认证
        /// </summary>
        public string IsRealName
        {
            get { return _isRealName; }
            set { _isRealName = value; }
        }
        
        private string _registerType;
        /// <summary>
        /// 注册类型
        /// </summary>
        public string RegisterType
            {
                get { return _registerType; }
                set { _registerType = value; }
        }

        private string _isApplyCert;
        /// <summary>
        /// 是否申请个人证书
        /// </summary>
        public string IsApplyCert 
            {
                get { return _isApplyCert; }
                set { _isApplyCert = value; }
        }

        private string _customLevel;
        /// <summary>
        /// 客户级别
        /// </summary>
        public string CustomLevel 
            {
                get { return _customLevel; }
                set { _customLevel = value; }
        }

        private string _customType;
        /// <summary>
        /// 客户类别
        /// </summary>
        public string CustomType
        {
            get { return _customType; }
            set { _customType = value; }
        }

        private string _regcellPhoneNum;
        /// <summary>
        /// 联系手机
        /// </summary>
        public string RegcellPhoneNum
        {
            get { return _regcellPhoneNum; }
            set { _regcellPhoneNum = value; }
        }

        private string _regEmail;
        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string RegEmailL{
            get { return _regEmail; }
            set { _regEmail = value; }
        }

        private string _loginPassword;
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPassword{
            get { return _loginPassword; }
            set { _loginPassword = value; }
        }

        private string _sex;
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex{
            get { return _sex; }
            set { _sex = value; }
        }

        private string _certType;
        /// <summary>
        /// 证件类型
        /// </summary>
        public string CertType{
            get { return _certType; }
            set { _certType = value; }
        }

        private string _certNum;
        /// <summary>
        /// 证件号码
        /// </summary>
        public string CertNum{
            get { return _certNum; }
            set { _certNum = value; }
        }

        private string _familyTel;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string FamilyTel{
            get { return _familyTel; }
            set { _familyTel = value; }
        }

        private string _officeTel;
        /// <summary>
        /// 其他电话
        /// </summary>
        public string OfficeTel{
            get { return _officeTel; }
            set { _officeTel = value; }
        }

        private string _apanage;
        /// <summary>
        /// 属地
        /// </summary>
        public string Apanage
        {
            get { return _apanage; }
            set { _apanage = value; }
        }

        private string _areaCode;
        /// <summary>
        /// 地区代码
        /// </summary>
        public string AreaCode{
            get { return _areaCode; }
            set { _areaCode = value; }
        }

        private string _cityCode;
        /// <summary>
        /// 城市编码
        /// </summary>
        public string CityCode{
            get { return _cityCode; }
            set { _cityCode = value; }
        }

        private String _contractAdd;
        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContractAdd
        {
            get { return _contractAdd; }
            set { _contractAdd = value; }
        }

        private string _companyAdd;
        /// <summary>
        /// 单位地址
        /// </summary>
        public string CompanyAdd{
            get { return _companyAdd; }
            set { _companyAdd = value; }
        }

        private string _companyZip;
        /// <summary>
        /// 单位邮编
        /// </summary>
        public string CompanyZip{
            get { return _companyZip; }
            set { _companyZip = value; }
        }

        private string _companyCode;
        /// <summary>
        /// 单位代码
        /// </summary>
        public string CompanyCode{
            get { return _companyCode; }
            set { _companyCode = value; }
        }

        private string _acceptOrgCode;
        /// <summary>
        /// 受理机构代码
        /// </summary>
        public string AcceptOrgCode{
            get { return _acceptOrgCode; }
            set { _acceptOrgCode = value; }
        }

        private string _acceptUID;
        /// <summary>
        /// 受理人
        /// </summary>
        public string AcceptUID{
            get { return _acceptUID; }
            set { _acceptUID = value; }
        }

        private string _acceptAreaCode;
        /// <summary>
        /// 受理地区代码
        /// </summary>
        public string AcceptAreaCode{
            get { return _acceptAreaCode; }
            set { _acceptAreaCode = value; }
        }

        private string _acceptCityCode;
        /// <summary>
        /// 受理城市代码
        /// </summary>
        public string AcceptCityCode{
            get { return _acceptCityCode; }
            set { _acceptCityCode = value; }
        }

        private string _acceptChannel;
        /// <summary>
        /// 受理渠道
        /// </summary>
        public string AcceptChannel{
            get { return _acceptChannel; }
            set { _acceptChannel = value; }
        }

        private string _acceptSeqNo;
        /// <summary>
        /// 受理流水号
        /// </summary>
        public string AcceptSeqNo{
            get { return _acceptSeqNo; }
            set { _acceptSeqNo = value; }
        }

        private string _feeFlag;
        /// <summary>
        /// 服务收费标志
        /// </summary>
        public string FeeFlag{
            get { return _feeFlag; }
            set { _feeFlag = value; }
        }

        private string _feeAmt;
        /// <summary>
        /// 服务收费金额
        /// </summary>
        public string FeeAmt{
            get { return _feeAmt; }
            set { _feeAmt = value; }
        }

        private string _inputUID;
        /// <summary>
        /// 操作人员代码
        /// </summary>
        public string InputUID{
            get { return _inputUID; }
            set { _inputUID = value; }
        }

        private string _inputTime;
        /// <summary>
        /// 操作时间
        /// </summary>
        public string InputTime{
            get { return _inputTime; }
            set { _inputTime = value; }
        }

        private string _checkUID;
        /// <summary>
        /// 授权员工标识
        /// </summary>
        public string CheckUID{
            get { return _checkUID; }
            set { _checkUID = value; }
        }

        private string _checkTime;
        /// <summary>
        /// 授权时间
        /// </summary>
        public string CheckTime{
            get { return _checkTime; }
            set { _checkTime = value; }
        }



    }
}

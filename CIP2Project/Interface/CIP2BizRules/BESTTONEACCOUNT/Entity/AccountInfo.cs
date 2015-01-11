using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// �˻���Ϣ
    /// </summary>
    public class AccountInfo
    {
        private string _name;
        /// <summary>
        /// �û���
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _loginID;
        /// <summary>
        /// ��¼��
        /// </summary>
        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }
        }

        private string _productNO;
        /// <summary>
        /// ���Ų�Ʒ��
        /// </summary>
        public string ProductNO
        {
            get { return _productNO; }
            set { _productNO = value; }
        }

        private string _isRealName;
        /// <summary>
        /// �Ƿ���ʵ����֤
        /// </summary>
        public string IsRealName
        {
            get { return _isRealName; }
            set { _isRealName = value; }
        }
        
        private string _registerType;
        /// <summary>
        /// ע������
        /// </summary>
        public string RegisterType
            {
                get { return _registerType; }
                set { _registerType = value; }
        }

        private string _isApplyCert;
        /// <summary>
        /// �Ƿ��������֤��
        /// </summary>
        public string IsApplyCert 
            {
                get { return _isApplyCert; }
                set { _isApplyCert = value; }
        }

        private string _customLevel;
        /// <summary>
        /// �ͻ�����
        /// </summary>
        public string CustomLevel 
            {
                get { return _customLevel; }
                set { _customLevel = value; }
        }

        private string _customType;
        /// <summary>
        /// �ͻ����
        /// </summary>
        public string CustomType
        {
            get { return _customType; }
            set { _customType = value; }
        }

        private string _regcellPhoneNum;
        /// <summary>
        /// ��ϵ�ֻ�
        /// </summary>
        public string RegcellPhoneNum
        {
            get { return _regcellPhoneNum; }
            set { _regcellPhoneNum = value; }
        }

        private string _regEmail;
        /// <summary>
        /// ��ϵ����
        /// </summary>
        public string RegEmailL{
            get { return _regEmail; }
            set { _regEmail = value; }
        }

        private string _loginPassword;
        /// <summary>
        /// ��¼����
        /// </summary>
        public string LoginPassword{
            get { return _loginPassword; }
            set { _loginPassword = value; }
        }

        private string _sex;
        /// <summary>
        /// �Ա�
        /// </summary>
        public string Sex{
            get { return _sex; }
            set { _sex = value; }
        }

        private string _certType;
        /// <summary>
        /// ֤������
        /// </summary>
        public string CertType{
            get { return _certType; }
            set { _certType = value; }
        }

        private string _certNum;
        /// <summary>
        /// ֤������
        /// </summary>
        public string CertNum{
            get { return _certNum; }
            set { _certNum = value; }
        }

        private string _familyTel;
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        public string FamilyTel{
            get { return _familyTel; }
            set { _familyTel = value; }
        }

        private string _officeTel;
        /// <summary>
        /// �����绰
        /// </summary>
        public string OfficeTel{
            get { return _officeTel; }
            set { _officeTel = value; }
        }

        private string _apanage;
        /// <summary>
        /// ����
        /// </summary>
        public string Apanage
        {
            get { return _apanage; }
            set { _apanage = value; }
        }

        private string _areaCode;
        /// <summary>
        /// ��������
        /// </summary>
        public string AreaCode{
            get { return _areaCode; }
            set { _areaCode = value; }
        }

        private string _cityCode;
        /// <summary>
        /// ���б���
        /// </summary>
        public string CityCode{
            get { return _cityCode; }
            set { _cityCode = value; }
        }

        private String _contractAdd;
        /// <summary>
        /// ��ϵ��ַ
        /// </summary>
        public string ContractAdd
        {
            get { return _contractAdd; }
            set { _contractAdd = value; }
        }

        private string _companyAdd;
        /// <summary>
        /// ��λ��ַ
        /// </summary>
        public string CompanyAdd{
            get { return _companyAdd; }
            set { _companyAdd = value; }
        }

        private string _companyZip;
        /// <summary>
        /// ��λ�ʱ�
        /// </summary>
        public string CompanyZip{
            get { return _companyZip; }
            set { _companyZip = value; }
        }

        private string _companyCode;
        /// <summary>
        /// ��λ����
        /// </summary>
        public string CompanyCode{
            get { return _companyCode; }
            set { _companyCode = value; }
        }

        private string _acceptOrgCode;
        /// <summary>
        /// �����������
        /// </summary>
        public string AcceptOrgCode{
            get { return _acceptOrgCode; }
            set { _acceptOrgCode = value; }
        }

        private string _acceptUID;
        /// <summary>
        /// ������
        /// </summary>
        public string AcceptUID{
            get { return _acceptUID; }
            set { _acceptUID = value; }
        }

        private string _acceptAreaCode;
        /// <summary>
        /// �����������
        /// </summary>
        public string AcceptAreaCode{
            get { return _acceptAreaCode; }
            set { _acceptAreaCode = value; }
        }

        private string _acceptCityCode;
        /// <summary>
        /// ������д���
        /// </summary>
        public string AcceptCityCode{
            get { return _acceptCityCode; }
            set { _acceptCityCode = value; }
        }

        private string _acceptChannel;
        /// <summary>
        /// ��������
        /// </summary>
        public string AcceptChannel{
            get { return _acceptChannel; }
            set { _acceptChannel = value; }
        }

        private string _acceptSeqNo;
        /// <summary>
        /// ������ˮ��
        /// </summary>
        public string AcceptSeqNo{
            get { return _acceptSeqNo; }
            set { _acceptSeqNo = value; }
        }

        private string _feeFlag;
        /// <summary>
        /// �����շѱ�־
        /// </summary>
        public string FeeFlag{
            get { return _feeFlag; }
            set { _feeFlag = value; }
        }

        private string _feeAmt;
        /// <summary>
        /// �����շѽ��
        /// </summary>
        public string FeeAmt{
            get { return _feeAmt; }
            set { _feeAmt = value; }
        }

        private string _inputUID;
        /// <summary>
        /// ������Ա����
        /// </summary>
        public string InputUID{
            get { return _inputUID; }
            set { _inputUID = value; }
        }

        private string _inputTime;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public string InputTime{
            get { return _inputTime; }
            set { _inputTime = value; }
        }

        private string _checkUID;
        /// <summary>
        /// ��ȨԱ����ʶ
        /// </summary>
        public string CheckUID{
            get { return _checkUID; }
            set { _checkUID = value; }
        }

        private string _checkTime;
        /// <summary>
        /// ��Ȩʱ��
        /// </summary>
        public string CheckTime{
            get { return _checkTime; }
            set { _checkTime = value; }
        }



    }
}

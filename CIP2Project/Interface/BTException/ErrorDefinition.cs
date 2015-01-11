//==============================================================================================================
//
// Class Name: ErrorDefinition
// Description: ������������
// Author: Է��
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-04
//
//==============================================================================================================
using System;

namespace Linkage.BestTone.Interface.BTException
{
    /// <summary>
    /// Summary description for ErrorDefinition.
    /// </summary>
    public class ErrorDefinition
    {

        #region ������

        public const int IError_Result_Success_Code = 0;
        public const string IError_Result_Success_Msg = "�ɹ�";

        public const int IError_Result_UnknowError_Code = -19999;
        public const string IError_Result_UnknowError_Msg = "δ֪����";

        public const int IError_Result_NotReallySuccess_Code = 1;
        public const string IError_Result_NotReallySuccess_Msg = "�ɹ�����,����������С��������";

        public const int IError_Result_UnknowQueryError_Code = -19998;
        public const string IError_Result_UnknowQueryError_Msg = "����δ֪ԭ���µĲ�ѯʧ��";

        public const int IError_Result_Null_Code = -1;
        public const string IError_Result_Null_Msg = "�޷����������û���Ϣ";

        #endregion

        #region SP����(-20000��-20499)

        public const int IError_Result_SPError_Code = -20000;
        public const string IError_Result_SPError_Msg = "SP����";

        public const int IError_Result_SPNotExist_Code = -20001;
        public const string IError_Result_SPNotExist_Msg = "SPID������";

        public const int IError_Result_SPPwdError_Code = -20002;
        public const string IError_Result_SPPwdError_Msg = "SP�������";

        public const int IError_Result_SPAuthorizationFail_Code = -20003;
        public const string IError_Result_SPAuthorizationFail_Msg = "SPδ��õ��ñ��ӿڵ���Ȩ";

        public const int IError_Result_SPServiceNotExist_Code = -20004;
        public const string IError_Result_SPServiceNotExist_Msg = "Service������";

        public const int IError_Result_SPServiceStatusError_Code = -20005;
        public const string IError_Result_SPServiceStatusError_Msg = "Service״̬�쳣";

        public const int IError_Result_SPServiceItemStatusError_Code = -20006;
        public const string IError_Result_SPServiceItemStatusError_Msg = "ServiceItem״̬�쳣";

        public const int IError_Result_SPServiceExpireForTwoMonth_Code = -20007;
        public const string IError_Result_SPServiceExpireForTwoMonth_Msg = "���ϴη�����ʱ���Ѿ�����2����,�������ٷ������߿۷�";

        public const int IError_Result_SPNoQualification_Code = -20008;
        public const string IError_Result_SPNoQualification_Msg = "SP���߱�����֧���ʸ񣬻����ǿ�ƽ̨û��Ϊ��SP��ͨ����֧���ʸ�";

        public const int IError_Result_PaymentTimesError_Code = -20009;
        public const string IError_Result_PaymentTimesError_Msg = "һ��������ֻ������һ������֧������";

        #endregion

        #region �û�����(-20500��20999)

        public const int IError_Result_UserError_Code = -20500;
        public const string IError_Result_UserError_Msg = "�û�����";

        public const int IError_Result_UserAuthorizationFail_Code = -20501;
        public const string IError_Result_UserAuthorizationFail_Msg = "�û���֤ʧ��";

        public const int IError_Result_UserStatusError_Code = -20502;
        public const string IError_Result_UserStatusError_Msg = "�û����ڷ�����״̬";

        public const int IError_Result_UserPortError_Code = -20503;
        public const string IError_Result_UserPortError_Msg = "�û�δ��ָ���˿�����";

        public const int IError_Result_UserNotExist_Code = -20504;
        public const string IError_Result_UserNotExist_Msg = "�û��ʺŲ�����";

        public const int IError_Result_UserCanNotUseIPAuth_Code = -20505;
        public const string IError_Result_UserCanNotUseIPAuth_Msg = "�����û�����ʹ��IP��ַ��֤����";

        public const int IError_Result_UserIPInCorrect_Code = -20506;
        public const string IError_Result_UserIPInCorrect_Msg = "��ǰIP��ַ���û�����˺�Ӧ�÷��䵽�ĵ�ַ����";

        public const int IError_Result_UserNotOnline_Code = -20507;
        public const string IError_Result_UserNotOnline_Msg = "���˺��û���δ��������";

        public const int IError_Result_UserNotReAuth_Code = -20508;
        public const string IError_Result_UserNotReAuth_Msg = "ͨ��һ��ͨ��֤���û�δ�������ο�����֤,����������";

        public const int IError_Result_UserCreditLimitation_Code = -20509;
        public const string IError_Result_UserCreditLimitation_Msg = "�û����(����)����";

        public const int IError_Result_UserPwdTooSimple_Code = -20510;
        public const string IError_Result_UserPwdTooSimple_Msg = "�û�������ڼ򵥣����޸�������¼";

        public const int IError_Result_UserCancelAuthentication_Code = -20511;
        public const string IError_Result_UserCancelAuthentication_Msg = "�û�ȡ����֤";

        public const int IError_Result_UserCancelPayment_Code = -20512;
        public const string IError_Result_UserCancelPayment_Msg = "�û�ȡ��֧��";

        public const int IError_Result_UserNotSelectContinue_Code = -20513;
        public const string IError_Result_UserNotSelectContinue_Msg = "�û�δѡ��������!����ԭ��1���û����״ζ�����ҳ���ϣ�û�н�������ѡ��2���û�����������ƽ̨ȡ���˰��¡�";

        public const int IError_Result_UserNotSubTheService_Code = -20514;
        public const string IError_Result_UserNotSubTheService_Msg = "�û�δ�������÷���,���������߿۷�!����SP���͵�ServcieID��ServceiItemID��û���ҵ���Ӧ�Ķ�����¼��Ҳ������SP���͵�ServiceID��ServiceItemID������";

        public const int IError_Result_PlugInError_Code = -20515;
        public const string IError_Result_PlugInError_Msg = "�û�������֧������!����ԭ��:1.δ��װ��ȫ���;2.������֧����ȫ���ԡ�";

        #endregion

        #region ������ϵ����(-21000~-210499)

        public const int IError_Result_UserSubscriptionError_Code = -21000;
        public const string IError_Result_UserSubscriptionError_Msg = "������ϵ����";

        public const int IError_Result_UserAreaError_Code = -21001;
        public const string IError_Result_UserAreaError_Msg = "�õ����û���������";

        public const int IError_Result_UserTypeError_Code = -21002;
        public const string IError_Result_UserTypeError_Msg = "������û���������";

        public const int IError_Result_ArriveConsumeQuota_Code = -21003;
        public const string IError_Result_ArriveConsumeQuota_Msg = "���������޶�";

        public const int IError_Result_TxIDNotExist_Code = -21004;
        public const string IError_Result_TxIDNotExist_Msg = "��ָ�����׺ŵ����Ѽ�¼";

        public const int IError_Result_UserCancelTran_Code = -21005;
        public const string IError_Result_UserCancelTran_Msg = "������;ֹͣ���û�δȷ��";

        public const int IError_Result_NotSubscribe_Code = -21006;
        public const string IError_Result_NotSubscribe_Msg = "�û�δ����";

        public const int IError_Result_NotBind_Code = -21007;
        public const string IError_Result_NotBind_Msg = "�û�δ��SP�û��ʺ�����";

        public const int IError_Result_HadBind_Code = -21008;
        public const string IError_Result_HadBind_Msg = "�û��Ѿ���SP�û��ʺ�����";

        public const int IError_Result_CreateUserAccountFail_Code = -21009;
        public const string IError_Result_CreateUserAccountFail_Msg = "���ɻ����ǿ��ʻ�ʧ��";

        public const int IError_Result_HadSubscribed_Code = -21010;
        public const string IError_Result_HadSubscribed_Msg = "���˺��Ѿ������˸ò�Ʒ,������Ч����,��������Ϊ�ظ�����";

        public const int IError_Result_HadCancel_Code = -21011;
        public const string IError_Result_HadCancel_Msg = "���˺��Ѿ��˶��˸ò�Ʒ,�˶�����Ϊ�ظ�����";

        public const int IError_Result_AccountStatusError_Code = -21012;
        public const string IError_Result_AccountStatusError_Msg = "�ʺ�״̬������,�޷�����";

        public const int IError_Result_InternalError_Code = -21013;
        public const string IError_Result_InternalError_Msg = "ϵͳ�ڲ�ԭ����ʧ��";

        public const int IError_Result_BalanceNotEnough_Code = -21014;
        public const string IError_Result_BalanceNotEnough_Msg = "�û�����";

        public const int IError_Result_ContinueNotPermit_Code = -21015;
        public const string IError_Result_ContinueNotPermit_Msg = "���˶��ò�Ʒ������������";


        public const int IError_Result_ServiceIDNotInProduct_Code = -21016;
        public const string IError_Result_ServiceIDNotInProduct_Msg = "��Ʒ��������ServiceID";

        public const int IError_Result_ProductNotExist_Code = -21017;
        public const string IError_Result_ProductNotExist_Msg = "��Ʒ������";

        //�¼���
        public const int IError_Result_UPSPaymentFail_Code = -21018;
        public const string IError_Result_UPSPaymentFail_Msg = "UPS֧��ʧ��";

        #endregion

        #region ��������(-21500~-21999)

        public const int IError_Result_InvalidRequestData_Code = -21500;
        public const string IError_Result_InvalidRequestData_Msg = "��������";

        public const int IError_Result_InValidFeeType_Code = -21501;
        public const string IError_Result_InValidFeeType_Msg = "��Ч��FeeType111111";

        public const int IError_Result_InValidIsRenewed_Code = -21502;
        public const string IError_Result_InValidIsRenewed_Msg = "��Ч��Renewableȡֵ11111";

        public const int IError_Result_InValidStopType_Code = -21503;
        public const string IError_Result_InValidStopType_Msg = "��Ч��StopType11111";

        public const int IError_Result_InValidProvinceCode_Code = -21504;
        public const string IError_Result_InValidProvinceCode_Msg = "��Ч��ʡ����";

        public const int IError_Result_InValidServiceID_Code = -21505;
        public const string IError_Result_InValidServiceID_Msg = "��Ч��ServiceID1111111";

        public const int IError_Result_InValidServiceItemID_Code = -21506;
        public const string IError_Result_InValidServiceItemID_Msg = "��Ч��ServiceItemID111111111";

        public const int IError_Result_InValidPaymentStatus_Code = -21507;
        public const string IError_Result_InValidPaymentStatus_Msg = "��Ч��PaymentStatus1111111111";

        public const int IError_Result_InValidProvinceURL_Code = -21508;
        public const string IError_Result_InValidProvinceURL_Msg = "��Ч��ProvinceURL111111";

        public const int IError_Result_InValidLedgerDate_Code = -21509;
        public const string IError_Result_InValidLedgerDate_Msg = "��Ч��LedgerDate111111111";

        public const int IError_Result_InValidPassword_Code = -21510;
        public const string IError_Result_InValidPassword_Msg = "��Ч��Password";

        public const int IError_Result_InValidSPID_Code = -21511;
        public const string IError_Result_InValidSPID_Msg = "��Ч��SPID";

        public const int IError_Result_InValidUserID_Code = -21512;
        public const string IError_Result_InValidUserID_Msg = "��Ч��UserID11111111111";

        public const int IError_Result_InValidTransactionID_Code = -21513;
        public const string IError_Result_InValidTransactionID_Msg = "��Ч��TransactionID11111111111";

        public const int IError_Result_InValidTimeStamp_Code = -21514;
        public const string IError_Result_InValidTimeStamp_Msg = "��Ч��ʱ���";

        public const int IError_Result_InValidStartTime_Code = -21515;
        public const string IError_Result_InValidStartTime_Msg = "��Ч��StartTime11111";

        public const int IError_Result_InValidEndTime_Code = -21516;
        public const string IError_Result_InValidEndTime_Msg = "��Ч��EndTime11111";

        public const int IError_Result_InValidStartTimeAndEndTime_Code = -21517;
        public const string IError_Result_InValidStartTimeAndEndTime_Msg = "��Ч��StartTime��EndTime1111";

        public const int IError_Result_InValidFee_Code = -21518;
        public const string IError_Result_InValidFee_Msg = "��Ч��Fee1111";

        public const int IError_Result_InValidSequenceID_Code = -21519;
        public const string IError_Result_InValidSequenceID_Msg = "��Ч�����к�";

        public const int IError_Result_InValidIPAddress_Code = -21520;
        public const string IError_Result_InValidIPAddress_Msg = "��Ч��IPA��ַ";

        public const int IError_Result_InValidIsFinal_Code = -21521;
        public const string IError_Result_InValidIsFinal_Msg = "��Ч��IsFinal11111111111111111";

        public const int IError_Result_InValidIfExtend_Code = -21522;
        public const string IError_Result_InValidIfExtend_Msg = "��Ч����������ʶ";

        public const int IError_Result_InValidDescription_Code = -21523;
        public const string IError_Result_InValidDescription_Msg = "��Ч����Ϣ����";

        public const int IError_Result_InValidDetailParams_Code = -21524;
        public const string IError_Result_InValidDetailParams_Msg = "��Ч��DetailParams111111";

        public const int IError_Result_InValidOfflinePaymentType_Code = -21525;
        public const string IError_Result_InValidOfflinePaymentType_Msg = "��Ч��PaymentType(����֧��)1111";

        public const int IError_Result_InValidLedgerType_Code = -21526;
        public const string IError_Result_InValidLedgerType_Msg = "��Ч����������";

        public const int IError_Result_InValidRoamType_Code = -21527;
        public const string IError_Result_InValidRoamType_Msg = "��Ч��RoamType11111";

        public const int IError_Result_InValidUserAccount_Code = -21528;
        public const string IError_Result_InValidUserAccount_Msg = "��Ч�Ŀ���";

        public const int IError_Result_InValidItemName_Code = -21530;
        public const string IError_Result_InValidItemName_Msg = "��Ч��ItemName11111";

        public const int IError_Result_InValidClockError_Code = -21531;
        public const string IError_Result_InValidClockError_Msg = "��Ч��ClockError1111";

        public const int IError_Result_InValidReturnUrl_Code = -21532;
        public const string IError_Result_InValidReturnUrl_Msg = "��Ч��ReturnURL1111";

        public const int IError_Result_InValidBatchCount_Code = -21533;
        public const string IError_Result_InValidBatchCount_Msg = "��Ч�İ��δ���";

        public const int IError_Result_InValidLockType_Code = -21534;
        public const string IError_Result_InValidLockType_Msg = "��Ч��LockType1111";

        public const int IError_Result_InValidLockAmount_Code = -21535;
        public const string IError_Result_InValidLockAmount_Msg = "��Ч��LockAmount1111";

        public const int IError_Result_InValidTotalCount_Code = -21536;
        public const string IError_Result_InValidTotalCount_Msg = "��Ч��TotalCount";

        public const int IError_Result_InValidOrderID_Code = -21537;
        public const string IError_Result_InValidOrderID_Msg = "��Ч��OrderID";

        public const int IError_Result_InValidTotalFee_Code = -21538;
        public const string IError_Result_InValidTotalFee_Msg = "��Ч��TotalFee";


        public const int IError_Result_InValidProductID_Code = -21539;
        public const string IError_Result_InValidProductID_Msg = "��Ч��ProductID";

        public const int IError_Result_InValidProductType_Code = -21540;
        public const string IError_Result_InValidProductType_Msg = "��Ч��ProductType";

        public const int IError_Result_InValidProductName_Code = -21541;
        public const string IError_Result_InValidProductName_Msg = "��Ч��ProductName";

        public const int IError_Result_InValidProductDesc_Code = -21542;
        public const string IError_Result_InValidProductDesc_Msg = "��Ч��ProductDesc";

        public const int IError_Result_InValidPrice_Code = -21543;
        public const string IError_Result_InValidPrice_Msg = "��Ч��Price";

        public const int IError_Result_InValidPackagePrice_Code = -21544;
        public const string IError_Result_InValidPackagePrice_Msg = "��Ч��PackagePrice";

        public const int IError_Result_InValidServiceType_Code = -21545;
        public const string IError_Result_InValidServiceType_Msg = "��Ч��ServiceType";

        public const int IError_Result_InValidCityCode_Code = -21546;
        public const string IError_Result_InValidCityCode_Msg = "��Ч��Citycode";

        public const int IError_Result_InValidActionType_Code = -21547;
        public const string IError_Result_InValidActionType_Msg = "��Ч��ActionType";

        public const int IError_Result_InValidAuthenticator_Code = -21548;
        public const string IError_Result_InValidAuthenticator_Msg = "��Ч��Authenticator";

        public const int IError_Result_InValidFeeTypeID_Code = -21549;
        public const string IError_Result_InValidFeeTypeID_Msg = "��Ч��FeeTypeID";

        public const int IError_Result_InValidSenderID_Code = -21550;
        public const string IError_Result_InValidSenderID_Msg = "��Ч��SenderID";

        public const int IError_Result_InValidAccountType_Code = -21551;
        public const string IError_Result_InValidAccountType_Msg = "��Ч��AccountType";

        public const int IError_Result_InValidAccount_Code = -21552;
        public const string IError_Result_InValidAccount_Msg = "��Ч��Account";

        public const int IError_Result_InValidStatus_Code = -21553;
        public const string IError_Result_InValidStatus_Msg = "��Ч��Status";

        public const int IError_Result_InValidBillMonth_Code = -21554;
        public const string IError_Result_InValidBillMonth_Msg = "��Ч��BillMonth";

        public const int IError_Result_App_DecryptInputNull_Code = -21555;
        public const string IError_Result_App_DecryptInputNull_Msg = "��������Ϊ��";

        public const int IError_Result_App_DecryptCountError_Code = -21556;
        public const string IError_Result_App_DecryptCountError_Msg = "���ܽ����������";

        public const int IError_Result_App_DecryptFailed_Code = -21557;
        public const string IError_Result_App_DecryptFailed_Msg = "����ʧ��";

        //��û���뵽�淶�еĴ�����

        public const int IError_Result_InValidRecordNull_Code = -10001;
        public const string IError_Result_InValidRecordNull_Msg = "�����ݸ�ʽ����ȷ";

        public const int IError_Result_InValidActionDate_Code = -21560;
        public const string IError_Result_InValidActionDate_Msg = "��Ч��ActionDate";

        public const int IError_Result_InValidPackageID_Code = -21561;
        public const string IError_Result_InValidPackageID_Msg = "��Ч��PackageID";

        public const int IError_Result_InValidESID_Code = -21562;
        public const string IError_Result_InValidESID_Msg = "��Ч��ESID";

        public const int IError_Result_InValidGotoURL_Code = -21563;
        public const string IError_Result_InValidGotoURL_Msg = "��Ч��GotoURL";

        public const int IError_Result_InValidResult_Code = -21564;
        public const string IError_Result_InValidResult_Msg = "��Ч��Result";

        public const int IError_Result_App_EncryptFailed_Code = -21565;
        public const string IError_Result_App_EncryptFailed_Msg = "����ʧ��";

        #endregion

        #region ϵͳ�ڲ�����(-22500~-22999)

        public const int IError_Result_System_UnknowError_Code = -22500;
        public const string IError_Result_System_UnknowError_Msg = "ϵͳ�ڲ�����";

        public const int IError_Result_System_IOError_Code = -22501;
        public const string IError_Result_System_IOError_Msg = "���̶�д����";

        #endregion

        #region �Ʒ�����������(-22000~-22499)

        public const int IError_Result_Billing_UnknowError_Code = -22000;
        public const string IError_Result_Billing_UnknowError_Msg = "�Ʒ�����������";

        public const int IError_Result_AccountBalance_Code = -22001;
        public const string IError_Result_AccountBalance_Msg = "���˳ɹ������������Χ��";

        public const int IError_Result_AccountNotBalance_Code = -22002;
        public const string IError_Result_AccountNotBalance_Msg = "���ʲ�ƽ";

        public const int IError_Result_CannotProceedAccountBalance_Code = -22003;
        public const string IError_Result_CannotProceedAccountBalance_Msg = "�����ܽ��и����ڵĶ���";

        public const int IError_Result_AccountBalancePeriodExceed_Code = -22004;
        public const string IError_Result_AccountBalancePeriodExceed_Msg = "�������ѹ�,���ܽ��и����ڶ���";

        #endregion

        #region �ͷ�ϵͳ(-24000~-24499)

        public const int IError_Result_TransactionIDNotFound_Code = -24001;
        public const string IError_Result_TransactionIDNotFound_Msg = "δ�ҵ������嵥";

        public const int IError_Result_ReFunding_Code = -24002;
        public const string IError_Result_ReFunding_Msg = "�˷�Ҫ�����ڴ�����";

        #endregion

        #region ϵͳ�ڲ�����(-22500~-22999)

        public const int BT_IError_Result_Success_Code = 0;
        public const string BT_IError_Result_Success_Msg = "�ɹ�";

        public const int BT_IError_Result_UnknowError_Code = -19999;
        public const string BT_IError_Result_UnknowError_Msg = "δ֪����";

         
        public const int BT_IError_Result_System_UnknowError_Code = -22500;
        public const string BT_IError_Result_System_UnknowError_Msg = "ϵͳ�ڲ�����";

        public const int BT_IError_Result_System_IOError_Code = -22501;
        public const string BT_IError_Result_System_IOError_Msg = "���̶�д����";

        #endregion

        #region ҵ�����(30000)

        public const int BT_IError_Result_BusinessError_Code = -30000;
        public const string BT_IError_Result_BusinessError_Msg = "ҵ���߼�����";

        public const int BT_IError_Result_AccountExists_Code = -30001;
        public const string BT_IError_Result_AccountExists_Msg = "�ʺ���ע��";

        public const int BT_IError_Result_CertificateCodeExists_Code = -30002;
        public const string BT_IError_Result_CertificateCodeExists_Msg = "֤�������Ѵ���";

        public const int BT_IError_Result_PhoneBound_Code = -30003;
        public const string BT_IError_Result_PhoneBound_Msg = "�õ绰�Ѿ����󶨣��������ظ���";

        public const int BT_IError_Result_BoundPhoneExceed_Code = -30004;
        public const string BT_IError_Result_BoundPhoneExceed_Msg = "�ѳ����绰�󶨸���";


        public const int BT_IError_Result_PhoneNotBeBound_Code = -30005;
        public const string BT_IError_Result_PhoneNotBeBound_Msg = "�õ绰��δ���󶨣����ܽ��";

        public const int BT_IError_Result_TransactionIDExist_Code = -30006;
        public const string BT_IError_Result_TransactionIDExist_Msg = "���׺��Ѵ���";

        public const int BT_IError_Result_PasswordError_Code = -30007;
        public const string BT_IError_Result_PasswordError_Msg = "�������";

        public const int BT_IError_Result_NoScoreInfo_Code = -30009;
        public const string BT_IError_Result_NoScoreInfo_Msg = "�޻��ּ�¼";

        public const int BT_IError_Result_BizIPLimit_Code = -30011;
        public const string BT_IError_Result_BizIPLimit_Msg = " ҵ��ƽ̨IP����";

        public const int BT_IError_Result_BizInterfaceLimit_Code = -30012;
        public const string BT_IError_Result_BizInterfaceLimit_Msg = "δ��ӿڷ���Ȩ��";

        public const int BT_IError_Result_TicketError_Code = -30013;
        public const string BT_IError_Result_TicketError_Msg = "Ticket����";


        public const int BT_IError_Result_PhoenPostionError_Code = -30015;
        public const string BT_IError_Result_PhoenPostionError_Msg = "�ֻ���λ����";

        public const int BT_IError_Result_NPData_Code = -30016;
        public const string BT_IError_Result_NPDataError_Msg = "�����ƶ�����Я������";

        public const int BT_IError_Result_NPDataNull_Code = -30017;
        public const string BT_IError_Result_NPDataNullError_Msg = "����������ƶ�����Я������";

        #endregion

        #region ��������(-31000~-31033|-21504~-21554)

        public const int BT_IError_Result_InValidParameter_Code = -31000;
        public const string BT_IError_Result_InValidParameter_Msg = "��������";

        public const int BT_IError_Result_InValidPhoneNum_Code = -31001;
        public const string BT_IError_Result_InValidPhoneNum_Msg = "��Ч�ĵ绰����";

        public const int BT_IError_Result_InValidCertificateCode_Code = -31002;
        public const string BT_IError_Result_InValidCertificateCode_Msg = "��Ч��֤����";

        public const int BT_IError_Result_InValidCertificateType_Code = -31003;
        public const string BT_IError_Result_InValidCertificateType_Msg = "��Ч��֤������";

        public const int BT_IError_Result_InValidRegistrationStyle_Code = -31004;
        public const string BT_IError_Result_InValidRegistrationStyle_Msg = "��Ч��ע�᷽ʽ";

        public const int BT_IError_Result_InValidUserInfo_Code = -31005;
        public const string BT_IError_Result_InValidUserInfo_Msg = "��Ч���û���Ϣ����";

        public const int BT_IError_Result_InValidUserType_Code = -31006;
        public const string BT_IError_Result_InValidUserType_Msg = "��Ч���û�����";

        public const int BT_IError_Result_InValidCustID_Code = -31007;
        public const string BT_IError_Result_InValidCustID_Msg = "��Ч�Ŀͻ�ID";

        public const int BT_IError_Result_InValidAreaCode_Code = -31008;
        public const string BT_IError_Result_InValidAreaCode_Msg = "��Ч������";


        public const int BT_IError_Result_InValidRealName_Code = -31009;
        public const string BT_IError_Result_InValidRealName_Msg = "��Ч������";

        public const int BT_IError_Result_InValidBirthday_Code = -31010;
        public const string BT_IError_Result_InValidBirthday_Msg = "��Ч������";

        public const int BT_IError_Result_InValidSex_Code = -31011;
        public const string BT_IError_Result_InValidSex_Msg = "��Ч���Ա�";

        public const int BT_IError_Result_InValidCustLevel_Code = -31012;
        public const string BT_IError_Result_InValidCustLevel_Msg = "��Ч�Ŀͻ�����";

        public const int BT_IError_Result_InValidEduLevel_Code = -31013;
        public const string BT_IError_Result_InValidEduLevel_Msg = "��Ч�Ľ���ˮƽ";

        public const int BT_IError_Result_InValidFavorite_Code = -31014;
        public const string BT_IError_Result_InValidFavorite_Msg = "��Ч�İ���";

        public const int BT_IError_Result_InValidIncomeLevel_Code = -31015;
        public const string BT_IError_Result_InValidIncomeLevel_Msg = "��Ч��I����ˮƽ";

        public const int BT_IError_Result_InValidEmail_Code = -31016;
        public const string BT_IError_Result_InValidEmail_Msg = "��Ч��Email";

        public const int BT_IError_Result_InValidPaymentAccountType_Code = -31017;
        public const string BT_IError_Result_InValidPaymentAccountType_Msg = "��Ч��֧���ʺ�����";

        public const int BT_IError_Result_InValidPaymentAccount_Code = -31018;
        public const string BT_IError_Result_InValidPaymentAccount_Msg = "��Ч��֧���ʺ�";

        public const int BT_IError_Result_InValidPaymentAccountPassword_Code = -31019;
        public const string BT_IError_Result_InValidPaymentAccountPassword_Msg = "��Ч��֧���ʺ�����";

        public const int BT_IError_Result_InValidCustContactTel_Code = -31020;
        public const string BT_IError_Result_InValidCustContactTel_Msg = "��Ч����ϵ�绰";

        public const int BT_IError_Result_InValidAssessmentInfos_Code = -31021;
        public const string BT_IError_Result_InValidAssessmentInfos_Msg = "��Ч��������Ϣ";

        public const int BT_IError_Result_InValidAddressRecords_Code = -31022;
        public const string BT_IError_Result_InValidAddressRecords_Msg = "��Ч�ĵ�ַ�б�";

        public const int BT_IError_Result_InValidBoundPhoneRecords_Code = -31023;
        public const string BT_IError_Result_InValidBoundPhoneRecords_Msg = "��Ч�İ󶨵绰�б�";

        public const int BT_IError_Result_InValidExtendField_Code = -31024;
        public const string BT_IError_Result_InValidExtendField_Msg = "��Ч��ExtendField";

        public const int BT_IError_Result_InValidCredit_Code = -31025;
        public const string BT_IError_Result_InValidCredit_Msg = "��Ч�������ֶ�";

        public const int BT_IError_Result_InValidCreditLevel_Code = -31026;
        public const string BT_IError_Result_InValidCreditLevel_Msg = "��Ч�����õȼ�";

        public const int BT_IError_Result_InValidScore_Code = -31027;
        public const string BT_IError_Result_InValidScore_Msg = "��Ч�Ļ���";

        public const int BT_IError_Result_InValidEffectiveTime_Code = -31028;
        public const string BT_IError_Result_InValidEffectiveTime_Msg = "��Ч��EffectiveTime";

        public const int BT_IError_Result_InValidExpireTime_Code = -31029;
        public const string BT_IError_Result_InValidExpireTime_Msg = "��Ч�Ĺ���ʱ��";

        public const int BT_IError_Result_InValidZipcode_Code = -31030;
        public const string BT_IError_Result_InValidZipcode_Msg = "��Ч���ʱ�";

        public const int BT_IError_Result_InValidSubScribeStyle_Code = -31031;
        public const string BT_IError_Result_InValidSubScribeStyle_Msg = "��Ч�Ķ�����ʽ";

        public const int BT_IError_Result_InValidServiceName_Code = -31032;
        public const string BT_IError_Result_InValidServiceName_Msg = "��Ч��ServiceName";

        public const int BT_IError_Result_InValidSubscribeDate_Code = -31033;
        public const string BT_IError_Result_InValidSubscribeDate_Msg = "��Ч��SubscribeDate";

        public const int BT_IError_Result_InValidProvinceID_Code = -21504;
        public const string BT_IError_Result_InValidProvinceID_Msg = "��Ч��ʡ����";

        public const int BT_IError_Result_InValidServiceID_Code = -21505;
        public const string BT_IError_Result_InValidServiceID_Msg = "��Ч�ķ���ID";

        public const int BT_IError_Result_InValidPassword_Code = -21510;
        public const string BT_IError_Result_InValidPassword_Msg = "��Ч������";

        public const int BT_IError_Result_InValidSPID_Code = -21511;
        public const string BT_IError_Result_InValidSPID_Msg = "��Ч��ϵͳ��ʶ";

        public const int BT_IError_Result_InValidTransactionID_Code = -21513;
        public const string BT_IError_Result_InValidTransactionID_Msg = "��Ч��TransactionID";

        public const int BT_IError_Result_InValidTimeStamp_Code = -21514;
        public const string BT_IError_Result_InValidTimeStamp_Msg = "��Ч��ʱ���";

        public const int BT_IError_Result_InValidStartTime_Code = -21515;
        public const string BT_IError_Result_InValidStartTime_Msg = "��Ч�Ŀ�ʼʱ��";

        public const int BT_IError_Result_InValidEndTime_Code = -21516;
        public const string BT_IError_Result_InValidEndTime_Msg = "��Ч�Ľ���ʱ��";

        public const int BT_IError_Result_InValidUserAccount_Code = -21528;
        public const string BT_IError_Result_InValidUserAccount_Msg = "��Ч�Ŀ���";

        public const int BT_IError_Result_InValidStatus_Code = -21553;
        public const string BT_IError_Result_InValidStatus_Msg = "��Ч��״̬";

        public const int BT_IError_Result_InValidSYSID_Code = -21554;
        public const string BT_IError_Result_InValidSYSID_Msg = "��Ч��ϵͳ��ʶ";

        //�¼Ӵ�����
        //public const int BT_IError_Result_InValidFee_Code = -30010;
        //public const string BT_IError_Result_InValidFee_Msg = "��Ч��Fee";

        //public const int BT_IError_Result_ContinueNotPermit_Code = -21015;
        //public const string BT_IError_Result_ContinueNotPermit_Msg = "���˶��ò�Ʒ������������";

        //public const int BT_IError_Result_UserAccountError_Code = -30008;
        //public const string BT_IError_Result_UserAccountError_Msg = "�û��ʺŴ���";

        //public const int BT_IError_Result_NoRelationgProvince_Code = -30011;
        //public const string BT_IError_Result_NoRelationgProvince_Msg = "�޴����Ŷ�Ӧʡ";

        //public const int BT_IError_Result_AuthenStyleAgainBound_Code = -30012;
        //public const string BT_IError_Result_AuthenStyleAgainBound_Msg = "����֤��ʽ�ѱ��󶨣���Ҫ�����Ƚ��";

        //public const int BT_IError_Result_AuthenStyleAgainUnBound_Code = -30013;
        //public const string BT_IError_Result_AuthenStyleAgainUnBound_Msg = "����֤��ʽ�ѱ���󣬲����ٴν��";

        //public const int BT_IError_Result_AuthenNamesExists_Code = -30014;
        //public const string BT_IError_Result_AuthenNamesExists_Msg = "����֤���ѱ�ʹ��";

        #endregion



        #region �µĴ�����(-40000~-50000)��2012/09/03��

        #region ϵͳ����(-40000~-40099)

        public const Int32 CIP_IError_Result_Success_Code = 0;
        public const string CIP_IError_Result_Success_Msg = "�ɹ�";

        public const Int32 CIP_IError_Result_UnknowError_Code = -40000;
        public const string CIP_IError_Result_UnknowError_Msg = "δ֪����";

        public const Int32 CIP_IError_Result_SPIDInValid_Code = -40001;
        public const string CIP_IError_Result_SPIDInValid_Msg = "ϵͳ��ʶ��Ч";

        public const Int32 CIP_IError_Result_SPStatusError_Code = -40002;
        public const string CIP_IError_Result_SPStatusError_Msg = "ϵͳ״̬�쳣";

        public const Int32 CIP_IError_Result_SPPasswordError_Code = -40003;
        public const string CIP_IError_Result_SPPasswordError_Msg = "ϵͳ�������";

        public const Int32 CIP_IError_Result_SPIPInValid_Code = -40004;
        public const string CIP_IError_Result_SPIPInValid_Msg = "IP��ʽ��Ч";

        public const Int32 CIP_IError_Result_SPIPLimited_Code = -40005;
        public const string CIP_IError_Result_SPIPLimite_Msg = "IP��������";

        public const Int32 CIP_IError_Result_SPInterfaceLimited_Code = -40006;
        public const string CIP_IError_Result_SPInterfaceLimited_Msg = "���ʽӿ�Ȩ������";
            
        #endregion

        #region ע����֤(-40100~-40199)

        public const Int32 CIP_IError_Result_RegisterException_Code = -40100;
        public const string CIP_IError_Result_RegisterException_Msg = "ע��ʧ��";

        public const Int32 CIP_IError_Result_RegisterVoiceException_Code = -40101;
        public const string CIP_IError_Result_RegisterVoiceException_Msg = "����ע��ʧ��";

        public const Int32 CIP_IError_Result_RegisterWebException_Code = -40102;
        public const string CIP_IError_Result_RegisterWebException_Msg = "Webע��ʧ��";

        public const Int32 CIP_IError_Result_RegisterUDBException_Code = -40103;
        public const string CIP_IError_Result_RegisterUDBException_Msg = "UDBע��ʧ��";

        public const Int32 CIP_IError_Result_RegisterCrmException_Code = -40104;
        public const string CIP_IError_Result_RegisterCrmException_Msg = "Crmע��ʧ��";

        public const Int32 CIP_IError_Result_AuthenException_Code = -40105;
        public const string CIP_IError_Result_AuthenException_Msg = "��֤ʧ��";

        public const Int32 CIP_IError_Result_TicketInValid_Code = -40106;
        public const string CIP_IError_Result_TicketInValid_Msg = "Ʊ�ݹ��ڻ���Ч";

        public const Int32 CIP_IError_Result_TransactionIDInValid_Code = -40107;
        public const string CIP_IError_Result_TransactionIDInValid_Msg = "��ˮ����Ч";

        #endregion

        #region �û�(-40200~-40299)

        public const Int32 CIP_IError_Result_User_UserIDInValid_Code = -40200;
        public const string CIP_IError_Result_User_UserIDInValid_Msg = "�û���ʶ��Ч";

        public const Int32 CIP_IError_Result_User_UserNotExist_Code = -40201;
        public const string CIP_IError_Result_User_UserNotExist_Msg = "�û�������";

        public const Int32 CIP_IError_Result_User_StatusException_Code = -40202;
        public const string CIP_IError_Result_User_StatusException_Msg = "�û�״̬�쳣";

        public const Int32 CIP_IError_Result_User_UserNameRepeat_Code = -40203;
        public const string CIP_IError_Result_User_UserNameRepeat_Msg = "�û����ظ�";

        public const Int32 CIP_IError_Result_User_UserTypeInValid_Code = -40204;
        public const string CIP_IError_Result_User_UserTypeInValid_Msg = "�û�������Ч";

        public const Int32 CIP_IError_Result_User_SetInfoException_Code = -40205;
        public const string CIP_IError_Result_User_SetInfoException_Msg = "�û���Ϣ�����쳣";

        public const Int32 CIP_IError_Result_User_SetAuthenStyleException_Code = -40206;
        public const string CIP_IError_Result_User_SetAuthenStyleException_Msg = "��֤��ʽ�����쳣";

        public const Int32 CIP_IError_Result_User_PasswordError_Code = -40207;
        public const string CIP_IError_Result_User_PasswordError_Msg = "�������";

        public const Int32 CIP_IError_Result_User_VoicePasswordInValid_Code = -40208;
        public const string CIP_IError_Result_User_VoicePasswordInValid_Msg = "����������Ч";

        public const Int32 CIP_IError_Result_User_SetPasswordException_Code = -40208;
        public const string CIP_IError_Result_User_SetPasswordException_Msg = "��������ʧ��";

        public const Int32 CIP_IError_Result_User_UserHasExist_Code = -40209;
        public const String CIP_IError_Result_User_UserHasExist_Msg = "�û��Ѵ���";

        #endregion

        #region �绰(-40300~-40399)

        public const Int32 CIP_IError_Result_Phone_NumberException_Code = -40300;
        public const string CIP_IError_Result_Phone_NumberException_Msg = "�绰�����쳣";

        public const Int32 CIP_IError_Result_Phone_NumberInValid_Code = -40301;
        public const string CIP_IError_Result_Phone_NumberInValid_Msg = "�绰��ʽ��Ч";

        public const Int32 CIP_IError_Result_Phone_BindPhoneOverLimit_Code = -40302;
        public const string CIP_IError_Result_Phone_BindPhoneOverLimit_Msg = "�󶨵绰������������";

        public const Int32 CIP_IError_Result_Phone_BindPhoneRepeat_Code = -40303;
        public const string CIP_IError_Result_Phone_BindPhoneRepeat_Msg = "�󶨵绰�ظ�";

        public const Int32 CIP_IError_Result_Phone_AuthenPhoneRepeat_Code = -40304;
        public const string CIP_IError_Result_Phone_AuthenPhoneRepeat_Msg = "��֤�绰�ظ�";

        public const Int32 CIP_IError_Result_Phone_AuthenPhoneInValid_Code = -40305;
        public const string CIP_IError_Result_Phone_AuthenPhoneInValid_Msg = "��֤�绰��Ч";

        public const Int32 CIP_IError_Result_Phone_PhoneBindException_Code = -40306;
        public const string CIP_IError_Result_Phone_PhoneBindException_Msg = "�绰���쳣";

        public const Int32 CIP_IError_Result_Phone_PhoneUnBindException_Code = -40307;
        public const string CIP_IError_Result_Phone_PhoneUnBindException_Msg = "�绰����쳣";

        #endregion

        #region ����(-40400~-40499)

        public const Int32 CIP_IError_Result_Email_Exception_Code = -40400;
        public const string CIP_IError_Result_Email_Exception_Msg = "�����쳣";

        public const Int32 CIP_IError_Result_Email_FormatInValid_Code = -40401;
        public const string CIP_IError_Result_Email_FormatInValid_Msg = "�����ʽ��Ч";

        public const Int32 CIP_IError_Result_Email_AuthenEmailInValid_Code = -40402;
        public const string CIP_IError_Result_Email_AuthenEmailInValid_Msg = "��֤������Ч";

        public const Int32 CIP_IError_Result_Email_AuthenEmailRepeat_Code = -40403;
        public const string CIP_IError_Result_Email_AuthenEmailRepeat_Msg = "��֤�����ظ�";

        public const Int32 CIP_IError_Result_Email_SetEmailException_Code = -40404;
        public const string CIP_IError_Result_Email_SetEmailException_Msg = "���������쳣";

        #endregion

        #region ����(-41000~-41999)

        public const Int32 CIP_IError_Result_ProvinceCodeInValid_Code = -41000;
        public const string CIP_IError_Result_ProvinceCodeInValid_Msg = "ʡ��������Ч";

        public const Int32 CIP_IError_Result_AreaCodeInValid_Code = -41001;
        public const string CIP_IError_Result_AreaCodeInValid_Msg = "������������Ч";

        #endregion

        #region �Ű��˻�

        public const Int32 CIP_IError_Result_BesttoneAcountException_Code = -42000;
        public const string CIP_IError_Result_BesttoneAcountException_Msg = "δ֪�쳣";

        public const Int32 CIP_IError_Result_BesttoneAccount_UnRegister_Code = -42001;
        public const string CIP_IError_Result_BesttoneAccount_UnRegister_Msg = "δ��ͨ�������ͨ�˻�";

        public const Int32 CIP_IError_Result_BesttoneAccount_RegisterError_Code = -42002;
        public const string CIP_IError_Result_BesttoneAccount_RegisterError_Msg = "�������ͨ�˻�ע��ʧ��";

        public const Int32 CIP_IError_Result_BesttoneAccount_CardNoInValid_Code = -42003;
        public const string CIP_IError_Result_BesttoneAccount_CardNoInValid_Msg = "��Ч����";

        public const Int32 CIP_IError_Result_BesttoneAccount_CardTypeInValid_Code = -42004;
        public const string CIP_IError_Result_BesttoneAccount_CardTypeInValid_Msg = "��Ч������";

        #endregion

        #endregion

    }
}

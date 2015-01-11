//==============================================================================================================
//
// Class Name: ErrorDefinition
// Description: 定义各类错误项
// Author: 苑峰
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

        #region 公共类

        public const int IError_Result_Success_Code = 0;
        public const string IError_Result_Success_Msg = "成功";

        public const int IError_Result_UnknowError_Code = -19999;
        public const string IError_Result_UnknowError_Msg = "未知错误";

        public const int IError_Result_NotReallySuccess_Code = 1;
        public const string IError_Result_NotReallySuccess_Msg = "成功锁定,锁定的数额小于请求数";

        public const int IError_Result_UnknowQueryError_Code = -19998;
        public const string IError_Result_UnknowQueryError_Msg = "由于未知原因导致的查询失败";

        public const int IError_Result_Null_Code = -1;
        public const string IError_Result_Null_Msg = "无符合条件的用户信息";

        #endregion

        #region SP错误(-20000至-20499)

        public const int IError_Result_SPError_Code = -20000;
        public const string IError_Result_SPError_Msg = "SP错误";

        public const int IError_Result_SPNotExist_Code = -20001;
        public const string IError_Result_SPNotExist_Msg = "SPID不存在";

        public const int IError_Result_SPPwdError_Code = -20002;
        public const string IError_Result_SPPwdError_Msg = "SP密码错误";

        public const int IError_Result_SPAuthorizationFail_Code = -20003;
        public const string IError_Result_SPAuthorizationFail_Msg = "SP未获得调用本接口的授权";

        public const int IError_Result_SPServiceNotExist_Code = -20004;
        public const string IError_Result_SPServiceNotExist_Msg = "Service不存在";

        public const int IError_Result_SPServiceStatusError_Code = -20005;
        public const string IError_Result_SPServiceStatusError_Msg = "Service状态异常";

        public const int IError_Result_SPServiceItemStatusError_Code = -20006;
        public const string IError_Result_SPServiceItemStatusError_Msg = "ServiceItem状态异常";

        public const int IError_Result_SPServiceExpireForTwoMonth_Code = -20007;
        public const string IError_Result_SPServiceExpireForTwoMonth_Msg = "距上次服务到期时间已经超过2个月,不允许再发送离线扣费";

        public const int IError_Result_SPNoQualification_Code = -20008;
        public const string IError_Result_SPNoQualification_Msg = "SP不具备离线支付资格，互联星空平台没有为该SP开通离线支付资格";

        public const int IError_Result_PaymentTimesError_Code = -20009;
        public const string IError_Result_PaymentTimesError_Msg = "一个帐期内只允许发送一次离线支付请求";

        #endregion

        #region 用户错误(-20500至20999)

        public const int IError_Result_UserError_Code = -20500;
        public const string IError_Result_UserError_Msg = "用户错误";

        public const int IError_Result_UserAuthorizationFail_Code = -20501;
        public const string IError_Result_UserAuthorizationFail_Msg = "用户认证失败";

        public const int IError_Result_UserStatusError_Code = -20502;
        public const string IError_Result_UserStatusError_Msg = "用户处于非正常状态";

        public const int IError_Result_UserPortError_Code = -20503;
        public const string IError_Result_UserPortError_Msg = "用户未在指定端口上网";

        public const int IError_Result_UserNotExist_Code = -20504;
        public const string IError_Result_UserNotExist_Msg = "用户帐号不存在";

        public const int IError_Result_UserCanNotUseIPAuth_Code = -20505;
        public const string IError_Result_UserCanNotUseIPAuth_Msg = "此类用户不能使用IP地址认证功能";

        public const int IError_Result_UserIPInCorrect_Code = -20506;
        public const string IError_Result_UserIPInCorrect_Msg = "当前IP地址与用户宽带账号应该分配到的地址不符";

        public const int IError_Result_UserNotOnline_Code = -20507;
        public const string IError_Result_UserNotOnline_Msg = "此账号用户并未拨号在线";

        public const int IError_Result_UserNotReAuth_Code = -20508;
        public const string IError_Result_UserNotReAuth_Msg = "通过一点通认证的用户未经过二次口令认证,不允许消费";

        public const int IError_Result_UserCreditLimitation_Code = -20509;
        public const string IError_Result_UserCreditLimitation_Msg = "用户余额(信用)不足";

        public const int IError_Result_UserPwdTooSimple_Code = -20510;
        public const string IError_Result_UserPwdTooSimple_Msg = "用户密码过于简单，请修改密码后登录";

        public const int IError_Result_UserCancelAuthentication_Code = -20511;
        public const string IError_Result_UserCancelAuthentication_Msg = "用户取消认证";

        public const int IError_Result_UserCancelPayment_Code = -20512;
        public const string IError_Result_UserCancelPayment_Msg = "用户取消支付";

        public const int IError_Result_UserNotSelectContinue_Code = -20513;
        public const string IError_Result_UserNotSelectContinue_Msg = "用户未选择续订购!可能原因：1、用户在首次订购的页面上，没有进行续费选择。2、用户主动从中心平台取消了包月。";

        public const int IError_Result_UserNotSubTheService_Code = -20514;
        public const string IError_Result_UserNotSubTheService_Msg = "用户未订购过该服务,不允许离线扣费!根据SP发送的ServcieID、ServceiItemID，没有找到对应的订购记录，也可能是SP发送的ServiceID、ServiceItemID有问题";

        public const int IError_Result_PlugInError_Code = -20515;
        public const string IError_Result_PlugInError_Msg = "用户不满足支付条件!可能原因:1.未安装安全插件;2.不满足支付安全策略。";

        #endregion

        #region 订购关系错误(-21000~-210499)

        public const int IError_Result_UserSubscriptionError_Code = -21000;
        public const string IError_Result_UserSubscriptionError_Msg = "订购关系错误";

        public const int IError_Result_UserAreaError_Code = -21001;
        public const string IError_Result_UserAreaError_Msg = "该地区用户不能消费";

        public const int IError_Result_UserTypeError_Code = -21002;
        public const string IError_Result_UserTypeError_Msg = "该类别用户不能消费";

        public const int IError_Result_ArriveConsumeQuota_Code = -21003;
        public const string IError_Result_ArriveConsumeQuota_Msg = "超出消费限额";

        public const int IError_Result_TxIDNotExist_Code = -21004;
        public const string IError_Result_TxIDNotExist_Msg = "无指定交易号的消费记录";

        public const int IError_Result_UserCancelTran_Code = -21005;
        public const string IError_Result_UserCancelTran_Msg = "交易中途停止，用户未确认";

        public const int IError_Result_NotSubscribe_Code = -21006;
        public const string IError_Result_NotSubscribe_Msg = "用户未订购";

        public const int IError_Result_NotBind_Code = -21007;
        public const string IError_Result_NotBind_Msg = "用户未与SP用户帐号捆绑";

        public const int IError_Result_HadBind_Code = -21008;
        public const string IError_Result_HadBind_Msg = "用户已经与SP用户帐号捆绑";

        public const int IError_Result_CreateUserAccountFail_Code = -21009;
        public const string IError_Result_CreateUserAccountFail_Msg = "生成互联星空帐户失败";

        public const int IError_Result_HadSubscribed_Code = -21010;
        public const string IError_Result_HadSubscribed_Msg = "该账号已经订购了该产品,且在有效期内,订购请求为重复订购";

        public const int IError_Result_HadCancel_Code = -21011;
        public const string IError_Result_HadCancel_Msg = "该账号已经退订了该产品,退订请求为重复请求";

        public const int IError_Result_AccountStatusError_Code = -21012;
        public const string IError_Result_AccountStatusError_Msg = "帐号状态不正常,无法订购";

        public const int IError_Result_InternalError_Code = -21013;
        public const string IError_Result_InternalError_Msg = "系统内部原因处理失败";

        public const int IError_Result_BalanceNotEnough_Code = -21014;
        public const string IError_Result_BalanceNotEnough_Msg = "用户余额不足";

        public const int IError_Result_ContinueNotPermit_Code = -21015;
        public const string IError_Result_ContinueNotPermit_Msg = "已退订该产品不允许续定购";


        public const int IError_Result_ServiceIDNotInProduct_Code = -21016;
        public const string IError_Result_ServiceIDNotInProduct_Msg = "产品不包含该ServiceID";

        public const int IError_Result_ProductNotExist_Code = -21017;
        public const string IError_Result_ProductNotExist_Msg = "产品不存在";

        //新加入
        public const int IError_Result_UPSPaymentFail_Code = -21018;
        public const string IError_Result_UPSPaymentFail_Msg = "UPS支付失败";

        #endregion

        #region 参数错误(-21500~-21999)

        public const int IError_Result_InvalidRequestData_Code = -21500;
        public const string IError_Result_InvalidRequestData_Msg = "参数错误";

        public const int IError_Result_InValidFeeType_Code = -21501;
        public const string IError_Result_InValidFeeType_Msg = "无效的FeeType111111";

        public const int IError_Result_InValidIsRenewed_Code = -21502;
        public const string IError_Result_InValidIsRenewed_Msg = "无效的Renewable取值11111";

        public const int IError_Result_InValidStopType_Code = -21503;
        public const string IError_Result_InValidStopType_Msg = "无效的StopType11111";

        public const int IError_Result_InValidProvinceCode_Code = -21504;
        public const string IError_Result_InValidProvinceCode_Msg = "无效的省份码";

        public const int IError_Result_InValidServiceID_Code = -21505;
        public const string IError_Result_InValidServiceID_Msg = "无效的ServiceID1111111";

        public const int IError_Result_InValidServiceItemID_Code = -21506;
        public const string IError_Result_InValidServiceItemID_Msg = "无效的ServiceItemID111111111";

        public const int IError_Result_InValidPaymentStatus_Code = -21507;
        public const string IError_Result_InValidPaymentStatus_Msg = "无效的PaymentStatus1111111111";

        public const int IError_Result_InValidProvinceURL_Code = -21508;
        public const string IError_Result_InValidProvinceURL_Msg = "无效的ProvinceURL111111";

        public const int IError_Result_InValidLedgerDate_Code = -21509;
        public const string IError_Result_InValidLedgerDate_Msg = "无效的LedgerDate111111111";

        public const int IError_Result_InValidPassword_Code = -21510;
        public const string IError_Result_InValidPassword_Msg = "无效的Password";

        public const int IError_Result_InValidSPID_Code = -21511;
        public const string IError_Result_InValidSPID_Msg = "无效的SPID";

        public const int IError_Result_InValidUserID_Code = -21512;
        public const string IError_Result_InValidUserID_Msg = "无效的UserID11111111111";

        public const int IError_Result_InValidTransactionID_Code = -21513;
        public const string IError_Result_InValidTransactionID_Msg = "无效的TransactionID11111111111";

        public const int IError_Result_InValidTimeStamp_Code = -21514;
        public const string IError_Result_InValidTimeStamp_Msg = "无效的时间戳";

        public const int IError_Result_InValidStartTime_Code = -21515;
        public const string IError_Result_InValidStartTime_Msg = "无效的StartTime11111";

        public const int IError_Result_InValidEndTime_Code = -21516;
        public const string IError_Result_InValidEndTime_Msg = "无效的EndTime11111";

        public const int IError_Result_InValidStartTimeAndEndTime_Code = -21517;
        public const string IError_Result_InValidStartTimeAndEndTime_Msg = "无效的StartTime和EndTime1111";

        public const int IError_Result_InValidFee_Code = -21518;
        public const string IError_Result_InValidFee_Msg = "无效的Fee1111";

        public const int IError_Result_InValidSequenceID_Code = -21519;
        public const string IError_Result_InValidSequenceID_Msg = "无效的序列号";

        public const int IError_Result_InValidIPAddress_Code = -21520;
        public const string IError_Result_InValidIPAddress_Msg = "无效的IPA地址";

        public const int IError_Result_InValidIsFinal_Code = -21521;
        public const string IError_Result_InValidIsFinal_Msg = "无效的IsFinal11111111111111111";

        public const int IError_Result_InValidIfExtend_Code = -21522;
        public const string IError_Result_InValidIfExtend_Msg = "无效的续订购标识";

        public const int IError_Result_InValidDescription_Code = -21523;
        public const string IError_Result_InValidDescription_Msg = "无效的信息描述";

        public const int IError_Result_InValidDetailParams_Code = -21524;
        public const string IError_Result_InValidDetailParams_Msg = "无效的DetailParams111111";

        public const int IError_Result_InValidOfflinePaymentType_Code = -21525;
        public const string IError_Result_InValidOfflinePaymentType_Msg = "无效的PaymentType(离线支付)1111";

        public const int IError_Result_InValidLedgerType_Code = -21526;
        public const string IError_Result_InValidLedgerType_Msg = "无效的帐期类型";

        public const int IError_Result_InValidRoamType_Code = -21527;
        public const string IError_Result_InValidRoamType_Msg = "无效的RoamType11111";

        public const int IError_Result_InValidUserAccount_Code = -21528;
        public const string IError_Result_InValidUserAccount_Msg = "无效的卡号";

        public const int IError_Result_InValidItemName_Code = -21530;
        public const string IError_Result_InValidItemName_Msg = "无效的ItemName11111";

        public const int IError_Result_InValidClockError_Code = -21531;
        public const string IError_Result_InValidClockError_Msg = "无效的ClockError1111";

        public const int IError_Result_InValidReturnUrl_Code = -21532;
        public const string IError_Result_InValidReturnUrl_Msg = "无效的ReturnURL1111";

        public const int IError_Result_InValidBatchCount_Code = -21533;
        public const string IError_Result_InValidBatchCount_Msg = "无效的包次次数";

        public const int IError_Result_InValidLockType_Code = -21534;
        public const string IError_Result_InValidLockType_Msg = "无效的LockType1111";

        public const int IError_Result_InValidLockAmount_Code = -21535;
        public const string IError_Result_InValidLockAmount_Msg = "无效的LockAmount1111";

        public const int IError_Result_InValidTotalCount_Code = -21536;
        public const string IError_Result_InValidTotalCount_Msg = "无效的TotalCount";

        public const int IError_Result_InValidOrderID_Code = -21537;
        public const string IError_Result_InValidOrderID_Msg = "无效的OrderID";

        public const int IError_Result_InValidTotalFee_Code = -21538;
        public const string IError_Result_InValidTotalFee_Msg = "无效的TotalFee";


        public const int IError_Result_InValidProductID_Code = -21539;
        public const string IError_Result_InValidProductID_Msg = "无效的ProductID";

        public const int IError_Result_InValidProductType_Code = -21540;
        public const string IError_Result_InValidProductType_Msg = "无效的ProductType";

        public const int IError_Result_InValidProductName_Code = -21541;
        public const string IError_Result_InValidProductName_Msg = "无效的ProductName";

        public const int IError_Result_InValidProductDesc_Code = -21542;
        public const string IError_Result_InValidProductDesc_Msg = "无效的ProductDesc";

        public const int IError_Result_InValidPrice_Code = -21543;
        public const string IError_Result_InValidPrice_Msg = "无效的Price";

        public const int IError_Result_InValidPackagePrice_Code = -21544;
        public const string IError_Result_InValidPackagePrice_Msg = "无效的PackagePrice";

        public const int IError_Result_InValidServiceType_Code = -21545;
        public const string IError_Result_InValidServiceType_Msg = "无效的ServiceType";

        public const int IError_Result_InValidCityCode_Code = -21546;
        public const string IError_Result_InValidCityCode_Msg = "无效的Citycode";

        public const int IError_Result_InValidActionType_Code = -21547;
        public const string IError_Result_InValidActionType_Msg = "无效的ActionType";

        public const int IError_Result_InValidAuthenticator_Code = -21548;
        public const string IError_Result_InValidAuthenticator_Msg = "无效的Authenticator";

        public const int IError_Result_InValidFeeTypeID_Code = -21549;
        public const string IError_Result_InValidFeeTypeID_Msg = "无效的FeeTypeID";

        public const int IError_Result_InValidSenderID_Code = -21550;
        public const string IError_Result_InValidSenderID_Msg = "无效的SenderID";

        public const int IError_Result_InValidAccountType_Code = -21551;
        public const string IError_Result_InValidAccountType_Msg = "无效的AccountType";

        public const int IError_Result_InValidAccount_Code = -21552;
        public const string IError_Result_InValidAccount_Msg = "无效的Account";

        public const int IError_Result_InValidStatus_Code = -21553;
        public const string IError_Result_InValidStatus_Msg = "无效的Status";

        public const int IError_Result_InValidBillMonth_Code = -21554;
        public const string IError_Result_InValidBillMonth_Msg = "无效的BillMonth";

        public const int IError_Result_App_DecryptInputNull_Code = -21555;
        public const string IError_Result_App_DecryptInputNull_Msg = "解密数据为空";

        public const int IError_Result_App_DecryptCountError_Code = -21556;
        public const string IError_Result_App_DecryptCountError_Msg = "解密结果项数不对";

        public const int IError_Result_App_DecryptFailed_Code = -21557;
        public const string IError_Result_App_DecryptFailed_Msg = "解密失败";

        //还没加入到规范中的错误码

        public const int IError_Result_InValidRecordNull_Code = -10001;
        public const string IError_Result_InValidRecordNull_Msg = "包数据格式不正确";

        public const int IError_Result_InValidActionDate_Code = -21560;
        public const string IError_Result_InValidActionDate_Msg = "无效的ActionDate";

        public const int IError_Result_InValidPackageID_Code = -21561;
        public const string IError_Result_InValidPackageID_Msg = "无效的PackageID";

        public const int IError_Result_InValidESID_Code = -21562;
        public const string IError_Result_InValidESID_Msg = "无效的ESID";

        public const int IError_Result_InValidGotoURL_Code = -21563;
        public const string IError_Result_InValidGotoURL_Msg = "无效的GotoURL";

        public const int IError_Result_InValidResult_Code = -21564;
        public const string IError_Result_InValidResult_Msg = "无效的Result";

        public const int IError_Result_App_EncryptFailed_Code = -21565;
        public const string IError_Result_App_EncryptFailed_Msg = "加密失败";

        #endregion

        #region 系统内部错误(-22500~-22999)

        public const int IError_Result_System_UnknowError_Code = -22500;
        public const string IError_Result_System_UnknowError_Msg = "系统内部错误";

        public const int IError_Result_System_IOError_Code = -22501;
        public const string IError_Result_System_IOError_Msg = "磁盘读写错误";

        #endregion

        #region 计费帐务结算错误(-22000~-22499)

        public const int IError_Result_Billing_UnknowError_Code = -22000;
        public const string IError_Result_Billing_UnknowError_Msg = "计费帐务结算错误";

        public const int IError_Result_AccountBalance_Code = -22001;
        public const string IError_Result_AccountBalance_Msg = "对账成功，误差在允许范围内";

        public const int IError_Result_AccountNotBalance_Code = -22002;
        public const string IError_Result_AccountNotBalance_Msg = "对帐不平";

        public const int IError_Result_CannotProceedAccountBalance_Code = -22003;
        public const string IError_Result_CannotProceedAccountBalance_Msg = "还不能进行该日期的对帐";

        public const int IError_Result_AccountBalancePeriodExceed_Code = -22004;
        public const string IError_Result_AccountBalancePeriodExceed_Msg = "对帐期已过,不能进行该日期对帐";

        #endregion

        #region 客服系统(-24000~-24499)

        public const int IError_Result_TransactionIDNotFound_Code = -24001;
        public const string IError_Result_TransactionIDNotFound_Msg = "未找到消费清单";

        public const int IError_Result_ReFunding_Code = -24002;
        public const string IError_Result_ReFunding_Msg = "退费要求正在处理中";

        #endregion

        #region 系统内部错误(-22500~-22999)

        public const int BT_IError_Result_Success_Code = 0;
        public const string BT_IError_Result_Success_Msg = "成功";

        public const int BT_IError_Result_UnknowError_Code = -19999;
        public const string BT_IError_Result_UnknowError_Msg = "未知错误";

         
        public const int BT_IError_Result_System_UnknowError_Code = -22500;
        public const string BT_IError_Result_System_UnknowError_Msg = "系统内部错误";

        public const int BT_IError_Result_System_IOError_Code = -22501;
        public const string BT_IError_Result_System_IOError_Msg = "磁盘读写错误";

        #endregion

        #region 业务错误(30000)

        public const int BT_IError_Result_BusinessError_Code = -30000;
        public const string BT_IError_Result_BusinessError_Msg = "业务逻辑错误";

        public const int BT_IError_Result_AccountExists_Code = -30001;
        public const string BT_IError_Result_AccountExists_Msg = "帐号已注册";

        public const int BT_IError_Result_CertificateCodeExists_Code = -30002;
        public const string BT_IError_Result_CertificateCodeExists_Msg = "证件号码已存在";

        public const int BT_IError_Result_PhoneBound_Code = -30003;
        public const string BT_IError_Result_PhoneBound_Msg = "该电话已经被绑定，不允许重复绑定";

        public const int BT_IError_Result_BoundPhoneExceed_Code = -30004;
        public const string BT_IError_Result_BoundPhoneExceed_Msg = "已超过电话绑定个数";


        public const int BT_IError_Result_PhoneNotBeBound_Code = -30005;
        public const string BT_IError_Result_PhoneNotBeBound_Msg = "该电话尚未被绑定，不能解绑";

        public const int BT_IError_Result_TransactionIDExist_Code = -30006;
        public const string BT_IError_Result_TransactionIDExist_Msg = "交易号已存在";

        public const int BT_IError_Result_PasswordError_Code = -30007;
        public const string BT_IError_Result_PasswordError_Msg = "密码错误";

        public const int BT_IError_Result_NoScoreInfo_Code = -30009;
        public const string BT_IError_Result_NoScoreInfo_Msg = "无积分记录";

        public const int BT_IError_Result_BizIPLimit_Code = -30011;
        public const string BT_IError_Result_BizIPLimit_Msg = " 业务平台IP受限";

        public const int BT_IError_Result_BizInterfaceLimit_Code = -30012;
        public const string BT_IError_Result_BizInterfaceLimit_Msg = "未获接口访问权限";

        public const int BT_IError_Result_TicketError_Code = -30013;
        public const string BT_IError_Result_TicketError_Msg = "Ticket错误";


        public const int BT_IError_Result_PhoenPostionError_Code = -30015;
        public const string BT_IError_Result_PhoenPostionError_Msg = "手机定位错误";

        public const int BT_IError_Result_NPData_Code = -30016;
        public const string BT_IError_Result_NPDataError_Msg = "网间移动号码携带错误";

        public const int BT_IError_Result_NPDataNull_Code = -30017;
        public const string BT_IError_Result_NPDataNullError_Msg = "无相关网间移动号码携带号码";

        #endregion

        #region 参数错误(-31000~-31033|-21504~-21554)

        public const int BT_IError_Result_InValidParameter_Code = -31000;
        public const string BT_IError_Result_InValidParameter_Msg = "参数错误";

        public const int BT_IError_Result_InValidPhoneNum_Code = -31001;
        public const string BT_IError_Result_InValidPhoneNum_Msg = "无效的电话号码";

        public const int BT_IError_Result_InValidCertificateCode_Code = -31002;
        public const string BT_IError_Result_InValidCertificateCode_Msg = "无效的证件号";

        public const int BT_IError_Result_InValidCertificateType_Code = -31003;
        public const string BT_IError_Result_InValidCertificateType_Msg = "无效的证件类型";

        public const int BT_IError_Result_InValidRegistrationStyle_Code = -31004;
        public const string BT_IError_Result_InValidRegistrationStyle_Msg = "无效的注册方式";

        public const int BT_IError_Result_InValidUserInfo_Code = -31005;
        public const string BT_IError_Result_InValidUserInfo_Msg = "无效的用户信息对象";

        public const int BT_IError_Result_InValidUserType_Code = -31006;
        public const string BT_IError_Result_InValidUserType_Msg = "无效的用户类型";

        public const int BT_IError_Result_InValidCustID_Code = -31007;
        public const string BT_IError_Result_InValidCustID_Msg = "无效的客户ID";

        public const int BT_IError_Result_InValidAreaCode_Code = -31008;
        public const string BT_IError_Result_InValidAreaCode_Msg = "无效的区号";


        public const int BT_IError_Result_InValidRealName_Code = -31009;
        public const string BT_IError_Result_InValidRealName_Msg = "无效的姓名";

        public const int BT_IError_Result_InValidBirthday_Code = -31010;
        public const string BT_IError_Result_InValidBirthday_Msg = "无效的生日";

        public const int BT_IError_Result_InValidSex_Code = -31011;
        public const string BT_IError_Result_InValidSex_Msg = "无效的性别";

        public const int BT_IError_Result_InValidCustLevel_Code = -31012;
        public const string BT_IError_Result_InValidCustLevel_Msg = "无效的客户级别";

        public const int BT_IError_Result_InValidEduLevel_Code = -31013;
        public const string BT_IError_Result_InValidEduLevel_Msg = "无效的教育水平";

        public const int BT_IError_Result_InValidFavorite_Code = -31014;
        public const string BT_IError_Result_InValidFavorite_Msg = "无效的爱好";

        public const int BT_IError_Result_InValidIncomeLevel_Code = -31015;
        public const string BT_IError_Result_InValidIncomeLevel_Msg = "无效的I收入水平";

        public const int BT_IError_Result_InValidEmail_Code = -31016;
        public const string BT_IError_Result_InValidEmail_Msg = "无效的Email";

        public const int BT_IError_Result_InValidPaymentAccountType_Code = -31017;
        public const string BT_IError_Result_InValidPaymentAccountType_Msg = "无效的支付帐号类型";

        public const int BT_IError_Result_InValidPaymentAccount_Code = -31018;
        public const string BT_IError_Result_InValidPaymentAccount_Msg = "无效的支付帐号";

        public const int BT_IError_Result_InValidPaymentAccountPassword_Code = -31019;
        public const string BT_IError_Result_InValidPaymentAccountPassword_Msg = "无效的支付帐号密码";

        public const int BT_IError_Result_InValidCustContactTel_Code = -31020;
        public const string BT_IError_Result_InValidCustContactTel_Msg = "无效的联系电话";

        public const int BT_IError_Result_InValidAssessmentInfos_Code = -31021;
        public const string BT_IError_Result_InValidAssessmentInfos_Msg = "无效的评估信息";

        public const int BT_IError_Result_InValidAddressRecords_Code = -31022;
        public const string BT_IError_Result_InValidAddressRecords_Msg = "无效的地址列表";

        public const int BT_IError_Result_InValidBoundPhoneRecords_Code = -31023;
        public const string BT_IError_Result_InValidBoundPhoneRecords_Msg = "无效的绑定电话列表";

        public const int BT_IError_Result_InValidExtendField_Code = -31024;
        public const string BT_IError_Result_InValidExtendField_Msg = "无效的ExtendField";

        public const int BT_IError_Result_InValidCredit_Code = -31025;
        public const string BT_IError_Result_InValidCredit_Msg = "无效的信用字段";

        public const int BT_IError_Result_InValidCreditLevel_Code = -31026;
        public const string BT_IError_Result_InValidCreditLevel_Msg = "无效的信用等级";

        public const int BT_IError_Result_InValidScore_Code = -31027;
        public const string BT_IError_Result_InValidScore_Msg = "无效的积分";

        public const int BT_IError_Result_InValidEffectiveTime_Code = -31028;
        public const string BT_IError_Result_InValidEffectiveTime_Msg = "无效的EffectiveTime";

        public const int BT_IError_Result_InValidExpireTime_Code = -31029;
        public const string BT_IError_Result_InValidExpireTime_Msg = "无效的过期时间";

        public const int BT_IError_Result_InValidZipcode_Code = -31030;
        public const string BT_IError_Result_InValidZipcode_Msg = "无效的邮编";

        public const int BT_IError_Result_InValidSubScribeStyle_Code = -31031;
        public const string BT_IError_Result_InValidSubScribeStyle_Msg = "无效的订购方式";

        public const int BT_IError_Result_InValidServiceName_Code = -31032;
        public const string BT_IError_Result_InValidServiceName_Msg = "无效的ServiceName";

        public const int BT_IError_Result_InValidSubscribeDate_Code = -31033;
        public const string BT_IError_Result_InValidSubscribeDate_Msg = "无效的SubscribeDate";

        public const int BT_IError_Result_InValidProvinceID_Code = -21504;
        public const string BT_IError_Result_InValidProvinceID_Msg = "无效的省代码";

        public const int BT_IError_Result_InValidServiceID_Code = -21505;
        public const string BT_IError_Result_InValidServiceID_Msg = "无效的服务ID";

        public const int BT_IError_Result_InValidPassword_Code = -21510;
        public const string BT_IError_Result_InValidPassword_Msg = "无效的密码";

        public const int BT_IError_Result_InValidSPID_Code = -21511;
        public const string BT_IError_Result_InValidSPID_Msg = "无效的系统标识";

        public const int BT_IError_Result_InValidTransactionID_Code = -21513;
        public const string BT_IError_Result_InValidTransactionID_Msg = "无效的TransactionID";

        public const int BT_IError_Result_InValidTimeStamp_Code = -21514;
        public const string BT_IError_Result_InValidTimeStamp_Msg = "无效的时间戳";

        public const int BT_IError_Result_InValidStartTime_Code = -21515;
        public const string BT_IError_Result_InValidStartTime_Msg = "无效的开始时间";

        public const int BT_IError_Result_InValidEndTime_Code = -21516;
        public const string BT_IError_Result_InValidEndTime_Msg = "无效的结束时间";

        public const int BT_IError_Result_InValidUserAccount_Code = -21528;
        public const string BT_IError_Result_InValidUserAccount_Msg = "无效的卡号";

        public const int BT_IError_Result_InValidStatus_Code = -21553;
        public const string BT_IError_Result_InValidStatus_Msg = "无效的状态";

        public const int BT_IError_Result_InValidSYSID_Code = -21554;
        public const string BT_IError_Result_InValidSYSID_Msg = "无效的系统标识";

        //新加错误定义
        //public const int BT_IError_Result_InValidFee_Code = -30010;
        //public const string BT_IError_Result_InValidFee_Msg = "无效的Fee";

        //public const int BT_IError_Result_ContinueNotPermit_Code = -21015;
        //public const string BT_IError_Result_ContinueNotPermit_Msg = "已退订该产品不允许续定购";

        //public const int BT_IError_Result_UserAccountError_Code = -30008;
        //public const string BT_IError_Result_UserAccountError_Msg = "用户帐号错误";

        //public const int BT_IError_Result_NoRelationgProvince_Code = -30011;
        //public const string BT_IError_Result_NoRelationgProvince_Msg = "无此区号对应省";

        //public const int BT_IError_Result_AuthenStyleAgainBound_Code = -30012;
        //public const string BT_IError_Result_AuthenStyleAgainBound_Msg = "该认证方式已被绑定，若要绑定须先解绑";

        //public const int BT_IError_Result_AuthenStyleAgainUnBound_Code = -30013;
        //public const string BT_IError_Result_AuthenStyleAgainUnBound_Msg = "该认证方式已被解绑，不能再次解绑";

        //public const int BT_IError_Result_AuthenNamesExists_Code = -30014;
        //public const string BT_IError_Result_AuthenNamesExists_Msg = "该认证名已被使用";

        #endregion



        #region 新的错误码(-40000~-50000)【2012/09/03】

        #region 系统错误(-40000~-40099)

        public const Int32 CIP_IError_Result_Success_Code = 0;
        public const string CIP_IError_Result_Success_Msg = "成功";

        public const Int32 CIP_IError_Result_UnknowError_Code = -40000;
        public const string CIP_IError_Result_UnknowError_Msg = "未知错误";

        public const Int32 CIP_IError_Result_SPIDInValid_Code = -40001;
        public const string CIP_IError_Result_SPIDInValid_Msg = "系统标识无效";

        public const Int32 CIP_IError_Result_SPStatusError_Code = -40002;
        public const string CIP_IError_Result_SPStatusError_Msg = "系统状态异常";

        public const Int32 CIP_IError_Result_SPPasswordError_Code = -40003;
        public const string CIP_IError_Result_SPPasswordError_Msg = "系统密码错误";

        public const Int32 CIP_IError_Result_SPIPInValid_Code = -40004;
        public const string CIP_IError_Result_SPIPInValid_Msg = "IP格式无效";

        public const Int32 CIP_IError_Result_SPIPLimited_Code = -40005;
        public const string CIP_IError_Result_SPIPLimite_Msg = "IP访问受限";

        public const Int32 CIP_IError_Result_SPInterfaceLimited_Code = -40006;
        public const string CIP_IError_Result_SPInterfaceLimited_Msg = "访问接口权限受限";
            
        #endregion

        #region 注册认证(-40100~-40199)

        public const Int32 CIP_IError_Result_RegisterException_Code = -40100;
        public const string CIP_IError_Result_RegisterException_Msg = "注册失败";

        public const Int32 CIP_IError_Result_RegisterVoiceException_Code = -40101;
        public const string CIP_IError_Result_RegisterVoiceException_Msg = "语音注册失败";

        public const Int32 CIP_IError_Result_RegisterWebException_Code = -40102;
        public const string CIP_IError_Result_RegisterWebException_Msg = "Web注册失败";

        public const Int32 CIP_IError_Result_RegisterUDBException_Code = -40103;
        public const string CIP_IError_Result_RegisterUDBException_Msg = "UDB注册失败";

        public const Int32 CIP_IError_Result_RegisterCrmException_Code = -40104;
        public const string CIP_IError_Result_RegisterCrmException_Msg = "Crm注册失败";

        public const Int32 CIP_IError_Result_AuthenException_Code = -40105;
        public const string CIP_IError_Result_AuthenException_Msg = "认证失败";

        public const Int32 CIP_IError_Result_TicketInValid_Code = -40106;
        public const string CIP_IError_Result_TicketInValid_Msg = "票据过期或无效";

        public const Int32 CIP_IError_Result_TransactionIDInValid_Code = -40107;
        public const string CIP_IError_Result_TransactionIDInValid_Msg = "流水号无效";

        #endregion

        #region 用户(-40200~-40299)

        public const Int32 CIP_IError_Result_User_UserIDInValid_Code = -40200;
        public const string CIP_IError_Result_User_UserIDInValid_Msg = "用户标识无效";

        public const Int32 CIP_IError_Result_User_UserNotExist_Code = -40201;
        public const string CIP_IError_Result_User_UserNotExist_Msg = "用户不存在";

        public const Int32 CIP_IError_Result_User_StatusException_Code = -40202;
        public const string CIP_IError_Result_User_StatusException_Msg = "用户状态异常";

        public const Int32 CIP_IError_Result_User_UserNameRepeat_Code = -40203;
        public const string CIP_IError_Result_User_UserNameRepeat_Msg = "用户名重复";

        public const Int32 CIP_IError_Result_User_UserTypeInValid_Code = -40204;
        public const string CIP_IError_Result_User_UserTypeInValid_Msg = "用户类型无效";

        public const Int32 CIP_IError_Result_User_SetInfoException_Code = -40205;
        public const string CIP_IError_Result_User_SetInfoException_Msg = "用户信息更新异常";

        public const Int32 CIP_IError_Result_User_SetAuthenStyleException_Code = -40206;
        public const string CIP_IError_Result_User_SetAuthenStyleException_Msg = "认证方式设置异常";

        public const Int32 CIP_IError_Result_User_PasswordError_Code = -40207;
        public const string CIP_IError_Result_User_PasswordError_Msg = "密码错误";

        public const Int32 CIP_IError_Result_User_VoicePasswordInValid_Code = -40208;
        public const string CIP_IError_Result_User_VoicePasswordInValid_Msg = "语音密码无效";

        public const Int32 CIP_IError_Result_User_SetPasswordException_Code = -40208;
        public const string CIP_IError_Result_User_SetPasswordException_Msg = "密码设置失败";

        public const Int32 CIP_IError_Result_User_UserHasExist_Code = -40209;
        public const String CIP_IError_Result_User_UserHasExist_Msg = "用户已存在";

        #endregion

        #region 电话(-40300~-40399)

        public const Int32 CIP_IError_Result_Phone_NumberException_Code = -40300;
        public const string CIP_IError_Result_Phone_NumberException_Msg = "电话号码异常";

        public const Int32 CIP_IError_Result_Phone_NumberInValid_Code = -40301;
        public const string CIP_IError_Result_Phone_NumberInValid_Msg = "电话格式无效";

        public const Int32 CIP_IError_Result_Phone_BindPhoneOverLimit_Code = -40302;
        public const string CIP_IError_Result_Phone_BindPhoneOverLimit_Msg = "绑定电话个数超过限制";

        public const Int32 CIP_IError_Result_Phone_BindPhoneRepeat_Code = -40303;
        public const string CIP_IError_Result_Phone_BindPhoneRepeat_Msg = "绑定电话重复";

        public const Int32 CIP_IError_Result_Phone_AuthenPhoneRepeat_Code = -40304;
        public const string CIP_IError_Result_Phone_AuthenPhoneRepeat_Msg = "认证电话重复";

        public const Int32 CIP_IError_Result_Phone_AuthenPhoneInValid_Code = -40305;
        public const string CIP_IError_Result_Phone_AuthenPhoneInValid_Msg = "认证电话无效";

        public const Int32 CIP_IError_Result_Phone_PhoneBindException_Code = -40306;
        public const string CIP_IError_Result_Phone_PhoneBindException_Msg = "电话绑定异常";

        public const Int32 CIP_IError_Result_Phone_PhoneUnBindException_Code = -40307;
        public const string CIP_IError_Result_Phone_PhoneUnBindException_Msg = "电话解绑异常";

        #endregion

        #region 邮箱(-40400~-40499)

        public const Int32 CIP_IError_Result_Email_Exception_Code = -40400;
        public const string CIP_IError_Result_Email_Exception_Msg = "邮箱异常";

        public const Int32 CIP_IError_Result_Email_FormatInValid_Code = -40401;
        public const string CIP_IError_Result_Email_FormatInValid_Msg = "邮箱格式无效";

        public const Int32 CIP_IError_Result_Email_AuthenEmailInValid_Code = -40402;
        public const string CIP_IError_Result_Email_AuthenEmailInValid_Msg = "认证邮箱无效";

        public const Int32 CIP_IError_Result_Email_AuthenEmailRepeat_Code = -40403;
        public const string CIP_IError_Result_Email_AuthenEmailRepeat_Msg = "认证邮箱重复";

        public const Int32 CIP_IError_Result_Email_SetEmailException_Code = -40404;
        public const string CIP_IError_Result_Email_SetEmailException_Msg = "邮箱设置异常";

        #endregion

        #region 其他(-41000~-41999)

        public const Int32 CIP_IError_Result_ProvinceCodeInValid_Code = -41000;
        public const string CIP_IError_Result_ProvinceCodeInValid_Msg = "省国标码无效";

        public const Int32 CIP_IError_Result_AreaCodeInValid_Code = -41001;
        public const string CIP_IError_Result_AreaCodeInValid_Msg = "地区国标码无效";

        #endregion

        #region 号百账户

        public const Int32 CIP_IError_Result_BesttoneAcountException_Code = -42000;
        public const string CIP_IError_Result_BesttoneAcountException_Msg = "未知异常";

        public const Int32 CIP_IError_Result_BesttoneAccount_UnRegister_Code = -42001;
        public const string CIP_IError_Result_BesttoneAccount_UnRegister_Msg = "未开通号码百事通账户";

        public const Int32 CIP_IError_Result_BesttoneAccount_RegisterError_Code = -42002;
        public const string CIP_IError_Result_BesttoneAccount_RegisterError_Msg = "号码百事通账户注册失败";

        public const Int32 CIP_IError_Result_BesttoneAccount_CardNoInValid_Code = -42003;
        public const string CIP_IError_Result_BesttoneAccount_CardNoInValid_Msg = "无效卡号";

        public const Int32 CIP_IError_Result_BesttoneAccount_CardTypeInValid_Code = -42004;
        public const string CIP_IError_Result_BesttoneAccount_CardTypeInValid_Msg = "无效卡类型";

        #endregion

        #endregion

    }
}

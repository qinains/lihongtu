using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;
using System.Data;
using System.Xml;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

using BTUCenter.Proxy;


/// <summary>
/// BestToneUCenterForBizTour 的摘要说明
/// </summary>
[WebService(Namespace = "http://BestToneUserCenter.vnet.cn")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class BestToneUCenterForBizTour : System.Web.Services.WebService
{

    public BestToneUCenterForBizTour()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    #region 注册认证

    public class UserRegistryResult
    {
        public string ProvinceID;
        public string SPID;
        public string CustID;
        public string UserAccount;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;

    }
    [WebMethod(Description = "用户注册接口")]
    public UserRegistryResult UserRegistry(string ProvinceID, string SPID, string RegistrationStyle, string TimeStamp, UserInfo UserDetailInfo)
    {
        UserRegistryResult Result = new UserRegistryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "初始";
        Result.CustID = UserDetailInfo.CustID;
        Result.ProvinceID = "35";
        Result.SPID = SPID;
        Result.UserAccount = UserDetailInfo.UserAccount;
        Result.ExtendField = "";

        string Message = "";
        try
        {
            #region MyRegion

            if (UserDetailInfo.UserAccount == null)
            {
                UserDetailInfo.UserAccount = "";
            }

            if (UserDetailInfo.Password == null)
            {
                UserDetailInfo.Password = "";
            }

            if (UserDetailInfo.CustID == null)
            {
                UserDetailInfo.CustID = "";
            }

            if (UserDetailInfo.Birthday == null)
            {
                UserDetailInfo.Birthday = "";
            }

            if (UserDetailInfo.EduLevel == null)
            {
                UserDetailInfo.EduLevel = "";
            }

            if (UserDetailInfo.Favorite == null)
            {
                UserDetailInfo.Favorite = "";
            }

            if (UserDetailInfo.IncomeLevel == null)
            {
                UserDetailInfo.IncomeLevel = "";
            }

            if (UserDetailInfo.Email == null)
            {
                UserDetailInfo.Email = "";
            }

            if (UserDetailInfo.PaymentAccount == null)
            {
                UserDetailInfo.PaymentAccount = "";
            }

            if (UserDetailInfo.PaymentAccountPassword == null)
            {
                UserDetailInfo.PaymentAccountPassword = "";
            }

            if (UserDetailInfo.PaymentAccountType == null)
            {
                UserDetailInfo.PaymentAccountType = "";
            }

            if (UserDetailInfo.EnterpriseID == null)
            {
                UserDetailInfo.EnterpriseID = "";
            }

            if (UserDetailInfo.ExtendField == null)
            {
                UserDetailInfo.ExtendField = "";
            }

            if (UserDetailInfo.CustContactTel == null)
            {
                UserDetailInfo.CustContactTel = "";
            }

            #endregion

            #region 判断是否是特殊的SPID

            string OldSPID = System.Configuration.ConfigurationManager.AppSettings["OldType_SPID"];

            int SIP = OldSPID.IndexOf(SPID);

            if (SIP >= 0)
            {
                // userType = "";
                switch (UserDetailInfo.UserType)
                {
                    case "01":
                        UserDetailInfo.UserType = "14";
                        break;
                    case "02":
                        UserDetailInfo.UserType = "20";
                        break;
                    case "03":
                        UserDetailInfo.UserType = "12";
                        break;
                    case "09":
                        UserDetailInfo.UserType = "90";
                        break;
                    case "11":
                        UserDetailInfo.UserType = "30";
                        break;
                    case "00":
                        UserDetailInfo.UserType = "42";
                        break;
                    default:
                        UserDetailInfo.UserType = "90";
                        break;
                }

            }

            #endregion

            UserInfoClass userObj = new UserInfoClass(UserDetailInfo);

            #region 数据校验
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserRegistry", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(RegistrationStyle))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRegistrationStyle_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRegistrationStyle_Msg + "，不能为空";
                return Result;
            }

            if (RegistrationStyle.Length != 2)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRegistrationStyle_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRegistrationStyle_Msg + "，长度有误";
                return Result;
            }

            if (ConstDefinition.Span_RegistrationStyle.IndexOf(RegistrationStyle) < 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRegistrationStyle_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRegistrationStyle_Msg + "，无该注册类型";
                return Result;
            }

            string VoicePwdSPID = System.Configuration.ConfigurationManager.AppSettings["VoicePwd_SPID"];

            int SIP1 = VoicePwdSPID.IndexOf(SPID);

            if (SIP1 < 0)
            {
                if (CommonUtility.IsEmpty(UserDetailInfo.Password))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + ",密码不能为空";
                    return Result;
                }

                if (!CommonUtility.IsNumeric(UserDetailInfo.Password))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "只能是数字";
                    return Result;
                }
            }

            string MBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["MBesttoneSPID"];
            //string NetBesttoneSPID =  ConfigurationManager.AppSettings["NetBesttoneSPID"];
            //if (SPID == MBesttoneSPID or SPID = )
            //如果是 移百，网百，或世博注册，则赋初值
            //bool IsPersonalUserName = false;
            if (RegistrationStyle == "05" || RegistrationStyle == "06")
            {
                //IsPersonalUserName = true ;
                //赋初值
                UserDetailInfo.UserType = "00";
                //UserDetailInfo.AreaCode = "10";
                //UserDetailInfo.UProvinceID = "01";
                //对密码进行校验，必须用户自己设密码
                if (CommonUtility.IsEmpty(UserDetailInfo.Password))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + ",密码不能为空";
                    return Result;
                }

                if (!CommonUtility.IsNumeric(UserDetailInfo.Password))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "只能是数字";
                    return Result;
                }
            }
            //数据校验
            Result.Result = userObj.UserInfoCommonCheck(out Result.ErrorDescription, this.Context);
            if (Result.Result != 0)
                return Result;


            #endregion

            //bool IsDistributePassword = false;
            //如果卡号为空则代表该用户是自动分配密码
            //如果密码为空则自动分配密码
            if (CommonUtility.IsEmpty(userObj.UserAccount) & CommonUtility.IsEmpty(userObj.Password))
            {
                Random rd = new Random();
                userObj.Password = rd.Next(111111, 999999).ToString();
            }
            Result.Result = userObj.UserInfoRegistry(this.Context, RegistrationStyle, SPID, out Result.ErrorDescription);
            Result.ProvinceID = "35";
            Result.UserAccount = userObj.UserAccount;
            Result.CustID = userObj.CustID;

            //如果注册成功通知积分系统,并发短信通知用户
            // 网百和移百是否通知积分系统？
            if (Result.Result == 0)
            {
                string ErrMsg = "";
                //通知积分系统
                //CommonBizRules.CustInfoNotify(userObj.CustID, userObj.UserAccount, "", "0", "", userObj.PaymentAccountPassword);
                CIP2BizRules.InsertCustInfoNotify(userObj.CustID, "2", SPID, "", "0", out ErrMsg);
                //移百系统注册的用户不发短信
                if (SIP1 >= 0)
                {
                    if (CommonUtility.IsEmpty(UserDetailInfo.CustID))
                        Message = "您已成为号码百事通商旅卡客户,卡号:" + Result.UserAccount + "密码:" + userObj.Password + ",欢迎使用号百商旅服务!此短信免费。";
                    else
                        Message = "尊敬的" + UserDetailInfo.RealName + "，您的补卡操作已经成功，您的新卡号:" + Result.UserAccount + "密码:" + userObj.Password + ",请您注意查收。如有疑问请您拨打114/118114进行咨询.此短信免费。";
                    //通知短信网关
                    //CommonBizRules.SendMessage(userObj.CustContactTel, Message, "35000000");
                    CommonBizRules.SendMessageV3(userObj.CustContactTel, Message, SPID);
                }
            }
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("用户注册接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";RegistrationStyle - " + RegistrationStyle);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                //记录UserDetailInfo
                UserInfoClass.WriteLogForUserInfo(UserDetailInfo, ref msg);

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription + "\r\n");
                msg.Append("ProvinceID - " + Result.ProvinceID);
                msg.Append("; SPID - " + Result.SPID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; Message - " + Message);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserRegistry", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, UserDetailInfo.CustID, UserDetailInfo.UserAccount, Result.Result,
                    Result.ErrorDescription, "", "UserRegistry");
            }
            catch { }
        }

        return Result;
    }

    public class UserAuthV2Result
    {
        public int Result;
        public string CustID;
        public string UserAccount;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "密码认证V2接口")]
    public UserAuthV2Result UserAuthV2(string SPID, string AuthenName, string AuthenType, string Password, string ExtendField)
    {
        UserAuthV2Result Result = new UserAuthV2Result();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        string Ticket = "";
        string IsNeedLogin = "";
        string outerid = "";
        string ProvinceID = "";
        string CustType = "";
        string RealName = "";
        string UserName = "";
        string NickName = "";
        string IsQuery = "";
        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(AuthenName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(AuthenType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }
            if (!CommonUtility.IsEmpty(ExtendField))
            {
                ProvinceID = CommonBizRules.GetValueFromXmlStr(ExtendField, "ProvinceID");
                IsNeedLogin = CommonBizRules.GetValueFromXmlStr(ExtendField, "IsNeedLogin");
                IsQuery = CommonBizRules.GetValueFromXmlStr(ExtendField, "IsQuery");
            }

            if (CommonUtility.IsEmpty(Password))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            #endregion

            Result.Result = BTForBusinessSystemInterfaceRules.UserAuthV2(SPID, AuthenName, AuthenType, Password,
                this.Context, ProvinceID, IsQuery, "",
                out Result.ErrorDescription, out Result.CustID, out Result.UserAccount, out CustType, out outerid, out ProvinceID, out  RealName, out  UserName, out  NickName);
            if (IsNeedLogin == "1")
            {
                //生成cookie
                Ticket = CommonBizRules.CreateTicket();
                string errMsg = "";
                int iCIPTicket = CIPTicketManager.insertCIPTicket(Ticket, SPID, Result.CustID, RealName, NickName, UserName, outerid, Result.ErrorDescription, AuthenName, AuthenType, out errMsg);
                if (iCIPTicket != 0)
                {
                    Result.Result = iCIPTicket;
                    Result.ErrorDescription = errMsg;
                }
            }
            Result.ExtendField = BTBizRules.GenerateOuterIDXml(outerid, ProvinceID, Ticket);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("密码认证鉴权V2接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";AuthenName - " + AuthenName);
                msg.Append(";AuthenType - " + AuthenType);
                msg.Append(";Password - " + Password);
                msg.Append(";ExtendField - " + ExtendField);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append(";CustType - " + CustType);
                msg.Append(";outerid - " + outerid);
                msg.Append(";ProvinceID - " + ProvinceID);
                msg.Append(";RealName - " + RealName);
                msg.Append(";UserName - " + UserName);
                msg.Append(";NickName - " + NickName);

                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthV2", msg);
                #endregion

                CommonBizRules.WriteDataCustAuthenLog(SPID, Result.CustID, ProvinceID, AuthenType, AuthenName, "1", Result.Result,
                      Result.ErrorDescription);
            }
            catch { }

        }
        return Result;

    }

    public class ReverseUserRegistryResult
    {
        public string ProvinceID;
        public string CustID;
        public string UserAccount;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "反向注册接口")]
    public ReverseUserRegistryResult ReverseUserRegistry(string ProvinceID, string SPID, string TimeStamp,
        string CertificateCode, string CertificateType, string RealName, string ContactPhone, string AreaCode)
    {
        ReverseUserRegistryResult Result = new ReverseUserRegistryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ProvinceID = "35";
        Result.CustID = "";
        Result.UserAccount = "";
        Result.ErrorDescription = "";
        Result.ExtendField = "";

        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "无该省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(RealName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，长度有误";
                return Result;
            }


            if (!CommonUtility.IsEmpty(ContactPhone))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, ContactPhone, out phone))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                    return Result;
                }
                ContactPhone = phone;
            }


            if (!CommonUtility.IsEmpty(CertificateType))
            {
                if (ConstDefinition.Span_CertificateType.IndexOf(CertificateType) < 0)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCertificateType_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCertificateType_Msg + "，无此类型";
                    return Result;
                }

                if (CommonUtility.IsEmpty(CertificateCode))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Msg + "，不能为空";
                    return Result;
                }
            }

            //校验AreaCode 
            if (!CommonUtility.IsEmpty(AreaCode))
            {
                if (AreaCode.Length == 2)
                    AreaCode = "0" + AreaCode;

                if (AreaCode.Length != ConstDefinition.Length_AreaCode)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidAreaCode_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidAreaCode_Msg + ",区号长度有误";
                    return Result;
                }
            }
            Result.Result = BTBizRules.ReverseUserRegistry(ProvinceID, SPID, TimeStamp, CertificateCode,
                CertificateType, RealName, ContactPhone, AreaCode, out Result.ErrorDescription, out Result.CustID, out Result.UserAccount);

            if (Result.Result == 0)
            {
                //通知积分系统
                // CommonBizRules.CustInfoNotify(Result.CustID, Result.UserAccount, "", "0","1","","42",ProvinceID,SPID);
            }
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("反向注册接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";CertificateCode - " + CertificateCode);
                msg.Append(";CertificateType - " + CertificateType);
                msg.Append(";RealName - " + RealName);
                msg.Append(";RealName - " + ContactPhone);
                msg.Append(";AreaCode - " + AreaCode);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                msg.Append("处理结果 - " + Result.Result + " " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("ReverseUserRegistry", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, "", "", Result.Result,
                    Result.ErrorDescription, "", "ReverseUserRegistry");
            }
            catch { }
        }

        return Result;

    }

    public class UserAuthResult
    {
        public string ProvinceID;
        public int Result;
        public UserInfo UserDetailInfo;
        public SubscriptionRecord[] SubscriptionRecords;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "密码认证鉴权接口")]  //包括激活码验证,lihongtu:增加phonenum 字段 
    public UserAuthResult UserAuth(string ProvinceID, string SPID, string TimeStamp, string CustID, string UserAccount, string PhoneNum, string Password)
    {
        UserAuthResult Result = new UserAuthResult();
        Result.ErrorDescription = "初始";
        Result.ProvinceID = "35";
        Result.UserDetailInfo = null;
        Result.SubscriptionRecords = null;
        Result.ExtendField = "";

        Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
        if (Result.Result != 0)
        {
            return Result;
        }

        //接口访问权限判断
        Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserAuth", this.Context, out Result.ErrorDescription);
        if (Result.Result != 0)
        {
            return Result;
        }

        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "无该省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (!CommonUtility.IsEmpty(CustID))
            {
                if (CustID.Length != ConstDefinition.Length_CustID)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，长度有误";
                    return Result;
                }
            }

            if (!CommonUtility.IsEmpty(UserAccount))
            {
                if (UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }

                if (UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }
            }

            if (!CommonUtility.IsEmpty(PhoneNum))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, PhoneNum, out phone))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                    return Result;
                }
                PhoneNum = phone;
            }

            if (CommonUtility.IsEmpty(Password))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            string EncryptedPassword = CryptographyUtil.Encrypt(Password);
            string t_password = Password;
            Password = "";

            Result.Result = UserInfoClass.UserInfoQueryV2(ProvinceID, SPID, UserAccount, CustID, PhoneNum, Password, out Result.ErrorDescription, out Result.UserDetailInfo, out Result.SubscriptionRecords);

            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("客户信息平台密码鉴权认证接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";EncryptedPassword - " + EncryptedPassword);
            msg.Append("\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("UserAuth", msg);


            if (Result.UserDetailInfo != null)
            {
                Result.Result = UserInfoClass.ValidUserPassword(Result.UserDetailInfo.CustID, EncryptedPassword);
                if (Result.Result == 0)
                {
                    Result.ErrorDescription = "认证通过";
                }
                else
                {
                    Result.UserDetailInfo = new UserInfo();
                    Result.ErrorDescription = "认证未通过";
                }
            }
            else
            {
                Result.ErrorDescription = "无此用户";
            }
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("密码认证鉴权接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";UserAccount - " + UserAccount);
                msg.Append(";CustID - " + CustID);
                msg.Append(";Password - " + Password);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                //记录UserDetailInfo
                UserInfoClass.WriteLogForUserInfo(Result.UserDetailInfo, ref msg);

                if (Result.SubscriptionRecords != null)
                    if (Result.SubscriptionRecords.Length > 0)
                    {
                        msg.Append("SubScriptionRecords: \r\n");
                        for (int i = 0; i < Result.SubscriptionRecords.Length; i++)
                        {
                            msg.Append("CustID - " + Result.SubscriptionRecords[i].CustID);
                            msg.Append("UserAccount - " + Result.SubscriptionRecords[i].UserAccount);
                            msg.Append("SubscribeStyle - " + Result.SubscriptionRecords[i].SubscribeStyle);
                            msg.Append("ServiceID - " + Result.SubscriptionRecords[i].ServiceID);
                            msg.Append("ServiceName - " + Result.SubscriptionRecords[i].ServiceName);
                            msg.Append("StartTime - " + Result.SubscriptionRecords[i].StartTime);
                            msg.Append("EndTime - " + Result.SubscriptionRecords[i].EndTime);
                            msg.Append("TransactionID - " + Result.SubscriptionRecords[i].TransactionID);

                            msg.Append("\r\n");
                        }
                    }
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserAuth", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, CustID, UserAccount, Result.Result,
                    Result.ErrorDescription, PhoneNum, "UserAuth");
            }
            catch { }
        }

        return Result;
    }

    #region 认证方式通知接口

    public class AuthStyleNotifyResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "认证方式通知接口")]
    public AuthStyleNotifyResult AuthStyleNotify(string SPID, string CustID, string AuthenName, string AuthenType, string OPType, string ExtendField)
    {
        AuthStyleNotifyResult Result = new AuthStyleNotifyResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.CustID = CustID;
        Result.ErrorDescription = "";
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AuthStyleNotify", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            if (CommonUtility.IsEmpty(CustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }


            if (CommonUtility.IsEmpty(AuthenName))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(AuthenType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(OPType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，OPtype不能为空";
                return Result;
            }

            if (!("2".Equals(AuthenType) || "1".Equals(AuthenType)))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，此认证方式的类型 接口不支持";
                return Result;
            }
            if ("1".Equals(AuthenType))
            {

                Result.Result = AuthStyleRules.AuthStyleNotify(SPID, CustID, AuthenName, AuthenType, OPType, "", out Result.ErrorDescription);

            }
            if ("2".Equals(AuthenType))
            {
                //非手机绑定不支持
                if (AuthenName.Length != 11 || !AuthenName.StartsWith("1"))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，认证方式需为手机";
                    return Result;
                }
                //手机认证绑定
                if ("0".Equals(OPType))
                {
                    Result.Result = PhoneBO.PhoneSet(SPID, CustID, AuthenName, "2", "2", out Result.ErrorDescription);
                }
                //手机认证解绑
                if ("1".Equals(OPType))
                {
                    Result.Result = PhoneBO.PhoneUnBind(CustID, AuthenName, "2", out Result.ErrorDescription);
                }
            }
            #endregion

        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("认证方式通知接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID);
                msg.Append(";AuthenName - " + AuthenName);
                msg.Append(";AuthenType - " + AuthenType);
                msg.Append(";OPType - " + OPType);
                msg.Append(";ExtendField - " + ExtendField);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AuthStyleNotify", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, AuthenName, AuthenType, Result.Result,
                    Result.ErrorDescription, CustID, "AuthStyleNotify");
            }
            catch { }
        }

        return Result;
    }

    #endregion

    #region 认证方式查询接口

    public class AuthStyleQueryByAuthenNameResult
    {
        public int Result;
        public string CustID;
        public string UserAccount;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "认证方式查询接口")]
    public AuthStyleQueryByAuthenNameResult AuthStyleQueryByAuthenName(string SPID, string AuthenName,
        string AuthenType, string ExtendField)
    {
        AuthStyleQueryByAuthenNameResult Result = new AuthStyleQueryByAuthenNameResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }
            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AuthStyleQueryByAuthenName", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(AuthenName))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(AuthenType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }
            #endregion

            Result.Result = BTForBusinessSystemInterfaceRules.AuthStyleQueryByAuthenName(SPID, AuthenName, AuthenType,
                out Result.ErrorDescription, out Result.CustID, out Result.UserAccount);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("认证方式查询接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";AuthenName - " + AuthenName);
                msg.Append(";AuthenType - " + AuthenType);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AuthStyleQueryByAuthenName", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, AuthenName, AuthenType, Result.Result,
                    Result.ErrorDescription, "", "AuthStyleQueryByAuthenName");
            }
            catch { }
        }

        return Result;

    }

    #endregion

    #region 用户认证方式查询接口
    public class UserAuthStyleQueryResult
    {
        public int Result;
        public AuthenStyleInfoRecord[] AuthenStyleInfoRecords;
        public string ErrorDescription;
        public string ExtendField;

    }
    [WebMethod(Description = "用户认证方式查询接口")]
    public UserAuthStyleQueryResult UserAuthStyleQuery(string SPID, string UserAccount, string ExtendField)
    {
        UserAuthStyleQueryResult Result = new UserAuthStyleQueryResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        Result.AuthenStyleInfoRecords = null;

        try
        {
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，SPID 不能为空";
                return Result;
            }
            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，SPID长度有误";
                return Result;
            }
            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserAuthStyleQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            if (CommonUtility.IsEmpty(UserAccount))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，UserAccount不能为空";
                return Result;
            }

            if (UserAccount.Length > ConstDefinition.Length_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，UserAccount长度有误";
                return Result;
            }

            Result.Result = BTBizRules.UserAuthStyleQuery(SPID, UserAccount, "", out Result.AuthenStyleInfoRecords, out Result.ErrorDescription);


        }
        catch (Exception ex)
        {
            Result.ErrorDescription += ex.Message;
            Result.Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("用户认证方式查询接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";UserAccount - " + UserAccount);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result.Result.ToString() + "\r\n");
            msg.Append(":ErrorDescription - " + Result.ErrorDescription);
            msg.Append(";查询结果 -" + "\r\n");
            if (Result.AuthenStyleInfoRecords != null && Result.AuthenStyleInfoRecords.Length > 0)
            {
                int len = Result.AuthenStyleInfoRecords.Length;
                for (int i = 0; i < len; i++)
                {
                    msg.Append("CustID - " + Result.AuthenStyleInfoRecords[i].CustID + "\t");
                    msg.Append("AuthenType - " + Result.AuthenStyleInfoRecords[i].AuthenType + "\t");
                    msg.Append("AuthenName - " + Result.AuthenStyleInfoRecords[i].AuthenName + "\r\n");

                }
            }
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthStyleQuery", msg);
            #endregion
        }


        return Result;
    }

    #endregion

    #endregion

    #region 客户基本信息

    public class UserInfoQueryResult
    {
        public string ProvinceID;
        public int Result;
        public string ErrorDescription;
        public string TimeStamp;
        public UserInfo UserDetailInfo;
        public SubscriptionRecord[] SubScriptionRecords;
        public string ExtendField;

    }
    [WebMethod(Description = "用户信息查询接口")]
    public UserInfoQueryResult UserInfoQuery(string ProvinceID, string SPID, string UserAccount, string CustID, string PhoneNum, string TimeStamp)
    {
        UserInfoQueryResult Result = new UserInfoQueryResult();
        Result.ProvinceID = "35";
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "初始";
        Result.TimeStamp = CommonUtility.TimeStamp;
        Result.UserDetailInfo = null;
        Result.SubScriptionRecords = null;
        Result.ExtendField = "";

        try
        {

            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",无此省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserInfoQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (!CommonUtility.IsEmpty(UserAccount))
            {
                if (UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }

                if (UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }
            }

            if (CommonUtility.IsEmpty(UserAccount) & CommonUtility.IsEmpty(CustID) & CommonUtility.IsEmpty(PhoneNum))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + ",关键信息不能全为空";
                return Result;
            }

            if (!CommonUtility.IsEmpty(PhoneNum))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, PhoneNum, out phone))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                    return Result;
                }
                PhoneNum = phone;
            }

            Result.Result = UserInfoClass.UserInfoQuery(ProvinceID, SPID, UserAccount, CustID, PhoneNum, "", out Result.ErrorDescription, out Result.UserDetailInfo, out Result.SubScriptionRecords);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog

                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("用户信息查询接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";UserAccount - " + UserAccount);
                msg.Append(";CustID - " + CustID);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; TimeStamp - " + Result.TimeStamp);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                //记录UserDetailInfo
                UserInfoClass.WriteLogForUserInfo(Result.UserDetailInfo, ref msg);

                if (Result.SubScriptionRecords != null)
                    if (Result.SubScriptionRecords.Length > 0)
                    {
                        msg.Append("SubScriptionRecords: \r\n");
                        for (int i = 0; i < Result.SubScriptionRecords.Length; i++)
                        {
                            msg.Append("CustID - " + Result.SubScriptionRecords[i].CustID);
                            msg.Append(";UserAccount - " + Result.SubScriptionRecords[i].UserAccount);
                            msg.Append(";SubscribeStyle - " + Result.SubScriptionRecords[i].SubscribeStyle);
                            msg.Append(";ServiceID - " + Result.SubScriptionRecords[i].ServiceID);
                            msg.Append(";ServiceName - " + Result.SubScriptionRecords[i].ServiceName);
                            msg.Append(";StartTime - " + Result.SubScriptionRecords[i].StartTime);
                            msg.Append(";EndTime - " + Result.SubScriptionRecords[i].EndTime);
                            msg.Append(";TransactionID - " + Result.SubScriptionRecords[i].TransactionID);

                            msg.Append("\r\n");
                        }
                    }
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserInfoQuery", msg);

                #endregion

                CommonBizRules.WriteDataLog(SPID, CustID, UserAccount, Result.Result,
                    Result.ErrorDescription, PhoneNum, "02");
            }
            catch { }

        }

        return Result;

    }

    public class BasicInfoQueryResult
    {
        public string ProvinceID;
        public int Result;
        public UserBasicInfoRecord[] UserBasicInfoRecords;
        public string ErrorDescription;
        public string ExtendField;

    }
    [WebMethod(Description = "基本信息查询接口")]
    public BasicInfoQueryResult BasicInfoQuery(string ProvinceID, string SPID, string TimeStamp, string UserAccount,
        string PhoneNum, string CertificateCode, string CertificateType, string RealName, string ExtendField)
    {
        BasicInfoQueryResult Result = new BasicInfoQueryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ProvinceID = "35";
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        Result.UserBasicInfoRecords = null;

        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "无该省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }


            if (!CommonUtility.IsEmpty(UserAccount))
            {
                if (UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }

                if (UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }

            }

            if (!CommonUtility.IsEmpty(PhoneNum))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, PhoneNum, out phone))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                    return Result;
                }
                PhoneNum = phone;
            }


            if (!CommonUtility.IsEmpty(CertificateType))
            {
                if (ConstDefinition.Span_CertificateType.IndexOf(CertificateType) < 0)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCertificateType_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCertificateType_Msg + "，无此类型";
                    return Result;
                }

                if (CommonUtility.IsEmpty(CertificateCode))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Msg + "，不能为空";
                    return Result;
                }
            }

            if (CommonUtility.IsEmpty(UserAccount) & CommonUtility.IsEmpty(PhoneNum) & CommonUtility.IsEmpty(CertificateCode) & CommonUtility.IsEmpty(CertificateType) & CommonUtility.IsEmpty(RealName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + ",关键信息不能全部为空";
                return Result;
            }

            Result.Result = BTBizRules.BasicInfoQuery(ProvinceID, SPID, UserAccount,
                 PhoneNum, CertificateCode, CertificateType, RealName, out Result.UserBasicInfoRecords, out Result.ErrorDescription);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("唯一性查询接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";UserAccount - " + UserAccount);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append(";CertificateCode - " + CertificateCode);
                msg.Append(";CertificateType - " + CertificateType);
                msg.Append(";ExtendField - " + ExtendField);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                if (Result.UserBasicInfoRecords != null)
                {
                    if (Result.UserBasicInfoRecords.Length > 0)
                    {
                        msg.Append("UserBasicInfoRecords: \r\n");
                        for (int i = 0; i < Result.UserBasicInfoRecords.Length; i++)
                        {
                            msg.Append("CertificateCode- " + Result.UserBasicInfoRecords[i].CertificateCode);
                            msg.Append(";CertificateType- " + Result.UserBasicInfoRecords[i].CertificateType);
                            msg.Append(";RealName- " + Result.UserBasicInfoRecords[i].RealName);
                            msg.Append(";UserAccount- " + Result.UserBasicInfoRecords[i].UserAccount);
                            if (Result.UserBasicInfoRecords[i].BoundPhoneRecords != null)
                            {
                                if (Result.UserBasicInfoRecords[i].BoundPhoneRecords.Length > 0)
                                {
                                    msg.Append("BoundPhoneRecords: ");
                                    for (int j = 0; j < Result.UserBasicInfoRecords[i].BoundPhoneRecords.Length; j++)
                                    {
                                        msg.Append(";Phone- " + Result.UserBasicInfoRecords[i].BoundPhoneRecords[j].Phone);
                                    }
                                }
                            }
                        }
                    }
                }
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("BasicInfoQuery", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, "", UserAccount, Result.Result,
                    Result.ErrorDescription, PhoneNum, "BasicInfoQuery");
            }
            catch { }
        }

        return Result;

    }

    public class UserInfoModifyResult
    {
        public string ProvinceID;
        public int Result;
        public string CustID;
        public string UserAccount;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "用户信息修改接口")]
    public UserInfoModifyResult UserInfoModify(string ProvinceID, string SPID, string TimeStamp, UserInfo UserDetailInfo)
    {

        //ProvinceID  请求的省份代码
        //SPID        请求发起方在认证鉴权系统登记的spid
        //TimeStamp   yyyy-MM-dd HH:m:ss 其中HH 取值为00-23,时区为东八区
        //UserDetailInfo 用户基本资料
        //注:不允许修改CustID,UserAccount,CertificateCode;若修改密码,则调用"用户密码重置接口"
        //;若修改绑定主叫号码,则调用"电话号码绑定接口"

        UserInfoModifyResult Result = new UserInfoModifyResult();

        Result.ProvinceID = "35";
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.CustID = UserDetailInfo.CustID;
        Result.UserAccount = UserDetailInfo.UserAccount;
        Result.ErrorDescription = "初始";
        Result.ExtendField = "";

        #region 判断是否是特殊的SPID

        string OldSPID = System.Configuration.ConfigurationManager.AppSettings["OldType_SPID"];

        int SIP = OldSPID.IndexOf(SPID);

        if (SIP >= 0)
        {
            switch (UserDetailInfo.UserType)
            {
                case "01":
                    UserDetailInfo.UserType = "14";
                    break;
                case "02":
                    UserDetailInfo.UserType = "20";
                    break;
                case "03":
                    UserDetailInfo.UserType = "12";
                    break;
                case "09":
                    UserDetailInfo.UserType = "90";
                    break;
                case "11":
                    UserDetailInfo.UserType = "30";
                    break;
                case "00":
                    UserDetailInfo.UserType = "42";
                    break;
                default:
                    UserDetailInfo.UserType = "90";
                    break;
            }

        }

        #endregion

        UserInfoClass userObj = new UserInfoClass(UserDetailInfo);
        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",无该省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(UserDetailInfo.CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }


            if (CommonUtility.IsEmpty(UserDetailInfo.UserAccount))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + ",不能为空";
                return Result;
            }

            if (UserDetailInfo.UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }

            if (UserDetailInfo.UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }


            Result.Result = userObj.UserInfoCommonCheck(out Result.ErrorDescription, this.Context);
            if (Result.Result != 0)
                return Result;


            #endregion

            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserInfoModify", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            Result.Result = userObj.ModifyUserInfo(SPID, out Result.ErrorDescription);
            //如果修改成功通知积分系统
            string ErrorMsg = "";
            if (Result.Result == 0)
                CIP2BizRules.InsertCustInfoNotify(UserDetailInfo.CustID, "2", SPID, "", "0", out ErrorMsg);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("用户信息修改接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                //记录UserDetailInfo
                UserInfoClass.WriteLogForUserInfo(UserDetailInfo, ref msg);

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription + "\r\n");
                msg.Append("ProvinceID - " + Result.ProvinceID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("ModifyUserInfo", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, UserDetailInfo.CustID, UserDetailInfo.UserAccount, Result.Result,
                    Result.ErrorDescription, "", "UserInfoModify");
            }
            catch { }
        }

        return Result;

    }

    [WebMethod(Description = "客户信息同步接口For浙江")]
    public string NewCardCustomerInfoExport(string strProvinceID, string strSPID, string strTimeStamp, string XmlInfor, string dealType)
    {
        string Result = ErrorDefinition.BT_IError_Result_UnknowError_Code.ToString();
        string ErrMsg = "";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(strProvinceID))
            {

                Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",不能为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (strProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",长度有误";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(strProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，无该省代码";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (CommonUtility.IsEmpty(strSPID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (strSPID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (dealType != "0" & dealType != "1")
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，DealType无此定义";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (CommonUtility.IsEmpty(XmlInfor))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，XmlInfor为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }
            string custID = "";
            string userType = "";
            string userAccount = "";
            string uProvinceID = "";


            string status = "";
            string realName = "";
            string cardClass = "";
            string credit = "";
            string registration = "";
            string certificateCode = "";
            string certificateType = "";
            string birthday = "";
            string sex = "";
            string custLevel = "";
            string custContactTel = "";
            string enterpriseID = "";
            string extendField = "";

            custID = CommonUtility.GetValueFromXML(XmlInfor, "custID");
            userType = CommonUtility.GetValueFromXML(XmlInfor, "userType");
            userAccount = CommonUtility.GetValueFromXML(XmlInfor, "userAccount");
            uProvinceID = CommonUtility.GetValueFromXML(XmlInfor, "uProvinceID");

            status = CommonUtility.GetValueFromXML(XmlInfor, "status");
            realName = CommonUtility.GetValueFromXML(XmlInfor, "realName");
            // cardClass = CommonUtility.GetValueFromXML(XmlInfor, "cardClass");
            credit = CommonUtility.GetValueFromXML(XmlInfor, "credit");
            registration = CommonUtility.GetValueFromXML(XmlInfor, "registration");
            certificateCode = CommonUtility.GetValueFromXML(XmlInfor, "certificateCode");
            certificateType = CommonUtility.GetValueFromXML(XmlInfor, "certificateType");
            birthday = CommonUtility.GetValueFromXML(XmlInfor, "birthday");
            sex = CommonUtility.GetValueFromXML(XmlInfor, "sex");
            custLevel = CommonUtility.GetValueFromXML(XmlInfor, "cardClass");
            custContactTel = CommonUtility.GetValueFromXML(XmlInfor, "custContactTel");
            enterpriseID = CommonUtility.GetValueFromXML(XmlInfor, "enterpriseID");
            extendField = CommonUtility.GetValueFromXML(XmlInfor, "extendField");

            if (CommonUtility.IsEmpty(userType))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + ",不能为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }


            if (ConstDefinition.Span_UserType.IndexOf(userType) < 0)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + "，无该用户类型";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (CommonUtility.IsEmpty(userAccount))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + "，不能为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }


            if (userAccount.Length != 12)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + "，长度有误";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (CommonUtility.IsEmpty(status))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + ",状态不能为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (ConstDefinition.Span_UserStatus.IndexOf(status) < 0)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + ",无此状态类型";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (CommonUtility.IsEmpty(custLevel))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustLevel_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustLevel_Msg + ",客户级别不能为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (ConstDefinition.Span_CustLevel.IndexOf(custLevel) < 0)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustLevel_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustLevel_Msg + ",无此客户级别";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (CommonUtility.IsEmpty(sex))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSex_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidSex_Msg + ",不能为空";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (ConstDefinition.Span_Sex.IndexOf(sex) < 0)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSex_Code.ToString();
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidSex_Msg + "，无此性别定义";
                Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                return Result;
            }

            if (!CommonUtility.IsEmpty(certificateType))
            {
                if (ConstDefinition.Span_CertificateType.IndexOf(certificateType) < 0)
                {

                    Result = ErrorDefinition.BT_IError_Result_InValidCertificateType_Code.ToString();
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidCertificateType_Msg + ",无此证件类型";
                    Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                    return Result;
                }

                if (certificateCode.Length > 20)
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Code.ToString();
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Msg + ",证件长度长度有误";
                    Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
                    return Result;

                }
            }

            string CustID = "";
            Result = BTBizRules.NewCardCustomerInfoExport(strSPID, dealType, userType, userAccount, uProvinceID,
             status, realName, cardClass, credit, registration, certificateCode, certificateType, birthday, sex, custLevel, custContactTel, enterpriseID,
             extendField, out CustID, out  ErrMsg).ToString();

            if (Result == "0")
            {
                //通知积分系统
                //CommonBizRules.CustInfoNotify(CustID, userAccount, "", dealType, "", "", userType,uProvinceID,strSPID);
            }
            Result = BTBizRules.GenerateResultXml(Result, ErrMsg);

        }
        catch (Exception ex)
        {
            Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code.ToString();
            ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            Result = BTBizRules.GenerateResultXml(Result, ErrMsg);
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("客户信息同步接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("ProvinceID - " + strProvinceID);
            msg.Append(";SPID - " + strSPID);
            msg.Append(";dealType - " + dealType);
            msg.Append(";XmlInfor - " + XmlInfor);
            msg.Append(";TimeStamp - " + strTimeStamp + "\r\n");

            msg.Append("; 处理结果 - " + Result + "\r\n");
            msg.Append("\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("NewCardCustomerInfoExport", msg);
            #endregion

        }
            #endregion

        return Result;

    }

    public class IncorporateCustResult
    {
        public int Result;
        public string ErrorDescription;
        public string SavedCustID;
        public string SavedUserAccount;
    }
    [WebMethod(Description = "客户合并接口")]
    public IncorporateCustResult IncorporateCust(string SPID, string IncorporatedCustID, string SavedCustID, string ExtendField)
    {
        IncorporateCustResult Result = new IncorporateCustResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.SavedCustID = SavedCustID;
        Result.ErrorDescription = "";
        Result.SavedUserAccount = "";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(IncorporatedCustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }


            if (CommonUtility.IsEmpty(SavedCustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }

            if (SavedCustID.Equals(IncorporatedCustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，合并号码与别合并号码不能相同";
                return Result;
            }

            #endregion

            Result.Result = BTForBusinessSystemInterfaceRules.IncorporateCust(SPID, IncorporatedCustID, SavedCustID, ExtendField,
                out Result.Result, out Result.ErrorDescription, out Result.SavedCustID, out Result.SavedUserAccount);

        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("帐号合并接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";IncorporatedCustID - " + IncorporatedCustID);
                msg.Append(";SavedCustID - " + SavedCustID);
                msg.Append(";ExtendField - " + ExtendField);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; SavedCustID - " + Result.SavedCustID);
                msg.Append("; SavedUserAccount - " + Result.SavedUserAccount + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("IncorporateCust_BTForScore", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, SavedCustID, IncorporatedCustID, Result.Result, Result.ErrorDescription,
                    Result.SavedCustID, "IncorporateCust_BTForScore");

            }
            catch { }

        }

        return Result;
    }

    public class CustProvinceRelationQueryResult
    {
        public string OuterID;
        public string CustID;
        public string CustAccount;
        public string ExtendField;
    }
    [WebMethod(Description = "省客户ID对应关系查询接口")]
    public CustProvinceRelationQueryResult CustProvinceRelationQuery(string SPID, string OuterID, string ProvinceID, string ExtendField)
    {
        CustProvinceRelationQueryResult Result = new CustProvinceRelationQueryResult();

        Result.ExtendField = "";
        Result.OuterID = "";
        Result.CustID = "";
        Result.CustAccount = "";
        string ErrorDescription = "";
        int result = ErrorDefinition.BT_IError_Result_UnknowError_Code;

        try
        {
            //IP是否允许访问
            result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrorDescription);
            if (result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            result = CommonBizRules.CheckInterfaceLimit(SPID, "CustProvinceRelationQuery", this.Context, out ErrorDescription);
            if (result != 0)
            {
                return Result;
            }

            #region 数据校验

            if (CommonUtility.IsEmpty(SPID))
            {

                result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(OuterID))
            {
                result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，OuterID不能为空";
                return Result;
            }
            if (CommonUtility.IsEmpty(ProvinceID))
            {
                result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }
            #endregion

            result = BTForCrmBizRules.CustProvinceRelationQuery(OuterID, ProvinceID, out Result.CustID, out Result.OuterID, out Result.CustAccount, out ErrorDescription);

        }
        catch (Exception e)
        {
            result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("省客户ID对应关系查询接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";OuterID - " + OuterID);
            msg.Append(";ProvinceID - " + ProvinceID);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + result);
            msg.Append("; 错误描述 - " + ErrorDescription);
            msg.Append("; CustID - " + Result.CustID);
            msg.Append("; OuterID - " + Result.OuterID);
            msg.Append("; UserAccount - " + Result.CustAccount);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("CustProvinceRelationQuery", msg);
            #endregion
        }
        return Result;
    }

    #region 帐号合并接口

    public class IncorporateUserAccountResult
    {
        public string ProvinceID;
        public int Result;
        public string ErrorDescription;
        public string CustID;
        public string UserAccount;
    }
    [WebMethod(Description = "帐号合并接口")]
    public IncorporateUserAccountResult IncorporateUserAccount(string ProvinceID, string SPID,
        UserAccountRecord[] DeleteUserAccountRecords, string IncorporatedCustID, string IncorporatedUserAccount)
    {
        IncorporateUserAccountResult Result = new IncorporateUserAccountResult();
        Result.CustID = IncorporatedCustID;
        Result.UserAccount = IncorporatedUserAccount;
        Result.ProvinceID = "35";
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";

        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(IncorporatedCustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(IncorporatedUserAccount))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，不能为空";
                return Result;
            }

            if (DeleteUserAccountRecords == null)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "DeleteUserAccountRecords为空";
                return Result;
            }

            for (int i = 0; i < DeleteUserAccountRecords.Length; i++)
            {
                if (CommonUtility.IsEmpty(DeleteUserAccountRecords[i].CustID))
                {

                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + ",第" + i.ToString() + "行CustID不能为空";
                    return Result;
                }

                if (CommonUtility.IsEmpty(DeleteUserAccountRecords[i].UserAccount))
                {

                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + ",第" + i.ToString() + "行UserAccount不能为空";
                    return Result;
                }
            }

            Result.Result = BTBizRules.IncorporateUserAccount(ProvinceID, SPID,
                DeleteUserAccountRecords, IncorporatedCustID, IncorporatedUserAccount, out Result.ErrorDescription);


        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("帐号合并接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";IncorporatedCustID - " + IncorporatedCustID);
                msg.Append(";IncorporatedUserAccount - " + IncorporatedUserAccount);
                msg.Append(";DeleteUserAccountRecords: \r\n");
                if (DeleteUserAccountRecords != null)
                {
                    for (int i = 0; i < DeleteUserAccountRecords.Length; i++)
                    {
                        msg.Append(";CustID - " + DeleteUserAccountRecords[i].CustID);
                        msg.Append(";UserAccount - " + DeleteUserAccountRecords[i].UserAccount);
                    }
                    msg.Append("\r\n");
                }

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount + "\r\n");


                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("IncorporateUserAccount", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, IncorporatedCustID, IncorporatedUserAccount, Result.Result,
                    Result.ErrorDescription, "", "IncorporateUserAccount");
            }
            catch { }
        }
        return Result;
    }

    #endregion

    #region 唯一性查询

    public class UniquenessQueryResult
    {
        public string ProvinceID;
        public string SPID;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;

    }

    public UniquenessQueryResult UniquenessQuery(string ProvinceID, string SPID, string TimeStamp, string UserAccount,
        string PhoneNum, string CertificateCode, string CertificateType, string ExtendField)
    {
        UniquenessQueryResult Result = new UniquenessQueryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.SPID = SPID;
        Result.ProvinceID = "35";
        Result.ErrorDescription = "";
        Result.ExtendField = "";


        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，无此provinceid";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，spid 长度有误";
                return Result;
            }

            if (!CommonUtility.IsEmpty(UserAccount))
            {
                if (UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }

                if (UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                    return Result;
                }

            }

            if (!CommonUtility.IsEmpty(PhoneNum))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, PhoneNum, out phone))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                    return Result;
                }
                PhoneNum = phone;
            }


            if (!CommonUtility.IsEmpty(CertificateType))
            {
                if (ConstDefinition.Span_CertificateType.IndexOf(CertificateType) < 0)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCertificateType_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCertificateType_Msg + "，无此CertificateType";
                    return Result;
                }

                if (CommonUtility.IsEmpty(CertificateCode))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Msg + "，不能为空";
                    return Result;
                }
            }

            if (CommonUtility.IsEmpty(UserAccount) & CommonUtility.IsEmpty(PhoneNum) & CommonUtility.IsEmpty(CertificateCode) & CommonUtility.IsEmpty(CertificateType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + ",关键信息不能全部为空";
                return Result;
            }

            Result.Result = BTBizRules.UniquenessQuery(ProvinceID, SPID, UserAccount,
                 PhoneNum, CertificateCode, CertificateType, out Result.ErrorDescription);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("唯一性查询接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("ProvinceID - " + ProvinceID);
            msg.Append(";SPID - " + SPID);
            msg.Append(";UserAccount - " + UserAccount);
            msg.Append(";PhoneNum - " + PhoneNum);
            msg.Append(";CertificateCode - " + CertificateCode);
            msg.Append(";CertificateType - " + CertificateType);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; ProvinceID - " + Result.ProvinceID);
            msg.Append("; SPID - " + Result.SPID);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("UniquenessQuery", msg);
            #endregion
        }

        return Result;

    }

    #endregion

    #endregion

    #region 电话服务接口

    #region 号码绑定接口(刘春利 2009-08-12)
    /// <summary>
    /// 号码绑定接口返回记录
    /// 作者：刘春利    时间：2009-08-12
    /// 修改：          时间：
    /// </summary>
    public class PhoneBindResult
    {
        public string ProvinceID;
        public string CustID;
        public string UserAccount;
        public BoundPhoneRecord[] BoundPhoneRecords;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;

    }
    /// <summary>
    /// 号码绑定接口
    /// 作者：刘春利    时间：2009-08-12
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "电话号码绑定接口")]
    public PhoneBindResult PhoneBind(string ProvinceID, string SPID, string TimeStamp, string CustID, string UserAccount, string PhoneNum)
    {
        PhoneBindResult Result = new PhoneBindResult();

        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ProvinceID = "35";
        Result.CustID = CustID;
        Result.UserAccount = UserAccount;
        Result.ErrorDescription = "";
        Result.BoundPhoneRecords = null;
        Result.ExtendField = "";



        try
        {
            #region 数据校验
            //检查ProvinceID是否为空
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "";
                return Result;
            }

            //检查ProvinceID长度是否有误
            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "";
                return Result;
            }

            //检查指定属性是否为空
            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",无此provinceid";
                return Result;
            }

            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            //检查SPID长度是否有误
            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "PhoneBind", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //检查CustID是否为空
            if (CommonUtility.IsEmpty(CustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }

            //检查UserAccount是否为空
            if (CommonUtility.IsEmpty(UserAccount))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，不能为空";
                return Result;
            }

            //检查UserAccount长度是否有误
            if (UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }

            //UserAccount
            if (UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }
            string phone = "";
            //验证电话号码有效性
            if (!CommonBizRules.PhoneNumValid(this.Context, PhoneNum, out phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                return Result;
            }
            PhoneNum = phone;
            #endregion

            Result.Result = BTBizRules.BindPhone(SPID, CustID, PhoneNum, out Result.ErrorDescription);

            string ErrMsg = "";
            int QueryResult = -1;
            Result.BoundPhoneRecords = BTBizRules.GetBoundPhone(CustID, UserAccount, out QueryResult, out ErrMsg);
            Result.ErrorDescription += ErrMsg;

            if (QueryResult != 0 & Result.Result == 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            }
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("电话号码绑定接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append(";CustID - " + CustID);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                if (Result.BoundPhoneRecords != null)
                {
                    if (Result.BoundPhoneRecords.Length > 0)
                    {
                        msg.Append("BoundPhoneRecords: \r\n");
                        for (int i = 0; i < Result.BoundPhoneRecords.Length; i++)
                        {
                            msg.Append("Phone- " + Result.BoundPhoneRecords[i].Phone);
                        }
                    }
                }
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PhoneBind", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, CustID, UserAccount, Result.Result,
                    Result.ErrorDescription, PhoneNum, "PhoneBind");
            }
            catch { }
        }


        return Result;

    }
    #endregion

    #region 电话号码解绑接口(刘春利 2009-08-12)
    /// <summary>
    /// 电话号码解绑接口返回记录
    /// 作者：刘春利    时间：2009-08-12
    /// 修改：          时间：
    /// </summary>
    public class PhoneUnBindReslut
    {
        public string ProvinceID;
        public string CustID;
        public string UserAccount;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;
    }

    /// <summary>
    /// 电话号码解绑接口
    /// 作者：刘春利    时间：2009-08-12
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "电话号码解绑接口")]
    public PhoneUnBindReslut PhoneUnBind(string ProvinceID, string SPID, string TimeStamp, string PhoneNum)
    {
        PhoneUnBindReslut Result = new PhoneUnBindReslut();
        Result.Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ProvinceID = "35";
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查ProvinceID是否为空
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
                return Result;
            }

            //检查ProvinceID长度是否有误
            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
                return Result;
            }

            //检查指定属性是否为空
            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
                return Result;
            }

            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg;
                return Result;
            }

            //检查SPID长度是否有误
            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg;
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "PhoneUnBind", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            string phone = "";
            //验证电话号码有效性
            if (!CommonBizRules.PhoneNumValid(this.Context, PhoneNum, out phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                return Result;
            }
            PhoneNum = phone;
            #endregion

            Result.Result = BTBizRules.UnBindPhone(PhoneNum, out Result.ErrorDescription, out Result.CustID, out Result.UserAccount);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("电话号码解绑接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PhoneUnBind", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, Result.CustID, Result.UserAccount, Result.Result,
                    Result.ErrorDescription, PhoneNum, "PhoneUnBind");
            }
            catch { }
        }

        return Result;
    }
    #endregion

    #region 电话绑定查询接口
    public class PhoneBindQueryResult
    {
        public string ProvinceID;
        public string CustID;
        public string UserAccount;
        public string RealName;
        public int Result;
        public string ErrorDescription;

    }
    [WebMethod(Description = "电话绑定查询接口")]
    public PhoneBindQueryResult PhoneBindQuery(string ProvinceID, string SPID, string PhoneNum)
    {
        PhoneBindQueryResult Result = new PhoneBindQueryResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.CustID = "";
        Result.UserAccount = "";
        Result.RealName = "";
        Result.ProvinceID = "35";

        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",无此provinceid";
                return Result;
            }


            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }


            if (CommonUtility.IsEmpty(PhoneNum))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }

            Result.Result = BTBizRules.BindPhoneQuery(SPID, PhoneNum, out Result.CustID, out Result.UserAccount, out Result.RealName, out Result.ErrorDescription);
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ex.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("认证方式查询接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; RealName - " + Result.RealName);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PhoneBindQuery", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, "", "", Result.Result,
                    Result.ErrorDescription, PhoneNum, "PhoneBindQuery");
            }
            catch { }

        }


        return Result;


    }

    #endregion

    #endregion

    #region 密码服务接口

    #region 密码重置接口  苑峰

    public class PasswordResetResult
    {
        public string ProvinceID;
        public string CustID;
        public string UserAccount;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "密码重置接口")]
    public PasswordResetResult PasswordReset(string ProvinceID, string SPID, string TimeStamp, string CustID, string UserAccount, string NewPassword, string OPType)
    {
        PasswordResetResult Result = new PasswordResetResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.CustID = CustID;
        Result.UserAccount = UserAccount;
        Result.ErrorDescription = "";
        Result.ProvinceID = "35";
        Result.ExtendField = "";
        Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
        if (Result.Result != 0)
        {
            return Result;
        }

        //接口访问权限判断
        Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "PasswordReset", this.Context, out Result.ErrorDescription);
        if (Result.Result != 0)
        {
            return Result;
        }
        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，无效的省标识";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }
            if (OPType != "0" & OPType != "1")
            {

                Result.Result = ErrorDefinition.IError_Result_InvalidRequestData_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_InvalidRequestData_Msg + "，操作类型不对";
                return Result;
            }

            if (CommonUtility.IsEmpty(UserAccount))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，不能为空";
                return Result;
            }

            if (UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }
            if (UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }
            if (OPType == "0") //如果是修改密码则密码不能为空
            {
                if (CommonUtility.IsEmpty(NewPassword))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                    return Result;
                }

                if (NewPassword.Length < ConstDefinition.Length_Min_Password)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + ",密码必须大于6位";
                    return Result;
                }
            }

            //如果是密码重置则随即分配密码
            if (OPType == "1")
            {
                Random rd = new Random();

                NewPassword = rd.Next(111111, 999999).ToString();
            }

            string ContactTel = "";
            string Email = "";
            string RealName = "";
            Result.Result = BTBizRules.PasswordReset(SPID, CustID, UserAccount, NewPassword, out Result.ErrorDescription, out ContactTel, out Email, out RealName);


            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");

            //如果是密码重置则发短信或发邮件通知用户
            if (OPType == "1")
            {
                msg.Append("客户信息平台密码重置短信接口测试准备调用 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(ContactTel + "尊敬的" + RealName + "您好，您的密码已成功修改,您的卡号:" + UserAccount + "密码：" + NewPassword + ".请您牢记您的密码");
                msg.Append("\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("PasswordReset", msg);

                CommonBizRules.SendMessage(ContactTel, "尊敬的" + RealName + "您好，您的密码已成功修改,您的卡号:" + UserAccount + "密码：" + NewPassword + ".请您牢记您的密码", "35000000");
            }
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("密码重置接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("ProvinceID - " + ProvinceID);
                msg.Append(";SPID - " + SPID);
                msg.Append(";UserAccount - " + UserAccount);
                msg.Append(";CustID - " + CustID);
                msg.Append(";NewPassword - " + NewPassword);
                msg.Append(";OPType - " + OPType);
                msg.Append(";TimeStamp - " + TimeStamp + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ProvinceID - " + Result.ProvinceID);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PasswordReset", msg);
                #endregion

                CommonBizRules.WriteDataLog(SPID, CustID, UserAccount, Result.Result,
                    Result.ErrorDescription, "", "PasswordReset");
            }
            catch { }
        }
        return Result;
    }
    
    #endregion

    #region 密码问题查询接口

    public class PwdQuestionQueryResult
    {
        public int QuestionID;
        public string Question;
        public string ExtendField;
    }
    [WebMethod(Description = "密码问题查询接口")]
    public int PwdQuestionQuery(string SPID, string ExtendField, out PwdQuestionQueryResult[] PwdResults, out int iResult, out string ErrorDescription)
    {
        PwdResults = null;      
        iResult = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;

        try
        {
            #region
            if (CommonUtility.IsEmpty(SPID))
            {

                iResult = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return iResult;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                iResult = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return iResult;
            }
            #endregion

            DataTable dt = new DataTable();
            iResult = BTForBusinessSystemInterfaceRules.PwdQuestionQuery(out dt, out ErrorDescription);
            if (iResult == 0)
            {
                int count = dt.Rows.Count;
                PwdResults = new PwdQuestionQueryResult[count];
                for (int i = 0; i < count; i++)
                {
                    PwdQuestionQueryResult PwdResult = new PwdQuestionQueryResult();
                    PwdResult.ExtendField = "";
                    PwdResult.QuestionID = int.Parse(dt.Rows[i]["QuestionID"].ToString());
                    PwdResult.Question = dt.Rows[i]["Question"].ToString();
                    PwdResults[i] = PwdResult;
                }
            }
            return iResult;
        }
        catch (Exception ex)
        {
            iResult = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = ex.Message.ToString();
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("密码问题查询接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + iResult);
            msg.Append("; 错误描述 - " + ErrorDescription);

            for (int i = 0; i < PwdResults.Length; i++)
            {
                msg.Append("; QuestionID - " + PwdResults[i].QuestionID);
                msg.Append("; Question - " + PwdResults[i].Question);
                msg.Append("; ExtendField - " + PwdResults[i].ExtendField + "\r\n");
            }
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("PwdQuestionQuery", msg);
            #endregion
        }

        return iResult;
    }


    #endregion

    #region 密码提示问题上传接口

    public class PwdQuestionUploadResult
    {
        public int QuestionID;
        public string Answer;
        public string ExtendField;
    }
    [WebMethod(Description = "密码提示问题上传接口")]
    public int PwdQuestionUpload(string SPID, string CustID, PwdQuestionUploadResult[] PwdQARecords, string ExtendField, out string OutCustID, 
                                        out string ErrorDescription, out string OutExtendField)
    {
        OutCustID = "";
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        OutExtendField = "";

        try
        {
            #region
            if (CommonUtility.IsEmpty(SPID))
            {

                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }

            if (PwdQARecords == null)
            {
                Result = ErrorDefinition.IError_Result_UnknowError_Code;
                ErrorDescription = "PwdQARecords为null值";
                return Result;
            }
            #endregion
            int count = PwdQARecords.Length;
            string[] Answer = new string[count];
            int[] QuestionID = new int[count];

            for (int i = 0; i < count; i++)
            {
                Answer[i] = PwdQARecords[i].Answer;
                QuestionID[i] = PwdQARecords[i].QuestionID;
            }

            Result = BTForBusinessSystemInterfaceRules.PwdQuestionUpload(CustID, QuestionID, Answer, out ErrorDescription);
            OutCustID = CustID;
        }
        catch (Exception err)
        {
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = err.Message.ToString(); ;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("密码问题上传接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";CustID - " + CustID);
            if (PwdQARecords != null)
            {
                for (int i = 0; i < PwdQARecords.Length; i++)
                {
                    msg.Append(";QuestionID" + i + " - " + PwdQARecords[i].QuestionID);
                    msg.Append(";Answer " + i + " - " + PwdQARecords[i].Answer);
                    msg.Append(";ExtendField " + i + " - " + PwdQARecords[i].ExtendField + "\r\n");
                }
            }
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result);
            msg.Append("; 错误描述 - " + ErrorDescription);
            msg.Append("CustID - " + OutCustID);
            msg.Append("ExtendField - " + OutExtendField);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("PwdQuestionUpload", msg);
            #endregion
        }

        return Result;
    }
    #endregion

    #region 密码提示问题验证接口

    [WebMethod(Description = "密码提示问题验证接口")]
    public int PwdQuestionAuth(string SPID, string CustID, int QuestionID, string Answer, string ExtendField, out string outCustID, out string ErrorDescription, out string OutExtendField)
    {
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        OutExtendField = "";
        outCustID = "";
        try
        {
            #region
            if (CommonUtility.IsEmpty(SPID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }
            if (QuestionID == 0)
            {
                Result = ErrorDefinition.IError_Result_UnknowError_Code;
                ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg + "，QuestionID不能为空";
                return Result;
            }
            if (CommonUtility.IsEmpty(Answer))
            {
                Result = ErrorDefinition.IError_Result_UnknowError_Code;
                ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg + "，Answer不能为空";
                return Result;
            }
            #endregion

            Result = BTForBusinessSystemInterfaceRules.PwdQuestionAuth(CustID, QuestionID, Answer, out ErrorDescription);
            outCustID = CustID;
        }
        catch (Exception err)
        {
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = err.Message.ToString(); ;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("密码提示问题验证接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";CustID - " + CustID);

            msg.Append(";QuestionID - " + QuestionID);
            msg.Append(";Answer - " + Answer);
            msg.Append(";ExtendField - " + ExtendField + "\r\n");


            msg.Append("处理结果 - " + Result);
            msg.Append("; 错误描述 - " + ErrorDescription);
            msg.Append("CustID - " + outCustID);
            msg.Append("ExtendField - " + OutExtendField);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("PwdQuestionAuth", msg);
            #endregion
        }

        return Result;
    }

    #endregion

    #region 密码获取接口

    public class GetPasswordResult
    {
        public int Result;
        public string Pwd;
        public string ErrMsg;
        public string ExtendField;
    }
    [WebMethod(Description = "密码获取接口")]
    public GetPasswordResult GetPassword(string SPID, string CustID, string ExtendField)
    {
        GetPasswordResult Result = new GetPasswordResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = "init";
        Result.Pwd = "";
        try
        {
            if (CommonUtility.IsEmpty(CustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }

            Result.Result = BTBizRules.GetPassword(CustID, out Result.Pwd, out Result.ErrMsg);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
            Result.ErrMsg = e.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("获取用户密码接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";CustID - " + CustID);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrMsg);
            msg.Append("; Pwd- " + Result.Pwd);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("GetPassword", msg);
            #endregion
        }

        return Result;
    }

    #endregion

    #region 个人密码问题查询接口
    public class UserPwdQuestionQueryResult
    {
        public int QuestionID;
        public string Question;
        public string ExtendField;
    }
    [WebMethod(Description = "个人密码问题查询接口")]
    public int UserPwdQuestionQuery(string SPID, string CustID, string ExtendField, out string OutCustID, out UserPwdQuestionQueryResult[] UserResults, 
                out string ErrorDescription, out string OutExtendField)
    {

        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        UserResults = new UserPwdQuestionQueryResult[0];
        OutExtendField = "";
        OutCustID = "";


        try
        {
            #region
            if (CommonUtility.IsEmpty(SPID))
            {

                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }
            #endregion

            DataTable dt = new DataTable();
            Result = BTForBusinessSystemInterfaceRules.UserPwdQuestionQuery(CustID, out dt, out ErrorDescription);


            if (Result == 0)
            {
                int count = dt.Rows.Count;
                UserResults = new UserPwdQuestionQueryResult[count];
                for (int i = 0; i < count; i++)
                {
                    UserPwdQuestionQueryResult PwdResult = new UserPwdQuestionQueryResult();
                    PwdResult.ExtendField = "";
                    PwdResult.QuestionID = int.Parse(dt.Rows[i]["QuestionID"].ToString());
                    PwdResult.Question = dt.Rows[i]["Question"].ToString();
                    UserResults[i] = PwdResult;
                }
            }

            return Result;
        }
        catch (Exception ex)
        {
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = ex.Message.ToString();
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("个人密码问题查询接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";CustID - " + CustID);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result);
            msg.Append("; 错误描述 - " + ErrorDescription);
            msg.Append("\r\n");

            for (int i = 0; i < UserResults.Length; i++)
            {
                msg.Append("; QuestionID - " + UserResults[i].QuestionID);
                msg.Append("; Question - " + UserResults[i].Question);
                msg.Append("; ExtendField - " + UserResults[i].ExtendField + "\r\n");
            }
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("UserPwdQuestionQuery", msg);
            #endregion
        }

        return Result;
    }

    #endregion

    #endregion

    #region 无用的接口

    #region 【无用的接口】客户升级请求接口
    /*
    public class CustLevelChangeResult
    {
        public string ProvinceID;
        public int Result;
        public string CustLevel;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "客户升级请求接口")]
    public CustLevelChangeResult CustLevelChange(string ProvinceID, string SPID, string CustID, string UserAccount)
    {
        CustLevelChangeResult Result = new CustLevelChangeResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        Result.ProvinceID = "35";
        Result.CustLevel = "";
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，无此省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }


            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }


            //if (CustID.Length != ConstDefinition.Length_CustID)
            //{

            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，长度有误";
            //    return Result;
            //}

            if (CommonUtility.IsEmpty(UserAccount))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，不能为空";
                return Result;
            }

            if (UserAccount.Length < ConstDefinition.Length_Min_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }

            if (UserAccount.Length > ConstDefinition.Length_Max_UserAccount)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                return Result;
            }

            //if (UserAccount.Length != ConstDefinition.Length_UserAccount)
            //{

            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
            //    return Result;
            //}
            #endregion

            Result.Result = BtForUnifyInterfaceBizRules.CustLevelChange(ProvinceID, SPID, CustID, UserAccount, out Result.CustLevel, out Result.ErrorDescription);

            //如果升级成功通知积分系统.并通知用户
            if (Result.Result == 0)
            {
                string realName = Result.ErrorDescription;
                string msg = "";

                if (Result.CustLevel == "1")
                    msg = "尊敬的" + realName + "，祝贺您成为号百商旅钻石卡客户，感谢使用号百商旅服务！";
                else if (Result.CustLevel == "2")
                    msg = "尊敬的" + realName + "，祝贺您成为号百商旅铂金卡客户，感谢使用号百商旅服务！";

                //通知积分系统
                CommonBizRules.CustInfoNotify(CustID, UserAccount, "", "1","","");
                //通过手机或Email向用户发送信息








                BtForUnifyInterfaceBizRules.SendMessage(CustID, msg, "35000000");
            }


        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
        }
        finally
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("客户升级请求接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("ProvinceID - " + ProvinceID);
            msg.Append(";SPID - " + SPID);
            msg.Append(";CustID - " + CustID);
            msg.Append(";UserAccount - " + UserAccount);
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("CustLevelChange", msg);
        }

        return Result;

    }
     */
    #endregion

    #region 【无用的接口】用户状态变更接口
    /*
    public class CustStatusChangeResult
    {
        public string ProvinceID;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;

    }

    [WebMethod(Description = "用户状态变更接口")]
    public CustStatusChangeResult CustStatusChange(string ProvinceID,
        string SPID, string CustID, string Status, string Description)
    {
        CustStatusChangeResult Result = new CustStatusChangeResult();

        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        Result.ProvinceID = "35";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，无此省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }


            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }


            //if (CustID.Length != ConstDefinition.Length_CustID)
            //{

            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，长度有误";
            //    return Result;
            //}

            if (CommonUtility.IsEmpty(Status))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + "，不能为空";
                return Result;
            }

            if (Status.Length != ConstDefinition.Length_CustStatus)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + ",长度有误";
                return Result;
            }

            if (ConstDefinition.Span_UserStatus.IndexOf(Status) < 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + "，无此客户状态";
                return Result;
            }

            if (Description.Length > ConstDefinition.MaxLength_CustStatusChangeDescription)
            {
                Result.Result = ErrorDefinition.IError_Result_InValidDescription_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_InValidDescription_Msg + "，长度有误";
                return Result;
            }

            #endregion

            string RealName = "";
            Result.Result = BtForUnifyInterfaceBizRules.CustStatusChange(ProvinceID, SPID, CustID, Status, Description, out Result.ErrorDescription, out RealName);

            //如果升级成功通知积分系统
            if (Result.Result == 0)
            {
                CommonBizRules.CustInfoNotify(CustID, "", "", "1","","");
                string Message = "";
                if(Status == "01" )
                    Message = "尊敬的" + RealName + "，您的商旅卡已冻结成功，如需解冻请在15个工作日内拨打114/118114进行修改。冻结期间您将不能享受商旅卡的各项服务。";
                else if(Status == "00" )
                    Message = "尊敬的" + RealName + "，您的商旅卡已解冻成功，如有任何疑问请你拨打114/118114进行咨询。";
                else if(Status == "03")
                    Message=  "尊敬的" + RealName + "，您的商旅卡已被注销，卡内积分从注销时起全部清零，如有任何疑问请你拨打号码百事通服务热线114/118114进行咨询。";
                
                BtForUnifyInterfaceBizRules.SendMessage(CustID, Message, "35000000");
            }


        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("用户状态变更接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("ProvinceID - " + ProvinceID);
            msg.Append(";SPID - " + SPID);
            msg.Append(":CustID - " + CustID);
            msg.Append(";Status - " + Status);
            msg.Append(";Description- " + Description + "\r\n");
            msg.Append(":处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("CustStatusChange", msg);
            #endregion

            //写数据库日志
            try
            {
                Description = Description + "变更后状态为：" + Status;
                if (Result.Result != 0)
                {
                    Description = Result.ErrorDescription;
                }
                CommonBizRules.WriteDataLog(SPID, CustID, "", Result.Result,
                   Description, "", "CustStatusChange");
            }
            catch { }
        }

        return Result;
    }
    */
    #endregion

    #region 【无用的接口】积分等级变更通知接口
    /*
    public class IntegralTigerGradeResult
    {
        public string ProvinceID;
        public int Result;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "积分等级变更通知接口")]
    public IntegralTigerGradeResult IntegralTigerGrade(string ProvinceID, string SPID, string IntegralInfo,
        string GradeOrCredit, string UpgradeOrFall, string CustID, string TimeStamp)
    {
        IntegralTigerGradeResult Result = new IntegralTigerGradeResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        Result.ProvinceID = "35";
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            ProvinceInfoManager proObject = new ProvinceInfoManager();
            object proDataObject = proObject.GetProvinceData(this.Context);
            if (proObject.GetPropertyByProvinceID(ProvinceID, "ProvinceCode", proDataObject) == "")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，无此省代码";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(IntegralInfo))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，积分值不能为空";
                return Result;
            }

            if (IntegralInfo.Length > ConstDefinition.Length_IntegralInfo)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，积分长度有误";
                return Result;
            }

            if (GradeOrCredit == "01")
            {
                if (UpgradeOrFall.Length != ConstDefinition.Length_CustLevel)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，等级信用选择长度有误";
                    return Result;
                }

                if (ConstDefinition.Span_CustLevel.IndexOf(UpgradeOrFall) < 0)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，等级信用选择无效";
                    return Result;
                }
            }
            else if (GradeOrCredit == "02")
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，暂不支持信用";
                return Result;
            }
            else
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，无效等级信用";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }

            //if (CustID.Length != ConstDefinition.Length_CustID)
            //{

            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，长度有误";
            //    return Result;
            //}

            #endregion

            Result.Result = BtForUnifyInterfaceBizRules.IntegralTigerGrade(ProvinceID, SPID, IntegralInfo, GradeOrCredit, UpgradeOrFall, CustID, out Result.ErrorDescription);

            if (Result.Result == 0)
            {
                // 发短信通知客户

                string RealName = Result.ErrorDescription;
                string msg = "";
                if (UpgradeOrFall == "1")
                    msg = "尊敬的" + RealName + "，您已经满足升级至号百商旅钻石卡客户的条件，请拨打118114完成升级。";
                else if (UpgradeOrFall == "2")
                    msg = "尊敬的" + RealName + "，您已经满足升级至号百商旅铂金卡客户的条件，请拨打118114完成升级。";

                BtForUnifyInterfaceBizRules.SendMessage(CustID, msg, "35000000");

            }
            else if (Result.Result == 1)
            {
                //降级 暂不支持
            }
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
        }
        finally
        {
            #region WriteLog

            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("客户积分等级变更通知接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("ProvinceID - " + ProvinceID);
            msg.Append(";SPID - " + SPID);
            msg.Append(";IntegralInfo - " + IntegralInfo);
            msg.Append(";GradeOrCredit - " + GradeOrCredit);
            msg.Append(";UpgradeOrFall - " + UpgradeOrFall);
            msg.Append(";CustID - " + CustID);
            msg.Append(";TimeStamp - " + TimeStamp + "\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("IntegralTigerGrade", msg);
            #endregion
        }

        return Result;
    }
     */
    #endregion

    #endregion


}


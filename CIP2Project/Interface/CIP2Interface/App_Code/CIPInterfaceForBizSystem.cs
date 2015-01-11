using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Text;
using System.Collections.Generic;
using Linkage.BestTone.Interface.Rule.Young.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
/// <summary>
/// CIPInterfaceForBizSystem 的摘要说明
/// </summary>
[WebService(Namespace = "http://BestToneUserCenter.vnet.cn")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CIPInterfaceForBizSystem : System.Web.Services.WebService
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(CIPInterfaceForBizSystem));
    public CIPInterfaceForBizSystem()
    {
        
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    //#region 注册认证

    //public class UserRegistryV2Result
    //{
    //    public string SPID;
    //    public string TimeStamp;
    //    public string CustID;
    //    public string TourCardID;
    //    public int Result;
    //    public string ErrMsg;
    //}
    //[WebMethod(Description = "注册接口（soap）")]
    //public UserRegistryV2Result UserRegistryV2(string SPID, string UserType, string CardID, string Password, string UProvinceID, string AreaCode,
    //                                           string RealName, string UserName, string AuthenPhone, string ContactTel, string IsNeedTourCard,
    //                                           string CertificateCode, string CertificateType, string Sex, string ExtendField)
    //{
    //    UserRegistryV2Result Result = new UserRegistryV2Result();
    //    string PwdType = "";

    //    Result.SPID = SPID;
    //    Result.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //    Result.CustID = "";
    //    //商旅卡9位
    //    Result.TourCardID = "";
    //    Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
    //    Result.ErrMsg = "";

    //    //商旅卡16位
    //    string sCardID = "";

    //    try
    //    {
    //        #region 数据合法性判断
    //        if (CommonUtility.IsEmpty(SPID))
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
    //            return Result;
    //        }

    //        if (SPID.Length != ConstDefinition.Length_SPID)
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
    //            return Result;
    //        }

    //        //IP是否允许访问
    //        Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
    //        if (Result.Result != 0)
    //        {
    //            return Result;
    //        }

    //        //接口访问权限判断
    //        Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserRegistryV2", this.Context, out Result.ErrMsg);
    //        if (Result.Result != 0)
    //        {
    //            return Result;
    //        }

    //        if (CommonUtility.IsEmpty(UserType))
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
    //            return Result;
    //        }

    //        string VoicePwdSPID = System.Configuration.ConfigurationManager.AppSettings["VoicePwd_SPID"];

    //        int SIP1 = VoicePwdSPID.IndexOf(SPID);
    //        if (SIP1 < 0)
    //        {
    //            if (CommonUtility.IsEmpty(Password))
    //            {
    //                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，密码不能为空";
    //                return Result;
    //            }

    //            if (!CommonUtility.IsEmpty(ExtendField))
    //            {
    //                PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");

    //            }



    //            if (!PwdType.Equals("2"))
    //            {
    //                if (Password.Length != ConstDefinition.Length_Min_Password)
    //                {
    //                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
    //                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "密码长度有误";
    //                    return Result;
    //                }
    //            }

    //            if (!CommonUtility.IsNumeric(Password) && !PwdType.Equals("2"))
    //            {
    //                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
    //                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "密码只能是数字";
    //                return Result;
    //            }
    //        }
    //        else
    //        {
    //            if (CommonUtility.IsEmpty(Password))
    //            {
    //                if (CommonUtility.IsEmpty(AuthenPhone) && CommonUtility.IsEmpty(ContactTel))
    //                {
    //                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
    //                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "认证电话和联系电话不能同时为空";
    //                    return Result;
    //                }
    //                Random rd = new Random();
    //                Password = rd.Next(111111, 999999).ToString();
    //            }

    //        }

    //        if (CommonUtility.IsEmpty(UProvinceID))
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
    //            return Result;
    //        }

    //        if (CommonUtility.IsEmpty(AreaCode))
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
    //            return Result;
    //        }

    //        if (CommonUtility.IsEmpty(RealName))
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
    //            return Result;
    //        }

    //        string UserNameSPID = System.Configuration.ConfigurationManager.AppSettings["UserName_SPID"];

    //        int SIP = UserNameSPID.IndexOf(SPID);
    //        if (SIP < 0)
    //        {
    //            if (CommonUtility.IsEmpty(UserName))
    //            {
    //                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，用户名不能为空";
    //                return Result;
    //            }
    //        }

    //        if (!CommonUtility.IsEmpty(AuthenPhone))
    //        {
    //            string phone = "";
    //            if (!CommonBizRules.PhoneNumValid(this.Context, AuthenPhone, out phone))
    //            {
    //                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
    //                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
    //                return Result;
    //            }
    //            string T_CustID = "";
    //            if (!CommonBizRules.HasBesttoneAccount(this.Context, AuthenPhone, out T_CustID, out Result.ErrMsg))
    //            {
    //                if (!String.IsNullOrEmpty(T_CustID))
    //                {
    //                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
    //                    Result.ErrMsg = "该手机号码已经被别的客户作为号码百事通账户";
    //                    return Result;
    //                }

    //            }
    //            AuthenPhone = phone;
    //        }


    //        if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(AuthenPhone))
    //        {
    //            if (UserName.Equals(AuthenPhone))
    //            {
    //                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
    //                Result.ErrMsg = "用户名不能和认证手机不能相同";
    //                return Result;
    //            }
    //        }

    //        if (!CommonUtility.IsEmpty(ContactTel))
    //        {
    //            string phone = "";
    //            if (!CommonBizRules.PhoneNumValid(this.Context, ContactTel, out phone))
    //            {
    //                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
    //                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
    //                return Result;
    //            }
    //            ContactTel = phone;
    //        }

    //        if (CommonUtility.IsEmpty(IsNeedTourCard))
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
    //            return Result;
    //        }

    //        if (CommonUtility.IsEmpty(Sex))
    //        {
    //            Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
    //            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
    //            return Result;
    //        }
    //        #endregion

    //        //数据库操作
    //        Result.Result = UserRegistry.getUserRegistry(SPID, UserType, CardID, Password, UProvinceID, AreaCode, RealName,
    //                                     UserName, AuthenPhone, ContactTel, IsNeedTourCard, CertificateCode, CertificateType,
    //                                     Sex, ExtendField, out Result.ErrMsg, out Result.CustID, out Result.TourCardID, out sCardID);

    //        if (Result.Result == 0)
    //        {
    //            //记录注册来源ip
    //            CommonBizRules.WriteTraceIpLog(Result.CustID, UserName, Result.SPID, HttpContext.Current.Request.UserHostAddress.ToString(), "jk_zc");
    //            //通知积分平台
    //            CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrMsg);

    //            string Message = "";
    //            string phone = CommonUtility.IsEmpty(AuthenPhone) ? ContactTel : AuthenPhone;

    //            if (SIP1 >= 0)
    //            {

    //                Message = "恭喜您成为号码百事通会员！请妥善保管您的密码；如需帮助请联系：4008-118114。";
    //                //通知短信网关
    //                //CommonBizRules.SendMessageV3(phone,Message,SPID);
    //            }

    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
    //        Result.ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
    //    }
    //    finally
    //    {
    //        try
    //        {
    //            #region 文本日志
    //            StringBuilder msg = new StringBuilder();
    //            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
    //            msg.Append("注册接口（soap）" + DateTime.Now.ToString("u") + "\r\n");
    //            msg.Append(";SPID - " + SPID);
    //            msg.Append(":UserType - " + UserType);
    //            msg.Append(":CardID - " + CardID);
    //            msg.Append(":Password - " + Password);
    //            msg.Append(":UProvinceID - " + UProvinceID);
    //            msg.Append(":AreaCode - " + AreaCode);
    //            msg.Append(":RealName - " + RealName);
    //            msg.Append(":UserName - " + UserName);
    //            msg.Append(":AuthenPhone - " + AuthenPhone);
    //            msg.Append(":ContactTel - " + ContactTel);
    //            msg.Append(":IsNeedTourCard - " + IsNeedTourCard);
    //            msg.Append(":CertificateCode - " + CertificateCode);
    //            msg.Append(":CertificateType - " + CertificateType);
    //            msg.Append(":Sex - " + Sex);
    //            msg.Append(":IP - " + HttpContext.Current.Request.UserHostAddress);
    //            msg.Append(";ExtendField - " + ExtendField + "\r\n");

    //            msg.Append("处理结果 - " + Result.Result);
    //            msg.Append("; 错误描述 - " + Result.ErrMsg);
    //            msg.Append(": SPID - " + Result.SPID);
    //            msg.Append(": TimeStamp - " + Result.TimeStamp);
    //            msg.Append("; CustID - " + Result.CustID);
    //            msg.Append(": sCardID - " + sCardID);
    //            msg.Append(": password - " + Password);
    //            msg.Append("; TourCardID - " + Result.TourCardID + "\r\n");

    //            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

    //            BTUCenterInterfaceLog.CenterForBizTourLog("UserRegistryV2", msg);
    //            #endregion

    //            #region 数据库日志
    //            StringBuilder inParam = new StringBuilder();
    //            inParam.Append("SPID:" + SPID);
    //            inParam.Append(",UserType:" + UserType);
    //            inParam.Append(",CardID:" + CardID);
    //            inParam.Append(",Password:" + Password);
    //            inParam.Append(",UProvinceID:" + UProvinceID);
    //            inParam.Append(",AreaCode:" + AreaCode);
    //            inParam.Append(",RealName:" + RealName);
    //            inParam.Append(",UserName:" + UserName);
    //            inParam.Append(",AuthenPhone:" + AuthenPhone);
    //            inParam.Append(",ContactTel:" + ContactTel);
    //            inParam.Append(",IsNeedTourCard:" + IsNeedTourCard);
    //            inParam.Append(",CertificateCode:" + CertificateCode);
    //            inParam.Append(",CertificateType:" + CertificateType);
    //            inParam.Append(",Sex:" + Sex);
    //            inParam.Append(",ExtendField:" + ExtendField);

    //            String outParam = String.Format("SPID:{0},TimeStamp:{1},CustID:{2},TourCardID:{3},Result:{4},ErrMsg:{5}",
    //                            Result.SPID, Result.TimeStamp, Result.CustID, Result.TourCardID, Result.Result, Result.ErrMsg);
    //            CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "UserRegistryV2", inParam.ToString(), outParam, Result.Result, Result.ErrMsg);
    //            #endregion
    //        }
    //        catch { }
    //    }

    //    return Result;
    //}

    //#endregion

    public class GetUserInfoByNameResult
    {
        public string userId;
        public string custId;
        public int Result;
        public string ErrMsg;
    }
    [WebMethod(Description = "综合平台userName反查userId接口（soap）")]
    public GetUserInfoByNameResult UnifyGetUserInfoByName(string SPID, string UserName)
    {
        GetUserInfoByNameResult Result = new GetUserInfoByNameResult();
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("{0}\r\n", DateTime.Now.ToString("u"));
        strLog.AppendFormat("参数:SPID:{0},UserName:{1}\r\n", SPID, UserName);
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = "";
        Result.userId = "";
        Result.custId = "";
        if (CommonUtility.IsEmpty(SPID))
        {
            Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
            strLog.Append(Result.ErrMsg + "\r\n");
            return Result;
        }

        if (SPID.Length != ConstDefinition.Length_SPID)
        {
            Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
            strLog.Append(Result.ErrMsg + "\r\n");
            return Result;
        }
        String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
        String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
        String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
        String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
        String clientIp = HttpContext.Current.Request.UserHostAddress;
        String clientAgent = HttpContext.Current.Request.UserAgent;
        try
        {
            UDBMBOSS _udbBoss = new UDBMBOSS();
            _udbBoss.GetUserInfoByName(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, out Result.Result, out Result.userId, out Result.ErrMsg);
            strLog.AppendFormat("结果:{0},{1},{2}\r\n", Result.Result, Result.ErrMsg,Result.userId);
            if (Result.Result == 0)
            {
                Result.Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", UserName, "", "", "", Convert.ToInt64(Result.userId), SPID, "2", out Result.custId, out Result.ErrMsg);
            }
        }
        catch (Exception e)
        {
            Result.ErrMsg = e.Message;
            strLog.AppendFormat("异常:{0}\r\n", e.Message);
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("UnifyGetUserInfoByName", strLog);
        }
        return Result;
    }

    public class IsUserNameExistResult
    {
        public string isExist;
        public int Result;
        public string  ErrMsg;
    }
    [WebMethod(Description = "综合平台用户名是否存在接口（soap）")]
    public IsUserNameExistResult IsUserNameExist(string SPID, string UserName)
    {
        IsUserNameExistResult Result = new IsUserNameExistResult();
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("{0}\r\n",DateTime.Now.ToString("u"));
        strLog.AppendFormat("参数:SPID:{0},UserName:{1}\r\n",SPID,UserName);
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = "";
        Result.isExist = "";
        if (CommonUtility.IsEmpty(SPID))
        {
            Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
            strLog.Append(Result.ErrMsg + "\r\n");
            return Result;
        }

        if (SPID.Length != ConstDefinition.Length_SPID)
        {
            Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
            Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
            strLog.Append(Result.ErrMsg + "\r\n");
            return Result;
        }
        String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
        String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
        String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
        String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
        String clientIp = HttpContext.Current.Request.UserHostAddress;
        String clientAgent = HttpContext.Current.Request.UserAgent;
        try
        {
            UDBMBOSS _udbBoss = new UDBMBOSS();
            _udbBoss.IsUserNameExists(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, out Result.Result, out Result.isExist,out Result.ErrMsg);
            strLog.AppendFormat("结果:{0},{1}\r\n",Result.Result,Result.ErrMsg);
        }
        catch (Exception e)
        {
            Result.ErrMsg = e.Message;
            strLog.AppendFormat("异常:{0}\r\n",e.Message);
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("IsUserNameExist", strLog);
        }
        return Result;
    }


    #region 综合平台根据username 反查userid 
    public class GetUserInfoByNameResultV2
    {
        public string SPID;
        public string UserID;
        public int Result;
        public string ErrMsg;
    }
    [WebMethod(Description = "综合平台根据username 反查userid （soap）")]
    public GetUserInfoByNameResultV2 GetUnifyPlatformUserInfoByName(string SPID, string UserName, string key, string ExtendField)
    {
        StringBuilder strLog = new StringBuilder();
        GetUserInfoByNameResultV2 Result = new GetUserInfoByNameResultV2();
        try
        {
            strLog.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            strLog.Append("综合平台根据username 反查userid （soap）" + DateTime.Now.ToString("u") + "\r\n");
            strLog.Append("请求IP - " + HttpContext.Current.Request.UserHostAddress);
            strLog.Append(";SPID - " + SPID);
            strLog.Append(";UserName - " + UserName);
            strLog.Append(";key - " + key);
            strLog.Append(";ExtendField - " + ExtendField);

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetUserInfoByName", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;

            Result.Result = CIP2BizRules.GetUnifyPlatformUserInfoByName(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, out Result.UserID, out Result.ErrMsg);
            strLog.AppendFormat("综合平台根据username 反查userid,Result:{0},userId:{1},ErrMsg:{2}\r\n", Result.Result,Result.UserID,  Result.ErrMsg);
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
            strLog.AppendFormat("异常:{0}\r\n", e.ToString());
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("GetUnifyPlatformUserInfoByName", strLog);
        }
        return Result;
    }

    #endregion


    public class ImplicitUnifyRegistryResult
    {
        public string SPID;
        public string TimeStamp;
        public string CustID;
        public string TourCardID;
        public int Result;
        public string ErrMsg;
        
    }
    [WebMethod(Description = "综合平台隐式注册接口（soap）")]
    public ImplicitUnifyRegistryResult ImplicitUnifyRegistry(string SPID, string UserType, string CardID, string Password, string UProvinceID, string AreaCode,
                                               string RealName, string UserName, string AuthenPhone, string ContactTel, string IsNeedTourCard,
                                               string CertificateCode, string CertificateType, string Sex,string SendSMS, string ExtendField)
    {
        StringBuilder strLog = new StringBuilder();
        ImplicitUnifyRegistryResult Result = new ImplicitUnifyRegistryResult();
        strLog.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
        strLog.Append("注册接口（soap）" + DateTime.Now.ToString("u") + "\r\n");
        strLog.Append(";SPID - " + SPID);
        strLog.Append(":UserType - " + UserType);
        strLog.Append(":CardID - " + CardID);
        strLog.Append(":Password - " + Password);
        strLog.Append(":UProvinceID - " + UProvinceID);
        strLog.Append(":AreaCode - " + AreaCode);
        strLog.Append(":RealName - " + RealName);
        strLog.Append(":UserName - " + UserName);
        strLog.Append(":AuthenPhone - " + AuthenPhone);
        strLog.Append(":ContactTel - " + ContactTel);
        strLog.Append(":IsNeedTourCard - " + IsNeedTourCard);
        strLog.Append(":SendSMS - " + SendSMS);
        strLog.Append(":CertificateCode - " + CertificateCode);
        strLog.Append(":CertificateType - " + CertificateType);
        strLog.Append(":Sex - " + Sex);
        strLog.Append(":IP - " + HttpContext.Current.Request.UserHostAddress);
        strLog.Append(";ExtendField - " + ExtendField + "\r\n");

        String PwdType = "";

        Result.SPID = SPID;
        Result.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Result.CustID = "";
        //商旅卡9位
        Result.TourCardID = "";
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = "";
        int Unify_Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        string Unify_ErrMsg = "";

        //商旅卡16位
        string sCardID = "";
        try
        {

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "ImplicitUnifyRegistry", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            if (CommonUtility.IsEmpty(Password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，密码不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            if (CommonUtility.IsEmpty(AuthenPhone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "认证电话不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //这里有疑问
            if (CommonUtility.IsEmpty(UProvinceID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //这里有疑问
            if (CommonUtility.IsEmpty(AreaCode))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //这里有疑问
            if (CommonUtility.IsEmpty(RealName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            string phone = "";
            if (!CommonBizRules.PhoneNumValid(this.Context, AuthenPhone, out phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            if (CommonUtility.IsEmpty(IsNeedTourCard))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            if (CommonUtility.IsEmpty(SendSMS))
            {
                SendSMS = "no";   // no: 不发，yes 发
            }

            if (!"yes".Equals(SendSMS))
            {
                SendSMS = "no";
            }

            if (CommonUtility.IsEmpty(Sex))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }
            //注册天翼账号
            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;
            //String sendSms = UDBConstDefinition.DefaultInstance.UnifyPlatformRegisterAccountSendSms;

        
            String userId = String.Empty;
            String o_userName = String.Empty;
            String accessToken = String.Empty;
            String host = System.Configuration.ConfigurationManager.AppSettings["host"];
            strLog.AppendFormat("注册天翼账号{0}:\r\n", host);
            Unify_Result = CIP2BizRules.RegisterUnifyPlatformAccount(appId, appSecret, version, clientType, clientIp, clientAgent, AuthenPhone, Password, SendSMS, out userId, out o_userName, out accessToken, out Unify_ErrMsg);
            Result.Result = Unify_Result;
            Result.ErrMsg = Unify_ErrMsg;
            strLog.AppendFormat("注册天翼账号,Result:{0},accessToken:{1},userId:{2},usrName:{3},ErrMsg:{4}\r\n", Unify_Result, accessToken, userId, o_userName, Unify_ErrMsg);

            if (Unify_Result == 0 && !String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(accessToken))
            {
                strLog.Append("天翼账号注册成功\r\n");
                strLog.Append("处理结果 - " + Result.Result);
                strLog.Append("; 错误描述 - " + Result.ErrMsg);
                strLog.Append(": SPID - " + Result.SPID);
                strLog.Append(": TimeStamp - " + Result.TimeStamp);
                strLog.Append("; CustID - " + Result.CustID);
                strLog.Append(": sCardID - " + sCardID);
                strLog.Append(": password - " + Password);
                strLog.Append("; TourCardID - " + Result.TourCardID + "\r\n");


                strLog.Append("建立天翼账号和号百账号绑定关系!\r\n");
                UDBMBOSS _UDBMBoss = new UDBMBOSS();
                UnifyAccountInfo accountInfo = new UnifyAccountInfo();
                Unify_Result = _UDBMBoss.UnifyPlatformGetUserInfo(appId, appSecret, version, clientType, accessToken, clientIp, clientAgent, out accountInfo, out Unify_ErrMsg);
                strLog.AppendFormat("先查询综合平台返回:Unify_Result:{0},ErrMsg:{1},UserID:{2}\r\n", Unify_Result, Unify_ErrMsg, Convert.ToString(accountInfo.userId));

                if (Unify_Result == 0 && !String.IsNullOrEmpty(Convert.ToString(accountInfo.userId)))
                {
                    strLog.Append("然后开始注册或者绑定号百客户\r\n");

                    #region 开始注册到号百
                    String MobileName = String.Empty;
                    String EmailName = String.Empty;
                    if (String.IsNullOrEmpty(RealName))
                    {
                        if (!String.IsNullOrEmpty(accountInfo.nickName))
                        {
                            RealName = accountInfo.nickName;
                        }

                        if (!String.IsNullOrEmpty(accountInfo.mobileName))
                        {
                            RealName = accountInfo.mobileName;
                        }
                    }


                    if (!String.IsNullOrEmpty(accountInfo.mobileName))
                    {
                        MobileName = accountInfo.mobileName;
                    }
                    if (!String.IsNullOrEmpty(accountInfo.emailName))
                    {
                        EmailName = accountInfo.emailName;
                    }
                    String EncrytpPassWord = CryptographyUtil.Encrypt(Password);
                    String OperType = "5";  // 接口注册 , 天翼账号注册成功后绑定
                    if (!String.IsNullOrEmpty(MobileName) || !String.IsNullOrEmpty(EmailName))
                    {

                        String OuterID, Status, CustType, CustLevel, NickName, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                        Result.CustID = String.Empty;

                        Result.Result = CIP2BizRules.BindCustInfoUnifyPlatform(UProvinceID, AreaCode, MobileName, EmailName, RealName, EncrytpPassWord, accountInfo.userId, SPID, OperType, out Result.CustID, out Result.ErrMsg);
                        strLog.Append("【开始注册或者绑定到号百的结果】:\r\n");
                        strLog.AppendFormat("Result:{0},CustID:{1},ErrMsg:{2}\r\n", Result.Result, Result.CustID, Result.ErrMsg);

                        if (Result.Result == 0 && !String.IsNullOrEmpty(Result.CustID))
                        {
                            Result.Result = CustBasicInfo.getCustInfo(SPID, Result.CustID, out Result.ErrMsg, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                                out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                                out AreaID, out RegistrationSource);
                            string TourCardErrorMsg = "";
                            int TourCardResult = -1;
                            if ("0".Equals(IsNeedTourCard))
                            {

                                TourCardResult = UserRegistry.GetTourCard(Result.CustID, CardID, UProvinceID, AreaCode, 1, CustLevel, "01", "1", out Result.TourCardID, out sCardID, out TourCardErrorMsg);
                            }
                            strLog.Append(" 处理结果 - " + Result.Result);
                            strLog.Append("; 错误描述 - " + Result.ErrMsg);
                            strLog.Append(": SPID - " + Result.SPID);
                            strLog.Append(": TimeStamp - " + Result.TimeStamp);
                            strLog.Append("; TourCardResult - " + TourCardResult);
                            strLog.Append("; TourCardErrorMsg - " + TourCardErrorMsg);
                            strLog.Append("; CustID - " + Result.CustID);
                            strLog.Append("; TourCardID - " + Result.TourCardID + "\r\n");
                            CommonBizRules.WriteTraceIpLog(Result.CustID, AuthenPhone, Result.SPID, HttpContext.Current.Request.UserHostAddress.ToString(), "jk_zc");
                            CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrMsg);
                        }
                    }
                    else
                    {
                        Result.Result = -7766;
                        Result.ErrMsg = "MobileName,或者EmailName为空,所以不注册号百客户";
                        strLog.Append("MobileName,或者EmailName为空,所以不注册号百客户\r\n");
                    }
                    #endregion
                }
                else
                { //查询综合平台客户信息失败,或者account.userid为空
                    strLog.Append("查询综合平台客户信息失败,或者account.userid为空,放弃注册或绑定动作\r\n");
                }
            }
            else
            {   //51109  && Unify_ErrMsg.Equals("用户名已经存在")   //51224  用户名必须是手机号码或邮箱地址
                //if (Convert.ToString(Unify_Result).Equals("51109") )
                //{
                strLog.AppendFormat("综合平台注册结果:{0},ErrMsg:{1}\r\n", Unify_Result, Unify_ErrMsg);
                long t_userId = 0;
                Result.Result = CIP2BizRules.GetUnifyPlatformUserInfoByName(appId, appSecret, version, clientType, clientIp, clientAgent, AuthenPhone, out t_userId, out Result.ErrMsg);
                strLog.AppendFormat("GetUnifyPlatformUserInfoByName:Result:{0},ErrMsg:{1},UserID:{2}\r\n", Result.Result, Result.ErrMsg, Convert.ToString(t_userId));
                if (Result.Result == 0 && t_userId != 0)
                {
                    string T_CustID = "";
                    strLog.Append("是否已经是号码百事通账户?\r\n");
                    if (!CommonBizRules.HasBesttoneAccount(this.Context, AuthenPhone, out T_CustID, out Result.ErrMsg))
                    {
                        if (!String.IsNullOrEmpty(T_CustID))
                        {
                            strLog.AppendFormat("已经是CustID:{0}这个客户的号码百事通账户,放弃注册，放弃绑定\r\n", T_CustID);
                            Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                            Result.ErrMsg = "该手机号码已经为号码百事通账户!";
                            return Result;
                        }
                        else
                        {
                            strLog.Append("不是号百百事通账户\r\n");
                        }
                    }
                    strLog.Append("开始注册号码百事通登录账号\r\n");
                    Result.Result = UserRegistry.getUserRegistry(SPID, UserType, CardID, Password, UProvinceID, AreaCode, RealName, UserName, AuthenPhone, ContactTel, IsNeedTourCard, CertificateCode, CertificateType, Sex, ExtendField, out Result.ErrMsg, out Result.CustID, out Result.TourCardID, out sCardID);
                    strLog.AppendFormat("注册号码百事通登录账号结果,Result:{0},CustId:{1},ErrMsg:{2}\r\n", Result.Result, Result.CustID, Result.ErrMsg);
                    if (Result.Result == 0 && !String.IsNullOrEmpty(Result.CustID))
                    {
                        String EncrytpPassWord = CryptographyUtil.Encrypt(Password);
                        String OperType = "6";  //天翼账号通过接口注册失败后，号百登录账号注册成功，后绑定成功
                        Result.Result = CIP2BizRules.BindCustInfoUnifyPlatform(UProvinceID, AreaCode, AuthenPhone, "", RealName, EncrytpPassWord, t_userId, SPID, OperType, out Result.CustID, out Result.ErrMsg);
                        strLog.AppendFormat("绑定结果:{0},{1},{2}<->{3}\r\n", Result.Result, Result.ErrMsg, Convert.ToString(t_userId), Result.CustID);

                        CommonBizRules.WriteTraceIpLog(Result.CustID, AuthenPhone, Result.SPID, HttpContext.Current.Request.UserHostAddress.ToString(), "jk_zc");
                        CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrMsg);

                    }
                }
                else
                {
                    //非手机，邮箱注册（按道理不应该再让纯用户名的用户注册进来，否则数据导入综合平台又会发生问题，他们不接收纯用户名，但是纯用户名用户语音方式注册这种情况会很多,1天日志查下来大概有20-30笔,）
                    strLog.Append("开始注册号码百事通登录账号，纯用户名\r\n");

                }
                //}
            }
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
            strLog.AppendFormat("异常:{0}\r\n", e.ToString());
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("UserRegistryV2", strLog);
        }
        return Result;
    }


    #region 综合平台注册认证
    /// <summary>
    /// 2014-06-13
    /// author lihongtu
    /// 重大改版
    /// 注册为号百用户后，如果是手机注册，则变成联系手机，无法认证(phoneclass=1)
    /// 原因：号百用户提升为天翼账号后，原则上不能再有号百客户产生（仅保留基本信息）
    /// 但是综合平台的隐式注册不支持用户名，仅支持手机，也就意味着以用户名方式注册为号百客户的，不能注册为天翼账号
    /// 
    /// </summary>
    public class UserRegistryV2Result
    {
        public string SPID;
        public string TimeStamp;
        public string CustID;
        public string TourCardID;
        public int Result;
        public string ErrMsg;
    }
    [WebMethod(Description = "综合平台注册接口（soap）")]
    public UserRegistryV2Result UserRegistryV2(string SPID, string UserType, string CardID, string Password, string UProvinceID, string AreaCode,
                                               string RealName, string UserName, string AuthenPhone, string ContactTel, string IsNeedTourCard,
                                               string CertificateCode, string CertificateType, string Sex, string ExtendField)
    {
        StringBuilder strLog = new StringBuilder();
        UserRegistryV2Result Result = new UserRegistryV2Result();
        strLog.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
        strLog.Append("注册接口（soap）" + DateTime.Now.ToString("u") + "\r\n");
        strLog.Append(";SPID - " + SPID);
        strLog.Append(":UserType - " + UserType);
        strLog.Append(":CardID - " + CardID);
        strLog.Append(":Password - " + Password);
        strLog.Append(":UProvinceID - " + UProvinceID);
        strLog.Append(":AreaCode - " + AreaCode);
        strLog.Append(":RealName - " + RealName);
        strLog.Append(":UserName - " + UserName);
        strLog.Append(":AuthenPhone - " + AuthenPhone);
        strLog.Append(":ContactTel - " + ContactTel);
        strLog.Append(":IsNeedTourCard - " + IsNeedTourCard);
        strLog.Append(":CertificateCode - " + CertificateCode);
        strLog.Append(":CertificateType - " + CertificateType);
        strLog.Append(":Sex - " + Sex);
        strLog.Append(":IP - " + HttpContext.Current.Request.UserHostAddress);
        strLog.Append(";ExtendField - " + ExtendField + "\r\n");

        String PwdType = "";

        Result.SPID = SPID;
        Result.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Result.CustID = "";
        //商旅卡9位
        Result.TourCardID = "";
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = "";
        int Unify_Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        string Unify_ErrMsg = "";

        //商旅卡16位
        string sCardID = "";
        try
        {

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserRegistryV2", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            if (CommonUtility.IsEmpty(Password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，密码不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            if (CommonUtility.IsEmpty(AuthenPhone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "认证电话不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //这里有疑问
            if (CommonUtility.IsEmpty(UProvinceID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //这里有疑问
            if (CommonUtility.IsEmpty(AreaCode))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            //这里有疑问
            if (CommonUtility.IsEmpty(RealName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            string phone = "";
            if (!CommonBizRules.PhoneNumValid(this.Context, AuthenPhone, out phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }


            if (CommonUtility.IsEmpty(IsNeedTourCard))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            if (CommonUtility.IsEmpty(Sex))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }
            //注册天翼账号
            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;
            String sendSms = UDBConstDefinition.DefaultInstance.UnifyPlatformRegisterAccountSendSms;

            if (String.IsNullOrEmpty(sendSms))
            {
                sendSms = "false";
            }

            String userId = String.Empty;
            String o_userName = String.Empty;
            String accessToken = String.Empty;
            String host = System.Configuration.ConfigurationManager.AppSettings["host"];
            strLog.AppendFormat("注册天翼账号{0}:\r\n",host);
            Unify_Result = CIP2BizRules.RegisterUnifyPlatformAccount(appId, appSecret, version, clientType, clientIp, clientAgent, AuthenPhone, Password, sendSms, out userId, out o_userName, out accessToken, out Unify_ErrMsg);
            Result.Result = Unify_Result;
            Result.ErrMsg = Unify_ErrMsg;
            strLog.AppendFormat("注册天翼账号,Result:{0},accessToken:{1},userId:{2},usrName:{3},ErrMsg:{4}\r\n", Unify_Result, accessToken, userId, o_userName, Unify_ErrMsg);

            if (Unify_Result == 0 && !String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(accessToken))
            {
                strLog.Append("天翼账号注册成功\r\n");
                strLog.Append("处理结果 - " + Result.Result);
                strLog.Append("; 错误描述 - " + Result.ErrMsg);
                strLog.Append(": SPID - " + Result.SPID);
                strLog.Append(": TimeStamp - " + Result.TimeStamp);
                strLog.Append("; CustID - " + Result.CustID);
                strLog.Append(": sCardID - " + sCardID);
                strLog.Append(": password - " + Password);
                strLog.Append("; TourCardID - " + Result.TourCardID + "\r\n");


                strLog.Append("建立天翼账号和号百账号绑定关系!\r\n");
                UDBMBOSS _UDBMBoss = new UDBMBOSS();
                UnifyAccountInfo accountInfo = new UnifyAccountInfo();
                Unify_Result = _UDBMBoss.UnifyPlatformGetUserInfo(appId, appSecret, version, clientType, accessToken, clientIp, clientAgent, out accountInfo, out Unify_ErrMsg);
                strLog.AppendFormat("先查询综合平台返回:Unify_Result:{0},ErrMsg:{1},UserID:{2}\r\n", Unify_Result, Unify_ErrMsg, Convert.ToString(accountInfo.userId));

                if (Unify_Result == 0 && !String.IsNullOrEmpty(Convert.ToString(accountInfo.userId)))
                {
                    strLog.Append("然后开始注册或者绑定号百客户\r\n");

                    #region 开始注册到号百
                    String MobileName = String.Empty;
                    String EmailName = String.Empty;
                    if (String.IsNullOrEmpty(RealName) )
                    {
                        if (!String.IsNullOrEmpty(accountInfo.nickName))
                        {
                            RealName = accountInfo.nickName;
                        }

                        if (!String.IsNullOrEmpty(accountInfo.mobileName))
                        {
                            RealName = accountInfo.mobileName;
                        }
                    }


                    if (!String.IsNullOrEmpty(accountInfo.mobileName))
                    {
                        MobileName = accountInfo.mobileName;
                    }
                    if (!String.IsNullOrEmpty(accountInfo.emailName))
                    {
                        EmailName = accountInfo.emailName;
                    }
                    String EncrytpPassWord = CryptographyUtil.Encrypt(Password);
                    String OperType = "5";  // 接口注册 , 天翼账号注册成功后绑定
                    if (!String.IsNullOrEmpty(MobileName) || !String.IsNullOrEmpty(EmailName))
                    {

                        String OuterID, Status, CustType, CustLevel, NickName, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                        Result.CustID = String.Empty;

                        Result.Result = CIP2BizRules.BindCustInfoUnifyPlatform(UProvinceID, AreaCode, MobileName, EmailName, RealName, EncrytpPassWord, accountInfo.userId, SPID, OperType, out Result.CustID, out Result.ErrMsg);
                        strLog.Append("【开始注册或者绑定到号百的结果】:\r\n");
                        strLog.AppendFormat("Result:{0},CustID:{1},ErrMsg:{2}\r\n", Result.Result, Result.CustID, Result.ErrMsg);

                        if (Result.Result == 0 && !String.IsNullOrEmpty(Result.CustID))
                        {
                            Result.Result = CustBasicInfo.getCustInfo(SPID, Result.CustID, out Result.ErrMsg, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                                out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                                out AreaID, out RegistrationSource);
                            string TourCardErrorMsg = "";
                            int TourCardResult = -1;
                            if ("0".Equals(IsNeedTourCard))
                            {
             
                                TourCardResult = UserRegistry.GetTourCard(Result.CustID, CardID, UProvinceID, AreaCode, 1, CustLevel, "01", "1", out Result.TourCardID, out sCardID, out TourCardErrorMsg);
                            }
                            strLog.Append(" 处理结果 - " + Result.Result);
                            strLog.Append("; 错误描述 - " + Result.ErrMsg);
                            strLog.Append(": SPID - " + Result.SPID);
                            strLog.Append(": TimeStamp - " + Result.TimeStamp);
                            strLog.Append("; TourCardResult - " + TourCardResult);
                            strLog.Append("; TourCardErrorMsg - " + TourCardErrorMsg);
                            strLog.Append("; CustID - " + Result.CustID);
                            strLog.Append("; TourCardID - " + Result.TourCardID + "\r\n");
                            CommonBizRules.WriteTraceIpLog(Result.CustID, AuthenPhone, Result.SPID, HttpContext.Current.Request.UserHostAddress.ToString(), "jk_zc");
                            CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrMsg);
                        }
                    }
                    else
                    {
                        Result.Result = -7766;
                        Result.ErrMsg = "MobileName,或者EmailName为空,所以不注册号百客户";
                        strLog.Append("MobileName,或者EmailName为空,所以不注册号百客户\r\n");
                    }
                    #endregion
                }
                else
                { //查询综合平台客户信息失败,或者account.userid为空
                    strLog.Append("查询综合平台客户信息失败,或者account.userid为空,放弃注册或绑定动作\r\n");
                }
            }
            else
            {   //51109  && Unify_ErrMsg.Equals("用户名已经存在")   //51224  用户名必须是手机号码或邮箱地址
                //if (Convert.ToString(Unify_Result).Equals("51109") )
                //{
                    strLog.AppendFormat("综合平台注册结果:{0},ErrMsg:{1}\r\n",Unify_Result,Unify_ErrMsg);
                    long t_userId = 0;
                    Result.Result = CIP2BizRules.GetUnifyPlatformUserInfoByName(appId, appSecret, version, clientType, clientIp, clientAgent, AuthenPhone, out t_userId, out Result.ErrMsg);
                    strLog.AppendFormat("GetUnifyPlatformUserInfoByName:Result:{0},ErrMsg:{1},UserID:{2}\r\n",Result.Result,Result.ErrMsg,Convert.ToString(t_userId));
                    if (Result.Result == 0 && t_userId != 0)
                    {
                        string T_CustID = "";
                        strLog.Append("是否已经是号码百事通账户?\r\n");
                        if (!CommonBizRules.HasBesttoneAccount(this.Context, AuthenPhone, out T_CustID, out Result.ErrMsg))
                        {
                            if (!String.IsNullOrEmpty(T_CustID))
                            {
                                strLog.AppendFormat("已经是CustID:{0}这个客户的号码百事通账户,放弃注册，放弃绑定\r\n", T_CustID);
                                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                                Result.ErrMsg = "该手机号码已经为号码百事通账户!";
                                return Result;
                            }
                            else
                            {
                                strLog.Append("不是号百百事通账户\r\n");
                            }
                        }
                        strLog.Append("开始注册号码百事通登录账号\r\n");
                        Result.Result = UserRegistry.getUserRegistry(SPID, UserType, CardID, Password, UProvinceID, AreaCode, RealName, UserName, AuthenPhone, ContactTel, IsNeedTourCard, CertificateCode, CertificateType, Sex, ExtendField, out Result.ErrMsg, out Result.CustID, out Result.TourCardID, out sCardID);
                        strLog.AppendFormat("注册号码百事通登录账号结果,Result:{0},CustId:{1},ErrMsg:{2}\r\n", Result.Result, Result.CustID, Result.ErrMsg);
                        if (Result.Result == 0 && !String.IsNullOrEmpty(Result.CustID))
                        {
                            String EncrytpPassWord = CryptographyUtil.Encrypt(Password);
                            String OperType = "6";  //天翼账号通过接口注册失败后，号百登录账号注册成功，后绑定成功
                            Result.Result = CIP2BizRules.BindCustInfoUnifyPlatform(UProvinceID, AreaCode, AuthenPhone, "", RealName, EncrytpPassWord, t_userId, SPID, OperType, out Result.CustID, out Result.ErrMsg);
                            strLog.AppendFormat("绑定结果:{0},{1},{2}<->{3}\r\n", Result.Result, Result.ErrMsg, Convert.ToString(t_userId), Result.CustID);

                            CommonBizRules.WriteTraceIpLog(Result.CustID, AuthenPhone, Result.SPID, HttpContext.Current.Request.UserHostAddress.ToString(), "jk_zc");
                            CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrMsg);
            
                        }
                    }
                    else
                    { 
                        //非手机，邮箱注册（按道理不应该再让纯用户名的用户注册进来，否则数据导入综合平台又会发生问题，他们不接收纯用户名，但是纯用户名用户语音方式注册这种情况会很多,1天日志查下来大概有20-30笔,）
                        strLog.Append("开始注册号码百事通登录账号，纯用户名\r\n");
                    
                    }
                //}
            }
        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
            strLog.AppendFormat("异常:{0}\r\n", e.ToString());
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("UserRegistryV2", strLog);
        }
        return Result;
    }


    public class LoginByIMSIResult
    {
        public int Result;
        public string ErrMsg;
        public string CustID;
        public string AccessToken;
        public long expiresIn;
        public string loginNum;
        public UnifyAccountInfo unifyAccount = new UnifyAccountInfo();
        public string ExtendField;
    }
    [WebMethod(Description = "天翼账号IMSI登录认证接口")]
    public LoginByIMSIResult LoginByIMSI(string SPID, string imsi)
    {
        LoginByIMSIResult Result = new LoginByIMSIResult();
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("imsi登录认证,时间:{0}\r\n",DateTime.Now.ToString("u"));
        strLog.AppendFormat("SPID:{0},IMSI:{1}\r\n",SPID,imsi);
        try
        {
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            if (CommonUtility.IsEmpty(imsi))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = "imsi号不能为空";
                strLog.Append(Result.ErrMsg + "\r\n");
                return Result;
            }

            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;

            strLog.Append("IMSI登录认证!\r\n");
            UDBMBOSS _UDBMBoss = new UDBMBOSS();
            UnifyAccountInfo accountInfo = new UnifyAccountInfo();
            string Unify_ErrMsg = String.Empty;
            string loginNum = String.Empty;
            string accessToken = String.Empty;
            long expiresIn = 0;
            //                                                                                    appId, appSecret, version, clientType, imsi, clientIp, clientAgent, out accountInfo, out loginNum, out accessToken, out expiresIn,out  ErrMsg
            int Unify_Result = _UDBMBoss.UnifyPlatformLoginByImsi(appId, appSecret, version, clientType,imsi, clientIp, clientAgent, out  Result.unifyAccount,out Result.loginNum,out Result.AccessToken, out Result.expiresIn, out Unify_ErrMsg);
            strLog.AppendFormat("登录认证:Unify_Result:{0},ErrMsg:{1},loginNum:{2},accessToken:{3}\r\n", Unify_Result, Unify_ErrMsg, loginNum,accessToken);
            if (Unify_Result == 0 && Result.unifyAccount != null )
            {
                //增加绑定动作 根据loginnum
                #region
                String CustID, OuterID, Status, CustType, CustLevel, RealName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                CustID = String.Empty;

                System.Text.RegularExpressions.Regex regMobile = new System.Text.RegularExpressions.Regex(@"^1[345678]\d{9}$");
                System.Text.RegularExpressions.Regex regEmail = new System.Text.RegularExpressions.Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
                String MobileName = String.Empty;
                String EmailName = String.Empty;
                RealName = String.Empty;
                if (Result.unifyAccount != null)
                {
                    RealName = Result.unifyAccount.userName;
                }

                if (regMobile.IsMatch(Result.unifyAccount.userName))
                {
                    MobileName = Result.unifyAccount.userName;
                }

                if (regEmail.IsMatch(Result.unifyAccount.userName))
                {
                    EmailName = Result.unifyAccount.userName;
                }
                String EncrytpPassWord = CryptographyUtil.Encrypt("123456");
                String OperType = "2";

                strLog.Append("【开始绑定号百】:\r\n");
                Result.Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", MobileName, EmailName, RealName, EncrytpPassWord, Result.unifyAccount.userId, SPID, OperType, out Result.CustID, out Result.ErrMsg);
                strLog.Append("【开始绑定到号百的结果】:\r\n");
                strLog.AppendFormat("Result:{0},CustID:{1}<->UserID:{2}ErrMsg:{3}\r\n", Result.Result, CustID, Result.unifyAccount.userId, Result.ErrMsg);
                #endregion

                //保存accessToken
                if (Result.Result == 0 && !String.IsNullOrEmpty(CustID))  //绑定成功
                {
                    Result.CustID = CustID;
                    strLog.Append("绑定成功，记录Accesstoken\r\n");
                    String Description = "接口登录";
                    Result.Result = CIP2BizRules.InsertAccessToken(SPID, HttpContext.Current.Request.UserHostAddress.ToString(), accessToken, Convert.ToString(Result.unifyAccount.userId), CustID, RealName, Result.unifyAccount.nickName, Result.loginNum, OperType, Description, out Result.ErrMsg);
                    strLog.AppendFormat("记录AccessToken结果,Result:{0},CustID:{1}<->AccessToken:{2}\r\n", Result.Result, CustID, accessToken);
                }
                else
                {
                    strLog.Append("绑定失败,无法记录AccessToken,\r\n");
                }
                strLog.AppendFormat("userid:{0}\r\n", Result.unifyAccount.userId);
            }
            else
            {
                strLog.Append("imsi登录失败\r\n");
            }
        }
        catch (Exception e)
        {
            strLog.AppendFormat("异常:{0}\r\n",e.Message);
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("LoginByIMSIResult", strLog);
        }
        return Result;
    }

    public class PotentialRegistryResult
    {
        public int Result;
        public string ErrMsg;
        public string CustID;
        public string ExtendField;
    }
    [WebMethod(Description="潜在客户注册接口")]
    public PotentialRegistryResult UserRegistryPotential(string SPID, string UserType, string UProvinceID, string AreaCode,
                                               string RealName, string UserName, string ContactTel, string CertificateCode, 
                                               string CertificateType, string Sex, string ExtendField)
    {
        PotentialRegistryResult Result = new PotentialRegistryResult();
        Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg + ",系统标识不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg + ",长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIPLimited_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIPLimite_Msg;
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserRegistryPotential", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPInterfaceLimited_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPInterfaceLimited_Msg;
                return Result;
            }

            if (CommonUtility.IsEmpty(ContactTel))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Msg + ",不能为空";
                return Result;
            }
            else
            {
                if (!CommonUtility.ValidateMobile(ContactTel))
                {
                    Result.Result = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Code;
                    Result.ErrMsg = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Msg;
                    return Result;
                }
            }
            //如果没有传递省市，则查询PhoneArea表
            String provinceid = String.Empty, areaid = String.Empty;
            if (String.IsNullOrEmpty(UProvinceID) || String.IsNullOrEmpty(AreaCode))
            {
                Int32 result_01;
                string ErrMsg_01;
                result_01 = BTForBusinessSystemInterfaceRules.GetPhoneTOArea(ContactTel, out provinceid, out areaid, out ErrMsg_01);
                if (result_01 != 0)
                {
                    provinceid = "02";
                    areaid = "021";
                }

                UProvinceID = provinceid;
                AreaCode = areaid;
            }

            //if (CommonUtility.IsEmpty(UProvinceID))
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，省份不能为空";
            //    return Result;
            //}

            //if (CommonUtility.IsEmpty(AreaCode))
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，地区不能为空";
            //    return Result;
            //}

            //if (CommonUtility.IsEmpty(RealName))
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，姓名不能为空";
            //    return Result;
            //}

            string UserNameSPID = System.Configuration.ConfigurationManager.AppSettings["UserName_SPID"];

            int SIP = UserNameSPID.IndexOf(SPID);
            if (SIP < 0)
            {
                if (CommonUtility.IsEmpty(UserName))
                {
                    Result.Result = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Code;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，用户名不能为空";
                    return Result;
                }
            }
            
            #endregion

            Result.Result = UserRegistry.getUserRegistryPotential(SPID, UserType, UProvinceID, AreaCode, RealName, UserName, ContactTel, CertificateCode, CertificateType, Sex, out Result.CustID, out Result.ErrMsg);
        }
        catch (Exception ex)
        {
            Result.ErrMsg += "异常:" + ex.Message;
        }
        finally
        {
            try
            {
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("注册接口（soap）" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":UserType - " + UserType);
                msg.Append(":UProvinceID - " + UProvinceID);
                msg.Append(":AreaCode - " + AreaCode);
                msg.Append(":RealName - " + RealName);
                msg.Append(":UserName - " + UserName);
                msg.Append(":ContactTel - " + ContactTel);
                msg.Append(":CertificateCode - " + CertificateCode);
                msg.Append(":CertificateType - " + CertificateType);
                msg.Append(":Sex - " + Sex);
                msg.Append(":IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrMsg);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserRegistryPotential", msg);
                #endregion

                #region 数据库日志
                StringBuilder inParam = new StringBuilder();
                inParam.Append("SPID:" + SPID);
                inParam.Append(",UserType:" + UserType);
                inParam.Append(",UProvinceID:" + UProvinceID);
                inParam.Append(",AreaCode:" + AreaCode);
                inParam.Append(",RealName:" + RealName);
                inParam.Append(",UserName:" + UserName);
                inParam.Append(",ContactTel:" + ContactTel);
                inParam.Append(",CertificateCode:" + CertificateCode);
                inParam.Append(",CertificateType:" + CertificateType);
                inParam.Append(",Sex:" + Sex);
                inParam.Append(",ExtendField:" + ExtendField);

                String outParam = String.Format("CustID:{0},Result:{1},ErrMsg:{2},ExtendField{3}",
                                Result.CustID, Result.Result, Result.ErrMsg, Result.ExtendField);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "UserRegistryPotential", inParam.ToString(), outParam, Result.Result, Result.ErrMsg);
                #endregion
            }
            catch { }
        }
        return Result;
    }

    public class PotentialUserToRegistryResult
    {
        public int Result;
        public string ErrMsg;
        public string CustID;
        public string TourCardID;
        public string CardID;
        public string ExtendField;
    }
    [WebMethod(Description="潜在客户注册为正式客户")]
    public PotentialUserToRegistryResult TranPotentialUserToRegistry(string SPID, string CustID, string UserType,string PwdType, string Password, string UProvinceID, string AreaCode,
                                                string RealName, string UserName, string Sex, string AuthenPhone, string ContactTel, string CertificateCode, string CertificateType,
                                                string IsNeedTourCard, string CardID, string ExtendField)
    {
        PotentialUserToRegistryResult Result = new PotentialUserToRegistryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        Result.CustID = string.Empty;
        Result.CardID = string.Empty;
        Result.TourCardID = string.Empty;
        Result.ExtendField = string.Empty;

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "TranPotentialUserToRegistry", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            string VoicePwdSPID = System.Configuration.ConfigurationManager.AppSettings["VoicePwd_SPID"];

            int SIP1 = VoicePwdSPID.IndexOf(SPID);
            if (SIP1 < 0)
            {
                if (CommonUtility.IsEmpty(Password))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，密码不能为空";
                    return Result;
                }

                //if (!CommonUtility.IsEmpty(ExtendField))
                //{
                //    PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");
                //}
                //2为web密码，其他均为语音密码
                if (!PwdType.Equals("2"))
                {
                    if (Password.Length != ConstDefinition.Length_Min_Password)
                    {
                        Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                        Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "语音密码长度有误";
                        return Result;
                    }
                }

                if (!CommonUtility.IsNumeric(Password) && !PwdType.Equals("2"))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "语音密码只能是数字";
                    return Result;
                }
            }
            else
            {
                if (CommonUtility.IsEmpty(Password))
                {
                    Random rd = new Random();
                    Password = rd.Next(111111, 999999).ToString();
                }
            }

            if (CommonUtility.IsEmpty(UProvinceID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，省份不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(AreaCode))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，地区不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(RealName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，姓名不能为空";
                return Result;
            }

            string UserNameSPID = System.Configuration.ConfigurationManager.AppSettings["UserName_SPID"];

            int SIP = UserNameSPID.IndexOf(SPID);
            if (SIP < 0)
            {
                if (CommonUtility.IsEmpty(UserName))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，用户名不能为空";
                    return Result;
                }
            }

            if (CommonUtility.IsEmpty(AuthenPhone) && CommonUtility.IsEmpty(ContactTel))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，认证电话和联系电话不能同时为空";
                return Result;
            }

            if (!CommonUtility.IsEmpty(AuthenPhone))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, AuthenPhone, out phone))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                    return Result;
                }
                AuthenPhone = phone;
            }

            if (!CommonUtility.IsEmpty(ContactTel))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, ContactTel, out phone))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg;
                    return Result;
                }
                ContactTel = phone;
            }
            #endregion

            Result.Result = UserRegistry.getPotentialUserToRegistryUser(SPID, CustID, UserType, PwdType, Password, UProvinceID, AreaCode, RealName, UserName, Sex, 
                                    AuthenPhone, ContactTel, CertificateCode, CertificateType, IsNeedTourCard, CardID, out Result.TourCardID, out Result.CardID, out Result.ErrMsg);
            if (Result.Result == 0)
            {
                //通知积分平台
                CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrMsg);

                string Message = "";
                string phone = CommonUtility.IsEmpty(AuthenPhone) ? ContactTel : AuthenPhone;

                if (SIP1 >= 0 && CommonUtility.IsEmpty(Password))
                {
                    Message = "您已成为号码百事通客户,密码:" + Password + ",欢迎使用号百服务!此短信免费。";
                    //通知短信网关
                    CommonBizRules.SendMessage(phone, Message, "35000000");
                }
            }
        }
        catch (Exception ex)
        {
            Result.ErrMsg += "异常:" + ex.Message;
        }
        finally
        {
            try
            {
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("注册接口（soap）" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":UserType - " + UserType);
                msg.Append(":UProvinceID - " + UProvinceID);
                msg.Append(":AreaCode - " + AreaCode);
                msg.Append(":RealName - " + RealName);
                msg.Append(":UserName - " + UserName);
                msg.Append(":ContactTel - " + ContactTel);
                msg.Append(":CertificateCode - " + CertificateCode);
                msg.Append(":CertificateType - " + CertificateType);
                msg.Append(":Sex - " + Sex);
                msg.Append(":IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrMsg);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("TranPotentialUserToRegistry", msg);
                #endregion

                #region 数据库日志
                StringBuilder inParam = new StringBuilder();
                inParam.Append("SPID:" + SPID);
                inParam.Append(",CustID:" + CustID);
                inParam.Append(",UserType:" + UserType);
                inParam.Append(",UProvinceID:" + UProvinceID);
                inParam.Append(",AreaCode:" + AreaCode);
                inParam.Append(",RealName:" + RealName);
                inParam.Append(",UserName:" + UserName);
                inParam.Append(",Sex:" + Sex);
                inParam.Append(",AuthenPhone:" + AuthenPhone);
                inParam.Append(",ContactTel:" + ContactTel);
                inParam.Append(",CertificateCode:" + CertificateCode);
                inParam.Append(",CertificateType:" + CertificateType);
                inParam.Append(",IsNeedTourCard:" + IsNeedTourCard);
                inParam.Append(",CardID:" + CardID);
                inParam.Append(",ExtendField:" + ExtendField);

                String outParam = String.Format("CustID:{0},TourCardID:{1},CardID:{2},Result:{3},ErrMsg:{4},ExtendField{5}",
                                Result.CustID,Result.TourCardID,Result.CardID, Result.Result, Result.ErrMsg, Result.ExtendField);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "UserRegistryPotential", inParam.ToString(), outParam, Result.Result, Result.ErrMsg);
                #endregion
            }
            catch { }
        }

        return Result;
    }

    public class UserRegistryV2WebResult
    {
        public string SPID;
        public string TimeStamp;
        public string CustID;
        public int Result;
        public string ErrMsg;
        public String ExtendField;
    }
    [WebMethod(Description="web注册接口")]
    public UserRegistryV2WebResult UserRegisteryV2Web(String SPID, String UserName, String RealName, String NickName, String PassWord, 
                                            String PhoneNum, String PhoneState, String Email, String EmailState, String ProvinceID, String AreaID,
                                            String CertificateType, String CertificateCode, String Sex, String Birthday, String EduleLevel, String IncomeLevel, String ExtendField)
    {
        UserRegistryV2WebResult Result = new UserRegistryV2WebResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        try
        {
            #region 数据有效性验证
            
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserRegisteryV2Web", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(UserName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，用户名不能为空";
                return Result;
            }

            if (CustBasicInfo.IsExistUser(UserName) != 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，用户名重复";
                return Result;
            }

            if (CommonUtility.IsEmpty(RealName))
            {
                RealName = UserName;
                //Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                //Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，真实姓名不能为空";
                //return Result;
            }

            //如果没有传递省市，则查询PhoneArea表
            String provinceid = String.Empty, areaid = String.Empty;
            if (String.IsNullOrEmpty(ProvinceID) || String.IsNullOrEmpty(AreaID))
            {
                Int32 result_01;
                string ErrMsg_01;
                result_01 = BTForBusinessSystemInterfaceRules.GetPhoneTOArea(PhoneNum, out provinceid, out areaid, out ErrMsg_01);
                if (result_01 != 0)
                {
                    provinceid = "02";
                    areaid = "021";
                }

                ProvinceID = provinceid;
                AreaID = areaid;
            }

            if (!CommonUtility.IsEmpty(Email))
            {
                if (!CommonUtility.ValidateEmail(Email))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidEmail_Code;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidEmail_Msg + "，邮箱格式不正确";
                    return Result;
                }
                //验证认证邮箱是否被使用
                if (EmailState == "1")
                {
                    Result.Result = SetMail.EmailSel("", Email, SPID, out Result.ErrMsg);
                    if (Result.Result != 0)
                    {
                        Result.ErrMsg = "认证邮箱已被注册";
                        return Result;
                    }
                }
            }

            if (!CommonUtility.IsEmpty(PhoneNum))
            {
                string phone = "";
                //这里是利用手机的认证方式，web注册时只容许手机
                if (!CommonUtility.ValidateMobile(PhoneNum))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Msg + "，手机格式不正确";
                    return Result;
                }
                //如果是认证手机，则先验证是否被占用
                if (PhoneState == "1")
                {
                    Result.Result = PhoneBO.PhoneSel("", PhoneNum, out Result.ErrMsg);
                    if (Result.Result != 0)
                    {
                        Result.ErrMsg = "认证手机已经被注册";
                        return Result;
                    }
                }
            }

            if (CommonUtility.IsEmpty(PassWord))
            {
                Random random = new Random();
                PassWord = random.Next(100000, 999999).ToString();
            }

            #endregion

            Result.Result = UserRegistry.getUserRegistryWeb(SPID, UserName, RealName, PassWord, PhoneNum, PhoneState, Email, EmailState, NickName, CertificateType, CertificateCode,
                                            Sex, Birthday, EduleLevel, IncomeLevel, ProvinceID, AreaID, out Result.CustID, out Result.ErrMsg);
            if (Result.Result == 0)
            {
                Int32 result;
                String errmsg = String.Empty;
                //通知积分平台
                CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out errmsg);
            }
        }
        catch (Exception ex)
        {
            Result.ErrMsg += ex.Message;
        }
        finally
        {
            try
            {
                #region 记录文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("Web注册接口（soap）" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(":IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";SPID - " + SPID);
                msg.Append(":UserName - " + UserName);
                msg.Append(":RealName - " + RealName);
                msg.Append(":NickName - " + NickName);
                msg.Append(":PassWord - " + PassWord);
                msg.Append(":PhoneNum - " + PhoneNum);
                msg.Append(":PhoneState - " + PhoneState);
                msg.Append(":Email - " + Email);
                msg.Append(":EmailState - " + EmailState);
                msg.Append(":ProvinceID - " + ProvinceID);
                msg.Append(":AreaID - " + AreaID);
                msg.Append(":CertificateType - " + CertificateType);
                msg.Append(":CertificateCode - " + CertificateCode);
                msg.Append(":Sex - " + Sex);
                msg.Append(":Birthday - " + Birthday);
                msg.Append(":EduleLevel - " + EduleLevel);
                msg.Append(":IncomeLevel - " + IncomeLevel);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrMsg);
                msg.Append("; CustID - " + Result.CustID + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserRegisteryV2Web", msg);

                #endregion

                #region 记录数据库日志

                StringBuilder inParam = new StringBuilder();
                inParam.AppendFormat("SPID - {0}", SPID);
                inParam.AppendFormat(";UserName - {0}", UserName);
                inParam.AppendFormat(";RealName - {0}", RealName);
                inParam.AppendFormat(";NickName - {0}", NickName);
                inParam.AppendFormat(";PassWord - {0}", PassWord);

                inParam.AppendFormat(";PhoneNum - {0}", PhoneNum);
                inParam.AppendFormat(";PhoneState - {0}", PhoneState);
                inParam.AppendFormat(";Email - {0}", Email);
                inParam.AppendFormat(";EmailState - {0}", EmailState);
                inParam.AppendFormat(";ProvinceID - {0}", ProvinceID);
                inParam.AppendFormat(";AreaID - {0}", AreaID);
                inParam.AppendFormat(";CertificateType - {0}", CertificateType);
                inParam.AppendFormat(";CertificateCode - {0}", CertificateCode);
                inParam.AppendFormat(";Sex - {0}", Sex);
                inParam.AppendFormat(";Birthday - {0}", Birthday);
                inParam.AppendFormat(";EduleLevel - {0}", EduleLevel);
                inParam.AppendFormat(";IncomeLevel - {0}", IncomeLevel);
                inParam.AppendFormat(";ExtendField - {0}", ExtendField);

                String outParam = String.Format("CustID:{0},ExtendField:{1},Result:{2},ErrMsg:{3}", Result.CustID, Result.ExtendField, Result.Result, Result.ErrMsg);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "UserRegisteryV2Web", inParam.ToString(), outParam, Result.Result, Result.ErrMsg);

                #endregion
            }
            catch { }
        }

        return Result;
    }


    public class YoungMemberQueryRessult
    {
        public ClubMember ClubMember;
        public int Result;
        public string ErrorDescription;
    }

    [WebMethod(Description = "飞Young俱乐部成员查询接口")]
    public YoungMemberQueryRessult YoungMemberQuery(string SPID, string CodeValue, string ExtendField)
    {
        YoungMemberQueryRessult Result = new YoungMemberQueryRessult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        string TestStr = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(CodeValue))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }
            #endregion
            string ProvinceID = "";
            string Areaid = "";
            strLog.AppendFormat("根据手机号码查询所在地区省ID\r\n");
            Result.Result = BTForBusinessSystemInterfaceRules.GetPhoneTOArea(CodeValue, out ProvinceID, out Areaid, out Result.ErrorDescription);
            
            strLog.AppendFormat("根据手机号码查询所在地区省ID返回:Result:{0},ErrMsg:{1},ProvinceID:{2},AreaId:{3}\r\n", Result.Result, Result.ErrorDescription, ProvinceID, Areaid);
            string SOO_ID = "1";
            string LAN_ID = "";

            if (!Areaid.StartsWith("0"))
            {
                Areaid = "0" + Areaid;
            }
            string AREA_NBR =Areaid;
            string ACC_NBR = CodeValue;
            string PROD_CLASS = "12";
            ClubMember cm = new ClubMember();
            int qryLanIdResult = CIP2BizRules.GetYangLanID(Areaid, out LAN_ID, out Result.ErrorDescription);
            strLog.AppendFormat("查询LanId:qryLanIdResult:{0},LAN_ID:{1}\r\n",qryLanIdResult,LAN_ID);
            if (qryLanIdResult == 0)
            {
                Result.Result = CrmSSO.GetCustIdByAccNbr(ProvinceID, SOO_ID, LAN_ID, AREA_NBR, ACC_NBR, PROD_CLASS, "", this.Context, out cm, out Result.ErrorDescription, out TestStr);
                Result.ClubMember = cm;
                strLog.AppendFormat("Result.Result:{0}\r\n",Result.Result);
                strLog.AppendFormat("ClubMember.CUST_ID={0},MEMBER_ID={1},MEMBER_NAME={2},MEMBER_CODE={3},MEMBERSHIP_LEVEL={4}\r\n", Result.ClubMember.CUST_ID, Result.ClubMember.MEMBER_ID, Result.ClubMember.MEMBER_NAME, Result.ClubMember.MEMBER_CODE, Result.ClubMember.MEMBERSHIP_LEVEL);
                
            }
            else {
                strLog.AppendFormat("查询LanId失败\r\n");
            }
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
            strLog.AppendFormat(ex.ToString());        
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("YoungMumberQuery", strLog);        
        }
        return Result;
    }


  

    public class YoungUserAuthV2Result
    {
        public string CustID;
        public string CustName;
        public string Sex;
        public string OuterID;
        public string ProvinceID;
        public string Areaid;
        public string PointType;
        public string PointValueSum;
        public string PointValue;
        public int Result;
        public string ErrorDescription;
    }

    [WebMethod(Description = "飞Young俱乐部客户认证接口")]
    public YoungUserAuthV2Result YoungUserAuthV2(string SPID, string AuthenName, string AuthenType, string Password, string ExtendField)
    {
        YoungUserAuthV2Result Result = new YoungUserAuthV2Result();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        string ProvinceID = "";
        string Areaid = "";
        string RealName = "";
        string UserName = "";
        string NickName = "";
        string CustType = "";
        string OutID = "";
        string CustID = "";
        string TestStr = "";
        string PasswdFlag = "";
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("YoungUserAuthV2 参数:SPID:{0},AuthenName:{1},Password:{2},ExtendField:{3}\r\n", SPID, AuthenName, Password, ExtendField);
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


            if (!CommonUtility.IsEmpty(ExtendField))
            {
                PasswdFlag = CommonBizRules.GetValueFromXmlStr(ExtendField, "PasswdFlag");
                if (!"1".Equals(PasswdFlag))
                {
                    if (CommonUtility.IsEmpty(Password))
                    {
                        Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                        return Result;
                    }
                }
            }
            else {
                PasswdFlag = "1";
                if (CommonUtility.IsEmpty(Password))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                    return Result;
                }
            }


            #endregion
            strLog.AppendFormat("根据手机号码查询所在地区省ID\r\n");
            Result.Result = BTForBusinessSystemInterfaceRules.GetPhoneTOArea(AuthenName, out ProvinceID, out Areaid, out Result.ErrorDescription);
            strLog.AppendFormat("根据手机号码查询所在地区省ID返回:Result:{0},ErrMsg:{1},ProvinceID:{2},AreaId:{3}\r\n", Result.Result, Result.ErrorDescription, ProvinceID, Areaid);
            strLog.AppendFormat("飞young认证:参数,ProvinceID:{0},AuthenType:{1},AuthenName:{2},Password:{3}\r\n", ProvinceID, AuthenType, AuthenName, Password);
            
            string PointType = "";
            string PointValueSum = "";
            string PointValue = "";
            Result.Result = CrmSSO.YoungUserAuthV2(ProvinceID, Areaid, AuthenType, AuthenName, "", Password, PasswdFlag, "", Context, out RealName, out UserName,
               out NickName, out OutID, out CustType, out CustID,  out PointType,out PointValueSum,out PointValue,out Result.ErrorDescription, out TestStr);
            strLog.AppendFormat("飞young认证返回:Result:{0},ErrMsg:{1},OutID:{2},CustID:{3}\r\n", Result.Result, Result.ErrorDescription, OutID, CustID);
            Result.CustID = CustID;
            Result.CustName = RealName;
            Result.ProvinceID = ProvinceID;
            Result.Areaid = Areaid;
            Result.OuterID = OutID;
            Result.Sex = "2";
            Result.PointType = PointType;
            Result.PointValue = PointValue;
            Result.PointValueSum = PointValueSum;

        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
            strLog.AppendFormat(ex.ToString());
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("YoungUserAuthV2", strLog);
            CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, ProvinceID, AuthenType, AuthenName, "1", Result.Result, Result.ErrorDescription);

        }
        return Result;
    }



    public class YoungUserAuthResult
    {
        public string CustID;
        public string OuterID;
        public string ProvinceID;
        public int Result;
        public string ErrorDescription;
    }
    [WebMethod(Description = "飞Young俱乐部客户认证接口")]
    public YoungUserAuthResult YoungUserAuth(string SPID, string AuthenName, string AuthenType, string Password, string ExtendField)
    {
        YoungUserAuthResult Result = new YoungUserAuthResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        string ProvinceID = "";
        string Areaid = "";
        string RealName = "";
        string UserName = "";
        string NickName = "";
        string CustType = "";
        string OutID = "";
        string CustID = "";
        string TestStr = "";
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("YoungUserAuth 参数:SPID:{0},AuthenName:{1},Password:{2},ExtendField:{3}\r\n",SPID,AuthenName,Password,ExtendField);
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

            if (CommonUtility.IsEmpty(Password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            #endregion
            strLog.AppendFormat("根据手机号码查询所在地区省ID\r\n");
            Result.Result = BTForBusinessSystemInterfaceRules.GetPhoneTOArea(AuthenName, out ProvinceID, out Areaid, out Result.ErrorDescription);
            strLog.AppendFormat("根据手机号码查询所在地区省ID返回:Result:{0},ErrMsg:{1},ProvinceID:{2},AreaId:{3}\r\n",Result.Result,Result.ErrorDescription,ProvinceID,Areaid);
            strLog.AppendFormat("飞young认证:参数,ProvinceID:{0},AuthenType:{1},AuthenName:{2},Password:{3}\r\n", ProvinceID, AuthenType, AuthenName, Password);
            Result.Result = CrmSSO.YoungUserAuth(ProvinceID, AuthenType, AuthenName, "", Password, "1", "", Context, out RealName, out UserName,
               out NickName, out OutID, out CustType, out CustID, out Result.ErrorDescription, out TestStr);
            strLog.AppendFormat("飞young认证返回:Result:{0},ErrMsg:{1},OutID:{2},CustID:{3}\r\n",Result.Result,Result.ErrorDescription,OutID,CustID);
            Result.CustID = CustID;
            Result.OuterID = OutID;
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
            strLog.AppendFormat(ex.ToString());
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("YoungUserAuth", strLog);
            CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, ProvinceID, AuthenType, AuthenName, "1", Result.Result, Result.ErrorDescription);

        }


        return Result;
    }


    public class UserAuthV4Result
    {
        public int Result;
        public string CustID;
        public string UserAccount;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "密码认证V4接口")]
    public UserAuthV4Result UserAuthV4(string SPID, string AuthenName, string AuthenType, string Password, string ExtendField)
    {
        logger.Info("UserAuthV4");
        UserAuthV4Result Result = new UserAuthV4Result();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        string Ticket = "";
        string IsNeedLogin = "";
        string outerid = "";
        string ProvinceID = "";
        string IsQuery = "";
        string PwdType = "";
        try
        {
            #region 数据校验
            logger.Info("UserAuthV4-SPID校验:"+SPID);
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }
            logger.Info("UserAuthV4-AuthenName校验:"+AuthenName);
            if (CommonUtility.IsEmpty(AuthenName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }
            logger.Info("UserAuthv4-AuthenType校验:"+AuthenType);
            if (CommonUtility.IsEmpty(AuthenType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }
            logger.Info("UserAuthV4-ExtendField校验:"+ExtendField);
            if (!CommonUtility.IsEmpty(ExtendField))
            {
                ProvinceID = CommonBizRules.GetValueFromXmlStr(ExtendField, "ProvinceID");
                IsNeedLogin = CommonBizRules.GetValueFromXmlStr(ExtendField, "IsNeedLogin");
                IsQuery = CommonBizRules.GetValueFromXmlStr(ExtendField, "IsQuery");
                PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");
                logger.Info("ProvinceID=" + ProvinceID + "IsNeedLogin=" + IsNeedLogin + "IsQuery=" + IsQuery + "PwdType=" + PwdType);
            }
            logger.Info("UserAuthV4-Password校验:"+Password);
            if (CommonUtility.IsEmpty(Password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            #endregion

            string CustType = "";
            string RealName = "";
            string UserName = "";
            string NickName = "";
            string CustAddress = "";
            string ResType = "";
            string RspCode = ""; 
            string RspDesc = "";
            string AuthenticationKey = "";
            string TestStr = "";
            QryCustInfoV2Return qryCustInfoReturn = null;
            Result.Result = BTForBusinessSystemInterfaceRules.UserAuthV4(SPID, AuthenName, AuthenType, Password,
                this.Context, ProvinceID, IsQuery, PwdType,
                out Result.ErrorDescription, out Result.CustID, out Result.UserAccount, out CustType, out outerid, out ProvinceID, out  RealName, out  UserName, out  NickName, out CustAddress, out ResType, out RspCode, out RspDesc, out AuthenticationKey, out TestStr, out qryCustInfoReturn);
            logger.Info("UserAuthV4-BTForBusinessSystemInterfaceRules.UserAuthV4-Result=" + Result.Result);
            logger.Info("RealName=" + RealName + "CustAddress=" + CustAddress + "ResType=" + ResType + "RspCode=" + RspCode + "RspDesc=" + RspDesc + "AuthenticationKey=" + AuthenticationKey);
            
            if (IsNeedLogin == "1")
            {
                //生成ticket
                Ticket = CommonBizRules.CreateTicket();
                string errMsg = "";
                int iCIPTicket = CIPTicketManager.insertCIPTicket(Ticket, SPID, Result.CustID, RealName, NickName, UserName, outerid, Result.ErrorDescription, AuthenName, AuthenType, out errMsg);
                StringBuilder t_msg = new StringBuilder();
                t_msg.Append("AuthenName=" + AuthenName + ";isneedlogin = 1;iCIPTicket=" + Convert.ToString(iCIPTicket));
                BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthV4", t_msg);
                if (iCIPTicket != 0)
                {
                    Result.Result = iCIPTicket;
                    Result.ErrorDescription = errMsg;
                }
            }

            Result.ExtendField = BTBizRules.GenerateOuterIDXmlV3(outerid, ProvinceID, Ticket, CustAddress, ResType, RspCode, RspDesc, AuthenticationKey, qryCustInfoReturn);
        }
        catch (Exception e)
        {
            logger.Info(e.StackTrace);
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append(ExtendField + "\r\n\r\n");
                msg.Append("密码认证鉴权V4接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";AuthenName - " + AuthenName);
                msg.Append(";AuthenType - " + AuthenType);
                msg.Append(";Password - " + Password);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthV4", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},AuthenName:{1},AuthenType:{2},Password:{3},ExtendField:{4}", SPID, AuthenName, AuthenType, Password, ExtendField);
                String outParam = String.Format("CustID:{0},UserAccount:{1}", Result.CustID, Result.UserAccount);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "UserAuthV2", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                CommonBizRules.WriteDataCustAuthenLog(SPID, Result.CustID, ProvinceID, AuthenType, AuthenName, "1", Result.Result, Result.ErrorDescription);
            }
            catch { }

        }
        
        
        
        return Result;
    }

    /// <summary>
    /// 
    /// author lihongtu
    /// 日期 2014-06-14
    /// </summary>
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
        string IsQuery = "";
        string PwdType = "";
       
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
                PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");
            }

            if (CommonUtility.IsEmpty(Password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            #endregion

            string CustType = "";
            string RealName = "";
            string UserName = "";
            string NickName = "";

           
            Result.Result = BTForBusinessSystemInterfaceRules.UserAuthV2(SPID, AuthenName, AuthenType, Password,
                this.Context, ProvinceID, IsQuery, PwdType,
                out Result.ErrorDescription, out Result.CustID, out Result.UserAccount, out CustType, out outerid, out ProvinceID, out  RealName, out  UserName, out  NickName);
            if (IsNeedLogin == "1")
            {
                //生成ticket
                Ticket = CommonBizRules.CreateTicket();
                string errMsg = "";
                int iCIPTicket = CIPTicketManager.insertCIPTicket(Ticket, SPID, Result.CustID, RealName, NickName, UserName, outerid, Result.ErrorDescription, AuthenName, AuthenType, out errMsg);
                StringBuilder t_msg = new StringBuilder();
                t_msg.Append("AuthenName=" + AuthenName + ";isneedlogin = 1;iCIPTicket=" + Convert.ToString(iCIPTicket));
                BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthV2", t_msg);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append(ExtendField + "\r\n\r\n");
                msg.Append("密码认证鉴权V2接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";AuthenName - " + AuthenName);
                msg.Append(";AuthenType - " + AuthenType);
                msg.Append(";Password - " + Password);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; UserAccount - " + Result.UserAccount);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthV2", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},AuthenName:{1},AuthenType:{2},Password:{3},ExtendField:{4}", SPID, AuthenName, AuthenType, Password, ExtendField);
                String outParam = String.Format("CustID:{0},UserAccount:{1}", Result.CustID, Result.UserAccount);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "UserAuthV2", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                CommonBizRules.WriteDataCustAuthenLog(SPID, Result.CustID, ProvinceID, AuthenType, AuthenName, "1", Result.Result, Result.ErrorDescription);
            }
            catch { }

        }
        return Result;

    }


    public class UnifyAccountAuthResult
    {
        public String CustID;
        public String loginNum;
        public String accessToken;
        public String msg;
        public int result;
        public long expiresIn;
        public UnifyAccountInfo unifyAccountInfo;
    
    }
    [WebMethod(Description = "天翼账号认证接口")]
    public UnifyAccountAuthResult UnifyAccountAuth(String SPID,String UserName,String PassWord,String ExtendField)
    {
        UnifyAccountAuthResult Result = new UnifyAccountAuthResult();
        Result.result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.msg = ErrorDefinition.IError_Result_UnknowError_Msg;
        String PwdType = "";
        StringBuilder msg = new StringBuilder();
        msg.AppendFormat("SPID:{0},UserName:{1},PassWord:{2},ExtendField:{3}\r\n",SPID,UserName,PassWord,ExtendField);
        #region 数据校验
        if (CommonUtility.IsEmpty(SPID))
        {
            Result.result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
            Result.msg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
            return Result;
        }

        if (CommonUtility.IsEmpty(UserName))
        {
            Result.result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
            Result.msg = "用户名不能为空";
            return Result;
        }


        if (!CommonUtility.IsEmpty(ExtendField))
        {
            PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");

        }

        if (CommonUtility.IsEmpty(PassWord))
        {
            Result.result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
            Result.msg = "密码不能为空";
            return Result;
        }

        #endregion
        String MobileName = String.Empty;
        String EmailName = String.Empty;
        #region  综合平台认证
        try
        {
            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;
            msg.AppendFormat("获取综合平台接入参数:appkey:{0},appSecret:{1},version:{2},clientType:{3},clientIp:{4},clientAgent:{5}\r\n",appId,appSecret,version,clientType,clientIp,clientAgent);
            if (String.IsNullOrEmpty(appId) || String.IsNullOrEmpty(appSecret) || String.IsNullOrEmpty(version) || String.IsNullOrEmpty(clientType) || String.IsNullOrEmpty(clientAgent))
            {
                Result.result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.msg = "综合平台参数获取失败";
                return Result;
            }

            
            UnifyAccountInfo account = new UnifyAccountInfo();
            String accessToken = String.Empty;
            msg.Append("开始登录天翼账号\r\n");
            Result.result = CIP2BizRules.LoginUnifyPlatform(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, HttpUtility.UrlEncode(PassWord), out  Result.unifyAccountInfo, out  Result.accessToken, out Result.loginNum, out  Result.expiresIn, out  Result.msg);
            msg.AppendFormat("登录天翼账号返回：Result.result:{0},msg:{1},accesstoken:{2},loginnum:{3}\r\n",Result.result,Result.msg,Result.accessToken,Result.loginNum);
            if (Result.unifyAccountInfo != null && !String.IsNullOrEmpty(Result.loginNum))
            {
                //增加绑定动作 根据loginnum
                #region
                String CustID, OuterID, Status, CustType, CustLevel, RealName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                CustID = String.Empty;
  
                System.Text.RegularExpressions.Regex regMobile = new System.Text.RegularExpressions.Regex(@"^1[345678]\d{9}$");
                System.Text.RegularExpressions.Regex regEmail = new System.Text.RegularExpressions.Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");

                RealName = Result.loginNum;
                if (regMobile.IsMatch(Result.loginNum))
                {
                    MobileName = Result.loginNum;
                }

                if (regEmail.IsMatch(Result.loginNum))
                {
                    EmailName = Result.loginNum;
                }
                String EncrytpPassWord = CryptographyUtil.Encrypt(PassWord);
                String OperType = "2";

                msg.Append("【开始绑定号百】:\r\n");
                Result.result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", MobileName, EmailName, RealName, EncrytpPassWord, Result.unifyAccountInfo.userId, SPID, OperType, out CustID, out Result.msg);
                msg.Append("【开始绑定到号百的结果】:\r\n");
                msg.AppendFormat("Result:{0},CustID:{1}<->UserID:{2}ErrMsg:{3}\r\n", Result.result, CustID, Result.unifyAccountInfo.userId,Result.msg);
                #endregion

                //保存accessToken
                if (Result.result == 0 && !String.IsNullOrEmpty(CustID))  //绑定成功
                {
                    Result.CustID = CustID;
                    msg.Append("绑定成功，记录Accesstoken\r\n");
                    String Description = "接口登录";
                    //Result.result = CIP2BizRules.InsertAccessToken(SPID, HttpContext.Current.Request.UserHostAddress.ToString(), Result.accessToken, Convert.ToString(Result.unifyAccountInfo.userId), CustID, RealName, Result.unifyAccountInfo.nickName, Result.loginNum, OperType, Description, out Result.msg);
                    Result.result = CIP2BizRules.InsertUnifyAccessToken(SPID, HttpContext.Current.Request.UserHostAddress.ToString(), Result.accessToken, Convert.ToString(Result.unifyAccountInfo.userId), CustID, RealName, Result.unifyAccountInfo.nickName, Result.loginNum,PassWord, OperType, Description, out Result.msg);

                    //InsertUnifyAccessToken
                    msg.AppendFormat("记录AccessToken结果,Result:{0},CustID:{1}<->AccessToken:{2}\r\n",Result.result,CustID,accessToken);
                }
                else
                {
                    msg.Append("绑定失败,无法记录AccessToken,\r\n");
                }
                msg.AppendFormat("userid:{0}\r\n", Result.unifyAccountInfo.userId);
            }
            //else
            //{
                #region 天翼账号认证失败(可能因为同一个号码，天翼账号的密码和本地密码不一致导致，也可能是天翼账号根本不存在)本地数据查询认证
//                String Unify_deadline = System.Configuration.ConfigurationManager.AppSettings["Unify_deadline"];

                //try
                //{

                //    SqlCommand cmd = new SqlCommand();
                //    cmd.CommandTimeout = 15;
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandText = "dbo.up_Customer_OV3_Interface_UserAuthV2";

                //    SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                //    parSPID.Value = SPID;
                //    cmd.Parameters.Add(parSPID);

                //    SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
                //    parAuthenName.Value = UserName;
                //    cmd.Parameters.Add(parAuthenName);

                //    SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                //    parAuthenType.Value = "2";
                //    cmd.Parameters.Add(parAuthenType);

                //    SqlParameter parPwd = new SqlParameter("@Pwd", SqlDbType.VarChar, 100);
                //    parPwd.Value = CryptographyUtil.Encrypt(PassWord);
                //    cmd.Parameters.Add(parPwd);


                //    if (String.IsNullOrEmpty(PwdType))
                //    {
                //        PwdType = "1";
                //    }

                //    SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 100);
                //    parPwdType.Value = PwdType;
                //    cmd.Parameters.Add(parPwdType);

                //    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                //    parResult.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parResult);

                //    SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                //    parErrMsg.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parErrMsg);

                //    SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                //    parCustID.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parCustID);

                //    SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                //    parUserAccount.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parUserAccount);

                //    SqlParameter parCustType = new SqlParameter("@CustType ", SqlDbType.VarChar, 2);
                //    parCustType.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parCustType);

                //    SqlParameter parUProvinceID = new SqlParameter("@UProvinceID ", SqlDbType.VarChar, 2);
                //    parUProvinceID.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parUProvinceID);

                //    SqlParameter parSysID = new SqlParameter("@SysID ", SqlDbType.VarChar, 8);
                //    parSysID.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parSysID);

                //    SqlParameter parAreaID = new SqlParameter("@AreaID ", SqlDbType.VarChar, 3);
                //    parAreaID.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parAreaID);

                //    SqlParameter parOuterID = new SqlParameter("@outerid ", SqlDbType.VarChar, 30);
                //    parOuterID.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parOuterID);

                //    SqlParameter parUserName = new SqlParameter("@UserName ", SqlDbType.VarChar, 30);
                //    parUserName.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parUserName);

                //    SqlParameter parRealName = new SqlParameter("@RealName ", SqlDbType.VarChar, 30);
                //    parRealName.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parRealName);

                //    SqlParameter parNickName = new SqlParameter("@NickName ", SqlDbType.VarChar, 30);
                //    parNickName.Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(parNickName);

                //    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                //    Result.result = int.Parse(parResult.Value.ToString());
                //    Result.msg = parErrMsg.Value.ToString();
                //    Result.CustID = parCustID.Value.ToString();
                //    String hbUserAccount = parUserAccount.Value.ToString();
                //    String hbCustType = parCustType.Value.ToString();
                //    String hbUProvinceID = parUProvinceID.Value.ToString();
                //    String hbSysID = parSysID.Value.ToString();
                //    String hbAreaID = parAreaID.Value.ToString();
                //    String hbouterid = parOuterID.Value.ToString();
                //    String ProvinceID1 = hbUProvinceID;
                //    String hbRealName = parRealName.Value.ToString();
                //    String hbUserName = parUserName.Value.ToString();
                //    String hbNickName = parNickName.Value.ToString();

                //    msg.AppendFormat("本地认证结果:{0},{1}\r\n", Result.result, Result.msg);
                    #region  如果天翼不存在该手机账号，则去天翼注册一把，回来绑定关系建立，如果存在，直接根据userid绑定custid

                    //if (Result.result == 0)
                    //{
                        //msg.Append("本地认证成功!\r\n");
                        //try
                        //{
                            //long t_userId = 0;
                            //msg.Append("检查该手机是否是天翼账号?\r\n");
                            //Result.result = CIP2BizRules.GetUnifyPlatformUserInfoByName(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, out t_userId, out Result.msg);
                            //msg.AppendFormat("检查结果GetUnifyPlatformUserInfoByName:Result:{0},ErrMsg:{1},UserID:{2}\r\n", Result.result, Result.msg, Convert.ToString(t_userId));
                            //if (t_userId > 0)
                            //{
                            //    //天翼存在
                            //    msg.Append("该手机号码存在于天翼账号库\r\n建立绑定关系\r\n");
                            //    Result.result = CIP2BizRules.BindCustId2UserId(SPID, "1", Result.CustID, t_userId, out Result.msg);
                            //    msg.AppendFormat("绑定结果:Result:{0},CustID:{1}<->UserID:{2}\r\n", Result.result, Result.CustID, t_userId);
                            //}
                            //else
                            //{
                                //天翼不存在
                                //msg.Append("该手机号码不存在于天翼账号库\r\n注册天翼账号\r\n");
                                //string o_userId = String.Empty;
                                //string o_userName = String.Empty;

                                //Result.result = CIP2BizRules.RegisterUnifyPlatformAccount(appId, appSecret, version, clientType, clientIp, clientAgent, UserName, PassWord, "false", out o_userId, out o_userName, out accessToken, out Result.msg);
                                //msg.AppendFormat("注册结果:Result:{0},ErrMsg:{1},UserID:{2}\r\n", Result.result, Result.msg, o_userId);
                                //if (Result.result == 0 && !String.IsNullOrEmpty(o_userId))
                                //{
                                //    long l_userId = 0;
                                //    try
                                //    {
                                //        l_userId = Convert.ToInt64(o_userId);
                                //    }
                                //    catch (Exception ee)
                                //    {
                                //        msg.AppendFormat("异常ee:{0}\r\n", ee.Message);
                                //    }

                                //    if (l_userId != 0)
                                //    {
                                //        msg.Append("建立绑定关系\r\n");
                                //        Result.result = CIP2BizRules.BindCustId2UserId(SPID, "1", Result.CustID, l_userId, out Result.msg);
                                //        Result.msg = Result.msg + ",天翼账号认证不成功，本地认证成功！";
                                //        msg.AppendFormat("绑定结果:Result:{0},CustID{1}<->UserID{2}\r\n", Result.result, Result.CustID, l_userId);
                                //    }
                                //}



                            //}
                        //}
                        //catch (Exception et)
                        //{
                        //    msg.AppendFormat("异常:{0}\r\n", et.Message);
                        //}
                    //}
                    #endregion
                //}
                //catch (Exception excep)
                //{
                //    msg.AppendFormat("登录失败,发生异常:{0}\r\n", excep.Message);
                //}
                #endregion
            //}
        }
        catch (Exception e)
        {
            msg.AppendFormat("登录失败，发生异常:{0}"+e.ToString());
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("UnifyAccountAuth", msg);
        }
        #endregion
        return Result;
    }

    public class UserAuthV3Result
    {
        public Int32 Result;
        public String CustID;
        public String UserAccount;
        public String AuthenType;
        public String ErrorDescription;
        public String ExtendField;
    }
    [WebMethod(Description="密码认证V3接口")]
    public UserAuthV3Result UserAuthV3(String SPID, String AuthenName, String Password, String ExtendField)
    {
        UserAuthV3Result Result = new UserAuthV3Result();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;

        //客户的属性
        String Ticket = "", CustID = "", CustType = "", AuthenType = "", UserName = "", UserAccount = "", RealName = "", NickName = "", OuterID = "", SysID = "", AreaID = "", OutProvinceID = "";
        String ProvinceID = "", IsNeedLogin = "", IsQuery = "", PwdType = "";
        try
        {
            #region 数据有限性验证
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

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "UserAuthV3", this.Context, out Result.ErrorDescription);
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

            if (CommonUtility.IsEmpty(Password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            if (!CommonUtility.IsEmpty(ExtendField))
            {
                ProvinceID = CommonBizRules.GetValueFromXmlStr(ExtendField, "ProvinceID");
                IsNeedLogin = CommonBizRules.GetValueFromXmlStr(ExtendField, "IsNeedLogin");
                IsQuery = CommonBizRules.GetValueFromXmlStr(ExtendField, "IsQuery");
                PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");
            }

            #endregion

            Result.Result = BTForBusinessSystemInterfaceRules.UserAuthV3(SPID, AuthenName, Password, PwdType, out CustID, out CustType, out AuthenType, 
                out UserName, out UserAccount, out RealName, out NickName, out SysID, out OuterID, out OutProvinceID, out AreaID, out Result.ErrorDescription);
            if (Result.Result != 0)
                return Result;

            Result.CustID = CustID;
            Result.UserAccount = UserAccount;
            Result.AuthenType = AuthenType;

            //如果需要登录则生成ticket并返回
            if (IsNeedLogin == "1")
            {
                Ticket = CommonBizRules.CreateTicket();
                Int32 result = ErrorDefinition.IError_Result_UnknowError_Code;
                String errmsg = ErrorDefinition.IError_Result_UnknowError_Msg;
                result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, UserName, NickName, OuterID, Result.ErrorDescription, AuthenName, Result.AuthenType, out errmsg);
                if (result != 0)
                {
                    Result.Result = result;
                    Result.ErrorDescription = errmsg;
                }
            }
            //else
            //{
            //    Result.CustID = CustID;
            //    Result.UserAccount = UserAccount;
            //    Result.AuthenType = AuthenType;
            //}

            Result.ExtendField = BTBizRules.GenerateOuterIDXml(OuterID, ProvinceID, Ticket);
        }
        catch (Exception ex)
        {
            Result.ErrorDescription += ex.Message;
        }
        finally
        {
            try
            {
                #region 文本日志

                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                msg.AppendFormat("【密码认证接口V3,DateTime:{0}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                msg.Append("输入参数—SPID:" + SPID);
                msg.Append(",AuthenName:" + AuthenName);
                msg.Append(",Password:" + Password);
                msg.Append(",ExtendField:" + ExtendField);

                msg.Append("\r\n");
                msg.Append("处理结果—Ticket:" + Ticket);
                msg.Append(",CustID:" + CustID);
                msg.Append(",CustType:" + CustType);
                msg.Append(",AuthenType:" + AuthenType);
                msg.Append(",UserName:" + UserName);
                msg.Append(",UserAccouont:" + UserAccount);
                msg.Append(",RealName:" + RealName);
                msg.Append(",NickName:" + NickName);
                msg.Append(",SysID:" + SysID);
                msg.Append(",OuterID:" + OuterID);
                msg.Append(",ProvinceID:" + OutProvinceID);
                msg.Append(",AreaID:" + AreaID);
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthV3", msg);

                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},AuthenName:{1},Password:{2},ExtendField:{3}", SPID, AuthenName, Password, ExtendField);
                String outParam = String.Format("Ticket:{0},CustID:{1},CustType:{2},AuthenType:{3},UserName:{4},UserAccount:{5},RealName:{6},NickName:{7},SysID:{8},OuterID:{9},ProvinceID:{10},AreaID:{11}",
                    Ticket, CustID, CustType, AuthenType, UserName, UserAccount, RealName, NickName, SysID, OuterID, OutProvinceID, AreaID);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress,SPID, "UserAuthV3", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }

        return Result;
    }

    public class TicketParseResult
    {
        public int Result;
        public string CustID;
        public string RealName;
        public string UserName;
        public string NickName;
        public string OuterID;
        public string LoginAuthenName;
        public string LoginAuthenType;
        public string ExtendField;
        public string ErrorDescription;
    }
    [WebMethod(Description = "客户信息平台票据解读接口")]
    public TicketParseResult TicketParse(string SPID, string Ticket, string ExtendField)
    {
        TicketParseResult Result = new TicketParseResult();

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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "TicketParse", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(Ticket))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_TicketError_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }

            #endregion

            Result.Result = CIPTicketManager.checkCIPTicket(SPID, Ticket, ExtendField, out Result.CustID, out Result.RealName, out Result.UserName, out Result.NickName, out Result.OuterID, "", out Result.LoginAuthenName, out Result.LoginAuthenType, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户信息平台票据解读接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";SPID - " + SPID);
                msg.Append(";Ticket - " + Ticket);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; RealName - " + Result.RealName);
                msg.Append("; UserName - " + Result.UserName);
                msg.Append("; NickName - " + Result.NickName);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("TicketParse", msg);
                #endregion
            }
            catch { }
        }

        return Result;

    }

    #endregion

    #region 客户信息服务接口

    #region 客户基本信息查询接口(周涛)

    /// <summary>
    /// 客户基本信息查询接口返回记录
    /// 作者：周涛      时间：2009-7-31
    /// 修改：          时间：
    /// </summary>
    public class CustBasicInfoQueryResult
    {
        public int Result;
        public string ErrorDescription;
        public string CustID;
        public string ProvinceID;
        public string AreaID;
        public string OuterID;
        public string Status;
        public string CustType;
        public string CustLevel;
        public string RealName;
        public string UserName;
        public string NickName;
        public string CertificateCode;
        public string CertificateType;
        public string Sex;
        public string Email;
        //public string EnterpriseID;
        public PhoneRecord[] PhoneRecords;
        public TourCardIDRecord[] TourCardIDRecords;
        public string ExtendField;
    }

    /// <summary>
    /// 客户基本信息查询接口
    /// 作者：周涛      时间：2009-7-31
    /// 修改：          时间：
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "客户基本信息查询接口")]
    public CustBasicInfoQueryResult CustBasicInfoQuery(string SPID, string CustID, string ExtendField)
    {
        CustBasicInfoQueryResult Result = new CustBasicInfoQueryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        Result.CustID = CustID;
        Result.ProvinceID = "";
        Result.AreaID = "";
        Result.OuterID = "";
        Result.Status = "";
        Result.CustType = "";
        Result.CustLevel = "";
        Result.RealName = "";
        Result.UserName = "";
        Result.NickName = "";
        Result.CertificateCode = "";
        Result.CertificateType = "";
        Result.Sex = "";
        Result.Email = "";
        Result.PhoneRecords = null;
        Result.TourCardIDRecords = null;
        Result.ExtendField = "";

        //企业ID
        string EnterpriseID = "";
        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "CustBasicInfoQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            
            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
            }

            if (CommonUtility.IsEmpty(SPID) && CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "关键字信息不能全为空";
                return Result;
            }
            #endregion

            //客户信息同步使用
            string Registration = "";

            //数据库操作
            Result.Result = CustBasicInfo.getCustInfo(SPID, CustID, out Result.ErrorDescription, out Result.OuterID, out Result.Status, out Result.CustType,
                                          out Result.CustLevel, out Result.RealName, out Result.UserName, out Result.NickName, out Result.CertificateCode,
                                          out Result.CertificateType, out Result.Sex, out Result.Email, out EnterpriseID, out Result.ProvinceID, out Result.AreaID, out Registration);

            string Birthday = "";
            string EduLevel = "";
            string Favorite = "";
            string IncomeLevel = "";
            int ExtendResult = CustExtendInfo.getCustExtendInfo(SPID, CustID, out Result.ErrorDescription, out Birthday,out EduLevel, out Favorite, out IncomeLevel);

            Result.ExtendField = "<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><EnterpriseID>" + EnterpriseID + "</EnterpriseID> <Birthday>" + Birthday + " </Birthday><CreateTime>" + Registration + "</CreateTime> </Root>";

            if (Result.Result != 0)
            {
                return Result;
            }
            string ErrMsg = "";
            int QueryResult = -1;
            //返回客户电话记录
            Result.PhoneRecords = CustBasicInfo.getPhoneRecord(CustID, out QueryResult, out ErrMsg);
            Result.ErrorDescription += ErrMsg;

            //返回客户商旅卡记录
            Result.TourCardIDRecords = CustBasicInfo.getTourCardIDRecord(CustID, out QueryResult, out ErrMsg);
            Result.ErrorDescription += ErrMsg;
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户基本信息查询接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append(": ProvinceID - " + Result.ProvinceID);
                msg.Append(": AreaID - " + Result.AreaID);
                msg.Append(": OuterID - " + Result.OuterID);
                msg.Append(": Status - " + Result.Status);
                msg.Append(": CustType - " + Result.CustType);
                msg.Append(": CustLevel - " + Result.CustLevel);
                msg.Append(": RealName - " + Result.RealName);
                msg.Append(": UserName - " + Result.UserName);
                msg.Append(": NickName - " + Result.NickName);
                msg.Append(": CertificateCode - " + Result.CertificateCode);
                msg.Append(": CertificateType - " + Result.CertificateType);
                msg.Append(": Sex -" + Result.Sex);
                msg.Append(": Email - " + Result.Email);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                if (Result.PhoneRecords != null)
                {
                    if (Result.PhoneRecords.Length > 0)
                    {
                        msg.Append("PhoneRecords: \r\n");
                        for (int i = 0; i < Result.PhoneRecords.Length; i++)
                        {
                            msg.Append(" Phone- " + Result.PhoneRecords[i].Phone);
                            msg.Append(" Phone- " + Result.PhoneRecords[i].PhoneClass);
                        }
                        msg.Append("\r\n");
                    }
                }

                if (Result.TourCardIDRecords != null)
                {
                    if (Result.TourCardIDRecords.Length > 0)
                    {
                        msg.Append("TourCardIDRecords: \r\n");
                        for (int i = 0; i < Result.TourCardIDRecords.Length; i++)
                        {
                            msg.Append(" CardID- " + Result.TourCardIDRecords[i].CardID);
                        }
                        msg.Append("\r\n");
                    }
                }

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("CustBasicInfoQuery", msg);
                #endregion

                #region 数据库日志

                //传入参数
                String inParameters = String.Format("SPID:{0},CustID:{1},ExtendField:{2}", SPID, CustID, ExtendField);
                //返回参数
                StringBuilder outParameters = new StringBuilder();
                outParameters.Append("SPID:" + SPID);
                outParameters.Append(", CustID:" + Result.CustID);
                outParameters.Append(", ProvinceID:" + Result.ProvinceID);
                outParameters.Append(", AreaID:" + Result.AreaID);
                outParameters.Append(", OuterID:" + Result.OuterID);
                outParameters.Append(", Status:" + Result.Status);
                outParameters.Append(", CustType:" + Result.CustType);
                outParameters.Append(", CustLevel:" + Result.CustLevel);
                outParameters.Append(", RealName:" + Result.RealName);
                outParameters.Append(", UserName:" + Result.UserName);
                outParameters.Append(", NickName:" + Result.NickName);
                outParameters.Append(", CertificateCode:" + Result.CertificateCode);
                outParameters.Append(", CertificateType:" + Result.CertificateType);
                outParameters.Append(", Sex -" + Result.Sex);
                outParameters.Append(", Email:" + Result.Email);
                outParameters.Append(", ExtendField:" + Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "CustBasicInfoQuery", inParameters, outParameters.ToString(), Result.Result, Result.ErrorDescription);

                //CommonBizRules.WriteDataLog(SPID, CustID, "", Result.Result, Result.ErrorDescription, "", "CustBasicInfoQuery");
                
                #endregion
                
            }
            catch (Exception ex)
            { }
        }
        return Result;
    }

    #endregion

    #region 客户扩展信息查询接口(周涛)

    public class CustExtendInfoQueryResult
    {
        public int Result;
        public string ErrorDescription;
        public string CustID;
        public string Birthday;
        public string EduLevel;
        public string Favorite;
        public string IncomeLevel;
        public string ExtendField;
    }

    [WebMethod(Description = "客户扩展信息查询接口")]
    public CustExtendInfoQueryResult CustExtendInfoQuery(string SPID, string CustID, string ExtendField)
    {

        CustExtendInfoQueryResult Result = new CustExtendInfoQueryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.CustID = CustID;
        Result.Birthday = "";
        Result.EduLevel = "";
        Result.Favorite = "";
        Result.IncomeLevel = "";
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "CustExtendInfoQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(SPID) && CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "关键字信息不能全为空";
                return Result;
            }
            #endregion

            //数据库操作
            Result.Result = CustExtendInfo.getCustExtendInfo(SPID, CustID, out Result.ErrorDescription, out Result.Birthday,
                                                           out Result.EduLevel, out Result.Favorite, out Result.IncomeLevel);
            
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
                #region 文本日志

                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户扩展信息查询接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("CustExtendInfoQuery", msg);

                #endregion

                #region 写数据库日志

                String inParameters = String.Format("SPID:{0},CustID:{1},ExtendFidld:{2}", SPID, CustID, ExtendField);
                StringBuilder outParameters = new StringBuilder();
                outParameters.Append("SPID:" + SPID);
                outParameters.Append(",CustID:" + Result.CustID);
                outParameters.Append(",ExtendField:" + Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "CustExtendInfoQuery", inParameters, outParameters.ToString(), Result.Result, Result.ErrorDescription);

                //CommonBizRules.WriteDataLog(SPID, CustID, "", Result.Result, Result.ErrorDescription, "", "CustExtendInfoQuery");
                #endregion
            }
            catch (Exception ex)
            { }
        }

        return Result;
    }

    #endregion

    #region 客户信息变更接口

    public class CustBasicInfoModifyResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
    }

    [WebMethod(Description = "客户信息变更接口")]
    public CustBasicInfoModifyResult CustBasicInfoModify(string SPID, string CustID, string RealName,
        string CertificateCode, string CertificateType, string Sex, string Email, string ExtendField)
    {
        CustBasicInfoModifyResult Result = new CustBasicInfoModifyResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.CustID = CustID;
        Result.ErrorDescription = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(RealName)
                && CommonUtility.IsEmpty(CertificateCode)
                && CommonUtility.IsEmpty(CertificateType)
                && CommonUtility.IsEmpty(Sex))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "修改参数不能全为空";
                return Result;
            }

            #endregion

            Result.Result = CustBasicInfo.UpdateCustinfo(SPID, CustID, RealName, CertificateCode, CertificateType, Sex, Email, out Result.ErrorDescription);
            if (Result.Result == 0)
            {
                //通知积分平台
                CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrorDescription);
                // 这里要判断该客户是否已经开过户，如果是开户的，并且修改了身份证的才同步
                CIP2BizRules.NotifyBesttoneAccountInfo(SPID, Result.CustID, out Result.ErrorDescription);   // 通知融合支付
            
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
                #region 文本日志

                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户信息变更接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":CustID - " + CustID);
                msg.Append(":RealName - " + RealName);
                msg.Append(":CertificateCode - " + CertificateCode);
                msg.Append(":CertificateType - " + CertificateType);
                msg.Append(":sex - " + Sex);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID + "\r\n");
                //msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("CustBasicInfoModify", msg);

                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},RealName:{2},CertificateCode:{3},CertificateType:{4},Sex:{5},Email:{6},ExtendField:{7}",
                    SPID, CustID, RealName, CertificateCode, CertificateType, Sex, Email, ExtendField);
                String outParam = String.Empty;
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "CustBasicInfoModify", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Result.Result, Result.ErrorDescription, "", "CustBasicInfoModify");
            }
            catch { }
        }


        return Result;
    }

    #endregion

    #region 客户信息变更接口


    public class CustBasicInfoMofityExtV2Result
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
    
    }
    [WebMethod(Description = "客户信息变更接口v2")]
    public CustBasicInfoMofityExtV2Result CustBasicInfoModifyExtV2(string SPID, string CustID, string RealName,
       string Birthday,string CertificateCode, string CertificateType, string Sex, string Email, string NickName, string ExtendField)
    {
        CustBasicInfoMofityExtV2Result Result = new CustBasicInfoMofityExtV2Result();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.CustID = CustID;
        Result.ErrorDescription = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }
            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(RealName)
                && CommonUtility.IsEmpty(CertificateCode)
                && CommonUtility.IsEmpty(CertificateType)
                && CommonUtility.IsEmpty(Sex)
                && CommonUtility.IsEmpty(NickName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "修改参数不能全为空";
                return Result;
            }


            if (!String.IsNullOrEmpty(Birthday) &&  !Utils.IsDate(Birthday))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "生日格式不合法";
                return Result;
            }


            #endregion

            Result.Result = CustBasicInfo.UpdateCustinfoExtV2(SPID, CustID, RealName, CertificateCode, CertificateType, Birthday,Sex, Email, NickName, out Result.ErrorDescription);
            if (Result.Result == 0)
            {
                //通知积分平台
                CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrorDescription);
                
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
                #region 文本日志

                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户信息变更接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("客户信息变更接口" + this.Context.Request.Url.AbsolutePath.ToString() + "\r\n");
                msg.Append("客户信息变更接口" + this.Context.Request.Url.AbsoluteUri.ToString()+ "\r\n");
                msg.Append("客户信息变更接口" + this.Context.Request.Url.Host.ToString() + "\r\n");
                msg.Append("客户信息变更接口" + this.Context.Request.Url.UserInfo.ToString() + "\r\n");
                msg.Append("客户信息变更接口" + this.Context.Request.UserHostAddress.ToString() + "\r\n");
                msg.Append("客户信息变更接口" + this.Context.Request.UserHostName.ToString() + "\r\n");

                //HttpContext.Current.Request.UserHostAddress
                //this.Context.Request.Url.AbsolutePath;
                //this.Context.Request.Url.AbsoluteUri;
                //this.Context.Request.Url.Host;
                //this.Context.Request.Url.UserInfo;
               
                msg.Append(";SPID - " + SPID);
                msg.Append(":CustID - " + CustID);
                msg.Append(":RealName - " + RealName);
                msg.Append(":CertificateCode - " + CertificateCode);
                msg.Append(":CertificateType - " + CertificateType);
                msg.Append(":sex - " + Sex);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID + "\r\n");
                //msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("CustBasicInfoModifyExtV2", msg);

                #endregion

                #region 写数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},RealName:{2},CertificateCode:{3},CertificateType:{4},Sex:{5},Email:{6},NickName:{7},ExtendField:{8}",
                    SPID, CustID, RealName, CertificateCode, CertificateType, Sex, Email, NickName, ExtendField);
                String outParam = String.Empty;

                //CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "CustBasicInfoModifyExt", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion
                CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Result.Result, Result.ErrorDescription, "", "CustBasicInfoModify");
            }
            catch { }
        }



        return Result;
    }


    public class CustBasicInfoModifyExtResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
    }

    [WebMethod(Description = "客户信息变更接口v2")]
    public CustBasicInfoModifyExtResult CustBasicInfoModifyExt(string SPID, string CustID, string RealName,
        string CertificateCode, string CertificateType, string Sex, string Email, string NickName, string ExtendField)
    {
        CustBasicInfoModifyExtResult Result = new CustBasicInfoModifyExtResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.CustID = CustID;
        Result.ErrorDescription = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }
            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(RealName)
                && CommonUtility.IsEmpty(CertificateCode)
                && CommonUtility.IsEmpty(CertificateType)
                && CommonUtility.IsEmpty(Sex)
                && CommonUtility.IsEmpty(NickName))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "修改参数不能全为空";
                return Result;
            }
            #endregion

            Result.Result = CustBasicInfo.UpdateCustinfoExt(SPID, CustID, RealName, CertificateCode, CertificateType, Sex, Email, NickName, out Result.ErrorDescription);
            if (Result.Result == 0)
            {
                //通知积分平台
                CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrorDescription);
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
                #region 文本日志

                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户信息变更接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":CustID - " + CustID);
                msg.Append(":RealName - " + RealName);
                msg.Append(":CertificateCode - " + CertificateCode);
                msg.Append(":CertificateType - " + CertificateType);
                msg.Append(":sex - " + Sex);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID + "\r\n");
                //msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("CustBasicInfoModify", msg);

                #endregion

                #region 写数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},RealName:{2},CertificateCode:{3},CertificateType:{4},Sex:{5},Email:{6},NickName:{7},ExtendField:{8}",
                    SPID, CustID, RealName, CertificateCode, CertificateType, Sex, Email, NickName, ExtendField);
                String outParam = String.Empty;

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "CustBasicInfoModifyExt", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Result.Result, Result.ErrorDescription, "", "CustBasicInfoModify");
            }
            catch { }
        }


        return Result;
    }

    #endregion

    #region 客户地址信息上传接口(周涛)
    public class AddressInfoUploadResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "客户地址信息上传接口")]
    public AddressInfoUploadResult AddressInfoUpload(string SPID, string CustID, AddressInfoRecord[] AddressInfoRecords, string ExtendField)
    {
        AddressInfoUploadResult Result = new AddressInfoUploadResult();

        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.CustID = CustID;
        Result.ErrorDescription = "";
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AddressInfoUploadQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
            }

            if (AddressInfoRecords == null)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "AddressInfoRecords为空";
                return Result;
            }
            #endregion

            Result.Result = AddressInfoBO.UploadAddressInfo(SPID, AddressInfoRecords, CustID, out Result.ErrorDescription);

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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户地址信息上传接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID + "\r\n");
                if (AddressInfoRecords != null)
                {
                    if (AddressInfoRecords.Length > 0)
                    {
                        msg.Append("AddressInfoRecords: \r\n");
                        for (int i = 0; i < AddressInfoRecords.Length; i++)
                        {
                            msg.Append("AddressID - " + AddressInfoRecords[i].AddressID);
                            msg.Append(" :Address - " + AddressInfoRecords[i].Address);
                            msg.Append(" :Zipcode - " + AddressInfoRecords[i].Zipcode);
                            msg.Append(" :Type - " + AddressInfoRecords[i].Type);
                            msg.Append(" :DealType - " + AddressInfoRecords[i].DealType);
                            msg.Append("ExtendField - " + AddressInfoRecords[i].ExtendField + "\r\n");
                        }
                    }
                }
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AddressInfoUploadQuery", msg);
                #endregion

                #region 数据库日志

                StringBuilder inParam = new StringBuilder();
                String outParam = String.Empty;

                inParam.Append("SPID:" + SPID);
                inParam.Append(",CustID:" + CustID);
                if (AddressInfoRecords != null && AddressInfoRecords.Length > 0)
                {
                    inParam.Append(",AddressInfo:");
                    foreach (AddressInfoRecord address in AddressInfoRecords)
                    {
                        inParam.Append("{");
                        inParam.AppendFormat("AddressID:{0},Address:{1},Zipcode:{2},Type:{3},DealType:{4},ExtendField:{5}",
                            address.AddressID, address.Address, address.Zipcode, address.Type, address.DealType, address.ExtendField);
                        inParam.Append("},");
                    }
                }
                inParam.Append(",ExtendField:" + ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "AddressInfoUpload", inParam.ToString(), outParam.ToString(), Result.Result, Result.ErrorDescription);
                #endregion

                //CommonBizRules.WriteDataLog(SPID, CustID, "", Result.Result, Result.ErrorDescription, "", "AddressInfoUploadQuery");
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 客户地址信息查询接口(周涛)
    public class AddressInfoQueryResult
    {
        public int Result;
        public string CustID;
        public AddressInfoRecord[] AddressInfoRecords;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "客户地址信息查询接口")]
    public AddressInfoQueryResult AddressInfoQuery(string SPID, string CustID, string ExtendField)
    {
        AddressInfoQueryResult Result = new AddressInfoQueryResult();

        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.CustID = CustID;
        Result.AddressInfoRecords = null;
        Result.ErrorDescription = "";
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AddressInfoQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
            }

            if (CommonUtility.IsEmpty(SPID) && CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "关键字信息不能全为空";
                return Result;
            }
            #endregion

            //数据库操作
            Result.AddressInfoRecords = AddressInfoBO.GetAddressInfo(SPID, CustID, out Result.Result, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("客户地址信息查询接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                if (Result.AddressInfoRecords != null)
                {
                    if (Result.AddressInfoRecords.Length > 0)
                    {
                        msg.Append("AddressInfoRecords: \r\n");
                        for (int i = 0; i < Result.AddressInfoRecords.Length; i++)
                        {
                            msg.Append("AddressID - " + Result.AddressInfoRecords[i].AddressID.ToString());
                            msg.Append(" :Address - " + Result.AddressInfoRecords[i].Address);
                            msg.Append(" :Zipcode - " + Result.AddressInfoRecords[i].Zipcode);
                            msg.Append(" :Type - " + Result.AddressInfoRecords[i].Type);
                            msg.Append(" :DealType - " + Result.AddressInfoRecords[i].DealType);
                            msg.Append("ExtendField - " + Result.AddressInfoRecords[i].ExtendField + "\r\n");
                        }
                    }
                }

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AddressInfoQuery", msg);

                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},ExtendField:{2}", SPID, CustID, ExtendField);
                StringBuilder outParam = new StringBuilder();
                if (Result.AddressInfoRecords != null && Result.AddressInfoRecords.Length > 0)
                {
                    outParam.Append("AddressInfo:");
                    foreach (AddressInfoRecord address in Result.AddressInfoRecords)
                    {
                        outParam.Append("{");
                        outParam.AppendFormat("AddressID:{0},Address:{1},Zipcode:{2},Type:{3},DealType:{4},ExtendField:{5}",
                            address.AddressID, address.Address, address.Zipcode, address.Type, address.DealType, address.ExtendField);
                        outParam.Append("},");
                    }
                }
                outParam.Append(",ExtendField:" + Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress,SPID, "AddressInfoQuery", inParam, outParam.ToString(), Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, CustID, "", Result.Result, Result.ErrorDescription, "", "AddressInfoQuery");
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 获取客户单条常用地址信息接口
    public class AddressInfoLoadResult
    {
        public int Result;
        public string ErrorDescription;
        public AddressInfo Address;
        public string ExtendField;
    }

    [WebMethod(Description = "获取客户单条常用地址信息接口")]
    public AddressInfoLoadResult AddressInfoLoad(string SPID, long AddressID, string ExtendField)
    {
        AddressInfoLoadResult Result = new AddressInfoLoadResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.Address = null;
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（不能为空）";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（长度有误）";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AddressInfoLoad", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            #endregion

            //数据库操作
            Result.Address = AddressInfoBO.AddressInfoLoad(SPID, AddressID, out Result.Result, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("获取客户单条常用地址信息接口（" + DateTime.Now.ToString("u") + "）\r\n");
                msg.Append("SPID: " + SPID);
                msg.Append("; AddressID: " + AddressID);
                msg.Append("; ExtendField: " + ExtendField + "\r\n");
                msg.Append("处理结果: " + Result.Result);
                msg.Append("; 错误描述: " + Result.ErrorDescription);
                if (Result.Address != null)
                {
                    msg.Append("\r\nAddressInfo: { ");
                    msg.Append("AddressID: " + Result.Address.AddressID.ToString());
                    msg.Append("; AreaCode: " + Result.Address.AreaCode);
                    msg.Append("; Address: " + Result.Address.Address);
                    msg.Append("; Zipcode: " + Result.Address.Zipcode);
                    msg.Append("; Type: " + Result.Address.Type);
                    msg.Append("; OtherType: " + Result.Address.OtherType);
                    msg.Append("; RelationPerson - " + Result.Address.RelationPerson);
                    msg.Append("; Mobile: " + Result.Address.Mobile);
                    msg.Append("; FixedPhone: " + Result.Address.FixedPhone + " }\r\n");
                }
                msg.Append("; ExtendField: " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AddressInfoLoad", msg);

                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID: {0}, AddressID: {1}, ExtendField: {2}", SPID, AddressID, ExtendField);
                StringBuilder outParam = new StringBuilder();
                outParam.Append("处理结果: " + Result.Result);
                outParam.Append(", 错误描述: " + Result.ErrorDescription);
                if (Result.Address != null)
                {
                    outParam.Append(", AddressInfo: ");
                    outParam.Append("{ ");
                    outParam.AppendFormat("AddressID: {0}, AreaCode: {1}, Address: {2}, Zipcode: {3}, Type: {4}, OtherType: {5}, RelationPerson: {6}, Mobile: {7}, FixedPhone: {8}",
                        Result.Address.AddressID, Result.Address.AreaCode, Result.Address.Address, Result.Address.Zipcode, Result.Address.Type, Result.Address.OtherType, Result.Address.RelationPerson, Result.Address.Mobile, Result.Address.FixedPhone);
                    outParam.Append(" }");
                }
                outParam.Append(", ExtendField: " + Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "AddressInfoLoad", inParam, outParam.ToString(), Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 添加客户常用地址信息接口
    public class AddressInfoAddResult
    {
        public int Result;
        public string ErrorDescription;
        public string CustID;
        public long AddressID;
        public string ExtendField;
    }

    [WebMethod(Description = "添加客户常用地址信息接口")]
    public AddressInfoAddResult AddressInfoAdd(string SPID, string CustID, AddressInfo Address, string ExtendField)
    {
        AddressInfoAddResult Result = new AddressInfoAddResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.CustID = CustID;
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（不能为空）";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（长度有误）";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AddressInfoLoad", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "（不能为空）";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "（长度有误）";
            }

            if (Address == null)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "（Address为空）";
                return Result;
            }
            #endregion

            //数据库操作
            Result.Result = AddressInfoBO.AddressInfoAdd(SPID, CustID, Address, out Result.AddressID, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("添加客户常用地址信息接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID);
                if (Address != null)
                {
                    msg.Append(";AddressInfo: \r\n");
                    msg.Append(" :AreaCode - " + Address.AreaCode);
                    msg.Append(" :Address - " + Address.Address);
                    msg.Append(" :Zipcode - " + Address.Zipcode);
                    msg.Append(" :Type - " + Address.Type);
                    msg.Append(" :OtherType - " + Address.OtherType);
                    msg.Append(" :RelationPerson - " + Address.RelationPerson);
                    msg.Append(" :Mobile - " + Address.Mobile);
                    msg.Append(" :FixedPhone - " + Address.FixedPhone + "\r\n");
                }
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; AddressID - " + Result.AddressID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AddressInfoLoad", msg);

                #endregion

                #region 数据库日志

                StringBuilder inParam = new StringBuilder();
                inParam.AppendFormat("SPID:{0},CustID:{1}", SPID, CustID);
                if (Address != null)
                {
                    inParam.Append(",AddressInfo:");
                    inParam.Append("{");
                    inParam.AppendFormat("AreaCode:{0},Address:{1},Zipcode:{2},Type:{3},OtherType:{4},RelationPerson:{5},Mobile:{6},FixedPhone:{7}",
                        Address.AreaCode, Address.Address, Address.Zipcode, Address.Type, Address.OtherType, Address.RelationPerson, Address.Mobile, Address.FixedPhone);
                    inParam.Append("}");
                }
                inParam.Append(",ExtendField:" + ExtendField + "\r\n");
                StringBuilder outParam = new StringBuilder();
                outParam.AppendFormat("处理结果:{0},错误描述:{1},CustID:{2},AddressID:{3},ExtendField:{4}", Result.Result, Result.ErrorDescription, Result.CustID, Result.AddressID, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "AddressInfoAdd", inParam.ToString(), outParam.ToString(), Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }
        return Result;
    }
    #endregion


    #region 修改客户常用地址信息接口
    public class AddressInfoSaveResult
    {
        public int Result;
        public string ErrorDescription;
        public string CustID;
        public string ExtendField;
    }

    [WebMethod(Description = "修改客户常用地址信息接口")]
    public AddressInfoSaveResult AddressInfoSave(string SPID, string CustID, AddressInfo Address, string ExtendField)
    {
        AddressInfoSaveResult Result = new AddressInfoSaveResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.CustID = CustID;
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（不能为空）";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（长度有误）";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AddressInfoLoad", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "（不能为空）";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "（长度有误）";
            }

            if (Address == null)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "（Address为空）";
                return Result;
            }
            #endregion

            //数据库操作
            Result.Result = AddressInfoBO.AddressInfoSave(SPID, CustID, Address, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("修改客户常用地址信息接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID);
                if (Address != null)
                {
                    msg.Append(";AddressInfo: \r\n");
                    msg.Append(" :AddressID - " + Address.AddressID);
                    msg.Append(" :AreaCode - " + Address.AreaCode);
                    msg.Append(" :Address - " + Address.Address);
                    msg.Append(" :Zipcode - " + Address.Zipcode);
                    msg.Append(" :Type - " + Address.Type);
                    msg.Append(" :OtherType - " + Address.OtherType);
                    msg.Append(" :RelationPerson - " + Address.RelationPerson);
                    msg.Append(" :Mobile - " + Address.Mobile);
                    msg.Append(" :FixedPhone - " + Address.FixedPhone + "\r\n");
                }
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AddressInfoLoad", msg);

                #endregion

                #region 数据库日志

                StringBuilder inParam = new StringBuilder();
                inParam.AppendFormat("SPID:{0},CustID:{1}", SPID, CustID);
                if (Address != null)
                {
                    inParam.Append(",AddressInfo:");
                    inParam.Append("{");
                    inParam.AppendFormat("AddreassID:{0};AreaCode:{1},Address:{2},Zipcode:{3},Type:{4},OtherType:{5},RelationPerson:{6},Mobile:{7},FixedPhone:{8}",
                        Address.AddressID, Address.AreaCode, Address.Address, Address.Zipcode, Address.Type, Address.OtherType, Address.RelationPerson, Address.Mobile, Address.FixedPhone);
                    inParam.Append("}");
                }
                inParam.Append(",ExtendField:" + ExtendField + "\r\n");
                StringBuilder outParam = new StringBuilder();
                outParam.AppendFormat("处理结果:{0},错误描述:{1},CustID:{2},ExtendField:{3}", Result.Result, Result.ErrorDescription, Result.CustID, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "AddressInfoSave", inParam.ToString(), outParam.ToString(), Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 删除客户单条常用地址信息接口
    public class AddressInfoDeleteResult
    {
        public int Result;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "删除客户单条常用地址信息接口")]
    public AddressInfoDeleteResult AddressInfoDelete(string SPID, long AddressID, string ExtendField)
    {
        AddressInfoDeleteResult Result = new AddressInfoDeleteResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（不能为空）";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "（长度有误）";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AddressInfoLoad", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            #endregion

            //数据库操作
            Result.Result = AddressInfoBO.AddressInfoDelete(SPID, AddressID, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("删除客户单条常用地址信息接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";AddressID - " + AddressID);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AddressInfoDelete", msg);

                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},AddressID:{1},ExtendField:{2}", SPID, AddressID, ExtendField);
                StringBuilder outParam = new StringBuilder();
                outParam.Append("处理结果:" + Result.Result);
                outParam.Append(",错误描述:" + Result.ErrorDescription);
                outParam.Append(",ExtendField:" + Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "AddressInfoLoad", inParam, outParam.ToString(), Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 省客户ID对应关系查询接口(周涛)

    public class CustProvinceRelationQueryResult
    {
        public string OuterID;
        public string CustID;
        public string ExtendField;
    }

    [WebMethod(Description = "省客户ID对应关系查询接口")]
    public CustProvinceRelationQueryResult CustProvinceRelationQuery(string SPID, string OuterID, string ProvinceID, string ExtendField)
    {
        CustProvinceRelationQueryResult Result = new CustProvinceRelationQueryResult();

        Result.OuterID = OuterID;
        Result.CustID = "";
        Result.ExtendField = "";

        int Res = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        string ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Res = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Res = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            //IP是否允许访问
            Res = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            if (Res != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Res = CommonBizRules.CheckInterfaceLimit(SPID, "CustProvinceRelationQuery", this.Context, out ErrMsg);
            if (Res != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(OuterID))
            {
                Res = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，OuterID不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Res = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Res = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
                return Result;
            }
            #endregion

            //数据库操作
            Res = CustProvinceRelation.getCustProvinceRelation(SPID, OuterID, ProvinceID, out Result.CustID, out ErrMsg);

        }
        catch (Exception e)
        {
            Res = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            try
            {
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("省客户ID对应关系查询接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":OuterID - " + OuterID);
                msg.Append(":ProvinceID - " + ProvinceID);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Res);
                msg.Append("; 错误描述 - " + ErrMsg);
                msg.Append(": OuterID - " + Result.OuterID);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("CustProvinceRelationQuery", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},OuterID:{1},ProvinceID:{2},ExtendField:{3}", SPID, OuterID, ProvinceID, ExtendField);
                String outParam = String.Format("OuterID:{0},CustID:{1},ExtendField:{2}", Result.OuterID, Result.CustID, Result.ExtendField);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "CustProvinceRelationQuery", inParam, outParam, Res, ErrMsg);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Res, ErrMsg, "", "CustProvinceRelationQuery");
            }
            catch { }
        }

        return Result;
    }

    #endregion

    #region 用户名识别接口(李宏图 2011-06-17)
    /// <summary>
    /// 用户名识别接口返回记录
    /// 作者：李宏图    时间：2011-6-17
    /// 修改：          时间：
    /// </summary>
    public class QueryByUserNameResult
    {
        public int Result;
        public BasicInfoV2Record[] BasicInfoV2Records;
        public string ErrorDescription;
        public string ExtendField;
    }

    /// <summary>
    /// 用户名识别接口
    /// 作者：李宏图    时间：2011-6-17
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "用户名识别接口")]
    public QueryByUserNameResult QueryByUserName(string SPID, string UserName, string ExtendField)
    {
        string custid_first = "";
        string custtype_first = "";
        QueryByUserNameResult Result = new QueryByUserNameResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.BasicInfoV2Records = null;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "QueryByUserName", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            #endregion

            //根据用户名取得相关的客户信息（客户基本信息表CustInfo）
            Result.BasicInfoV2Records = CustBasicInfo.GetQueryByUserName(SPID, UserName, out Result.Result, out Result.ErrorDescription);

            if (Result.BasicInfoV2Records.Length == 0)
            {
                Result.Result = ErrorDefinition.IError_Result_Null_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_Null_Msg;
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("用户名识别接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";UserName - " + UserName);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                if (Result.BasicInfoV2Records != null)
                {
                    if (Result.BasicInfoV2Records.Length > 0)
                    {
                        msg.Append("BasicInfoV2Records: \r\n");
                        for (int i = 0; i < Result.BasicInfoV2Records.Length; i++)
                        {
                            msg.Append("CustID- " + Result.BasicInfoV2Records[i].CustID + ";");
                            msg.Append("CustType- " + Result.BasicInfoV2Records[i].CustType + ";");
                            msg.Append("PhoneClass- " + Result.BasicInfoV2Records[i].PhoneClass + ";");
                            msg.Append("RealName- " + Result.BasicInfoV2Records[i].RealName + ";");
                            msg.Append("Sex- " + Result.BasicInfoV2Records[i].Sex + ";");
                            msg.Append("ExtendField- " + Result.BasicInfoV2Records[i].ExtendField + "\r\n");
                        }

                        custid_first = Result.BasicInfoV2Records[0].CustID;
                        custtype_first = Result.BasicInfoV2Records[0].CustType;
                    }
                }
                msg.Append("\r\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("QueryByUserName", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},UserName:{1},ExtendField:{2}", SPID, UserName, ExtendField);
                StringBuilder outParam = new StringBuilder();
                if (Result.BasicInfoV2Records != null && Result.BasicInfoV2Records.Length > 0)
                {
                    outParam.Append("BasicInfoV2Record:");
                    foreach (BasicInfoV2Record basicInfo in Result.BasicInfoV2Records)
                    {
                        outParam.Append("{");
                        outParam.AppendFormat("CustID:{0},CustType:{1},RealName:{2},Sex:{3},PhoneClass:{4},ExtendField:{5}", 
                            basicInfo.CustID, basicInfo.CustType, basicInfo.RealName, basicInfo.Sex, basicInfo.PhoneClass, basicInfo.ExtendField);
                        outParam.Append("},");
                    }
                }
                outParam.Append("ExtendField:" + Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "QueryByUserName", inParam, outParam.ToString(), Result.Result, Result.ErrorDescription);
                #endregion
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 查询客户来源接口(李宏图 2011-08-31)
    /// <summary>
    /// 查询客户来源接口返回记录
    /// 作者：李宏图    时间：2011-08-31
    /// 修改：          时间：
    /// </summary>
    public class QueryByCustFromResult
    {
        public int Result;
        public string FromContent;
        public string ErrorDescription;
        public string ExtendField;
    }

    /// <summary>
    /// 查询客户来源接口
    /// 作者：李宏图    时间：2009-7-31
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "查询客户来源")]
    public QueryByCustFromResult QueryCustFrom(string SPID, string CustID, string ExtendField)
    {
        QueryByCustFromResult Result = new QueryByCustFromResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.FromContent = "";
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "QueryCustFrom", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            #endregion

            Result.FromContent = CustBasicInfo.QueryCustFrom(SPID, CustID, out Result.Result, out Result.ErrorDescription);

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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("查询客户来源" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID + "\r\n");
                msg.Append(";CustID - " + CustID + "\r\n");
                msg.Append(";fromContent - " + Result.FromContent + "\r\n");
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result + "\r\n");
                msg.Append("; 错误描述 - " + Result.ErrorDescription + "\r\n");
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("\r\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("QueryCustFrom", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},ExtendField:{2}", SPID, CustID, ExtendField);
                String outParam = String.Format("FromContent:{0},ExtendField:{1}", Result.FromContent, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "QueryCustFrom", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 变更客户政企相关属性
    [WebMethod(Description = "变更客户政企相关属性")]
    public string CustEnterpriseModify(string requestXml)
    {
        string ResultXML = "";
        string ErrMsg = "未知错误!";

        string Version = "";
        string SPID = "";
        string CustID = "";
        string EnterpriseChange = "";
        int Result = -19999;

        try
        {
            if (CommonUtility.IsEmpty(requestXml))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                ResultXML = BTBizRules.SetEnterpriseInfo(Version, SPID, Result, ErrMsg);
                return ResultXML;
            }

            Result = BTBizRules.GetEnterpriseInfo(requestXml, out Version, out SPID, out CustID, out EnterpriseChange, out ErrMsg);
            if (Result != 0)
            {
                ResultXML = BTBizRules.SetEnterpriseInfo(Version, SPID, Result, ErrMsg);
                return ResultXML;
            }

            Result = BTForBusinessSystemInterfaceRules.CustEnterpriseModify(SPID, CustID, EnterpriseChange, out ErrMsg);
            ResultXML = BTBizRules.SetEnterpriseInfo(Version, SPID, Result, ErrMsg);
        }
        catch (System.Exception ex)
        {
            Result = -22500;
            ResultXML = BTBizRules.SetEnterpriseInfo(Version, SPID, Result, ex.Message);
            return ResultXML;
        }
        finally
        {
            try
            {
                #region 数据库日志

                String inParam = String.Format("requestXml:{0}", requestXml);
                String outParam = String.Format("Version:{0},SPID:{1},CustID:{2},EnterpriseChange:{3},ResultXML:{4}", Version, SPID, CustID, EnterpriseChange, ResultXML);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "CustEnterpriseModify", inParam, outParam, Result, ErrMsg);
                #endregion
            }
            catch { }
        }




        return ResultXML;
    }
    #endregion

    #endregion

    #region 常旅客信息

    public class FrequentUserQueryResult
    {
        public int Result;
        public string CustID;
        public FrequentUserInfo[] FrequentUserInfos;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "常旅客信息查询接口")]
    public FrequentUserQueryResult FrequentUserQuery(string SPID, string CustID, string ExtendField)
    {
        FrequentUserQueryResult Result = new FrequentUserQueryResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code; ;
        Result.CustID = CustID;
        Result.ErrorDescription = "";
        Result.FrequentUserInfos = null;
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "FrequentUserQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
            }
            #endregion

            //数据库操作
            Result.FrequentUserInfos = FrequentUserBO.GetFrequentUser(SPID, CustID, out Result.Result, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("常旅客信息查询接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID + "\r\n");
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID + "\r\n");
                if (Result.FrequentUserInfos != null)
                {
                    msg.Append("FrequentUserInfos: \r\n");
                    if (Result.FrequentUserInfos.Length > 0)
                    {
                        for (int i = 0; i < Result.FrequentUserInfos.Length; i++)
                        {
                            msg.Append("FrequentUserID - " + Result.FrequentUserInfos[i].FrequentUserID);
                            msg.Append(": DealType - " + Result.FrequentUserInfos[i].DealType);
                            msg.Append(": RealName - " + Result.FrequentUserInfos[i].RealName);
                            msg.Append(": CertificateCode - " + Result.FrequentUserInfos[i].CertificateCode);
                            msg.Append(": CertificateType - " + Result.FrequentUserInfos[i].CertificateType);
                            msg.Append(": Phone - " + Result.FrequentUserInfos[i].Phone);
                            msg.Append(": ExtendField - " + Result.FrequentUserInfos[i].ExtendField + "\r\n");
                        }
                    }
                }
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("FrequentUserQueryResult", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},ExtendField:{2}", SPID, CustID, ExtendField);
                StringBuilder outParam = new StringBuilder();

                outParam.Append("CustID:" + Result.CustID);
                outParam.Append(",ExtendField:" + Result.ExtendField);
                if (Result.FrequentUserInfos != null && Result.FrequentUserInfos.Length > 0)
                {
                    outParam.Append(",FrequentUserInfo:");
                    foreach (FrequentUserInfo info in Result.FrequentUserInfos)
                    {
                        outParam.Append("{");
                        outParam.AppendFormat("FrequentUserID:{0},DealType:{1},RealName:{2},CertificateCode:{3},CertificateType:{4},Phone:{5},ExtendField:{6}",
                            info.FrequentUserID, info.DealType, info.RealName, info.CertificateCode, info.CertificateType, info.Phone, info.ExtendField);
                        outParam.Append("},");
                    }
                }

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress,SPID, "FrequentUserQuery", inParam, outParam.ToString(), Result.Result, Result.ErrorDescription);
                #endregion

                //CommonBizRules.WriteDataLog(SPID, CustID, "", Result.Result, Result.ErrorDescription, "", "FrequentUserQueryResult");
            }
            catch { }
        }

        return Result;
    }
    public class FrequentUserUploadResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "常旅客信息上传接口")]
    public FrequentUserUploadResult FrequentUserUpload(string SPID, string CustID, FrequentUserInfo[] FrequentUserInfos, string ExtendField)
    {
        FrequentUserUploadResult Result = new FrequentUserUploadResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        Result.CustID = CustID;
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断

            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "FrequentUserUploadQuery", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
            }

            if (FrequentUserInfos == null)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "FrequentUserInfos为空";
                return Result;
            }
            #endregion

            //数据库操作
            Result.Result = FrequentUserBO.UploadFrequentUser(SPID, CustID, FrequentUserInfos, out Result.ErrorDescription);
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
                //写文本日志
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("常旅客信息上传接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";CustID - " + CustID + "\r\n");
                if (FrequentUserInfos != null)
                {
                    if (FrequentUserInfos.Length > 0)
                    {
                        for (int i = 0; i < FrequentUserInfos.Length; i++)
                        {
                            msg.Append("FrequentUserInfos : " + "\r\n)");
                            msg.Append("FrequentUserID" + FrequentUserInfos[i].FrequentUserID);
                            msg.Append(": DealType - " + FrequentUserInfos[i].DealType);
                            msg.Append(": RealName - " + FrequentUserInfos[i].RealName);
                            msg.Append(": CertificateCode - " + FrequentUserInfos[i].CertificateCode);
                            msg.Append(": CertificateType - " + FrequentUserInfos[i].CertificateType);
                            msg.Append(": Phone - " + FrequentUserInfos[i].Phone);
                            msg.Append(": ExtendField - " + FrequentUserInfos[i].ExtendField + "\r\n");
                        }
                    }

                }
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("FrequentUserUploadResult", msg);
                #endregion

                #region 写数据库日志
                StringBuilder inParam = new StringBuilder();
                inParam.Append("SPID:" + SPID);
                inParam.Append(",CustID:" + CustID);
                inParam.Append(",ExtendField:" + ExtendField);
                if (FrequentUserInfos != null && FrequentUserInfos.Length > 0)
                {
                    inParam.Append(",FrequentUserInfo:");
                    foreach (FrequentUserInfo info in FrequentUserInfos)
                    {
                        inParam.Append("{");
                        inParam.AppendFormat("FrequentUserID:{0},DealType:{1},RealName:{2},CertificateCode:{3},CertificateType:{4},Phone:{5},ExtendField:{6}", 
                            info.FrequentUserID, info.DealType, info.RealName, info.CertificateCode, info.CertificateType, info.Phone, info.ExtendField);
                        inParam.Append("},");
                    }
                }

                String outParam = String.Format("CustID:{0},ExtendField:{1}", Result.CustID, Result.ExtendField);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "FrequentUserUpload", inParam.ToString(), outParam, Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, CustID, "", Result.Result, Result.ErrorDescription, "", "FrequentUserUploadResult");
            }
            catch { }
        }
        return Result;
    }
    
    #endregion

    #region 密码服务接口

    #region 密码设置接口(周涛)
    public class SetPwdResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "密码设置接口")]
    public SetPwdResult SetPwd(string SPID, string CustID, string Pwd, string PwdType, string ExtendField)
    {
        SetPwdResult Result = new SetPwdResult();

        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.CustID = CustID;
        Result.ExtendField = "";

        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "SetPwd", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(Pwd))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
                return Result;
            }

            if (CommonUtility.IsEmpty(PwdType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
                return Result;
            }

            string tmpPwdType = "1;2";
            if (tmpPwdType.IndexOf(PwdType) < 0)
            {
                Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "，密码类型错误";
                return Result;
            }

            if (PwdType.Equals("1"))   // 1 是语音密码 2 是web密码
            {
                if (Pwd.Length != ConstDefinition.Length_Min_Password)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "长度有误,语音密码只能是6位";
                    return Result;
                }

                if (!CommonUtility.IsNumeric(Pwd))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "语音密码只能是数字";
                    return Result;
                }
            }
            #endregion

            //数据库操作
            //修改本地密码，从2014-07-21起废除 （本应从2014-07-03日起废除，但是因翼周边等客户端产品老用户老版本的兼容性，导致7月3日来不及废除[天翼账号修改密码无法支持仅提供新密码修改用户密码]）
            //割接后经各方协商，由客户信息平台保存或叫缓存客户登陆密码（天翼接口登录）,以便老用户老版本客户端，依旧可以仅提供新密码修改密码，以达到兼容老用户老版本客户端的要求。（ps:该需求非常变态恶心，但只能妥协!!）
            Result.Result = PassWordBO.SetPassword(SPID, CustID, Pwd, PwdType, ExtendField, out Result.ErrorDescription);
            #region  修改天翼账号密码
            
            string AccessToken = "";
            string OldPassWord = "";
            Result.Result = CIP2BizRules.FetchAccessTokenPasswordFromCustID(CustID, out AccessToken, out OldPassWord, out Result.ErrorDescription);
            StringBuilder strLog = new StringBuilder();
            if (Result.Result == 0 && !String.IsNullOrEmpty(AccessToken) && !String.IsNullOrEmpty(OldPassWord))
            {
                string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
                string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
                string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
                string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
                string clientIp = HttpContext.Current.Request.UserHostAddress;
                string clientAgent = HttpContext.Current.Request.UserAgent;
                Result.Result = CIP2BizRules.UpdateUnifyPassWord(appId, appSecret, version, clientType, AccessToken, OldPassWord, Pwd, clientIp, clientAgent, out Result.ErrorDescription);
               
                strLog.AppendFormat("UpdateUnifyPassWord:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrorDescription);
                if (Result.Result != 0)
                {
                    //消除accesstoken                
                }
                else
                {
                    Result.Result = CIP2BizRules.UpdatePasswordFromCustIDAccessToken(CustID, AccessToken, Pwd, out Result.ErrorDescription);
                    strLog.AppendFormat("UpdatePasswordFromCustIDAccessToken:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrorDescription);
                }
            }
            else
            {
                Result.ErrorDescription = "修改密码失败，根据CustID获取AccessToken失败:CustID=" + CustID + ",AccessToken=" + AccessToken;
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("密码设置接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":CustID - " + CustID);
                msg.Append(":PwdType - " + PwdType);
                msg.Append(":新密码 - " + Pwd);

                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("SetPwdResult", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},Pwd:{2},PwdType:{3},ExtendField:{4}", SPID, CustID, Pwd, PwdType, ExtendField);
                String outParam = String.Format("CustID:{0},ExtendField:{1}", Result.CustID, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "SetPwd", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Result.Result, Result.ErrorDescription, "", "SetPwdResult");
            }
            catch { }
        }


        return Result;
    }
    #endregion

    


    #region 修改综合平台密码
    public class UnifyUpdatePwdResult
    {
        public Int32 Result;
        public String ErrMsg;
        public String ExtendField;
    }
    [WebMethod(Description = "综合平台密码修改接口")]
    public UnifyUpdatePwdResult UnifyUpdatePwd(String SPID, String CustID, String AccessToken,String PwdOld, String PwdNew,  String ExtendField)
    {
        UnifyUpdatePwdResult Result = new UnifyUpdatePwdResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        StringBuilder strLog = new StringBuilder();
        strLog.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
        strLog.Append("综合平台密码修改接口" + DateTime.Now.ToString("u") + "\r\n");
        strLog.Append(";SPID - " + SPID);
        strLog.Append(":CustID - " + CustID);
        strLog.Append(":PwdOld - " + PwdOld);
        strLog.Append(":PwdNew - " + PwdNew);
        strLog.Append(";ExtendField - " + ExtendField + "\r\n");
        try
        {
            #region 数据合法性验证
            //数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }


            if (CommonUtility.IsEmpty(CustID) && CommonUtility.IsEmpty(AccessToken))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "CustID和AccessTokien不能同时为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }


            if (CommonUtility.IsEmpty(PwdNew))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidPassword_Msg;
                return Result;
            }

        
            #endregion

            Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            Result.ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            #region
            //根据custid直接从accesstoken表中获取accesstoken
            //虽然一个custid可能对应多个不同的accesstoken(因为一个custid可能对应多个userid),但是所有userid的密码都是统一个，不用担心密码修改到别人头上
            String UserID = String.Empty;
            if (CommonUtility.IsEmpty(AccessToken))
            {
                Result.Result = CIP2BizRules.FetchAccessTokenFromCustID(CustID, out AccessToken, out Result.ErrMsg);
            }
            else 
            {
                Result.Result = 0;
            }
            
            strLog.AppendFormat("根据custid查accesstoken结果:{0},ErrMsg:{1}\r\n",Result.Result,Result.ErrMsg);
            if (Result.Result == 0 && !String.IsNullOrEmpty(AccessToken))
            {
                string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
                string appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
                string version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
                string clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
                string clientIp = HttpContext.Current.Request.UserHostAddress;
                string clientAgent = HttpContext.Current.Request.UserAgent;
                Result.Result = CIP2BizRules.UpdateUnifyPassWord(appId, appSecret, version, clientType, AccessToken, PwdOld, PwdNew, clientIp, clientAgent, out Result.ErrMsg);
                strLog.AppendFormat("修改综合密码结果:{0},ErrMsg:{1}\r\n",Result.Result,Result.ErrMsg);
                if (Result.Result != 0)
                {
                    //消除accesstoken                
                }
                else //同时修改本地密码
                {
                    Result.Result = PassWordBO.SetPassword(SPID, CustID, CryptographyUtil.Encrypt(PwdNew), "2", "", out Result.ErrMsg);
                    strLog.AppendFormat("修改本地密码结果:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
                }
            }
            else
            {



                //Result.ErrMsg = "修改密码失败，根据CustID获取AccessToken失败:CustID=" + CustID + ",AccessToken=" + AccessToken;
            }
            #endregion
        }
        catch (Exception ex)
        {
            Result.ErrMsg += ex.Message;
            strLog.AppendFormat("异常:{0}\r\n",ex.Message);
        }
        finally
        {
            try
            {
                #region 文本日志
                strLog.Append("处理结果 - " + Result.Result);
                strLog.Append("; 错误描述 - " + Result.ErrMsg);
                strLog.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                strLog.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("UnifyUpdatePwd", strLog);
                #endregion

            }
            catch { }
        }

        return Result;
    }

    #endregion


    #region 密码修改接口

    public class ReSetPwdResult
    {
        public Int32 Result;
        public String ErrorDescription;
        public String ExtendField;
    }

    [WebMethod(Description="密码修改接口")]
    public ReSetPwdResult ReSetPwd(String SPID, String CustID, String PwdOld, String PwdNew, String PwdType, String ExtendField)
    {
        ReSetPwdResult Result = new ReSetPwdResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        try
        {
            #region 数据合法性验证
            //数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "ReSetPwd", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return Result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            //if (CommonUtility.IsEmpty(PwdOld))
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg;
            //    return Result;
            //}

            if (CommonUtility.IsEmpty(PwdNew))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg;
                return Result;
            }

            string tmpPwdType = "1;2";
            if (tmpPwdType.IndexOf(PwdType) < 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，密码类型错误";
                return Result;
            }

            if (PwdType.Equals("1"))
            {
                if (PwdNew.Length != ConstDefinition.Length_Min_Password)
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "长度有误,语音密码只能是6位";
                    return Result;
                }

                if (!CommonUtility.IsNumeric(PwdNew))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code; ;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "语音密码只能是数字";
                    return Result;
                }
            }

            #endregion

            Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            //验证旧密码
            bool IsRight = PassWordBO.OldPwdIsRight(CustID, PwdOld, PwdType, out Result.ErrorDescription);
            if (IsRight)
            {
                Result.Result = PassWordBO.SetPassword(SPID, CustID, PwdNew, PwdType, ExtendField, out Result.ErrorDescription);
                if (Result.Result != 0)
                {
                    Result.Result = -22500;
                    Result.ErrorDescription = "密码修改失败";
                }
                Result.ErrorDescription = "密码修改成功";
            }
            else
            {
                Result.Result = -20504;
                Result.ErrorDescription = "原始密码不匹配";
            }
        }
        catch (Exception ex)
        {
            Result.ErrorDescription += ex.Message;
        }
        finally
        {
            try
            {
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("密码修改接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":CustID - " + CustID);
                msg.Append(":PwdOld - " + PwdOld);
                msg.Append(":PwdNew - " + PwdNew);
                msg.Append(":PwdType - " + PwdType);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("ReSetPwd", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},Pwd:{2},PwdOld:{3},PwdNew:{4},PwdType:{5},ExtendField:{4}", SPID, CustID, PwdOld, PwdNew, PwdType, ExtendField);
                String outParam = String.Format("ExtendField:{0}", Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "ReSetPwd", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }

        return Result;
    }

    #endregion


    public class CaptchaResult
    {
        public Int64 Result;
        public string CustID;
        public string Phone;
        public string AuthenCode;
        public string ExtendField;
        public string ErrorDescription;
    }
    [WebMethod(Description = "综合平台获取验证码接口")]
    public CaptchaResult Captcha(String SPID, String phone, String type,String ExtendField)
    {
        CaptchaResult Result = new CaptchaResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        StringBuilder strLog = new StringBuilder();
        strLog.Append("====================================\r\n");
        strLog.Append("通过手机找回综合平台密码接口\r\n");
        strLog.AppendFormat("时间:{0}\r\n", DateTime.Now.ToString("u"));
        strLog.AppendFormat("参数,SPID:{0},phone:{1},ExtendField:{2}\r\n",SPID,phone,ExtendField);
        try
        {
            #region 数据合法性验证
            //数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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

            if (CommonUtility.IsEmpty(phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription ="phone不能为空";
                return Result;
            }

            #endregion
            String jsonResult = String.Empty;
            string sign = String.Empty;
            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;
            strLog.AppendFormat("获取综合平台接入参数:appId:{0},appSecret:{1},version:{2},clientType:{3},clientIp:{4},clientAgent:{5}\r\n", appId, appSecret, version, clientType, clientIp, clientAgent);

            string paras = String.Empty;
            string format = "json";
            string parameters = "mobile=" + phone + "&type=" + type + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
            strLog.AppendFormat("parameters:={0}\r\n", parameters);
            paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
            strLog.AppendFormat("paras:={0}\r\n", paras);
            sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
            strLog.AppendFormat("sign:={0}\r\n", sign);
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("appId", appId);
            postData.Add("version", version);
            postData.Add("clientType", clientType);
            postData.Add("paras", paras);
            postData.Add("sign", sign);
            postData.Add("format", format);

            System.Net.WebClient webclient = new System.Net.WebClient();
            webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformCaptchaUrl, "POST", postData);
            jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
            strLog.AppendFormat("jsonResult:{0}\r\n", jsonResult);

            Dictionary<string, string> result_dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
  
            String result = String.Empty;
            String msg = String.Empty;
            result_dic.TryGetValue("msg", out msg);
            result_dic.TryGetValue("result", out result);

            Result.Result = Convert.ToInt64(result);
            Result.ErrorDescription = msg;

        }
        catch (Exception e)
        {
            Result.ErrorDescription += e.Message;
        }
        finally
        {
            strLog.Append("====================================\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("Captcha", strLog);
        }
        return Result;
    }



    public class UnifyAccountRegisterResult
    {
        public long Result;
        public string SPID;
        public string CustID;
        public string TourCardID;
        public string ErrorDescription;
        public string ExtendField;
    }
    [WebMethod(Description = "综合平台登录账号显示注册")]
    public UnifyAccountRegisterResult UnifyAccountRegister(string SPID, string phone, string password, string captcha, string ExtendField)
    {
        UnifyAccountRegisterResult Result = new UnifyAccountRegisterResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        StringBuilder strLog = new StringBuilder();
        strLog.Append("====================================\r\n");
        strLog.Append("综合平台登录账号显示注册\r\n");
        strLog.AppendFormat("时间:{0}\r\n", DateTime.Now.ToString("u"));
        strLog.AppendFormat("参数:SPID:{0},phone:{1},password:{2},captcha:{3},ExtendField:{4}", SPID, phone, password, captcha, ExtendField);

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "密码" + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "电话号码" + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(captcha))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "手机验证码" + "，不能为空";
                return Result;
            }
            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            #endregion

            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;
            strLog.AppendFormat("获取综合平台接入参数:appId:{0},appSecret:{1},version:{2},clientType:{3},clientIp:{4},clientAgent:{5}\r\n", appId, appSecret, version, clientType, clientIp, clientAgent);
            String jsonResult = String.Empty;
            string sign = String.Empty;

            string paras = String.Empty;
            string format = "json";
            string parameters = "userName=" + phone + "&password=" + password + "&captcha=" + captcha + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
            strLog.AppendFormat("parameters:={0}\r\n", parameters);
            paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
            strLog.AppendFormat("paras:={0}\r\n", paras);
            sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
            strLog.AppendFormat("sign:={0}\r\n", sign);
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("appId", appId);
            postData.Add("version", version);
            postData.Add("clientType", clientType);
            postData.Add("paras", paras);
            postData.Add("sign", sign);
            postData.Add("format", format);

            System.Net.WebClient webclient = new System.Net.WebClient();
            webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyAccountRegisterUrl, "POST", postData);
            jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
            strLog.AppendFormat("jsonResult:{0}\r\n", jsonResult);
            Dictionary<string, string> result_dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

            String userName = String.Empty;
            String accessToken = String.Empty;
            String expiresIn = String.Empty;

            String result = String.Empty;
            String msg = String.Empty;
            result_dic.TryGetValue("userName", out userName);
            result_dic.TryGetValue("accessToken", out accessToken);
            result_dic.TryGetValue("expiresIn", out expiresIn);
            result_dic.TryGetValue("msg", out msg);
            result_dic.TryGetValue("result", out result);
            Result.Result = Convert.ToInt64(result);
            Result.ErrorDescription = msg;


            ///////////////////////////////////////////
            if (Result.Result == 0 && !String.IsNullOrEmpty(accessToken))
            {
                strLog.Append("天翼账号注册成功\r\n");
                strLog.Append("处理结果 - " + Result.Result);
                strLog.Append("; 错误描述 - " + Result.ErrorDescription);
                strLog.Append(": SPID - " + Result.SPID);
            

                strLog.Append("建立天翼账号和号百账号绑定关系!\r\n");
                UDBMBOSS _UDBMBoss = new UDBMBOSS();
                UnifyAccountInfo accountInfo = new UnifyAccountInfo();
                string Unify_ErrMsg = String.Empty;
                int Unify_Result = _UDBMBoss.UnifyPlatformGetUserInfo(appId, appSecret, version, clientType, accessToken, clientIp, clientAgent, out accountInfo, out Unify_ErrMsg);
                strLog.AppendFormat("先查询综合平台返回:Unify_Result:{0},ErrMsg:{1},UserID:{2}\r\n", Unify_Result, Unify_ErrMsg, Convert.ToString(accountInfo.userId));

                if (Unify_Result == 0 && !String.IsNullOrEmpty(Convert.ToString(accountInfo.userId)))
                {
                    strLog.Append("然后开始注册或者绑定号百客户\r\n");

                    #region 开始注册到号百
                    String MobileName = String.Empty;
                    String EmailName = String.Empty;
                    String RealName = String.Empty;
  
                    if (!String.IsNullOrEmpty(accountInfo.nickName))
                    {
                        RealName = accountInfo.nickName;
                    }

                    if (!String.IsNullOrEmpty(accountInfo.mobileName))
                    {
                        RealName = accountInfo.mobileName;
                    }
           

                    if (!String.IsNullOrEmpty(accountInfo.mobileName))
                    {
                        MobileName = accountInfo.mobileName;
                    }
                    if (!String.IsNullOrEmpty(accountInfo.emailName))
                    {
                        EmailName = accountInfo.emailName;
                    }
                    String EncrytpPassWord = CryptographyUtil.Encrypt(password);
                    String OperType = "5";  // 接口注册 , 天翼账号注册成功后绑定
                    if (!String.IsNullOrEmpty(MobileName) || !String.IsNullOrEmpty(EmailName))
                    {

                        String OuterID, Status, CustType, CustLevel, NickName, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                        Result.CustID = String.Empty;
                        Result.Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", MobileName, EmailName, RealName, EncrytpPassWord, accountInfo.userId, SPID, OperType, out Result.CustID, out Result.ErrorDescription);
                        strLog.Append("【开始注册或者绑定到号百的结果】:\r\n");
                        strLog.AppendFormat("Result:{0},CustID:{1},ErrMsg:{2}\r\n", Result.Result, Result.CustID, Result.ErrorDescription);

                        if (Result.Result == 0 && !String.IsNullOrEmpty(Result.CustID))
                        {
                            String UserName = String.Empty;
                            String CertificateCode = String.Empty;
                            String CertificateType = String.Empty;
                            String Sex = String.Empty;
                            Result.Result = CustBasicInfo.getCustInfo(SPID, Result.CustID, out Result.ErrorDescription, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                                out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                                out AreaID, out RegistrationSource);

                            strLog.Append(" 处理结果 - " + Result.Result);
                            strLog.Append("; 错误描述 - " + Result.ErrorDescription);
                            strLog.Append(": SPID - " + Result.SPID);
                            strLog.Append("; CustID - " + Result.CustID);
                            CommonBizRules.WriteTraceIpLog(Result.CustID, phone, Result.SPID, HttpContext.Current.Request.UserHostAddress.ToString(), "mall_zc");
                            CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", SPID, "", "0", out Result.ErrorDescription);
                        }
                    }
                    else
                    {
                        Result.Result = -7766;
                        Result.ErrorDescription = "MobileName,或者EmailName为空,所以不注册号百客户";
                        strLog.Append("MobileName,或者EmailName为空,所以不注册号百客户\r\n");
                    }
                    #endregion
                }
                else
                { //查询综合平台客户信息失败,或者account.userid为空
                    strLog.Append("查询综合平台客户信息失败,或者account.userid为空,放弃注册或绑定动作\r\n");
                }
            }
        }
        catch (Exception e)
        {
            Result.Result = -19988;
            Result.ErrorDescription = e.ToString();
            strLog.AppendFormat("系统异常:{0}\r\n",e.ToString());

        }
        finally
        {
            strLog.Append("====================================\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("UnifyAccountRegister", strLog);
        }
        return Result;

    }


    public class UnifyAccountFindPwdResult
    {
        public long Result;
        public string CustID;
        public string PassWord;
        public string Type;
        public string ExtendField;
        public string ErrorDescription;
    }


    [WebMethod(Description = "通过手机找回综合平台密码接口")]
    public UnifyAccountFindPwdResult UnifyAccountFindPwd(string SPID,  string phone, string password,string authencode, string ExtendField)
    {
        UnifyAccountFindPwdResult Result = new UnifyAccountFindPwdResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        StringBuilder strLog = new StringBuilder();
        strLog.Append("====================================\r\n");
        strLog.Append("通过手机找回综合平台密码接口\r\n");
        strLog.AppendFormat("时间:{0}\r\n",DateTime.Now.ToString("u") );
        strLog.AppendFormat("参数:SPID:{0},phone:{1},password:{2},authencode:{3},ExtendField:{4}",SPID,phone,password,authencode,ExtendField);
        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(password))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "密码" + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "电话号码" + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(authencode))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "手机验证码" + "，不能为空";
                return Result;
            }
            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            #endregion

            String appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
            String appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
            String version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
            String clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            String clientIp = HttpContext.Current.Request.UserHostAddress;
            String clientAgent = HttpContext.Current.Request.UserAgent;
            strLog.AppendFormat("获取综合平台接入参数:appId:{0},appSecret:{1},version:{2},clientType:{3},clientIp:{4},clientAgent:{5}\r\n", appId, appSecret, version, clientType, clientIp, clientAgent);
            String jsonResult = String.Empty;
            string sign = String.Empty;

            string paras = String.Empty;
            string format = "json";
            string parameters = "mobile=" + phone + "&password=" + password + "&captcha=" + authencode + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
            strLog.AppendFormat("parameters:={0}\r\n", parameters);
            paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
            strLog.AppendFormat("paras:={0}\r\n", paras);
            sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
            strLog.AppendFormat("sign:={0}\r\n", sign);
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("appId", appId);
            postData.Add("version", version);
            postData.Add("clientType", clientType);
            postData.Add("paras", paras);
            postData.Add("sign", sign);
            postData.Add("format", format);

            System.Net.WebClient webclient = new System.Net.WebClient();
            webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformFindPwdUrl, "POST", postData);
            jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
            strLog.AppendFormat("jsonResult:{0}\r\n", jsonResult);
            Dictionary<string, string> result_dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
            String result = String.Empty;
            String msg = String.Empty;
            result_dic.TryGetValue("msg", out msg);
            result_dic.TryGetValue("result", out result);
            try
            {
                Result.Result = Convert.ToInt64(result);       
            }
            catch (Exception e)
            {
                Result.Result = -198;
            }
            

            Result.ErrorDescription = msg;
            
        }
        catch (Exception e)
        {
            Result.ErrorDescription += e.Message;
            Result.Result = -198;
            strLog.AppendFormat("异常:{0}\r\n",e.Message);
        }
        finally
        { 
            strLog.Append("====================================\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("UnifyAccountFindPwd", strLog);
        }
        return Result;
    }


    #region 通过手机找回|重置密码接口

    public class FindPassWordBackByMobileResult
    {
        public int Result;
        public string CustID;
        public string PassWord;
        public string Type;
        public string ExtendField;
        public string ErrorDescription;
    }

    [WebMethod(Description = "通过手机找回密码接口")]
    public FindPassWordBackByMobileResult FindPassWordBackByMobile(string SPID, string type, string phone, string authencode, string ExtendField)
    {
        // type 1 代表语音密码 type 2 代表web密码
        FindPassWordBackByMobileResult Result = new FindPassWordBackByMobileResult();

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

            if (CommonUtility.IsEmpty(type))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "密码类型" + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "电话号码" + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(authencode))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "手机验证码" + "，不能为空";
                return Result;
            }
            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "FindPassWordBackByMobile", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            #endregion

            Result.CustID = PhoneBO.IsAuthenPhone(phone, SPID, out Result.ErrorDescription);
            if (Result.CustID != "")
            {
                Result.Result = PhoneBO.SelSendSMSMassage(Result.CustID, phone, authencode, out Result.ErrorDescription);
                if (Result.Result == 0) // 校验码验证通过
                {
                    List<string> list = new List<string>();
                    list = FindPwd.SelTypeFindPassWord(Convert.ToInt32(type), phone, out Result.ErrorDescription);
                    string message = "";
                    if (list[0].ToString() == "-30009")
                    {
                        Result.Result = -30009;
                        Result.ErrorDescription = "找回密码失败:!";
                    }

                    if (list[0].ToString() == "0")
                    {
                        string y = list[2].ToString();
                        if ("1".Equals(type))
                        {
                            message = "您的语音密码为：" + y;
                            Result.Type = "1";
                        }
                        else if ("2".Equals(type))
                        {
                            message = "您的Web密码为：" + y;
                            Result.Type = "2";
                        }
                        Result.PassWord = y;
                        string IP = HttpContext.Current.Request.UserHostAddress;
                        string Msg = "";
                        FindPwd.InsertFindPwdLog(list[1].ToString(), list[3].ToString(), type, "2", phone, 0, SPID, IP, "...", out Msg);
                        //CommonBizRules.SendMessage(phone, message, SPID);
                        CommonBizRules.SendMessageV3(phone, "您的密码是:"+y, SPID);

                    }
                    else
                    {
                        Result.Result = Convert.ToInt32(list[0]);
                        Result.ErrorDescription = "找回密码失败:!" + list[4].ToString();
                    }
                }
                else
                {
                    Result.ErrorDescription = "手机验证码" + "，校验未通过!";
                    return Result;
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("通过手机找回密码接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";SPID - " + SPID);
                msg.Append("手机号:" + phone);
                msg.Append("\r\n");
                msg.Append("密码:" + Result.PassWord);

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);

                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("FindPassWordBackByMobile", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},type:{1},phone:{2},authencode:{3},ExtendField:{4}", SPID, type, phone, authencode, ExtendField);
                String outParam = String.Format("CustID:{0},PassWord:{1},Type:{2},ExtendField:{3}", Result.CustID, Result.PassWord, Result.Type, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress,SPID, "FindPassWordBackByMobile", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }

        return Result;

    }

    public class ResetPwdByPhoneResult
    {
        public Int32 Result;
        public String ErrMsg;
        public String ExtendField;
    }
    [WebMethod(Description="通过手机重置密码")]
    public ResetPwdByPhoneResult ResetPwdByPhone(String SPID, String CustID, String PhoneNum, String AuthenCode, String PwdType, String PassWord, String ExtendField)
    {
        ResetPwdByPhoneResult Result = new ResetPwdByPhoneResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
        Result.ExtendField = String.Empty;
        try
        {
            #region 数据有效性验证

            //数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "ResetPwdByPhone", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (!ValidateUtility.ValidateMobile(PhoneNum))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrMsg = "手机号码格式有误";
                return Result;
            }
            String custid = PhoneBO.IsAuthenPhone(PhoneNum, SPID, out Result.ErrMsg);
            if (String.IsNullOrEmpty(custid))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrMsg += "验证手机有误";
                return Result;
            }

            if (String.IsNullOrEmpty(PassWord))
            {
                Result.Result = ErrorDefinition.IError_Result_InValidPassword_Code;
                Result.ErrMsg = ErrorDefinition.IError_Result_InValidPassword_Msg + ",新密码不能为空";
                return Result;
            }

            if (!CustID.Equals(custid))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrMsg = "客户CustID和验证手机有误";
                return Result;
            }
            if (String.IsNullOrEmpty(PwdType))
            {
                PwdType = "2";
            }
            if ((!PwdType.Equals("1")) && (!PwdType.Equals("2")))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
                Result.ErrMsg = "密码类型有误";
                return Result;
            }

            #endregion

            //验证码校验
            Result.Result = PhoneBO.SelSendSMSMassage(CustID, PhoneNum, AuthenCode, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                Result.ErrMsg = "验证码验证失败：" + Result.ErrMsg;
                return Result;
            }

            //修改密码
            Result.Result = PassWordBO.SetPassword(SPID, CustID, PassWord, PwdType, ExtendField, out Result.ErrMsg);

        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
            Result.ErrMsg += ex.Message;
        }
        finally
        {
            try
            {
                #region 文本日志

                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("通过手机重置密码接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";SPID - " + SPID);
                msg.Append("手机号:" + PhoneNum);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrMsg);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("ResetPwdByPhone", msg);

                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},PhoneNum:{2},AuthenCode:{3},PwdType:{4},PassWord:{5},ExtendField:{6}",
                                                    SPID, CustID, PhoneNum, AuthenCode, PwdType, PassWord, ExtendField);
                String outParam = String.Format("Result:{0},ErrMsg:{1},ExtendField:{2}", Result.Result, Result.ErrMsg, Result.ExtendField);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "ResetPwdByPhone", inParam, outParam, Result.Result, Result.ErrMsg);

                #endregion
            }
            catch { }
        }

        return Result;
    }


    public class VerifyAuthenCodeResult
    {
        public int Result;
        public string CustID;
        public string Phone;
        public string AuthenCode;
        public string ExtendField;
        public string ErrorDescription;
    }

    [WebMethod(Description = "校验验证码接口")]
    public VerifyAuthenCodeResult VerifyAuthenCode(string SPID, string phone, string AuthenCode, string ExtendField)
    {
        VerifyAuthenCodeResult Result = new VerifyAuthenCodeResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        Result.ExtendField = "";

        try
        {
            #region 数据校验


            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }


            if (CommonUtility.IsEmpty(AuthenCode))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = "验证码不能为空!";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "VerifyAuthenCode", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (!ValidateUtility.ValidateMobile(phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription = "手机号码格式有误";
                return Result;
            }
            String custid = PhoneBO.IsAuthenPhone(phone, SPID, out Result.ErrorDescription);
            if (String.IsNullOrEmpty(custid))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription += ",验证手机有误";
                return Result;
            }


            Result.Result = CommonBizRules.SPInterfaceGrant(SPID, "VerifyAuthenCode", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription += "method access denied! please contact administrator! ";
                return Result;
            }



            if (custid != "")
            {
                //验证码校验
                Result.Result = PhoneBO.SelSendSMSMassage(custid, phone, AuthenCode, out Result.ErrorDescription);
                if (Result.Result != 0)
                {
                    Result.ErrorDescription = "验证码验证失败：" + Result.ErrorDescription;
                    return Result;
                }
            }
            else
            {
                Result.Result = -1728;
                Result.ErrorDescription = "该手机号码不是认证电话!";
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
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("验证码验证接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
            msg.Append(";SPID - " + SPID);
            msg.Append(";手机号 - " + phone);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("验证码：" + Result.AuthenCode);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; CustID - " + Result.CustID);

            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("VerifyAuthenCode", msg);
            #endregion
        }
        return Result;
    }

    public class GiveMeCheckCodeResult
    {
        public int Result;
        public string CustID;
        public string Phone;
        public string AuthenCode;
        public string ExtendField;
        public string ErrorDescription;
    }
    [WebMethod(Description = "获取验证码接口")]
    public GiveMeCheckCodeResult GetAuthenCode(string SPID, string phone, string ExtendField)
    {
        GiveMeCheckCodeResult Result = new GiveMeCheckCodeResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetAuthenCode", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (!ValidateUtility.ValidateMobile(phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription = "手机号码格式有误";
                return Result;
            }
            String custid = PhoneBO.IsAuthenPhone(phone, SPID, out Result.ErrorDescription);
            if (String.IsNullOrEmpty(custid))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription += ",验证手机有误";
                return Result;
            }


            Result.Result = CommonBizRules.SPInterfaceGrant(SPID, "GetAuthenCode", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription += "method access denied! please contact administrator! ";
                return Result;
            }

            #endregion

            if (custid != "" )
            {
                Random r = new Random();
                string AuthenCode = "";
                AuthenCode += r.Next(100000, 999999).ToString();
                //这里要根据custid 去查找该客户的认证手机,比较认证手机是否和phone一致
                //在另外一个webservice接口中根据phone获得custid，根据custid获得password
                // CryptographyUtil.Decrypt(password);

                //CommonBizRules.SendMessage(phone, "您的验证码是:" + AuthenCode, "35000000");
                int k = PhoneBO.PhoneSel("", phone, out Result.ErrorDescription);
                if (k == 0) {
                    CommonBizRules.SendMessageV3(phone, "您的验证码是:" + AuthenCode, SPID);
                    //将AuthenCode插入数据库
                    PhoneBO.InsertPhoneSendMassage(custid, "验证码信息内容", AuthenCode, phone, DateTime.Now, "描述未知", 1, 0, "1", out Result.ErrorDescription);
                }
                Result.AuthenCode = AuthenCode;
                Result.CustID = custid;
                Result.Result = 0;
                Result.ErrorDescription = "您的验证码已经下发到手机，请查看!";
            }
            else
            {
                Result.Result = -1728;
                Result.ErrorDescription = "该手机号码不是认证电话!";
                //CommonBizRules.SendMessage(phone, Result.ErrorDescription, "35000000");
                //PhoneBO.InsertPhoneSendMassage(custid, "验证码信息内容", "非认证电话", phone, DateTime.Now, "描述未知", 1, 0, "1", out ErrMsg);
            }

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
            msg.Append("根据手机找回密码之验证码下发接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
            msg.Append(";SPID - " + SPID);
            msg.Append(";手机号 - " + phone);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("验证码：" + Result.AuthenCode);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; CustID - " + Result.CustID);

            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("GiveMeCheckCode", msg);
            #endregion
        }
        return Result;

    }

    #endregion

    #region 通过邮箱重置密码

    public class ResetPwdByEmailCheckResult
    {
        public Int32 Result;
        public String ErrorDescription;
        public String CustID;
        public String UserName;
        public String Email;
    }

    [WebMethod(Description = "邮箱重置密码前邮箱用户名等信息确认，确认成功，发送重置链接邮件")]
    public ResetPwdByEmailCheckResult ResetPwdByEmailCheck(String SPID, String UserName, String Email, String RedirectUrl, String ExtendField)
    {
        ResetPwdByEmailCheckResult Result = new ResetPwdByEmailCheckResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.CustID = "";
        Result.UserName = UserName;
        Result.Email = Email;

        try
        {
            #region 数据合法性验证
            //数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "CheckEmailByUserName", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }
            if (CommonUtility.IsEmpty(Email))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidEmail_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidEmail_Msg + "——邮箱不能为空";
                return Result;
            }
            //验证邮箱格式
            if (!CommonUtility.ValidateEmail(Email))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidEmail_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidEmail_Msg + "——邮箱格式不正确";
                return Result;
            }
            if (CommonUtility.IsEmpty(RedirectUrl))
            {
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg + "——参数RedirectUrl不正确，RedirectUrl不能为空";
                return Result;
            }
            //Int32 expiredHour = 0;
            //if (!String.IsNullOrEmpty(ExpiredHour))
            //{
            //    if (!Int32.TryParse(ExpiredHour, out expiredHour))
            //    {
            //        Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg + "——参数ExpiredHour(单位:小时)输入不正确，ExpiredHour必须是大于等于0的整数，为0时表示无超时时间";
            //        return Result;
            //    }
            //}
            #endregion

            int i = SetMail.FindPwdByEmail(UserName, Email, out Result.ErrorDescription);
            if (i == 0)
            {
                //查询出CustID
                String[] str = FindPwd.SelPwdByEmailandName(UserName, Email, out Result.ErrorDescription);
                String CustID = str[0].ToString();
                Result.CustID = CustID;
                //生成随机验证码
                Random random = new Random();
                String authenCode = random.Next(111111, 999999).ToString();
                //数据库操作
                String encryptUrl = CommonBizRules.EncryptEmailURl(SPID, CustID, Email, RedirectUrl, authenCode, HttpContext.Current);
                String resetUrl = ConstHelper.DefaultInstance.BesttoneResetPwdByEmail + "?UrlParam=" + HttpUtility.UrlEncode(encryptUrl);
                String link = "点击重置密码:<a href='" + resetUrl + "'>" + resetUrl + "</a>";

                Int32 expiredHour = ConstHelper.DefaultInstance.ResetPwdExpiredHour;

                Result.Result = SetMail.InsertEmailByResetPwd(CustID, "2", link, authenCode, 1, Email, DateTime.Now, "描述", "中国电信号码百事通：找回密码", 1, expiredHour, out Result.ErrorDescription);
            }
            else
            {
                Result.Result = i;
                return Result;
            }
        }
        catch (Exception ex)
        {
            Result.ErrorDescription = ErrorDefinition.BT_IError_Result_UnknowError_Msg + ex.Message;
        }
        finally
        {
            try
            {
                #region 数据库日志

                String inParam = String.Format("SPID:{0},UserName:{1},Email:{2},RedirectUrl:{3},ExtendField:{4}", SPID, UserName, Email, RedirectUrl, ExtendField);
                String outParam = String.Format("CustID:{0},UserName:{1},Email:{2}", Result.CustID, Result.UserName, Result.Email);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "ResetPwdByEmailCheck", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion
            }
            catch { }
        }
        return Result;
    }

    #endregion

    #region 【暂时不用】客户密码认证接口 (仝波 2009-08-11)
    public class CustPwdAuthResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "客户密码认证接口")]
    public CustPwdAuthResult CustPwdAuth(string SPID, string CustID, string Pwd, string PwdType, string ExtendField)
    {
        CustPwdAuthResult Result = new CustPwdAuthResult();
        Result.CustID = "";
        Result.ExtendField = "";
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;

        try
        {
            #region 数据校验
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

            if (CommonUtility.IsEmpty(CustID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }
            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(Pwd))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(PwdType))
            {
                Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "，不能为空";
                return Result;
            }

            string tmpPwdType = "0;1;2";
            if (tmpPwdType.IndexOf(PwdType) < 0)
            {
                Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "，密码类型错误";
                return Result;
            }


            #endregion

            // Result.Result = UserAuthenClass.CustPwdAuth(SPID, CustID, Pwd, PwdType, ExtendField, this.Context, out Result.CustID, out Result.ErrorDescription);
        }
        catch (Exception err)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = err.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("客户密码认证接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";CustID - " + CustID);
            msg.Append(";Pwd - " + Pwd);
            msg.Append(";PwdType - " + PwdType);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result);
            msg.Append("NewCustID - " + Result.CustID);
            msg.Append("; 错误描述 - " + Result.ErrorDescription + "\r\n");
            msg.Append("; ExtendField - " + ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("CustPwdAuth", msg);
            #endregion
        }
        return Result;
    }
    #endregion

    #region 【暂时不用】密码问题查询接口
    public class PwdQuestionQueryResult
    {
        public int Result;
        public PwdQuestionRecord[] PwdQuestionRecords;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "密码问题查询接口")]
    public PwdQuestionQueryResult PwdQuestionQuery(string SPID, string ExtendField)
    {
        PwdQuestionQueryResult Result = new PwdQuestionQueryResult();
        return Result;
    }
    #endregion

    #region 【暂时不用】密码提示问题上传接口
    public class PwdQuestionUploadResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "密码提示问题上传接口")]
    public PwdQuestionUploadResult PwdQuestionUpload(string SPID, string CustID, PwdQARecord[] PwdQARecords, string ExtendField)
    {
        PwdQuestionUploadResult Result = new PwdQuestionUploadResult();
        return Result;
    }
    #endregion

    #region 【暂时不用】密码提示问题验证接口
    public class PwdQuestionAuthResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "密码提示问题验证接口")]
    public PwdQuestionAuthResult PwdQuestionAuth(string SPID, string CustID, string QuestionID, string Answer, string ExtendField)
    {
        PwdQuestionAuthResult Result = new PwdQuestionAuthResult();
        return Result;
    }
    #endregion

    #endregion

    #region 电话服务接口

    #region 电话识别接口(刘春利 2009-07-31)
    /// <summary>
    /// 电话识别接口返回记录
    /// 作者：刘春利    时间：2009-7-31
    /// 修改：          时间：
    /// </summary>
    public class QueryByPhoneResult
    {
        public int Result;
        public BasicInfoV2Record[] BasicInfoV2Records;
        public string ErrorDescription;
        public string ExtendField;
    }

    /// <summary>
    /// 电话识别接口
    /// 作者：刘春利    时间：2009-7-31
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "电话识别接口")]
    public QueryByPhoneResult QueryByPhone(string SPID, string PhoneNum, string ExtendField)
    {
        string custid_first = "";
        string custtype_first = "";
        QueryByPhoneResult Result = new QueryByPhoneResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.BasicInfoV2Records = null;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "QueryByPhone", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //检查电话号码是否为空
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
            #endregion

            //根据电话取得相关的客户信息（客户电话表CustPhone）
            Result.BasicInfoV2Records = PhoneBO.GetQueryByPhone(SPID, PhoneNum, out Result.Result, out Result.ErrorDescription);

            if (Result.BasicInfoV2Records.Length == 0)
            {
                Result.Result = ErrorDefinition.IError_Result_Null_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_Null_Msg;
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
                StringBuilder outParam = new StringBuilder();
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("电话识别接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                outParam.Append("Result:" + Result.Result);
                outParam.Append(",ErrMsg:" + Result.ErrorDescription);
                outParam.Append(",ExtendField:" + Result.ExtendField);

                if (Result.BasicInfoV2Records != null)
                {
                    if (Result.BasicInfoV2Records.Length > 0)
                    {
                        msg.Append("BasicInfoV2Records: \r\n");
                        
                        for (int i = 0; i < Result.BasicInfoV2Records.Length; i++)
                        {
                            msg.Append("CustID- " + Result.BasicInfoV2Records[i].CustID + ";");
                            msg.Append("CustType- " + Result.BasicInfoV2Records[i].CustType + ";");
                            msg.Append("PhoneClass- " + Result.BasicInfoV2Records[i].PhoneClass + ";");
                            msg.Append("RealName- " + Result.BasicInfoV2Records[i].RealName + ";");
                            msg.Append("Sex- " + Result.BasicInfoV2Records[i].Sex + ";");
                            msg.Append("ExtendField- " + Result.BasicInfoV2Records[i].ExtendField + "\r\n");

                            outParam.Append(",[");
                            outParam.Append(",CustID:" + Result.BasicInfoV2Records[i].CustID);
                            outParam.Append(",CustType: " + Result.BasicInfoV2Records[i].CustType);
                            outParam.Append(",PhoneClass: " + Result.BasicInfoV2Records[i].PhoneClass);
                            outParam.Append(",RealName: " + Result.BasicInfoV2Records[i].RealName);
                            outParam.Append(",Sex: " + Result.BasicInfoV2Records[i].Sex);
                            outParam.Append(",ExtendField: " + Result.BasicInfoV2Records[i].ExtendField);
                            outParam.Append("]");
                        }
                        
                        custid_first = Result.BasicInfoV2Records[0].CustID;
                        custtype_first = Result.BasicInfoV2Records[0].CustType;
                    }
                }
                msg.Append("\r\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("QueryByPhone", msg);
                #endregion

                //数据库日志
                //String inParam = String.Format("SPID:{0},PhoneNum:{1},ExtendField:{2}", SPID, PhoneNum, ExtendField);
                //CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "QueryByPhone", inParam, outParam.ToString(), Result.Result, Result.ErrorDescription);

                //写数据库日志
                //CIP2BizRules.WriteQueryByPhoneDataLog(PhoneNum, SPID, custid_first, custtype_first, "", Result.Result, Result.ErrorDescription);
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 电话识别接口v2(李宏图 2012-03-13)
    /// <summary>
    /// 电话识别接口返回记录
    /// 作者：李宏图    时间：2009-7-31
    /// 修改：          时间：
    /// </summary>
    public class QueryByPhoneV2Result
    {
        public int Result;
        public BasicInfoV3Record[] BasicInfoV2Records;
        public string ErrorDescription;
        public string ExtendField;
    }
    /// <summary>
    /// 电话识别接口
    /// 作者：李宏图    时间：2009-7-31
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "电话识别接口")]
    public QueryByPhoneV2Result QueryByPhoneV2(string SPID, string PhoneNum, string ExtendField)
    {
        string custid_first = "";
        string custtype_first = "";
        QueryByPhoneV2Result Result = new QueryByPhoneV2Result();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.BasicInfoV2Records = null;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "QueryByPhoneV2", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //检查电话号码是否为空
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
            #endregion

            //根据电话取得相关的客户信息（客户电话表CustPhone）
            Result.BasicInfoV2Records = PhoneBO.GetQueryByPhoneV2(SPID, PhoneNum, out Result.Result, out Result.ErrorDescription);

            if (Result.BasicInfoV2Records.Length == 0)
            {
                Result.Result = ErrorDefinition.IError_Result_Null_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_Null_Msg;
            }
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
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("电话识别接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";PhoneNum - " + PhoneNum);
            msg.Append(";ExtendField - " + ExtendField + "\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            if (Result.BasicInfoV2Records != null)
            {
                if (Result.BasicInfoV2Records.Length > 0)
                {
                    msg.Append("BasicInfoV2Records: \r\n");
                    for (int i = 0; i < Result.BasicInfoV2Records.Length; i++)
                    {
                        msg.Append("CustID- " + Result.BasicInfoV2Records[i].CustID + ";");
                        msg.Append("CustType- " + Result.BasicInfoV2Records[i].CustType + ";");
                        msg.Append("PhoneClass- " + Result.BasicInfoV2Records[i].PhoneClass + ";");
                        msg.Append("RealName- " + Result.BasicInfoV2Records[i].RealName + ";");
                        msg.Append("RealName- " + Result.BasicInfoV2Records[i].EnterpriseName + ";");
                        msg.Append("Sex- " + Result.BasicInfoV2Records[i].Sex + ";");
                        msg.Append("ExtendField- " + Result.BasicInfoV2Records[i].ExtendField + "\r\n");
                    }

                    custid_first = Result.BasicInfoV2Records[0].CustID;
                    custtype_first = Result.BasicInfoV2Records[0].CustType;
                }
            }
            msg.Append("\r\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("QueryByPhoneV2", msg);
            #endregion

            //写数据库日志
            try
            {
                CIP2BizRules.WriteQueryByPhoneDataLog(PhoneNum, SPID, custid_first, custtype_first, "", Result.Result, Result.ErrorDescription);
            }
            catch { }
        }
        return Result;
    }
    #endregion

    #region 积分客户数据同步失败查询(李宏图 2012-07-07)
    /// <summary>
    /// 积分客户数据同步失败返回记录
    /// 作者：李宏图    时间：2012-7-07
    /// 修改：          时间：
    /// </summary>
    public class QueryCustinfoNotifyResult
    {
        public int Result;
        public CustInfoNotifyRecord[] CustInfoNotifyRecords;
        public string ErrorDescription;
        public string ExtendField;
    }
    /// <summary>
    /// 积分客户数据同步失败查询接口
    /// 作者：李宏图    时间：2012-7-09
    /// </summary>
    /// <param name="SPID"></param>
    /// <param name="ExtendField"></param>
    /// <returns></returns>
    [WebMethod(Description = "积分客户数据同步失败查询")]
    public QueryCustinfoNotifyResult QueryCustinfoNotify(string SPID, string ExtendField)
    {
        QueryCustinfoNotifyResult Result = new QueryCustinfoNotifyResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.CustInfoNotifyRecords = null;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "QueryCustinfoNotify", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            #endregion
            Result.CustInfoNotifyRecords  =  CIP2BizRules.QueryCustInfoNotify();
            if (Result.CustInfoNotifyRecords.Length == 0)
            {
                Result.Result = ErrorDefinition.IError_Result_Null_Code;
                Result.ErrorDescription = ErrorDefinition.IError_Result_Null_Msg;
            }
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
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("积分客户数据同步失败查询接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";ExtendField - " + ExtendField + "\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("\r\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("QueryCustinfoNotify", msg);
            #endregion
        }
            return Result;
    }
    
    #endregion

    #region 删除同步失败积分客户数据(李宏图 2012-07-07)
    /// <summary>
    /// 积分客户数据同步失败返回记录
    /// 作者：李宏图    时间：2012-7-07
    /// 修改：          时间：
    /// </summary>
    public class DeleteCustinfoNotifyFailedResult
    {
        public int Result;
        public string ErrorDescription;
        public string ExtendField;
    }
    /// <summary>
    /// 积分客户数据同步失败查询接口
    /// 作者：李宏图    时间：2012-7-09
    /// </summary>
    /// <param name="SPID"></param>
    /// <param name="ExtendField"></param>
    /// <returns></returns>
    [WebMethod(Description = "积分客户数据同步失败查询")]
    public DeleteCustinfoNotifyFailedResult DeleteCustinfoFailedNotify(string SPID, string SequenceID,string ExtendField)
    {
        DeleteCustinfoNotifyFailedResult Result = new DeleteCustinfoNotifyFailedResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
       
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "QueryCustinfoNotify", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            #endregion
            CIP2BizRules.DeleteHasPulledFailedCustInfoNotifyData(SequenceID);
  
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
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("删除积分客户数据同步失败接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SPID - " + SPID);
            msg.Append(";ExtendField - " + ExtendField + "\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("\r\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("DeleteCustinfoFailedNotify", msg);
            #endregion
        }
        return Result;
    }

    #endregion

    #region 电话设置接口（刘春利 2009-08-05）
    /// <summary>
    /// 电话设置接口返回记录
    /// 作者：刘春利    时间：2009-08-05
    /// 修改：          时间：
    /// </summary>
    public class PhoneSetResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    /// <summary>
    /// 电话设置接口
    /// 作者：刘春利    时间：2009-08-05
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "电话设置接口")]
    public PhoneSetResult PhoneSet(string SPID, string CustID, string PhoneNum, string PhoneClass, string PhoneType, string ExtendField)
    {
        PhoneSetResult Result = new PhoneSetResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.CustID = CustID;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "PhoneSet", this.Context, out Result.ErrorDescription);
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

            //检查CustID长度是否小于16位
            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            //检查Phone是否为空
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
            else
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_Phone_NumberInValid_Code;
                Result.ErrorDescription = ErrorDefinition.CIP_IError_Result_Phone_NumberInValid_Msg + ",不能为空";
                return Result;
            }

            //检查PhoneClass是否为空
            if (CommonUtility.IsEmpty(PhoneClass))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }

            //检查PhoneType是否为空
            if (CommonUtility.IsEmpty(PhoneType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }
            #endregion

            //电话设置
            Result.Result = PhoneBO.PhoneSet(SPID, CustID, PhoneNum, PhoneClass, PhoneType, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("电话设置接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append("CustID - " + CustID);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append(";PhoneType - " + PhoneType);
                msg.Append(";PhoneClass - " + PhoneClass + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PhoneSet", msg);
                #endregion

                #region 写数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1},PhoneNum:{2},PhoneClass:{3},PhoneType:{4},ExtendField:{5}", 
                                                    SPID, CustID, PhoneNum, PhoneClass, PhoneType, ExtendField);
                String outParam = String.Format("Result:{0},CustID:{1},ErrorDescription:{2},ExtendField:{3}",
                                        Result.Result, Result.CustID, Result.ErrorDescription, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "PhoneSet", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Result.Result, Result.ErrorDescription, PhoneNum, "PhoneSet");
            }
            catch { }
        }

        return Result;
    }
    #endregion

    #region 电话解绑接口（刘春利 2009-08-06）
    /// <summary>
    /// 电话解绑接口返回记录
    /// 作者：刘春利    时间：2009-08-06 2012-07-20 解绑短信通知客户
    /// 修改：          时间：
    /// </summary>
    public class PhoneUnBindResult
    {
        public int Result;
        public string CustID;
        public string ProvinceID;
        public string ErrorDescription;
        public string ExtendField;
    }

    /// <summary>
    /// 电话解绑接口
    /// 作者：刘春利    时间：2009-08-06
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "电话解绑接口")]
    public PhoneUnBindResult PhoneUnBind(string SPID, string PhoneNum, string PhoneClass, string CustID, string ExtendField)
    {
        PhoneUnBindResult Result = new PhoneUnBindResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.CustID = CustID;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "PhoneUnBind", this.Context, out Result.ErrorDescription);
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

            //检查CustID长度是否小于16位
            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            //检查Phone是否为空
            if (!CommonUtility.IsEmpty(PhoneNum))
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, PhoneNum, out phone))
                {
                    Result.Result = ErrorDefinition.CIP_IError_Result_Phone_NumberInValid_Code;
                    Result.ErrorDescription = ErrorDefinition.CIP_IError_Result_Phone_NumberInValid_Msg + "，电话格式无效";
                    return Result;
                }
                PhoneNum = phone;
            }
            else 
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Code;
                Result.ErrorDescription = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Msg + "，不能为空";
                return Result;
            }

            //检查PhoneClass是否为空
            if (CommonUtility.IsEmpty(PhoneClass))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }
            #endregion

            //电话解绑
            Result.Result = PhoneBO.PhoneUnBind(CustID, PhoneNum, PhoneClass, out Result.ErrorDescription);
            if (Result.Result == 0)
            {
                //电话解绑通知用户
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(this.Context, "SPData");
                String SPName = spInfo.GetPropertyBySPID(SPID, "SPName", SPData);
                CommonBizRules.SendMessage(PhoneNum, "您的号码已被解除绑定(将不能通过该号码登录号百平台):此操作由" + SPID + "-" + SPName + "-平台发起!", "35000000");
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("电话解绑接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append("CustID - " + CustID);
                msg.Append(";PhoneNum - " + PhoneNum);
                msg.Append(";PhoneClass - " + PhoneClass + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PhoneUnBind", msg);
                #endregion

                #region 写数据库日志

                String inParam = String.Format("SPID:{0},PhoneNum:{1},PhoneClass:{2},CustID:{3},ExtendField:{4}", SPID, PhoneNum, PhoneClass, CustID, ExtendField);
                String outParam = String.Format("Result:{0},CustID:{1},ProvinceID:{2},ErrorDescription:{3},ExtendField:{4}",
                                    Result.Result, Result.CustID, Result.ProvinceID, Result.ErrorDescription, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "PhoneUnBind", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Result.Result, Result.ErrorDescription, PhoneNum, "PhoneUnBind");
            }
            catch { }
        }

        return Result;
    }
    #endregion

    #region 认证电话变更接口（刘春利 2009-08-10）
    /// <summary>
    /// 认证电话变更接口返回记录
    /// 作者：刘春利    时间：2009-08-10
    /// 修改：          时间：
    /// </summary>
    public class AuthenPhoneChangeResult
    {
        public int Result;
        public string CustID;
        public string ErrorDescription;
        public string ExtendField;
    }

    /// <summary>
    /// 认证电话变更接口
    /// 作者：刘春利    时间：2009-08-10
    /// 修改：          时间：
    /// </summary>
    [WebMethod(Description = "认证电话变更接口")]
    public AuthenPhoneChangeResult AuthenPhoneChange(string SPID, string AuthenPhone, string CustID, string ExtendField)
    {
        AuthenPhoneChangeResult Result = new AuthenPhoneChangeResult();
        Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg;
        Result.CustID = CustID;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //检查SPID是否为空
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + ",不能为空";
                return Result;
            }

            //检查SPID长度是否有误
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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AuthenPhoneChange", this.Context, out Result.ErrorDescription);
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

            //检查CustID长度是否小于16位
            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return Result;
            }

            //检查AuthenPhone是否为空
            if (CommonUtility.IsEmpty(AuthenPhone))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Code;
                Result.ErrorDescription = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Msg + "，不能为空";
                return Result;
            }
            else 
            {
                string phone = "";
                if (!CommonBizRules.PhoneNumValid(this.Context, AuthenPhone, out phone))
                {
                    Result.Result = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Code;
                    Result.ErrorDescription = ErrorDefinition.CIP_IError_Result_Phone_AuthenPhoneInValid_Msg + "，认证电话无效";
                    return Result;
                }
                AuthenPhone = phone;
            }

            #endregion

            Result.Result = PhoneBO.ChangeAuthenPhone(CustID, AuthenPhone, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("认证电话变更接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append("CustID - " + CustID);
                msg.Append(";AuthenPhone - " + AuthenPhone + "\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AuthenPhoneChange", msg);
                #endregion

                #region 写数据库日志

                String inParam = String.Format("SPID:{0},AuthenPhone:{1},CustID:{2},ExtendField:{3}", SPID, AuthenPhone, CustID, ExtendField);
                String outParam = String.Format("Result:{0},CustID:{1},ErrorDescription:{2},ExtendField:{3}", Result.Result, Result.CustID, Result.ErrorDescription, Result.ExtendField);

                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "AuthenPhoneChange", inParam, outParam, Result.Result, Result.ErrorDescription);

                #endregion

                //CommonBizRules.WriteDataLog(SPID, Result.CustID, "", Result.Result, Result.ErrorDescription, "", "AuthenPhoneChange");
            }
            catch { }
        }

        return Result;
    }
    #endregion

    #region 【暂时屏蔽不使用】定位手机位置接口

    public class PhonePositionResult
    {
        public int Result;
        public string PhoneID;
        public string PhoneType;
        public string ResultLevel;
        public string MaxAge;
        public string ActiveTime;
        public string AreaID;
        public string Area;
        public string LatitudeType;
        public string Latitude;
        public string LongitudeType;
        public string Longitude;
        public string ExtendField;
        public string ErrorDescription;
    }

    //[WebMethod(Description = "定位手机位置接口")]
    public PhonePositionResult PhonePosition(string SPID, string PhoneID, string PhoneType, string ResultLevel, string MaxAge, string ExtendField)
    {
        
        PhonePositionResult Result = new PhonePositionResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        System.DateTime startTime = System.DateTime.Now;

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
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "PhonePosition", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(PhoneID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                Result.ErrorDescription = "移动台标识不能为空";
                return Result;
            }
            else
            {

                string ptn = @"^133|153|18[0|9]\d{8}$";
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(ptn);

                if (reg.IsMatch(PhoneID))
                {

                }
                else
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                    Result.ErrorDescription = "非合法的移动台标识";
                    return Result;
                }

            }


            if (CommonUtility.IsEmpty(PhoneType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                Result.ErrorDescription = "移动台类型不能为空";
                return Result;
            }
            if (!"0".Equals(PhoneType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                Result.ErrorDescription = "移动台类型不支持";
                return Result;
            }

            if (CommonUtility.IsEmpty(ResultLevel))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                Result.ErrorDescription = "返回精度要求不能为空";
                return Result;
            }
            if (!("1".Equals(ResultLevel) || "2".Equals(ResultLevel)))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                Result.ErrorDescription = "返回精度要求不支持";
                return Result;
            }

            if (CommonUtility.IsEmpty(MaxAge))
            {
                MaxAge = "30";
            }


            #endregion
            Result.Result = PhoneBO.PhonePositionQuery(SPID, PhoneID, PhoneType, ResultLevel, MaxAge, startTime, ExtendField, out  Result.ActiveTime, out Result.AreaID, out Result.Area,
                out Result.LatitudeType, out Result.Latitude, out Result.LongitudeType, out Result.Longitude, out Result.ExtendField, out Result.ErrorDescription);
            Result.PhoneID = PhoneID;
            Result.PhoneType = PhoneType;
            Result.MaxAge = MaxAge;
            Result.ResultLevel = ResultLevel;

            //Result.Result = CIPTicketManager.checkCIPTicket(SPID, Ticket, ExtendField, out Result.CustID, out Result.RealName, out Result.UserName, out Result.NickName, out Result.OuterID, "", out Result.LoginAuthenName, out Result.LoginAuthenType, out Result.ErrorDescription);
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
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("定位手机位置接口 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";SPID - " + SPID);
                msg.Append(";PhoneID - " + PhoneID);
                msg.Append(";PhoneType - " + PhoneType);
                msg.Append(";MaxAge - " + MaxAge);
                msg.Append(";ExtendField - " + ExtendField);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; PhoneID - " + Result.PhoneID);
                msg.Append("; PhoneType - " + Result.PhoneType);
                msg.Append("; AreaID - " + Result.AreaID);
                msg.Append("; Area - " + Result.Area);
                msg.Append("; ErrorDescription - " + Result.ErrorDescription);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PhonePosition", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},PhoneID:{1},PhoneType:{2},ResultLevel:{3},MaxAge:{4},ExtendField:{5}",
                                    SPID, PhoneID, PhoneType, ResultLevel, MaxAge, ExtendField);

                String outParam = String.Format("Result:{0},PhoneID:{1},PhoneType:{2},ResultLevel:{3},MaxAge:{4},ActiveTime:{5},AreaID:{6},Area:{7},LatitudeType:{8},Latitude:{9},LongitudeType:{10},Longitude:{11},ExtendField:{12},ErrorDescription:{13}",
                                        Result.Result, Result.PhoneID, Result.PhoneType, Result.ResultLevel, Result.MaxAge, Result.ActiveTime, Result.AreaID, Result.Area, Result.LatitudeType, Result.Latitude, Result.LongitudeType, Result.Longitude, Result.ExtendField, Result.ErrorDescription);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "PhonePosition", inParam, outParam, Result.Result, Result.ErrorDescription);
                
                #endregion
            }
            catch { }
        }

        return Result;

    }
    #endregion

    #region 手机号归属地信息查询接口
    public class PhoneAreaQueryResult
    {
        public string ProvinceID;
        public string AreaID;
        public string ExtendField;
    }
    [WebMethod(Description = "手机号归属地信息查询接口")]
    public PhoneAreaQueryResult PhoneAreaQuery(string SPID, string NPNumber, string ExtendField)
    {
        PhoneAreaQueryResult Result = new PhoneAreaQueryResult();
        Result.ProvinceID = "";
        Result.AreaID = "";
        Result.ExtendField = "";

        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        Int32 result1 = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        try
        {
            if (CommonUtility.IsEmpty(SPID))
            {
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                return Result;
            }
            if (CommonUtility.IsEmpty(NPNumber))
            {
                return Result;
            }

            result1 = BTForBusinessSystemInterfaceRules.GetPhoneTOArea(NPNumber, out Result.ProvinceID, out Result.AreaID, out ErrMsg);
        }
        catch
        { }
        finally
        {
            try
            {
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("手机号归属地信息查询接口" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";SPID - " + SPID);
                msg.Append(":NPNumber - " + NPNumber);
                msg.Append(";ExtendField - " + ExtendField + "\r\n");

                msg.Append("处理结果 - ");

                msg.Append(": ProvinceID - " + Result.ProvinceID);
                msg.Append(": AreaID - " + Result.AreaID);
                msg.Append("; ExtendField - " + Result.ExtendField);

                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("PhoneAreaQuery", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},NPNumber:{1},ExtendField:{2}", SPID, NPNumber, ExtendField);
                String outParam = String.Format("ProvinceID:{0},AreaID:{1},ExtendField:{2}", Result.ProvinceID, Result.AreaID, Result.ExtendField);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "PhoneAreaQuery", inParam, outParam, result1, ErrMsg);

                #endregion
            }
            catch { }
        }

        return Result;
    }
    #endregion

    #endregion

    #region 商旅卡

    public class TourCardResult
    {
        public int Result;
        public string CardID;
        public string CardType;
        public string InnerCardID;
        public string ErrorDescription;
        public string ExtendField;
        public string parCardID;
        public string CardRegSource;
        public string CardRegType;
    }

    [WebMethod(Description="根据客户编号生成商旅卡接口")]
    public TourCardResult GenerateTourcardByCustId(string SPID,string CustID)
    {
        TourCardResult result = new TourCardResult();
        result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        result.CardID = "";
        result.CardType = "";
        result.InnerCardID = "";
        result.ExtendField = "";
        result.parCardID = "";
        result.ErrorDescription = "";
        result.CardRegSource = "";
        result.CardRegType = "";
        try
        {
            #region 数据有效性验证
            //SPID是否允许访问
            if (CommonUtility.IsEmpty(SPID))
            {
                result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return result;
            }

            //IP是否允许访问
            result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out result.ErrorDescription);
            if (result.Result != 0)
            {
                return result;
            }

            //接口访问权限判断
            result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "CustBasicInfoQuery", this.Context, out result.ErrorDescription);
            if (result.Result != 0)
            {
                return result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return result;
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
            }

            if (CommonUtility.IsEmpty(SPID) && CommonUtility.IsEmpty(CustID))
            {
                result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "关键字信息不能全为空";
                return result;
            }

            #endregion

            CustBasicInfoQueryResult QueryResult = CustBasicInfoQuery(SPID, CustID, "");
            if (QueryResult.CustID == "")
            {
                result.Result = -2568;
                result.ErrorDescription = "该客户尚未注册!";
                // 该客户尚未注册
                return result;
            }

            if(QueryResult.TourCardIDRecords!=null && QueryResult.TourCardIDRecords.Length>0){
                // 该客户已经有商旅卡
                //返回错误
                result.Result = -2569;
                result.ErrorDescription = "该客户已经有商旅卡!";
                return result;
            }

            string UProvinceID = QueryResult.ProvinceID;
            string AreaCode = QueryResult.AreaID;
            string CustLevel = QueryResult.CustLevel;
            Int32 CardType = 1;
            string CardID = "";
            string CardRegSource = "01";
            string CardRegType = "1";
            string o_UserAccount = "";
            string o_sUserAccount = "";
            string ErrMsg = "";

            //result.Result = UserRegistry.genTourCard(CustID, CardID, UProvinceID, AreaCode, CardType, CustLevel, CardRegSource, CardRegType, out o_sUserAccount,out o_UserAccount, out ErrMsg);
            result.Result = UserRegistry.GenerationCard(SPID, CustID, CardID, UProvinceID, AreaCode, CardType, CustLevel, CardRegSource, CardRegType, out o_UserAccount, out o_sUserAccount, out ErrMsg);

            result.InnerCardID = o_UserAccount;
            
            result.ErrorDescription = ErrMsg;
        }
        catch (System.Exception ex)
        {
            result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
        }
        finally
        {
            try
            {
                #region 文本日志
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("根据客户编号生成商旅卡接口 " + DateTime.Now.ToString("u") + "\r\n");

                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append("\r\n");

                msg.Append("处理结果 - " + result.Result);
                msg.Append("; 错误描述 - " + result.ErrorDescription);
                msg.Append("; InnerCardID - " + result.InnerCardID);
                msg.Append("; ExtendField - " + result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("GenerateTourcardByCustId", msg);
                #endregion

                #region 数据库日志

                String inParam = String.Format("SPID:{0},CustID:{1}", SPID, CustID);
                String outParam = String.Format("Result:{0},CardID:{1},CardType:{2},InnerCardID:{3},ErrorDescription:{4},ExtendField:{5},parCardID:{6},CardRegSource:{7},CardRegType:{8}",
                                        result.Result, result.CardID, result.CardType, result.InnerCardID, result.ErrorDescription, result.ExtendField, result.parCardID, result.CardRegSource, result.CardRegType);
                CommonBizRules.WriteCallInterfaceLog_DB(HttpContext.Current.Request.UserHostAddress, SPID, "GenerateTourcardByCustId", inParam, outParam, result.Result, result.ErrorDescription);
                
                #endregion
            }
            catch { }

        }
        return result;
    }

    #endregion

    /******************单点登陆*************************************************/
    public class IntensiveAssertionResult
    {
	    public int Result;
        public string ErrorDescription;
        public string CAP02001_RES;
    }
    [WebMethod(Description = "集团积分商城单点省积分商城断言查询")]
    public  IntensiveAssertionResult IntensiveAssertionSelect(string CAP02001_REQ)
    {
        string ErrMsg = "";
        IntensiveAssertionResult Result = new IntensiveAssertionResult();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(CAP02001_REQ);
        String Ticket = xmlDoc.SelectNodes("/Root/Body/AssertionReq/Ticket")[0].InnerText;

        SqlConnection mycon = null;
        SqlCommand cmd = new SqlCommand();
        try
        {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IntensiveAssertionSelect";


                SqlParameter parTicket = new SqlParameter("@Ticket", SqlDbType.VarChar, 50);
                parTicket.Value = Ticket;
                cmd.Parameters.Add(parTicket);

  
                SqlParameter parAccountType = new SqlParameter("@AccountType",SqlDbType.VarChar, 10);
                parAccountType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAccountType);

                SqlParameter parAccountID = new SqlParameter("@AccountID", SqlDbType.VarChar, 30);
                parAccountID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAccountID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 4);
                parProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 4);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);


                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                int SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

                string AccountType = parAccountType.Value.ToString();
                string AccountID = parAccountID.Value.ToString();
                string CustID = parCustID.Value.ToString();
                string ProvinceID = parProvinceID.Value.ToString();
                string AreaID = parAreaID.Value.ToString();

                StringBuilder response = new StringBuilder();
                #region 拼接请求xml字符串

                response.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                response.Append("<Root>");
                response.Append("<Body>");
                response.Append("<AssertionResp>");

                //添加参数
                response.AppendFormat("<AccountType>{0}</AccountType>", AccountType);
                response.AppendFormat("<AccountID>{0}</AccountID>", AccountID);
                response.AppendFormat("<CustID>{0}</CustID>", CustID);
                response.AppendFormat("<ProvinceID>{0}</ProvinceID>", ProvinceID);
                response.AppendFormat("<AreaID>{0}</AreaID>", AreaID);

                response.Append("</AssertionResp>");
                response.Append("</Body>");
                response.Append("</Root>");
                #endregion

                Result.Result = SqlResult;
                Result.ErrorDescription = ErrMsg;
                Result.CAP02001_RES = response.ToString();
        }
        catch(System.Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }finally
        {
            #region 文本日志
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("集团积分商城单点省积分商城断言查询 " + DateTime.Now.ToString("u") + "\r\n");

            msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; CAP02001_RESPONSE - " + Result.CAP02001_RES + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("IntensiveAssertionSelect", msg);
            #endregion
        }

        return Result;
    }




    #region CAP03001 向全国ua查询该客户归属UA身份认证地址
    public static int GetUAAuthenAddress(string SPID,string ProvinceID, HttpContext SpecificContext, string SPDataCacheName, string ExtendField, out MBOSSClass.SSOAddressResp SSOAddress, out string ErrMsg, out string newExtendField)
    {
        newExtendField = "";
        int result = -20005;
        ErrMsg = "";
        SSOAddress = new MBOSSClass.SSOAddressResp();

        MBOSSClass Mboss = new MBOSSClass();
        //SPInfoManager spInfo = new SPInfoManager();
        try
        {
        //    Object SPData = spInfo.GetSPData(SpecificContext, SPDataCacheName);
        //    privateKeyFile = spInfo.GetCAInfo("35111111", 1, SPData, out UserName, out privateKeyPassword);
            result = Mboss.AuthenSelectArddess(SPID,ProvinceID,SpecificContext,SPDataCacheName, out SSOAddress, out ErrMsg);
        }
        catch (Exception err)
        {
            ErrMsg = err.Message;
            result = -20001;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("CAP03001身份认证地址 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";ProvinceID - " + ProvinceID);
            msg.Append(";SpecificContext - " + SpecificContext);
            msg.Append(";SPDataCacheName - " + SPDataCacheName);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + result);
            msg.Append("SSOAddress - " + SSOAddress.SSOAddress);
            msg.Append("AssertionAddress - " + SSOAddress.AssertionAddress);
            msg.Append("; 错误描述 - " + ErrMsg);
            msg.Append("; ExtendField - " + newExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("GetUAAuthenAddress", msg);
            #endregion
        }

        return result;
    }
    #endregion

    #region  CAP01003 向归属地ＵＡ发送身份认证请求
    public static int SendSSOAuthanXML(string ProvinceID,string SPID, string RedirectURL, HttpContext SpecificContext, string SPDataCacheName, MBOSSClass.AcceptAccountTypeList[] AcceptAccountTypes, string ExtendField, string TransactionID, out string ResultXML, out string ErrMsg, out string newExtendField)
    {
        int result = -20005;
        ResultXML = "";
        ErrMsg = "";
        newExtendField = "";
        MBOSSClass Mboss = new MBOSSClass();
        //SPInfoManager spInfo = new SPInfoManager();
        //byte[] privateKeyFile;
        //string UserName = "";
        //string privateKeyPassword = "";
        try
        {
            //Object SPData = spInfo.GetSPData(SpecificContext, SPDataCacheName);
            //privateKeyFile = spInfo.GetCAInfo("35111111", 1, SPData, out UserName, out privateKeyPassword);
            result = Mboss.SSOAuthanXML(ProvinceID,SPID, RedirectURL, AcceptAccountTypes, SpecificContext, SPDataCacheName, out ResultXML, out ErrMsg, out TransactionID);
        }
        catch (Exception err)
        {
            result = -20005;
            ErrMsg = err.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append(" CAP01003 发送身份认证请求 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";RedirectURL - " + RedirectURL);
            msg.Append(";SpecificContext - " + SpecificContext);
            msg.Append(";SPDataCacheName - " + SPDataCacheName + "\r\n");
            if (AcceptAccountTypes != null)
            {
                for (int i = 0; i < AcceptAccountTypes.Length; i++)
                {
                    msg.Append(";AcceptAccountType - " + AcceptAccountTypes[i].AcceptAccountType);
                }
            }
            else
            {
                msg.Append(";AcceptAccountType - " + "");
            }
            msg.Append("\r\n ;ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + result);
            msg.Append("ResultXML - " + ResultXML);
            msg.Append("; 错误描述 - " + ErrMsg);
            msg.Append("; ExtendField - " + newExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("SendSSOAuthanXML", msg);
            #endregion
        }
        return result;
    }
    #endregion

    #region   CAP02001  票据解读函数
    public static int SendUATicket(string UAProvinceID,string SPID,string ticket, string CustID, string URL, HttpContext SpecificContext, string SPDataCacheName, string ExtendField,string TransactionID, out string ErrMsg, out string newExtendField)
    {
        int result = -20005;
        //byte[] privateKeyFile;
        //byte[] publicKeyFile;
        //string privateKeyPassword = "";
        //string UserName = "";
        string ticketXML = "";
        newExtendField = "";
        ErrMsg = "";

        MBOSSClass.BilByCompilingResult bcr = new MBOSSClass.BilByCompilingResult();

        //SPInfoManager spInfo = new SPInfoManager();
        MBOSSClass mbss = new MBOSSClass();

        try
        {
            //Object SPData = spInfo.GetSPData(SpecificContext, SPDataCacheName);
            //publicKeyFile = spInfo.GetCAInfo("35111111", 0, SPData, out UserName, out privateKeyPassword);
            //privateKeyFile = spInfo.GetCAInfo("35111111", 1, SPData, out UserName, out privateKeyPassword);
            result = mbss.SendUATicket(UAProvinceID,SPID, ticket, URL, SpecificContext, SPDataCacheName, TransactionID, out bcr, out ticketXML, out ErrMsg);
        }
        catch (Exception err)
        {
            result = -20006;
            ErrMsg = err.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append(" CAP02001票据解读接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";ticket - " + ticket);
            msg.Append(";SpecificContext - " + SpecificContext);
            msg.Append(";SPDataCacheName - " + SPDataCacheName + "\r\n");
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + result);
            msg.Append("UATicketXML - " + ticketXML);
            msg.Append("; 错误描述 - " + ErrMsg);
            msg.Append("; ExtendField - " + newExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("SendUATicket", msg);
            #endregion
        }
        return result;
    }
    #endregion

    #region 网间移动号码携带查询接口

    public class NPDataResult
    {
        public int Result;
        public string ExtendField;
    }

    [WebMethod(Description = "网间移动号码携带查询接口")]
    public NPDataResult NPData(string SPID, string NPNumber, string ExtendField)
    {
        NPDataResult Result = new NPDataResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ExtendField = "";
        string ErrorDescription = "";
        System.DateTime startTime = System.DateTime.Now;

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "NPData", this.Context, out ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(NPNumber))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_NPData_Code;
                ErrorDescription = "手机号码不能为空";
                return Result;
            }
            else
            {
                string ptn = @"^1\d{10}$";
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(ptn);

                if (reg.IsMatch(NPNumber))
                {

                }
                else
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                    ErrorDescription = "非合法的手机号码";
                    return Result;
                }
            }

            #endregion
       
            string PortInNetID="";
            string PortOutNetID=""; 
            string HomeNetID=""; 
            string BeginDate="";


            Result.Result = NpDataManager.NpDataManagerQuery(SPID, NPNumber, out  PortInNetID, out  PortOutNetID, out  HomeNetID, out  BeginDate, out  ErrorDescription);
            Result.ExtendField = BTBizRules.GenerateNpDataXml(NPNumber, PortInNetID, PortOutNetID, HomeNetID, BeginDate, ErrorDescription);   
   
 }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {

            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("网间移动号码携带查询接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
            msg.Append(";SPID - " + SPID);
            msg.Append(";NPNumber - " + NPNumber);
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("NPData", msg);
            #endregion

        }
        return Result;
    }

    #endregion

    #region  常支付账号

    [WebMethod(Description = "常支付账号数据上传")]
    public string PaymentAccountUpload(string RequestXml)
    {
        string ResultXML = "";

        //生成对象
        BankAccount baObj = new BankAccount();
        try
        {
            //解析传入的XML参数
            baObj.ParseXML(RequestXml);
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateResultXML();
                return ResultXML;
            }

 
            //数据校验
            baObj.DataCheck();
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateResultXML();
                return ResultXML;
            }

            //账号入临时库
            baObj.InsertPaymentAccountTMP();
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateResultXML();
                return ResultXML;
            }

            //处理成功则生成结果XML
            ResultXML = baObj.GenerateResultXML();
            return ResultXML;
        }
        catch (System.Exception ex)
        {
            baObj.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            baObj.ErrMsg = ex.Message;
            //生成结果XML
            ResultXML = baObj.GenerateResultXML();
            return ResultXML;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("常支付账号数据上传接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("请求参数 - " + RequestXml);
            msg.Append("\r\n");
            msg.Append("处理结果 - " + ResultXML);
            msg.Append("\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("PaymentAccountUpload", msg);

            #endregion

        }
        return ResultXML;
    }

    [WebMethod(Description = "常支付账号状态通知接口")]
    public string PaymentStatusNotify(string RequestXml)
    {
        string ResultXML = "";
        //生成对象
        BankAccount baObj = new BankAccount();
        try
        {
            //解析传入的XML参数
            baObj.ParseXMLStatusNotify(RequestXml);
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateStatusNotifyResultXML();
                return ResultXML;
            }

            //数据校验
            baObj.DataCheckStatusNotify();
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateStatusNotifyResultXML();
                return ResultXML;
            }

            //账号入临时库
            baObj.InsertStatusNotify();
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateStatusNotifyResultXML();
                return ResultXML;
            }

            //处理成功则生成结果XML
            ResultXML = baObj.GenerateStatusNotifyResultXML();
            return ResultXML;
        }
        catch (System.Exception ex)
        {
            baObj.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            baObj.ErrMsg = ex.Message;
            //生成结果XML
            ResultXML = baObj.GenerateStatusNotifyResultXML();
            return ResultXML;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("常支付账号状态通知接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("请求参数 - " + RequestXml);
            msg.Append("\r\n");
            msg.Append("处理结果 - " + ResultXML);
            msg.Append("\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("PaymentStatusNotify", msg);

            #endregion

        }
        return ResultXML;
    }

    [WebMethod(Description = "常支付账号查询接口")]
    public string PaymentAccountQuery(string RequestXml)
    {
        string ResultXML = "";

        //生成对象
        BankAccount baObj = new BankAccount();
        try
        {
            //解析传入的XML参数
            baObj.ParseXMLAccountQuery(RequestXml);
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateAccountQueryResultXML();
                return ResultXML;
            }

            //数据校验
            baObj.DataCheckAccountQuery();
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateAccountQueryResultXML();
                return ResultXML;
            }

            //账号入临时库
            baObj.InsertAccountQuery();
            if (baObj.Result != 0)
            {
                //生成结果XML
                ResultXML = baObj.GenerateAccountQueryResultXML();
                return ResultXML;
            }

            //处理成功则生成结果XML
            ResultXML = baObj.GenerateAccountQueryResultXML();
            return ResultXML;
        }
        catch (System.Exception ex)
        {
            baObj.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            baObj.ErrMsg = ex.Message;
            //生成结果XML
            ResultXML = baObj.GenerateAccountQueryResultXML();
            return ResultXML;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("常支付账号查询接口" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("请求参数 - " + RequestXml);
            msg.Append("\r\n");
            msg.Append("处理结果 - " + ResultXML);
            msg.Append("\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("PaymentAccountQuery", msg);

            #endregion

        }
        return ResultXML;
    }

    #endregion

}
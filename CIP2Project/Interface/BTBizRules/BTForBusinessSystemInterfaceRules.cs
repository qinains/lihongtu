using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using System.Web;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Linkage.BestTone.Interface.Rule
{
    public class BTForBusinessSystemInterfaceRules
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BTForBusinessSystemInterfaceRules));

        /// <summary>
        /// 密码认证v4
        /// 作者：lihongtu   时间：2013-08-28
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="AuthenName"></param>
        /// <param name="AuthenType"></param>
        /// <param name="PassWord"></param>
        /// <param name="Context"></param>
        /// <param name="ProvinceID"></param>
        /// <param name="IsQuery"></param>
        /// <param name="PwdType"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="CustType"></param>
        /// <param name="outerid"></param>
        /// <param name="ProvinceID1"></param>
        /// <param name="RealName"></param>
        /// <param name="UserName"></param>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public static int UamAuthen(string SPID, string AuthenName, string AuthenType, string PassWord, HttpContext Context, string ProvinceID, string IsQuery, string PwdType, out string ErrMsg,  out string ResType, out string RspCode, out string RspDesc, out string AuthenticationKey)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            ResType = "";
            RspCode = "";
            RspDesc = "";
            AuthenticationKey = "";
            string TestStr = "";
            //不需要密码，取信息
            if (IsQuery == "1")
            {
                logger.Info("不需要密码，取信息");
                Result = CrmSSO.UserAuthUam(ProvinceID, AuthenType, AuthenName, "", PassWord, "0", "", Context, out ResType, out RspCode, out RspDesc, out AuthenticationKey, out ErrMsg, out TestStr);
            }
            else
            {
                logger.Info("需要密码，取信息");
                Result = CrmSSO.UserAuthUam(ProvinceID, AuthenType, AuthenName, "", PassWord, "1", "", Context, out ResType, out RspCode, out RspDesc, out AuthenticationKey, out ErrMsg, out TestStr);
            }
            return Result;
        }



        /// <summary>
        /// 密码认证v4  out ResType, out RspCode, out RspDesc, out AuthenticationKey
        /// 作者：lihongtu   时间：2013-9-1
        /// </summary>
        public static int UserAuthV4(string SPID, string AuthenName, string AuthenType, string Password, HttpContext Context, string ProvinceID, string IsQuery, string PwdType, out string ErrMsg, out string CustID, out string UserAccount, out string CustType, out string outerid, out string ProvinceID1, out  string RealName, out string UserName, out string NickName, out string CustAddress, out string ResType, out string RspCode, out string RspDesc, out string AuthenticationKey, out string TestStr, out QryCustInfoV2Return qryCustInfoReturn)
        {

            StringBuilder msg_inbound = new StringBuilder();
            msg_inbound.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            msg_inbound.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg_inbound.Append("从login2中来\r\n");
            msg_inbound.Append("ProvinceID=" + ProvinceID + "\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("SSO", msg_inbound);
            qryCustInfoReturn = null; 
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";
            CustType = "";
            TestStr = "";
            string UProvinceID = "";
            string SysID = "";
            string AreaID = "";
            int CrmResult = ErrorDefinition.IError_Result_UnknowError_Code;

            outerid = "";
            CustAddress = "";
            string ProvinceID2 = "";
            string Areaid = "";
            int result1 = -1;
            //手机
            if (AuthenType == "7")
            {
                result1 = GetPhoneTOArea(AuthenName, out ProvinceID2, out Areaid, out ErrMsg);
            }

            ProvinceID1 = ProvinceID;

            RealName = "";
            UserName = "";
            NickName = "";

            string AuthenTypes = "3";
            string ProvinceID3 = System.Configuration.ConfigurationManager.AppSettings["ProvinceID3"];
            logger.Info("UserAuthV4 ProvinceID3=" + ProvinceID3);
            string ProvinceID3_1 = System.Configuration.ConfigurationManager.AppSettings["ProvinceID3_1"];
            logger.Info("UserAuthV4 ProvinceID3_1=" + ProvinceID3_1);
            string POSSPID = System.Configuration.ConfigurationManager.AppSettings["POSSPID"];
            logger.Info("UserAuthV4 POSSPID=" + POSSPID);
            msg_inbound.Append("ProvinceID3=" + ProvinceID3 + "\r\n");
            msg_inbound.Append("ProvinceID3_1=" + ProvinceID3_1 + "\r\n");

            if (ProvinceID == "")
            {
                if (result1 == 0)
                {
                    if (ProvinceID3.IndexOf(ProvinceID2) >= 0 && AuthenTypes != AuthenType)
                    {
                        ProvinceID1 = ProvinceID2;
                        ProvinceID = ProvinceID2;
                    }
                    else if (ProvinceID3_1.IndexOf(ProvinceID) >= 0 && AuthenTypes != AuthenType)
                    {
                        ProvinceID1 = ProvinceID2;
                        ProvinceID = ProvinceID2;
                    }
                }
            }
            logger.Info("UserAuthV4 ProvinceID=" + ProvinceID);
            logger.Info("UserAuthV4 ProvinceID1=" + ProvinceID1);
            logger.Info("UserAuthV4 ProvinceID2=" + ProvinceID2);

            ResType = "";
            RspCode = "";
            RspDesc = "";
            AuthenticationKey = "";

            //POS登录

            try
            {
                if (ProvinceID != "" && POSSPID.IndexOf(SPID) > 0 && ProvinceID3_1.IndexOf(ProvinceID) >= 0 && AuthenTypes != AuthenType && AuthenType != "18")
                {
                    logger.Info("UserAuthV4 POS登录");
                    
                    string dealType = "0";
                    msg_inbound.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    msg_inbound.Append("准备进入pos登陆crm认证\r\n");
                    Result = CrmSSO.UserAuthCrm(ProvinceID, AuthenType, AuthenName, "", Password, "0", "", Context, out RealName, out UserName,
                         out NickName, out outerid, out CustType, out CustID, out ErrMsg, out TestStr);
                    msg_inbound.Append("从pos登陆crm认证结束:result" + Result + "\r\n");
                    BTUCenterInterfaceLog.CenterForBizTourLog("REVERSE-SSO", msg_inbound);
                    if (Result == 0)
                    {
                        msg_inbound.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        msg_inbound.Append("从pos登陆认证结束后准备通知积分息系统\r\n");
                        BTUCenterInterfaceLog.CenterForBizTourLog("REVERSE-SSO", msg_inbound);
                        //通知积分系统
                        // CommonBizRules.CustInfoNotify(CustID, CustID, "", dealType, "", "", CustType, ProvinceID, SPID);
                        CIP2BizRules.InsertCustInfoNotify(CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], Password, "0", out ErrMsg);

                    }
                    return Result;
                }
                else if (ProvinceID != "" && ProvinceID3_1.IndexOf(ProvinceID) >= 0 && AuthenTypes != AuthenType)
                {
                    logger.Info("UserAuthV4 非pos登陆进入UserAuthCrm:ProvinceID=" + ProvinceID + ";Authentypes=" + AuthenTypes);
                    StringBuilder msg1 = new StringBuilder();
                    msg1.Append("UserAuthV4 非pos登陆进入UserAuthCrm:ProvinceID=" + ProvinceID + "\r\n");
                    msg1.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg1);
                    
                    string dealType = "0";
                    //不需要密码，取信息
                    if (IsQuery == "1")
                    {
                        logger.Info("UserAuthV4 不需要密码，取信息");
                        Result = CrmSSO.UserAuthCrmV2(ProvinceID, AuthenType, AuthenName, "", Password, "0", "", Context, out RealName, out UserName,
                        out NickName, out outerid, out CustType, out CustID, out CustAddress, out ErrMsg, out TestStr, out qryCustInfoReturn);
                    }
                    //需要密码进行验证
                    else if (ProvinceID != "" && ProvinceID3.IndexOf(ProvinceID) >= 0 && AuthenTypes != AuthenType)
                    {
                        logger.Info("UserAuthV4 需要密码进行验证");
                        StringBuilder msg2 = new StringBuilder();
                        msg2.Append("开始向crm发请求");
                        msg2.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg2);

                        //Result = CrmSSO.UserAuthCrm(ProvinceID, AuthenType, AuthenName, "", Password, "1", "", Context, out RealName, out UserName,
                        //    out NickName, out outerid, out CustType, out CustID, out ErrMsg, out TestStr);
                        
                        Result = UamAuthen(SPID, AuthenName, AuthenType, Password, Context, ProvinceID, IsQuery, "1", out ErrMsg, out ResType, out RspCode, out RspDesc, out AuthenticationKey);
                        if (Result != 0)
                        {
                            return Result;
                        }
                        else
                        {
                            Result = CrmSSO.UserAuthCrmV2(ProvinceID, AuthenType, AuthenName, "", Password, "0", "", Context, out RealName, out UserName,
                            out NickName, out outerid, out CustType, out CustID, out CustAddress, out ErrMsg, out TestStr, out qryCustInfoReturn);

                        }

                        msg2.Append("请求结束result" + Result);
                        msg2.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg2);
                    }
                    else if (AuthenType == "18")
                    {
                        Result = CrmSSO.UserAuthCrmV2(ProvinceID, AuthenType, AuthenName, "", Password, "1", "", Context, out RealName, out UserName,
                            out NickName, out outerid, out CustType, out CustID, out CustAddress,out ErrMsg, out TestStr, out qryCustInfoReturn);
                     
                    }

                    if (Result == 0)
                    {
                        StringBuilder msg3 = new StringBuilder();
                        msg3.Append("准备通知积分商城");
                        msg3.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg3);
                        //通知积分系统
                        // CommonBizRules.CustInfoNotify(CustID, CustID, "", dealType, "", "", CustType, ProvinceID, SPID);
                        CIP2BizRules.InsertCustInfoNotify(CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], Password, "0", out ErrMsg);
                        msg3.Append("通知积分商城完成");
                        msg3.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg3);
                    }
                    return Result;
                }
            }
            catch (Exception e)
            {
                logger.Info(e.StackTrace);
            }

            ///////////


            int lenRep = AuthenName.IndexOf("-");
            if (lenRep < 7)
                AuthenName = AuthenName.Replace("-", "");


            try
            {
                #region 本地数据查询认证

                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 15;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_UserAuthV2";


                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

                SqlParameter parPwd = new SqlParameter("@Pwd", SqlDbType.VarChar, 100);
                parPwd.Value = CryptographyUtil.Encrypt(Password);
                cmd.Parameters.Add(parPwd);


                SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 100);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustType = new SqlParameter("@CustType ", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID ", SqlDbType.VarChar, 2);
                parUProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parSysID = new SqlParameter("@SysID ", SqlDbType.VarChar, 8);
                parSysID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSysID);

                SqlParameter parAreaID = new SqlParameter("@AreaID ", SqlDbType.VarChar, 3);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parOuterID = new SqlParameter("@outerid ", SqlDbType.VarChar, 30);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);


                SqlParameter parUserName = new SqlParameter("@UserName ", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);


                SqlParameter parRealName = new SqlParameter("@RealName ", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);


                SqlParameter parNickName = new SqlParameter("@NickName ", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);




                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();
                CustType = parCustType.Value.ToString();
                UProvinceID = parUProvinceID.Value.ToString();
                SysID = parSysID.Value.ToString();
                AreaID = parAreaID.Value.ToString();

                outerid = parOuterID.Value.ToString();
                ProvinceID1 = UProvinceID;

                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();

                #endregion

                //如果不是e家卡和c网用户则直接返回结果
                if (CustType == "41" || CustType == "42" || CustType == "50" || CustType == "70")
                {
                    return Result;
                }

                if (Result != 0)
                    return Result;

                #region 非商旅卡进行远程认证

                string ExtendField = "";


                SysInfoManager sm = new SysInfoManager();
                Object sysData = sm.GetSysData(Context);

                string url = sm.GetPropertyBySysID(SysID, SysData.Field_InterfaceUrl, sysData);

                string str = "";

                try
                {
                    if (SysID == "15999999")
                    {
                        BTUCenter.Proxy.JX.CRMUserAuthResult resultObj = new BTUCenter.Proxy.JX.CRMUserAuthResult();
                        BTUCenter.Proxy.JX.JXCRMForBTUCenter obj = new BTUCenter.Proxy.JX.JXCRMForBTUCenter();
                        obj.Url = url;

                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                        if (resultObj.ErrorDescription == null)
                            ErrMsg = "";
                        else
                            ErrMsg = "" + resultObj.ErrorDescription;
                        CrmResult = resultObj.Result;


                    }
                    else if (SysID == "13999999")
                    {
                        BTUCenter.Proxy.AH.CRMUserAuthResult resultObj = new BTUCenter.Proxy.AH.CRMUserAuthResult();

                        BTUCenter.Proxy.AH.CRMForBTUCenter obj = new BTUCenter.Proxy.AH.CRMForBTUCenter();
                        obj.Url = url;

                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                        ErrMsg = resultObj.ErrorDescription;
                        CrmResult = resultObj.Result;

                    }
                    else if (SysID == "14999999")
                    {
                        BTUCenter.Proxy.FJ.IntegralApplyWSFactory obj = new BTUCenter.Proxy.FJ.IntegralApplyWSFactory();
                        BTUCenter.Proxy.FJ.CRMUserAuthResponse resultObj = new BTUCenter.Proxy.FJ.CRMUserAuthResponse();

                        obj.Url = url;
                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, AreaID, Password, ExtendField);
                        ErrMsg = resultObj.errorDescription;
                        CrmResult = resultObj.result;
                    }
                    else if (SysID == "22999999")
                    {
                        BTUCenter.Proxy.HN.EaiWebService obj = new BTUCenter.Proxy.HN.EaiWebService();
                        BTUCenter.Proxy.HN.CRMUserAuthResult resultObj = new BTUCenter.Proxy.HN.CRMUserAuthResult();
                        obj.Url = url;
                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);
                        // resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, AreaID, Password, ExtendField);
                        ErrMsg = resultObj.errorDescription;
                        CrmResult = resultObj.result;

                    }
                    else
                    {
                        BTUCenter.Proxy.CRMUserAuthResult resultObj = new BTUCenter.Proxy.CRMUserAuthResult();

                        BTUCenter.Proxy.CRMForBTUCenter obj = new BTUCenter.Proxy.CRMForBTUCenter();

                        obj.Url = url;

                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                        ErrMsg = resultObj.ErrorDescription;
                        CrmResult = resultObj.Result;
                    }
                }
                catch (Exception e)
                {
                    logger.Info(e.ToString());
                    CrmResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                    ErrMsg = str + "+++" + url + "==" + e.Message;

                }
                finally
                {
                    #region WriteLog
                    StringBuilder msg = new StringBuilder();
                    msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                    msg.Append("CRM密码认证接口 " + DateTime.Now.ToString("u") + "\r\n");
                    msg.Append(";SysID - " + SysID);
                    msg.Append(";AuthenName - " + AuthenName);
                    msg.Append(";AuthenType - " + AuthenType);
                    msg.Append(";Password - " + Password);
                    msg.Append(";CustType - " + CustType);
                    msg.Append(";AreaID - " + AreaID);
                    msg.Append(";ExtendField - " + ExtendField);
                    msg.Append("\r\n");

                    msg.Append("处理结果 - " + Result);
                    msg.Append("CrmResult - " + CrmResult);
                    msg.Append("; 错误描述 - " + ErrMsg + "\r\n");
                    msg.Append("; ExtendField - " + ExtendField + "\r\n");
                    msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                    BTUCenterInterfaceLog.CenterForBizTourLog("CRMUserAuth", msg);
                    #endregion
                }


                #endregion

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            if (CrmResult == -20505 & Result == 0)
            {
                Result = -20505;
            }
            else if (CrmResult != 0 & Result == 0) //当rusult 值为1时代表身份定位成功， 密码认证失败，
            {
                if (POSSPID.IndexOf(SPID) >= 0)
                {
                    Result = 1;
                }
                else
                {
                    Result = CrmResult;
                }
            }

            if (CrmResult == 0)

                if (CrmResult == 0 & Result == 0)
                    Result = 0;

            return Result;

        }



        /// <summary>
        /// 密码认证v2
        /// 作者：tongbo   时间：2009-8-14
        /// </summary>
        public static int UserAuthV2(string SPID, string AuthenName, string AuthenType, string Password, HttpContext Context, string ProvinceID,string IsQuery,string PwdType, out string ErrMsg, out string CustID, out string UserAccount, out string CustType, out string outerid, out string ProvinceID1,out  string RealName, out string  UserName, out string NickName)
        {

            StringBuilder msg_inbound = new StringBuilder();
            msg_inbound.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            msg_inbound.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg_inbound.Append("从login2中来\r\n");
            msg_inbound.Append("ProvinceID="+ProvinceID+"\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("SSO", msg_inbound);

            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";
            CustType = "";
            string UProvinceID = "";
            string SysID = "";
            string AreaID = "";
            int CrmResult = ErrorDefinition.IError_Result_UnknowError_Code;
         
            outerid = "";
            string ProvinceID2 = "";
            string Areaid = "";
            int result1 = -1;
            //手机
            if (AuthenType == "7")  
            {
                result1 = GetPhoneTOArea(AuthenName, out ProvinceID2, out Areaid, out ErrMsg); 
            }

            ProvinceID1 = ProvinceID;

            RealName = "";
            UserName = "";
            NickName = "";

            string AuthenTypes = "3";
            string ProvinceID3 = System.Configuration.ConfigurationManager.AppSettings["ProvinceID3"];
            logger.Info("ProvinceID3=" + ProvinceID3);
            string ProvinceID3_1 = System.Configuration.ConfigurationManager.AppSettings["ProvinceID3_1"];
            logger.Info("ProvinceID3_1=" + ProvinceID3_1);
            string POSSPID = System.Configuration.ConfigurationManager.AppSettings["POSSPID"];
            logger.Info("POSSPID=" + POSSPID);
            msg_inbound.Append("ProvinceID3=" + ProvinceID3+"\r\n");
            msg_inbound.Append("ProvinceID3_1=" + ProvinceID3_1+"\r\n");

            if (ProvinceID == "")
            {
                if (result1 == 0)
                {
                    if (ProvinceID3.IndexOf(ProvinceID2) >= 0 && AuthenTypes != AuthenType)
                    {
                        ProvinceID1 = ProvinceID2;
                        ProvinceID = ProvinceID2;                      
                    }
                    else if (ProvinceID3_1.IndexOf(ProvinceID) >= 0 && AuthenTypes != AuthenType)
                    {
                        ProvinceID1 = ProvinceID2;
                        ProvinceID = ProvinceID2;             
                    }
                }
            }
            
            //POS登录
            if (ProvinceID!="" && POSSPID.IndexOf(SPID) > 0 && ProvinceID3_1.IndexOf(ProvinceID) >= 0 && AuthenTypes != AuthenType && AuthenType!="18")
            {
                logger.Info("POS登录");
                string TestStr = "";
                string dealType = "0";
                msg_inbound.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                msg_inbound.Append("准备进入pos登陆crm认证\r\n");
                Result = CrmSSO.UserAuthCrm(ProvinceID, AuthenType, AuthenName, "", Password, "0", "", Context, out RealName, out UserName,
                     out NickName, out outerid, out CustType, out CustID, out ErrMsg, out TestStr);
                msg_inbound.Append("从pos登陆crm认证结束:result"+Result+"\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("REVERSE-SSO", msg_inbound);
                if (Result == 0)
                {
                    msg_inbound.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    msg_inbound.Append("从pos登陆认证结束后准备通知积分息系统\r\n");
                    BTUCenterInterfaceLog.CenterForBizTourLog("REVERSE-SSO", msg_inbound);
                    //通知积分系统
                    // CommonBizRules.CustInfoNotify(CustID, CustID, "", dealType, "", "", CustType, ProvinceID, SPID);
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], Password, "0", out ErrMsg);

                }
                return Result;
            }
            else if (ProvinceID!="" && ProvinceID3_1.IndexOf(ProvinceID) >= 0 && AuthenTypes!=AuthenType )
            {
                StringBuilder msg1 = new StringBuilder();
                msg1.Append("非pos登陆进入UserAuthCrm:ProvinceID="+ProvinceID+"\r\n");
                msg1.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg1);
                string TestStr = "";
                string dealType = "0";
                //不需要密码，取信息
                if (IsQuery == "1" )
                {
                    Result = CrmSSO.UserAuthCrm(ProvinceID, AuthenType, AuthenName, "", Password, "0", "", Context, out RealName, out UserName,
                    out NickName, out outerid, out CustType, out CustID, out ErrMsg, out TestStr);
                }
                //需要密码进行验证
                else if (ProvinceID != "" && ProvinceID3.IndexOf(ProvinceID) >= 0 && AuthenTypes != AuthenType )
                {
                    StringBuilder msg2 = new StringBuilder();
                    msg2.Append("开始向crm发请求");
                    msg2.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg2);

                    Result = CrmSSO.UserAuthCrm(ProvinceID, AuthenType, AuthenName, "", Password, "1", "", Context, out RealName, out UserName,
                        out NickName, out outerid, out CustType, out CustID, out ErrMsg, out TestStr);
                     
                    msg2.Append("请求结束result" + Result);
                    msg2.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg2);
                }
                else if (AuthenType == "18") {
                    Result = CrmSSO.UserAuthCrm(ProvinceID, AuthenType, AuthenName, "", Password, "1", "", Context, out RealName, out UserName,
                        out NickName, out outerid, out CustType, out CustID, out ErrMsg, out TestStr);
                }

                if (Result == 0)
                {
                    StringBuilder msg3 = new StringBuilder();
                    msg3.Append("准备通知积分商城");
                    msg3.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth", msg3);
                    //通知积分系统
                   // CommonBizRules.CustInfoNotify(CustID, CustID, "", dealType, "", "", CustType, ProvinceID, SPID);
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], Password, "0", out ErrMsg);
                    msg3.Append("通知积分商城完成");
                    msg3.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    BTUCenterInterfaceLog.CenterForBizTourLog("CRMversessoUserAuth",msg3);
                }
                return Result;
            }
             

            int lenRep = AuthenName.IndexOf("-");
            if(lenRep <7)
                AuthenName = AuthenName.Replace("-", "");
          

            try
            {
                #region 本地数据查询认证

                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 15;
                cmd.CommandType = CommandType.StoredProcedure;              
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_UserAuthV2";
                   

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

                SqlParameter parPwd = new SqlParameter("@Pwd", SqlDbType.VarChar, 100);
                parPwd.Value = CryptographyUtil.Encrypt(Password);
                cmd.Parameters.Add(parPwd);


                SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 100);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustType = new SqlParameter("@CustType ", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID ", SqlDbType.VarChar, 2);
                parUProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parSysID = new SqlParameter("@SysID ", SqlDbType.VarChar, 8);
                parSysID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSysID);

                SqlParameter parAreaID = new SqlParameter("@AreaID ", SqlDbType.VarChar, 3);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parOuterID = new SqlParameter("@outerid ", SqlDbType.VarChar, 30);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);


                SqlParameter parUserName = new SqlParameter("@UserName ", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);


                SqlParameter parRealName = new SqlParameter("@RealName ", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);


                SqlParameter parNickName = new SqlParameter("@NickName ", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);




                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();
                CustType = parCustType.Value.ToString();
                UProvinceID = parUProvinceID.Value.ToString();
                SysID = parSysID.Value.ToString();
                AreaID = parAreaID.Value.ToString();

                outerid = parOuterID.Value.ToString();
                ProvinceID1 = UProvinceID;

                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();

                #endregion

                //如果不是e家卡和c网用户则直接返回结果
                if (CustType == "41" || CustType == "42" || CustType == "50" || CustType=="70")
                {
                    return Result;
                }

                if (Result != 0)
                    return Result;
                  
                #region 非商旅卡进行远程认证

                string ExtendField = "";

                /////河南信息卖场
                //string HNInfo = System.Configuration.ConfigurationManager.AppSettings["HNInfo"];
                //string HNInfoSysID = System.Configuration.ConfigurationManager.AppSettings["HNInfoSysID"];
                //int custTypeInt = HNInfo.IndexOf(CustType);
                //if (custTypeInt >= 0)
                //{
                //    SysID = HNInfoSysID.Split(';')[custTypeInt];
                //}

                SysInfoManager sm = new SysInfoManager();
                Object sysData = sm.GetSysData(Context);
               
                string url = sm.GetPropertyBySysID(SysID, SysData.Field_InterfaceUrl, sysData);

                string str = "";

                try
                {
                    if (SysID == "15999999")
                    {
                        BTUCenter.Proxy.JX.CRMUserAuthResult resultObj = new BTUCenter.Proxy.JX.CRMUserAuthResult();
                        BTUCenter.Proxy.JX.JXCRMForBTUCenter obj = new BTUCenter.Proxy.JX.JXCRMForBTUCenter();
                        obj.Url = url;
                     
                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);
                      
                        if (resultObj.ErrorDescription == null)
                            ErrMsg = "";
                        else
                            ErrMsg = "" + resultObj.ErrorDescription;
                        CrmResult = resultObj.Result;

                       
                    }
                    else if (SysID == "13999999")
                    {
                        BTUCenter.Proxy.AH.CRMUserAuthResult resultObj = new BTUCenter.Proxy.AH.CRMUserAuthResult();

                        BTUCenter.Proxy.AH.CRMForBTUCenter obj = new BTUCenter.Proxy.AH.CRMForBTUCenter();
                        obj.Url = url;

                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                        ErrMsg = resultObj.ErrorDescription;
                        CrmResult = resultObj.Result;

                    }
                    else if(SysID == "14999999")
                    {
                        BTUCenter.Proxy.FJ.IntegralApplyWSFactory obj=new BTUCenter.Proxy.FJ.IntegralApplyWSFactory();
                        BTUCenter.Proxy.FJ.CRMUserAuthResponse resultObj = new BTUCenter.Proxy.FJ.CRMUserAuthResponse();

                        obj.Url = url;
                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, AreaID, Password, ExtendField);
                        ErrMsg = resultObj.errorDescription ;
                        CrmResult = resultObj.result;
                    }
                    else if (SysID == "22999999")
                    {
                        BTUCenter.Proxy.HN.EaiWebService obj = new BTUCenter.Proxy.HN.EaiWebService();
                        BTUCenter.Proxy.HN.CRMUserAuthResult resultObj = new BTUCenter.Proxy.HN.CRMUserAuthResult();
                        obj.Url = url;
                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);
                       // resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, AreaID, Password, ExtendField);
                        ErrMsg = resultObj.errorDescription;
                        CrmResult = resultObj.result;

                    }                   
                    else
                    {
                        BTUCenter.Proxy.CRMUserAuthResult resultObj = new BTUCenter.Proxy.CRMUserAuthResult();

                        BTUCenter.Proxy.CRMForBTUCenter obj = new BTUCenter.Proxy.CRMForBTUCenter();
                       
                        obj.Url = url;

                        resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                        ErrMsg = resultObj.ErrorDescription;
                        CrmResult = resultObj.Result;
                    }  
                }
                catch (Exception e)
                {
                    CrmResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                    ErrMsg =str+"+++"+url+"=="+ e.Message;
                   
                }
                finally
                {
                    #region WriteLog
                    StringBuilder msg = new StringBuilder();
                    msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                    msg.Append("CRM密码认证接口 " + DateTime.Now.ToString("u") + "\r\n");
                    msg.Append(";SysID - " + SysID);
                    msg.Append(";AuthenName - " + AuthenName);
                    msg.Append(";AuthenType - " + AuthenType);
                    msg.Append(";Password - "   + Password);
                    msg.Append(";CustType - " + CustType);
                    msg.Append(";AreaID - "     + AreaID);
                    msg.Append(";ExtendField - " + ExtendField);
                    msg.Append("\r\n");

                    msg.Append("处理结果 - "    + Result);
                    msg.Append("CrmResult - "   + CrmResult);
                    msg.Append("; 错误描述 - "  + ErrMsg + "\r\n");
                    msg.Append("; ExtendField - " + ExtendField + "\r\n");
                    msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                    BTUCenterInterfaceLog.CenterForBizTourLog("CRMUserAuth", msg);
                    #endregion
                }


                #endregion

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }
           
            if (CrmResult == -20505 & Result == 0 )
            {
                Result = -20505;
            }            
            else if (CrmResult != 0 & Result ==0) //当rusult 值为1时代表身份定位成功， 密码认证失败，
            {
                if (POSSPID.IndexOf(SPID) >= 0)
                {
                    Result = 1;
                }
                else
                {
                    Result = CrmResult;
                }
            }

            if (CrmResult == 0)

            if (CrmResult == 0 & Result == 0) 
                Result = 0;

            return Result;

        }

        public static int UserAuthV3(String SPID, string AuthenName, String Password, String PwdType, out String CustID, out String CustType, out String AuthenType, out String UserName, out String UserAccount
            ,out String RealName, out String NickName, out String SysID, out String OuterID, out String ProvinceID, out String AreaID, out String ErrMsg)
        {
            Int32 result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
            
            CustID = String.Empty;
            CustType = String.Empty;
            AuthenType = String.Empty;
            UserName = String.Empty;
            UserAccount = String.Empty;
            RealName = String.Empty;
            NickName = String.Empty;
            SysID = String.Empty;
            OuterID = String.Empty;
            ProvinceID = String.Empty;
            AreaID = String.Empty;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "up_Customer_OV3_Interface_UserAuthV3";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                    parSPID.Value = SPID;
                    cmd.Parameters.Add(parSPID);

                    SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
                    parAuthenName.Value = AuthenName;
                    cmd.Parameters.Add(parAuthenName);

                    SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 100);
                    parPassword.Value = CryptographyUtil.Encrypt(Password);
                    cmd.Parameters.Add(parPassword);

                    SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 2);
                    parPwdType.Value = PwdType;
                    cmd.Parameters.Add(parPwdType);
                    //输出参数
                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                    parCustID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                    parCustType.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parCustType);

                    SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                    parAuthenType.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parAuthenType);

                    SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 20);
                    parUserName.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parUserName);

                    SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                    parUserAccount.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parUserAccount);

                    SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 20);
                    parRealName.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parRealName);

                    SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 20);
                    parNickName.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parNickName);

                    SqlParameter parSysID = new SqlParameter("@SysID", SqlDbType.VarChar, 2);
                    parSysID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parSysID);
                    
                    SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar, 30);
                    parOuterID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parOuterID);

                    SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 3);
                    parProvinceID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parProvinceID);

                    SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 3);
                    parAreaID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parAreaID);
                    

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                    ErrMsg = parErrMsg.Value.ToString();
                    CustID = parCustID.Value.ToString();
                    CustType = parCustType.Value.ToString();
                    AuthenType = parAuthenType.Value.ToString();
                    UserName = parUserName.Value.ToString();
                    UserAccount = parUserAccount.Value.ToString();
                    RealName = parRealName.Value.ToString();
                    NickName = parNickName.Value.ToString();
                    SysID = parSysID.Value.ToString();
                    OuterID = parOuterID.Value.ToString();
                    ProvinceID = parProvinceID.Value.ToString();
                    AreaID = parAreaID.Value.ToString();

                    result = Convert.ToInt32(parResult.Value);
                }

            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return result;
        }

        /// <summary> 
        /// 密码认证v2
        /// 作者：tongbo   时间：2009-8-14
        /// </summary>
        public static int MUserAuthV2(string SPID,string UAProvinceID, string AuthenName, string AuthenType, MBOSSClass.AuthenRecord[] AuthenRecords, HttpContext Context, out string ErrMsg, out string CustID, out string UserAccount, out string CustType, out string outerid, out string ProvinceID,
            out string RealName, out string UserName, out string NickName, out string dealType, out int type, out string areaid)
        {
            areaid = "";
            type = 0;
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";
            CustType = "";
            UserName = "";
            NickName = "";
            RealName = "";
            string UProvinceID = "";
            string SysID = "";
           
            int CrmResult = ErrorDefinition.IError_Result_UnknowError_Code;

            outerid = "";
            ProvinceID = "";
            dealType = "0";

            try
            {
                //5, E家卡
                //6，c网手机用户卡
                //7，c网手机用户手机号
                //8，省百事通卡
                //须远程认证
                //string AuthenTypeSpan = "5,6,7,8";
                //if (AuthenTypeSpan.IndexOf(AuthenType) < 0) //本地认证
                //{
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                string MSProvinceID=System.Configuration.ConfigurationManager.AppSettings["MSProvinceID"];
                if (UAProvinceID!="" && MSProvinceID.IndexOf(UAProvinceID) >= 0)
                {
                    cmd.CommandText = "dbo.up_Customer_OV3_Interface_MUserAuthV1";
                }
                else
                {                  
                    cmd.CommandText = "dbo.up_Customer_OV3_Interface_MUserAuthV2";
                    type = 1;
                }

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

                SqlParameter parAuthenRecords = new SqlParameter("@AuthenRecords", SqlDbType.Text);
                MBOSSClass.AuthenRecord[] au = new MBOSSClass.AuthenRecord[0];
                parAuthenRecords.Value = MBOSSClass.GenerateXmlForAuthenRecords(AuthenRecords == null ? au : AuthenRecords);
                cmd.Parameters.Add(parAuthenRecords);
                ErrMsg = MBOSSClass.GenerateXmlForAuthenRecords(AuthenRecords == null ? au : AuthenRecords);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID ", SqlDbType.VarChar, 2);
                parProvinceID.Value = UAProvinceID;
                cmd.Parameters.Add(parProvinceID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustType = new SqlParameter("@CustType ", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID ", SqlDbType.VarChar, 2);
                parUProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parSysID = new SqlParameter("@SysID ", SqlDbType.VarChar, 4);
                parSysID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSysID);

                SqlParameter parAreaID = new SqlParameter("@AreaID ", SqlDbType.VarChar, 4);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parOuterID = new SqlParameter("@outerid ", SqlDbType.VarChar, 30);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);


                SqlParameter parUserName = new SqlParameter("@UserName ", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);


                SqlParameter parRealName = new SqlParameter("@RealName ", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);


                SqlParameter parNickName = new SqlParameter("@NickName ", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter pardealType = new SqlParameter("@dealType ", SqlDbType.VarChar, 1);
                pardealType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pardealType);
                

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();
                CustType = parCustType.Value.ToString();
                UProvinceID = parUProvinceID.Value.ToString();
                SysID = parSysID.Value.ToString();
                areaid = parAreaID.Value.ToString();

                outerid = parOuterID.Value.ToString();
                ProvinceID = UProvinceID;
                dealType = pardealType.Value.ToString();

                NickName = parNickName.Value.ToString();
                UserName = parUserName.Value.ToString();
                RealName = parRealName.Value.ToString();
               
                return Result;
            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;

        }


        /// <summary> 
        /// 密码认证v2
        /// 作者：tongbo   时间：2009-8-14
        /// </summary>
        public static int MUserAuthV3(string SPID, string UAProvinceID,string JFProvinceID, string AuthenName, string AuthenType, MBOSSClass.AuthenRecord[] AuthenRecords, HttpContext Context, out string ErrMsg, out string CustID, out string UserAccount, out string CustType, out string outerid, out string ProvinceID,
            out string RealName, out string UserName, out string NickName, out string dealType, out int type, out string areaid)
        {
            areaid = "";
            type = 0;
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";
            CustType = "";
            UserName = "";
            NickName = "";
            RealName = "";
            string UProvinceID = "";
            string SysID = "";

            int CrmResult = ErrorDefinition.IError_Result_UnknowError_Code;

            outerid = "";
            ProvinceID = "";
            dealType = "0";

            try
            {
                //5, E家卡
                //6，c网手机用户卡
                //7，c网手机用户手机号
                //8，省百事通卡
                //须远程认证
                //string AuthenTypeSpan = "5,6,7,8";
                //if (AuthenTypeSpan.IndexOf(AuthenType) < 0) //本地认证
                //{
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                string MSProvinceID = System.Configuration.ConfigurationManager.AppSettings["MSProvinceID"];
                if (UAProvinceID != "" && MSProvinceID.IndexOf(UAProvinceID) >= 0)
                {
                    cmd.CommandText = "dbo.up_Customer_OV3_Interface_MUserAuthV1";
                }
                else
                {
                    cmd.CommandText = "dbo.up_Customer_OV3_Interface_MUserAuthV3";
                    type = 1;
                }


                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 256);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

                SqlParameter parAuthenRecords = new SqlParameter("@AuthenRecords", SqlDbType.Text);
                MBOSSClass.AuthenRecord[] au = new MBOSSClass.AuthenRecord[0];
                parAuthenRecords.Value = MBOSSClass.GenerateXmlForAuthenRecords(AuthenRecords == null ? au : AuthenRecords);
                cmd.Parameters.Add(parAuthenRecords);
                ErrMsg = MBOSSClass.GenerateXmlForAuthenRecords(AuthenRecords == null ? au : AuthenRecords);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID ", SqlDbType.VarChar, 2);
                parProvinceID.Value = UAProvinceID;
                cmd.Parameters.Add(parProvinceID);


                SqlParameter parJFProvinceID = new SqlParameter("@JFProvinceID ", SqlDbType.VarChar, 2);
                parJFProvinceID.Value = JFProvinceID;
                cmd.Parameters.Add(parJFProvinceID);

                


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustType = new SqlParameter("@CustType ", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID ", SqlDbType.VarChar, 2);
                parUProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parSysID = new SqlParameter("@SysID ", SqlDbType.VarChar, 4);
                parSysID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSysID);

                SqlParameter parAreaID = new SqlParameter("@AreaID ", SqlDbType.VarChar, 4);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parOuterID = new SqlParameter("@outerid ", SqlDbType.VarChar, 30);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);


                SqlParameter parUserName = new SqlParameter("@UserName ", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);


                SqlParameter parRealName = new SqlParameter("@RealName ", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);


                SqlParameter parNickName = new SqlParameter("@NickName ", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter pardealType = new SqlParameter("@dealType ", SqlDbType.VarChar, 1);
                pardealType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pardealType);


                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();
                CustType = parCustType.Value.ToString();
                UProvinceID = parUProvinceID.Value.ToString();
                SysID = parSysID.Value.ToString();
                areaid = parAreaID.Value.ToString();

                outerid = parOuterID.Value.ToString();
                ProvinceID = UProvinceID;
                dealType = pardealType.Value.ToString();

                NickName = parNickName.Value.ToString();
                UserName = parUserName.Value.ToString();
                RealName = parRealName.Value.ToString();

                return Result;


            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }


            return Result;

        }


        public static string PhoneToArea(string @ProvinceID, string Phone,out string strPhone)
        {
            string AreaID = "";
            strPhone = "";
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;   
                cmd.CommandText = "up_Customer_OV3_Interface_PhoneToArea";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parAreaID = new SqlParameter("@AreaID ", SqlDbType.VarChar, 4);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parstrPhone = new SqlParameter("@strPhone ", SqlDbType.VarChar, 20);
                parstrPhone.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parstrPhone);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                AreaID = parAreaID.Value.ToString();
                strPhone = parstrPhone.Value.ToString();
                
            }
            catch
            { }
            return AreaID;
        }

        /*

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="AuthenType"></param>
        /// <param name="AuthenName"></param>
        /// <param name="LoginType"></param>
        /// <param name="Results1"></param>
        /// <param name="SPID"></param>
        /// <param name="DealTime"></param>
        /// <param name="Description"></param>
        public static void InsertCustAuthenLog(string CustID,string AuthenType,string AuthenName,string LoginType,int Results1,string SPID,DateTime  DealTime, string Description)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "dbo.up_Customer_OV3_Interface_InsertCustAuthenLog";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 38);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parLoginType = new SqlParameter("@LoginType", SqlDbType.Int);
                parLoginType.Value = LoginType;
                cmd.Parameters.Add(parLoginType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Value = Results1;
                cmd.Parameters.Add(parResult);

                SqlParameter parSPID = new SqlParameter("@SPID ", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);
                
                SqlParameter parDealTime = new SqlParameter("@DealTime ", SqlDbType.DateTime);
                parDealTime.Value = DealTime;
                cmd.Parameters.Add(parDealTime);

                SqlParameter parDescription = new SqlParameter("@Description ", SqlDbType.VarChar, 40);
                parDescription.Value = Description;
                cmd.Parameters.Add(parDescription);

                    
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            }
            catch (Exception ex)
            {
                
            }

        }
         * */
        /// <summary>
        /// 认证方式查询接口
        /// 作者：张英杰   时间：2009-8-10
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="AuthenName"></param>
        /// <param name="AuthenType"></param>
        /// <param name="Password"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public static int AuthStyleQueryByAuthenName(string SPID, string AuthenName, string AuthenType, out string ErrMsg, out string CustID, out string UserAccount)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "dbo.up_Customer_OV3_Interface_AuthStyleQueryByAuthenName";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 48);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 1);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

               

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();


            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;

        }

        /// <summary>
        /// 认证方式通知接口
        /// 作者：张英杰   时间：2009-8-10
        /// </summary>
        public static int AuthStyleNotify(string SPID, string CustID, string AuthenName,
            string AuthenType, string OPType, string ExtendField, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_V3_Interface_AuthStyleNotify";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 48);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 1);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

                SqlParameter parOPType = new SqlParameter("@OPType", SqlDbType.VarChar, 8);
                parOPType.Value = OPType;
                cmd.Parameters.Add(parOPType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        
        }

        /// <summary>
        /// 帐号合并接口
        /// </summary>
        public static int IncorporateCust(string SPID, string IncorporatedCustID, string SavedCustID, string ExtendField,
            out int Result, out string ErrorDescription, out string SavedCustID_out, out string SavedUserAccount)
        {
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";
            SavedCustID_out = "";
            SavedUserAccount = "";

            try
            {
/******************************统一平台帐号合并接口************************************************************/
 
               Result = IncorporateCust_xy(SPID, IncorporatedCustID, SavedCustID, ExtendField,
                out Result, out ErrorDescription);
               if (Result != 0)
               {
                   SavedCustID_out = "";
                   SavedUserAccount = "";

                   return Result;
               }
        
               try
               {

                   /****************************生成XML***************************************/
                   XmlDocument xmldoc;
                   XmlNode xmlnode;
                   XmlElement xmlelem;
                   XmlElement xmlelem2;
                   XmlText xmltext;
                   string XML;
                   xmldoc = new XmlDocument();
                   //加入XML的声明段落

                   xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                   xmldoc.AppendChild(xmlnode);
                   //加入一个根元素
                   xmlelem = xmldoc.CreateElement("", "ROOT", "");
                   xmldoc.AppendChild(xmlelem);
                   xmlelem2 = xmldoc.CreateElement("SPID");
                   xmlelem2 = xmldoc.CreateElement("", "SPID", "");
                   xmltext = xmldoc.CreateTextNode(SPID);
                   xmlelem2.AppendChild(xmltext);
                   xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
                   xmlelem2 = xmldoc.CreateElement("IncorporatedCustID");
                   xmlelem2 = xmldoc.CreateElement("", "IncorporatedCustID", "");
                   xmltext = xmldoc.CreateTextNode(IncorporatedCustID);
                   xmlelem2.AppendChild(xmltext);
                   xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
                   xmlelem2 = xmldoc.CreateElement("SavedCustID");
                   xmlelem2 = xmldoc.CreateElement("", "SavedCustID", "");
                   xmltext = xmldoc.CreateTextNode(SavedCustID);
                   xmlelem2.AppendChild(xmltext);
                   xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
                   xmlelem2 = xmldoc.CreateElement("ExtendField");
                   xmlelem2 = xmldoc.CreateElement("", "ExtendField", "");
                   xmltext = xmldoc.CreateTextNode(ExtendField);
                   xmlelem2.AppendChild(xmltext);
                   xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
                   XML = xmldoc.OuterXml;
                   XML = XML.Substring(XML.IndexOf("<ROOT>"));
                   XML = @"<?xml version='1.0' encoding='gb2312' standalone='yes' ?>" + XML;


                   /***************************发送数据给统一平台****************************/
                   IncorporateCust_YZ yz = new IncorporateCust_YZ();
                   yz.Url = ConfigurationManager.AppSettings["JFUrl"];
                   string ResultXML = yz.IncorporateCust(XML);
                   //"<?xml version='1.0' encoding='utf-16' standalone='yes'?><root><Result>0</Result><ErrorDescription>成功</ErrorDescription><CustID>333333</CustID><ExtendField>555555</ExtendField></root>";


                   /***************************解析xml****************************/

                   Result = int.Parse(CommonUtility.GetValueFromXML(ResultXML, "Result"));

                   ErrorDescription = CommonUtility.GetValueFromXML(ResultXML, "ErrorDescription");
                   SavedCustID_out = CommonUtility.GetValueFromXML(ResultXML, "CustID");
                   SavedUserAccount = CommonUtility.GetValueFromXML(ResultXML, "SavedUserAccount");

               }
               catch (Exception e)
               {
                   Result = ErrorDefinition.IError_Result_System_UnknowError_Code; ;
                   ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
               }
               finally
               {
                   #region WriteLog
                   StringBuilder msg1 = new StringBuilder();
                   msg1.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                   msg1.Append("统一平台帐号合并接口 " + DateTime.Now.ToString("u") + "\r\n");
                   msg1.Append(";SPID - " + SPID);
                   msg1.Append(";IncorporatedCustID - " + IncorporatedCustID);
                   msg1.Append(";SavedCustID - " + SavedCustID);
                   msg1.Append(";ExtendField - " + ExtendField);
                   msg1.Append("\r\n");

       
                   msg1.Append("处理结果 - " + Result);
                   msg1.Append("; 错误描述 - " + ErrorDescription);
                   msg1.Append("; SavedCustID - " + SavedCustID_out);
                   msg1.Append("; SavedUserAccount - " + SavedUserAccount + "\r\n");
                   msg1.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                   BTUCenterInterfaceLog.CenterForBizTourLog("IncorporateCust_ScoreForBT", msg1);
                   #endregion
               }
/***************************帐户合并接口流程*******************************************************************/
               if (Result != 0)
               {
                   return Result;
               }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V2_Interface_IncorporateCust";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);
                
                SqlParameter parIncorporatedCustID = new SqlParameter("@IncorporatedCustID", SqlDbType.VarChar, 16);
                parIncorporatedCustID.Value = IncorporatedCustID;
                cmd.Parameters.Add(parIncorporatedCustID);
          

                SqlParameter parSavedCustID = new SqlParameter("@SavedCustID", SqlDbType.VarChar, 16);
                parSavedCustID.Value = SavedCustID;
                cmd.Parameters.Add(parSavedCustID);


                SqlParameter parExtendField = new SqlParameter("@ExtendField", SqlDbType.VarChar, 16);
                parExtendField.Value = ExtendField;
                cmd.Parameters.Add(parExtendField);

               
                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);
              
                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                SqlParameter parSavedCustID_out = new SqlParameter("@SavedCustID_out", SqlDbType.VarChar, 16);
                parSavedCustID_out.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSavedCustID_out);

                SqlParameter parSavedUserAccount = new SqlParameter("@SavedUserAccount ", SqlDbType.VarChar, 16);
                parSavedUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSavedUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();
                SavedCustID_out = parSavedCustID_out.Value.ToString();
                SavedUserAccount = parSavedUserAccount.Value.ToString();

               

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }
           
            return Result;
        }



        /// <summary>
        /// 帐号合并验证
        /// </summary>
        public static int IncorporateCust_xy(string SPID, string IncorporatedCustID, string SavedCustID, string ExtendField,
            out int Result, out string ErrorDescription)
        {
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";
        

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V2_Interface_IncorporateCustYZ";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parIncorporatedCustID = new SqlParameter("@IncorporatedCustID", SqlDbType.VarChar, 16);
                parIncorporatedCustID.Value = IncorporatedCustID;
                cmd.Parameters.Add(parIncorporatedCustID);


                SqlParameter parSavedCustID = new SqlParameter("@SavedCustID", SqlDbType.VarChar, 16);
                parSavedCustID.Value = SavedCustID;
                cmd.Parameters.Add(parSavedCustID);


                SqlParameter parExtendField = new SqlParameter("@ExtendField", SqlDbType.VarChar, 16);
                parExtendField.Value = ExtendField;
                cmd.Parameters.Add(parExtendField);

         
                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);
              

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();             



            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PwdQuestionRecord"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public static int PwdQuestionQuery(out DataTable PwdQuestionRecord,  out string ErrorDescription)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";

            PwdQuestionRecord = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V5_Interface_PwdQuestionQuery";

                DataSet ds = DBUtility.FillData(cmd,DBUtility.BestToneCenterConStr);

                DataTable dt = ds.Tables[0];
                if (dt != null)
                {
                    PwdQuestionRecord = dt;
                    Result = 0;
                    ErrorDescription = "";
                }
                else
                {
                    Result = ErrorDefinition.IError_Result_UnknowError_Code;
                    ErrorDescription = "数据为null！";
                }



            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="PwdQuestionRecord"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public static int UserPwdQuestionQuery(string CustID, out DataTable PwdQuestionRecord, out string ErrorDescription)
        {
            int Result = -19999;
            ErrorDescription = "";
            PwdQuestionRecord = new DataTable();

            SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V5_Interface_UserPwdQuestionQuery";
                cmd.Connection = conn;
              


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


             
                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                DataSet ds  = DBUtility.FillData(cmd,DBUtility.BestToneCenterConStr);
                PwdQuestionRecord = ds.Tables[0];

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();              
             

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="QuestionID"></param>
        /// <param name="Answer"></param>
        /// <param name="Result"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public static int PwdQuestionUpload(string CustID, int[] QuestionID, string[] Answer,  out string ErrorDescription)
        {
            int Result =-19999;
            ErrorDescription = "";

            SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V5_Interface_InsertAnswer";
                cmd.Connection = conn;
                cmd.Transaction = tran;


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
            
                cmd.Parameters.Add(parCustID);

                SqlParameter parQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int);
               
                cmd.Parameters.Add(parQuestionID);


                SqlParameter parAnswer = new SqlParameter("@Answer", SqlDbType.VarChar, 100);
               
                cmd.Parameters.Add(parAnswer);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);
                for (int i = 0; i < QuestionID.Length; i++)
                {

                  
                    parCustID.Value = CustID;
                   
                    parQuestionID.Value = QuestionID[i];
                  
                    parAnswer.Value = Answer[i];                   

                    cmd.ExecuteNonQuery();
                   // DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                    Result = int.Parse(parResult.Value.ToString());
                    ErrorDescription = parErrorDescription.Value.ToString();
                    if (Result != 0)
                    {
                        tran.Rollback();
                        return Result;
                    }                    
                }
                tran.Commit();  

            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="QuestionID"></param>
        /// <param name="Answer"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public static int PwdQuestionAuth(string CustID, int QuestionID, string Answer, out string ErrorDescription)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";
            
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V5_Interface_PwdQuestionAuth";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int);
                parQuestionID.Value = QuestionID;
                cmd.Parameters.Add(parQuestionID);


                SqlParameter parAnswer = new SqlParameter("@Answer", SqlDbType.VarChar, 100);
                parAnswer.Value = Answer;
                cmd.Parameters.Add(parAnswer);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();
              

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }

            return Result;
        }


        public static int CustEnterpriseModify(string SPID,string CustID,string EnterpriseChange,out string ErrorDescription)
        {
              int Result = ErrorDefinition.IError_Result_UnknowError_Code;
              ErrorDescription = "";
            
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_CustEnterpriseModify";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parEnterpriseChange = new SqlParameter("@EnterpriseChange", SqlDbType.Int);
                parEnterpriseChange.Value = EnterpriseChange;
                cmd.Parameters.Add(parEnterpriseChange);


                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();
              

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }

            return Result;
        }


        public static int GetPhoneTOArea(string phone, out string ProvinceID, out string Areaid, out string ErrMsg)
        {
            int Result = -19999;
            ProvinceID = "";
            Areaid = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_GetPhoneTOArea";

                SqlParameter parPhone = new SqlParameter("@Phone ", SqlDbType.VarChar, 20);
                parPhone.Value = phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID ", SqlDbType.VarChar, 2);
                parProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaid = new SqlParameter("@Areaid ", SqlDbType.VarChar, 3);
                parAreaid.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaid);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

              

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                ProvinceID = parProvinceID.Value.ToString();
                Areaid = parAreaid.Value.ToString();

            }
            catch(Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }
    }
}

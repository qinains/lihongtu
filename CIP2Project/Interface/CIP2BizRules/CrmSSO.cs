using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web;
using System.Configuration;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;
using BTUCenter.CSB.Proxy;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Rule.Young.Entity;

using System.Collections;
using log4net;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// Crm查询认证
    /// </summary>
    public class CrmSSO
    {
        public static int count = 0;
        private static readonly ILog logger = LogManager.GetLogger(typeof(CrmSSO));

        /// <summary>
        /// 作者 lihongtu
        /// </summary>
        /// <param name="UAProvinceID"></param>
        /// <param name="AuthenType"></param>
        /// <param name="Username"></param>
        /// <param name="RealName1"></param>
        /// <param name="Password"></param>
        /// <param name="PasswdFlag"></param>
        /// <param name="ScoreBesttoneSPID"></param>
        /// <param name="context"></param>
        /// <param name="RealName"></param>
        /// <param name="UserName"></param>
        /// <param name="NickName"></param>
        /// <param name="OutID"></param>
        /// <param name="CustType"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="TestStr"></param>
        /// <returns></returns>
        public static int UserAuthUam(string UAProvinceID, string AuthenType, string Username, string RealName1, string Password, string PasswdFlag, string ScoreBesttoneSPID, HttpContext context, out string RspType,out string RspCode,out string RspDesc,out string AuthenticationKey,out string ErrMsg, out string TestStr)
        {
            logger.Info("UserAuthUam;");
            logger.Info("Parameters;");
            logger.Info("UAProvinceID=" + UAProvinceID + ";AuthenType=" + AuthenType + ";Username=" + Username + ";RealName1=" + RealName1 + ";Password=" + Password + ";PasswdFlag=" + PasswdFlag + ";ScoreBesttoneSPID=" + ScoreBesttoneSPID);
            int Result = -19999;

            RspType = String.Empty;
            RspCode = String.Empty;
            RspDesc = String.Empty;
            AuthenticationKey = String.Empty;
            ErrMsg = String.Empty;
            TestStr = String.Empty;
            string str = String.Empty;
            string rStr = String.Empty;

            try
            {
                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS37059";                                    //业务功能编码
                string ServiceCode = "SVC33051";                                //接口协议编码
                string ServiceContractVer = "SVC3305120130509";                 //协议当前使用的版本号
                string ActionCode = "0";                                        //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //服务等级,处理的优先级
                string SrcOrgID = "100000";                                     //发起方机构代码
                string SrcSysID = "1000000020";                                 //发起方(系统/平台)编码
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //发起方(系统/平台)签名
                //落地方机构编码
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }
                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }
                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //请求时间，时间格式字符
             
                //标识类型
                string AccountType = "";
                //标识类型编码装换
                Result = ConvertAuthenType(AuthenType, out AccountType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //标识号码
                string AccountID = "";
                //所属地代码
                string CityCode = "";
                //固话(9)、小灵通(10)、宽带的特殊处理(11)、手机(7)
                if (AuthenType == "9" || AuthenType == "10" || AuthenType == "11")
                {
                    if (Username.IndexOf('-') > 0)
                    {
                        string[] alUsername = Username.Split('-');
                        CityCode = alUsername[0];
                        AccountID = alUsername[1];
                    }
                    else
                    {
                        CityCode = "";
                        AccountID = Username;
                    }
                }
                else
                {
                    CityCode = "";
                    AccountID = Username;
                }


                string PWDType = "0" + PasswdFlag;

                //密码
                string CCPasswd = Password;

                //构建Uam查询xml
                XMLExchange xMLExchange = new XMLExchange();

                str = xMLExchange.BuildUamAuthenXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID, ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime, AccountType, AccountID, PWDType, Password, UAProvinceID);


                logger.Info("请求crm-uam枢纽报文:");
                logger.Info(str);

                BTUCenter.CSB.Proxy.IDEPService obj = new BTUCenter.CSB.Proxy.IDEPService();
                //DEPService obj = new DEPService();
                //obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEP-CSBServiceURL"];
 
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm-uam枢纽返回报文:");
                logger.Info(rStr);
     
                AuthenUamReturn authenUamReturn = xMLExchange.AnalysisUamAuthenReturnXML(rStr);
                if (authenUamReturn.TcpCont.Response.RspType == "0")
                {
                    RspType = authenUamReturn.SvcCont.AuthenticationQryResp.RspType;
                    RspCode = authenUamReturn.SvcCont.AuthenticationQryResp.RspCode;
                    RspDesc = authenUamReturn.SvcCont.AuthenticationQryResp.RspDesc;
                    AuthenticationKey = authenUamReturn.SvcCont.AuthenticationQryResp.AuthenticationKey;
                    Result = 0;
                }

            }
            catch (Exception ex1)
            {
                logger.Info(ex1.ToString());
                ErrMsg = ex1.Message.ToString();
                Result = -29999;
            }
            finally
            {
                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);
            }
            return Result;
        }


        public static int UserAuthCrm(string UAProvinceID, string AuthenType, string Username, string RealName1,string Password,string PasswdFlag,string  ScoreBesttoneSPID,HttpContext context,out string RealName, out string UserName, out string NickName, out string OutID, out string CustType,out string CustID, out string ErrMsg,out string TestStr)
        {
            logger.Info("UAProvinceID=" + UAProvinceID + ";AuthenType=" + AuthenType + ";Username=" + Username + ";RealName1=" + RealName1 + ";Password=" + Password + ";PasswdFlag=" + PasswdFlag + ";ScoreBesttoneSPID=" + ScoreBesttoneSPID);
            int Result = -19999;
         
            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            CustType = "";
            CustID = "";
            ErrMsg = "";
            TestStr = "";
            string str = "";
            string rStr = "";
   
            try
            {
                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS17002";                                    //业务功能编码
                string ServiceCode = "SVC11001";                                //接口协议编码
                string ServiceContractVer = "SVC1100120091002";                 //协议当前使用的版本号
                string ActionCode = "0";                                        //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");       
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //服务等级,处理的优先级
                string SrcOrgID = "100000";                                     //发起方机构代码
                string SrcSysID = "1000000020";                                 //发起方(系统/平台)编码
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //发起方(系统/平台)签名
                //落地方机构编码
                string DstOrgID = "";                                           
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }
                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }

                #region 隐藏不用
                /*
                switch (UAProvinceID)
                {
                    case "02":
                        //落地方机构编码
                        DstOrgID = "600102";
                        //落地方(系统/平台)编码
                        DstSysID = "6001020001";
                        break;
                    case "19":
                        //落地方机构编码
                        DstOrgID = "600203";
                        //落地方(系统/平台)编码
                        DstSysID = "6002030001";
                        break;
                    case "05":
                        //落地方机构编码
                        DstOrgID = "609906";
                        //落地方(系统/平台)编码
                        DstSysID = "6099060001";
                        break;
                    case "08":
                        //落地方机构编码
                        DstOrgID = "609905";
                        //落地方(系统/平台)编码
                        DstSysID = "6099050001";
                        break;
                    case "30":
                        //落地方机构编码
                        DstOrgID = "600404";
                        //落地方(系统/平台)编码
                        DstSysID = "6004040001";
                        break;
                    case "20":
                        //落地方机构编码
                        DstOrgID = "600101";
                        //落地方(系统/平台)编码
                        DstSysID = "6001010001";
                        break;
                    default:
                        ErrMsg = "指定省不在枢纽中";
                        return -1;
                        break;
                }
                **/
                #endregion

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //请求时间，时间格式字符
                string InfoTypeID = "31";                                                       //资料类型代码
                //标识类型
                string CodeType = "";
                //标识类型编码装换
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;
               
                //标识号码
                string CodeValue = "";
                //所属地代码
                string CityCode = "";
                //固话、小灵通、宽带的特殊处理
                if (AuthenType == "9" || AuthenType == "10" || AuthenType == "11" )
                {
                    if (Username.IndexOf('-') > 0)
                    {
                        string[] alUsername = Username.Split('-');
                        CityCode = alUsername[0];
                        CodeValue = alUsername[1];
                    }
                    else
                    {
                        CityCode = "";
                        CodeValue = Username;
                    }
                }
                else
                {
                    CityCode = "";
                    CodeValue = Username;
                }
               
                //是否需填密码
                //string PasswdFlag = "1";
                //密码
                string CCPasswd = Password;
                
                //构建Crm查询xml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                logger.Info("请求crm枢纽报文:");
                logger.Info(str);
                //与Crm交互查询
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm枢纽返回报文:");
                logger.Info(rStr);
                //解析Crm返回客户信息
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    //客户类型转换
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //证件类型转换
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);
                    
                    //会员类型转换
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //全国CRM用户注册到号百
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //客户类型
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //未知
                        OutID,
                        ScoreBesttoneSPID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                        out CustID,
                        out ErrMsg);

                    if (Result != 0)
                    {
                        return Result;
                    }
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", ScoreBesttoneSPID, "", "0", out ErrMsg);
                }
                else
                {
                    ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
                    int rspcode = -1;
                    try
                    {
                        rspcode = int.Parse(qryCustInfoReturn.TcpCont.Response.RspCode);
                    }
                    catch {
                        return rspcode;
                    }
                    return rspcode;
                }
            }
            catch (Exception ex1)
            {
                ErrMsg = ex1.Message.ToString();
                Result = -29999;
            }
            finally
            {
                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);
            }
            return Result;
        }



        public static int UserAuthCrmV3(string UAProvinceID, string AuthenType, string Username, string RealName1, string Password, string PasswdFlag, string ScoreBesttoneSPID, HttpContext context, out string RealName, out string UserName, out string NickName, out string OutID, out string CustType, out string CustID, out string CustAddress, out string ErrMsg, out string TestStr,out string ExtendField)
        {
            logger.Info("UserAuthCrmV2");
            logger.Info("UAProvinceID=" + UAProvinceID + ";AuthenType=" + AuthenType + ";Username=" + Username + ";RealName1=" + RealName1 + ";Password=" + Password + ";PasswdFlag=" + PasswdFlag + ";ScoreBesttoneSPID=" + ScoreBesttoneSPID);
            int Result = -19999;

            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            CustAddress = "";
            CustType = "";
            CustID = "";
            ErrMsg = "";
            TestStr = "";
            ExtendField = "";
            string str = "";
            string rStr = "";

            try
            {
                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS17002";                                    //业务功能编码
                string ServiceCode = "SVC11001";                                //接口协议编码
                string ServiceContractVer = "SVC1100120091002";                 //协议当前使用的版本号
                string ActionCode = "0";                                        //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //服务等级,处理的优先级
                string SrcOrgID = "100000";                                     //发起方机构代码
                string SrcSysID = "1000000020";                                 //发起方(系统/平台)编码
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //发起方(系统/平台)签名
                //落地方机构编码
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }
                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }

                #region 隐藏不用
                /*
                switch (UAProvinceID)
                {
                    case "02":
                        //落地方机构编码
                        DstOrgID = "600102";
                        //落地方(系统/平台)编码
                        DstSysID = "6001020001";
                        break;
                    case "19":
                        //落地方机构编码
                        DstOrgID = "600203";
                        //落地方(系统/平台)编码
                        DstSysID = "6002030001";
                        break;
                    case "05":
                        //落地方机构编码
                        DstOrgID = "609906";
                        //落地方(系统/平台)编码
                        DstSysID = "6099060001";
                        break;
                    case "08":
                        //落地方机构编码
                        DstOrgID = "609905";
                        //落地方(系统/平台)编码
                        DstSysID = "6099050001";
                        break;
                    case "30":
                        //落地方机构编码
                        DstOrgID = "600404";
                        //落地方(系统/平台)编码
                        DstSysID = "6004040001";
                        break;
                    case "20":
                        //落地方机构编码
                        DstOrgID = "600101";
                        //落地方(系统/平台)编码
                        DstSysID = "6001010001";
                        break;
                    default:
                        ErrMsg = "指定省不在枢纽中";
                        return -1;
                        break;
                }
                **/
                #endregion

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //请求时间，时间格式字符
                string InfoTypeID = "31";                                                       //资料类型代码
                //标识类型
                string CodeType = "";
                //标识类型编码装换
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //标识号码
                string CodeValue = "";
                //所属地代码
                string CityCode = "";
                //固话、小灵通、宽带的特殊处理
                if (AuthenType == "9" || AuthenType == "10" || AuthenType == "11")
                {
                    if (Username.IndexOf('-') > 0)
                    {
                        string[] alUsername = Username.Split('-');
                        CityCode = alUsername[0];
                        CodeValue = alUsername[1];
                    }
                    else
                    {
                        CityCode = "";
                        CodeValue = Username;
                    }
                }
                else
                {
                    CityCode = "";
                    CodeValue = Username;
                }

                //是否需填密码
                //string PasswdFlag = "1";
                //密码
                string CCPasswd = Password;

                //构建Crm查询xml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                logger.Info("请求crm枢纽报文:");
                logger.Info(str);
                //与Crm交互查询
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm枢纽返回报文:");
                logger.Info(rStr);
                //解析Crm返回客户信息
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    CustAddress = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress;

                    //客户类型转换
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //证件类型转换
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //会员类型转换
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //全国CRM用户注册到号百
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //客户类型
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //未知
                        OutID,
                        ScoreBesttoneSPID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                        out CustID,
                        out ErrMsg);

                    if (Result != 0)
                    {
                        return Result;
                    }
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", ScoreBesttoneSPID, "", "0", out ErrMsg);
                }
                else
                {
                    ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
                    int rspcode = -1;
                    try
                    {
                        rspcode = int.Parse(qryCustInfoReturn.TcpCont.Response.RspCode);
                    }
                    catch
                    {
                        return rspcode;
                    }
                    return rspcode;
                }
            }
            catch (Exception ex1)
            {
                ErrMsg = ex1.Message.ToString();
                Result = -29999;
            }
            finally
            {
                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);
            }
            return Result;
        }


        public static int UserAuthCrmV2(string UAProvinceID, string AuthenType, string Username, string RealName1, string Password, string PasswdFlag, string ScoreBesttoneSPID, HttpContext context, out string RealName, out string UserName, out string NickName, out string OutID, out string CustType, out string CustID, out string CustAddress, out string ErrMsg, out string TestStr, out QryCustInfoV2Return qryCustInfoReturn)
        {
            logger.Info("UserAuthCrmV2");
            logger.Info("UAProvinceID=" + UAProvinceID + ";AuthenType=" + AuthenType + ";Username=" + Username + ";RealName1=" + RealName1 + ";Password=" + Password + ";PasswdFlag=" + PasswdFlag + ";ScoreBesttoneSPID=" + ScoreBesttoneSPID);
            int Result = -19999;

            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            CustAddress = "";
            CustType = "";
            CustID = "";
            ErrMsg = "";
            TestStr = "";
            string str = "";
            string rStr = "";
            qryCustInfoReturn = null;
            try
            {
                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS17002";                                    //业务功能编码
                string ServiceCode = "SVC11001";                                //接口协议编码
                string ServiceContractVer = "SVC1100120091002";                 //协议当前使用的版本号
                string ActionCode = "0";                                        //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //服务等级,处理的优先级
                string SrcOrgID = "100000";                                     //发起方机构代码
                string SrcSysID = "1000000020";                                 //发起方(系统/平台)编码
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //发起方(系统/平台)签名
                //落地方机构编码
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }
                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //请求时间，时间格式字符
                string InfoTypeID = "31";                                                       //资料类型代码
                //标识类型
                string CodeType = "";
                //标识类型编码装换
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //标识号码
                string CodeValue = "";
                //所属地代码
                string CityCode = "";
                //固话、小灵通、宽带的特殊处理
                if (AuthenType == "9" || AuthenType == "10" || AuthenType == "11")
                {
                    if (Username.IndexOf('-') > 0)
                    {
                        string[] alUsername = Username.Split('-');
                        CityCode = alUsername[0];
                        CodeValue = alUsername[1];
                    }
                    else
                    {
                        CityCode = "";
                        CodeValue = Username;
                    }
                }
                else
                {
                    CityCode = "";
                    CodeValue = Username;
                }

                //是否需填密码
                //string PasswdFlag = "1";
                //密码
                string CCPasswd = Password;

                //构建Crm查询xml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                logger.Info("请求crm-v4枢纽报文:");
                logger.Info(str);
                //与Crm交互查询
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm-v4枢纽返回报文:");
                logger.Info(rStr);
                //解析Crm返回客户信息
                qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoV2XML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    CustAddress = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress;

                    //客户类型转换
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //证件类型转换
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //会员类型转换
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //全国CRM用户注册到号百
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //客户类型
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //未知
                        OutID,
                        ScoreBesttoneSPID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                        out CustID,
                        out ErrMsg);

                    if (Result != 0)
                    {
                        return Result;
                    }
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", ScoreBesttoneSPID, "", "0", out ErrMsg);
                }
                else
                {
                    ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
                    int rspcode = -1;
                    try
                    {
                        rspcode = int.Parse(qryCustInfoReturn.TcpCont.Response.RspCode);
                    }
                    catch
                    {
                        return rspcode;
                    }
                    return rspcode;
                }
            }
            catch (Exception ex1)
            {
                logger.Info(ex1.StackTrace);
                ErrMsg = ex1.Message.ToString();
                Result = -29999;
            }
            finally
            {
                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);
            }
            return Result;
        }
  

        public static int GetCustIdByAccNbr(string UAProvinceID,string SOO_ID, string LAN_ID, string AREA_NBR, string ACC_NBR, string PROD_CLASS, string ScoreBesttoneSPID, HttpContext context, out ClubMember cm,out string ErrMsg, out string TestStr)
        {
            int Result = -19999;
            ErrMsg = "";
            TestStr = "";
            string str = "";
            string rStr = "";
            cm = new ClubMember();
            StringBuilder param = new StringBuilder();
            try
            {

                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS17006";                                    //业务功能编码
                string ServiceCode = "SVC33003";                                //接口协议编码
                string ServiceContractVer = "SVC3300320120719";                 //协议当前使用的版本号
                string ActionCode = "0";                                        //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();
                string ServiceLevel = "1";                                      //服务等级,处理的优先级
                string SrcOrgID = "100000";                                     //发起方机构代码
                string SrcSysID = "1000000020";                                 //发起方(系统/平台)编码
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //发起方(系统/平台)签名
                //落地方机构编码
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }
                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }
                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //请求时间，时间格式字符
  
                //构建Crm查询xml
                XMLExchange xMLExchange = new XMLExchange();
                str =xMLExchange.BuildYoungQryMemberXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                SOO_ID, LAN_ID, AREA_NBR, ACC_NBR, PROD_CLASS);


                StringBuilder requestXml = new StringBuilder();

                #region 拼接请求xml字符串

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<ContractRoot>");
                requestXml.Append("<TcpCont>");
                requestXml.AppendFormat("<ActionCode>{0}</ActionCode>", ActionCode);
                requestXml.AppendFormat("<TransactionID>{0}</TransactionID>", TransactionID);
                requestXml.AppendFormat("<ServiceLevel>{0}</ServiceLevel>", ServiceLevel);
                requestXml.AppendFormat("<BusCode>{0}</BusCode>", BusCode);
                requestXml.AppendFormat("<ServiceCode>{0}</ServiceCode>", ServiceCode);
                requestXml.AppendFormat("<ServiceContractVer>{0}</ServiceContractVer>", ServiceContractVer);
                requestXml.AppendFormat("<SrcOrgID>{0}</SrcOrgID>", SrcOrgID);
                requestXml.AppendFormat("<SrcSysID>{0}</SrcSysID>", SrcSysID);
                requestXml.AppendFormat("<SrcSysSign>{0}</SrcSysSign>", SrcSysSign);
                requestXml.AppendFormat("<DstOrgID>{0}</DstOrgID>", DstOrgID);
                requestXml.AppendFormat("<DstSysID>{0}</DstSysID>", DstSysID);
                requestXml.AppendFormat("<ReqTime>{0}</ReqTime>", ReqTime);
                requestXml.Append("</TcpCont>");
                requestXml.Append("<SvcCont>");
                requestXml.Append("<SOO type=\"QRY_CLUB_MEMBER_REQ_TYPE\">");
                requestXml.Append("<PUB_REQ>");
                requestXml.Append("<SOO_ID>1</SOO_ID>");
                requestXml.Append("</PUB_REQ>");
                requestXml.AppendFormat("<PROD_INST_ID>:getProdInstIdByAccNbr({0},{1},{2},{3})</PROD_INST_ID>", LAN_ID, AREA_NBR, ACC_NBR, PROD_CLASS);    //LAN_ID + "," + AREA_NBR + "," + ACC_NBR + "," + PROD_CLASS 
                requestXml.Append("</SOO>");
                requestXml.Append("</SvcCont>");
                requestXml.Append("</ContractRoot>");

                #endregion




                //与Crm交互查询
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(requestXml.ToString());
                TestStr = rStr;

               

                QryYoungInfoReturn qryYoungInfoReturn =  xMLExchange.AnalysisQryYoungMumberXML(rStr);

                cm.ASSESS_DATE = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.ASSESS_DATE;
                cm.CUST_ID = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.CUST_ID;
                cm.EFF_DATE = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.EFF_DATE;
                cm.EXP_DATE = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.EXP_DATE;
                cm.MEMBER_CODE = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.MEMBER_CODE;
                cm.MEMBER_ID = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.MEMBER_ID;
                cm.MEMBER_NAME = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.MEMBER_NAME;
                cm.MEMBERSHIP_LEVEL = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.MEMBERSHIP_LEVEL;
                cm.STATUS_CD = qryYoungInfoReturn.SvcCont.SOO.CLUB_MEMBER.STATUS_CD;
                Result = 0;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message.ToString();
                Result = -29999;           
            
            }
            finally
            {
                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);            
            }
            return Result;
        }

        /// <summary>
        /// 飞young客户认证v2
        /// </summary>
        /// <param name="UAProvinceID"></param>
        /// <param name="AuthenType"></param>
        /// <param name="UserName1"></param>
        /// <param name="RealName1"></param>
        /// <param name="PassWord"></param>
        /// <param name="PasswordFlag"></param>
        /// <param name="ScoreBesttoneSPID"></param>
        /// <param name="context"></param>
        /// <param name="RealName"></param>
        /// <param name="UserName1"></param>
        /// <param name="NickName"></param>
        /// <param name="OutID"></param>
        /// <param name="CustType"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="TestStr"></param>
        /// <returns></returns>
        public static int YoungUserAuthV2(string UAProvinceID, string Areaid, string AuthenType, string Username, string RealName1, string Password, string PasswdFlag, string ScoreBesttoneSPID, HttpContext context, out string RealName, out string UserName, out string NickName, out string OutID, out string CustType, out string CustID, out string PointType,out string PointValueSum,out string PointValue, out string ErrMsg, out string TestStr)
        {
            int Result = -19999;

            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            CustType = "";
            CustID = "";
            ErrMsg = "";
            TestStr = "";
            string str = "";
            string rStr = "";
            PointType = "";
            PointValueSum = "";
            PointValue = "";
            try
            {

                if (!String.IsNullOrEmpty(Areaid) && !Areaid.StartsWith("0"))
                {
                    Areaid = "0" + Areaid;
                }

                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS17006";                                    //业务功能编码
                string ServiceCode = "SVC11001";                                //接口协议编码
                string ServiceContractVer = "SVC1100120110525";                 //协议当前使用的版本号
                string ActionCode = "0";                                        //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //服务等级,处理的优先级
                string SrcOrgID = "100000";                                     //发起方机构代码
                string SrcSysID = "1000000020";                                 //发起方(系统/平台)编码
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //发起方(系统/平台)签名
                //落地方机构编码
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }
                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //请求时间，时间格式字符
                string InfoTypeID = "36";                                                       //资料类型代码
                //标识类型
                string CodeType = "";
                //标识类型编码装换
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //标识号码
                string CodeValue = "";
                //所属地代码
                string CityCode = "";
                //固话、小灵通、宽带的特殊处理
                if (AuthenType == "9" || AuthenType == "10" || AuthenType == "11")
                {
                    if (Username.IndexOf('-') > 0)
                    {
                        string[] alUsername = Username.Split('-');
                        CityCode = alUsername[0];
                        CodeValue = alUsername[1];
                    }
                    else
                    {
                        CityCode = Areaid;
                        CodeValue = Username;
                    }
                }
                else
                {
                    CityCode = Areaid;
                    CodeValue = Username;
                }

                //是否需填密码
                //string PasswdFlag = "1";
                //密码
                string CCPasswd = Password;

                //构建Crm查询xml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildYoungQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);

                BTUCenterInterfaceLog.CenterForBizTourLog("YoungUserAuthV2", new StringBuilder(str));

                //与Crm交互查询
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;

                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);

                QryCustInfoReturn qryCustInfoReturn = new QryCustInfoReturn();
                AuthenYoungInfoReturn authYoungReturn = xMLExchange.AnalysisAuthYoungInfoXML(rStr);
     
                if (String.IsNullOrEmpty(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName)  || String.IsNullOrEmpty(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup))
                {

                    //交易流水号
                    date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    if (count >= 9)
                        count = 0;
                    else
                        count = count + 1;
                    TransactionID = "1000000020" + date + count.ToString();

                    BusCode = "BUS17002";                                            //业务功能编码
                    ServiceCode = "SVC11001";                                        //接口协议编码
                    ServiceContractVer = "SVC1100120091002";                         //协议当前使用的版本号

                    InfoTypeID = "31";
                    str = xMLExchange.BuildYoungQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                    ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                    InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);

                    rStr = obj.exchange(str);
                    qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);
 
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustBrand = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustBrand;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityName;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceCode = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceCode;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceName;

                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CityCode = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CityCode;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeType = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeType;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum;
                    authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType;
                }
                if (authYoungReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    //客户类型转换
                    CustType = ConvertCustType(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //证件类型转换
                    string IdentType = ConvertIdentType(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //会员类型转换
                    string CustServiceLevel = ConvertCustServiceLevel(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);


                    PointType = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointType;
                    PointValueSum = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValueSum;
                    PointValue = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValue;

                    if (RealName1 != "")
                        RealName = RealName1;

                    //全国CRM用户注册到号百
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //客户类型
                        IdentType,
                        authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //未知
                        OutID,
                        ScoreBesttoneSPID,
                        authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                        out CustID,
                        out ErrMsg);

                    if (Result != 0)
                    {
                        return Result;
                    }
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", ScoreBesttoneSPID, "", "0", out ErrMsg);
                }
                else
                {
                    ErrMsg = "错误类型及编码:" + authYoungReturn.TcpCont.Response.RspType
                             + "应答代码:" + authYoungReturn.TcpCont.Response.RspCode
                             + "描述:" + authYoungReturn.TcpCont.Response.RspDesc;
                    int rspcode = -1;
                    try
                    {
                        rspcode = int.Parse(authYoungReturn.TcpCont.Response.RspCode);
                    }
                    catch
                    {
                        return rspcode;
                    }
                    return rspcode;
                }
            }
            catch (Exception ex1)
            {
                ErrMsg = ex1.Message.ToString();
                Result = -29999;
                //str = ErrMsg;
                BTUCenterInterfaceLog.CenterForBizTourLog("YoungUserAuthV2", new StringBuilder(ErrMsg));
            }
            finally
            {
                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);
            }
            return Result;
        }

        public static int YoungUserAuth(string UAProvinceID, string AuthenType, string Username, string RealName1, string Password, string PasswdFlag, string ScoreBesttoneSPID, HttpContext context, out string RealName, out string UserName, out string NickName, out string OutID, out string CustType, out string CustID, out string ErrMsg, out string TestStr)
        {
            int Result = -19999;

            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            CustType = "";
            CustID = "";
            ErrMsg = "";
            TestStr = "";
            string str = "";
            string rStr = "";

            try
            {
                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS17006";                                    //业务功能编码
                string ServiceCode = "SVC11001";                                //接口协议编码
                string ServiceContractVer = "SVC1100120110525";                 //协议当前使用的版本号
                string ActionCode = "0";                                        //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //服务等级,处理的优先级
                string SrcOrgID = "100000";                                     //发起方机构代码
                string SrcSysID = "1000000020";                                 //发起方(系统/平台)编码
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //发起方(系统/平台)签名
                //落地方机构编码
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }
                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }

              

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //请求时间，时间格式字符
                string InfoTypeID = "36";                                                       //资料类型代码
                //标识类型
                string CodeType = "";
                //标识类型编码装换
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //标识号码
                string CodeValue = "";
                //所属地代码
                string CityCode = "";
                //固话、小灵通、宽带的特殊处理
                if (AuthenType == "9" || AuthenType == "10" || AuthenType == "11")
                {
                    if (Username.IndexOf('-') > 0)
                    {
                        string[] alUsername = Username.Split('-');
                        CityCode = alUsername[0];
                        CodeValue = alUsername[1];
                    }
                    else
                    {
                        CityCode = "";
                        CodeValue = Username;
                    }
                }
                else
                {
                    CityCode = "";
                    CodeValue = Username;
                }

                //是否需填密码
                //string PasswdFlag = "1";
                //密码
                string CCPasswd = Password;

                //构建Crm查询xml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildYoungQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);

                //与Crm交互查询
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;

                //解析Crm返回客户信息
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

    
                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    //客户类型转换
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //证件类型转换
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //会员类型转换
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //全国CRM用户注册到号百
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //客户类型
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //未知
                        OutID,
                        ScoreBesttoneSPID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                        out CustID,
                        out ErrMsg);

                    if (Result != 0)
                    {
                        return Result;
                    }
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", ScoreBesttoneSPID, "", "0", out ErrMsg);
                }
                else
                {
                    ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
                    int rspcode = -1;
                    try
                    {
                        rspcode = int.Parse(qryCustInfoReturn.TcpCont.Response.RspCode);
                    }
                    catch
                    {
                        return rspcode;
                    }
                    return rspcode;
                }
            }
            catch (Exception ex1)
            {
                ErrMsg = ex1.Message.ToString();
                Result = -29999;
            }
            finally
            {
                LogCrmSSOPost(str);
                LogCrmSSOReturn(rStr);
            }
            return Result;
        }



        public static int UserAuthCrm1(string UAProvinceID,string AreaCode, string AuthenType, string Username, string RealName1, string Password, string PasswdFlag, string ScoreBesttoneSPID, HttpContext context, out string RealName, out string UserName, out string NickName, out string OutID, out string CustType, out string CustID, out string ErrMsg, out string TestStr)
        {
            int Result = -19999;

            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            CustType = "";
            CustID = "";
            ErrMsg = "";
            TestStr = "";
            string str = "";
            string rStr = "";

            try
            {
                if (ScoreBesttoneSPID == "")
                {
                    ScoreBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
                }

                string BusCode = "BUS17002";                                            //业务功能编码
                string ServiceCode = "SVC11001";                                        //接口协议编码
                string ServiceContractVer = "SVC1100120091002";                         //协议当前使用的版本号
                string ActionCode = "0";                                                //请求标识
                //交易流水号
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");               
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();

                string ServiceLevel = "1";                                              //服务等级,处理的优先级
                string SrcOrgID = "100000";                                             //发起方机构代码
                string SrcSysID = "1000000020";                                         //发起方(系统/平台)编码
                //发起方(系统/平台)签名
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];

                //落地方机构编码
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "指定省对应不到落地方机构编码";
                    return -1;
                }

                //落地方(系统/平台)编码
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "指定省对应不到落地方(系统/平台)编码";
                    return -1;
                }

                #region 隐藏不用
                //TestStr = "DstOrgID" + DstOrgID + "DstSysID" + DstSysID;
                //return -1;
                /*
                switch (UAProvinceID)
                {
                    case "02":
                        //落地方机构编码
                        DstOrgID = "600102";
                        //落地方(系统/平台)编码
                        DstSysID = "6001020001";
                        break;
                    case "19":
                        //落地方机构编码
                        DstOrgID = "600203";
                        //落地方(系统/平台)编码
                        DstSysID = "6002030001";
                        break;
                    case "05":
                        //落地方机构编码
                        DstOrgID = "609906";
                        //落地方(系统/平台)编码
                        DstSysID = "6099060001";
                        break;
                    case "08":
                        //落地方机构编码
                        DstOrgID = "609905";
                        //落地方(系统/平台)编码
                        DstSysID = "6099050001";
                        break;
                    case "30":
                        //落地方机构编码
                        DstOrgID = "600404";
                        //落地方(系统/平台)编码
                        DstSysID = "6004040001";
                        break;
                    case "20":
                        //落地方机构编码
                        DstOrgID = "600101";
                        //落地方(系统/平台)编码
                        DstSysID = "6001010001";
                        break;
                    default:
                        ErrMsg = "指定省不在枢纽中";
                        return -1;
                        break;
                }
                **/
                #endregion

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");               //请求时间，时间格式字符
                string InfoTypeID = "31";                                               //资料类型代码
                //标识类型
                string CodeType = "";
                //标识类型编码装换
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //标识号码
                string CodeValue = "";
                //所属地代码
                string CityCode = "";

                //固话
                if (AuthenType == "9" )
                {
                    //string[] alUsername = Username.Split('-');
                    CityCode = AreaCode;
                    //CodeValue = alUsername[1];
                    if (Username.StartsWith(CityCode))
                    {
                        //CodeValue =  Username.TrimStart(CityCode);
                        CodeValue = Username.Substring(CityCode.Length);
                    }
                    else 
                    {
                        CodeValue = UserName;
                    }
                }
                else
                {
                    //标识号码
                    CodeValue = Username;
                    //所属地代码
                    CityCode = "";
                }

                //是否需填密码
                //    string PasswdFlag = "1";
                //密码
                string CCPasswd = Password;

                //构建Crm查询xml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                
                //Crm查询
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;

                //解析Crm返回客户信息
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    //客户类型转换
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //证件类型转换
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //客户会员等级转换
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //全国CRM用户注册到号百
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        //"021",
                        CustType,//客户类型
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",//未知
                        OutID,
                        ScoreBesttoneSPID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                        out CustID,
                       out  ErrMsg);

                    if (Result != 0)
                    {
                        //err_code.InnerHtml = ErrMsg;
                        return Result;
                    }
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", ScoreBesttoneSPID, "", "0", out ErrMsg);

                }
                else
                {

                    ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                                         + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                                         + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;

                    //return -1;
                    int rspcode = -1;
                    try
                    {
                        rspcode = int.Parse(qryCustInfoReturn.TcpCont.Response.RspCode);
                    }
                    catch
                    {
                        return rspcode;
                    }
                    return rspcode;

                }
            }
            catch (Exception ex1)
            {
                ErrMsg = ex1.Message.ToString();
                Result = -29999;
            }
            finally
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("枢纽用户验证接口 发送报文" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("XML - " + str);
                BTUCenterInterfaceLog.CenterForCRM("CrmSSOPost", msg);
                StringBuilder msgResult = new StringBuilder();
                msgResult.Append("枢纽用户验证接口 处理结果" + DateTime.Now.ToString("u") + "\r\n");
                msgResult.Append("XML - " + rStr);
                msgResult.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForCRM("CrmSSOReturn", msgResult);
                #endregion
            }
            return Result;
        }

        #region 类型转换


        private static Int32 ConvertAuthenType(String authenType, out String AccountType, out String ErrMsg)
        {
            Int32 result = 0;
            AccountType = String.Empty;
            ErrMsg = String.Empty;
            switch (authenType)
            {
                case "9":
                    AccountType = "c2000001";
                    break;
                case "10":
                    AccountType = "2000003";
                    break;
                case "11":
                    AccountType = "c2000002";
                    break;
                case "7":
                    AccountType = "c2000004";
                    break;
                default:
                    ErrMsg = "认证类型错误";
                    result = -1;
                    break;
            }
            return result;
        
        }

        /// <summary>
        /// 标识类型转换
        /// </summary>
        /// <returns></returns>
        private static Int32 ConvertCodeType(String authenType,out String codeType,out String ErrMsg)
        {
            Int32 result = 0;
            ErrMsg = String.Empty;
            codeType = String.Empty;
            switch (authenType)
            {
                case "5":
                    codeType = "23";
                    break;
                case "7":
                    codeType = "11";
                    break;
                case "9":
                    codeType = "12";
                    break;
                case "11":
                    codeType = "13";
                    break;
                case "10":
                    codeType = "17";
                    break;
                case "99":
                    codeType = "15";
                    break;
                default:
                    ErrMsg = "认证类型错误";
                    result = -1;
                    break;
            }
            return result;
        }

        //10	政企客户 10
        //11	家庭客户 20
        //12	个人客户 30
        //99	其它客户 90
        /// <summary>
        /// 客户类型转换
        /// </summary>
        /// <param name="custGroup"></param>
        /// <returns></returns>
        private static String ConvertCustType(String custGroup)
        {
            String CustType = String.Empty;
            switch (custGroup)
            {
                case "10":
                    CustType = "10";
                    break;
                case "11":
                    CustType = "20";
                    break;
                case "12":
                    CustType = "30";
                    break;
                case "99":
                    CustType = "90";
                    break;
                default:
                    CustType = "90";
                    break;
            }
            return CustType;
        }

        /*0	  统一客户标识码 9
        *1	  身份证         0
        *2	  军官证         2   
        *3	  护照           3
        *4	  港澳台通行证   6
        *5	  部队干部离休证 9
        *6	  工商营业执照   9
        *7	  单位证明       9
        *9	  驾驶证         9
        *10	  学生证         9
        *11	  教师证         9
        *12	  户口本/居住证  9
        *13	  老人证         9
        *14	  士兵证         1
        *15	  组织机构代码证 9
        *17	  工作证         9
        *18	  暂住证         9
        *19	  电信识别编码   9
        *20	  集团客户标识码 9
        *21	  VIP卡          9
        *22	  警官证
        *99	  其它           9
        *
        0－身份证
        1－士兵证
        2－军官证
        3－护照
        4－保留
        5－台胞证
        6－港澳通行证
        7－国际海员证
        9－其它
        10-部队干部离休证
        11-工商营业执照
        12-单位证明
        13-驾驶证
        14-学生证
        15-教师证
        16-户口本/居住证
        17-老人证
        18-组织机构代码证
        19-工作证
        20-暂住证
        21-电信识别编码
        22-集团客户标识码
        23-VIP卡
        24-警官证
        */
        /// <summary>
        /// 证件类型转换
        /// </summary>
        /// <returns></returns>
        private static String ConvertIdentType(String identity)
        {
            String IdentType = String.Empty;
            switch (identity)
            {
                case "0":
                    IdentType = "25";
                    break;
                case "1":
                    IdentType = "0";
                    break;
                case "2":
                    IdentType = "2";
                    break;
                case "3":
                    IdentType = "3";
                    break;
                case "4":
                    IdentType = "6";
                    break;
                case "5":
                    IdentType = "10";
                    break;
                case "6":
                    IdentType = "11";
                    break;
                case "7":
                    IdentType = "12";
                    break;
                case "9":
                    IdentType = "13";
                    break;
                case "10":
                    IdentType = "14";
                    break;
                case "11":
                    IdentType = "15";
                    break;
                case "12":
                    IdentType = "16";
                    break;
                case "13":
                    IdentType = "17";
                    break;
                case "14":
                    IdentType = "1";
                    break;
                case "15":
                    IdentType = "18";
                    break;
                case "17":
                    IdentType = "19";
                    break;
                case "18":
                    IdentType = "20";
                    break;
                case "19":
                    IdentType = "21";
                    break;
                case "20":
                    IdentType = "22";
                    break;
                case "21":
                    IdentType = "23";
                    break;
                case "22":
                    IdentType = "24";
                    break;
                default:
                    IdentType = "9";
                    break;
            }

            return IdentType;
        }

        /** 10 非会员
            11 钻
            12 金
            13 银
            14 普通
            1：钻石卡会员
            2：金卡会员
            3：银卡会员
            4：普通会员（积分商城注册客户归属）
            5：非会员
        * */
        /// <summary>
        /// 客户会员等级转换
        /// </summary>
        /// <returns></returns>
        private static String ConvertCustServiceLevel(String custservicelevel)
        {
            String CustServiceLevel = String.Empty;
            switch (custservicelevel)
            {
                case "10":
                    CustServiceLevel = "5";
                    break;
                case "11":
                    CustServiceLevel = "1";
                    break;
                case "12":
                    CustServiceLevel = "2";
                    break;
                case "13":
                    CustServiceLevel = "3";
                    break;
                case "14":
                    CustServiceLevel = "4";
                    break;
                default:
                    CustServiceLevel = "5";
                    break;
            }
            return CustServiceLevel;
        }

        #endregion

        #region 日志

        /// <summary>
        /// POST到Crm报文日志
        /// </summary>
        /// <param name="msg"></param>
        private static void LogCrmSSOPost(String postXml)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("枢纽用户验证接口 发送报文" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("XML - " + postXml);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            BTUCenterInterfaceLog.CenterForCRM("CrmSSOPost", msg);
        }

        /// <summary>
        /// Crm返回的报文日志
        /// </summary>
        /// <param name="msg"></param>
        private static void LogCrmSSOReturn(String returnXml)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("枢纽用户验证接口 处理结果" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("XML - " + returnXml);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForCRM("CrmSSOReturn", msg);
        }

        #endregion

    }
        
}

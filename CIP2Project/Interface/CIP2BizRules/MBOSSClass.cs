/*****************************************************
 *   描述: 3.5 UA提供给客户信息平台的调用函数
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者:  仝 波
 * 联系方式: 
 *    公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 ****************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services.Protocols;

using System.Web;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;

namespace Linkage.BestTone.Interface.Rule 
{
    public class MBOSSClass
    {
        #region

        /// <param name="ServiceCode">接口协议编码</param>
        /// <param name="Version">协议当前使用的版本号</param>
        /// <param name="ActionCode">请求标识，为请求会话控制，值为0</param>
        /// <param name="TransactionID">交易流水号，发起方请求时生成并填写，该流水号全局唯一。【2位UA平台编码代码】+【3位业务平台编码代码】+【8位日期编码YYYYMMDD】＋【10位流水号】</param>
        /// <param name="SrcSysID">发起方(系统/平台)编码</param>
        /// <param name="DstSysID">落地方(系统/平台)编码,即目的地系统编码</param>
        /// <param name="ReqTime">请求时间，YYYYMMDDHHMMSS,发起方(系统/平台)用请求方的主机时间填写请求时间</param>
        /// <param name="SSQReqLists">扩展属性</param>
        
        public static string ServiceCode = "";              //System.Configuration.ConfigurationManager.AppSettings["ServiceCode"];
        public static string Version = "";                  //System.Configuration.ConfigurationManager.AppSettings["Version"];
        public static string ActionCode = "";               //System.Configuration.ConfigurationManager.AppSettings["ActionCode"];
        public static string TransactionID = "";            //System.Configuration.ConfigurationManager.AppSettings["TransactionID"];
        public static string SrcSysID = "";                 //System.Configuration.ConfigurationManager.AppSettings["SrcSysID"];
        public static string DstSysID = "";                 //System.Configuration.ConfigurationManager.AppSettings["DstSysID"];
        public static string ReqTime = "";                  //System.Configuration.ConfigurationManager.AppSettings["ReqTime"];

        //public static byte[] privateKeyFile = System.Configuration.ConfigurationManager.AppSettings["privateKeyFile"];
        //public static string publicKeyFile = System.Configuration.ConfigurationManager.AppSettings["publicKeyFile"];
        //public static string privateKeyPassword = System.Configuration.ConfigurationManager.AppSettings["privateKeyPassword"];

        public static SSQReqList[] SSQReqLists = GetSSQReqLists();

        private static SSQReqList[] GetSSQReqLists()
        {
            string SSQReqListsStr =  System.Configuration.ConfigurationManager.AppSettings["SSQReqLists"];
            string ReqTypeStr = System.Configuration.ConfigurationManager.AppSettings["ReqType"];
            string ReqCodeStr = System.Configuration.ConfigurationManager.AppSettings["ReqCode"];
            string ReqDescStr = System.Configuration.ConfigurationManager.AppSettings["ReqDesc"];
            int count = 0;
            SSQReqList[] SSQReqLists;
            string[] ReqTypeList;
            string[] ReqCodeList;
            string[] ReqDescList;
            try
            {
                count = int.Parse(SSQReqListsStr);

            }
            catch { count = 0; }

            ReqTypeList = ReqTypeStr.Split(';');
            ReqCodeList = ReqCodeStr.Split(';');
            ReqDescList = ReqDescStr.Split(';');

            SSQReqLists = new SSQReqList[count];

            for (int i = 0; i < count; i++)
            {
                SSQReqList SSQReq = new SSQReqList();
                SSQReq.ReqCode = ReqCodeList[i];
                SSQReq.ReqDesc = ReqDescList[i];
                SSQReq.ReqType = ReqTypeList[i];
                SSQReqLists[i] = SSQReq;

            }
            return SSQReqLists;

        }

        #endregion

        #region CAP01003 业务系统SSO认证函数,生成到SSO认证的xml字符串

        /// <summary>
        /// 可接收的帐号类型列表
        /// </summary>
        public class AcceptAccountTypeList
        {
            public string AcceptAccountType;
        }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public class SSQReqList
        {
            public string ReqType;
            public string ReqCode;
            public string ReqDesc;
        }

        /// <summary>
        /// CAP01003 业务系统SSO认证函数,生成到SSO认证的xml字符串
        /// </summary>  
        /// <param name="ProvinceID">ProvinceID</param>
        /// <param name="SPID">SPID</param>
        /// <param name="RedirectURL">重定向URL</param>
        /// <param name="AcceptAccountTypes">可接收的帐号类型列表</param>
        /// <param name="privateKeyFile">私钥</param>
        /// <param name="privateKeyPassword">私钥密码</param>
        /// <param name="ResultXML">返回的xml</param>
        /// <param name="ErrMsg">返回的信息</param>
        /// <returns>返回值</returns>
        public int SSOAuthanXML(string ProvinceID,string SPID,string RedirectURL, AcceptAccountTypeList[] AcceptAccountTypes, System.Web.HttpContext SpecificContext, string SPDataCacheName,
            out string ResultXML, out string ErrMsg, out string TransactionID)
        {
            ResultXML = "";
            ErrMsg = "";
            int Result = 0;
            TransactionID = "";
            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem;
            XmlElement xmlelem2;
            XmlElement xmlelem3;
            XmlElement xmlelem4;

            XmlElement xmlelem5;

            XmlText xmltext;

            SPInfoManager spInfo = new SPInfoManager();
            byte[] privateKeyFile;
            string UserName = "";
            string privateKeyPassword = "";
            try
            {
                Object SPData = spInfo.GetSPData(SpecificContext, SPDataCacheName);
                privateKeyFile = spInfo.GetCAInfo(SPID, 1, SPData, out UserName, out privateKeyPassword);
            }
            catch (Exception err)
            {
                Result = -20005;
                ErrMsg = err.Message;          
                return Result;
            }

            xmldoc = new XmlDocument();
            //加入XML的声明段落

            xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);
            //加入一个根元素
            xmlelem = xmldoc.CreateElement("", "CAPRoot", "");
            xmldoc.AppendChild(xmlelem);

            #region 会话控制
            ///////////////////////////////////////////
            xmlelem2 = xmldoc.CreateElement("SessionHeader");
            xmlelem2 = xmldoc.CreateElement("", "SessionHeader", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);


            xmlelem3 = xmldoc.CreateElement("ServiceCode");
            xmlelem3 = xmldoc.CreateElement("", "ServiceCode", "");
            xmltext = xmldoc.CreateTextNode("CAP01003");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("Version");
            xmlelem3 = xmldoc.CreateElement("", "Version", "");
            xmltext = xmldoc.CreateTextNode("mbossUacVersion1");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("ActionCode");
            xmlelem3 = xmldoc.CreateElement("", "ActionCode", "");
            xmltext = xmldoc.CreateTextNode("0");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            // 【2位UA平台编码代码】+【3位业务平台编码代码】+【8位日期编码】+【10位流水号】
            //TransactionID = "35000";
            //TransactionID += DateTime.Now.ToString("yyyyMMdd");
            //Random r = new Random(Guid.NewGuid().GetHashCode());
            //TransactionID += r.Next(10000000, 99999999).ToString();
            //r = new Random(Guid.NewGuid().GetHashCode());
            //TransactionID += r.Next(10, 99).ToString();
            TransactionID = "35000" + CommonBizRules.CreateTransactionID();

            //12392652948909910320090
            xmlelem3 = xmldoc.CreateElement("TransactionID");
            xmlelem3 = xmldoc.CreateElement("", "TransactionID", "");
            xmltext = xmldoc.CreateTextNode(TransactionID);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("SrcSysID");
            xmlelem3 = xmldoc.CreateElement("", "SrcSysID", "");
            xmltext = xmldoc.CreateTextNode("35000");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            //发起方(系统/平台)签名
            xmlelem3 = xmldoc.CreateElement("DigitalSign");
            xmlelem3 = xmldoc.CreateElement("", "DigitalSign", "");
            xmlelem2.AppendChild(xmlelem3);

            //落地方(系统/平台)编码
            xmlelem3 = xmldoc.CreateElement("DstSysID");
            xmlelem3 = xmldoc.CreateElement("", "DstSysID", "");
            xmltext = xmldoc.CreateTextNode(ProvinceID);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            ReqTime = DateTime.Now.ToString("yyyyMMddHHmmss") ;
            xmlelem3 = xmldoc.CreateElement("ReqTime");
            xmlelem3 = xmldoc.CreateElement("", "ReqTime", "");
            xmltext = xmldoc.CreateTextNode(ReqTime);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("Request");
            xmlelem3 = xmldoc.CreateElement("", "Request", "");
            xmlelem2.AppendChild(xmlelem3);
            if (SSQReqLists.Length == 0)
            {
                xmlelem4 = xmldoc.CreateElement("ReqType");
                xmlelem4 = xmldoc.CreateElement("", "ReqType", "");
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqCode");
                xmlelem4 = xmldoc.CreateElement("", "ReqCode", "");
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqDesc");
                xmlelem4 = xmldoc.CreateElement("", "ReqDesc", "");
                xmlelem3.AppendChild(xmlelem4);
            }

            for (int i = 0; i < SSQReqLists.Length; i++)
            {
                SSQReqList ssqReq = new SSQReqList();
                ssqReq = SSQReqLists[i];
                xmlelem4 = xmldoc.CreateElement("ReqType");
                xmlelem4 = xmldoc.CreateElement("", "ReqType", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqType);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqCode");
                xmlelem4 = xmldoc.CreateElement("", "ReqCode", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqCode);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqDesc");
                xmlelem4 = xmldoc.CreateElement("", "ReqDesc", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqDesc);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);
            }

            #endregion

            #region 业务控制
            //////////////////////////////////////

            xmlelem2 = xmldoc.CreateElement("SessionBody");
            xmlelem2 = xmldoc.CreateElement("", "SessionBody", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

            xmlelem3 = xmldoc.CreateElement("SPSSOAuthReq");
            xmlelem3 = xmldoc.CreateElement("", "SPSSOAuthReq", "");
            xmlelem2.AppendChild(xmlelem3);
            // xmldoc.ChildNodes.Item(2).AppendChild(xmlelem2);

            xmlelem4 = xmldoc.CreateElement("RedirectURL");
            xmlelem4 = xmldoc.CreateElement("", "RedirectURL", "");
            xmltext = xmldoc.CreateTextNode(RedirectURL);
            xmlelem4.AppendChild(xmltext);
            xmlelem3.AppendChild(xmlelem4);

            xmlelem4 = xmldoc.CreateElement("AcceptAccountTypeList");
            xmlelem4 = xmldoc.CreateElement("", "AcceptAccountTypeList", "");
            xmlelem3.AppendChild(xmlelem4);

            xmlelem5 = xmldoc.CreateElement("AcceptAccountType");
            xmlelem5 = xmldoc.CreateElement("", "AcceptAccountType", "");

            for (int i = 0; i < AcceptAccountTypes.Length; i++)
            {
                AcceptAccountTypeList AcceptAccountType = new AcceptAccountTypeList();
                AcceptAccountType = AcceptAccountTypes[i];
                xmlelem5 = xmldoc.CreateElement("AcceptAccountType");
                xmlelem5 = xmldoc.CreateElement("", "AcceptAccountType", "");
                xmltext = xmldoc.CreateTextNode(AcceptAccountType.AcceptAccountType);
                xmlelem5.AppendChild(xmltext);

                xmlelem4.AppendChild(xmlelem5);
            }
            #endregion

            ResultXML = xmldoc.OuterXml;
            ResultXML = ResultXML.Substring(ResultXML.IndexOf("<CAPRoot>"));
            ResultXML = ResultXML.Replace("<DigitalSign />", "<DigitalSign/>");
            
            Result = AddDigitalSignXML(ResultXML, privateKeyFile, privateKeyPassword, out ResultXML, out ErrMsg);

            return Result;
        }

        #endregion

        #region CAP02001 票据解读函数(即断言查询解析)

        /// <summary>
        /// UA系统的可接受的帐号列表
        /// </summary>
        public class AuthenRecord
        {
            public string AuthenType;
            public string AuthenName;
            public string areaid;
            public string ExtendField;
        }
        /// <summary>
        /// 断言
        /// </summary>
        public class BilByCompilingResult
        {
            public string Assertion;//断言标识
            public string UAID;//断言颁发的UA的标识
            public string UA_URL;//断言颁发UA的URL
            public string NotBefore;//断言使用时间，不在此时间之前
            public string NotOnOrAfter;//断言使用时间，不在此时间之后
            public string IssueInstant;//断言颁发时间
            public string AudienceID;//断言接受者标识
            public string AuthInstant;//认证时间
            public string AuthMethod;//认证方式
            public string AccountType;//帐号类型
            public string AccountID;//帐号标识
            public string ProvinceID;
            public AuthenRecord[] AccountInfos;//用户在业务系统中可接受的帐号列表

            public int Result;//返回值
            public string ErrMsg;//返回信息

        }
        /// <summary>
        /// 【貌似没用】用户在业务系统中可接受的帐号列表
        /// </summary>
        public class AccountListResult
        {
            public string AcccountType;//帐号类型
            public string AccountID;//帐号标识
           // public PWDAttrListResult[] PWDAttrLists;//密码属性列表
        }
        /// <summary>
        /// 密码属性列表
        /// </summary>
        public class PWDAttrListResult
        {
            public string PWDAttr;//密码属性
        }





        /// <summary>
        /// 根据ticket到省ua查询断言并进行解析
        /// </summary>
        /// <param name="UATicket"></param>
        /// <param name="privateKeyFile"></param>
        /// <param name="UATicket"></param>      
        /// <param name="PublicKeyFile"></param>
        /// <param name="UATicketXML"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int SendUATicket(string UAProvinceID,string SPID, string UATicket, string URL, System.Web.HttpContext SpecificContext, 
            string SPDataCacheName, string TransactionID, out BilByCompilingResult bbcResult, out string UATicketXML, out string ErrMsg)
        {
            UATicketXML = "";
            ErrMsg = "";
            int Result = -19999;
            bbcResult = new BilByCompilingResult();
            bbcResult.Result = -19999;

            StringBuilder strLog = new StringBuilder();

            #region
            byte[] privateKeyFile = new byte[0];
            string privateKeyPassword = "";
            string UserName = "";
            byte[] PublicKeyFile = new byte[0];
            SPInfoManager spInfo = new SPInfoManager();
            try
            {  
                Object SPData = spInfo.GetSPData(SpecificContext, SPDataCacheName);
                PublicKeyFile=spInfo.GetCAInfo(SPID, 0, SPData, out UserName, out privateKeyPassword);
                privateKeyFile = spInfo.GetCAInfo("35999991", 1, SPData, out UserName, out privateKeyPassword);

            }
            catch (Exception err)
            {
                ErrMsg = err.Message;
                Result = -20001;
             
                return Result;
            }
            #endregion

            try
            {
                //拼凑查询断言的xml
                Result = GetUATicketXML(UAProvinceID, UATicket, privateKeyFile, privateKeyPassword, TransactionID, out UATicketXML, out ErrMsg);
                //log("发送的:" + UATicketXML);
                strLog.AppendFormat("断言查询请求报文:{0}\r\n", UATicketXML);

                if (Result != 0)
                    return Result;

                /******************************************/

                string NewXML = "";

                try
                {
                    //断言查询
                    UaService u = new UaService();
                    u.Url = URL;// System.Configuration.ConfigurationManager.AppSettings["GetInfoByTicketURL"];
                    NewXML = u.SelectAssertion(UATicketXML);
                    //log("接受：" + NewXML);
                    strLog.AppendFormat("断言查询返回报文：{0}\r\n", NewXML);
                }
                catch (System.Exception ex)
                {
                    //log("错误：" + ex.Message);
                    strLog.AppendFormat("异常:{0}\r\n", ex.Message);
                }


                string DigitalSign = GetNewXML(NewXML, "DigitalSign");
                string OldXML = GetValueFromXML(NewXML, "DigitalSign");
                //验证
                Result = VerifySignByPublicKey(DigitalSign, PublicKeyFile, OldXML, out ErrMsg);
                //log("验证签名：" + Result + "==" + ErrMsg);
                strLog.AppendFormat("签名验证结果:{0},{1}\r\n", Result, ErrMsg);
                if (Result != 0)
                    return Result;

                //解析
                bbcResult = BilByCompiling(DigitalSign);
                ErrMsg = bbcResult.ErrMsg;
                Result = bbcResult.Result;
                if (Result != 0)
                    return Result;
            }
            catch (Exception err)
            {
                ErrMsg = err.Message;
                Result = -20009;
                log(Result + "--" + ErrMsg);
                log(err.StackTrace);
                return Result;
            }
            finally
            {
                log(strLog.ToString());
            }
           
            return Result;
        }

        /// <summary>
        /// 生成CAP02001查询XML
        /// </summary>
        /// <param name="UATicket">断言</param>       
        /// <param name="privateKeyFile">私钥</param>
        /// <param name="privateKeyPassword">私钥密码</param>
        /// <param name="ResultXML"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int GetUATicketXML(string UAProvinceID, string UATicket, byte[] privateKeyFile, string privateKeyPassword, string TransactionID, out string ResultXML, out string ErrMsg)
        {
            int Result = 0;
            ResultXML = "";
            ErrMsg = "";


            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem;
            XmlElement xmlelem2;
            XmlElement xmlelem3;
            XmlElement xmlelem4;
            XmlText xmltext;


            xmldoc = new XmlDocument();
            //加入XML的声明段落

            xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);
            //加入一个根元素
            xmlelem = xmldoc.CreateElement("", "CAPRoot", "");
            xmldoc.AppendChild(xmlelem);

            ///////////////////////////////////////////
            xmlelem2 = xmldoc.CreateElement("SessionHeader");
            xmlelem2 = xmldoc.CreateElement("", "SessionHeader", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);


            xmlelem3 = xmldoc.CreateElement("ServiceCode");
            xmlelem3 = xmldoc.CreateElement("", "ServiceCode", "");
            xmltext = xmldoc.CreateTextNode("CAP02001");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("Version");
            xmlelem3 = xmldoc.CreateElement("", "Version", "");
            xmltext = xmldoc.CreateTextNode("mbossUacVersion1");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("ActionCode");
            xmlelem3 = xmldoc.CreateElement("", "ActionCode", "");
            xmltext = xmldoc.CreateTextNode("0");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("TransactionID");
            xmlelem3 = xmldoc.CreateElement("", "TransactionID", "");
            xmltext = xmldoc.CreateTextNode(TransactionID);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("SrcSysID");
            xmlelem3 = xmldoc.CreateElement("", "SrcSysID", "");
            xmltext = xmldoc.CreateTextNode("35000");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);


            //发起方(系统/平台)签名
            xmlelem3 = xmldoc.CreateElement("DigitalSign");
            xmlelem3 = xmldoc.CreateElement("", "DigitalSign", "");
            xmlelem2.AppendChild(xmlelem3);


            //落地方(系统/平台)编码
            xmlelem3 = xmldoc.CreateElement("DstSysID");
            xmlelem3 = xmldoc.CreateElement("", "DstSysID", "");
            xmltext = xmldoc.CreateTextNode(UAProvinceID);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            ReqTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            xmlelem3 = xmldoc.CreateElement("ReqTime");
            xmlelem3 = xmldoc.CreateElement("", "ReqTime", "");
            xmltext = xmldoc.CreateTextNode(ReqTime);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);


            xmlelem3 = xmldoc.CreateElement("Request");
            xmlelem3 = xmldoc.CreateElement("", "Request", "");
            xmlelem2.AppendChild(xmlelem3);

            if (SSQReqLists.Length == 0)
            {
                //xmlelem3 = xmldoc.CreateElement("Request");
                //xmlelem3 = xmldoc.CreateElement("", "Request", "");
                //xmlelem2.AppendChild(xmlelem3);

                xmlelem4 = xmldoc.CreateElement("ReqType");
                xmlelem4 = xmldoc.CreateElement("", "ReqType", "");
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqCode");
                xmlelem4 = xmldoc.CreateElement("", "ReqCode", "");
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqDesc");
                xmlelem4 = xmldoc.CreateElement("", "ReqDesc", "");
                xmlelem3.AppendChild(xmlelem4);
            }

            for (int i = 0; i < SSQReqLists.Length; i++)
            {
                SSQReqList ssqReq = new SSQReqList();
                ssqReq = SSQReqLists[i];
                xmlelem4 = xmldoc.CreateElement("ReqType");
                xmlelem4 = xmldoc.CreateElement("", "ReqType", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqType);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqCode");
                xmlelem4 = xmldoc.CreateElement("", "ReqCode", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqCode);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqDesc");
                xmlelem4 = xmldoc.CreateElement("", "ReqDesc", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqDesc);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);
            }



            //////////////////////////////////////

            xmlelem2 = xmldoc.CreateElement("SessionBody");
            xmlelem2 = xmldoc.CreateElement("", "SessionBody", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

            xmlelem3 = xmldoc.CreateElement("AssertionQueryReq");
            xmlelem3 = xmldoc.CreateElement("", "AssertionQueryReq", "");
            xmlelem2.AppendChild(xmlelem3);
            // xmldoc.ChildNodes.Item(2).AppendChild(xmlelem2);

            xmlelem4 = xmldoc.CreateElement("Ticket");
            xmlelem4 = xmldoc.CreateElement("", "Ticket", "");
            xmltext = xmldoc.CreateTextNode(UATicket);
            xmlelem4.AppendChild(xmltext);
            xmlelem3.AppendChild(xmlelem4);

            ResultXML = xmldoc.OuterXml;
            ResultXML = ResultXML.Substring(ResultXML.IndexOf("<CAPRoot>"));
            // XML = @"<?xml version='1.0' encoding='gb2312' standalone='yes' ?>" + XML;
            //  XML = @"<?xml version='1.0' encoding='UTF-8' ?>" + XML;
            ResultXML = ResultXML.Replace("<DigitalSign />", "<DigitalSign/>");
            ResultXML = ResultXML.Replace(" />", "/>");
            Result = AddDigitalSignXML(ResultXML, privateKeyFile, privateKeyPassword, out ResultXML, out ErrMsg);

            return Result;
        }

        /// <summary>
        /// 断言解析
        /// </summary>
        /// <param name="UATicket">断言</param>       
        /// <returns>BilByCompilingResult</returns>
        public BilByCompilingResult BilByCompiling(string UATicketXML)
        {
            BilByCompilingResult Result = new BilByCompilingResult();
        //    string DigitalSign = GetValueFromXML(UATicketXML, "DigitalSign");

       //     Result.Result = VerifySignByPublicKey(UATicketXML, PublicKeyFile, DigitalSign, out Result.ErrMsg);

            Result.Assertion = GetValueFromXML(UATicketXML, "Assertion");
            Result.AccountID = GetValueFromXML(UATicketXML, "AccountID");
            Result.AccountType = GetValueFromXML(UATicketXML, "AccountType");
            Result.AudienceID = GetValueFromXML(UATicketXML, "AudienceID");
            Result.AuthInstant = GetValueFromXML(UATicketXML, "AuthInstant");
            Result.AuthMethod = GetValueFromXML(UATicketXML, "AuthMethod");
            Result.ProvinceID = GetValueFromXML(UATicketXML, "ProvinceID");
            Result.IssueInstant = GetValueFromXML(UATicketXML, "IssueInstant");
            Result.NotBefore = GetValueFromXML(UATicketXML, "NotBefore");
            Result.NotOnOrAfter = GetValueFromXML(UATicketXML, "NotOnOrAfter");
            Result.UA_URL = GetValueFromXML(UATicketXML, "UA_URL");
            Result.UAID = GetValueFromXML(UATicketXML, "UAID");

            AuthenRecord[] AccountInfos = GetAccountInfoFromXML(UATicketXML);
            Result.AccountInfos = AccountInfos;
       
            switch (Result.AccountType)
            {
                case "2000001":                   
                    Result.AccountType = "9";
                    break;
                case "2000002":                   
                    Result.AccountType = "11";
                    break;
                case "2000003":
                    Result.AccountType = "10";
                  
                    break;
                case "2000004":
                    Result.AccountType = "7";
                    break;  
                case "0000000":
                    Result.AccountType = "99";
                    break;
                case "0000001":
                    Result.AccountType = "5";
                    break;               
                default:
                    Result.AccountType = "-1";
                    break;
            }

         
         
            return Result;
        }

        /// <summary>
        /// 网厅返回给UA系统的可接受的帐号列表解析
        /// </summary>
        /// <param name="XmlInfo">返回的帐号列表（XML）</param>
        /// <returns></returns>
        public static AuthenRecord[] GetAccountInfoFromXML(string XmlInfo)
        {
            string XMLValue = "";
            XmlNodeList nodeList = null;
            XmlNodeList nodeList1 = null;
            AuthenRecord[] ais = new AuthenRecord[0];
            try
            {
                XmlDocument xmlReader = new XmlDocument();
                xmlReader.LoadXml(XmlInfo);

                nodeList = xmlReader.GetElementsByTagName("AccountList");
                nodeList1 = nodeList[0].SelectNodes("AccountInfo");
                ais = new AuthenRecord[nodeList1.Count];
                for (int i = 0; i < nodeList1.Count; i++)
                {
                    AuthenRecord ai = new AuthenRecord();
                    ai.AuthenName = nodeList1[0].SelectNodes("AccountID")[0].InnerText == null ? "" : nodeList1[i].SelectNodes("AccountID")[0].InnerText;
                    ai.AuthenType = nodeList1[0].SelectNodes("AccountType")[0].InnerText == null ? "" : nodeList1[i].SelectNodes("AccountType")[0].InnerText;

                    try
                    {
                        ai.areaid = "";
                        XmlNodeList node3 = nodeList1[i].SelectNodes("PWDAttrList/PWDAttr");
                        for (int r = 0; r < node3.Count; r++)
                        {
                            if (node3[r].SelectNodes("AttrName")[0].InnerText == "CityCode")
                            {
                                ai.areaid = node3[r].SelectNodes("AttrValue")[0].InnerText == null ? "" : node3[r].SelectNodes("AttrValue")[0].InnerText;
                            }
                        }
                    }
                    catch
                    {
                        ai.areaid = "";
                    }

                    ai.ExtendField = "";
                    switch (ai.AuthenType)
                    {
                        //case "2000001":
                        //    ai.AuthenType = "9";
                        //    break;
                        //case "2000002":
                        //    ai.AuthenType = "11";
                        //    break;
                        //case "2000003":
                        //    ai.AuthenType = "10";
                        //    break;
                        //case "2000004":
                        //    ai.AuthenType = "7";
                        //    break;
                        case "0000000":
                            ai.AuthenType = "0";
                            break;
                        default:
                            ai.AuthenType = "-1";
                            break;
                    }

                    ais[i] = ai;
                }
            }
            catch (Exception)
            { ais = new AuthenRecord[0]; }

            return ais;
        }

        #endregion

        #region CAP03001 SSO认证地址查询接口

        public class SSOAddressResp
        {
            public string SSOAddress;
            public string AssertionAddress;
        }

        public int AuthenSelectArddess(string SPID,string ProvinceID, System.Web.HttpContext SpecificContext, string SPDataCacheName, out  SSOAddressResp SSOAddress, out string ErrMsg)
        {
            int Result = 0;
            string ResultXML = "";
            ErrMsg = "";
            SSOAddress = new SSOAddressResp();
            SSOAddress.AssertionAddress = "";
            SSOAddress.SSOAddress = "";
            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem;
            XmlElement xmlelem2;
            XmlElement xmlelem3;
            XmlElement xmlelem4;

            XmlText xmltext;

            xmldoc = new XmlDocument();
            //加入XML的声明段落

            #region 
            byte[] privateKeyFile=new byte[0];
            string privateKeyPassword = "";
            string UserName = "";
            SPInfoManager spInfo = new SPInfoManager();
            try
            {
              Object SPData = spInfo.GetSPData(SpecificContext, SPDataCacheName);
              privateKeyFile = spInfo.GetCAInfo(SPID, 1, SPData, out UserName, out privateKeyPassword);
            }catch(Exception err)
            {
              ErrMsg = err.Message;
              Result = -20001;
            }
            #endregion

            #region 生成xml
            xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);
            //加入一个根元素
            xmlelem = xmldoc.CreateElement("", "CAPRoot", "");
            xmldoc.AppendChild(xmlelem);

            #region 会话控制
            ///////////////////////////////////////////
            xmlelem2 = xmldoc.CreateElement("SessionHeader");
            xmlelem2 = xmldoc.CreateElement("", "SessionHeader", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);


            xmlelem3 = xmldoc.CreateElement("ServiceCode");
            xmlelem3 = xmldoc.CreateElement("", "ServiceCode", "");
            xmltext = xmldoc.CreateTextNode(ServiceCode);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("Version");
            xmlelem3 = xmldoc.CreateElement("", "Version", "");
            xmltext = xmldoc.CreateTextNode(Version);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("ActionCode");
            xmlelem3 = xmldoc.CreateElement("", "ActionCode", "");
            xmltext = xmldoc.CreateTextNode(ActionCode);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);


            xmlelem3 = xmldoc.CreateElement("TransactionID");
            xmlelem3 = xmldoc.CreateElement("", "TransactionID", "");
            xmltext = xmldoc.CreateTextNode(TransactionID);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("SrcSysID");
            xmlelem3 = xmldoc.CreateElement("", "SrcSysID", "");
            xmltext = xmldoc.CreateTextNode(SrcSysID);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);


            //发起方(系统/平台)签名
            xmlelem3 = xmldoc.CreateElement("DigitalSign");
            xmlelem3 = xmldoc.CreateElement("", "DigitalSign", "");
            xmlelem2.AppendChild(xmlelem3);


            //落地方(系统/平台)编码
            xmlelem3 = xmldoc.CreateElement("DstSysID");
            xmlelem3 = xmldoc.CreateElement("", "DstSysID", "");
            xmltext = xmldoc.CreateTextNode(DstSysID);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);

            xmlelem3 = xmldoc.CreateElement("ReqTime");
            xmlelem3 = xmldoc.CreateElement("", "ReqTime", "");
            xmltext = xmldoc.CreateTextNode(ReqTime);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);


            xmlelem3 = xmldoc.CreateElement("Request");
            xmlelem3 = xmldoc.CreateElement("", "Request", "");
            xmlelem2.AppendChild(xmlelem3);

            if (SSQReqLists.Length == 0)
            {
                xmlelem3 = xmldoc.CreateElement("Request");
                xmlelem3 = xmldoc.CreateElement("", "Request", "");
                xmlelem2.AppendChild(xmlelem3);

                xmlelem4 = xmldoc.CreateElement("ReqType");
                xmlelem4 = xmldoc.CreateElement("", "ReqType", "");
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqCode");
                xmlelem4 = xmldoc.CreateElement("", "ReqCode", "");
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqDesc");
                xmlelem4 = xmldoc.CreateElement("", "ReqDesc", "");
                xmlelem3.AppendChild(xmlelem4);
            }

            for (int i = 0; i < SSQReqLists.Length; i++)
            {
                SSQReqList ssqReq = new SSQReqList();
                ssqReq = SSQReqLists[i];
                xmlelem4 = xmldoc.CreateElement("ReqType");
                xmlelem4 = xmldoc.CreateElement("", "ReqType", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqType);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqCode");
                xmlelem4 = xmldoc.CreateElement("", "ReqCode", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqCode);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);

                xmlelem4 = xmldoc.CreateElement("ReqDesc");
                xmlelem4 = xmldoc.CreateElement("", "ReqDesc", "");
                xmltext = xmldoc.CreateTextNode(ssqReq.ReqDesc);
                xmlelem4.AppendChild(xmltext);
                xmlelem3.AppendChild(xmlelem4);
            }
            #endregion

            #region 业务控制
            //////////////////////////////////////

            xmlelem2 = xmldoc.CreateElement("SessionBody");
            xmlelem2 = xmldoc.CreateElement("", "SessionBody", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

            xmlelem3 = xmldoc.CreateElement("SPSSOAuthReq");
            xmlelem3 = xmldoc.CreateElement("", "SPSSOAuthReq", "");
            xmlelem2.AppendChild(xmlelem3);
            // xmldoc.ChildNodes.Item(2).AppendChild(xmlelem2);

            xmlelem4 = xmldoc.CreateElement("ProvinceID");
            xmlelem4 = xmldoc.CreateElement("", "ProvinceID", "");
            xmltext = xmldoc.CreateTextNode(ProvinceID);
            xmlelem4.AppendChild(xmltext);
            xmlelem3.AppendChild(xmlelem4);

            #endregion

            ResultXML = xmldoc.OuterXml;
            ResultXML = ResultXML.Substring(ResultXML.IndexOf("<CAPRoot>"));
            ResultXML = ResultXML.Replace("<DigitalSign />", "<DigitalSign/>");

            Result = AddDigitalSignXML(ResultXML, privateKeyFile, privateKeyPassword, out ResultXML, out ErrMsg);

            #endregion

            try
            {

                #region  发送到客户端
                UaService u = new UaService();
                u.Url = System.Configuration.ConfigurationManager.AppSettings["GetInfoByTicketURL"];
                string req = "";
                req = u.authReq(ResultXML);
                #endregion

                #region
                SSOAddress.SSOAddress = GetValueFromXML(req, "SSOAddress") == null ? "" : GetValueFromXML(req, "SSOAddress");
                SSOAddress.AssertionAddress = GetValueFromXML(req, "AssertionAddress") == null ? "" : GetValueFromXML(req, "AssertionAddress");
                #endregion

            }
            catch (Exception err)
            {
                ErrMsg = err.Message;
                Result = -20001;
            }
            finally
            {
                #region 
                #endregion
            }

            return Result;
        }

        #endregion     

        /// <summary>
        ///  获取省uam证书密钥
        /// </summary>
        /// <param name="SpecificContext"></param>
        /// <param name="SPID"></param>
        /// <param name="SecretKey"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int GetMBOSSSecretKey(HttpContext SpecificContext, string SPID, out string SecretKey, out string ErrMsg)
        {
            int Result = -19999;
            SecretKey = String.Empty;
            ErrMsg = String.Empty;
            try
            {
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(SpecificContext, "SPData");
                SecretKey = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                Result = 0;
            }
            catch (Exception e)
            {
                Result = -19999;
                SecretKey = "";
                ErrMsg = e.Message;
            }
            return Result;
        }


        /// <summary>
        /// 获取省SSO认证和断言查询地址(从数据库查询)
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="AssertionAddress">省级UA的断言查询地址</param>
        /// <param name="SSOAddress">省级UA的SSO接入地址</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int GetMBOSSAddress(HttpContext SpecificContext,string SPID, out string AssertionAddress, out string SSOAddress,out string ErrMsg)
        {
            int Result = -19999;
            AssertionAddress = "";
            SSOAddress = "";
            ErrMsg = "";
            try
            {
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(SpecificContext, "SPData");
               
                AssertionAddress = spInfo.GetPropertyBySPID(SPID, "interfaceUrlV2", SPData);
               
                SSOAddress = spInfo.GetPropertyBySPID(SPID, "InterfaceUrl", SPData);
              
                Result = 0;

                #region 隐藏不用

                /*
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_MbossAddress";


                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parAssertionAddress = new SqlParameter("@AssertionAddress ", SqlDbType.VarChar, 512);
                parAssertionAddress.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAssertionAddress);

                SqlParameter parSSOAddress = new SqlParameter("@SSOAddress ", SqlDbType.VarChar, 512);
                parSSOAddress.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSSOAddress);



                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                SSOAddress = parSSOAddress.Value.ToString();
                AssertionAddress = parAssertionAddress.Value.ToString();
                //CustID = parCustID.Value.ToString();  
                 */
                #endregion

            }
            catch(Exception err){
                Result = -19999;
                AssertionAddress = "";
                SSOAddress = "";
                ErrMsg =err.Message ;
            }

            return Result;

        }

        public static void CIPTicketHistory(string UATicket,string CustID,string ErrMsg)
        {
            #region 入库
           
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_ReadUATicket";

                SqlParameter parTicket = new SqlParameter("@Ticket", SqlDbType.VarChar, 20);
                parTicket.Value = UATicket;
                cmd.Parameters.Add(parTicket);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parDescription = new SqlParameter("@Description", SqlDbType.VarChar, 40);
                parDescription.Value = ErrMsg;
                cmd.Parameters.Add(parDescription);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                 
            }
            catch { }
            #endregion
        }

        public static void log(string str)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r");
            msg.Append(str);
            BTUCenterInterfaceLog.CenterForBizTourLog("SSO", msg);
        }

        #region 私有函数

        /// <summary>
        /// 公钥验证登陆函数
        /// </summary>
        /// <param name="OriginalStr">原始数据</param>
        /// <param name="PublicKeyFile">公钥</param>
        /// <param name="SignStr">签名</param>
        /// <param name="ErrMsg">返回信息</param>
        /// <returns>返回值</returns>
        public  int VerifySignByPublicKey(string OriginalStr, byte[] PublicKeyFile, string SignStr, out string ErrMsg)
        {
            int Result = -19999;

            ErrMsg = "";

            try
            {
                //进行der解码
                SignStr = DERDecode(SignStr);
                //-----------------------------------------------------------------------------------------

                X509Certificate2 myX509Certificate2 = new X509Certificate2(PublicKeyFile);
                
                DSACryptoServiceProvider dsa = (DSACryptoServiceProvider)myX509Certificate2.PublicKey.Key;
                byte[] OriginalByte = System.Text.Encoding.Default.GetBytes(OriginalStr);
                byte[] SignByte = fromHexString(SignStr);
                //  null ? GET('name') : '';

                Result = dsa.VerifyData(OriginalByte, SignByte) == true ? 0 : -29999;

            }
            catch (Exception err)
            {
                ErrMsg = err.Message;
                Result = -19999;
            }
            return Result;
        }

        /// <summary>
        /// 私钥验证登陆函数
        /// </summary>
        /// <param name="OriginalStr">原始数据</param>
        /// <param name="privateKeyFile">私钥</param>
        /// <param name="privateKeyPassword">私钥密码</param>
        /// <param name="ResultStr">返回数据</param>
        /// <param name="ErrMsg">返回信息</param>
        /// <returns>返回值</returns>
        private int DigSignByprivateKey(string OriginalStr, byte[] privateKeyFile, string privateKeyPassword, out string ResultStr, out string ErrMsg)
        {
            int Result = -19999;
            ErrMsg = "";
            ResultStr = "";
            try
            {
                // X509Certificate2 x509 = new X509Certificate2(privateKeyFile, privateKeyPassword);
                X509Certificate2 x509 = new X509Certificate2(privateKeyFile, privateKeyPassword);
                DSACryptoServiceProvider dsa = (DSACryptoServiceProvider)x509.PrivateKey;
                log(privateKeyPassword);
                byte[] OriginalByte = System.Text.Encoding.UTF8.GetBytes(OriginalStr);
                log(OriginalStr);
                byte[] s = dsa.SignData(OriginalByte);

                //der编码
                ResultStr = DEREncode(s);
                log(ResultStr + "$$$$$");
                Result = 0;
            }

            catch (Exception e)
            {
                ErrMsg = e.Message;
                log(ErrMsg);
                Result = -19999;
            }

            return Result;
        }

        /// <summary> 获取认证方式xml字符串
        /// 获取认证方式xml字符串
        /// </summary>
        /// <param name="DeleteUserAccountRecords"></param>
        /// <returns></returns>
        public static string GenerateXmlForAuthenRecords(MBOSSClass.AuthenRecord[] AuthenRecords)
        {
            string Result = "";

            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem;
            XmlElement xmlelem2;
            XmlElement xmlelem3;
            XmlText xmltext;
            try
            {
                xmldoc = new XmlDocument();
                //加入XML的声明段落
                xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);
                //加入一个根元素
                xmlelem = xmldoc.CreateElement("", "ROOT", "");
                xmldoc.AppendChild(xmlelem);

                for (int i = 0; i < AuthenRecords.Length; i++)
                {
                    //加入另外一个元素
                    xmlelem2 = xmldoc.CreateElement("AuthenRecord");
                    xmlelem2 = xmldoc.CreateElement("", "AuthenRecord", "");

                    xmlelem3 = xmldoc.CreateElement("", "AuthenType", "");
                    xmltext = xmldoc.CreateTextNode(AuthenRecords[i].AuthenType.ToString());
                    xmlelem3.AppendChild(xmltext);
                    xmlelem2.AppendChild(xmlelem3);

                    xmlelem3 = xmldoc.CreateElement("", "AuthenName", "");
                    xmltext = xmldoc.CreateTextNode(AuthenRecords[i].AuthenName.ToString());
                    xmlelem3.AppendChild(xmltext);
                    xmlelem2.AppendChild(xmlelem3);

                    xmlelem3 = xmldoc.CreateElement("", "areaid", "");
                    xmltext = xmldoc.CreateTextNode(AuthenRecords[i].areaid .ToString());
                    xmlelem3.AppendChild(xmltext);
                    xmlelem2.AppendChild(xmlelem3);

                    xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
                }
                //保存创建好的XML文档

                // xmldoc.Save(@".\DeleteUserAccountRecord.xml");
                Result = xmldoc.OuterXml;

            }
            catch
            { }

            return Result;

        }
        
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="XmlInfo"></param>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static string GetValueFromXML(string XmlInfo, string NodeName)
        {
            string nodeValue = "";

            try
            {
                XmlDocument xmlReader = new XmlDocument();
                xmlReader.LoadXml(XmlInfo);

                XmlNodeList nodeList = null;

                nodeList = xmlReader.GetElementsByTagName(NodeName);
                nodeValue = (nodeList.Count != 0) ? nodeList[0].InnerText : "";
            }
            catch (Exception)
            {
                nodeValue = "";
            }

            return nodeValue;
        }

        /// <summary>
        /// 更新DigitalSign
        /// </summary>
        /// <param name="XmlInfo"></param>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static string GetNewXML(string XmlInfo, string NodeName)
        {
            string nodeValue = "";

            try
            {
                XmlDocument xmlReader = new XmlDocument();
                xmlReader.LoadXml(XmlInfo);

                XmlNodeList nodeList = null;

                nodeList = xmlReader.GetElementsByTagName(NodeName);

                if (nodeList.Count > 0)
                {
                    XmlNode node = nodeList[0];
                    node.InnerText = "";
                    nodeValue = xmlReader.OuterXml;
                }

                nodeValue = nodeValue.Replace("<DigitalSign></DigitalSign>", "<DigitalSign/>");
                nodeValue = nodeValue.Replace(" />", "/>");
                nodeValue = nodeValue.Replace("> ", ">");
                return nodeValue;
            }
            catch (Exception)
            {
                nodeValue = "";
            }

            return nodeValue;
        }

        /// <summary>
        /// 对原XML进行加密，并写到DigitalSign节点
        /// </summary>
        /// <param name="OldXML">原XML</param>
        /// <param name="privateKeyFile">私钥</param>
        /// <param name="privateKeyPassword">私钥密码</param>
        /// <param name="ResultXML">返回加密过的xml</param>
        /// <param name="ErrMsg">返回信息</param>
        /// <returns></returns>
        public  static int AddDigitalSignXML(string OldXML, byte[] privateKeyFile, string privateKeyPassword, out string ResultXML, out string ErrMsg)
        {
            ErrMsg = "";
            ResultXML = "";
            string NewXML = "";
            int Result = -19999;
            MBOSSClass mboss = new MBOSSClass();
            try
            {
                //进行私钥加密
                Result = mboss.DigSignByprivateKey(OldXML, privateKeyFile, privateKeyPassword, out NewXML, out  ErrMsg);
               
                if (Result != 0)
                    return Result;
               
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(OldXML);
                //修改XML，将加密过的xml写到原DigitalSign节点
                //根据节点名称找个所在的节点
                //XmlNode DigitalSignNode = doc.SelectSingleNode("descendant::CAPRoot/SessionHeader/DigitalSign");
                XmlNode DigitalSignNode = doc.SelectSingleNode("/CAPRoot/SessionHeader/DigitalSign");


                //将加密过的xml写到原DigitalSign节点
                DigitalSignNode.InnerText = NewXML;
               
                ResultXML = doc.OuterXml;

                ResultXML = ResultXML.Replace("<DigitalSign></DigitalSign>", "<DigitalSign/>");
                ResultXML = ResultXML.Replace(" />", "/>");
                Result = 0;
                
            }
            catch (Exception err)
            {
                Result = -29999;
                ErrMsg = err.Message;
            }

            return Result;
        }
        
        #endregion

        #region 编码

        /** DER编码规则
            DSA签名产生得到的是r和s两个整数。对于DSAWithSHA1来说，这两个整数都不会超过20字节。由于DSA签名需要使用一个随机数，所以每次签名的结果是不同的。

            JAVA产生的DSAWithSHA1的签名结果是个下面ASN.1结构的DER编码：
            Dss-Sig-Value  ::=  SEQUENCE  {
                          r       INTEGER,
                          s       INTEGER  }

            .NET要求的签名结果40字节，前面20字节是r，后面20字节是s，采用大端字节序。

            DER编码采用TLV的形式，每种类型有个Tag的编码，是为T部分的编码，接着是长度的编码，是为L部分的编码，最后是内容的编码，是为V部分的编码。
            对于SEQUENCE来说，Tag的编码为0x30，其内容编码就是其容纳的ASN.1对象的逐个TLV编码拼起来。
            对于INTEGER来说，Tag的编码为0x02，其内容编码就是使用最小的字节进行的补码形式。前9位不能全为1或者全为0。最高位为0表示正数，最高位为1表示负数。由于这里都是正数，所以如果最高位为1的话，前面要补0，也就是说最长的整数内容部分的编码长度为20+1=21字节。
            长度的编码对于不超过127的来说，只占有一个字节，编码就是值本身。这里就是这种情况。

            例子说明：
            Java的签名
            30 2C
               02 14
                  1C FA 3A BB 5C 0F 37 FB D7 74 CB 51 E5 64 B5 76 B9 26 4F 7A 
               02 14 
                  62 7E FF 2D 11 4B 2D D3 27 F3 8B 30 D1 6D B4 D2 78 5A 72 5B

            30表示是SEQUENCE
            2C表示后面编码有0x2c字节
            02表示是INTEGER
            14表示后面编码有0x14字节
            1C FA 3A BB 5C 0F 37 FB D7 74 CB 51 E5 64 B5 76 B9 26 4F 7A这部分就是r内容的编码
            这里r内容的编码刚好是20自己，直接拷贝即可。如果小于20字节，则前面补0到20字节。如果是21字节，则去掉最前面的1个字节，这个字节应该是0，表示r是正数；如果这个字节不是0则签名是错的。
            02 表示是INTEGER
            14表示后面编码有0x14字节
            62 7E FF 2D 11 4B 2D D3 27 F3 8B 30 D1 6D B4 D2 78 5A 72 5B这部分是s内容的编码。处理方式和r内容的编码一样。

            最后转换得到.NET的签名为
            1C FA 3A BB 5C 0F 37 FB D7 74 CB 51 E5 64 B5 76 B9 26 4F 7A 62 7E FF 2D 11 4B 2D D3 27 F3 8B 30 D1 6D B4 D2 78 5A 72 5B
        * */

        /// <summary>
        /// 对der进行解码
        /// </summary>
        /// <param name="DERStr"></param>
        /// <returns></returns>
        private static string DERDecode(string DERStr)
        {
            string Result = "";
            string R = "";
            string S = "";
            byte[] RByte = new byte[20];
            byte[] SByte = new byte[20];
            byte[] Content = fromHexString(DERStr);

            //获取整数R字段的长度
            string IntRStr = Content[3].ToString();
            ////从16进制转为10进制
            //uint x = uint.Parse(IntRStr, System.Globalization.NumberStyles.AllowHexSpecifier);   
            int IntRLength = int.Parse(IntRStr);

            //标识S 段的起始位
            int j = 0;
            //取出R
            if (IntRLength == 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    RByte[i] = Content[4 + i];
                }

                j = 24;
            }
            else if (IntRLength > 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    RByte[i] = Content[5 + i];
                }
                j = 25;
            }
            else
            {
                //补足不足20位的字节
                int NotEnough = 20 - IntRLength;
                for (int i = 0; i < NotEnough; i++)
                {
                    byte[] tmp = new byte[] { 00 };
                    RByte[i] = tmp[0];
                }

                for (int i = 0; i < NotEnough; i++)
                {
                    RByte[NotEnough + i] = Content[4 + i];
                }
                j = 24 - IntRLength;
            }

            //获取s整数

            int IntSLength = int.Parse(Content[j + 1].ToString());

            //取出R
            if (IntSLength == 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    SByte[i] = Content[j + 2 + i];
                }
            }
            else if (IntSLength > 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    SByte[i] = Content[j + 3 + i];
                }
            }
            else
            {
                //补足不足20位的字节
                int NotEnough = 20 - IntSLength;
                for (int i = 0; i < NotEnough; i++)
                {
                    byte[] tmp = new byte[] { 00 };
                    SByte[i] = tmp[0];
                }

                for (int i = 0; i < IntSLength; i++)
                {
                    SByte[NotEnough + i] = Content[j + 2 + i];
                }
            }

            R = toHexString(RByte);
            S = toHexString(SByte);
            Result = R + S;
            return Result;
        }

        /// <summary>
        /// 进行DER编码
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string DEREncode(byte[] b)
        {
            string str = toHexString(b);
            string newstr1 = str.Substring(0, 40);
            string newstr2 = str.Substring(40, 40);
            string newstr = "302C0214" + newstr1 + "0214" + newstr2;
            return newstr;
        }

        /// <summary>
        /// 转成16进制
        /// </summary>
        /// <param name="digestByte"></param>
        /// <returns></returns>
        public static byte[] toHex(byte[] digestByte)
        {
            byte[] rtChar = new byte[digestByte.Length * 2];
            for (int i = 0; i < digestByte.Length; i++)
            {
                byte b1 = (byte)(digestByte[i] >> 4 & 0x0f);
                byte b2 = (byte)(digestByte[i] & 0x0f);
                rtChar[i * 2] = (byte)(b1 < 10 ? b1 + 48 : b1 + 55);
                rtChar[i * 2 + 1] = (byte)(b2 < 10 ? b2 + 48 : b2 + 55);
            }
            return rtChar;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public static String toHexString(byte[] digestByte)
        {
            string Result = "";
            Result = byteToHexStr(digestByte);
            return Result;
        }

        public static byte[] fromHex(byte[] sc)
        {
            byte[] res = new byte[sc.Length / 2];
            for (int i = 0; i < sc.Length; i++)
            {
                byte c1 = (byte)(sc[i] - 48 < 17 ? sc[i] - 48 : sc[i] - 55);
                i++;
                byte c2 = (byte)(sc[i] - 48 < 17 ? sc[i] - 48 : sc[i] - 55);
                res[i / 2] = (byte)(c1 * 16 + c2);
            }
            return res;
        }

        public static byte[] fromHexString(String hex)
        {
            return fromHex(System.Text.Encoding.Default.GetBytes(hex));
        }

        #endregion

    }
}




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
    /// Crm��ѯ��֤
    /// </summary>
    public class CrmSSO
    {
        public static int count = 0;
        private static readonly ILog logger = LogManager.GetLogger(typeof(CrmSSO));

        /// <summary>
        /// ���� lihongtu
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

                string BusCode = "BUS37059";                                    //ҵ���ܱ���
                string ServiceCode = "SVC33051";                                //�ӿ�Э�����
                string ServiceContractVer = "SVC3305120130509";                 //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                        //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                     //���𷽻�������
                string SrcSysID = "1000000020";                                 //����(ϵͳ/ƽ̨)����
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //����(ϵͳ/ƽ̨)ǩ��
                //��ط���������
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }
                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }
                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //����ʱ�䣬ʱ���ʽ�ַ�
             
                //��ʶ����
                string AccountType = "";
                //��ʶ���ͱ���װ��
                Result = ConvertAuthenType(AuthenType, out AccountType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //��ʶ����
                string AccountID = "";
                //�����ش���
                string CityCode = "";
                //�̻�(9)��С��ͨ(10)����������⴦��(11)���ֻ�(7)
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

                //����
                string CCPasswd = Password;

                //����Uam��ѯxml
                XMLExchange xMLExchange = new XMLExchange();

                str = xMLExchange.BuildUamAuthenXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID, ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime, AccountType, AccountID, PWDType, Password, UAProvinceID);


                logger.Info("����crm-uam��Ŧ����:");
                logger.Info(str);

                BTUCenter.CSB.Proxy.IDEPService obj = new BTUCenter.CSB.Proxy.IDEPService();
                //DEPService obj = new DEPService();
                //obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEP-CSBServiceURL"];
 
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm-uam��Ŧ���ر���:");
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

                string BusCode = "BUS17002";                                    //ҵ���ܱ���
                string ServiceCode = "SVC11001";                                //�ӿ�Э�����
                string ServiceContractVer = "SVC1100120091002";                 //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                        //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");       
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                     //���𷽻�������
                string SrcSysID = "1000000020";                                 //����(ϵͳ/ƽ̨)����
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //����(ϵͳ/ƽ̨)ǩ��
                //��ط���������
                string DstOrgID = "";                                           
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }
                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }

                #region ���ز���
                /*
                switch (UAProvinceID)
                {
                    case "02":
                        //��ط���������
                        DstOrgID = "600102";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6001020001";
                        break;
                    case "19":
                        //��ط���������
                        DstOrgID = "600203";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6002030001";
                        break;
                    case "05":
                        //��ط���������
                        DstOrgID = "609906";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6099060001";
                        break;
                    case "08":
                        //��ط���������
                        DstOrgID = "609905";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6099050001";
                        break;
                    case "30":
                        //��ط���������
                        DstOrgID = "600404";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6004040001";
                        break;
                    case "20":
                        //��ط���������
                        DstOrgID = "600101";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6001010001";
                        break;
                    default:
                        ErrMsg = "ָ��ʡ������Ŧ��";
                        return -1;
                        break;
                }
                **/
                #endregion

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //����ʱ�䣬ʱ���ʽ�ַ�
                string InfoTypeID = "31";                                                       //�������ʹ���
                //��ʶ����
                string CodeType = "";
                //��ʶ���ͱ���װ��
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;
               
                //��ʶ����
                string CodeValue = "";
                //�����ش���
                string CityCode = "";
                //�̻���С��ͨ����������⴦��
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
               
                //�Ƿ���������
                //string PasswdFlag = "1";
                //����
                string CCPasswd = Password;
                
                //����Crm��ѯxml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                logger.Info("����crm��Ŧ����:");
                logger.Info(str);
                //��Crm������ѯ
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm��Ŧ���ر���:");
                logger.Info(rStr);
                //����Crm���ؿͻ���Ϣ
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    //�ͻ�����ת��
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //֤������ת��
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);
                    
                    //��Ա����ת��
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //ȫ��CRM�û�ע�ᵽ�Ű�
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //�ͻ�����
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //δ֪
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
                    ErrMsg = "�������ͼ�����:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "Ӧ�����:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "����:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
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

                string BusCode = "BUS17002";                                    //ҵ���ܱ���
                string ServiceCode = "SVC11001";                                //�ӿ�Э�����
                string ServiceContractVer = "SVC1100120091002";                 //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                        //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                     //���𷽻�������
                string SrcSysID = "1000000020";                                 //����(ϵͳ/ƽ̨)����
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //����(ϵͳ/ƽ̨)ǩ��
                //��ط���������
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }
                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }

                #region ���ز���
                /*
                switch (UAProvinceID)
                {
                    case "02":
                        //��ط���������
                        DstOrgID = "600102";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6001020001";
                        break;
                    case "19":
                        //��ط���������
                        DstOrgID = "600203";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6002030001";
                        break;
                    case "05":
                        //��ط���������
                        DstOrgID = "609906";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6099060001";
                        break;
                    case "08":
                        //��ط���������
                        DstOrgID = "609905";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6099050001";
                        break;
                    case "30":
                        //��ط���������
                        DstOrgID = "600404";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6004040001";
                        break;
                    case "20":
                        //��ط���������
                        DstOrgID = "600101";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6001010001";
                        break;
                    default:
                        ErrMsg = "ָ��ʡ������Ŧ��";
                        return -1;
                        break;
                }
                **/
                #endregion

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //����ʱ�䣬ʱ���ʽ�ַ�
                string InfoTypeID = "31";                                                       //�������ʹ���
                //��ʶ����
                string CodeType = "";
                //��ʶ���ͱ���װ��
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //��ʶ����
                string CodeValue = "";
                //�����ش���
                string CityCode = "";
                //�̻���С��ͨ����������⴦��
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

                //�Ƿ���������
                //string PasswdFlag = "1";
                //����
                string CCPasswd = Password;

                //����Crm��ѯxml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                logger.Info("����crm��Ŧ����:");
                logger.Info(str);
                //��Crm������ѯ
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm��Ŧ���ر���:");
                logger.Info(rStr);
                //����Crm���ؿͻ���Ϣ
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    CustAddress = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress;

                    //�ͻ�����ת��
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //֤������ת��
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //��Ա����ת��
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //ȫ��CRM�û�ע�ᵽ�Ű�
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //�ͻ�����
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //δ֪
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
                    ErrMsg = "�������ͼ�����:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "Ӧ�����:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "����:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
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

                string BusCode = "BUS17002";                                    //ҵ���ܱ���
                string ServiceCode = "SVC11001";                                //�ӿ�Э�����
                string ServiceContractVer = "SVC1100120091002";                 //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                        //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                     //���𷽻�������
                string SrcSysID = "1000000020";                                 //����(ϵͳ/ƽ̨)����
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //����(ϵͳ/ƽ̨)ǩ��
                //��ط���������
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }
                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //����ʱ�䣬ʱ���ʽ�ַ�
                string InfoTypeID = "31";                                                       //�������ʹ���
                //��ʶ����
                string CodeType = "";
                //��ʶ���ͱ���װ��
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //��ʶ����
                string CodeValue = "";
                //�����ش���
                string CityCode = "";
                //�̻���С��ͨ����������⴦��
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

                //�Ƿ���������
                //string PasswdFlag = "1";
                //����
                string CCPasswd = Password;

                //����Crm��ѯxml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                logger.Info("����crm-v4��Ŧ����:");
                logger.Info(str);
                //��Crm������ѯ
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;
                logger.Info("crm-v4��Ŧ���ر���:");
                logger.Info(rStr);
                //����Crm���ؿͻ���Ϣ
                qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoV2XML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    CustAddress = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress;

                    //�ͻ�����ת��
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //֤������ת��
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //��Ա����ת��
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //ȫ��CRM�û�ע�ᵽ�Ű�
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //�ͻ�����
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //δ֪
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
                    ErrMsg = "�������ͼ�����:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "Ӧ�����:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "����:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
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

                string BusCode = "BUS17006";                                    //ҵ���ܱ���
                string ServiceCode = "SVC33003";                                //�ӿ�Э�����
                string ServiceContractVer = "SVC3300320120719";                 //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                        //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();
                string ServiceLevel = "1";                                      //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                     //���𷽻�������
                string SrcSysID = "1000000020";                                 //����(ϵͳ/ƽ̨)����
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //����(ϵͳ/ƽ̨)ǩ��
                //��ط���������
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }
                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }
                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //����ʱ�䣬ʱ���ʽ�ַ�
  
                //����Crm��ѯxml
                XMLExchange xMLExchange = new XMLExchange();
                str =xMLExchange.BuildYoungQryMemberXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                SOO_ID, LAN_ID, AREA_NBR, ACC_NBR, PROD_CLASS);


                StringBuilder requestXml = new StringBuilder();

                #region ƴ������xml�ַ���

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




                //��Crm������ѯ
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
        /// ��young�ͻ���֤v2
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

                string BusCode = "BUS17006";                                    //ҵ���ܱ���
                string ServiceCode = "SVC11001";                                //�ӿ�Э�����
                string ServiceContractVer = "SVC1100120110525";                 //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                        //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                     //���𷽻�������
                string SrcSysID = "1000000020";                                 //����(ϵͳ/ƽ̨)����
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //����(ϵͳ/ƽ̨)ǩ��
                //��ط���������
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }
                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //����ʱ�䣬ʱ���ʽ�ַ�
                string InfoTypeID = "36";                                                       //�������ʹ���
                //��ʶ����
                string CodeType = "";
                //��ʶ���ͱ���װ��
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //��ʶ����
                string CodeValue = "";
                //�����ش���
                string CityCode = "";
                //�̻���С��ͨ����������⴦��
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

                //�Ƿ���������
                //string PasswdFlag = "1";
                //����
                string CCPasswd = Password;

                //����Crm��ѯxml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildYoungQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);

                BTUCenterInterfaceLog.CenterForBizTourLog("YoungUserAuthV2", new StringBuilder(str));

                //��Crm������ѯ
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

                    //������ˮ��
                    date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    if (count >= 9)
                        count = 0;
                    else
                        count = count + 1;
                    TransactionID = "1000000020" + date + count.ToString();

                    BusCode = "BUS17002";                                            //ҵ���ܱ���
                    ServiceCode = "SVC11001";                                        //�ӿ�Э�����
                    ServiceContractVer = "SVC1100120091002";                         //Э�鵱ǰʹ�õİ汾��

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

                    //�ͻ�����ת��
                    CustType = ConvertCustType(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //֤������ת��
                    string IdentType = ConvertIdentType(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //��Ա����ת��
                    string CustServiceLevel = ConvertCustServiceLevel(authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);


                    PointType = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointType;
                    PointValueSum = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValueSum;
                    PointValue = authYoungReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValue;

                    if (RealName1 != "")
                        RealName = RealName1;

                    //ȫ��CRM�û�ע�ᵽ�Ű�
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //�ͻ�����
                        IdentType,
                        authYoungReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //δ֪
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
                    ErrMsg = "�������ͼ�����:" + authYoungReturn.TcpCont.Response.RspType
                             + "Ӧ�����:" + authYoungReturn.TcpCont.Response.RspCode
                             + "����:" + authYoungReturn.TcpCont.Response.RspDesc;
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

                string BusCode = "BUS17006";                                    //ҵ���ܱ���
                string ServiceCode = "SVC11001";                                //�ӿ�Э�����
                string ServiceContractVer = "SVC1100120110525";                 //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                        //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();


                string ServiceLevel = "1";                                      //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                     //���𷽻�������
                string SrcSysID = "1000000020";                                 //����(ϵͳ/ƽ̨)����
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];    //����(ϵͳ/ƽ̨)ǩ��
                //��ط���������
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }
                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }

              

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");                       //����ʱ�䣬ʱ���ʽ�ַ�
                string InfoTypeID = "36";                                                       //�������ʹ���
                //��ʶ����
                string CodeType = "";
                //��ʶ���ͱ���װ��
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //��ʶ����
                string CodeValue = "";
                //�����ش���
                string CityCode = "";
                //�̻���С��ͨ����������⴦��
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

                //�Ƿ���������
                //string PasswdFlag = "1";
                //����
                string CCPasswd = Password;

                //����Crm��ѯxml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildYoungQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);

                //��Crm������ѯ
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;

                //����Crm���ؿͻ���Ϣ
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

    
                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    //�ͻ�����ת��
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //֤������ת��
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //��Ա����ת��
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //ȫ��CRM�û�ע�ᵽ�Ű�
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        CustType,               //�ͻ�����
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",                    //δ֪
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
                    ErrMsg = "�������ͼ�����:" + qryCustInfoReturn.TcpCont.Response.RspType
                             + "Ӧ�����:" + qryCustInfoReturn.TcpCont.Response.RspCode
                             + "����:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
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

                string BusCode = "BUS17002";                                            //ҵ���ܱ���
                string ServiceCode = "SVC11001";                                        //�ӿ�Э�����
                string ServiceContractVer = "SVC1100120091002";                         //Э�鵱ǰʹ�õİ汾��
                string ActionCode = "0";                                                //�����ʶ
                //������ˮ��
                string date = DateTime.Now.ToString("yyyyMMddHHmmssfff");               
                if (count >= 9)
                    count = 0;
                else
                    count = count + 1;
                string TransactionID = "1000000020" + date + count.ToString();

                string ServiceLevel = "1";                                              //����ȼ�,��������ȼ�
                string SrcOrgID = "100000";                                             //���𷽻�������
                string SrcSysID = "1000000020";                                         //����(ϵͳ/ƽ̨)����
                //����(ϵͳ/ƽ̨)ǩ��
                string SrcSysSign = System.Configuration.ConfigurationManager.AppSettings["SrcSysSign"];

                //��ط���������
                string DstOrgID = "";
                DstOrgID = CommonBizRules.GetReginCodeByProvinceID(UAProvinceID, context);
                if (DstOrgID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط���������";
                    return -1;
                }

                //��ط�(ϵͳ/ƽ̨)����
                string DstSysID = "";
                DstSysID = CommonBizRules.GetSPOuterIDBySPID(UAProvinceID + "999999", context);
                if (DstSysID == "")
                {
                    ErrMsg = "ָ��ʡ��Ӧ������ط�(ϵͳ/ƽ̨)����";
                    return -1;
                }

                #region ���ز���
                //TestStr = "DstOrgID" + DstOrgID + "DstSysID" + DstSysID;
                //return -1;
                /*
                switch (UAProvinceID)
                {
                    case "02":
                        //��ط���������
                        DstOrgID = "600102";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6001020001";
                        break;
                    case "19":
                        //��ط���������
                        DstOrgID = "600203";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6002030001";
                        break;
                    case "05":
                        //��ط���������
                        DstOrgID = "609906";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6099060001";
                        break;
                    case "08":
                        //��ط���������
                        DstOrgID = "609905";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6099050001";
                        break;
                    case "30":
                        //��ط���������
                        DstOrgID = "600404";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6004040001";
                        break;
                    case "20":
                        //��ط���������
                        DstOrgID = "600101";
                        //��ط�(ϵͳ/ƽ̨)����
                        DstSysID = "6001010001";
                        break;
                    default:
                        ErrMsg = "ָ��ʡ������Ŧ��";
                        return -1;
                        break;
                }
                **/
                #endregion

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");               //����ʱ�䣬ʱ���ʽ�ַ�
                string InfoTypeID = "31";                                               //�������ʹ���
                //��ʶ����
                string CodeType = "";
                //��ʶ���ͱ���װ��
                Result = ConvertCodeType(AuthenType, out CodeType, out ErrMsg);
                if (Result != 0)
                    return Result;

                //��ʶ����
                string CodeValue = "";
                //�����ش���
                string CityCode = "";

                //�̻�
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
                    //��ʶ����
                    CodeValue = Username;
                    //�����ش���
                    CityCode = "";
                }

                //�Ƿ���������
                //    string PasswdFlag = "1";
                //����
                string CCPasswd = Password;

                //����Crm��ѯxml
                XMLExchange xMLExchange = new XMLExchange();
                str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
                ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
                InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);
                
                //Crm��ѯ
                DEPService obj = new DEPService();
                obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
                rStr = obj.exchange(str);
                TestStr = rStr;

                //����Crm���ؿͻ���Ϣ
                QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

                if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                {
                    RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                    UserName = "";
                    NickName = "";
                    CustID = "";
                    OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                    //�ͻ�����ת��
                    CustType = ConvertCustType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);

                    //֤������ת��
                    string IdentType = ConvertIdentType(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);

                    //�ͻ���Ա�ȼ�ת��
                    string CustServiceLevel = ConvertCustServiceLevel(qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);

                    if (RealName1 != "")
                        RealName = RealName1;

                    //ȫ��CRM�û�ע�ᵽ�Ű�
                    Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                        //"021",
                        CustType,//�ͻ�����
                        IdentType,
                        qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                        RealName,
                        CustServiceLevel,
                        "2",//δ֪
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

                    ErrMsg = "�������ͼ�����:" + qryCustInfoReturn.TcpCont.Response.RspType
                                         + "Ӧ�����:" + qryCustInfoReturn.TcpCont.Response.RspCode
                                         + "����:" + qryCustInfoReturn.TcpCont.Response.RspDesc;

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
                msg.Append("��Ŧ�û���֤�ӿ� ���ͱ���" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append("XML - " + str);
                BTUCenterInterfaceLog.CenterForCRM("CrmSSOPost", msg);
                StringBuilder msgResult = new StringBuilder();
                msgResult.Append("��Ŧ�û���֤�ӿ� ������" + DateTime.Now.ToString("u") + "\r\n");
                msgResult.Append("XML - " + rStr);
                msgResult.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForCRM("CrmSSOReturn", msgResult);
                #endregion
            }
            return Result;
        }

        #region ����ת��


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
                    ErrMsg = "��֤���ʹ���";
                    result = -1;
                    break;
            }
            return result;
        
        }

        /// <summary>
        /// ��ʶ����ת��
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
                    ErrMsg = "��֤���ʹ���";
                    result = -1;
                    break;
            }
            return result;
        }

        //10	����ͻ� 10
        //11	��ͥ�ͻ� 20
        //12	���˿ͻ� 30
        //99	�����ͻ� 90
        /// <summary>
        /// �ͻ�����ת��
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

        /*0	  ͳһ�ͻ���ʶ�� 9
        *1	  ���֤         0
        *2	  ����֤         2   
        *3	  ����           3
        *4	  �۰�̨ͨ��֤   6
        *5	  ���Ӹɲ�����֤ 9
        *6	  ����Ӫҵִ��   9
        *7	  ��λ֤��       9
        *9	  ��ʻ֤         9
        *10	  ѧ��֤         9
        *11	  ��ʦ֤         9
        *12	  ���ڱ�/��ס֤  9
        *13	  ����֤         9
        *14	  ʿ��֤         1
        *15	  ��֯��������֤ 9
        *17	  ����֤         9
        *18	  ��ס֤         9
        *19	  ����ʶ�����   9
        *20	  ���ſͻ���ʶ�� 9
        *21	  VIP��          9
        *22	  ����֤
        *99	  ����           9
        *
        0�����֤
        1��ʿ��֤
        2������֤
        3������
        4������
        5��̨��֤
        6���۰�ͨ��֤
        7�����ʺ�Ա֤
        9������
        10-���Ӹɲ�����֤
        11-����Ӫҵִ��
        12-��λ֤��
        13-��ʻ֤
        14-ѧ��֤
        15-��ʦ֤
        16-���ڱ�/��ס֤
        17-����֤
        18-��֯��������֤
        19-����֤
        20-��ס֤
        21-����ʶ�����
        22-���ſͻ���ʶ��
        23-VIP��
        24-����֤
        */
        /// <summary>
        /// ֤������ת��
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

        /** 10 �ǻ�Ա
            11 ��
            12 ��
            13 ��
            14 ��ͨ
            1����ʯ����Ա
            2���𿨻�Ա
            3��������Ա
            4����ͨ��Ա�������̳�ע��ͻ�������
            5���ǻ�Ա
        * */
        /// <summary>
        /// �ͻ���Ա�ȼ�ת��
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

        #region ��־

        /// <summary>
        /// POST��Crm������־
        /// </summary>
        /// <param name="msg"></param>
        private static void LogCrmSSOPost(String postXml)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("��Ŧ�û���֤�ӿ� ���ͱ���" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("XML - " + postXml);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            BTUCenterInterfaceLog.CenterForCRM("CrmSSOPost", msg);
        }

        /// <summary>
        /// Crm���صı�����־
        /// </summary>
        /// <param name="msg"></param>
        private static void LogCrmSSOReturn(String returnXml)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("��Ŧ�û���֤�ӿ� ������" + DateTime.Now.ToString("u") + "\r\n");
            msg.Append("XML - " + returnXml);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForCRM("CrmSSOReturn", msg);
        }

        #endregion

    }
        
}

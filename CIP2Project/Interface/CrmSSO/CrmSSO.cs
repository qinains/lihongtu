/*
 * 
 *�Ѿ����� 
 * 
 * 
*/
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

namespace Linkage.BestTone.CrmSSO.Rule
{
    public class CrmSSO
    {

        public static int UserAuthCrm(string UAProvinceID, string AuthenType, string Username, string Password, out string RealName, out string UserName, out string NickName, out string OutID, out string CustType, out string CustID, out string ErrMsg, out string TestStr, out string dealType)
        {
            int Result = 0;

            RealName="";
            UserName="";
            NickName="";
            OutID = "";
            CustType="";
            CustID=""; 
            ErrMsg="";
            TestStr = "";
            dealType = "";

            //ҵ���ܱ���
            string BusCode = "BUS17002";
            //�ӿ�Э�����
            string ServiceCode = "SVC11001";
            //Э�鵱ǰʹ�õİ汾��
            string date = DateTime.Now.ToString("yyyyMMdd");
            string ServiceContractVer = "SVC1100120091002";
            //�����ʶ
            string ActionCode = "0";
            //������ˮ��

            Random r = new Random();
            string rand1 = r.Next(10000, 99999).ToString();
            string rand2 = r.Next(10000, 99999).ToString();
            string TransactionID = "1000000020" + date + rand1 + rand2;
            //����ȼ�,��������ȼ�
            string ServiceLevel = "1";
            //���𷽻�������
            string SrcOrgID = "100000";
            //����(ϵͳ/ƽ̨)����
            string SrcSysID = "1000000020";
            //����(ϵͳ/ƽ̨)ǩ��
            string SrcSysSign = "123";

            //��ط���������
            string DstOrgID = "";
            //��ط�(ϵͳ/ƽ̨)����
            string DstSysID = "";

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
                default:
                    ErrMsg = "ָ��ʡ������Ŧ��";
                    return -1;
                    break;
            }

            //����ʱ�䣬ʱ���ʽ�ַ�
            string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");
            //�������ʹ���
            string InfoTypeID = "31";
            //��ʶ����
            string CodeType = "";
            //��ʶ���ͱ���װ��
            switch (AuthenType)
            {
                case "7":
                    CodeType = "11";
                    break;
                case "9":
                    CodeType = "12";
                    break;
                case "11":
                    CodeType = "13";
                    break;
                case "10":
                    CodeType = "17";
                    break;
                default:
                    ErrMsg = "��֤���ʹ���";
                    return -1;
                    break;
            }
            //��ʶ����
            string CodeValue = Username;
            //�����ش���
            string CityCode = "";
            //�Ƿ���������
            string PasswdFlag = "1";
            //����
            string CCPasswd = Password;

            XMLExchange xMLExchange = new XMLExchange();
            string str = xMLExchange.BuildQryCustInfoXML(BusCode, ServiceCode, ServiceContractVer, ActionCode, TransactionID,
            ServiceLevel, SrcOrgID, SrcSysID, SrcSysSign, DstOrgID, DstSysID, ReqTime,
            InfoTypeID, CodeType, CodeValue, CityCode, PasswdFlag, CCPasswd);

            //SPInfoManager spInfo = new SPInfoManager();
            //Object SPData = spInfo.GetSPData(this.Context, "SPData");
            //string url = spInfo.GetPropertyBySPID(SPID, "InterfaceUrl1", SPData);
            DEPService obj = new DEPService();
            obj.Url = System.Configuration.ConfigurationManager.AppSettings["DEPServiceURL"];
            string rStr = obj.exchange(str);
            TestStr=rStr;
            /***

            Response.ContentType = "text/xml";
            Response.Charset = "UTF-8";
            Response.Clear();
            if (SourceType == "8")
            {

                Response.Write(str);
            }
            if (SourceType == "9")
            {

                Response.Write(rStr);
            }
            Response.End();
            */

         
            QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

            if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
            {

                RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                UserName = "";
                NickName = "";
                OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;
                //10	����ͻ� 10
                //11	��ͥ�ͻ� 20
                //12	���˿ͻ� 30
                //99	�����ͻ� 90

                switch (qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup)
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
                CustID = "";


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
                *99	  ����           9
                *
                *0�����֤
                *1��ʿ��֤
                *2������֤
                *3������
                *4������
                *5��̨��֤
                *6���۰�ͨ��֤
                *7�����ʺ�Ա֤
                *9������
                */
                string IdentType = "";


                switch (qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType)
                {
                    case "1":
                        IdentType = "0";
                        break;
                    case "4":
                        IdentType = "6";
                        break;
                    case "14":
                        IdentType = "1";
                        break;
                    case "2":
                        IdentType = "2";
                        break;
                    case "3":
                        IdentType = "3";
                        break;
                    default:
                        IdentType = "9";
                        break;
                }

                //ȫ��CRM�û�ע�ᵽ�Ű�
                Result = UserRegistry.getUserRegistryCrm(UAProvinceID,//qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceCode,
                   qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                   //"021",
                    CustType,//�ͻ�����
                    IdentType,
                    qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                    RealName,
                    qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel,
                    "2",//δ֪
                    OutID,
                    System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"],
                    qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                    out CustID,
                   out  ErrMsg,
                   out dealType);

                if (Result != 0)
                {
                    //err_code.InnerHtml = ErrMsg;
                    return Result;
                }


            }
            else
            {

                ErrMsg = "�������ͼ�����:" + qryCustInfoReturn.TcpCont.Response.RspType
                                     + "Ӧ�����:" + qryCustInfoReturn.TcpCont.Response.RspCode
                                     + "����:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
                
                return -1;

            }
            
            return Result;
        }
    }
        
}

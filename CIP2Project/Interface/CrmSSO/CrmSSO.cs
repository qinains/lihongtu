/*
 * 
 *已经作废 
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

            //业务功能编码
            string BusCode = "BUS17002";
            //接口协议编码
            string ServiceCode = "SVC11001";
            //协议当前使用的版本号
            string date = DateTime.Now.ToString("yyyyMMdd");
            string ServiceContractVer = "SVC1100120091002";
            //请求标识
            string ActionCode = "0";
            //交易流水号

            Random r = new Random();
            string rand1 = r.Next(10000, 99999).ToString();
            string rand2 = r.Next(10000, 99999).ToString();
            string TransactionID = "1000000020" + date + rand1 + rand2;
            //服务等级,处理的优先级
            string ServiceLevel = "1";
            //发起方机构代码
            string SrcOrgID = "100000";
            //发起方(系统/平台)编码
            string SrcSysID = "1000000020";
            //发起方(系统/平台)签名
            string SrcSysSign = "123";

            //落地方机构编码
            string DstOrgID = "";
            //落地方(系统/平台)编码
            string DstSysID = "";

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
                default:
                    ErrMsg = "指定省不在枢纽中";
                    return -1;
                    break;
            }

            //请求时间，时间格式字符
            string ReqTime = DateTime.Now.ToString("yyyyMMddHHMMss");
            //资料类型代码
            string InfoTypeID = "31";
            //标识类型
            string CodeType = "";
            //标识类型编码装换
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
                    ErrMsg = "认证类型错误";
                    return -1;
                    break;
            }
            //标识号码
            string CodeValue = Username;
            //所属地代码
            string CityCode = "";
            //是否需填密码
            string PasswdFlag = "1";
            //密码
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
                //10	政企客户 10
                //11	家庭客户 20
                //12	个人客户 30
                //99	其它客户 90

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
                *99	  其它           9
                *
                *0－身份证
                *1－士兵证
                *2－军官证
                *3－护照
                *4－保留
                *5－台胞证
                *6－港澳通行证
                *7－国际海员证
                *9－其他
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

                //全国CRM用户注册到号百
                Result = UserRegistry.getUserRegistryCrm(UAProvinceID,//qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceCode,
                   qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                   //"021",
                    CustType,//客户类型
                    IdentType,
                    qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                    RealName,
                    qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel,
                    "2",//未知
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

                ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                                     + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                                     + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;
                
                return -1;

            }
            
            return Result;
        }
    }
        
}

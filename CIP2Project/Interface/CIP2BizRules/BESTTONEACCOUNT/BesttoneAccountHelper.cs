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
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Collections.Specialized;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;
using System.Net;

namespace Linkage.BestTone.Interface.Rule
{
    public static class BesttoneAccountHelper
    {
        private static IDispatchControl serviceProxy = new IDispatchControl();

        #region �ӿڷ���

        #region �˻���ѯ

        /// <summary>
        /// ����  phoneNumber -> PRODUCTNO
        /// </summary>
        public static Int32 RegisterBesttoneAccount(String phoneNumber,String realName,String contactTel,String contactMail,String sex,String certtype,String certnum,  String TransactionID, out String ErrMsg)
        {

 
            log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" RegisterBesttoneAccount:");

            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            string command = "100201|310000-TEST1-127.0.0.1|1.0|127.0.0.1";
            string ResultXML = "";
            string CARDNO = "8600021008207231";
            string CARDTYPE = "1";
            if (String.IsNullOrEmpty(realName))
            {
                realName = phoneNumber;
            } 
            string NAME = realName;    //M  ��ʵ������������
            string LOGINID = "";         //O
            string PRODUCTNO = "yy"+phoneNumber;   //M  �������ͨ�˻����ֻ��ţ�
            string ISREALNAME = "3";        //M �Ƿ���ʵ����֤  3δ����֤
            string REGISTERTYPE = "Y";       //M ע������ 1�ֻ���
            string ISAPPLYCERT = "1";       // M �Ƿ��������֤�� 1��
            string CUSTOMLEVEL = "1";        //M �ͻ�����  1��ͨ
            string CUSTOMTYPE = "1";          //M �ͻ����  1���� 


            string REGCELLPHONENUM = "";      //M ��ϵ�ֻ� ��18900000000 PRODUCTNO
            if(String.IsNullOrEmpty(contactTel))
            {
                REGCELLPHONENUM = phoneNumber;
            }else
            {
                REGCELLPHONENUM = contactTel;
            }

            string REGEMAIL = contactMail;              //M ��ϵ���� 
            //if (String.IsNullOrEmpty(contactMail))
            //{
            //    REGEMAIL = phoneNumber + "@189.cn";
            //} 
            //else
            //{
            //    REGEMAIL = contactMail;
            //}

            REGEMAIL = "";

            string LOGINPASSWORD = "";    //O ��¼����    ��11.5���ݲ���

            string SEX = sex;     //O
            string CERTTYPE = certtype;    //֤������ M X����
            string CERTNUM = certnum;      //֤������ M 9999
            string FAMILYTEL = "";       //��ͥ��ϵ�绰  O
            string OFFICETEL = "";       //�����绰 �칫�ҵ绰 0
            //string APANAGE = "";
            //string AREACODE = AREACODE;    //AREACODE
            //string CITYCODE = CITYCODE;    //CITYCODE
            string CONTRACTADD = "";        //��ϵ��ַ O
            string COMPANYADD = "";         //��λ��ַ O
            string COMPANYZIP = "";         //��λ�ʱ� O
            string COMPANYCODE = "";         //��λ���� O
            //string ACCEPTORGCODE = ACCEPTORGCODE;     //ACCEPTORGCODE
            string ACCEPTUID = "";       //������ O
            //string ACCEPTAREACODE = ""; 
            //string ACCEPTCITYCODE = "";
            string ACCEPTCHANNEL = "02";    //�������� M  02 WEB
            string ACCEPTSEQNO = TransactionID;      //������ˮ M 20λ���� ���÷�����
            string FEEFLAG = "1";   //�����շѱ�־ M 1���շ�
            string FEEAMT = "0";      //�����շѽ�� M �Է�Ϊ��λ 0

            string INPUTUID = "";    //������Ա���� O
            string INPUTTIME = "";   //����ʱ�� M yyyyMMddHHmmss��ʽ
            string CHECKUID = "";    //��ȨԱ����ʶ O
            string CHECKTIME = "";   //��Ȩʱ�� O yyyyMMddHHmmss��ʽ
          
            try
            {
                Result = GetRequestXML(CARDNO, CARDTYPE, NAME, LOGINID, PRODUCTNO, ISREALNAME, REGISTERTYPE,
                ISAPPLYCERT, CUSTOMLEVEL, CUSTOMTYPE, REGCELLPHONENUM, REGEMAIL, LOGINPASSWORD, SEX, CERTTYPE, CERTNUM, FAMILYTEL,
                OFFICETEL,    CONTRACTADD, COMPANYADD, COMPANYZIP,
                COMPANYCODE, ACCEPTUID,  ACCEPTCHANNEL,
                ACCEPTSEQNO, FEEFLAG, FEEAMT, INPUTUID, INPUTTIME, CHECKUID, CHECKTIME, TransactionID,
                out ResultXML, out  ErrMsg);
                log(String.Format("�����������xml{0}", ResultXML));
                string ret = serviceProxy.dispatchCommand(command, ResultXML);
                log(String.Format("�������ؽ��:{0}", ret));

                System.Xml.XmlDocument xd = new XmlDocument();
                xd.LoadXml(ret);
                XmlNode xmlNode1 = xd.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE");
                XmlNode xmlNode2 = xd.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT");
                if ("000000".Equals(xmlNode1.InnerText))
                {
                    Result = 0;
                    ErrMsg = xmlNode2.InnerText;
                }else
                {
                    Result = -99999;
                    ErrMsg = xmlNode2.InnerText;
                }
                
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
            return Result;
        }

        


        /// <summary>
        /// ��װ�����������
        /// <PayPlatRequestParameter>
        /// <CTRL-INFO 
        /// WEBSVRNAME="��������" 
        /// WEBSVRCODE="�������" 
        /// APPFROM="���󱾷����Ӧ�ñ�ʶ��"
        /// 	KEEP="���η���ı�ʶ��ˮ" />
        /// <PARAMETERS>
        /// ....  ��ɫ����Ҫ��ǰ������� ���� �����Ա�
        /// </PARAMETERS>
        /// </PayPlatRequestParameter>
        /// </summary>
        public static int GetRequestXML(string CARDNO, string CARDTYPE, string NAME, string LOGINID, string PRODUCTNO, string ISREALNAME, string REGISTERTYPE,
            string ISAPPLYCERT ,string CUSTOMLEVEL ,string CUSTOMTYPE , string REGCELLPHONENUM,string REGEMAIL,string LOGINPASSWORD,string SEX, string CERTTYPE,string CERTNUM,string FAMILYTEL,
            string OFFICETEL,  string CONTRACTADD, string COMPANYADD, string COMPANYZIP,
            string COMPANYCODE,string ACCEPTUID,string ACCEPTCHANNEL,
            string ACCEPTSEQNO,string FEEFLAG,string FEEAMT,string INPUTUID,string INPUTTIME,string CHECKUID,string CHECKTIME,string KEEP,
            out string ResultXML, out string ErrMsg)
        {
           
            int Result = 0;
            ResultXML = "";
            ErrMsg = "";


            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem_ROOT;
            XmlElement xmlelem_CTRLINFO;
            XmlElement xmlelem_PARAMETERS;


            XmlElement xmlelem_CARDNO;
            XmlElement xmlelem_CARDTYPE;
            XmlElement xmlelem_NAME;
            XmlElement xmlelem_LOGINID;
            XmlElement xmlelem_PRODUCTNO;
            XmlElement xmlelem_ISREALNAME;
            XmlElement xmlelem_REGISTERTYPE; //
            XmlElement xmlelem_ISAPPLYCERT;  //
            XmlElement xmlelem_CUSTOMLEVEL;  //
            XmlElement xmlelem_CUSTOMTYPE;   //

            XmlElement xmlelem_REGCELLPHONENUM; //
            XmlElement xmlelem_REGEMAIL;  //
            XmlElement xmlelem_LOGINPASSWORD; //

            XmlElement xmlelem_SEX;  // 
            XmlElement xmlelem_CERTTYPE;  // 
            XmlElement xmlelem_CERTNUM;   //
            XmlElement xmlelem_FAMILYTEL;  //
            XmlElement xmlelem_OFFICETEL;   //
            XmlElement xmlelem_APANAGE;    // 
            XmlElement xmlelem_AREACODE;   //
            XmlElement xmlelem_CITYCODE;   //
            XmlElement xmlelem_CONTRACTADD; //
            XmlElement xmlelem_COMPANYADD;  // 
            XmlElement xmlelem_COMPANYZIP;   // 
            XmlElement xmlelem_COMPANYCODE;   // 
            XmlElement xmlelem_ACCEPTORGCODE;  // 
            XmlElement xmlelem_ACCEPTUID;  //
            XmlElement xmlelem_ACCEPTAREACODE;  // 
            XmlElement xmlelem_ACCEPTCITYCODE;  //
            XmlElement xmlelem_ACCEPTCHANNEL;  //
            XmlElement xmlelem_ACCEPTSEQNO;  // 
            XmlElement xmlelem_FEEFLAG;  // 
            XmlElement xmlelem_FEEAMT;  // 
            XmlElement xmlelem_INPUTUID;  // 
            XmlElement xmlelem_INPUTTIME;  // 
            XmlElement xmlelem_CHECKUID;  // 
            XmlElement xmlelem_CHECKTIME; // 


            XmlText xmltext;


            xmldoc = new XmlDocument();
            //����XML����������
           
            xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
           
            xmldoc.AppendChild(xmlnode);
            //����һ����Ԫ��
            xmlelem_ROOT = xmldoc.CreateElement("", "PayPlatRequestParameter", "");
            xmldoc.AppendChild(xmlelem_ROOT);

            ///////////////////////////////////////////

            xmlelem_CTRLINFO = xmldoc.CreateElement("", "CTRL-INFO", "");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem_CTRLINFO);
            xmlelem_CTRLINFO.SetAttribute("WEBSVRNAME", "������ȫ���˻���");  //�������ƣ��ο���¼10.2
            xmlelem_CTRLINFO.SetAttribute("WEBSVRCODE", "100201");   //������룬�ο���¼10.2
            xmlelem_CTRLINFO.SetAttribute("APPFROM", "100201|310000-TEST1-127.0.0.1|1.0|127.0.0.1");  //20 λ   ����˷���Ŀͻ��˱�ʶ���� ��ʽ��ʡ��-Ӧ��ϵͳ����-�汾��-IP ʡ�ݱ���ο���9.9
            xmlelem_CTRLINFO.SetAttribute("KEEP", KEEP);    //2012 09 16 20 21 52

            xmlelem_PARAMETERS = xmldoc.CreateElement("PARAMETERS");
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem_PARAMETERS);




            //CARDNO
            //xmlelem_CARDNO = xmldoc.CreateElement("CARDNO");
            //xmltext = xmldoc.CreateTextNode(CARDNO);
            //xmlelem_CARDNO.AppendChild(xmltext);
            //xmlelem_PARAMETERS.AppendChild(xmlelem_CARDNO);


            //CARDTYPE
            //xmlelem_CARDTYPE = xmldoc.CreateElement("CARDTYPE");
            //xmltext = xmldoc.CreateTextNode(CARDTYPE);
            //xmlelem_CARDTYPE.AppendChild(xmltext);
            //xmlelem_PARAMETERS.AppendChild(xmlelem_CARDTYPE);


            //NAME
            xmlelem_NAME = xmldoc.CreateElement("NAME");
            xmltext = xmldoc.CreateTextNode(NAME);
            xmlelem_NAME.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_NAME);


            //LOGINID
            xmlelem_LOGINID = xmldoc.CreateElement("LOGINID");
            xmltext = xmldoc.CreateTextNode(LOGINID);
            xmlelem_LOGINID.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_LOGINID);

            //PRODUCTNO
            xmlelem_PRODUCTNO = xmldoc.CreateElement("PRODUCTNO");
            xmltext = xmldoc.CreateTextNode(PRODUCTNO);
            xmlelem_PRODUCTNO.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_PRODUCTNO);

            //ISREALNAME
            xmlelem_ISREALNAME = xmldoc.CreateElement("ISREALNAME");
            xmltext = xmldoc.CreateTextNode(ISREALNAME);
            xmlelem_ISREALNAME.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ISREALNAME);

            //xmlelem_REGISTERTYPE
            xmlelem_REGISTERTYPE = xmldoc.CreateElement("REGISTERTYPE");
            xmltext = xmldoc.CreateTextNode(REGISTERTYPE);
            xmlelem_REGISTERTYPE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_REGISTERTYPE);

            //xmlelem_ISAPPLYCERT
            xmlelem_ISAPPLYCERT = xmldoc.CreateElement("ISAPPLYCERT");
            xmltext = xmldoc.CreateTextNode(ISAPPLYCERT);
            xmlelem_ISAPPLYCERT.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ISAPPLYCERT);

            //xmlelem_CUSTOMLEVEL
            xmlelem_CUSTOMLEVEL = xmldoc.CreateElement("CUSTOMLEVEL");
            xmltext = xmldoc.CreateTextNode(CUSTOMLEVEL);
            xmlelem_CUSTOMLEVEL.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CUSTOMLEVEL);

            //xmlelem_CUSTOMTYPE
            xmlelem_CUSTOMTYPE = xmldoc.CreateElement("CUSTOMTYPE");
            xmltext = xmldoc.CreateTextNode(CUSTOMTYPE);
            xmlelem_CUSTOMTYPE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CUSTOMTYPE);


            //xmlelem_REGCELLPHONENUM
            xmlelem_REGCELLPHONENUM = xmldoc.CreateElement("REGCELLPHONENUM");
            xmltext = xmldoc.CreateTextNode(REGCELLPHONENUM);
            xmlelem_REGCELLPHONENUM.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_REGCELLPHONENUM);

            //xmlelem_REGEMAIL
            xmlelem_REGEMAIL = xmldoc.CreateElement("REGEMAIL");
            xmltext = xmldoc.CreateTextNode(REGEMAIL);
            xmlelem_REGEMAIL.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_REGEMAIL);

            //xmlelem_LOGINPASSWORD
            xmlelem_LOGINPASSWORD = xmldoc.CreateElement("LOGINPASSWORD");
            xmltext = xmldoc.CreateTextNode(LOGINPASSWORD);
            xmlelem_LOGINPASSWORD.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_LOGINPASSWORD);

            //xmlelem_SEX
            xmlelem_SEX = xmldoc.CreateElement("SEX");
            xmltext = xmldoc.CreateTextNode(SEX);
            xmlelem_SEX.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_SEX);

            //xmlelem_CERTTYPE
            xmlelem_CERTTYPE = xmldoc.CreateElement("CERTTYPE");
            xmltext = xmldoc.CreateTextNode(CERTTYPE);
            xmlelem_CERTTYPE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CERTTYPE);

            //xmlelem_CERTNUM
            xmlelem_CERTNUM = xmldoc.CreateElement("CERTNUM");
            xmltext = xmldoc.CreateTextNode(CERTNUM);
            xmlelem_CERTNUM.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CERTNUM);

            //xmlelem_FAMILYTEL
            xmlelem_FAMILYTEL = xmldoc.CreateElement("FAMILYTEL");
            xmltext = xmldoc.CreateTextNode(FAMILYTEL);
            xmlelem_FAMILYTEL.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_FAMILYTEL);

            //xmlelem_OFFICETEL
            xmlelem_OFFICETEL = xmldoc.CreateElement("OFFICETEL");
            xmltext = xmldoc.CreateTextNode(OFFICETEL);
            xmlelem_OFFICETEL.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_OFFICETEL);


            //xmlelem_APANAGE
            xmlelem_APANAGE = xmldoc.CreateElement("APANAGE");

            //xmltext = xmldoc.CreateTextNode("113310000000000");
            xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.APANAGE);
            //BesttoneAccountConstDefinition.DefaultInstance.APANAGE;
            xmlelem_APANAGE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_APANAGE);


            //xmlelem_AREACODE
            xmlelem_AREACODE = xmldoc.CreateElement("AREACODE");
            xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.AREACODE);
            xmlelem_AREACODE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_AREACODE);

            //xmlelem_CITYCODE
            xmlelem_CITYCODE = xmldoc.CreateElement("CITYCODE");
            xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.CITYCODE);
            xmlelem_CITYCODE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CITYCODE);

            //xmlelem_CONTRACTADD
            xmlelem_CONTRACTADD = xmldoc.CreateElement("CONTRACTADD");
            xmltext = xmldoc.CreateTextNode(CONTRACTADD);
            xmlelem_CONTRACTADD.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CONTRACTADD);


            //xmlelem_COMPANYADD
            xmlelem_COMPANYADD = xmldoc.CreateElement("COMPANYADD");
            xmltext = xmldoc.CreateTextNode(COMPANYADD);
            xmlelem_COMPANYADD.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_COMPANYADD);


            //xmlelem_COMPANYZIP
            xmlelem_COMPANYZIP = xmldoc.CreateElement("COMPANYZIP");
            xmltext = xmldoc.CreateTextNode(COMPANYZIP);
            xmlelem_COMPANYZIP.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_COMPANYZIP);

            //xmlelem_COMPANYCODE
            xmlelem_COMPANYCODE = xmldoc.CreateElement("COMPANYCODE");
            xmltext = xmldoc.CreateTextNode(COMPANYCODE);
            xmlelem_COMPANYCODE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_COMPANYCODE);

            //ACCEPTORGCODE	����������� 001310000000000
            //SUPPLYORGCODE	�������� 113310000000000
            //xmlelem_ACCEPTORGCODE
            xmlelem_ACCEPTORGCODE = xmldoc.CreateElement("ACCEPTORGCODE");
            //xmltext = xmldoc.CreateTextNode("001310000000000");
            xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
          
            //BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE;
            xmlelem_ACCEPTORGCODE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTORGCODE);

            //xmlelem_ACCEPTUID
            xmlelem_ACCEPTUID = xmldoc.CreateElement("ACCEPTUID");
            xmltext = xmldoc.CreateTextNode(ACCEPTUID);
            xmlelem_ACCEPTUID.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTUID);

            //xmlelem_ACCEPTAREACODE
            xmlelem_ACCEPTAREACODE = xmldoc.CreateElement("ACCEPTAREACODE");
            xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
            xmlelem_ACCEPTAREACODE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTAREACODE);

            //xmlelem_ACCEPTCITYCODE
            xmlelem_ACCEPTCITYCODE = xmldoc.CreateElement("ACCEPTCITYCODE");
            xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);
            xmlelem_ACCEPTCITYCODE.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTCITYCODE);


            //xmlelem_ACCEPTCHANNEL
            xmlelem_ACCEPTCHANNEL = xmldoc.CreateElement("ACCEPTCHANNEL");
            xmltext = xmldoc.CreateTextNode(ACCEPTCHANNEL);
            xmlelem_ACCEPTCHANNEL.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTCHANNEL);

            //xmlelem_ACCEPTSEQNO
            xmlelem_ACCEPTSEQNO = xmldoc.CreateElement("ACCEPTSEQNO");
            xmltext = xmldoc.CreateTextNode(ACCEPTSEQNO);
            xmlelem_ACCEPTSEQNO.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTSEQNO);

            //xmlelem_FEEFLAG
            xmlelem_FEEFLAG = xmldoc.CreateElement("FEEFLAG");
            xmltext = xmldoc.CreateTextNode(FEEFLAG);
            xmlelem_FEEFLAG.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_FEEFLAG);


            //xmlelem_FEEAMT
            xmlelem_FEEAMT = xmldoc.CreateElement("FEEAMT");
            xmltext = xmldoc.CreateTextNode(FEEAMT);
            xmlelem_FEEAMT.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_FEEAMT);

            //xmlelem_INPUTUID
            xmlelem_INPUTUID = xmldoc.CreateElement("INPUTUID");
            xmltext = xmldoc.CreateTextNode(INPUTUID);
            xmlelem_INPUTUID.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_INPUTUID);


            //xmlelem_INPUTTIME
            xmlelem_INPUTTIME = xmldoc.CreateElement("INPUTTIME");
            xmltext = xmldoc.CreateTextNode(DateTime.Now.ToString("yyyyMMddHHmmss"));
            xmlelem_INPUTTIME.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_INPUTTIME);

            //xmlelem_CHECKUID
            xmlelem_CHECKUID = xmldoc.CreateElement("CHECKUID");
            xmltext = xmldoc.CreateTextNode(CHECKUID);
            xmlelem_CHECKUID.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CHECKUID);

            //xmlelem_CHECKTIME
            xmlelem_CHECKTIME = xmldoc.CreateElement("CHECKTIME");
            xmltext = xmldoc.CreateTextNode(DateTime.Now.ToString("yyyyMMddHHmmss"));
            xmlelem_CHECKTIME.AppendChild(xmltext);
            xmlelem_PARAMETERS.AppendChild(xmlelem_CHECKTIME);


            ResultXML = xmldoc.OuterXml;
         

            return Result;
        }



        /// <summary>
        /// �ͻ���Ϣͬ�� ������� 100090
        /// </summary>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static Int32 NotifyBesttoneAccountInfo( string PRODUCTNO,string NEWNAME,string NEWIDTYPE,string NEWIDNO,string NEWEMAIL,       
            out string ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            String TransactionID = CreateTransactionID();
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            string ACCEPTUID = ""; 
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�ͻ���Ϣͬ��\" WEBSVRCODE=\"100090\" APPFROM=\"100090|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���

                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + PRODUCTNO);
                requestXml.AppendFormat("<NEWNAME>{0}</NEWNAME>", NEWNAME);
                requestXml.AppendFormat("<NEWIDTYPE>{0}</NEWIDTYPE>", NEWIDTYPE);
                requestXml.AppendFormat("<NEWIDNO>{0}</NEWIDNO>", NEWIDNO);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);  //001310000010000  formal  113310000000000 test
                requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", ACCEPTUID);
                requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
                requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);
                requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");
                requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");
                requestXml.AppendFormat("<INPUTUID>{0}</INPUTUID>", "");
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.AppendFormat("<CHECKUID>{0}</CHECKUID>", "");
                requestXml.AppendFormat("<CHECKTIME>{0}</CHECKTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");
              

                #endregion

                //����ӿ�
                log(String.Format("�ͻ���Ϣͬ��:{0}\r\n", requestXml));
                responseXml = serviceProxy.dispatchCommand("100090|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("�ͻ���Ϣͬ��:{0}\r\n", responseXml));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                   
                }

            }
            catch (System.Exception ex)
            {
                log("�ͻ���Ϣͬ��:" + ex.ToString());
            }
            finally
            {
                //log();
            }
            return Result;
        }


        //900000	���öԳƼ�����Կ lihongtu
        public static Int32 GetSymmetryPassWord(out String SymmetryPassWord, out String ErrMsg)
       {
           Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
           ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
           String TransactionID = CreateTransactionID();
           StringBuilder requestXml = new StringBuilder();
           String responseXml = String.Empty;
           SymmetryPassWord = "";
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"���öԳƼ�����Կ\" WEBSVRCODE=\"900000\" APPFROM=\"900000|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���

                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");  //002310000000000
                                                                                                     //001310000000000   
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
  
                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                log(String.Format("���öԳƼ�����Կ����:{0}", requestXml));
                responseXml = serviceProxy.dispatchCommand("900000|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("��ȡ�ԳƼ�����Կ����:{0}", responseXml));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                if (responseCode == "000000")
                {
                    XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];

                    SymmetryPassWord = dataNode.Attributes["PINKEY"].Value;
                    Result = 0;
                    ErrMsg = String.Empty;
                }

            }
            catch (System.Exception ex)
            {
                log("��ȡ�Գ���Կ:"+ex.ToString());
            }
            return Result;
       }

        
        //�����ӿ� 
        public static Int32 CancelBesttoneAccount(String CustomerNo,String ProductNo,String CustomerName, String IDType,String IDNo,out String ErrMsg)
        {

            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            String TransactionID = CreateTransactionID();
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            string ACCEPTUID = ""; 
            try
            {   
                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"���������\" WEBSVRCODE=\"100021\" APPFROM=\"100021|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<CUSTOMERNO>{0}</CUSTOMERNO>", "");
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy"+ProductNo);
                requestXml.AppendFormat("<CUSTOMERNAME>{0}</CUSTOMERNAME>", CustomerName);
                requestXml.AppendFormat("<IDTYPE>{0}</IDTYPE>", IDType);
                requestXml.AppendFormat("<IDNO>{0}</IDNO>", IDNo);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000010000");  //001310000010000  formal  113310000000000 test
                requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", ACCEPTUID);
                requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
                requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);
                requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");
                requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");
                requestXml.AppendFormat("<INPUTUID>{0}</INPUTUID>", "");
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.AppendFormat("<CHECKUID>{0}</CHECKUID>", "");
                requestXml.AppendFormat("<CHECKTIME>{0}</CHECKTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");
                responseXml = serviceProxy.dispatchCommand("100021|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("���������{0}", responseXml));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                if (responseCode == "000000")
                {

                    Result = 0;
                    ErrMsg = "�����ɹ�";
                }
                

            }
            catch (System.Exception ex)
            {
                log("����������쳣:" + ex.ToString());
            }
            finally
            {
            }
            return Result;
        }





        // �ͻ���Ϣ��ѯ�ӿ� lihongtu
        public static Int32 QueryCustInfo(String ProductNo, out CustInfo custinfo, out String ErrMsg)
        {
 
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            String TransactionID = CreateTransactionID();
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            custinfo = new CustInfo();
            try
            {
                #region ƴ������xml�ַ���
                //100101	�ͻ���ѯ
                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�ͻ���ѯ\" WEBSVRCODE=\"100101\" APPFROM=\"100101|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���

                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy"+ProductNo); 

                //

                
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);  //002310000000000
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");  //002310000000000
                //001310000000000   
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                log(String.Format("�ͻ���Ϣ��ѯ����:{0}", requestXml));
                responseXml = serviceProxy.dispatchCommand("100101|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("�ͻ���Ϣ��ѯ����:{0}", responseXml));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                
                if (responseCode == "000000")
                {
                    XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];
                    custinfo.CustomerNo = dataNode.Attributes["CUSTOMER_NO"].Value;
                    custinfo.ProductNo = dataNode.Attributes["PRODUCT_NO"].Value;
                    custinfo.CustomerName = dataNode.Attributes["CUSTOMER_NAME"].Value;
                    custinfo.IdType = dataNode.Attributes["ID_TYPE"].Value;
                    custinfo.IdNo = dataNode.Attributes["ID_NO"].Value;
                    Result = 0;
                    ErrMsg = String.Empty;
                }

            }
            catch (System.Exception ex)
            {
                log("�ͻ���Ϣ��ѯ:" + ex.ToString());
            }
            return Result;



        }
        //�������� 
        // lihongtu
        public static Int32 ResetBesttoneAccountPayPassword(String besttoneAccount, String cerType, String cerNo, String customerName, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"��������\" WEBSVRCODE=\"200101\" APPFROM=\"200101|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy"+besttoneAccount);
                requestXml.AppendFormat("<IDTYPE>{0}</IDTYPE>", cerType);
                requestXml.AppendFormat("<IDNO>{0}</IDNO>", cerNo);
                requestXml.AppendFormat("<CUSTOMERNAME>{0}</CUSTOMERNAME>", customerName);

                //BesttoneAccountConstDefinition.DefaultInstance
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");  //002310000000000
                requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
                //requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", ACCEPTAREACODE);
                //requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", ACCEPTCITYCODE);

                requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
                requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);

                requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");
                requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");
                requestXml.AppendFormat("<INPUTUID>{0}</INPUTUID>", "");
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.AppendFormat("<CHECKUID>{0}</CHECKUID>", "");
                requestXml.AppendFormat("<CHECKTIME>{0}</CHECKTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                log(String.Format("������������:{0}", requestXml));
                responseXml = serviceProxy.dispatchCommand("200101|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("�������뷵��:{0}",responseXml));
                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                    ErrMsg = String.Empty;
                }

                #endregion
            }
            catch (Exception ex)
            {
                log("��������:"+ex.ToString());
            }

            return Result;
        }

        /// <summary>
        /// �������ͨ�˻���ѯ lihongtu
        /// </summary>
        public static Int32 BesttoneAccountInfoQuery(String besttoneAccount, out AccountItem accountInfo, out String ResCode,out String ErrMsg)
        {
          
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            accountInfo = null;
            ResCode = "000000";
            string ACCOUNTTYPE = "1";
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            //��ˮ��
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�˻���ѯ\" WEBSVRCODE=\"100100\" APPFROM=\"100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy"+besttoneAccount);
                requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", ACCOUNTTYPE);

                //BesttoneAccountConstDefinition.DefaultInstance
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");
                
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                log(String.Format("�˻���ѯ����:{0}", requestXml));
                responseXml = serviceProxy.dispatchCommand("100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("�˻���ѯ����:{0}",responseXml));
                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ResCode = responseCode;

                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                   
                    accountInfo = new AccountItem();
                    XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];
                    accountInfo.AccountNo = dataNode.Attributes["ACCOUNTNO"].Value;
                    accountInfo.AccountName = dataNode.Attributes["ACCOUNTNAME"].Value;
                    accountInfo.AccountType = dataNode.Attributes["ACCOUNTTYPE"].Value;
                    accountInfo.AccountStatus = dataNode.Attributes["ACCOUNTSTATUS"].Value;
                    accountInfo.AccountBalance = Convert.ToInt64(dataNode.Attributes["ACCOUNTBALANCE"].Value);
                    accountInfo.PredayBalance = Convert.ToInt64(dataNode.Attributes["PREDAYBALANCE"].Value);
                    accountInfo.PreMonthBalance = Convert.ToInt64(dataNode.Attributes["PREMONTHBALANCE"].Value);
                    accountInfo.AvailableBalance = Convert.ToInt64(dataNode.Attributes["AVAILABLEBALANCE"].Value);
                    accountInfo.UnAvailableBalance = Convert.ToInt64(dataNode.Attributes["UNAVAILABLEBALANCE"].Value);
                    accountInfo.AvailableLecash = Convert.ToInt64(dataNode.Attributes["AVAILABLECASH"].Value);
                    accountInfo.CardNum = dataNode.Attributes["CARDNUM"].Value;
                    accountInfo.CardType = dataNode.Attributes["CARDTYPE"].Value;
                }
                Result = 0;
                #endregion

            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                log(ex.ToString());
            }
            finally
            {

            }
            return Result;
        }


        /// <summary>
        /// �����޸Ľӿ� lihongtu
        /// </summary>
        public static Int32 ModifyBestPayPassword(String besttoneAccount, String oldPassword, String newPassword, String surePassword, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                //requestXml.Append("<CTRL-INFO WEBSVRNAME=\"֧�������޸�\" WEBSVRCODE=\"300030\" APPFROM=\"APP101\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"֧�������޸�\" WEBSVRCODE=\"300030\" APPFROM=\"300030|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>",besttoneAccount);
                requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1" );
                requestXml.AppendFormat("<OLDPASSWD>{0}</OLDPASSWD>", oldPassword);
                requestXml.AppendFormat("<NEWPASSWD>{0}</NEWPASSWD>", newPassword);
                requestXml.AppendFormat("<CONFIRMPASSWD>{0}</CONFIRMPASSWD>", surePassword);
                //BesttoneAccountConstDefinition.DefaultInstance


                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");
                requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
                requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
                requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);

                //requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", ACCEPTAREACODE);
                //requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", ACCEPTCITYCODE);

                requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");
                requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");
                requestXml.AppendFormat("<INPUTUID>{0}</INPUTUID>", "");
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.AppendFormat("<CHECKUID>{0}</CHECKUID>", "");
                requestXml.AppendFormat("<CHECKTIME>{0}</CHECKTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                log(String.Format("�޸���������:{0}:", requestXml));
                responseXml = serviceProxy.dispatchCommand("300030|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("�޸����뷵��:{0}:", responseXml));
                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                    ErrMsg = String.Empty;
                }

                #endregion
            }
            catch (Exception ex)
            {
                log("�޸������쳣:"+ex.ToString());
            }

            return Result;
        }

        /// <summary>
        /// ��ѯ��ʷ����(��ֵ�����ס��˿�) lihongtu
        /// </summary>
        public static Int32 QueryHistoryTxn(DateTime startDate, DateTime endDate, String besttoneAccount, String txnType, String txnChannel, Int32 maxReturnRecord, Int32 startRecord,
                                                            out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            txnItemList = null;
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�û���ʷ���ײ�ѯ\" WEBSVRCODE=\"100105\" APPFROM=\"100105|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy"+besttoneAccount);
                requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", txnType);
                requestXml.AppendFormat("<TXNCHANNEL>{0}</TXNCHANNEL>", txnChannel);
                requestXml.AppendFormat("<FROMDATE>{0}</FROMDATE>", startDate.ToString("yyyyMMdd"));
                requestXml.AppendFormat("<TODATE>{0}</TODATE>", endDate.ToString("yyyyMMdd"));
                if (maxReturnRecord == -1)
                {
                    requestXml.AppendFormat("<MaxRecord>{0}</MaxRecord>", "");
                }
                requestXml.AppendFormat("<MaxRecord>{0}</MaxRecord>", maxReturnRecord);
                requestXml.AppendFormat("<StartRecord>{0}</StartRecord>", startRecord);
                //BesttoneAccountConstDefinition.DefaultInstance
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                log(String.Format("�û���ʷ���ײ�ѯ����:{0}:", requestXml));
                responseXml = serviceProxy.dispatchCommand("100105|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("�û���ʷ���ײ�ѯ����:{0}:", responseXml));
                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;

                    txnItemList = new List<TxnItem>();
                    XmlNodeList dataNodes = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS");
                    foreach (XmlNode node in dataNodes)
                    {
                        TxnItem txnItem = new TxnItem();
                        txnItem.AcceptSeqNO = node.Attributes["ACCEPTSEQNO"].Value;
                        txnItem.AcceptDate = node.Attributes["ACCEPTDATE"].Value;
                        txnItem.AcceptTime = node.Attributes["ACCEPTTIME"].Value;
                        txnItem.TxnAmount = Convert.ToInt64(node.Attributes["TXNAMOUNT"].Value);
                        txnItem.TxnType = node.Attributes["TXNTYPE"].Value;
                        txnItem.TxnChannel = node.Attributes["TXNCHANNEL"].Value;
                        txnItem.MerchantName = node.Attributes["MERCHANTNAME"].Value;
                        txnItem.TxnDscpt = node.Attributes["TXNDSCPT"].Value;
                        txnItem.CancelFlag = node.Attributes["CANCELFLAG"].Value;

                        txnItemList.Add(txnItem);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
            finally { }

            return Result;
        }


        /// <summary>
        /// ��ѯ���ս���(��ֵ�����ס��˿�) lihontu
        /// </summary>
        public static Int32 QueryAllTypeTxn(String besttoneAccount, String txnType, String txnChannel,
                                                            out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            txnItemList = null;
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�û����ս��ײ�ѯ\" WEBSVRCODE=\"100104\" APPFROM=\"100104|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy"+besttoneAccount);
                requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", txnType);
                requestXml.AppendFormat("<TXNCHANNEL>{0}</TXNCHANNEL>", txnChannel);
                //BesttoneAccountConstDefinition.DefaultInstance

                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", ACCEPTORGCODE);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                log(String.Format("�û����ս��ײ�ѯ����:{0}:", requestXml));
                responseXml = serviceProxy.dispatchCommand("100104|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                log(String.Format("�û����ս��ײ�ѯ����:{0}:", responseXml));
                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;

                    txnItemList = new List<TxnItem>();
                    XmlNodeList dataNodes = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS");
                    foreach (XmlNode node in dataNodes)
                    {
                        TxnItem txnItem = new TxnItem();
                        txnItem.AcceptSeqNO = node.Attributes["ACCEPTSEQNO"].Value;   
                        txnItem.AcceptDate = node.Attributes["ACCEPTDATE"].Value;  
                        txnItem.AcceptTime = node.Attributes["ACCEPTTIME"].Value;
                        txnItem.TxnAmount = Convert.ToInt64(node.Attributes["TXNAMOUNT"].Value); 
                        txnItem.TxnType = node.Attributes["TXNTYPE"].Value;  
                        txnItem.TxnChannel = node.Attributes["TXNCHANNEL"].Value;    
                        txnItem.MerchantName = node.Attributes["MERCHANTNAME"].Value;  
                        txnItem.TxnDscpt = node.Attributes["TXNDSCPT"].Value; 
                        txnItem.CancelFlag = node.Attributes["CANCELFLAG"].Value;  

                        txnItemList.Add(txnItem);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
            finally { }

            return Result;
        }

        /// <summary>
        /// �������ͨ�˻���ѯ
        /// </summary>
        public static Int32 QueryBesttoneAccount(String besttoneAccount,out AccountItem accountInfo, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("�������ͨ�˻���ѯ��ڲ���:besttoneAccount:{0}\r\n", besttoneAccount);
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            accountInfo = null;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            //��ˮ��
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���
                strLog.AppendFormat("line 1204\r\n");
                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�˻���ѯ\" WEBSVRCODE=\"100100\" APPFROM=\"100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");
        
                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
                strLog.AppendFormat("line 1212,requestXml:{0}\r\n", requestXml);
                requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1");
                strLog.AppendFormat("line 1214,requestXml:{0}\r\n", requestXml);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                strLog.AppendFormat("line 1216,requestXml:{0}\r\n", requestXml);
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                strLog.AppendFormat("line 1219,requestXml:{0}\r\n", requestXml);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                strLog.AppendFormat("line 1221,requestXml:{0}\r\n", requestXml);
                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");
                strLog.AppendFormat("line 1224:{0}\r\n", requestXml);
                #endregion
                strLog.AppendFormat("requestXml={0}\r\n", requestXml);
                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

                strLog.AppendFormat("responseXml={0}\r\n", responseXml);

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                    accountInfo = new AccountItem();
                    XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];
                    accountInfo.AccountNo = dataNode.Attributes["ACCOUNTNO"].Value;
                    accountInfo.AccountName = dataNode.Attributes["ACCOUNTNAME"].Value;
                    accountInfo.AccountType = dataNode.Attributes["ACCOUNTTYPE"].Value;
                    accountInfo.AccountStatus = dataNode.Attributes["ACCOUNTSTATUS"].Value;
                    accountInfo.AccountBalance = Convert.ToInt64(dataNode.Attributes["ACCOUNTBALANCE"].Value);
                    accountInfo.PredayBalance = Convert.ToInt64(dataNode.Attributes["PREDAYBALANCE"].Value);
                    accountInfo.PreMonthBalance = Convert.ToInt64(dataNode.Attributes["PREMONTHBALANCE"].Value);
                    accountInfo.AvailableBalance = Convert.ToInt64(dataNode.Attributes["AVAILABLEBALANCE"].Value);
                    accountInfo.UnAvailableBalance = Convert.ToInt64(dataNode.Attributes["UNAVAILABLEBALANCE"].Value);
                    accountInfo.AvailableLecash = Convert.ToInt64(dataNode.Attributes["AVAILABLECASH"].Value);
                    accountInfo.CardNum = dataNode.Attributes["CARDNUM"].Value;
                    accountInfo.CardType = dataNode.Attributes["CARDTYPE"].Value;
                }

                #endregion

            }
            catch (Exception ex)
            {
                strLog.AppendFormat("�쳣:{0}\r\n",ex.ToString());
                //log("�˻���ѯ�쳣:" + ex.ToString());
            }
            finally
            {
                log(strLog.ToString());
            }
            return Result;
        }

        /// <summary>
        /// �˻�����ѯ
        /// </summary>
        public static Int32 QueryAccountBalance(String besttoneAccount, out long balance, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            balance = 0;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            //��ˮ��
            String TransactionID = CreateTransactionID();
            try
            {

                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"����ѯ\" WEBSVRCODE=\"100108\" APPFROM=\"100108|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
                requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1");
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("100108|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                    balance = Convert.ToInt64(xmlDoc.SelectNodes("/PayPlatResponseParameter/BALANCE")[0].InnerText);
                }
                else
                {
                    Result = Convert.ToInt32(responseCode);
                }

                #endregion

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return Result;
        }


        /// <summary>
        /// ������ѯ
        /// </summary>
        public static Int32 QueryCardBalance(String cardNo,String accountType, out long balance, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            balance = 0;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            //��ˮ��
            String TransactionID = CreateTransactionID();
            try
            {

                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"����ѯ\" WEBSVRCODE=\"100108\" APPFROM=\"100108|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", cardNo);
                requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", accountType);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("100108|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                    balance = Convert.ToInt64(xmlDoc.SelectNodes("/PayPlatResponseParameter/BALANCE")[0].InnerText);
                }
                else
                {
                    Result = Convert.ToInt32(responseCode);
                }

                #endregion

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return Result;
        }

        #endregion

        #region ������ѯ

        #region ���н��ײ�ѯ

        /// <summary>
        /// ��ѯ�������н��׶���
        /// </summary>
        public static Int32 QueryAllTxnCurrentDay(String besttoneAccount, 
                out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnCurrentDay(besttoneAccount, " ", "02", out txnItemList, out ErrMsg);

            return Result;
        }

        /// <summary>
        /// ��ʷ���׶�����ѯ
        /// </summary>
        public static Int32 QueryAllTxnHistory(DateTime startDate, DateTime endDate, String besttoneAccount, Int32 maxReturnRecord,Int32 startRecord, 
                out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnHistory(startDate, endDate, besttoneAccount, "", "02",  maxReturnRecord, startRecord, out txnItemList, out ErrMsg);

            return Result;
        }


        #endregion

        #region ���Ѷ�����ѯ

        /// <summary>
        /// ��ѯ�������Ѷ���
        /// </summary>
        public static Int32 QueryBusinessTxnCurrentDay(String besttoneAccount, out IList<TxnItem> txnItemList, 
                out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnCurrentDay(besttoneAccount, "131090", "02", out txnItemList, out ErrMsg);

            return Result;
        }

        /// <summary>
        /// ��ʷ���Ѷ�����ѯ
        /// </summary>
        public static Int32 QueryBusinessTxnHistory(DateTime startDate, DateTime endDate, String besttoneAccount, Int32 maxReturnRecord, Int32 startRecord, 
                out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnHistory(startDate, endDate, besttoneAccount, "131090", "02", maxReturnRecord, startRecord, out txnItemList, out ErrMsg);

            return Result;
        }

        #endregion

        #region ��ֵ������ѯ

        /// <summary>
        /// ��ѯ�����ֵ����
        /// </summary>
        public static Int32 QueryRechargeTxnCurrentDay(String besttoneAccount, out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnCurrentDay(besttoneAccount, "121080", "02", out txnItemList, out ErrMsg);

            return Result;
        }

        /// <summary>
        /// ��ѯ��ʷ��ֵ����
        /// </summary>
        public static Int32 QueryRechargeTxnHistory(DateTime startDate, DateTime endDate, String besttoneAccount, Int32 maxReturnRecord, Int32 startRecord, 
                                                            out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnHistory(startDate, endDate, besttoneAccount, "121080", "02", maxReturnRecord, startRecord, out txnItemList, out ErrMsg);


            return Result;
        }


        #endregion

        #region �˿����ѯ

        /// <summary>
        /// ��ѯ�����˿��
        /// </summary>
        public static Int32 QueryRefundTxnICurrentDay(String besttoneAccount, 
                                                            out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnCurrentDay(besttoneAccount, "131030", "02", out txnItemList, out ErrMsg);

            return Result;
        }

        /// <summary>
        /// ��ʷ�˿����ѯ
        /// </summary>
        public static Int32 QueryRefundTxnHistory(DateTime startDate, DateTime endDate, String besttoneAccount, Int32 maxReturnRecord, Int32 startRecord, 
                                                            out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            Result = QueryTxnHistory(startDate, endDate, besttoneAccount, "131030", "02", maxReturnRecord, startRecord, out txnItemList, out ErrMsg);

            return Result;
        }


        #endregion

        #region ˽�з���

        /// <summary>
        /// ��ѯ���ս���(��ֵ�����ס��˿�)
        /// </summary>
        private static Int32 QueryTxnCurrentDay(String besttoneAccount, String txnType, String txnChannel, 
                                                            out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            txnItemList = null;
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�û����ս��ײ�ѯ\" WEBSVRCODE=\"100104\" APPFROM=\"100104|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
                requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", txnType);
                requestXml.AppendFormat("<TXNCHANNEL>{0}</TXNCHANNEL>", txnChannel);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("100104|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;

                    txnItemList = new List<TxnItem>();
                    XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET")[0];
                    if (dataNode != null)
                    {
                        foreach (XmlNode node in dataNode.ChildNodes)
                        {
                            TxnItem txnItem = new TxnItem();
                            txnItem.AcceptSeqNO = node.Attributes["ACCEPTSEQNO"].Value;
                            txnItem.AcceptDate = node.Attributes["ACCEPTDATE"].Value;
                            txnItem.AcceptTime = node.Attributes["ACCEPTTIME"].Value;
                            txnItem.TxnAmount = Convert.ToInt64(node.Attributes["TXNAMOUNT"].Value);
                            txnItem.TxnType = node.Attributes["TXNTYPE"].Value;
                            txnItem.TxnChannel = node.Attributes["TXNCHANNEL"].Value;
                            txnItem.MerchantName = node.Attributes["MERCHANTNAME"].Value;
                            txnItem.TxnDscpt = node.Attributes["TXNDSCPT"].Value;
                            txnItem.CancelFlag = node.Attributes["CANCELFLAG"].Value;

                            txnItemList.Add(txnItem);
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {

            }
            finally { }

            return Result;
        }

        /// <summary>
        /// ��ʷ���ײ�ѯ(��ֵ�����ס��˿�)
        /// </summary>
        private static Int32 QueryTxnHistory(DateTime startDate, DateTime endDate, String besttoneAccount, String txnType, String txnChannel, Int32 maxReturnRecord, Int32 startRecord, 
                                                            out IList<TxnItem> txnItemList, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            txnItemList = null;
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"�û���ʷ���ײ�ѯ\" WEBSVRCODE=\"100105\" APPFROM=\"100105|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
                requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", txnType);
                requestXml.AppendFormat("<TXNCHANNEL>{0}</TXNCHANNEL>", txnChannel);
                requestXml.AppendFormat("<FROMDATE>{0}</FROMDATE>", startDate.ToString("yyyyMMdd"));
                requestXml.AppendFormat("<TODATE>{0}</TODATE>", endDate.ToString("yyyyMMdd"));
                if (maxReturnRecord == 0)
                    requestXml.Append("<MaxRecord></MaxRecord>");
                else
                    requestXml.AppendFormat("<MaxRecord>{0}</MaxRecord>", maxReturnRecord);
                if (startRecord == 0)
                    requestXml.Append("<StartRecord></StartRecord>");
                else
                    requestXml.AppendFormat("<StartRecord>{0}</StartRecord>", startRecord);

                //requestXml.AppendFormat("<MaxRecord>{0}</MaxRecord>", maxReturnRecord);
                //requestXml.AppendFormat("<StartRecord>{0}</StartRecord>", startRecord);

                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("100105|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;

                    txnItemList = new List<TxnItem>();
                    XmlNode dataNode = xmlDoc.SelectSingleNode("PayPlatResponseParameter/RESULTDATESET");
                    if (dataNode != null)
                    {
                        foreach (XmlNode node in dataNode.ChildNodes)
                        {
                            TxnItem txnItem = new TxnItem();
                            txnItem.AcceptSeqNO = node.Attributes["ACCEPTSEQNO"].Value;
                            txnItem.AcceptDate = node.Attributes["ACCEPTDATE"].Value;
                            txnItem.AcceptTime = node.Attributes["ACCEPTTIME"].Value;
                            txnItem.TxnAmount = Convert.ToInt64(node.Attributes["TXNAMOUNT"].Value);
                            txnItem.TxnType = node.Attributes["TXNTYPE"].Value;
                            txnItem.TxnChannel = node.Attributes["TXNCHANNEL"].Value;
                            txnItem.MerchantName = node.Attributes["MERCHANTNAME"].Value;
                            txnItem.TxnDscpt = node.Attributes["TXNDSCPT"].Value;
                            txnItem.CancelFlag = node.Attributes["CANCELFLAG"].Value;

                            txnItemList.Add(txnItem);
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {

            }
            finally { }

            return Result;
        }

        #endregion

        #endregion

        #region ֧�����������

        /// <summary>
        /// �����޸Ľӿ�
        /// </summary>
        public static Int32 ModifyPayPassword(String besttoneAccount, String oldPassword, String newPassword,String surePassword, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"֧�������޸�\" WEBSVRCODE=\"300030\" APPFROM=\"300030|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", besttoneAccount);
                requestXml.AppendFormat("<OLDPASSWD>{0}</OLDPASSWD>", oldPassword);
                requestXml.AppendFormat("<NEWPASSWD>{0}</NEWPASSWD>", newPassword);
                requestXml.AppendFormat("<CONFIRMPASSWD>{0}</CONFIRMPASSWD>", surePassword);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
                requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
                requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);
                requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");
                requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");
                requestXml.AppendFormat("<INPUTUID>{0}</INPUTUID>", "");
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.AppendFormat("<CHECKUID>{0}</CHECKUID>", "");
                requestXml.AppendFormat("<CHECKTIME>{0}</CHECKTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("300030|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").Value;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").Value;
                if (responseCode == "000000")
                {
                    Result = 0;
                    ErrMsg = String.Empty;
                }

                #endregion
            }
            catch (Exception ex)
            { 
                
            }

            return Result;
        }

        /// <summary>
        /// �������ýӿ�
        /// </summary>
        public static Int32 ResetPayPassword(String besttoneAccount,String cerType,String cerNo,String name, out String ErrMsg)
        { 
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"��������\" WEBSVRCODE=\"200101\" APPFROM=\"200101|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", besttoneAccount);
                requestXml.AppendFormat("<IDTYPE>{0}</IDTYPE>", cerType);
                requestXml.AppendFormat("<IDNO>{0}</IDNO>", cerNo);
                requestXml.AppendFormat("<CUSTOMERNAME>{0}</CUSTOMERNAME>", "");
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
                requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
                requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);
                requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");
                requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");
                requestXml.AppendFormat("<INPUTUID>{0}</INPUTUID>", "");
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requestXml.AppendFormat("<CHECKUID>{0}</CHECKUID>", "");
                requestXml.AppendFormat("<CHECKTIME>{0}</CHECKTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("200101|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                    ErrMsg = String.Empty;
                }

                #endregion
            }
            catch (Exception ex)
            {

            }

            return Result;
        }

        #endregion

        #region ��ֵ|�ۿ�

        /// <summary>
        /// ����Ȩ�����ޡ�
        /// </summary>
        public static Int32 CardAuthen(String cardNo, String password, String cardType, out long balance, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            balance = 0;

            return Result;
        }

        /// <summary>
        /// ���ۿ�����ṩ�ӿڡ�
        /// </summary>
        public static Int32 CardDeductionBalance(String transactionID,String orderSeq,String cardNo, String cardPwd, String cardType,
            long tranAmount,DateTime reqTime, String orderDesc,out String out_UptranSeq, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("����ʼ���ۿDateTime:{0}��\r\n[����]��", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            strLog.AppendFormat("TransactionID:{0},OrderSeq:{1},CardNo:{2},CardType:{3},TranAmount:{4},ReqTime:{5},OrderDesc:{6},",
                transactionID, orderSeq, cardNo, cardType, tranAmount, reqTime.ToString("yyyy-MM-dd HH:mm:ss"), orderDesc);

            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            out_UptranSeq = String.Empty;

            RechargeBackRecordDAO _rechargeRecord_dao = new RechargeBackRecordDAO();
            RechargeBackRecord _rechargeRecord_entity = null;
            try
            {
                //���ô���
                //WebProxy proxy = new WebProxy();
                //proxy.BypassProxyOnLocal = false;
                //proxy.Credentials = new NetworkCredential("wudan", "abcd_123", "yellowpage");
                //GlobalProxySelection.Select = proxy;

                //3DES������
                String cardInfo = String.Format("cardNo={0}&password={1}", cardNo, cardPwd);
                strLog.AppendFormat("CardNo:{0},", cardInfo);

                cardInfo = DESEncrypt(cardInfo, BesttoneAccountConstDefinition.DefaultInstance.COMMKEY + "0000000000000000");
                strLog.AppendFormat("CardNoEncrypt:{0},", cardInfo);

                //MD5ǩ��
                String mac = String.Format("COMMCODE={0}&COMMPWD={1}&ORDID={2}&ORDPAYID={3}&REQTIME={4}&TRANSAMT={5}&KEY={6}",BesttoneAccountConstDefinition.DefaultInstance.COMMCODE,
                    BesttoneAccountConstDefinition.DefaultInstance.COMMPWD, orderSeq, transactionID, reqTime.ToString("yyyyMMddHHmmss"), tranAmount, BesttoneAccountConstDefinition.DefaultInstance.COMMKEY);
                strLog.AppendFormat("Mac:{0},", mac);
                mac = BitConverter.ToString(MD5Encrypt(mac)).Replace("-", "");
                strLog.AppendFormat("MacEncrypt:{0}\r\n", mac);
                
                NameValueCollection collection = new NameValueCollection();
                collection.Add("COMMCODE", BesttoneAccountConstDefinition.DefaultInstance.COMMCODE);
                collection.Add("SUBCOMMCODE", "");
                collection.Add("COMMPWD", BesttoneAccountConstDefinition.DefaultInstance.COMMPWD);
                collection.Add("CARDTYPE", cardType);
                collection.Add("ORDID", orderSeq);
                collection.Add("ORDPAYID", transactionID);
                collection.Add("TRANSAMT", tranAmount.ToString());
                collection.Add("CARDINFOENC", cardInfo);
                collection.Add("REQTIME", reqTime.ToString("yyyyMMddHHmmss"));
                collection.Add("ORDERVALIDITYTIME", "20340513111500");
                collection.Add("PRODUCTDESC", "");
                collection.Add("ATTACH", "");
                collection.Add("CUSTOMERIP", "228.112.116.118");
                collection.Add("CUSTOMERACCOUNT", "tylzhuang@gmail.com");
                collection.Add("CUSTOMERTELE", "");
                collection.Add("MAC", mac);

                WebClient client = new WebClient();
                byte[] info = client.UploadValues(BesttoneAccountConstDefinition.DefaultInstance.CardPayHttpsPage, collection);
                responseXml = System.Text.Encoding.UTF8.GetString(info);

                strLog.AppendFormat("[���ر���]:{0}\r\n", responseXml);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                #region ������������

                String uptranSeq = xmlDoc.SelectNodes("/result/UPTRANSEQ")[0].InnerText;
                String tranDate = xmlDoc.SelectNodes("/result/TRANDATE")[0].InnerText;
                String responseCode = xmlDoc.SelectNodes("/result/RETURNCODE")[0].InnerText;
                String responseMsg = xmlDoc.SelectNodes("/result/RETNINFO")[0].InnerText;
                //String balance = xmlDoc.SelectNodes("/result/BALANCE")[0].InnerText;
                //String rechargeAmount = xmlDoc.SelectNodes("/result/RECHARGEAMOUNT")[0].InnerText;
                String sign = xmlDoc.SelectNodes("/result/SIGN")[0].InnerText;

                out_UptranSeq = uptranSeq;

                //��¼���ط�����ˮ��¼
                //_rechargeRecord_entity = new RechargeBackRecord();
                //_rechargeRecord_entity.UptranSeq = uptranSeq;
                //_rechargeRecord_entity.TranDate = tranDate.Substring(0, 8);
                //_rechargeRecord_entity.RetnCode = responseCode;
                //_rechargeRecord_entity.RetnInfo = responseMsg;
                //_rechargeRecord_entity.OrderTransactionID = transactionID;
                //_rechargeRecord_entity.OrderSeq = orderSeq;
                //_rechargeRecord_entity.EncodeType = "1";
                //_rechargeRecord_entity.Sign = sign;
                //_rechargeRecord_entity.RechargeType = cardType;

                //��֤ǩ��:16����ת��(MD5����)
                String newSign = String.Format("RETURNCODE={0}&UPTRANSEQ={1}&TRANDATE={2}&COMMCODE={3}&ORDID={4}&ORDPAYID={5}&KEY={6}",
                    responseCode, uptranSeq, tranDate, BesttoneAccountConstDefinition.DefaultInstance.COMMCODE, orderSeq, transactionID, BesttoneAccountConstDefinition.DefaultInstance.COMMKEY);
                String md5EncodingSign = BesttoneAccountHelper.MACSign(newSign);
                if (sign != md5EncodingSign)
                {
                    Result = -1;
                    ErrMsg = "ǩ������";
                    return Result;
                }

                if (responseCode == "0000")
                {
                    Result = 0;
                    ErrMsg = "";
                }
                else
                {
                    Result = Convert.ToInt32(responseCode);
                }

                #endregion
            }
            catch (Exception ex)
            {
                Result = -3024;
                ErrMsg = ex.Message;
                strLog.AppendFormat("�쳣��Ϣ:{0},", ex.Message);
            }
            finally
            {
                log("CardDeductionBalance", strLog.ToString());
                //if (_rechargeRecord_entity != null)
                //    _rechargeRecord_dao.Insert(_rechargeRecord_entity);
            }
            return Result;
        }

        /// <summary>
        /// ���������У��
        /// </summary>
        /// <param name="CardNo">0-�������ͨ��;1-����ͨ��</param>
        /// <param name="CardType"></param>
        /// <returns></returns>
        public static Int32 VerifyCardNo(String CardNo,out String ErrMsg)
        {
           //�������ͨ��λ��Ϊ16λ���������ǰ4λΪ8888����5��6λΪ00����7-15λΪ���кţ���16λΪ�����֤�롣
           Int32 Result = 0;
           ErrMsg = "";
           Regex regCardNo = new Regex(@"888800\d{9}\w{1}$");
           if (!regCardNo.IsMatch(CardNo))
           {
               Result = -1;
               ErrMsg = "������Ŀ��Ų���ȷ����˶Ժ��ٴ����룬лл��";
           }
          return Result;
        }


        /// <summary>
        /// �������ͨ�˻���ֵ�ӿ�
        /// </summary>
        public static Int32 AccountRecharge(String transactionID, String besttoneAccount, long balance, DateTime reqTime, out long currentBalance, out string ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("���˻���ֵ��DateTime:{0}��\r\n[����]:", reqTime.ToString("yyyyMMdd HH:mm:ss"));
            strLog.AppendFormat("BesttoneAccount:{0},Balance:{1},ReqTime:{2}\r\n", besttoneAccount, balance, reqTime.ToString("yyyyMMdd HH:mm:ss"));

            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            currentBalance = 0;

            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            //��ˮ��
            //String TransactionID = CreateTransactionID();
            try
            {
                #region ƴ������xml�ַ���

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"��ֵ(Ӫҵ��)\" WEBSVRCODE=\"200401\" APPFROM=\"200401|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + transactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //��Ӳ���
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
                requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1");
                requestXml.AppendFormat("<BUSINESSTYPE>{0}</BUSINESSTYPE>", "1200");
                requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", "121080");
                requestXml.AppendFormat("<TXNAMOUNT>{0}</TXNAMOUNT>", balance);
                requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", 1);
                requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", 0);
                requestXml.AppendFormat("<TRANSFERORGCODE>{0}</TRANSFERORGCODE>", "");
                requestXml.AppendFormat("<PAYORGCODE>{0}</PAYORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<SUPPLYORGCODE>{0}</SUPPLYORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.SUPPLYORGCODE);
                requestXml.AppendFormat("<TERMINALSEQNO>{0}</TERMINALSEQNO>", transactionID);
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
                requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
                requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
                requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);
                requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", transactionID);
                requestXml.AppendFormat("<ACCEPTTRANSDATE>{0}</ACCEPTTRANSDATE>", reqTime.ToString("yyyyMMdd"));
                requestXml.AppendFormat("<ACCEPTTRANSTIME>{0}</ACCEPTTRANSTIME>", reqTime.ToString("HHmmss"));
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", reqTime.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                strLog.AppendFormat("[������]:{0}\r\n", requestXml.ToString());
                #endregion

                //����ӿ�
                responseXml = serviceProxy.dispatchCommand("200401|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                strLog.AppendFormat("[���ر���]:{0}\r\n", responseXml);

                #region �����ӿڷ��ز���

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
                ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
                if (responseCode == "000000")
                {
                    Result = 0;
                    currentBalance = Convert.ToInt64(xmlDoc.SelectNodes("/PayPlatResponseParameter/BALANCE")[0].InnerText);
                }
                else
                {
                    Result = Convert.ToInt32(responseCode);
                }

                #endregion

            }
            catch (Exception ex)
            {
                Result = -3025;
                strLog.AppendFormat("�쳣��Ϣ:{0}", ex.Message);
            }
            finally
            {
                log("AccountRecharge", strLog.ToString());
            }
            return Result;
        }

        #endregion

        #endregion

        #region ��������

        /// <summary>
        /// ����20λ���������ˮ��:35+yyMMddHHmmss+�����(6λ)
        /// </summary>
        public static String CreateTransactionID()
        {
            String date = DateTime.Now.ToString("yyyyMMddHHmmss");

            //6λ�����
            Random r = new Random(Guid.NewGuid().GetHashCode());
            String TransactionID = "00" + date + r.Next(1000, 9999).ToString();

            return TransactionID;
        }

        /// <summary>
        /// ���ɶ�����
        /// </summary>
        /// <returns></returns>
        public static String CreateOrderSeq()
        {
            String date = DateTime.Now.ToString("yyyyMMddHHmmss");

            //6λ�����
            Random r = new Random(Guid.NewGuid().GetHashCode());
            //String TransactionID = "CIP35" + date + r.Next(100000, 999999).ToString();
            String TransactionID = "11" + date + r.Next(1000, 9999).ToString();

            return TransactionID;
        }

        /// <summary>
        /// ������ת��ΪԪ(����2ΪС��)
        /// </summary>
        public static String ConvertAmountToYuan(long amount)
        {
            double amount_new = amount / 100.0;
            return amount_new.ToString("F2");
        }

        /// <summary>
        /// ��������ת��
        /// </summary>
        public static String ConvertTxnType(String txnType)
        {
            String returnType = "����";
            switch (txnType)
            {
                //����
                //case "121020":
                //    returnType = "��ֵ";
                //    break;
                //case "131010":
                //    returnType = "����";
                //    break;
                //case "131030":
                //    returnType = "�˿�";
                //    break;
                //default:
                //    returnType = "����";
                //    break;

                //��ʽ
                case "121080":
                    returnType = "��ֵ";
                    break;
                case "131090":
                    returnType = "����";
                    break;
                case "261010":
                    returnType = "�˿�";
                    break;
                case "261020":
                    returnType = "�˿�������";
                    break;
                default:
                    returnType = "����";
                    break;
            }

            return returnType;
        }

        /// <summary>
        /// �ʻ�����ת��
        /// </summary>
        public static String ConvertAccountType(String cardType)
        {
            switch (cardType)
            { 
                    
                case "1":
                    return "1";     //�˻�
                case "2":
                    return "Y";     //���ο����������ͨ����
                case "3":
                    return "A";     //���¹���
                default:
                    return "";
            }
        }

        #endregion

        #region ����

        //private static String APANAGE = "101310000000000";              //�������ࣨ1λ��+�������ͣ�2λ��+�������루4λ��+���أ�2��+000000
        //private static String ACCEPTORGCODE = "002310000000000";        //�����������
        //private static String SUPPLYORGCODE = "113310000000000";        //��������
        //private static String AREACODE = "310000";                      //
        //private static String ACCEPTAREACODE = "310000";                //�����������
        //private static String CITYCODE = "310100";                      //
        //private static String ACCEPTCITYCODE = "310100";                //������д���

        //���س���
        //private static String COMMCODE = "0018888888";                  //�̻�����
        //private static String COMMPWD = "123321";                       //�̻���������
        //private static String COMMKEY = "G7AXS7874305BV59";             //3DES����key
        //private static byte[] IV ={ 50, 51, 52, 53, 54, 55, 56, 57 };

        #endregion

        #region ����

        /// <summary>
        /// 3DES����
        /// </summary>
        public static String DESEncrypt(String sourceStr,String key)
        {
            String returnValue = "";
            byte[] input = null;
            byte[] output = null;
            byte[] bkey = null;
            try
            {
                input = Encoding.UTF8.GetBytes(sourceStr);
                bkey = Convert.FromBase64String(key);

                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.Zeros;
                //�����ԳƵļ��ܶ���
                ICryptoTransform encryptObj = des.CreateEncryptor(bkey, BesttoneAccountConstDefinition.DefaultInstance.COMMIV);
                output = encryptObj.TransformFinalBlock(input, 0, input.Length);

                des.Clear();
                //ת��Ϊ64λ����
                returnValue = Convert.ToBase64String(output);
                returnValue = BitConverter.ToString(Encoding.UTF8.GetBytes(returnValue)).Replace("-", "");
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        /// <summary>
        /// MD5����
        /// </summary>
        public static byte[] MD5Encrypt(String sourceStr)
        {
            byte[] input = null;
            byte[] output = null;
            input = Encoding.UTF8.GetBytes(sourceStr);
            output = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(input);

            return output;
        }

        /// <summary>
        /// MACǩ��
        /// </summary>
        public static String MACSign(String sourceStr)
        {
            return BitConverter.ToString(MD5Encrypt(sourceStr)).Replace("-", "");
        }

        #endregion




        private static void log(string str)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneAccountHelper", msg);
        }

        private static void log(string methodname,string content)
        {
            try
            {
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                msg.Append(content);
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneAccountHelper" + @"\" + methodname, msg);
            }
            catch { }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class BankAccount
    {
        #region ��֧���˺������ϴ��ӿ�ʹ��
        //���캯��
        public void ParseXML(string RequestXML)
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";
            //�����������
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RequestXML);

            //string xPathExpression1 = "CIPRoot/SessionHeader/Version";
            //XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            //if (selecNode == null)
            //{
            //    errMsg = "��������,��Version���";
            //    result = -21500;
            //    return ;
            //}

            //versionNo = selecNode.InnerText;
            string xPathExpression1 = "CIPRoot/SPID";
            XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��SPID���";
                result = -21500;
                return ;
            }
            spID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/CustID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��CustID���";
                result = -21500;
                return ;
            }
            custID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/PaymentSequenceID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��PaymentSequenceID���";
                result = -21500;
                return ;
            }
            paymentSequenceID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/PaymentAccount";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��PaymentAccount���";
                result = -21500;
                return ;
            }
            paymentAccount = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/BankID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��BankID���";
                result = -21500;
                return ;
            }
            bankID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Last4No";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��Last4No���";
                result = -21500;
                return ;
            }
            last4No = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Source";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��Source���";
                result = -21500;
                return;
            }
            source = selecNode.InnerText;


            
            result = 0;
        }

        /// <summary>
        /// �����������������У��
        /// </summary>
        public void DataCheck()
        {
            //��ʼ���������
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //���ݺϷ����ж�
            if (CommonUtility.IsEmpty(spID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "����Ϊ��";
                return ;
            }
            if (spID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "��������";
                return ;
            }

            if (CommonUtility.IsEmpty(custID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "����Ϊ��";
                return ;
            }

            if (custID.Length > ConstDefinition.Length_CustID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "��������";
                return;
            }
            if(CommonUtility.IsEmpty(paymentSequenceID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "֧����ˮ�ʺŲ���Ϊ��";
                return;
            }
            if (CommonUtility.IsEmpty(paymentAccount))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "���п��Ų���Ϊ��";
                return;
            }
            if (CommonUtility.IsEmpty(bankID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "���д��벻��Ϊ��";
                return;
            }
            if (CommonUtility.IsEmpty(last4No))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "���ÿ���4λ����Ϊ��";
                return;
            }
            if(last4No.Length!=4)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "���ÿ���4λ���Ȳ���";
                return;
            }

            if (!("1".Equals(source) || "2".Equals(source)))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "��Դ���Ͳ�����";
                return;
            }
            result = 0;
            errMsg = "";
        }

        /// <summary>
        /// ��֧���˺�����ʱ��
        /// </summary>
        public void InsertPaymentAccountTMP()
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //�������
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_InsertPaymentAccount";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = spID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parPaymentSequenceID = new SqlParameter("@PaymentSequenceID", SqlDbType.VarChar, 30);
                parPaymentSequenceID.Value = paymentSequenceID;
                cmd.Parameters.Add(parPaymentSequenceID);

                SqlParameter parPaymentAccount = new SqlParameter("@PaymentAccount", SqlDbType.VarChar, 512);
                parPaymentAccount.Value = paymentAccount;
                cmd.Parameters.Add(parPaymentAccount);

                SqlParameter parBankID = new SqlParameter("@BankID", SqlDbType.VarChar, 20);
                parBankID.Value = bankID;
                cmd.Parameters.Add(parBankID);

                SqlParameter parLast4No = new SqlParameter("@Last4No", SqlDbType.VarChar, 4);
                parLast4No.Value = last4No;
                cmd.Parameters.Add(parLast4No);

                SqlParameter parSource = new SqlParameter("@Source", SqlDbType.VarChar, 1);
                parSource.Value = source;
                cmd.Parameters.Add(parSource);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parUploadTime = new SqlParameter("@UploadTime", SqlDbType.DateTime);
                parUploadTime.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUploadTime);

               

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                result = int.Parse(parResult.Value.ToString());
                uploadTime = parUploadTime.Value.ToString();
                errMsg = parErrorDescription.Value.ToString();

            }
            catch (System.Exception ex)
            {
                result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                errMsg = ex.Message;
            }

        }

        #endregion

        #region ��֧���˺�״̬֪ͨ�ӿ�ʹ��
        //���캯��
        public void ParseXMLStatusNotify(string RequestXML)
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";
            //�����������
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RequestXML);

            //string xPathExpression1 = "CIPRoot/SessionHeader/Version";
            //XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            //if (selecNode == null)
            //{
            //    errMsg = "��������,��Version���";
            //    result = -21500;
            //    return;
            //}

            //versionNo = selecNode.InnerText;


            string xPathExpression1 = "CIPRoot/SPID";
            XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��SPID���";
                result = -21500;
                return;
            }
            spID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/CustID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��CustID���";
                result = -21500;
                return;
            }
            custID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/PaymentSequenceID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��PaymentSequenceID���";
                result = -21500;
                return;
            }
            paymentSequenceID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Source";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��Source���";
                result = -21500;
                return;
            }
            source = selecNode.InnerText;


            xPathExpression1 = "CIPRoot/Status";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��Status���";
                result = -21500;
                return;
            }
            status = selecNode.InnerText;

           

            result = 0;
        }

        /// <summary>
        /// �����������������У��
        /// </summary>
        public void DataCheckStatusNotify()
        {
            //��ʼ���������
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //���ݺϷ����ж�
            if (CommonUtility.IsEmpty(spID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "����Ϊ��";
                return;
            }
            if (spID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "��������";
                return;
            }

            if (CommonUtility.IsEmpty(custID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "����Ϊ��";
                return;
            }

            if (custID.Length > ConstDefinition.Length_CustID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "��������";
                return;
            }
            if (CommonUtility.IsEmpty(paymentSequenceID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "֧����ˮ�ʺŲ���Ϊ��";
                return;
            }

       

            if (!("1".Equals(source) || "2".Equals(source)))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "��Դ���Ͳ�����";
                return;
            }

            if (CommonUtility.IsEmpty(status))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "֧��״̬����Ϊ��";
                return;
            }

            if (!("0".Equals(status) || "-1".Equals(status) || "-2".Equals(status)))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "֧��״̬���Ͳ�����";
                return;
            }
            result = 0;
            errMsg = "";
        }

        /// <summary>
        /// ��֧���˺�����ʱ��
        /// </summary>
        public void InsertStatusNotify()
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //�������
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_InsertPaymentStatusNotify";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = spID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parPaymentSequenceID = new SqlParameter("@PaymentSequenceID", SqlDbType.VarChar, 30);
                parPaymentSequenceID.Value = paymentSequenceID;
                cmd.Parameters.Add(parPaymentSequenceID);

                SqlParameter parSource = new SqlParameter("@Source", SqlDbType.VarChar, 1);
                parSource.Value = source;
                cmd.Parameters.Add(parSource);

                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Value = status;
                cmd.Parameters.Add(parStatus);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription ", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                result = int.Parse(parResult.Value.ToString());
                errMsg = parErrorDescription.Value.ToString();
            }
            catch (System.Exception ex)
            {
                result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                errMsg = ex.Message;
            }

        }

        #endregion

        #region ��֧���˺Ų�ѯ�ӿ�ʹ��
        //���캯��
        public void ParseXMLAccountQuery(string RequestXML)
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";
            //�����������
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RequestXML);

            //string xPathExpression1 = "CIPRoot/SessionHeader/Version";
            //XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            //if (selecNode == null)
            //{
            //    errMsg = "��������,��Version���";
            //    result = -21500;
            //    return;
            //}

            //versionNo = selecNode.InnerText;
            string xPathExpression1 = "CIPRoot/SPID";
            XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��SPID���";
                result = -21500;
                return;
            }
            spID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/CustID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��CustID���";
                result = -21500;
                return;
            }
            custID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Source";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "��������,��Source���";
                result = -21500;
                return;
            }
            source = selecNode.InnerText;

            result = 0;
        }

        /// <summary>
        /// �����������������У��
        /// </summary>
        public void DataCheckAccountQuery()
        {
            //��ʼ���������
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //���ݺϷ����ж�
            if (CommonUtility.IsEmpty(spID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "����Ϊ��";
                return;
            }
            if (spID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "��������";
                return;
            }

            if (CommonUtility.IsEmpty(custID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "����Ϊ��";
                return;
            }

            if (custID.Length > ConstDefinition.Length_CustID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "��������";
                return;
            }
           
            result = 0;
            errMsg = "";
        }

        /// <summary>
        /// ��֧���˺�����ʱ��
        /// </summary>
        public void InsertAccountQuery()
        { 
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

           
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_InsertPaymentAccountQuery";
                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = spID;
                cmd.Parameters.Add(parSPID);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSource = new SqlParameter("@Source", SqlDbType.VarChar, 1);
                parSource.Value = CustID;
                cmd.Parameters.Add(parSource);





                DataSet dt = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                paymentAccountRecord = "<PaymentAccountRecords>";

                //if (dt.Tables[0].Rows.Count == 0)
                //{
                //    result = -210005;
                //    errMsg = "�����ݣ�";
                //    return;
                //}

                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    paymentAccountRecord += "<PaymentAccountRecord>";
                    paymentAccountRecord += "<PaymentAccount>" + dt.Tables[0].Rows[i]["PaymentAccount"].ToString() + "</PaymentAccount><BankID>" 
                        + dt.Tables[0].Rows[i]["BankID"].ToString() + "</BankID><BackNo>"
                        + dt.Tables[0].Rows[i]["BackNo"].ToString() + "</BackNo><DealTime>" + dt.Tables[0].Rows[i]["DealTime"].ToString() + "</DealTime><Source>" 
                        + dt.Tables[0].Rows[i]["Source"].ToString()+"</Source>" ;
                    paymentAccountRecord += "</PaymentAccountRecord>";
                }

                paymentAccountRecord += "</PaymentAccountRecords>";
                result = 0;
                errMsg = "";
              
            }
            catch (System.Exception ex)
            {
                paymentAccountRecord = "";
                result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                errMsg = ex.Message;
            }

        }

        /// <summary>
        /// ���ɽ��XML
        /// </summary>
        /// <returns></returns>
        public string GenerateAccountQueryResultXML()
        {
            string resultXML = "";

            try
            {
                XmlElement XmlRoot = null;
                XmlElement XmlRoot1 = null;
                XmlDocument XmlDom = new XmlDocument();
                XmlDocument XmlDom1 = new XmlDocument();

                string Result1 = "<CIPRoot></CIPRoot>";
                XmlDom.LoadXml(Result1);
                XmlRoot = XmlDom.DocumentElement;

     

                //XmlElement xe1 = XmlDom.CreateElement("SessionHeader");//����һ��<SessionHeader>�ڵ� 

               // XmlElement xe2 = XmlDom.CreateElement("SessionBody");//����һ��<SessionBody>�ڵ� 

               // XmlElement xesub1 = XmlDom.CreateElement("Version");
               // xesub1.InnerText = versionNo;//�����ı��ڵ� 
               // xe1.AppendChild(xesub1);//��ӵ�<SessionHeader>�ڵ��� 
               // XmlElement xesub2 = XmlDom.CreateElement("SPID");
               // xesub2.InnerText = spID;
               // xe1.AppendChild(xesub2);

                //XmlRoot.AppendChild(xe1);

         
                XmlElement xesub3 = XmlDom.CreateElement("Result");
                xesub3.InnerText = result.ToString();//�����ı��ڵ� 
                XmlRoot.AppendChild(xesub3);//��ӵ�<SessionBody>�ڵ��� 

              
                XmlDom1.LoadXml(paymentAccountRecord);
                XmlRoot1 = XmlDom1.DocumentElement;

                XmlNode _node2 = XmlDom1.DocumentElement;
                XmlNode _nodeCopy = XmlDom.CreateNode(XmlNodeType.Element, _node2.Name, XmlDom.DocumentElement.NamespaceURI);
                _nodeCopy.InnerXml = _node2.InnerXml;
                XmlDom.DocumentElement.AppendChild(_nodeCopy); 

                //XmlRoot.AppendChild(nB);

                XmlElement xesub4 = XmlDom.CreateElement("ErrorDescription");
                xesub4.InnerText = errMsg;
                XmlRoot.AppendChild(xesub4);
                //XmlRoot.AppendChild(xe2);

                resultXML = XmlDom.OuterXml;
            }
            catch (Exception ex)
            {
                resultXML = ex.Message;
            }


            return resultXML;
        }
        #endregion

        /// <summary>
        /// ���ɽ��XML
        /// </summary>
        /// <returns></returns>
        public string GenerateResultXML()
        {
            string resultXML = "";

            try
            {
                XmlElement XmlRoot = null;
                XmlDocument XmlDom = new XmlDocument();
                string Result1 = "<CIPRoot></CIPRoot>";
                XmlDom.LoadXml(Result1);
                XmlRoot = XmlDom.DocumentElement;

                XmlElement xesub3 = XmlDom.CreateElement("Result");
                xesub3.InnerText = result.ToString();//�����ı��ڵ� 
                XmlRoot.AppendChild(xesub3);//��ӵ�<SessionBody>�ڵ��� 


                XmlElement xesub2 = XmlDom.CreateElement("PaymentSequenceID");
                xesub2.InnerText = paymentSequenceID;

                XmlRoot.AppendChild(xesub2);

                XmlElement xesub5 = XmlDom.CreateElement("UploadTime");
                xesub5.InnerText = uploadTime;
                XmlRoot.AppendChild(xesub5);//��ӵ�<SessionBody>�ڵ��� 

                XmlElement xesub4 = XmlDom.CreateElement("ErrorDescription");
                xesub4.InnerText = errMsg;
                XmlRoot.AppendChild(xesub4);


                resultXML = XmlDom.OuterXml;
            }
            catch (Exception ex)
            {
                resultXML = ex.Message;
            }

            return resultXML;
        }

        /// <summary>
        /// ���ɽ��XML
        /// </summary>
        /// <returns></returns>
        public string GenerateStatusNotifyResultXML()
        {
            string resultXML = "";

            try
            {
                XmlElement XmlRoot = null;
                XmlDocument XmlDom = new XmlDocument();
                string Result1 = "<CIPRoot></CIPRoot>";
                XmlDom.LoadXml(Result1);
                XmlRoot = XmlDom.DocumentElement;

                XmlElement xesub3 = XmlDom.CreateElement("Result");
                xesub3.InnerText = result.ToString();//�����ı��ڵ� 
                XmlRoot.AppendChild(xesub3);//��ӵ�<SessionBody>�ڵ��� 

                XmlElement xesub4 = XmlDom.CreateElement("ErrorDescription");
                xesub4.InnerText = errMsg;
                XmlRoot.AppendChild(xesub4);

                resultXML = XmlDom.OuterXml;
            }
            catch (Exception ex)
            {
                resultXML = ex.Message;
            }

            return resultXML;
        }

        #region ���Ժ��ֶζ���

        private string versionNo;
        private string spID;
        private string custID;
        private string paymentSequenceID;
        private string paymentAccount;
        private string bankID;
        private string last4No;
        private string source;
        private string uploadTime;
        private string extendField;
        private int result = ErrorDefinition.IError_Result_UnknowError_Code;
        private string errMsg="";
        private string status;

        private string paymentAccountRecord; 
           
        public string PaymentAccountRecord 
        {
            get
            {
                return paymentAccountRecord;
            }
            set
            {
                paymentAccountRecord = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public string VersionNo
        {
            get
            {
                return versionNo;
            }
            set
            {
                versionNo = value;
            }
        }

        public string SPID
        {
            get
            {
                return spID;
            }
            set
            {
                spID = value;
            }
        }

        public string CustID
        {
            get
            {
                return custID;
            }
            set
            {
                custID = value;
            }
        }

        public string PaymentSequenceID
        {
            get
            {
                return paymentSequenceID;
            }
            set
            {
                paymentSequenceID = value;
            }
        }

        public string PaymentAccount
        {
            get
            {
                return paymentAccount;
            }
            set
            {
                paymentAccount = value;
            }
        }

        public string BankID
        {
            get
            {
                return bankID;
            }
            set
            {
                bankID = value;
            }
        }

        public string Last4No
        {
            get
            {
                return last4No;
            }
            set
            {
                last4No = value;
            }
        }

        public string ExtendField
        {
            get
            {
                return extendField;
            }
            set
            {
                extendField = value;
            }
        }

        public int Result
        {
            get
            {
                //throw new System.NotImplementedException();
                return result;
            }
            set
            {
                result = value;
            }
        }

        public string ErrMsg
        {
            get
            {
                return errMsg;
            }
            set
            {
                errMsg = value;
            }
        }

        #endregion
    }
}

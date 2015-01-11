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
        #region 常支付账号数据上传接口使用
        //构造函数
        public void ParseXML(string RequestXML)
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";
            //解析请求参数
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RequestXML);

            //string xPathExpression1 = "CIPRoot/SessionHeader/Version";
            //XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            //if (selecNode == null)
            //{
            //    errMsg = "参数错误,无Version结点";
            //    result = -21500;
            //    return ;
            //}

            //versionNo = selecNode.InnerText;
            string xPathExpression1 = "CIPRoot/SPID";
            XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无SPID结点";
                result = -21500;
                return ;
            }
            spID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/CustID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无CustID结点";
                result = -21500;
                return ;
            }
            custID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/PaymentSequenceID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无PaymentSequenceID结点";
                result = -21500;
                return ;
            }
            paymentSequenceID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/PaymentAccount";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无PaymentAccount结点";
                result = -21500;
                return ;
            }
            paymentAccount = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/BankID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无BankID结点";
                result = -21500;
                return ;
            }
            bankID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Last4No";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无Last4No结点";
                result = -21500;
                return ;
            }
            last4No = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Source";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无Source结点";
                result = -21500;
                return;
            }
            source = selecNode.InnerText;


            
            result = 0;
        }

        /// <summary>
        /// 对请求参数进行数据校验
        /// </summary>
        public void DataCheck()
        {
            //初始化结果参数
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //数据合法性判断
            if (CommonUtility.IsEmpty(spID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return ;
            }
            if (spID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return ;
            }

            if (CommonUtility.IsEmpty(custID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return ;
            }

            if (custID.Length > ConstDefinition.Length_CustID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return;
            }
            if(CommonUtility.IsEmpty(paymentSequenceID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "支付流水帐号不能为空";
                return;
            }
            if (CommonUtility.IsEmpty(paymentAccount))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "银行卡号不能为空";
                return;
            }
            if (CommonUtility.IsEmpty(bankID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "银行代码不能为空";
                return;
            }
            if (CommonUtility.IsEmpty(last4No))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "信用卡后4位不能为空";
                return;
            }
            if(last4No.Length!=4)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "信用卡后4位长度不对";
                return;
            }

            if (!("1".Equals(source) || "2".Equals(source)))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "来源类型不存在";
                return;
            }
            result = 0;
            errMsg = "";
        }

        /// <summary>
        /// 常支付账号入临时表
        /// </summary>
        public void InsertPaymentAccountTMP()
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //处理入库
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

        #region 常支付账号状态通知接口使用
        //构造函数
        public void ParseXMLStatusNotify(string RequestXML)
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";
            //解析请求参数
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RequestXML);

            //string xPathExpression1 = "CIPRoot/SessionHeader/Version";
            //XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            //if (selecNode == null)
            //{
            //    errMsg = "参数错误,无Version结点";
            //    result = -21500;
            //    return;
            //}

            //versionNo = selecNode.InnerText;


            string xPathExpression1 = "CIPRoot/SPID";
            XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无SPID结点";
                result = -21500;
                return;
            }
            spID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/CustID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无CustID结点";
                result = -21500;
                return;
            }
            custID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/PaymentSequenceID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无PaymentSequenceID结点";
                result = -21500;
                return;
            }
            paymentSequenceID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Source";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无Source结点";
                result = -21500;
                return;
            }
            source = selecNode.InnerText;


            xPathExpression1 = "CIPRoot/Status";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无Status结点";
                result = -21500;
                return;
            }
            status = selecNode.InnerText;

           

            result = 0;
        }

        /// <summary>
        /// 对请求参数进行数据校验
        /// </summary>
        public void DataCheckStatusNotify()
        {
            //初始化结果参数
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //数据合法性判断
            if (CommonUtility.IsEmpty(spID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return;
            }
            if (spID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return;
            }

            if (CommonUtility.IsEmpty(custID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return;
            }

            if (custID.Length > ConstDefinition.Length_CustID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return;
            }
            if (CommonUtility.IsEmpty(paymentSequenceID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "支付流水帐号不能为空";
                return;
            }

       

            if (!("1".Equals(source) || "2".Equals(source)))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "来源类型不存在";
                return;
            }

            if (CommonUtility.IsEmpty(status))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "支付状态不能为空";
                return;
            }

            if (!("0".Equals(status) || "-1".Equals(status) || "-2".Equals(status)))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "支付状态类型不存在";
                return;
            }
            result = 0;
            errMsg = "";
        }

        /// <summary>
        /// 常支付账号入临时表
        /// </summary>
        public void InsertStatusNotify()
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //处理入库
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

        #region 常支付账号查询接口使用
        //构造函数
        public void ParseXMLAccountQuery(string RequestXML)
        {
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";
            //解析请求参数
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RequestXML);

            //string xPathExpression1 = "CIPRoot/SessionHeader/Version";
            //XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            //if (selecNode == null)
            //{
            //    errMsg = "参数错误,无Version结点";
            //    result = -21500;
            //    return;
            //}

            //versionNo = selecNode.InnerText;
            string xPathExpression1 = "CIPRoot/SPID";
            XmlNode selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无SPID结点";
                result = -21500;
                return;
            }
            spID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/CustID";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无CustID结点";
                result = -21500;
                return;
            }
            custID = selecNode.InnerText;

            xPathExpression1 = "CIPRoot/Source";
            selecNode = xmldoc.SelectSingleNode(xPathExpression1);//multi node
            if (selecNode == null)
            {
                errMsg = "参数错误,无Source结点";
                result = -21500;
                return;
            }
            source = selecNode.InnerText;

            result = 0;
        }

        /// <summary>
        /// 对请求参数进行数据校验
        /// </summary>
        public void DataCheckAccountQuery()
        {
            //初始化结果参数
            result = ErrorDefinition.IError_Result_UnknowError_Code;
            errMsg = "";

            //数据合法性判断
            if (CommonUtility.IsEmpty(spID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return;
            }
            if (spID.Length != ConstDefinition.Length_SPID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return;
            }

            if (CommonUtility.IsEmpty(custID))
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                return;
            }

            if (custID.Length > ConstDefinition.Length_CustID)
            {
                Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                errMsg = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
                return;
            }
           
            result = 0;
            errMsg = "";
        }

        /// <summary>
        /// 常支付账号入临时表
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
                //    errMsg = "无数据！";
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
        /// 生成结果XML
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

     

                //XmlElement xe1 = XmlDom.CreateElement("SessionHeader");//创建一个<SessionHeader>节点 

               // XmlElement xe2 = XmlDom.CreateElement("SessionBody");//创建一个<SessionBody>节点 

               // XmlElement xesub1 = XmlDom.CreateElement("Version");
               // xesub1.InnerText = versionNo;//设置文本节点 
               // xe1.AppendChild(xesub1);//添加到<SessionHeader>节点中 
               // XmlElement xesub2 = XmlDom.CreateElement("SPID");
               // xesub2.InnerText = spID;
               // xe1.AppendChild(xesub2);

                //XmlRoot.AppendChild(xe1);

         
                XmlElement xesub3 = XmlDom.CreateElement("Result");
                xesub3.InnerText = result.ToString();//设置文本节点 
                XmlRoot.AppendChild(xesub3);//添加到<SessionBody>节点中 

              
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
        /// 生成结果XML
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
                xesub3.InnerText = result.ToString();//设置文本节点 
                XmlRoot.AppendChild(xesub3);//添加到<SessionBody>节点中 


                XmlElement xesub2 = XmlDom.CreateElement("PaymentSequenceID");
                xesub2.InnerText = paymentSequenceID;

                XmlRoot.AppendChild(xesub2);

                XmlElement xesub5 = XmlDom.CreateElement("UploadTime");
                xesub5.InnerText = uploadTime;
                XmlRoot.AppendChild(xesub5);//添加到<SessionBody>节点中 

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
        /// 生成结果XML
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
                xesub3.InnerText = result.ToString();//设置文本节点 
                XmlRoot.AppendChild(xesub3);//添加到<SessionBody>节点中 

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

        #region 属性和字段定义

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

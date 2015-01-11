using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Xml;
using System.Configuration;

namespace Linkage.BestTone.Interface.Rule
{
    public class BTForCrmBizRules
    {
        public static int CustInfoUpload(string SysID, string ID, string UserType, string UserAccount, string CustLevel,
         string RealName, string ContactTel, string Address, string ZipCode, string CertificateCode,
         string CertificateType, string AreaID, string Sex, string Email, string dealType, string ExtendField, AuthenRecord[] AuthenRecords, string spid, out string CustID, out string ErrMsg)
        {
            int result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_OV3_Interface_CustInfoUpload";

                SqlParameter parSysID = new SqlParameter("@SysID", SqlDbType.VarChar, 8);
                parSysID.Value = SysID+"999999";
                cmd.Parameters.Add(parSysID);

                SqlParameter parID = new SqlParameter("@ID", SqlDbType.VarChar, 30);
                parID.Value = ID;
                cmd.Parameters.Add(parID);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = UserType;
                cmd.Parameters.Add(parUserType);


                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = (UserAccount == null ? "" : UserAccount);
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Value = CustLevel;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 20);
                parRealName.Value = (RealName == null ? "" : RealName);
                cmd.Parameters.Add(parRealName);

         //        string ContactTel, string Address, string ZipCode, string CertificateCode,
         //string CertificateType, string AreaID, string Sex, string Email, string dealType, string ExtendField,


                SqlParameter parContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 20);
                parContactTel.Value = (ContactTel == null ? "" : ContactTel);
                cmd.Parameters.Add(parContactTel);


                SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.VarChar, 200);
                parAddress.Value = (Address == null ? "" : Address); 
                cmd.Parameters.Add(parAddress);


                SqlParameter parZipCode = new SqlParameter("@ZipCode", SqlDbType.VarChar, 6);
                parZipCode.Value = (ZipCode == null ? "" : ZipCode); 
                cmd.Parameters.Add(parZipCode);

                 //string CertificateCode,
         //string CertificateType, string AreaID, string Sex, string Email, string dealType, string ExtendField,

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = (CertificateCode == null ? "" : CertificateCode); 
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 3);
                parAreaID.Value = AreaID;
                cmd.Parameters.Add(parAreaID);


                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);


                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 50);
                parEmail.Value = (Email == null ? "" : Email); 
                cmd.Parameters.Add(parEmail);

                SqlParameter pardealType = new SqlParameter("@dealType", SqlDbType.VarChar,1);
                pardealType.Value = dealType;
                cmd.Parameters.Add(pardealType);

                SqlParameter parAuthenRecords = new SqlParameter("@AuthenRecords", SqlDbType.Text);
                AuthenRecord[] au = new AuthenRecord[0];
                parAuthenRecords.Value = BTBizRules.GenerateXmlForAuthenRecords(AuthenRecords == null ? au : AuthenRecords);
                cmd.Parameters.Add(parAuthenRecords);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = spid;
                cmd.Parameters.Add(parSPID);

                SqlParameter parResult = new SqlParameter("@Result ", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();

            }
            catch (Exception e)
            {
                result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg+e.Message;
            }

            return result;

        
        
        }

        public static int UserAuthenStyleUpload(string ProvinceID, string ID, string AreaID, string ExtendField, AuthenRecord[] AuthenRecords,string SPID, out string CustID, out string ErrMsg)
        {
            int result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_OV3_Interface_UserAuthenStyleUpload";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parID = new SqlParameter("@ID", SqlDbType.VarChar, 30);
                parID.Value = ID;
                cmd.Parameters.Add(parID);

              
                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 3);
                parAreaID.Value = AreaID;
                cmd.Parameters.Add(parAreaID);


                SqlParameter parAuthenRecords = new SqlParameter("@AuthenRecords", SqlDbType.Text);
                AuthenRecord[] au = new AuthenRecord[0];
                parAuthenRecords.Value = BTBizRules.GenerateXmlForAuthenRecords(AuthenRecords == null ? au : AuthenRecords);
                cmd.Parameters.Add(parAuthenRecords);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parResult = new SqlParameter("@Result ", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();

            }
            catch (Exception e)
            {
                result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
            }

            return result;
        }

        public static int CustStatusChange(string ProvinceID, string CRMID, string CustType, string CRMAccount, string Status,string SPID,
         out string ErrMsg,out string OutProvinceID,out string OutCRMID)
        {
            int result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            OutProvinceID = "";
            OutCRMID="";
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_OV3_Interface_CustStatusChange";

                SqlParameter pProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                pProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(pProvinceID);

                SqlParameter pCRMID = new SqlParameter("@CRMID", SqlDbType.VarChar, 30);
                pCRMID.Value = CRMID;
                cmd.Parameters.Add(pCRMID);

                SqlParameter pCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                pCustType.Value = CustType;
                cmd.Parameters.Add(pCustType);
               

                SqlParameter pStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                pStatus.Value = Status;
                cmd.Parameters.Add(pStatus);


                SqlParameter pSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                pSPID.Value = SPID;
                cmd.Parameters.Add(pSPID);

                SqlParameter pResult = new SqlParameter("@Result", SqlDbType.Int);
                pResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pResult);

                SqlParameter pErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                pErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pErrMsg);

                SqlParameter pOutProvinceID = new SqlParameter("@OutProvinceID", SqlDbType.VarChar, 2);
                pOutProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOutProvinceID);
                

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                result = int.Parse(pResult.Value.ToString());
                ErrMsg = pErrMsg.Value.ToString();
                OutProvinceID = pOutProvinceID.Value.ToString();
                OutCRMID = CRMID;


            }
            catch (Exception e)
            {
                result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message.ToString();
            }

            return result;

        }

        /// <summary>
        /// 帐号合并验证
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="IncorporatedCustID"></param>
        /// <param name="SavedCustID"></param>
        /// <param name="ExtendField"></param>
        /// <param name="Result"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public static int IncorporateCust_CRM(string ProvinceID, string IncorporatedCRMID, string SavedCRMID,string SPID,
             out string ErrorDescription, out string IncorporatedCustID, out string SavedCustID)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";
            IncorporatedCustID = "";
            SavedCustID = "";
           
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_ov3_Interface_IncorporateCust_BTForCRM1";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parIncorporatedCRMID = new SqlParameter("@IncorporatedCRMID", SqlDbType.VarChar, 30);
                parIncorporatedCRMID.Value = IncorporatedCRMID;
                cmd.Parameters.Add(parIncorporatedCRMID);


                SqlParameter parSavedCRMID = new SqlParameter("@SavedCRMID", SqlDbType.VarChar, 30);
                parSavedCRMID.Value = SavedCRMID;
                cmd.Parameters.Add(parSavedCRMID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                SqlParameter parIncorporatedCustID = new SqlParameter("@IncorporatedCustID", SqlDbType.VarChar, 16);
                parIncorporatedCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parIncorporatedCustID);

                SqlParameter parSavedCustID = new SqlParameter("@SavedCustID", SqlDbType.VarChar, 16);
                parSavedCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSavedCustID);

               

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();


                IncorporatedCustID = parIncorporatedCustID.Value.ToString();
                SavedCustID = parSavedCustID.Value.ToString();             


            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }

            return Result;
        }

        public static int IncorporateAccount(string ProvinceID, string IncorporatedCustType, string IncorporatedCRMID,
            string IncorporatedAccount,string SavedCRMID,string SavedAccount,string SavedCustType,string SPID,out string OutProvinceID,
            out string ErrorDescription, out string OuterID, out string CustID,out string OutSavedCustType)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";
            OutProvinceID = "";
            OutSavedCustType = "";
            OuterID = "";
            CustID="";

            string SavedCustID="";
            string IncorporatedCustID="";
            string SavedUserAccount = "";
         //   string SPID = "35000000";
            string ExtendField = "";
     //       string IncorporatedAccount = "";

            Result = IncorporateCust_CRM(ProvinceID, IncorporatedCRMID, SavedCRMID, SPID,
                out ErrorDescription, out IncorporatedCustID, out SavedCustID);
            string sss = SavedCustID;
            if (Result != 0)
            {               
                return Result;
            }
            #region
            
            //try
            //{

            //    /****************************生成XML***************************************/
            //    XmlDocument xmldoc;
            //    XmlNode xmlnode;
            //    XmlElement xmlelem;
            //    XmlElement xmlelem2;
            //    XmlText xmltext;
            //    string XML;
            //    xmldoc = new XmlDocument();
            //    //加入XML的声明段落

            //    xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            //    xmldoc.AppendChild(xmlnode);
            //    //加入一个根元素
            //    xmlelem = xmldoc.CreateElement("", "ROOT", "");
            //    xmldoc.AppendChild(xmlelem);
            //    xmlelem2 = xmldoc.CreateElement("SPID");
            //    xmlelem2 = xmldoc.CreateElement("", "SPID", "");
            //    xmltext = xmldoc.CreateTextNode(SPID);
            //    xmlelem2.AppendChild(xmltext);
            //    xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //    xmlelem2 = xmldoc.CreateElement("IncorporatedCustID");
            //    xmlelem2 = xmldoc.CreateElement("", "IncorporatedCustID", "");
            //    xmltext = xmldoc.CreateTextNode(IncorporatedCustID);
            //    xmlelem2.AppendChild(xmltext);
            //    xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //    xmlelem2 = xmldoc.CreateElement("SavedCustID");
            //    xmlelem2 = xmldoc.CreateElement("", "SavedCustID", "");
            //    xmltext = xmldoc.CreateTextNode(SavedCustID);
            //    xmlelem2.AppendChild(xmltext);
            //    xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //    xmlelem2 = xmldoc.CreateElement("ExtendField");
            //    xmlelem2 = xmldoc.CreateElement("", "ExtendField", "");
            //    xmltext = xmldoc.CreateTextNode(ExtendField);
            //    xmlelem2.AppendChild(xmltext);
            //    xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //    XML = xmldoc.OuterXml;
            //    XML = XML.Substring(XML.IndexOf("<ROOT>"));
            //    XML = @"<?xml version='1.0' encoding='gb2312' standalone='yes' ?>" + XML;


            //    /***************************发送数据给统一平台****************************/
            //    BTUCenter.Proxy.IncorporateCust_YZ yz = new BTUCenter.Proxy.IncorporateCust_YZ();
            //    yz.Url = ConfigurationManager.AppSettings["JFUrl"];
            //    string ResultXML = yz.IncorporateCust(XML);
            //    //"<?xml version='1.0' encoding='utf-16' standalone='yes'?><root><Result>0</Result><ErrorDescription>成功</ErrorDescription><CustID>333333</CustID><ExtendField>555555</ExtendField></root>";


            //    /***************************解析xml****************************/

            //    Result = int.Parse(CommonUtility.GetValueFromXML(ResultXML, "Result"));

            //    ErrorDescription = CommonUtility.GetValueFromXML(ResultXML, "ErrorDescription");
            //    SavedCustID = CommonUtility.GetValueFromXML(ResultXML, "CustID");
            //    SavedUserAccount = CommonUtility.GetValueFromXML(ResultXML, "SavedUserAccount");
            //    if (Result != 0)
            //    {
            //        return Result;
            //    }

            //}
            //catch (Exception e)
            //{
            //    Result = ErrorDefinition.IError_Result_System_UnknowError_Code; ;
            //    ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
            //}
            //finally
            //{
            //    #region WriteLog
            //    StringBuilder msg1 = new StringBuilder();
            //    msg1.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            //    msg1.Append("CRM平台帐号合并接口 " + DateTime.Now.ToString("u") + "\r\n");
            //    msg1.Append(";SPID - " + SPID);
            //    msg1.Append(";IncorporatedCustID - " + IncorporatedAccount);
            //    msg1.Append(";SavedCustID - " + SavedCustID);
            //    msg1.Append(";ExtendField - " + ExtendField);
            //    msg1.Append("\r\n");
            //    msg1.Append("sss:" + sss+"\r");

            //    msg1.Append("处理结果 - " + Result);
            //    msg1.Append("; 错误描述 - " + ErrorDescription);
            //    msg1.Append("; SavedCustID - " + SavedCustID);
            //    msg1.Append("; SavedUserAccount - " + SavedUserAccount + "\r\n");
            //    msg1.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            //    BTUCenterInterfaceLog.CenterForBizTourLog("IncorporateCust_ScoreForCRM", msg1);
            //    #endregion
            //}

            #endregion


            /***************************帐户合并接口流程*******************************************************************/


          

            #region
            try
            {
                SqlCommand cmd = new  SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_ov3_Interface_IncorporateCust_BTForCRM2";//up_V5_bestTone_IncorporateCust_BTForCRM
                                       

                SqlParameter pProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                pProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(pProvinceID);

                SqlParameter pIncorporatedCustID = new SqlParameter("@IncorporatedCustID", SqlDbType.VarChar, 16);
                pIncorporatedCustID.Value = IncorporatedCustID;
                cmd.Parameters.Add(pIncorporatedCustID);

                //SqlParameter pIncorporatedCRMID = new SqlParameter("@IncorporatedCRMID", SqlDbType.VarChar, 16);
                //pIncorporatedCRMID.Value = IncorporatedCRMID;
                //cmd.Parameters.Add(pIncorporatedCRMID);

                SqlParameter pSavedCustID = new SqlParameter("@SavedCustID", SqlDbType.VarChar, 16);
                pSavedCustID.Value = SavedCustID;
                cmd.Parameters.Add(pSavedCustID);

                //SqlParameter pSavedCRMID = new SqlParameter("@SavedCRMID", SqlDbType.VarChar, 16);
                //pSavedCRMID.Value = SavedCRMID;
                //cmd.Parameters.Add(pSavedCRMID);

                //SqlParameter pIncorporatedCustType = new SqlParameter("@IncorporatedCustType", SqlDbType.VarChar, 2);
                //pIncorporatedCustType.Value = IncorporatedCustType;
                //cmd.Parameters.Add(pIncorporatedCustType);

                //SqlParameter pSavedAccount = new SqlParameter("@SavedAccount", SqlDbType.VarChar, 16);
                //pSavedAccount.Value = SavedAccount;
                //cmd.Parameters.Add(pSavedAccount);

                //SqlParameter pSavedCustType = new SqlParameter("@SavedCustType", SqlDbType.VarChar, 2);
                //pSavedCustType.Value = SavedCustType;
                //cmd.Parameters.Add(pSavedCustType);

                SqlParameter pOutProvinceID = new SqlParameter("@OutProvinceID", SqlDbType.VarChar, 2);
                pOutProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOutProvinceID);

                SqlParameter pResult = new SqlParameter("@Result", SqlDbType.Int);
                pResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pResult);

                SqlParameter pErrorDescription = new SqlParameter("@ErrorDescription", SqlDbType.VarChar, 256);
                pErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pErrorDescription);

  

                SqlParameter pCRMID = new SqlParameter("@CRMID", SqlDbType.VarChar, 16);
                pCRMID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pCRMID);

                SqlParameter pCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                pCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pCustID);

                SqlParameter pOutSavedCustType = new SqlParameter("@OutSavedCustType", SqlDbType.VarChar, 2);
                pOutSavedCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOutSavedCustType);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(pResult.Value.ToString());
                ErrorDescription = pErrorDescription.Value.ToString();
                OutProvinceID = pOutProvinceID.Value.ToString();
                OutSavedCustType = SavedCustType;
                OuterID = SavedCRMID;
                CustID = SavedCustID;
            #endregion

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message.ToString();
            }

            return Result;
        }

        /// <summary>
        /// 修改认证信息
        /// </summary>
        /// <param name="OriginalID"></param>
        /// <param name="NewID"></param>
        /// <param name="DealType"></param>
        /// <param name="AuthenRecords"></param>
        /// <returns></returns>
        public static int ChangeUserAuthenStyle(string ProvinceID,string OriginalID, string NewID,
        string DealType, AuthenRecord[] AuthenRecords, string SPID,out string NewCustID, out string OriginalCustID, out string ErrorDescription)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";
            NewCustID = "";
            OriginalCustID = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_OV3_Interface_ChangeUserAuthenStyleCRM";


                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parOriginalID = new SqlParameter("@OriginalID", SqlDbType.VarChar, 30);
                parOriginalID.Value = OriginalID;
                cmd.Parameters.Add(parOriginalID);

                SqlParameter parNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 30);
                parNewID.Value = NewID;
                cmd.Parameters.Add(parNewID);


                SqlParameter parDealType = new SqlParameter("@DealType", SqlDbType.VarChar, 1);
                parDealType.Value = DealType;
                cmd.Parameters.Add(parDealType);


                SqlParameter parAuthenRecords = new SqlParameter("@AuthenRecords", SqlDbType.Text);
                AuthenRecord[] au = new AuthenRecord[0];
                parAuthenRecords.Value = BTBizRules.GenerateXmlForAuthenRecords(AuthenRecords == null ? au : AuthenRecords);
                cmd.Parameters.Add(parAuthenRecords);


                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                SqlParameter parNewCustID = new SqlParameter("@NewCustID", SqlDbType.VarChar, 16);
                parNewCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNewCustID);

                SqlParameter parOriginalCustID = new SqlParameter("@OriginalCustID", SqlDbType.VarChar, 16);
                parOriginalCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOriginalCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();


                OriginalCustID = parOriginalCustID.Value.ToString();
                NewCustID = parNewCustID.Value.ToString();


            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }

            return Result;
        }

        public static int CustProvinceRelationQuery(string OuterID, string ProvinceID, out string CustID, out string SID,out string CustAccount, out string ErrorDescription)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = "";
            CustID = "";
            SID = "";
            CustAccount = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_OV3_Interface_CustProvinceRelationQuery";

                SqlParameter parOuterid = new SqlParameter("@Outerid", SqlDbType.VarChar, 30);
                parOuterid.Value = OuterID;
                cmd.Parameters.Add(parOuterid);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription", SqlDbType.VarChar, 256);
                parErrorDescription.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrorDescription);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSID = new SqlParameter("@SID", SqlDbType.VarChar, 16);
                parSID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSID);

                SqlParameter parCustAccount = new SqlParameter("@CustAccount", SqlDbType.VarChar, 16);
                parCustAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustAccount);



                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrorDescription = parErrorDescription.Value.ToString();


                SID = parSID.Value.ToString();
                CustID = parCustID.Value.ToString();
                CustAccount = parCustAccount.Value.ToString();


            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrorDescription = ex.Message;
            }

            return Result;
        }


    }
}

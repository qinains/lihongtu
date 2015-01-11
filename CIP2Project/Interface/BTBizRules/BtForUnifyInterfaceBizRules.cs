using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Data.Sql;
using System.Configuration;
using BTUCenter.Proxy;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{

    public class BtForUnifyInterfaceBizRules
    {
        /// <summary>
        /// 用户状态变更接口 created by liye 
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="Status"></param>
        /// <param name="ErrMsg"></param>
        /// <returns>0-成功 </returns>
        public static int CustStatusChange(string ProvinceID,string SPID, string CustID, string Status,string Description, out string ErrMsg, out string RealName)
        {
            int result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            RealName = "";
            SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "up_BT_V2_Interface_CustStatusChange";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Transaction = tran;

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parProvinceID=new SqlParameter("@ProvinceID",SqlDbType.VarChar,2);
                parProvinceID.Value=ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Value = Status;
                cmd.Parameters.Add(parStatus);

                SqlParameter parDescrption = new SqlParameter("@Description", SqlDbType.VarChar, 256);
                parDescrption.Value = Description;
                cmd.Parameters.Add(parDescrption);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                cmd.ExecuteNonQuery();
                tran.Commit();
               
                result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                RealName = parRealName.Value.ToString();

            }
            catch (Exception ex)
            {
                result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
                if (tran != null)
                    tran.Rollback();
            }
            finally
            {
                conn.Close();
            }
         
            return result;
        }

        /// <summary>
        /// 客户升级请求接口 created by liye 
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="ErrMsg"></param>
        /// <returns>0-成功</returns>
        public static int CustLevelChange(string ProvinceID, string SPID, string CustID, string UserAccount,out string CustLevel,out string ErrMsg)
        {
            int result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustLevel = "A";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_BT_V2_Interface_CustLevelChange";
            try
            {
                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustLevel = parCustLevel.Value.ToString();
            }
            catch (Exception ex)
            {
                result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 客户积分等级变更通知接口 created by liye 
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="IntegralInfo"></param>
        /// <param name="GradeOrCredit"></param>
        /// <param name="UpgradeOrFall"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int IntegralTigerGrade(string ProvinceID, string SPID, string IntegralInfo,string GradeOrCredit, string UpgradeOrFall, string CustID,out string ErrMsg)
        {
            int result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_BT_V2_Interface_IntegralTigerGrade";

            try
            {
                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                int integralNum=int.Parse(IntegralInfo);
                SqlParameter parIntegralInfo = new SqlParameter("@IntegralInfo", SqlDbType.Int);
                parIntegralInfo.Value = integralNum;
                cmd.Parameters.Add(parIntegralInfo);

                SqlParameter parGradeOrCredit = new SqlParameter("@GradeOrCredit", SqlDbType.VarChar, 2);
                parGradeOrCredit.Value = GradeOrCredit;
                cmd.Parameters.Add(parGradeOrCredit);

                SqlParameter parUpgradeOrFall = new SqlParameter("@UpgradeOrFall", SqlDbType.VarChar, 2);
                parUpgradeOrFall.Value = UpgradeOrFall;
                cmd.Parameters.Add(parUpgradeOrFall);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message+"，数据库内部错误";
            }
            
            return result;
        }


        /// <summary>
        /// 企业客户信息同步上传接口
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="CorporationID"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="CorporationName"></param>
        /// <param name="CorporationType"></param>
        /// <param name="ExtendField"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int EnterpriseInfoUplod(string ProvinceID, string SPID, string CorporationID, string CustID,string UserAccount, 
                                     string CorporationName, string CorporationType, out string OCustID,out string OUserAccount,string ExtendField,out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OUserAccount = "";
            OCustID = "";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_BT_V2_Interface_EnterpriseInfoUpLoad";
            try
            {
                SqlParameter parCorporationID = new SqlParameter("@EnterpriseID", SqlDbType.VarChar, 30);
                parCorporationID.Value = CorporationID;
                cmd.Parameters.Add(parCorporationID);

                SqlParameter parCorporationName = new SqlParameter("@EnterpriseName", SqlDbType.VarChar, 50);
                parCorporationName.Value = CorporationName;
                cmd.Parameters.Add(parCorporationName);

                SqlParameter parCorporationType = new SqlParameter("@EnterpriseType", SqlDbType.VarChar, 2);
                parCorporationType.Value = CorporationType;
                cmd.Parameters.Add(parCorporationType);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parEncryedPassword = new SqlParameter("@EncryedPassword", SqlDbType.VarChar, 50);
                parEncryedPassword.Value = CryptographyUtil.Encrypt("111111");
                cmd.Parameters.Add(parEncryedPassword);

                SqlParameter parOCustID = new SqlParameter("@OCustID", SqlDbType.VarChar, 16);
                parOCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOCustID);

                SqlParameter parOUserAccount = new SqlParameter("@OUserAccount", SqlDbType.VarChar, 16);
                parOUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOUserAccount);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 1024);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                OUserAccount = parOUserAccount.Value.ToString();
                OCustID = parOCustID.Value.ToString();
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
       
            }
            catch(Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;

        }

        /// <summary>
        /// 依据CustID获取客户详细信息
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public static DataSet GetCustDetailInfo(string CustID)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_GetCustDetailInfo";
                
                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
              
            }catch
            {
            }
       
            return ds;
        }

        /// <summary>
        /// 发送信息给客户。如果手机为空，则发送邮件
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="Message"></param>
        /// <param name="SPID"></param>
        public static void SendMessage(string CustID, string Message, string SPID)
        {
            string MobilePhone = "";
            string Email = "";
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_GetCustDetailInfo";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null)
                {
                    MobilePhone = ds.Tables[0].Rows[0]["CustContactTel"].ToString();
                    Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    if(!CommonUtility.IsEmpty(MobilePhone))
                    {
                        // send short msg
                        //CommonBizRules.SendMessage(MobilePhone, Message, SPID);
                        CommonBizRules.SendMessageV3(MobilePhone, Message, SPID);
                    }
                    else if(!CommonUtility.IsEmpty(Email))
                    {
                        // send short email
                    }
                  
             
                }


            }
            catch 
            {
            }

        }

        /// <summary>
        /// 同步企业信息到积分系统
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="CorporationID"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="CorporationName"></param>
        /// <param name="CorporationType"></param>
        /// <param name="?"></param>
        public static void EnterpriseInfoNotify(string ProvinceID,string SPID,string CorporationID,string CustID,string UserAccount,
                                                  string CorporationName,string CorporationType,string ExtendField)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            string strResult="";
            string ErrMsg = "";
            try
            {
                #region XML
                //string CustInfoXML = "";
                //XmlDocument xmldoc;
                //XmlNode xmlnode;
                //XmlElement xmlelem;
                //XmlElement xmlelem2 = null;
                //XmlElement xmlelem3;
                //XmlText xmltext;
                //xmldoc = new XmlDocument();
                ////加入XML的声明段落
                //xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                //xmldoc.AppendChild(xmlnode);
                ////加入一个根元素
                //xmlelem = xmldoc.CreateElement("", "ROOT", "");
                //xmldoc.AppendChild(xmlelem);

                ////加入另外一个元素
                //xmlelem2 = xmldoc.CreateElement("EnterpriseInfoUplodRequest");
                //xmlelem2 = xmldoc.CreateElement("", "EnterpriseInfoUplodRequest", "");

                //xmlelem3 = xmldoc.CreateElement("", "ProvinceID", "");
                //xmltext = xmldoc.CreateTextNode(ProvinceID);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmlelem3 = xmldoc.CreateElement("", "SPID", "");
                //xmltext = xmldoc.CreateTextNode(SPID);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmlelem3 = xmldoc.CreateElement("", "CorporationID", "");
                //xmltext = xmldoc.CreateTextNode(CorporationID);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmlelem3 = xmldoc.CreateElement("", "CustID", "");
                //xmltext = xmldoc.CreateTextNode(CustID);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmlelem3 = xmldoc.CreateElement("", "UserAccount", "");
                //xmltext = xmldoc.CreateTextNode(UserAccount);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmlelem3 = xmldoc.CreateElement("", "CorporationName", "");
                //xmltext = xmldoc.CreateTextNode(CorporationName);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmlelem3 = xmldoc.CreateElement("", "CorporationType", "");
                //xmltext = xmldoc.CreateTextNode(CorporationType);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmlelem3 = xmldoc.CreateElement("", "ExtendField", "");
                //xmltext = xmldoc.CreateTextNode(ExtendField);
                //xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                //xmlelem3.AppendChild(xmltext);
                //xmlelem2.AppendChild(xmlelem3);

                //xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                ////保存创建好的XML文档

                //xmldoc.Save(@"C:\BasicUserInfo.xml");
                //CustInfoXML = xmldoc.OuterXml;
                //CustInfoXML = CustInfoXML.Substring(CustInfoXML.IndexOf("<ROOT>"));
                //CustInfoXML = @"<?xml version='1.0' encoding='gb2312'?>" + CustInfoXML;



                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
                UnifyInterfaceForUCenter obj = new UnifyInterfaceForUCenter();
                obj.Url = ConfigurationManager.AppSettings["UnifyInterUrl"];
                strResult = obj.enterpriseInfoUplod(SPID, CorporationID, CustID, UserAccount, CorporationName, CorporationType);
            
                XmlDocument xmlObj = new XmlDocument();
                xmlObj.LoadXml(strResult);
                Result = int.Parse(xmlObj.GetElementsByTagName("result")[0].InnerText);
                ErrMsg = xmlObj.GetElementsByTagName("errorDescription")[0].InnerText;
            }
            catch (Exception ex)
            {
                ErrMsg = "1," + ex.Message;
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
            }
            finally
            {
                string IsWriteLog = ConfigurationManager.AppSettings["IsWriteLog"];
                if (IsWriteLog == "0")
                {
                    //如果通知失败则插入用户信息失败记录表
                    // if (Result != 0)
                    //
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "up_BT_V2_Interface_InsertCustInfoNotifyFailRecord";

                    SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                    parCustID.Value = CustID;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                    parUserAccount.Value = UserAccount;
                    cmd.Parameters.Add(parUserAccount);

                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Value = Result;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Value = ErrMsg;
                    cmd.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                }
            }

        }


    }
}

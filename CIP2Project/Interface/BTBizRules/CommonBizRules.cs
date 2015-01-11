using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;

namespace Linkage.BestTone.Interface.Rule
{
    public class CommonBizRules
    {
        /// <summary> 验证电话号码有效性
        /// 验证电话号码有效性
        /// </summary>
        /// <param name="PhoneNum">电话号码</param>
        /// <returns></returns>
        public static bool PhoneNumValid(string PhoneNum)
        {
            bool result = true;

            //string dStyle = @"^0\d{2,3}[1-9]\d{6,7}$";
            string dStyle = @"^(\d{3,4}\d{7,8})$";
            if (CommonUtility.ValidateStr(PhoneNum, dStyle))
            {
                result = false;
            }
            if (PhoneNum.Length != 11 & PhoneNum.Length != 12)
            {
                result = false;
            }

            return result;
        }


        /// <summary> 绑定电话
        /// 绑定电话
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int BindPhone(string SPID, string CustID, string UserAccount, string PhoneNum, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {

                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_BindPhoneNum";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();




            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }

            return Result;


        }


        /// <summary> 获得绑定电话列表
        /// 获得绑定电话列表
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static BoundPhoneRecord[] GetBoundPhone(string CustID, string UserAccount, out int Result, out string ErrMsg)
        {
            BoundPhoneRecord[] BoundPhoneRecords = null;
            ErrMsg = "";
            Result = ErrorDefinition.IError_Result_UnknowError_Code;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_GetBoundPhone";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null)
                    if (ds.Tables.Count != 0)
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;

                            BoundPhoneRecords = new BoundPhoneRecord[RowCount];
                            BoundPhoneRecord rs = new BoundPhoneRecord();
                            for (int i = 0; i < RowCount; i++)
                            {
                                rs = new BoundPhoneRecord();
                                rs.Phone = ds.Tables[0].Rows[i]["BoundPhoneNumber"].ToString().Trim();
                                BoundPhoneRecords[i] = rs;
                            }
                        }
                Result = ErrorDefinition.IError_Result_Success_Code;
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取绑定号码出错，" + e.Message;
            }

            return BoundPhoneRecords;
        }

        /// <summary> 电话解绑
        /// 电话解绑
        /// </summary>
        /// <param name="PhoneNum"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int UnBindPhone(string PhoneNum, out string ErrMsg, out string CustID, out string UserAccount)
        {

            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_UnBindPhone";

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);


                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                CustID = parCustID.Value.ToString().Trim();
                UserAccount = parUserAccount.Value.ToString().Trim();
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;

        }

        /// <summary> 密码重置
        /// 密码重置
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int PasswordReset(string SPID, string CustID, string UserAccount, string NewPassword, out string ErrMsg, out string ContactTel, out string Email, out string RealName)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";

            ContactTel = "";
            Email = "";
            RealName = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_PasswordReset";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parNewPassword = new SqlParameter("@NewPassword", SqlDbType.VarChar, 50);
                parNewPassword.Value = CryptographyUtil.Encrypt(NewPassword);
                cmd.Parameters.Add(parNewPassword);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 20);
                parContactTel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parContactTel);

                SqlParameter parEmail = new SqlParameter("@Email ", SqlDbType.VarChar, 256);
                parEmail.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEmail);

                SqlParameter parRealName = new SqlParameter("@RealName ", SqlDbType.VarChar,30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);



                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                ContactTel = parContactTel.Value.ToString();
                Email = parEmail.Value.ToString();
                RealName = parRealName.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;


        }

        /// <summary> 基本信息查询
        /// 基本信息查询
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="CertificateCode"></param>
        /// <param name="CertificateType"></param>
        /// <param name="RealName"></param>
        /// <param name="UserBasicInfoRecords"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int BasicInfoQuery(string ProvinceID, string SPID, string UserAccount,
            string PhoneNum, string CertificateCode, string CertificateType, string RealName,
            out UserBasicInfoRecord[] UserBasicInfoRecords, out string ErrMsg)
        {

            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            UserBasicInfoRecords = null;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_BasicInfoQuery";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);


                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 1);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);


                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                //SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                //parResult.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parResult);

                //SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                //parErrMsg.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parErrMsg);

                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds != null)
                    if (ds.Tables != null)
                        if (ds.Tables.Count > 0)
                        {
                            DataTable tb = ds.Tables[0];
                            UserBasicInfoRecords = new UserBasicInfoRecord[tb.Rows.Count];
                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                UserBasicInfoRecords[i] = new UserBasicInfoRecord();
                                UserBasicInfoRecords[i].CertificateCode = tb.Rows[i]["CertificateCode"].ToString();
                                UserBasicInfoRecords[i].CertificateType = tb.Rows[i]["CertificateType"].ToString();
                                UserBasicInfoRecords[i].CustID = tb.Rows[i]["CustID"].ToString();
                                UserBasicInfoRecords[i].RealName = tb.Rows[i]["RealName"].ToString();
                                UserBasicInfoRecords[i].UserAccount = tb.Rows[i]["UserAccount"].ToString();
                                UserBasicInfoRecords[i].Status = tb.Rows[i]["Status"].ToString();

                                UserBasicInfoRecords[i].BoundPhoneRecords = GetBoundPhone(UserBasicInfoRecords[i].CustID, UserBasicInfoRecords[i].UserAccount, out Result, out ErrMsg);
                            }
                        }

                if (UserBasicInfoRecords == null || UserBasicInfoRecords.Length == 0)
                {
                    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                    ErrMsg = "无记录";
                    return Result;
                }

                if (UserBasicInfoRecords.Length > 0)
                {
                    Result = ErrorDefinition.IError_Result_Success_Code;
                    return Result;
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;
        }

        /// <summary>业务开通
        /// 业务开通
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="TimeStamp"></param>
        /// <param name="TransactionID"></param>
        /// <param name="SubScribeStyle"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="SPID"></param>
        /// <param name="ServiceID"></param>
        /// <param name="ServiceName"></param>
        /// <param name="Fee"></param>
        /// <param name="SubscribeDate"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="ExtendField"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int UserSubScribe(string ProvinceID,
            string TimeStamp, string TransactionID, string SubScribeStyle, string CustID, string UserAccount, string SPID,
            string ServiceID, string ServiceName, int Fee, string SubscribeDate, string StartTime, string EndTime, string ExtendField,
            out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_UserSubScribe";


                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 36);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parTransactionID = new SqlParameter("@TransactionID", SqlDbType.VarChar, 36);
                parTransactionID.Value = TransactionID;
                cmd.Parameters.Add(parTransactionID);

                SqlParameter parSubScribeStyle = new SqlParameter("@SubScribeStyle", SqlDbType.VarChar, 1);
                parSubScribeStyle.Value = SubScribeStyle;
                cmd.Parameters.Add(parSubScribeStyle);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parServiceID = new SqlParameter("@ServiceID", SqlDbType.VarChar, 16);
                parServiceID.Value = ServiceID;
                cmd.Parameters.Add(parServiceID);

                SqlParameter parServiceName = new SqlParameter("@ServiceName", SqlDbType.VarChar, 100);
                parServiceName.Value = ServiceName;
                cmd.Parameters.Add(parServiceName);

                SqlParameter parFee = new SqlParameter("@Fee", SqlDbType.Int);
                parFee.Value = Fee;
                cmd.Parameters.Add(parFee);

                SqlParameter parSubscribeDate = new SqlParameter("@SubscribeDate", SqlDbType.DateTime);
                parSubscribeDate.Value = DateTime.Parse(SubscribeDate);
                cmd.Parameters.Add(parSubscribeDate);

                SqlParameter parStartTime = new SqlParameter("@StartTime", SqlDbType.DateTime);
                parStartTime.Value = DateTime.Parse(StartTime);
                cmd.Parameters.Add(parStartTime);

                SqlParameter parEndTime = new SqlParameter("@EndTime", SqlDbType.DateTime);
                parEndTime.Value = DateTime.Parse(EndTime);
                cmd.Parameters.Add(parEndTime);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 100);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;

        }

        /// <summary> 业务退订
        /// 业务退订
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="ServiceID"></param>
        /// <param name="TransactionID"></param>
        /// <param name="EndTime"></param>
        /// <param name="TimeStamp"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int CancelBySubscription(string ProvinceID,
            string SPID, string ServiceID, string TransactionID, string EndTime, string TimeStamp, string CustID, string UserAccount, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_CancelBySubscription";


                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 36);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parTransactionID = new SqlParameter("@TransactionID", SqlDbType.VarChar, 36);
                parTransactionID.Value = TransactionID;
                cmd.Parameters.Add(parTransactionID);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parServiceID = new SqlParameter("@ServiceID", SqlDbType.VarChar, 16);
                parServiceID.Value = ServiceID;
                cmd.Parameters.Add(parServiceID);

                SqlParameter parEndTime = new SqlParameter("@EndTime", SqlDbType.DateTime);
                parEndTime.Value = DateTime.Parse(EndTime);
                cmd.Parameters.Add(parEndTime);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 100);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;

        }


        ////用户订购信息基本性校验（长度，是否为空）
        //public static int UserSubInfoCheck(UserInfo UserBasicInfo)
        //{

        //    return 0;
        //}

        /// <summary>
        /// 合并帐号
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="DeleteUserAccountRecords"></param>
        /// <param name="IncorporatedCustID"></param>
        /// <param name="IncorporatedUserAccount"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int IncorporateUserAccount(string ProvinceID, string SPID,
            UserAccountRecord[] DeleteUserAccountRecords, string IncorporatedCustID, string IncorporatedUserAccount, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V2_Interface_IncorporateUserAccount";

                string DeleteUserAccountXML = GenerateXmlForDeleteUserAccountRecords(DeleteUserAccountRecords);

                SqlParameter parDeleteUserAccountXML = new SqlParameter("@DeleteUserAccountRecords", SqlDbType.Text);
                parDeleteUserAccountXML.Value = DeleteUserAccountXML;
                cmd.Parameters.Add(parDeleteUserAccountXML);

                SqlParameter parIncorporatedUserAccount = new SqlParameter("@IncorporatedUserAccount", SqlDbType.VarChar, 16);
                parIncorporatedUserAccount.Value = IncorporatedUserAccount;
                cmd.Parameters.Add(parIncorporatedUserAccount);

                SqlParameter parIncorporatedUserCustID = new SqlParameter("@IncorporatedCustID", SqlDbType.VarChar, 16);
                parIncorporatedUserCustID.Value = IncorporatedCustID;
                cmd.Parameters.Add(parIncorporatedUserCustID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 100);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;
        }


        /// <summary> 获取需删除帐号记录的xml字符串
        /// 获取需删除帐号记录的xml字符串
        /// </summary>
        /// <param name="DeleteUserAccountRecords"></param>
        /// <returns></returns>
        public static string GenerateXmlForDeleteUserAccountRecords(UserAccountRecord[] DeleteUserAccountRecords)
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

                for (int i = 0; i < DeleteUserAccountRecords.Length; i++)
                {
                    //加入另外一个元素
                    xmlelem2 = xmldoc.CreateElement("DeleteUserAccountRecord");
                    xmlelem2 = xmldoc.CreateElement("", "DeleteUserAccountRecord", "");

                    xmlelem3 = xmldoc.CreateElement("", "CustID", "");
                    xmltext = xmldoc.CreateTextNode(DeleteUserAccountRecords[i].CustID.ToString());
                    xmlelem3.AppendChild(xmltext);
                    xmlelem2.AppendChild(xmlelem3);

                    xmlelem3 = xmldoc.CreateElement("", "UserAccount", "");
                    xmltext = xmldoc.CreateTextNode(DeleteUserAccountRecords[i].UserAccount.ToString());
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

        /// <summary> 获取认证方式xml字符串
        /// 获取认证方式xml字符串
        /// </summary>
        /// <param name="DeleteUserAccountRecords"></param>
        /// <returns></returns>
        public static string GenerateXmlForAuthenRecords(AuthenRecord[] AuthenRecords)
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

        /// <summary>  查询积分上传明细
        ///  查询积分上传明细
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="ScoreDetailRecords"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int ScoreInfoQuery(string ProvinceID, string SPID, string CustID,
             string UserAccount, string StartTime, string EndTime, out ScoreDetailRecord[] ScoreDetailRecords, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ScoreDetailRecords = null;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V2_Interface_ScoreDetailInfoQuery";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parStartTime = new SqlParameter("@StartTime", SqlDbType.DateTime);
                parStartTime.Value = DateTime.Parse(StartTime);
                cmd.Parameters.Add(parStartTime);

                SqlParameter parEndTime = new SqlParameter("@EndTime", SqlDbType.DateTime);
                parEndTime.Value = DateTime.Parse(EndTime);
                cmd.Parameters.Add(parEndTime);

                DataSet ScoreData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ScoreData == null)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "无积分记录";
                    return Result;
                }

                if (ScoreData.Tables.Count == 0)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "无积分记录";
                    return Result;
                }

                if (ScoreData.Tables[0].Rows.Count == 0)
                {
                    Result = ErrorDefinition.BT_IError_Result_NoScoreInfo_Code;
                    ErrMsg = "无积分记录";
                    return Result;
                }

                DataTable dt = ScoreData.Tables[0];
                ScoreDetailRecords = new ScoreDetailRecord[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ScoreDetailRecords[i] = new ScoreDetailRecord();
                    ScoreDetailRecords[i].Description = dt.Rows[i]["Description"].ToString();
                    ScoreDetailRecords[i].EffectiveTime = DateTime.Parse(dt.Rows[i]["EffectiveTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim(); ;
                    ScoreDetailRecords[i].ExpireTime = DateTime.Parse(dt.Rows[i]["ExpireTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim(); ;
                    ScoreDetailRecords[i].Score = long.Parse(dt.Rows[i]["Score"].ToString());
                    ScoreDetailRecords[i].ScoreType = dt.Rows[i]["ScoreType"].ToString();
                    ScoreDetailRecords[i].SPID = dt.Rows[i]["SPID"].ToString();
                    ScoreDetailRecords[i].UploadTime = DateTime.Parse(dt.Rows[i]["UploadTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim(); ;
                }

                Result = 0;
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg + "," + e.Message;
            }

            return Result;
        }


        /// <summary> 唯一性查询
        /// 唯一性查询
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="CertificateCode"></param>
        /// <param name="CertificateType"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int UniquenessQuery(string ProvinceID, string SPID, string UserAccount,
           string PhoneNum, string CertificateCode, string CertificateType, out string ErrMsg)
        {

            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_BT_V2_Interface_UniquenessQuery";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);


                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 1);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }


            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;
        }

        /// <summary> 写数据库日志
        /// 写数据库日志
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="Result"></param>
        /// <param name="ErrorDescription"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="LogType"></param>
        public static void WriteDataLog(string SPID, string CustID, string UserAccount, int Result, string ErrorDescription,
           string PhoneNum, string InterfaceName)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_BT_V2_Interface_WriteDataLog";

            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = (SPID == null) ? "" : SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
            parCustID.Value = (CustID == null) ? "" : CustID;
            cmd.Parameters.Add(parCustID);


            SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
            parUserAccount.Value = (UserAccount == null) ? "" : UserAccount;
            cmd.Parameters.Add(parUserAccount);

            SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
            parResult.Value = Result;
            cmd.Parameters.Add(parResult);

            SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription", SqlDbType.VarChar, 256);
            parErrorDescription.Value = (ErrorDescription == null) ? "" : ErrorDescription;
            cmd.Parameters.Add(parErrorDescription);

            SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
            parPhoneNum.Value = (PhoneNum == null) ? "" : PhoneNum;
            cmd.Parameters.Add(parPhoneNum);

            SqlParameter parInterfaceName = new SqlParameter("@InterfaceName", SqlDbType.VarChar, 48);
            parInterfaceName.Value = (InterfaceName == null) ? "" : InterfaceName;
            cmd.Parameters.Add(parInterfaceName);


            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

        }

        /// <summary> 通知积分系统用户信息
        /// 通知积分系统用户信息
        /// </summary>
        /// <param name="CustID"></param>
       /// <param name="UserAccount"></param>
       /// <param name="ExtendField"></param>
       /// <param name="DealType"></param>
       /// <param name="RegistryType"> 1：反向注册</param>
        public static void CustInfoNotify(string CustID, string UserAccount, string ExtendField, string DealType, string RegistryType, string PaymentPwd)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            string ErrMsg = "";

            DataSet ds = null;
            SqlCommand cmd = new SqlCommand();

            StringBuilder msg = new StringBuilder();

            DateTime startTime =DateTime.Now;
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_QueryCustInfo";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);


                string CustInfoXML = "";
                XmlDocument xmldoc;
                XmlNode xmlnode;
                XmlElement xmlelem;
                XmlElement xmlelem2 =null;
                XmlElement xmlelem3;
                XmlText xmltext;
                    xmldoc = new XmlDocument();
                    //加入XML的声明段落
                    xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                    xmldoc.AppendChild(xmlnode);
                    //加入一个根元素
                    xmlelem = xmldoc.CreateElement("", "ROOT", "");
                    xmldoc.AppendChild(xmlelem);

                        //加入另外一个元素
                        xmlelem2 = xmldoc.CreateElement("BasicUserInfo");
                        xmlelem2 = xmldoc.CreateElement("", "BasicUserInfo", "");

                        xmlelem3 = xmldoc.CreateElement("", "birthday", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["Birthday"].ToString());
                        xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        xmlelem3 = xmldoc.CreateElement("", "cardClass", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CustLevel"].ToString());
                        xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        xmlelem3 = xmldoc.CreateElement("", "certificateCode", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CertificateCode"].ToString());
                        xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "certificateType", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CertificateType"].ToString());
                        xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "credit", "");
                        xmltext = xmldoc.CreateTextNode("");
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "custContactTel", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CustContactTel"].ToString());
                        xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "custID", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CustID"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        xmlelem3 = xmldoc.CreateElement("", "OuterCustID", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["outerID"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        xmlelem3 = xmldoc.CreateElement("", "custType", "");
                        string custType = "1"; 
                
                        //客户类型， 1：个人客户，2：政企客户
                        //判断依据： 若卡号为 9位且第四位为0，1，2， 或卡号为 16位 且第
                        if (UserAccount.Length == 9)
                        {
                            string tmpAA = UserAccount.Substring(3, 1);
                            if (tmpAA == "1" || tmpAA == "0")
                            {
                                custType = "2";
                            }
                        }
                        xmltext = xmldoc.CreateTextNode(custType);
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "enterpriseID", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["EnterpriseID"].ToString());
                        xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "extendField", "");
                        xmltext = xmldoc.CreateTextNode("");
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "realName", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["RealName"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "registration", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["RegistrationDate"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);


                        xmlelem3 = xmldoc.CreateElement("", "sex", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["Sex"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        //同步到积分系统时用status=02来标识反向注册
                        string status = ds.Tables[0].Rows[0]["status"].ToString();
                        if(RegistryType == "1")
                            status = "02";
                        
                        xmlelem3 = xmldoc.CreateElement("", "status", "");
                        xmltext = xmldoc.CreateTextNode(status);
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        xmlelem3 = xmldoc.CreateElement("", "uProvinceID", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["UProvinceID"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        xmlelem3 = xmldoc.CreateElement("", "userAccount", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["UserAccount"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);

                        xmlelem3 = xmldoc.CreateElement("", "userType", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["UserType"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                        xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
  
                        xmlelem3 = xmldoc.CreateElement("", "AuthenName", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["AuthenName"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                        xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                        xmlelem3 = xmldoc.CreateElement("", "AreaCode", "");
                        xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["AreaCode"].ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                        xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                        xmlelem3 = xmldoc.CreateElement("", "PaymentPwd", "");
                        xmltext = xmldoc.CreateTextNode(PaymentPwd);
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                        xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                    //保存创建好的XML文档

                   // xmldoc.Save(@".\BasicUserInfo.xml");
                        CustInfoXML = xmldoc.OuterXml;
                        CustInfoXML = CustInfoXML.Substring(CustInfoXML.IndexOf("<ROOT>"));
                        CustInfoXML = @"<?xml version='1.0' encoding='gb2312'?>" + CustInfoXML;



                    UnifyInterfaceForUCenter obj = new UnifyInterfaceForUCenter();
                //BaiscUserInfo CustInfoObj = new BaiscUserInfo();
                //CustInfoObj.birthday = ds.Tables[0].Rows[0]["Birthday"].ToString();
                //CustInfoObj.cardClass = ds.Tables[0].Rows[0]["CustLevel"].ToString();
                //CustInfoObj.certificateCode = ds.Tables[0].Rows[0]["CertificateCode"].ToString();
                //CustInfoObj.certificateType = ds.Tables[0].Rows[0]["CertificateType"].ToString();
                //CustInfoObj.credit = "";
                //CustInfoObj.custContactTel = ds.Tables[0].Rows[0]["CustContactTel"].ToString();
                //CustInfoObj.custId = ds.Tables[0].Rows[0]["CustID"].ToString();
                //CustInfoObj.custLevel = ds.Tables[0].Rows[0]["CustLevel"].ToString();
                //CustInfoObj.enterpriseId = ds.Tables[0].Rows[0]["EnterpriseID"].ToString();
                //CustInfoObj.extendField = "";
                //CustInfoObj.realName = ds.Tables[0].Rows[0]["RealName"].ToString();
                //CustInfoObj.registration = ds.Tables[0].Rows[0]["RegistrationDate"].ToString();
                //CustInfoObj.sex = ds.Tables[0].Rows[0]["Sex"].ToString();
                //CustInfoObj.status = ds.Tables[0].Rows[0]["Status"].ToString();
                //CustInfoObj.UProvinceID = ds.Tables[0].Rows[0]["UProvinceID"].ToString();
                //CustInfoObj.userAccount = ds.Tables[0].Rows[0]["UserAccount"].ToString();
                //CustInfoObj.userType = ds.Tables[0].Rows[0]["UserType"].ToString();
                //string sd = CustInfoObj.
                startTime = DateTime.Now;
                msg.Append("调用开始    " + startTime.ToString() + "." + startTime.Millisecond.ToString()); 
                msg.Append("    " + UserAccount);
               
                obj.Url = ConfigurationManager.AppSettings["UnifyInterUrl"];
                string strResult = obj.newCardCustomerInfoExport("35000000", CustInfoXML, DealType);

                XmlDocument xmlObj = new XmlDocument();
                xmlObj.LoadXml(strResult);
                Result = int.Parse(xmlObj.GetElementsByTagName("result")[0].InnerText);
                ErrMsg = xmlObj.GetElementsByTagName("errorDescription")[0].InnerText;

                DateTime endTime = DateTime.Now;
                long interval = endTime.Ticks - startTime.Ticks;
                string strInterval = interval.ToString();
                msg.Append("    调用结束    " + endTime.ToString() + "." + endTime.Millisecond.ToString() + "    " + strInterval + "\r\n");

                //Result = ResultObj.Result;
                //ErrMsg = ResultObj.ErrorDescription;

            }
            catch (Exception ex)
            {
                ErrMsg = "1," + ex.Message;
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                DateTime endTime = DateTime.Now;
                long interval = endTime.Ticks - startTime.Ticks;
                string strInterval = interval.ToString();
                msg.Append("    调用结束    " + endTime.ToString() + "." + endTime.Millisecond.ToString() + "    " + strInterval + "\r\n");
            }
            finally
            {
                string IsWriteLog = ConfigurationManager.AppSettings["IsWriteLog"];
                if (IsWriteLog == "0")
                {
                    //如果通知失败则插入用户信息失败记录表
                    // if (Result != 0)
                    //
                    cmd = new SqlCommand();
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
                    // }
                }

                BTUCenterInterfaceLog.CenterForBizTourLog("TmpNewCardCustomerInfoExport", msg);
            }

        }

        /// <summary> 通知短信网关
        /// 通知短信网关
        /// </summary>
        /// <param name="PhoneNum"></param>
        /// <param name="Message"></param>
        /// <param name="SPID"></param>
        public static void SendMessage(string PhoneNum, string Message, string SPID)
        {
            string ErrMsg = "";
            string Result = "-19999";
            try
            {
                UnifyInterfaceProxy obj = new UnifyInterfaceProxy();
                string UnifyInterUrl = ConfigurationManager.AppSettings["UnifyInterUrl"];
                obj.Url = UnifyInterUrl;
                Result = obj.insertSendDatas(PhoneNum, Message, SPID);
                XmlDocument xmlD = new XmlDocument();
                xmlD.LoadXml(Result);
                ErrMsg = xmlD.GetElementsByTagName("errorDescription")[0].InnerText;
                Result = xmlD.GetElementsByTagName("result")[0].InnerText;
                //如果失败则记入表
            }
            catch (Exception ex)
            {
                string df = ex.Message;
            }
            finally 
            {
                string IsWriteLog = ConfigurationManager.AppSettings["IsWriteLog"];
                try
                {
                    if (IsWriteLog == "0")
                    {
                        CommonBizRules.WriteDataLog("35000000", "", "", int.Parse(Result), ErrMsg, PhoneNum, "SendMessage");
                    }
                }
                catch { }
            }
        }


        /// <summary>从xml字符串中获取值
        /// 从xml字符串中获取值
        /// </summary>
        /// <param name="XmlStr"></param>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static string GetValueFromXmlStr(string XmlStr, string NodeName)
        {
            string nodeValue = "";

            try
            {
                XmlDocument xmlReader = new XmlDocument();
                xmlReader.LoadXml(XmlStr);

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

        #region 反向注册
        public static int ReverseUserRegistry(string ProvinceID, string SPID, string TimeStamp, string CertificateCode,
            string CertificateType, string RealName, string ContactPhone, string AreaCode, out string ErrMsg, out string CustID, out string UserAccount)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";
            SqlCommand cmd = new SqlCommand();

            StringBuilder msg = new StringBuilder();
            DateTime startTime = DateTime.Now;
            try
            {
                //myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                //cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_ReverseUserRegistry";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 3);
                parAreaCode.Value = (AreaCode == null) ? "" : AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 1);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parContactTel = new SqlParameter("@CustContactTel", SqlDbType.VarChar, 20);
                parContactTel.Value = ContactPhone;
                cmd.Parameters.Add(parContactTel);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parOCustID = new SqlParameter("@oCustID ", SqlDbType.VarChar, 16);
                parOCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOCustID);

                SqlParameter parOUserAccount = new SqlParameter("@oUserAccount ", SqlDbType.VarChar, 16);
                parOUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOUserAccount);

                startTime = DateTime.Now;
                msg.Append("调用开始    " + startTime.ToString() + "." + startTime.Millisecond.ToString());


                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parOCustID.Value.ToString();
                UserAccount = parOUserAccount.Value.ToString();
                msg.Append("    " + UserAccount);
                DateTime endTime = DateTime.Now;
                long interval = endTime.Ticks - startTime.Ticks;
                string strInterval = interval.ToString();
                msg.Append("    调用结束    " + endTime.ToString() + "." + endTime.Millisecond.ToString() + "    " + strInterval + "\r\n");
            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                DateTime endTime = DateTime.Now;
                long interval = endTime.Ticks - startTime.Ticks;
                string strInterval = interval.ToString();
                msg.Append("    调用结束    " + endTime.ToString() + "." + endTime.Millisecond.ToString() + "    " + strInterval + "\r\n");
            }
            finally
            {
                BTUCenterInterfaceLog.CenterForBizTourLog("TmpReserviseUserRestry", msg);
            }

            return Result;
        }


        #endregion

        #region 客户信息同步接口
        public static int NewCardCustomerInfoExport(string SPID,string dealType, string userType, string userAccount, string uProvinceID, 
            string status, string realName, string cardClass, string credit, string registration, string certificateCode, 
            string certificateType, string birthday, string sex, string custLevel, string custContactTel, string enterpriseID, 
            string extendField, out string CustID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_ReceiveCustInfo";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parDealType = new SqlParameter("@DealType", SqlDbType.VarChar, 1);
                parDealType.Value = dealType;
                cmd.Parameters.Add(parDealType);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = userType;
                cmd.Parameters.Add(parUserType);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = userAccount;
                cmd.Parameters.Add(parUserAccount);

                //SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                //parCustID.Value = custID;
                //cmd.Parameters.Add(parCustID);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = uProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                //SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 3);
                //parAreaCode.Value = areaCode;
                //cmd.Parameters.Add(parAreaCode);


                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Value = status;
                cmd.Parameters.Add(parStatus);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = realName;
                cmd.Parameters.Add(parRealName);


                SqlParameter parRegistration = new SqlParameter("@registration", SqlDbType.VarChar, 30);
                parRegistration.Value = registration;
                cmd.Parameters.Add(parRegistration);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = certificateCode;
                cmd.Parameters.Add(parCertificateCode);


                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 1);
                parCertificateType.Value = certificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 19);
                parBirthday.Value = birthday;
                cmd.Parameters.Add(parBirthday);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Value = custLevel;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parContactTel = new SqlParameter("@CustContactTel", SqlDbType.VarChar, 20);
                parContactTel.Value = custContactTel;
                cmd.Parameters.Add(parContactTel);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parOCustID = new SqlParameter("@oCustID ", SqlDbType.VarChar, 16);
                parOCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOCustID);

                //SqlParameter parOUserAccount = new SqlParameter("@oUserAccount ", SqlDbType.VarChar, 16);
                //parOUserAccount.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parOUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parOCustID.Value.ToString();
                //userAccount = parOUserAccount.Value.ToString();
            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }


            return Result;
        
        }
        #endregion

        #region 获取返回结果xml 串
        public static string GenerateResultXml(string strResult, string ErrMsg)
        {
            string Result = "";
            try
            {
                XmlElement XmlRoot = null;
                XmlDocument XmlDom = new XmlDocument();
                Result = "<Root></Root>";
                XmlDom.LoadXml(Result);
                XmlRoot = XmlDom.DocumentElement;

                VNetXml.XMLSetChild(XmlDom, XmlRoot, "result", strResult);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "errorDescription", ErrMsg);

                Result = @"<?xml version='1.0' encoding='gb2312' standalone='yes'?>" + XmlRoot.OuterXml;
            }
            catch
            {
                
            }

            return Result;
        
        }
        #endregion


        #region 获取outerID,ProvinceID结果xml 串
        public static string GenerateOuterIDXml(string OuterID, string ProvinceID)
        {
            string Result = "";
            try
            {
                XmlElement XmlRoot = null;
                XmlDocument XmlDom = new XmlDocument();
                Result = "<Root></Root>";
                XmlDom.LoadXml(Result);
                XmlRoot = XmlDom.DocumentElement;

                VNetXml.XMLSetChild(XmlDom, XmlRoot, "OuterID", OuterID);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "ProvinceID", ProvinceID);

                Result = @"<?xml version='1.0' encoding='gb2312' standalone='yes'?>" + XmlRoot.OuterXml;
            }
            catch
            {

            }

            return Result;

        }
        #endregion


        #region 电话号码绑定查询接口
        public static int BindPhoneQuery(string SPID, string PhoneNum,out string CustID, out string UserAccount, out string RealName, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";
            RealName = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_BT_V2_Interface_BindPhoneQuery";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);
                
                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 40);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 40);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 40);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parResalName = new SqlParameter("@RealName", SqlDbType.VarChar, 40);
                parResalName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResalName);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();
                RealName = parResalName.Value.ToString();
            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = "1,"+ex.Message;
            }

            return Result;



        }
        #endregion

        public static int UserAuthStyleQuery(string SPID, string UserAccount, string ExtendField, out AuthenStyleInfoRecord[] AuthenStyleInfoRecords,out string ErrMsg)
        {
            int result =0;
            ErrMsg = "";
            AuthenStyleInfoRecords = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "up_BT_V2_Interface_UserAuthStyleQuery";
            cmd.CommandType=CommandType.StoredProcedure;
            try
            {
                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parResult = new SqlParameter("@Result",SqlDbType.Int);
                parResult.Direction =ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg =new SqlParameter("@ErrMsg",SqlDbType.VarChar,256);
                parErrMsg.Direction=ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DataSet ds =DBUtility.FillData(cmd,DBUtility.BestToneCenterConStr);

                if(ds !=null && ds.Tables[0].Rows.Count>0)
                {
                    int len = ds.Tables[0].Rows.Count;
                    AuthenStyleInfoRecords = new AuthenStyleInfoRecord[len];

                    for(int i=0;i<len;i++)
                    {
                        AuthenStyleInfoRecord obj = new AuthenStyleInfoRecord();
                        obj.AuthenName = ds.Tables[0].Rows[i]["AuthenName"].ToString();
                        obj.AuthenType = ds.Tables[0].Rows[i]["AuthenType"].ToString();
                        obj.CustID = ds.Tables[0].Rows[i]["CustID"].ToString();
                        
                        AuthenStyleInfoRecords[i]=obj;
                    }

                }
                else
                {
                    AuthenStyleInfoRecords = null;
                }

                result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString();

            }catch(Exception ex)
            {
                ErrMsg += ex.Message;
                result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            }

            return result;

        }

        #region 获取密码接口

        public static int GetPassword(string CustID, out string Pwd, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            Pwd = "";
            ErrMsg = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "up_BT_V3_Interface_GetPassword";
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPwd = new SqlParameter("@Pwd", SqlDbType.VarChar, 256);
                parPwd.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPwd);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                Pwd = parPwd.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();

                if (Result == 0)
                    Pwd = CryptographyUtil.Decrypt(Pwd);
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;
        }
        #endregion

    }


}
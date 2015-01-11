/*********************************************************************************************************
 *     描述: 客户信息平台—语音平台注册接口（soap）
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 周涛
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-08-11
 *********************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 用户注册类
    /// </summary>
    public class UserRegistry
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(UserRegistry));
        /// <summary>
        /// 语音平台注册接口（soap）
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="UserType"></param>
        /// <param name="CardID"></param>
        /// <param name="Password"></param>
        /// <param name="UProvinceID"></param>
        /// <param name="AreaCode"></param>
        /// <param name="RealName"></param>
        /// <param name="AuthenPhone"></param>
        /// <param name="ContactTel"></param>
        /// <param name="IsNeedTourCard"></param>
        /// <param name="CertificateCode"></param>
        /// <param name="CertificateType"></param>
        /// <param name="Sex"></param>
        /// <param name="ExtendField"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="oCustID"></param>
        /// <param name="oTourCardID">9位商旅卡</param>
        /// <param name="sCardID">16位商旅卡</param>
        /// <returns></returns>
        public static  int getUserRegistry(string SPID, string UserType, string CardID, string Password, string UProvinceID, string AreaCode,
                                               string RealName, string UserName, string AuthenPhone, string ContactTel, string IsNeedTourCard, string CertificateCode,
                                               string CertificateType, string Sex, string ExtendField, out string ErrMsg, out string oCustID, out string oTourCardID, out string sCardID)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            oCustID = "";
            oTourCardID = "";
            sCardID = "";
            string PwdType = "";
            string RegisterSource = "00";
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            try
            {

                if (!CommonUtility.IsEmpty(ExtendField))
                {
                    PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");

                }

                if (!CommonUtility.IsEmpty(ExtendField))
                {
                    RegisterSource = CommonBizRules.GetValueFromXmlStr(ExtendField, "RegisterSource");

                }

                //--end 


                myCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV2";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = UserType;
                cmd.Parameters.Add(parUserType);

                SqlParameter parIsNeedTourCard = new SqlParameter("@IsNeedTourCard", SqlDbType.VarChar, 1);
                parIsNeedTourCard.Value = IsNeedTourCard;
                cmd.Parameters.Add(parIsNeedTourCard);

                SqlParameter parCardID = new SqlParameter("@CardID", SqlDbType.VarChar, 16);
                parCardID.Value = CardID;
                cmd.Parameters.Add(parCardID);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(Password);
                cmd.Parameters.Add(parPassword);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = UProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                parAreaCode.Value = AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parAuthenPhone = new SqlParameter("@AuthenPhone", SqlDbType.VarChar, 15);
                parAuthenPhone.Value = AuthenPhone;
                cmd.Parameters.Add(parAuthenPhone);

                SqlParameter parContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 15);
                parContactTel.Value = ContactTel;
                cmd.Parameters.Add(parContactTel);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);


                SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 1);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@oCustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter paroTourCardID = new SqlParameter("@oTourCardID", SqlDbType.VarChar, 9);
                paroTourCardID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paroTourCardID);

                SqlParameter parsCardID = new SqlParameter("@sCardID", SqlDbType.VarChar, 16);
                parsCardID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parsCardID);

                cmd.ExecuteNonQuery();

                oCustID = parCustID.Value.ToString().Trim();
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
                oTourCardID = paroTourCardID.Value.ToString().Trim();
                sCardID = parsCardID.Value.ToString().Trim();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            finally //修改 刘春利20091110
            {
                if (myCon.State != ConnectionState.Closed)
                {
                    myCon.Close();
                }
            }

            return Result;
        }


        /// <summary>
        /// 作者：李宏图   时间：2009-8-18
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="UProvinceID"></param>
        /// <param name="AreaCode"></param>
        /// <param name="CardType"></param>
        /// <param name="CustLevel"></param>
        /// <param name="CardRegSource"></param>
        /// <param name="CardRegType"></param>
        /// <param name="oTourCardID"></param>
        /// <param name="sCardID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int GetTourCard(string CustID, string CardID, string UProvinceID, string AreaCode, int CardType, string CustLevel, string CardRegSource, string CardRegType,
                                              out string oTourCardID, out string sCardID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            oTourCardID = "";
            sCardID = "";
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            myCon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_OV3_Interface_UserRegistryV2";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parCardID = new SqlParameter("@parCardID", SqlDbType.VarChar, 16);
                parCardID.Value = CardID;
                cmd.Parameters.Add(parCardID);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = UProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 3);
                parAreaCode.Value = AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parCardType = new SqlParameter("@CardType", SqlDbType.Int);
                parCardType.Value = CardType;
                cmd.Parameters.Add(parCardType);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Value = CustLevel;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parCardRegSource = new SqlParameter("@CardRegSource", SqlDbType.VarChar, 2);
                parCardRegSource.Value = CardRegSource;
                cmd.Parameters.Add(parCardRegSource);

                SqlParameter parCardRegType = new SqlParameter("@CardRegType", SqlDbType.VarChar, 1);
                parCardRegType.Value = CardRegType;
                cmd.Parameters.Add(parCardRegType);

                SqlParameter parsUserAccount = new SqlParameter("@sUserAccount", SqlDbType.VarChar, 9);
                parsUserAccount.Direction = ParameterDirection.Output; ;
                cmd.Parameters.Add(parsUserAccount);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                cmd.ExecuteNonQuery();
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
                oTourCardID = parsUserAccount.Value.ToString().Trim();
                sCardID = parUserAccount.Value.ToString().Trim();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = "商旅卡生成失败！" + ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;

            }
            return Result;
        }



        public static int getUserRegistryV2(string SPID, string UserType, string CardID, string Password, string UProvinceID, string AreaCode,
                                               string RealName, string UserName, string AuthenPhone, string ContactTel, string IsNeedTourCard, string CertificateCode,
                                               string CertificateType, string Sex, string ExtendField, out string ErrMsg, out string oCustID, out string oTourCardID, out string sCardID)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            oCustID = "";
            oTourCardID = "";
            sCardID = "";
            string PwdType = "";
            string RegisterSource = "2";

            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            try
            {

                if (!CommonUtility.IsEmpty(ExtendField))
                {
                    PwdType = CommonBizRules.GetValueFromXmlStr(ExtendField, "PwdType");

                }

                if (!CommonUtility.IsEmpty(ExtendField))
                {
                    RegisterSource = CommonBizRules.GetValueFromXmlStr(ExtendField, "RegisterSource");

                }

                //--end 


                myCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV3";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = UserType;
                cmd.Parameters.Add(parUserType);

                SqlParameter parIsNeedTourCard = new SqlParameter("@IsNeedTourCard", SqlDbType.VarChar, 1);
                parIsNeedTourCard.Value = IsNeedTourCard;
                cmd.Parameters.Add(parIsNeedTourCard);

                SqlParameter parCardID = new SqlParameter("@CardID", SqlDbType.VarChar, 16);
                parCardID.Value = CardID;
                cmd.Parameters.Add(parCardID);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(Password);
                cmd.Parameters.Add(parPassword);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = UProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                parAreaCode.Value = AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parAuthenPhone = new SqlParameter("@AuthenPhone", SqlDbType.VarChar, 15);
                parAuthenPhone.Value = AuthenPhone;
                cmd.Parameters.Add(parAuthenPhone);

                SqlParameter parContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 15);
                parContactTel.Value = ContactTel;
                cmd.Parameters.Add(parContactTel);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);


                SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 1);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);

                SqlParameter parRegisterSource = new SqlParameter("@RegisterSource", SqlDbType.Int);
                parRegisterSource.Value = Convert.ToInt16(RegisterSource);
                cmd.Parameters.Add(parRegisterSource);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@oCustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter paroTourCardID = new SqlParameter("@oTourCardID", SqlDbType.VarChar, 9);
                paroTourCardID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paroTourCardID);

                SqlParameter parsCardID = new SqlParameter("@sCardID", SqlDbType.VarChar, 16);
                parsCardID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parsCardID);

                cmd.ExecuteNonQuery();

                oCustID = parCustID.Value.ToString().Trim();
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
                oTourCardID = paroTourCardID.Value.ToString().Trim();
                sCardID = parsCardID.Value.ToString().Trim();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            finally //修改 刘春利20091110
            {
                if (myCon.State != ConnectionState.Closed)
                {
                    myCon.Close();
                }
            }

            return Result;
        }

        /// <summary>
        /// web快速注册函数
        /// 作者：李宏图      时间：2012-8-18
        /// 修改：          时间：
        /// </summary>
        /// <returns></returns>
        public static int quickUserRegistryWeb(string SPID, string password, string telephone, string phonestate, out string CustID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV2WebQuick";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

               SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(password);
                cmd.Parameters.Add(parPassword);

                SqlParameter parTelephone = new SqlParameter("@Telephone", SqlDbType.VarChar, 20);
                parTelephone.Value = CommonUtility.IsEmpty(telephone) ? "" : telephone;
                cmd.Parameters.Add(parTelephone);

                SqlParameter parPhoneState = new SqlParameter("@PhoneState", SqlDbType.VarChar, 1);
                parPhoneState.Value = phonestate;
                cmd.Parameters.Add(parPhoneState);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;


        }

        /// <summary>
        /// 0门槛注册
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int UserRegisterWebLowStint(String SPID, String UserName, String PassWord, out String CustID, out String ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryWebLowStint";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 500);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);


                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(PassWord);
                cmd.Parameters.Add(parPassword);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;
        }


        public static int UserRegisterWebLowStintV3(String SPID, String UserName, String PassWord, String Device,String ShareCode, out String CustID, out String ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryWebLowStintV3";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 500);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(PassWord);
                cmd.Parameters.Add(parPassword);

                SqlParameter parDevice = new SqlParameter("@Device", SqlDbType.VarChar, 128);
                parDevice.Value = Device;
                cmd.Parameters.Add(parDevice);


                SqlParameter parShareCode = new SqlParameter("@ShareCode", SqlDbType.VarChar, 6);
                parShareCode.Value = ShareCode;
                cmd.Parameters.Add(parShareCode);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;
        }



        public static int UserRegisterWebLowStintV2(String SPID, String UserName, String PassWord, String Device, out String CustID, out String ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryWebLowStintV2";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 500);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(PassWord);
                cmd.Parameters.Add(parPassword);

                SqlParameter parDevice = new SqlParameter("@Device", SqlDbType.VarChar, 128);
                parDevice.Value = Device;
                cmd.Parameters.Add(parDevice);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;
        }



        /// <summary>
        /// web快速注册函数
        /// 作者：李宏图      时间：2012-8-18
        /// 修改：          时间：
        /// </summary>
        /// <returns></returns>
        public static int quickUserRegistryWebV4(string SPID, string password, string telephone, string phonestate, string username, string email, string device,out string CustID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            int RegistrationSouce = 2; // 默认是web注册

            if ("android".Equals(device)) {
                RegistrationSouce = 22;  // wap 且 android
            }

            if ("ios".Equals(device)) {
                RegistrationSouce = 21;  // wap 且 ios
            }

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV4WebQuick";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(password);
                cmd.Parameters.Add(parPassword);

                SqlParameter parTelephone = new SqlParameter("@Telephone", SqlDbType.VarChar, 20);
                parTelephone.Value = CommonUtility.IsEmpty(telephone) ? "" : telephone;
                cmd.Parameters.Add(parTelephone);

                SqlParameter parPhoneState = new SqlParameter("@PhoneState", SqlDbType.VarChar, 1);
                parPhoneState.Value = phonestate;
                cmd.Parameters.Add(parPhoneState);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 500);
                parUserName.Value = username;
                cmd.Parameters.Add(parUserName);


                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 200);
                parEmail.Value = email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parRegistrationSource = new SqlParameter("@RegistrationSource", SqlDbType.Int);
                parRegistrationSource.Value = RegistrationSouce;
                cmd.Parameters.Add(parRegistrationSource);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;


        }

        /// <summary>
        /// web快速注册函数
        /// 作者：李宏图      时间：2012-8-18
        /// 修改：          时间：
        /// </summary>
        /// <returns></returns>
        public static int quickUserRegistryWebV3(string SPID, string password, string telephone, string phonestate, string username,string email,out string CustID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV3WebQuick";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(password);
                cmd.Parameters.Add(parPassword);

                SqlParameter parTelephone = new SqlParameter("@Telephone", SqlDbType.VarChar, 20);
                parTelephone.Value = CommonUtility.IsEmpty(telephone) ? "" : telephone;
                cmd.Parameters.Add(parTelephone);

                SqlParameter parPhoneState = new SqlParameter("@PhoneState", SqlDbType.VarChar, 1);
                parPhoneState.Value = phonestate;
                cmd.Parameters.Add(parPhoneState);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 500);
                parUserName.Value = username;
                cmd.Parameters.Add(parUserName);


                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 200);
                parEmail.Value = email;
                cmd.Parameters.Add(parEmail);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;


        }


        protected static void log(string str)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("createBesttoneAccount-userregistry", msg);
        }

        /// <summary>
        /// 号码百事通账户回写客户信息 - 李宏图
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="RealName"></param>
        /// <param name="CertificateCode"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int WriteBackBestToneAccountToCustInfo(string SPID, string CustID, string RealName, string CertificateCode, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_WriteBackBestToneAccountToCustInfo";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 20);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
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
       

        /// <summary>
        /// 开通号百百事通账户函数
        /// 作者：李宏图      时间：2012-8-18
        /// 修改：          时间：Insert into BesttoneAccount(BestPayAccount,CustID,PW,CreateTime,Status) values(@BestPayAccount,@CustID,@PW,@CreateTime,@Status)");
        /// </summary>
        /// <returns></returns>
        public static int CreateBesttoneAccount(string SPID, string CustID, string Account, out string ErrMsg)
        {

            log(String.Format("CustID:{0},Acount:{1}",CustID,Account));
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
     
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CreateBesttoneAccount";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccount = new SqlParameter("@Account", SqlDbType.VarChar, 20);
                parAccount.Value = Account;
                cmd.Parameters.Add(parAccount);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("CustID:{0},Acount:{1}", CustID, Account));
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
               
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;


        }

        //在开户请求前纪录日志
        public static int BeforeCreateBesttoneAccount(string SPID,string TransactionID,string CustID,string Account,out string ErrMsg)
        {
            log(String.Format("before-CustID:{0},Acount:{1},TransactionID{2}", CustID, Account, TransactionID));
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_BeforeBesttoneAccount";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccount = new SqlParameter("@Account", SqlDbType.VarChar, 20);
                parAccount.Value = Account;
                cmd.Parameters.Add(parAccount);


                SqlParameter parTransactionID = new SqlParameter("@TransactionID", SqlDbType.VarChar, 20);
                parTransactionID.Value = TransactionID;
                cmd.Parameters.Add(parTransactionID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("before-CustID:{0},Acount:{1},TransactionID{2}", CustID, Account, TransactionID));
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;
        }



        //在开户请求前纪录日志
        public static int AfterCreateBesttoneAccount(string SPID, string TransactionID, string CustID, string Account, out string ErrMsg)
        {
            log(String.Format("before-CustID:{0},Acount:{1},TransactionID{2}", CustID, Account, TransactionID));
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_AfterBesttoneAccount";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccount = new SqlParameter("@Account", SqlDbType.VarChar, 20);
                parAccount.Value = Account;
                cmd.Parameters.Add(parAccount);


                SqlParameter parTransactionID = new SqlParameter("@TransactionID", SqlDbType.VarChar, 20);
                parTransactionID.Value = TransactionID;
                cmd.Parameters.Add(parTransactionID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("before-CustID:{0},Acount:{1},TransactionID{2}", CustID, Account, TransactionID));
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;
        }


        //
        public static int OnlyBindingBesttoneAccount(string SPID, string TransactionID, string CustID, string Account, out string ErrMsg)
        {
            log(String.Format("before-CustID:{0},Acount:{1},TransactionID{2}", CustID, Account, TransactionID));
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_OnlyBindingBesttoneAccount";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccount = new SqlParameter("@Account", SqlDbType.VarChar, 20);
                parAccount.Value = Account;
                cmd.Parameters.Add(parAccount);


                SqlParameter parTransactionID = new SqlParameter("@TransactionID", SqlDbType.VarChar, 20);
                parTransactionID.Value = TransactionID;
                cmd.Parameters.Add(parTransactionID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("before-CustID:{0},Acount:{1},TransactionID{2}", CustID, Account, TransactionID));
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;
        }



        /// <summary>
        /// 开通号百百事通账户函数
        /// 作者：李宏图      时间：2012-8-18
        /// 修改：          时间：Insert into BesttoneAccount(BestPayAccount,CustID,PW,CreateTime,Status) values(@BestPayAccount,@CustID,@PW,@CreateTime,@Status)");
        /// </summary>
        /// <returns></returns>
        public static int CreateBesttoneAccountV2(string SPID, string CustID,string RealName,string Account, out string ErrMsg)
        {

            log(String.Format("CustID:{0},Acount:{1},RealName{2}", CustID, Account,RealName));
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CreateBesttoneAccountV2";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccount = new SqlParameter("@Account", SqlDbType.VarChar, 20);
                parAccount.Value = Account;
                cmd.Parameters.Add(parAccount);


                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 20);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("CustID:{0},Acount:{1},RealName{2}", CustID, Account,RealName));
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;


        }


        /// <summary>
        /// web注册函数
        /// 作者：周涛      时间：2009-8-18
        /// 修改：          时间：
        /// </summary>
        /// <returns></returns>
        public static int getUserRegistryWeb(string SPID,string username,string realname,string password,string telephone,string phonestate,string email,string emailstate,
                                            string NickName, string CertificateType,string certno,string sex,string birthday,string EduLevel,string IncomeLevel,string Province,
                                            string Area, out string CustID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV2Web";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Value = username;
                cmd.Parameters.Add(parUserName);

                SqlParameter parFullName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parFullName.Value = realname;
                cmd.Parameters.Add(parFullName);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(password);
                cmd.Parameters.Add(parPassword);

                SqlParameter parTelephone = new SqlParameter("@Telephone", SqlDbType.VarChar, 20);
                parTelephone.Value = CommonUtility.IsEmpty(telephone) ? "" : telephone;
                cmd.Parameters.Add(parTelephone);

                SqlParameter parPhoneState = new SqlParameter("@PhoneState", SqlDbType.VarChar, 1);
                parPhoneState.Value = phonestate;
                cmd.Parameters.Add(parPhoneState);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 256);
                parEmail.Value = CommonUtility.IsEmpty(email) ? "" : email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parEmailState = new SqlParameter("@EmailState", SqlDbType.VarChar, 1);
                parEmailState.Value = emailstate;
                cmd.Parameters.Add(parEmailState);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Value = CommonUtility.IsEmpty(NickName) ? "" : NickName;
                cmd.Parameters.Add(parNickName);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar,2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parCertno = new SqlParameter("@Certno", SqlDbType.VarChar, 30);
                parCertno.Value = CommonUtility.IsEmpty(certno) ? "" : certno;
                cmd.Parameters.Add(parCertno);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = sex;
                cmd.Parameters.Add(parSex);

                //DateTime a = DateTime.Parse(birthday);

                if (!CommonUtility.IsEmpty(birthday))
                {
                    SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.DateTime, 8);
                    parBirthday.Value = DateTime.Parse(birthday);
                    cmd.Parameters.Add(parBirthday);
                }
                else
                {
                    SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.DateTime, 8);
                    parBirthday.Value = DBNull.Value;
                    cmd.Parameters.Add(parBirthday);
                }

                SqlParameter parEduLevel = new SqlParameter("@EduLevel", SqlDbType.VarChar, 2);
                parEduLevel.Value = EduLevel;
                cmd.Parameters.Add(parEduLevel);

                SqlParameter parIncomeLevel = new SqlParameter("@IncomeLevel", SqlDbType.VarChar, 1);
                parIncomeLevel.Value = IncomeLevel;
                cmd.Parameters.Add(parIncomeLevel);

                SqlParameter parProvince = new SqlParameter("@Province", SqlDbType.VarChar, 2);
                parProvince.Value = Province;
                cmd.Parameters.Add(parProvince);

                SqlParameter parArea = new SqlParameter("@Area", SqlDbType.VarChar, 3);
                parArea.Value = Area;
                cmd.Parameters.Add(parArea);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;
        }

        /// <summary>
        /// 全国CRM注册用户(先查看对应用户在客户信息平台是否存在，如果存在则更新，如果不存在，则插入)
        /// 作者：张英杰   时间：2009-9-8
        /// 修改：
        /// </summary>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>int</returns>
        public static int getUserRegistryCrm(string ProvinceID ,string AreaID,string CustType,string CertificateType,string CertificateCode,string RealName,
                                              string CustLevel, string Sex, string OuterID, string SourceSPID, string CustAddress, out string OCustID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OCustID = "";
           

            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            myCon.Open();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV2Crm";



                SqlParameter parUProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaID", SqlDbType.VarChar, 4);
                parAreaCode.Value = AreaID;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                parCustType.Value = CustType;
                cmd.Parameters.Add(parCustType);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);


                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 100);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Value = CustLevel;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar,30);
                parOuterID.Value = OuterID;
                cmd.Parameters.Add(parOuterID);

                SqlParameter parSourceSPID = new SqlParameter("@SourceSPID", SqlDbType.VarChar, 8);
                parSourceSPID.Value = SourceSPID;
                cmd.Parameters.Add(parSourceSPID);


                SqlParameter parCustAddress = new SqlParameter("@CustAddress", SqlDbType.VarChar, 256);
                parCustAddress.Value = CustAddress;
                cmd.Parameters.Add(parCustAddress);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parCustID = new SqlParameter("OCustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                cmd.ExecuteNonQuery();

                OCustID = parCustID.Value.ToString().Trim();
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }


        /// <summary>
        /// UDB用户注册更新到号百
        /// </summary>
        public static Int32 getUserRegistryUnifyPlatform(UnifyAccountInfo accountInfo, out String CustID, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = String.Empty;
            String province = String.Empty;
            String city = String.Empty;
            
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV2UnifyPlatform";
                    cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV2UnifyPlatform";
                    //up_Customer_V3_Interface_UserRegistryV2UDB


                    SqlParameter parProvince = new SqlParameter("@Province", SqlDbType.VarChar);
                    parProvince.Value = accountInfo.province;  // 这里要做转换
                    
                    cmd.Parameters.Add(parProvince);

                    SqlParameter parCity = new SqlParameter("@City", SqlDbType.VarChar);
                    parCity.Value = accountInfo.city;
                    cmd.Parameters.Add(parCity);

                    SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar);
                    parAuthenName.Value = accountInfo.mobileName;
                    cmd.Parameters.Add(parAuthenName);

                    SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar);
                    //parAuthenType.Value = UDBBusiness.ConvertAuthenType(Convert.ToString(accountInfo.userType));
                    parAuthenType.Value = "2";  // 先写死,因为现在只有手机
                    
                    cmd.Parameters.Add(parAuthenType);

                    SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar);
                    parCustType.Value = "42";
                    cmd.Parameters.Add(parCustType);

                    SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar);
                    parRealName.Value = String.IsNullOrEmpty(accountInfo.aliasName) ? accountInfo.nickName : accountInfo.zhUserName;
                    cmd.Parameters.Add(parRealName);

                    SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar);
                    parUserName.Value = accountInfo.userName;
                    cmd.Parameters.Add(parUserName);

                    SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar);
                    parNickName.Value = accountInfo.nickName;
                    cmd.Parameters.Add(parNickName);

                    Random random = new Random();
                    String randomPwd = random.Next(100000, 999999).ToString();
                    SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                    parPassword.Value = CryptographyUtil.Encrypt(randomPwd);
                    cmd.Parameters.Add(parPassword);

                    SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar);
                    parOuterID.Value = accountInfo.pUserId;
                    cmd.Parameters.Add(parOuterID);

                    SqlParameter parSourceSPID = new SqlParameter("@SourceSPID", SqlDbType.VarChar);
                    parSourceSPID.Value = "35999998";
                    cmd.Parameters.Add(parSourceSPID);


                    SqlParameter parOutCustID = new SqlParameter("@OCustID", SqlDbType.VarChar, 16);
                    parOutCustID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parOutCustID);


                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                    Result = Convert.ToInt32(parResult.Value);
                    ErrMsg = parErrMsg.Value.ToString();
                    CustID = parOutCustID.Value.ToString();

                }
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// UDB用户注册更新到号百
        /// </summary>
        public static Int32 getUserRegistryUDB(UDBAccountInfo accountInfo, out String CustID, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = String.Empty;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "up_Customer_V3_Interface_UserRegistryV2UDB";

                    SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar);
                    parProvinceID.Value = accountInfo.ProvinceID;
                    cmd.Parameters.Add(parProvinceID);

                    SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar);
                    parAreaID.Value = String.Empty;
                    cmd.Parameters.Add(parAreaID);

                    SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar);
                    parAuthenName.Value = accountInfo.UserID;
                    cmd.Parameters.Add(parAuthenName);

                    SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar);
                    parAuthenType.Value = UDBBusiness.ConvertAuthenType(accountInfo.NumFlag);
                    cmd.Parameters.Add(parAuthenType);

                    SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar);
                    parCustType.Value = "42";
                    cmd.Parameters.Add(parCustType);

                    SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar);
                    parRealName.Value = String.IsNullOrEmpty(accountInfo.Alias) ? accountInfo.UserID : accountInfo.Alias;
                    //parRealName.Value = String.Empty;
                    cmd.Parameters.Add(parRealName);

                    SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar);
                    //parUserName.Value = String.IsNullOrEmpty(accountInfo.Alias) ? accountInfo.UserID : accountInfo.Alias;
                    parUserName.Value = String.Empty;
                    cmd.Parameters.Add(parUserName);

                    SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar);
                    parNickName.Value = String.IsNullOrEmpty(accountInfo.Alias) ? accountInfo.UserID : accountInfo.Alias;
                    cmd.Parameters.Add(parNickName);

                    Random random = new Random();
                    String randomPwd = random.Next(100000, 999999).ToString();
                    SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                    parPassword.Value = CryptographyUtil.Encrypt(randomPwd);
                    cmd.Parameters.Add(parPassword);

                    SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar);
                    parOuterID.Value = accountInfo.PUserID;
                    cmd.Parameters.Add(parOuterID);

                    SqlParameter parSourceSPID = new SqlParameter("@SourceSPID", SqlDbType.VarChar);
                    parSourceSPID.Value = accountInfo.SourceSPID;
                    cmd.Parameters.Add(parSourceSPID);


                    SqlParameter parOutCustID = new SqlParameter("@OCustID", SqlDbType.VarChar, 16);
                    parOutCustID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parOutCustID);


                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                    Result = Convert.ToInt32(parResult.Value);
                    ErrMsg = parErrMsg.Value.ToString();
                    CustID = parOutCustID.Value.ToString();

                }
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// 潜在客户注册
        /// </summary>
        public static Int32 getUserRegistryPotential(string SPID, string UserType, string UProvinceID, string AreaCode,
                                               string RealName, string UserName, string ContactTel, string CertificateCode,
                                               string CertificateType, string Sex, out string oCustID,out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            oCustID = "";
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            try
            {
                myCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistry_Potential";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = UserType;
                cmd.Parameters.Add(parUserType);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = UProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                parAreaCode.Value = AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 15);
                parContactTel.Value = ContactTel;
                cmd.Parameters.Add(parContactTel);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parCustID = new SqlParameter("@oCustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                
                cmd.ExecuteNonQuery();

                oCustID = parCustID.Value.ToString().Trim();
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            finally
            {
                if (myCon.State != ConnectionState.Closed)
                {
                    myCon.Close();
                }
            }

            return Result;
        }

        /// <summary>
        /// 将潜在客户注册为正式客户
        /// </summary>
        public static Int32 getPotentialUserToRegistryUser(string SPID, string CustID, string UserType, string PwdType, string Password, string UProvinceID, string AreaCode, 
                                                string RealName, string UserName, string Sex, string AuthenPhone, string ContactTel, string CertificateCode, string CertificateType, 
                                                string IsNeedTourCard, string CardID, out string outTourCardID, out string outCardID, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            outTourCardID = "";
            outCardID = "";
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            try
            {
                myCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UserRegistry_PotentialUserToRegistry";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = UserType;
                cmd.Parameters.Add(parUserType);

                SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 1);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 128);
                parPassword.Value = CryptographyUtil.Encrypt(Password);
                cmd.Parameters.Add(parPassword);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = UProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                parAreaCode.Value = AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parAuthenPhone = new SqlParameter("@AuthenPhone", SqlDbType.VarChar, 15);
                parAuthenPhone.Value = AuthenPhone;
                cmd.Parameters.Add(parAuthenPhone);

                SqlParameter parContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 15);
                parContactTel.Value = ContactTel;
                cmd.Parameters.Add(parContactTel);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parIsNeedTourCard = new SqlParameter("@IsNeedTourCard", SqlDbType.VarChar, 1);
                parIsNeedTourCard.Value = IsNeedTourCard;
                cmd.Parameters.Add(parIsNeedTourCard);

                SqlParameter parCardID = new SqlParameter("@CardID", SqlDbType.VarChar, 16);
                parCardID.Value = CardID;
                cmd.Parameters.Add(parCardID);

                SqlParameter paroTourCardID = new SqlParameter("@outTourCardID", SqlDbType.VarChar, 9);
                paroTourCardID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paroTourCardID);

                SqlParameter parsCardID = new SqlParameter("@outCardID", SqlDbType.VarChar, 16);
                parsCardID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parsCardID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                cmd.ExecuteNonQuery();

                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
                outTourCardID = paroTourCardID.Value.ToString().Trim();
                outCardID = parsCardID.Value.ToString().Trim();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            finally
            {
                if (myCon.State != ConnectionState.Closed)
                {
                    myCon.Close();
                }
            }

            return Result;
        }

        /// <summary>
        /// 生成商旅卡
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="CardID"></param>
        /// <param name="ProvinceID"></param>
        /// <param name="AreaID"></param>
        /// <param name="CardType">卡的类型1：个人卡2：政企卡</param>
        /// <param name="CustLevel">卡的级别</param>
        /// <param name="CardRegSource">卡注册来源</param>
        /// <param name="CardRegType">注册类型</param>
        /// <param name="CardID">16位卡</param>
        /// <param name="sCardID">9位卡</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns></returns>
        public static int GenerationCard(string SPID, string CustID, string CardID, string ProvinceID, string AreaID, int CardType,
            string CustLevel, string CardRegSource, string CardRegType, out string UserAccount, out string sUserAccount, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
            UserAccount = "";
            sUserAccount = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_UserRegistryV2";

                //SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                //parSPID.Value = SPID;
                //cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parCardID = new SqlParameter("@parCardID", SqlDbType.VarChar, 16);
                parCardID.Value = CardID;
                cmd.Parameters.Add(parCardID);

                SqlParameter parProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("@AreaCode", SqlDbType.VarChar, 3);
                parAreaID.Value = AreaID;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parCardType = new SqlParameter("@CardType", SqlDbType.Int);
                parCardType.Value = CardType;
                cmd.Parameters.Add(parCardType);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Value = CustLevel;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parCardRegSource = new SqlParameter("@CardRegSource", SqlDbType.VarChar, 2);
                parCardRegSource.Value = CardRegSource;
                cmd.Parameters.Add(parCardRegSource);

                SqlParameter parCardRegType = new SqlParameter("@CardRegType", SqlDbType.VarChar, 1);
                parCardRegType.Value = CardRegType;
                cmd.Parameters.Add(parCardRegType);

                ///////////////////////////////////////

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parsUserAccount = new SqlParameter("@sUserAccount", SqlDbType.VarChar, 9);
                parsUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parsUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();
                sUserAccount = parsUserAccount.Value.ToString();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;

        }

        /// <summary>
        /// 写日志功能
        /// </summary>
        protected static void WriteLog(String str)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("UserRegistryTest", msg);
        }
    }
}

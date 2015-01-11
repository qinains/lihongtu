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

namespace Linkage.BestTone.CrmSSO.Rule
{
    public class UserRegistry
    {

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

            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            myCon.Open();
            try
            {

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

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 50);
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

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 1);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

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
            return Result;
        }

  

        /// <summary>
        /// web注册函数
        /// 作者：周涛      时间：2009-8-18
        /// 修改：          时间：
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="username"></param>
        /// <param name="fullname"></param>
        /// <param name="password"></param>
        /// <param name="telephone"></param>
        /// <param name="phonestate"></param>
        /// <param name="email"></param>
        /// <param name="emailstate"></param>
        /// <param name="CertificateType"></param>
        /// <param name="certno"></param>
        /// <param name="sex"></param>
        /// <param name="birthday"></param>
        /// <param name="EduLevel"></param>
        /// <param name="IncomeLevel"></param>
        /// <param name="Province"></param>
        /// <param name="Area"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int getUserRegistryWeb(string SPID,string username,string fullname,string password,string telephone,string phonestate,string email,string emailstate,
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

                SqlParameter parFullName = new SqlParameter("@FullName", SqlDbType.VarChar, 50);
                parFullName.Value = fullname;
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

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 1);
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
        /// 全国CRM注册用户
        /// 作者：张英杰   时间：2009-9-8
        /// 修改：
        /// </summary>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>int</returns>
        public static int getUserRegistryCrm(string ProvinceID ,string AreaID,string CustType,string CertificateType,string CertificateCode,string RealName,
                                              string CustLevel, string Sex, string OuterID, string SourceSPID, string CustAddress, out string OCustID, out string ErrMsg, out string dealType)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OCustID = "";
            dealType = "";

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

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 1);
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

                SqlParameter pardealType = new SqlParameter("@dealType", SqlDbType.VarChar, 2);
                pardealType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pardealType);

                cmd.ExecuteNonQuery();

                OCustID = parCustID.Value.ToString().Trim();
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim() + ProvinceID;
                dealType = pardealType.Value.ToString().Trim();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }

    }
}

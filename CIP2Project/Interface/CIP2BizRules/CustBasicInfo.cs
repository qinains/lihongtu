/*********************************************************************************************************
 *     描述: 客户信息平台—客户基本信息查询接口
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 周涛
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 *********************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    public class CustBasicInfo
    {



        /// <summary>
        /// 客户基本信息查询接口返回记录
        /// 作者：李宏图      时间：2009-7-31
        /// 修改：          时间：
        /// </summary>
        public static int getCustInfoByCustId(string SPID, string CustID, out string ErrMsg, out string OuterID, out string Status, out string CustType,
                             out string CustLevel, out string RealName, out string UserName, out string NickName, out string CertificateCode,
                             out string CertificateType, out string Sex, out string Email, out string EnterpriseID, out string ProvinceID, out string AreaID, out string Registration)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OuterID = "";
            Status = "";
            CustType = "";
            CustLevel = "";
            RealName = "";
            UserName = "";
            NickName = "";
            CertificateCode = "";
            CertificateType = "";
            Sex = "";
            Email = "";
            EnterpriseID = "";
            ProvinceID = "";
            AreaID = "";
            Registration = "";

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_QueryCustBasicInfoByCustID";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar, 20);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);

                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parStatus);

                SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSex);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEmail);

                SqlParameter parEnterpriseID = new SqlParameter("@EnterpriseID", SqlDbType.VarChar, 2);
                parEnterpriseID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEnterpriseID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 6);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parRegistration = new SqlParameter("@Registration", SqlDbType.DateTime, 8);
                parRegistration.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRegistration);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                OuterID = parOuterID.Value.ToString();
                Status = parStatus.Value.ToString();
                CustType = parCustType.Value.ToString();
                CustLevel = parCustLevel.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                CertificateCode = parCertificateCode.Value.ToString();
                CertificateType = parCertificateType.Value.ToString();
                Sex = parSex.Value.ToString();
                Email = parEmail.Value.ToString();
                EnterpriseID = parEnterpriseID.Value.ToString();
                ProvinceID = parProvinceID.Value.ToString();
                AreaID = parAreaID.Value.ToString();
                Registration = parRegistration.Value.ToString();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }


        public static int getCustExtendInfo(string SPID, string CustID, out string ErrMsg, out string BirthDay)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            BirthDay = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustExcentInfoQuery";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parBirthDay = new SqlParameter("@BirthDay", SqlDbType.DateTime);
                parBirthDay.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(BirthDay);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

           
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                BirthDay = parBirthDay.Value.ToString();
        
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }

            return Result;
        }



        /// <summary>
        /// 客户基本信息查询接口返回记录
        /// 作者：周涛      时间：2009-7-31
        /// 修改：          时间：
        /// </summary>
        public static int getCustInfoV2(string SPID, string CustID, out string ErrMsg, out string OuterID, out string Status, out string CustType,
                             out string CustLevel, out string RealName, out string UserName, out string NickName, out string CertificateCode,
                             out string CertificateType, out string Sex, out string Email, out string CreateTime,out string EnterpriseID, out string ProvinceID, out string AreaID, out string Registration)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OuterID = "";
            Status = "";
            CustType = "";
            CustLevel = "";
            RealName = "";
            UserName = "";
            NickName = "";
            CertificateCode = "";
            CertificateType = "";
            Sex = "";
            Email = "";
            CreateTime = "";
            EnterpriseID = "";
            ProvinceID = "";
            AreaID = "";
            Registration = "";

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustBasicInfoQueryV2";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar, 20);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);

                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parStatus);

                SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSex);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEmail);

                SqlParameter parCreateTime = new SqlParameter("@CreateTime", SqlDbType.VarChar, 100);
                parCreateTime.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCreateTime);


                SqlParameter parEnterpriseID = new SqlParameter("@EnterpriseID", SqlDbType.VarChar, 2);
                parEnterpriseID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEnterpriseID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 6);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parRegistration = new SqlParameter("@Registration", SqlDbType.DateTime, 8);
                parRegistration.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRegistration);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                OuterID = parOuterID.Value.ToString();
                Status = parStatus.Value.ToString();
                CustType = parCustType.Value.ToString();
                CustLevel = parCustLevel.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                CertificateCode = parCertificateCode.Value.ToString();
                CertificateType = parCertificateType.Value.ToString();
                Sex = parSex.Value.ToString();
                Email = parEmail.Value.ToString();
                EnterpriseID = parEnterpriseID.Value.ToString();
                ProvinceID = parProvinceID.Value.ToString();
                AreaID = parAreaID.Value.ToString();
                Registration = parRegistration.Value.ToString();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }


        public static int GetCustHeadPicMLength(String CustID, out Int32 HeadPicLength, out String ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            HeadPicLength = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustHeadPicInfoQueryLength";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parHeadPicLength = new SqlParameter("@HeadPicLength", SqlDbType.BigInt);
                parHeadPicLength.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parHeadPicLength);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                if (parHeadPicLength != null)
                {
                    if (parHeadPicLength.Value != null)
                    {
                        HeadPicLength = Convert.ToInt32(parHeadPicLength.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }

        public static int GetCustHeadPicInfoM(String CustID, Int32 HeadPicLength, out String HeadPic, out String ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            HeadPic = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustHeadPicInfoQueryV2";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parHeadPic = new SqlParameter("@HeadPic", SqlDbType.VarBinary, HeadPicLength);
                parHeadPic.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parHeadPic);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                //HeadPic = parHeadPic.Value.ToString();
                if (parHeadPic != null)
                {
                    if (parHeadPic.Value != null)
                    {
                        HeadPic = CryptographyUtil.ToBase64String((byte[])parHeadPic.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }

            return Result;
        }


        public static int GetCustHeadPicInfo(String CustID, out String HeadPic, out String ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            HeadPic = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustHeadPicInfoQuery";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

              
                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parHeadPic = new SqlParameter("@HeadPic", SqlDbType.VarChar, 5000);
                parHeadPic.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parHeadPic);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                HeadPic = parHeadPic.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }


            return Result;
        }

        /// <summary>
        /// 客户基本信息查询接口返回记录
        /// 作者：李宏图 时间：2014-01-09
        /// 修改：           时间：
        /// </summary>
        public static int GetCustInfoWithExtendField(string SPID, string CustID, out string ErrMsg, out string OuterID, out string Status, out string CustType,
                                out string CustLevel, out string RealName, out string UserName, out string NickName, out string CertificateCode,
                                out string CertificateType, out string Sex, out string Email, out string EnterpriseID, out string ProvinceID,
                                out string AreaID, out string Registration,  out string Birthday, out string EduLevel, out string IncomeLevel,out string Favorite,out string Address)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OuterID = "";
            Status = "";
            CustType = "";
            CustLevel = "";
            RealName = "";
            UserName = "";
            NickName = "";
            CertificateCode = "";
            CertificateType = "";
            Sex = "";
            Email = "";
            EnterpriseID = "";
            ProvinceID = "";
            AreaID = "";
            Registration = "";


            Birthday = "";
            EduLevel = "";
            IncomeLevel = "";
            Favorite = "";
            Address = "";

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustBasicInfoQueryWithExtendInfo";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar, 20);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);

                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parStatus);

                SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSex);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEmail);

                SqlParameter parEnterpriseID = new SqlParameter("@EnterpriseID", SqlDbType.VarChar, 2);
                parEnterpriseID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEnterpriseID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 6);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parRegistration = new SqlParameter("@Registration", SqlDbType.VarChar, 10);
                parRegistration.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRegistration);

                // 扩展信息

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 10);
                parBirthday.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parBirthday);

                SqlParameter parEduLevel = new SqlParameter("@EduLevel", SqlDbType.VarChar,2);
                parEduLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEduLevel);

                SqlParameter parIncomeLevel = new SqlParameter("@IncomeLevel", SqlDbType.VarChar,1);
                parIncomeLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parIncomeLevel);

                SqlParameter parFavorite = new SqlParameter("@Favorite", SqlDbType.VarChar,256);
                parFavorite.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parFavorite);

                SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.VarChar, 1000);
                parAddress.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAddress);


                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                OuterID = parOuterID.Value.ToString();
                Status = parStatus.Value.ToString();
                CustType = parCustType.Value.ToString();
                CustLevel = parCustLevel.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                CertificateCode = parCertificateCode.Value.ToString();
                CertificateType = parCertificateType.Value.ToString();
                Sex = parSex.Value.ToString();
                Email = parEmail.Value.ToString();
                EnterpriseID = parEnterpriseID.Value.ToString();
                ProvinceID = parProvinceID.Value.ToString();
                AreaID = parAreaID.Value.ToString();
                Registration = parRegistration.Value.ToString();
                
                //扩展信息
  
                Birthday = parBirthday.Value.ToString();
                EduLevel = parEduLevel.Value.ToString();
                IncomeLevel = parIncomeLevel.Value.ToString();
                Favorite = parFavorite.Value.ToString();
                Address = parAddress.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }



        /// <summary>
        /// 客户基本信息查询接口返回记录
        /// 作者：李宏图      时间：2014-06-11
        /// 修改：李宏图      时间：2014-06-11
        /// </summary>
        public static int getCustInfoAndUnifyPlatformCustinfo(string SPID, string CustID, out string ErrMsg, out string OuterID, out string Status, out string CustType,
                             out string CustLevel, out string RealName, out string UserName, out string NickName, out string CertificateCode,
                             out string CertificateType, out string Sex, out string Email, out string EnterpriseID, out string ProvinceID, out string AreaID, out string Registration)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OuterID = "";
            Status = "";
            CustType = "";
            CustLevel = "";
            RealName = "";
            UserName = "";
            NickName = "";
            CertificateCode = "";
            CertificateType = "";
            Sex = "";
            Email = "";
            EnterpriseID = "";
            ProvinceID = "";
            AreaID = "";
            Registration = "";

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustBasicInfoQuery";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar, 20);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);

                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parStatus);

                SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSex);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEmail);

                SqlParameter parEnterpriseID = new SqlParameter("@EnterpriseID", SqlDbType.VarChar, 2);
                parEnterpriseID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEnterpriseID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 6);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parRegistration = new SqlParameter("@Registration", SqlDbType.DateTime, 8);
                parRegistration.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRegistration);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                OuterID = parOuterID.Value.ToString();
                Status = parStatus.Value.ToString();
                CustType = parCustType.Value.ToString();
                CustLevel = parCustLevel.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                CertificateCode = parCertificateCode.Value.ToString();
                CertificateType = parCertificateType.Value.ToString();
                Sex = parSex.Value.ToString();
                Email = parEmail.Value.ToString();
                EnterpriseID = parEnterpriseID.Value.ToString();
                ProvinceID = parProvinceID.Value.ToString();
                AreaID = parAreaID.Value.ToString();
                Registration = parRegistration.Value.ToString();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }


        /// <summary>
        /// 客户基本信息查询接口返回记录
        /// 作者：周涛      时间：2009-7-31
        /// 修改：          时间：
        /// </summary>
        public static int getCustInfo(string SPID, string CustID, out string ErrMsg, out string OuterID, out string Status, out string CustType,
                             out string CustLevel, out string RealName, out string UserName, out string NickName, out string CertificateCode,
                             out string CertificateType, out string Sex, out string Email,out string EnterpriseID, out string ProvinceID, out string AreaID, out string Registration)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            OuterID = "";
            Status = "";
            CustType = "";
            CustLevel = "";
            RealName = "";
            UserName = "";
            NickName = "";
            CertificateCode = "";
            CertificateType = "";
            Sex = "";
            Email = "";
            EnterpriseID = "";
            ProvinceID = "";
            AreaID = "";
            Registration = "";

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustBasicInfoQuery";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar, 20);
                parOuterID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOuterID);

                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parStatus);

                SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSex);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEmail);

                SqlParameter parEnterpriseID = new SqlParameter("@EnterpriseID", SqlDbType.VarChar, 2);
                parEnterpriseID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEnterpriseID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 6);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parRegistration = new SqlParameter("@Registration", SqlDbType.DateTime, 8);
                parRegistration.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRegistration);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                OuterID = parOuterID.Value.ToString();
                Status = parStatus.Value.ToString();
                CustType = parCustType.Value.ToString();
                CustLevel = parCustLevel.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                CertificateCode = parCertificateCode.Value.ToString();
                CertificateType = parCertificateType.Value.ToString();
                Sex = parSex.Value.ToString();
                Email = parEmail.Value.ToString();
                EnterpriseID = parEnterpriseID.Value.ToString();
                ProvinceID = parProvinceID.Value.ToString();
                AreaID = parAreaID.Value.ToString();
                Registration = parRegistration.Value.ToString();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;
        }


        public static int GetPhoneRecordV2(String CustID, out String Phone, out String PhoneClasss, out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            Phone = "";
            PhoneClasss = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustPhoneQueryV2";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                //SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 20);
                //parPhone.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parPhone);

                //SqlParameter parPhoneClass = new SqlParameter("@PhoneClass", SqlDbType.VarChar, 1);
                //parPhoneClass.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parPhoneClass);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString();
                //Phone = parPhone.Value.ToString();
                //PhoneClasss = parPhoneClass.Value.ToString();

            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }


        /// <summary>
        /// 客户基本信息查询接口返回记录--客户电话号码记录
        /// 作者：周涛      时间：2009-7-31
        /// 修改：          时间：
        /// </summary>
        public static PhoneRecord[] getPhoneRecord(string CustID, out int Result, out string ErrMsg)
        {
            PhoneRecord[] PhoneRecords = null;
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustPhoneQuery";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            PhoneRecords = new PhoneRecord[RowCount];
                            PhoneRecord phoneRecord = new PhoneRecord();
                            for (int i = 0; i < RowCount; i++)
                            {
                                phoneRecord = new PhoneRecord();
                                phoneRecord.Phone = ds.Tables[0].Rows[i]["Phone"].ToString().Trim();
                                phoneRecord.PhoneClass = ds.Tables[0].Rows[i]["PhoneClass"].ToString().Trim();
                                PhoneRecords[i] = phoneRecord;
                            }

                        }
                        Result = ErrorDefinition.IError_Result_Success_Code;

                    }
                }

              
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取客户电话号码出错，" + e.Message;
            }

            return PhoneRecords;

        }

        /// <summary>
        /// 客户基本信息查询接口返回记录--客户商旅卡记录
        /// 作者：周涛      时间：2009-7-31
        /// 修改：          时间：
        /// </summary>
        public static TourCardIDRecord[] getTourCardIDRecord(string CustID, out int Result, out string ErrMsg)
        {
            TourCardIDRecord[] TourCardIDRecords = null;
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustTourCardIDQuery";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            TourCardIDRecords = new TourCardIDRecord[RowCount];
                            TourCardIDRecord tourCardIDRecord = new TourCardIDRecord();
                            for (int i = 0; i < RowCount; i++)
                            {
                                tourCardIDRecord = new TourCardIDRecord();
                                tourCardIDRecord.CardID = ds.Tables[0].Rows[i]["CardID"].ToString().Trim();

                                if (ds.Tables[0].Rows[i]["EnterpriseID"] == null || ds.Tables[0].Rows[i]["EnterpriseID"] == System.DBNull.Value || ds.Tables[0].Rows[i]["EnterpriseID"] == "")
                                {
                                    tourCardIDRecord.CardType = "1";
                                }
                                else if (Convert.ToInt32(ds.Tables[0].Rows[i]["EnterpriseID"]) > 1)
                                {
                                    tourCardIDRecord.CardType = "2";
                                }
                                else
                                {
                                    tourCardIDRecord.CardType = "1";
                                }

                                tourCardIDRecord.InnerCardID = ds.Tables[0].Rows[i]["InnerCardID"].ToString().Trim();
                                TourCardIDRecords[i] = tourCardIDRecord;
                            }

                            Result = ErrorDefinition.IError_Result_Success_Code;

                        }
                    }


                }


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取客户电话号码出错，" + e.Message;
            }

            return TourCardIDRecords;
        }

        /// <summary>
        /// web注册时，判断用户是否存在
        /// 作者：周涛      时间：2009-8-18
        /// 修改：          时间：
        /// </summary>
        public static int IsExistUser(string username)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            string ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsExistUser";

                SqlParameter parUserName = new SqlParameter("@username", SqlDbType.VarChar, 30);
                parUserName.Value = username;
                cmd.Parameters.Add(parUserName);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString().Trim());
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "用户验证出错，" + ex.Message;
            }

            return Result;
        }



        /// <summary>
        /// 作者：赵锐
        /// 日期：2009年8月10日
        /// </summary>
        /// <param name="CustID">用户编号</param>
        /// <param name="ProvinceID">省份编号</param>
        /// <param name="AreaID">城市编号</param>
        /// <param name="CertificateType">证件类型</param>
        /// <param name="CertificateCode">证件号码</param>
        /// <param name="RealName">客户姓名</param>
        /// <param name="Sex">性别</param>
        /// <param name="NickName">昵称</param>
        /// <param name="DealTime">修改时间</param>
        /// <param name="BirthDay">生日</param>
        /// <param name="EduLevel">文化程度</param>
        /// <param name="IncomeLevel">收入水平</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int UpdateCustInfoById(string CustID, string ProvinceID, string AreaID, string CertificateType, string CertificateCode, string RealName, string Sex, string NickName, DateTime DealTime, string BirthDay, string EduLevel, string IncomeLevel, out string ErrMsg)
        {
          
            ErrMsg = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdateCustInfo";

                SqlParameter parCustID = new SqlParameter("CustID",SqlDbType.VarChar,16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parProvinceID = new SqlParameter("ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parAreaID = new SqlParameter("AreaID", SqlDbType.VarChar, 3);
                parAreaID.Value = AreaID;
                cmd.Parameters.Add(parAreaID);               

                SqlParameter parCertificateType = new SqlParameter("CertificateType", SqlDbType.Char, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parCertificateCode = new SqlParameter("CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parRealName = new SqlParameter("RealName", SqlDbType.VarChar, 50);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);


               

                SqlParameter parSex = new SqlParameter("Sex",SqlDbType.Char,1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

               

                SqlParameter parNickName = new SqlParameter("NickName",SqlDbType.VarChar,30);
                parNickName.Value = NickName;
                cmd.Parameters.Add(parNickName);


                SqlParameter parDealTime = new SqlParameter("DealTime", SqlDbType.DateTime);
                parDealTime.Value = DealTime;
                cmd.Parameters.Add(parDealTime);


                if (!CommonUtility.IsEmpty(BirthDay))
                {
                    SqlParameter parBirthday = new SqlParameter("Birthday", SqlDbType.DateTime, 8);
                    parBirthday.Value = DateTime.Parse(BirthDay);
                    cmd.Parameters.Add(parBirthday);
                }
                else
                {
                    SqlParameter parBirthday = new SqlParameter("Birthday", SqlDbType.DateTime, 8);
                    parBirthday.Value = DBNull.Value;
                    cmd.Parameters.Add(parBirthday);
                }

                SqlParameter parEduLevel = new SqlParameter("EduLevel",SqlDbType.VarChar,2);
                parEduLevel.Value = EduLevel;
                cmd.Parameters.Add(parEduLevel);


                SqlParameter parIncomeLevel = new SqlParameter("IncomeLevel", SqlDbType.VarChar, 1);
                parIncomeLevel.Value = IncomeLevel;
                cmd.Parameters.Add(parIncomeLevel);

                SqlParameter parSqlResult = new SqlParameter("SqlResult",SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg",SqlDbType.VarChar,256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch(Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        }

        public static string[] MobileServiceUserAuthv2(string Account, string Password)
        {

            string[] mobileServiceResults = new string[4];


            string ErrMsg = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_MobileServiceUserAuthv2";

                SqlParameter parAccount = new SqlParameter("@Account", SqlDbType.VarChar, 64);
                parAccount.Value = Account;
                cmd.Parameters.Add(parAccount);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 64);
                parPassword.Value = Password;
                cmd.Parameters.Add(parPassword);


                SqlParameter parOrgID = new SqlParameter("@ORG_ID", SqlDbType.VarChar, 64);
                parOrgID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOrgID);


                SqlParameter parFlag = new SqlParameter("@Flag", SqlDbType.VarChar, 1);
                parFlag.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parFlag);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);



                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

                mobileServiceResults[0] = parResult.Value.ToString();
                mobileServiceResults[1] = ErrMsg;
                mobileServiceResults[2] = parOrgID.Value.ToString();
                mobileServiceResults[3] = parFlag.Value.ToString();


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return mobileServiceResults;


        }
        
        /// <summary>
        /// 设置客户基本信息
        /// </summary>
        public static int UpdateCustinfo(string SPID, string CustID, string RealName, 
                            string CertificateCode, string CertificateType, string Sex, string Email, out string ErrMsg)
        {
            ErrMsg = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdateCustinfo_1";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                if(String.IsNullOrEmpty(CertificateCode)){
                    parCertificateCode.Value = "";
                }else
                {
                    parCertificateCode.Value = CertificateCode;
                }
                
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = "0";
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                if(String.IsNullOrEmpty(Sex))
                {
                    parSex.Value = "2";
                }else
                {
                     parSex.Value = Sex;
                }
               
                cmd.Parameters.Add(parSex);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                if(String.IsNullOrEmpty(Email))
                {
                    parEmail.Value = "";
                }else
                {
                    parEmail.Value = Email;
                }
                cmd.Parameters.Add(parEmail);

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



        public static int UpdateCustinfoExtV2(string SPID, string CustID, string RealName, string CertificateCode, string CertificateType,
            string Birthday,string Sex, string Email, string NickName, out string ErrMsg)
        {

            ErrMsg = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdateCustinfo_3";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 10);
                parBirthday.Value = Birthday;
                cmd.Parameters.Add(parBirthday);


                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 100);
                parNickName.Value = NickName;
                cmd.Parameters.Add(parNickName);


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




        public static int UpdateCustinfoExtV5(string CustID,string HeadPic, out string ErrMsg)
        {

            ErrMsg = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                byte[] headImage = CryptographyUtil.FromBase64String("QUJDREVGRw==");

                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdateCustinfo_withExtendInfo_savepic";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar,16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parHeadPic = new SqlParameter("@HeadPic", SqlDbType.VarBinary, headImage.Length);
                parHeadPic.Value = headImage;
                cmd.Parameters.Add(parHeadPic);

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


        public static int UpdateCustinfoExtV4(string SPID, string ProvinceID, string AreaID, string CustID, string RealName, string CertificateCode, string CertificateType,
          string Birthday, string Sex, string Email, string NickName, string EduLevel, string IncomeLevel, string Favorite, string Address, string HeadPic, out string ErrMsg)
        {
            //CryptographyUtil
            ErrMsg = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {

                byte[] headImage = new byte[0];
                if (!String.IsNullOrEmpty(HeadPic))
                {
                   headImage =  CryptographyUtil.FromBase64String(HeadPic);
                }
                    

                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdateCustinfo_withExtendInfoV2";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                if (String.IsNullOrEmpty(ProvinceID))
                {
                    ProvinceID = "02";
                }

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                if (String.IsNullOrEmpty(AreaID))
                {
                    AreaID = "021";
                }

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 3);
                parAreaID.Value = AreaID;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                if (String.IsNullOrEmpty(CertificateCode))
                {
                    CertificateCode = "431124198810053635";
                }
                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                if (String.IsNullOrEmpty(CertificateType))
                {
                    CertificateType = "0";
                }

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                if (String.IsNullOrEmpty(Sex))
                {
                    Sex = "2";
                }
                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

                if (String.IsNullOrEmpty(Birthday))
                {
                    Birthday = "1988-10-05";
                }

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 10);
                parBirthday.Value = Birthday;
                cmd.Parameters.Add(parBirthday);


                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 100);
                parNickName.Value = NickName;
                cmd.Parameters.Add(parNickName);

                SqlParameter parEduLevel = new SqlParameter("@EduLevel", SqlDbType.VarChar, 1);
                parEduLevel.Value = EduLevel;
                cmd.Parameters.Add(parEduLevel);

                SqlParameter parIncomeLevel = new SqlParameter("@IncomeLevel", SqlDbType.VarChar, 1);
                parIncomeLevel.Value = IncomeLevel;
                cmd.Parameters.Add(parIncomeLevel);

                SqlParameter parFavorite = new SqlParameter("@Favorite", SqlDbType.VarChar, 100);
                parFavorite.Value = Favorite;
                cmd.Parameters.Add(parFavorite);

                SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.VarChar, 200);
                parAddress.Value = Address;
                cmd.Parameters.Add(parAddress);

                SqlParameter parHeadPic = new SqlParameter("@HeadPic", SqlDbType.VarBinary, headImage.Length);
                parHeadPic.Value = headImage;
                cmd.Parameters.Add(parHeadPic);

                SqlParameter parHeadPicLength = new SqlParameter("@HeadPicLength", SqlDbType.BigInt);
                parHeadPicLength.Value = headImage.Length;
                cmd.Parameters.Add(parHeadPicLength);


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



        public static int UpdateCustinfoExtV3(string SPID, string ProvinceID,string AreaID,string CustID, string RealName, string CertificateCode, string CertificateType,
              string Birthday, string Sex, string Email, string NickName, string EduLevel, string IncomeLevel, string Favorite, string Address, string HeadPic, out string ErrMsg)
        {
            //CryptographyUtil
            ErrMsg = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdateCustinfo_withExtendInfo";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                if (String.IsNullOrEmpty(ProvinceID))
                {
                    ProvinceID = "02";
                }

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                if (String.IsNullOrEmpty(AreaID))
                {
                    AreaID = "021";
                }

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 3);
                parAreaID.Value = AreaID;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                if (String.IsNullOrEmpty(CertificateCode))
                {
                    CertificateCode = "431124198810053635";
                }
                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                if (String.IsNullOrEmpty(CertificateType))
                {
                    CertificateType = "0";
                }

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                if (String.IsNullOrEmpty(Sex))
                {
                    Sex = "2";
                }
                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

                if (String.IsNullOrEmpty(Birthday))
                {
                    Birthday = "1988-10-05";
                }

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 10);
                parBirthday.Value = Birthday;
                cmd.Parameters.Add(parBirthday);


                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 100);
                parNickName.Value = NickName;
                cmd.Parameters.Add(parNickName);

                SqlParameter parEduLevel = new SqlParameter("@EduLevel", SqlDbType.VarChar, 1);
                parEduLevel.Value = EduLevel;
                cmd.Parameters.Add(parEduLevel);

                SqlParameter parIncomeLevel = new SqlParameter("@IncomeLevel", SqlDbType.VarChar, 1);
                parIncomeLevel.Value = IncomeLevel;
                cmd.Parameters.Add(parIncomeLevel);

                SqlParameter parFavorite = new SqlParameter("@Favorite", SqlDbType.VarChar, 100);
                parFavorite.Value = Favorite;
                cmd.Parameters.Add(parFavorite);

                SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.VarChar, 200);
                parAddress.Value = Address;
                cmd.Parameters.Add(parAddress);

                SqlParameter parHeadPic = new SqlParameter("@HeadPic", SqlDbType.VarChar, 5000);
                parHeadPic.Value = HeadPic;
                cmd.Parameters.Add(parHeadPic);
                
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
        /// 设置客户基本信息-扩展信息
        /// </summary>
        public static int UpdateCustinfoExt(string SPID, string CustID, string RealName, string CertificateCode, string CertificateType, 
                            string Sex, string Email, string NickName,out string ErrMsg)
        {
            ErrMsg = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdateCustinfo_2";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CertificateCode;
                cmd.Parameters.Add(parCertificateCode);

                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = Sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 100);
                parNickName.Value = NickName;
                cmd.Parameters.Add(parNickName);


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
        /// 查询客户来源
        /// </summary>
        public static string QueryCustFrom(string SPID, string CustID, out int Result, out string ErrMsg)
        {
            string custfrom = "";
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
                cmd.CommandText = "up_Customer_V3_Interface_QueryCustFrom";

                SqlParameter parCustID = new SqlParameter("@CustId", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null)
                    if (ds.Tables.Count != 0)
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;

                            BasicInfoV2Record rs = new BasicInfoV2Record();
                            for (int i = 0; i < RowCount; i++)
                            {
                                custfrom = ds.Tables[0].Rows[i]["fromContent"].ToString().Trim();
                            }
                        }
                Result = int.Parse(parResult.Value.ToString());
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "查询客户来源出错，" + e.Message;
            }

            return custfrom;
        }

        /// <summary>
        /// 获取用户名返回识别出的用户信息
        /// </summary>
        public static BasicInfoV2Record[] GetQueryByUserName(string SPID, string UserName, out int Result, out string ErrMsg)
        {
            List<BasicInfoV2Record> BasicInfoList = new List<BasicInfoV2Record>();
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
                cmd.CommandText = "up_Customer_V3_Interface_QueryByUserName";

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 20);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.VarChar, 100);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null)
                    if (ds.Tables.Count != 0)
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;

                            BasicInfoV2Record rs = new BasicInfoV2Record();
                            for (int i = 0; i < RowCount; i++)
                            {
                                string strSourceSPID = ds.Tables[0].Rows[i]["SourceSPID"].ToString().Trim();

                                rs = new BasicInfoV2Record();
                                rs.CustID = ds.Tables[0].Rows[i]["CustID"].ToString().Trim();
                                rs.CustType = ds.Tables[0].Rows[i]["CustType"].ToString().Trim();
                                rs.PhoneClass = ds.Tables[0].Rows[i]["PhoneClass"].ToString().Trim();
                                rs.RealName = ds.Tables[0].Rows[i]["RealName"].ToString().Trim();
                                rs.Sex = ds.Tables[0].Rows[i]["Sex"].ToString().Trim();
                                BasicInfoList.Add(rs);

                            }
                        }
                Result = int.Parse(parResult.Value.ToString());
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取用户名识别信息出错，" + e.Message;
            }

            return BasicInfoList.ToArray();
        }

        /// <summary>
        /// 根据CustID查询用户帐号
        /// </summary>
        public static int GetUserAccount(string CustID, out string UserAccount, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
            UserAccount = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_GetUserAccount";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                ///////////////////////////////////////

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 9);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;
        }
    }
}

/*********************************************************************************************************
 *     描述: 客户信息平台—常旅客信息查询接口
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 周涛
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-08-05
 *********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class FrequentUserBO
    {
        /// <summary>
        /// 常旅客信息查询接口返回记录
        /// 作者：周涛      时间：2009-08-05
        /// 修改：          时间：
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="Result"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static FrequentUserInfo[] GetFrequentUser(string SPID, string CustID, out int Result, out string ErrMsg)
        {
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            FrequentUserInfo[] FrequentUserInfos = null;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_FrequentUserQuery";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            FrequentUserInfos = new FrequentUserInfo[RowCount];
                            FrequentUserInfo frequentUserInfo = new FrequentUserInfo();
                            for (int i = 0; i < RowCount; i++)
                            {
                                frequentUserInfo = new FrequentUserInfo();
                                frequentUserInfo.FrequentUserID = int.Parse(ds.Tables[0].Rows[i]["SequenceID"].ToString().Trim());
                                frequentUserInfo.CertificateCode = ds.Tables[0].Rows[i]["CertificateCode"].ToString().Trim();
                                frequentUserInfo.CertificateType = ds.Tables[0].Rows[i]["CertificateType"].ToString().Trim();
                                frequentUserInfo.RealName = ds.Tables[0].Rows[i]["FrequentUserName"].ToString().Trim();
                                frequentUserInfo.Phone = ds.Tables[0].Rows[i]["Phone"].ToString().Trim();
                                frequentUserInfo.DealType = "3";
                                frequentUserInfo.ExtendField = "";
                                FrequentUserInfos[i] = frequentUserInfo;
                            }
                        }
                    }
                }

                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
                
            }
            catch(Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取常旅客信息查询出错，" + e.Message;
            }

            return FrequentUserInfos;

        }

        /// <summary>
        /// 常旅客信息上传接口返回记录
        /// 作者：周涛      时间：2009-08-05
        /// 修改：          时间：
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="FrequentUserInfos"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int UploadFrequentUser(string SPID, string CustID, FrequentUserInfo[] FrequentUserInfos, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            SqlTransaction myTrans = null;
            //修改 刘春利20091110

            try
            {
                myCon.Open();
                myTrans = myCon.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_FrequentUserUploadQuery";
                cmd.Connection = myTrans.Connection;
                cmd.Transaction = myTrans;

                for (int i = 0; i < FrequentUserInfos.Length; i++)
                {

                    SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                    parSPID.Value = SPID;
                    cmd.Parameters.Add(parSPID);

                    SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                    parCustID.Value = CustID;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parFrequentUserID = new SqlParameter("@FrequentUserID", SqlDbType.BigInt);
                    parFrequentUserID.Value = FrequentUserInfos[i].FrequentUserID;
                    cmd.Parameters.Add(parFrequentUserID);

                    SqlParameter parDealType = new SqlParameter("@DealType", SqlDbType.VarChar, 1);
                    parDealType.Value = FrequentUserInfos[i].DealType == null ? "" : FrequentUserInfos[i].DealType;
                    cmd.Parameters.Add(parDealType);

                    SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 16);
                    parRealName.Value = FrequentUserInfos[i].RealName == null ? "" : FrequentUserInfos[i].RealName;
                    cmd.Parameters.Add(parRealName);

                    SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                    parCertificateCode.Value = FrequentUserInfos[i].CertificateCode == null ? "" : FrequentUserInfos[i].CertificateCode;
                    cmd.Parameters.Add(parCertificateCode);

                    SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                    parCertificateType.Value = FrequentUserInfos[i].CertificateType == null ? "" : FrequentUserInfos[i].CertificateType;
                    cmd.Parameters.Add(parCertificateType);

                    SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 20);
                    parPhone.Value = FrequentUserInfos[i].Phone == null ? "" : FrequentUserInfos[i].Phone;
                    cmd.Parameters.Add(parPhone);

                    SqlParameter parExtendField = new SqlParameter("@ExtendField", SqlDbType.VarChar, 256);
                    parExtendField.Value = FrequentUserInfos[i].ExtendField == null ? "" : FrequentUserInfos[i].ExtendField;
                    cmd.Parameters.Add(parExtendField);

                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    cmd.ExecuteNonQuery();

                    Result = int.Parse(parResult.Value.ToString().Trim());
                    ErrMsg = parErrMsg.Value.ToString();

                    if (Result != 0)
                    {
                        myTrans.Rollback();
                        return Result;
                    }

                    cmd.Parameters.Clear();
                }
                myTrans.Commit();

            }
            catch (Exception e)
            {
                if (myTrans != null)
                {
                    myTrans.Rollback();
                }
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
        
    }
}

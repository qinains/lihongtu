/*********************************************************************************************************
 *     描述: 客户信息平台—客户地址信息查询接口
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 周涛
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-08-03
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
    /// <summary>
    /// 客户地址信息管理
    /// </summary>
    public class AddressInfoBO
    {


        public static DictCity[] GetProvinces(out int Result, out string ErrMsg)
        {
            DictCity[] dictCitys = null;
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_ProvinceInfoQuery";

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
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            dictCitys = new DictCity[RowCount];
           
                            for (int i = 0; i < RowCount; i++)
                            {
                                DictCity dc = new DictCity();
                                dc.ID = ds.Tables[0].Rows[i]["id"].ToString().Trim();
                                dc.Name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                                dc.Code = ds.Tables[0].Rows[i]["code"].ToString().Trim();
                                dc.Grade = int.Parse(ds.Tables[0].Rows[i]["grade"].ToString().Trim());
                                dictCitys[i] = dc;
                            }
                        }
                    }
                }
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取省信息异常，" + e.Message;
            }
            return dictCitys;
        }


        public static DictCity[] GetCities(string ProvinceID,out int Result, out string ErrMsg)
        {
            DictCity[] dictCitys = null;
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CityInfoQuery";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 32);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

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
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            dictCitys = new DictCity[RowCount];
                            for (int i = 0; i < RowCount; i++)
                            {
                                DictCity dc = new DictCity();
                                dc.ID = ds.Tables[0].Rows[i]["id"].ToString().Trim();
                                dc.Name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                                dc.Code = ds.Tables[0].Rows[i]["code"].ToString().Trim();
                                dc.Grade = int.Parse(ds.Tables[0].Rows[i]["grade"].ToString().Trim());
                                dictCitys[i] = dc;
                            }
                        }
                    }
                }
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取市信息异常，" + e.Message;
            }
            return dictCitys;
        }

        public static DictCity[] GetDistircts(string CityID, out int Result, out string ErrMsg)
        {
            DictCity[] dictCitys = null;
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_DistirctInfoQuery";

                SqlParameter parCityID = new SqlParameter("@CityID", SqlDbType.VarChar, 32);
                parCityID.Value = CityID;
                cmd.Parameters.Add(parCityID);

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
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            dictCitys = new DictCity[RowCount];

                            for (int i = 0; i < RowCount; i++)
                            {
                                DictCity dc = new DictCity();
                                dc.ID = ds.Tables[0].Rows[i]["id"].ToString().Trim();
                                dc.Name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                                dc.Code = ds.Tables[0].Rows[i]["code"].ToString().Trim();
                                dc.Grade = int.Parse(ds.Tables[0].Rows[i]["grade"].ToString().Trim());
                                dictCitys[i] = dc;
                            }
                        }
                    }
                }
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取区信息异常，" + e.Message;
            }
            return dictCitys;
        }


        public static String GetDictCityByCode(string AreaCode, int Grade, out string Code,out int Result, out string ErrMsg)
        {

            String DcName = String.Empty;
            Code = String.Empty;
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_GetDictCityByCode";

                SqlParameter parAreaCode = new SqlParameter("@Grade", SqlDbType.Int);
                parAreaCode.Value = Grade;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parGrade = new SqlParameter("@AreaCode", SqlDbType.VarChar, 30);
                parGrade.Value = AreaCode;
                cmd.Parameters.Add(parGrade);


                SqlParameter parName = new SqlParameter("@Name", SqlDbType.VarChar, 60);
                parName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parName);

                SqlParameter parCode = new SqlParameter("@Code", SqlDbType.VarChar, 30);
                parCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCode);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                DcName = parName.Value.ToString().Trim();
                Code = parCode.Value.ToString().Trim();
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取地市信息异常，" + e.Message;
            }
            return DcName;
        }

        /// <summary>
        /// 客户地址信息查询接口返回记录
        /// 作者：周涛      时间：2009-08-03
        /// 修改：          时间：
        /// </summary>
        public static AddressInfoRecord[] GetAddressInfo(string SPID, string CustID, out int Result, out string ErrMsg)
        {
            AddressInfoRecord[] AddressInfoRecords = null;
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);

            try
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = null;
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_AddressInfoQuery";

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
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            AddressInfoRecords = new AddressInfoRecord[RowCount];
                            AddressInfoRecord addressInfoRecord = new AddressInfoRecord();
                            for (int i = 0; i < RowCount; i++)
                            {
                                addressInfoRecord = new AddressInfoRecord();
                                addressInfoRecord.AddressID = long.Parse(ds.Tables[0].Rows[i]["SequenceID"].ToString().Trim());
                                addressInfoRecord.AreaCode = ds.Tables[0].Rows[i]["AreaCode"].ToString().Trim();
                                addressInfoRecord.Address = ds.Tables[0].Rows[i]["Address"].ToString().Trim();
                                addressInfoRecord.Zipcode = ds.Tables[0].Rows[i]["ZipCode"].ToString().Trim();
                                addressInfoRecord.Type = ds.Tables[0].Rows[i]["Type"].ToString().Trim();
                                addressInfoRecord.OtherType = ds.Tables[0].Rows[i]["OtherType"].ToString().Trim();
                                addressInfoRecord.RelationPerson = ds.Tables[0].Rows[i]["RelationPerson"].ToString().Trim();
                                addressInfoRecord.Mobile = ds.Tables[0].Rows[i]["Mobile"].ToString().Trim();
                                addressInfoRecord.FixedPhone = ds.Tables[0].Rows[i]["FixedPhone"].ToString().Trim();
                                addressInfoRecord.DealType = "3";
                                addressInfoRecord.ExtendField = "";
                                AddressInfoRecords[i] = addressInfoRecord;
                            }
                        }
                    }
                }
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取客户地址信息出错，" + e.Message;
            }
            return AddressInfoRecords;
        }

        public static AddressInfo AddressInfoLoad(string SPID, long AddressID, out int Result, out string ErrMsg)
        {
            AddressInfo Address = null;
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataSet ds = null;
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddressInfoLoad";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID); 

                SqlParameter parAddressID = new SqlParameter("@AddressID", SqlDbType.BigInt, 8);
                parAddressID.Value = AddressID;
                cmd.Parameters.Add(parAddressID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
                if ( Result == 0 && ds != null )
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {

                          Address = new AddressInfo();
                          Address.AddressID = long.Parse(ds.Tables[0].Rows[0]["SequenceID"].ToString().Trim());
                          Address.AreaCode = ds.Tables[0].Rows[0]["AreaCode"].ToString().Trim();
                          Address.Address = ds.Tables[0].Rows[0]["Address"].ToString().Trim();
                          Address.Zipcode = ds.Tables[0].Rows[0]["ZipCode"].ToString().Trim();
                          Address.Type = ds.Tables[0].Rows[0]["Type"].ToString().Trim();
                          Address.OtherType = ds.Tables[0].Rows[0]["OtherType"].ToString().Trim();
                          Address.RelationPerson = ds.Tables[0].Rows[0]["RelationPerson"].ToString().Trim();
                          Address.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString().Trim();
                          Address.FixedPhone = ds.Tables[0].Rows[0]["FixedPhone"].ToString().Trim();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取客户常用地址信息出错，" + e.Message;
            }
            return Address;
        }

        public static int AddressInfoAdd(string SPID, string CustID, AddressInfo Address, out long NewAddressID, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            SqlTransaction myTrans = null;
            try
            {
                myCon.Open();
                myTrans = myCon.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myTrans.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddressInfoAdd";
                cmd.Transaction = myTrans;

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);
                
                SqlParameter parNewAddressID = new SqlParameter("@NewAddressID", SqlDbType.BigInt, 8);
                parNewAddressID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNewAddressID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                parAreaCode.Value = Address.AreaCode == null ? "" : Address.AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.VarChar, 256);
                parAddress.Value = Address.Address == null ? "" : Address.Address;
                cmd.Parameters.Add(parAddress);

                SqlParameter parZipcode = new SqlParameter("@Zipcode", SqlDbType.VarChar, 6);
                parZipcode.Value = Address.Zipcode == null ? "" : Address.Zipcode;
                cmd.Parameters.Add(parZipcode);

                SqlParameter parType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                parType.Value = Address.Type == null ? "" : Address.Type;
                cmd.Parameters.Add(parType);

                SqlParameter parOtherType = new SqlParameter("@OtherType", SqlDbType.VarChar, 50);
                parOtherType.Value = Address.OtherType == null ? "" : Address.OtherType;
                cmd.Parameters.Add(parOtherType);

                SqlParameter parRelationPerson = new SqlParameter("@RelationPerson", SqlDbType.VarChar, 20);
                parRelationPerson.Value = Address.RelationPerson == null ? "" : Address.RelationPerson;
                cmd.Parameters.Add(parRelationPerson);

                SqlParameter parMobile = new SqlParameter("@Mobile", SqlDbType.VarChar, 20);
                parMobile.Value = Address.Mobile == null ? "" : Address.Mobile;
                cmd.Parameters.Add(parMobile);

                SqlParameter parFixedPhone = new SqlParameter("@FixedPhone", SqlDbType.VarChar, 20);
                parFixedPhone.Value = Address.FixedPhone == null ? "" : Address.FixedPhone;
                cmd.Parameters.Add(parFixedPhone);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                cmd.ExecuteNonQuery();

                NewAddressID = int.Parse(parNewAddressID.Value.ToString().Trim());
                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();

                if (Result != 0)
                {
                    myTrans.Rollback();
                    return Result;
                }
                myTrans.Commit();
            }
            catch (Exception e)
            {
                if (myTrans != null)
                {
                    myTrans.Rollback();
                }
                NewAddressID = -1;
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

        public static int AddressInfoSave(string SPID, string CustID, AddressInfo Address, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            SqlTransaction myTrans = null;
            try
            {
                myCon.Open();
                myTrans = myCon.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myTrans.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddressInfoSave";
                cmd.Transaction = myTrans;

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAddressID = new SqlParameter("@AddressID", SqlDbType.BigInt, 8);
                parAddressID.Value = Address.AddressID;
                cmd.Parameters.Add(parAddressID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                parAreaCode.Value = Address.AreaCode == null ? "" : Address.AreaCode;
                cmd.Parameters.Add(parAreaCode);

                SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.VarChar, 256);
                parAddress.Value = Address.Address == null ? "" : Address.Address;
                cmd.Parameters.Add(parAddress);

                SqlParameter parZipcode = new SqlParameter("@Zipcode", SqlDbType.VarChar, 6);
                parZipcode.Value = Address.Zipcode == null ? "" : Address.Zipcode;
                cmd.Parameters.Add(parZipcode);

                SqlParameter parType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                parType.Value = Address.Type == null ? "" : Address.Type;
                cmd.Parameters.Add(parType);

                SqlParameter parOtherType = new SqlParameter("@OtherType", SqlDbType.VarChar, 50);
                parOtherType.Value = Address.OtherType == null ? "" : Address.OtherType;
                cmd.Parameters.Add(parOtherType);

                SqlParameter parRelationPerson = new SqlParameter("@RelationPerson", SqlDbType.VarChar, 20);
                parRelationPerson.Value = Address.RelationPerson == null ? "" : Address.RelationPerson;
                cmd.Parameters.Add(parRelationPerson);

                SqlParameter parMobile = new SqlParameter("@Mobile", SqlDbType.VarChar, 20);
                parMobile.Value = Address.Mobile == null ? "" : Address.Mobile;
                cmd.Parameters.Add(parMobile);

                SqlParameter parFixedPhone = new SqlParameter("@FixedPhone", SqlDbType.VarChar, 20);
                parFixedPhone.Value = Address.FixedPhone == null ? "" : Address.FixedPhone;
                cmd.Parameters.Add(parFixedPhone);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                cmd.ExecuteNonQuery();

                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();

                if (Result != 0)
                {
                    myTrans.Rollback();
                    return Result;
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
            finally
            {
                if (myCon.State != ConnectionState.Closed)
                {
                    myCon.Close();
                }
            }
            return Result;
        }

        public static int AddressInfoDelete(string SPID, long AddressID, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            SqlTransaction myTrans = null;
            try
            {
                myCon.Open();
                myTrans = myCon.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myTrans.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddressInfoDelete";
                cmd.Transaction = myTrans;

                SqlParameter parAddressID = new SqlParameter("@AddressID", SqlDbType.BigInt, 8);
                parAddressID.Value = AddressID;
                cmd.Parameters.Add(parAddressID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                cmd.ExecuteNonQuery();

                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();

                if (Result != 0)
                {
                    myTrans.Rollback();
                    return Result;
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
        /// 客户地址信息上传接口返回记录
        /// 作者：周涛      时间：2009-08-04
        /// 修改：          时间：
        /// </summary>
        public static int UploadAddressInfo(string SPID, AddressInfoRecord[] AddressInfoRecords, string CustID, out string ErrMsg)
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
                cmd.Connection = myTrans.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_AddressInfoUploadQuery";
                cmd.Transaction = myTrans;


                for (int i = 0; i < AddressInfoRecords.Length; i++)
                {

                    SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                    parSPID.Value = SPID;
                    cmd.Parameters.Add(parSPID);

                    SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                    parCustID.Value = CustID;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    SqlParameter parAddressID = new SqlParameter("@AddressID", SqlDbType.BigInt, 8);
                    parAddressID.Value = AddressInfoRecords[i].AddressID;
                    cmd.Parameters.Add(parAddressID);

                    SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                    parAreaCode.Value = AddressInfoRecords[i].AreaCode == null ? "" : AddressInfoRecords[i].AreaCode;
                    cmd.Parameters.Add(parAreaCode);

                    SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.VarChar, 256);
                    parAddress.Value = AddressInfoRecords[i].Address == null ? "" : AddressInfoRecords[i].Address;
                    cmd.Parameters.Add(parAddress);

                    SqlParameter parZipcode = new SqlParameter("@Zipcode", SqlDbType.VarChar, 6);
                    parZipcode.Value = AddressInfoRecords[i].Zipcode == null ? "" : AddressInfoRecords[i].Zipcode;
                    cmd.Parameters.Add(parZipcode);

                    SqlParameter parType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                    parType.Value = AddressInfoRecords[i].Type == null ? "" : AddressInfoRecords[i].Type;
                    cmd.Parameters.Add(parType);

                    SqlParameter parOtherType = new SqlParameter("@OtherType", SqlDbType.VarChar, 50);
                    parOtherType.Value = AddressInfoRecords[i].OtherType == null ? "" : AddressInfoRecords[i].OtherType;
                    cmd.Parameters.Add(parOtherType);

                    SqlParameter parRelationPerson = new SqlParameter("@RelationPerson", SqlDbType.VarChar, 20);
                    parRelationPerson.Value = AddressInfoRecords[i].RelationPerson == null ? "" : AddressInfoRecords[i].RelationPerson;
                    cmd.Parameters.Add(parRelationPerson);

                    SqlParameter parMobile = new SqlParameter("@Mobile", SqlDbType.VarChar, 20);
                    parMobile.Value = AddressInfoRecords[i].Mobile == null ? "" : AddressInfoRecords[i].Mobile;
                    cmd.Parameters.Add(parMobile);

                    SqlParameter parFixedPhone = new SqlParameter("@FixedPhone", SqlDbType.VarChar, 20);
                    parFixedPhone.Value = AddressInfoRecords[i].FixedPhone == null ? "" : AddressInfoRecords[i].FixedPhone;
                    cmd.Parameters.Add(parFixedPhone);

                    SqlParameter parDealType = new SqlParameter("@DealType", SqlDbType.VarChar, 1);
                    parDealType.Value = AddressInfoRecords[i].DealType == null ? "" : AddressInfoRecords[i].DealType;
                    cmd.Parameters.Add(parDealType);

//                    SqlParameter parExtendField = new SqlParameter("@ExtendField", SqlDbType.VarChar, 256);
//                    parExtendField.Value = AddressInfoRecords[i].ExtendField == null ? "" : AddressInfoRecords[i].ExtendField;
//                    cmd.Parameters.Add(parExtendField);

                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    cmd.ExecuteNonQuery();

                    Result = int.Parse(parResult.Value.ToString().Trim());
                    ErrMsg = parErrMsg.Value.ToString().Trim();

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
            finally //修改 刘春利 20091110
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

/*********************************************************************************************************
 *   描述: 客户信息平台二期问题答案管理
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2002
 *     作者: 张英杰
 * 联系方式: 
 *    公司: 中国电信集团号百信息服务有限公司
 * 创建日期: 2009-08-03
 *********************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    public class PhoneBO
    {
        /// <summary>
        /// 积分SPID
        /// </summary>
        private static string strScoreBesttonePSID = System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"];
        //private static String strScoreBesttonePSID = ConstHelper.ScoreBesttoneSPID;

        /// <summary>
        /// 号百SPID
        /// </summary>
        private static string strBesttoneSPID = System.Configuration.ConfigurationManager.AppSettings["BesttoneSPID"];
        //private static String strBesttoneSPID = ConstHelper.BesttoneSPID;

        #region 手机号相关查询



        /// <summary>
        /// 查询客户的手机
        /// 作者：李宏图
        /// 日期：2014年11月27日
        /// </summary>
        public static string SelPhoneNumV2(string CustID, string SPID, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            string Phone = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelPhoneNumV2";

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPhone);

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSPID = new SqlParameter("SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                Phone = parPhone.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return Phone;
        }




        /// <summary>
        /// 查询客户的认证手机
        /// 作者：赵锐
        /// 日期：2009年9月27日
        /// </summary>
        public static string SelPhoneNum(string CustID, string SPID, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            string Phone = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelPhoneNum";

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPhone);

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSPID = new SqlParameter("SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                Phone = parPhone.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return Phone;
        }

        /// <summary>
        /// 查看用户是否有认证手机
        /// </summary>
        public static Boolean IsHasAuthenPhone(String CustID)
        {
            bool result = false;
            String ErrMsg = String.Empty;
            try
            {
                String phone = SelPhoneNum(CustID, "", out ErrMsg);
                if (!String.IsNullOrEmpty(phone))
                    result = true;
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 获取用户所有手机号码
        /// </summary>
        public static DataSet GetAllPhone(string CustId, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            DataSet ds = new DataSet();
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {

                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelAllPhone";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustId;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return ds;
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
        /// 获取主叫号码返回识别出的用户信息
        /// </summary>
        public static BasicInfoV2Record[] GetQueryByPhone(string SPID, string PhoneNum, out int Result, out string ErrMsg)
        {
            List<BasicInfoV2Record> BasicInfoList = new List<BasicInfoV2Record>();
            ErrMsg = "";
            Result = ErrorDefinition.IError_Result_UnknowError_Code;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;
            StringBuilder msg = new StringBuilder();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_QueryByPhone";

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

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

                                //积分商城用户可以取得所有客户信息，其他系统用户只能看到号百客户信息
                                if (strScoreBesttonePSID == SPID)
                                {
                                    BasicInfoList.Add(rs);
                                }
                                else
                                {
                                    if ("41".Equals(rs.CustType) || "42".Equals(rs.CustType))
                                    {
                                        BasicInfoList.Add(rs);
                                    }
                                }
                            }
                        }
                Result = int.Parse(parResult.Value.ToString());
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取电话识别信息出错，" + e.Message;
                msg.AppendFormat(e.ToString());
            }
            finally {
                BTUCenterInterfaceLog.CenterForBizTourLog("QueryByPhone", msg);
            }

            return BasicInfoList.ToArray();
        }

        /// <summary>
        /// 获取主叫号码返回识别出的用户信息
        /// </summary>
        public static BasicInfoV3Record[] GetQueryByPhoneV2(string SPID, string PhoneNum, out int Result, out string ErrMsg)
        {
            List<BasicInfoV3Record> BasicInfoList = new List<BasicInfoV3Record>();
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
                cmd.CommandText = "up_Customer_V3_Interface_QueryByPhoneV2";

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

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

                            BasicInfoV3Record rs = new BasicInfoV3Record();
                            for (int i = 0; i < RowCount; i++)
                            {
                                string strSourceSPID = ds.Tables[0].Rows[i]["SourceSPID"].ToString().Trim();

                                rs = new BasicInfoV3Record();
                                rs.CustID = ds.Tables[0].Rows[i]["CustID"].ToString().Trim();
                                rs.CustType = ds.Tables[0].Rows[i]["CustType"].ToString().Trim();
                                rs.PhoneClass = ds.Tables[0].Rows[i]["PhoneClass"].ToString().Trim();
                                rs.RealName = ds.Tables[0].Rows[i]["RealName"].ToString().Trim();
                                rs.EnterpriseName = ds.Tables[0].Rows[i]["EnterpriseName"].ToString().Trim();
                                rs.Sex = ds.Tables[0].Rows[i]["Sex"].ToString().Trim();

                                //积分商城用户可以取得所有客户信息，其他系统用户只能看到号百客户信息
                                if (strScoreBesttonePSID == SPID)
                                {
                                    BasicInfoList.Add(rs);
                                }
                                else
                                {
                                    if ("41".Equals(rs.CustType) || "42".Equals(rs.CustType))
                                    {
                                        BasicInfoList.Add(rs);
                                    }
                                }
                            }
                        }
                Result = int.Parse(parResult.Value.ToString());
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取电话识别信息出错，" + e.Message;
            }

            return BasicInfoList.ToArray();
        }

        #endregion

        protected static void log(string method,string str)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("PhoneBO-"+method, msg);
        }


        public static int SPInterfaceGrant(string SPID, string InterfaceName, out string ErrMsg)
        {
            ErrMsg = "";
            int Result = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendFormat("SPID:{0}\r\n", SPID);

            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                sbLog.AppendFormat("mycon:{0}\r\n", mycon);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SPInterfaceGrant";


                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parInterfaceName = new SqlParameter("@InterfaceName", SqlDbType.VarChar, 50);
                parInterfaceName.Value = InterfaceName;
                cmd.Parameters.Add(parInterfaceName);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = Convert.ToInt32(parResult.Value.ToString());

                ErrMsg = Convert.ToString(parErrMsg.Value.ToString());

            }
            catch (Exception e)
            {
                sbLog.AppendFormat("SPInterfaceGrant异常:{0}\r\n", e.StackTrace);
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            finally
            {
                log("SPInterfaceGrant", sbLog.ToString());
            }

            return Result;
        
        }


        //==============================================================================================

        /// <summary>
        /// 验证电话是否可以做认证电话(这里的电话包括手机和电话)
        /// 作者：赵锐
        /// 日期:2009年8月5日
        /// </summary>
        public static int PhoneSel(string CustID, string Phone, out string ErrMsg)
        {
            ErrMsg = "";
            int Result = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendFormat("CustID:{0}\r\n",CustID);
            sbLog.AppendFormat("Phone:{0}\r\n", Phone);
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                sbLog.AppendFormat("mycon:{0}\r\n", mycon);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_PhoneSel";


                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = Convert.ToInt32(parResult.Value.ToString());

                ErrMsg = Convert.ToString(parErrMsg.Value.ToString());

            }
            catch (Exception e)
            {
                sbLog.AppendFormat("PhoneBO异常:{0}\r\n", e.StackTrace);
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            finally
            {
                log("PhoneSel", sbLog.ToString());
            }

            return Result;
        }


        /// <summary>
        /// 校验验证码发送次数
        /// 作者：李宏图
        /// 日期:2013年11月19日
        /// </summary>
        public static int PhoneSelV2(string CustID, string Phone, out string ErrMsg)
        {
            ErrMsg = "";
            int Result = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_PhoneSelV2";


                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = Convert.ToInt32(parResult.Value.ToString());

                ErrMsg = Convert.ToString(parErrMsg.Value.ToString());

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;
        }



        /// <summary>
        /// 验证手机是否存在
        /// 0代表可以用来做认证手机，否则不可以
        /// </summary>
        public static string IsAuthenPhoneV3(string Phone, string SPID, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            string CustID = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsAuthenPhoneV3";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parSPID = new SqlParameter("SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parSqlResult = new SqlParameter("Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return CustID;
        }


        /// <summary>
        /// 验证手机是否是认证手机 已经验证该手机是否被别的客户用作账户号
        /// 0代表可以用来做认证手机，否则不可以
        /// </summary>
        public static string IsAuthenPhoneV2(string Phone, string SPID, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            string CustID = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsAuthenPhoneV2";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parSPID = new SqlParameter("SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parSqlResult = new SqlParameter("Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return CustID;
        }

        /// <summary>
        /// 验证手机是否是认证手机
        /// </summary>
        public static string IsAuthenPhone(string Phone, string SPID, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            string CustID = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsAuthenPhone";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parSPID = new SqlParameter("SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parSqlResult = new SqlParameter("Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return CustID;
        }


        /// <summary>
        /// 验证手机号码是否是别人的认证手机，同时验证手机号码是否已经开过户
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int IsBesttoneAccountBindV6(string Phone, string CustID, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_IsBesttoneAccountBindV6";

                SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        }

        /// <summary>
        /// 验证手机是否是认证手机
        /// </summary>
        public static int IsBesttoneAccountBind(string Phone, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_IsBesttoneAccountBind";

                SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        }





        /// <summary>
        /// 删除一条电话记录
        /// </summary>
        public static int DelPhoneV2(string Phone, string SequenceID, out string ErrMsg)
        {
            ErrMsg = "";
            int Result = 0;
            DataSet ds = new DataSet();
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_DelPhoneV2";

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parSequenceID = new SqlParameter("SequenceID", SqlDbType.BigInt);
                parSequenceID.Value = SequenceID;
                cmd.Parameters.Add(parSequenceID);


                SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = Convert.ToInt32(parResult.Value.ToString());
                ErrMsg = Convert.ToString(parErrMsg.Value.ToString());

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return Result;
        }


        /// <summary>
        /// 删除一条电话记录
        /// </summary>
        public static int DelPhone(string Phone, string SequenceID, out string ErrMsg)
        {
            ErrMsg = "";
            int Result = 0;
            DataSet ds = new DataSet();
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_DelPhone";

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);


                SqlParameter parSequenceID = new SqlParameter("SequenceID", SqlDbType.BigInt);
                parSequenceID.Value = SequenceID;
                cmd.Parameters.Add(parSequenceID);


                SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = Convert.ToInt32(parResult.Value.ToString());

                ErrMsg = Convert.ToString(parErrMsg.Value.ToString());


            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return Result;
        }

        #region 绑定电话、认证电话设置


        /// <summary>
        /// 为客户设置电话(同一个电话对同一种客户类型只能认证一次，不同客户类型可以多次认证)
        /// </summary>
        public static int PhoneSetV2(string SPID, string CustID, string PhoneNum, string PhoneClass, string PhoneType, out string ErrMsg) 
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_PhoneSetV2";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

                SqlParameter parPhoneClass = new SqlParameter("@PhoneClass", SqlDbType.VarChar, 1);
                parPhoneClass.Value = PhoneClass;
                cmd.Parameters.Add(parPhoneClass);

                SqlParameter parPhoneType = new SqlParameter("@PhoneType", SqlDbType.VarChar, 1);
                parPhoneType.Value = PhoneType;
                cmd.Parameters.Add(parPhoneType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                CustID = parCustID.Value.ToString().Trim();
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;
        }


        /// <summary>
        /// 为客户设置电话(同一个电话对同一种客户类型只能认证一次，不同客户类型可以多次认证)
        /// </summary>
        public static int PhoneSetV3(string SPID, string CustID, string PhoneNum, string PhoneClass, string PhoneType, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_PhoneSetV3";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

                SqlParameter parPhoneClass = new SqlParameter("@PhoneClass", SqlDbType.VarChar, 1);
                parPhoneClass.Value = PhoneClass;
                cmd.Parameters.Add(parPhoneClass);

                SqlParameter parPhoneType = new SqlParameter("@PhoneType", SqlDbType.VarChar, 1);
                parPhoneType.Value = PhoneType;
                cmd.Parameters.Add(parPhoneType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                CustID = parCustID.Value.ToString().Trim();
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;
        }

        /// <summary>
        /// 为客户设置电话(同一个电话对同一种客户类型只能认证一次，不同客户类型可以多次认证)
        /// </summary>
        public static int PhoneSet(string SPID, string CustID, string PhoneNum, string PhoneClass, string PhoneType, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_PhoneSet";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

                SqlParameter parPhoneClass = new SqlParameter("@PhoneClass", SqlDbType.VarChar, 1);
                parPhoneClass.Value = PhoneClass;
                cmd.Parameters.Add(parPhoneClass);

                SqlParameter parPhoneType = new SqlParameter("@PhoneType", SqlDbType.VarChar, 1);
                parPhoneType.Value = PhoneType;
                cmd.Parameters.Add(parPhoneType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                CustID = parCustID.Value.ToString().Trim();
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }

            return Result;
        }

        /// <summary>
        /// 认证电话变更
        /// </summary>
        public static int ChangeAuthenPhone(string CustID, string AuthenPhone, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[up_Customer_V3_Interface_AuthenPhoneChange]";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAuthenPhone = new SqlParameter("@AuthenPhone", SqlDbType.VarChar, 20);
                parAuthenPhone.Value = AuthenPhone;
                cmd.Parameters.Add(parAuthenPhone);

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

        /// <summary>
        /// 将电话从客户的一般电话属性中解除
        /// </summary>
        public static int PhoneUnBind(string CustID, string PhoneNum, string PhoneClass, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UnBindPhone";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = PhoneNum;
                cmd.Parameters.Add(parPhoneNum);

                SqlParameter parPhoneClass = new SqlParameter("@PhoneClass", SqlDbType.VarChar, 1);
                parPhoneClass.Value = PhoneClass;
                cmd.Parameters.Add(parPhoneClass);

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

        #endregion

        #region 手机初定位

        /// <summary>
        /// 手机初定位查询
        /// </summary>
        /// <returns></returns>
        public static int PhonePositionQuery(string SPID, string PhoneID, string PhoneType, string ResultLevel, string MaxAge, System.DateTime startTime, string ExtendField, out string ActiveTime, out string AreaID, out string Area,
             out string LatitudeType, out string Latitude, out string LongitudeType, out string Longitude, out string outExtendField, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

            AreaID = "";
            Area = "";
            ActiveTime = "";
            outExtendField = "";
            LatitudeType = "";
            Latitude = "";
            LongitudeType = "";
            Longitude = "";

            System.DateTime endTime = System.DateTime.Now;
            System.TimeSpan ts = endTime - startTime;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_PhonePostionQuery";
            try
            {
                SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                sPID.Value = SPID;
                cmd.Parameters.Add(sPID);


                SqlParameter phoneID = new SqlParameter("@PhoneID", SqlDbType.VarChar, 48);
                phoneID.Value = PhoneID;
                cmd.Parameters.Add(phoneID);

                SqlParameter phoneType = new SqlParameter("@PhoneType", SqlDbType.VarChar, 2);
                phoneType.Value = PhoneType;
                cmd.Parameters.Add(phoneType);

                SqlParameter maxAge = new SqlParameter("@MaxAge", SqlDbType.Int);
                maxAge.Value = MaxAge;
                cmd.Parameters.Add(maxAge);


                SqlParameter timeDiff = new SqlParameter("@TimeDiff", SqlDbType.Int);
                timeDiff.Value = ts.Milliseconds;
                cmd.Parameters.Add(timeDiff);

                SqlParameter resultLevel = new SqlParameter("@ResultLevel", SqlDbType.VarChar, 2);
                resultLevel.Value = ResultLevel;
                cmd.Parameters.Add(resultLevel);


                SqlParameter parActiveTime = new SqlParameter("@ActiveTime", SqlDbType.DateTime);
                parActiveTime.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parActiveTime);

                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 3);
                parAreaID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAreaID);

                SqlParameter parArea = new SqlParameter("@Area", SqlDbType.VarChar, 50);
                parArea.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parArea);

                SqlParameter parLatitudeType = new SqlParameter("@LatitudeType", SqlDbType.VarChar, 2);
                parLatitudeType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLatitudeType);

                SqlParameter parLatitude = new SqlParameter("@Latitude", SqlDbType.VarChar, 20);
                parLatitude.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLatitude);

                SqlParameter parLongitudeType = new SqlParameter("@LongitudeType", SqlDbType.VarChar, 2);
                parLongitudeType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLongitudeType);

                SqlParameter parLongitude = new SqlParameter("@Longitude", SqlDbType.VarChar, 20);
                parLongitude.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLongitude);

                //SqlParameter parID = new SqlParameter("@ID", SqlDbType.BigInt);
                //parResult.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                AreaID = parAreaID.Value.ToString();
                Area = parArea.Value.ToString();
                LatitudeType = parLatitudeType.Value.ToString();
                Latitude = parLatitude.Value.ToString();
                LongitudeType = parLongitudeType.Value.ToString();
                Longitude = parLongitude.Value.ToString();

                //iD = Int64.Parse(parID.Value.ToString());
                Result = int.Parse(parResult.Value.ToString());

                ActiveTime = parActiveTime.Value.ToString();

                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }

            if (Result != 0)
            {
                try
                {
                    L1XMLExchange l1XMLExchange = new L1XMLExchange();
                    string ReqXml = l1XMLExchange.BuildQryCustInfoXML(PhoneID, ResultLevel);

                    StringBuilder msg_L1XML = new StringBuilder();
                    msg_L1XML.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                    msg_L1XML.Append("手机粗定位请求参数++++++++++++++++++++++\r\n\r\n");
                    msg_L1XML.Append(ReqXml + "\r\n");
                    msg_L1XML.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                    BTUCenterInterfaceLog.CenterForBizTourLog("PhonePosition", msg_L1XML);

                    string strUrl = System.Configuration.ConfigurationManager.AppSettings["PhonePostionQueryUrl"];

                    string returnValue = "";

                    if ("1".Equals(ResultLevel))
                    {
                        returnValue = HttpClient.PostMyData(strUrl, ReqXml, 10000);
                    }
                    else
                    {
                        returnValue = HttpClient.PostMyData(strUrl, ReqXml, 3000);
                    }
                    PhoenPostionQueryReturn phoenPostionQueryReturn = new PhoenPostionQueryReturn();


                    if (!"".Equals(returnValue))
                    {
                        phoenPostionQueryReturn = l1XMLExchange.AnalysisPhoenPostionQueryXML(returnValue);
                    }
                    else
                    {
                        Result = ErrorDefinition.BT_IError_Result_PhoenPostionError_Code;
                        ErrMsg = "落地方系统没有响应或超时";
                    }

                    LatitudeType = phoenPostionQueryReturn.LATITUDETYPE;
                    Latitude = phoenPostionQueryReturn.LATITUDE;
                    LongitudeType = phoenPostionQueryReturn.LONGITUDETYPE;
                    Longitude = phoenPostionQueryReturn.LONGITUDE;

                    System.DateTime endTime2 = System.DateTime.Now;
                    System.TimeSpan ts2 = endTime2 - endTime;

                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.CommandText = "up_Customer_V3_Interface_PhonePostionL1Query";


                    SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                    sPID.Value = SPID;
                    cmd2.Parameters.Add(sPID);


                    SqlParameter phoneID = new SqlParameter("@PhoneID", SqlDbType.VarChar, 48);
                    phoneID.Value = PhoneID;
                    cmd2.Parameters.Add(phoneID);

                    SqlParameter phoneType = new SqlParameter("@PhoneType", SqlDbType.VarChar, 2);
                    phoneType.Value = PhoneType;
                    cmd2.Parameters.Add(phoneType);

                    SqlParameter maxAge = new SqlParameter("@MaxAge", SqlDbType.Int);
                    maxAge.Value = MaxAge;
                    cmd2.Parameters.Add(maxAge);


                    SqlParameter timeDiff = new SqlParameter("@TimeDiff", SqlDbType.Int);
                    timeDiff.Value = ts.Milliseconds;
                    cmd2.Parameters.Add(timeDiff);

                    SqlParameter l1TimeDiff = new SqlParameter("@L1TimeDiff", SqlDbType.Int);
                    l1TimeDiff.Value = ts2.Milliseconds;
                    cmd2.Parameters.Add(l1TimeDiff);


                    SqlParameter areaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 3);
                    areaID.Value = phoenPostionQueryReturn.ROAMCITY;
                    cmd2.Parameters.Add(areaID);



                    SqlParameter reqXml = new SqlParameter("@ReqXml", SqlDbType.Text);
                    reqXml.Value = ReqXml;
                    cmd2.Parameters.Add(reqXml);

                    SqlParameter rspXml = new SqlParameter("@RspXml", SqlDbType.Text);
                    rspXml.Value = returnValue;
                    cmd2.Parameters.Add(rspXml);

                    SqlParameter l1Result = new SqlParameter("@L1Result", SqlDbType.Int);
                    if (Result != 0 && Result != -1)
                    {
                        l1Result.Value = Result;
                    }
                    else
                    {
                        l1Result.Value = phoenPostionQueryReturn.RESULT;
                    }

                    cmd2.Parameters.Add(l1Result);


                    SqlParameter resultLevel = new SqlParameter("@ResultLevel", SqlDbType.VarChar, 2);
                    resultLevel.Value = ResultLevel;
                    cmd2.Parameters.Add(resultLevel);

                    SqlParameter latitudeType = new SqlParameter("@LatitudeType", SqlDbType.VarChar, 2);
                    latitudeType.Value = LatitudeType;
                    cmd2.Parameters.Add(latitudeType);

                    SqlParameter latitude = new SqlParameter("@Latitude", SqlDbType.VarChar, 20);
                    latitude.Value = Latitude;
                    cmd2.Parameters.Add(latitude);

                    SqlParameter longitudeType = new SqlParameter("@LongitudeType", SqlDbType.VarChar, 2);
                    longitudeType.Value = LongitudeType;
                    cmd2.Parameters.Add(longitudeType);

                    SqlParameter longitude = new SqlParameter("@Longitude", SqlDbType.VarChar, 20);
                    longitude.Value = Longitude;
                    cmd2.Parameters.Add(longitude);


                    SqlParameter parActiveTime = new SqlParameter("@ActiveTime", SqlDbType.DateTime);
                    parActiveTime.Direction = ParameterDirection.Output;
                    cmd2.Parameters.Add(parActiveTime);

                    SqlParameter parArea = new SqlParameter("@Area", SqlDbType.VarChar, 50);
                    parArea.Direction = ParameterDirection.Output;
                    cmd2.Parameters.Add(parArea);

                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd2.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd2.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd2, DBUtility.BestToneCenterConStr);

                    AreaID = phoenPostionQueryReturn.ROAMCITY;
                    Area = parArea.Value.ToString();

                    ActiveTime = parActiveTime.Value.ToString();
                    if (Result == 0 || Result == -1)
                    {
                        Result = int.Parse(parResult.Value.ToString());
                        ErrMsg = parErrMsg.Value.ToString();
                    }

                }
                catch (Exception ex)
                {
                    Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
                }
                return Result;

            }
            return Result;
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="Message"></param>
        /// <param name="DealTime"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int GetPhoneSendMessage(string Phone, string Message, out DateTime DealTime,out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            DealTime = DateTime.Now;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_GetSendSMSRecordV2";

                SqlParameter parMessage = new SqlParameter("Message", SqlDbType.Text);
                parMessage.Value = Message;
                cmd.Parameters.Add(parMessage);


                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parDealTime = new SqlParameter("DealTime", SqlDbType.DateTime);
                parDealTime.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parDealTime);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                DealTime = Convert.ToDateTime(parDealTime.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return SqlResult;
            
        }


        #region 手机验证码

        /// <summary>
        /// 插入一条手机验证码
        /// 作者：赵锐
        /// 日期：2009年8月5日
        /// </summary>
        public static int InsertPhoneSendMassage(string CustID, string Message, string AuthenCode, string Phone,
             DateTime DealTime, string Description, int NotifyCount, int Result, string OPType, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_SendSMSRecord";

                SqlParameter parOPType = new SqlParameter("OPType", SqlDbType.VarChar, 1);
                parOPType.Value = OPType;
                cmd.Parameters.Add(parOPType);

                SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
                parResult.Value = Result;
                cmd.Parameters.Add(parResult);

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parMessage = new SqlParameter("Message", SqlDbType.Text);
                parMessage.Value = Message;
                cmd.Parameters.Add(parMessage);

                SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
                parAuthenCode.Value = AuthenCode;
                cmd.Parameters.Add(parAuthenCode);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parDealTime = new SqlParameter("DealTime", SqlDbType.DateTime);
                parDealTime.Value = DealTime;
                cmd.Parameters.Add(parDealTime);

                SqlParameter parDescription = new SqlParameter("Description", SqlDbType.VarChar, 40);
                parDescription.Value = Description;
                cmd.Parameters.Add(parDescription);

                SqlParameter parNotifyCount = new SqlParameter("NotifyCount", SqlDbType.Int);
                parNotifyCount.Value = NotifyCount;
                cmd.Parameters.Add(parNotifyCount);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return SqlResult;
        }


        public static int SelSendSMSMessageAuthenCode(string Phone,out string AuthenCode,out string ErrMsg)
        {
            AuthenCode = "";
            ErrMsg = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelSendSMSMassageAuthenCode";

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
                parAuthenCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAuthenCode);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                AuthenCode = Convert.ToString(parAuthenCode.Value.ToString());
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        
        }

        /// <summary>
        /// 验证客户手机验证码
        /// 作者：赵锐
        /// 日期：2009年8月5日
        /// </summary>
        public static int SelSendSMSMassageOnRegister(string CustID, string Phone, string AuthenCode, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_SelSendSMSMassageOnRegister";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                if (String.IsNullOrEmpty(CustID))
                {
                    CustID = "";
                }
                parCustID.Value = CustID;

                cmd.Parameters.Add(parCustID);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
                parAuthenCode.Value = AuthenCode;
                cmd.Parameters.Add(parAuthenCode);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return SqlResult;
        }


        /// <summary>
        /// 验证客户手机验证码
        /// 作者：赵锐
        /// 日期：2009年8月5日
        /// </summary>
        public static int SelSendSMSMassage(string CustID, string Phone, string AuthenCode, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_SelSendSMSMassage";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                if (String.IsNullOrEmpty(CustID))
                {
                    CustID = "";
                }
                parCustID.Value = CustID;

                cmd.Parameters.Add(parCustID);

                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
                parAuthenCode.Value = AuthenCode;
                cmd.Parameters.Add(parAuthenCode);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return SqlResult;
        }

        #endregion

    }
}
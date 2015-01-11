/*********************************************************************************************************
 *     描述: 客户信息平台—密码设置接口
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

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 密码业务层方法
    /// 1:语音密码，2:web密码
    /// </summary>
    public class PassWordBO
    {
        /// <summary>
        /// 判断原始密码是否正确
        /// 作者：周涛      时间：2009-8-18
        /// 修改：          时间：
        /// </summary>
        public static bool OldPwdIsRight(string CustID, String OldPwd, string PwdType, out string ErrMsg)
        {
            bool temp = false;
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_OldPwdIsRight";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOldPwd = new SqlParameter("@OldPwd", SqlDbType.VarChar, 128);
                if (String.IsNullOrEmpty(OldPwd))
                    parOldPwd.Value = "";
                else
                    parOldPwd.Value = CryptographyUtil.Encrypt(OldPwd);
                cmd.Parameters.Add(parOldPwd);

                SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 1);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

                if (Result == 0)
                {
                    temp = true;
                }
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "原始密码错误" + ex.Message;
            }
            return temp;
        }

        /// <summary>
        /// 判断语音密码是否为空
        /// </summary>
        public static bool VoicePwdIsNull(string CustID, out string ErrMsg)
        {
            bool temp = false;
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_VoicePwdIsNull";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

                if (Result == 0)
                {
                    temp = true;
                }
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "VoicePwdIsNull-错误" + ex.Message;
            }
            return temp;
        }

        /// <summary>
        /// 密码设置接口
        /// </summary>
        public static int SetPassword(string SPID, string CustID, string Pwd, string PwdType, string ExtendField, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_SetPwd";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parPwd = new SqlParameter("@Pwd", SqlDbType.VarChar, 50);
                parPwd.Value = CryptographyUtil.Encrypt(Pwd);
                cmd.Parameters.Add(parPwd);

                SqlParameter parPwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 1);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();

            }
            catch(Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }
            return Result;


        }

        /// <summary>
        /// 重置号码百事通账户支付密码
        /// </summary>
        public static int ReSetPayPassword(String SPID, String CustID,String PasswordOld,String PasswordNew,out String ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            try
            {

            }
            catch { }

            return Result;
        }

        #region 密码提示问题

        /// <summary>
        /// 作者：张英杰   时间：2009-8-3
        /// 修改：
        /// 取得提示问题列表
        /// </summary>
        public static DataSet QueryPwdQuestion()
        {
            DataSet tmpData = new DataSet();

            try
            {
                string Sql_PwdQuestionRecord = "select [QuestionID],[Question] from [PwdQuestion]";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql_PwdQuestionRecord;

                // 获取数据
                tmpData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                // 添加默认项目
                if (tmpData.Tables[0].Rows.Count <= 0)
                {
                    DataRow dr = tmpData.Tables[0].NewRow();
                    dr[0] = 1;
                    dr[1] = "无问题";
                    tmpData.Tables[0].Rows.Add(dr);
                }
                else
                {
                    DataRow dr = tmpData.Tables[0].NewRow();
                    dr[0] = 0;
                    dr[1] = "请选择问题";
                    tmpData.Tables[0].Rows.InsertAt(dr, 0);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return tmpData;
        }

        /// <summary>
        /// 根据custID获取问题答案
        /// 作者：张英杰   时间：2009-8-3
        /// 修改：
        /// </summary>
        public static DataSet QueryPwdQuestionAnswer(String custID)
        {
            DataSet tmpData = new DataSet();

            try
            {
                string Sql_PwdQuestionRecord = "select [SequenceID],[QuestionID],[Answer]from [Answer] where [CustID]=@CustID  order by [QuestionID] ";

                SqlCommand cmd = new SqlCommand();
                //cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql_PwdQuestionRecord;

                cmd.Parameters.AddWithValue("@CustID", custID);

                // 获取数据
                tmpData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return tmpData;
        }

        /// <summary>
        /// 更新密码问题答案
        /// </summary>
        public static int UpdatePwdQuestionAnswer(String SequenceID, String CustID, int QuestionID, String Answer, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_UserPortal_Answer";
            try
            {
                SqlParameter sequenceID = new SqlParameter("@SequenceID", SqlDbType.BigInt);

                if ("".Equals(SequenceID))
                {

                    sequenceID.Value = 0;
                    cmd.Parameters.Add(sequenceID);

                }
                else
                {
                    sequenceID.Value = Int64.Parse(SequenceID);
                    cmd.Parameters.Add(sequenceID);
                }
                SqlParameter custID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                custID.Value = CustID;
                cmd.Parameters.Add(custID);

                SqlParameter questionID = new SqlParameter("@QuestionID", SqlDbType.Int);
                questionID.Value = QuestionID;
                cmd.Parameters.Add(questionID);

                SqlParameter answer = new SqlParameter("@Answer", SqlDbType.VarChar, 256);
                answer.Value = Answer;
                cmd.Parameters.Add(answer);

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
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            return Result;

        }

        #endregion

    }
}

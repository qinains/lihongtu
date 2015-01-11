/*********************************************************************************************************
 *     ����: �ͻ���Ϣƽ̨���������ýӿ�
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2005
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2005
 *     ����: ����
 * ��ϵ��ʽ: 
 *     ��˾: �����Ƽ�(�Ͼ�)�ɷ����޹�˾
 * ��������: 2009-08-11
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
    /// ����ҵ��㷽��
    /// 1:�������룬2:web����
    /// </summary>
    public class PassWordBO
    {
        /// <summary>
        /// �ж�ԭʼ�����Ƿ���ȷ
        /// ���ߣ�����      ʱ�䣺2009-8-18
        /// �޸ģ�          ʱ�䣺
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
                ErrMsg = "ԭʼ�������" + ex.Message;
            }
            return temp;
        }

        /// <summary>
        /// �ж����������Ƿ�Ϊ��
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
                ErrMsg = "VoicePwdIsNull-����" + ex.Message;
            }
            return temp;
        }

        /// <summary>
        /// �������ýӿ�
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
        /// ���ú������ͨ�˻�֧������
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

        #region ������ʾ����

        /// <summary>
        /// ���ߣ���Ӣ��   ʱ�䣺2009-8-3
        /// �޸ģ�
        /// ȡ����ʾ�����б�
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

                // ��ȡ����
                tmpData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                // ���Ĭ����Ŀ
                if (tmpData.Tables[0].Rows.Count <= 0)
                {
                    DataRow dr = tmpData.Tables[0].NewRow();
                    dr[0] = 1;
                    dr[1] = "������";
                    tmpData.Tables[0].Rows.Add(dr);
                }
                else
                {
                    DataRow dr = tmpData.Tables[0].NewRow();
                    dr[0] = 0;
                    dr[1] = "��ѡ������";
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
        /// ����custID��ȡ�����
        /// ���ߣ���Ӣ��   ʱ�䣺2009-8-3
        /// �޸ģ�
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

                // ��ȡ����
                tmpData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return tmpData;
        }

        /// <summary>
        /// �������������
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

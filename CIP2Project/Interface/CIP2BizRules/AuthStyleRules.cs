/*********************************************************************************************************
 *   ����: �ͻ���Ϣƽ̨��������𰸹���
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2005
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2002
 *     ����: ��Ӣ��
 * ��ϵ��ʽ: 
 *    ��˾: �й����ż��źŰ���Ϣ�������޹�˾
 * ��������: 2009-08-03
 *********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Data;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// ��֤��ʽ������
    /// </summary>
    public class AuthStyleRules
    {
        /// <summary>
        /// ��֤��ʽ֪ͨ�ӿ�
        /// ���ߣ���Ӣ��   ʱ�䣺2009-8-10
        /// </summary>
        public static int AuthStyleNotify(string SPID, string CustID, string AuthenName,
            string AuthenType, string OPType, string ExtendField, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_AuthStyleNotify";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 48);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 1);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);

                SqlParameter parOPType = new SqlParameter("@OPType", SqlDbType.VarChar, 8);
                parOPType.Value = OPType;
                cmd.Parameters.Add(parOPType);

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
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;

        }

        /// <summary>
        /// ��֤��ʽ��ѯ�ӿ�
        /// ���ߣ���Ӣ��   ʱ�䣺2009-8-10
        /// </summary>
        public static int AuthStyleQueryByAuthenName(string SPID, string AuthenName, string AuthenType, 
            out string ErrMsg, out string CustID, out string UserAccount)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            CustID = "";
            UserAccount = "";

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_V3_Interface_AuthStyleQueryByAuthenName";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 48);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 1);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);



                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parCustID = new SqlParameter("@CustID ", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount ", SqlDbType.VarChar, 16);
                parUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustID = parCustID.Value.ToString();
                UserAccount = parUserAccount.Value.ToString();


            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;

        }

    }
}

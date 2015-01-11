/*********************************************************************************************************
 *     ����: �ͻ���Ϣƽ̨��ʡ�ͻ�ID��Ӧ��ϵ��ѯ�ӿ�
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2005
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2005
 *     ����: ����
 * ��ϵ��ʽ: 
 *     ��˾: �����Ƽ�(�Ͼ�)�ɷ����޹�˾
 * ��������: 2009-08-03
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

    public class CustProvinceRelation
    {

        /// <summary>
        /// ʡ�ͻ�ID��Ӧ��ϵ��ѯ�ӿڷ��ؼ�¼
        /// ���ߣ�����      ʱ�䣺2009-08-03
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public static int getCustProvinceRelation(string SPID, string OuterID, string ProvinceID, out string CustID,  out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_CustProvinceRelationQuery";

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parOuterID = new SqlParameter("@OuterID", SqlDbType.VarChar, 20);
                parOuterID.Value = OuterID;
                cmd.Parameters.Add(parOuterID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 20);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int, 4);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString().Trim());
                ErrMsg = parErrMsg.Value.ToString().Trim();
                CustID = parCustID.Value.ToString().Trim();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + e.Message;
            }


            return Result;
        }

        /// <summary>
        /// ��ȡ����ʡ����Ϣ
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProvince()
        {
            DataTable dt = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_GetProvince";

                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds != null)
                {

                    dt = ds.Tables[0];
                }

            }
            catch { }
            return dt;
        }

    }
}

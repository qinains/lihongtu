/*********************************************************************************************************
 *     描述: 客户信息平台—客户扩展信息查询接口
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 周涛
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
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
    /// 用户扩展信息管理
    /// </summary>
    public class CustExtendInfo
    {
        /// <summary>
        /// 客户扩展信息查询接口返回记录
        /// 作者：周涛      时间：2009-07-31
        /// 修改：          时间：
        /// </summary>
        public static int getCustExtendInfo(string SPID, string CustID, out string ErrMsg, out string Birthday,
                                            out string EduLevel, out string Favorite, out string IncomeLevel)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            Birthday = "";
            EduLevel = "";
            Favorite = "";
            IncomeLevel = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_CustExtendInfoQuery";

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

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 10);
                parBirthday.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parBirthday);

                SqlParameter parEduLevel = new SqlParameter("@EduLevel", SqlDbType.VarChar, 1);
                parEduLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parEduLevel);

                SqlParameter parFavorite = new SqlParameter("@Favorite", SqlDbType.VarChar, 256);
                parFavorite.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parFavorite);

                SqlParameter parIncomeLevel = new SqlParameter("@IncomeLevel", SqlDbType.VarChar, 1);
                parIncomeLevel.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parIncomeLevel);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                Birthday = parBirthday.Value.ToString();
                EduLevel = parEduLevel.Value.ToString();
                Favorite = parFavorite.Value.ToString();
                IncomeLevel = parIncomeLevel.Value.ToString();


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

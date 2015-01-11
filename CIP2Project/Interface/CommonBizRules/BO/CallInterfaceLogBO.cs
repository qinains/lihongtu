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
    /// 接口调用日志管理类
    /// </summary>
    public class CallInterfaceLogBO
    {
        /// <summary>
        /// 插入一条日志
        /// </summary>
        /// <param name="entity"></param>
        public static void InsertLog(CallInterfaceLog entity)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("up_Customer_V3_Interface_WriteCallInterfaceLog"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter parIP = new SqlParameter("@IP", entity.IP);
                    cmd.Parameters.Add(parIP);

                    SqlParameter parSPID = new SqlParameter("@SPID", entity.SPID);
                    cmd.Parameters.Add(parSPID);

                    SqlParameter parInterfaceName = new SqlParameter("@InterfaceName", entity.InterfaceName);
                    cmd.Parameters.Add(parInterfaceName);

                    SqlParameter parInParameters = new SqlParameter("@InParameters", entity.InParameters.Replace("\'", "\""));          //处理单引号，单引号在sql插入中会报错
                    cmd.Parameters.Add(parInParameters);

                    SqlParameter parOutParameters = new SqlParameter("@OutParameters", entity.OutParameters.Replace("\'", "\""));
                    cmd.Parameters.Add(parOutParameters);

                    SqlParameter parCallResult = new SqlParameter("@CallResult", entity.CallResult);
                    cmd.Parameters.Add(parCallResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", entity.ErrMsg.Replace("\'", "\""));
                    cmd.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}

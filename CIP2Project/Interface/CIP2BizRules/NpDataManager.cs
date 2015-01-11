/*********************************************************************************************************
 *   描述: 客户信息平台二期网间移动号码携带管理
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2002
 *     作者: 张英杰
 * 联系方式: 
 *    公司: 中国电信集团号百信息服务有限公司
 * 创建日期: 2010-05-12
 *********************************************************************************************************/
using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Web;
using System.Configuration;
using System.Data.OracleClient;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;




namespace Linkage.BestTone.Interface.Rule
{
    public class NpDataManager
    {

        public static int NpDataManagerQuery(string SPID, string NPNumber, out string PortInNetID, out string PortOutNetID, out string HomeNetID, out string BeginDate, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            PortInNetID = "";
            PortOutNetID = "";
            HomeNetID = "";
            BeginDate = "";
            //ErrorDescription = "";
            try
            {

                OracleDataReader reader = OracleDBUtility.ExecuteReader("select portinnetid,portoutnetid,homenetid, to_char(begindate,'YYYY-MM-DD') as begindate from npdata where npnumber=" + NPNumber + "");
                //string importFileDate = "";

                if (reader.Read())
                {
                    PortInNetID = reader.GetString(0);
                    PortOutNetID = reader.GetString(1);
                    HomeNetID = reader.GetString(2);
                    BeginDate = reader.GetString(3);
                }
                else
                {
                    Result = ErrorDefinition.BT_IError_Result_NPDataNull_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_NPDataNullError_Msg;
                    return Result;
                }



            }
            catch (Exception e)
            {
                ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg+e.Message.ToString();
                return Result;

            }
            Result = ErrorDefinition.IError_Result_Success_Code;
            ErrMsg = ErrorDefinition.IError_Result_Success_Msg;

            return Result;
        }

    }
}
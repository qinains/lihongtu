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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;



namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 票据管理类
    /// </summary>
    public class CIPTicketManager
    {

        /// <summary>
        /// 创建票据
        /// 作者：张英杰   时间：2009-8-14
        /// 修改：
        /// </summary>
        public static int insertCIPTicket(string Ticket, string SPID, string CustID, string RealName, string UserName, string NickName, 
                                        string OuterID,string Description,string LoginAuthenName,string LoginAuthenType, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            StringBuilder str = new StringBuilder();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_InsertCIPTicket";

            try
            {
                str.AppendFormat("++++++++++++++++++++++++++++++++\r\n");
                str.AppendFormat("【DateTime:{0}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                str.AppendFormat("ticket:{0},spid:{1},custid:{2},realname:{3},username:{4},nickname:{5},outerid:{6},authenname:{7},authentype:{8}",
                    Ticket, SPID, CustID, RealName, UserName, NickName, OuterID, LoginAuthenName, LoginAuthenType);

                SqlParameter ticket = new SqlParameter("@Ticket", SqlDbType.VarChar, 50);
                ticket.Value = Ticket;
                cmd.Parameters.Add(ticket);

                SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 50);
                sPID.Value = SPID;
                cmd.Parameters.Add(sPID);


                SqlParameter custID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                custID.Value = CustID;
                cmd.Parameters.Add(custID);

                SqlParameter realName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                realName.Value = RealName;
                cmd.Parameters.Add(realName);

                SqlParameter userName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                userName.Value = UserName;
                cmd.Parameters.Add(userName);

                SqlParameter nickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                nickName.Value = NickName;
                cmd.Parameters.Add(nickName);

                SqlParameter outID = new SqlParameter("@OutID", SqlDbType.VarChar, 30);
                outID.Value = OuterID;
                cmd.Parameters.Add(outID);

                SqlParameter description = new SqlParameter("@Description", SqlDbType.VarChar, 30);
                description.Value = Description;
                cmd.Parameters.Add(description);


                SqlParameter loginAuthenName = new SqlParameter("@LoginAuthenName", SqlDbType.VarChar, 48);
                loginAuthenName.Value = LoginAuthenName;
                cmd.Parameters.Add(loginAuthenName);

                SqlParameter loginAuthenType = new SqlParameter("@LoginAuthenType", SqlDbType.Int);
                loginAuthenType.Value = LoginAuthenType;
                cmd.Parameters.Add(loginAuthenType);

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
            finally
            {
                str.AppendFormat("result:{0},ErrMsg:{1}", Result, ErrMsg);
                str.AppendFormat("++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("insertCIPTicketTest", str);
            }
            return Result;

        }

        /// <summary>
        /// 暂时无用
        /// </summary>
        public static int insertUAMTicket(string Ticket, string SPID, string CustID, string RealName, string UserName, string NickName, 
                                        string OuterID, string Description, string LoginAuthenName, string LoginAuthenType, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_InsertUAMTicket";
            try
            {
                SqlParameter ticket = new SqlParameter("@Ticket", SqlDbType.VarChar, 23);
                ticket.Value = Ticket;
                cmd.Parameters.Add(ticket);

                SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 50);
                sPID.Value = SPID;
                cmd.Parameters.Add(sPID);


                SqlParameter custID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                custID.Value = CustID;
                cmd.Parameters.Add(custID);

                SqlParameter realName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                realName.Value = RealName;
                cmd.Parameters.Add(realName);

                SqlParameter userName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                userName.Value = UserName;
                cmd.Parameters.Add(userName);

                SqlParameter nickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                nickName.Value = NickName;
                cmd.Parameters.Add(nickName);

                SqlParameter outID = new SqlParameter("@OutID", SqlDbType.VarChar, 30);
                outID.Value = OuterID;
                cmd.Parameters.Add(outID);

                SqlParameter description = new SqlParameter("@Description", SqlDbType.VarChar, 30);
                description.Value = Description;
                cmd.Parameters.Add(description);


                SqlParameter loginAuthenName = new SqlParameter("@LoginAuthenName", SqlDbType.VarChar, 48);
                loginAuthenName.Value = LoginAuthenName;
                cmd.Parameters.Add(loginAuthenName);

                SqlParameter loginAuthenType = new SqlParameter("@LoginAuthenType", SqlDbType.Int);
                loginAuthenType.Value = LoginAuthenType;
                cmd.Parameters.Add(loginAuthenType);

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


        /// <summary>
        /// 客户信息平台票据解读接口
        /// 作者：lihongtu   时间：2009-8-17
        /// 修改：
        /// </summary>
        public static int checkYgTicket(string SPID, string Ticket, string ExtendField, out string CustID, out string RealName, out string UserName,
                        out string NickName, out string OutID, string Description, out string LoginAuthenName, out string LoginAuthenType, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            LoginAuthenName = "";
            LoginAuthenType = "";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_CheckYgTicket";
            try
            {
                SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 50);
                sPID.Value = SPID;
                cmd.Parameters.Add(sPID);

                SqlParameter ticket = new SqlParameter("@Ticket", SqlDbType.VarChar, 50);
                ticket.Value = Ticket;
                cmd.Parameters.Add(ticket);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parOutID = new SqlParameter("@OutID", SqlDbType.VarChar, 30);
                parOutID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOutID);

                SqlParameter parLoginAuthenName = new SqlParameter("@LoginAuthenName", SqlDbType.VarChar, 48);
                parLoginAuthenName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLoginAuthenName);

                SqlParameter parLoginAuthenType = new SqlParameter("@LoginAuthenType", SqlDbType.Int);
                parLoginAuthenType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLoginAuthenType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                CustID = parCustID.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                OutID = parOutID.Value.ToString();
                LoginAuthenName = parLoginAuthenName.Value.ToString();
                LoginAuthenType = parLoginAuthenType.Value.ToString();


                Result = int.Parse(parResult.Value.ToString());

                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            finally
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("++++++++++++++++++++++++++++++++\r\n");
                str.AppendFormat("result:{0},ErrMsg:{1},ticket:{2}", Result, ErrMsg, Ticket);
                str.AppendFormat("++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("checkCIPTicketTest3", str);
            }
            return Result;

        }



        /// <summary>
        /// 客户信息平台票据解读接口
        /// 作者：张英杰   时间：2009-8-17
        /// 修改：
        /// </summary>
        public static int checkCIPTicket(string SPID, string Ticket, string ExtendField, out string CustID, out string RealName, out string UserName, 
                        out string NickName, out string OutID, string Description, out string LoginAuthenName,out string LoginAuthenType, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            LoginAuthenName = "";
            LoginAuthenType = "";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_CheckCIPTicket";
            try
            {
                SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 50);
                sPID.Value = SPID;
                cmd.Parameters.Add(sPID);

                SqlParameter ticket = new SqlParameter("@Ticket", SqlDbType.VarChar, 50);
                ticket.Value = Ticket;
                cmd.Parameters.Add(ticket);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parOutID = new SqlParameter("@OutID", SqlDbType.VarChar, 30);
                parOutID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOutID);

                SqlParameter parLoginAuthenName = new SqlParameter("@LoginAuthenName", SqlDbType.VarChar, 48);
                parLoginAuthenName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLoginAuthenName);

                SqlParameter parLoginAuthenType = new SqlParameter("@LoginAuthenType", SqlDbType.Int);
                parLoginAuthenType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLoginAuthenType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                CustID = parCustID.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                OutID = parOutID.Value.ToString();
                LoginAuthenName = parLoginAuthenName.Value.ToString();
                LoginAuthenType = parLoginAuthenType.Value.ToString();


                Result = int.Parse(parResult.Value.ToString());

                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + ex.Message;
            }
            finally
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("++++++++++++++++++++++++++++++++\r\n");
                str.AppendFormat("result:{0},ErrMsg:{1},ticket:{2}", Result, ErrMsg, Ticket);
                str.AppendFormat("++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("checkCIPTicketTest3", str);
            }
            return Result;

        }

        /// <summary>
        /// 暂时无用
        /// </summary>
        public static int checkUAMTicket(string SPID, string Ticket, string ExtendField, out string CustID, out string RealName, out string UserName, 
                        out string NickName, out string OutID, string Description, out string LoginAuthenName, out string LoginAuthenType, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = "";
            RealName = "";
            UserName = "";
            NickName = "";
            OutID = "";
            LoginAuthenName = "";
            LoginAuthenType = "";





            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_CheckUAMTicket";
            try
            {

                SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 50);
                sPID.Value = SPID;
                cmd.Parameters.Add(sPID);

                SqlParameter ticket = new SqlParameter("@Ticket", SqlDbType.VarChar, 30);
                ticket.Value = Ticket;
                cmd.Parameters.Add(ticket);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                parRealName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRealName);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                parNickName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parNickName);

                SqlParameter parOutID = new SqlParameter("@OutID", SqlDbType.VarChar, 30);
                parOutID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOutID);

                SqlParameter parLoginAuthenName = new SqlParameter("@LoginAuthenName", SqlDbType.VarChar, 48);
                parLoginAuthenName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLoginAuthenName);

                SqlParameter parLoginAuthenType = new SqlParameter("@LoginAuthenType", SqlDbType.Int);
                parLoginAuthenType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLoginAuthenType);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                CustID = parCustID.Value.ToString();
                RealName = parRealName.Value.ToString();
                UserName = parUserName.Value.ToString();
                NickName = parNickName.Value.ToString();
                OutID = parOutID.Value.ToString();
                LoginAuthenName = parLoginAuthenName.Value.ToString();
                LoginAuthenType = parLoginAuthenType.Value.ToString();


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

    }
}
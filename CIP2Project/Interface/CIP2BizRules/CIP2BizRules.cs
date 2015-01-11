using System;
using System.Collections.Generic;
using System.Text;
using Linkage.BestTone.Interface.BTException;
using System.Data.SqlClient;
using System.Data;
using Linkage.BestTone.Interface.Utility;
using System.Xml;
using BTUCenter.Proxy;
using System.Configuration;
using System.Text.RegularExpressions;
using Passport.Common.Web.Mail;
namespace Linkage.BestTone.Interface.Rule
{
    public class CIP2BizRules
    {


        protected static void logByMethod(string logContent, string method)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(logContent);
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog(method, msg);
        }

        protected static void log(string str)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneAccountMain-IsBesttoneAccountBindV2", msg);
        }


        /// <summary>
        /// �Ű�SPID
        /// </summary>
        private static string strSPID = System.Configuration.ConfigurationManager.AppSettings["BesttoneSPID"];


        /// <summary>
        /// ��¼�󱣴�AccessToken
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="ip"></param>
        /// <param name="AccessToken"></param>
        /// <param name="UserID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int InsertUnifyAccessToken(string SPID, string IP, string AccessToken, string UserID, string CustID, string RealName, string NickName, string LoginName,string LoginPassWord, string OperType, string Description, out string ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("AccessToken:{0},UserId:{1}\r\n", AccessToken, UserID);
            ErrMsg = String.Empty;
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_InsertUnifyAccessToken";

                SqlParameter parAccessToken = new SqlParameter("@AccessToken", SqlDbType.VarChar, 72);
                parAccessToken.Value = AccessToken;
                cmd.Parameters.Add(parAccessToken);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 36);
                parUserID.Value = UserID;
                cmd.Parameters.Add(parUserID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                if (String.IsNullOrEmpty(RealName))
                {
                    RealName = LoginName;
                }
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                if (String.IsNullOrEmpty(NickName))
                {
                    NickName = LoginName;
                }
                parNickName.Value = NickName;

                cmd.Parameters.Add(parNickName);


                SqlParameter parLoginName = new SqlParameter("@LoginName", SqlDbType.VarChar, 48);
                parLoginName.Value = LoginName;
                cmd.Parameters.Add(parLoginName);

                SqlParameter parLoginPassWord = new SqlParameter("@LoginPassWord", SqlDbType.VarChar, 100);
                parLoginPassWord.Value = LoginPassWord;
                cmd.Parameters.Add(parLoginPassWord);
               

                SqlParameter parDescription = new SqlParameter("@Description", SqlDbType.VarChar, 256);
                parDescription.Value = Description;
                cmd.Parameters.Add(parDescription);

                SqlParameter parOperType = new SqlParameter("@OperType", SqlDbType.Int);
                parOperType.Value = OperType;
                cmd.Parameters.Add(parOperType);

                SqlParameter parIP = new SqlParameter("@IP", SqlDbType.VarChar, 15);
                parIP.Value = IP;
                cmd.Parameters.Add(parIP);


                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());

                ErrMsg = parErrMsg.Value.ToString();

                strLog.AppendFormat("up_Customer_V3_Interface_InsertAccessToken:����:Result:{0},ErrMsg:{1}\r\n", SqlResult, ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "InsertAccessToken");
            }
            return SqlResult;
        }



        /// <summary>
        /// ��¼�󱣴�AccessToken
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="ip"></param>
        /// <param name="AccessToken"></param>
        /// <param name="UserID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int InsertAccessToken(string SPID,string IP,string AccessToken, string UserID,string CustID,string RealName,string NickName,string LoginName,string OperType,string Description, out string ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("AccessToken:{0},UserId:{1}\r\n",AccessToken,UserID);
            ErrMsg = String.Empty;
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_InsertAccessToken";

                SqlParameter parAccessToken = new SqlParameter("@AccessToken", SqlDbType.VarChar, 72);
                parAccessToken.Value = AccessToken;
                cmd.Parameters.Add(parAccessToken);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 36);
                parUserID.Value = UserID;
                cmd.Parameters.Add(parUserID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                if (String.IsNullOrEmpty(RealName))
                {
                    RealName = LoginName;
                }
                parRealName.Value = RealName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parNickName = new SqlParameter("@NickName", SqlDbType.VarChar, 30);
                if (String.IsNullOrEmpty(NickName))
                {
                    NickName = LoginName;
                }
                parNickName.Value = NickName;
                
                cmd.Parameters.Add(parNickName);

                
                SqlParameter parLoginName = new SqlParameter("@LoginName", SqlDbType.VarChar, 48);
                parLoginName.Value = LoginName;
                cmd.Parameters.Add(parLoginName);


                SqlParameter parDescription = new SqlParameter("@Description", SqlDbType.VarChar, 256);
                parDescription.Value = Description;
                cmd.Parameters.Add(parDescription);

                SqlParameter parOperType = new SqlParameter("@OperType", SqlDbType.Int);
                parOperType.Value = OperType;
                cmd.Parameters.Add(parOperType);

                SqlParameter parIP = new SqlParameter("@IP", SqlDbType.VarChar,15);
                parIP.Value = IP;
                cmd.Parameters.Add(parIP);


                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());

                ErrMsg = parErrMsg.Value.ToString();

                strLog.AppendFormat("up_Customer_V3_Interface_InsertAccessToken:����:Result:{0},ErrMsg:{1}\r\n", SqlResult, ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "InsertAccessToken");
            }
            return SqlResult;
       }

        /// <summary>
        /// �����ۺ�ƽ̨��UserID������Űٵ�CustID
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int GetCustIDByUserID(String UserID, out String CustID, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            int SqlResult = 0;
            ErrMsg = "";
            CustID = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_GetCustIDByUserID";


                SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 36);
                parUserID.Value = UserID;
                cmd.Parameters.Add(parUserID);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());

                CustID = parCustID.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();

                strLog.AppendFormat("up_Customer_V3_Interface_GetCustIDByUserID:����:CustID:{0},ErrMsg:{1}\r\n", CustID, ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "GetCustIDByUserID");
            }
            return SqlResult;
        }


        /// <summary>
        /// ����custid����Loginpassword
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="AccessToken"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int UpdatePasswordFromCustIDAccessToken(String CustID, String AccessToken, String PassWord, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            ErrMsg = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_UpdatePasswordFromCustIDAccessToken";


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccessToken = new SqlParameter("@AccessToken", SqlDbType.VarChar, 72);
                parAccessToken.Value = AccessToken;
                cmd.Parameters.Add(parAccessToken);

                SqlParameter parPassWord = new SqlParameter("@PassWord", SqlDbType.VarChar, 100);
                parPassWord.Value = PassWord;
                cmd.Parameters.Add(parPassWord);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
            
                ErrMsg = parErrMsg.Value.ToString();
                strLog.AppendFormat("UpdatePasswordFromCustIDAccessToken:����:Result:{0},ErrMsg:{1}\r\n", SqlResult, ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "UpdatePasswordFromCustIDAccessToken");
            }
            return SqlResult;

        }

        /// <summary>
        /// ����custid��ȡaccesstoken&password
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="AccessToken"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int FetchAccessTokenPasswordFromCustID(String CustID, out String AccessToken,out String PassWord, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            ErrMsg = "";
            AccessToken = "";
            PassWord = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_FetchAccessTokenPassWordFromCustID";


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccessToken = new SqlParameter("@AccessToken", SqlDbType.VarChar, 72);
                parAccessToken.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAccessToken);

                SqlParameter parPassWord = new SqlParameter("@PassWord", SqlDbType.VarChar, 100);
                parPassWord.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPassWord);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                AccessToken = parAccessToken.Value.ToString();
                PassWord = parPassWord.Value.ToString();

                ErrMsg = parErrMsg.Value.ToString();
                strLog.AppendFormat("FetchAccessTokenPassWordFromCustID:����:Result:{0},ErrMsg:{1},CustID:{2}AccessToken:{3},PassWord:{4}\r\n", SqlResult, ErrMsg, CustID, AccessToken,PassWord);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "FetchAccessTokenPassWordFromCustID");
            }
            return SqlResult;

        }


        /// <summary>
        /// ����custid��ȡaccesstoken
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="AccessToken"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int FetchAccessTokenFromCustID(String CustID, out String AccessToken, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            ErrMsg = "";
            AccessToken = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_FetchAccessTokenFromCustID";


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAccessToken = new SqlParameter("@AccessToken", SqlDbType.VarChar, 72);
                parAccessToken.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAccessToken);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                AccessToken = parAccessToken.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();
                strLog.AppendFormat("FetchAccessTokenFromCustID:����:Result:{0},ErrMsg:{1},CustID:{2}AccessToken:{3}\r\n", SqlResult,ErrMsg,CustID, AccessToken);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "FetchAccessTokenFromCustID");
            }
            return SqlResult;
        
        }

        /// <summary>
        /// step 0����CustID��ȡUserID
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="UserID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int FetchUserIDByCustID(String CustID, out String UserID, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            ErrMsg = "";
            UserID = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_FetchUserIDByCustID";


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 72);
                parUserID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserID);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                UserID = parUserID.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();
                strLog.AppendFormat("FetchUserIDByCustID:����:UserID:{0},ErrMsg:{1}\r\n", UserID, ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "FetchUserIDByCustID");
            }
            return SqlResult;
        }

        /// <summary>
        /// step 1.����UserID�õ���¼ʱ�����accesstoken
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="AccessToken"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
       
        public static int TakeAccessTokenFromUserID(String UserID, out String AccessToken, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            ErrMsg = "";
            AccessToken = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_TakeAccessTokenFromUserID";


                SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 36);
                parUserID.Value = UserID;
                cmd.Parameters.Add(parUserID);


                SqlParameter parAccessToken = new SqlParameter("@AccessToken", SqlDbType.VarChar, 72);
                parAccessToken.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parAccessToken);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                AccessToken = parAccessToken.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();
                strLog.AppendFormat("GetUserIDByAccessToken:����:AccessToken:{0},ErrMsg:{1}\r\n", AccessToken, ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "TakeAccessTokenFromUserID");
            }
            return SqlResult;
        }

        public static int GetUserIDByAccessToken(String AccessToken, out String UserID, out String ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            ErrMsg = "";
            UserID = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_GetUserIDByAccessToken";


                SqlParameter parAccessToken = new SqlParameter("@AccessToken", SqlDbType.VarChar, 72);
                parAccessToken.Value = AccessToken;
                cmd.Parameters.Add(parAccessToken);


                SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 36);
                parUserID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parUserID);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                
                UserID = parUserID.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();

                strLog.AppendFormat("GetUserIDByAccessToken:����:UserID:{0},ErrMsg:{1}\r\n", UserID, ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg + "\r\n");
            }
            finally
            {
                logByMethod(strLog.ToString(), "GetUserIDByAccessToken");
            }
            return SqlResult;
        }

        public static int GetYangLanID(string AreaID, out string LanID, out string ErrMsg)
        {
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("CIP2BizRules:AreaID:{0}\r\n", AreaID);
            ErrMsg = "";
            LanID = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_QueryYoungLanID";


                SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 4);
                parAreaID.Value = AreaID;
                cmd.Parameters.Add(parAreaID);


                SqlParameter parLanID = new SqlParameter("@LanID", SqlDbType.VarChar, 7);
                parLanID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parLanID);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
              
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());

                LanID = parLanID.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();

                strLog.AppendFormat("up_Customer_V3_Interface_QueryYoungLanID:����:LanID:{0},ErrMsg:{1}\r\n",LanID,ErrMsg);
            }
            catch (Exception ex)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
                strLog.AppendFormat(ErrMsg+"\r\n");
            }
            finally {
                logByMethod(strLog.ToString(), "GetYangLanID");
            }
            return SqlResult;
        
        }

        /// <summary>
        /// ��֤�ֻ��Ƿ��Ѱ󶨵�ĳ���ͻ�ͷ��
        /// </summary>
        public static int IsBesttoneAccountBindV3(string CustID, out string BesttoneAccount,out string ErrMsg)
        {
            log(String.Format("custid={0}", CustID));
            ErrMsg = "";
            BesttoneAccount = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsBesttoneAccountBindV3";




                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);
                log(String.Format("custid={0}", CustID));



                SqlParameter parBesttoneAccount = new SqlParameter("@BesttoneAccount", SqlDbType.VarChar,50);
                parBesttoneAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parBesttoneAccount);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("custid={0}", CustID));
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
        /// ��֤�ֻ��Ƿ��Ѱ󶨵�ĳ���ͻ�ͷ��
        /// </summary>
        public static int IsBesttoneAccountBindV2( string CustID,out string ErrMsg)
        {
            log(String.Format("custid={0}", CustID));
            ErrMsg = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsBesttoneAccountBindV2";

  


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar,16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);
                log(String.Format("custid={0}", CustID));

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("custid={0}",CustID));
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
        /// ��֤�ֻ��Ƿ��Ѱ󶨵�ĳ���ͻ�ͷ��
        /// </summary>
        public static int IsBesttoneAccountBindV4(string CustID, out string CreateTime, out string ErrMsg)
        {
            log(String.Format("custid={0}", CustID));
            ErrMsg = "";
            CreateTime = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsBesttoneAccountBindV4";




                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);
                log(String.Format("custid={0}", CustID));


                SqlParameter parCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
                parCreateTime.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCreateTime);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("custid={0}", CustID));
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CreateTime = parCreateTime.Value.ToString();


            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        }




        /// <summary>
        /// ��֤�ֻ��Ƿ��Ѱ󶨵�ĳ���ͻ�ͷ��
        /// </summary>
        public static int IsBesttoneAccountBindV5(string CustID, out string Phone,out string CreateTime, out string ErrMsg)
        {
            log(String.Format("custid={0}", CustID));
            ErrMsg = "";
            CreateTime = "";
            Phone = "";
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsBesttoneAccountBindV5";
                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);
                log(String.Format("custid={0}", CustID));

                SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar,11);   //18918790558
                parPhone.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPhone);

                SqlParameter parCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
                parCreateTime.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCreateTime);

                SqlParameter parSqlResult = new SqlParameter("@Result", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);
                log(String.Format("custid={0}", CustID));
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CreateTime = parCreateTime.Value.ToString();
                Phone = parPhone.Value.ToString();

            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        }


        public static int QueryBestPayAllTxn(string besttoneAccount, string txnType, string txnChannel, int maxReturnRecord, int startRecord, out IList<TxnItem> AllTxnList, out string ErrMsg)
        {

            int QueryBestPayAllTxnResult = 0;

            AllTxnList = new List<TxnItem>();
            
            //����
            IList<TxnItem> CurrentDayTxnList = new List<TxnItem>();
            Int32 CurrentResult = BesttoneAccountHelper.QueryAllTypeTxn(besttoneAccount, txnType, txnChannel, out CurrentDayTxnList, out ErrMsg);


            //��ʷ  3����ǰ
            IList<TxnItem> HistoryDayTxnList = new List<TxnItem>();
            DateTime today = DateTime.Now;
            DateTime lastday = today.AddDays(-90);

            DateTime startDate = Convert.ToDateTime(lastday.ToString("yyyy-MM-dd"));
            DateTime endDate = Convert.ToDateTime(today.ToString("yyyy-MM-dd"));

            Int32 HistoryResult = BesttoneAccountHelper.QueryHistoryTxn(startDate, endDate, besttoneAccount, txnType, txnChannel, maxReturnRecord, startRecord, out HistoryDayTxnList, out ErrMsg);

            if (CurrentResult==0)
            {
                if (CurrentDayTxnList != null && CurrentDayTxnList.Count > 0)
                {
                    foreach (TxnItem ti in CurrentDayTxnList)
                    {
                        AllTxnList.Add(ti);
                    }
                }
            }else
            {
                QueryBestPayAllTxnResult = -1;
            }

            if(HistoryResult==0)
            {
                if (HistoryDayTxnList != null && HistoryDayTxnList.Count > 0)
                {
                    foreach (TxnItem ti in HistoryDayTxnList)
                    {
                        AllTxnList.Add(ti);
                    }
                }


            }else
            {
                QueryBestPayAllTxnResult = -1;
            }

            return QueryBestPayAllTxnResult;
        }




        public static int BindingBestpayAccount2BesttoneAccount(String SourceID,String CustID,String BestPayAccount,out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            string ProvinceID = "";
            string AreaID = "";
            string OuterID = "";
            string Status = "";
            string CustType = "";
            string CustLevel = "";
            string RealName = "";
            string UserName = "";
            string NickName = "";
            string CertificateCode = "";
            string CertificateType = "";
            string Sex = "";
            string Email = "";
            string EnterpriseID = "";
            string Registration = "";


            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                //ȡ�ÿͻ�������Ϣ
                Result = CustBasicInfo.getCustInfoByCustId(strSPID, CustID, out ErrMsg, out OuterID, out Status, out CustType,
                                          out CustLevel, out RealName, out UserName, out NickName, out CertificateCode,
                                          out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID, out AreaID, out Registration);

                if (Result != 0)
                {
                    ErrMsg = ErrMsg + "ȡ�ÿͻ�������Ϣ����,�޴˿ͻ�";
                    return Result;
                }
                else
                {
                    ErrMsg = "";
                }

                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_BindingBestpayAccount2BesttoneAccount";


                SqlParameter parSourceID = new SqlParameter("@SourceID", SqlDbType.VarChar, 2);
                parSourceID.Value = SourceID;
                cmd.Parameters.Add(parSourceID);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parBestPayAccount = new SqlParameter("@BestPayAccount", SqlDbType.VarChar, 16);
                parBestPayAccount.Value = BestPayAccount;
                cmd.Parameters.Add(parBestPayAccount);

               
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
            finally
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("��֧���˺Ű󶨺Űٿͻ��˺�" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";CustID - " + CustID);
                msg.Append(";BestPayAccount - " + BestPayAccount);
                
                msg.Append("������ - " + Result);
                msg.Append("; �������� - " + ErrMsg);
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("BindingBestpayAccount2BesttoneAccount", msg);
                #endregion

                //д���ݿ���־
                try
                {  
                    CommonBizRules.WriteDataLog("", CustID, BestPayAccount, Result, ErrMsg, "", "BindingBestpayAccount2BesttoneAccount");
                }
                catch { }
            }

            return Result;





        }


        /// <summary>
        /// ���ߣ����ͼ      ʱ�䣺2013-01-24
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="RedirectUrl"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int GetRedirectUrlByProvince(String ProvinceID,out String RedirectUrl,out String ErrMsg)
        {
            RedirectUrl = "";
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_GetRedirectUrlByProvince";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parRedirectUrl = new SqlParameter("@RedirectUrl", SqlDbType.VarChar, 500);
                parRedirectUrl.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parRedirectUrl);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                RedirectUrl = parRedirectUrl.Value.ToString();
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }
            finally
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("����ʡ��Ż�ȡ���ص�ַ��" + DateTime.Now.ToString("u") + "\r\n");
         
                msg.Append(";ProvinceID - " + ProvinceID + "\r\n");
                msg.Append(";RedirectUrl - " + RedirectUrl + "\r\n");
                msg.Append("������ - " + Result);
                msg.Append("; �������� - " + ErrMsg);
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("GetRedirectUrlByProvince", msg);
                #endregion
            }
            return Result;
        }


        /// <summary>
        /// ����֧�����в�������
        /// ���ߣ����ͼ   ʱ�䣺2012-8-15
        /// �޸ģ�
        /// </summary>
        public static int InsertBestPayNotify(String MerchantID, String OrderSeq, String TransDate, String AuthCode, String Account, String UserAccount, String RtnCode, String Mac, out String ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_InsertBestPayNotify";

                SqlParameter parMerchantID = new SqlParameter("@MerchantID", SqlDbType.VarChar, 30);
                parMerchantID.Value = MerchantID;
                cmd.Parameters.Add(parMerchantID);

                SqlParameter parOrderSeq = new SqlParameter("@OrderSeq", SqlDbType.VarChar, 30);
                parOrderSeq.Value = OrderSeq;
                cmd.Parameters.Add(parOrderSeq);

                SqlParameter parTransDate = new SqlParameter("@TransDate", SqlDbType.Date, 8);
                parTransDate.Value = TransDate;
                cmd.Parameters.Add(parTransDate);

                SqlParameter parAuthCode = new SqlParameter("@AuthCode", SqlDbType.VarChar,16);
                parAuthCode.Value = AuthCode;
                cmd.Parameters.Add(parAuthCode);

                SqlParameter parAccount = new SqlParameter("@Account", SqlDbType.VarChar, 32);
                parAccount.Value = Account;
                cmd.Parameters.Add(parAccount);


                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 128);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);


                SqlParameter parRtnCode = new SqlParameter("@RtnCode", SqlDbType.VarChar, 4);
                parRtnCode.Value = RtnCode;
                cmd.Parameters.Add(parRtnCode);


                SqlParameter parMac = new SqlParameter("@Mac", SqlDbType.VarChar, 32);
                parMac.Value = Mac;
                cmd.Parameters.Add(parMac);


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
            finally
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("��Ʒ�̳�ͬ��ԭʼ���в�������" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";MerchantID - " + MerchantID);
                msg.Append(";OrderSeq - " + OrderSeq);
                msg.Append(";TransDate - " + TransDate);
                msg.Append(";AuthCode - " + AuthCode);
                msg.Append(";Account - " + Account + "\r\n");
                msg.Append(";UserAccount - " + UserAccount + "\r\n");
                msg.Append(";RtnCode - " + RtnCode + "\r\n");
                msg.Append(";Mac - " + Mac + "\r\n");
                msg.Append("������ - " + Result);
                msg.Append("; �������� - " + ErrMsg);
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("InsertBestPayNotify", msg);
                #endregion

                //д���ݿ���־
                try
                {
                    CommonBizRules.WriteDataLog("", Account, UserAccount, Result, ErrMsg, "", "InsertBestPayNotify");  // ���￼�����һ����
                }
                catch { }
            }

            return Result;



        }

        /// <summary>
        /// �ͻ���Ϣ��ά�޸���Ϣ��ͬ���� 
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="CustID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int NotifyBesttoneAccountInfo(string SPID,string CustID,out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("SPID:{0},CustID:{1}\r\n",SPID,CustID);
            string flag = "1";
            try
            {

                string ProductNo = "";
                string CreateTime = "";
                // ����custidȥ�󶨱��л��productno
                Result = IsBesttoneAccountBindV5(CustID, out ProductNo, out CreateTime, out ErrMsg);
                strLog.AppendFormat("�󶨹�ϵ��ѯ-Result:{0},ProductNo:{1},CreateTime:{2},ErrMsg:{3}\r\n", Result, ProductNo, CreateTime, ErrMsg);
                CustInfo custInfo = new CustInfo();
                Result = BesttoneAccountHelper.QueryCustInfo(ProductNo, out custInfo, out ErrMsg);
                strLog.AppendFormat("�˻���Ϣ��ѯ-Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
                string ErrorDescription = "";
                string OuterID = "";
                string Status = "";
                string CustType = "";
                string CustLevel = "";
                string RealName = "";
                string UserName = "";
                string NickName = "";
                string CertificateCode = "";
                string CertificateType = "";
                string Sex = "";
                string Email = "";
                string EnterpriseID = "";
                string ProvinceID = "";
                string AreaID = "";
                string Registration = "";
                Result = CustBasicInfo.getCustInfo(SPID, CustID, out ErrorDescription, out OuterID, out Status, out CustType,
                              out CustLevel, out RealName, out UserName, out NickName, out CertificateCode,
                              out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID, out AreaID, out Registration);
                strLog.AppendFormat("�ͻ���Ϣ��ѯ-Result:{0},ErrMsg:{1},Status:{2},CertificateType:{3},CertificateCode:{4}\r\n", Result, ErrorDescription, Status, CertificateType, CertificateCode);
                if (Result == 0)
                {
                    strLog.AppendFormat("RealName:{0},CustomerName:{1},CertificateType:{2},CertificateCode:{3},idType:{4},idNo:{5}\r\n", RealName, custInfo.CustomerName, CertificateType, CertificateCode, custInfo.IdType, custInfo.IdNo);
                    if (!String.IsNullOrEmpty(RealName))
                    {
                        if (!RealName.Equals(custInfo.CustomerName))
                        {
                            custInfo.CustomerName = RealName;
                            flag = "0";
                        }
                    }

                    if (!String.IsNullOrEmpty(CertificateType) && "0".Equals(CertificateType))
                    {
                        if (!String.IsNullOrEmpty(CertificateCode))
                        {
                            if (!CertificateCode.Equals(custInfo.IdNo))
                            {
                                custInfo.IdNo = CertificateCode;
                                flag = "0";
                            }
                        }
                    }
                    strLog.AppendFormat("flag:{0}\r\n", flag);
                    if ("0".Equals(flag))
                    {
                        BesttoneAccountHelper.NotifyBesttoneAccountInfo(ProductNo, custInfo.CustomerName, "1", custInfo.IdNo, ProductNo + "@189.cn", out ErrMsg);
                    }
                }
                else
                {
                    strLog.AppendFormat("�ͻ���Ϣ��ѯʧ��:Result:{0},ErrMst:{1}\r\n", Result, ErrMsg);
                }

            }
            catch (System.Exception e)
            {
                strLog.AppendFormat(e.ToString());
            }
            finally {
                logByMethod(strLog.ToString(), "NotifyBesttoneAccountInfo");
            }


            return Result;
        }

        /// <summary>
        /// �ۺ�ƽ̨��֤�ӿ�
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="account"></param>
        /// <param name="accessToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int LoginUnifyPlatform(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, String password, out UnifyAccountInfo account, out String accessToken, out String loginNum,out long expiresIn,out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            account = new UnifyAccountInfo();
            accessToken = String.Empty;
            loginNum = String.Empty;
            expiresIn = 0;
            int _result = -1;
            String msg = String.Empty;
            UDBMBOSS unifyPlatformService = new UDBMBOSS();
            try
            {
                unifyPlatformService.UnifyPlatformUserAuth(appId, appSecret, version, clientType, clientIp, clientAgent, userName, password, out  account, out  accessToken, out loginNum,out  expiresIn, out  _result, out  msg);
                ErrMsg = msg;
            }
            catch (Exception e)
            {
                msg = e.ToString();
                ErrMsg = msg;
            }
            Result = _result;
            return Result;


        }


        public static int GetUnifyPlatformUserInfoByName(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, out String userId, out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            userId = "";
            ErrMsg = "";
            String msg = String.Empty;
            int _result = -1;
            try
            {
                UDBMBOSS unifyPlatformService = new UDBMBOSS();
                unifyPlatformService.GetUserInfoByName(appId, appSecret, version, clientType, clientIp, clientAgent, userName, out _result, out userId, out msg);
                ErrMsg = msg;
            }
            catch (Exception e)
            {
                msg = e.ToString();
                ErrMsg = msg;
            }
            Result = _result;
            return Result;
        }

        /// <summary>
        /// ע�ᵽ�ۺ�ƽ̨
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="sendSms"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int RegisterUnifyPlatformAccount(String appId,String appSecret,String version,String clientType,String clientIp,String clientAgent,
                String userName,String password,String sendSms,out String userId,out String o_userName,out String accessToken,out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            userId = String.Empty;
            o_userName = String.Empty;
            accessToken = String.Empty;
            long expiresIn = 0;
            int _result = -1;
            String msg = String.Empty;
            UDBMBOSS unifyPlatformService = new UDBMBOSS();
            try
            {
                unifyPlatformService.UnifyPlatformRegisterAccount(appId, appSecret, version, clientType, clientIp, clientAgent,
                                                                    userName, password, sendSms, out userId, out o_userName, out accessToken,
                                                                    out expiresIn, out _result, out msg);
                ErrMsg = msg;
            }
            catch (Exception e)
            {
                msg = e.ToString();
                ErrMsg = msg;
            }
            Result = _result;
            return Result;
        }

        public static bool ValidateUserName(string UserName, string Regular)
        {
            if (null == UserName)
            {
                return false;
            }
            if (UserName.IndexOf(" ") >= 0)
            {
                return false;
            }

            if (UserName.Length < 5 || UserName.Length > 25)
            {
                return false;
            }

            return Regex.IsMatch(UserName, Regular);
        }



        public static int BindUserIDCustID(String SourceSPID, String OperType, String UserID, String CustID, out String ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_BindUserIDCustID";

                SqlParameter parSourceSPID = new SqlParameter("@SourceSPID", SqlDbType.VarChar, 8);
                parSourceSPID.Value = SourceSPID;
                cmd.Parameters.Add(parSourceSPID);

                SqlParameter parOperType = new SqlParameter("@OperType", SqlDbType.VarChar, 8);
                parOperType.Value = OperType;
                cmd.Parameters.Add(parOperType);

                SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 30);
                parUserID.Value = UserID;
                cmd.Parameters.Add(parUserID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

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
            finally
            {

                try
                {
                    CommonBizRules.WriteDataLog("", CustID, "", Result, ErrMsg, "", "BindUserIDCustID");
                }
                catch { }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int GetUnifyPlatformUserInfoByName(String appId,String appSecret,String version,String clientType,String clientIp,String clientAgent,String userName,out long userId,out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            userId = 0;
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("���ղ���,userName:{0}\r\n",userName);
            try
            {

                #region
                string paras = String.Empty;
                string sign = String.Empty;
                string format = "json";
                string parameters = "userName=" + userName + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strLog.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strLog.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strLog.AppendFormat("sign:={0}\r\n", sign);
                System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                System.Net.WebClient webclient = new System.Net.WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//��ȡPOST��ʽ����ӵ�header�������ΪGET��ʽ�Ļ���ȥ����仰����  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformGetUserInfoByNameUrl, "POST", postData);
                string jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strLog.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

                String strUserId = String.Empty;
                String strResult = String.Empty;
                result_dic.TryGetValue("userId", out strUserId);
                result_dic.TryGetValue("msg", out ErrMsg);
                result_dic.TryGetValue("result", out strResult);

                if (!String.IsNullOrEmpty(strResult))
                {
                    Result = Convert.ToInt16(strResult);
                }

                if (!String.IsNullOrEmpty(strUserId))
                {
                    try
                    {
                        userId = System.Int64.Parse(strUserId);
                    }
                    catch (Exception e1)
                    {
                        strLog.AppendFormat("useridת���쳣:{0}\r\n",e1.Message);
                    }
                }
            }
            catch (Exception e)
            {
                ErrMsg = e.Message;
                strLog.AppendFormat("�쳣:{0}\r\n",e.Message);
            }
            finally
            {
                BTUCenterInterfaceLog.CenterForBizTourLog("GetUnifyPlatformUserInfoByName", strLog);
            }

            return Result;
        }


        public static int BindCustId2UserId(String SourceSPID, String OperType, String CustID, long UserID, out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("���ղ���:SPID:{0},OperType:{1},CustID:{2},UserID:{3}\r\n",SourceSPID,OperType,CustID,UserID);
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_BindCustId2UserId";

                SqlParameter parSourceSPID = new SqlParameter("@SourceSPID", SqlDbType.VarChar, 8);
                parSourceSPID.Value = SourceSPID;
                cmd.Parameters.Add(parSourceSPID);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOperType = new SqlParameter("@OperType", SqlDbType.VarChar, 2);
                parOperType.Value = OperType;
                cmd.Parameters.Add(parOperType);

                try
                {
                    SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.BigInt);
                    parUserID.Value = UserID;
                    cmd.Parameters.Add(parUserID);
                }
                catch (Exception e9)
                {
                    strLog.AppendFormat("e9:{0}\r\n", e9.ToString());
                }

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
                strLog.AppendFormat("�쳣:{0}\r\n", e.ToString());
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;

            }
            finally
            {
                BTUCenterInterfaceLog.CenterForBizTourLog("BindCustId2UserId", strLog);
                try
                {
                    CommonBizRules.WriteDataLog("", CustID, "", Result, ErrMsg, "", "BindCustId2UserId");
                }
                catch { }
            }

            return Result;
        }

        /// <summary>
        /// ע�ᵽ�ۺ�ƽ̨�󣬽���userid��custid�İ󶨹�ϵ ,��¼��������
        /// ���ߣ����ͼ      ʱ�䣺2014-06-12
        /// �޸ģ�
        /// </summary>
        public static int BindCustInfoUnifyPlatform(string ProvinceID,string AreaID,string MobileName,
            string EmailName,string RealName , string WebPwd , Int64 UserID,string SourceSPID ,string OperType,out string CustID,  out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            CustID = String.Empty;
            StringBuilder strLog = new StringBuilder();
            strLog.AppendFormat("ProvinceID:{0},AreaID:{1},MobileName:{2},EmailName:{3},RealName:{4},WebPwd:{5},UserID:{6},SourceSPID:{7},OperType:{8}\r\n",ProvinceID,AreaID,MobileName,EmailName,RealName,WebPwd,UserID,SourceSPID,OperType);
                
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            //strLog.AppendFormat("");
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_BindCustInfoUnifyPlatformV3";

                try
                {
                    SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                    parProvinceID.Value = ProvinceID;
                    cmd.Parameters.Add(parProvinceID);
                }
                catch (Exception e1)
                {
                    strLog.AppendFormat("e1:{0}\r\n", e1.ToString());
                }


                try
                {
                    SqlParameter parAreaID = new SqlParameter("@AreaID", SqlDbType.VarChar, 4);
                    parAreaID.Value = AreaID;
                    cmd.Parameters.Add(parAreaID);

                }
                catch (Exception e2)
                {
                    strLog.AppendFormat("e2:{0}\r\n", e2.ToString());
                }

                try
                {
                    SqlParameter parMobileName = new SqlParameter("@MobileName", SqlDbType.VarChar, 50);
                    parMobileName.Value = MobileName;
                    cmd.Parameters.Add(parMobileName);
                }
                catch (Exception e3)
                {
                    strLog.AppendFormat("e3:{0}\r\n", e3.ToString());
                }


                try
                {
                    SqlParameter parEmailName = new SqlParameter("@EmailName", SqlDbType.VarChar, 50);
                    parEmailName.Value = EmailName;
                    cmd.Parameters.Add(parEmailName);
                }
                catch (Exception e4)
                {
                    strLog.AppendFormat("e4:{0}\r\n", e4.ToString());
                }

                try
                {
                    SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 50);
                    parRealName.Value = RealName;
                    cmd.Parameters.Add(parRealName);
                }
                catch (Exception e5)
                {
                    strLog.AppendFormat("e5:{0}\r\n", e5.ToString());
                }

                try
                {
                    SqlParameter parWebPwd = new SqlParameter("@WebPwd", SqlDbType.VarChar, 50);
                    parWebPwd.Value = WebPwd;
                    cmd.Parameters.Add(parWebPwd);

                }
                catch (Exception e6)
                {
                    strLog.AppendFormat("e6:{0}\r\n", e6.ToString());
                }

                try
                {
                    SqlParameter parUserID = new SqlParameter("@current_receive_UserID", SqlDbType.BigInt);
                    parUserID.Value = UserID;
                    cmd.Parameters.Add(parUserID);
                }
                catch (Exception e9)
                {
                    strLog.AppendFormat("e9:{0}\r\n", e9.ToString());
                }


                try
                {
                    SqlParameter parSourceSPID = new SqlParameter("@SourceSPID", SqlDbType.VarChar, 8);
                    parSourceSPID.Value = SourceSPID;
                    cmd.Parameters.Add(parSourceSPID);

                }
                catch (Exception e7)
                {
                    strLog.AppendFormat("e7:{0}\r\n", e7.ToString());
                }

                try
                {
                    SqlParameter parOperType = new SqlParameter("@OperType", SqlDbType.VarChar, 2);
                    parOperType.Value = OperType;
                    cmd.Parameters.Add(parOperType);
                }
                catch (Exception e8)
                {
                    strLog.AppendFormat("e8:{0}\r\n", e8.ToString());
                }


                SqlParameter parCustID = null;
                try
                {
                    parCustID = new SqlParameter("@output_CustID", SqlDbType.VarChar, 16);
                    parCustID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parCustID);

                }
                catch (Exception e10)
                {
                    strLog.AppendFormat("e10:{0}\r\n", e10.ToString());
                }

                SqlParameter parResult = null;
                try
                {
                    parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                }
                catch (Exception e11)
                {
                    strLog.AppendFormat("e11:{0}\r\n",e11.ToString());                  
                }
                SqlParameter parErrMsg = null;
                try
                {
                    parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);
                }
                catch (Exception e12)
                {
                    strLog.AppendFormat("e12:{0}\r\n",e12.ToString());
                }

                try
                {
                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                }
                catch (Exception e13)
                {
                    strLog.AppendFormat("e13:{0}\r\n",e13.ToString());
                }

                try
                {
                    strLog.AppendFormat("parCustID:{0}\r\n", parCustID);
                    if (parCustID != null)
                    {
                        CustID = parCustID.Value.ToString();
                    }
                }
                catch (Exception e14)
                {

                    strLog.AppendFormat("e14:{0}\r\n", e14.ToString());
                }
                
                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }
            catch (Exception e)
            {
                strLog
                    .AppendFormat("�쳣:{0}\r\n",e.ToString());
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }
            finally
            {
                BTUCenterInterfaceLog.CenterForBizTourLog("BindCustInfoUnifyPlatform", strLog);
                try
                {
                    CommonBizRules.WriteDataLog("", CustID, "", Result, ErrMsg, "", "BindCustInfoUnifyPlatform");
                }
                catch { }
            }

            return Result;
        }

        public static int UpdateUnifyPassWord(String appId,String appSecret,String version,String clientType,String AccessToken, String PassWord, String nPassWord, String ClientIp, String ClientAgent,out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            UDBMBOSS udbBoss = new UDBMBOSS();
            try
            {
            
                Result = udbBoss.UnifyPlatformUpdatePwd(appId, appSecret, version, clientType, AccessToken, PassWord, nPassWord, ClientIp, ClientAgent, out ErrMsg);
    
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_System_UnknowError_Msg + "," + e.Message;
            }
            return Result;
        }

        /// <summary>
        /// ��ͻ���Ϣͬ��ԭʼ���в�������
        /// ���ߣ�������      ʱ�䣺2009-8-15
        /// �޸ģ�
        /// </summary>
        public static int InsertCustInfoNotify(string CustID, string OPType, string ToSPID, string paymentPwd, string DealType, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            paymentPwd = CryptographyUtil.Encrypt(paymentPwd);

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_InsertCustInfoNotify";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOPType = new SqlParameter("@OPType", SqlDbType.VarChar, 1);
                parOPType.Value = OPType;
                cmd.Parameters.Add(parOPType);

                SqlParameter parToSPID = new SqlParameter("@ToSPID", SqlDbType.VarChar, 8);
                parToSPID.Value = ToSPID;
                cmd.Parameters.Add(parToSPID);

                SqlParameter parDealType = new SqlParameter("@DealType", SqlDbType.Int);
                parDealType.Value = DealType;
                cmd.Parameters.Add(parDealType);

                SqlParameter parpaymentPwd = new SqlParameter("@paymentPwd", SqlDbType.VarChar, 50);
                parpaymentPwd.Value = paymentPwd;
                cmd.Parameters.Add(parpaymentPwd);

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
            finally
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append("��ͻ���Ϣͬ��ԭʼ���в�������" + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";CustID - " + CustID);
                msg.Append(";OPType - " + OPType);
                msg.Append(";ToSPID - " + ToSPID);
                msg.Append(";paymentPwd - " + paymentPwd);
                msg.Append(";DealType - " + DealType + "\r\n");
                msg.Append("������ - " + Result);
                msg.Append("; �������� - " + ErrMsg);
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("InsertCustInfoNotify", msg);
                #endregion

                //д���ݿ���־
                try
                {
                    CommonBizRules.WriteDataLog("", CustID, "", Result, ErrMsg, "", "InsertCustInfoNotify");
                }
                catch { }
            }

            return Result;
        }

        /// <summary>
        /// ֪ͨ����ϵͳ�û���Ϣ
        /// ���ߣ�������      ʱ�䣺2009-8-15
        /// �޸ģ�
        /// </summary>
        public static void newCardCustomerInfoExport(string SequenceID, string CustID, string DealType, string PaymentPwd, string OPType, out string ErrMsg, string ExtendField)
        {
            StringBuilder myStringBuilder = new StringBuilder();
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            int OtherResult = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            myStringBuilder.Append("*++++++++++++++++++++++++++++++++++++" + ErrMsg + "\r\n");
            string OtherErrMsg = "";
            string ProvinceID = "";
            string AreaID = "";
            string OuterID = "";
            string Status = "";
            string CustType = ""; 
            string CardType = "";
            string CustLevel = "";
            string RealName = "";
            string UserName = "";
            string NickName = "";
            string CertificateCode = "";
            string CertificateType = "";
            string Sex = "";
            string Email = "";
            string EnterpriseID = "";
            string Registration = "";
            PhoneRecord[] PhoneRecords = null;
            string Phone = "";
            TourCardIDRecord[] TourCardIDRecords = null;
            CustAuthenInfoRecord[] CustAuthenInfoRecords = null;
            string Birthday = "";
            string EduLevel = "";
            string Favorite = "";
            string IncomeLevel = "";
            string UserAccount = "";
            
            myStringBuilder.Append("*++++++++++++++++++++++++++++++++++++***************************" + "\r\n");
            DateTime startTime = DateTime.Now;
            try
            {
                #region ȡ�ÿͻ���Ϣ
                try
                {
                    //ȡ�ÿͻ�������Ϣ
                    Result = CustBasicInfo.getCustInfo(strSPID, CustID, out ErrMsg, out OuterID, out Status, out CustType,
                                              out CustLevel, out RealName, out UserName, out NickName, out CertificateCode,
                                              out CertificateType, out Sex, out Email,out EnterpriseID, out ProvinceID, out AreaID, out Registration);

                    if (Result != 0)
                    {
                        ErrMsg = ErrMsg + "ȡ�ÿͻ�������Ϣ����";
                        return;
                    }
                    else
                    {
                        ErrMsg = "";
                    }

                    //ȡ�ÿͻ���֤�绰
                    PhoneRecords = CustBasicInfo.getPhoneRecord(CustID, out OtherResult, out OtherErrMsg);
                    
                    if (PhoneRecords != null && PhoneRecords.Length != 0 && OtherResult == 0)
                    {
                        Phone = PhoneRecords[0].Phone.ToString();
                        OtherErrMsg = "";
                    }
                    else
                    {
                        Phone = "";
                        OtherErrMsg = OtherErrMsg + "ȡ�ÿͻ���֤�绰����";                        
                    }

                    //ȡ�ÿͻ�����
                    //1. ���CUSTLEVEL  ����ڵ� ʡ���ſͻ�ͳһ�� 1  ���˿ͻ��� 
                    //���ɣ� ��ֵĿǰ�Ի���ƽ̨�����壬ֻ�Ǳ�֤����ƽ̨������У�鲻����
                    //2. ʡ���ſͻ��Ŀ��Ŵ��ա� 
                    //���ɣ� ʡ���ſͻ��Ŀ��Ŷ� ����ƽ̨�����壬 ���� ��Щʡ���ſͻ���������û�п��ŵ�

                    if (CustType == "41" || CustType == "42")
                    {
                        TourCardIDRecords = CustBasicInfo.getTourCardIDRecord(CustID, out OtherResult, out OtherErrMsg);
                        
                        if (TourCardIDRecords != null && TourCardIDRecords.Length != 0 && OtherResult == 0)
                        {
                            UserAccount = TourCardIDRecords[0].InnerCardID.ToString();
                            CardType = TourCardIDRecords[0].CardType.ToString();
                            OtherErrMsg = "";
                        }
                        else
                        {
                            UserAccount = "";
                            CardType = "1";
                        }
                    }
                    else
                    {
                        UserAccount = "";
                        CardType = "1";
                    }

                    //myStringBuilder.Append("UserAccount=" + UserAccount + "\r\n");
                    //myStringBuilder.Append("CardType=" + CardType + "\r\n");
                    //myStringBuilder.Append("CustAuthenInfoRecord=" + OtherResult + "\r\n");
                    //myStringBuilder.Append("CustAuthenInfoRecord=" + OtherErrMsg + "\r\n");

                    //ȡ��Birthday
                    OtherResult = CustExtendInfo.getCustExtendInfo(strSPID, CustID, out OtherErrMsg, out Birthday,
                                                               out EduLevel, out Favorite, out IncomeLevel);
                    
                    //myStringBuilder.Append("CustExtendInfoResult=" + OtherResult + "\r\n");
                    //myStringBuilder.Append("CustExtendInfoResult=" + OtherErrMsg + "\r\n");
                    //myStringBuilder.Append("Birthday=" + Birthday + "\r\n");

                    if (OtherResult != 0)
                    {
                        OtherErrMsg = OtherErrMsg + "ȡ��Birthday����";
                        Birthday = "";
                    }
                    else
                    {
                        OtherErrMsg = "";
                    }
                }
                catch (Exception e)
                {
                    Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                    ErrMsg = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
                    return;
                }
                #endregion

                #region XML
                string CustInfoXML = "";
                XmlDocument xmldoc;
                XmlNode xmlnode;
                XmlElement xmlelem;
                XmlElement xmlelem2 = null;
                XmlElement xmlelem3;
                XmlText xmltext;
                xmldoc = new XmlDocument();
                //����XML����������
                xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);
                //����һ����Ԫ��
                xmlelem = xmldoc.CreateElement("", "ROOT", "");
                xmldoc.AppendChild(xmlelem);

                //��������һ��Ԫ��
                xmlelem2 = xmldoc.CreateElement("BasicUserInfo");
                xmlelem2 = xmldoc.CreateElement("", "BasicUserInfo", "");

                xmlelem3 = xmldoc.CreateElement("", "birthday", "");
                xmltext = xmldoc.CreateTextNode(Birthday);
                xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                xmlelem3 = xmldoc.CreateElement("", "cardClass", "");
                xmltext = xmldoc.CreateTextNode(CustLevel);
                xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                xmlelem3 = xmldoc.CreateElement("", "certificateCode", "");
                xmltext = xmldoc.CreateTextNode(CertificateCode);
                xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "certificateType", "");
                xmltext = xmldoc.CreateTextNode(CertificateType);
                xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "credit", "");
                xmltext = xmldoc.CreateTextNode("");
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "custContactTel", "");
                xmltext = xmldoc.CreateTextNode(Phone);
                xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "custID", "");
                xmltext = xmldoc.CreateTextNode(CustID);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                xmlelem3 = xmldoc.CreateElement("", "OuterCustID", "");
                xmltext = xmldoc.CreateTextNode(OuterID);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                xmlelem3 = xmldoc.CreateElement("", "custType", "");
                xmltext = xmldoc.CreateTextNode(CardType);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "enterpriseID", "");
                xmltext = xmldoc.CreateTextNode("");
                xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "extendField", "");
                xmltext = xmldoc.CreateTextNode("");
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "realName", "");
                xmltext = xmldoc.CreateTextNode(RealName);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "registration", "");
                xmltext = xmldoc.CreateTextNode(Registration);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);


                xmlelem3 = xmldoc.CreateElement("", "sex", "");
                xmltext = xmldoc.CreateTextNode(Sex);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                //ͬ��������ϵͳʱ��status=02����ʶ����ע��
                xmlelem3 = xmldoc.CreateElement("", "status", "");
                xmltext = xmldoc.CreateTextNode(Status);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                xmlelem3 = xmldoc.CreateElement("", "uProvinceID", "");
                xmltext = xmldoc.CreateTextNode(ProvinceID);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                xmlelem3 = xmldoc.CreateElement("", "userAccount", "");
                xmltext = xmldoc.CreateTextNode(UserAccount);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);

                xmlelem3 = xmldoc.CreateElement("", "userType", "");
                xmltext = xmldoc.CreateTextNode(CustType);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);
                xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                xmlelem3 = xmldoc.CreateElement("", "AuthenName", "");
                xmltext = xmldoc.CreateTextNode(UserName);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);
                xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                xmlelem3 = xmldoc.CreateElement("", "AreaCode", "");
                xmltext = xmldoc.CreateTextNode(AreaID);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);
                xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                xmlelem3 = xmldoc.CreateElement("", "PaymentPwd", "");
                xmltext = xmldoc.CreateTextNode(PaymentPwd);
                xmlelem3.AppendChild(xmltext);
                xmlelem2.AppendChild(xmlelem3);
                xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

                //���洴���õ�XML�ĵ�

                // xmldoc.Save(@".\BasicUserInfo.xml");
                CustInfoXML = xmldoc.OuterXml;
                CustInfoXML = CustInfoXML.Substring(CustInfoXML.IndexOf("<ROOT>"));
                CustInfoXML = @"<?xml version='1.0' encoding='gb2312'?>" + CustInfoXML;

                //myStringBuilder.Append("*****==========**************************************************" + "\r\n");
                //myStringBuilder.Append("Birthday=" + Birthday + "\r\n");
                //myStringBuilder.Append("CustLevel=" + CustLevel + "\r\n");
                //myStringBuilder.Append("CertificateCode=" + CertificateCode + "\r\n");
                //myStringBuilder.Append("CertificateType=" + CertificateType + "\r\n");
                //myStringBuilder.Append("credit=" + "" + "\r\n");
                //myStringBuilder.Append("Phone=" + Phone + "\r\n");
                //myStringBuilder.Append("CustID=" + CustID + "\r\n");
                //myStringBuilder.Append("OuterID=" + OuterID + "\r\n");
                //myStringBuilder.Append("custType=" + CardType + "\r\n");
                //myStringBuilder.Append("enterpriseID=" + "" + "\r\n");
                //myStringBuilder.Append("extendField=" + "" + "\r\n");
                //myStringBuilder.Append("RealName=" + RealName + "\r\n");
                //myStringBuilder.Append("Registration=" + Registration + "\r\n");
                //myStringBuilder.Append("Sex=" + Sex + "\r\n");
                //myStringBuilder.Append("Status=" + Status + "\r\n");
                //myStringBuilder.Append("ProvinceID=" + ProvinceID + "\r\n");
                //myStringBuilder.Append("UserAccount=" + UserAccount + "\r\n");
                //myStringBuilder.Append("userType=" + CustType + "\r\n");
                //myStringBuilder.Append("UserName=" + UserName + "\r\n");
                //myStringBuilder.Append("AreaID=" + AreaID + "\r\n");
                //myStringBuilder.Append("PaymentPwd=" + PaymentPwd + "\r\n");
                #endregion

                NewCardCustomerInfoService obj = new NewCardCustomerInfoService();
                obj.Url = ConfigurationManager.AppSettings["JFUrl"];
                string SPID = ConfigurationManager.AppSettings["BesttoneSPID"];
                string TimeStamp = DateTime.Now.ToString();

                //myStringBuilder.Append("********=========*****************************************" + "\r\n");
                //myStringBuilder.Append("JFUrl=" + obj.Url + "\r\n");
                //myStringBuilder.Append("ProvinceID=" + ProvinceID + "\r\n");
                //myStringBuilder.Append("SPID=" + SPID + "\r\n");
                //myStringBuilder.Append("TimeStamp=" + TimeStamp + "\r\n");
                //myStringBuilder.Append("CustInfoXML=" + CustInfoXML + "\r\n");
                //myStringBuilder.Append("DealType=" + DealType + "\r\n");
                string starttime = DateTime.Now.ToString();

                string strResult = obj.NewCardCustomerInfoExport(ProvinceID, SPID, TimeStamp, CustInfoXML, DealType);

                string endtime = DateTime.Now.ToString();

                XmlDocument xmlObj = new XmlDocument();
                xmlObj.LoadXml(strResult);
                Result = int.Parse(xmlObj.GetElementsByTagName("result")[0].InnerText);
                ErrMsg = xmlObj.GetElementsByTagName("errorDescription")[0].InnerText;

                myStringBuilder.Append("************************************************************" + "\r\n");
                myStringBuilder.Append("starttime=" + starttime + "\r\n");
                myStringBuilder.Append("endtime=" + endtime + "\r\n");
                myStringBuilder.Append("Result=" + Result + "\r\n");
                myStringBuilder.Append("ErrMsg=" + ErrMsg + "\r\n");
            }
            catch (Exception ex)
            {
                ErrMsg = "1," + ex.Message;
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
            }
            finally
            {
                myStringBuilder.Append("************************************************************" + "\r\n");
                myStringBuilder.Append("SequenceID=" + SequenceID + "\r\n");
                myStringBuilder.Append("CustID=" + CustID + "\r\n");
                myStringBuilder.Append("ProvinceID=" + ProvinceID + "\r\n");
                myStringBuilder.Append("CustType=" + CustType + "\r\n");
                myStringBuilder.Append("OPType=" + OPType + "\r\n");
                myStringBuilder.Append("OuterID=" + OuterID + "\r\n");
                myStringBuilder.Append("DealType=" + DealType + "\r\n");
                myStringBuilder.Append("PaymentPwd=" + PaymentPwd + "\r\n");
                myStringBuilder.Append("Result=" + Result + "\r\n");
                myStringBuilder.Append("ErrMsg=" + ErrMsg + "\r\n");
                myStringBuilder.Append("OtherErrMsg=" + OtherErrMsg + "\r\n");                
                BTUCenterInterfaceLog.CenterForBizTourLog("newCardCustomerInfoExport", myStringBuilder);
                string ToSPID = ConfigurationManager.AppSettings["ScoreBesttoneSPID"];

                //����ԭʼ��,������ʷ��
                CIP2BizRules.UpdateCustInfoNotify(SequenceID, CustID, ProvinceID, CustType, OPType, ToSPID, Convert.ToInt32(DealType), PaymentPwd, Result, ErrMsg);                
            }
        }

        /// <summary>
        /// ����ԭʼ��,������ʷ��
        /// ���ߣ�������      ʱ�䣺2009-8-15
        /// �޸ģ�
        /// </summary>
        private static void UpdateCustInfoNotify(string SequenceID, string CustID, string ProvinceID, string CustType, string OPType,
            string ToSPID, int DealType, string PaymentPwd, int Result, string Description)
        {
            PaymentPwd = CryptographyUtil.Encrypt(PaymentPwd);

            SqlCommand sqlCmd = new SqlCommand("up_Customer_V3_Interface_UpdateCustInfoNotify");
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@SequenceID", SqlDbType.BigInt);
            sqlCmd.Parameters.Add("@CustID", SqlDbType.VarChar, 16);
            sqlCmd.Parameters.Add("@ProvinceID", SqlDbType.VarChar, 2);
            sqlCmd.Parameters.Add("@CustType", SqlDbType.VarChar, 2);
            sqlCmd.Parameters.Add("@OPType", SqlDbType.VarChar, 1);
            sqlCmd.Parameters.Add("@ToSPID", SqlDbType.VarChar, 8);
            sqlCmd.Parameters.Add("@DealType", SqlDbType.Int);
            sqlCmd.Parameters.Add("@PaymentPwd", SqlDbType.VarChar, 50);
            sqlCmd.Parameters.Add("@Result", SqlDbType.Int);
            sqlCmd.Parameters.Add("@Description", SqlDbType.VarChar, 256);

            sqlCmd.Parameters["@SequenceID"].Value = SequenceID;
            sqlCmd.Parameters["@CustID"].Value = CommonBizRules.DealData(CustID);
            sqlCmd.Parameters["@ProvinceID"].Value = CommonBizRules.DealData(ProvinceID);
            sqlCmd.Parameters["@CustType"].Value = CommonBizRules.DealData(CustType);
            sqlCmd.Parameters["@OPType"].Value = CommonBizRules.DealData(OPType);
            sqlCmd.Parameters["@ToSPID"].Value = CommonBizRules.DealData(ToSPID);
            sqlCmd.Parameters["@DealType"].Value = DealType;
            sqlCmd.Parameters["@PaymentPwd"].Value = CommonBizRules.DealData(PaymentPwd);
            sqlCmd.Parameters["@Result"].Value = Result;
            sqlCmd.Parameters["@Description"].Value = CommonBizRules.DealData(Description);

            //���ڲ��������ݿⷽ��
            DBUtility.Execute(sqlCmd, DBUtility.BestToneCenterConStr);
        }


        public static CustInfoNotifyRecord[] QueryCustInfoNotify()
        {
  

            DataSet CustinfoNofityData = new DataSet();
            CustInfoNotifyRecord[] CustInfoNotifyRecordArray = new CustInfoNotifyRecord[0];
            CustinfoNofityData = QueryCustInfoNotifyData();
            List<CustInfoNotifyRecord> custInfoList = new List<CustInfoNotifyRecord>();
            if(CustinfoNofityData!=null){
	            if(CustinfoNofityData.Tables.Count!=0){
		            if(CustinfoNofityData.Tables[0].Rows.Count!=0){
			            int RowCount = CustinfoNofityData.Tables[0].Rows.Count;
			            CustInfoNotifyRecord cr = new CustInfoNotifyRecord();
			            for(int i=0;i<RowCount;i++){
				            cr = new CustInfoNotifyRecord();
                            cr.SequenceID = CustinfoNofityData.Tables[0].Rows[i]["SequenceID"].ToString().Trim();
                            cr.CustID = CustinfoNofityData.Tables[0].Rows[i]["CustID"].ToString().Trim();
                            cr.OPType = CustinfoNofityData.Tables[0].Rows[i]["OPType"].ToString().Trim();
                            cr.DealType = CustinfoNofityData.Tables[0].Rows[i]["DealType"].ToString().Trim();
                            cr.PaymentPwd = CustinfoNofityData.Tables[0].Rows[i]["PaymentPwd"].ToString().Trim();
                            cr.ProvinceId = CustinfoNofityData.Tables[0].Rows[i]["ProvinceId"].ToString().Trim();
                            cr.AreaId = CustinfoNofityData.Tables[0].Rows[i]["AreaId"].ToString().Trim();
                            cr.CustType = CustinfoNofityData.Tables[0].Rows[i]["CustType"].ToString().Trim();
                            cr.CertificateType = CustinfoNofityData.Tables[0].Rows[i]["CertificateType"].ToString().Trim();
                            cr.CertificateCode = CustinfoNofityData.Tables[0].Rows[i]["CertificateCode"].ToString().Trim();
                            cr.RealName = CustinfoNofityData.Tables[0].Rows[i]["RealName"].ToString().Trim();
                            cr.CustLevel = CustinfoNofityData.Tables[0].Rows[i]["CustLevel"].ToString().Trim();
                            cr.Sex = CustinfoNofityData.Tables[0].Rows[i]["Sex"].ToString().Trim();
                            cr.RegistrationSource = CustinfoNofityData.Tables[0].Rows[i]["RegistrationSource"].ToString().Trim();
                            cr.UserName = CustinfoNofityData.Tables[0].Rows[i]["UserName"].ToString().Trim();
                            cr.NickName = CustinfoNofityData.Tables[0].Rows[i]["NickName"].ToString().Trim();
                            cr.Email = CustinfoNofityData.Tables[0].Rows[i]["Email"].ToString().Trim();
                            cr.EmailClass = CustinfoNofityData.Tables[0].Rows[i]["EmailClass"].ToString().Trim();
                            cr.SourceSpid = CustinfoNofityData.Tables[0].Rows[i]["SourceSpid"].ToString().Trim();
                            cr.OuterId = CustinfoNofityData.Tables[0].Rows[i]["OuterId"].ToString().Trim();
                            cr.DealTime = CustinfoNofityData.Tables[0].Rows[i]["DealTime"].ToString().Trim();
                            cr.CreateTime = CustinfoNofityData.Tables[0].Rows[i]["CreateTime"].ToString().Trim();
				            custInfoList.Add(cr);
			            }
			            CustInfoNotifyRecordArray = custInfoList.ToArray();

		            }
	            }
            }
            return CustInfoNotifyRecordArray;
        }

        /// <summary>
        /// ȡ�ÿͻ���Ϣͬ��ʧ��ԭʼ����Ϣ
        /// </summary>
        /// <returns></returns>
        private static DataSet QueryCustInfoNotifyData()
        {
            DataSet newCardCustomerData = new DataSet();

            try
            {
                SqlCommand SqlCmd = new SqlCommand("up_Customer_V3_Interface_QueryCustInfoNotifyInfo");
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add("@NotifyCount", SqlDbType.Int);
                SqlCmd.Parameters["@NotifyCount"].Value =System.Configuration.ConfigurationManager.AppSettings["NotifyCount"];
                newCardCustomerData = DBUtility.FillData(SqlCmd, DBUtility.BestToneCenterConStr);
            }
            catch (Exception e)
            {
                throw e;
            }

            return newCardCustomerData;
        }

        public static void DeleteHasPulledFailedCustInfoNotifyData(string SequenceId)
        {
            try
            {
                SqlCommand SqlCmd = new SqlCommand("up_Customer_V3_Interface_DeleteCustInfoNotifyInfo");
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add("@SequenceId", SqlDbType.BigInt);
                SqlCmd.Parameters["@SequenceId"].Value = SequenceId;
                SqlCmd.Parameters.Add("@NotifyCount", SqlDbType.Int);
                SqlCmd.Parameters["@NotifyCount"].Value = System.Configuration.ConfigurationManager.AppSettings["NotifyCount"];
                DBUtility.Execute(SqlCmd, DBUtility.BestToneCenterConStr);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// ȡ�ÿͻ���֤��Ϣ
        /// </summary>
        private static CustAuthenInfoRecord[] getCustAuthenInfoRecord(string CustID, out int Result, out string ErrMsg)
        {
            CustAuthenInfoRecord[] CustAuthenInfoRecords = null;
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_getCustAuthenInfo";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            CustAuthenInfoRecords = new CustAuthenInfoRecord[RowCount];
                            CustAuthenInfoRecord CustAuthenInfoRecord = null;
                            for (int i = 0; i < RowCount; i++)
                            {
                                CustAuthenInfoRecord = new CustAuthenInfoRecord();
                                CustAuthenInfoRecord.AuthenName = ds.Tables[0].Rows[i]["AuthenName"].ToString().Trim();
                                CustAuthenInfoRecord.AuthenType = ds.Tables[0].Rows[i]["AuthenType"].ToString().Trim();
                                CustAuthenInfoRecords[i] = CustAuthenInfoRecord;
                            }                           

                            Result = ErrorDefinition.IError_Result_Success_Code;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "��ȡ�ͻ���֤������" + e.Message;
            }

            return CustAuthenInfoRecords;
        }

        /// <summary>
        /// д���ݿ���־
        /// </summary>
        public static void WriteQueryByPhoneDataLog(string PhoneNum, string SPID, string CustID, string CustType, string ProvinceID,  int Result, string Description)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_WriteQueryByPhoneDataLog";

            SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
            parPhoneNum.Value = (PhoneNum == null) ? "" : PhoneNum;
            cmd.Parameters.Add(parPhoneNum);

            SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
            parCustID.Value = (CustID == null) ? "" : CustID;
            cmd.Parameters.Add(parCustID);

            SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
            parProvinceID.Value = (ProvinceID == null) ? "" : ProvinceID;
            cmd.Parameters.Add(parProvinceID);

            SqlParameter parCustType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
            parCustType.Value = (CustType == null) ? "" : CustType;
            cmd.Parameters.Add(parCustType);

            SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
            parResult.Value = Result;
            cmd.Parameters.Add(parResult);

            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = (SPID == null) ? "" : SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parDescription = new SqlParameter("@Description", SqlDbType.VarChar, 256);
            parDescription.Value = (Description == null) ? "" : Description;
            cmd.Parameters.Add(parDescription);

            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

        }


    }
}

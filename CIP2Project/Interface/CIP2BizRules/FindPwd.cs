using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Collections;
namespace Linkage.BestTone.Interface.Rule
{
    public class FindPwd
    {

        /// <summary>
        /// 通过Custid和web密码查询用户信息记录是否存在
        /// 作者：赵锐
        /// 日期：2009年9月27日
        /// </summary>
        public static int SelState(string CustId, string Pwd, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_SelCustIdandPwd";


                SqlParameter parCustId = new SqlParameter("CustId", SqlDbType.VarChar, 16);
                parCustId.Value = CustId;
                cmd.Parameters.Add(parCustId);

                SqlParameter parPwd = new SqlParameter("Pwd",SqlDbType.VarChar,128);
                parPwd.Value = Pwd;
                cmd.Parameters.Add(parPwd);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);


                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

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
        /// 通过认证号码查询密码
        /// 作者：赵锐
        /// 日期：2009年8月11日
        /// </summary>
        public static List<string> SelTypeFindPassWord(int PwdType, string Phone,out string ErrMsg)
        {
            List<string> list = new List<string>();
            ErrMsg = "";
            string Num = null;
            string JmNum =null;
            string CustId = null;
            string CustType = null;
            int SqlResult = 0;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                Random random = new Random();

                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelPhoneUPPwd";

                Num = random.Next(111111, 999999).ToString();
                JmNum=CryptographyUtil.Encrypt(Num);


                SqlParameter parJmNum = new SqlParameter("Num", SqlDbType.VarChar, 20);
                parJmNum.Value = JmNum;
                cmd.Parameters.Add(parJmNum);


                SqlParameter parPwdType = new SqlParameter("PwdType", SqlDbType.Int);
                parPwdType.Value = PwdType;
                cmd.Parameters.Add(parPwdType);



                SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
                parPhone.Value = Phone;
                cmd.Parameters.Add(parPhone);

                SqlParameter parCustId = new SqlParameter("CustId", SqlDbType.VarChar, 16);
                parCustId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustId);

                SqlParameter parCustType = new SqlParameter("CustType",SqlDbType.VarChar,2);
                parCustType.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustType);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg",SqlDbType.VarChar,256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                CustId = parCustId.Value.ToString();
                CustType = parCustType.Value.ToString();
                list.Add(SqlResult.ToString());
                list.Add(CustId);
                list.Add(Num);
                list.Add(CustType);
                list.Add(ErrMsg);
            }
            catch(Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return list;
        }

        /// <summary>
        /// 通过用户输入的验证码和新密码修改用户密码
        /// </summary>
        /// <param name="Email">邮箱</param>
        /// <param name="AuthenCode">验证码</param>
        /// <param name="NewPwd">新密码</param>
        /// <returns></returns>
        public static int UpdateWebPwd(string Email, string AuthenCode, string NewPwd,out string ErrMsg)
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
                cmd.CommandText = "up_Customer_OV3_Interface_UpdateWebPWD";

                SqlParameter parEmail = new SqlParameter("Email",SqlDbType.VarChar,100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);


                SqlParameter parAuthenCode = new SqlParameter("AuthenCode",SqlDbType.VarChar,6);
                parAuthenCode.Value = AuthenCode;
                cmd.Parameters.Add(parAuthenCode);

                SqlParameter parNewPwd = new SqlParameter("NewPwd",SqlDbType.VarChar,128);
                parNewPwd.Value = NewPwd;
                cmd.Parameters.Add(parNewPwd);

                SqlParameter parSqlResult = new SqlParameter("SqlResult",SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg",SqlDbType.VarChar,256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
            }

            catch(Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return SqlResult;
        }


        /// <summary>
        /// 邮箱查询密码
        /// </summary>
        public static int SelPwdByEmailV2(string Email, out string Pwd,out string ErrMsg)
        {
            DataTable dt = new DataTable();
            ErrMsg = "";
            int SqlResult = 0;
            Pwd = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {

                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelPwdByEmailV2";

                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);


                SqlParameter parPwd = new SqlParameter("pwd", SqlDbType.VarChar, 128);
                parPwd.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPwd);


                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);


                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                Pwd = parPwd.Value.ToString();
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
        /// 用户名和邮箱查询密码
        /// </summary>
        public static string[] SelPwdByEmailandName(string Name, string Email, out string ErrMsg)
        {
            DataTable dt = new DataTable();
            string[] str = new string[2];
            ErrMsg = "";
            int SqlResult = 0;
            string CustID = "";
            string Pwd = "";
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {

                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelPwdByEmailandName";

                SqlParameter parName = new SqlParameter("Name", SqlDbType.VarChar, 30);
                parName.Value = Name;
                cmd.Parameters.Add(parName);


                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar,100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);


                SqlParameter parPwd = new SqlParameter("pwd", SqlDbType.VarChar, 128);
                parPwd.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPwd);

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);



                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                Pwd = parPwd.Value.ToString();
                CustID = parCustID.Value.ToString();
                ErrMsg = parErrMsg.Value.ToString();
                if (SqlResult==0)
                {
                    str[0] = CustID;
                    str[1] = Pwd;
                }

            }
            catch(Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return str;
        }

        /// <summary>
        /// 创建客户密码找回记录日志
        /// 作者：张英杰   时间：2009-8-25
        /// 修改：
        /// </summary>
        /// <param name="CustID">客户id</param>
        /// <param name="CustType">客户类型</param>
        /// <param name="PwdType">密码类型 	1:语音密码2:web密码</param>
        /// <param name="OPType">1：通过提示问题找回2：通过手机找回3：通过邮箱找回4：后台重置</param>
        /// <param name="AuthenNumber">找回号码</param>
        /// <param name="ResultIn">找回结果</param>
        /// <param name="SPID">发起系统</param>
        /// <param name="IPAddress">IP地址</param>
        /// <param name="Description">描述</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>int</returns>
        public static int InsertFindPwdLog(string CustID, string CustType, string PwdType, string OPType, string AuthenNumber, int ResultIn, string SPID, string IPAddress, string Description, out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_InsertPwdResetLog";
            try
            {

                SqlParameter custID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                custID.Value = CustID;
                cmd.Parameters.Add(custID);

                SqlParameter custType = new SqlParameter("@CustType", SqlDbType.VarChar, 2);
                custType.Value = CustType;
                cmd.Parameters.Add(custType);

                SqlParameter pwdType = new SqlParameter("@PwdType", SqlDbType.VarChar, 1);
                pwdType.Value = PwdType;
                cmd.Parameters.Add(pwdType);


                SqlParameter oPType = new SqlParameter("@OPType", SqlDbType.VarChar, 1);
                oPType.Value = OPType;
                cmd.Parameters.Add(oPType);


                SqlParameter authenNumber = new SqlParameter("@AuthenNumber", SqlDbType.VarChar, 100);
                authenNumber.Value = AuthenNumber;
                cmd.Parameters.Add(authenNumber);

                SqlParameter resultIn = new SqlParameter("@Result", SqlDbType.Int);
                resultIn.Value = ResultIn;
                cmd.Parameters.Add(resultIn);

                SqlParameter sPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                sPID.Value = SPID;
                cmd.Parameters.Add(sPID);

                SqlParameter iPAddress = new SqlParameter("@IPAddress", SqlDbType.VarChar, 15);
                iPAddress.Value = IPAddress;
                cmd.Parameters.Add(iPAddress);

                SqlParameter description = new SqlParameter("@Description", SqlDbType.VarChar, 40);
                description.Value = Description;
                cmd.Parameters.Add(description);

                SqlParameter parResult = new SqlParameter("@ResultOut", SqlDbType.Int);
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
        /// 【暂时无用】校验邮箱验证码
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Email">邮箱</param>
        /// <param name="AuthenCode">验证码</param>
        /// <returns></returns>
        public static int selEmailAuDel(string UserName, string Email, string AuthenCode, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_OV3_Interface_SelEmailAuDel";


                SqlParameter parUserName = new SqlParameter("UserName", SqlDbType.VarChar, 16);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

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

    }
}

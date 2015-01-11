using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using Passport.Common.Web.Mail;
namespace Linkage.BestTone.Interface.Rule
{
    public class SetMail
    {
        /// <summary>
        ///  校验邮箱是否可以进行邮箱认证
        /// 作者：赵锐
        /// 日期：2009年8月6日
        /// </summary>
        public static int EmailSel(string CustID, string Email, string SourceSPID, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_MailSel";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);


                SqlParameter parSourceSPID = new SqlParameter("SourceSPID", SqlDbType.VarChar, 8);
                parSourceSPID.Value = SourceSPID;
                cmd.Parameters.Add(parSourceSPID);


                SqlParameter parSqlResult = new SqlParameter("@SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
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

        /// <summary>
        /// 根据认证邮箱查询客户id
        /// </summary>
        public static int EmailSel(String Email, out String CustID, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            CustID = "";
            try
            {
                SqlCommand cmd = new SqlCommand("up_Customer_V3_Interface_SelByAuthenMail");
                cmd.CommandType = CommandType.StoredProcedure;

                IDbDataParameter parEmail = new SqlParameter("@Email",SqlDbType.VarChar,100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

                IDbDataParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parCustID);

                IDbDataParameter parResult = new SqlParameter("@Result",SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                IDbDataParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                CustID = parCustID.Value.ToString();
                Result = Convert.ToInt32(parResult.Value);
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch { }

            return Result;
        }

        /// <summary>
        /// 通过用户名和认证邮箱查询用户记录是否存在
        /// 作者：赵锐
        /// 日期：2009年8月11日
        /// </summary>
        public static int FindPwdByEmail(string UserName, string Email, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {

                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_FindPwdByEmail";

                SqlParameter parUserName = new SqlParameter("UserName", SqlDbType.VarChar, 30);
                parUserName.Value = UserName;
                cmd.Parameters.Add(parUserName);

                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                //此参数无用，存储过程中无返回值
                SqlParameter parPwd = new SqlParameter("pwd", SqlDbType.VarChar, 128);
                parPwd.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parPwd);

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

        /// <summary>
        /// 对客户的验证码进行比对验证,如果比对成功则设置为CustInfo的认证Email
        /// 作者：赵锐
        /// 日期：2009年8月11日
        /// </summary>
        public static int SelSendEmailMassage(string CustID, string Email, out string ErrMsg)
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
                cmd.CommandText = "up_Customer_V3_Interface_SelSendEmailMassage";


                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

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

        /// <summary>
        /// 查询客户的邮箱地址
        /// 作者：赵锐
        /// 日期：2009年9月27日
        /// </summary>
        public static DataSet SelEmailAddress(string CustID, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = 0;
            string Email = null;
            string EmailClass = null;
            DataSet ds = new DataSet();
            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_SelEmailAddress";


                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                parSqlResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parSqlResult);

                SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();     
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return ds;
        }
        
        /// <summary>
        /// 判断用户是否有认证邮箱
        /// </summary>
        public static Boolean IsExistsAuthEmail(string CustID , out string ErrMsg)
        {
            bool tmp = false;
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsExistsAuthEmail";

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar,255);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = Convert.ToInt32(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

                if(Result.ToString().Equals("0"))
                {
                    tmp = true;
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            }
            return tmp;
        }

        /// <summary>
        /// 已发送邮件信息验证
        /// </summary>
        public static Int32 CheckEmaklSend(String CustID, String Email, String AuthenCode, out String ErrMsg)
        {
            Int32 result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "up_Customer_V3_Interface_CheckEmailSend";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                    parCustID.Value = CustID;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                    parEmail.Value = Email;
                    cmd.Parameters.Add(parEmail);

                    SqlParameter parAuthenCode = new SqlParameter("@AuthenCode", SqlDbType.VarChar, 6);
                    parAuthenCode.Value = AuthenCode;
                    cmd.Parameters.Add(parAuthenCode);

                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                    result = Convert.ToInt32(parResult.Value);
                    ErrMsg = parErrMsg.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return result;
        }

        #region 发送邮件

        /// <summary>
        /// 发送邮件：如果成功则向邮件历史表中插入一条记录，如果失败则向SendEmailRecord中插入一条记录
        /// 作者：赵锐
        /// 日期：2009年8月6日
        /// </summary>
        public static int InsertEmailSendMassage(string CustID, string OPType, string Message, string AuthenCode, int Result, string Email, 
            DateTime DealTime, string Description, string SubjectName, int NotifyCount, out string ErrMsg)
        {

            ErrMsg = "";
            int SqlResult = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            string EmailModelPath = System.AppDomain.CurrentDomain.BaseDirectory + "MailModel.xml";
            try
            {
                //实时发送邮件
                SqlResult = EmailSend(Message, Email, EmailModelPath, SubjectName, out ErrMsg);
                if (SqlResult == 0) //若成功则直接记录发送邮件历史表
                {
                    InsertEmailSendHistory(CustID, OPType, Message, AuthenCode, Email, 0, "", SubjectName);
                    //return Result; //发送成功则不继续往下走
                }
                else//若失败则记录失败日志
                {
                    SqlConnection mycon = null;
                    SqlCommand cmd = new SqlCommand();

                    mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                    cmd.Connection = mycon;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "up_Customer_V3_Interface_SendEmailRecord";

                    SqlParameter parSubjectName = new SqlParameter("SubjectName", SqlDbType.VarChar, 100);
                    parSubjectName.Value = SubjectName;
                    cmd.Parameters.Add(parSubjectName);

                    SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                    parCustID.Value = CustID;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parOPType = new SqlParameter("OPType", SqlDbType.VarChar, 1);
                    parOPType.Value = OPType;
                    cmd.Parameters.Add(parOPType);

                    SqlParameter parMessage = new SqlParameter("Message", SqlDbType.Text);
                    parMessage.Value = Message;
                    cmd.Parameters.Add(parMessage);

                    SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
                    parAuthenCode.Value = AuthenCode;
                    cmd.Parameters.Add(parAuthenCode);

                    SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
                    parResult.Value = Result;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                    parEmail.Value = Email;
                    cmd.Parameters.Add(parEmail);

                    SqlParameter parDealTime = new SqlParameter("DealTime", SqlDbType.DateTime);
                    parDealTime.Value = DealTime;
                    cmd.Parameters.Add(parDealTime);

                    SqlParameter parDescription = new SqlParameter("Description", SqlDbType.VarChar, 40);
                    parDescription.Value = Description;
                    cmd.Parameters.Add(parDescription);

                    SqlParameter parNotifyCount = new SqlParameter("NotifyCount", SqlDbType.Int);
                    parNotifyCount.Value = NotifyCount;
                    cmd.Parameters.Add(parNotifyCount);

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
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        }

        /// <summary>
        /// 重置密码时发送邮件:如果成功则向邮件历史表中插入一条记录，如果失败则向SendEmailRecord中插入一条记录
        /// </summary>
        public static int InsertEmailByResetPwd(string CustID, string OPType, string Message, string AuthenCode, int Result,
            string Email, DateTime DealTime, string Description, string SubjectName, int NotifyCount, Int32 ExpiredHour, out string ErrMsg)
        {
            ErrMsg = "";
            int SqlResult = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            //获取模板路径
            string EmailModelPath = System.AppDomain.CurrentDomain.BaseDirectory + @"XmlModel\ResetPwdMailModel.xml";
            try
            {
                //设置邮件信息
                MailMessage mailMsg = new MailMessage();
                mailMsg.Subject = SubjectName;
                mailMsg.MailEncoding = MailEncodings.UTF8;
                mailMsg.MailType = MailTypes.Html;
                mailMsg.ReceiversList.Add(Email);

                String bodyText = mailMsg.ReadHtml(EmailModelPath);
                bodyText = bodyText.Replace("Message", Message);
                //链接超时时间
                if (ExpiredHour > 0)
                    bodyText = bodyText.Replace("ExpiredHour", String.Format("此链接将在{0}小时后失效", ExpiredHour));
                else
                    bodyText = bodyText.Replace("ExpiredHour", "");

                mailMsg.MailBody = System.Text.Encoding.UTF8.GetBytes(bodyText);

                //实时发送邮件
                //SqlResult = CIP2BizRules.EmailSend(mailMsg, out ErrMsg);
                SqlResult = EmailSend(mailMsg, out ErrMsg);

                if (SqlResult == 0) //若成功则直接记录发送邮件历史表
                {
                    InsertEmailSendHistory(CustID, OPType, Message, AuthenCode, Email, 0, "", SubjectName);
                }
                else
                {
                    #region 如果邮件发送失败，则将邮件插入数据库，再通过windows服务继续发送

                    SqlConnection mycon = null;
                    SqlCommand cmd = new SqlCommand();

                    mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                    cmd.Connection = mycon;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "up_Customer_V3_Interface_SendEmailRecord";

                    SqlParameter parSubjectName = new SqlParameter("SubjectName", SqlDbType.VarChar, 100);
                    parSubjectName.Value = SubjectName;
                    cmd.Parameters.Add(parSubjectName);

                    SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                    parCustID.Value = CustID;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parOPType = new SqlParameter("OPType", SqlDbType.VarChar, 1);
                    parOPType.Value = OPType;
                    cmd.Parameters.Add(parOPType);

                    SqlParameter parMessage = new SqlParameter("Message", SqlDbType.Text);
                    parMessage.Value = Message;
                    cmd.Parameters.Add(parMessage);

                    SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
                    parAuthenCode.Value = AuthenCode;
                    cmd.Parameters.Add(parAuthenCode);

                    SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
                    parResult.Value = Result;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                    parEmail.Value = Email;
                    cmd.Parameters.Add(parEmail);

                    SqlParameter parDealTime = new SqlParameter("DealTime", SqlDbType.DateTime);
                    parDealTime.Value = DealTime;
                    cmd.Parameters.Add(parDealTime);

                    SqlParameter parDescription = new SqlParameter("Description", SqlDbType.VarChar, 40);
                    parDescription.Value = Description;
                    cmd.Parameters.Add(parDescription);

                    SqlParameter parNotifyCount = new SqlParameter("NotifyCount", SqlDbType.Int);
                    parNotifyCount.Value = NotifyCount;
                    cmd.Parameters.Add(parNotifyCount);

                    SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
                    parSqlResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parSqlResult);

                    SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                    SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());

                    ErrMsg = parErrMsg.Value.ToString();

                    #endregion
                }
            }
            catch (Exception e)
            {
                SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return SqlResult;
        }

        /// <summary>
        /// 发送邮件
        /// 作者：苑峰      时间：2010-3-2
        /// 修改：
        /// </summary>
        public static int EmailSend(string EmailMessage, string EmailAddress, string EmailModelPath, string SubjectName,
            out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            try
            {
                string Extension = "";     //扩展信息

                StringBuilder myStringBuilder = new StringBuilder();

                //调用邮件发送接口
                SMTPEmailSend smtp = new SMTPEmailSend();
                //string Subject = System.Configuration.ConfigurationManager.AppSettings["Subject"];
                string FormName = System.Configuration.ConfigurationManager.AppSettings["FormName_old"];
                string From = System.Configuration.ConfigurationManager.AppSettings["From_old"];
                string UserID = System.Configuration.ConfigurationManager.AppSettings["UserID_old"];
                string Password = System.Configuration.ConfigurationManager.AppSettings["Password_old"];
                string ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName_old"];
                
                //邮件的发送
                smtp.EmailSend(EmailMessage, EmailAddress, Extension, out Result, EmailModelPath, SubjectName, From, FormName, UserID, Password, ServerName, out ErrMsg);

            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "邮件发送失败" + ex.Message;

            }

            return Result;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public static int EmailSend(MailMessage mailMsg, out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            try
            {
                //获取一些配置信息
                string FormName = System.Configuration.ConfigurationManager.AppSettings["FormName"];
                string From = System.Configuration.ConfigurationManager.AppSettings["From"];
                string UserID = System.Configuration.ConfigurationManager.AppSettings["UserID"];
                string Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
                string ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                //邮件信息
                mailMsg.From = From;
                mailMsg.FromName = FormName;
                //服务器信息
                MailServerDes des = new MailServerDes();
                des.UserID = UserID;
                des.Password = Password;
                des.ServerName = ServerName;
                //邮件的发送
                SMTPEmailSend smtp = new SMTPEmailSend();
                smtp.EmailSend(mailMsg, des, out Result, out ErrMsg);
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "邮件发送失败" + ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// 对已成功发送的邮件向SendEmailRecordHistory中插入一条历史记录
        /// </summary>
        public static void InsertEmailSendHistory(string CustID, string OPType, string Message, string AuthenCode, string Email, int Result, string Description, string SubjectName)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.CommandText = "up_Customer_V3_Interface_InsertEmailSendHistory";

                SqlParameter parSubjectName = new SqlParameter("SubjectName", SqlDbType.VarChar, 100);
                parSubjectName.Value = SubjectName;
                cmd.Parameters.Add(parSubjectName);

                SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parOPType = new SqlParameter("OPType", SqlDbType.VarChar, 1);
                parOPType.Value = OPType;
                cmd.Parameters.Add(parOPType);

                SqlParameter parMessage = new SqlParameter("Message", SqlDbType.Text);
                parMessage.Value = Message;
                cmd.Parameters.Add(parMessage);

                SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
                parResult.Value = Result;
                cmd.Parameters.Add(parResult);

                SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
                parAuthenCode.Value = AuthenCode;
                cmd.Parameters.Add(parAuthenCode);

                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                parEmail.Value = Email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parDescription = new SqlParameter("Description", SqlDbType.VarChar, 40);
                parDescription.Value = Description;
                cmd.Parameters.Add(parDescription);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 对邮件进行验证，如果验证通过则将邮件从SendEmailRecordHistory删除并插入到SendEmailRecordHistory2中
        /// </summary>
        public static Int32 InsertEmailSendHistory2(String CustID, String Email, String AuthenCode, out String ErrMsg)
        {
            Int32 result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "up_Customer_V3_Interface_InsertEmailSendHistory2";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                    parCustID.Value = CustID;
                    cmd.Parameters.Add(parCustID);

                    SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                    parEmail.Value = Email;
                    cmd.Parameters.Add(parEmail);

                    SqlParameter parAuthenCode = new SqlParameter("@AuthenCode", SqlDbType.VarChar, 6);
                    parAuthenCode.Value = AuthenCode;
                    cmd.Parameters.Add(parAuthenCode);

                    SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                    parResult.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResult);

                    SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
                    parErrMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parErrMsg);

                    DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                    result = Convert.ToInt32(parResult.Value);
                    ErrMsg = parErrMsg.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return result;
        }

        #endregion

    }
}

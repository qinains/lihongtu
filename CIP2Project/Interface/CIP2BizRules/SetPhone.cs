using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 暂时不用(方法都移植到PhoneBO类中)
    /// </summary>
   public class SetPhone
   {
       
       /// <summary>
       /// 验证手机号是否可以验证
       /// 作者：赵锐
       /// 日期:2009年8月5日
       /// </summary>
       public static int PhoneSel(string CustID, string Phone,out string ErrMsg)
       {
           ErrMsg = "";
           int Result = 0;
           SqlConnection mycon = null;
           SqlCommand cmd = new SqlCommand();
           try
           {
               mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
               cmd.Connection = mycon;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "up_Customer_V3_Interface_PhoneSel";


               SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
               parCustID.Value = CustID;
               cmd.Parameters.Add(parCustID);

               SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
               parPhone.Value = Phone;
               cmd.Parameters.Add(parPhone);

               SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
               parResult.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parResult);

               SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar,256);
               parErrMsg.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parErrMsg);

               DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

               Result = Convert.ToInt32(parResult.Value.ToString());

               ErrMsg = Convert.ToString(parErrMsg.Value.ToString());

           }
           catch( Exception e )
           {
               Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
               ErrMsg = e.Message;
           }

           return Result;
       }

       /// <summary>
       /// 查询客户的手机号
       /// 作者：赵锐
       /// 日期：2009年9月27日
       /// </summary>
       public static string SelPhoneNum(string CustID,string SPID,out string ErrMsg)
       {
           ErrMsg = "";
           int SqlResult = 0;
           string Phone = "";
           SqlConnection mycon = null;
           SqlCommand cmd = new SqlCommand();
           try
           {
               mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
               cmd.Connection = mycon;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "up_Customer_V3_Interface_SelPhoneNum";

               SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
               parPhone.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parPhone);

               SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
               parCustID.Value = CustID;
               cmd.Parameters.Add(parCustID);

               SqlParameter parSPID = new SqlParameter("SPID",SqlDbType.VarChar,8);
               parSPID.Value = SPID;
               cmd.Parameters.Add(parSPID);

               SqlParameter parSqlResult = new SqlParameter("SqlResult", SqlDbType.Int);
               parSqlResult.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parSqlResult);

               SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
               parErrMsg.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parErrMsg);

               DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

               SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
               ErrMsg = parErrMsg.Value.ToString();
               Phone = parPhone.Value.ToString();
           }
           catch (Exception e)
           {
               SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
               ErrMsg = e.Message;
           }
           return Phone;
       }

       /// <summary>
       /// 获取用户所有手机号码
       /// </summary>
       public static DataSet GetAllPhone(string CustId,out string ErrMsg)
       {
           ErrMsg = "";
           int SqlResult = 0;
           DataSet ds = new DataSet();
           SqlConnection mycon = null;
           SqlCommand cmd = new SqlCommand();
           try
           {
             
               mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
               cmd.Connection = mycon;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "up_Customer_V3_Interface_SelAllPhone";

               SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
               parCustID.Value = CustId;
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
       /// 删除一条电话记录
       /// </summary>
       public static int DelPhone(string Phone, string SequenceID,out string ErrMsg)
       {
           ErrMsg = "";
           int Result = 0;
           DataSet ds = new DataSet();
           SqlConnection mycon = null;
           SqlCommand cmd = new SqlCommand();
           try
           {
               mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
               cmd.Connection = mycon;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "up_Customer_V3_Interface_DelPhone";

               SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
               parPhone.Value = Phone;
               cmd.Parameters.Add(parPhone);


               SqlParameter parSequenceID = new SqlParameter("SequenceID",SqlDbType.BigInt);
               parSequenceID.Value = SequenceID;
               cmd.Parameters.Add(parSequenceID);


               SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
               parResult.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parResult);

               SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
               parErrMsg.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parErrMsg);

               DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

               Result = Convert.ToInt32(parResult.Value.ToString());

               ErrMsg = Convert.ToString(parErrMsg.Value.ToString());


           }
           catch (Exception e)
           {
               Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
               ErrMsg = e.Message;
           }
           return Result;
       }

       /// <summary>
       /// 验证手机是否是认证手机
       /// </summary>
       public static string IsAuthenPhone(string Phone, string SPID, out string ErrMsg)
       {
           ErrMsg = "";
           int SqlResult = 0;
           string CustID = "";
           SqlConnection mycon = null;
           SqlCommand cmd = new SqlCommand();
           try
           {
               mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
               cmd.Connection = mycon;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "up_Customer_V3_Interface_IsAuthenPhone";

               SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
               parCustID.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parCustID);

               SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
               parPhone.Value = Phone;
               cmd.Parameters.Add(parPhone);

               SqlParameter parSPID = new SqlParameter("SPID", SqlDbType.VarChar, 8);
               parSPID.Value = SPID;
               cmd.Parameters.Add(parSPID);

               SqlParameter parSqlResult = new SqlParameter("Result", SqlDbType.Int);
               parSqlResult.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parSqlResult);

               SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
               parErrMsg.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parErrMsg);

               DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

               SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
               ErrMsg = parErrMsg.Value.ToString();
               CustID = parCustID.Value.ToString();
           }
           catch (Exception e)
           {
               SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
               ErrMsg = e.Message;
           }
           return CustID;
       }

       /// <summary>
       /// 插入一条手机验证码
       /// 作者：赵锐
       /// 日期：2009年8月5日
       /// </summary>
       public static int InsertPhoneSendMassage(string CustID, string Message, string AuthenCode, string Phone,
            DateTime DealTime, string Description, int NotifyCount, int Result, string OPType, out string ErrMsg)
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
               cmd.CommandText = "up_Customer_V3_Interface_SendSMSRecord";

               SqlParameter parOPType = new SqlParameter("OPType", SqlDbType.VarChar, 1);
               parOPType.Value = OPType;
               cmd.Parameters.Add(parOPType);

               SqlParameter parResult = new SqlParameter("Result", SqlDbType.Int);
               parResult.Value = Result;
               cmd.Parameters.Add(parResult);

               SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
               parCustID.Value = CustID;
               cmd.Parameters.Add(parCustID);

               SqlParameter parMessage = new SqlParameter("Message", SqlDbType.Text);
               parMessage.Value = Message;
               cmd.Parameters.Add(parMessage);

               SqlParameter parAuthenCode = new SqlParameter("AuthenCode", SqlDbType.VarChar, 6);
               parAuthenCode.Value = AuthenCode;
               cmd.Parameters.Add(parAuthenCode);

               SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
               parPhone.Value = Phone;
               cmd.Parameters.Add(parPhone);

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
           catch (Exception e)
           {
               Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
               ErrMsg = e.Message;
           }

           return SqlResult;
       }

       /// <summary>
       /// 验证客户手机验证码
       /// 作者：赵锐
       /// 日期：2009年8月5日
       /// </summary>
       public static int SelSendSMSMassage(string CustID, string Phone, string AuthenCode, out string ErrMsg)
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
               cmd.CommandText = "up_Customer_V3_Interface_SelSendSMSMassage";

               SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
               parCustID.Value = CustID;
               cmd.Parameters.Add(parCustID);

               SqlParameter parPhone = new SqlParameter("Phone", SqlDbType.VarChar, 20);
               parPhone.Value = Phone;
               cmd.Parameters.Add(parPhone);

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

       /// <summary>
       /// 根据验证码获取密码
       /// </summary>
       public static string GiveMePasswordByCustID(string SPID,string type, string CustID,out int result, out string ErrMsg)
       {
           result = 0;
           ErrMsg = "";
           CustID = "";
           int SqlResult = 0;
           string PassWord = "";
           SqlConnection mycon = null;
           SqlCommand cmd = new SqlCommand();
           try
           {
               mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
               cmd.Connection = mycon;
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.CommandText = "up_Customer_V3_Interface_GiveMePasswordByCustID";


               SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
               parCustID.Value = CustID;
               cmd.Parameters.Add(parCustID);


               SqlParameter parPwd = new SqlParameter("Pwd", SqlDbType.VarChar, 36);
               parPwd.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parPwd);


               SqlParameter parType = new SqlParameter("type", SqlDbType.VarChar, 1);
               parType.Value = type;
               cmd.Parameters.Add(parType);



               SqlParameter parSqlResult = new SqlParameter("Result", SqlDbType.Int);
               parSqlResult.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parSqlResult);

               SqlParameter parErrMsg = new SqlParameter("ErrMsg", SqlDbType.VarChar, 256);
               parErrMsg.Direction = ParameterDirection.Output;
               cmd.Parameters.Add(parErrMsg);

               DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

               SqlResult = Convert.ToInt32(parSqlResult.Value.ToString());
               result = SqlResult;
               ErrMsg = parErrMsg.Value.ToString();
               PassWord = parPwd.Value.ToString();


           }
           catch (Exception e)
           {
               SqlResult = ErrorDefinition.IError_Result_System_UnknowError_Code;
               ErrMsg = e.Message;
           }
           return PassWord;

       }

   }
}

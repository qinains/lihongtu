using System;
using System.Collections.Generic;
using System.Text;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
namespace Linkage.BestTone.Interface.Rule.ThirdPartyAuthen
{
    public class ThirdPartyAccount
    {
        private String Insert_Sql = String.Format("Insert into AccountBindingRecord(TransactionID_SP,TransactionID_Ext,CustID,SPID,OptionType,ReqTime,CompleteTime,BestPayAccount,PW,Status,IsDeleted) values(@TransactionID_SP,@TransactionID_Ext,@CustID,@SPID,@OptionType,@ReqTime,@CompleteTime,@BestPayAccount,@PW,@Status,@IsDeleted)");
        private String Update_Sql = String.Format("Update AccountBindingRecord set TransactionID_SP = @TransactionID_SP,TransactionID_Ext = @TransactionID_Ext,CustID = @CustID,SPID = @SPID,OptionType = @OptionType,ReqTime = @ReqTime,CompleteTime = @CompleteTime,BestPayAccount = @BestPayAccount,PW = @PW,Status= @Status,IsDeleted = @IsDeleted where ID = @ID");
        private String Select_Sql = String.Format("Select ID,TransactionID_SP,TransactionID_Ext,CustID,SPID,OptionType,ReqTime,CompleteTime,BestPayAccount,PW,Status,IsDeleted from AccountBindingRecord ");

        public static Int32 IsOpenIdHasBindingCustId(String SourceType,String OpenID,out String CustID,out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code; ;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            CustID = "";

            SqlConnection mycon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                mycon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = mycon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_IsOpenIdHasBindingCustId";


                SqlParameter parSourceType = new SqlParameter("@SourceType", SqlDbType.VarChar, 16);
                parSourceType.Value = SourceType;
                cmd.Parameters.Add(parSourceType);


                SqlParameter parOpenID = new SqlParameter("@OpenID", SqlDbType.VarChar, 16);
                parOpenID.Value = OpenID;
                cmd.Parameters.Add(parOpenID);


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

                Result = Convert.ToInt32(parSqlResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

           return Result;

        }

    }
}

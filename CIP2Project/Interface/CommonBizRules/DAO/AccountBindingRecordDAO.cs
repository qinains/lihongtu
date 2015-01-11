using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class AccountBindingRecordDAO
    {
        private String Insert_Sql = String.Format("Insert into AccountBindingRecord(TransactionID_SP,TransactionID_Ext,CustID,SPID,OptionType,ReqTime,CompleteTime,BestPayAccount,PW,Status,IsDeleted) values(@TransactionID_SP,@TransactionID_Ext,@CustID,@SPID,@OptionType,@ReqTime,@CompleteTime,@BestPayAccount,@PW,@Status,@IsDeleted)");
        private String Update_Sql = String.Format("Update AccountBindingRecord set TransactionID_SP = @TransactionID_SP,TransactionID_Ext = @TransactionID_Ext,CustID = @CustID,SPID = @SPID,OptionType = @OptionType,ReqTime = @ReqTime,CompleteTime = @CompleteTime,BestPayAccount = @BestPayAccount,PW = @PW,Status= @Status,IsDeleted = @IsDeleted where ID = @ID");
        private String Select_Sql = String.Format("Select ID,TransactionID_SP,TransactionID_Ext,CustID,SPID,OptionType,ReqTime,CompleteTime,BestPayAccount,PW,Status,IsDeleted from AccountBindingRecord ");

        public bool Insert(AccountBindingRecord obj)
        {
            bool result = false;
            try
            {
                IDataParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@TransactionID_SP",obj.TransactionID_SP),
                    new SqlParameter("@TransactionID_Ext",obj.TransactionID_Ext),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@SPID",obj.SPID),
                    new SqlParameter("@OptionType",obj.OptionType),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@BestPayAccount",obj.BestPayAccount),
                    new SqlParameter("@PW",obj.PW),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@IsDeleted",obj.IsDeleted)
                };

                SqlCommand cmd = new SqlCommand(Insert_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }

            return result;
        }

        public bool Update(AccountBindingRecord obj)
        {
            bool result = false;
            try
            {
                IDataParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@TransactionID_SP",obj.TransactionID_SP),
                    new SqlParameter("@TransactionID_Ext",obj.TransactionID_Ext),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@SPID",obj.SPID),
                    new SqlParameter("@OptionType",obj.OptionType),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@BestPayAccount",obj.BestPayAccount),
                    new SqlParameter("@PW",obj.PW),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@IsDeleted",obj.IsDeleted)
                };

                SqlCommand cmd = new SqlCommand(Update_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }

            return result;
        }

        #region ²éÑ¯

        public AccountBindingRecord QueryByID(Int64 id)
        {
            AccountBindingRecord obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<AccountBindingRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public AccountBindingRecord QueryByTransactionID(String transactionid)
        {
            AccountBindingRecord obj = null;
            try
            {
                String where = String.Format(" where TransactionID_Ext = '{0}'", transactionid);
                IList<AccountBindingRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                {
                    obj = list[0];
                }
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public IList<AccountBindingRecord> QueryByCustID(String custid)
        {
            IList<AccountBindingRecord> list = null;
            try
            {
                String where = String.Format(" where CustID = '{0}'", custid);
                list = QueryByWhere(where);
            }
            catch (Exception ex) { throw ex; }

            return list;
        }

        public IList<AccountBindingRecord> QueryByBestAccount(String account)
        {
            IList<AccountBindingRecord> list = null;
            try
            {
                String where = String.Format(" where BestPayAccount = '{0}'", account);
                list = QueryByWhere(where);
            }
            catch (Exception ex) { throw ex; }

            return list;
        }

        public IList<AccountBindingRecord> QueryBySPID(String spid)
        {
            IList<AccountBindingRecord> list = null;
            try
            {
                String where = String.Format(" where SPID = '{0}'", spid);
                list = QueryByWhere(where);
            }
            catch (Exception ex) { throw ex; }

            return list;
        }

        #endregion

        #region Ë½ÓÐº¯Êý

        private IList<AccountBindingRecord> QueryByWhere(String where)
        {
            IList<AccountBindingRecord> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<AccountBindingRecord>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    AccountBindingRecord entity = new AccountBindingRecord();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.TransactionID_SP = row["TransactionID_SP"].ToString();
                    entity.TransactionID_Ext = row["TransactionID_Ext"].ToString();
                    entity.CustID = row["CustID"].ToString();
                    entity.SPID = row["SPID"].ToString();
                    entity.OptionType = Convert.ToInt32(row["OptionType"]);
                    entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                    entity.CompleteTime = Convert.ToDateTime(row["CompleteTime"]);
                    entity.BestPayAccount = row["BestPayAccount"].ToString();
                    entity.PW = row["PW"].ToString();
                    entity.Status = Convert.ToInt32(row["Status"]);
                    entity.IsDeleted = Convert.ToInt32(row["IsDeleted"]);

                    list.Add(entity);
                }
            }

            return list;
        }

        #endregion
    }
}

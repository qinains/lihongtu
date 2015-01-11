using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class AccountRechargeRecordDAO
    {
        private String Insert_Sql = "Insert into AccountRechargeRecord (RechargeTransactionID,RechargeDate,OrderSeq,OrderAmount,RechargeType,OrderDesc,ReqTime,CompleteTime,Status,ReturnCode,ReturnDesc)"
                                    + "values(@RechargeTransactionID,@RechargeDate,@OrderSeq,@OrderAmount,@RechargeType,@OrderDesc,@ReqTime,@CompleteTime,@Status,@ReturnCode,@ReturnDesc)";

        private String Update_Sql = "Update AccountRechargeRecord set RechargeTransactionID = @RechargeTransactionID,RechargeDate=@RechargeDate,OrderSeq = @OrderSeq,OrderAmount=@OrderAmount,RechargeType=@RechargeType,OrderDesc = @OrderDesc, "
                                    + "ReqTime = @ReqTime,CompleteTime = @CompleteTime,Status = @Status,ReturnCode=@ReturnCode,ReturnDesc=@ReturnDesc where ID = @ID";

        private String Select_Sql = "Select ID,RechargeTransactionID,RechargeDate,OrderSeq,OrderAmount,RechargeType,OrderDesc,ReqTime,CompleteTime,Status,ReturnCode,ReturnDesc From AccountRechargeRecord ";

        public bool Insert(AccountRechargeRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@RechargeTransactionID",obj.RechargeTransactionID),
                    new SqlParameter("@RechargeDate",obj.RechargeDate),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@RechargeType",obj.RechargeType),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@ReturnCode",obj.ReturnCode),
                    new SqlParameter("@ReturnDesc",obj.ReturnDesc)
                };

                SqlCommand cmd = new SqlCommand(Insert_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }

            return result;
        }

        public bool Update(AccountRechargeRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@RechargeTransactionID",obj.RechargeTransactionID),
                    new SqlParameter("@RechargeDate",obj.RechargeDate),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@RechargeType",obj.RechargeType),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@ReturnCode",obj.ReturnCode),
                    new SqlParameter("@ReturnDesc",obj.ReturnDesc)
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

        public AccountRechargeRecord QueryByID(Int64 id)
        {
            AccountRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<AccountRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public AccountRechargeRecord QueryByOrderSeq(String orderSeq)
        {
            AccountRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where OrderSeq = '{0}'", orderSeq);
                IList<AccountRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public AccountRechargeRecord QueryByTransactionID(String orderTransactionID)
        {
            AccountRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where RechargeTransactionID = '{0}'", orderTransactionID);
                IList<AccountRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        #endregion

        #region Ë½ÓÐº¯Êý

        private IList<AccountRechargeRecord> QueryByWhere(String where)
        {
            IList<AccountRechargeRecord> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<AccountRechargeRecord>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    AccountRechargeRecord entity = new AccountRechargeRecord();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.RechargeTransactionID = row["RechargeTransactionID"].ToString();
                    entity.RechargeDate = row["RechargeDate"].ToString();
                    entity.OrderSeq = row["OrderSeq"].ToString();
                    entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                    entity.RechargeType = row["RechargeType"].ToString();
                    entity.OrderDesc = row["OrderDesc"].ToString();
                    entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                    entity.CompleteTime = Convert.ToDateTime(row["CompleteTime"]);
                    entity.Status = Convert.ToInt32(row["Status"]);
                    entity.ReturnCode = row["ReturnCode"].ToString();
                    entity.ReturnDesc = row["ReturnDesc"].ToString();

                    list.Add(entity);
                }
            }

            return list;
        }

        #endregion
    }
}

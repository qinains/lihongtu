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
    public class BankRechargeRecordDAO
    {
        private String Insert_Sql = "Insert into BankRechargeRecord (OrderSeq,PayTransactionID,OrderDate,CurType,OrderAmount,ProductAmount,AttachAmount,"
                                    + "OrderDesc,TargetAccount,Status,ReqTime,PayTime,UptranSeq,TranDate,Sign,ReturnCode,ReturnDesc)"
                                    + "values(@OrderSeq,@PayTransactionID,@OrderDate,@CurType,@OrderAmount,@ProductAmount,@AttachAmount,@OrderDesc,"
                                    + "@TargetAccount,@Status,@ReqTime,@PayTime,@UptranSeq,@TranDate,@Sign,@ReturnCode,@ReturnDesc)";

        private String Update_Sql = "Update BankRechargeRecord set OrderSeq = @OrderSeq,PayTransactionID=@PayTransactionID,OrderDate = @OrderDate, CurType = @CurType, "
                                    + "OrderAmount = @OrderAmount, ProductAmount = @ProductAmount,AttachAmount = @AttachAmount, OrderDesc = @OrderDesc,"
                                    + "TargetAccount = @TargetAccount,Status=@Status, ReqTime = @ReqTime,PayTime = @PayTime,"
                                    + "UptranSeq=@UptranSeq,TranDate=@TranDate,Sign=@Sign,ReturnCode=@ReturnCode,ReturnDesc=@ReturnDesc where ID = @ID";

        private String Select_Sql = "Select ID,OrderSeq,PayTransactionID,OrderDate,CurType,OrderAmount,ProductAmount,AttachAmount,OrderDesc,TargetAccount,Status,ReqTime,PayTime,UptranSeq,TranDate,Sign,ReturnCode,ReturnDesc From BankRechargeRecord ";

        public bool Insert(BankRechargeRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@PayTransactionID",obj.PayTransactionID),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@ProductAmount",obj.ProductAmount),
                    new SqlParameter("@AttachAmount",obj.AttachAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@TargetAccount",obj.TargetAccount),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@PayTime",obj.PayTime),
                    new SqlParameter("@UptranSeq",obj.UptranSeq),
                    new SqlParameter("@TranDate",obj.TranDate),
                    new SqlParameter("@Sign",obj.Sign),
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

        public bool Update(BankRechargeRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@PayTransactionID",obj.PayTransactionID),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@ProductAmount",obj.ProductAmount),
                    new SqlParameter("@AttachAmount",obj.AttachAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@TargetAccount",obj.TargetAccount),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@PayTime",obj.PayTime),
                    new SqlParameter("@UptranSeq",obj.UptranSeq),
                    new SqlParameter("@TranDate",obj.TranDate),
                    new SqlParameter("@Sign",obj.Sign),
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

        public BankRechargeRecord QueryByID(Int64 id)
        {
            BankRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<BankRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public BankRechargeRecord QueryByOrderSeq(String orderSeq)
        {
            BankRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where OrderSeq = '{0}'", orderSeq);
                IList<BankRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public BankRechargeRecord QueryByOrderTransacntionID(String transactionid)
        {
            BankRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where PayTransactionID = '{0}'", transactionid);
                IList<BankRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        #endregion

        #region Ë½ÓÐº¯Êý

        private IList<BankRechargeRecord> QueryByWhere(String where)
        {
            IList<BankRechargeRecord> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<BankRechargeRecord>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BankRechargeRecord entity = new BankRechargeRecord();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.OrderSeq = row["OrderSeq"].ToString();
                    entity.PayTransactionID = row["PayTransactionID"].ToString();
                    entity.OrderDate = row["OrderDate"].ToString();
                    entity.CurType = row["CurType"].ToString();
                    entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                    entity.ProductAmount = Convert.ToInt64(row["ProductAmount"]);
                    entity.AttachAmount = Convert.ToInt64(row["AttachAmount"]);
                    entity.OrderDesc = row["OrderDesc"].ToString();
                    entity.TargetAccount = row["TargetAccount"].ToString();
                    entity.Status = Convert.ToInt32(row["Status"]);
                    entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                    entity.PayTime = Convert.ToDateTime(row["PayTime"]);
                    entity.UptranSeq = row["UptranSeq"].ToString();
                    entity.TranDate = row["TranDate"].ToString();
                    entity.Sign = row["Sign"].ToString();
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

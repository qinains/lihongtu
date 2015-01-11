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
    public class BankRechargeOrderDAO
    {
        private String Insert_Sql = "Insert into BankRechargeOrder (OrderSeq,OrderTransactionID,OrderDate,CurType,OrderAmount,ProductAmount,AttachAmount,"
                                    + "OrderDesc,CustID,TargetAccount,SourceSPID,ReqTime,DeductionTime,CompleteTime,Status,RechargeCount)"
                                    + "values(@OrderSeq,@OrderTransactionID,@OrderDate,@CurType,@OrderAmount,@ProductAmount,@AttachAmount,@OrderDesc,"
                                    + "@CustID,@TargetAccount,@SourceSPID,@ReqTime,@DeductionTime,@CompleteTime,@Status,@RechargeCount)";

        private String Update_Sql = "Update BankRechargeOrder set OrderSeq = @OrderSeq,OrderTransactionID=@OrderTransactionID,OrderDate = @OrderDate, CurType = @CurType, "
                                    + "OrderAmount = @OrderAmount, ProductAmount = @ProductAmount,AttachAmount = @AttachAmount, OrderDesc = @OrderDesc,"
                                    + "CustID = @CustID, TargetAccount = @TargetAccount,SourceSPID=@SourceSPID, ReqTime = @ReqTime,DeductionTime = @DeductionTime,CompleteTime = @CompleteTime,"
                                    + "Status = @Status,RechargeCount=@RechargeCount where ID = @ID";

        private String Select_Sql = "Select ID,OrderSeq,OrderTransactionID,OrderDate,CurType,OrderAmount,ProductAmount,AttachAmount,OrderDesc,CustID,TargetAccount,SourceSPID,ReqTime,DeductionTime,CompleteTime,Status,RechargeCount From BankRechargeOrder ";
        
        public bool Insert(BankRechargeOrder obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@OrderTransactionID",obj.OrderTransactionID),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@ProductAmount",obj.ProductAmount),
                    new SqlParameter("@AttachAmount",obj.AttachAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@TargetAccount",obj.TargetAccount),
                    new SqlParameter("@SourceSPID",obj.SourceSPID),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@DeductionTime",obj.DeductionTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@RechargeCount",obj.RechargeCount)
                };

                SqlCommand cmd = new SqlCommand(Insert_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }

            return result;
        }

        public bool Update(BankRechargeOrder obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@OrderTransactionID",obj.OrderTransactionID),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@ProductAmount",obj.ProductAmount),
                    new SqlParameter("@AttachAmount",obj.AttachAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@TargetAccount",obj.TargetAccount),
                    new SqlParameter("@SourceSPID",obj.SourceSPID),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@DeductionTime",obj.DeductionTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@RechargeCount",obj.RechargeCount)
                };

                SqlCommand cmd = new SqlCommand(Update_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            }
            catch { }

            return result;
        }

        #region 查询

        public BankRechargeOrder QueryByID(Int64 id)
        {
            BankRechargeOrder obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<BankRechargeOrder> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public BankRechargeOrder QueryByOrderSeq(String orderSeq)
        {
            BankRechargeOrder obj = null;
            try
            {
                String where = String.Format(" where OrderSeq = '{0}'", orderSeq);
                IList<BankRechargeOrder> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public BankRechargeOrder QueryByOrderTransacntionID(String transactionid)
        {
            BankRechargeOrder obj = null;
            try
            {
                String where = String.Format(" where OrderTransactionID = '{0}'", transactionid);
                IList<BankRechargeOrder> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        /// <summary>
        /// 查询账户当日充值成功金额数
        /// </summary>
        public long QueryCurrentRechargeAmount(String account)
        {
            long rechargeAmount = 0;
            String strSql = String.Format("select isnull(sum(OrderAmount),0) from BankRechargeOrder where OrderDate='{0}' and TargetAccount='{1}' and Status in (3,9)", DateTime.Now.ToString("yyyyMMdd"), account);
            SqlCommand cmd = new SqlCommand(strSql);
            cmd.CommandType = CommandType.Text;
            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                rechargeAmount = Convert.ToInt64(ds.Tables[0].Rows[0][0]);
            }

            return rechargeAmount;
        }

        public IList<BankRechargeOrder> QueryByCustID(String custid)
        {
            IList<BankRechargeOrder> list = null;
            try
            {
                String where = String.Format(" where CustID = '{0}'", custid);
                list = QueryByWhere(where);
            }
            catch { }

            return list;
        }

        public IList<BankRechargeOrder> QueryByBestAccount(String account)
        {
            IList<BankRechargeOrder> list = null;
            try
            {
                String where = String.Format(" where TargetAccount = '{0}'", account);
                list = QueryByWhere(where);
            }
            catch { }

            return list;
        }

        public long QueryRechargeAmount(String account, String orderdate)
        {
            long rechargeCount = 0;
            try
            {

            }
            catch { }

            return rechargeCount;
        }

        /// <summary>
        /// 查询待充值的订单
        /// </summary>
        public IList<BankRechargeOrder> QueryWaitToRechargeOrder(Int32 count)
        {
            IList<BankRechargeOrder> list = null;

            try
            {
                String sql = String.Empty;
                if (count == 0)
                    sql = String.Format("Select * From BankRechargeOrder Where Status='2' Order by ReqTime", count);
                else
                    sql = String.Format("Select top {0} * From BankRechargeOrder Where Status='2' Order by ReqTime", count);

                SqlCommand cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;

                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null && ds.Tables[0] != null)
                {
                    list = new List<BankRechargeOrder>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        BankRechargeOrder entity = new BankRechargeOrder();
                        entity.ID = Convert.ToInt64(row["ID"]);
                        entity.OrderSeq = row["OrderSeq"].ToString();
                        entity.OrderTransactionID = row["OrderTransactionID"].ToString();
                        entity.OrderDate = row["OrderDate"].ToString();
                        entity.CurType = row["CurType"].ToString();
                        entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                        entity.ProductAmount = Convert.ToInt64(row["ProductAmount"]);
                        entity.AttachAmount = Convert.ToInt64(row["AttachAmount"]);
                        entity.OrderDesc = row["OrderDesc"].ToString();
                        entity.CustID = row["CustID"].ToString();
                        entity.TargetAccount = row["TargetAccount"].ToString();
                        entity.SourceSPID = row["SourceSPID"].ToString();
                        entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                        entity.DeductionTime = Convert.ToDateTime(row["DeductionTime"]);
                        entity.CompleteTime = Convert.ToDateTime(row["CompleteTime"]);
                        entity.Status = Convert.ToInt32(row["Status"]);
                        entity.RechargeCount = Convert.ToInt32(row["RechargeCount"]);

                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        #endregion

        #region 私有函数

        private IList<BankRechargeOrder> QueryByWhere(String where)
        {
            IList<BankRechargeOrder> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<BankRechargeOrder>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BankRechargeOrder entity = new BankRechargeOrder();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.OrderSeq = row["OrderSeq"].ToString();
                    entity.OrderTransactionID = row["OrderTransactionID"].ToString();
                    entity.OrderDate = row["OrderDate"].ToString();
                    entity.CurType = row["CurType"].ToString();
                    entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                    entity.ProductAmount = Convert.ToInt64(row["ProductAmount"]);
                    entity.AttachAmount = Convert.ToInt64(row["AttachAmount"]);
                    entity.OrderDesc = row["OrderDesc"].ToString();
                    entity.CustID = row["CustID"].ToString();
                    entity.TargetAccount = row["TargetAccount"].ToString();
                    entity.SourceSPID = row["SourceSPID"].ToString();
                    entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                    entity.DeductionTime = Convert.ToDateTime(row["DeductionTime"]);
                    entity.CompleteTime = Convert.ToDateTime(row["CompleteTime"]);
                    entity.Status = Convert.ToInt32(row["Status"]);
                    entity.RechargeCount = Convert.ToInt32(row["RechargeCount"]);

                    list.Add(entity);
                }
            }

            return list;
        }

        #endregion
    }
}

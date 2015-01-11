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
    public class RechargeBackRecordDAO
    {
        private String Insert_Sql = "Insert into RechargeBackRecord (UptranSeq,TranDate,RetnCode,RetnInfo,OrderTransactionID,OrderSeq,EncodeType,Sign,RechargeType)"
                                    + "values(@UptranSeq,@TranDate,@RetnCode,@RetnInfo,@OrderTransactionID,@OrderSeq,@EncodeType,@Sign,@RechargeType)";

        private String Update_Sql = "Update RechargeBackRecord set UptranSeq = @UptranSeq,TranDate=@TranDate,RetnCode = @RetnCode,RetnInfo=@RetnInfo,OrderTransactionID=@OrderTransactionID,OrderSeq = @OrderSeq, "
                                    + "EncodeType = @EncodeType,Sign = @Sign,RechargeType=@RechargeType where ID = @ID";

        private String Select_Sql = "Select ID,UptranSeq,TranDate,RetnCode,RetnInfo,OrderTransactionID,OrderSeq,EncodeType,Sign,RechargeType From RechargeBackRecord ";

        public bool Insert(RechargeBackRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@UptranSeq",obj.UptranSeq),
                    new SqlParameter("@TranDate",obj.TranDate),
                    new SqlParameter("@RetnCode",obj.RetnCode),
                    new SqlParameter("@RetnInfo",obj.RetnInfo),
                    new SqlParameter("@OrderTransactionID",obj.OrderTransactionID),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@EncodeType",obj.EncodeType),
                    new SqlParameter("@Sign",obj.Sign),
                    new SqlParameter("@RechargeType",obj.RechargeType)
                };

                SqlCommand cmd = new SqlCommand(Insert_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }

            return result;
        }

        public bool Update(RechargeBackRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@UptranSeq",obj.UptranSeq),
                    new SqlParameter("@TranDate",obj.TranDate),
                    new SqlParameter("@RetnCode",obj.RetnCode),
                    new SqlParameter("@RetnInfo",obj.RetnInfo),
                    new SqlParameter("@OrderTransactionID",obj.OrderTransactionID),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@EncodeType",obj.EncodeType),
                    new SqlParameter("@Sign",obj.Sign),
                    new SqlParameter("@RechargeType",obj.RechargeType)
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

        public RechargeBackRecord QueryByID(Int64 id)
        {
            RechargeBackRecord obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<RechargeBackRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public RechargeBackRecord QueryByUptranSeq(String untranseq)
        {
            RechargeBackRecord obj = null;
            try
            {
                String where = String.Format(" where UptranSeq = '{0}'", untranseq);
                IList<RechargeBackRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public RechargeBackRecord QueryByOrderSeq(String orderSeq)
        {
            RechargeBackRecord obj = null;
            try
            {
                String where = String.Format(" where OrderSeq = '{0}'", orderSeq);
                IList<RechargeBackRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public RechargeBackRecord QueryByTransactionID(String orderTransactionID)
        {
            RechargeBackRecord obj = null;
            try
            {
                String where = String.Format(" where OrderTransactionID = '{0}'", orderTransactionID);
                IList<RechargeBackRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        #endregion

        #region Ë½ÓÐº¯Êý

        private IList<RechargeBackRecord> QueryByWhere(String where)
        {
            IList<RechargeBackRecord> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<RechargeBackRecord>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    RechargeBackRecord entity = new RechargeBackRecord();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.UptranSeq = row["UptranSeq"].ToString();
                    entity.TranDate = row["TranDate"].ToString();
                    entity.RetnCode = row["RetnCode"].ToString();
                    entity.RetnInfo = row["RetnInfo"].ToString();
                    entity.OrderTransactionID = row["OrderTransactionID"].ToString();
                    entity.OrderSeq = row["OrderSeq"].ToString();
                    entity.EncodeType = row["EncodeType"].ToString();
                    entity.Sign = row["Sign"].ToString();
                    entity.RechargeType = row["RechargeType"].ToString();

                    list.Add(entity);
                }
            }

            return list;
        }

        #endregion
    }
}

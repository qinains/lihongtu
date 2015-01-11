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

    public class CardRechargeRecordDAO
    {
        private String Insert_Sql = "Insert into CardRechargeRecord (PayTransactionID,OrderSeq,OrderDate,CurType,OrderAmount,OrderDesc,"
                                    + "CardNo,CardPwd,CardType,TargetAccount,Status,ReqTime,PayTime,UptranSeq,TranDate,Sign,ReturnCode,ReturnDesc)"
                                    + " values(@PayTransactionID,@OrderSeq,@OrderDate,@CurType,@OrderAmount,@OrderDesc,@CardNo,@CardPwd,"
                                    + "@CardType,@TargetAccount,@Status,@ReqTime,@PayTime,@UptranSeq,@TranDate,@Sign,@ReturnCode,@ReturnDesc)";

        private String Update_Sql = "Update CardRechargeRecord set PayTransactionID=@PayTransactionID,OrderSeq=@OrderSeq,OrderDate=@OrderDate,"
                                    + "CurType=@CurType,OrderAmount=@OrderAmount,OrderDesc=@OrderDesc,CardNo=@CardNo,CardPwd=@CardPwd,CardType=@CardType,"
                                    + "TargetAccount=@TargetAccount,Status=@Status,ReqTime=@ReqTime,PayTime=@PayTime,UptranSeq=@UptranSeq,TranDate=@TranDate,Sign=@Sign,"
                                    + "ReturnCode=@ReturnCode,ReturnDesc=@ReturnDesc where ID = @ID";

        private String Select_Sql = String.Format("Select ID,PayTransactionID,OrderSeq,OrderDate,CurType,OrderAmount,OrderDesc,CardNo,CardPwd,CardType,TargetAccount,Status,ReqTime,PayTime,UptranSeq,TranDate,Sign,ReturnCode,ReturnDesc From CardRechargeRecord ");
        
        public bool Insert(CardRechargeRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@PayTransactionID",obj.PayTransactionID),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@CardNo",obj.CardNo),
                    new SqlParameter("@CardPwd",obj.CardPwd),
                    new SqlParameter("@CardType",obj.CardType),
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
            catch (Exception ex) { throw ex; }

            return result;
        }
        
        public bool Update(CardRechargeRecord obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@PayTransactionID",obj.PayTransactionID),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@CardNo",obj.CardNo),
                    new SqlParameter("@CardPwd",obj.CardPwd),
                    new SqlParameter("@CardType",obj.CardType),
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
            catch (Exception ex) { throw ex; }

            return result;
        }

        #region ²éÑ¯

        public CardRechargeRecord QueryByID(Int64 id)
        {
            CardRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<CardRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public CardRechargeRecord QueryByTransactionID(String transactionid)
        {
            CardRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where PayTransactionID = '{0}'", transactionid);
                IList<CardRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public CardRechargeRecord QueryByOrderSeq(String orderseq)
        {
            CardRechargeRecord obj = null;
            try
            {
                String where = String.Format(" where OrderSeq = '{0}'", orderseq);
                IList<CardRechargeRecord> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        #endregion

        #region Ë½ÓÐº¯Êý

        private IList<CardRechargeRecord> QueryByWhere(String where)
        {
            IList<CardRechargeRecord> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<CardRechargeRecord>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CardRechargeRecord entity = new CardRechargeRecord();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.PayTransactionID = row["PayTransactionID"].ToString();
                    entity.OrderSeq = row["OrderSeq"].ToString();
                    entity.OrderDate = row["OrderDate"].ToString();
                    entity.CurType = row["CurType"].ToString();
                    entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                    entity.OrderDesc = row["OrderDesc"].ToString();
                    entity.CardNo = row["CardNo"].ToString();
                    entity.CardPwd = row["CardPwd"].ToString();
                    entity.CardType = row["CardType"].ToString();
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

using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class BesttoneAccountDAO
    {
        private String Insert_Sql = String.Format("Insert into BesttoneAccount(BestPayAccount,CustID,PW,CreateTime,Status) values(@BestPayAccount,@CustID,@PW,@CreateTime,@Status)");
        private String Select_Sql = String.Format("Select ID,BestPayAccount,CustID,PW,CreateTime,Status from BesttoneAccount ");
        private String Update_Sql = String.Format("Update BesttoneAccount set BestPayAccount = @BestPayAccount,CustID = @CustID,PW = @PW,Status = @Status where ID = @ID");
        private String Delete_Sql = String.Format("Delete From BesttoneAccount Where BestPayAccount =  @BestPayAccount");
        public bool Insert(BesttoneAccount obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@BestPayAccount",obj.BestPayAccount),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@PW",obj.PW),
                    new SqlParameter("@CreateTime",obj.CreateTime),
                    new SqlParameter("@Status",obj.Status)
                };
                SqlCommand cmd = new SqlCommand(Insert_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool Update(BesttoneAccount obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@BestPayAccount",obj.BestPayAccount),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@PW",obj.PW),
                    new SqlParameter("@Status",obj.Status)
                };

                SqlCommand cmd = new SqlCommand(Update_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }

            return result;
        }

        public bool Delete(String BestPayAccount) 
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@BestPayAccount",BestPayAccount),
                };

                SqlCommand cmd = new SqlCommand(Delete_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch(Exception ex){
               
            }
            return result;
        }


        #region 查询

        public BesttoneAccount QueryByID(Int64 id)
        {
            BesttoneAccount obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<BesttoneAccount> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public BesttoneAccount QueryByCustID(String custid)
        {
            BesttoneAccount obj = null;
            try
            {
                String where = String.Format(" where CustID = '{0}'", custid);
                IList<BesttoneAccount> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                {
                    obj = list[0];
                }
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public BesttoneAccount QueryByBestAccount(String account)
        {
            BesttoneAccount obj = null;
            try
            {
                String where = String.Format(" where BestPayAccount = '{0}'", account);
                IList<BesttoneAccount> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                {
                    obj = list[0];
                }
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        #endregion

        #region 私有方法

        private IList<BesttoneAccount> QueryByWhere(String where)
        {
            IList<BesttoneAccount> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<BesttoneAccount>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BesttoneAccount entity = new BesttoneAccount();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.BestPayAccount = row["BestPayAccount"].ToString();
                    entity.CustID = row["CustID"].ToString();
                    entity.PW = row["PW"].ToString();
                    entity.CreateTime = Convert.ToDateTime(row["CreateTime"]);
                    entity.Status = Convert.ToInt32(row["Status"]);

                    list.Add(entity);
                }
            }

            return list;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class UDBSPInfoBO
    {

        public UDBSPInfo GetBySPID(String SPID)
        {
            UDBSPInfo entity = null;
            try
            {
                String sql = String.Format("select * from UDBSPInfo where SPID ='{0}'", SPID);
                DataSet ds = null;
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    entity = new UDBSPInfo();
                    entity.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    entity.SPID = ds.Tables[0].Rows[0]["SPID"].ToString();
                    entity.UDBSPID = ds.Tables[0].Rows[0]["UDBSPID"].ToString();
                    entity.UDBKey = ds.Tables[0].Rows[0]["UDBKey"].ToString();
                    entity.RedirectUrl = ds.Tables[0].Rows[0]["RedirectUrl"].ToString();
                    entity.ExtendField_1 = ds.Tables[0].Rows[0]["ExtendField_1"].ToString();
                    entity.ExtendField_2 = ds.Tables[0].Rows[0]["ExtendField_2"].ToString();
                    entity.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreateTime"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        public UDBSPInfo GetByUDBSPID(String UDBSPID)
        {
            UDBSPInfo entity = null;
            try
            {
                String sql = String.Format("select * from UDBSPInfo where UDBSPID ='{0}'", UDBSPID);
                DataSet ds = null;
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    entity.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    entity.SPID = ds.Tables[0].Rows[0]["SPID"].ToString();
                    entity.UDBSPID = ds.Tables[0].Rows[0]["UDBSPID"].ToString();
                    entity.UDBKey = ds.Tables[0].Rows[0]["UDBKey"].ToString();
                    entity.RedirectUrl = ds.Tables[0].Rows[0]["RedirectUrl"].ToString();
                    entity.ExtendField_1 = ds.Tables[0].Rows[0]["ExtendField_1"].ToString();
                    entity.ExtendField_2 = ds.Tables[0].Rows[0]["ExtendField_2"].ToString();
                    entity.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreateTime"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        public DataSet QueryAll()
        {
            DataSet ds = null;

            try
            {
                String sql = String.Format("select * from UDBSPInfo");
                ds = null;
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

    }
}
    
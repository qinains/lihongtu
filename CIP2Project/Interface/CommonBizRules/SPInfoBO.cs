using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class SPInfoBO:AbstractInfoManage
    {
        private const String selectSql = "Select SPID,SPName,ProvinceID,SPType,InterfaceUrl,InterfaceUrlV2 From [dbo].[SPInfo]";
        private const String _DataCacheName = "SysDataCache";
        private String _DataCacheExpireTime = ConfigurationManager.AppSettings["SysDataCacheTimeOut"].ToString();
        private DataSet _ObjectData = new SysData();

        public SPInfoBO()
        {
            _ObjectData = new SysData();
        }

        protected override string DataCacheName
        {
            get
            {
                return _DataCacheName;
            }
        }
        protected override string DataCacheExpireTime
        {
            get
            {
                return _DataCacheExpireTime;
            }
        }
        protected override DataSet ObjectData
        {
            get
            {
                return _ObjectData;
            }
        }

        protected override object GetDataFromDB()
        {
            DataSet ds = new DataSet();
            DataSet sysDs = new SysData();
            try
            {
                SqlCommand cmd = new SqlCommand(selectSql);
                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                DataColumnCollection columns = ds.Tables[0].Columns;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = sysDs.Tables[SysData.TableName].NewRow();
                    foreach(DataColumn column in columns)
                    {
                        if (!sysDs.Tables[SysData.TableName].Columns.Contains(column.ColumnName))
                            continue;
                        newRow[column.ColumnName] = row[column.ColumnName];
                    }

                    sysDs.Tables[SysData.TableName].Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sysDs;
        }
        
    }
}

//==============================================================================================================
//
// Class Name: ProvinceInfoManager
// Description: 省信息管理

// Author: 苑峰
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-08
//
//==============================================================================================================
using System;
using System.Text;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    public class ProvinceInfoManager
    {
        private const string ProvinceDataCacheName = "ProvinceDataCache";

        // Province信息变动较少,Cache有效期可设置长一些(单位:小时)
        private string ProvinceDataCacheExpireTime = ConfigurationManager.AppSettings["ProvinceDataCacheExpireTime"];

        public ProvinceInfoManager()
        {

        }

        #region ..获取省数据..

        public object GetProvinceData(HttpContext context)
        {
            object provinceData = CacheUtility.Get(context, ProvinceDataCacheName);
            if (provinceData == null || provinceData.Equals(null))
            {
                provinceData = this.GetProvinceData();
                DateTime expireTime = DateTime.Now.AddHours(Convert.ToDouble(ProvinceDataCacheExpireTime));
                CacheUtility.Set(context, provinceData, ProvinceDataCacheName, expireTime);
            }

            return provinceData;
        }
        private object GetProvinceData()
        {
            ProvinceData provinceData = new ProvinceData();
            DataSet tmpData = new DataSet();

            try
            {
                string Sql_GetProvinceInfo =
                        @"Select ProvinceID,ProvinceCode,ProvinceName,ProvinceType,'' InterfaceUrl, RegionProvince " +
                        "From [dbo].[Province]";


                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql_GetProvinceInfo;

                // 获取数据
                tmpData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                // 数据转换
                // DataSet --> SPData
                int intTargetColumnCount = provinceData.Tables[ProvinceData.TableName].Columns.Count;
                foreach (DataRow row in tmpData.Tables[0].Rows)
                {
                    object[] newRow = new object[intTargetColumnCount];
                    for (int j = 0; j < intTargetColumnCount; j++)
                    {
                        newRow[j] = row[j];
                    }
                    provinceData.Tables[ProvinceData.TableName].Rows.Add(newRow);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return provinceData;
        }

        #endregion

        /// <summary>
        /// 根据ProvinceID获取指定属性
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="PropertyName"></param>
        /// <param name="ProvinceDataObj"></param>
        /// <returns></returns>
        public string GetPropertyByProvinceID(string ProvinceID, string PropertyName, object ProvinceDataObj)
        {
            string propertyValue = "";

            ProvinceData provinceData = (ProvinceData)ProvinceDataObj;
            if (provinceData == null)
                return propertyValue;
            if (provinceData.Tables[ProvinceData.TableName].Rows.Count == 0)
                return propertyValue;

            foreach (DataRow row in provinceData.Tables[ProvinceData.TableName].Rows)
            {
                if (ProvinceID == Convert.ToString(row[ProvinceData.Field_ProvinceID]))
                {
                    propertyValue = Convert.ToString(row[PropertyName]);
                    break;
                }
            }

            return propertyValue;
        }


        /// <summary>
        /// 根据ProvinceCode获取指定属性
        /// </summary>
        /// <param name="ProvinceCode"></param>
        /// <param name="PropertyName"></param>
        /// <param name="ProvinceDataObj"></param>
        /// <returns></returns>
        public string GetPropertyByProvinceCode(string ProvinceCode, string PropertyName, object ProvinceDataObj)
        {
            string propertyValue = "";

            ProvinceData provinceData = (ProvinceData)ProvinceDataObj;
            if (provinceData == null)
                return propertyValue;
            if (provinceData.Tables[ProvinceData.TableName].Rows.Count == 0)
                return propertyValue;

            foreach (DataRow row in provinceData.Tables[ProvinceData.TableName].Rows)
            {
                if (ProvinceCode == Convert.ToString(row[ProvinceData.Field_ProvinceCode]))
                {
                    propertyValue = Convert.ToString(row[PropertyName]);
                    break;
                }
            }

            return propertyValue;
        }


    }// End Class
}

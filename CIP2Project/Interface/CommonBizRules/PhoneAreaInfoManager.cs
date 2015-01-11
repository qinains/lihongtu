//==============================================================================================================
//
// Class Name: AreaInfoManager
// Description: 地市信息管理
// Author: 苑峰
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-08
//
//==============================================================================================================
using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    public class PhoneAreaInfoManager
    {
        private const string PhoneAreaDataCacheName = "PhoneAreaDataCache";

        // 电话区号信息基本不变,Cache有效期可设置长一些(单位:天)
        private string PhoneAreaDataCacheExpireTime = ConfigurationManager.AppSettings["PhoneAreaDataCacheExpireTime"];

        public PhoneAreaInfoManager()
        {

        }

        #region ..获取电话区号信息数据..

        public object GetPhoneAreaData(HttpContext context, string ProvinceID)
        {
            object phoneareaData = CacheUtility.Get(context, PhoneAreaDataCacheName);
            if (phoneareaData == null || phoneareaData.Equals(null))
            {
                phoneareaData = this.GetPhoneAreaData(ProvinceID);
                DateTime expireTime = DateTime.Now.AddDays(Convert.ToDouble(PhoneAreaDataCacheExpireTime));
                CacheUtility.Set(context, phoneareaData, PhoneAreaDataCacheName, expireTime);
            }

            return phoneareaData;
        }
        public object GetPhoneAreaData(HttpContext context)
        {
            object phoneareaData = CacheUtility.Get(context, PhoneAreaDataCacheName);
            if (phoneareaData == null || phoneareaData.Equals(null))
            {
                phoneareaData = this.GetPhoneAreaData();
                DateTime expireTime = DateTime.Now.AddDays(Convert.ToDouble(PhoneAreaDataCacheExpireTime));
                CacheUtility.Set(context, phoneareaData, PhoneAreaDataCacheName, expireTime);
            }

            return phoneareaData;
        }
        private object GetPhoneAreaData(string ProvinceID)
        {
            PhoneAreaData phoneareaData = new PhoneAreaData();
            DataSet tmpData = new DataSet();

            try
            {
                string Sql_GetPhoneArea =
                        "Select AreaID AreaID,  AreaName AreaName,ProvinceID ProvinceID " +
                        "From [dbo].[Area] where ProvinceID='" + ProvinceID + "'";


                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql_GetPhoneArea;

                // 获取数据
                tmpData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                // 数据转换
                // DataSet --> PhoneAreaData
                int intTargetColumnCount = phoneareaData.Tables[PhoneAreaData.TableName].Columns.Count;
                foreach (DataRow row in tmpData.Tables[0].Rows)
                {
                    object[] newRow = new object[intTargetColumnCount];
                    for (int j = 0; j < intTargetColumnCount; j++)
                    {
                        newRow[j] = row[j];
                    }
                    phoneareaData.Tables[PhoneAreaData.TableName].Rows.Add(newRow);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return phoneareaData;
        }
        private object GetPhoneAreaData()
        {
            PhoneAreaData phoneareaData = new PhoneAreaData();
            DataSet tmpData = new DataSet();

            try
            {
                string Sql_GetPhoneArea =
                        "Select AreaID AreaID,  AreaName AreaName,ProvinceID ProvinceID " +
                        "From [dbo].[Area]";


                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql_GetPhoneArea;

                // 获取数据
                tmpData = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                // 数据转换
                // DataSet --> PhoneAreaData
                int intTargetColumnCount = phoneareaData.Tables[PhoneAreaData.TableName].Columns.Count;
                foreach (DataRow row in tmpData.Tables[0].Rows)
                {
                    object[] newRow = new object[intTargetColumnCount];
                    for (int j = 0; j < intTargetColumnCount; j++)
                    {
                        newRow[j] = row[j];
                    }
                    phoneareaData.Tables[PhoneAreaData.TableName].Rows.Add(newRow);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return phoneareaData;
        }

        #endregion


        /// <summary>
        /// 根据电话区号获取指定属性
        /// </summary>
        /// <param name="PhoneAreaCode"></param>
        /// <param name="PropertyName"></param>
        /// <param name="PhoneAreaDataObj"></param>
        /// <returns></returns>
        public string GetPropertyByPhoneAreaCode(string PhoneAreaCode, string PropertyName, object PhoneAreaDataObj)
        {
            string propertyValue = "";

            PhoneAreaData phoneareaData = (PhoneAreaData)PhoneAreaDataObj;
            if (phoneareaData == null)
                return propertyValue;
            if (phoneareaData.Tables[PhoneAreaData.TableName].Rows.Count == 0)
                return propertyValue;

            foreach (DataRow row in phoneareaData.Tables[PhoneAreaData.TableName].Rows)
            {
                if (PhoneAreaCode == Convert.ToString(row[PhoneAreaData.Field_PhoneAreaID]))
                {
                    propertyValue = Convert.ToString(row[PropertyName]);
                    break;
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// 根据电话号码获取归属省ID
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <param name="PhoneAreaDataObj"></param>
        /// <returns></returns>
        public string GetProvinceIDByPhoneNumber(string PhoneNumber, object PhoneAreaDataObj)
        {
            string provinceid = "";
            string phoneareacode = "";

            // 获取前4位作为电话区号获取ProvinceID
            if (PhoneNumber.Length >= 4)
                phoneareacode = PhoneNumber.Substring(0, 4);
            provinceid = GetPropertyByPhoneAreaCode(phoneareacode, PhoneAreaData.Field_ProvinceID, PhoneAreaDataObj);
            if (!CommonUtility.IsEmpty(provinceid))
                return provinceid;


            // 若未获取则以前3位作为电话区号获取ProvinceID
            phoneareacode = PhoneNumber.Substring(0, 3);
            provinceid = GetPropertyByPhoneAreaCode(phoneareacode, PhoneAreaData.Field_ProvinceID, PhoneAreaDataObj);

            return provinceid;
        }
    }// End Class
}

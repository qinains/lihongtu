using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Web;
using System.Web.Services;

using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{

    public class SysInfoManager
    {
        private const string SysDataCacheName = "SysDataCache";

        private const string SqlGetSys = "Select SPID,SPName,ProvinceID,SPType,InterfaceUrl,InterfaceUrlV2 From [dbo].[SPInfo]";

        private string SysDataCacheTimeOut = ConfigurationManager.AppSettings["SysDataCacheTimeOut"];

        #region ..��ȡSysData..
        /// <summary>
        /// ��ȡSys����
        /// </summary>
        /// <param name="webService"></param>
        /// <returns></returns>
        public object GetSysData(WebService SpecificWebService)
        {
            object sysData = CacheUtility.Get(SpecificWebService, SysDataCacheName);
            if (sysData == null || sysData.Equals(null))
            {
                sysData = this.GetSysData();
                CacheUtility.Set(SpecificWebService, sysData, SysDataCacheName,
                    DateTime.Now.AddHours(Convert.ToDouble(SysDataCacheTimeOut)));
            }

            return sysData;
        }

        /// <summary>
        /// ��ȡSP����
        /// </summary>
        /// <param name="SpecificContext"></param>
        /// <returns></returns>
        public object GetSysData(HttpContext SpecificContext)
        {
            object sysData = CacheUtility.Get(SpecificContext, SysDataCacheName);
            if (sysData == null || sysData.Equals(null))
            {
                sysData = this.GetSysData();
                CacheUtility.Set(SpecificContext, sysData, SysDataCacheName,
                    DateTime.Now.AddHours(Convert.ToDouble(SysDataCacheTimeOut)));
            }

            return sysData;
        }

        /// <summary>
        /// �����ݿ��ȡSP���ݼ���
        /// </summary>
        /// <returns></returns>
        private object GetSysData()
        {
            SysData sysData = new SysData();
            DataSet tmpData = new DataSet();

            try
            {
                SqlCommand selCmd = new SqlCommand();
                selCmd.CommandType = CommandType.Text;
                selCmd.CommandText = SysInfoManager.SqlGetSys;

                // ��ȡ����
                tmpData = DBUtility.FillData(selCmd, DBUtility.BestToneCenterConStr);

                // ����ת��
                // DataSet --> SysData
                int intTargetColumnCount = sysData.Tables[SysData.TableName].Columns.Count;
                foreach (DataRow row in tmpData.Tables[0].Rows)
                {
                    object[] newRow = new object[intTargetColumnCount];
                    for (int j = 0; j < intTargetColumnCount; j++)
                    {
                        newRow[j] = row[j];
                    }
                    sysData.Tables[SysData.TableName].Rows.Add(newRow);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return sysData;
        }
        #endregion
        
        /// <summary>
        /// ����SysID��ȡָ������
        /// </summary>
        /// <param name="PhoneAreaCode"></param>
        /// <param name="PropertyName"></param>
        /// <param name="PhoneAreaDataObj"></param>
        /// <returns></returns>
        public string GetPropertyBySysID(string SysID,string PropertyName, object SysDataObj)
        {
            string propertyValue = "";

            SysData SysData = (SysData)SysDataObj;
            if (SysData == null)
                return propertyValue;
            if (SysData.Tables[SysData.TableName].Rows.Count == 0)
                return propertyValue;

            foreach (DataRow row in SysData.Tables[SysData.TableName].Rows)
            {
                if (SysID == Convert.ToString(row[SysData.Field_SysID]))
                {
                    propertyValue = Convert.ToString(row[PropertyName]);
                    break;
                }
            }

            return propertyValue;
        }
    }
}

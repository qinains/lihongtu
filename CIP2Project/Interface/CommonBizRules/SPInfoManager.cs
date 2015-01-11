/*********************************************************************************************************
 *     ����: SP��Ϣ����
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2005
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2005
 *     ����: Է��
 * ��ϵ��ʽ: 
 *     ��˾: �����Ƽ�(�Ͼ�)�ɷ����޹�˾
 * ��������: 2009-07-31
 **********/


using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Services;

using Linkage.BestTone.Interface.Utility;


namespace Linkage.BestTone.Interface.Rule
{
    public class SPInfoManager
    {
       // private const string SPDataCacheName = "SPDataCache";

        private const string SqlGetSPInfo = "Select a.SPID,a.SPName,ProvinceID,SPType,InterfaceUrl,InterfaceUrlV2, SecretKey, SPOuterID" +
                                            " From [dbo].[SPInfo] a, [dbo].[SPSecret] b where b.Status=0 and a.SPID=b.SPID" +
                                            " union Select SPID,SPName,ProvinceID,SPType,InterfaceUrl,InterfaceUrlV2, '' SecretKey, SPOuterID" +
                                            " From [dbo].[SPInfo] where SPID not in (select SPID from Spsecret)";

        private const string SqlGetSPIPList = "Select SPID,StartIPNumber,EndIPNumber From [dbo].[SPIPList] where status=0";

        private const string SqlGetSPInterfaceLimit = "Select SPID,InterfaceName From [dbo].[SPInterfaceLimit]";

        private const string SqlGetSPCAInfo = "Select SPID, CerInfo, CerType,CerPassword,CerUserName From [dbo].[CAInfo] where status = 0";

        private string SPDataCacheTimeOut = ConfigurationManager.AppSettings["SPDataCacheTimeOut"];

        #region ..��ȡSPData..
       
        /// <summary>
        /// ��ȡSP���ݼ�
        /// ���ߣ�Է��      ʱ�䣺2009-7-31
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public object GetSPData(HttpContext SpecificContext, string SPDataCacheName)
        {
            object SPData = CacheUtility.Get(SpecificContext, SPDataCacheName);
            if (SPData == null || SPData.Equals(null))
            {
                SPData = this.GetSPData(SPDataCacheName);
                CacheUtility.Set(SpecificContext, SPData, SPDataCacheName,
                    DateTime.Now.AddHours(Convert.ToDouble(SPDataCacheTimeOut)));
            }

            return SPData;
        }

        /// <summary>
        /// �����ݿ��ȡSP���ݼ���
        /// ���ߣ�Է��      ʱ�䣺2009-7-31
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        private object GetSPData( string SPDataCacheName)
        {
            object ResultData = null;
            DataSet tmpData = new DataSet();

            try
            {
                SqlCommand selCmd = new SqlCommand();
                selCmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                if (SPDataCacheName == "SPData")
                {
                    selCmd.CommandText = SPInfoManager.SqlGetSPInfo;
                    SPData SPData = new SPData();
                    dt = SPData.Tables[SPData.TableName];
                    ResultData = SPData;
                }
                else if (SPDataCacheName == "SPIPListData")
                {
                    selCmd.CommandText = SPInfoManager.SqlGetSPIPList;
                    SPIPListData SPIPListData = new SPIPListData();
                    dt = SPIPListData.Tables[SPIPListData.TableName];
                    ResultData = SPIPListData;
                }
                else if (SPDataCacheName == "SPInterfaceLimitData")
                {
                    selCmd.CommandText = SPInfoManager.SqlGetSPInterfaceLimit;
                    SPInterfaceLimitData SPInterfaceLimitData = new SPInterfaceLimitData();
                    dt = SPInterfaceLimitData.Tables[SPInterfaceLimitData.TableName];
                    ResultData = SPInterfaceLimitData;
                }
                else if (SPDataCacheName == "SPCAData")
                {
                    selCmd.CommandText = SPInfoManager.SqlGetSPCAInfo;
                    SPCAData SPCAData = new SPCAData();
                    dt = SPCAData.Tables[SPCAData.TableName];
                    ResultData = SPCAData;
                }

                // ��ȡ����
                tmpData = DBUtility.FillData(selCmd, DBUtility.BestToneCenterConStr);

                // ����ת��
                // DataSet --> Object

                int intTargetColumnCount = dt.Columns.Count;
                foreach (DataRow row in tmpData.Tables[0].Rows)
                {
                    object[] newRow = new object[intTargetColumnCount];
                    for (int j = 0; j < intTargetColumnCount; j++)
                    {
                        newRow[j] = row[j];
                    }
                    dt.Rows.Add(newRow);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return ResultData;
        }
        #endregion

        /// <summary>
        /// ����SPID��ȡָ������
        /// ���ߣ�Է��      ʱ�䣺2009-7-31
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public string GetPropertyBySPID(string SPID, string PropertyName, object SPDataObj)
        {
            string propertyValue = "";
            if (SPDataObj == null)
                return propertyValue;
                        
            //string aa = SPDataObj.GetType().ToString() ;

            DataTable dt = null;
            if (SPDataObj.GetType().Name == "SPData")
            {
                SPData SPData = (SPData)SPDataObj;
                dt = SPData.Tables[SPData.TableName];
            }
            //else if (SPDataObj.GetType().Name == "SPIPListData")
            //{
            //    SPIPListData SPIPListData = (SPIPListData)SPDataObj;
            //    dt = SPIPListData.Tables[SPIPListData.TableName];
            //}
            //else if (SPDataObj.GetType().Name == "SPInterfaceLimitData")
            //{
            //    SPInterfaceLimitData SPInterfaceLimitData = (SPInterfaceLimitData)SPDataObj;
            //    dt = SPInterfaceLimitData.Tables[SPInterfaceLimitData.TableName];
            //}
            

            if (dt.Rows.Count == 0)
                return propertyValue;

            foreach (DataRow row in dt.Rows)
            {
                if (SPID == Convert.ToString(row[SPData.Field_SPID]))
                {
                    propertyValue = Convert.ToString(row[PropertyName]);
                    break;
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// ��ȡ֤����Ϣ
        /// ���ߣ�Է��      ʱ�䣺2009-7-31
        /// �޸ģ�          ʱ�䣺
        /// CerType:0(��Կ),1(˽Կ)
        /// </summary>
        public byte[] GetCAInfo(string SPID, int CerType, object SPDataObj,out string UserName,out string UserPassWord)
        {
            UserName = "";
            UserPassWord = "";

            byte[] Result = null;
            DataTable dt = new DataTable();
            SPCAData SPCAData = (SPCAData)SPDataObj;
            dt = SPCAData.Tables[SPCAData.TableName];

            if (dt.Rows.Count == 0)
                return Result;

            foreach (DataRow row in dt.Rows)
            {
                string str =Convert.ToString(row[SPData.Field_SPID]);
                int a = int.Parse(row[SPCAData.Field_CerType].ToString());
                if (SPID == Convert.ToString(row[SPData.Field_SPID]) & CerType == int.Parse(row[SPCAData.Field_CerType].ToString()))
                {
                    Result = (byte[]) row["CerInfo"];
                    UserName = (string)row["CerUserName"];
                    UserPassWord = (string)row["CerPassword"];
                    break;
                }
            }

            return Result;
        }

    }
}

/*********************************************************************************************************
 *     描述: 
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 苑峰
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 **********/


using System;
using System.Data;

namespace Linkage.BestTone.Interface.Rule
{
    public partial class SPData : DataSet
    {
        public const string TableName = "SPInfo";
        public const string Field_SPID = "SPID";
        public const string Field_SPName = "SPName";
        public const string Field_ProvinceID = "ProvinceID";
        public const string Field_SPType = "SPType";
        public const string Field_InterfaceUrl = "InterfaceUrl";
        public const string Field_InterfaceUrlV2 = "InterfaceUrlV2";
        public const string Field_SecretKey = "SecretKey";
        public const string Field_SPOuterID = "SPOuterID";

        public SPData()
        {
            this.buildSPDataTable();
        }

        private void buildSPDataTable()
        {
            DataTable tmpDataTable = new DataTable(TableName);
            tmpDataTable.Columns.Add(Field_SPID, typeof(System.String));
            tmpDataTable.Columns.Add(Field_SPName, typeof(System.String));
            tmpDataTable.Columns.Add(Field_ProvinceID, typeof(System.String));
            tmpDataTable.Columns.Add(Field_SPType, typeof(System.String));
            tmpDataTable.Columns.Add(Field_InterfaceUrl, typeof(System.String));
            tmpDataTable.Columns.Add(Field_InterfaceUrlV2, typeof(System.String));
            tmpDataTable.Columns.Add(Field_SecretKey, typeof(System.String));
            tmpDataTable.Columns.Add(Field_SPOuterID, typeof(System.String));

            this.Tables.Add(tmpDataTable);
        }

        

    }
}

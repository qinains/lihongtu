//==============================================================================================================
//
// Class Name: PEGProvinceData
// Description: PEG-省功能配置表
// Author: 苑峰
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-08
//
//==============================================================================================================
using System;
using System.Data;

namespace Linkage.BestTone.Interface.Rule
{
    public class ProvinceData : DataSet
    {
        public const string TableName = "Province";

        public const string Field_ProvinceID = "ProvinceID";
        public const string Field_ProvinceCode = "ProvinceCode";
        public const string Field_ProvinceName = "ProvinceName";
        public const string Field_ProvinceType = "ProvinceType";
        public const string Field_InterfaceURL = "InterfaceURL";
        public const string Field_RegionProvince = "RegionProvince";


        public ProvinceData()
        {
            this.buildPEGProvinceDataTable();
        }

        private void buildPEGProvinceDataTable()
        {
            DataTable tmpDataTable = new DataTable(TableName);

            tmpDataTable.Columns.Add(Field_ProvinceID, typeof(System.String));
            tmpDataTable.Columns.Add(Field_ProvinceCode, typeof(System.String));
            tmpDataTable.Columns.Add(Field_ProvinceName, typeof(System.String));
            tmpDataTable.Columns.Add(Field_ProvinceType, typeof(System.String));
            tmpDataTable.Columns.Add(Field_InterfaceURL, typeof(System.String));
            tmpDataTable.Columns.Add(Field_RegionProvince, typeof(System.String));
            
            this.Tables.Add(tmpDataTable);
        }
    }// End Class
}

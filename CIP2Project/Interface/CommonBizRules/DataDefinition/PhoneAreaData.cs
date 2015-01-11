//==============================================================================================================
//
// Class Name: AreaData
// Description: 地市信息表

// Author: 苑峰
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-08
//
//==============================================================================================================
using System;
using System.Data;

namespace Linkage.BestTone.Interface.Rule
{
    public class PhoneAreaData : DataSet
    {
        public const string TableName = "Area";

        public const string Field_PhoneAreaID = "AreaID";
        public const string Field_AreaName = "AreaName";
        public const string Field_ProvinceID = "ProvinceID";

        public PhoneAreaData()
        {
            this.buildAreaDataTable();
        }

        private void buildAreaDataTable()
        {
            DataTable tmpDataTable = new DataTable(TableName);

            tmpDataTable.Columns.Add(Field_PhoneAreaID, typeof(System.String));
            tmpDataTable.Columns.Add(Field_AreaName, typeof(System.String));
            tmpDataTable.Columns.Add(Field_ProvinceID, typeof(System.String));

            this.Tables.Add(tmpDataTable);
        }
    }// End Class
}

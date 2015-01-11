using System;
using System.Data;

namespace Linkage.BestTone.Interface.Rule
{
    public partial class SysData : DataSet
    {
        public const string TableName = "SysInfo";
        public const string Field_SysID = "SysID";
        public const string Field_SysName = "SysName";
        public const string Field_ProvinceID = "ProvinceID";
        public const string Field_SysType = "SysType";
        public const string Field_InterfaceUrl = "InterfaceUrl";
        public const string Field_InterfaceUrlV2 = "InterfaceUrlV2";

        public SysData()
        {
            this.buildSysDataTable();
        }

        private void buildSysDataTable()
        {
            DataTable tmpDataTable = new DataTable(TableName);

            tmpDataTable.Columns.Add(Field_SysID, typeof(System.String));
            tmpDataTable.Columns.Add(Field_SysName, typeof(System.String));

            tmpDataTable.Columns.Add(Field_ProvinceID, typeof(System.String));
            tmpDataTable.Columns.Add(Field_SysType, typeof(System.String));
            tmpDataTable.Columns.Add(Field_InterfaceUrl, typeof(System.String));

            tmpDataTable.Columns.Add(Field_InterfaceUrlV2, typeof(System.String));

            this.Tables.Add(tmpDataTable);
        }

    }
}

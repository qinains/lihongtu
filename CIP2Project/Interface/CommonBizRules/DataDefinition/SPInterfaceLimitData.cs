/*********************************************************************************************************
 *     ����: 
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

namespace Linkage.BestTone.Interface.Rule
{
    public partial class SPInterfaceLimitData : DataSet
    {
        public const string TableName = "SPInterfaceLimit";
        public const string Field_SPID = "SPID";
        public const string Field_InterfaceName = "InterfaceName";

        public SPInterfaceLimitData()
        {
            this.buildSPInterfaceLimitDataTable();
        }

        private void buildSPInterfaceLimitDataTable()
        {
            DataTable tmpDataTable = new DataTable(TableName);
            tmpDataTable.Columns.Add(Field_SPID, typeof(System.String));
            tmpDataTable.Columns.Add(Field_InterfaceName, typeof(System.String));

            this.Tables.Add(tmpDataTable);
        }

    }
}

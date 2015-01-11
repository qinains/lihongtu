using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class CustInfoDAO
    {

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// 组装分页查询sql 
        /// Author Lihongtu 
        /// CreateTime 2013-04-12
        /// </summary>
        /// <param name="OriginalSql"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private String EnvelopSql(String OriginalSql, long from, long to)
        {
            String select_fields = GetValue(OriginalSql, "select", "from");  // 获得select 和from之间的字段
            String orderby_fields = OriginalSql.Substring(OriginalSql.IndexOf("order by") + "order by ".Length); //获得order by 后面的字段
            String rownum_field = " Row_Number() over  (Order by " + orderby_fields + " ) as RowId";
            //在 OriginalSql Select ...后面插入rownum_field 
            Regex r = r = new Regex(select_fields);
            if (r.IsMatch(OriginalSql))
            {
                OriginalSql = r.Replace(OriginalSql, rownum_field + "," + select_fields);
            }
            r = new Regex("order by " + orderby_fields);
            if (r.IsMatch(OriginalSql))
            {
                OriginalSql = r.Replace(OriginalSql, "");
            }
            String outter_sql = "select *  from (" + OriginalSql + ")  U  where 1=1 and U.RowId between " + from + " and " + to;
            return outter_sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SourceSPID"></param>
        /// <param name="CustID"></param>
        /// <param name="FromIndex"></param>
        /// <param name="ToIndex"></param>
        /// <param name="FromDatetime"></param>
        /// <param name="ToDatetime"></param>
        /// <param name="OrderByMap"></param>
        /// <param name="cnt"></param>
        /// <param name="strLog"></param>
        /// <returns></returns>
        public DataSet QueryCustInfo(String SourceSPID, String CustID,  long FromIndex, long ToIndex, String FromDatetime, String ToDatetime,  Dictionary<String, String> OrderByMap, out Int32 cnt, out StringBuilder strLog)
        {
            #region 初始化变量
            cnt = 0;
            DataSet ds = null;
            strLog = new StringBuilder();
            StringBuilder sql = new StringBuilder();
              sql.Append("  select custid,provinceid,areaid,realname,custlevel,sex,username,createtime");
              sql.Append("  from custinfo with(nolock) where custtype in ('41','42') and status='00' ");
            StringBuilder count_sql = new StringBuilder();
            #endregion
            #region try
            try
            {
                #region 拼条件语句


                if (!String.IsNullOrEmpty(CustID))
                {
                    sql.Append(" and CustID='").Append(CustID).AppendFormat("'");
                }

                if (!String.IsNullOrEmpty(SourceSPID))
                {
                    sql.Append(" and SourceSPID='").Append(SourceSPID).AppendFormat("'");
                }


                if (!String.IsNullOrEmpty(FromDatetime) && !String.IsNullOrEmpty(ToDatetime))
                {
                    sql.Append(" and convert(varchar(10),createtime,120) >='").Append(FromDatetime).Append("' and convert(varchar(10),createtime,120)<='").Append(ToDatetime).Append("' ");
                }
                #endregion

                #region 统计行数

                count_sql.Append(" select count(*)  as cnt from ( ").Append(sql.ToString()).Append(" ) a  "); ;

                #endregion

                #region 拼排序语句
                StringBuilder orderby = new StringBuilder();
                String t_orderby = "";
                if (OrderByMap.Keys.Count > 0)
                {
                    orderby.Append(" order by ");
                    foreach (KeyValuePair<String, String> om in OrderByMap)
                    {
                        String OrderName = om.Key;
                        String OrderMethod = om.Value;
                        orderby.Append(OrderName);
                        if ("1".Equals(OrderMethod))
                        {
                            orderby.Append(" asc ");
                        }
                        else if ("2".Equals(OrderMethod))
                        {
                            orderby.Append(" desc ");
                        }
                        orderby.Append(",");
                    }
                }

                if (!String.IsNullOrEmpty(orderby.ToString()))
                {
                    t_orderby = orderby.ToString().Substring(0, orderby.ToString().Length - 1);   //去掉最后一个逗号
                }
                sql.Append(t_orderby);
                #endregion
                #region 执行查询

                strLog.AppendFormat("sql 语句:{0}\r\n", EnvelopSql(sql.ToString(), FromIndex, ToIndex));
                SqlCommand cmd = new SqlCommand(EnvelopSql(sql.ToString(), FromIndex, ToIndex));
                cmd.CommandType = CommandType.Text;
                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                #endregion

                #region 返回记录条数
                cmd = new SqlCommand(count_sql.ToString());
                cmd.CommandType = CommandType.Text;
                DataSet ds_count = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                
                if (ds_count != null && ds_count.Tables[0] != null && ds_count.Tables[0].Rows[0] != null)
                {
                    cnt = Convert.ToInt32(ds_count.Tables[0].Rows[0][0].ToString());
                }
                #endregion
            }
            catch (Exception e)
            {
                strLog.Append(e.ToString());
            }
            finally
            {
                strLog.AppendFormat("sql:{0}\r\n", sql.ToString());
            }
            #endregion
            return ds;
        }
    }
}

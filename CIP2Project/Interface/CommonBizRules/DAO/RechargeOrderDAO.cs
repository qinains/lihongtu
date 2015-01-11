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
    public class RechargeOrderDAO
    {
        private String Insert_Sql = "Insert into RechargeOrder (OrderSeq,PayTransactionID,OrderDate,CurType,OrderAmount,ProductAmount,AttachAmount,"
                                    + "OrderDesc,CustID,TargetAccount,RechargeType,SourceSPID,ReqTime,PayTime,CompleteTime,Status,RechargeCount,UptranSeq,ReturnCode,ReturnDesc,NeedInvoice)"
                                    + "values(@OrderSeq,@PayTransactionID,@OrderDate,@CurType,@OrderAmount,@ProductAmount,@AttachAmount,@OrderDesc,"
                                    + "@CustID,@TargetAccount,@RechargeType,@SourceSPID,@ReqTime,@PayTime,@CompleteTime,@Status,@RechargeCount,@UptranSeq,@ReturnCode,@ReturnDesc,@NeedInvoice)";

        private String Update_Sql = "Update RechargeOrder set OrderSeq = @OrderSeq,PayTransactionID=@PayTransactionID,OrderDate = @OrderDate, CurType = @CurType, "
                                    + "OrderAmount = @OrderAmount, ProductAmount = @ProductAmount,AttachAmount = @AttachAmount, OrderDesc = @OrderDesc,"
                                    + "CustID = @CustID, TargetAccount = @TargetAccount,RechargeType=@RechargeType,SourceSPID=@SourceSPID, ReqTime = @ReqTime,PayTime = @PayTime,CompleteTime = @CompleteTime,"
                                    + "Status = @Status,RechargeCount=@RechargeCount,UptranSeq=@UptranSeq,ReturnCode=@ReturnCode,ReturnDesc=@ReturnDesc where ID = @ID";

        private String Select_Sql = "Select ID,OrderSeq,PayTransactionID,OrderDate,CurType,OrderAmount,ProductAmount,AttachAmount,OrderDesc,CustID,TargetAccount,RechargeType,SourceSPID,ReqTime,PayTime,CompleteTime,Status,RechargeCount,UptranSeq,ReturnCode,ReturnDesc From RechargeOrder ";
        
        public bool Insert(RechargeOrder obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@PayTransactionID",obj.PayTransactionID),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@ProductAmount",obj.ProductAmount),
                    new SqlParameter("@AttachAmount",obj.AttachAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@TargetAccount",obj.TargetAccount),
                    new SqlParameter("@RechargeType",obj.RechargeType),
                    new SqlParameter("@SourceSPID",obj.SourceSPID),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@PayTime",obj.PayTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@RechargeCount",obj.RechargeCount),
                    new SqlParameter("@UptranSeq",obj.UptranSeq),
                    new SqlParameter("@ReturnCode",obj.ReturnCode),
                    new SqlParameter("@ReturnDesc",obj.ReturnDesc),
                    new SqlParameter("@NeedInvoice",obj.NeedInvoice)
                };

                SqlCommand cmd = new SqlCommand(Insert_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }

            return result;
        }

        public bool Update(RechargeOrder obj)
        {
            bool result = false;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ID",obj.ID),
                    new SqlParameter("@OrderSeq",obj.OrderSeq),
                    new SqlParameter("@PayTransactionID",obj.PayTransactionID),
                    new SqlParameter("@OrderDate",obj.OrderDate),
                    new SqlParameter("@CurType",obj.CurType),
                    new SqlParameter("@OrderAmount",obj.OrderAmount),
                    new SqlParameter("@ProductAmount",obj.ProductAmount),
                    new SqlParameter("@AttachAmount",obj.AttachAmount),
                    new SqlParameter("@OrderDesc",obj.OrderDesc),
                    new SqlParameter("@CustID",obj.CustID),
                    new SqlParameter("@TargetAccount",obj.TargetAccount),
                    new SqlParameter("@RechargeType",obj.RechargeType),
                    new SqlParameter("@SourceSPID",obj.SourceSPID),
                    new SqlParameter("@ReqTime",obj.ReqTime),
                    new SqlParameter("@PayTime",obj.PayTime),
                    new SqlParameter("@CompleteTime",obj.CompleteTime),
                    new SqlParameter("@Status",obj.Status),
                    new SqlParameter("@RechargeCount",obj.RechargeCount),
                    new SqlParameter("@UptranSeq",obj.UptranSeq),
                    new SqlParameter("@ReturnCode",obj.ReturnCode),
                    new SqlParameter("@ReturnDesc",obj.ReturnDesc)
                };

                SqlCommand cmd = new SqlCommand(Update_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            }
            catch { }

            return result;
        }

        #region 查询

        public RechargeOrder QueryByID(Int64 id)
        {
            RechargeOrder obj = null;
            try
            {
                String where = String.Format(" where ID = '{0}'", id);
                IList<RechargeOrder> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        public RechargeOrder QueryByOrderSeq(String orderSeq)
        {
            RechargeOrder obj = null;
            try
            {
                String where = String.Format(" where OrderSeq = '{0}'", orderSeq);
                IList<RechargeOrder> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }


        /// <summary>
        ///  CreateTime 2013-04-12
        ///  Author Lihongtu
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="BesttoneAccount"></param>
        /// <param name="RechargeTransactionID"></param>
        /// <param name="OrderSeq"></param>
        /// <param name="InvoiceCode"></param>
        /// <param name="InvoiceType"></param>
        /// <param name="InvoiceContent"></param>
        /// <param name="InvoiceTtitle"></param>
        /// <param name="FromIndex"></param>
        /// <param name="ToIndex"></param>
        /// <param name="fromDatetime"></param>
        /// <param name="toDatetime"></param>
        /// <param name="status"></param>
        /// <param name="OrderByMap"></param>
        /// <param name="strLog"></param>
        /// <returns></returns>
        public DataSet QueryInvoice(String CustID, String BesttoneAccount, String RechargeTransactionID, String OrderSeq, String InvoiceCode, String InvoiceType, String InvoiceContent, String InvoiceTitle, long FromIndex, long ToIndex, String FromDatetime, String ToDatetime, String Status, Dictionary<String, String> OrderByMap, out  Int32 cnt,out StringBuilder strLog)
        {
            cnt = 0;
            DataSet ds = null;
            strLog = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            sql.Append("select b.paytransactionid,a.orderseq,a.invoicecode,a.invoicetype,a.invoicetitle,a.invoicecontent, ");
            sql.Append(" a.contactperson ,a.contactphone,a.requesttime,b.custid,b.orderamount,b.needinvoice as status,b.targetaccount  ");
            sql.Append("from  invoice a left join rechargeorder b");
            sql.Append(" on  a.orderseq = b.orderseq where 1=1 and a.status<>'9'  ");   //过滤掉充值失败的，如果充值成功且需要充值，invoice的status 应为"0"
            StringBuilder count_sql = new StringBuilder();
            try
            {
                #region 拼条件语句

                if (!String.IsNullOrEmpty(CustID))
                {
                    sql.Append(" and b.custid='").Append(CustID).Append("'");
                }

                if (!String.IsNullOrEmpty(BesttoneAccount))
                {
                    sql.Append(" and b.targetaccount='").Append(BesttoneAccount).AppendFormat("'");
                }

                if (!String.IsNullOrEmpty(RechargeTransactionID))
                {
                    sql.Append(" and b.paytransactionid='").Append(RechargeTransactionID).Append("'");
                }

                if (!String.IsNullOrEmpty(OrderSeq))
                {
                    sql.Append(" and a.orderseq='").Append(OrderSeq).Append("'");
                }
                
                //InvoiceCode
                if (!String.IsNullOrEmpty(InvoiceCode))
                {
                    sql.Append(" and a.invoicecode='").Append(InvoiceCode).Append("'");
                }

                //InvoiceType
                if (!String.IsNullOrEmpty(InvoiceType))
                {
                    sql.Append(" and a.invoicetype='").Append(InvoiceType).Append("'");
                }

                //InvoiceContent
                if (!String.IsNullOrEmpty(InvoiceContent))
                {
                    sql.Append(" and a.invoicecontent like '%").Append(InvoiceContent).Append("%'");
                }

                //InvoiceTitle
                if (!String.IsNullOrEmpty(InvoiceTitle))
                {
                    sql.Append(" and a.invoicetitle like '%").Append(InvoiceTitle).Append("%'");
                }


                if (!String.IsNullOrEmpty(FromDatetime) && !String.IsNullOrEmpty(ToDatetime))
                {   
                    sql.Append(" and convert(varchar(10),a.requesttime,120) >='").Append(FromDatetime).Append("' and convert(varchar(10),a.requesttime,120)<='").Append(ToDatetime).Append("' ");
                }

                if (!String.IsNullOrEmpty(Status))
                {
                    sql.Append(" and b.needinvoice='").Append(Status).Append("'");
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
            return ds;
        }

        public DataSet GetInvoiceInfo(String OrderSeq, out StringBuilder strLog)
        {
            DataSet ds = null;
            strLog = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            sql.Append(" select ");
            sql.Append(" b.paytransactionid as RechargeTransactionID,");
            sql.Append(" a.OrderSeq,a.InvoiceCode,a.InvoiceType,a.InvoiceContent, ");
            sql.Append(" a.InvoiceTitle,a.ContactPerson,a.ContactPhone,a.RequestTime,b.CustID,b.orderamount as Amount, ");
            sql.Append(" a.Status,b.needInvoice,b.targetaccount as BesttoneAccount,a.Address,a.Zip, ");
            sql.Append(" a.Mem,a.Operator,a.operatetime,a.RefuseReason, ");
            sql.Append(" a.ExpressName,a.ExpressCode ");
            sql.Append(" from invoice a,rechargeorder b where a.orderseq = b.orderseq ");
            
            try
            {
                if (!String.IsNullOrEmpty(OrderSeq))
                {
                    sql.Append(" and a.orderseq='").Append(OrderSeq).Append("'");
                }
            }
            catch (Exception e)
            {
                strLog.Append(e.ToString());
            }
            #region 执行查询
            strLog.AppendFormat("sql 语句:{0}\r\n", sql.ToString());
            SqlCommand cmd = new SqlCommand(sql.ToString());
            cmd.CommandType = CommandType.Text;
            ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            #endregion
            return ds;
        }
        
        /// <summary>
        /// 复合查询
        /// Author:Lihongtu
        /// Createtime:2013-04-12
        /// </summary>
        /// <param name="orderSeq"></param>
        /// <returns></returns>
        public DataSet QueryByOrderSeqV2(String orderSeq,out StringBuilder strLog)
        {
            DataSet ds = null;
            strLog = new StringBuilder();

            StringBuilder sql = new StringBuilder();

            sql.Append(" select a.*,b.realname as custname from ");
            sql.Append(" ( ");
            sql.Append(" select a.PayTransactionID,'' as RechargeTransactionID ,a.OrderSeq, ");
            sql.Append(" '' as RechargeDate,a.reqtime, ");
            sql.Append(" case when b.paytime is null then c.paytime  ");
            sql.Append(" else b.paytime ");
            sql.Append(" end ");
            sql.Append(" as PayTime,a.completetime,a.custid, ");
            sql.Append(" a.rechargetype as RechargeSrc,b.cardno as cardno, ");
            sql.Append(" a.orderamount as amount,a.status,a.returncode,a.returndesc as returnmsg,a.targetaccount,'' as needinvoice ");
            sql.Append(" from rechargeorder a ");
            sql.Append(" left join  cardrechargerecord b on a.orderseq = b.orderseq ");
            sql.Append(" left join bankrechargerecord c on a.orderseq = c.orderseq ");
            sql.Append(" ) a,custinfo b with(nolock) where a.custid = b.custid ");
            
            try
            {
                if (!String.IsNullOrEmpty(orderSeq))
                {
                    sql.Append(" and a.orderseq='").Append(orderSeq).Append("'");
                }
            }
            catch (Exception e)
            {
                strLog.Append(e.ToString());
            }
            #region 执行查询
            strLog.AppendFormat("sql 语句:{0}\r\n", sql.ToString());
            SqlCommand cmd = new SqlCommand(sql.ToString());
            cmd.CommandType = CommandType.Text;
            ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            #endregion

            return ds;
        }



        public bool InsertInvoice(String OrderSeq, String InvoiceType, String InvoiceContent, String InvoiceTitle, String ContactPerson, String ContactPhone, String Address, String Zip, String Mem,String status, out StringBuilder strLog)
        {
            bool result = false;
            strLog = new StringBuilder();

            try
            {

                RechargeOrder ro = QueryByOrderSeq(OrderSeq);
                if (ro == null)
                {
                    strLog.AppendFormat("OrderSeq:{0}充值订单不存在!", OrderSeq);
                    return result;
                }

                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",OrderSeq),
                    new SqlParameter("@InvoiceType",InvoiceType),
                    new SqlParameter("@InvoiceContent",InvoiceContent),
                    new SqlParameter("@InvoiceTitle",InvoiceTitle),
                    new SqlParameter("@ContactPerson",ContactPerson),
                    new SqlParameter("@ContactPhone",ContactPhone),
                    new SqlParameter("@Address",Address),
                    new SqlParameter("@Zip",Zip),
                    new SqlParameter("@Mem",Mem),
                    new SqlParameter("@Status",status),   // 0-申请中
                    new SqlParameter("@RequestTime",DateTime.Now),
                    new SqlParameter("@OperateTime",DateTime.Now)
                };
                String Insert_Invoice_Sql = "Insert into Invoice (OrderSeq,InvoiceType,InvoiceContent,InvoiceTitle,ContactPerson,ContactPhone,Address,"
                                    + "Zip,Mem,Status,RequestTime,OperateTime) "
                                    + " values (@OrderSeq,@InvoiceType,@InvoiceContent,@InvoiceTitle,@ContactPerson,@ContactPhone,@Address,"
                                    + "@Zip,@Mem,@Status,@RequestTime,@OperateTime)";

                SqlCommand cmd = new SqlCommand(Insert_Invoice_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                //RechargeOrder 中 needvoice {0,不需开票,1需要开票,(申请)2,完成开票,3,拒绝开票}
                String Update_RechargeOrder_Sql = "Update RechargeOrder set needinvoice='1' where OrderSeq=@OrderSeq";
                parameters = new SqlParameter[] { new SqlParameter("@OrderSeq", OrderSeq) };
                cmd = new SqlCommand(Update_RechargeOrder_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

            }
            catch (Exception e) { strLog.Append(e.ToString()); }
            finally {
                log("InsertInvoice",strLog.ToString());
            }

            return result;
        }


        /// <summary>
        /// 开发票请求
        /// Author Lihongtu
        /// Createtime 2013-04-12
        /// </summary>
        /// <param name="OrderSeq"></param>
        /// <param name="InvoiceType"></param>
        /// <param name="InvoiceContent"></param>
        /// <param name="InvoiceTtitle"></param>
        /// <param name="ContactPerson"></param>
        /// <param name="ContactPhone"></param>
        /// <param name="Address"></param>
        /// <param name="Zip"></param>
        /// <param name="Mem"></param>
        /// <param name="Operator"></param>
        /// <param name="strLog"></param>
        /// <returns></returns>
        public bool InsertInvoice(String OrderSeq, String InvoiceType, String InvoiceContent, String InvoiceTitle, String ContactPerson, String ContactPhone, String Address, String Zip,String Mem,   out StringBuilder strLog)
        {
            bool result = false;
            strLog = new StringBuilder();

            try
            {

                RechargeOrder ro = QueryByOrderSeq(OrderSeq);
                if (ro == null)
                {
                    strLog.AppendFormat("OrderSeq:{0}充值订单不存在!",OrderSeq);
                    return result;
                }
                
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",OrderSeq),
                    new SqlParameter("@InvoiceType",InvoiceType),
                    new SqlParameter("@InvoiceContent",InvoiceContent),
                    new SqlParameter("@InvoiceTitle",InvoiceTitle),
                    new SqlParameter("@ContactPerson",ContactPerson),
                    new SqlParameter("@ContactPhone",ContactPhone),
                    new SqlParameter("@Address",Address),
                    new SqlParameter("@Zip",Zip),
                    new SqlParameter("@Mem",Mem),
                    new SqlParameter("@Status","0"),   // 0-申请中
                    new SqlParameter("@RequestTime",DateTime.Now),
                    new SqlParameter("@OperateTime",DateTime.Now)
                };
                String Insert_Invoice_Sql = "Insert into Invoice (OrderSeq,InvoiceType,InvoiceContent,InvoiceTitle,ContactPerson,ContactPhone,Address,"
                                    + "Zip,Mem,Status,RequestTime,OperateTime) "
                                    + " values (@OrderSeq,@InvoiceType,@InvoiceContent,@InvoiceTitle,@ContactPerson,@ContactPhone,@Address,"
                                    + "@Zip,@Mem,@Status,@RequestTime,@OperateTime)";

                SqlCommand cmd = new SqlCommand(Insert_Invoice_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                //RechargeOrder 中 needvoice {0,不需开票,1需要开票,(申请)2,完成开票,3,拒绝开票}
                String Update_RechargeOrder_Sql = "Update RechargeOrder set needinvoice='1' where OrderSeq=@OrderSeq";
                parameters = new SqlParameter[] { new SqlParameter("@OrderSeq", OrderSeq) };
                cmd = new SqlCommand(Update_RechargeOrder_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);           
            
            }
            catch (Exception e) { strLog.Append(e.ToString()); }

            return result;
        }


        public void UpdateInvoice(String OrderSeq,String status, out StringBuilder strLog)
        {
            
            strLog = new StringBuilder();
            strLog.Append("开始更新发票状态为申请中!\r\n");
            try
            {
                RechargeOrder ro = QueryByOrderSeq(OrderSeq);
                if (ro == null)
                {
                    strLog.AppendFormat("OrderSeq:{0}充值订单不存在!", OrderSeq);
                    return ;
                }

                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",OrderSeq),
                    new SqlParameter("@Status",status),   // 0-申请，1-完成
                    new SqlParameter("@OperateTime",DateTime.Now)
                };
                String Update_Invoice_Sql = " Update Invoice set Status=@Status,OperateTime=@OperateTime where OrderSeq=@OrderSeq";
                SqlCommand cmd = new SqlCommand(Update_Invoice_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                strLog.Append("更新发票状态为申请中完毕\r\n");

            }
            catch (Exception e) { strLog.Append(e.ToString()); }
          
        }


        /// <summary>
        /// 开发票接口
        /// Author Lihongtu
        /// Createtime 2013-04-12
        /// </summary>
        /// <param name="OrderSeq"></param>
        /// <param name="InvoiceCode"></param>
        /// <param name="Operator"></param>
        /// <param name="ExpressName"></param>
        /// <param name="ExpressCode"></param>
        /// <param name="strLog"></param>
        /// <returns></returns>
        public bool UpdateInvoice(String OrderSeq ,String InvoiceCode ,String Operator ,String ExpressName ,String ExpressCode ,out StringBuilder strLog)
        {
            bool result = false;
            strLog = new StringBuilder();
            try
            {
                RechargeOrder ro = QueryByOrderSeq(OrderSeq);
                if (ro == null)
                {
                    strLog.AppendFormat("OrderSeq:{0}充值订单不存在!", OrderSeq);
                    return result;
                }

                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",OrderSeq),
                    new SqlParameter("@InvoiceCode",InvoiceCode),
                    new SqlParameter("@Status","1"),   // 1-完成
                    new SqlParameter("@OperateTime",DateTime.Now),
                    new SqlParameter("@Operator",Operator),
                    new SqlParameter("@ExpressCode",ExpressCode),
                    new SqlParameter("@ExpressName",ExpressName)
                };
                String Update_Invoice_Sql = " Update Invoice set InvoiceCode=@InvoiceCode,Status=@Status,OperateTime=@OperateTime,Operator=@Operator,ExpressCode=@ExpressCode,ExpressName=@ExpressName where OrderSeq=@OrderSeq";
                SqlCommand cmd = new SqlCommand(Update_Invoice_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                //RechargeOrder 中 needvoice {0,不需开票,1需要开票,(申请)2,完成开票,3,拒绝开票}
                String Update_RechargeOrder_Sql = "Update RechargeOrder set needinvoice='2' where OrderSeq=@OrderSeq";
                parameters = new SqlParameter[] { new SqlParameter("@OrderSeq", OrderSeq) };
                cmd = new SqlCommand(Update_RechargeOrder_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
                
                //注意这里暂时没有事务保护，后加以完善
            }
            catch (Exception e) { strLog.Append(e.ToString()); }
            return result;
        }

        /// <summary>
        /// 拒绝开票
        /// Author Lihongtu
        /// Createtime 2013-04-12
        /// </summary>
        /// <param name="OrderSeq"></param>
        /// <param name="Operator"></param>
        /// <param name="RefuseReason"></param>
        /// <param name="strLog"></param>
        /// <returns></returns>
        public bool RefuseInvoice(String OrderSeq, String Operator, String RefuseReason,out StringBuilder strLog)
        {
            bool result = false;
            strLog = new StringBuilder();
            try
            {
                RechargeOrder ro = QueryByOrderSeq(OrderSeq);
                if (ro == null)
                {
                    strLog.AppendFormat("OrderSeq:{0}充值订单不存在!", OrderSeq);
                    return result;
                }
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@OrderSeq",OrderSeq),
                    new SqlParameter("@Status","2"),   // 1-拒绝
                    new SqlParameter("@OperateTime",DateTime.Now),
                    new SqlParameter("@Operator",Operator),
                    new SqlParameter("@RefuseReason",RefuseReason)
                };
                String Update_Invoice_Sql = " Update Invoice set Status=@Status,OperateTime=@OperateTime,Operator=@Operator,RefuseReason=@RefuseReason where OrderSeq=@OrderSeq";
                SqlCommand cmd = new SqlCommand(Update_Invoice_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);


                //RechargeOrder 中 needvoice {0,不需开票,1需要开票,(申请)2,完成开票,3,拒绝开票}
                String Update_RechargeOrder_Sql = "Update RechargeOrder set needinvoice='3' where OrderSeq=@OrderSeq";
                parameters = new SqlParameter[] { new SqlParameter("@OrderSeq", OrderSeq) };
                cmd = new SqlCommand(Update_RechargeOrder_Sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);
                result = DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                //注意这里暂时没有事务保护，后加以完善
            }
            catch (Exception e) { strLog.Append(e.ToString()); }
            return result;
        }


        public RechargeOrder QueryByOrderTransacntionID(String transactionid)
        {
            RechargeOrder obj = null;
            try
            {
                String where = String.Format(" where PayTransactionID = '{0}'", transactionid);
                IList<RechargeOrder> list = QueryByWhere(where);
                if (list != null && list.Count > 0)
                    obj = list[0];
            }
            catch (Exception ex) { throw ex; }

            return obj;
        }

        /// <summary>
        /// 查询账户当日充值成功金额数
        /// </summary>
        public long QueryCurrentRechargeAmount(String account)
        {
            long rechargeAmount = 0;
            String strSql = String.Format("select isnull(sum(OrderAmount),0) from RechargeOrder where OrderDate='{0}' and TargetAccount='{1}' and Status in (3,9)", DateTime.Now.ToString("yyyyMMdd"), account);
            SqlCommand cmd = new SqlCommand(strSql);
            cmd.CommandType = CommandType.Text;
            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                rechargeAmount = Convert.ToInt64(ds.Tables[0].Rows[0][0]);
            }
            return rechargeAmount;
        }

        public IList<RechargeOrder> QueryByCustID(String custid)
        {
            IList<RechargeOrder> list = null;
            try
            {
                String where = String.Format(" where custid = '{0}'", custid);
                list = QueryByWhere(where);
            }
            catch { }

            return list;
        }

        public IList<RechargeOrder> QueryByBestAccount(String account)
        {
            IList<RechargeOrder> list = null;
            try
            {
                String where = String.Format(" where TargetAccount = '{0}'", account);
                list = QueryByWhere(where);
            }
            catch { }

            return list;
        }


        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private  string GetValue(string str, string s, string e) 
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
        private String EnvelopSql(String OriginalSql,long from,long to)
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

 

        public DataSet QueryRechargeOrderV2(String BesttoneAccount, String CustID, String OrderSEQ, String RechargeTransactionID, long FromIndex, long ToIndex, String FromDatetime, String ToDatetime, String RechargeSrc, String Status, String NeedInvoice, Dictionary<String, String> OrderByMap,out Int32 cnt,out StringBuilder strLog)
        {
            cnt = 0;
            DataSet ds = null;
            strLog = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            sql.Append("select a.*,b.realname from (");
            sql.Append("select ro.paytransactionid,ro.orderseq,ro.completetime,ro.orderamount,ro.productamount,ro.attachamount,");
            sql.Append("ro.custid,ro.targetaccount,ro.status,ro.uptranseq,");
            sql.Append("ro.returncode,ro.returndesc,ro.needinvoice,card.cardno,ro.orderdesc,ro.rechargetype ");
            sql.Append("from rechargeorder ro left join  cardrechargerecord card on ro.orderseq = card.orderseq ) a,custinfo b with(nolock)");
            sql.Append("where a.custid = b.custid  ");

            StringBuilder count_sql = new StringBuilder();



            try
            {
                #region 拼条件语句


                if (!String.IsNullOrEmpty(OrderSEQ))
                {
                    sql.Append(" and a.orderseq='").Append(OrderSEQ).AppendFormat("'");
                }

                if (!String.IsNullOrEmpty(BesttoneAccount))
                {
                    sql.Append(" and a.targetaccount='").Append(BesttoneAccount).AppendFormat("'");
                }

                if (!String.IsNullOrEmpty(CustID))
                {
                    sql.Append(" and a.custid='").Append(CustID).Append("'");
                }

                if (!String.IsNullOrEmpty(RechargeTransactionID))
                {
                    sql.Append(" and a.paytransactionid='").Append(RechargeTransactionID).Append("'");
                }

                if (!String.IsNullOrEmpty(NeedInvoice))
                {
                    sql.Append(" and a.needinvoice='").Append(NeedInvoice).Append("'");
                }

                if (!String.IsNullOrEmpty(Status))
                {
                    sql.Append(" and a.status='").Append(Status).Append("'");
                }

                if (!String.IsNullOrEmpty(RechargeSrc))
                {
                    sql.Append(" and a.rechargetype='").Append(RechargeSrc).Append("'");
                }

                if (!String.IsNullOrEmpty(FromDatetime) && !String.IsNullOrEmpty(ToDatetime))
                {
                    sql.Append(" and convert(varchar(10),a.completetime,120) >='").Append(FromDatetime).Append("' and convert(varchar(10),a.completetime,120)<='").Append(ToDatetime).Append("' ");
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
               
                if (!String.IsNullOrEmpty(orderby.ToString())) {
                    t_orderby = orderby.ToString().Substring(0, orderby.ToString().Length-1);   //去掉最后一个逗号
                }
                sql.Append(t_orderby);
                #endregion
                #region 执行查询

                strLog.AppendFormat("sql 语句:{0}\r\n", EnvelopSql(sql.ToString(), FromIndex, ToIndex));
                SqlCommand cmd = new SqlCommand(EnvelopSql(sql.ToString(),FromIndex,ToIndex));
                cmd.CommandType = CommandType.Text;
                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                #endregion

                cmd = new SqlCommand(count_sql.ToString());
                cmd.CommandType = CommandType.Text;
                DataSet ds_count = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds_count != null && ds_count.Tables[0] != null && ds_count.Tables[0].Rows[0] != null)
                {
                   cnt =  Convert.ToInt32(ds_count.Tables[0].Rows[0][0].ToString());
                }

            }
            catch (Exception e)
            {
                strLog.Append(e.ToString());
            }
            finally
            {
                strLog.AppendFormat("sql:{0}\r\n",sql.ToString());
            }
            return ds;
        }


        public IList<RechargeOrder> QueryRechargeOrder(String BesttoneAccount, String CustID, String OrderSEQ, String RechargeTransactionID, long FromIndex, long ToIndex, String FromDatetime, String ToDatetime, String RechargeSrc, String Status, String NeedInvoice, Dictionary<String, String> OrderByMap)
        {
            StringBuilder strLog = new StringBuilder();

            IList<RechargeOrder> list = null;
            try
            {

                //select * from (select Row_Number() over     
                //(Order by targetaccount,orderdate) as RowId ,* from RechargeOrder ) U 
                //where U.RowId between 10 and 20

                StringBuilder sql = new StringBuilder();
                sql.Append("select ID ,OrderSeq ,PayTransactionID ,OrderDate ,CurType ,OrderAmount ,ProductAmount ,AttachAmount ,OrderDesc ,CustID ,TargetAccount ,RechargeType ,SourceSPID,ReqTime ,PayTime ,CompleteTime ,Status ,RechargeCount ,UptranSeq ,ReturnCode ,ReturnDesc from   (Select Row_Number() ");
                //over (Order by ");

                if (OrderByMap.Keys.Count > 0) 
                {
                    sql.Append(" over (Order by ");
                    foreach (KeyValuePair<String, String> om in OrderByMap)
                    {
                        String OrderName = om.Key;
                        String OrderMethod = om.Value;

                        sql.Append(OrderName + " ");  // 排序字段
                        if ("1".Equals(OrderMethod))
                        {
                            sql.Append(" asc ");
                        }
                        else if ("2".Equals(OrderMethod))
                        {
                            sql.Append(" desc ");
                        }
                    }
                    sql.Append(" ) ");
                }

                sql.Append(" as RowId, * from RechargeOrder where 1=1 ");

                if (!String.IsNullOrEmpty(BesttoneAccount))
                {
                    sql.Append(" and TargetAccount='").Append(BesttoneAccount).AppendFormat("'");
                }

                if (!String.IsNullOrEmpty(CustID))
                {
                    sql.Append(" and CustID='").Append(CustID).Append("'");
                }

                if (!String.IsNullOrEmpty(RechargeTransactionID))
                {
                    sql.Append(" and RechargeTransactionID='").Append(RechargeTransactionID).Append("'");
                }

                if (!String.IsNullOrEmpty(NeedInvoice))
                {
                    sql.Append(" and NeedInvoice='").Append(NeedInvoice).Append("'");
                }

                if (!String.IsNullOrEmpty(Status))
                {
                    sql.Append(" and Status='").Append(Status).Append("'");
                }

                if (!String.IsNullOrEmpty(RechargeSrc))
                {
                    sql.Append(" and RechargeSrc='").Append(RechargeSrc).Append("'");
                }

                if (!String.IsNullOrEmpty(FromDatetime) && !String.IsNullOrEmpty(ToDatetime))
                {
                    sql.Append(" and convert(varchar(10),CompleteTime,112) >='").Append(FromDatetime).Append("' and convert(varchar(10),CompleteTime,112)<='").Append(ToDatetime).Append("' ");
                }


                sql.AppendFormat(" ) R where 1=1  ");

                if (FromIndex != 0 && ToIndex != 0)
                {
                    sql.AppendFormat("  and R.RowId between {0} and {1} ",FromIndex,ToIndex);
                }
                

                strLog.Append(sql.ToString());

                SqlCommand cmd = new SqlCommand(sql.ToString());
                cmd.CommandType = CommandType.Text;
                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null && ds.Tables[0] != null)
                {
                    list = new List<RechargeOrder>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        RechargeOrder entity = new RechargeOrder();
                        entity.ID = Convert.ToInt64(row["ID"]);
                        entity.OrderSeq = row["OrderSeq"].ToString();
                        entity.PayTransactionID = row["PayTransactionID"].ToString();
                        entity.OrderDate = row["OrderDate"].ToString();
                        entity.CurType = row["CurType"].ToString();
                        entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                        entity.ProductAmount = Convert.ToInt64(row["ProductAmount"]);
                        entity.AttachAmount = Convert.ToInt64(row["AttachAmount"]);
                        entity.OrderDesc = row["OrderDesc"].ToString();
                        entity.CustID = row["CustID"].ToString();
                        entity.TargetAccount = row["TargetAccount"].ToString();
                        entity.RechargeType = row["RechargeType"].ToString();
                        entity.SourceSPID = row["SourceSPID"].ToString();
                        entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                        entity.PayTime = Convert.ToDateTime(row["PayTime"]);
                        entity.CompleteTime = Convert.ToDateTime(row["CompleteTime"]);
                        entity.Status = Convert.ToInt32(row["Status"]);
                        entity.RechargeCount = Convert.ToInt32(row["RechargeCount"]);
                        entity.UptranSeq = row["UptranSeq"].ToString();
                        entity.ReturnCode = row["ReturnCode"].ToString();
                        entity.ReturnDesc = row["ReturnDesc"].ToString();

                        list.Add(entity);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { 
            
            }
            return list;
        }


        /// <summary>
        /// 查询待充值的订单
        /// </summary>
        public IList<RechargeOrder> QueryWaitToRechargeOrder(Int32 count)
        {
            IList<RechargeOrder> list = null;

            try
            {
                String sql = String.Empty;
                if (count == 0)
                    sql = String.Format("Select * From RechargeOrder Where Status='2' Order by ReqTime", count);
                else
                    sql = String.Format("Select top {0} * From RechargeOrder Where Status='2' Order by ReqTime", count);

                SqlCommand cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;

                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null && ds.Tables[0] != null)
                {
                    list = new List<RechargeOrder>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        RechargeOrder entity = new RechargeOrder();
                        entity.ID = Convert.ToInt64(row["ID"]);
                        entity.OrderSeq = row["OrderSeq"].ToString();
                        entity.PayTransactionID = row["PayTransactionID"].ToString();
                        entity.OrderDate = row["OrderDate"].ToString();
                        entity.CurType = row["CurType"].ToString();
                        entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                        entity.ProductAmount = Convert.ToInt64(row["ProductAmount"]);
                        entity.AttachAmount = Convert.ToInt64(row["AttachAmount"]);
                        entity.OrderDesc = row["OrderDesc"].ToString();
                        entity.CustID = row["CustID"].ToString();
                        entity.TargetAccount = row["TargetAccount"].ToString();
                        entity.RechargeType = row["RechargeType"].ToString();
                        entity.SourceSPID = row["SourceSPID"].ToString();
                        entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                        entity.PayTime = Convert.ToDateTime(row["PayTime"]);
                        entity.CompleteTime = Convert.ToDateTime(row["CompleteTime"]);
                        entity.Status = Convert.ToInt32(row["Status"]);
                        entity.RechargeCount = Convert.ToInt32(row["RechargeCount"]);
                        entity.UptranSeq = row["UptranSeq"].ToString();
                        entity.ReturnCode = row["ReturnCode"].ToString();
                        entity.ReturnDesc = row["ReturnDesc"].ToString();

                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        #endregion

        #region 私有函数

        private IList<RechargeOrder> QueryByWhere(String where)
        {
            IList<RechargeOrder> list = null;

            String sql = Select_Sql.Insert(Select_Sql.Length, where);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;

            DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
            if (ds != null && ds.Tables[0] != null)
            {
                list = new List<RechargeOrder>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    RechargeOrder entity = new RechargeOrder();
                    entity.ID = Convert.ToInt64(row["ID"]);
                    entity.OrderSeq = row["OrderSeq"].ToString();
                    entity.PayTransactionID = row["PayTransactionID"].ToString();
                    entity.OrderDate = row["OrderDate"].ToString();
                    entity.CurType = row["CurType"].ToString();
                    entity.OrderAmount = Convert.ToInt64(row["OrderAmount"]);
                    entity.ProductAmount = Convert.ToInt64(row["ProductAmount"]);
                    entity.AttachAmount = Convert.ToInt64(row["AttachAmount"]);
                    entity.OrderDesc = row["OrderDesc"].ToString();
                    entity.CustID = row["CustID"].ToString();
                    entity.TargetAccount = row["TargetAccount"].ToString();
                    entity.RechargeType = row["RechargeType"].ToString();
                    entity.SourceSPID = row["SourceSPID"].ToString();
                    entity.ReqTime = Convert.ToDateTime(row["ReqTime"]);
                    entity.PayTime = Convert.ToDateTime(row["PayTime"]);
                    entity.CompleteTime = Convert.ToDateTime(row["CompleteTime"]);
                    entity.Status = Convert.ToInt32(row["Status"]);
                    entity.RechargeCount = Convert.ToInt32(row["RechargeCount"]);
                    entity.UptranSeq = row["UptranSeq"].ToString();
                    entity.ReturnCode = row["ReturnCode"].ToString();
                    entity.ReturnDesc = row["ReturnDesc"].ToString();


                    list.Add(entity);
                }
            }

            return list;
        }

        #endregion

        private static void log(string methodname, string content)
        {
            try
            {
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                msg.Append(content);
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
                BTUCenterInterfaceLog.CenterForBizTourLog("RechargeOrderDao" + @"\" + methodname, msg);
            }
            catch { }
        }


    }
}

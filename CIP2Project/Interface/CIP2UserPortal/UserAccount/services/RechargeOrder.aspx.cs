using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
public partial class UserAccount_services_RechargeOrder : System.Web.UI.Page
{


    //订单充值操作类
    RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
    //卡扣款记录操作类
    CardRechargeRecordDAO _cardRechargeRecord_dao = new CardRechargeRecordDAO();
    //充值操作类
    AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();

    CustInfoDAO _custInfo_dao = new CustInfoDAO();

   

    /// <summary>
    /// Author Lihongtu
    /// CreateTime 2013-04-12
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder returnMsg = new StringBuilder();
        StringBuilder strLog = new StringBuilder();
        String logName = String.Empty;
        try
        {
            String method = Request["Method"];
            String parameter = Request["Request"];
            if (String.IsNullOrEmpty(parameter))
            {
                System.IO.Stream content = Request.InputStream;
                System.IO.StreamReader sr = new System.IO.StreamReader(content);
                parameter = sr.ReadToEnd();
                sr.Close();
                content.Close();
            }
       
            switch (method)
            {
                //查询卡余额
                case "RechargeInfoQuery":
                    logName = "RechargeInfoQuery";
                    returnMsg.Append(RechargeInfoQuery(parameter,out strLog));
                    break;
                //卡充值
                case "GetRechargeInfo":
                    logName = "GetRechargeInfo";
                    returnMsg.Append(GetRechargeInfo(parameter,out strLog));
                    break;
                case "InvoiceInfoQuery":
                    logName = "InvoiceInfoQuery";
                    returnMsg.Append(InvoiceInfoQuery(parameter,out strLog));
                    break;
                case "GetInvoiceInfo":
                    logName = "GetInvoiceInfo";
                    returnMsg.Append(GetInvoiceInfo(parameter,out strLog));
                    break;
                case "InvoiceRequest":
                    logName = "InvoiceRequest";
                    returnMsg.Append(InvoiceRequest(parameter,out strLog));
                    break;
                case "InvoiceCheck":
                    logName = "InvoiceCheck";
                    returnMsg.Append(InvoiceCheck(parameter, out strLog));
                    break;
                case "InvoiceRefuse":
                    logName = "InvoiceRefuse";
                    returnMsg.Append(InvoiceRefuse(parameter, out strLog));
                    break;
                case "BesttoneCustInfoQuery":
                    logName = "BesttoneCustInfoQuery";
                    returnMsg.Append(BesttoneCustInfoQuery(parameter, out strLog));
                    break;
                default:
                    returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                    returnMsg.AppendFormat("<result returnCode=\"-9999\" msg ='方法名错误'/>");
                    break;
            }
        }
        catch (Exception ex)
        {
            returnMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            returnMsg.AppendFormat("<result returnCode=\"-9999\" msg ='"+ex.Message+"'/>");
            strLog.AppendFormat("，异常out：{0}", ex.Message);
        }
        finally
        {
            strLog.AppendFormat("返回报文:{0}\r\n",returnMsg.ToString());
            log(logName, strLog.ToString());
        }
        Response.Write(returnMsg.ToString());
        Response.Flush();
        Response.End();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String BesttoneCustInfoQuery(String parameter, out StringBuilder strLog)
    {
        #region 初始化变量
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;

        strLog = new StringBuilder();
        String returnMsg = String.Empty;

        String version = "";
        String SPID = "";
        String fromIndex = "";
        String rowCount = "";
        String CustID = "";
        String SourceSPID = "";     
        String fromDatetime = "";
        String _fromDatetime = "";
        String toDatetime = "";
        String _toDatetime = "";

        Dictionary<String, String> OrderByMap = new Dictionary<String, String>();

        #endregion
        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n", parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode fromIndexNode = xmlDoc.SelectNodes("/root/srchcond/fromIndex")[0];
            fromIndex = fromIndexNode.Attributes["value"].Value;

            XmlNode rowCountNode = xmlDoc.SelectNodes("/root/srchcond/rowCount")[0];
            String _rowCount = rowCountNode.Attributes["value"].Value;

            Regex r = new Regex(@"\[\d*\]$");
            if (r.IsMatch(_rowCount))
            {
                rowCount = _rowCount.Replace("[", "");
                rowCount = rowCount.Replace("]", "");
            }
            else
            {
                // 格式不对
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<root>";
                returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"rowCount格式不对！\" />";
                returnMsg = returnMsg + "</root>";
            }

            XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CustID")[0];
            CustID = CustIDNode.Attributes["value"].Value;

            XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/SourceSPID")[0];
            SourceSPID = BesttoneAccountNode.Attributes["value"].Value;

            XmlNode fromDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/fromDatetime")[0];
            fromDatetime = fromDatetimeNode.Attributes["value"].Value;

            XmlNode toDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/toDatetime")[0];
            toDatetime = toDatetimeNode.Attributes["value"].Value;

            XmlNodeList fieldsNodeList = xmlDoc.SelectNodes("/root/srchcond/sortFields/field");

            foreach (XmlNode node in fieldsNodeList)
            {
                String key = node.Attributes["value"].Value;
                String value = node.Attributes["desc"].Value;
                OrderByMap.Add(key, value);
            }
        }
        catch (Exception e)
        {
            // xml出错
            strLog.Append(e.ToString());
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<root>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            returnMsg = returnMsg + "</root>";
            return returnMsg;
        }

        #endregion
        #region 校验请求数据
        try
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(fromIndex))   // fromIndex 如果为空默认为1
            {
                fromIndex = "1";
            }

            if (String.IsNullOrEmpty(rowCount))   // 如果rowCount请求行数为空，默认20行
            {
                rowCount = System.Configuration.ConfigurationManager.AppSettings["RechargeQueryRowCount"];
                if (String.IsNullOrEmpty(rowCount))
                {
                    rowCount = "20";
                }
            }

            if (Convert.ToInt32(rowCount) > 100) //最大只能返回100行 
            {
                rowCount = "100";
            }

            if (!String.IsNullOrEmpty(fromDatetime))
            {
                if (IsDate(fromDatetime))
                {
                    _fromDatetime = Convert.ToDateTime(fromDatetime).ToString("yyyy-MM-dd");
                }
                else
                {
                    //非法日期格式
                    returnMsg = "";
                    returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                    returnMsg = returnMsg + "<result returnCode = \"-1004\" msg = \"fromDatetime非法日期格式！\" />";
                    return returnMsg;
                }
            }

            if (!String.IsNullOrEmpty(toDatetime))
            {
                if (IsDate(toDatetime))
                {
                    _toDatetime = Convert.ToDateTime(toDatetime).ToString("yyyy-MM-dd");
                }
                else
                {
                    //非法日期格式
                    returnMsg = "";
                    returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                    returnMsg = returnMsg + "<result returnCode = \"-1005\" msg = \"toDatetime非法日期格式！\" />";
                    return returnMsg;
                }
            }
        }
        catch (Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='" + e.ToString() + "'  />";
            return returnMsg;
        }


        #endregion
        #region 权限校验

        //IP是否允许访问
        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress + " ：IP地址未经授权!" + "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "BesttoneCustInfoQuery", this.Context, out ErrorDescription);

        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }
        #endregion

        #region dao执行Sql查询
        long _fromIndex = Convert.ToInt32(fromIndex);
        long _toIndex = _fromIndex + Convert.ToInt32(rowCount) - 1;   //rowCount 请求返回的行数
        StringBuilder daologstr = new StringBuilder();
        Int32 recordCount = 0;   // recordCount 实际返回的行数
        DataSet ds = _custInfo_dao.QueryCustInfo(SourceSPID, CustID, _fromIndex, _toIndex, _fromDatetime, _toDatetime, OrderByMap, out recordCount, out daologstr);
        strLog.Append(daologstr.ToString());
        #endregion

        #region 拼接返回xml
        //custid,provinceid,areaid,realname,custlevel,sex,username,createtime
        StringBuilder dataset = new StringBuilder();
        if (ds != null && ds.Tables[0] != null)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dataset.Append("<Set>");
                dataset.AppendFormat("<CustID>{0}</CustID>", row["custid"].ToString());
                dataset.AppendFormat("<ProvinceID>{0}</ProvinceID>", row["provinceid"].ToString());
                dataset.AppendFormat("<AreaID>{0}</AreaID>", row["areaid"].ToString());
                dataset.AppendFormat("<RealName>{0}</RealName>", row["realname"].ToString());
                dataset.AppendFormat("<CustLevel>{0}</CustLevel>", row["custlevel"].ToString());
                dataset.AppendFormat("<Sex>{0}</Sex>", row["sex"].ToString());
                dataset.AppendFormat("<UserName>{0}</UserName>", row["cardno"].ToString());
                dataset.AppendFormat("<CreateTime>{0}</CreateTime>", row["createtime"].ToString());
                dataset.Append("</Set>");
            }
        }

        #endregion
        #region 成功返回xml
        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" recordCount='" + Convert.ToString(recordCount) + "' from='" + Convert.ToString(_fromIndex) + "' to='" + Convert.ToString(_toIndex) + "'>";
        returnMsg = returnMsg + dataset.ToString();
        returnMsg = returnMsg + "</result>";
                #endregion

        return returnMsg;
    }


    /// <summary>
    /// 充值记录查询
    /// Author Lihongtu
    /// CreateTime 2013-04-12
    /// </summary>
    /// <param name="parameter">xml格式</param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String RechargeInfoQuery(String parameter,out StringBuilder strLog)
    {

        #region 初始化变量
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;

        strLog = new StringBuilder();
        String returnMsg = String.Empty;

        String version = "";
        String SPID = "";
        String fromIndex = "";
        String rowCount = "";
        String CustID = "";
        String BesttoneAccount = "";
        String RechargeTransactionID = "";
        String OrderSeq = "";
        String fromDatetime = "";
        String _fromDatetime = "";
        String toDatetime = "";
        String _toDatetime = "";
        String rechargeSRC = "";
        String status = "";
        String needInvoice = "";
        Dictionary<String, String> OrderByMap = new Dictionary<String, String>();

        #endregion

        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n",parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode fromIndexNode = xmlDoc.SelectNodes("/root/srchcond/fromIndex")[0];
            fromIndex = fromIndexNode.Attributes["value"].Value;

            XmlNode rowCountNode = xmlDoc.SelectNodes("/root/srchcond/rowCount")[0];
            String _rowCount = rowCountNode.Attributes["value"].Value;
            
            Regex r = new Regex(@"\[\d*\]$");
            if (r.IsMatch(_rowCount))
            {
                rowCount = _rowCount.Replace("[","");
                rowCount = rowCount.Replace("]","");
            }
            else
            { 
                // 格式不对
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<root>";
                returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"rowCount格式不对！\" />";
                returnMsg = returnMsg + "</root>";
            }
  
            XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CustID")[0];
            CustID = CustIDNode.Attributes["value"].Value;

            XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
            BesttoneAccount = BesttoneAccountNode.Attributes["value"].Value;

            XmlNode RechargeTransactionIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/RechargeTransactionID")[0];
            RechargeTransactionID = RechargeTransactionIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/srchcond/conds/OrderSeq")[0];
            OrderSeq = OrderSeqNode.Attributes["value"].Value;

            XmlNode fromDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/fromDatetime")[0];
            fromDatetime = fromDatetimeNode.Attributes["value"].Value;

            XmlNode toDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/toDatetime")[0];
            toDatetime = toDatetimeNode.Attributes["value"].Value;

            XmlNode rechargeSRCNode = xmlDoc.SelectNodes("/root/srchcond/conds/rechargeSRC")[0];
            rechargeSRC = rechargeSRCNode.Attributes["value"].Value;

            XmlNode statusNode = xmlDoc.SelectNodes("/root/srchcond/conds/status")[0];
            status = statusNode.Attributes["value"].Value;

            XmlNode needInvoiceNode = xmlDoc.SelectNodes("/root/srchcond/conds/needInvoice")[0];
            needInvoice = needInvoiceNode.Attributes["value"].Value;

            XmlNodeList fieldsNodeList = xmlDoc.SelectNodes("/root/srchcond/sortFields/field");
           
            foreach (XmlNode node in fieldsNodeList)
            {
                String key = node.Attributes["value"].Value;
                String value = node.Attributes["desc"].Value;
                OrderByMap.Add(key,value);
            }
        }
        catch (Exception e)
        { 
            // xml出错
            strLog.Append(e.ToString());
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<root>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            returnMsg = returnMsg + "</root>";
            return returnMsg;
        }

        #endregion

        #region 校验请求数据
        try 
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                  return returnMsg;
            }

            if (String.IsNullOrEmpty(fromIndex))   // fromIndex 如果为空默认为1
            {
                fromIndex = "1";
            }

            if (String.IsNullOrEmpty(rowCount))   // 如果rowCount请求行数为空，默认20行
            {
                rowCount =System.Configuration.ConfigurationManager.AppSettings["RechargeQueryRowCount"];
                if (String.IsNullOrEmpty(rowCount))
                {
                    rowCount = "20";
                }
            }

            if (!String.IsNullOrEmpty(fromDatetime))
            {
                if (IsDate(fromDatetime))
                {
                    _fromDatetime = Convert.ToDateTime(fromDatetime).ToString("yyyy-MM-dd");
                }
                else
                { 
                    //非法日期格式
                    returnMsg = "";
                    returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                    returnMsg = returnMsg + "<result returnCode = \"-1004\" msg = \"fromDatetime非法日期格式！\" />";
                    return returnMsg;
                }
            }

            if (!String.IsNullOrEmpty(toDatetime))
            {
                if (IsDate(toDatetime))
                {
                    _toDatetime = Convert.ToDateTime(toDatetime).ToString("yyyy-MM-dd");
                }
                else
                {
                    //非法日期格式
                    returnMsg = "";
                    returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                    returnMsg = returnMsg + "<result returnCode = \"-1005\" msg = \"toDatetime非法日期格式！\" />";
                    return returnMsg;
                }
            }
        }
        catch(Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='"+e.ToString()+"'  />";
            return returnMsg;
        }


        #endregion

        #region 权限校验
       
        //IP是否允许访问
        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress +" ：IP地址未经授权!"+ "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "RechargeInfoQuery", this.Context, out ErrorDescription);
        
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }
        #endregion

        #region dao执行Sql查询
        long _fromIndex = Convert.ToInt32(fromIndex);
        long _toIndex = _fromIndex + Convert.ToInt32(rowCount) -1 ;   //rowCount 请求返回的行数
        StringBuilder daologstr = new StringBuilder();
        Int32 recordCount = 0;   // recordCount 实际返回的行数
        DataSet ds = _rechargeOrder_dao.QueryRechargeOrderV2(BesttoneAccount, CustID, OrderSeq, RechargeTransactionID, _fromIndex, _toIndex, _fromDatetime, _toDatetime, rechargeSRC, status, needInvoice, OrderByMap, out recordCount, out daologstr);
        strLog.Append(daologstr.ToString());
        #endregion

        #region 拼接返回xml
       
        StringBuilder dataset = new StringBuilder();
        if (ds != null && ds.Tables[0] != null)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dataset.Append("<Set>");
                dataset.AppendFormat("<RechargeTransactionID>{0}</RechargeTransactionID>", row["paytransactionid"].ToString());
                dataset.AppendFormat("<OrderSeq>{0}</OrderSeq>", row["orderseq"].ToString());
                dataset.AppendFormat("<RechargeDate>{0}</RechargeDate>", row["completetime"].ToString());
                dataset.AppendFormat("<CustID>{0}</CustID>", row["custid"].ToString());
                dataset.AppendFormat("<CustName>{0}</CustName>", row["realname"].ToString());
                dataset.AppendFormat("<RechargeSrc>{0}</RechargeSrc>", row["RechargeType"].ToString());
                dataset.AppendFormat("<CardNo>{0}</CardNo>", row["cardno"].ToString()); //卡号
                dataset.AppendFormat("<Amount>{0}</Amount>", row["orderamount"].ToString());
                dataset.AppendFormat("<Status>{0}</Status>", row["status"].ToString());
                dataset.AppendFormat("<ReturnCode>{0}</ReturnCode>", row["returncode"].ToString());
                dataset.AppendFormat("<ReturnMsg>{0}</ReturnMsg>", row["returndesc"].ToString());
                dataset.AppendFormat("<BesttoneAccount>{0}</BesttoneAccount>", row["targetaccount"].ToString());
                dataset.AppendFormat("<InvoiceRequest>{0}</InvoiceRequest>", row["needinvoice"].ToString()); //需新增字段
                dataset.Append("</Set>");
            }
        }

        #endregion

        #region 成功返回xml
        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" recordCount='"+Convert.ToString(recordCount)+"' from='"+Convert.ToString(_fromIndex)+"' to='"+Convert.ToString(_toIndex)+"'>";
        returnMsg = returnMsg + dataset.ToString();
        returnMsg = returnMsg + "</result>";
        #endregion

        return returnMsg;
    
    }

    /// <summary>
    ///  获取充值信息
    /// Author Lihongtu
    /// CreateTime 2013-04-12
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String GetRechargeInfo(String parameter,out StringBuilder strLog)
    {
        #region 初始化变量
        strLog = new StringBuilder();
        String returnMsg = String.Empty;
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        String version = "";
        String SPID = "";
        String OrderSeq = "";
        #endregion

        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n", parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/srchcond/conds/OrderSeq")[0];
            OrderSeq = OrderSeqNode.Attributes["value"].Value;
        }
        catch (Exception e)
        {
            // xml出错
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<root>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            returnMsg = returnMsg + "</root>";
            return returnMsg;
        }
        #endregion

        #region 校验请求数据
        try
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                return returnMsg;
            }
   
        }
        catch (Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='" + e.ToString() + "'  />";
            return returnMsg;
        }

        #endregion

        #region 权限校验

        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress + " ：IP地址未经授权!" + "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "RechargeInfoQuery", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }

        #endregion

        #region 执行dao查询

        StringBuilder daologstr = new StringBuilder();
        DataSet ds = _rechargeOrder_dao.QueryByOrderSeqV2(OrderSeq, out daologstr);
        strLog.Append(daologstr.ToString());

        #endregion


        #region 拼接返回 xml

        Int32 recordCount = 0;   // recordCount 实际返回的行数
        StringBuilder dataset = new StringBuilder();
        if (ds != null && ds.Tables[0] != null)
        {
            recordCount = ds.Tables[0].Rows.Count;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dataset.Append("<ID></ID>");
                dataset.AppendFormat("<PayTransactionID>{0}</PayTransactionID>", row["PayTransactionID"].ToString());
                dataset.AppendFormat("<RechargeTransactionID>{0}</RechargeTransactionID>", row["RechargeTransactionID"].ToString());
                dataset.AppendFormat("<OrderSeq>{0}</OrderSeq>", row["OrderSeq"].ToString());
                dataset.AppendFormat("<RechargeDate>{0}</RechargeDate>", row["RechargeDate"].ToString());
                dataset.AppendFormat("<ReqTime>{0}</ReqTime>", row["reqtime"].ToString());
                dataset.AppendFormat("<PayTime>{0}</PayTime>", row["paytime"].ToString());
                dataset.AppendFormat("<CompleteTime>{0}</CompleteTime>", row["completetime"].ToString()); //卡号
                dataset.AppendFormat("<CustID>{0}</CustID>", row["custid"].ToString());
                dataset.AppendFormat("<CustName>{0}</CustName>", row["custname"].ToString());
                dataset.AppendFormat("<RechargeSrc>{0}</RechargeSrc>", row["RechargeSrc"].ToString()); //?
                dataset.AppendFormat("<CardNo>{0}</CardNo>", row["cardno"].ToString());
                dataset.AppendFormat("<Amount>{0}</Amount>", row["amount"].ToString());  //?
                dataset.AppendFormat("<Status>{0}</Status>", row["status"].ToString());
                dataset.AppendFormat("<ReturnCode>{0}</ReturnCode>", row["returncode"].ToString());
                dataset.AppendFormat("<ReturnMsg>{0}</ReturnMsg>", row["returndesc"].ToString());
                dataset.AppendFormat("<BesttoneAccount>{0}</BesttoneAccount>", row["targetaccount"].ToString());
                dataset.AppendFormat("<InvoiceRequest>{0}</InvoiceRequest>", row["needinvoice"].ToString());   //字段没有
            }
        }

        #endregion


        #region 成功返回xml

        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" >";
        returnMsg = returnMsg + dataset.ToString();
        returnMsg = returnMsg + "</result>";
        #endregion

        return returnMsg;
    }

    /// <summary>
    /// 开发票记录查询接口
    /// Author Lihongtu
    /// CreateTime 2013-04-12
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String InvoiceInfoQuery(String parameter,out StringBuilder strLog)
    {
        #region 初始化变量
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;

        strLog = new StringBuilder();
        String returnMsg = String.Empty;

        String version = "";
        String SPID = "";
        String fromIndex = "";
        String rowCount = "";
        String CustID = "";
        String BesttoneAccount = "";
        String RechargeTransactionID = "";
        String OrderSeq = "";

        String InvoiceCode = "";
        String InvoiceType = "";
        String InvoiceContent = "";
        String InvoiceTitle = "";

        String fromDatetime = "";
        String _fromDatetime = "";
        String toDatetime = "";
        String _toDatetime = "";
        String status = "";

        Dictionary<String, String> OrderByMap = new Dictionary<String, String>();

        #endregion

        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n", parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode fromIndexNode = xmlDoc.SelectNodes("/root/srchcond/fromIndex")[0];
            fromIndex = fromIndexNode.Attributes["value"].Value;

            XmlNode rowCountNode = xmlDoc.SelectNodes("/root/srchcond/rowCount")[0];
            String _rowCount = rowCountNode.Attributes["value"].Value;

            Regex r = new Regex(@"\[\d*\]$");
            if (r.IsMatch(_rowCount))
            {
                rowCount = _rowCount.Replace("[", "");
                rowCount = rowCount.Replace("]", "");
            }
            else
            {
                // 格式不对
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"rowCount格式不对！\" />";
            }

            XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CustID")[0];
            CustID = CustIDNode.Attributes["value"].Value;

            XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
            BesttoneAccount = BesttoneAccountNode.Attributes["value"].Value;

            XmlNode RechargeTransactionIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/RechargeTransactionID")[0];
            RechargeTransactionID = RechargeTransactionIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/srchcond/conds/OrderSeq")[0];
            OrderSeq = OrderSeqNode.Attributes["value"].Value;

            XmlNode InvoiceCodeNode = xmlDoc.SelectNodes("/root/srchcond/conds/InvoiceCode")[0];
            InvoiceCode = InvoiceCodeNode.Attributes["value"].Value;

            XmlNode InvoiceTypeNode = xmlDoc.SelectNodes("/root/srchcond/conds/InvoiceType")[0];
            InvoiceType = InvoiceTypeNode.Attributes["value"].Value;

            XmlNode InvoiceContentNode = xmlDoc.SelectNodes("/root/srchcond/conds/InvoiceContent")[0];
            InvoiceContent = InvoiceContentNode.Attributes["value"].Value;

            XmlNode InvoiceTitleNode = xmlDoc.SelectNodes("/root/srchcond/conds/InvoiceTitle")[0];
            InvoiceTitle = InvoiceTitleNode.Attributes["value"].Value;

            XmlNode fromDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/fromDatetime")[0];
            fromDatetime = fromDatetimeNode.Attributes["value"].Value;

            XmlNode toDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/toDatetime")[0];
            toDatetime = toDatetimeNode.Attributes["value"].Value;

            XmlNode statusNode = xmlDoc.SelectNodes("/root/srchcond/conds/status")[0];
            status = statusNode.Attributes["value"].Value;

            XmlNodeList fieldsNodeList = xmlDoc.SelectNodes("/root/srchcond/sortFields/field");

            foreach (XmlNode node in fieldsNodeList)
            {
                String key = node.Attributes["value"].Value;
                String value = node.Attributes["desc"].Value;
                OrderByMap.Add(key, value);
            }
        }
        catch (Exception e)
        {
            // xml出错
            strLog.Append(e.ToString());
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            return returnMsg;
        }

        #endregion

        #region 校验请求数据
        try
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(fromIndex))   // fromIndex 如果为空默认为1
            {
                fromIndex = "1";
            }

            if (String.IsNullOrEmpty(rowCount))   // 如果rowCount请求行数为空，默认20行
            {
                rowCount = System.Configuration.ConfigurationManager.AppSettings["RechargeQueryRowCount"];
                if (String.IsNullOrEmpty(rowCount))
                {
                    rowCount = "20";
                }
            }

            if (!String.IsNullOrEmpty(fromDatetime))
            {
                if (IsDate(fromDatetime))
                {
                    _fromDatetime = Convert.ToDateTime(fromDatetime).ToString("yyyy-MM-dd");
                }
                else
                {
                    //非法日期格式
                    returnMsg = "";
                    returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                    returnMsg = returnMsg + "<result returnCode = \"-1004\" msg = \"fromDatetime非法日期格式！\" />";
                    return returnMsg;
                }
            }

            if (!String.IsNullOrEmpty(toDatetime))
            {
                if (IsDate(toDatetime))
                {
                    _toDatetime = Convert.ToDateTime(toDatetime).ToString("yyyy-MM-dd");
                }
                else
                {
                    //非法日期格式
                    returnMsg = "";
                    returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                    returnMsg = returnMsg + "<result returnCode = \"-1005\" msg = \"toDatetime非法日期格式！\" />";
                    return returnMsg;
                }
            }
        }
        catch (Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='" + e.ToString() + "'  />";
            return returnMsg;
        }


        #endregion

        #region 权限校验

        //IP是否允许访问
        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress + " ：IP地址未经授权!" + "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "InvoiceInfoQuery", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }
        #endregion

        #region dao执行Sql查询
        long _fromIndex = Convert.ToInt32(fromIndex);
        long _toIndex = _fromIndex + Convert.ToInt32(rowCount) - 1;   //rowCount 请求返回的行数
        StringBuilder daologstr = new StringBuilder();
        Int32 recordCount = 0;   // recordCount 实际返回的行数
        DataSet ds = _rechargeOrder_dao.QueryInvoice(CustID, BesttoneAccount, RechargeTransactionID, OrderSeq, InvoiceCode, InvoiceType, InvoiceContent, InvoiceTitle, _fromIndex, _toIndex, _fromDatetime, _toDatetime, status, OrderByMap, out recordCount, out daologstr);
        strLog.Append(daologstr.ToString());
        #endregion

        #region 拼接返回xml
        
        StringBuilder dataset = new StringBuilder();
        if (ds != null && ds.Tables[0] != null)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dataset.Append("<Set>");
                dataset.Append("<ID></ID>");
                dataset.AppendFormat("<RechargeTransactionID>{0}</RechargeTransactionID>", row["paytransactionid"].ToString());
                dataset.AppendFormat("<OrderSeq>{0}</OrderSeq>", row["orderseq"].ToString());
                dataset.AppendFormat("<InvoiceCode>{0}</InvoiceCode>", row["invoicecode"].ToString());
                dataset.AppendFormat("<InvoiceType>{0}</InvoiceType>", row["invoicetype"].ToString());
                dataset.AppendFormat("<InvoiceContent>{0}</InvoiceContent>", row["invoicecontent"].ToString());
                dataset.AppendFormat("<InvoiceTitle>{0}</InvoiceTitle>", row["invoicetitle"].ToString());
                dataset.AppendFormat("<ContactPerson>{0}</ContactPerson>", row["contactperson"].ToString()); //卡号
                dataset.AppendFormat("<ContactPhone>{0}</ContactPhone>", row["contactphone"].ToString());
                dataset.AppendFormat("<RequestTime>{0}</RequestTime>", row["requesttime"].ToString());
                dataset.AppendFormat("<CustID>{0}</CustID>", row["custid"].ToString());
                dataset.AppendFormat("<Amount>{0}</Amount>", row["orderamount"].ToString());
                dataset.AppendFormat("<Status>{0}</Status>", row["status"].ToString());
                dataset.AppendFormat("<BesttoneAccount>{0}</BesttoneAccount>", row["targetaccount"].ToString());
                dataset.Append("</Set>");
            }
        }

        #endregion

        #region 成功返回xml
        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" recordCount='" + Convert.ToString(recordCount) + "' from='" + Convert.ToString(_fromIndex) + "' to='" + Convert.ToString(_toIndex) + "'>";
        returnMsg = returnMsg + dataset.ToString();
        returnMsg = returnMsg + "</result>";
        #endregion

        return returnMsg;
    }

    /// <summary>
    /// 获取开发票信息
    /// Author Lihongtu
    /// Createtime 2013-04-13
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String GetInvoiceInfo(String parameter,out StringBuilder strLog)
    {
        #region 初始化变量
        strLog = new StringBuilder();
        String returnMsg = String.Empty;
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        String version = "";
        String SPID = "";
        String OrderSeq = "";
        #endregion

        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n", parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/srchcond/conds/OrderSeq")[0];
            OrderSeq = OrderSeqNode.Attributes["value"].Value;
        }
        catch (Exception e)
        {
            // xml出错
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<root>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            returnMsg = returnMsg + "</root>";
            return returnMsg;
        }
        #endregion

        #region 校验请求数据
        try
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                return returnMsg;
            }

        }
        catch (Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='" + e.ToString() + "'  />";
            return returnMsg;
        }

        #endregion

        #region 权限校验

        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress + " ：IP地址未经授权!" + "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "GetInvoiceInfo", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }

        #endregion

        #region 执行dao查询

        StringBuilder daologstr = new StringBuilder();
        DataSet ds = _rechargeOrder_dao.GetInvoiceInfo(OrderSeq, out daologstr);
        strLog.Append(daologstr.ToString());

        #endregion

        #region 拼接返回 xml

        Int32 recordCount = 0;   // recordCount 实际返回的行数
        StringBuilder dataset = new StringBuilder();
        if (ds != null && ds.Tables[0] != null)
        {
            recordCount = ds.Tables[0].Rows.Count;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
          
                dataset.AppendFormat("<RechargeTransactionID>{0}</RechargeTransactionID>", row["RechargeTransactionID"].ToString());
                dataset.AppendFormat("<OrderSeq>{0}</OrderSeq>", row["OrderSeq"].ToString());
                dataset.AppendFormat("<InvoiceCode>{0}</InvoiceCode>", row["InvoiceCode"].ToString());
                dataset.AppendFormat("<InvoiceType>{0}</InvoiceType>", row["InvoiceType"].ToString());
                dataset.AppendFormat("<InvoiceContent>{0}</InvoiceContent>", row["InvoiceContent"].ToString());
                dataset.AppendFormat("<InvoiceTitle>{0}</InvoiceTitle>", row["InvoiceTitle"].ToString()); //卡号
                dataset.AppendFormat("<ContactPerson>{0}</ContactPerson>", row["ContactPerson"].ToString());
                dataset.AppendFormat("<ContactPhone>{0}</ContactPhone>", row["ContactPhone"].ToString());
                dataset.AppendFormat("<RequestTime>{0}</RequestTime>", row["RequestTime"].ToString()); //?
                dataset.AppendFormat("<CustID>{0}</CustID>", row["CustID"].ToString());
                dataset.AppendFormat("<Amount>{0}</Amount>", row["Amount"].ToString());  //?
                dataset.AppendFormat("<Status>{0}</Status>", row["needInvoice"].ToString());
                dataset.AppendFormat("<Address>{0}</Address>", row["Address"].ToString());
                dataset.AppendFormat("<BesttoneAccount>{0}</BesttoneAccount>", row["BesttoneAccount"].ToString());
                dataset.AppendFormat("<Zip>{0}</Zip>", row["Zip"].ToString());
                dataset.AppendFormat("<Mem>{0}</Mem>", row["Mem"].ToString());
                dataset.AppendFormat("<Operator>{0}</Operator>", row["Operator"].ToString());
                dataset.AppendFormat("<Operatetime>{0}</Operatetime>", row["operatetime"].ToString());
                dataset.AppendFormat("<RefuseReason>{0}</RefuseReason>", row["RefuseReason"].ToString());
                dataset.AppendFormat("<ExpressName>{0}</ExpressName>", row["ExpressName"].ToString());
                dataset.AppendFormat("<ExpressCode>{0}</ExpressCode>", row["ExpressCode"].ToString());

            }
        }

        #endregion

        #region 成功返回xml

        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" >";
        returnMsg = returnMsg + dataset.ToString();
        returnMsg = returnMsg + "</result>";
        #endregion

        return returnMsg;
    }

    /// <summary>
    /// 开发票申请接口
    /// Author Lihongtu
    /// Createtime 2013-04-12
    /// 写入动作
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String InvoiceRequest(String parameter,out StringBuilder strLog)
    {
        #region 初始化变量
        strLog = new StringBuilder();
        String returnMsg = String.Empty;
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        String version = "";
        String SPID = "";
        String OrderSeq = "";
        String InvoiceType = "";
        String InvoiceContent = "";
        String InvoiceTitle = "";
        String ContactPerson = "";
        String ContactPhone = "";
        String Address = "";
        String Zip = "";
        String Mem = "";
      
        #endregion

        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n", parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/Data/conds/OrderSeq")[0];
            OrderSeq = OrderSeqNode.Attributes["value"].Value;

            XmlNode InvoiceTypeNode = xmlDoc.SelectNodes("/root/Data/values/InvoiceType")[0];
            InvoiceType = InvoiceTypeNode.Attributes["value"].Value;

            XmlNode InvoiceContentNode = xmlDoc.SelectNodes("/root/Data/values/InvoiceContent")[0];
            InvoiceContent = InvoiceContentNode.Attributes["value"].Value;

            XmlNode InvoiceTitleNode = xmlDoc.SelectNodes("/root/Data/values/InvoiceTitle")[0];
            InvoiceTitle = InvoiceTitleNode.Attributes["value"].Value;

            XmlNode ContactPersonNode = xmlDoc.SelectNodes("/root/Data/values/ContactPerson")[0];
            ContactPerson = ContactPersonNode.Attributes["value"].Value;

            XmlNode ContactPhoneNode = xmlDoc.SelectNodes("/root/Data/values/ContactPhone")[0];
            ContactPhone = ContactPhoneNode.Attributes["value"].Value;

            XmlNode AddressNode = xmlDoc.SelectNodes("/root/Data/values/Address")[0];
            Address = AddressNode.Attributes["value"].Value;

            XmlNode ZipNode = xmlDoc.SelectNodes("/root/Data/values/Zip")[0];
            Zip = ZipNode.Attributes["value"].Value;

            XmlNode MemNode = xmlDoc.SelectNodes("/root/Data/values/Mem")[0];
            Mem = MemNode.Attributes["value"].Value;


        }
        catch (Exception e)
        {
            // xml出错
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            return returnMsg;
        }
        #endregion

        #region 校验请求数据
        try
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(OrderSeq))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"充值订单号不能为空！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(InvoiceType))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"发票类型不能为空！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(InvoiceContent))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"发票内容不能为空！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(InvoiceTitle))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"发票抬头不能为空！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(ContactPerson))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"联系人不能为空！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(ContactPhone))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"联系电话不能为空！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(Address))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"地址不能为空！\" />";
                return returnMsg;
            }
  
        }
        catch (Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='" + e.ToString() + "'  />";
            return returnMsg;
        }

        #endregion

        #region 权限校验

        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress + " ：IP地址未经授权!" + "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "InvoiceRequest", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }

        #endregion

        #region 执行dao查询

        StringBuilder daologstr = new StringBuilder();
    
        bool flag = _rechargeOrder_dao.InsertInvoice(OrderSeq, InvoiceType, InvoiceContent, InvoiceTitle, ContactPerson, ContactPhone, Address, Zip, Mem, out daologstr);

        strLog.Append(daologstr.ToString());

        #endregion

        #region 成功返回xml

        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        if (flag)
        {
            returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" >";
        }
        else
        {
            returnMsg = returnMsg + "<result returnCode = \"-9010\" msg = \"开票请求失败\" >";
        }
        returnMsg = returnMsg + "</result>";
        #endregion

        return returnMsg;
    }

    /// <summary>
    /// 开发票接口
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String InvoiceCheck(String parameter, out StringBuilder strLog)
    {
        #region 初始化变量
        strLog = new StringBuilder();
        String returnMsg = String.Empty;
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        String version = "";
        String SPID = "";
        String OrderSeq = "";
        String InvoiceCode = "";
        String Operator = "";
        String ExpressName = "";
        String ExpressCode = "";
        #endregion

        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n", parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/Data/conds/OrderSeq")[0];
            OrderSeq = OrderSeqNode.Attributes["value"].Value;

            XmlNode InvoiceCodeNode = xmlDoc.SelectNodes("/root/Data/values/InvoiceCode")[0];
            InvoiceCode = InvoiceCodeNode.Attributes["value"].Value;

            XmlNode OperatorNode = xmlDoc.SelectNodes("/root/Data/values/Operator")[0];
            Operator = OperatorNode.Attributes["value"].Value;

            XmlNode ExpressNameNode = xmlDoc.SelectNodes("/root/Data/values/ExpressName")[0];
            ExpressName = ExpressNameNode.Attributes["value"].Value;

            XmlNode ExpressCodeNode = xmlDoc.SelectNodes("/root/Data/values/ExpressCode")[0];
            ExpressCode = ExpressCodeNode.Attributes["value"].Value;

        }
        catch (Exception e)
        {
            // xml出错
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<root>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            returnMsg = returnMsg + "</root>";
            return returnMsg;
        }
        #endregion

        #region 校验请求数据
        try
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(Operator))  
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"操作人不能为空！！\" />";
                return returnMsg;
            }


            if (String.IsNullOrEmpty(InvoiceCode))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"发票号不能为空！！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(OrderSeq))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"充值订单号不能为空！！\" />";
                 return returnMsg;
            }

     

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                return returnMsg;
            }

        }
        catch (Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='" + e.ToString() + "'  />";
            return returnMsg;
        }

        #endregion

        #region 权限校验

        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress + " ：IP地址未经授权!" + "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "InvoiceCheck", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }

        #endregion

        #region 执行dao查询

        StringBuilder daologstr = new StringBuilder();
        bool flag = _rechargeOrder_dao.UpdateInvoice(OrderSeq, InvoiceCode, Operator, ExpressName, ExpressCode, out daologstr);
        strLog.Append(daologstr.ToString());

        #endregion

        #region 成功返回xml

        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        if (flag)
        {
            returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" >";
        }
        else
        {
            returnMsg = returnMsg + "<result returnCode = \"-9120\" msg = \"失败\" >";
        }
        returnMsg = returnMsg + "</result>";
        #endregion

        return returnMsg;
    }

    /// <summary>
    /// 拒开发票接口
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="strLog"></param>
    /// <returns></returns>
    protected String InvoiceRefuse(String parameter, out StringBuilder strLog)
    {
        #region 初始化变量
        strLog = new StringBuilder();
        String returnMsg = String.Empty;
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        String ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        String version = "";
        String SPID = "";
        String OrderSeq = "";
        String Operator = "";
        String RefuseReason = "";

        #endregion

        #region 解析xml请求包
        try
        {
            strLog.AppendFormat("请求参数Request:{0}\r\n", parameter);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameter);

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/Data/conds/OrderSeq")[0];
            OrderSeq = OrderSeqNode.Attributes["value"].Value;

            XmlNode OperatorNode = xmlDoc.SelectNodes("/root/Data/values/Operator")[0];
            Operator = OperatorNode.Attributes["value"].Value;

            XmlNode RefuseReasonNode = xmlDoc.SelectNodes("/root/Data/values/RefuseReason")[0];
            RefuseReason = RefuseReasonNode.Attributes["value"].Value;

        }
        catch (Exception e)
        {
            // xml出错
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<root>";
            returnMsg = returnMsg + "<result returnCode = \"-1001\" msg = \"xml请求包格式不对！\" />";
            returnMsg = returnMsg + "</root>";
            return returnMsg;
        }
        #endregion

        #region 校验请求数据
        try
        {
            if (String.IsNullOrEmpty(SPID))   // SPID不能为空
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"SPID不能为空！！\" />";
                return returnMsg;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1003\" msg = \"SPID长度不对！\" />";
                return returnMsg;
            }

            if (String.IsNullOrEmpty(OrderSeq))   
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"订单号不能为空！！\" />";
                return returnMsg;
            }
            if (String.IsNullOrEmpty(Operator))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"操作人不能为空！！\" />";
                return returnMsg;
            }
            if (String.IsNullOrEmpty(RefuseReason))
            {
                returnMsg = "";
                returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
                returnMsg = returnMsg + "<result returnCode = \"-1002\" msg = \"决绝理由不能为空！！\" />";
                return returnMsg;
            }
        }
        catch (Exception e)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1006\" msg ='" + e.ToString() + "'  />";
            return returnMsg;
        }

        #endregion

        #region 权限校验

        Result = CommonBizRules.CheckIPLimit(SPID, Request.UserHostAddress, this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1007\" msg ='" + Request.UserHostAddress + " ：IP地址未经授权!" + "'  />";
            return returnMsg;

        }

        //接口访问权限判断
        Result = CommonBizRules.SPInterfaceGrant(SPID, "InvoiceRefuse", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            returnMsg = "";
            returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            returnMsg = returnMsg + "<result returnCode = \"-1008\" msg ='" + ErrorDescription + "'  />";
            return returnMsg;
        }

        #endregion

        #region 执行dao查询
        StringBuilder daologstr = new StringBuilder();
        bool flag = _rechargeOrder_dao.RefuseInvoice(OrderSeq, Operator, RefuseReason, out daologstr);
        strLog.Append(daologstr.ToString());
        #endregion

        #region 成功返回xml

        returnMsg = "";
        returnMsg = returnMsg + "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        if (flag)
        {
            returnMsg = returnMsg + "<result returnCode = \"00\" msg = \"成功\" >";
        }
        else
        {
            returnMsg = returnMsg + "<result returnCode = \"-9103\" msg = \"失败\" >";
        }
        returnMsg = returnMsg + "</result>";
        #endregion
        return returnMsg;
    }


    /// 是否为日期型字符串  
    /// </summary>  
    /// <param name="StrSource">日期字符串(2008-05-08)</param>  
    /// <returns></returns>  
    private  bool IsDate(string StrSource)
    {
            return Regex.IsMatch(StrSource,@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]" +
                                                            @"|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|" +
                                                            @"1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?" +
                                                            @"2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468]" +
                                                            @"[048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
    }

    /// <summary>  
    /// 是否为时间型字符串  
    /// </summary>  
    /// <param name="source">时间字符串(15:00:00)</param>  
    /// <returns></returns>  
    private  bool IsTime(string StrSource)
    {
        return Regex.IsMatch(StrSource,@"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
    }


    /// <summary>  
    /// 是否为日期+时间型字符串  
    /// </summary>  
    /// <param name="source"></param>  
    /// <returns></returns>  
    private  bool IsDateTime(string StrSource)
    {
        return Regex.IsMatch(StrSource,@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?" +
                                                            @"[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?" +
                                                            @"[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]" +
                                                            @"|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-" +
                                                            @"9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[" +
                                                            @"2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23" +
                                                            @"|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
    }


  



    /// <summary>
    /// 记录日志
    /// </summary>
    protected void log(String name, string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog(name, msg);
    }

}

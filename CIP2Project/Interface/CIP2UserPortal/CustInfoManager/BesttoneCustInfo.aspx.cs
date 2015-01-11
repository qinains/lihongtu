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

public partial class CustInfoManager_BesttoneCustInfo : System.Web.UI.Page
{

    CustInfoDAO _custInfo_dao = new CustInfoDAO();


    /// 是否为日期型字符串  
    /// </summary>  
    /// <param name="StrSource">日期字符串(2008-05-08)</param>  
    /// <returns></returns>  
    private bool IsDate(string StrSource)
    {
        return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]" +
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
    private bool IsTime(string StrSource)
    {
        return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
    }


    /// <summary>  
    /// 是否为日期+时间型字符串  
    /// </summary>  
    /// <param name="source"></param>  
    /// <returns></returns>  
    private bool IsDateTime(string StrSource)
    {
        return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?" +
                                                            @"[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?" +
                                                            @"[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]" +
                                                            @"|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-" +
                                                            @"9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[" +
                                                            @"2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23" +
                                                            @"|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
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
            returnMsg.AppendFormat("<result returnCode=\"-9999\" msg ='" + ex.Message + "'/>");
            strLog.AppendFormat("，异常out：{0}", ex.Message);
        }
        finally
        {
            strLog.AppendFormat("返回报文:{0}\r\n", returnMsg.ToString());
            log(logName, strLog.ToString());
        }
        Response.Write(returnMsg.ToString());
        Response.Flush();
        Response.End();
    }
}

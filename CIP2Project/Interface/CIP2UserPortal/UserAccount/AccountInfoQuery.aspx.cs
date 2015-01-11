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
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using log4net;

using BTUCenter.Proxy;

public partial class UserAccount_AccountInfoQuery : System.Web.UI.Page
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(UserAccount_AccountInfoQuery));
    private static IDispatchControl serviceProxy = new IDispatchControl();

    protected String QueryAccountInfo()
    {
        //请求参数
        String CustID = Request["CustID"];
        String SPID = Request["SPID"];
        String wt = Request["wt"];   // json or xml

        logger.Info("AccountInfoQuery_log");
        logger.Info("CustID=" + CustID);
        logger.Info("SPID =" + SPID);
        logger.Info("writetype=" + wt);
        //返回参数
        String ErrMsg = String.Empty;
        Int32 Result = 0;
        Int32 ResultQueryCustInfo = 0;

        StringBuilder ResponseMsg = new StringBuilder();

        //账户信息字段
        String AccountNo = String.Empty;
        String AccountName = String.Empty;
        String AccountType = String.Empty;
        String AccountStatus = String.Empty;
        String AccountBalance = String.Empty;
        String PredayBalance = String.Empty;
        String PreMonthBalance = String.Empty;
        String AvailableBalance = String.Empty;
        String UnAvailableBalance = String.Empty;
        String AvailableLecash = String.Empty;
        String CardNum = String.Empty;
        String CardType = String.Empty;

        //客户信息字段
        String OuterID = String.Empty;
        String Status = String.Empty;
        String CustType = String.Empty;
        String CustLevel = String.Empty;
        String RealName = String.Empty;
        String UserName = String.Empty;
        String NickName = String.Empty;
        String CertificateCode = String.Empty;
        String CertificateType = String.Empty;
        String Sex = String.Empty;
        String Email = String.Empty;
        String EnterpriseID = String.Empty;
        String ProvinceID = String.Empty;
        String AreaID = String.Empty;
        String Registration = String.Empty;


        try
        {
            BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
            BesttoneAccount entity = _besttoneAccount_dao.QueryByCustID(CustID);
            if (entity == null)
            {
                // 返回错误信息
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "无此用户");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "999");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "无此账户");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                logger.Info("根据custid查账号没查到！");
                return ResponseMsg.ToString();
            }
            else
            {
                //查询账户余额
                AccountItem item =null;
                ErrMsg = "";
                Result = QueryBesttoneAccount(entity.BestPayAccount, out item, out ErrMsg);
                if (Result == 0)
                {
                    logger.Info("AccountInfoQuery_log:账户查询成功");
                    if (item != null)
                    {
                        AccountNo = item.AccountNo;
                        AccountName = item.AccountName;    // 账户名称 
                        AccountType = item.AccountType;        // 账户类型 （资金）
                        AccountStatus = item.AccountStatus;   //  账户状态
                        AccountBalance = item.AccountBalance.ToString();
                        PredayBalance = item.PredayBalance.ToString();
                        PreMonthBalance = item.PreMonthBalance.ToString();
                        AvailableBalance = item.AvailableBalance.ToString();
                        UnAvailableBalance = item.UnAvailableBalance.ToString();
                        AvailableLecash = item.AvailableLecash.ToString();
                        CardNum = item.CardNum;
                        CardType = item.CardType;

                        logger.Info("AccountNo=" + AccountNo);
                        logger.Info("AccountName=" + AccountName);
                        logger.Info("AccountType=" + AccountType);
                        logger.Info("AccountStatus=" + AccountStatus);
                        logger.Info("AccountBalance=" + AccountBalance);
                        logger.Info("PredayBalance=" + PredayBalance);
                        logger.Info("PreMonthBalance=" + PreMonthBalance);
                        logger.Info("AvailableBalance=" + AvailableBalance);
                        logger.Info("UnAvailableBalance=" + UnAvailableBalance);
                        logger.Info("CardNum=" + CardNum);
                        logger.Info("CardType=" + CardType);
                    }

                    if ("1".Equals(AccountType))
                    {
                        AccountType = "资金账户";
                    }
                    else if ("2".Equals(AccountType))
                    {
                        AccountType = "脱机账户";
                    }
                    else if ("3".Equals(AccountType))
                    {
                        AccountType = "代金券账户";
                    }
                    else if ("4".Equals(AccountType))
                    {
                        AccountType = "积分账户";
                    }

                    if ("0".Equals(AccountStatus))
                    {
                        AccountStatus = "未激活";
                    }
                    else if ("1".Equals(AccountStatus))
                    {
                        AccountStatus = "正常";
                    }
                    else if ("2".Equals(AccountStatus))
                    {
                        AccountStatus = "挂失";
                    }
                    else if ("3".Equals(AccountStatus))
                    {
                        AccountStatus = "冻结";
                    }
                    else if ("4".Equals(AccountStatus))
                    {
                        AccountStatus = "锁定";
                    }
                    else if ("9".Equals(AccountStatus))
                    {
                        AccountStatus = "已销户";
                    }
                    else
                    { //为定义
                        AccountStatus = "未定义";
                    }

                    //客户信息查询
                    ResultQueryCustInfo = CustBasicInfo.getCustInfo(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType,
                                                  out CustLevel, out RealName, out UserName, out NickName, out CertificateCode,
                                                  out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID, out AreaID, out Registration);

                    if (ResultQueryCustInfo == 0)
                    {
                        logger.Info("AccountInfoQuery_log:客户信息查询成功！");
                    }
                    else
                    {
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "客户信息查询失败");
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "999");
                            ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "客户信息查询失败");
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                        logger.Info("AccountInfoQuery_log:客户信息查询失败！");
                        return ResponseMsg.ToString();
                    }


                    if (Result == 0 && ResultQueryCustInfo == 0)
                    {
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))  // 以json格式 返回
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"AccountNo\":\"{0}\",", AccountNo);
                            ResponseMsg.AppendFormat("\"AccountName\":\"{0}\",", AccountName);
                            ResponseMsg.AppendFormat("\"BesttoneAccount\":\"{0}\",", entity.BestPayAccount);
                            ResponseMsg.AppendFormat("\"RealName\":\"{0}\",", RealName);
                            ResponseMsg.AppendFormat("\"UserName\":\"{0}\",", UserName);
                            ResponseMsg.AppendFormat("\"NickName\":\"{0}\",", NickName);
                            ResponseMsg.AppendFormat("\"Sex\":\"{0}\",", Sex);
                            ResponseMsg.AppendFormat("\"AccountType\":\"{0}\",", AccountType);
                            ResponseMsg.AppendFormat("\"AccountStatus\":\"{0}\",", AccountStatus);
                            ResponseMsg.AppendFormat("\"AccountBalance\":\"{0}\",", AccountBalance);
                            ResponseMsg.AppendFormat("\"PredayBalance\":\"{0}\",", PredayBalance);
                            ResponseMsg.AppendFormat("\"PreMonthBalance\":\"{0}\",", PreMonthBalance);
                            ResponseMsg.AppendFormat("\"AvailableBalance\":\"{0}\",", AvailableBalance);
                            ResponseMsg.AppendFormat("\"UnAvailableBalance\":\"{0}\",", UnAvailableBalance);
                            ResponseMsg.AppendFormat("\"AvailableLecash\":\"{0}\",", AvailableLecash);
                            ResponseMsg.AppendFormat("\"CardNum\":\"{0}\",", CardNum);
                            ResponseMsg.AppendFormat("\"CardType\":\"{0}\"", CardType);
                            ResponseMsg.Append("}");
                        }
                        else
                        {  //  以 xml 格式返回 
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>");
                            ResponseMsg.AppendFormat("<AccountNo>{0}</AccountNo>", AccountNo);
                            ResponseMsg.AppendFormat("<AccountName>{0}</AccountName>", AccountName);

                            ResponseMsg.AppendFormat("<RealName>{0}</RealName>", RealName);
                            ResponseMsg.AppendFormat("<UserName>{0}</UserName>", AccountName);
                            ResponseMsg.AppendFormat("<NickName>{0}</NickName>", AccountName);
                            ResponseMsg.AppendFormat("<Sex>{0}</Sex>", AccountName);

                            ResponseMsg.AppendFormat("<AccountType>{0}</AccountType>", AccountType);
                            ResponseMsg.AppendFormat("<AccountStatus>{0}</AccountStatus>", AccountStatus);
                            ResponseMsg.AppendFormat("<AccountBalance>{0}</AccountBalance>", AccountBalance);
                            ResponseMsg.AppendFormat("<PredayBalance>{0}</PredayBalance>", PredayBalance);
                            ResponseMsg.AppendFormat("<PreMonthBalance>{0}</PreMonthBalance>", PreMonthBalance);
                            ResponseMsg.AppendFormat("<AvailableBalance>{0}</AvailableBalance>", AvailableBalance);
                            ResponseMsg.AppendFormat("<UnAvailableBalance>{0}</UnAvailableBalance>", UnAvailableBalance);
                            ResponseMsg.AppendFormat("<AvailableLecash>{0}</AvailableLecash>", AvailableLecash);
                            ResponseMsg.AppendFormat("<CardNum>{0}</CardNum>", CardNum);
                            ResponseMsg.AppendFormat("<CardType>{0}</CardType>", CardType);
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                    }

                }
                else
                {
                    // 返回错误信息
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {

                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "账户查询失败");
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "999");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "账户查询失败");
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }

                    logger.Info("AccountInfoQuery_log:账户查询失败!");
                }

            }

        }
        catch (Exception ex)
        {
            logger.Info(ex.Message);

        }
        return ResponseMsg.ToString();
    }

    /// <summary>
    /// 生成20位数字随机流水号:35+yyMMddHHmmss+随机数(6位)
    /// </summary>
    public static String CreateTransactionID()
    {
        String date = DateTime.Now.ToString("yyyyMMddHHmmss");

        //6位随机数
        Random r = new Random(Guid.NewGuid().GetHashCode());
        String TransactionID = "00" + date + r.Next(1000, 9999).ToString();

        return TransactionID;
    }


    /// <summary>
    /// 号码百事通账户查询
    /// </summary>
    public  Int32 QueryBesttoneAccount(String besttoneAccount, out AccountItem accountInfo, out String ErrMsg)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("号码百事通账户查询入口参数:besttoneAccount:{0}\r\n", besttoneAccount);
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        accountInfo = null;

        StringBuilder requestXml = new StringBuilder();
        String responseXml = String.Empty;
        //流水号
        String TransactionID = CreateTransactionID();
        try
        {
            #region 拼接请求xml字符串
            strLog.AppendFormat("line 1204\r\n");
            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            requestXml.Append("<PayPlatRequestParameter>");
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"账户查询\" WEBSVRCODE=\"100100\" APPFROM=\"100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
            requestXml.Append("<PARAMETERS>");

            //添加参数
            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
            strLog.AppendFormat("line 1212,requestXml:{0}\r\n", requestXml);
            requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1");
            strLog.AppendFormat("line 1214,requestXml:{0}\r\n", requestXml);
            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
            strLog.AppendFormat("line 1216,requestXml:{0}\r\n", requestXml);
            //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");
            requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
            strLog.AppendFormat("line 1219,requestXml:{0}\r\n", requestXml);
            requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
            strLog.AppendFormat("line 1221,requestXml:{0}\r\n", requestXml);
            requestXml.Append("</PARAMETERS>");
            requestXml.Append("</PayPlatRequestParameter>");
            strLog.AppendFormat("line 1224:{0}\r\n", requestXml);
            #endregion
            strLog.AppendFormat("requestXml={0}\r\n", requestXml);
            //请求接口
            responseXml = serviceProxy.dispatchCommand("100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

            strLog.AppendFormat("responseXml={0}\r\n", responseXml);

            #region 解析接口返回参数

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);

            String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
            ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
            if (responseCode == "000000")
            {
                Result = 0;
                accountInfo = new AccountItem();
                XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];
                accountInfo.AccountNo = dataNode.Attributes["ACCOUNTNO"].Value;
                accountInfo.AccountName = dataNode.Attributes["ACCOUNTNAME"].Value;
                accountInfo.AccountType = dataNode.Attributes["ACCOUNTTYPE"].Value;
                accountInfo.AccountStatus = dataNode.Attributes["ACCOUNTSTATUS"].Value;
                accountInfo.AccountBalance = Convert.ToInt64(dataNode.Attributes["ACCOUNTBALANCE"].Value);
                accountInfo.PredayBalance = Convert.ToInt64(dataNode.Attributes["PREDAYBALANCE"].Value);
                accountInfo.PreMonthBalance = Convert.ToInt64(dataNode.Attributes["PREMONTHBALANCE"].Value);
                accountInfo.AvailableBalance = Convert.ToInt64(dataNode.Attributes["AVAILABLEBALANCE"].Value);
                accountInfo.UnAvailableBalance = Convert.ToInt64(dataNode.Attributes["UNAVAILABLEBALANCE"].Value);
                accountInfo.AvailableLecash = Convert.ToInt64(dataNode.Attributes["AVAILABLECASH"].Value);
                accountInfo.CardNum = dataNode.Attributes["CARDNUM"].Value;
                accountInfo.CardType = dataNode.Attributes["CARDTYPE"].Value;
            }

            #endregion

        }
        catch (Exception ex)
        {
            strLog.AppendFormat("异常:{0}\r\n", ex.ToString());
           
        }
        finally
        {
            logger.Info("客户端调用：responseXml=" + responseXml);
        }
        return Result;
    }





    protected void Page_Load(object sender, EventArgs e)
    {

        String ResponseText = QueryAccountInfo();

        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }



}

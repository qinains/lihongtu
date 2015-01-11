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


public partial class UserAccount_ChangePayPasssword : System.Web.UI.Page
{
    private static IDispatchControl serviceProxy = new IDispatchControl();


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



    public String ChangePayPassword(String SPID, String CustID, String oldPassWord, String newPassWord, String confirmPassWord, String wt)
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        StringBuilder ResponseMsg = new StringBuilder();

        #region
        if (CommonUtility.IsEmpty(SPID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(CustID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }



        if (CommonUtility.IsEmpty(oldPassWord))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "oldPassword不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "oldPassword不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }



        if (CommonUtility.IsEmpty(newPassWord))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "newPassword不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "newPassword不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(confirmPassWord))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "confirmPassWord不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "confirmPassWord不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        #endregion

        try
        {
            string BindedBestpayAccount = "";
            string CreateTime = "";
            Result = CIP2BizRules.IsBesttoneAccountBindV5(CustID, out BindedBestpayAccount, out CreateTime, out ErrMsg);

            if (Result == 0 && !String.IsNullOrEmpty(BindedBestpayAccount))
            {

                BestPayEncryptService bpes = new BestPayEncryptService();
                string e_oldPassWord = "";
                string e_newPassWord = "";
                string e_confirmPassWord = "";

                AccountItem ai = new AccountItem();
                String ResCode = "";

                int QueryBesttoneAccountResult = BesttoneAccountInfoQuery(BindedBestpayAccount, out ai, out ResCode, out ErrMsg);
                if (QueryBesttoneAccountResult == 0)
                {
                    if (ai != null)
                    {
                        e_oldPassWord = bpes.encryptNoKey(oldPassWord, ai.AccountNo);
                        e_newPassWord = bpes.encryptNoKey(newPassWord, ai.AccountNo);
                        e_confirmPassWord = bpes.encryptNoKey(confirmPassWord, ai.AccountNo);

                        //strLog.AppendFormat("e_oldPassWord{0},e_newPassWord{1},e_confirmPassWord{2}", e_oldPassWord, e_newPassWord, e_confirmPassWord);

                        int ModifyBestPayPasswordResult = ModifyBestPayPassword(ai.AccountNo, e_oldPassWord, e_newPassWord, e_confirmPassWord, out ErrMsg);

                        if (ModifyBestPayPasswordResult == 0)
                        {
                            //success = "0";
   
                                // 返回错误信息
                                ResponseMsg.Length = 0;
                                if ("json".Equals(wt))
                                {
                                    ResponseMsg.Append("{");
                                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改密码成功！");
                                    ResponseMsg.Append("}");
                                }
                                else
                                {
                                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                    ResponseMsg.Append("<PayPlatRequestParameter>");
                                    ResponseMsg.Append("<PARAMETERS>");
                                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改密码成功！");
                                    ResponseMsg.Append("</PARAMETERS>");
                                    ResponseMsg.Append("</PayPlatRequestParameter>");
                                }
                                return ResponseMsg.ToString();
                            
                        }
                        else
                        {
                            //strLog.Append(",失败3");
                            //Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);

                                // 返回错误信息
                                ResponseMsg.Length = 0;
                                if ("json".Equals(wt))
                                {
                                    ResponseMsg.Append("{");
                                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "9916");
                                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改密码失败！");
                                    ResponseMsg.Append("}");
                                }
                                else
                                {
                                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                    ResponseMsg.Append("<PayPlatRequestParameter>");
                                    ResponseMsg.Append("<PARAMETERS>");
                                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "9916");
                                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改密码失败！");
                                    ResponseMsg.Append("</PARAMETERS>");
                                    ResponseMsg.Append("</PayPlatRequestParameter>");
                                }
                                return ResponseMsg.ToString();
                            
                        }
                    }
                    else
                    {
                        //strLog.Append(",失败2");
                        //Response.Redirect("ErrorInfo.aspx?ErrorInfo=账户信息未获取");

                            // 返回错误信息
                            ResponseMsg.Length = 0;
                            if ("json".Equals(wt))
                            {
                                ResponseMsg.Append("{");
                                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "9917");
                                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改密码失败！账户信息未能获取");
                                ResponseMsg.Append("}");
                            }
                            else
                            {
                                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                ResponseMsg.Append("<PayPlatRequestParameter>");
                                ResponseMsg.Append("<PARAMETERS>");
                                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "9917");
                                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改密码失败！账户信息未能获取");
                                ResponseMsg.Append("</PARAMETERS>");
                                ResponseMsg.Append("</PayPlatRequestParameter>");
                            }
                            return ResponseMsg.ToString();
                        
                    }
                }
                else
                {
                    //strLog.Append(",失败1");
                    //Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
    
                        // 返回错误信息
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "9918");
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改密码失败2！");
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "9918");
                            ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改密码失败2！");
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                        return ResponseMsg.ToString();
                    
                }
            }
            else
            {
                // 未开通账户


                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "9919");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改密码失败！账户未开通");
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "9919");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改密码失败！账户未开通");
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                
            }
        }
        catch (Exception ecp)
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "9920");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ecp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "9920");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ecp.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        return ResponseMsg.ToString();
    }

    /// <summary>
    /// 密码修改接口 lihongtu
    /// </summary>
    public static Int32 ModifyBestPayPassword(String besttoneAccount, String oldPassword, String newPassword, String surePassword, out String ErrMsg)
    {
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

        StringBuilder requestXml = new StringBuilder();
        String responseXml = String.Empty;
        String TransactionID = CreateTransactionID();
        try
        {
            #region 拼接请求xml字符串

            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            requestXml.Append("<PayPlatRequestParameter>");
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"支付密码修改\" WEBSVRCODE=\"300030\" APPFROM=\"300030|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");

            requestXml.Append("<PARAMETERS>");

            //添加参数
            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", besttoneAccount);
            requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1");
            requestXml.AppendFormat("<OLDPASSWD>{0}</OLDPASSWD>", oldPassword);
            requestXml.AppendFormat("<NEWPASSWD>{0}</NEWPASSWD>", newPassword);
            requestXml.AppendFormat("<CONFIRMPASSWD>{0}</CONFIRMPASSWD>", surePassword);
       
            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
            //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");
            requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
            requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
            requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);

            //requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", ACCEPTAREACODE);
            //requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", ACCEPTCITYCODE);

            requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02");
            requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
            requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");
            requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");
            requestXml.AppendFormat("<INPUTUID>{0}</INPUTUID>", "");
            requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
            requestXml.AppendFormat("<CHECKUID>{0}</CHECKUID>", "");
            requestXml.AppendFormat("<CHECKTIME>{0}</CHECKTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

            requestXml.Append("</PARAMETERS>");
            requestXml.Append("</PayPlatRequestParameter>");

            #endregion

            //请求接口
            //log(String.Format("修改密码请求:{0}:", requestXml));
            responseXml = serviceProxy.dispatchCommand("300030|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
            //log(String.Format("修改密码返回:{0}:", responseXml));
            #region 解析接口返回参数

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);

            String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
            ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;
            if (responseCode == "000000")
            {
                Result = 0;
                ErrMsg = String.Empty;
            }

            #endregion
        }
        catch (Exception ex)
        {
            //log("修改密码异常:" + ex.ToString());
        }

        return Result;
    }

    /// <summary>
    /// 号码百事通账户查询 lihongtu
    /// </summary>
    public static Int32 BesttoneAccountInfoQuery(String besttoneAccount, out AccountItem accountInfo, out String ResCode, out String ErrMsg)
    {

        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        accountInfo = null;
        ResCode = "000000";
        string ACCOUNTTYPE = "1";
        StringBuilder requestXml = new StringBuilder();
        String responseXml = String.Empty;
        //流水号
        String TransactionID = CreateTransactionID();
        try
        {
            #region 拼接请求xml字符串

            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            requestXml.Append("<PayPlatRequestParameter>");
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"账户查询\" WEBSVRCODE=\"100100\" APPFROM=\"100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
            requestXml.Append("<PARAMETERS>");

            //添加参数
            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
            requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", ACCOUNTTYPE);

            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);

            requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
            requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

            requestXml.Append("</PARAMETERS>");
            requestXml.Append("</PayPlatRequestParameter>");

            #endregion

            //请求接口
            //log(String.Format("账户查询请求:{0}", requestXml));
            responseXml = serviceProxy.dispatchCommand("100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
            //log(String.Format("账户查询返回:{0}", responseXml));
            #region 解析接口返回参数

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);

            String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
            ResCode = responseCode;

            ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
            if (responseCode == "000000")
            {

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
            Result = 0;
            #endregion

        }
        catch (Exception ex)
        {
            
        }
        finally
        {

        }
        return Result;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
         String oldPassWord = Request["oldPassWord"];
         String newPassWord = Request["newPassWord"];
         String confirmPassWord = Request["confirmPassWord"];
         String SPID = Request["SPID"];
         String CustID = Request["CustID"];
         String wt = Request["wt"];   // json or xml
         String ResponseText = ChangePayPassword(SPID, CustID, oldPassWord, newPassWord,  confirmPassWord, wt);
         if (!"json".Equals(wt))
         {
             Response.ContentType = "xml/text";
         }
         Response.Write(ResponseText);
         Response.Flush();
         Response.End();

    }
}

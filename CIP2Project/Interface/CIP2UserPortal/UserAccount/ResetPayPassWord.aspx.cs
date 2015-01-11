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

public partial class UserAccount_ResetPayPassWord : System.Web.UI.Page
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

    public String ResetPayPassWord(String SPID,String CustID,String wt)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

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

        String Phone = String.Empty;
        BesttoneAccount account = null;
        BesttoneAccountDAO dao = new BesttoneAccountDAO();
        account = dao.QueryByCustID(CustID);
        if (account != null)
        {
            Phone = account.BestPayAccount;
        }
        else
        { 
            //未开户
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "该CustID尚未开户！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "该CustID尚未开户！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        


        ////////验证码校验
        //////Result = PhoneBO.SelSendSMSMassage(CustID, Phone, AuthenCode, out ErrMsg);
        //////if (Result != 0)
        //////{
        //////    // 验证码未校验通过  return 
        //////    ResponseMsg.Length = 0;
        //////    if ("json".Equals(wt))
        //////    {
        //////        ResponseMsg.Append("{");
        //////        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1000");
        //////        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "验证码未校验通过!");
        //////        ResponseMsg.Append("}");
        //////    }
        //////    else
        //////    {
        //////        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //////        ResponseMsg.Append("<PayPlatRequestParameter>");
        //////        ResponseMsg.Append("<PARAMETERS>");
        //////        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1000");
        //////        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "验证码未校验通过！");
        //////        ResponseMsg.Append("</PARAMETERS>");
        //////        ResponseMsg.Append("</PayPlatRequestParameter>");
        //////    }
        //////    return ResponseMsg.ToString();
        //////}

        String t_custid = PhoneBO.IsAuthenPhone(Phone, SPID, out ErrMsg);
            // t_custid 可以为空，但不能是别人的custid,可以为空是说明此客户无认证电话
        if (!String.IsNullOrEmpty(t_custid))
        {
            if (CustID != t_custid)
            {
                //是别人的手机号，不能用来开户  return 
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1000");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", Phone + "(" + CustID + ")是别人的手机号(" + t_custid + ")，不能用来开户！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1000");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "是别人的手机号，不能用来开户！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }

        }

        #endregion


        try
        {
           
            Linkage.BestTone.Interface.Rule.CustInfo custInfo = new Linkage.BestTone.Interface.Rule.CustInfo();
            int QueryCustInfoResult = QueryCustInfo(account.BestPayAccount, out custInfo, out ErrMsg);
            if (QueryCustInfoResult == 0)
            {
                Result = ResetBesttoneAccountPayPassword(account.BestPayAccount, custInfo.IdType, custInfo.IdNo, custInfo.CustomerName, out ErrMsg);
                if (Result == 0)
                {
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码重置成功!");
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码重置成功！");
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
  
                }
                else
                {
                    //ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                    //Descriptioin = "重置密码失败!";  失败  return
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "910");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码重置失败!");
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "910");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码重置失败！");
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                }
            }
            else
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "920");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "无此账户!");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "920");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "无此账户！");
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
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "930");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ecp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "930");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ecp.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        return ResponseMsg.ToString();
    
    }

    // 客户信息查询接口 lihongtu
    public static Int32 QueryCustInfo(String ProductNo, out CustInfo custinfo, out String ErrMsg)
    {

        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        String TransactionID = CreateTransactionID();
        StringBuilder requestXml = new StringBuilder();
        String responseXml = String.Empty;
        custinfo = new CustInfo();
        try
        {
            #region 拼接请求xml字符串
            //100101	客户查询
            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            requestXml.Append("<PayPlatRequestParameter>");
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"客户查询\" WEBSVRCODE=\"100101\" APPFROM=\"100101|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
            requestXml.Append("<PARAMETERS>");

            //添加参数

            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + ProductNo);
            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);  //002310000000000
            requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
            requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

            requestXml.Append("</PARAMETERS>");
            requestXml.Append("</PayPlatRequestParameter>");

            #endregion

            //请求接口
      
            responseXml = serviceProxy.dispatchCommand("100101|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);

            String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
            ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;

            if (responseCode == "000000")
            {
                XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];
                custinfo.CustomerNo = dataNode.Attributes["CUSTOMER_NO"].Value;
                custinfo.ProductNo = dataNode.Attributes["PRODUCT_NO"].Value;
                custinfo.CustomerName = dataNode.Attributes["CUSTOMER_NAME"].Value;
                custinfo.IdType = dataNode.Attributes["ID_TYPE"].Value;
                custinfo.IdNo = dataNode.Attributes["ID_NO"].Value;
                Result = 0;
                ErrMsg = String.Empty;
            }

        }
        catch (System.Exception ex)
        {
          
        }
        return Result;

    }
    //重置密码 
    // lihongtu
    public static Int32 ResetBesttoneAccountPayPassword(String besttoneAccount, String cerType, String cerNo, String customerName, out String ErrMsg)
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
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"密码重置\" WEBSVRCODE=\"200101\" APPFROM=\"200101|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
            requestXml.Append("<PARAMETERS>");

            //添加参数
            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
            requestXml.AppendFormat("<IDTYPE>{0}</IDTYPE>", cerType);
            requestXml.AppendFormat("<IDNO>{0}</IDNO>", cerNo);
            requestXml.AppendFormat("<CUSTOMERNAME>{0}</CUSTOMERNAME>", customerName);

            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
            requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
            requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
            requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);

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
            responseXml = serviceProxy.dispatchCommand("200101|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
            #region 解析接口返回参数

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);

            String responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
            ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
            if (responseCode == "000000")
            {
                Result = 0;
                ErrMsg = String.Empty;
            }
            #endregion
        }
        catch (Exception ex)
        {
           
        }

        return Result;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //请求参数
        String CustID = Request["CustID"];
        String SPID = Request["SPID"];
        //String Phone = Request["Phone"];
        //String AuthenCode = Request["AuthenCode"];
        String wt = Request["wt"];   // json or xml
        String ResponseText = ResetPayPassWord(SPID, CustID, wt);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }
}

using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Text;


using System.Data.SqlClient;
using System.Data;
using uamsso;

/// <summary>
/// WebService 的摘要说明
/// </summary>

[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WebService : System.Web.Services.WebService, IUDBForUAMServiceSoap
{

    public WebService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    } 

    public class TicketParseResult
    {
        public int Result;
        public string CustID;
        public string RealName;
        public string UserName;
        public string NickName;
        public string OuterID;
        public string LoginAuthenName;
        public string LoginAuthenType;
        public string ExtendField;
        public string ErrorDescription;
    }

    [WebMethod(Description = "积分反向单点登录-UAM断言查询")]
    public string accountInfoQuery(string uamxml)
    {
        TicketParseResult Result = new TicketParseResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        Result.ExtendField = "";

        string uamreturnxml = "";

        UamUserInfoRequest uair = new UamUserInfoRequest();
        XMLExchange xe = new XMLExchange();

        string actioncode = "1";
        string transactionid = "";
        string rsptime = DateTime.Now.ToString("yyyyMMddHHmmss"); ;
        string digitalsign = "";
        string rsptype = "0";
        string rspcode = "0000";
        string rspdesc = "success";
        string accounttype = "";
        string accountid = "";
        string pwdtype = "01";
        string trustedacclist = "";
        string returnurl = "http://wtwebtest.ct10000.com/tymh/wtToJt.do";
        returnurl = System.Configuration.ConfigurationManager.AppSettings["UAMReturnUrl"]; 
        string SPID = "35000050";
        string Ticket = "";
        String provinceid = String.Empty;
        try
        {
            log(String.Format("【集团网厅查询参数:】Time:{0},uamxml:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uamxml));
            //解析参数
            uair = xe.AnalysisUamBackXML(uamxml);
            transactionid = uair.TransactionID;
            Ticket = uair.Ticket;

            //客户信息平台到集团网厅时，provinceid作为ticket的一部分放在ticket前两位
            provinceid = Ticket.Substring(0, 2);
            if (CommonUtility.IsEmpty(Ticket))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_TicketError_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_TicketError_Msg + "，票据不能为空";
                rspcode = "-112";
                rspdesc = Result.ErrorDescription;
                uamreturnxml = xe.BuildUamCustInfoXML_New(actioncode, transactionid, rsptime, digitalsign, rsptype, rspcode, rspdesc, accounttype, accountid, pwdtype, "", trustedacclist, returnurl, provinceid);
                return uamreturnxml;
            }
            // 9 固话 10 小灵通 11 宽带 7 手机 
            // 9 和 11需要把citycode带给uam
            string ExtendField = "9";
            //解析票据
            Result.Result = CIPTicketManager.checkCIPTicket(SPID, Ticket, ExtendField, out Result.CustID, out Result.RealName, out Result.UserName, out Result.NickName, out Result.OuterID, "", out Result.LoginAuthenName, out Result.LoginAuthenType, out Result.ErrorDescription);

            //认证类型转换
            accounttype = ConvertAuthenType(Result.LoginAuthenType);

            if (Result.Result == 0)
            {
                accountid = Result.LoginAuthenName;
                string citycode = "";
                //固话和宽带号
                if (accounttype.Equals("2000001") || accounttype.Equals("2000002"))
                {
                    if (accountid.IndexOf('-') > 0)
                    {
                        string[] pwdattrlist = accountid.Split('-');
                        citycode = pwdattrlist[0];
                        accountid = pwdattrlist[1];
                    }
                }
                     
                uamreturnxml = xe.BuildUamCustInfoXML_New(actioncode, transactionid, rsptime, digitalsign, rsptype, rspcode, rspdesc, accounttype, accountid, pwdtype,citycode, trustedacclist, returnurl,provinceid);
            }
            else
            {
                Result.Result = ErrorDefinition.BT_IError_Result_TicketError_Code;
                Result.ErrorDescription = Ticket + "票据解析失败";
                rspcode = "-113";
                rsptype = "8004";
                accountid = "";
                rspdesc = Result.ErrorDescription;
                uamreturnxml = xe.BuildUamCustInfoXML_New(actioncode, transactionid, rsptime, digitalsign, rsptype, rspcode, rspdesc, accounttype, accountid, pwdtype,"", trustedacclist, returnurl,provinceid);
            }
        }
        catch (System.Exception ex)
        {
            Result.Result = 978;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
            rspcode = "978";
            rsptype = "4104";
            rspdesc = ErrorDefinition.IError_Result_System_UnknowError_Msg + ex.Message;
        }
        finally
        {
            //写数据库日志
            try
            {
                #region WriteLog
                StringBuilder msg = new StringBuilder();
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
                msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"积分反向单点-登录UAM请求客户信息平台断言查询 " + DateTime.Now.ToString("u") + "\r\n");
                msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
                msg.Append(";SPID - " + SPID);
                msg.Append(";Ticket - " + Ticket);
                msg.Append("\r\n");
                msg.Append("返回给uam的报文:\r\n");
                msg.Append(uamreturnxml);
                msg.Append("\r\n");
                msg.Append("处理结果 - " + Result.Result);
                msg.Append("; 错误描述 - " + Result.ErrorDescription);
                msg.Append("; CustID - " + Result.CustID);
                msg.Append("; RealName - " + Result.RealName);
                msg.Append("; UserName - " + Result.UserName);
                msg.Append("; NickName - " + Result.NickName);
                msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
                msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

                BTUCenterInterfaceLog.CenterForBizTourLog("AccountInfoQuery", msg);
                #endregion
            }
            catch { }
        }

        return uamreturnxml;
    }

    /// <summary>
    /// 认证类型转换
    /// </summary>
    /// <param name="authenType"></param>
    /// <returns></returns>
    protected String ConvertAuthenType(String authenType)
    {
        String AuthenType = String.Empty;
        switch (authenType)
        { 
            case "9":
                AuthenType = "2000001";
                break;
            case "10":
                AuthenType = "2000003";
                break;
            case "11":
                AuthenType = "2000002";
                break;
            case "7":
                AuthenType = "2000004";
                break;
            default :
                AuthenType = "0000000";
                break;
        }

        return AuthenType;
    }

    protected void log(String msg)
    {
        StringBuilder strMsg = new StringBuilder();
        strMsg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        strMsg.Append(msg);
        strMsg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("AccountInfoQuery", strMsg);
    }
}


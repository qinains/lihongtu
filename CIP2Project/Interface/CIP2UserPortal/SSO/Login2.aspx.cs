using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using BTUCenter.Proxy;
using System.Text;
using System.Text.RegularExpressions;

using System.Net;
using System.IO;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Security.Cryptography;

public partial class SSO_Login2 : System.Web.UI.Page
{
    // 积分商城反向单点登录集团网厅の李宏图 
    string ReturnURL = "";
    string ProvinceID = "";
    string AuthenType = "";
    string AuthenName = "";
    string Password = "";
    string SPID = "";
    string AccountType = "";

    protected void sendTicket2UA(string url)
    {
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        string Url = "";
        try
        {
            string Ticket = CommonBizRules.CreateTicket();
            //string sDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //Random r = new Random();
            //Ticket = sDate + r.Next(10000, 99999).ToString();

            string SPID = "35999999";
            string CustID = TokenValidate.CustID;
            string RealName = TokenValidate.RealName;
            string NickName = TokenValidate.NickName;
            string UserName = TokenValidate.UserName;
            string OutID = TokenValidate.OuterID;
            string LoginAuthenName = TokenValidate.LoginAuthenName;
            string LoginAuthenType = TokenValidate.LoginAuthenType;

            String er = "";

            Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, NickName, UserName, OutID, "", LoginAuthenName, LoginAuthenType, out er);

            if (Result != 0)
            {

                return;
            }


            if (url.IndexOf("?") > 0)
            {
                Url = url + "&UATicket=" + Ticket;
            }
            else
            {
                Url = url + "?UATicket=" + Ticket;
            }



            if (url == "")
            {
                Response.Redirect("http://www.118114.cn/");
            }
            else
            {
                Response.Redirect(Url, false);
            }
        }

        catch (Exception e)
        {
            return;

        }


    }

    protected void sendTicket2JF(string ticket, string CustID,string RealName, string UserAccount, string OutID, string UserName, string AuthenName, string AuthenType, string NickName)
    {
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        int Result00 = ErrorDefinition.IError_Result_UnknowError_Code;
        string Url = "";
        try
        {

            string SPID = "35000050";

            string LoginAuthenName = TokenValidate.LoginAuthenName;
            string LoginAuthenType = TokenValidate.LoginAuthenType;
            LoginAuthenName = AuthenName;
            LoginAuthenType = AuthenType;

            String er = "";
            Result = CIPTicketManager.insertCIPTicket(ticket, SPID, CustID, RealName, NickName, UserName, OutID, "", LoginAuthenName, LoginAuthenType, out er);
            log("ticket:" + ticket + "\r\n");
            log("insertCIPTicket-Result:" + Result + "\r\n");
            log("insertCIPTicket-errmsg:" + er + "\r\n");

            //string er00 = "";
            //Result00 = CIPTicketManager.insertUAMTicket(ticket, SPID, CustID, RealName, NickName, UserName, OutID, "", LoginAuthenName, LoginAuthenType, out er00);
            //log("insertUAMTicket-Result00:" + Result00 + "\r\n");
            //log("insertUAMTicket-errmsg:" + er00 + "\r\n");

            if (Result != 0)
            {
                return;
            }

            if (ReturnURL.IndexOf("?") > 0)
            {
                Url = ReturnURL + "&Ticket=" + ticket;
            }
            else
            {
                Url = ReturnURL + "?Ticket=" + ticket;
            }

            if (ReturnURL == "")
            {
                Response.Redirect("http://www.118114.cn/");
            }
            else
            {
                Response.Redirect(Url, false);
            }
        }
        catch (Exception e)
        {
            return;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        log("step1");
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        log("step2");
        if (CommonUtility.IsParameterExist("SSORequestXML", this.Page))  // 带着token过来的是网厅的认证请求
        {
            #region 隐藏
            MBOSSClass mboss = new MBOSSClass();
          
            SPInfoManager spInfo = new SPInfoManager();
            string SPID = "35999991";
            string UAProvinceID = "35";
            string SourceType = "";
            string uaURL = "";
            string privateKeyPassword = "";
            string UserName = "";
            string ErrMsg = "";
     
            string CAP01002_XML = Request["SSORequestXML"];

            string DigitalSign = MBOSSClass.GetNewXML(CAP01002_XML, "DigitalSign");

            string DigitalSignValue = MBOSSClass.GetValueFromXML(CAP01002_XML, "DigitalSign");
            //从中取出RedirectURL
            string RedirectURL = MBOSSClass.GetValueFromXML(CAP01002_XML, "RedirectURL");
            //验证 CAP01002_XM 合法性
            byte[] PublicKeyFile = new byte[0];

            try
            {
                Object SPData = spInfo.GetSPData(this.Context, "");  //SPDataCacheName 这里要去问tongbo
                PublicKeyFile = spInfo.GetCAInfo(SPID, 0, SPData, out UserName, out privateKeyPassword);
            }
            catch (Exception err)
            {
                //验证签名未通过
                ErrMsg = err.Message;
                Result = -20001;
                Response.Redirect(RedirectURL, true);
                return;
            }

            Result = mboss.VerifySignByPublicKey(DigitalSign, PublicKeyFile, DigitalSignValue, out ErrMsg);
            //<CAPRoot><SessionHeader><ServiceCode>CAP01003</ServiceCode><Version>mbossUacVersion1</Version><ActionCode>0</ActionCode><TransactionID>35000201109254969771818</TransactionID><SrcSysID>35000</SrcSysID><DigitalSign>302C02141DB53BC5D52562D69EFD959B32F6E10D4BF6421E02145983D67CC81B0F376CA688B39F6AD1896EA0E082</DigitalSign><DstSysID>18</DstSysID><ReqTime>20110925000030</ReqTime><Request><ReqType/><ReqCode/><ReqDesc/></Request></SessionHeader><SessionBody><SPSSOAuthReq><RedirectURL>http://Customer.besttone.com.cn/UserPortal/SSO/SelectAssertion.aspx</RedirectURL><AcceptAccountTypeList><AcceptAccountType>0000000</AcceptAccountType></AcceptAccountTypeList></SPSSOAuthReq></SessionBody></CAPRoot>
            log("从网厅来:" + CAP01002_XML );
            if (Result != 0)
            {
                // 签名校验未通过,直接将请求原路打回 
                Response.Redirect(RedirectURL, true);
                return;
            }
            string sessionid = this.Page.Session.SessionID;
            string globaltoken  = Request.Cookies[sessionid].Value.ToString();
            uaURL = RedirectURL;
            if (globaltoken != null && !"".Equals(globaltoken)){
                this.sendTicket2UA(uaURL);
            }else{
                this.Response.Redirect(uaURL);
            }

            #endregion
        }
        else
        {
            //不带token的是积分商城过来的认证请求
            string direction = Request["Direction"];
            string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            string key0 = System.Configuration.ConfigurationManager.AppSettings["ScoreSystemSecret"];
 
            if (CommonUtility.IsParameterExist("Direction", this.Page))
            {
                #region 反向登录到集团网厅方向
                string ProvinceID = Request["ProvinceID"];
                if ("uam".Equals(direction))
                {   
                    //获取本地的token，如果没有token则返回到积分商城登录
                    //string token = Request.Cookies[CookieName].Value;
                    String token = PageUtility.GetCookie(CookieName);
                    if (String.IsNullOrEmpty(token))
                        Response.Redirect("http://www.ct10000.com");

                    string ProvinceID0 = "";  // 2013.01.23 添加
                    string CustID0 = "";
                    string RealName0 = "";
                    string UserName0 = "";
                    string NickName0 = "";
                    string OuterID0 = "";
                    string CustType0 = "";
                    string AuthenName0 = "";
                    string AuthenType0 = "";
                    string ErrMsg0 = "";

                    //解析token
                    UserToken UT0 = new UserToken();
                    // 2013.01.23 修改
                    //int Result0 = UT0.ParseUserToken(token, key0, out CustID0, out RealName0, out UserName0, out NickName0, out OuterID0, out CustType0, out AuthenName0, out AuthenType0, out ErrMsg0);
                    int Result0 = UT0.ParseScoreUserToken(token, key0,  out ProvinceID0, out CustID0, out RealName0, out UserName0, out NickName0, out OuterID0, out CustType0, out AuthenName0, out AuthenType0, out ErrMsg0);
                    //日志
                    log(String.Format("【token解析结果：】Result:{0},ErrMsg:{1},AuthenName:{2},AuthenType:{3},CustID:{4},OuterID:{5},CustType:{6}", Result0, ErrMsg0, AuthenName0, AuthenType0, CustID0, OuterID0, CustType0));

                    if (Result0 == 0)
                    {
                        //生成ticket，反向单点登录要求ticket前面加上省id
                        //string sDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        //Random r = new Random();
                        //string ticket = ProvinceID + sDate + r.Next(10000, 99999).ToString();
                        string ticket = ProvinceID + CommonBizRules.CreateTicket();
                        //积分商城
                        SPID = "35000010";
                        //将ticket插入数据库
                        Result = CIPTicketManager.insertCIPTicket(ticket, SPID, CustID0, RealName0, NickName0, UserName0, OuterID0, "", AuthenName0, AuthenType0, out ErrMsg0);

                        log(String.Format("【ticket生成结果：{0}】Result:{1},ErrMsg:{2},ticket:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Result, ErrMsg0, ticket));
                        
                        if (Result != 0)
                        {
                            this.Response.Redirect("http://www.ct10000.com");
                        }
                        string uamURL = System.Configuration.ConfigurationManager.AppSettings["UAMUrl"] + "?AccountIndex=" + ticket;

                        log(String.Format("【登录网厅：{0}】uamURL:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uamURL));

                        this.Response.Redirect(uamURL);
                    }
                    else
                    {
                        this.Response.Redirect("http://www.ct10000.com");
                    }
                }
                else 
                {
                    this.Response.Redirect("http://www.ct10000.com");
                }
            #endregion
            }
            else
            {
                if (!this.IsPostBack)
                {
                    #region 积分商城直接登录

                    string SPTokenRequest = Request["SPTokenRequest"];
                    log(String.Format("【SPTokenRequest参数为：{1}】SPTokenRequest:{0}", SPTokenRequest, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    string key = System.Configuration.ConfigurationManager.AppSettings["ScoreSystemSecret"];
                    string JFLoginUrl = System.Configuration.ConfigurationManager.AppSettings["JFLoginUrl"];
                    //生成全局token写入cookie,该全局token为了将来网厅请求时候，查询该客户登陆状态用
                    UserToken UT = new UserToken();

                    ProvinceID = Request["ProvinceID"];
                    AuthenType = Request["AuthenType"];
                    AuthenName = Request["AuthenName"];
                    Password = Request["Password"];
                    ReturnURL = Request["ReturnURL"];   // 这个ReturnUrl 用来还给积分商城ticket用
                    AccountType = Request["AccountType"];

                    string RealName = "";
                    string UserName = "";
                    string NickName = "";
                    string CustType = "";
                    string CustID = "";
                    string ErrMsg = "";
                    string OutID = "";
                    string UserAccount = "";
                    string SPID = "";

                    //解析SPTokenRequest参数
                    int Resultjf = SSOClass.ParseJFLoginRequest(SPTokenRequest, this.Context, out SPID, out ProvinceID, out AuthenType, out AuthenName, out Password, out ReturnURL, out ErrMsg);

                    if (Resultjf != 0)
                    {
                        //日志
                        log(String.Format("【解析SPTokenRequest参数失败：{0}】Result:{1},ErrMsg:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Result, ErrMsg));
                        string err = System.Web.HttpUtility.UrlEncode("积分商城请求的token解密失败!ErrMsg=" + ErrMsg, Encoding.UTF8);
                        Response.Redirect(JFLoginUrl + "?Result=" + Resultjf + "&ErrMsg=" + err, true);
                        return;
                    }
                    //日志
                    log(String.Format("【解析SPTokenRequest参数成功：{0}】SPID:{1},ProvinceID:{2},AuthenType:{3},AuthenName:{4},ReturnURL:{5}", 
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), SPID, ProvinceID, AuthenType, AuthenName, ReturnURL));
                    
                    string o_ProvinceID = "";
                    Result = BTForBusinessSystemInterfaceRules.UserAuthV2(SPID, AuthenName, AuthenType, Password, this.Context, ProvinceID, "", "",
                        out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  o_ProvinceID, out  RealName, out  UserName, out  NickName);
                    
                    //日志
                    log(String.Format("【Crm认证结果:{0}】Result:{1},ErrMsg:{2},CustID:{3},UserAccount:{4},CustType:{5},OutID:{6},ProvinceID:{7}", 
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Result, ErrMsg, CustID, UserAccount, CustType, OutID, o_ProvinceID));
                    
                    if (Result != 0)
                    {
                        string err = System.Web.HttpUtility.UrlEncode(ErrMsg, Encoding.UTF8);
                        string jf_loginUrl = "";
                        if (ReturnURL.IndexOf("?") > 0)
                        {
                            jf_loginUrl = ReturnURL + "&Result=" + Result + "&ErrMsg=" + err;
                        }
                        else
                        {
                            jf_loginUrl = ReturnURL + "?Result=" + Result + "&ErrMsg=" + err;
                        }

                        //直接将请求原路打回 --假设请求中有ReturnURL
                        Response.Redirect(jf_loginUrl + "", true);
                        return;
                    }
                    //生成token 修改 2013.01.23
                    string UserTokenValue = UT.GenerateJFUserToken(ProvinceID,CustID, RealName, UserName, NickName, OutID, CustType, AuthenName, AuthenType, key, out ErrMsg);
                    //UT.GenerateUserToken(CustID, RealName, UserName, NickName, OutID, CustType, AuthenName, AuthenType, key, out ErrMsg);
                    log("token-Result:" + Result);
                    string CookieName0 = System.Configuration.ConfigurationManager.AppSettings["CookieName"];  //CookieName = CIPUT
                    PageUtility.SetCookie(UserTokenValue, CookieName0, this.Page);

                    //生成流水号
                    //string TransactionId = "";
                    //string sDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    //Random r = new Random(Guid.NewGuid().GetHashCode());
                    //TransactionId = "35999999" + sDate + r.Next(10000, 99999).ToString();
                    String TransactionId = "35999999" + CommonBizRules.CreateTransactionID();

                    //生成ticket
                    //sDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    //r = new Random();
                    //string Ticket = sDate + r.Next(10000, 99999).ToString();
                    String Ticket = CommonBizRules.CreateTicket();

                    StringBuilder msg0 = new StringBuilder();
                    msg0.Append("++++++++++++++++++++++++++++++++++++++token===++++++++++++++++++++" + UserTokenValue + "++++++++++++++++++++++++++\r\n");
                    BTUCenterInterfaceLog.CenterForBizTourLog("REVERSE-SSO", msg0);

                    this.sendTicket2JF(Ticket, CustID, RealName, UserAccount, OutID, UserName, AuthenName, AuthenType, NickName);

                    #endregion
                }
            }
        }
    }

    public static void log(string str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("REVERSE-SSO", msg);
    }
}

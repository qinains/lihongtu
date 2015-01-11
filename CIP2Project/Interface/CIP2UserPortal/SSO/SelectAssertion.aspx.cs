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
using System.Text;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using BTUCenter.Proxy;


public partial class SSO_SelectAssertion : System.Web.UI.Page
{
    public static string UATicket = "";

    private String RawUrl = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (CommonUtility.IsParameterExist("UATicket", this.Page))
            {
                UATicket = Request["UATicket"];
                log("【UATicket:】" + UATicket);
                ssoFunc();
            }
            else
            {
                Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=请从网厅登陆4&FunctionName=请从网厅登陆4", true);
                return;
            }
        }
    }


    protected String GetTicketFromRawUrl(String RawUrl)
    {
        return null;
    }

    protected String GetReturnUrlFromRawUrl(String RawUrl)
    {
        return null;
    }


    protected void ssoFunc()
    {
        string QH = System.Configuration.ConfigurationManager.AppSettings["HQList"];
        //上海，广州处理
        string UAOUTID = System.Configuration.ConfigurationManager.AppSettings["UAOUTIDLIst"];

        string UAProvinceID =Request.Cookies["UAProvinceID"].Value.ToString();
        //是否是SSO的省
        if (QH.IndexOf(UAProvinceID) < 0 && UAOUTID.IndexOf(UAProvinceID) < 0)
        {
            Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=未开通单点登录&FunctionName=请从 http://jf.ct10000.com 直接登入集团积分商城", true);
            return;
        }

        MBOSSClass mboss = new MBOSSClass();
        string AssertionAddress = Request.Cookies["AssertionAddress"].Value.ToString();             //获取断言查询地址
        string TransactionID = Request.Cookies["TransactionID"].Value.ToString();                   //获取流水号
        
        string xml="";
        int result = -19999;
        string ErrMsg = "";

        string SPID = UAProvinceID + "999991";
        string CustID = "", RealName = "",NickName = "",UserName = "",OutID = "",UserAccount = "",CustType = "",ProvinceID = "" ,AuthenName = "",AuthenType = "";
        try
        {
           
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            //密钥
            string key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            //断言
            MBOSSClass.BilByCompilingResult bil = new MBOSSClass.BilByCompilingResult();
            //查询断言并解析
            result = mboss.SendUATicket(UAProvinceID,SPID, UATicket, AssertionAddress, this.Context, "SPCAData",TransactionID, out bil, out xml, out ErrMsg);
            AuthenType = bil.AccountType;
            AuthenName = bil.AccountID;
            
            if (result != 0)
            {
                Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=请从网厅登陆1&FunctionName=请从网厅登陆1", true);
                return;
            }
            //用户入库是否存在
            int type=0;
            string p = bil.AccountID;
            string dealType = "";
            string areaid = "";
            string jtUAProvinceID = "";

            log("UAProvinceID:集团ua：" + UAProvinceID );

            if ("35".Equals(UAProvinceID))
            {
                if (!"".Equals(bil.ProvinceID))
                    jtUAProvinceID = bil.ProvinceID;
                else
                    jtUAProvinceID = UAProvinceID;

                result = 0;
                type = 1;
            }
            else 
            {
                result = BTForBusinessSystemInterfaceRules.MUserAuthV2(SPID, UAProvinceID, bil.AccountID, bil.AccountType, bil.AccountInfos, Context,
                    out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  ProvinceID,
                    out  RealName, out  UserName, out  NickName, out dealType, out type, out areaid);
                log(bil.ProvinceID+"-!35-BTForBusinessSystemInterfaceRules.MUserAuthV2：" + result + "-bil.ProvinceID=" + bil.ProvinceID + "-UAProvinceID=" + UAProvinceID + "-areaid=" + areaid + "-custid=" + CustID + "-OutID=" + OutID + "-ErrMsg=" + ErrMsg);
            }

            string CustID1 = CustID;
            string RealName1="";
            string UserName1="";
            string NickName1="";
            string CustType1="";
            log("MUserAuthV2:" + result + ";CustID=" + CustID + " @----@" + ErrMsg + "==" + type);
            if (result!=0)
            {
                Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo="+ErrMsg+"&FunctionName=请从网厅登陆2", true);
                return;
            }
            //模式3情况下，获取证件号和类型         
            string TestStr = "";
            if (type == 1 )
            {
                string OutID1 = "";
                if (QH.IndexOf(UAProvinceID) >= 0)
                {
                    if (bil.AccountType == "9" || bil.AccountType == "10" || bil.AccountType == "11")
                    {
                        if (areaid != "")
                        {
                            bil.AccountID = areaid + "-" + bil.AccountID;
                        }
                    }
                }
                else if (UAOUTID.IndexOf(UAProvinceID) >= 0)
                {
                    bil.AccountID = OutID;
                    bil.AccountType = "99";
                  
                    RealName = bil.AccountID;
                }
                else
                {
                    if (bil.AccountType == "9" || bil.AccountType == "10" || bil.AccountType == "11")
                    {
                        string phone = "";
                        areaid = BTForBusinessSystemInterfaceRules.PhoneToArea(UAProvinceID, bil.AccountID, out phone);
                        if (areaid != "")
                        {
                            bil.AccountID = areaid +"-"+ phone;
                        }
                    }
                }

                int result1 = -1234;
                if ("35".Equals(UAProvinceID))
                {
                    log("CrmSSO.UserAuthCrm1:provinceid=" + bil.ProvinceID + ";areacode:" + bil.AccountInfos[0].areaid + ";accountid:" + bil.AudienceID + "|TestStr=" + TestStr);
                    result1 = CrmSSO.UserAuthCrm1(bil.ProvinceID, bil.AccountInfos[0].areaid, bil.AccountType, bil.AccountID, p, "", "0", UAProvinceID + "999991", this.Context, out RealName1, out UserName1, out NickName1, out OutID1, out CustType1, out CustID1, out ErrMsg, out TestStr);
                }
                else {
                    result1 = CrmSSO.UserAuthCrm(UAProvinceID, bil.AccountType, bil.AccountID, p, "", "0", UAProvinceID + "999991", this.Context, out RealName1, out UserName1, out NickName1, out OutID1, out CustType1, out CustID1, out ErrMsg, out TestStr);
                }

                if (result1 == 0)
                {
                    OutID = OutID1;
                    CustID = CustID1;
                    CustType = CustType1;
                }
                else {
                    Response.Redirect("../ErrorInfo.aspx?Result="+result1+"&ErrorInfo=" + ErrMsg + "&FunctionName=请从网厅登陆5", true);
                    return;
                }
                UserName = RealName1;
                NickName = RealName1;
                log(result1 + "==" + UAProvinceID + "=UAProvinceID;" + bil.AccountType + " =bil.AccountType;" + bil.AccountID + "=bil.AccountID;" + "" + "" + RealName + "=RealName;" + UserName + "=UserName;" + NickName +
                 "=NickName;" + OutID + "=OutID;" + CustType + "=CustType;" + CustID + "=CustID1;" + ErrMsg + "=ErrMsg");
            }

            if (dealType == "0" )            //通知积分系统
                CIP2BizRules.InsertCustInfoNotify(CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], "", "0", out ErrMsg);
           
            //生成cookie
            UserToken UT = new UserToken();
            string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OutID, CustType,AuthenName,AuthenType,key, out ErrMsg);
            string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);

            //生成ticket
            result = CIPTicketManager.insertCIPTicket(TransactionID, SPID, CustID, RealName, NickName, UserName, OutID, "", AuthenName, AuthenType, out ErrMsg);
            log("insertCIPTicket:" + ErrMsg + result);
            if (result != 0)
            {
                Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=请从网厅登陆3&FunctionName=请从网厅登陆3", true);
                return;
            }

            string Url = Request.Cookies["ReturnURL"].Value.ToString(); // System.Configuration.ConfigurationManager.AppSettings["SSOReturnURL"]; ;

            PageUtility.ExpireCookie("ReturnURL", this.Page);
            PageUtility.ExpireCookie("SPID", this.Page);
            PageUtility.ExpireCookie("UAProvinceID", this.Page);
            PageUtility.ExpireCookie("TransactionID", this.Page);

            Response.Redirect(Url + "?Ticket=" + TransactionID);
        }
        catch (System.Exception ex)
        {
            ErrMsg=ex.Message;           
        }
        finally
        {
            try
            {
                CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, ProvinceID, AuthenType, AuthenName,"2", result, ErrMsg);
            }
            catch { }
        }
    }

    public static void log(string str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("SSO", msg);
    }

}

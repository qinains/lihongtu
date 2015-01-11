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

public partial class SSO_SelectAssertion1 : System.Web.UI.Page
{
    public static String UATicket = "";
    private String RawUrl = String.Empty;
    private String ReturnUrl = String.Empty;
    private String QueryString = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            StringBuilder strLog = new StringBuilder();

            if (CommonUtility.IsParameterExist("UATicket", this.Page))
            {
                RawUrl = this.Context.Request.RawUrl;
                UATicket = GetParameterValueFromDecodeUrl("UATicket");
                ReturnUrl = GetParameterValueFromDecodeUrl("returnURL");
                strLog.AppendFormat("【UATicket:】{0};【RawUrl:】{1}\r\n", UATicket, RawUrl);
                log(strLog.ToString());
                QueryAssertionByTicket();
            }
            else
            {
                strLog.AppendFormat("ErrorInfo:{0}",RawUrl);
                log(strLog.ToString());
                Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=UAM返回的参数不正确!缺少UATicket", true);
                return;
            }
        }
    }


    protected String GetParameterValueFromDecodeUrl(String Key)
    {
        QueryString = RawUrl.Substring(RawUrl.IndexOf("UATicket="));
        String decodeQueryString = HttpUtility.UrlDecode(QueryString);
        String[] Parameters = QueryString.Split('&');
        foreach (String p in Parameters)
        {
            String[] vp = p.Split('=');
            if (vp[0].Equals(Key))
            {
                return vp[1];
            }
        }
        return String.Empty;
    }

   
    

    long LongRandom(long min, long max, Random rand)
    {
        byte[] buf = new byte[8];
        rand.NextBytes(buf);
        long longRand = BitConverter.ToInt64(buf, 0);
        return (Math.Abs(longRand % (max - min)) + min);
    }

    protected void QueryAssertionByTicket()
    {
        MBOSSClass mboss = new MBOSSClass();
        StringBuilder strLog = new StringBuilder();
        string xml = "";
        int Result = -19999;
        string ErrMsg = String.Empty;
        string UAProvinceID = "35";
        string SPID = UAProvinceID + "999991";
        string CustID = "", RealName = "", NickName = "", UserName = "", OutID = "", UserAccount = "", CustType = "", ProvinceID = "", AuthenName = "", AuthenType = "";
        string AssertionAddress = String.Empty;
        string SSOAddress = String.Empty;
        string SecretKey = String.Empty;
        try
        {
            //获取到集团断言查询地址,这里要注意个问题，取地址是从数据库中去，测试库上的地址已经改为新的断言地址，在正式库上是否要改？
            Result = mboss.GetMBOSSAddress(this.Context, SPID, out AssertionAddress, out SSOAddress, out ErrMsg);
            if (Result != 0)
            {
                strLog.Append("没有获取到集团断言查询地址;\r\n");
                //SendJF();   没有获取到集团断言查询地址
                return;
            }
            PageUtility.SetCookie(AssertionAddress, "AssertionAddress", this.Page);
            PageUtility.SetCookie(SSOAddress, "SSOAddress", this.Page);
            //35000 20130808 5146985330
            string TransactionID = "35000" + DateTime.Now.ToString("yyyyMMdd") + Convert.ToString(LongRandom(1000000000, 9999999999, new Random()));                   //获取流水号
            //密钥
            Result = mboss.GetMBOSSSecretKey(this.Context, SPID, out SecretKey, out ErrMsg);
            if (Result != 0)
            {
                strLog.Append("没有获取到密钥;\r\n");
                //没有获取到密钥；
                return;
            }

            //断言
            MBOSSClass.BilByCompilingResult bil = new MBOSSClass.BilByCompilingResult();
            //查询断言并解析
            Result = mboss.SendUATicket(UAProvinceID, SPID, UATicket, AssertionAddress, this.Context, "SPCAData", TransactionID, out bil, out xml, out ErrMsg);
            AuthenType = bil.AccountType;
            AuthenName = bil.AccountID;
            strLog.AppendFormat("断言查询返回的报文:{0}\r\n",xml);
            if (Result != 0)
            {
                //Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=请从网厅登陆1&FunctionName=请从网厅登陆1", true);
                Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=断言解析错误!", true);
                return;
            }


            //用户入库是否存在,没有则新增，有则update
            string dealType = String.Empty;
            int  type = 0;
            string areaid = String.Empty;

            Result = BTForBusinessSystemInterfaceRules.MUserAuthV2(SPID, UAProvinceID, bil.AccountID, bil.AccountType, bil.AccountInfos, Context,
            out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  ProvinceID,
            out  RealName, out  UserName, out  NickName, out dealType, out type, out areaid);

            if (Result != 0)
            {
                Response.Redirect("../ErrorInfo.aspx?Result=" + Result + "&ErrorInfo=" + ErrMsg + "&FunctionName=请从网厅登陆4", true);
                return;
            }

            //如果客户信息不全，则去crm查询一把  这里要注意的是，必须根据集团返回的断言中的UAID 当成省码传给枢纽
            string TestStr = String.Empty; 
            Result = CrmSSO.UserAuthCrm1(bil.ProvinceID, bil.AccountInfos[0].areaid, bil.AccountType, bil.AccountID, UAProvinceID, "", "0", UAProvinceID + "999991", this.Context, out RealName, out UserName, out NickName, out OutID, out CustType, out CustID, out ErrMsg, out TestStr);
            if (Result != 0)
            {
                Response.Redirect("../ErrorInfo.aspx?Result=" + Result + "&ErrorInfo=" + ErrMsg + "&FunctionName=请从网厅登陆5", true);
                return;
            }
            strLog.Append(Result + "==" + UAProvinceID + "=UAProvinceID;" + bil.AccountType + " =bil.AccountType;" + bil.AccountID + "=bil.AccountID;" + "" + "" + RealName + "=RealName;" + UserName + "=UserName;" + NickName + "=NickName;" + OutID + "=OutID;" + CustType + "=CustType;" + CustID + "=CustID1;" + ErrMsg + "=ErrMsg\r\n");
            if (dealType == "0")            //通知积分系统
                CIP2BizRules.InsertCustInfoNotify(CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], "", "0", out ErrMsg);

            //生成ticket
            Result = CIPTicketManager.insertCIPTicket(TransactionID, SPID, CustID, RealName, NickName, UserName, OutID, "", AuthenName, AuthenType, out ErrMsg);
            Response.Redirect(ReturnUrl + "?Ticket=" + TransactionID);
        }
        catch (Exception e)
        {
            strLog.AppendFormat(e.Message);
        }
        finally
        {
            try
            {
                CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, ProvinceID, AuthenType, AuthenName, "2", Result, ErrMsg);
           
            }
            catch { }
        }



    }


    public static void log(string str)
    {
        StringBuilder msg = new StringBuilder();
       
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(HttpContext.Current.Request.UserHostAddress+"\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("UAM-SelectAssertion", msg);
    }

}

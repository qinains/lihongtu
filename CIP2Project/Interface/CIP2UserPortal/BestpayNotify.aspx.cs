using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

public partial class BestpayNotify : System.Web.UI.Page
{
    String MERCHANTID = String.Empty;
    String ORDERSEQ = String.Empty;
    String TRANSDATE = String.Empty;
    String AUTHCODE = String.Empty;
    String ACCOUNT = String.Empty;
    String USERACCOUNT = String.Empty;
    String RTNCODE = String.Empty;
    String MAC = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        int Result = 0;
        String ErrMsg = "";
        if(!this.IsPostBack)
        {

            if (CommonUtility.IsParameterExist("MERCHANTID", this.Page))
            {
                MERCHANTID = Request["MERCHANTID"];
            }
            if (CommonUtility.IsParameterExist("ORDERSEQ", this.Page))
            {
                ORDERSEQ = Request["ORDERSEQ"];
            }
            if (CommonUtility.IsParameterExist("TRANSDATE", this.Page))
            {
                TRANSDATE = Request["TRANSDATE"];
            }
            if (CommonUtility.IsParameterExist("AUTHCODE", this.Page))
            {
                AUTHCODE = Request["AUTHCODE"];
            }
            if (CommonUtility.IsParameterExist("ACCOUNT", this.Page))
            {
                ACCOUNT = Request["ACCOUNT"];
            }
            if (CommonUtility.IsParameterExist("USERACCOUNT", this.Page))
            {
                USERACCOUNT = Request["USERACCOUNT"];
            }
            if (CommonUtility.IsParameterExist("RTNCODE", this.Page))
            {
                RTNCODE = Request["RTNCODE"];
            }
            if (CommonUtility.IsParameterExist("MAC", this.Page))
            {
                MAC = Request["MAC"];
            }
            String BestpayKey = System.Configuration.ConfigurationManager.AppSettings["BestpayKey"];
            //MERCHANTID=0018888888&ORDERSEQ=20120626114801001&TRANSDATE=20120626&AUTHCODE=AV12346&ACCOUNT=18901951201&USERACCOUNT=xiakun &RTNCODE=0&KEY= KWSDAWAD
            String newMAC = GetMD5Hash("MERCHANTID=" + MERCHANTID + "&ORDERSEQ=" + ORDERSEQ + "&TRANSDATE=" + TRANSDATE + "&AUTHCODE=" + AUTHCODE + "&ACCOUNT=" + ACCOUNT + "&USERACCOUNT=" + USERACCOUNT + "&RTNCODE=" + RTNCODE + "&KEY=" + BestpayKey);
            if (MAC.Equals(newMAC))
            {
                // 1.在客户信息平台中绑定AUTHCODE->custid  //ACCOUNT=custid
                Result = CIP2BizRules.BindingBestpayAccount2BesttoneAccount("3", USERACCOUNT, ACCOUNT, out ErrMsg);
                if(Result==0)
                {
                    Response.Write("00");
                    // 2.返回翼支付一个返回值
                }else
                {
                    Response.Write(Result);
                }
                
                
                // 3.通知精品商城



            }
        }
 
    
    }


    private static string GetMD5Hash(string input)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");

    }


}

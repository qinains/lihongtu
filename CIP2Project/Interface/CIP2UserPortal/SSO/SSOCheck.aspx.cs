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



public partial class SSO_SSOCheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
            this.ssoFunc();
    }
    protected void ssoFunc()
    {
        //string SPTokenRequest = "";
        //if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        //{
        //    SPTokenRequest = Request["SPTokenRequest"];

        //}
        //else
        //{
            //string str=this.TextBox1.Text.Trim();
            //MBOSSClass mboss = new MBOSSClass();
            //SPInfoManager spInfo = new SPInfoManager();

           
            //byte[] privateKeyFile;

      
            //string ErrMsg = "";
            //MBOSSClass.SSOAddressResp SSOAddress=new MBOSSClass.SSOAddressResp();
          
            //Object SPData = spInfo.GetSPData(this.Context, "35000001");
            //string UserName = "";
            //string privateKeyPassword = "";
            //privateKeyFile = spInfo.GetCAInfo("35111111", 1, SPData, out UserName, out privateKeyPassword);

            //int result = mboss.AuthenSelectArddess(str, privateKeyFile, privateKeyPassword, out SSOAddress, out ErrMsg);

           
            //if(result ==0)
            //{
           
            //}
             SSOAddress1 = "http://135.129.9.60:7001/services/CrmToMallSoapImpl?wsdl";// SSOAddress.SSOAddress;
             AssertionAddress1 = " http://forum.ct10000.com:809/uam-gate/services/SelectAssertion?wsdl";//SSOAddress.AssertionAddress;
            this.TextBox2.Text = "省级UA的SSO接入地址:" + SSOAddress1;
            this.TextBox3.Text = "省级UA的断言查询地址:" + AssertionAddress1;
           // this.TextBox4.Text = result.ToString();


            PageUtility.SetCookie(AssertionAddress1, "AssertionAddress", this.Page);
        //}
       
    }

    public static string SSOAddress1 = "";
    public static string AssertionAddress1 = "";

   
    public string StrSSORequestXML="";
    public string GetInfoByTicketURL="";

    protected void Button2_Click(object sender, EventArgs e)
    {
        string mm = Request["UATicket"].ToString();
        
        Response.Redirect("SelectAssertion.aspx?StrSSORequestXML=" + mm);
    }

}

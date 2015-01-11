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
using System.Net;
using System.IO;
using System.Xml;


public partial class SSO_login1 : System.Web.UI.Page
{
    string SPID = "35" + "999991";
    string USPID = String.Empty;                       //省uam
    string ReturnURL = "";
    string SSOAddress = String.Empty;                  //SSO认证地址
    string AssertionAddress = String.Empty;            //断言查询地址
   
    string ErrMsg = "";
    int Result = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!this.IsPostBack )
        {
            ssoFunc();
        }
    }

    protected void ssoFunc()
    {
        string UAProvinceID = Request["ProvinceID"];
        PageUtility.SetCookie(UAProvinceID, "UAProvinceID", this.Page);

        if (CommonUtility.IsParameterExist("UATicket", this.Page))
        {
            Response.Redirect("SelectAssertion.aspx?UATicket=" + Request["UATicket"]);
        }
        if (CommonUtility.IsParameterExist("ProvinceID", this.Page))
        {
            ReturnURL = Request.Cookies["ReturnURL"].Value.ToString();
        }
        else
            return ;

        USPID = UAProvinceID + "999991";
       
        //1判断全局Token是否存在
        try
        {
            MBOSSClass mboss = new MBOSSClass();
            //MBOSSClass.SSOAddressResp SSOAddress;
            //Result = mboss.AuthenSelectArddess(ProvinceID, this.Context, "SPCAData", out SSOAddress, out ErrMsg);
            //if (Result != 0)
            //{
            //    SendJF();
            //    return;
            //}
            //string SSOAddress1 = SSOAddress.SSOAddress;//省级UA的SSO接入地址
            //string AssertionAddress1 = SSOAddress.AssertionAddress;//省级UA的断言查询地址
            
            //获取SSO认证地址和断言查询地址
            Result = mboss.GetMBOSSAddress(this.Context, USPID, out AssertionAddress, out SSOAddress, out ErrMsg);
            if (Result != 0)
            {                 
                SendJF();
                return;
            }
            
            PageUtility.SetCookie(AssertionAddress, "AssertionAddress", this.Page);
            PageUtility.SetCookie(SSOAddress, "SSOAddress", this.Page);

            //SPID = this.Response.Cookies["SPID"].Value.ToString();  
            //向归属地ＵＡ发送身份认证请求
            //MBOSSClass.AcceptAccountTypeList[] acs = new MBOSSClass.AcceptAccountTypeList[1];
            //MBOSSClass.AcceptAccountTypeList ac = new MBOSSClass.AcceptAccountTypeList();
            //ac.AcceptAccountType = "0000000";
            //acs[0] = ac;
            MBOSSClass.AcceptAccountTypeList[] acs = new MBOSSClass.AcceptAccountTypeList[1];
            MBOSSClass.AcceptAccountTypeList ac = new MBOSSClass.AcceptAccountTypeList();
            //ac.AcceptAccountType = "2000001";

            //acs[0] = ac;
            //MBOSSClass.AcceptAccountTypeList ac1 = new MBOSSClass.AcceptAccountTypeList();
            //ac1.AcceptAccountType = "2000002";
            //acs[1] = ac1;
            //MBOSSClass.AcceptAccountTypeList ac2 = new MBOSSClass.AcceptAccountTypeList();
            //ac2.AcceptAccountType = "2000003";
            //acs[2] = ac2;
            //MBOSSClass.AcceptAccountTypeList ac3 = new MBOSSClass.AcceptAccountTypeList();
            //ac3.AcceptAccountType = "2000004";
            //acs[3] = ac3;

            //MBOSSClass.AcceptAccountTypeList ac4 = new MBOSSClass.AcceptAccountTypeList();
            ac.AcceptAccountType = "0000000";
            acs[0] = ac;

            string ResultXML = "";
            string TransactionID = "";
            string SelectAssertion = System.Configuration.ConfigurationManager.AppSettings["SelectAssertion"];

            log("积分商城ReturnURL=" + ReturnURL);
            Result = mboss.SSOAuthanXML(UAProvinceID,SPID, SelectAssertion, acs, this.Context, "SPCAData", out ResultXML, out ErrMsg, out TransactionID);
            log(" mboss.SSOAuthanXML Result" + Result + "ResultXML " + ResultXML + "ErrMsg" + ErrMsg);

            if (Result != 0)
            {
                SendJF();
                return;
            }

            PageUtility.SetCookie(TransactionID, "TransactionID", this.Page);
            
            //post到sso认证地址
            Response.Write("<form name='frm' id='frm' action='" + SSOAddress + "' method='post'>");
            Response.Write("<input name='SSORequestXML' value='" + ResultXML + "'  type='hidden'  >");
            Response.Write("</form>");
            Response.Write("<script language='javascript'>frm.submit();</script>");
        }
        catch (Exception err)
        {
            SendJF();
            return;
        }
       
    }

    public void SendJF()
    {
        Response.Redirect("../ErrorInfo.aspx?Result=-19999&ErrorInfo=请从网厅登陆&FunctionName=请从网厅登陆", true); ;
    }

    public static void log(string str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        BTUCenterInterfaceLog.CenterForBizTourLog("SSO", msg);
    }
    
    #region 同步通过POST方式发送数据
    
    /// <summary>
    /// 通过POST方式发送数据
    /// </summary>
    /// <param name="Url">url</param>
    /// <param name="postDataStr">Post数据</param>
    /// <param name="cookie">Cookie容器</param>
    /// <returns></returns>
    public string SendDataByPost(string Url, string postDataStr)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
       

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = postDataStr.Length;
        Stream myRequestStream = request.GetRequestStream();
        StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
        myStreamWriter.Write(postDataStr);
        myStreamWriter.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        string retString = myStreamReader.ReadToEnd();
        myStreamReader.Close();
        myResponseStream.Close();

        return retString;
    }
    
    #endregion

}

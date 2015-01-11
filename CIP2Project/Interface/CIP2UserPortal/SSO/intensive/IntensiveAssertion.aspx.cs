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


public partial class SSO_intensive_IntensiveAssertion : System.Web.UI.Page
{
    string SPID = "35999991";
    protected void Page_Load(object sender, EventArgs e)
    {

        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        string ErrMsg = "";

        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        string token = PageUtility.GetCookie(CookieName);
        string ProvinceID = "";
        string CustID = "";
        string RealName = "";
        string UserName = "";
        string NickName = "";
        string OuterID = "";
        string CustType = "";
        string AuthenName = "";
        string AuthenType = "";
        
        if (String.IsNullOrEmpty(token))
        {
            Response.Redirect("../../ErrorInfo.aspx?Result=-19999&ErrorInfo=请先登录积分商城", true);
            return;
        }

        UserToken UT = new UserToken();
        string key = System.Configuration.ConfigurationManager.AppSettings["ScoreSystemSecret"];
        Result = UT.ParseScoreUserToken(token, key, out ProvinceID, out CustID, out RealName, out UserName, out NickName, out OuterID, out CustType, out AuthenName, out AuthenType, out ErrMsg);



        MBOSSClass mboss = new MBOSSClass();
        SPInfoManager spInfo = new SPInfoManager();


        string privateKeyPassword = "";
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
        if (Result != 0)
        {
            // 签名校验未通过,直接将请求原路打回 
            Response.Redirect(RedirectURL, true);
            return;
        }


    }
}

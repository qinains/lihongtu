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

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.Rule;

public partial class Demo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string comefrom_url = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
        Response.Write("-----------");
        bool se = HttpContext.Current.Request.IsSecureConnection;
        bool au = HttpContext.Current.Request.IsAuthenticated;
        Response.Write("se="+se);
        Response.Write("au="+au);
        Response.Write("-----------");

        //Response.AddHeader("P3P", "CP=CAO PSA OUR");
        String SrcSsDeviceNo = "3500000000408201";
        String UDBKey = "3C67B5657DF383DFE5FDBC449FFC850B8EB79459AA369011";
        String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String ReturnUrl = "http://go.118114.cn";
        String digest = String.Empty, PassportLoginRequestValue = String.Empty;

        digest = CryptographyUtil.ToBase64String(CryptographyUtil.Hash(SrcSsDeviceNo + TimeStamp + ReturnUrl));
        
        PassportLoginRequestValue = HttpUtility.UrlEncode(SrcSsDeviceNo + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + ReturnUrl + "$" + digest, UDBKey));

        Response.Write(digest + "<br/>");
        Response.Write(PassportLoginRequestValue + "<br/>");
        this.hdUDBUrl.Value = "http://Service.Passport.189.cn/Logon/UDBCommon/S/PassportLogin.aspx?PassportLoginRequest=" + PassportLoginRequestValue;

        Response.Write("-----------");

    
    }
}

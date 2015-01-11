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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.Rule;
public partial class SSO_mobile_TianYiLoginTest : System.Web.UI.Page
{
    String SPTokenRequest = String.Empty;
    String EncryptStr = String.Empty;
    String SPID = String.Empty;
    String UAProvinceID = String.Empty;
    String SourceType = String.Empty;
    String ReturnURL = String.Empty;
    String TimeStamp = String.Empty;
    String Digest = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        CreateSPTokenRequest();

        this.HyperLink1.NavigateUrl = "TianYiLogin.aspx?SPTokenRequest=" + SPTokenRequest;
    }

    protected void CreateSPTokenRequest()
    {
        SPID = "35433334";
        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
        UAProvinceID = "02";
        SourceType = "4";
        ReturnURL = "http://114yg.cn";  // 这里问翼购要地址 wap的
        TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        Digest = CryptographyUtil.GenerateAuthenticator(UAProvinceID + "$" + SourceType + "$" + ReturnURL + "$" + TimeStamp, ScoreSystemSecret);
        EncryptStr = CryptographyUtil.Encrypt(UAProvinceID + "$" + SourceType + "$" + ReturnURL + "$" + TimeStamp + "$" + Digest, ScoreSystemSecret);
        SPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + EncryptStr);
    }
}

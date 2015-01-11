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
public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //CommonBizRules.ErrorHappenedRedircet(-22500, "测试", "登录", this.Context);
        //CommonBizRules.SuccessRedirect(@"", "登录", this.Context );
        string PageName = Request.Url.AbsolutePath;
        int LastLine = PageName.LastIndexOf('/');
        int LocationASPX = PageName.LastIndexOf(".aspx");
        PageName = PageName.Substring(LastLine+1, LocationASPX - LastLine);
        Response.Write(PageName);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        //SPInfoManager spInfo = new SPInfoManager();
        //Object SPData = spInfo.GetSPData(this.Context, "SPData");
        //string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);

        //string CustID = txtCustID.Text.Trim();
        //string RealName = txtRealName.Text.Trim();
        //string NickName = txtNickName.Text.Trim();
        //string UserName = txtUserName.Text.Trim();
        //string AuthName="";
        //string AuthType="";
        //string CustType = "";
        //UserToken UT = new UserToken();
        //string ErrMsg = "";
        //string OuterID = "";
     
        //string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OuterID, "41", UserName,"1",key, out ErrMsg);
        //string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];

        //PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
        //UT.ParseUserToken(UserTokenValue, key, out CustID, out RealName, out UserName, out NickName, out OuterID, out CustType, out AuthName,out AuthType,  out ErrMsg);
        
        //txtCustID2.Text = CustID;
        //txtNickName2.Text = NickName;
        //txtRealName2.Text = RealName;
        //txtUserName2.Text = UserName;
    }
}

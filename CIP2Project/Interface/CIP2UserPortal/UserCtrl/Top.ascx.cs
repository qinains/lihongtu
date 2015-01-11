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

public partial class UserCtrl_Top : System.Web.UI.UserControl
{

    private string welcome;
    public string Welcome
    {
        get { return welcome; }
        set
        {
            welcome = value; 
            
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        top_welcome.InnerHtml = welcome;
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        if (PageUtility.IsCookieExist(CookieName, this.Context))
        {
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
            UserToken UT = new UserToken();
            string strCIPToken = Request.Cookies.Get(CookieName).Value;
            string custID;
            string realName;
            string userName;
            string nickName;
            string outerID;
            string custType;
            string loginAuthenName;
            string loginAuthenType;
            string errMsg;
            int result = UT.ParseUserToken(strCIPToken, key, out custID, out realName, out userName, out nickName, out outerID, out custType, out loginAuthenName, out loginAuthenType, out errMsg);
            
            if (result==0){

                if (realName!=null&&! "".Equals(realName))
                {
                    top_name.InnerHtml = "您好，" + realName;
                }
                else if (nickName != null && !"".Equals(nickName))
                {
                    top_name.InnerHtml = "您好，" + nickName;
                }
                else if (userName != null && !"".Equals(userName))
                {
                    top_name.InnerHtml = "您好，" + userName;
                }
                
            }
           
        }

    }
}

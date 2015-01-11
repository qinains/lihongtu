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
using System.Text;

public partial class SSO_LogoutUDB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            StringBuilder strMsg = new StringBuilder();
            try
            {
                Response.Write("<iframe frameborder='1'  width='100'   height='100'src='" + UDBConstDefinition.DefaultInstance.UDBLogoutUrl + "'  style='display:none'></iframe>");

                //清掉客户信息平台的cookie
                string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                PageUtility.ExpireCookie(CookieName, this.Page);

                String ReturnUrl = PageUtility.GetCookie("ReturnUrl");
                PageUtility.ExpireCookie("ReturnUrl", this.Page);
                this.hdReturnUrl.Value = Request["ReturnUrl"] == null ? String.Empty : Request["ReturnUrl"];
            }
            catch (Exception ex)
            {
                strMsg.Append(",异常:" + ex.Message);
            }
            finally
            {
                WriteLog(strMsg.ToString());
            }
        }
        
    }

    /// <summary>
    /// 写日志功能
    /// </summary>
    protected void WriteLog(String str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("LogoutUDB", msg);
    }
}

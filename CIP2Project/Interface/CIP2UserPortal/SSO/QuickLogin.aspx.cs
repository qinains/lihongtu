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
using System.Text;
using Linkage.BestTone.Interface.BTException;

public partial class SSO_QuickLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string SPID = "35000000";
        string AuthenType = "";
        string AuthenName = "";
        string Password = "";
        string CustID = "";
        string RealName = "";
        string NickName = "";
        string UserName = "";
        string OutID = "";
        string UserAccount = "";
        string CustType = "";
        string ProvinceID = "";
        string Ticket = "";
        string ReturnUrl = "";

        int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        string ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;

        try
        {
            if (CommonUtility.IsParameterExist("LoginTicket", this.Page))
            {
                Ticket = Request["LoginTicket"];
                Result = CIPTicketManager.checkCIPTicket(SPID, Ticket, "", out CustID, out RealName, out UserName, out NickName, out OutID, "", out AuthenName, out AuthenType, out ErrMsg);
                Log(String.Format("SPID:{0},Ticket:{1},CustID:{2},RealName:{3},UserName:{4},NickName:{5},OutID:{6},AuthenName:{7},AuthenType:{8},Result:{9},ErrMsg:{10}——【DateTime:{11}】",
                    SPID, Ticket, CustID, RealName, UserName, NickName, OutID, AuthenName, AuthenType, Result, ErrMsg, DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
                
                if (Result == 0)
                {
                    SPInfoManager spInfo = new SPInfoManager();
                    Object SPData = spInfo.GetSPData(this.Context, "SPData");
                    string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);

                    UserToken UT = new UserToken();

                    string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OutID, CustType, AuthenName, AuthenType, key, out ErrMsg);

                    string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];

                    PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
                    if (CommonUtility.IsParameterExist("ReturnUrl", this.Page))
                    {
                        ReturnUrl = Request["ReturnUrl"];
                        Response.Redirect(ReturnUrl);
                    }

                    Response.Redirect("http://www.118114.cn");
                }
                else
                {
                    Response.Redirect("../ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                }
            }
            else
            {
                Response.Redirect("http://www.118114.cn");
            }
        }
        catch (Exception ex)
        {
            ErrMsg += ex.Message;
        }
        finally
        {
            Log(String.Format("LoginTicket:{0},ErrMsg:{1}——【DateTime:{2}】", Ticket, ErrMsg, DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
        }
    }

    protected void Log(String strMsg)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(strMsg);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("QuickLogin", msg);
    }
}

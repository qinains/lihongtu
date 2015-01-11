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
public partial class FindByPhone : System.Web.UI.Page
{
    public String AuthenPhone = String.Empty;
    public String SPID = String.Empty;
    public String SmsAuthenCode = String.Empty;
    public String ReturnUrl = String.Empty;
    public String SPTokenRequest;
    public String newSPTokenRequest;
    public Int32 Result;
    public String ErrMsg = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (!Page.IsPostBack)
       // {
            SPID = Request["SPID"] == null ? String.Empty : Request["SPID"].ToString();
            
            if ("35433334".Equals(SPID))
            {
                ReturnUrl = "http://www.114yg.cn/userCenterAction.do?actions=intoUserLogin";
            }
            else
            {
                ReturnUrl = "http://sso.118114.cn/SSO/loginV2.action";
            
            }
            this.hdReturnUrl.Value = ReturnUrl;
            AuthenPhone = Request["AuthenPhone"];
            SmsAuthenCode = Request["AuthenCode"];
            if (!String.IsNullOrEmpty(AuthenPhone))
            {
                this.hdAuthenPhone.Value = AuthenPhone;
            }
            if (!String.IsNullOrEmpty(SmsAuthenCode))
            {
                this.hdAuthenCode.Value = SmsAuthenCode;
            }

        //}

    }
    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("FindByPhoneV2", msg);
    }


    protected void RestPasswordByPhoneBtn_Click(object sender, ImageClickEventArgs e)
    {

        SPID = Request["SPID"] == null ? String.Empty : Request["SPID"].ToString();

        if ("35433334".Equals(SPID))
        {
            ReturnUrl = "http://www.114yg.cn/userCenterAction.do?actions=intoUserLogin";
        }
        else
        {
            ReturnUrl = "http://sso.118114.cn/SSO/loginV2.action";

        }
        this.hdReturnUrl.Value = ReturnUrl;
        AuthenPhone = Request["AuthenPhone"];
        SmsAuthenCode = Request["AuthenCode"];
        if (!String.IsNullOrEmpty(AuthenPhone))
        {
            this.hdAuthenPhone.Value = AuthenPhone;
        }
        if (!String.IsNullOrEmpty(SmsAuthenCode))
        {
            this.hdAuthenCode.Value = SmsAuthenCode;
        }


        AuthenPhone = this.hdAuthenPhone.Value;
        SmsAuthenCode = this.hdAuthenCode.Value;
        ReturnUrl = this.hdReturnUrl.Value;
        String CustID = "";
        StringBuilder sbLog = new StringBuilder();
        sbLog.AppendFormat("AuthenPhone:{0}\r\n",AuthenPhone);
        sbLog.AppendFormat("SmsAuthenCode:{0}\r\n", SmsAuthenCode);
        try
        {
            CustID = PhoneBO.IsAuthenPhone(AuthenPhone, SPID, out ErrMsg);
            sbLog.AppendFormat("CustID:{0}\r\n",CustID);
            sbLog.AppendFormat("ErrMsg:{0}\r\n",ErrMsg);
            if (!String.IsNullOrEmpty(CustID))
            {
                Result = PhoneBO.SelSendSMSMassage(CustID, AuthenPhone, SmsAuthenCode, out ErrMsg);
                sbLog.AppendFormat("SelSendSMSMassage:Result:{0}-{1}\r\n",Result,ErrMsg);
                if (Result == 0)
                {
                    Response.Redirect("ResetPwdByPhone.aspx?UrlParam=" +CustID+"$2$"+ ReturnUrl,false);
                }
                else
                {
                    CommonBizRules.SuccessRedirect("../ErrorInfo.aspx", "找回密码失败:" + ErrMsg, HttpContext.Current);
                }
            }
            else
            {
                CommonBizRules.SuccessRedirect("../ErrorInfo.aspx", "该手机号码不是认证手机，找回密码失败:" + ErrMsg, HttpContext.Current);
            }
        }
        catch (Exception ex)
        {
            sbLog.AppendFormat("异常:{0}\r\n",ex.Message);
        }
        finally
        {
            log(sbLog.ToString());
        }
        
        
    }
}

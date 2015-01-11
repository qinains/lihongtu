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
using System.Text.RegularExpressions;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using log4net;

public partial class CustInfoManager_M_AuthenMobileM : System.Web.UI.Page
{
    public String SPTokenRequest;
    public String newSPTokenRequest;
    public Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    public String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
    public String SPID = "";
    public String ReturnUrl;
    public String LoginPassword = "";
    public String AuthenCode = "";
    public String CheckCode = "";
    public String Phone = "";
    public String CustID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
    
            SPID = Request["SPID"];
            ParseSPTokenRequest();
            CreateSPTokenRequest();
        

    }


    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //日志
            strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
            //解析请求参数
            Result = SSOClass.ParseBesttoneAccountPageRequestM(SPTokenRequest, this.Context, out SPID, out CustID, out ReturnUrl, out ErrMsg);
            this.HiddenField_CUSTID.Value = CustID;
            this.HiddenField_SPID.Value = SPID;
            this.HiddenField_URL.Value = ReturnUrl;
        }

    }

    protected void CreateSPTokenRequest()
    {

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

        String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

        UserToken UT = new UserToken();
        newSPTokenRequest = UT.GenerateBestAccountMainUserTokenM(CustID, ReturnUrl, TimeStamp, ScoreSystemSecret, out ErrMsg);
        newSPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + newSPTokenRequest);
    }
    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("wap-m-authenphone", msg);
    }


   

    protected void SetAuthenPhoneBtn_Click(object sender, EventArgs e)
    {
        LoginPassword = Request["LoginPassword"];
        Phone = Request["Phone"];
        AuthenCode = Request["AuthenCode"];
        CheckCode = Request["CheckCode"];

        // 校验LoginPassword

        try
        {
            if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(CheckCode), this.Context))
            {
                errorHint.InnerHtml = "<script type='text/javascript'>showError('验证码校验未通过！')</script>";
                return;
            }
            else
            {
                string webpwd = CryptographyUtil.Encrypt(LoginPassword);
                int i = FindPwd.SelState(CustID, webpwd, out ErrMsg);
                if (i != 0)
                {
                    errorHint.InnerHtml = "<script type='text/javascript'>  $('#LoginPassword').attr('value','" + LoginPassword + "');$('#Phone').attr('value','" + Phone + "');$('#AuthenCode').attr('value','" + AuthenCode + "');$('#CheckCode').attr('value','" + CheckCode + "');showError('登录密码输入错误，请重新输入！')</script>";
                    return;
                }
                else
                {
                    Result = PhoneBO.SelSendSMSMassage(CustID, Phone, AuthenCode, out ErrMsg);   // 校验手机验证码
                    if (Result == 0)
                    {
                        Result = PhoneBO.PhoneSetV2(SPID, CustID, Phone, "2", "2", out ErrMsg);
                        if (Result == 0)
                        {
                            //跳转
                            errorHint.InnerHtml = "<script type='text/javascript'>showError('认证手机设置成功！')</script>";
                            //Response.Redirect("m.114yg.cn",true);
                            return;
                        }
                        else
                        {
                            errorHint.InnerHtml = "<script type='text/javascript'>showError('" + ErrMsg + "！')</script>";
                            return;
                        }
                    }
                    else
                    {
                        errorHint.InnerHtml = "<script type='text/javascript'>showError('" + ErrMsg + "！')</script>";
                        return;
                    }
                }

            }
        }
        catch (Exception exp)
        {
            errorHint.InnerHtml = "<script type='text/javascript'>showError('" + exp.ToString() + "！')</script>";
            return;
        }


     

    }
}

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

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public partial class CustInfoManager_M_LowSintRegisterM : System.Web.UI.Page
{

    public String ReturnUrl = "";
    public String Device = "wap";
    public String SPID = "35433334";
    public String CustID = "";
    public String SPTokenRequest = "";

    public String UserName = String.Empty;
    public String Password = String.Empty;
    public String Password2 = String.Empty;

    private Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    private String newSPTokenRequest = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Device = Request["Device"];
        if (String.IsNullOrEmpty(Device))
        {
            Device = "android";
        }
        ParseSPTokenRequest();
    }

    /// <summary>
    /// 判断并解析SPTokenRequest参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        string UAProvinceID = "";
        string SourceType = "";
        //string ReturnURL = "";
        try
        {

            if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
            {
                SPTokenRequest = Request["SPTokenRequest"];
                strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
                Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnUrl, out ErrMsg);
                this.HiddenField_SPID.Value = SPID;
                strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnUrl));
            }
            else
            {
                //this.errorHint.InnerText = "SPTokenRequest参数缺失";
                return;
            }
        }
        catch (System.Exception ex)
        {
            log(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
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
        BTUCenterInterfaceLog.CenterForBizTourLog("wap-mobile-register", msg);
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        UserName = Request.Form["UserName"].ToString().Trim();

        Password = Request.Form["Password"].ToString().Trim();

        Password2 = Request.Form["Password2"].ToString().Trim();

        try
        {

            Result = CustBasicInfo.IsExistUser(UserName);
            if (Result != 0)
            {
                errorHint.InnerHtml = "<script type='text/javascript'>showError('用户名已存在！')</script>";
                return;
            }

            Result = UserRegistry.UserRegisterWebLowStint(SPID, UserName, Password, out CustID, out ErrMsg);

            if (Result == 0)
            { 
                    // 重定向到欢迎页面

                String IPAddress = Request.UserHostAddress.ToString();
                CommonBizRules.WriteTraceIpLog(CustID, UserName, SPID, IPAddress, "client_wap");


                String youhuiquan_url = "http://www.114yg.cn/facadeHome.do?actions=facadeHome&method=sendCouponToRegist&wt=json&from=" + Device + "&custId=" + CustID;
                String jsonmsg = HttpMethods.HttpGet(youhuiquan_url);
                System.Collections.Generic.Dictionary<string, string> resuzt = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(jsonmsg);
                //{"returnCode":"00000"}
                string youhuiquan = "";
                resuzt.TryGetValue("returnCode", out youhuiquan);


                String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(this.Context, "SPData");
                String key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
                String Digest = CryptographyUtil.GenerateAuthenticator(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg, key);
                String temp = SPID + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg + "$" + Digest, key);
                String RegistryResponseValue = HttpUtility.UrlEncode(temp);

                //给用户写cookie
                UserToken UT = new UserToken();
                String RealName = UserName;
                String NickName = UserName;
                string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, "", "42", UserName, "1", key, out ErrMsg);
                string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
                CreateSPTokenRequest();
                StringBuilder URL = new StringBuilder();
                URL.Append("LowSintRegisterMSuccess.aspx?SPID=");
                Response.Redirect(URL.ToString() + SPID + "&SPTokenRequest=" + newSPTokenRequest, true);
            }
            else
            {
                errorHint.InnerHtml = "<script type='text/javascript'>showError('注册失败:"+ErrMsg+"')</script>";
                return;
            }
        }
        catch (Exception exp)
        {
            errorHint.InnerHtml = "<script type='text/javascript'>showError('"+exp.ToString()+"！')</script>";
            return;
        }
    }
}

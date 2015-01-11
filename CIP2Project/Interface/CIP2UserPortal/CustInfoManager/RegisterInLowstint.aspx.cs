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
using log4net;

public partial class CustInfoManager_RegisterInLowstint : System.Web.UI.Page
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(CustInfoManager_RegisterInLowstint));
    private String SPTokenRequest = "";
    private Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    private String SPID = "";
    private String CustID = "";
    private String ReturnUrl = "";  // 默认值


    private String UserName;
    private String PassWord;
    private String PassWord2;


    private String checkCode;

    private String HeadFooter = "";
    private String From = "";
    private String newSPTokenRequest = "";
        
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");

        this.PanelForYiYouHeader.Visible = false;
        this.PanelForYiYouFooter.Visible = false;
        this.PanelForYiGouHeader.Visible = false;
        this.PanelForYiGouFooter.Visible = false;


        ParseSPTokenRequest();
        if ("yiyou".Equals(From))
        {
            if ("yes".Equals(HeadFooter))
            {
                this.PanelForYiYouHeader.Visible = true;
                this.PanelForYiYouFooter.Visible = true;
            }
        }
        else
        {
            if ("yes".Equals(HeadFooter))
            {
                this.PanelForYiGouHeader.Visible = true;
                this.PanelForYiGouFooter.Visible = true;
            }
        }
    }


    protected void ParseSPTokenRequest()
    {
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //解析请求参数
            Result = BeginParseSPToken(SPTokenRequest, this.Context, out SPID, out CustID, out HeadFooter, out ReturnUrl, out From, out ErrMsg);
            this.HiddenField_SPID.Value = SPID;
        }
    }

    protected void CreateSPTokenRequest()
    {
        StringBuilder sbLog = new StringBuilder();
        SPInfoManager spInfo = new SPInfoManager();
        try
        {
            sbLog.Append("spInfo.GetSPData\r\n");
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            sbLog.Append("ScoreSystemSecret");
            String _HeadFooter = "yes";
            String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

            UserToken UT = new UserToken();
            newSPTokenRequest = UT.GenerateBestAccountMainUserToken(CustID, ReturnUrl, _HeadFooter, TimeStamp, ScoreSystemSecret, out ErrMsg);
            newSPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + newSPTokenRequest);
        }
        catch (Exception ep)
        {
            sbLog.Append(ep.Message);
        }
        finally
        {
            log(sbLog.ToString());
        }

    }


    protected int BeginParseSPToken(string SourceStr, HttpContext context, out string SPID, out string CustID,
         out string HeadFooter, out string ReturnURL, out string From, out string ErrMsg)
    {
        StringBuilder strLog = new StringBuilder();

        strLog.AppendFormat("-----------解析SPTokenRequest开始:-----------\r\n");
        strLog.AppendFormat("Params: SPTokenRequest:{0}\r\n", SourceStr);
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = "";
        SPID = "";
        CustID = "";
        HeadFooter = "";
        ReturnURL = "";
        From = "";
        string TimeStamp = "";

        string Digest = "";
        try
        {
            string[] alSourceStr = SourceStr.Split('$');
            SPID = alSourceStr[0].ToString();
            strLog.AppendFormat("SPID:{0}\r\n", SPID);
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(context, "SPData");
            string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            strLog.AppendFormat("获取密钥:{0}\r\n", ScoreSystemSecret);
            string EncryptSourceStr = alSourceStr[1].ToString();
            strLog.AppendFormat("密文:{0}\r\n", EncryptSourceStr);
            string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
            strLog.AppendFormat("解密.....\r\n");
            strLog.AppendFormat("明文:{0}\r\n", RequestStr);
            string[] alRequest = RequestStr.Split('$');

            //加密顺序：URLEncoding(SPID + "$" + Base64(Encrypt(CustId + "$"  + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + From+ "$" + Digest)))
            //Digest = Base64(Encrypt(Hash(CustId + "$"+ReturnURL +"$"+ HeadFooter "$"+TimeStamp+"$"+From)))
            CustID = alRequest[0].ToString();
            strLog.AppendFormat("CustID:{0}\r\n", CustID);
            ReturnURL = alRequest[1].ToString();
            strLog.AppendFormat("ReturnURL:{0}\r\n", ReturnURL);
            HeadFooter = alRequest[2].ToString();
            strLog.AppendFormat("HeadFooter:{0}\r\n", HeadFooter);
            TimeStamp = alRequest[3].ToString();
            strLog.AppendFormat("TimeStamp:{0}\r\n", TimeStamp);
            From = alRequest[4].ToString();
            strLog.AppendFormat("From:{0}\r\n", From);
            Digest = alRequest[5].ToString();
            strLog.AppendFormat("Digest:{0}\r\n", Digest);
            //校验摘要 Digest 信息
            string NewDigest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + From, ScoreSystemSecret);
            strLog.AppendFormat("NewDigest:{0}\r\n", NewDigest);
            if (Digest != NewDigest)
            {
                Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                ErrMsg = "无效的Digest";
                return Result;
            }

            Result = 0;
        }
        catch (Exception e)
        {
            Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrMsg = e.Message;
        }
        finally
        {
            strLog.AppendFormat("-----------解析SPTokenRequest结束:-----------\r\n");
            log(strLog.ToString());
        }
        return Result;
    }



    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("RegisterInLowstint", msg);
    }




    protected void register_Click(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");

        UserName = Request.Form["userName"].ToString().Trim();

        PassWord = Request.Form["password"].ToString().Trim();

        PassWord2 = Request.Form["password2"].ToString().Trim();

        checkCode = Request.Form["checkCode"].ToString().Trim();

        String IPAddress = Request.UserHostAddress.ToString();

        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Request.Url.AbsoluteUri);
        StringBuilder sbLog = new StringBuilder();
        sbLog.AppendFormat("userName:{0}\r\n",UserName);
        sbLog.AppendFormat("password:{0}\r\n",PassWord);
        sbLog.AppendFormat("password2:{0}\r\n",PassWord2);
        sbLog.AppendFormat("checkCode:{0}\r\n",checkCode);
        try
        {
            if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(checkCode), this.Context))
            {
                //hintError提示错误验证码校验未通过
                errorHint.InnerHtml = "验证码校验未通过!";
                sbLog.AppendFormat("验证码校验未通过!");
                return;
            }

            if (!PassWord2.Equals(PassWord))
            {
                errorHint.InnerHtml = "密码不一致!";
                return;
            }

            Result = CustBasicInfo.IsExistUser(UserName);

            if (Result != 0)
            {
                errorHint.InnerHtml = "用户名已经存在!";
                return;
            }

            Result = UserRegistry.UserRegisterWebLowStint(SPID, UserName, PassWord, out CustID, out ErrMsg);

            if (Result == 0)
            {
                //记录注册来源ip地址
                CommonBizRules.WriteTraceIpLog(CustID, UserName, SPID, IPAddress,"web_zc");


                if ("35433334".Equals(SPID)) {
                    String youhuiquan_url = "http://www.114yg.cn/facadeHome.do?actions=facadeHome&method=sendCouponToRegist&wt=json&from=web&custId=" + CustID;
                    String jsonmsg = HttpMethods.HttpGet(youhuiquan_url);
                    System.Collections.Generic.Dictionary<string, string> resuzt = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(jsonmsg);
                    //{"returnCode":"00000"}
                    string youhuiquan = "";
                    resuzt.TryGetValue("returnCode", out youhuiquan);
                }


                // 重定向到欢迎页面
                sbLog.AppendFormat("注册成功:{0}\r\n",Result);
                String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(this.Context, "SPData");
                String key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
                String Digest = CryptographyUtil.GenerateAuthenticator(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg, key);
                String temp = SPID + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg + "$" + Digest, key);
                String RegistryResponseValue = HttpUtility.UrlEncode(temp);
                sbLog.Append("给用户写Cookie\r\n");
                //给用户写cookie
                UserToken UT = new UserToken();
                String RealName = UserName;
                String NickName = UserName;
                string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, "", "42", UserName, "1", key, out ErrMsg);
                string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
                sbLog.Append("创建新的SPTokenRequest\r\n");
                CreateSPTokenRequest();

                StringBuilder URL = new StringBuilder();
                String RegisterInLowstintSuccessURL = ConfigurationManager.AppSettings["RegisterInLowstintSuccessURL"].ToString(); //// 邮箱指向authenv2.aspx
                if (String.IsNullOrEmpty(RegisterInLowstintSuccessURL))
                {
                    RegisterInLowstintSuccessURL = "RegisterSuccessV2.aspx?SPID=";
                }
                URL.Append(RegisterInLowstintSuccessURL);
                //Response.Redirect(URL.ToString() + SPID + "&ReturnUrl=" + ReturnUrl + "&SPTokenRequest=" + newSPTokenRequest, false);
                //用Redirect 无法从request  的refer 中获得从哪个页面来的

                //     Server.Transfer    

                //Server.Transfer方法把执行流程从当前的ASPX文件转到同一服务器上的另一个ASPX页面。调用Server.Transfer时，当前的ASPX页面终止执行，执行流程转入另一个ASPX页面，但新的ASPX页面仍使用前一ASPX页面创建的应答流。    

                //如果用Server.Transfer方法实现页面之间的导航，浏览器中的URL不会改变，因为重定向完全在服务器端进行，浏览器根本不知道服务器已经执行了一次页面变换。    

                //默认情况下，Server.Transfer方法不会把表单数据或查询字符串从一个页面传递到另一个页面，但只要把该方法的第二个参数设置成True，就可以保留第一个页面的表单数据和查询字符串。    

                //同时，使用Server.Transfer时应注意一点：目标页面将使用原始页面创建的应答流，这导致ASP.NET的机器验证检查（Machine    Authentication    Check，MAC）认为新页面的ViewState已被篡改。因此，如果要保留原始页面的表单数据和查询字符串集合，必须把目标页面Page指令的EnableViewStateMac属性设置成False。  
                sbLog.Append("重定向:");
                //Response.Redirect(URL.ToString() + SPID + "&ReturnUrl=" + ReturnUrl + "&SPTokenRequest=" + newSPTokenRequest, true);
                Server.Transfer(URL.ToString() + SPID + "&ReturnUrl=" + ReturnUrl + "&SPTokenRequest=" + newSPTokenRequest, true);
            }
            else
            {
                sbLog.AppendFormat("注册失败:{0}\r\n",ErrMsg);
                errorHint.InnerHtml = "注册失败:"+ErrMsg;
                return;
            }

        }
        catch (Exception ex)
        {
            sbLog.Append(ex.Message);
            errorHint.InnerHtml = ex.ToString();
            return;
            //重定向到错误页面
        }
        finally
        {
            log(sbLog.ToString());
        }

    }
}

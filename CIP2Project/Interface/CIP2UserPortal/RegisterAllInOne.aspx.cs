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
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public partial class RegisterAllInOne : System.Web.UI.Page
{

    private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterAllInOne));
    private String SPID = "35000000";
    private String SPTokenRequest = "";
    private String ReturnUrl = "http://118114.cn";
    private Int32  Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
    private String PassWord = "";
    private String Mobile = "";
    private String CheckPhoneCode = "";
    private String CustID = "";
    private String BestToneAccount = "";
    private String realName = "";
    private String sex = "";
    private String certnum = "";
    private String HeadFooter = "";
    private String From = "";
    private String Email = "";
    private String UserName = "";

    protected void Page_Load(object sender, EventArgs e)
    {


        //bool IsHttps = HttpContext.Current.Request.IsSecureConnection;
        //if (!IsHttps)
        //{

        //    String AbsoluteUri = HttpContext.Current.Request.Url.AbsoluteUri;     //http://localhost/CIP2UserPortal/SSO/YiYou_Login.aspx      
        //    if (AbsoluteUri.Contains("8081"))
        //    {
        //        Response.Redirect("https://customer.besttone.com.cn:8443/RegisterAllInOne.aspx?SPTokenRequest=" + Request["SPTokenRequest"]);
        //    }
        //    else
        //    {
        //        Response.Redirect("https://customer.besttone.com.cn/UserPortal/RegisterAllInOne.aspx?SPTokenRequest=" + Request["SPTokenRequest"]);
        //    }
        //}

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
        else {
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
            Result = ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID, out HeadFooter, out ReturnUrl, out From, out ErrMsg);
            this.HiddenField_SPID.Value = SPID;
        }
    }


    protected  int ParseBesttoneAccountPageRequest(string SourceStr, HttpContext context, out string SPID, out string CustID,
         out string HeadFooter, out string ReturnURL,out string From, out string ErrMsg)
    {
        StringBuilder strLog = new StringBuilder();

        strLog.AppendFormat("-----------解析SPTokenRequest开始:-----------\r\n");
        strLog.AppendFormat("Params: SPTokenRequest:{0}\r\n",SourceStr);
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
        finally {
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
        BTUCenterInterfaceLog.CenterForBizTourLog("RegisterAllInOne", msg);
    }

    protected void SendEmail(string CustID,string Mail)
    {
        DateTime datetime = DateTime.Now;
        string ErrMsg = "";

        //给客户认证邮箱发EMAIL
        string m = CommonBizRules.EncryptEmailURl(CustID, Mail, this.Context);
        string url = "<a href='" + m + "'>" + m + "</a>";
        SetMail.InsertEmailSendMassage(CustID, "1", url, "", 1, Mail, datetime, "", "中国电信号码百事通：激活邮箱;尊敬的用户，点击此链接激活您的帐号", 0, out ErrMsg);

    }

    protected void register_Click(object sender, EventArgs e)
    {
       
        StringBuilder strLog = new StringBuilder();
        try
        {
            logger.Info("RegisterAllInOne-注册来源:"+HttpContext.Current.Request.RawUrl);
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            strLog.AppendFormat("----------------------注册事件开始:------------------\r\n");
            strLog.AppendFormat("SPID:{0}\r\n", SPID);
            strLog.AppendFormat("ReturnUrl:{0}\r\n", ReturnUrl);
            PassWord = Request.Form["password"].ToString().Trim();
            strLog.AppendFormat("PassWord:{0}\r\n", PassWord);
            Mobile = Request.Form["mobile"].ToString().Trim();
            strLog.AppendFormat("mobile:{0}\r\n", Mobile);
            CheckPhoneCode = Request.Form["checkCode"].ToString().Trim();
            strLog.AppendFormat("checkCode:{0}\r\n", CheckPhoneCode);
            Email = Request.Form["email"].ToString().Trim();
            UserName = Request.Form["userName"].ToString().Trim();

            strLog.AppendFormat("ViewState[phonestate]:{0}\r\n", ViewState["phonestate"]);
            strLog.AppendFormat("Request.Form[phonestate]:{0}\r\n", Request.Form["phonestate"]);
            if (ViewState["phonestate"] == null)
            {
                ViewState["phonestate"] = Request.Form["phonestate"].ToString();
                string a = (string)ViewState["phonestate"];
            }

            if (((string)ViewState["phonestate"]).Equals("0"))
            {
                strLog.AppendFormat("phonestate==0\r\n");
                //判断手机验证码
                string needCheckCode = "0";  //ConfigurationManager.AppSettings["needCheckCode"];
                strLog.AppendFormat("判断手机验证码\r\n");
                //strLog.AppendFormat("needCheckCode:{0}\r\n}", needCheckCode);
                if ("0".Equals(needCheckCode))
                {
                    //strLog.AppendFormat("needCheckCode==0");
                    Result = PhoneBO.SelSendSMSMassage("", Mobile, CheckPhoneCode, out ErrMsg);
                    if (Result != 0)
                    {
                        strLog.AppendFormat("手机验证码验证错误\r\n");
                        hintCode.InnerHtml = "手机验证码错误，请重新输入";  // 这里如何控制样式
                        return;
                    }
                    strLog.AppendFormat("手机验证码验证无误\r\n");
                }
                //strLog.AppendFormat("does not needCheckCode:{0}\r\n}", needCheckCode);
            }
            //strLog.AppendFormat("phonestate!=0 \r\n");
            strLog.AppendFormat("-----------------quickUserRegistryWeb  begin------------------\r\n");
            Result = UserRegistry.quickUserRegistryWebV3(SPID, PassWord, Mobile, (string)ViewState["phonestate"],UserName,Email, out CustID, out ErrMsg);
            if (Result != 0)
            {
                strLog.AppendFormat("注册失败!\r\n");
                CommonBizRules.ErrorHappenedRedircet(Result, ErrMsg, "用户注册", this.Context);
                return;
            }

            strLog.AppendFormat("注册成功!\r\n");
            //短信通知
           // string VoicePwdSPID = System.Configuration.ConfigurationManager.AppSettings["VoicePwd_SPID"];
           // int SIP1 = VoicePwdSPID.IndexOf(SPID);
            String SMS_Message = String.Empty;
           // if (SIP1 >= 0)
          //  {
                SMS_Message = "恭喜您成为号码百事通会员！请妥善保管您的密码；如需帮助请联系：4008-118114。";
                //通知短信网关
                //CommonBizRules.SendMessageV3(Mobile, SMS_Message, SPID);   //2013-11-19 注释掉
         //   }

            strLog.AppendFormat("检查邮箱是否需要发送\r\n");
            if (!String.IsNullOrEmpty(Email))
            {
                SendEmail(CustID, Email);
            }
            
            strLog.AppendFormat("写Cookie\r\n");
            String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:ta:ss");
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            String key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
            String Digest = CryptographyUtil.GenerateAuthenticator(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg, key);
            String temp = SPID + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg + "$" + Digest, key);
            String RegistryResponseValue = HttpUtility.UrlEncode(temp);

            //给用户写cookie
            UserToken UT = new UserToken();
            String RealName = Mobile;
            String NickName = Mobile;
            string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, "", "42", UserName, "1", key, out ErrMsg);
            string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
            //通知积分平台
            //strLog.AppendFormat("通知积分平台,CustID:{0}\r\n", CustID);
           
            CIP2BizRules.InsertCustInfoNotify(CustID, "2", SPID, "", "0", out ErrMsg);
            strLog.AppendFormat("写入数据库日志\r\n");
            //记登录日志
            CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, "35", "0", "", "2", Result, ErrMsg);
            strLog.AppendFormat("-----------------quickUserRegistryWeb  end------------------\r\n");

            strLog.AppendFormat("是否开户\r\n");
            strLog.AppendFormat("--------------------openBestToneAccount begin------------------------\r\n");
            String hid_openAccount = Request.Form["hid_openAccount"].ToString().Trim();
            //strLog.AppendFormat("hid_openAccount:{0}\r\n", hid_openAccount);
            if ("1".Equals(hid_openAccount))
            {
                strLog.AppendFormat("hid_openAccount==1 需要开户\r\n");
                // 开户要做的事情  需要前面注册获得的custID
                string BindedBestpayAccount = "";
                string CreateTime = "";
                strLog.AppendFormat("先查看该CustID:{0}头上是否有账户\r\n", CustID);
                int IsBesttoneAccountBindV5Result = CIP2BizRules.IsBesttoneAccountBindV5(CustID, out BindedBestpayAccount, out CreateTime, out ErrMsg);
                //strLog.AppendFormat("查看结果:IsBesttoneAccountBindV5Result:{0},BindedBestpayAccount:{1},CreateTime:{2},ErrMsg:{3}\r\n", IsBesttoneAccountBindV5Result, BindedBestpayAccount, CreateTime, ErrMsg);
                if (IsBesttoneAccountBindV5Result == 0)
                {
                    //strLog.AppendFormat("IsBesttoneAccountBindV5Result==0,该CustID:{0}头上有账户BindedBestpayAccount:{1}\r\n", CustID, BindedBestpayAccount);
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=该账户绑定关系未解除，请联系管理人员！");
                }
                strLog.AppendFormat("该CustID:{0}头上无绑定账户\r\n",CustID);
                String TransactionID = BesttoneAccountHelper.CreateTransactionID();
                AccountItem ai = new AccountItem();
                string ResponseCode = "";
                BestToneAccount = Request.Form["mobile"].ToString().Trim();
                //strLog.AppendFormat("开户账号:{0}\r\n", BestToneAccount);
                realName = Request.Form["realName"].ToString().Trim();
                //strLog.AppendFormat("realName:{0}\r\n", realName);
                certnum = Request.Form["certnum"].ToString().Trim();
                //strLog.AppendFormat("certnum:{0}\r\n", certnum);
                strLog.AppendFormat("去翼支付查看该账号是否已经存在\r\n");
                int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(BestToneAccount, out ai, out ResponseCode, out ErrMsg);
                //strLog.AppendFormat("查看结果 QueryBesttoneAccountResult:{0},ResponseCode:{1},ErrMsg:{2}\r\n", QueryBesttoneAccountResult, ResponseCode, ErrMsg);
                if (QueryBesttoneAccountResult == 0)
                {

                    if ("200010".Equals(ResponseCode))   // 未开户
                    {
                        strLog.AppendFormat("200010-未开户\r\n");
                        strLog.AppendFormat("准备开户\r\n");
                        //strLog.AppendFormat("开户前日志参数:SPID:{0},TransactionID:{1},CustID:{2},BestToneAccount:{3}\r\n", SPID, TransactionID, BestToneAccount);
                        UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);
                        //strLog.AppendFormat("日志结果 ErrMsg:{0} \r\n", ErrMsg);
                        strLog.AppendFormat("开户...\r\n");

                        Result = BesttoneAccountHelper.RegisterBesttoneAccount(BestToneAccount, realName, BestToneAccount, "", sex, "1", certnum, TransactionID, out ErrMsg);
                        //strLog.AppendFormat("开户结果:Result:{0},ErrMsg:{1},TransactionID:{2}\r\n", Result, ErrMsg, TransactionID);
                        if (Result == 0)
                        {
                            //strLog.AppendFormat("开户成功\r\n,准备去将账户{0}绑定到{1}上\r\n", BestToneAccount, CustID);
                            int BindResult = UserRegistry.CreateBesttoneAccount(SPID, CustID, BestToneAccount, out ErrMsg);
                            //strLog.AppendFormat("绑定结果:BindResult:{0},ErrMsg:{1}上\r\n", BindResult, ErrMsg);
                            if (BindResult == 0)
                            {
                                strLog.AppendFormat("开户后日志\r\n");
                                UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);
                                int retWriteBack = UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, certnum, out ErrMsg);
                                //strLog.AppendFormat("开户后日志结果:ErrMsg:{0}\r\n", ErrMsg);
                                Response.Redirect("NewOpenAccountResult.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequest) + "&CreateBesttoneAccountResult=0&RegistryResponse=" + HttpUtility.UrlEncode(RegistryResponseValue), true);
                            }
                            else
                            {
                                Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                            }
                        }
                        else
                        {
                            Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                        }

                    }
                    else
                    {
                        // 账户已存在
                        strLog.AppendFormat("账户已经存在\r\n");
                        strLog.AppendFormat("仅仅做绑定\r\n");
                        UserRegistry.OnlyBindingBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);

                        int BindResult = UserRegistry.CreateBesttoneAccount(SPID, CustID, BestToneAccount, out ErrMsg);
                        //strLog.AppendFormat("绑定结果:ErrMsg:{0}\r\n", ErrMsg);
                        if (BindResult == 0)
                        {
                            UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);
                            int retWriteBack = UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, certnum, out ErrMsg);
                            Response.Redirect("NewOpenAccountResult.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequest) + "&CreateBesttoneAccountResult=0&RegistryResponse=" + HttpUtility.UrlEncode(RegistryResponseValue),true);
                        }
                        else
                        {
                            Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                        }
                    }
                }
                else
                {
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                }
            }
            //不需要开户
            //这里应该先到一个结果页面,并将ReturnUrl传给结果页面，结果页面倒计时3秒后自动跳转到ReturnUrl，根据注册和开户跳转到不同的结果页面
            if (ReturnUrl.IndexOf("?") > 0)
            {
                Response.Redirect(ReturnUrl + "&RegistryResponse=" + RegistryResponseValue, false);
            }
            else
            {
                Response.Redirect(ReturnUrl + "?RegistryResponse=" + RegistryResponseValue, false);
            }
        }
        catch (Exception ex)
        {
            strLog.AppendFormat(ex.ToString());
            Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }
        
 

    }
}

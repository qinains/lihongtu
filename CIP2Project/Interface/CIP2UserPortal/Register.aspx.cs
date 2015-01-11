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

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public partial class Register : System.Web.UI.Page
{
    public string SPID = "";
    public string ReturnUrl = "";
    public string CustID = "";
    public string ErrMsg = "";
    public string TimeStamp = "";
    public int Result;
    public string emailstate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        err_username.InnerHtml = "";
        err_phone_code.InnerHtml = "";
        err_page_code.InnerHtml = "";
        err_email.InnerHtml = "";
        err_password.InnerHtml = "";
        Top1.Welcome = "号码百事通用户注册";
        backCount.Value = Convert.ToString((Convert.ToInt32(backCount.Value) - 1));

        if (!Page.IsPostBack)
        {

            //地区联动--start
            ProvinceInfoManager proInfo = new ProvinceInfoManager();
            object ProData = proInfo.GetProvinceData(this.Context);
            DataSet ds = (DataSet)ProData;
            this.proInfoList.DataSource = ds;
            this.proInfoList.DataTextField = "ProvinceName";
            this.proInfoList.DataValueField = "ProvinceID";
            this.proInfoList.DataBind();

            ListItem li = new ListItem("请选择", "-999");
            proInfoList.Items.Add(li);
            proInfoList.SelectedIndex = proInfoList.Items.Count - 1;

            this.areaInfoList.Items.Add(li);
            areaInfoList.SelectedIndex = areaInfoList.Items.Count - 1;

            SPID = Request["SPID"] == null ? ConstHelper.DefaultInstance.BesttoneSPID : HttpUtility.HtmlDecode(Request["SPID"]);
            ReturnUrl = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.HtmlDecode(Request["ReturnUrl"]);
            HiddenField_SPID.Value = SPID;
            HiddenField_URL.Value = ReturnUrl;

            btn_OK.Attributes.Add("onclick", "return CheckInput();");
            CertificateType.Attributes.Add("onchange", "ShowCertificateInfo()");
        }

        if (proInfoList.SelectedIndex != proInfoList.Items.Count - 1)
        {
            PhoneAreaInfoManager areaInfo = new PhoneAreaInfoManager();
            object areaData = areaInfo.GetPhoneAreaData(this.Context, proInfoList.Value);
            PhoneAreaData pad = (PhoneAreaData)areaData;
            this.areaInfoList.DataSource = pad;
            this.areaInfoList.DataTextField = "AreaName";
            this.areaInfoList.DataValueField = "AreaID";
            this.areaInfoList.DataBind();
        }
        else
        {
            areaInfoList.Value = "请选择";
        }

    }
    protected void btn_OK_Click(object sender, EventArgs e)
    {


        if (CertificateType.Value != "")
        {
            certno.Style.Value = "display:block";
            certnoL.Style.Value = "display:block";
        }
        else
        {
            certno.Style.Value = "display:none";
            certnoL.Style.Value = "display:none";
        }
        this.areaInfoList.Value = resulttxt.Value.ToString();
        SPID = Request["SPID"] == null ? ConstHelper.DefaultInstance.BesttoneSPID : HttpUtility.HtmlDecode(Request["SPID"]);
        ReturnUrl = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.HtmlDecode(Request["ReturnUrl"]);

        string username = Request.Form["username"].ToString().Trim();
        string fullname = Request.Form["fullname"].ToString().Trim();
        string password = Request.Form["password"].ToString().Trim();

        if (CommonUtility.IsEmpty(password))
        {
            err_password.InnerHtml = "密码不能为空格";
            return;
        }
        string telephone = Request.Form["telephone"].ToString().Trim();
        string phonecode = Request.Form["phone_code"].ToString().Trim();

        if (ViewState["phonestate"] == null)
        {
            ViewState["phonestate"] = Request.Form["phonestate"].ToString();
            string a = (string)ViewState["phonestate"];
        }

        string email = Request.Form["email"].ToString().Trim();
        string NickName = Request.Form["NickName"].ToString();
        string CertificateType1 = Request.Form["CertificateType"].ToString();
        string certnoS = Request.Form["certno"].ToString().Trim();
        string sex = Request.Form["sex"].ToString();
        string birthday = Request.Form["birthday"].ToString().Trim();
        string EduLevel = Request.Form["EduLevel"].ToString().Trim();
        string IncomeLevel = Request.Form["IncomeLevel"].ToString();
        string Province = stext.Value.ToString();
        string Area = resulttxt.Value.ToString();
        string ErrMsg = "";
        int Result;

        //判断用户名是否存在
        if (CustBasicInfo.IsExistUser(username) != 0)
        {
            err_username.InnerHtml = "该用户名已经存在";

            return;
        }


        if (((string)ViewState["phonestate"]).Equals("0"))
        {
            //判断手机验证码
            Result = PhoneBO.SelSendSMSMassage("", telephone, phonecode, out ErrMsg);
            if (Result != 0)
            {
                err_phone_code.InnerHtml = "手机验证码错误，请重新输入";

                return;
            }

            //判断页面验证码
            if (!CommonUtility.ValidateValidateCode(HttpUtility.HtmlDecode(Request.Form["page_code"].ToString().Trim().ToUpper()), this.Context))
            {
                err_page_code.InnerHtml = "页面验证码错误，请重新输入";

                return;
            }
        }

        //当为认证邮箱时，判断是否已经被绑定
        if (Chk_Mail.Checked && !CommonUtility.IsEmpty(email))
        {
            Result = SetMail.EmailSel("", email, SPID, out ErrMsg);
            if (Result != 0)
            {
                err_email.InnerHtml = "该邮箱已经被其他用户绑定";

                return;
            }
            emailstate = "0";
        }

        TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Result = UserRegistry.getUserRegistryWeb(SPID, username, fullname, password, telephone, (string)ViewState["phonestate"], email, emailstate,
                                                NickName, CertificateType1, certnoS, sex, birthday, EduLevel, IncomeLevel, Province, Area, out CustID, out ErrMsg);
        if (Result != 0)
        {
            //跳转至错误页面
            if (Result == -30002)
            {
                Err_certno.InnerHtml = ErrMsg;
            }else{
                CommonBizRules.ErrorHappenedRedircet(Result, ErrMsg, "用户注册", this.Context);
            }

            return;
        }

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
        string Digest = CryptographyUtil.GenerateAuthenticator(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg, key);
        string temp = SPID + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + CustID + "$" + Result + "$" + ErrMsg + "$" + Digest, key);
        string RegistryResponseValue = HttpUtility.UrlEncode(temp);

        //给用户写cookie
        UserToken UT = new UserToken();
        string key2 = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
        string UserTokenValue = UT.GenerateUserToken(CustID, fullname, username, NickName, "", "42", username, "1", key2, out ErrMsg);
        string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
        PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);

        //给客户认证邮箱发EMAIL
        string m = CommonBizRules.EncryptEmailURl(CustID, email, this.Context);
        string url = "点击完成认证:<a href='" + m + "'>" + m + "</a>";
        if (Chk_Mail.Checked && !CommonUtility.IsEmpty(email))
        {
            DateTime datetime = DateTime.Now;
            SetMail.InsertEmailSendMassage(CustID, "1", url, "", 1, email, datetime, "", "中国电信号码百事通：激活邮箱", 0, out ErrMsg);
        }

        //通知积分平台
        CIP2BizRules.InsertCustInfoNotify(CustID, "2", SPID, "", "0", out ErrMsg);
        //记登录日志
        CommonBizRules.WriteDataCustAuthenLog(SPID, CustID, "35", "0", "", "2", Result, ErrMsg);

        //跳转至成功页面
        if (ReturnUrl.IndexOf("?") > 0)
        {
            CommonBizRules.SuccessRedirect(ReturnUrl + "&RegistryResponse=" + RegistryResponseValue, "成功注册", this.Context);
        }
        else
        {
            CommonBizRules.SuccessRedirect(ReturnUrl + "?RegistryResponse=" + RegistryResponseValue, "成功注册", this.Context);
        }

    }

}

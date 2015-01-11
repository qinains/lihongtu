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
using System.Text;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
public partial class SetMobile3 : System.Web.UI.Page
{

    public String AuthenPhone = String.Empty;
    public String SmsAuthenCode = String.Empty;
    public String SPID = String.Empty;
    public String CustID = String.Empty;
    public String ReturnUrl = String.Empty;
    public String newSPTokenRequest;
    public String SPTokenRequest;
    public String HeadFooter;
    private String From = "";

    public Int32 Result;
    public String ErrMsg=String.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            TokenValidate.IsRedircet = true;
            TokenValidate.Validate();
            //CustID = TokenValidate.CustID;

            SPID = Request["SPID"] == null ? String.Empty : Request["SPID"].ToString();
            AuthenPhone = Request["AuthenMobile"];
            SmsAuthenCode = Request["AuthenCode"];
            ParseSPTokenRequest();
            //CustID = TokenValidate.CustID;
            CreateSPTokenRequest();

        }
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
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID, out HeadFooter, out ReturnUrl, out ErrMsg);
            if (!String.IsNullOrEmpty(CustID))
            {
                this.custidtxt.Value = CustID;
            }
            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                this.returnurltxt.Value = ReturnUrl;
            }
            if (!String.IsNullOrEmpty(SPID))
            {
                this.spidtxt.Value = SPID;
            }
            strLog.AppendFormat("SPID:{0};CustID:{1};HeadFooter:{2};ReturnUrl:{3};ErrMsg:{4}",SPID,CustID,HeadFooter,ReturnUrl,ErrMsg);
        }
        log(strLog.ToString());
    }

    protected void CreateSPTokenRequest()
    {

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

        String _HeadFooter = "yes";
        String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

        UserToken UT = new UserToken();
        newSPTokenRequest = UT.GenerateBestAccountMainUserToken(CustID, ReturnUrl, _HeadFooter, TimeStamp, ScoreSystemSecret, out ErrMsg);
        newSPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + newSPTokenRequest);
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("SetMobile3", msg);
    }


    protected void MobileAuthenButton_Click(object sender, ImageClickEventArgs e)
    {
        AuthenPhone = Request["AuthenMobile"];
        SmsAuthenCode = Request["AuthenCode"];
        StringBuilder sbLog = new StringBuilder();
        sbLog.AppendFormat("AuthenPhone:{0}\r\n",AuthenPhone);
        sbLog.AppendFormat("AuthenCode:{0}\r\n", SmsAuthenCode);
        CustID = this.custidtxt.Value;
        ReturnUrl = this.returnurltxt.Value;
        SPID = this.spidtxt.Value;
        sbLog.AppendFormat("SPID:{0}\r\n", SPID);
        sbLog.AppendFormat("CustID:{0}\r\n",CustID);
        sbLog.AppendFormat("ReturnUrl:{0}\r\n",ReturnUrl);
        ErrMsg = String.Empty;

        try
        {
            DateTime starttime = DateTime.Now;
            int k = PhoneBO.PhoneSel(CustID, AuthenPhone, out ErrMsg);
            DateTime endtime = DateTime.Now;
            System.TimeSpan deltatime = endtime.Subtract(starttime);
            sbLog.AppendFormat("判断手机是否可以作为认证手机返回结果k={0},ErrMsg:{1}\r\n", k,ErrMsg);
            sbLog.AppendFormat("判断手机是否可以作为认证手机消耗时间:{0}\r\n", deltatime.Milliseconds);
            if (k == 0)   //代表该手机没有被认证过
            {
                sbLog.AppendFormat("{0}:该手机没有被用过\r\n",AuthenPhone);
                ErrMsg = String.Empty;
                starttime = DateTime.Now;
                int w = PhoneBO.SelSendSMSMassageOnRegister(CustID, AuthenPhone, SmsAuthenCode, out ErrMsg);   // 校验手机验证码   这里要注意的是 通过短信上行注册认证手机获取手机验证码的时候，没有将custid和短信验证码绑定，因此校验验证码的时候不能关联custid条件，这里的参数CustID是从cookie中获得的
                endtime = DateTime.Now;
                deltatime = endtime.Subtract(starttime);
                sbLog.AppendFormat("校验手机验证码返回结果w={0},ErrMsg:{1}\r\n",w,ErrMsg);
                sbLog.AppendFormat("校验手机验证码消耗时间:{0}\r\n",deltatime.Milliseconds);
                if (w == 0)
                {
                    sbLog.AppendFormat("校验手机验证码通过:{0}\r\n",SmsAuthenCode);
                    ErrMsg = String.Empty;
                    starttime = DateTime.Now;
                    int y = PhoneBO.PhoneSetV2(SPID, CustID, AuthenPhone, "2", "2", out ErrMsg);
                    endtime = DateTime.Now;
                    deltatime = endtime.Subtract(starttime);
                    sbLog.AppendFormat("设置认证手机结果y={0},ErrMsg={1}\r\n",y,ErrMsg);
                    sbLog.AppendFormat("设置认证手机消耗时间:{0}\r\n",deltatime.Milliseconds);
                    Response.Redirect(ReturnUrl, true);
                    //CommonBizRules.SuccessRedirect(ReturnUrl, "您已成功设置认证手机", HttpContext.Current);
                }
                else
                {
                    Result = w;
                    this.AuthenCodeError.InnerText = "校验手机验证码失败:" + ErrMsg;
                    sbLog.AppendFormat("校验手机验证码失败:{0}|{1}\r\n",w,ErrMsg);
                    return;
                }

            }
            else
            {
                Result = k;
                this.AuthenPhoneError.InnerText = "设置认证手机失败:" + ErrMsg;
                sbLog.AppendFormat("设置认证手机失败:{0}|{1}\r\n",k,ErrMsg);
                return;
            }
        }
        catch (Exception ept)
        {
            sbLog.AppendFormat("异常:{0}",ept.Message);
        }
        finally
        {
            log(sbLog.ToString());
        }



    }
}

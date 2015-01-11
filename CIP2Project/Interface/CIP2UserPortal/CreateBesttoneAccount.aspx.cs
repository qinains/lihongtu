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
public partial class CreateBesttoneAccount : System.Web.UI.Page
{
    string ReturnUrl = "";
    string phoneNum = "";
    string CustID = "";
    string HeadFooter = "";
    string stamp = "";
    string checkCode = "";
    string contactMail = "";
    string sex = "";
  
    string certnum = "";

    public PhoneRecord[] phones =null;
    string realName = "";

    public string _hidphone = "";

    string TransactionID = DateTime.Now.ToString("yyyyMMddHHmmss");
    public string newSPTokenRequest = "";
    public string SPTokenRequest = "";
    string SPID = "35000000";


    string ErrMsg = "";
    int Result = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("OpenBestToneAccount:Page_Load");
        ParseSPTokenRequest();


        //if (!IsPostBack)
        //{

            if (Result == 0)
            {
                
                int QueryResult = 0;
                strLog.AppendFormat(String.Format("CustID:{0},SPID{1},HeadFooter:{2},ReturnUrl:{3}", CustID, SPID, HeadFooter,ReturnUrl));
                this.myCustID.Value = CustID;
                this.myReturnUrl.Value = ReturnUrl;
                this.HiddenField_SPID.Value = SPID;
                if ("yes".Equals(HeadFooter))
                {
                    this.header.Visible = true;
                    this.footer.Visible = true;
                }
                else
                {
                    this.header.Visible = false;
                    this.footer.Visible = false;
                }

                phones = CustBasicInfo.getPhoneRecord(CustID, out QueryResult, out ErrMsg);     //默认行为： 取出登录用户的认证手机作为开户账户号,可能为多个，需用户选择
                if (QueryResult == 0 && phones != null && phones.Length > 0)
                {
                    strLog.AppendFormat("getPhoneRecord成功!");
                    phoneNum = phones[0].Phone;
                    if(!IsPostBack)
                    {
                        this.mobile.Text = phoneNum;
                    }
                    this.hidCheckMobile.Value = phoneNum;
                    //this.contactTel.Text = phoneNum;
                    strLog.AppendFormat(String.Format("phoneNum:{0}", phoneNum));
                }
                else
                {
                    strLog.AppendFormat(String.Format("ErrMsg:{0}", ErrMsg));
                    if(!IsPostBack)
                    {
                        this.mobile.Text = "";
                    }
                    
                }

            }
            else
            {
                Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);

            }

        //}

        log(strLog.ToString());
        
    }
    protected void CreateSPTokenRequest()
    {

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

        //string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
        String _HeadFooter = "yes";
        String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

        UserToken UT = new UserToken();
        newSPTokenRequest = UT.GenerateBestAccountMainUserToken(CustID, ReturnUrl, _HeadFooter, TimeStamp, ScoreSystemSecret, out ErrMsg);
        newSPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + newSPTokenRequest);
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
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID,
            out HeadFooter, out ReturnUrl, out ErrMsg);
            //日志
            strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},CustID:{3},HeadFooter:{4},stamp:{5},ReturnUrl:{6}", Result, ErrMsg, SPID, CustID, HeadFooter, stamp, ReturnUrl));

           

        }
        log(strLog.ToString());
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("createBesttoneAccount", msg);
    }


    protected void register_Click(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            ParseSPTokenRequest();
            //CreateSPTokenRequest();

            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            realName = Request["realName"];
            //contactMail = Request["contactMail"];
            sex = Request["sex"];
            certnum = Request["certnum"];
            checkCode = Request["checkCode"];
            strLog.AppendFormat("开户页面手机验证码:{0}", checkCode);

            //判断手机验证码
            if (checkCode != null && !"".Equals(checkCode))
            {
                Result = PhoneBO.SelSendSMSMassage("", this.mobile.Text, checkCode, out ErrMsg);
                if (Result != 0)
                {
                    hintCode.InnerHtml = "手机验证码错误，请重新输入";  // 这里如何控制样式
                    //this.hidCheckMobile.Value = this.mobile.Text;
                    return;
                }
            }

            string BindedBestpayAccount = "";
            string CreateTime = "";
            int IsBesttoneAccountBindV5Result = CIP2BizRules.IsBesttoneAccountBindV5(this.myCustID.Value, out BindedBestpayAccount, out CreateTime, out ErrMsg);
            if (IsBesttoneAccountBindV5Result == 0)
            {
                Response.Redirect("ErrorInfo.aspx?ErrorInfo=该账户绑定关系未解除，请联系管理人员！");
            }
            
            TransactionID = BesttoneAccountHelper.CreateTransactionID();
            AccountItem ai = new AccountItem();
            string ResponseCode = "";
            int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(this.mobile.Text, out ai, out ResponseCode, out ErrMsg);
            strLog.AppendFormat("查询账户信息返回:{0},{1},{2}", QueryBesttoneAccountResult, ErrMsg, this.mobile.Text);
            if (QueryBesttoneAccountResult == 0)
            {
                if ("200010".Equals(ResponseCode))  // 200010 -> 客户不存在
                {

                    //todo 发起开户请求日志
                    UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, this.myCustID.Value, this.mobile.Text, out  ErrMsg);
                    strLog.AppendFormat("BeforeCreateBesttoneAccount:ErrMsg:{0}", ErrMsg);
                    //String realName,String contactTel,String sex,String certtype,String certnum, 
                    Result = BesttoneAccountHelper.RegisterBesttoneAccount(this.mobile.Text, realName, this.mobile.Text, "", sex, "1", certnum, TransactionID, out ErrMsg);
                    if (Result == 0)
                    {


                        strLog.AppendFormat("开户结果:{0},{1},{2}", Result, ErrMsg, this.myCustID.Value);
                        // todo 建立绑定关系，插入绑定关系表
                        int ret = 0;

                        ret = UserRegistry.CreateBesttoneAccount(SPID, this.myCustID.Value, this.mobile.Text, out ErrMsg);
                        strLog.AppendFormat("CreateBesttoneAccount:ErrMsg:{0}", ErrMsg);
                        if (ret == 0)
                        {

                            //todo 开户完成 建立绑定关系 日志
                            UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, this.myCustID.Value, this.mobile.Text, out  ErrMsg);
                            strLog.AppendFormat("AfterCreateBesttoneAccount:ErrMsg:{0}", ErrMsg);
                            strLog.AppendFormat("绑定结果:ret:{0},ErrMsg:{1},ReturnUrl:{2}", ret, ErrMsg, ReturnUrl);

                            int retWriteBack = UserRegistry.WriteBackBestToneAccountToCustInfo(SPID,this.myCustID.Value,realName,certnum,out ErrMsg);
                            strLog.AppendFormat("回写客户信息结果:retWriteBack:{0},ErrMsg:{1}",retWriteBack,ErrMsg);

                            strLog.AppendFormat("SPTokenRequest={0}", SPTokenRequest);
                            strLog.AppendFormat("Redirect to Url:{0}", "OpenAccountResult.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequest) + "&CreateBesttoneAccountResult=0");
                            Response.Redirect("OpenAccountResult.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequest) + "&CreateBesttoneAccountResult=0");
                       
                        
                        }
                        else
                        {
                            strLog.AppendFormat("绑定结果:{0},{1}", ret, ErrMsg);
                            Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                        }
                    }
                    else
                    {
                        strLog.AppendFormat("开户结果:{0},{1}", Result, ErrMsg);
                        Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);

                    }


                }
                else  // 账户已存在
                {
                    // todo 建立绑定关系，插入绑定关系表
                    UserRegistry.OnlyBindingBesttoneAccount(SPID, TransactionID, this.myCustID.Value, this.mobile.Text, out  ErrMsg);
                    strLog.AppendFormat("OnlyBindingBesttoneAccount:ErrMsg:{0}", ErrMsg);
                    int ret = 0;

                    ret = UserRegistry.CreateBesttoneAccount(SPID, this.myCustID.Value, this.mobile.Text, out ErrMsg);
                    if (ret == 0)
                    {
                        int retWriteBack = UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, this.myCustID.Value, realName, certnum, out ErrMsg);
                        strLog.AppendFormat("回写客户信息结果:retWriteBack:{0},ErrMsg:{1}", retWriteBack, ErrMsg);
                        strLog.AppendFormat("绑定结果:ret:{0},ErrMsg:{1},ReturnUrl:{2}", ret, ErrMsg, ReturnUrl);
                        strLog.AppendFormat(String.Format("SPTokenRequest={0}", SPTokenRequest));
                        Response.Redirect("OpenAccountResult.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequest) + "&CreateBesttoneAccountResult=0");
                        //Response.Redirect(this.myReturnUrl.Value);
                    }
                    else
                    {
                        strLog.AppendFormat("绑定结果:{0},{1}", ret, ErrMsg);
                        Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                    }
                }
            }
            else
            {
                strLog.AppendFormat("查询账户信息返回:{0},{1},{2}", QueryBesttoneAccountResult, ErrMsg, this.mobile.Text);
                Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
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


  

}

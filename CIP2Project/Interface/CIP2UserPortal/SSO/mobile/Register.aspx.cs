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
public partial class SSO_mobile_Register : System.Web.UI.Page
{
    public String Device = "android";
    private String SPID = "35000000";
    private String SPTokenRequest = "";
    private String ReturnUrl = "http://118114.cn";
    private Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
    private String PassWord ="";
    private String Mobile = "";
    private String CheckPhoneCode = "";
    private String CustID = "";
    private String sex = "";
    private String realName = "";
    private String certnum = "";
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
                this.errorHint.InnerText = "SPTokenRequest参数缺失";
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

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("mobile-register", msg);
    }


    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        String Url = "";
        try
        {
            PassWord = Request.Form["password"].ToString().Trim();
            Mobile = Request.Form["mobile"].ToString().Trim();
            CheckPhoneCode = Request.Form["checkCode"].ToString().Trim();
            strLog.AppendFormat("接收到password:{0}，Mobile:{1},CheckPhoneCode:{2}\r\n", PassWord, Mobile, CheckPhoneCode);


            string UserName = "";
            string Email = "";
 
            if (ViewState["phonestate"] == null)
            {
                strLog.AppendFormat("phonestate=null\r\n");
                ViewState["phonestate"] = Request.Form["phonestate"].ToString();
                string a = (string)ViewState["phonestate"];
            }
            if (((string)ViewState["phonestate"]).Equals("0"))
            {
                Result = PhoneBO.SelSendSMSMassage("", Mobile, CheckPhoneCode, out ErrMsg);
                if (Result != 0)
                {
                    strLog.AppendFormat("手机验证码校验未通过!\r\n");
                    return;
                }
            }

            strLog.AppendFormat("手机验证码校验通过!\r\n");
            Result = UserRegistry.quickUserRegistryWebV4(SPID, PassWord, Mobile, (string)ViewState["phonestate"], UserName, Email,Device, out CustID, out ErrMsg);
            if (Result != 0)
            {
                strLog.AppendFormat("注册失败!\r\n");
                return;
            }
            strLog.AppendFormat("注册成功!CustID:{0}\r\n", CustID);
            String hid_openAccount = Request.Form["hid_openAccount"].ToString().Trim();
            if ("1".Equals(hid_openAccount))
            {
                strLog.AppendFormat("开户过程\r\n");
                string BindedBestpayAccount = "";
                string CreateTime = "";

                int IsBesttoneAccountBindV5Result = CIP2BizRules.IsBesttoneAccountBindV5(CustID, out BindedBestpayAccount, out CreateTime, out ErrMsg);
                if (IsBesttoneAccountBindV5Result == 0)
                {
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=该账户绑定关系未解除，请联系管理人员！");
                }
                String TransactionID = BesttoneAccountHelper.CreateTransactionID();
                AccountItem ai = new AccountItem();
                string ResponseCode = "";
                string BestToneAccount = Request.Form["mobile"].ToString().Trim();
                realName = Request.Form["realName"].ToString().Trim();
                certnum = Request.Form["certnum"].ToString().Trim();
                int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(BestToneAccount, out ai, out ResponseCode, out ErrMsg);
                if (QueryBesttoneAccountResult == 0)
                {

                    if ("200010".Equals(ResponseCode))   // 未开户
                    {
                        strLog.AppendFormat("该号码未开过户:\r\n");
                        UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);
                        Result = BesttoneAccountHelper.RegisterBesttoneAccount(BestToneAccount, realName, BestToneAccount, "", sex, "1", certnum, TransactionID, out ErrMsg);
                        if (Result == 0)
                        {
                            strLog.AppendFormat("开户成功:\r\n");
                            int BindResult = UserRegistry.CreateBesttoneAccount(SPID, CustID, BestToneAccount, out ErrMsg);
                            if (BindResult == 0)
                            {
                                UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);
                                int retWriteBack = UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, certnum, out ErrMsg);
                                //
                                //Response.Redirect("NewOpenAccountResult.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequest) + "&CreateBesttoneAccountResult=0&RegistryResponse=" + HttpUtility.UrlEncode(RegistryResponseValue), true);

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
                        UserRegistry.OnlyBindingBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);
                        int BindResult = UserRegistry.CreateBesttoneAccount(SPID, CustID, BestToneAccount, out ErrMsg);
                        if (BindResult == 0)
                        {
                            UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, BestToneAccount, out  ErrMsg);
                            int retWriteBack = UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, certnum, out ErrMsg);
                            //Response.Redirect("NewOpenAccountResult.aspx?SPTokenRequest=" + HttpUtility.UrlEncode(SPTokenRequest) + "&CreateBesttoneAccountResult=0&RegistryResponse=" + HttpUtility.UrlEncode(RegistryResponseValue), true);
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

            if (ReturnUrl.IndexOf("?") > 0)
            {
                Url = ReturnUrl + "&CustID=" + CustID + "&welcomeName=" + Mobile;

            }
            else
            {
                Url = ReturnUrl + "?CustID=" + CustID + "&welcomeName=" + Mobile;
            }

            Response.Redirect(Url, true);
        }
        catch (Exception ex)
        {
            strLog.AppendFormat(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }

    }
}

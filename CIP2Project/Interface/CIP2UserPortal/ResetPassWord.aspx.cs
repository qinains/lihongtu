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
public partial class ResetPassWord : System.Web.UI.Page
{

    public int IsBesttoneAccountBindV5Result = -1;
    public string BesttoneAccount = "";
    public string ReturnUrl = "";

    string CustID = "";
    string HeadFooter = "";
    string stamp = "";

    string checkCode = "";

    public string SPTokenRequest = "";
    string SPID = "35000000";


    string ErrMsg = "";
    int Result = 0;
    public string success = "1";
    public string sendmsg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            strLog.AppendFormat("ResetPayPassWord").Append("\n");

            ParseSPTokenRequest();
            if (!IsPostBack)
            {


                if (Result == 0)
                {
                    //int QueryResult = 0;
                    strLog.AppendFormat(String.Format("CustID:{0},SPID:{1},HeadFooter:{2}", CustID, SPID, HeadFooter)).Append("\n");
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

                    

                    //PhoneRecord[] phones = CustBasicInfo.getPhoneRecord(CustID, out QueryResult, out ErrMsg);
                    //if (QueryResult == 0 && phones != null && phones.Length > 0) {
                    //    strLog.AppendFormat("getPhoneRecord成功!");
                    //    BesttoneAccount = phones[0].Phone;
                        this.mobile.Value = BesttoneAccount;
                        string BindedBestpayAccount = "";
                        string CreateTime = "";
                        IsBesttoneAccountBindV5Result = CIP2BizRules.IsBesttoneAccountBindV5(CustID, out BindedBestpayAccount, out CreateTime, out ErrMsg);
                        if (IsBesttoneAccountBindV5Result == 0)
                        {
                            if (!String.IsNullOrEmpty(BindedBestpayAccount))
                            {
                                BesttoneAccount = BindedBestpayAccount;
                                this.mobile.Value = BindedBestpayAccount;
                            }
                        }


                        strLog.AppendFormat(String.Format("phoneNum:{0}", BesttoneAccount));
                    //}
                    //else
                    //{
                    //    strLog.AppendFormat(String.Format("ErrMsg:{0}", ErrMsg));
                    //    this.mobile.Value = "";
                    //}

                }
                else
                {
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);

                }
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            //checkCode = Request["checkCode"];
            BesttoneAccount = Request["mobile"];
            //strLog.AppendFormat(String.Format("重置密码页面验证码:{0}", checkCode));
            //判断手机验证码
            //if (checkCode != null && !"".Equals(checkCode))
            //{
            //    Result = PhoneBO.SelSendSMSMassage("", BesttoneAccount, checkCode, out ErrMsg);
            //    if (Result != 0)
            //    {
            //        hintCode.InnerHtml = "手机验证码错误，请重新输入";  // 这里如何控制样式
            //        return;
            //    }
            //}
            //else
            //{
            //    hintCode.InnerHtml = "手机验证不能为空，请重新输入";  // 这里如何控制样式
            //    return;
            //}

            Linkage.BestTone.Interface.Rule.CustInfo custinfo = new Linkage.BestTone.Interface.Rule.CustInfo();
            Result = BesttoneAccountHelper.QueryCustInfo(BesttoneAccount, out custinfo, out ErrMsg);
            if (Result == 0)
            {
                if (custinfo != null)
                {

                    int ret = BesttoneAccountHelper.ResetBesttoneAccountPayPassword(BesttoneAccount, custinfo.IdType, custinfo.IdNo, custinfo.CustomerName, out ErrMsg);
                    if (ret == 0)
                    {
                        //提示密码重置成功
                        success = "0";
                        sendmsg = "支付密码已发送至" + BesttoneAccount;
                        //Response.Redirect(ReturnUrl);
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
        catch (System.Exception ex)
        {
            log(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
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
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID,
            out HeadFooter, out ReturnUrl, out ErrMsg);
            this.HiddenField_SPID.Value = SPID;
            //日志
            strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},CustID:{3},HeadFooter:{4},stamp:{5},ReturnUrl:{6}", Result, ErrMsg, SPID, CustID, HeadFooter, stamp, ReturnUrl));

        }
        log(strLog.ToString());
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("ResetPayPassWord", msg);
    }

}

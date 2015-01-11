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
public partial class ChangePayPWD2 : System.Web.UI.Page
{
    string BesttoneAccount = "";
    public string ReturnUrl = "";
    string CustID = "";
    string HeadFooter = "";
    string stamp = "";


    public string SPTokenRequest = "";
    string SPID = "35000000";


    string oldPassWord = "";
    string newPassWord = "";
    string confirmPassWord = "";
    string ErrMsg = "";
    int Result = 0;
    public string success = "1";
    public int IsBesttoneAccountBindV5Result = -1;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        ParseSPTokenRequest();
        if (!IsPostBack)
        {
            strLog.AppendFormat("ChangePayPassword2-Page_Load");
            if (Result == 0)
            {

                strLog.AppendFormat(String.Format("CustID:{0},SPID{1},HeadFooter{2}", CustID, SPID, HeadFooter));
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
               
                string BindedBestpayAccount = "";
                string CreateTime = "";
                IsBesttoneAccountBindV5Result = CIP2BizRules.IsBesttoneAccountBindV5(CustID, out BindedBestpayAccount, out CreateTime, out ErrMsg);
                if (IsBesttoneAccountBindV5Result == 0)
                {
                    if (!String.IsNullOrEmpty(BindedBestpayAccount))
                    {
                        BesttoneAccount = BindedBestpayAccount;

                    }
                }

                strLog.AppendFormat(String.Format("phoneNum:{0}", BesttoneAccount));
                oldPassWord = Request.Form["oldPassWord"];
                newPassWord = Request.Form["newPassWord"];
                confirmPassWord = Request.Form["confirmPassWord"];

       
            }
            else
            {
                Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
            }
            log(strLog.ToString());
        }
    }


    protected void Modify_Click(object sender, EventArgs e)
    {
        ///////////////////////////////////////////
        StringBuilder strLog = new StringBuilder();
        try
        {
            String oldPassWord = Request.Form["oldPassWord"];
            String newPassWord = Request.Form["newPassWord"];
            String confirmPassWord = Request.Form["confirmPassWord"];
            string BindedBestpayAccount = "";
            string CreateTime = "";
            IsBesttoneAccountBindV5Result = CIP2BizRules.IsBesttoneAccountBindV5(CustID, out BindedBestpayAccount, out CreateTime, out ErrMsg);
            if (IsBesttoneAccountBindV5Result == 0)
            {
                if (!String.IsNullOrEmpty(BindedBestpayAccount))
                {
                    BesttoneAccount = BindedBestpayAccount;

                }
            }

            strLog.AppendFormat("【开始修改密码,事件：{0}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            strLog.AppendFormat("参数：oldPassWord:{0},newPassWord:{1},confirmPassWord:{2},BesttonePayAccount:{3}", oldPassWord, newPassWord, confirmPassWord, BesttoneAccount);

            BestPayEncryptService bpes = new BestPayEncryptService();
            string e_oldPassWord = "";
            string e_newPassWord = "";
            string e_confirmPassWord = "";

            AccountItem ai = new AccountItem();
            String ResCode = "";
            int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(BesttoneAccount, out ai, out ResCode, out ErrMsg);
            if (QueryBesttoneAccountResult == 0)
            {
                if (ai != null)
                {
                    e_oldPassWord = bpes.encryptNoKey(oldPassWord, ai.AccountNo);
                    e_newPassWord = bpes.encryptNoKey(newPassWord, ai.AccountNo);
                    e_confirmPassWord = bpes.encryptNoKey(confirmPassWord, ai.AccountNo);

                    strLog.AppendFormat("e_oldPassWord{0},e_newPassWord{1},e_confirmPassWord{2}", e_oldPassWord, e_newPassWord, e_confirmPassWord);

                    int ModifyBestPayPasswordResult = BesttoneAccountHelper.ModifyBestPayPassword(ai.AccountNo, e_oldPassWord, e_newPassWord, e_confirmPassWord, out ErrMsg);

                    if (ModifyBestPayPasswordResult == 0)
                    {
                        success = "0";
                        
                    }
                    else
                    {
                        strLog.Append(",失败3");
                        Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
                    }
                }
                else
                {
                    strLog.Append(",失败2");
                    Response.Redirect("ErrorInfo.aspx?ErrorInfo=账户信息未获取");
                }
            }
            else
            {
                strLog.Append(",失败1");
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
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID, out HeadFooter, out ReturnUrl, out ErrMsg);
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
        BTUCenterInterfaceLog.CenterForBizTourLog("ChangePayPassword2", msg);
    }
}

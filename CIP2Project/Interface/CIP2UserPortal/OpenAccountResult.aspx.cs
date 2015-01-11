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
public partial class OpenAccountResult : System.Web.UI.Page
{
    string BesttoneAccount = "";
    string ReturnUrl = "";
    string CustID = "";
    string HeadFooter = "";
    string stamp = "";


    string SPTokenRequest = "";
    string SPID = "35000000";
    string ErrMsg = "";
    int Result = 0;

    public string CreateBesttoneAccountResult = "-1";

    protected void Page_Load(object sender, EventArgs e)
    {

        StringBuilder strLog = new StringBuilder();

        ParseSPTokenRequest();

        if (Result == 0)
        {
            strLog.AppendFormat(String.Format("CustID:{0},SPID:{1},HeadFooter:{2},ReturnUrl:{3}", CustID, SPID, HeadFooter, ReturnUrl));
            log(strLog.ToString());



            if (CommonUtility.IsParameterExist("CreateBesttoneAccountResult", this.Page))
            {
                CreateBesttoneAccountResult = Request["CreateBesttoneAccountResult"];
                if ("0".Equals(CreateBesttoneAccountResult))
                {
                    this.successpage.Visible = true;
                    this.failedpage.Visible = false;
                }
                else
                {
                    this.successpage.Visible = false;
                    this.failedpage.Visible = true;
                }
                strLog.AppendFormat("OpenAccountResult-wait3-redirect-to:{0}",ReturnUrl);
                log(strLog.ToString());
                this.txtHid.Value = ReturnUrl;
            }
            else
            {
                Response.Redirect("ErrorInfo.aspx?ErrorInfo=丢失CreateBesttoneAccountResult参数");
            }

        }
        else
        {

            Response.Redirect("ErrorInfo.aspx?ErrorInfo=" + ErrMsg);
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
            //日志
            strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},CustID:{3},HeadFooter:{4},stamp:{5},ReturnUrl:{6}", Result, ErrMsg, SPID, CustID, HeadFooter, stamp, ReturnUrl));



        }
        log(strLog.ToString());
    }
    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("OpenAccountResult", msg);
    }

}

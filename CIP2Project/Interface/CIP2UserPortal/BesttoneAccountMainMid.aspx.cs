using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

public partial class BesttoneAccountMainMid : System.Web.UI.Page
{


    string ReturnUrl = "";
    string CustID = "";
    string HeadFooter = "";
    string stamp = "";
    string phoneNum = "";
    string BesttoneAccount = ""; 
    public string newSPTokenRequest = "";
    public string SPTokenRequest = "";
    string SPID = "35000000";

    public IList<TxnItem> txnItemList = new List<TxnItem>();
    public String BesttoneAccountBalance = "";


    public BesttoneAccountStatus BesttoneAccountStatus;

    string ErrMsg = "";
    int Result = 0;

    public int UnBindOrUnRegister = -1;  // 未绑定或者为开通账户 当该值为0时代表该用户为绑定或者未开通  

    protected void Page_Load(object sender, EventArgs e)
    {
        ParseSPTokenRequest();
        if(Result ==0)
        {   
            int QueryResult =  CIP2BizRules.IsBesttoneAccountBindV3(CustID,out BesttoneAccount, out ErrMsg);
            if (QueryResult == BesttoneAccountStatus.UnBind_OR_UnRegister)
            {
                CreateNewSPTokenRequest();
                // 转到 绑定和开通混合页面
                Response.Redirect("BindingAndRegister.aspx?SPTokenRequest="+newSPTokenRequest);

            } 
            else
            {
                // 转到 账户中心页面
                Response.Redirect("BesttoneAccountMain.aspx?SPTokenReuest="+SPTokenRequest);
            }

        }

    }

    protected void ParseSPTokenRequest()
    {
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //日志
            log("【SPTokenRequest参数】:" + SPTokenRequest);
            //解析请求参数
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID,
            out HeadFooter, out ReturnUrl, out ErrMsg);
            //日志
            log(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},CustID:{3},HeadFooter:{4},stamp:{5},ReturnUrl:{6}", Result, ErrMsg, SPID, CustID, HeadFooter, stamp, ReturnUrl));



        }
    }


    protected void CreateNewSPTokenRequest()
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

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneAccountMainMid", msg);
    }

}

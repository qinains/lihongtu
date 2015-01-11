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
public partial class CustInfoManager_M_LowSintRegisterMSuccess : System.Web.UI.Page
{

    public String SPID;
    public String ReturnUrl;
    public String newSPTokenRequest;
    public String CustID;
    public Int32 Result;
    public String ErrMsg;
    public String SPTokenRequest;

    protected void Page_Load(object sender, EventArgs e)
    {
        ParseSPTokenRequest();
        CreateSPTokenRequest();
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
            Result = SSOClass.ParseBesttoneAccountPageRequestM(SPTokenRequest, this.Context, out SPID, out CustID, out ReturnUrl, out ErrMsg);

            this.HiddenField_CUSTID.Value = CustID;
            this.HiddenField_SPID.Value = SPID;
            this.HiddenField_URL.Value = ReturnUrl;

        }

    }

    protected void CreateSPTokenRequest()
    {

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

        String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

        UserToken UT = new UserToken();
        newSPTokenRequest = UT.GenerateBestAccountMainUserTokenM(CustID, ReturnUrl, TimeStamp, ScoreSystemSecret, out ErrMsg);
        newSPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + newSPTokenRequest);
    }



}

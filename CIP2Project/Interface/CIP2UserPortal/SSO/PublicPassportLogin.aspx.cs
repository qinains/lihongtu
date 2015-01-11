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
using BTUCenter.Proxy;
using System.Text.RegularExpressions;
using System.Text;

public partial class SSO_PublicPassportLogin : System.Web.UI.Page
{
    #region 天翼账号登录变量
    private String UDBLoginURL = String.Empty;
    private String UDBReturnURL = String.Empty;
    private String UdbSrcSsDeviceNo = String.Empty;
    private String UdbKey = String.Empty;

    private string login189Url;
    public string Login189Url
    {
        get { return login189Url; }
        set
        {
            login189Url = value;
        }
    }

    private string passportLoginRequestValue;
    public string PassportLoginRequestValue
    {
        get { return passportLoginRequestValue; }
        set
        {
            passportLoginRequestValue = value;
        }
    }
    #endregion

    //url参数=SPID+ProvinceID+SourceType+ReturnURL

    #region 单点登录全局变量
    string SPTokenRequest = "";
    string SPID = "35000000";
    string ReturnURL = "http://www.118114.cn/";
    string UAProvinceID = "";
    string SourceType = "";

    string ErrMsg = "";
    int Result = 0;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");
        //判断并解析SPTokenRequest参数
        ParseSPTokenRequest();
        //生成udb请求参数,注意CreateUdbPassportLoginRequest()方法必须放在ParseSPTokenRequst()后面
        CreateUdbPassportLoginRequest();

        Response.Redirect("YiYou_Login.aspx?SPTokenRequest=" + SPTokenRequest + "&login189Url=" +HttpUtility.UrlEncode(login189Url));
    }


    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
       
            if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
            {
                SPTokenRequest = Request["SPTokenRequest"];
                //日志
                strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
                //解析请求参数
                Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnURL, out ErrMsg);
                //日志
                strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnURL));

                if (Result != 0)
                {
                    //errorHint.InnerHtml = "请求参数SPTokenRequest不正确";
                }

                if (!CommonUtility.ValidateUrl(ReturnURL.Trim()))
                {
                    //errorHint.InnerHtml = "请求参数ReturnURL不正确";
                }


           
            }
            else
            {
                // 缺少参数 SPTokenRequest
                SPTokenRequest = "35433333%24dqS%2BhL04fl53JX5nAN7zsMtH8iUrZAg6OAvGImW0XvlceX36EB%2Flki%2BTx6GQAbC%2F7fwXuoU4M68G%0ACOcdPBCsXRIvwwjzkK8f%2BvZXOuZU0mgYNYRTyVxpm6Olgj7wN8Yqno3VZ14RwXYfyMZ0rqUXEHaR%0ATWPyEFCvsc54PR6i9nGnnJyyDTVszg%3D%3D";
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

    protected void CreateUdbPassportLoginRequest()
    {
        UDBReturnURL = System.Configuration.ConfigurationManager.AppSettings["UDBReturnURL"];
        UDBReturnURL = UDBReturnURL + "&ReturnUrl=" + HttpUtility.UrlEncode(ReturnURL);
        UdbSrcSsDeviceNo = System.Configuration.ConfigurationManager.AppSettings["UdbSrcSsDeviceNo"];
        UdbKey = System.Configuration.ConfigurationManager.AppSettings["UdbKey"];
        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String Digest = CryptographyUtil.ToBase64String(CryptographyUtil.Hash(UdbSrcSsDeviceNo + TimeStamp + UDBReturnURL));
        passportLoginRequestValue = System.Web.HttpUtility.UrlEncode(UdbSrcSsDeviceNo + "$" + CryptographyUtil.Encrypt(TimeStamp + "$" + UDBReturnURL + "$" + Digest, UdbKey));
        UDBLoginURL = System.Configuration.ConfigurationManager.AppSettings["UDBLoginURL"];
        login189Url = UDBLoginURL + "?PassportLoginRequest=" + passportLoginRequestValue;
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("publicpassportlogin", msg);
    }


}

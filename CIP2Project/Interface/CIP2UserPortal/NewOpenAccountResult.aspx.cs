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
public partial class NewOpenAccountResult : System.Web.UI.Page
{
    string BesttoneAccount = "";
    string ReturnUrl = "";
    string CustID = "";
    string HeadFooter = "";
    string stamp = "";
    String From = "";

    string SPTokenRequest = "";
    string SPID = "35000000";
    string ErrMsg = "";
    int Result = 0;
    string RegistryResponse = "";
    public string CreateBesttoneAccountResult = "-1";

    protected void Page_Load(object sender, EventArgs e)
    {

        StringBuilder strLog = new StringBuilder();
        RegistryResponse = Request["RegistryResponse"];
        ParseSPTokenRequest();
        if (ReturnUrl.IndexOf("?") > 0)
        {
            ReturnUrl = ReturnUrl + "&RegistryResponse=" + RegistryResponse;
         
        }
        else
        {
            ReturnUrl = ReturnUrl + "?RegistryResponse=" + RegistryResponse;
          
        }

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
                strLog.AppendFormat("OpenAccountResult-wait3-redirect-to:{0}", ReturnUrl);
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
            Result = ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID, out HeadFooter, out ReturnUrl, out From, out ErrMsg);
            //日志
            strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},CustID:{3},HeadFooter:{4},stamp:{5},ReturnUrl:{6}", Result, ErrMsg, SPID, CustID, HeadFooter, stamp, ReturnUrl));



        }
        log(strLog.ToString());
    }

    protected int ParseBesttoneAccountPageRequest(string SourceStr, HttpContext context, out string SPID, out string CustID,
         out string HeadFooter, out string ReturnURL, out string From, out string ErrMsg)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("解析SPTokenRequest过程:\r\n");
        strLog.AppendFormat("Params: SPTokenRequest:{0}\r\n", SourceStr);
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = "";
        SPID = "";
        CustID = "";
        HeadFooter = "";
        ReturnURL = "";
        From = "";
        string TimeStamp = "";

        string Digest = "";
        try
        {
            string[] alSourceStr = SourceStr.Split('$');
            SPID = alSourceStr[0].ToString();
            strLog.AppendFormat("SPID:{0}\r\n", SPID);
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(context, "SPData");
            string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            strLog.AppendFormat("获取密钥:{0}\r\n", ScoreSystemSecret);
            string EncryptSourceStr = alSourceStr[1].ToString();
            strLog.AppendFormat("密文:{0}\r\n", EncryptSourceStr);
            string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
            strLog.AppendFormat("解密.....\r\n");
            strLog.AppendFormat("明文:{0}\r\n", RequestStr);
            string[] alRequest = RequestStr.Split('$');

            //加密顺序：URLEncoding(SPID + "$" + Base64(Encrypt(CustId + "$"  + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + From+ "$" + Digest)))
            //Digest = Base64(Encrypt(Hash(CustId + "$"+ReturnURL +"$"+ HeadFooter "$"+TimeStamp+"$"+From)))
            CustID = alRequest[0].ToString();
            strLog.AppendFormat("CustID:{0}\r\n", CustID);
            ReturnURL = alRequest[1].ToString();
            strLog.AppendFormat("ReturnURL:{0}\r\n", ReturnURL);
            HeadFooter = alRequest[2].ToString();
            strLog.AppendFormat("HeadFooter:{0}\r\n", HeadFooter);
            TimeStamp = alRequest[3].ToString();
            strLog.AppendFormat("TimeStamp:{0}\r\n", TimeStamp);
            From = alRequest[4].ToString();
            strLog.AppendFormat("From:{0}\r\n", From);
            Digest = alRequest[5].ToString();
            strLog.AppendFormat("Digest:{0}\r\n", Digest);
            //校验摘要 Digest 信息
            string NewDigest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + From, ScoreSystemSecret);
            strLog.AppendFormat("NewDigest:{0}\r\n", NewDigest);
            if (Digest != NewDigest)
            {
                Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                ErrMsg = "无效的Digest";
                return Result;
            }

            Result = 0;
        }
        catch (Exception e)
        {
            Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrMsg = e.Message;
        }
        finally
        {
            log(strLog.ToString());
        }
        return Result;
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("NewOpenAccountResult", msg);
    }


}

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
using log4net;

public partial class CustInfoManager_RegisterSuccess : System.Web.UI.Page
{
    private String RegistryResponse;

    public String CustID = String.Empty;

    public String SPID = String.Empty;

    public String HeadFooter = String.Empty;

    public String ReturnURL = String.Empty;

    public String From = String.Empty;

    public String ErrMsg = String.Empty;

    public Int32 Result;

    public String SetAuthenPhoneUrl = String.Empty;

    public String SetAuthenEmailUrl = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        RegistryResponse = Request["RegistryResponse"];
        if (!IsPostBack)
        {
            Result = BeginParseToken(RegistryResponse, this.Context, out SPID, out CustID, out HeadFooter, out ReturnURL, out From, out ErrMsg);
       
        
        }

    }


    protected int BeginParseToken(string SourceStr, HttpContext context, out string SPID, out string CustID,
         out string HeadFooter, out string ReturnURL, out string From, out string ErrMsg)
    {
        StringBuilder strLog = new StringBuilder();

        strLog.AppendFormat("-----------解析SPTokenRequest开始:-----------\r\n");
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
            strLog.AppendFormat("-----------解析SPTokenRequest结束:-----------\r\n");
            log(strLog.ToString());
        }
        return Result;
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("RegisterSuccess", msg);
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect(SetAuthenPhoneUrl + "?SPID=" + SPID + "&CustID=" + CustID + "&ReturnURL=" + ReturnURL, false);
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect(SetAuthenEmailUrl + "?SPID=" + SPID + "&CustID=" + CustID + "&ReturnURL=" + ReturnURL, false);
    }
}

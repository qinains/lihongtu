﻿using System;
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
public partial class UnifyPlatformWebRegister : System.Web.UI.Page
{
    private String SPTokenRequest = "";
    private Int32 Result = 0;
    private String ErrMsg = String.Empty;

    public String SPID = String.Empty;
    public String CustID = String.Empty;

    public String ReturnUrl = String.Empty;  // 业平台返回地址

    private String HeadFooter = "";
    private String From = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");
        ParseSPTokenRequest();

        StringBuilder strLog = new StringBuilder();
        #region
        //SPTokenRequest = Request["SPTokenRequest"];
        //strLog.AppendFormat("-----------解析SPTokenRequest开始:-----------\r\n");
        //strLog.AppendFormat("Params: SPTokenRequest:{0}\r\n", SPTokenRequest);
        //string TimeStamp = "";
        //string Digest = "";
        //try
        //{
        //    string[] alSourceStr = SPTokenRequest.Split('$');
        //    SPID = alSourceStr[0].ToString();
        //    strLog.AppendFormat("SPID:{0}\r\n", SPID);

        //    SPInfoManager spInfo = new SPInfoManager();
        //    Object SPData = spInfo.GetSPData(this.Context, "SPData");
        //    string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

        //    strLog.AppendFormat("获取密钥:{0}\r\n", ScoreSystemSecret);
        //    string EncryptSourceStr = alSourceStr[1].ToString();
        //    strLog.AppendFormat("密文:{0}\r\n", EncryptSourceStr);
        //    string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
        //    strLog.AppendFormat("解密.....\r\n");
        //    strLog.AppendFormat("明文:{0}\r\n", RequestStr);
        //    string[] alRequest = RequestStr.Split('$');

        //    //加密顺序：URLEncoding(SPID + "$" + Base64(Encrypt(CustId + "$"  + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + From+ "$" + Digest)))
        //    //Digest = Base64(Encrypt(Hash(CustId + "$"+ReturnURL +"$"+ HeadFooter "$"+TimeStamp+"$"+From)))
        //    CustID = alRequest[0].ToString();
        //    strLog.AppendFormat("CustID:{0}\r\n", CustID);
        //    ReturnUrl = alRequest[1].ToString();
        //    strLog.AppendFormat("ReturnURL:{0}\r\n", ReturnUrl);
        //    TimeStamp = alRequest[3].ToString();
        //    strLog.AppendFormat("TimeStamp:{0}\r\n", TimeStamp);
        //    Digest = alRequest[5].ToString();
        //    strLog.AppendFormat("Digest:{0}\r\n", Digest);
        //    //校验摘要 Digest 信息
        //    string NewDigest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + ReturnUrl + "$" + TimeStamp, ScoreSystemSecret);
        //    strLog.AppendFormat("NewDigest:{0}\r\n", NewDigest);
        //    if (Digest != NewDigest)
        //    {
        //        Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
        //        ErrMsg = "无效的Digest";
        //        strLog.AppendFormat("签名:result:{0},msg:{1}\r\n", Result, ErrMsg);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
        //    ErrMsg = ex.Message;
        //    strLog.AppendFormat("解析SPTokenRequest异常:{0},msg:{1}\r\n", Result, ErrMsg);
        //}
        //finally
        //{
        //    strLog.AppendFormat("-----------解析SPTokenRequest结束:-----------\r\n");
        //    log(strLog.ToString());
        //}

        #endregion


        string unifyPlatformWebRegisterUrl = UDBConstDefinition.DefaultInstance.UnifyPlatformWebRegisterUrl;   //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_LogonUrl"];  // 综合平台回调客户信息平台地址
        string appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
        string returnURL = UDBConstDefinition.DefaultInstance.UnifyPlatformWebRegisterCallBackUrl;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatformCallBackUrl"];
        returnURL = HttpUtility.UrlEncode(returnURL + "?SPID=" + SPID + "&ReturnUrl=" + HttpUtility.UrlEncode(ReturnUrl));
        
        unifyPlatformWebRegisterUrl = unifyPlatformWebRegisterUrl + "?appKey=" + appId + "&returnUrl=" + returnURL+"&needToken=true";
        strLog.Append("pageLoad()\r\n");
        strLog.AppendFormat("产品returnurl:{0}\r\n", ReturnUrl);
        strLog.AppendFormat("unifyPlatformWebRegisterUrl:{0}\r\n", unifyPlatformWebRegisterUrl);
        log(strLog.ToString());
        if (Result == 0)
        {
            Response.Redirect(unifyPlatformWebRegisterUrl, false);
        }
        else
        {
            strLog.AppendFormat("解析token异常，终止操作：{0}，msg:{1}\r\n",Result,ErrMsg);
            log(strLog.ToString());
        }
        
    }

    protected void ParseSPTokenRequest()
    {
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //解析请求参数
            Result = BeginParseSPToken(SPTokenRequest, this.Context, out SPID, out CustID, out HeadFooter, out ReturnUrl, out From, out ErrMsg);
          
        }
    }

    //protected void ParseSPTokenRequest()
    //{
    //    if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
    //    {
    //        SPTokenRequest = Request["SPTokenRequest"];
    //        //解析请求参数
    //        Result = BeginParseSPToken(SPTokenRequest, this.Context, out SPID, out CustID, out ReturnUrl, out ErrMsg);

    //    }
    //}

    //protected int BeginParseSPToken(string SourceStr, HttpContext context, out string SPID, out string CustID,
    //    out string ReturnURL, out string ErrMsg)
    //{
    //    StringBuilder strLog = new StringBuilder();

    //    strLog.AppendFormat("-----------解析SPTokenRequest开始:-----------\r\n");
    //    strLog.AppendFormat("Params: SPTokenRequest:{0}\r\n", SourceStr);
    //    int Result = ErrorDefinition.IError_Result_UnknowError_Code;
    //    ErrMsg = "";
    //    SPID = "";
    //    CustID = "";
    //    ReturnURL = "";
    //    string TimeStamp = "";

    //    string Digest = "";
    //    try
    //    {
    //        string[] alSourceStr = SourceStr.Split('$');
    //        SPID = alSourceStr[0].ToString();
    //        strLog.AppendFormat("SPID:{0}\r\n", SPID);
    //        SPInfoManager spInfo = new SPInfoManager();
    //        Object SPData = spInfo.GetSPData(context, "SPData");
    //        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
    //        strLog.AppendFormat("获取密钥:{0}\r\n", ScoreSystemSecret);
    //        string EncryptSourceStr = alSourceStr[1].ToString();
    //        strLog.AppendFormat("密文:{0}\r\n", EncryptSourceStr);
    //        string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
    //        strLog.AppendFormat("解密.....\r\n");
    //        strLog.AppendFormat("明文:{0}\r\n", RequestStr);
    //        string[] alRequest = RequestStr.Split('$');

    //        //加密顺序：URLEncoding(SPID + "$" + Base64(Encrypt(CustId + "$"  + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + From+ "$" + Digest)))
    //        //Digest = Base64(Encrypt(Hash(CustId + "$"+ReturnURL +"$"+ HeadFooter "$"+TimeStamp+"$"+From)))
    //        CustID = alRequest[0].ToString();
    //        strLog.AppendFormat("CustID:{0}\r\n", CustID);
    //        ReturnURL = alRequest[1].ToString();
    //        strLog.AppendFormat("ReturnURL:{0}\r\n", ReturnURL);
    //        TimeStamp = alRequest[3].ToString();
    //        strLog.AppendFormat("TimeStamp:{0}\r\n", TimeStamp);
    //        Digest = alRequest[5].ToString();
    //        strLog.AppendFormat("Digest:{0}\r\n", Digest);
    //        //校验摘要 Digest 信息
    //        string NewDigest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + ReturnURL + "$" + TimeStamp, ScoreSystemSecret);
    //        strLog.AppendFormat("NewDigest:{0}\r\n", NewDigest);
    //        if (Digest != NewDigest)
    //        {
    //            Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
    //            ErrMsg = "无效的Digest";
    //            return Result;
    //        }

    //        Result = 0;
    //    }
    //    catch (Exception e)
    //    {
    //        Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
    //        ErrMsg = e.Message;
    //    }
    //    finally
    //    {
    //        strLog.AppendFormat("-----------解析SPTokenRequest结束:-----------\r\n");
    //        log(strLog.ToString());
    //    }
    //    return Result;
    //}


    protected int BeginParseSPToken(string SourceStr, HttpContext context, out string SPID, out string CustID,
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
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyPlatformRegisterWebPage", msg);
    }
}
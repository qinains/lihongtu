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
using Newtonsoft.Json;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Specialized;

public partial class UnifyPlatformCustomer_GetUserInfo : System.Web.UI.Page
{

    public String SPID;
    public Int32 Result;
    public String ErrMsg;
    public String AccessToken;

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        AccessToken = Request["accessToken"];

        String ResponseText = GetUserInfo(SPID, AccessToken);
        Response.ContentType = "xml/text";
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }

    public String GetUserInfo(String SPID, String AccessToken)
    {
        StringBuilder strMsg = new StringBuilder();
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
        strMsg.AppendFormat("接收参数 SPID:{0},AccessToken:{1}\r\n", SPID, AccessToken);
        
        #region 数据校验
        if (CommonUtility.IsEmpty(SPID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(AccessToken))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AccessToken不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }


        #endregion

        String appId = String.Empty;
        String appSecret = String.Empty;
        String version = String.Empty;
        String clientType = String.Empty;
        String clientIp = String.Empty;
        String clientAgent = String.Empty;
        #region

        try
        {
            try
            {
                appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId;
                appSecret = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;
                version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;
                clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
                clientIp = HttpContext.Current.Request.UserHostAddress;
                clientAgent = HttpContext.Current.Request.UserAgent;
                strMsg.AppendFormat("获取综合平台接入参数:appId:{0},appSecret:{1},version:{2},clientType:{3},clientIp:{4},clientAgent:{5}\r\n", appId, appSecret, version, clientType, clientIp, clientAgent);

            }
            catch (Exception e)
            {
                ResponseMsg.Length = 0;
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "获取综合平台参数异常:" + e.ToString());
                ResponseMsg.Append("}");
                return ResponseMsg.ToString();
            }

            UnifyAccountInfo accountInfo = new UnifyAccountInfo();
            try
            {

                UDBMBOSS _UDBMBoss = new UDBMBOSS();
                Result = _UDBMBoss.UnifyPlatformGetUserInfo(appId, appSecret, version, clientType, AccessToken, clientIp, clientAgent, out accountInfo, out ErrMsg);
                strMsg.AppendFormat("根据accesstoken去综合平台查询结果,Result:{0},ErrMsg{1}\r\n", Result, ErrMsg);
                if (accountInfo != null)
                {
                    strMsg.AppendFormat("account.userId:{0},accountInfo.userName:{1},accountInfo.province:{2},accountInfo.city:{3},accountInfo.mobileName:{4},accountInfo.emailName:{5},accountInfo.gender:{6}\r\n", accountInfo.userId, accountInfo.userName, accountInfo.province, accountInfo.city, accountInfo.mobileName, accountInfo.emailName, accountInfo.gender);

                    ResponseMsg.Length = 0;
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                    ResponseMsg.AppendFormat("\"userName\":\"{0}\",", accountInfo.userName);
                    ResponseMsg.AppendFormat("\"userId\":\"{0}\",", accountInfo.userId);
                    ResponseMsg.AppendFormat("\"nickname\":\"{0}\",", accountInfo.nickName);
                    ResponseMsg.AppendFormat("\"gender\":\"{0}\",", accountInfo.gender);
                    ResponseMsg.AppendFormat("\"birthday\":\"{0}\",", accountInfo.birthday);
                    ResponseMsg.AppendFormat("\"province\":\"{0}\",", accountInfo.province);
                    ResponseMsg.AppendFormat("\"city\":\"{0}\",", accountInfo.city);
                    ResponseMsg.AppendFormat("\"address\":\"{0}\",", accountInfo.address);
                    ResponseMsg.AppendFormat("\"mail\":\"{0}\",", accountInfo.mail);
                    ResponseMsg.AppendFormat("\"qq\":\"{0}\",", accountInfo.qq);
                    ResponseMsg.AppendFormat("\"position\":\"{0}\",", accountInfo.position);
                    ResponseMsg.AppendFormat("\"intro\":\"{0}\",", accountInfo.intro);
                    ResponseMsg.AppendFormat("\"photo\":\"{0}\",", accountInfo.userIconUrl1);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "");
                    ResponseMsg.Append("}");
                    return ResponseMsg.ToString();
                }
            }
            catch (Exception e)
            {
                ResponseMsg.Length = 0;
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "根据accesstoken去综合平台查询:" + e.ToString());
                ResponseMsg.Append("}");
                return ResponseMsg.ToString();
            }
        }
        catch (Exception ex)
        {
            strMsg.AppendFormat("ex:{0}\r\n",ex.ToString());
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ":" + ex.ToString());
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }
        finally
        {
            log(strMsg.ToString());        
        }
        #endregion
        return ResponseMsg.ToString();
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyPlatformGetUserInfo_http", msg);
    }

}

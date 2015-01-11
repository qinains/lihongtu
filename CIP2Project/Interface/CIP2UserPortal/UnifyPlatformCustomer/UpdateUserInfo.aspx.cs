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

public partial class UnifyPlatformCustomer_UpdateUserInfo : System.Web.UI.Page
{
    public String SPID;
    public Int32 Result;
    public String ErrMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public String UpdateUserInfo( String SPID,String AccessToken,UnifyAccountInfo accountInfo)
    {
        StringBuilder strLog = new StringBuilder();
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
        strLog.AppendFormat("接收参数 SPID:{0},gender:{1},birthday:{2},province:{3},city:{4},address:{5},mail:{6},qq:{7},position:{8},intro:{9},nickname:{10}\r\n",
            SPID, accountInfo.gender,
            accountInfo.birthday, accountInfo.province, accountInfo.city,
            accountInfo.address, accountInfo.mail, accountInfo.qq, accountInfo.position, accountInfo.intro,
            accountInfo.nickName);

        #region  数据校验
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
                strLog.AppendFormat("获取综合平台接入参数:appId:{0},appSecret:{1},version:{2},clientType:{3},clientIp:{4},clientAgent:{5}\r\n", appId, appSecret, version, clientType, clientIp, clientAgent);

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


            try
            {
                UDBMBOSS _UDBMBoss = new UDBMBOSS();
                Result = _UDBMBoss.UnifyPlatformUpdateUserInfo(appId, appSecret, version, clientType, AccessToken, clientIp, clientAgent, accountInfo, out ErrMsg);

                ResponseMsg.Length = 0;
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ":" + ErrMsg);
                ResponseMsg.Append("}");
                return ResponseMsg.ToString();
                

            }
            catch (Exception e)
            {
                strLog.AppendFormat("ex:{0}\r\n", e.ToString());
                ResponseMsg.Length = 0;
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改信息失败:" + e.ToString());
                ResponseMsg.Append("}");
                return ResponseMsg.ToString();
            }


        }
        catch(Exception ex)
        {
            strLog.AppendFormat("ex:{0}\r\n",ex.ToString());
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改信息失败:" + ex.ToString());
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }
        finally
        {
            log(strLog.ToString());
        }
        return ResponseMsg.ToString();
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyPlatformUpdateUserInfo_http", msg);
    }
}

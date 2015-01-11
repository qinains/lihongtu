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
using System.Text;
using System.Text.RegularExpressions;
public partial class UnifyPlatformCustomer_UnifyPlatformAccountBindingBesttoneAccount : System.Web.UI.Page
{

    public String SPID = String.Empty;
    public String AccessToken = String.Empty;
    public String CustID = String.Empty;
    public String OperType = String.Empty;

    public Int32 Result;
    public String ErrMsg;

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        AccessToken = Request["AccessToken"];
        CustID = Request["CustID"];  //非必填
        OperType = Request["OperType"];
        String ResponseText = UnifyAccountBinding(SPID, CustID,AccessToken, OperType); 
        Response.ContentType = "xml/text";
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }

    public String UnifyAccountBinding(String SPID,String CustID,String AccessToken,String OperType)
    {
        StringBuilder strLog = new StringBuilder();
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;

        strLog.AppendFormat("接收参数 SPID:{0},CustID:{1},AccessToken:{2},OperType:{3}\r\n",SPID,CustID,AccessToken,OperType);

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
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AccessToken不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }

 
        if (CommonUtility.IsEmpty(OperType))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "OperType不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }
        #endregion

        String appId =String.Empty ;
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

            UnifyAccountInfo accountInfo = new UnifyAccountInfo();
            strLog.Append("根据accesstoken去综合平台查询\r\n");

            try
            {

                UDBMBOSS _UDBMBoss = new UDBMBOSS();
                Result = _UDBMBoss.UnifyPlatformGetUserInfo(appId, appSecret, version, clientType, AccessToken, clientIp, clientAgent, out accountInfo, out ErrMsg);
                strLog.AppendFormat("根据accesstoken去综合平台查询结果,Result:{0},ErrMsg{1}\r\n", Result, ErrMsg);
                if (accountInfo != null)
                {
                    strLog.AppendFormat("account.userId:{0},accountInfo.userName:{1},accountInfo.province:{2},accountInfo.city:{3},accountInfo.mobileName:{4},accountInfo.emailName:{5},accountInfo.gender:{6}\r\n", accountInfo.userId, accountInfo.userName, accountInfo.province, accountInfo.city, accountInfo.mobileName, accountInfo.emailName, accountInfo.gender);
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

            strLog.Append("建立绑定关系\r\n");
            try
            {

                if (Result == 0 && accountInfo != null && !String.IsNullOrEmpty(Convert.ToString(accountInfo.userId)))
                {
                    Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", accountInfo.mobileName,
                        accountInfo.emailName, accountInfo.nickName, "", accountInfo.userId, SPID, OperType, out CustID, out ErrMsg);
                    strLog.AppendFormat("绑定结果Result:{0},Errmsg:{1}\r\n", Result, ErrMsg);

                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                    ResponseMsg.AppendFormat("\"UserID\":\"{0}\"", accountInfo.userId);
                    ResponseMsg.Append("}");
                    strLog.AppendFormat("返回json:{0}\r\n", ResponseMsg.ToString());
                }
            }
            catch (Exception e)
            {
                ResponseMsg.Length = 0;
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1000");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "建立绑定关系失败:" + e.ToString());
                ResponseMsg.Append("}");
                return ResponseMsg.ToString();
            }
        }
        catch (Exception ex)
        {
            strLog.AppendFormat("ex:{0}\r\n",ex.ToString());
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ":" + ex.ToString());
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
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyPlatformAccountBindingBesttoneAccount", msg);
    }

}

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

public partial class UnifyPlatformCustomer_UpdatePwd : System.Web.UI.Page
{
    public String SPID;
    public Int32 Result;
    public String ErrMsg;
    public String accessToken;
    public String CustID;
    public String password;
    public String nPassword;
    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        accessToken = Request["accessToken"];
        CustID = Request["CustID"];
        password = Request["password"];
        nPassword = Request["nPassword"];
        String ResponseText = UpdatePwd(SPID, accessToken, password, nPassword);
        Response.ContentType = "xml/text";
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }


    public String UpdatePwd(String SPID, String accessToken, String password, String nPassword)
    {
        StringBuilder strMsg = new StringBuilder();
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
        strMsg.AppendFormat("接收参数 SPID:{0},accessToken:{1},password:{2},nPassword:{3}\r\n", SPID, accessToken, password, nPassword);

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

        if (CommonUtility.IsEmpty(accessToken) && CommonUtility.IsEmpty(CustID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "accessToken和CustID不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(password))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "password不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(nPassword))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "nPassword不能为空！");
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

        #region  获取综合平台接入参数

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
        #endregion
        String jsonResult = String.Empty;
        string sign = String.Empty;
        try
        {

            #region
      
            if(CommonUtility.IsEmpty(accessToken) && !CommonUtility.IsEmpty(CustID))
            {
                   Result = CIP2BizRules.FetchAccessTokenFromCustID(CustID,out accessToken,out ErrMsg);         
            }

            if(Result!=0 || CommonUtility.IsEmpty(accessToken))
            {
                ResponseMsg.Length = 0;
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-10");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "accesstoken没拿到，修改密码失败");
                ResponseMsg.Append("}");
                return ResponseMsg.ToString();
            }
            string paras = String.Empty;
            string format = "json";
            string parameters = "accessToken=" + accessToken + "&password=" + password + "&nPassword=" + nPassword + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
            strMsg.AppendFormat("parameters:={0}\r\n", parameters);
            paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
            strMsg.AppendFormat("paras:={0}\r\n", paras);
            sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
            strMsg.AppendFormat("sign:={0}\r\n", sign);
            NameValueCollection postData = new NameValueCollection();
            postData.Add("appId", appId);
            postData.Add("version", version);
            postData.Add("clientType", clientType);
            postData.Add("paras", paras);
            postData.Add("sign", sign);
            postData.Add("format", format);

            WebClient webclient = new WebClient();
            webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformUpdatePwdUrl, "POST", postData);
            jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
            strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
            #endregion

            Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
            int result = -1;
            String s_result = String.Empty;
            String msg = String.Empty;
            result_dic.TryGetValue("msg", out msg);
            result_dic.TryGetValue("result", out s_result);
            result = Convert.ToInt32(s_result);
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", msg);
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }
        catch (Exception e)
        {
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-10");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", e.ToString());
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }


        return ResponseMsg.ToString();
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyUpdatePwd_http", msg);
    }

}

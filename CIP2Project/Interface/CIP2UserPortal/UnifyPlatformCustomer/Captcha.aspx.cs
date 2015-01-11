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
public partial class UnifyPlatformCustomer_Captcha : System.Web.UI.Page
{
    public String SPID;
    public Int32 Result;
    public String ErrMsg;
    public String type;  //  1普通注册验证码 2可用于初始密码的验证码 3用于营销目的验证码 4重置密码的验证码 不传默认为1
    public String mobile;

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        mobile = Request["mobile"];
        type = Request["type"];
        if (String.IsNullOrEmpty(type))
        {
            type = "1";
        }
        String ResponseText = Captcha(SPID, mobile, type);
        Response.ContentType = "xml/text";
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }

    public String Captcha(String SPID,String mobile,String type)
    {
        StringBuilder strMsg = new StringBuilder();
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
        strMsg.AppendFormat("接收参数 SPID:{0},mobile:{1},type:{2}\r\n", SPID, mobile, type);

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


        if (CommonUtility.IsEmpty(mobile))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "mobile不能为空！");
            ResponseMsg.Append("}");
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(type))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "type不能为空！");
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
            string paras = String.Empty;
            string format = "json";
            string parameters = "mobile=" + mobile + "&type=" + type +"&clientIp="+clientIp+ "&clientAgent=" + clientAgent;
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
            byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformCaptchaUrl, "POST", postData);
            jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
            strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
            #endregion

            Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
            int result = -1;
            String s_result = String.Empty;
            String msg = String.Empty;
            result_dic.TryGetValue("msg",out msg);
            result_dic.TryGetValue("result",out s_result);
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
        }


        return ResponseMsg.ToString();
    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("Captcha_http", msg);
    }
}

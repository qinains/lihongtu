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

public partial class UnifyPlatformCustomer_UnifyLogin : System.Web.UI.Page
{
    public String SPID;
    public Int32 Result;
    public String ErrMsg;
    public String userName; 
    public String password;

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        userName = Request["userName"];
        password = Request["password"];
        String ResponseText = login(SPID, userName, password);
        Response.ContentType = "xml/text";
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }

    public String login(String SPID, String userName, String password)
    {
        StringBuilder strMsg = new StringBuilder();
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
        strMsg.AppendFormat("接收参数 SPID:{0},userName:{1},password:{2}\r\n", SPID, userName, password);

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

        if (CommonUtility.IsEmpty(userName))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "userName不能为空！");
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
            string parameters = "userName=" + userName + "&password=" + password  + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
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
            byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformUpdateYingShiLoginUrl, "POST", postData);
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



            String userId = String.Empty;
            String zhUserName = String.Empty;
            String pUserId = String.Empty;
            String productUid  = String.Empty;
            String userType = String.Empty;
            String status = String.Empty;
            String aliasName = String.Empty;
            String provinceId = String.Empty;
            String cityId = String.Empty;
            String accessToken = String.Empty;
            String expiresIn = String.Empty;
            String loginNum = String.Empty;
            String nickName = String.Empty;
            String mobileName = String.Empty;
            String userIconUrl = String.Empty;
            String userIconUrl2 = String.Empty;
            String userIconUrl3 = String.Empty;

            result_dic.TryGetValue("userId", out userId);
            result_dic.TryGetValue("userName", out userName);
            result_dic.TryGetValue("zhUserName", out zhUserName);
            result_dic.TryGetValue("pUserId", out pUserId);
            result_dic.TryGetValue("productUid", out productUid);
            result_dic.TryGetValue("userType", out userType);
            result_dic.TryGetValue("status", out status);
            result_dic.TryGetValue("aliasName", out aliasName);
            result_dic.TryGetValue("provinceId", out provinceId);
            result_dic.TryGetValue("cityId", out cityId);
            result_dic.TryGetValue("accessToken", out accessToken);
            result_dic.TryGetValue("expiresIn", out expiresIn);
            result_dic.TryGetValue("loginNum", out loginNum);
            result_dic.TryGetValue("nickName", out nickName);
            result_dic.TryGetValue("mobileName", out mobileName);
            result_dic.TryGetValue("userIconUrl", out userIconUrl);
            result_dic.TryGetValue("userIconUrl2", out userIconUrl2);
            result_dic.TryGetValue("userIconUrl3", out userIconUrl3);

            ResponseMsg.Length = 0;
            ResponseMsg.Append("{");
            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", s_result);


            ResponseMsg.AppendFormat("\"userId\":\"{0}\",", userId);
            ResponseMsg.AppendFormat("\"userName\":\"{0}\",", userName);
            ResponseMsg.AppendFormat("\"zhUserName\":\"{0}\",", zhUserName);
            ResponseMsg.AppendFormat("\"pUserId\":\"{0}\",", pUserId);
            ResponseMsg.AppendFormat("\"productUid\":\"{0}\",", productUid);
            ResponseMsg.AppendFormat("\"userType\":\"{0}\",", userType);
            ResponseMsg.AppendFormat("\"status\":\"{0}\",", status);
            ResponseMsg.AppendFormat("\"aliasName\":\"{0}\",", aliasName);
            ResponseMsg.AppendFormat("\"provinceId\":\"{0}\",", provinceId);
            ResponseMsg.AppendFormat("\"cityId\":\"{0}\",", cityId);
            ResponseMsg.AppendFormat("\"accessToken\":\"{0}\",", accessToken);
            ResponseMsg.AppendFormat("\"expiresIn\":\"{0}\",", expiresIn);
            ResponseMsg.AppendFormat("\"loginNum\":\"{0}\",", loginNum);
            ResponseMsg.AppendFormat("\"nickName\":\"{0}\",", nickName);
            ResponseMsg.AppendFormat("\"mobileName\":\"{0}\",", mobileName);
            ResponseMsg.AppendFormat("\"userIconUrl\":\"{0}\",", userIconUrl);
            ResponseMsg.AppendFormat("\"userIconUrl2\":\"{0}\",", userIconUrl2);
            ResponseMsg.AppendFormat("\"userIconUrl3\":\"{0}\",", userIconUrl3);
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
        BTUCenterInterfaceLog.CenterForBizTourLog("UnifyLogin_http", msg);
    }


}

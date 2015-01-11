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
public partial class SSO_mobile_Open189CallBackWap : System.Web.UI.Page
{
    public String ticket = "";

    #region 私有变量
    /// <summary>
    /// UDB系统返回参数
    /// </summary>
    /// 
    private String PassportLoginResponseValue = String.Empty;
    /// <summary>
    /// UDB返回UDBTicket
    /// </summary>
    private String UDBTicket = String.Empty;
    /// <summary>
    /// 应用系统需要返回的url
    /// </summary>
    public String ReturnUrl = String.Empty;
    /// <summary>
    /// 应用系统在UDB的SPID
    /// </summary>
    private String UDBSPID = String.Empty;
    private String UDBKey = String.Empty;
    /// <summary>
    /// 应用系统在客户信息平台的SPID
    /// </summary>
    private String SPID = String.Empty;
    private String key = String.Empty;
    /// <summary>
    /// 返回方设备标识【UDB 系统】
    /// </summary>
    private String DesSsDeviceNo = String.Empty;

    private UDBMBOSS _UDBMBoss = new UDBMBOSS();


    private String LSID = String.Empty;
    private String appId = String.Empty;
    private String paras = String.Empty;
    private String sign = String.Empty;

    public string welcomeName = String.Empty;
    public string CustID;
    public string RealName;
    public string UserName;
    public string NickName;
    public string CertificateType;
    public string CertificateCode;
    public string Email;
    public string Sex;
    public string Phone;
    public string Phone1;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {


        if (CommonUtility.IsParameterExist("appId", this.Page) && CommonUtility.IsParameterExist("paras", this.Page) && CommonUtility.IsParameterExist("sign", this.Page) && CommonUtility.IsParameterExist("SPID", this.Page))
        {
            ProcessUnifyPlatformReturn();
        }
        else
        {
            Redirect("ErrMsg", "appId,paras,sign,spid缺失");
        }
    }


    /// <summary>
    /// 开始UDBSSO功能
    /// </summary>
    protected void ProcessUnifyPlatformReturn()
    {
        StringBuilder strMsg = new StringBuilder();
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        try
        {
            #region 获取参数并验证

            SPID = Request["SPID"];
            ReturnUrl = Request["ReturnUrl"] == null ? String.Empty : Request["ReturnUrl"];
            LSID = Request["LSID"];
            appId = Request["appId"];
            paras = Request["paras"];
            sign = Request["sign"];

            string unifyPlatform_appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
            string unifyPlatform_appSecretKey = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];


            strMsg.AppendFormat("【验证参数，DateTime：{0}】:SPID:{1},LSID:{2},ReturnUrl:{3},appId:{4},paras:{5},sign:{6}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), SPID, LSID, ReturnUrl, appId, paras, sign);

            string unifyPlatformResponse = CryptographyUtil.XXTeaDecrypt(paras, unifyPlatform_appSecretKey);
            strMsg.AppendFormat("unifyPlatformResponse:{0}\r\n", unifyPlatformResponse);
            string newsign = CryptographyUtil.HMAC_SHA1(unifyPlatform_appId + paras, unifyPlatform_appSecretKey);
            strMsg.AppendFormat("newsign:{0},sign:{1}\r\n", newsign, sign);
            strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
            if (!newsign.Equals(sign))
            {
                Redirect("ErrMsg", "签名不正确");
            }

            //paras {result,accessToken,timeStamp,userId,productUid,loginNum,nickName,userIconUrl,userIconUrl2,userIconUrl3,isThirdAccount}
            string result = "";
            string accessToken = "";
            string timeStamp = "";
            Int64 userId = 0;
            string productUid = "";
            string loginNum = "";
            string nickName = "";
            string userIconUrl = "";
            string userIconUrl2 = "";
            string userIconUrl3 = "";
            string isThirdAccount = "";


            Dictionary<String, String> parames = new Dictionary<string, string>();
            strMsg.Append("开始解析unifyPlatformResponse\r\n");
            try
            {
                parames = splitParameters(unifyPlatformResponse);
                strMsg.AppendFormat("params:{0}\r\n", parames);
            }
            catch (Exception exp)
            {
                strMsg.AppendFormat(exp.ToString());
            }
            strMsg.Append("解析unifyPlatformResponse完毕\r\n");
            foreach (KeyValuePair<String, String> p in parames)
            {
                if (p.Key.Equals("result"))
                {
                    result = p.Value;
                    strMsg.AppendFormat("result:{0}\r\n", result);
                }
                if (p.Key.Equals("accessToken"))
                {
                    accessToken = p.Value;
                    strMsg.AppendFormat("accessToken:{0}\r\n", accessToken);
                }
                if (p.Key.Equals("timeStamp"))
                {
                    timeStamp = p.Value;
                    strMsg.AppendFormat("timeStamp:{0}\r\n", timeStamp);
                }
                if (p.Key.Equals("userId"))
                {
                    strMsg.AppendFormat("返回的原始userid:{0}\r\n",p.Value);
                    if (!String.IsNullOrEmpty(p.Value))
                    {
                        try
                        {
                            userId = System.Int64.Parse(p.Value);
                        }
                        catch (Exception e)
                        {
                            strMsg.AppendFormat("userid异常:{0}\r\n",e.Message);
                            userId = 0;
                        }
                        
                    
                    }
                    else
                    {
                        userId = 0;
                    }
                                       
                    strMsg.AppendFormat("userId:{0}\r\n", userId);
                }
                if (p.Key.Equals("productUid"))
                {
                    productUid = p.Value;
                    strMsg.AppendFormat("productUid:{0}\r\n", productUid);
                }
                if (p.Key.Equals("loginNum"))
                {
                    loginNum = p.Value;
                    strMsg.AppendFormat("loginNum:{0}\r\n", loginNum);
                }
                if (p.Key.Equals("nickName"))
                {
                    nickName = p.Value;
                    strMsg.AppendFormat("nickName:{0}\r\n", nickName);
                }
                if (p.Key.Equals("userIconUrl"))
                {
                    userIconUrl = p.Value;
                    strMsg.AppendFormat("userIconUrl:{0}\r\n", userIconUrl);
                }
                if (p.Key.Equals("userIconUrl2"))
                {
                    userIconUrl2 = p.Value;
                    strMsg.AppendFormat("userIconUrl2:{0}\r\n", userIconUrl2);
                }
                if (p.Key.Equals("userIconUrl3"))
                {
                    userIconUrl3 = p.Value;
                    strMsg.AppendFormat("userIconUrl3:{0}\r\n", userIconUrl3);
                }
                if (p.Key.Equals("isThirdAccount"))
                {
                    isThirdAccount = p.Value;
                    strMsg.AppendFormat("isThirdAccount:{0}\r\n", isThirdAccount);
                }

            }
            strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);

            #endregion

            #region 开始注册到号百
            welcomeName = loginNum;
            UserName = loginNum;
            RealName = loginNum;
            Phone = loginNum;
            strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
            if ("0".Equals(result) && !String.IsNullOrEmpty(accessToken) && !String.IsNullOrEmpty(loginNum))
            {
                String  OuterID, Status, CustType, CustLevel,   NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                
                Regex regMobile = new Regex(@"^1[345678]\d{9}$");
                Regex regEmail = new Regex(@"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$");
                String MobileName = String.Empty;
                String EmailName = String.Empty;
                RealName = loginNum;
                if (regMobile.IsMatch(loginNum))
                {
                    MobileName = loginNum;
                }

                if (regEmail.IsMatch(loginNum))
                {
                    EmailName = loginNum;
                }
                String EncrytpPassWord = CryptographyUtil.Encrypt("123456");
                String OperType = "2";

                if (!String.IsNullOrEmpty(loginNum))
                {
                    strMsg.Append("【开始注册或绑定到号百】:\r\n");
                    Result = CIP2BizRules.BindCustInfoUnifyPlatform("02", "021", MobileName, EmailName, RealName, EncrytpPassWord, userId, SPID, OperType, out CustID, out ErrMsg);
                    strMsg.Append("【开始注册或绑定到号百的结果】:\r\n");
                    strMsg.AppendFormat("Result:{0},CustID:{1},ErrMsg:{2}\r\n", Result, CustID, ErrMsg);

                    //注册成功,种下cookie
                    if (Result == 0)
                    {
                        Result = CustBasicInfo.getCustInfo(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                            out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                            out AreaID, out RegistrationSource);

                        UserName = loginNum;
                        strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                        if (Result != 0)
                        {
                            strMsg.Append(",ErrMsg:客户不存在" + CustID);
                            //客户不存在
                            Redirect("ErrMsg", "客户不存在");
                        }
                        else
                        {

                            if (RealName != null && !"".Equals(RealName))
                            {
                                welcomeName = RealName;
                            }
                            else if (NickName != null && !"".Equals(NickName))
                            {
                                welcomeName = NickName;
                            }
                            else if (UserName != null && !"".Equals(UserName))
                            {
                                welcomeName = UserName;
                            }
                            else
                            {
                                welcomeName = loginNum;
                            }

                            if (ReturnUrl.IndexOf("?") > 0)
                            {
                                ReturnUrl = ReturnUrl + "&CustID=" + CustID + "&welcomeName=" + welcomeName + "&UserID=" + Convert.ToString(userId);
                            }
                            else
                            {
                                ReturnUrl = ReturnUrl + "?CustID=" + CustID + "&welcomeName=" + welcomeName + "&UserID=" + Convert.ToString(userId);
                            }
                            strMsg.AppendFormat("ReturnUrl:{0}\r\n", ReturnUrl);
                            //Response.Redirect(ReturnUrl, false);
                        }

                        //登录tab写入cookie
                        //PageUtility.SetCookie("LoginTabCookie", "UDBTab", 8760);

                        //strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                        ////生成Ticket
                        //ticket = CommonBizRules.CreateTicket();
                        //Result = CIPTicketManager.insertCIPTicket(ticket, SPID, CustID, RealName, UserName, NickName, OuterID, "unifyTicket", Convert.ToString(userId), "42", out ErrMsg);
                        //strMsg.AppendFormat("【生成ticket】:Result:{0},Ticket:{1}", Result, ticket);
                        //strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                        //if (Result != 0)
                        //{
                        //    strMsg.Append(",ErrMsg:Ticket生成失败" + ticket);
                        //    Redirect("ErrMsg", "Ticket生成失败");
                        //}

                        //insertAccessToken
                        if ((userId != 0) && !String.IsNullOrEmpty(CustID) && !String.IsNullOrEmpty(loginNum))
                        {
                            strMsg.Append("记录AccessToken\r\n");
                            strMsg.AppendFormat("CustID:{0}<->AccessToken:{1}\r\n", CustID, accessToken);
                            String Description = "client登录";
                            Result = CIP2BizRules.InsertAccessToken(SPID, HttpContext.Current.Request.UserHostAddress.ToString(), accessToken, Convert.ToString(userId), CustID, RealName, NickName, loginNum, OperType, Description, out ErrMsg);
                            strMsg.AppendFormat("【保存Accesstoken】:Result:{0},accessToken:{1},CustID:{2},userId:{3}", Result, accessToken, CustID, Convert.ToString(userId));

                        }
                        else
                        {
                            strMsg.Append("因为CustID为空，导致AccessToken无法记录,可能是绑定失败的原因\r\n");
                        }


                        ReturnUrl = Request["ReturnUrl"];
                        strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
                        strMsg.AppendFormat("Response.Redirect to {0}\r\n", ReturnUrl);
                    }
                    else
                    {
                        strMsg.Append("绑定失败\r\n");
                    }
                }
                else
                {
                    Result = -7766;
                    ErrMsg = "loginNum为空,所以不注册号百客户";
                    strMsg.Append("loginNum为空,所以不注册号百客户\r\n");
                }
                strMsg.AppendFormat("ReturnUrl: {0}\r\n", ReturnUrl);
            }

            //下面删掉一大段
            #endregion
        }
        catch (Exception ex)
        {
            strMsg.AppendFormat(",ErrMsg:{0}", ex.Message);
        }
        finally
        {
            WriteLog(strMsg.ToString());
        }
    }


    private Dictionary<String, String> splitParameters(string paraStr)
    {
        StringBuilder sbLog = new StringBuilder();
        sbLog.AppendFormat("splitParameters:{0}\r\n", paraStr);
        Dictionary<String, String> parameters = new Dictionary<string, string>();

        if (!String.Empty.Equals(paraStr))
        {
            string[] array = paraStr.Trim().Split('&');

            foreach (string temp in array)
            {
                if (!String.Empty.Equals(temp))
                {
                    string ttemp = temp.Trim();
                    int index = ttemp.IndexOf("=");
                    if (index > 0)
                    {
                        String key = ttemp.Substring(0, index);
                        String value = ttemp.Substring(index + 1);
                        if (String.Empty.Equals(key) || String.Empty.Equals(value)) continue;
                        if (!parameters.ContainsKey(key))
                        {
                            parameters.Add(key.Trim(), value.Trim());
                        }
                    }
                }
            }
        }

        return parameters;
    }



    /// <summary>
    /// 页面跳转
    /// </summary>
    protected void Redirect(String argsName, String argsValue)
    {
        if (ReturnUrl.IndexOf('?') > 0)
        {
            ReturnUrl = ReturnUrl + "&" + argsName + "=" + argsValue;
        }
        else
        {
            ReturnUrl = ReturnUrl + "?" + argsName + "=" + argsValue;
        }
        StringBuilder sbLog = new StringBuilder();
        sbLog.AppendFormat("Redirect to URL:{0}\r\n", ReturnUrl);
        WriteLog(sbLog.ToString());
        Response.Redirect(ReturnUrl);
    }

    /// <summary>
    /// 写日志功能
    /// </summary>
    protected void WriteLog(String str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("Open189CallBackClient", msg);
    }
}

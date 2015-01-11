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

public partial class SSO_LoginUDB : System.Web.UI.Page
{
    #region 私有变量
    /// <summary>
    /// UDB系统返回参数
    /// </summary>
    private String PassportLoginResponseValue = String.Empty;
    /// <summary>
    /// UDB返回UDBTicket
    /// </summary>
    private String UDBTicket = String.Empty;
    /// <summary>
    /// 应用系统需要返回的url
    /// </summary>
    private String ReturnUrl = String.Empty;
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

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (CommonUtility.IsParameterExist("PassportLoginResponse", this.Page) && CommonUtility.IsParameterExist("SPID", this.Page))
        {
            //SPID:35433333,PassportLoginResponse:3500000000000001$kmqTK/VaNXGJRVwt377B8R2C2wzWTJX9l8B8zqsZ4WHlRAeyTCeY4SDB2KTG4FNwk4T/baQYgzVDXbsuf2OG3O75n9dKKz0LtiXV3ffdgg6dMqqBcvKHrKHBbzqOZZHccBropdg YK8Gd674RCUaWIyBDHzdcMh3,ErrMsg:未将对象引用设置到对象的实例
            BeginUDBSSO();
        }
        else
        {
            //参数失败
            //Redirect("ErrMsg", "参数无效:PassportLoginResponse和SPID参数不能缺少");
        }
    }

    /// <summary>
    /// 开始UDBSSO功能
    /// </summary>
    protected void BeginUDBSSO()
    {
        StringBuilder strMsg = new StringBuilder();
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        try
        {
            #region 获取参数并验证

            SPID = Request["SPID"];
            String temp_ReturnUrl = Request["ReturnUrl"] == null ? String.Empty : Request["ReturnUrl"];
            PassportLoginResponseValue = Request["PassportLoginResponse"];
            strMsg.AppendFormat("【验证参数，DateTime：{0}】:SPID:{1},PassportLoginResponse:{2},temp_ReturnUrl:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), SPID, PassportLoginResponseValue, temp_ReturnUrl);
            //根据SPID查询应用系统对应的UDBSPID信息
            UDBSPInfoBO _udbspinfo_bo = new UDBSPInfoBO();
            UDBSPInfo _udbspinfo_entity = _udbspinfo_bo.GetBySPID(SPID);
            if (_udbspinfo_entity != null)
            {
                UDBSPID = _udbspinfo_entity.UDBSPID;
                UDBKey = _udbspinfo_entity.UDBKey;
                ReturnUrl = _udbspinfo_entity.RedirectUrl;
            }
            else
            {
                UDBSPID = UDBConstDefinition.DefaultInstance.BesttoneUDBSPID;
                UDBKey = UDBConstDefinition.DefaultInstance.BesttoneUDBKey;
                ReturnUrl = UDBConstDefinition.DefaultInstance.UDBLoginSuccessRedirectUrl;
            }

            if (String.IsNullOrEmpty(ReturnUrl))
            {
                //其他业务系统的Ticket解析页面是不固定的，通过参数ReturnUrl传递
                ReturnUrl = temp_ReturnUrl;
            }
            else
            {
                //针对精品商城，精品商城的Ticket解析页面是固定的，是配在数据库中,而此时参数ReturnUrl及为最终认证成功的跳转页面
                if (!String.IsNullOrEmpty(temp_ReturnUrl))
                {
                    if (ReturnUrl.IndexOf('?') >= 0)
                    {
                        ReturnUrl += "&ReturnUrl=" + HttpUtility.UrlEncode(temp_ReturnUrl);
                    }
                    else
                    {
                        ReturnUrl += "?ReturnUrl=" + HttpUtility.UrlEncode(temp_ReturnUrl);
                    }
                }
            }

            strMsg.AppendFormat(",ReturnUrl:{0}", ReturnUrl);

            //根据客户信息平台的SPID，获取在客户信息平台的key
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

            //解析PassportLoginResponseValue
            String[] tempArray = PassportLoginResponseValue.Split('$');
            DesSsDeviceNo = tempArray[0];
            String tempStr = CryptographyUtil.Decrypt(tempArray[1], UDBKey);
            String[] digestArray = tempStr.Split('$');
            Result = Convert.ToInt32(digestArray[0]);
            UDBTicket = digestArray[1];
            String timeStamp = digestArray[2];
            String digest = digestArray[3];
            
            String newDigest = CryptographyUtil.ToBase64String(CryptographyUtil.Hash(Result + DesSsDeviceNo + UDBTicket + timeStamp));
            strMsg.AppendFormat(",DesSsDeviceNo:{0},Result:{1},UDBTicket{2},timeStamp:{3},digest:{4},newDigest:{5}\r\n", DesSsDeviceNo, Result, UDBTicket, timeStamp, digest, newDigest);
            if (!digest.Equals(newDigest))
            {
                //digest不吻合，失败
                strMsg.AppendFormat(",ErrMsg:{0}", "digest有误不匹配");
                Redirect("ErrMsg", "digest有误不匹配");
            }
            if (Result != 0)
            {
                //失败，则返回
                strMsg.AppendFormat(",ErrMsg:{0}", "返回Ticket失败");
                Redirect("ErrMsg", "返回Ticket失败");
            }

            #endregion

            #region 根据UDBTkcket到UDB查询用户信息

            strMsg.Append("【开始查询信息】:");

            UDBAccountInfo accountInfo = new UDBAccountInfo();

            //根据UDBTicket到UDB查询用户信息
            Result = _UDBMBoss.AccountInfoQuery(UDBSPID, UDBSPID, UDBTicket, UDBKey, out accountInfo, out ErrMsg);
            accountInfo.SourceSPID = UDBConstDefinition.DefaultInstance.UDBSPID;
            strMsg.AppendFormat(",Result:{0},UserID:{1},UserIDType:{2},UserType:{3},PUserID:{4},Alias:{5},UserIDStatus:{6},UserIDSsStatus:{7},Description:{8},ProvinceID:{9},NumFlag:{10}\r\n",
                Result, accountInfo.UserID, accountInfo.UserIDType, accountInfo.UserType, accountInfo.PUserID, accountInfo.Alias, accountInfo.UserIDStatus, accountInfo.UserIDSsStatus, accountInfo.Description, accountInfo.ProvinceID, accountInfo.NumFlag);

            if (Result == 0)
            {
                String CustID, OuterID, Status, CustType, CustLevel, RealName, UserName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
                //检测对应用户是否在号百系统，不在，则注册进来
                strMsg.Append("【开始注册到号百】:");
                Result = UserRegistry.getUserRegistryUDB(accountInfo, out CustID, out ErrMsg);
                strMsg.AppendFormat("Result:{0},CustID:{1}\r\n", Result, CustID);

                //注册成功
                if (Result == 0)
                {
                    Result = CustBasicInfo.getCustInfo(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType, out CustLevel, out RealName,
                        out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID,
                        out AreaID, out RegistrationSource);
                    if (Result != 0)
                    {
                        strMsg.Append(",ErrMsg:客户不存在" + CustID);
                        //客户不存在
                        Redirect("ErrMsg", "客户不存在");
                    }
                    //生成token
                    UserToken UT = new UserToken();
                    String userTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OuterID, CustType, accountInfo.UserID, UDBBusiness.ConvertAuthenType(accountInfo.NumFlag), key, out ErrMsg);
                    String CookieName = ConfigurationManager.AppSettings["CookieName"];
                    PageUtility.SetCookie(CookieName, userTokenValue, this.Page);

                    //生成Ticket
                    String ticket = CommonBizRules.CreateTicket();
                    Result = CIPTicketManager.insertCIPTicket(ticket, SPID, CustID, RealName, UserName, NickName, OuterID, "UDBTicket", accountInfo.UserID, UDBBusiness.ConvertAuthenType(accountInfo.NumFlag), out ErrMsg);
                    strMsg.AppendFormat("【生成ticket】:Result:{0},Ticket:{1}", Result, ticket);
                    if (Result != 0)
                    {
                        strMsg.Append(",ErrMsg:Ticket生成失败" + ticket);
                        Redirect("ErrMsg", "Ticket生成失败");
                    }
                    strMsg.Append(",Message:生成ticket成功，返回业务系统");
                    Redirect("Ticket", ticket);
                }
                else
                {
                    strMsg.Append(",ErrMsg:用户注册到号百失败");
                    Redirect("ErrMsg", "用户注册到号百失败" + ErrMsg);
                }
            }
            else if (Result == 5)
            {
                strMsg.Append(",ErrMsg:用户已删除");
                Redirect("ErrMsg", "用户已删除");
            }
            else
            {
                strMsg.Append(",ErrMsg:查询用户信息失败");
                Redirect("ErrMsg", "查询用户信息失败");
            }

            #endregion
        }
        catch(Exception ex)
        {
            strMsg.AppendFormat(",ErrMsg:{0}", ex.Message);
        }
        finally
        {
            WriteLog(strMsg.ToString());
        }
    }

    /// <summary>
    /// 页面跳转
    /// </summary>
    protected void Redirect(String argsName,String argsValue)
    {
        if (ReturnUrl.IndexOf('?') > 0)
        {
            ReturnUrl = ReturnUrl + "&" + argsName + "=" + argsValue;
        }
        else
        {
            ReturnUrl = ReturnUrl + "?" + argsName + "=" + argsValue;
        }

        if (argsName != "Ticket")
        {
            //PageUtility.SetCookie("ReturnUrl", ReturnUrl);
            Response.Redirect("LogoutUDB.aspx?ReturnUrl=" + HttpUtility.UrlEncode(ReturnUrl));
        }
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
        BTUCenterInterfaceLog.CenterForBizTourLog("LoginUDB", msg);
    }
}

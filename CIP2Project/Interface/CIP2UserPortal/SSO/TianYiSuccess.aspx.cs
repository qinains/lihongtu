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

public partial class SSO_TianYiSuccess : System.Web.UI.Page
{
    public string ReturnUrl="http://118114.cn";
    public String ticket = "";

    #region 私有变量
    /// <summary>
    /// UDB系统返回参数
    /// </summary>
    private string PassportLoginResponseValue = String.Empty;
    /// <summary>
    /// UDB返回UDBTicket
    /// </summary>
    private string UDBTicket = String.Empty;
    /// <summary>
    /// 应用系统需要返回的url
    /// </summary>
    //private String ReturnUrl = String.Empty;
    /// <summary>
    /// 应用系统在UDB的SPID
    /// </summary>
    private string UDBSPID = String.Empty;
    private string UDBKey = String.Empty;
    /// <summary>
    /// 应用系统在客户信息平台的SPID
    /// </summary>
    private string SPID = String.Empty;
    private string key = String.Empty;
    /// <summary>
    /// 返回方设备标识【UDB 系统】
    /// </summary>
    private string DesSsDeviceNo = String.Empty;

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
        //ReturnUrl = Request["ReturnUrl"];
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
            ReturnUrl = Request["ReturnUrl"] == null ? String.Empty : Request["ReturnUrl"];
            PassportLoginResponseValue = Request["PassportLoginResponse"];
            strMsg.AppendFormat("从门户过来【验证参数，DateTime：{0}】:SPID:{1},PassportLoginResponse:{2},ReturnUrl:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), SPID, PassportLoginResponseValue, ReturnUrl);
            UDBKey = System.Configuration.ConfigurationManager.AppSettings["UdbKey"];

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
            //Result = _UDBMBoss.AccountInfoCheck("3500000000408201", "3500000000408201", UDBTicket, UDBKey, out accountInfo, out ErrMsg);
            Result = _UDBMBoss.AccountInfoQuery("3500000000408201", "3500000000408201", UDBTicket, UDBKey, out accountInfo, out ErrMsg);
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


                    //登录tab写入cookie
                    PageUtility.SetCookie("LoginTabCookie", "UDBTab",8760);
                  
                    //生成Ticket
                    ticket = CommonBizRules.CreateTicket();

                    Result = CIPTicketManager.insertCIPTicket(ticket, SPID, CustID, RealName, UserName, NickName, OuterID, "UDBTicket", accountInfo.UserID, UDBBusiness.ConvertAuthenType(accountInfo.NumFlag), out ErrMsg);
                    strMsg.AppendFormat("【生成ticket】:Result:{0},Ticket:{1},ReturnUrl:{2}", Result, ticket, ReturnUrl);
                    if (Result != 0)
                    {
                        strMsg.Append(",ErrMsg:Ticket生成失败" + ticket);
                        Redirect("ErrMsg", "Ticket生成失败");
                    }
                    strMsg.Append(",Message:生成ticket成功，返回业务系统");
                    //后续就重定向到 QuickLogin1 ？ QuickLogin1 是将登陆状态写入cookie的地方
                    //Response.Redirect("QuickLogin1.aspx?LoginTicket=" + ticket + "&ReturnUrl=" + ReturnUrl);  这里放到页面通过js跳转了，因为要区分 window.top.location.href 和 window.location.href ，js写起来比较方便
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
        catch (Exception ex)
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
    protected void Redirect(string argsName, string argsValue)
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
    protected void WriteLog(string str)
    {
        StringBuilder msg = new StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("LoginUDB", msg);
    }
}

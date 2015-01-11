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
public partial class UserCtrl_ValidateCIPToken : System.Web.UI.UserControl
{
    private string CIPDomain = System.Configuration.ConfigurationManager.AppSettings["CIPDomain"];
    private string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
   
    public UserCtrl_ValidateCIPToken()
    {

    } 

    protected void Page_Load(object sender, EventArgs e)
    {
        //Validate();
    }

    /// <summary>
    /// 校验用户是否登录成功
    /// </summary>
    public void Validate()
    {

        if (!PageUtility.IsCookieExist(CookieName, this.Context))
        {
            result = ErrorDefinition.IError_Result_UserAuthorizationFail_Code;
            errMsg = "您尚未登录";
            this.ErrorHappened();
            return;
        }

        string strCIPToken = Request.Cookies.Get(CookieName).Value;

        if (CommonUtility.IsEmpty(strCIPToken))
        {
            result = ErrorDefinition.IError_Result_UserAuthorizationFail_Code;
            errMsg = "您尚未登录.";
            this.ErrorHappened();
            return;
        }

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
        UserToken UT = new UserToken();
        result = UT.ParseUserToken(strCIPToken, key, out custID, out realName, out userName, out nickName, out outerID, out custType, out loginAuthenName,out loginAuthenType,out errMsg);
        //如果验证成功则重新生成Cookie以更新超时时间
        if(result ==0)
        {
            string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OuterID, CustType, loginAuthenName, loginAuthenType, key, out errMsg);

            PageUtility.SetCookie(UserTokenValue, CookieName, this.Page);
        }
    }
    
    /// <summary>
    /// 错误处理
    /// </summary>
    private void ErrorHappened()
    {

        //如果允许跳转则跳转，否则不处理
        if(isRedircet)
        {
            CommonBizRules.ErrorHappenedRedircet(result, errMsg, "您尚未登录，请登录", this.Context);
        }
    }


    #region 成员变量
    private int result = ErrorDefinition.IError_Result_UnknowError_Code;

    public int Result
    {
        get { return result; }
    }

    private string errMsg = "";

    public string ErrMsg
    {
        get { return errMsg; }
    }

    private string custID = "";

    public string CustID
    {
        get { return custID; }
    }

    private string realName = "";

    public string RealName
    {
        get { return realName; }
    }

    private string nickName = "";

    public string NickName
    {
        get { return nickName; }
    }

    private string userName = "";

    public string UserName
    {
        get { return userName; }
    }

    private string outerID = "";

    public string OuterID
    {
        get { return outerID; }
    }

    private string custType = "";

    public string CustType
    {
        get { return custType; }
    }

    private string loginAuthenName = "";

    public string LoginAuthenName
    {
        get { return loginAuthenName; }
    }
    private string loginAuthenType = "";

    public string LoginAuthenType
    {
        get { return loginAuthenType; }
    }

    private bool isRedircet = true;

    public bool IsRedircet
    {
        set { isRedircet = value; }
    }

    
    #endregion

}

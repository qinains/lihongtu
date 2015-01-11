using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

/// <summary>
/// 所有页面基类(对于非必须登录页面不继承此类)
/// </summary>
public class BasePage:System.Web.UI.Page
{
    private string CIPDomain = System.Configuration.ConfigurationManager.AppSettings["CIPDomain"];
    private string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
    private int result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    public BasePage()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        String RealName, NickName, OuterID, CustType, LoginAuthenName, LoginAuthenType;
        HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
        if (cookie == null)
        {
            CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "您尚未登录，请登录", this.Context);
            return;
        }
        string strCIPToken = HttpContext.Current.Request.Cookies.Get(CookieName).Value;

        if (CommonUtility.IsEmpty(strCIPToken))
        {
            CommonBizRules.ErrorHappenedRedircet(result, ErrMsg, "您尚未登录，请登录", this.Context);
            return;
        }

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(HttpContext.Current, "SPData");
        string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
        UserToken UT = new UserToken();
        result = UT.ParseUserToken(strCIPToken, key, out custID, out RealName, out userName, out NickName, out OuterID, out CustType, out LoginAuthenName, out LoginAuthenType, out ErrMsg);
        //如果验证成功则重新生成Cookie以更新超时时间
        if (result == 0)
        {
            string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OuterID, CustType, LoginAuthenName, LoginAuthenType, key, out ErrMsg);

            PageUtility.SetCookie(CookieName, UserTokenValue);
        }
        //this.custID = "117663768";//117663768,26251932
    }

    #region 成员变量

    private string custID = "";
    public string CustID
    {
        get { return custID; }
    }

    private string userName = "";
    public string UserName
    {
        get { return userName; }
    }

    #endregion
}

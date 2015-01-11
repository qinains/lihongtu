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
using BTUCenter.Proxy;
using System.Text;
using System.Text.RegularExpressions;

using System.Net;
using System.IO;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Security.Cryptography;


public partial class SSO_intensive_IntensiveLogin : System.Web.UI.Page
{
    // 积分商城集约运营の集团积分商城SSO到省积分商城 Author:李宏图 CreateTime:2013-01-23
    string ProvinceID = "";
    string SPID = "35999991";
    protected void Page_Load(object sender, EventArgs e)
    {
        string CookieName = "";
        string token = "";
        string CustID = "";
        string RealName = "";
        string UserName = "";
        string NickName = "";
        string OuterID = "";
        string CustType = "";
        string AuthenName = "";
        string AuthenType = "";
        string RedirectUrl = "";
        string key = "";
        string ErrMsg = "";
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        if (!this.IsPostBack)
        {
            CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            token = PageUtility.GetCookie(CookieName);
            if (String.IsNullOrEmpty(token))
            {
                Response.Redirect("../../ErrorInfo.aspx?Result=-19999&ErrorInfo=请先登录积分商城", true);
                return;
            }
           
            UserToken UT = new UserToken();
            key = System.Configuration.ConfigurationManager.AppSettings["ScoreSystemSecret"];
            Result = UT.ParseScoreUserToken(token, key, out ProvinceID, out CustID, out RealName, out UserName, out NickName, out OuterID, out CustType, out AuthenName, out AuthenType, out ErrMsg);
            if (Result == 0)
            {
                string par_ProvinceID = Request["ProvinceID"];
                if (par_ProvinceID.Equals(ProvinceID))
                {
                    String Ticket = CommonBizRules.CreateTicket();
                    Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, NickName, UserName, OuterID, "", AuthenName, AuthenType, out ErrMsg);

                    if (Result != 0)
                    {
                        Response.Redirect("../../ErrorInfo.aspx?Result=-19999&ErrorInfo=票据生成失败,请重新登录积分商城", true);
                        return;   // 重定向至哪里
                    }

                    // 根据ProvinceID 查出 Redirecturl

                    Result = CIP2BizRules.GetRedirectUrlByProvince(ProvinceID,out RedirectUrl, out ErrMsg);
                    if (Result == 0)
                    {
                        if (!String.IsNullOrEmpty(RedirectUrl))
                        {
                            if (RedirectUrl.IndexOf("?") > 0)
                            {
                                RedirectUrl = RedirectUrl + "&Ticket=" + Ticket;
                            }
                            else
                            {
                                RedirectUrl = RedirectUrl + "?Ticket=" + Ticket;
                            }
                        }
                        else { 
                            // 根据provinvce 获取redirecturl 为空 ,该转向哪里
                            Response.Redirect("../../ErrorInfo.aspx?Result=-19999&ErrorInfo=未配置该省的返回地址", true);
                        }

                    }
                    else { 
                        // 根据province获得url失败,该返回哪里?
                        Response.Redirect("../../ErrorInfo.aspx?Result=-19999&ErrorInfo=根据省ID获取返回地址失败", true);
                    }
                    Response.Redirect(RedirectUrl, true);
                }
                else
                {
                    Response.Redirect("../../ErrorInfo.aspx?Result=-19999&ErrorInfo=省ID不匹配当前token中的省ID", true);
                }
            }
            else { // token  存在但是解析失败
                RedirectUrl = Request.Url.AbsoluteUri;
                Response.Redirect(RedirectUrl, true);
            }
        }
    }


    protected void Query_Assertion()
    {

        MBOSSClass mboss = new MBOSSClass();

        SPInfoManager spInfo = new SPInfoManager();
        
        int Result = ErrorDefinition.IError_Result_UnknowError_Code;
        string  ErrMsg = "";
        string UserName = "";
        string privateKeyPassword = "";
        string CAP01002_XML = Request["SSORequestXML"];
        string DigitalSign = MBOSSClass.GetNewXML(CAP01002_XML, "DigitalSign");
        string DigitalSignValue = MBOSSClass.GetValueFromXML(CAP01002_XML, "DigitalSign");
        //从中取出RedirectURL
        string RedirectURL = MBOSSClass.GetValueFromXML(CAP01002_XML, "RedirectURL");
        //验证 CAP01002_XM 合法性
        byte[] PublicKeyFile = new byte[0];

        try
        {
            Object SPData = spInfo.GetSPData(this.Context, "");  //SPDataCacheName 这里要去问tongbo
            PublicKeyFile = spInfo.GetCAInfo(SPID, 0, SPData, out UserName, out privateKeyPassword);
        }
        catch (Exception err)
        {
            //验证签名未通过
            ErrMsg = err.Message;
            Result = -20001;
            Response.Redirect(RedirectURL, true);
            return;
        }

        Result = mboss.VerifySignByPublicKey(DigitalSign, PublicKeyFile, DigitalSignValue, out ErrMsg);
        if (Result != 0)
        {
            // 签名校验未通过,直接将请求原路打回 
            Response.Redirect(RedirectURL, true);
            return;
        }

    }

}

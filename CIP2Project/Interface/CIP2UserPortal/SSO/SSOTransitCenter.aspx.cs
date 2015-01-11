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
using System.Text;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

public partial class SSO_SSOTransitCenter : System.Web.UI.Page
{


    public string SPTokenRequest = "";
    public string SPID = "";
    public string UAProvinceID = "";
    public string SourceType = "";
    public string ReturnURL = "";


    private Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    private String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;

    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder strLog = new StringBuilder();
        try
        {
            strLog.Append("1.开始解析SPTokenRequest\r\n");
            ParseSPTokenRequest();
            strLog.Append("2.解析SPTokenRequest结束\r\n");
            strLog.Append("3.校验全局Token\r\n");
            this.TokenValidate.IsRedircet = false;
            this.TokenValidate.Validate();
            strLog.Append("4.校验全局Token结束\r\n");
            //log("TokenValidate:" + TokenValidate.Result+"\r\n");
            if (TokenValidate.Result == 0)
            {
                strLog.Append("全局token存在\r\n");
                this.ssoFunc();
            }
            else
            {
                strLog.Append("全局token不存在\r\n");

                string Ticket = "179000179000";
                string Url = "";

                if (ReturnURL.IndexOf("?") > 0)
                {
                    strLog.Append("ReturnUrl包含问号\r\n");
                    Url = ReturnURL + "&Ticket=" + Ticket;
                }
                else
                {
                    strLog.Append("ReturnUrl不包含问号\r\n");
                    Url = ReturnURL + "?Ticket=" + Ticket;
                }
                strLog.Append("重定向\r\n");
                Response.Redirect(Url,false);
            }
        }
        catch (System.Exception  ex)
        {
            strLog.Append(ex.ToString());
            CommonBizRules.ErrorHappenedRedircet(Result, ErrMsg + "异常信息:" + ex.ToString(), "您尚未登录，请登录", this.Context);
        }
        finally
        {
            log(strLog.ToString());
        }
    }


    protected void ssoFunc()
    {
        string Url = "";
        try
        {
            string Ticket = CommonBizRules.CreateTicket();

            string CustID = TokenValidate.CustID;
            string RealName = TokenValidate.RealName;
            string NickName = TokenValidate.NickName;
            string UserName = TokenValidate.UserName;
            string OutID = TokenValidate.OuterID;
            string LoginAuthenName = TokenValidate.LoginAuthenName;
            string LoginAuthenType = TokenValidate.LoginAuthenType;
            log(String.Format("ssoFunc: TokenValidate.RealName:{0},TokenValidate.NickName:{1},TokenValidate.UserName:{2},TokenValidate.LoginAuthenName:{3},TokenValidate.LoginAuthenType:{4}", TokenValidate.RealName, TokenValidate.NickName, TokenValidate.UserName, TokenValidate.LoginAuthenName, TokenValidate.LoginAuthenType));
            String er = "";
            Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, UserName, NickName, OutID, "", LoginAuthenName, LoginAuthenType, out er);

            if (Result != 0)
            {
                Response.Redirect(ReturnURL,false);
                //return;
            }

            if (ReturnURL.IndexOf("?") > 0)
            {
                Url = ReturnURL + "&Ticket=" + Ticket;
            }
            else
            {
                Url = ReturnURL + "?Ticket=" + Ticket;
            }
            Response.Redirect(Url,false);
        }

        catch (Exception e)
        {
            log(e.ToString());
        }
    }


    /// <summary>
    /// 判断并解析SPTokenRequest参数
    /// </summary>
    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        try
        {

            if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
            {
                SPTokenRequest = Request["SPTokenRequest"];
                //日志
                strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
                //解析请求参数
                Result = SSOClass.ParseLoginRequest(SPTokenRequest, this.Context, out SPID, out UAProvinceID, out SourceType, out ReturnURL, out ErrMsg);
                //日志
                strLog.AppendFormat(String.Format("【解析参数结果】:Result:{0},ErrMsg:{1},SPID:{2},ProvinceID:{3},SourceType:{4},ReturnURL:{5}\r\n", Result, ErrMsg, SPID, UAProvinceID, SourceType, ReturnURL));

          
            }
        }
        catch (System.Exception ex)
        {
            strLog.AppendFormat(ex.ToString());
        }
        finally
        {
            log(strLog.ToString());
        }

    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("SSOTransitCenter", msg);
    }

}

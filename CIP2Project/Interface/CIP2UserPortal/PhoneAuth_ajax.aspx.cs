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

public partial class PhoneAuth_ajax : System.Web.UI.Page
{
    public int Result;

    protected void Page_Load(object sender, EventArgs e)
    {
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["typeId"].ToString().Equals("1") && !String.IsNullOrEmpty(SPID) )
            {
                //判断手机是否被绑定
                Result = PhoneAuth();
                if (Result != 0)
                {
                    Response.Write(Result.ToString());
                }
                else
                {
                    //发送验证码
                    SendCode();
                    Result = 0;
                    Response.Write(Result.ToString());
                }

            }
        }
    }

    public int PhoneAuth()
    {
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        if (String.IsNullOrEmpty(SPID))
        {
            return -1;
        }
        String ErrorDescription = "";
        int Result = CommonBizRules.SPInterfaceGrant(SPID, "SendSMSCode", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            return -2;
        }

        string ErrMsg="";
        string PhoneNum = HttpUtility.HtmlDecode(Request.QueryString["PhoneNum"].ToString());
        
        //int Result = PhoneBO.PhoneSel("", "", PhoneNum, SPID, out ErrMsg);
        //int Result = 0;
        Result = PhoneBO.PhoneSel("", PhoneNum, out ErrMsg);
        return Result;
    }

    public void SendCode()
    {
        Random random = new Random();
        string AuthenCode = random.Next(111111, 999999).ToString();
        string PhoneNum = HttpUtility.HtmlDecode(Request.QueryString["PhoneNum"].ToString());
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        string msg = "";
        if (String.IsNullOrEmpty(SPID))
        {
            return;
        }
        string ErrorDescription = "";
        int Result = CommonBizRules.SPInterfaceGrant(SPID, "SendSMSCode", this.Context, out ErrorDescription);
        if (Result != 0)
        {
            return;
        }

        Result = PhoneBO.InsertPhoneSendMassage("", "", AuthenCode, PhoneNum, DateTime.Now, "", 1, 0, "1", out msg);
        //CommonBizRules.SendMessage(PhoneNum, "您的验证码是" + AuthenCode, SPID);
        CommonBizRules.SendMessageV3(PhoneNum, "您的验证码是" + AuthenCode, SPID);
    }
}

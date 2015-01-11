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
public partial class PutAuthenCodeForOpenAccount : System.Web.UI.Page
{
    public int Result;
    public int k;
    protected void Page_Load(object sender, EventArgs e)
    {
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["typeId"].ToString().Equals("1") && !String.IsNullOrEmpty(SPID))
            {
                //发送验证码
                SendCode();
                Result = 0;
                Response.Write(Result.ToString());


            }
        }
    }


    public void SendCode()
    {
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        if (String.IsNullOrEmpty(SPID))
        {
            return;
        }
        String ErrorDescription = "";


        //int Result = CommonBizRules.SPInterfaceGrant(SPID, "SendSMSCode", this.Context, out ErrorDescription);
        int Result = PhoneBO.SPInterfaceGrant(SPID, "SendSMSCode", out ErrorDescription);
        if (Result != 0)
        {
            return;
        }
        Random random = new Random();
        string AuthenCode = random.Next(111111, 999999).ToString();
        string PhoneNum = HttpUtility.HtmlDecode(Request.QueryString["PhoneNum"].ToString());
        string msg = "";
        k = PhoneBO.PhoneSel("", PhoneNum, out msg);    // 验证电话是否可以做认证电话(这里的电话包括手机和电话)
        if (k == 0)
        {
            Result = PhoneBO.InsertPhoneSendMassage("", "欢迎开通号码百事通账户，验证码为" + AuthenCode + "，有效期2分钟。", AuthenCode, PhoneNum, DateTime.Now, "", 1, 0, "1", out msg);
            //CommonBizRules.SendMessageV3(PhoneNum, "欢迎开通号码百事通账户，验证码为" + AuthenCode + "，有效期2分钟。", SPID);
            Response.Write(k);
        }
        else 
        {
            Response.Write(k);
        }

   
    }
}

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
using log4net;
using Linkage.BestTone.Interface.Rule;
public partial class PutAuthenCodeForRegisterAllInOne : System.Web.UI.Page
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(PutAuthenCodeForRegisterAllInOne));
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
            }
        }
    }

    public static String CreateTransactionID()
    {
        String date = DateTime.Now.ToString("yyyyMMddHHmmss");
        //6位随机数
        Random r = new Random(Guid.NewGuid().GetHashCode());
        String TransactionID = "00" + date + r.Next(1000, 9999).ToString();
        return TransactionID;
    }


    public void SendCode()
    {
        int ajaxcode = -1;
        string SPID = HttpUtility.HtmlDecode(Request.QueryString["SPID"].ToString());
        if (String.IsNullOrEmpty(SPID))
        {
            return;
        }
        String ErrorDescription = "";

        int count = Convert.ToInt32(HttpUtility.HtmlDecode(Request.QueryString["count"].ToString())); // 发送次数
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
        DateTime DealTime = DateTime.Now;
        //Result = PhoneBO.PhoneSel(CustID, Phone, out ErrMsg);    // 验证电话是否可以做认证电话(这里的电话包括手机和电话)


        k = PhoneBO.PhoneSel("", PhoneNum, out msg);    // 验证电话是否可以做认证电话(这里的电话包括手机和电话)
        if (k == 0)
        {
            CommonBizRules.SendMessageV3(PhoneNum, "欢迎注册号码百事通会员，验证码为" + AuthenCode + "，有效期2分钟。", SPID);
            Result = PhoneBO.InsertPhoneSendMassage("", "欢迎注册号码百事通会员，验证码为" + AuthenCode + "，有效期2分钟。", AuthenCode, PhoneNum, DateTime.Now, "", count, 0, "1", out msg);
            logger.Info(PhoneNum + "<->" + AuthenCode);
            Response.Write(k);
        }
        else
        {
            logger.Info(PhoneNum + ":" + msg);
            Response.Write(k);
        }
    }
}

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
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Data.SqlClient;
using log4net;


public partial class UserAccount_GetCheckCodeForResetPayPassword : System.Web.UI.Page
{
    public String SPID;
    public String CustID;
    public String Phone;
    public Int32 Result;
    public String ErrMsg;
    public String wt;


    public String GetAuthenCode(String SPID, String CustID,String wt)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        #region
        if (CommonUtility.IsEmpty(SPID))
        {

            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(CustID))
        {

            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

 

        String Phone = String.Empty;
        BesttoneAccount account = null;
        BesttoneAccountDAO dao = new BesttoneAccountDAO();
        account = dao.QueryByCustID(CustID);
        if (account != null)
        {
            Phone = account.BestPayAccount;
        }
        else
        {
            //未开户
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "该CustID尚未开户！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "该CustID尚未开户！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        #endregion


        try
        {
            //String CustID = PhoneBO.IsAuthenPhone(Phone, SPID, out ErrMsg);

            Result = CommonBizRules.SPInterfaceGrant(SPID, "GetAuthenCode", this.Context, out ErrMsg);
            if (Result != 0)
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "991");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "调用下行短信接口权限未开通！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "991");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "调用下行短信接口权限未开通！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();

            }

            Random random = new Random();
            String AuthenCode = random.Next(111111, 999999).ToString();

            DateTime DealTime = DateTime.Now;
            int k = PhoneBO.PhoneSelV2("", Phone, out ErrMsg);    // 检查发送验证码次数
            if (k == 0)
            {
                CommonBizRules.SendMessageV3(Phone, "您正在通过翼购客户端找回支付密码，验证码为：" + AuthenCode + "，有效期2分钟。", SPID);
                Result = PhoneBO.InsertPhoneSendMassage(CustID, "您正在通过翼购客户端找回支付密码，验证码为：" + AuthenCode + "，有效期2分钟。", AuthenCode, Phone, DateTime.Now, "客户端找回支付密码", 1, 0, "1", out ErrMsg);

            }
            else
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "992");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "该手机号码已经被注册过了！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "992");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "该手机号码已经被注册过了！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }


            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "您的验证码是:" + AuthenCode);
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "您的验证码是:" + AuthenCode);
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();


        }
        catch (Exception exp)
        {
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-25367");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "异常:" + exp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-25367");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "异常:" + exp.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        return ResponseMsg.ToString();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        CustID = Request["CustID"];
        wt = Request["wt"];
        String ResponseText = GetAuthenCode(SPID, CustID,wt);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }
}

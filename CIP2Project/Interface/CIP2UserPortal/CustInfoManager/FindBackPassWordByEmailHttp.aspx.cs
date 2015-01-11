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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using System.Text;
public partial class CustInfoManager_FindBackPassWordByEmailHttp : System.Web.UI.Page
{

    public String SPID;
    public String Email;
    public String wt;

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        Email = Request["Email"];
        wt = Request["wt"];
        String ResponseText = FindPwdByEmail(SPID,Email);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }

    /// <summary>
    /// 通过邮箱找回密码
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public String FindPwdByEmail(String SPID,String Email)
    {
        StringBuilder ResponseMsg = new StringBuilder();
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        String CustID = String.Empty;

        if (CommonUtility.IsEmpty(SPID))
        {
            // 返回错误信息
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


        if (CommonUtility.IsEmpty(Email))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "Email不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "Email不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        try
        {
            Result = SetMail.EmailSel(Email, out CustID, out ErrMsg);
            if (Result != 0)
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "认证邮箱有误！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "认证邮箱有误！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
            else
            {
                string encryptPwd = "";
                string Pwd = "";
                Result = FindPwd.SelPwdByEmailV2(Email, out encryptPwd, out ErrMsg);
                if (Result == 0)
                {
                    Pwd = CryptographyUtil.Decrypt(encryptPwd);
                    Result = SetMail.InsertEmailSendMassage(CustID, "2", "您的密码是:" + Pwd, "", 1, Email, DateTime.Now, "找回密码", "中国电信号码百事通：找回密码", 0, out ErrMsg);
                    if (Result == 0)
                    {
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "邮件发送成功！");
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                            ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "邮件发送成功！");
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                        return ResponseMsg.ToString();
                    }
                    else
                    {
                        ResponseMsg.Length = 0;
                        if ("json".Equals(wt))
                        {
                            ResponseMsg.Append("{");
                            ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-930");
                            ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ErrMsg);
                            ResponseMsg.Append("}");
                        }
                        else
                        {
                            ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                            ResponseMsg.Append("<PayPlatRequestParameter>");
                            ResponseMsg.Append("<PARAMETERS>");
                            ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-930");
                            ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ErrMsg);
                            ResponseMsg.Append("</PARAMETERS>");
                            ResponseMsg.Append("</PayPlatRequestParameter>");
                        }
                        return ResponseMsg.ToString();
                    }

                }
                else
                {
                    //密码找回失败
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-940");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ErrMsg);
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-940");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ErrMsg);
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                }
            }
        }
        catch (Exception exp)
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-950");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", exp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-950");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", exp.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        return ResponseMsg.ToString();
    }
    


}

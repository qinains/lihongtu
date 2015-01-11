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

public partial class CustInfoManager_GetAuthenCode : System.Web.UI.Page
{
    public String SPID;
    public String Phone;
    public Int32 Result;
    public String ErrMsg;
    public String wt;


    public String GetAuthenCode(String SPID,String Phone)
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

        if (CommonUtility.IsEmpty(Phone))
        {
            
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "Phone不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "Phone不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }     


        #endregion


        try
        {
            String CustID = PhoneBO.IsAuthenPhone(Phone, SPID, out ErrMsg);
            if (String.IsNullOrEmpty(CustID))
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "验证手机有误！手机未注册");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "验证手机有误！手机未注册");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();


            }

            //Result = CommonBizRules.SPInterfaceGrant(SPID, "GetAuthenCode", this.Context, out ErrMsg);
            Result = PhoneBO.SPInterfaceGrant(SPID, "GetAuthenCode", out ErrMsg);
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

            Random r = new Random();
            String AuthenCode = "";
            AuthenCode += r.Next(100000, 999999).ToString();
            int k = PhoneBO.PhoneSelV2("", Phone, out ErrMsg);
            if (k == 0)
            {
                CommonBizRules.SendMessageV3(Phone, "您的验证码是:" + AuthenCode, SPID);
                Result = PhoneBO.InsertPhoneSendMassage(CustID, "找回密码时下发的验证码信息内容", AuthenCode, Phone, DateTime.Now, "描述未知", 1, 0, "1", out ErrMsg);

            }
            if (Result == 0)
            {
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
            else
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-1002");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "发送验证码失败！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-1002");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "发送验证码失败！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }

           

        }catch(Exception exp)
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
        Phone = Request["Phone"];
        wt = Request["wt"];
        String ResponseText = GetAuthenCode(SPID, Phone);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }
    
}

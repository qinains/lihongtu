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

public partial class CustInfoManager_FindBackPassWordByMobile : System.Web.UI.Page
{

    public String SPID;
    public String CustID;
    public String Phone;
    public String AuthenCode;
    public String PassWord;
    public Int32 Result;
    public String ErrMsg;
    public String wt;


    public String FindBackPassWordByMobile(String SPID, String Phone, String AuthenCode, String PassWord)
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


        //if (CommonUtility.IsEmpty(CustID))
        //{

        //    ResponseMsg.Length = 0;
        //    if ("json".Equals(wt))
        //    {
        //        ResponseMsg.Append("{");
        //        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
        //        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
        //        ResponseMsg.Append("}");
        //    }
        //    else
        //    {
        //        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //        ResponseMsg.Append("<PayPlatRequestParameter>");
        //        ResponseMsg.Append("<PARAMETERS>");
        //        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
        //        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
        //        ResponseMsg.Append("</PARAMETERS>");
        //        ResponseMsg.Append("</PayPlatRequestParameter>");
        //    }
        //    return ResponseMsg.ToString();
        //}     

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


        if (CommonUtility.IsEmpty(AuthenCode))
        {

            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AuthenCode不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AuthenCode不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (String.IsNullOrEmpty(PassWord))
        {
            PassWord = "111111";
        }

        #endregion
        try
        {
            String t_custid = PhoneBO.IsAuthenPhone(Phone, SPID, out ErrMsg);
            if (String.IsNullOrEmpty(t_custid))
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

            //if (!CustID.Equals(t_custid))
            //{

            //    ResponseMsg.Length = 0;
            //    if ("json".Equals(wt))
            //    {
            //        ResponseMsg.Append("{");
            //        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "993");
            //        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "客户CustID和验证手机有误");
            //        ResponseMsg.Append("}");
            //    }
            //    else
            //    {
            //        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            //        ResponseMsg.Append("<PayPlatRequestParameter>");
            //        ResponseMsg.Append("<PARAMETERS>");
            //        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "993");
            //        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "客户CustID和验证手机有误");
            //        ResponseMsg.Append("</PARAMETERS>");
            //        ResponseMsg.Append("</PayPlatRequestParameter>");
            //    }
            //    return ResponseMsg.ToString();
            //}

            //验证码校验
            Result = PhoneBO.SelSendSMSMassage(t_custid, Phone, AuthenCode, out ErrMsg);
            if (Result != 0)
            {
                //Result.ErrMsg = "验证码验证失败：" + Result.ErrMsg;
                //return Result;

                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "992");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "验证码验证失败");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "992");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "验证码验证失败");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }

            //修改密码
            Result = PassWordBO.SetPassword(SPID, t_custid, PassWord, "2", "", out ErrMsg);
            ResponseMsg.Length = 0;
            if (Result == 0)
            {
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码重置成功！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码重置成功！");
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
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-2508");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码重置失败");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-2508");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码重置失败");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }

        }
        catch (Exception exp)
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-2508");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码重置失败"+exp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-2508");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码重置失败"+exp.ToString());
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
        //CustID = Request["CustID"];
        Phone = Request["Phone"];
        AuthenCode = Request["AuthenCode"];
        PassWord = Request["PassWord"];
        wt = Request["wt"];
        String ResponseText = FindBackPassWordByMobile(SPID,  Phone, AuthenCode, PassWord);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }
}

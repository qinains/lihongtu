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

public partial class CustInfoManager_ModifyLoginPassword : System.Web.UI.Page
{

    public String SPID;
    public String CustID;
    public String OldPwd;
    public String NewPwd;
    public Int32 Result;
    public String ErrMsg;
    public String wt;

    public String ModifyLoginPassword(String SPID, String CustID, String OldPwd, String NewPwd)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;

        #region 数据校验

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
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(OldPwd))
        {

            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "OldPwd不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "OldPwd不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(NewPwd))
        {

            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "NewPwd不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "NewPwd不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        #endregion

        #region
        //验证旧密码
        bool IsRight = PassWordBO.OldPwdIsRight(CustID, OldPwd, "2", out ErrMsg);
        if (IsRight)
        {
            Result = PassWordBO.SetPassword(SPID, CustID, NewPwd, "2", "", out ErrMsg);
            if (Result != 0)
            {
                Result = -22500;
                ErrMsg = "密码修改失败";

                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-22500");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码修改失败！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-22500");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码修改失败！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();

            }
            else
            {
                Result = 0;
                ErrMsg = "密码修改成功";
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "密码修改成功！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "密码修改成功！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();

            }

            
        }
        else
        {
            Result = -20504;
            ErrMsg = "原始密码不匹配";
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-20504");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "原始密码不匹配！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-20504");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "原始密码不匹配！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();

        }
        #endregion

        return ResponseMsg.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        CustID = Request["CustID"];
        OldPwd = Request["OldPwd"];
        NewPwd = Request["NewPwd"];
        wt = Request["wt"];
        String ResponseText = ModifyLoginPassword(SPID, CustID, OldPwd, NewPwd);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }
}

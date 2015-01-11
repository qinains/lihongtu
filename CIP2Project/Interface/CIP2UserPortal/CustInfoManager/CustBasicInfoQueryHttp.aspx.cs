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

public partial class CustInfoManager_CustBasicInfoQueryHttp : System.Web.UI.Page
{
    public String SPID;
    public String CustID;
    public String wt;
    public Int32 Result;
    public String ErrMsg;


    public String QueryCustBasicInfo(String SPID, String CustID)
    {
        StringBuilder ResponseMsg = new StringBuilder();

        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;

        #region 数据校验

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

        if (CommonUtility.IsEmpty(CustID))
        {
           
            // 返回错误信息
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

        #endregion


        #region  查询
        String OuterID = String.Empty;
        String Status = String.Empty;
        String CustType = String.Empty;
        String CustLevel = String.Empty;
        String RealName = String.Empty;
        String UserName = String.Empty;
        String NickName = String.Empty;
        String CertificateCode = String.Empty;
        String CertificateType = String.Empty;
        String Sex = String.Empty;
        String Email = String.Empty;
        String EnterpriseID = String.Empty;
        String ProvinceID = String.Empty;
        String AreaID = String.Empty;
        String Registration = String.Empty;
        

        //扩展字段 
    
        String Birthday = String.Empty;
        String EduLevel = String.Empty;
        String IncomeLevel = String.Empty;
        String Favorite = String.Empty;
        String Address = String.Empty;
        StringBuilder strLog = new StringBuilder();
        try
        {
            Result = CustBasicInfo.GetCustInfoWithExtendField(SPID, CustID, out ErrMsg, out OuterID, out Status, out CustType,
                                                out CustLevel, out RealName, out UserName, out NickName, out CertificateCode,
                                                out CertificateType, out Sex, out Email, out EnterpriseID, out ProvinceID, out AreaID, out Registration,
                                                out Birthday, out EduLevel, out IncomeLevel, out Favorite, out Address);

            int QueryPhoneResult = -1;
            String QueryPhoneErrMSG = String.Empty;
            PhoneRecord[] PhoneRecords = null;
            String Mobile = String.Empty;
            String PhoneClass = String.Empty;


            if (Result == 0)
            {
                PhoneRecords = CustBasicInfo.getPhoneRecord(CustID, out QueryPhoneResult, out QueryPhoneErrMSG);
                Result = QueryPhoneResult;
                ErrMsg = QueryPhoneErrMSG;
            }

            ResponseMsg.Length = 0;
            if (Result == 0)
            {
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\",", ErrMsg);
                    ResponseMsg.AppendFormat("\"CustID\":\"{0}\",", CustID);
                    ResponseMsg.AppendFormat("\"Status\":\"{0}\",", Status);
                    ResponseMsg.AppendFormat("\"CustType\":\"{0}\",", CustType);
                    ResponseMsg.AppendFormat("\"CustLevel\":\"{0}\",", CustLevel);
                    ResponseMsg.AppendFormat("\"RealName\":\"{0}\",", RealName);
                    ResponseMsg.AppendFormat("\"UserName\":\"{0}\",", UserName);
                    ResponseMsg.AppendFormat("\"NickName\":\"{0}\",", NickName);
                    ResponseMsg.AppendFormat("\"CertificateCode\":\"{0}\",", CertificateCode);
                    ResponseMsg.AppendFormat("\"CertificateType\":\"{0}\",", CertificateType);
                    ResponseMsg.AppendFormat("\"Sex\":\"{0}\",", Sex);
                    ResponseMsg.AppendFormat("\"Email\":\"{0}\",", Email);
                    ResponseMsg.AppendFormat("\"EnterpriseID\":\"{0}\",", EnterpriseID);
                    ResponseMsg.AppendFormat("\"ProvinceID\":\"{0}\",", ProvinceID);
                    ResponseMsg.AppendFormat("\"AreaID\":\"{0}\",", AreaID);
                    if (QueryPhoneResult == 0 && PhoneRecords != null && PhoneRecords.Length > 0)
                    {
                        ResponseMsg.AppendFormat("\"Phone\":\"{0}\",", PhoneRecords[0].Phone);
                        ResponseMsg.AppendFormat("\"PhoneClass\":\"{0}\",", PhoneRecords[0].PhoneClass);
                    }
                    ResponseMsg.AppendFormat("\"Birthday\":\"{0}\",", Birthday);
                    ResponseMsg.AppendFormat("\"EduLevel\":\"{0}\",", EduLevel);
                    ResponseMsg.AppendFormat("\"IncomeLevel\":\"{0}\",", IncomeLevel);
                    ResponseMsg.AppendFormat("\"Favorite\":\"{0}\",", Favorite);
                    ResponseMsg.AppendFormat("\"Address\":\"{0}\",", Address);
                    ResponseMsg.AppendFormat("\"Registration\":\"{0}\"", Registration);
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");

                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
            }
            else
            {
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", Result);
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ErrMsg);
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", Result);
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ErrMsg);
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
            }
        }
        catch (Exception exp)
        {
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-2508");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", exp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-2508");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", exp.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
        }
        finally
        {
            //log(strLog.ToString());
        }



        #endregion
        return ResponseMsg.ToString();
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("CustBasicInfoQueryHttp", msg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SPID = Request["SPID"];
        CustID = Request["CustID"];

        wt = Request["wt"];   // json or xml

        String ResponseText = QueryCustBasicInfo(SPID, CustID);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }



}

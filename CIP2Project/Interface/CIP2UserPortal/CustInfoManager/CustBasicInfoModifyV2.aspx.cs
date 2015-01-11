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

public partial class CustInfoManager_CustBasicInfoModifyV2 : System.Web.UI.Page
{
    public String SPID;
    public String CustID;
    public String RealName;
    public String CertificateCode;
    public String CertificateType;
    public String Sex;
    public String Email;
    public String NickName;
    public String Birthday;
    public String EducationLevel;
    public String IncomeLevel;
    public String ProvinceID;
    public String AreaID;
    public String Address;
    public String Favorite;
    public String wt;

    public String HeadPic;


    public Int32 Result;
    public String ErrMsg;





    public String ModifyCustBasicInfo(string SPID, string CustID, string RealName, string NickName, string CertificateCode, string CertificateType, string Sex, string Email,string Birthday, string EducationLevel,string IncomeLevel,string ProvinceID,string AreaID,string Address,string Favorite, string HeadPic)
    {
        StringBuilder ResponseMsg = new StringBuilder();
        #region 数据合法性判断
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


        if (CommonUtility.IsEmpty(RealName)
            && CommonUtility.IsEmpty(CertificateCode)
            && CommonUtility.IsEmpty(CertificateType)
            && CommonUtility.IsEmpty(Sex))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改信息参数不能全为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改信息参数不能全为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }



        if (!CommonUtility.IsEmpty(EducationLevel))
        {
            if (EducationLevel.Equals("1") || EducationLevel.Equals("2") || EducationLevel.Equals("3") || EducationLevel.Equals("4") || EducationLevel.Equals("5") || EducationLevel.Equals("6"))
            {

            }
            else {
                // 返回错误信息
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "教育水平能1-6！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "教育水平能1-6！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
        }

        if (!CommonUtility.IsEmpty(IncomeLevel))
        {
            if (IncomeLevel.Equals("1") || IncomeLevel.Equals("2") || IncomeLevel.Equals("3") || IncomeLevel.Equals("4") || IncomeLevel.Equals("5") )
            {

            }
            else
            {
                // 返回错误信息
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "收入水平能1-5！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "收入水平能1-5！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
        }


        if (!CommonUtility.IsEmpty(CertificateType))
        {
            if ( CertificateType.Equals("0") || CertificateType.Equals("1") || CertificateType.Equals("2") || CertificateType.Equals("3") || CertificateType.Equals("4") || CertificateType.Equals("5") || CertificateType.Equals("6") || CertificateType.Equals("7") || CertificateType.Equals("8") || CertificateType.Equals("9"))
            {

            }
            else
            {
                // 返回错误信息
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "证件类型只能0-9！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "999");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "证件类型只能0-9！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
        }

        // 省代码 地市代码校验

        #endregion
        try
        {
            Result = CustBasicInfo.UpdateCustinfo(SPID, CustID, RealName, CertificateCode, CertificateType, Sex, Email, out ErrMsg);
            if (Result == 0)
            {
                //通知积分平台
                CIP2BizRules.InsertCustInfoNotify(CustID, "2", SPID, "", "0", out ErrMsg);
                // 这里要判断该客户是否已经开过户，如果是开户的，并且修改了身份证的才同步
                //CIP2BizRules.NotifyBesttoneAccountInfo(SPID, CustID, out ErrMsg);   // 通知融合支付

                // 返回错误信息
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改信息成功！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改信息成功！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();

            }
            else
            {
                // 返回错误信息
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-989");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改信息失败！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-989");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改信息失败！");
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
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-989");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", exp.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-989");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", exp.ToString());
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
        RealName = Request["RealName"];
        CustID = Request["CustID"];
        CertificateCode = Request["CertificateCode"];
        CertificateType = Request["CertificateType"];    // 0 身份证
        Sex = Request["Sex"];
        Email = Request["Email"];
        
        NickName = Request["NickName"];
        Birthday = Request["Birthday"];
        EducationLevel = Request["EducationLevel"];
        IncomeLevel = Request["IncomeLevel"];
        ProvinceID = Request["ProvinceID"];
        AreaID = Request["AreaID"];
        Address = Request["Address"];
        Favorite = Request["Favorite"];

        HeadPic = Request["HeadPic"];
        wt = Request["wt"];

        String ResponseText = ModifyCustBasicInfo(SPID, CustID, RealName, NickName, CertificateCode, CertificateType, Sex, Email, Birthday,  EducationLevel, IncomeLevel, ProvinceID, AreaID, Address, Favorite,  HeadPic);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();
    }



}

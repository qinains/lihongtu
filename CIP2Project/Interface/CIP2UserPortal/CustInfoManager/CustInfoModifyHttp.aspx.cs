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
using System.Text;
using System.Text.RegularExpressions;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using log4net;

public partial class CustInfoManager_CustInfoModifyHttp : System.Web.UI.Page
{
    
    public String SPID = String.Empty;
    public String CustID = String.Empty;
    public String RealName = String.Empty;
    public String UserName = String.Empty;
    public String NickName = String.Empty;
    public String CertificateCode = String.Empty;
    public String CertificateType = String.Empty;
    public String Sex = String.Empty;
    public String Email = String.Empty;

    public String ProvinceID = String.Empty;
    public String AreaID = String.Empty;
    public String Registration = String.Empty;  // 注册来源：11: 客户端注册-android；12: 客户端注册-ios；21:wap-ios；22:wap-android

    public String Birthday = String.Empty;
    public String EduLevel = String.Empty;
    public String IncomeLevel = String.Empty;
    public String Favorite = String.Empty;
    public String Address = String.Empty;
    public String HeadPic = String.Empty;

    public Int32 Result;
    public String ErrMsg;
    public String wt = "json";


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("client-android-modifycustinfo", msg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CustID = Request["CustID"];
        SPID = Request["SPID"];
        RealName = Request["RealName"];
        UserName = Request["UserName"];
        NickName = Request["NickName"];
        CertificateCode = Request["CertificateCode"];
        CertificateType = Request["CertificateType"];
        Sex = Request["Sex"];
        Email = Request["Email"];

        ProvinceID = Request["ProvinceID"];
        AreaID = Request["AreaID"];
        Registration = Request["Registration"]; // 注册来源：11: 客户端注册-android；12: 客户端注册-ios；21:wap-ios；22:wap-android

        Birthday = Request["Birthday"];
        EduLevel = Request["EduLevel"];
        IncomeLevel = Request["IncomeLevel"];
        Favorite = Request["Favorite"];
        Address = Request["Address"];
        HeadPic = Request["HeadPic"];
        wt = Request["wt"];

        StringBuilder strLog = new StringBuilder();
        //strLog.Append("----modify custinfo parameter-----");
        //strLog.AppendFormat("NickName:{0}",NickName);
        //strLog.AppendFormat("Address:{0}",Address);
        //strLog.AppendFormat("Birthday:{0}", Birthday);
        //strLog.AppendFormat("Favorite:{0}", Favorite);
        //strLog.AppendFormat("HeadPic:{0}",HeadPic);
        //strLog.AppendFormat("EduLevel:{0}", EduLevel);
        //strLog.AppendFormat("IncomeLevel:{0}", IncomeLevel);
        //strLog.Append("----------------------------------------");
        //log(strLog.ToString());

        String ResponseText = ModifyCustInfo(SPID, CustID, RealName, NickName, CertificateCode, CertificateType, Sex, Email, ProvinceID, AreaID, Registration, Birthday, EduLevel, IncomeLevel, Favorite, Address, HeadPic);
        if (!"json".Equals(wt))
        {
            Response.ContentType = "xml/text";
        }
        Response.Write(ResponseText);
        Response.Flush();
        Response.End();

    }

    public String ModifyCustInfo(String SPID,String CustID,String RealName,  String NickName, String CertificateCode, String CertificateType, String Sex, String Email, String ProvinceID, String AreaID, String Registration, String Birthday, String EduLevel, String IncomeLevel, String Favorite, String Address, String HeadPic)
    {
        StringBuilder ResponseMsg = new StringBuilder();
        Result = ErrorDefinition.IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.IError_Result_UnknowError_Msg;
        #region
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


        if (CommonUtility.IsEmpty(ProvinceID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "ProvinceID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "ProvinceID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(AreaID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "AreaID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "AreaID不能为空！");
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
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "修改信息参数不能全为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "999");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "修改信息参数不能全为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        #endregion

        try
        {
            //Result = CustBasicInfo.UpdateCustinfoExtV3(SPID, ProvinceID, AreaID, CustID, RealName, CertificateCode, CertificateType, Birthday, Sex, Email, NickName, EduLevel, IncomeLevel, Favorite, Address, HeadPic, out  ErrMsg);
            //UpdateCustinfoExtV4
            Result = CustBasicInfo.UpdateCustinfoExtV4(SPID, ProvinceID, AreaID, CustID, RealName, CertificateCode, CertificateType, Birthday, Sex, Email, NickName, EduLevel, IncomeLevel, Favorite, Address, HeadPic, out  ErrMsg);
            //CustID = "117663910";
            //Result = CustBasicInfo.UpdateCustinfoExtV5(CustID, HeadPic, out ErrMsg);

            if (Result == 0)
            {
                //通知积分平台
                //CIP2BizRules.InsertCustInfoNotify(CustID, "2", SPID, "", "0", out ErrMsg);
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
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ErrMsg);
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-989");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ErrMsg);
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }
        }
        catch (Exception e)
        {
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "-989");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", e.ToString());
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "-989");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", e.ToString());
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }
        return ResponseMsg.ToString();
    }

}

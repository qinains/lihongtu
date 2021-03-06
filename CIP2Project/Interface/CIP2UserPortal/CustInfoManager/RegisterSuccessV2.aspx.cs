﻿using System;
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
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Text;

public partial class CustInfoManager_RegisterSuccessV2 : System.Web.UI.Page
{

    public String SPID;
    public String ReturnUrl;
    public String newSPTokenRequest;
    public String CustID;
    public String ErrMsg;
    public String SPTokenRequest;

    public String HeadFooter;
    private String From = "";
    public Int32 Result;



    protected void Page_Load(object sender, EventArgs e)
    {
        string comefrom_url = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];  //http://localhost/CIP2UserPortal/CustInfoManager/RegisterInLowstint.aspx
        if (String.IsNullOrEmpty(comefrom_url))
        {
            Response.Redirect("../ErrorInfo.aspx?ErrorInfo=The page which come from does not valided! Please don't hack my website!");
        }

        if (!HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString().Contains("RegisterInLowstint.aspx"))
        {
            Response.Redirect("../ErrorInfo.aspx?ErrorInfo=The page which come from does not valided! Please don't hack my website!");
        }
        

        this.PanelForYiYouHeader.Visible = false;
        this.PanelForYiYouFooter.Visible = false;
        this.PanelForYiGouHeader.Visible = false;
        this.PanelForYiGouFooter.Visible = false;


        SPID = Request["SPID"];
        ReturnUrl = Request["ReturnUrl"];
        ParseSPTokenRequest();
        CreateSPTokenRequest();

        if ("yiyou".Equals(From))
        {
            if ("yes".Equals(HeadFooter))
            {
                this.PanelForYiYouHeader.Visible = true;
                this.PanelForYiYouFooter.Visible = true;
            }
        }
        else
        {
            if ("yes".Equals(HeadFooter))
            {
                this.PanelForYiGouHeader.Visible = true;
                this.PanelForYiGouFooter.Visible = true;
            }
        }
    }

    protected void ParseSPTokenRequest()
    {
        StringBuilder strLog = new StringBuilder();
        if (CommonUtility.IsParameterExist("SPTokenRequest", this.Page))
        {
            SPTokenRequest = Request["SPTokenRequest"];
            //日志
            strLog.AppendFormat("【SPTokenRequest参数】:" + SPTokenRequest);
            //解析请求参数
            Result = SSOClass.ParseBesttoneAccountPageRequest(SPTokenRequest, this.Context, out SPID, out CustID, out HeadFooter, out ReturnUrl, out ErrMsg);

            this.HiddenField_CUSTID.Value = CustID;
            this.HiddenField_SPID.Value = SPID;
            this.HiddenField_URL.Value = ReturnUrl;

        }

    }

    protected void CreateSPTokenRequest()
    {

        SPInfoManager spInfo = new SPInfoManager();
        Object SPData = spInfo.GetSPData(this.Context, "SPData");
        string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

        String _HeadFooter = "yes";
        String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

        UserToken UT = new UserToken();
        newSPTokenRequest = UT.GenerateBestAccountMainUserToken(CustID, ReturnUrl, _HeadFooter, TimeStamp, ScoreSystemSecret, out ErrMsg);
        newSPTokenRequest = HttpUtility.UrlEncode(SPID + "$" + newSPTokenRequest);
    }



}

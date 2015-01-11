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
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

/// <summary>
// *     描述: 修改客户信息AJAX处理页面
// * 开发平台: Windows XP + Microsoft SQL Server 2005
// * 开发语言: C#
// * 开发工具: Microsoft Visual Studio.Net 2005
// *     作者: 赵锐
// * 联系方式: 
// *     公司: 联创科技(南京)股份有限公司
// * 创建日期: 2009-07-31
/// </summary>


public partial class Ajax_CustInfo_setCustInfo : System.Web.UI.Page
{
    public int k;
    public string Msg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString())==1)
            {
                updateCustInfo();
            }
           
        }
    }

    public void updateCustInfo()
    {
        string custid = HttpUtility.HtmlDecode(Request.QueryString["custid"].ToString());
        string realname = HttpUtility.HtmlDecode(Request.QueryString["realname"].ToString());
        string nickname = HttpUtility.HtmlDecode(Request.QueryString["nickname"].ToString());
        string certificate = HttpUtility.HtmlDecode(Request.QueryString["certificate"].ToString());
        string certno = HttpUtility.HtmlDecode(Request.QueryString["certno"].ToString());
        string sex = HttpUtility.HtmlDecode(Request.QueryString["sex"].ToString());
        string birthday = HttpUtility.HtmlDecode(Request.QueryString["birthday"].ToString());
        string Edu = HttpUtility.HtmlDecode(Request.QueryString["Edu"].ToString());
        string Income = HttpUtility.HtmlDecode(Request.QueryString["Income"].ToString());
        string pro = HttpUtility.HtmlDecode(Request.QueryString["pro"].ToString());
        string area = HttpUtility.HtmlDecode(Request.QueryString["area"].ToString());
        string spid = HttpUtility.HtmlDecode(Request.QueryString["spid"].ToString());
        k = CustBasicInfo.UpdateCustInfoById(custid, pro, area, certificate, certno, realname, sex, nickname, DateTime.Now, birthday, Edu, Income, out Msg);
        if (k==0)
        {
           CIP2BizRules.InsertCustInfoNotify(custid,"2", spid, "","0", out Msg);  // 通知积分商城
           CIP2BizRules.NotifyBesttoneAccountInfo(spid, custid, out Msg);   // 通知融合支付
        }
            Response.Write(k);
    }


    
}

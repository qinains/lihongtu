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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace WebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String str = Request["PassportLoginResponse"] == null ? String.Empty : Server.UrlDecode(Request["PassportLoginResponse"]); 
            Response.Write(str);
            //str = Server.UrlDecode(str);

            //Response.Write(str);
            //String spid = "35000022";
            //String ip = "192.168.155.13";
            //String ErrMsg = String.Empty;
            //Int32 result = CommonBizRules.CheckIPLimit(spid, ip, this.Context, out ErrMsg);
            //if (result == 0)
            //{
            //    Response.Write("tongguo");
            //}
            //else
            //{
            //    Response.Write("no");
            //}
        }
    }
}

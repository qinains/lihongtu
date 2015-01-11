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

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;

public partial class RegisterNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Top1.Welcome = "号码百事通用户注册";

        if (!Page.IsPostBack)
        {
            //地区联动--start
            ProvinceInfoManager proInfo = new ProvinceInfoManager();
            object ProData = proInfo.GetProvinceData(this.Context);
            DataSet ds = (DataSet)ProData;
            this.proInfoList.DataSource = ds;
            this.proInfoList.DataTextField = "ProvinceName";
            this.proInfoList.DataValueField = "ProvinceID";
            this.proInfoList.DataBind();

            ListItem li = new ListItem("请选择", "-999");
            proInfoList.Items.Add(li);
            proInfoList.SelectedIndex = proInfoList.Items.Count - 1;

            this.areaInfoList.Items.Add(li);
            areaInfoList.SelectedIndex = areaInfoList.Items.Count - 1;

            HiddenField_SPID.Value = Request["SPID"] == null ? ConstHelper.DefaultInstance.BesttoneSPID : HttpUtility.HtmlDecode(Request["SPID"]);
            HiddenField_URL.Value = Request["ReturnUrl"] == null ? ConstHelper.DefaultInstance.BesttoneLoginPage : HttpUtility.HtmlDecode(Request["ReturnUrl"]);

        }

        if (proInfoList.SelectedIndex != proInfoList.Items.Count - 1)
        {
            PhoneAreaInfoManager areaInfo = new PhoneAreaInfoManager();
            object areaData = areaInfo.GetPhoneAreaData(this.Context, proInfoList.Value);
            PhoneAreaData pad = (PhoneAreaData)areaData;
            this.areaInfoList.DataSource = pad;
            this.areaInfoList.DataTextField = "AreaName";
            this.areaInfoList.DataValueField = "AreaID";
            this.areaInfoList.DataBind();
        }
        else
        {
            areaInfoList.Value = "请选择";
        }
    }

}

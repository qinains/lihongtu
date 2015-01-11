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
using System.Globalization;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Text;

public partial class CustInfoManager_CustInfoModify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnSaveServer.Click += new EventHandler(btnSaveServer_Click);
        if (!IsPostBack)
        {
            this.hdSPID.Value = Request["SPID"] == null ? "35000000" : Request["SPID"];
            //this.ValidateCIPToken1.IsRedircet = true;
            //this.ValidateCIPToken1.Validate();
            //this.hdCustID.Value = this.ValidateCIPToken1.CustID;

            this.hdCustID.Value = "43105411";
            if (String.IsNullOrEmpty(this.hdCustID.Value))
            {
                return;
            }

            EditBind();
        }
    }

    void btnSaveServer_Click(object sender, EventArgs e)
    {
        string spid = this.hdSPID.Value;
        string ReturnUrl = Request["ReturnUrl"];
        Response.Redirect("Success.aspx?Description=" + HttpUtility.UrlEncode("设置客户信息成功"));
    }

    protected DataTable GetCityName(string ProvinceID)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("AreaName", typeof(string)));
        dt.Columns.Add(new DataColumn("AreaId", typeof(string)));
        PhoneAreaInfoManager areaInfo = new PhoneAreaInfoManager();
        object areaData = areaInfo.GetPhoneAreaData(HttpContext.Current);
        DataSet pad = (DataSet)areaData;

        DataRow[] rows = pad.Tables["Area"].Select("ProvinceID='" + ProvinceID + "'");
        for (int i = 0; i < rows.Length; i++)
        {
            DataRow dr = dt.NewRow();
            dr["AreaName"] = rows[i]["AreaName"].ToString();
            dr["AreaId"] = rows[i]["AreaId"].ToString();

            dt.Rows.Add(dr);
        }

        return dt;
    }

    protected void EditBind()
    {
        Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        String ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        String provinceid = String.Empty, areaid = String.Empty;
        try
        {
            //基本信息
            string OuterID, Status, CustType, CustLevel, RealName, UserName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, Registration;
            //扩展信息
            string Birthday, EduLevel, Favorite, IncomeLevel;
            Result = CustBasicInfo.getCustInfo(this.hdSPID.Value, this.hdCustID.Value, out  ErrMsg, out  OuterID, out  Status, out  CustType,
                             out  CustLevel, out  RealName, out  UserName, out  NickName, out  CertificateCode,
                             out  CertificateType, out  Sex, out  Email, out  EnterpriseID, out  ProvinceID, out  AreaID, out  Registration);
            if (Result != 0)
            {

            }

            this.txtUserName.Text = UserName;
            this.txtRealName.Text = RealName;
            this.txtNickName.Text = NickName;
            this.DDLSex.SelectedValue = Sex;
            provinceid = ProvinceID;
            areaid = AreaID;
            this.DDLCertificateType.SelectedValue = CertificateType;
            if (!String.IsNullOrEmpty(CertificateType))
                this.txtCertificateCode.Text = CertificateCode;

            //获取扩展信息
            Result = CustExtendInfo.getCustExtendInfo(this.hdSPID.Value, this.hdCustID.Value, out ErrMsg, out Birthday, out  EduLevel, out  Favorite, out  IncomeLevel);
            if (!String.IsNullOrEmpty(Birthday))
            {
                this.txtBirthday.Text = DateTime.Parse(Birthday).ToString("yyyy-MM-dd");
            }
            this.DDLEdueLevel.SelectedValue = EduLevel;
            this.DDLIncomeLevel.SelectedValue = IncomeLevel;

        }
        catch (Exception ex)
        {

        }

        #region 绑定省市

        ProvinceInfoManager provinceMgr = new ProvinceInfoManager();
        object ProData = provinceMgr.GetProvinceData(this.Context);
        DataSet ds = (DataSet)ProData;
        ListItem li = null;

        this.DDLProvinceList.Items.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            if (row["ProvinceID"].ToString() == "35" || row["ProvinceName"].ToString() == "全国中心")
                continue;
            li = new ListItem(row["ProvinceName"].ToString(), row["ProvinceID"].ToString());
            this.DDLProvinceList.Items.Add(li);
        }

        this.DDLProvinceList.SelectedValue = provinceid;

        this.DDLAreaList.DataSource = GetCityName(provinceid);
        this.DDLAreaList.DataTextField = "AreaName";
        this.DDLAreaList.DataValueField = "AreaID";
        this.DDLAreaList.DataBind();

        this.DDLAreaList.SelectedValue = areaid;
        #endregion

    }
}

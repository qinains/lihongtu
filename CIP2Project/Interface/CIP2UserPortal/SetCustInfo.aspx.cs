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
using System.Globalization;
using Linkage.BestTone.Interface.Utility;
public partial class setCustInfo1 : System.Web.UI.Page
{
    public string Spid = "35000000";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Top1.Welcome = "号码百事通设置用户信息";
        this.Master.setTopWelcome("号码百事通用户信息");
        if (!Page.IsPostBack)
        {
            string url = Request.Url.AbsoluteUri;
            if (Request.QueryString["SPID"] != null)
            {
                Spid = Request.QueryString["SPID"].ToString();
                this.spidtxt.Value = Spid;
            }
            
            TokenValidate.IsRedircet = true;            //正式要恢复
            TokenValidate.Validate();                       //正式要恢复
            string CustID = TokenValidate.CustID;   //正式要恢复
            
            if (CustID != "")
            {
                this.custidtxt.Value = CustID;
            }

            
            #region 定义需要的参数

            //输出错误信息
            string Msg = "";
            //客户姓名
            string RealName = "";
            //昵称
            string NickName = "";
            //证件类型
            string CertificateType = "";
            //证件号码
            string CertificateCode = "";
            //性别
            string Sex = "";
            //客户所属省
            string ProvinceID = "";
            //客户归属地市
            string AreaID = "";
            //生日
            string Birthday = "";
            //文化程度
            string EduLevel = "";
            //收入水平
            string IncomeLevel = "";

            //外部客户ID
            string OuterID = "";
            //状态
            string Status = "";
            //客户类型
            string CustType = "";
            //客户级别
            string CustLevel = "";
            //邮箱
            string Email = "";
            //用户名
            string UserName = "";
            //爱好(废除)
            string Favorite = "";
            //？
            string Registration = "";

            string EnterpriseID = "";
            #endregion

            //调用用户基本信息查询函数将以上的参数带入函数内
            int k = CustBasicInfo.getCustInfo(Spid, CustID, out Msg, out OuterID, out Status, out CustType, out CustLevel, out RealName, out UserName, out NickName, out CertificateCode, out CertificateType, out Sex, out Email,out EnterpriseID, out ProvinceID, out AreaID, out Registration);

            //判断 如果函数返回0则对页面的HTML文本框赋值
            if (k == 0)
            {
                //string r = null;
                this.realnametxt.Value = RealName;
                this.nicknametxt.Value = NickName;

                this.certificatetxt.Value = CertificateType;
                this.certnotxt.Value = CertificateCode;

                this.sextxt.Value = Sex;
                this.stext.Value = ProvinceID;
                this.resulttxt.Value = AreaID;
                //this.emailtxt.Value = Email;
                //this.usernameTxt.Value = UserName;
           
                //初始话页面时给省份下拉框绑定所有省份
                ProvinceInfoManager proInfo = new ProvinceInfoManager();
                object ProData = proInfo.GetProvinceData(this.Context);
                DataSet ds = (DataSet)ProData;
                ListItem li = null;
                this.proInfoList.Items.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["ProvinceID"].ToString() == "35" || row["ProvinceName"].ToString() == "全国中心")
                        continue;
                    li = new ListItem(row["ProvinceName"].ToString(), row["ProvinceID"].ToString());
                    this.proInfoList.Items.Add(li);
                }
                //this.proInfoList.DataSource = ds;
                //this.proInfoList.DataTextField = "ProvinceName";
                //this.proInfoList.DataValueField = "ProvinceID";
                //this.proInfoList.DataBind();

                //初始话页面时给城市下拉绑定所有城市
                this.areaInfoList.DataSource = GetCityName(this.stext.Value);
                this.areaInfoList.DataTextField = "AreaName";
                this.areaInfoList.DataValueField = "AreaID";
                this.areaInfoList.DataBind();

                this.areaid.DataSource = GetCityName(this.stext.Value);
                this.areaid.DataTextField = "AreaID";
                this.areaid.DataValueField = "AreaID";
                this.areaid.DataBind();

                this.certificateSel.Value = this.certificatetxt.Value;
               
                this.sexSel.Value = this.sextxt.Value;
                this.proInfoList.Value = this.stext.Value;
                this.areaInfoList.Value = this.resulttxt.Value;
                this.areaid.Value = this.resulttxt.Value;

                if(certificateSel.Value=="")
                {
                    certnotxt.Style.Value = "display:block";
                    certnoL.Style.Value = "display:block"; 
                }
                
            }
            //调用客户扩展信息查询函数将以上参数带入函数内
            int y = CustExtendInfo.getCustExtendInfo(Spid, CustID, out Msg, out Birthday, out EduLevel, out Favorite, out IncomeLevel);
            //判断 如果函数返回0则对页面HTML文本框赋值
            if (y == 0)
            {
                if (!CommonUtility.IsEmpty(Birthday))
                {
                    DateTime da = Convert.ToDateTime(Birthday);
                    this.birthdaytxt.Value = da.ToShortDateString();
                }

                this.Edutxt.Value = EduLevel;
                this.Incometxt.Value = IncomeLevel;
                this.EduSel.Value = this.Edutxt.Value;
                this.IncomeSel.Value = this.Incometxt.Value;
            }
        }
    }

    public static DataTable GetCityName(string ProvinceID)
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

    protected void urlRedirectButton_ServerClick(object sender, EventArgs e)
    {
        string spid = this.spidtxt.Value;
        string ReturnUrl = null;
        if (Request.QueryString["ReturnUrl"] != null)
        {
            ReturnUrl = Request.QueryString["ReturnUrl"].ToString();
        }

        CommonBizRules.SuccessRedirect(ReturnUrl, "设置客户信息成功", HttpContext.Current);

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string spid = this.spidtxt.Value;
        Response.Redirect("verifyEmail.aspx?id=5&SPID=" + spid + "");
    }

    
}

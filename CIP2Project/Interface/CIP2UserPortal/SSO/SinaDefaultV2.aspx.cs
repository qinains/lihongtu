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
using NetDimension.Web;
using NetDimension.Weibo;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Configuration;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using System.IO;
public partial class SSO_SinaDefaultV2 : System.Web.UI.Page
{
    Cookie cookie = new Cookie("WeiboDemo", 24, TimeUnit.Hour);
    Client Sina = null;
    string UserID = string.Empty;
    string ReturnUrl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (CommonUtility.IsParameterExist("ReturnUrl", this.Page))
        {
            ReturnUrl = Request["ReturnUrl"];
        }
        else
        {
            Logs.logSave("没有ReturnUrl返回");
        }

        if (string.IsNullOrEmpty(cookie["AccessToken"]))
        {
            Response.Redirect("SinaLogin.aspx");
        }
        else
        {
            Sina = new Client(new OAuth(ConfigurationManager.AppSettings["AppKey"], ConfigurationManager.AppSettings["AppSecret"], cookie["AccessToken"], null)); //用cookie里的accesstoken来实例化OAuth，这样OAuth就有操作权限了
        }
        UserID = Sina.API.Account.GetUID();

        string CustID = QueryByOpenID(UserID);
        if (String.IsNullOrEmpty(CustID)) // 已有绑定关系
        {
            //直接单点登录
            string AuthenName = "";
            string AuthenType = "";
            string RealName = "";
            string NickName = "";
            string UserName = "";
            string OutID = "";
            string UserAccount = "";
            string CustType = "";
            string ProvinceID = "";


            string _connectionString = WebConfigurationManager.ConnectionStrings["BestToneCenterConStr"].ConnectionString;

            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("select  RealName,UserName,NickName,OuterID,CustType from custinfo where custid=@CustID", con);
            cmd.Parameters.Add("@CustID", SqlDbType.NVarChar, 16).Value = CustID;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RealName = (string)reader["RealName"];
                    UserName = (string)reader["UserName"];
                    NickName = (string)reader["NickName"];
                    OutID = (string)reader["OuterID"];
                    CustType = (string)reader["CustType"];

                }
            }

            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
            string ErrMsg = "";
            //生成token并保存
            UserToken UT = new UserToken();
            string UserTokenValue = UT.GenerateUserToken(CustID, RealName, UserName, NickName, OutID, CustType, AuthenName, AuthenType, key, out ErrMsg);
            string CookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            PageUtility.SetCookie(CookieName, UserTokenValue);
            //begin                    
            Response.Redirect(ReturnUrl,true);
            //end

        }
        else
        { // 未有绑定关系 (可能有号百账号-则去绑定,可能没有号百账号,则注册) 
            string SelectOauthAssertion = System.Configuration.ConfigurationManager.AppSettings["SelectOauthAssertion"];
            SelectOauthAssertion = SelectOauthAssertion + "?code=" + UserID + "&returnUrl=" + ReturnUrl + "&oauthtype=1";    // 0 代表qq 1代表sina
            Response.Redirect(SelectOauthAssertion, true);  //SelectOauthAssertion 指向地址:    http://sso.besttone.cn/SSO/boundingV2.action?code=***&returnUrl=***
            //boundingV2.action 会forward到 他自己的一个auth.jsp ,这个jsp会嵌入两个iframe,其中一个iframe的src，指向客户信息平台的AuthBindLogin.aspx,另个iframe指向 客户信息平台的AuthRegister.aspx
            //同时分别带上SPTokenRequest和code参数,这个SPTokenRequest参数中的ReturnUrl
        }




    }

    public static string QueryByOpenID(String OpenID)
    {
        string CustID = "";
        try
        {
            String where = String.Format(" where OpenID = '{0}'", OpenID);
            CustID = QueryByWhere(where);
        }
        catch (Exception ex) { throw ex; }
        return CustID;
    }


    #region 私有函数

    private static string QueryByWhere(String where)
    {
        string CustID = "";

        String sql = "select custid from oauth ".Insert("select custid from oauth ".Length, where);
        SqlCommand cmd = new SqlCommand(sql);
        cmd.CommandType = CommandType.Text;

        DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
        if (ds != null && ds.Tables[0] != null)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                CustID = Convert.ToString(row["custid"]);
            }
        }
        return CustID;
    }

    #endregion

    public static void logSave(string detail)
    {

        StreamWriter sw = null;
        DateTime date = DateTime.Now;
        string FileName = date.Year + "-" + date.Month;
        try
        {
            #region 检测日志目录是否存在
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
            #endregion

            FileName = HttpContext.Current.Server.MapPath("~/Logs/") + FileName + "-qzone.txt";

            #region 检测日志文件是否存在
            if (!File.Exists(FileName))
                sw = File.CreateText(FileName);
            else
            {
                sw = File.AppendText(FileName);
            }
            #endregion

            #region 向log文件中写数相关日志
            sw.WriteLine("IP        :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "\r");
            sw.WriteLine("Time      :" + date + "\r");
            sw.WriteLine("URL       :" + HttpContext.Current.Request.Url + "\r");
            sw.WriteLine("Details   :" + detail + "\r");
            sw.WriteLine("------------------------------------------------------------");
            //sw.WriteLine("");
            sw.Flush();
            #endregion
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            if (sw != null)
                sw.Close();
        }
    }



}

using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Linkage.BestTone.Interface.Rule;
using System.Text;

using System.Data.Sql;
using System.Data.SqlClient;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using Newtonsoft.Json;
public partial class SSO_QQDefault : System.Web.UI.Page
{

    private readonly string client_id = Utils.GetAppSeting("qzone_AppID");
    private readonly string client_secret = Utils.GetAppSeting("qzone_AppKey");
    private readonly string redirect_uri = Utils.GetAppSeting("qzone_Redirect_uri");

    //临时变量，发送URL,接受返回结果
    private string send_url = "", rezult = "";
    //用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
    private string state = "";
    //临时Authorization Code，官方提示10分钟过期
    private string code = "";
    //通过Authorization Code返回结果获取到的Access Token
    private string access_token = "";
    //expires_in是该Access Token的有效期，单位为秒
    private string expires_in = "";
    private string refresh_token = "";
    private string openid = "";
    //通过Access Token返回来的client_id 
    private string new_client_id = "";
    private string openkey = "";
    private string ReturnUrl = "";
    private string SPID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DoCallback();
    }


    void DoCallback()
    {
        StringBuilder strLog = new StringBuilder();

            if (CommonUtility.IsParameterExist("ReturnUrl", this.Page))
            {
                ReturnUrl = Request["ReturnUrl"];
            }
            else {
                //Logs.logSave("没有ReturnUrl返回");
                strLog.AppendFormat("没有ReturnUrl返回\r\n");
            }
            

            if(CommonUtility.IsParameterExist("code", this.Page))
            {
                code = Request["code"];
               
            }else
            {
                //Logs.logSave("没有code返回");
                strLog.AppendFormat("没有code返回\r\n");
            }
            if (CommonUtility.IsParameterExist("openid", this.Page))
            {
                openid = Request["openid"];
            }else
            {
                //Logs.logSave("没有openid返回");
                strLog.AppendFormat("没有openid返回\r\n");
            }
            if (CommonUtility.IsParameterExist("openkey", this.Page))
            {
                openkey = Request["openkey"];
            }else
            {
                //Logs.logSave("没有openkey返回");
                strLog.AppendFormat("没有openkey返回\r\n");
            }
            
            //写日志
            //Logs.logSave("返回CODE结果：" + code+",返回的openid:"+openid+",返回的openkey:"+openkey);
            strLog.AppendFormat("返回CODE结果：" + code + ",返回的openid:" + openid + ",返回的openkey:" + openkey+"\r\n");
            //==============通过Authorization Code和基本资料获取Access Token=================
            send_url = "https://open.t.qq.com/cgi-bin/oauth2/access_token?grant_type=authorization_code&client_id=" + client_id + "&client_secret=" + client_secret + "&code=" + code + "&state=" + state + "&redirect_uri=" + Utils.UrlEncode(redirect_uri);
            //https://open.t.qq.com/cgi-bin/oauth2/access_token?client_id=APP_KEY&client_secret=APP_SECRET&redirect_uri=http://www.myurl.com/example&grant_type=authorization_code&code=CODE
            send_url = "https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id=" + client_id + "&client_secret=" + client_secret + "&code=" + code + "&state=" + state + "&redirect_uri=" + Utils.UrlEncode(redirect_uri);
            //写日志
            //Logs.logSave("第二步，通过Authorization Code获取Access Token，发送URL：" + send_url);
            strLog.AppendFormat("第二步，通过Authorization Code获取Access Token，发送URL：" + send_url+"\r\n");
            //发送并接受返回值
            rezult = HttpMethods.HttpGet(send_url);
            // 返回内容：access_token=7a0fae7d2183c0c54ef18589fffe6475&expires_in=604800&refresh_token=15a0d166120bda818cd0782c0b7a8c1a&name=huoxintang
            //写日志
            //Logs.logSave("取得返回结果：" + rezult);
            strLog.AppendFormat("取得返回结果：" + rezult+"\r\n");
            //如果失败
            if (rezult.Contains("error"))
            {
                //出错了
                //写日志
                //Logs.logSave("出错了：" + rezult);
                strLog.AppendFormat("出错了：" + rezult+"\r\n");
                HttpContext.Current.Response.End();
            }
            else
            {

                //======================通过Access Token来获取用户的OpenID 这一步不需要 =======graph需要=======

                string[] parm = rezult.Split('&');

                //取得 access_token
                access_token = parm[0].Split('=')[1];
                //取得 过期时间
                expires_in = parm[1].Split('=')[1];

                //refresh_token = parm[2].Split('=')[1];  用graph 可能没有refresh_token

                //拼接url
                send_url = "https://graph.qq.com/oauth2.0/me?access_token=" + access_token;
                //发送并接受返回值
                rezult = HttpMethods.HttpGet(send_url);
                //写日志
                //Logs.logSave("第三步，发送 access_token：" + send_url);
                strLog.AppendFormat("第三步，发送 access_token：" + send_url+"\r\n");
                //如果失败
                if (rezult.Contains("error"))
                {
                    //出错了
                    //写日志
                    //Logs.logSave("出错了：" + rezult);
                    strLog.AppendFormat("出错了：" + rezult+"\r\n");
                    HttpContext.Current.Response.End();
                }
                //写日志
                //Logs.logSave("得到返回结果：" + rezult);
                strLog.AppendFormat("得到返回结果：" + rezult+"\r\n");

                //取得文字出现
                int str_start = rezult.IndexOf('(') + 1;
                int str_last = rezult.LastIndexOf(')') - 1;

                //取得JSON字符串
                rezult = rezult.Substring(str_start, (str_last - str_start));
                //反序列化JSON
                Dictionary<string, string> _dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(rezult);


                //取值
                _dic.TryGetValue("client_id", out new_client_id);
                _dic.TryGetValue("openid", out openid);

                //储存获取数据用到的信息
                HttpContext.Current.Session["access_token"] = access_token;
                HttpContext.Current.Session["client_id"] = client_id;
                HttpContext.Current.Session["openid"] = openid;
                HttpContext.Current.Session["openkey"] = openkey;

                // 这里张剑锋还拿到了  Level,NickName,Gender


                //========继续您的业务逻辑编程==========================================

                //取到 openId
                //openId与您系统的user数据进行关联
                //一个openid对应一个QQ，一个openid也要对应到您系统的一个账号：QQ--OpenId--User；
                //这个时候有两种情况：
                //【1】您让用户绑定系统已有的用户，那么让用户输入用户名密码，找到该用户，然后绑定OpenId
                //【2】为用户生成一个系统用户，直接绑定OpenId

                //上面完成之后，设置用户的登录状态，完整绑定和登录


                //=============通过Access Token和OpenID来获取用户资料  ====
                send_url = "https://open.t.qq.com/api/user/info?access_token=" + access_token + "&oauth_consumer_key=" + client_id + "&openid=" + openid + "&openkey=" + openkey + "&oauth_version=2.a";
                //https://open.t.qq.com/api/user/info?access_token=7a0fae7d2183c0c54ef18589fffe6475&oauth_consumer_key=801210600&openid=65FCC7BC2B69619BC13BCF6C16FB06C3&oauth_version=2.a&openkey=05FB5E1C75119B141BAD0444C6EA41CE
                send_url = "https://graph.qq.com/user/get_user_info?access_token=" + access_token + "&oauth_consumer_key=" + client_id + "&openid=" + openid + "&openkey=" + openkey + "&oauth_version=2.a";

                //发送并接受返回值
                //Logs.logSave("发送send_url：" + send_url);
                strLog.AppendFormat("发送send_url：" + send_url+"\r\n");
                rezult = HttpMethods.HttpGet(send_url);
                //写日志
                //Logs.logSave("第四步，通过get_user_info方法获取数据：" + send_url);
                //Logs.logSave("rezult：" + rezult);
                strLog.AppendFormat("第四步，通过get_user_info方法获取数据：" + send_url+"\r\n");
                strLog.AppendFormat("rezult：" + rezult+"\r\n");
                //反序列化JSON
                
                /**
                Dictionary<string, object> _data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rezult);
                object jsondata = null;
                _data.TryGetValue("data", out jsondata);
                string js_data = jsondata.ToString();
                Dictionary<string, object> useinfo_data = JsonConvert.DeserializeObject<Dictionary<string, object>>(js_data);
                object nick = null;
                object j_openid = null;
                object sex = null;
                object province_code = null;
                object head = null;
                object j_name = null;
                useinfo_data.TryGetValue("nick", out nick);
                useinfo_data.TryGetValue("openid", out j_openid);
                useinfo_data.TryGetValue("sex", out sex);
                useinfo_data.TryGetValue("province_code", out province_code);
                useinfo_data.TryGetValue("head", out head);
                useinfo_data.TryGetValue("name", out j_name);
                Logs.logSave("=====================");
                Logs.logSave("nickname:" + nick.ToString());
                Logs.logSave("openid:"+j_openid.ToString());
                Logs.logSave("sex:"+sex.ToString());
                Logs.logSave("Province_code:" + province_code.ToString());
                Logs.logSave("head:" + head.ToString());
                Logs.logSave("name:" + j_name.ToString());
                //Logs.logSave("jsondata：" + jsondata);

                **/







                Dictionary<string, string> _dic2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(rezult);

                string ret = "", msg = "", nickname = "", face = "", sex = "",ret_openid="",ret_name="";

                //取值
                _dic2.TryGetValue("ret", out ret);
                _dic2.TryGetValue("msg", out msg);

                //如果失败
                if (ret != "0")
                {
                    //出错了
                    //写日志
                    //Logs.logSave("出错了：" + rezult);
                    strLog.AppendFormat("出错了：" + rezult+"\r\n");
                    //HttpContext.Current.Response.Write(rezult);
                    HttpContext.Current.Response.End();
                }

                _dic2.TryGetValue("nickname", out nickname);
                _dic2.TryGetValue("head", out face);

                _dic2.TryGetValue("gender", out sex);
                _dic2.TryGetValue("openid", out ret_openid);
                _dic2.TryGetValue("name", out ret_name);

                //写日志
                ///Logs.logSave("得到返回结果:" + rezult);
                strLog.AppendFormat("得到返回结果:" + rezult+"\r\n");
                //string newline = "<br>";
                //string str = "";
                //str += "openid：" + openid + newline;
                //str += "昵称：" + nickname + newline;
                //str += "名称：" + ret_name + newline;
                //str += "性别：" + sex + newline;
                //str += "默认头像：" + face + newline;


                //页面输出结果：
                //HttpContext.Current.Response.Write("返回结果如下：" + rezult + newline + newline);

                //HttpContext.Current.Response.Write("经过处理后：" + newline + str);

                
                /**
                string newline = "<br>";
                string str = "";
                str += "openid：" + j_openid.ToString() + newline;
                str += "昵称：" + nick.ToString() + newline;
                str += "名称：" + j_name.ToString() + newline;
                str += "性别：" + sex.ToString() + newline;
                str += "默认头像：" + head.ToString() + newline;
                str += "省份：" + province_code.ToString() + newline;
                **/
                
                //页面输出结果：
                //HttpContext.Current.Response.Write("返回结果如下：" + rezult + newline + newline);

                //HttpContext.Current.Response.Write("经过处理后：" + newline + str);

                string CustID = QueryByOpenID(openid);
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
                    SqlCommand cmd = new SqlCommand("select  RealName,UserName,NickName,OuterID,CustType,SourceSPID from custinfo where custid=@CustID", con);
                    cmd.Parameters.Add("@CustID", SqlDbType.NVarChar, 16).Value = CustID;
                    using (con)
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                             RealName = (string)reader["RealName"];
                             UserName  = (string)reader["UserName"];
                             NickName = (string)reader["NickName"];
                             OutID = (string)reader["OuterID"];
                             CustType = (string)reader["CustType"];
                             SPID = (string)reader["SourceSPID"];
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

                    TokenValidate.IsRedircet = false;
                    TokenValidate.Validate();

                    //begin
                    this.ssoFunc();
                    //Response.Redirect(ReturnUrl, true);                 
                    //end


                }
                else { // 未有绑定关系 (可能有号百账号-则去绑定,可能没有号百账号,则注册) 
                    string SelectOauthAssertion = System.Configuration.ConfigurationManager.AppSettings["SelectOauthAssertion"];
                    SelectOauthAssertion = SelectOauthAssertion + "?code=" + openid + "&returnUrl=" + ReturnUrl+"&oauthtype=0";    // 0 代表qq 1代表sina
                    Response.Redirect(SelectOauthAssertion, true);  //SelectOauthAssertion 指向地址:    http://sso.besttone.cn/SSO/boundingV2.action?code=***&returnUrl=***
                    //boundingV2.action 会forward到 他自己的一个auth.jsp ,这个jsp会嵌入两个iframe,其中一个iframe的src，指向客户信息平台的AuthBindLogin.aspx,另个iframe指向 客户信息平台的AuthRegister.aspx
                    //同时分别带上SPTokenRequest和code参数,这个SPTokenRequest参数中的ReturnUrl
                }
            }


            log(strLog.ToString());
    }


    protected void ssoFunc()
    {
        string Url = "";
        try
        {
            string Ticket = CommonBizRules.CreateTicket();

            string CustID = TokenValidate.CustID;
            string RealName = TokenValidate.RealName;
            string NickName = TokenValidate.NickName;
            string UserName = TokenValidate.UserName;
            string OutID = TokenValidate.OuterID;
            string LoginAuthenName = TokenValidate.LoginAuthenName;
            string LoginAuthenType = TokenValidate.LoginAuthenType;
            log(String.Format("ssoFunc: TokenValidate.RealName:{0},TokenValidate.NickName:{1},TokenValidate.UserName:{2},TokenValidate.LoginAuthenName:{3},TokenValidate.LoginAuthenType:{4}",
                TokenValidate.RealName, TokenValidate.NickName, TokenValidate.UserName, TokenValidate.LoginAuthenName, TokenValidate.LoginAuthenType));
            String er = "";
            int Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, CustID, RealName, UserName, NickName, OutID, "", LoginAuthenName, LoginAuthenType, out er);

            if (Result != 0)
            {
               
                return;
            }

            if (ReturnUrl.IndexOf("?") > 0)
            {
                Url = ReturnUrl + "&Ticket=" + Ticket;
            }
            else
            {
                Url = ReturnUrl + "?Ticket=" + Ticket;
            }

            if (CommonUtility.IsParameterExist("NeedLogin", this.Page))
            {
                Url = Url + "&NeedLogin=" + Request["NeedLogin"];
            }
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            Response.Redirect(Url,true);
        }

        catch (Exception e)
        {
           
        }
    }




    public static string  QueryByOpenID(String OpenID)
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


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("passportlogin", msg);
    }

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

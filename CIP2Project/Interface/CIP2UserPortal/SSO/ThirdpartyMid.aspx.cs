using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;
using NetDimension.Web;
using NetDimension.Weibo;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

using Linkage.BestTone.Interface.Rule.ThirdPartyAuthen;



public partial class SSO_ThirdpartyMid : System.Web.UI.Page
{

    public string SPID = "35000000";
    public string ReturnUrl = "";
    public string CustID = "";
    public string ErrMsg = "";
    public string TimeStamp = "";
    public int Result;




    Cookie cookie = new Cookie("WeiboDemo", 24, TimeUnit.Hour);
    Client Sina = null;
    string SinaOpenID = string.Empty;
    public string QQOpenID = string.Empty;
    public string Source = "0";  // 默认 0 代表Sina ,1 代表QQ




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



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Int32 ResultIsOpenIDBinding = 0;
            Source = Request.QueryString["Source"];
            if ("0".Equals(Source))
            {
                if (string.IsNullOrEmpty(cookie["AccessToken"]))
                {
                    Response.Redirect("SinaLogin.aspx");
                }
                else
                {
                    Sina = new Client(new OAuth(ConfigurationManager.AppSettings["AppKey"], ConfigurationManager.AppSettings["AppSecret"], cookie["AccessToken"], null)); //用cookie里的accesstoken来实例化OAuth，这样OAuth就有操作权限了
                }

                SinaOpenID = Sina.API.Account.GetUID();

                ResultIsOpenIDBinding = ThirdPartyAccount.IsOpenIdHasBindingCustId("0",SinaOpenID, out CustID, out ErrMsg);
                if (ResultIsOpenIDBinding == 0)
                {
                    // 代表SinaOpenId 已经和某个custId建立绑定关系
                }
                else
                {
                    // 没有绑定关系则可能有两种情况1.有custid了，但是没绑定，2.没有custid ,这两种情况都得redirect 左绑右注册页面 ->建立绑定关系
                }

            }
            else if ("1".Equals(Source))
            {
                QQCallback();
                ResultIsOpenIDBinding = ThirdPartyAccount.IsOpenIdHasBindingCustId("1", openid, out CustID, out ErrMsg);
                if(ResultIsOpenIDBinding==0)
                {
                    // 代表qqOpenId 已经和某个custId建立绑定关系
                }else
                {
                    // 没有绑定关系则可能有两种情况1.有custid了，但是没绑定，2.没有custid ,这两种情况都得redirect 左绑右注册页面 ->建立绑定关系
                }

            }
            else
            {
                // 即不是qq也不是sina，应该去哪里?
                Response.Redirect("Error.aspx");
            }

        } 

    }


    void QQCallback()
    {


        if (CommonUtility.IsParameterExist("code", this.Page))
        {
            code = Request["code"];

        }
        else
        {
            log("没有code返回");
        }
        if (CommonUtility.IsParameterExist("openid", this.Page))
        {
            openid = Request["openid"];
        }
        else
        {
            log("没有openid返回");
        }
        if (CommonUtility.IsParameterExist("openkey", this.Page))
        {
            openkey = Request["openkey"];
        }
        else
        {
            log("没有openkey返回");
        }

        //写日志
        log("返回CODE结果：" + code + ",返回的openid:" + openid + ",返回的openkey:" + openkey);

        //==============通过Authorization Code和基本资料获取Access Token=================
        send_url = "https://open.t.qq.com/cgi-bin/oauth2/access_token?grant_type=authorization_code&client_id=" + client_id + "&client_secret=" + client_secret + "&code=" + code + "&state=" + state + "&redirect_uri=" + Utils.UrlEncode(redirect_uri);
        //https://open.t.qq.com/cgi-bin/oauth2/access_token?client_id=APP_KEY&client_secret=APP_SECRET&redirect_uri=http://www.myurl.com/example&grant_type=authorization_code&code=CODE
        send_url = "https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id=" + client_id + "&client_secret=" + client_secret + "&code=" + code + "&state=" + state + "&redirect_uri=" + Utils.UrlEncode(redirect_uri);
        //写日志
        log("第二步，通过Authorization Code获取Access Token，发送URL：" + send_url);

        //发送并接受返回值
        rezult = HttpMethods.HttpGet(send_url);
        // 返回内容：access_token=7a0fae7d2183c0c54ef18589fffe6475&expires_in=604800&refresh_token=15a0d166120bda818cd0782c0b7a8c1a&name=huoxintang
        //写日志
        log("取得返回结果：" + rezult);

        //如果失败
        if (rezult.Contains("error"))
        {
            //出错了
            //写日志
            log("出错了：" + rezult);
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
            log("第三步，发送 access_token：" + send_url);
            //如果失败
            if (rezult.Contains("error"))
            {
                //出错了
                //写日志
                log("出错了：" + rezult);
                HttpContext.Current.Response.End();
            }
            //写日志
            log("得到返回结果：" + rezult);
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
            log("发送send_url：" + send_url);
            rezult = HttpMethods.HttpGet(send_url);
            //写日志
            log("第四步，通过get_user_info方法获取数据：" + send_url);
            log("rezult：" + rezult);

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
            Logs.logSave("jsondata：" + jsondata);

            **/







            Dictionary<string, string> _dic2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(rezult);

            string ret = "", msg = "", nickname = "", face = "", sex = "", ret_openid = "", ret_name = "";

            //取值
            _dic2.TryGetValue("ret", out ret);
            _dic2.TryGetValue("msg", out msg);

            //如果失败
            if (ret != "0")
            {
                //出错了
                //写日志
                log("出错了：" + rezult);

                //HttpContext.Current.Response.Write(rezult);
                HttpContext.Current.Response.End();
            }

            _dic2.TryGetValue("nick", out nickname);
            _dic2.TryGetValue("head", out face);

            _dic2.TryGetValue("sex", out sex);
            _dic2.TryGetValue("openid", out ret_openid);
            _dic2.TryGetValue("name", out ret_name);

            //写日志
            log("得到返回结果" + rezult);

            string newline = "<br>";
            string str = "";
            str += "openid：" + ret_openid + newline;
            str += "昵称：" + nickname + newline;
            str += "名称：" + ret_name + newline;
            str += "性别：" + sex + newline;
            str += "默认头像：" + face + newline;


            //页面输出结果：
            HttpContext.Current.Response.Write("返回结果如下：" + rezult + newline + newline);

            HttpContext.Current.Response.Write("经过处理后：" + newline + str);


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
            HttpContext.Current.Response.Write("返回结果如下：" + rezult + newline + newline);

            HttpContext.Current.Response.Write("经过处理后：" + newline + str);

        }



    }


    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("ThirdPartyMid", msg);
    }


}

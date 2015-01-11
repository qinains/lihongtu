
/*********************************************************************************************************
 * 描述: 客户信息平台
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 李宏图
 * 联系方式: 
 *     公司: 号百信息服务有限公司
 * 创建日期: 2012-08-10
 *********************************************************************************************************/
using System;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace Linkage.BestTone.Interface.Rule
{
    public class Utils
    {
        /// <summary>
        /// 获取AppSeting字符串函数
        /// </summary>
        /// <param name="Str">所要获取的字符串</param>
        /// <returns></returns>
        public static string GetAppSeting(string key)
        {

            if (key == null || key == "")
                return "";
            string outstr = "";
            outstr = System.Configuration.ConfigurationManager.AppSettings[key];
            if (outstr == null || outstr == "")
                return "";

            return outstr;
        }

        /// <summary>
        /// 编码转换UnicodeToGB，如，\u548c换成中文汉字
        /// </summary>
        /// <param name="content">输入内容</param>
        /// <returns>输出内容</returns>
        private string UnicodeToGB(string content)
        {
            Regex objRegex = new Regex("&#(?<UnicodeCode>[\\d]{5});", RegexOptions.IgnoreCase);
            Match objMatch = objRegex.Match(content);
            System.Text.StringBuilder sb = new System.Text.StringBuilder(content);
            while (objMatch.Success)
            {
                string code = Convert.ToString(Convert.ToInt32(objMatch.Result("${UnicodeCode}")), 16);
                byte[] array = new byte[2];
                array[0] = (byte)Convert.ToInt32(code.Substring(2), 16);
                array[1] = (byte)Convert.ToInt32(code.Substring(0, 2), 16);

                sb.Replace(objMatch.Value, System.Text.Encoding.Unicode.GetString(array));

                objMatch = objMatch.NextMatch();
            }
            return sb.ToString();
        }


        /// <summary>
        /// 获取字符传的位数,位数不能少于1
        /// </summary>
        public static string getstr(string str1,int leng)
        {
            string tmp = str1;
            if (leng>0)
            {
                if (tmp.Length > leng)
                {
                    tmp.Remove(leng);
                    tmp = tmp+"...";
                }
            }
            return tmp;
        }

        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="Str">所要截取的字符串</param>
        /// <param name="Num">截取字符串的长度</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num)
        {
            if (Str == null || Str == "")
                return "";
            string outstr = "";
            int n = 0;
            foreach (char ch in Str)
            {
                n += System.Text.Encoding.Default.GetByteCount(ch.ToString());
                if (n > Num)
                    break;
                else
                    outstr += ch;
            }
            return outstr;
        }
      
        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="Str">所要截取的字符串</param>
        /// <param name="Num">截取字符串的长度</param>
        /// <param name="Num">截取字符串后省略部分的字符串</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num, string LastStr)
        {
            return (Str.Length > Num) ? Str.Substring(0, Num) + LastStr : Str;
        }

        /// <summary>
        /// MD5加密字符串处理
        /// </summary>
        /// <param name="Half">加密是16位还是32位；如果为true为16位</param>
        /// <param name="Input">待加密码字符串</param>
        /// <returns></returns>
        public static string MD5(string Input, bool Half)
        {
            string output = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Input, "MD5").ToLower();
            if (Half)//16位MD5加密（取32位加密的9~25字符）
                output = output.Substring(8, 16);
            return output;
        }

        public static string MD5(string Input)
        {
            return MD5(Input, true);
        }


        /// <summary>
        /// 过滤Sql
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FilterSql(string sql)
        {
            sql = sql.Replace("'", "''");
            return sql;
        }


        ///  <summary>  
		/// 分析用户请求是否正常  
		///  </summary>  
		///  <param name="Str">传入用户提交数据  </param>  
		///  <returns>返回是否含有SQL注入式攻击代码  </returns>  
		private bool ProcessSqlStr(string Str)  
		{  
			bool ReturnValue = true;  
			try  
			{  
				if (Str.Trim() != "")  
				{  
					string SqlStr = "and |exec |insert |select |delete |update |count |* |chr |mid |master |truncate |char |declare";  

					string[] anySqlStr = SqlStr.Split('|');  
					foreach (string ss in anySqlStr)  
					{  
						if (Str.ToLower().IndexOf(ss) >= 0)  
						{  
							ReturnValue = false;  
							break;  
						}  
					}  
				}  
			}  
			catch  
			{  
				ReturnValue = false;  
			}  
			return ReturnValue;  
		}  
		

        /// <summary>
        /// 执行一个JS语句
        /// </summary>
        /// <param name="sentence">要执行的语句</param>
        public static void ExecuteJs(string sentence)
        {
            HttpContext.Current.Response.Write("<script language=\"javascript\" type=\"text/javascript\">");
            HttpContext.Current.Response.Write(sentence);
            HttpContext.Current.Response.Write("</script>");
        }



        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                return IsNumeric(expression.ToString());
            }
            return false;

        }




        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 移动：134[0-8],135,136,137,138,139,150,151,157,158,159,182,187,188
        /// 联通：130,131,132,152,155,156,185,186
        /// 电信：133,1349,153,180,189
        /// /^1([3][0-9]|[5][012356789]|[8][01256789])\d{8}$/;
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool isMobilePhone(string mobile)
        {
            return Regex.IsMatch(mobile, @"^1([3][0-9]|[5][0123456789]|[8][0123456789])\d{8}$");
        }

        //    Regex regMobile = new Regex(@"^1[3458]\d{9}$");
        public static bool isMobilePhoneV2(string mobile)
        {
            return Regex.IsMatch(mobile, @"^1[3458]\d{9}$");
        }
        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
            {
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression, defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(expression, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (str == null)
                return defValue;
            if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*$"))
            {
                if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                {
                    return Convert.ToInt32(str);
                }
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
            {
                return defValue;
            }

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                return HttpContext.Current.Request.Cookies[strName][key].ToString();
            }

            return "";
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (result == "127.0.0.1")
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
                if (result == "")
                { result = "127.0.0.1";  }
            }

            return result;

        }

        public static bool IsTelephone(string telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }

        public static bool IsHandset(string handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(handset, @"^[1]+[3,5]+\d{9}");
        }

        public bool IsIDcard(string idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(idcard, @"(^\d{18}$)|(^\d{15}$)");
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }


        /// <summary>
        /// 判断是否时间格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTime(string str)
        {
            bool bol = false;
            DateTime Dt = new DateTime();
            if (DateTime.TryParse(str, out Dt))
            {
                bol = true;
            }
            else
            {
                bol = false;
            }
            return bol;
        }      
 
        

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFileFullPath(string strName)
        {
            if (HttpContext.Current.Server.MapPath(strName)== null)
            {
                return "";
            }
            return HttpContext.Current.Server.MapPath(strName);
        }

        /// <summary>
        /// 替换字符串中的空格
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string ReplaceSpace(string strName)
        {

            return strName.Replace(" ", "-").Replace("&", "-");

        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
      
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
          
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;

        }
        /// <summary>
        /// 过滤html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FilterHtmlStr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            //html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }


        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource"></param>
        /// <returns></returns>
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }


        /// <summary>
        /// URL路径编码
        /// </summary>
        /// <returns></returns>
        public static string UrlEncode(string strHtml)
        {
            return HttpUtility.UrlEncode(strHtml, Encoding.Default);
            
        }

        /// <summary>
        /// URL路径解码
        /// </summary>
        /// <returns></returns>
        public static string UrlDecode(string strHtml)
        {
            return HttpUtility.UrlDecode(strHtml, Encoding.Default);
        }

        ///// <summary>
        ///// URL路径编码
        ///// </summary>
        ///// <returns></returns>
        //public static string UrlPathEncode(string strHtml)
        //{
        //    return HttpUtility.UrlPathEncode(strHtml);
        //}

     

    }
}

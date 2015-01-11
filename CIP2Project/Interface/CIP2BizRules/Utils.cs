
/*********************************************************************************************************
 * ����: �ͻ���Ϣƽ̨
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2005
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2005
 *     ����: ���ͼ
 * ��ϵ��ʽ: 
 *     ��˾: �Ű���Ϣ�������޹�˾
 * ��������: 2012-08-10
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
        /// ��ȡAppSeting�ַ�������
        /// </summary>
        /// <param name="Str">��Ҫ��ȡ���ַ���</param>
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
        /// ����ת��UnicodeToGB���磬\u548c�������ĺ���
        /// </summary>
        /// <param name="content">��������</param>
        /// <returns>�������</returns>
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
        /// ��ȡ�ַ�����λ��,λ����������1
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
        /// ��ȡ�ַ�������
        /// </summary>
        /// <param name="Str">��Ҫ��ȡ���ַ���</param>
        /// <param name="Num">��ȡ�ַ����ĳ���</param>
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
        /// ��ȡ�ַ�������
        /// </summary>
        /// <param name="Str">��Ҫ��ȡ���ַ���</param>
        /// <param name="Num">��ȡ�ַ����ĳ���</param>
        /// <param name="Num">��ȡ�ַ�����ʡ�Բ��ֵ��ַ���</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num, string LastStr)
        {
            return (Str.Length > Num) ? Str.Substring(0, Num) + LastStr : Str;
        }

        /// <summary>
        /// MD5�����ַ�������
        /// </summary>
        /// <param name="Half">������16λ����32λ�����ΪtrueΪ16λ</param>
        /// <param name="Input">���������ַ���</param>
        /// <returns></returns>
        public static string MD5(string Input, bool Half)
        {
            string output = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Input, "MD5").ToLower();
            if (Half)//16λMD5���ܣ�ȡ32λ���ܵ�9~25�ַ���
                output = output.Substring(8, 16);
            return output;
        }

        public static string MD5(string Input)
        {
            return MD5(Input, true);
        }


        /// <summary>
        /// ����Sql
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FilterSql(string sql)
        {
            sql = sql.Replace("'", "''");
            return sql;
        }


        ///  <summary>  
		/// �����û������Ƿ�����  
		///  </summary>  
		///  <param name="Str">�����û��ύ����  </param>  
		///  <returns>�����Ƿ���SQLע��ʽ��������  </returns>  
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
        /// ִ��һ��JS���
        /// </summary>
        /// <param name="sentence">Ҫִ�е����</param>
        public static void ExecuteJs(string sentence)
        {
            HttpContext.Current.Response.Write("<script language=\"javascript\" type=\"text/javascript\">");
            HttpContext.Current.Response.Write(sentence);
            HttpContext.Current.Response.Write("</script>");
        }



        /// <summary>
        /// �ж϶����Ƿ�ΪInt32���͵�����
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
        /// �ж϶����Ƿ�ΪInt32���͵�����
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
        /// �ƶ���134[0-8],135,136,137,138,139,150,151,157,158,159,182,187,188
        /// ��ͨ��130,131,132,152,155,156,185,186
        /// ���ţ�133,1349,153,180,189
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
        /// �Ƿ�ΪDouble����
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
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression, defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
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
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static int StrToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
            {
                return defValue;
            }

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        /// �жϸ������ַ�������(strNumber)�е������ǲ��Ƕ�Ϊ��ֵ��
        /// </summary>
        /// <param name="strNumber">Ҫȷ�ϵ��ַ�������</param>
        /// <returns>���򷵼�true �����򷵻� false</returns>
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
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
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
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
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
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
        /// <param name="strValue">����ʱ��(����)</param>
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
        /// ��cookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <returns>cookieֵ</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return "";
        }

        /// <summary>
        /// ��cookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <returns>cookieֵ</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                return HttpContext.Current.Request.Cookies[strName][key].ToString();
            }

            return "";
        }

        /// <summary>
        /// ��õ�ǰҳ��ͻ��˵�IP
        /// </summary>
        /// <returns>��ǰҳ��ͻ��˵�IP</returns>
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
        /// �Ƿ�Ϊip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }


        /// <summary>
        /// �ж��Ƿ�ʱ���ʽ
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
        /// �жϵ�ǰҳ���Ƿ���յ���Post����
        /// </summary>
        /// <returns>�Ƿ���յ���Post����</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        /// <summary>
        /// �жϵ�ǰҳ���Ƿ���յ���Get����
        /// </summary>
        /// <returns>�Ƿ���յ���Get����</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// ���ָ����������ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <returns>��������ֵ</returns>
        public static string GetFileFullPath(string strName)
        {
            if (HttpContext.Current.Server.MapPath(strName)== null)
            {
                return "";
            }
            return HttpContext.Current.Server.MapPath(strName);
        }

        /// <summary>
        /// �滻�ַ����еĿո�
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string ReplaceSpace(string strName)
        {

            return strName.Replace(" ", "-").Replace("&", "-");

        }


        /// <summary>
        /// ���ָ����������ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <returns>��������ֵ</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// ���ָ��Url������ֵ
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <returns>Url������ֵ</returns>
        public static string GetQueryString(string strName)
        {
      
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// ��õ�ǰ����Url��ַ
        /// </summary>
        /// <returns>��ǰ����Url��ַ</returns>
        public static string GetUrl()
        {
          
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// ������һ��ҳ��ĵ�ַ
        /// </summary>
        /// <returns>��һ��ҳ��ĵ�ַ</returns>
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
        /// ����html��ǩ
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
            html = regex1.Replace(html, ""); //����<script></script>��� 
            html = regex2.Replace(html, ""); //����href=javascript: (<A>) ���� 
            html = regex3.Replace(html, " _disibledevent="); //���������ؼ���on...�¼� 
            html = regex4.Replace(html, ""); //����iframe 
            html = regex5.Replace(html, ""); //����frameset 
            html = regex6.Replace(html, ""); //����frameset 
            html = regex7.Replace(html, ""); //����frameset 
            html = regex8.Replace(html, ""); //����frameset 
            html = regex9.Replace(html, "");
            //html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }


        /// <summary>
        /// �Ƿ�Ϊ�������ַ���
        /// </summary>
        /// <param name="StrSource"></param>
        /// <returns></returns>
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }


        /// <summary>
        /// URL·������
        /// </summary>
        /// <returns></returns>
        public static string UrlEncode(string strHtml)
        {
            return HttpUtility.UrlEncode(strHtml, Encoding.Default);
            
        }

        /// <summary>
        /// URL·������
        /// </summary>
        /// <returns></returns>
        public static string UrlDecode(string strHtml)
        {
            return HttpUtility.UrlDecode(strHtml, Encoding.Default);
        }

        ///// <summary>
        ///// URL·������
        ///// </summary>
        ///// <returns></returns>
        //public static string UrlPathEncode(string strHtml)
        //{
        //    return HttpUtility.UrlPathEncode(strHtml);
        //}

     

    }
}

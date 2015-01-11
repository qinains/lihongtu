//==============================================================================================================
//
// Class Name: CommonUtility
// Description: 提供常用的简单公用方法




// Author: 苑峰
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-05
//
//==============================================================================================================
using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
namespace Linkage.BestTone.Interface.Utility
{
   public class CommonUtility
    {
       /// <summary>
       /// 从xml字符串中取值
       /// </summary>
       /// <param name="XmlInfo"></param>
       /// <param name="NodeName"></param>
       /// <returns></returns>
       public static string GetValueFromXML(string XmlInfo, string NodeName)
       {
           string nodeValue = "";

           try
           {
               XmlDocument xmlReader = new XmlDocument();
               xmlReader.LoadXml(XmlInfo);

               XmlNodeList nodeList = null;

               nodeList = xmlReader.GetElementsByTagName(NodeName);
               nodeValue = (nodeList.Count != 0) ? nodeList[0].InnerText : "";
           }
           catch (Exception)
           {
               nodeValue = "";
           }

           return nodeValue;
       }

	   /// <summary>
	   /// 当前时间戳(标准格式时间字符串)
	   /// </summary>
        public static string TimeStamp
        {
            get
            {
                string strCurrentTime = "";
                string strTimeStamp = "";

				strCurrentTime = DateTime.Now.ToString("u").ToUpper();
				if ( strCurrentTime.IndexOf( "Z" ) >= 0 )
					strTimeStamp = strCurrentTime.Substring( 0 , strCurrentTime.IndexOf( "Z" ) );

				return strTimeStamp;
            }
        }

       /// <summary>
       /// 返回指定长度的随机数字串；
       /// 参数最大值：25
       /// </summary>
       /// <param name="pLen">指定长度；最大值：25</param>
       /// <returns></returns>
       public static string RandomStrOnFixLen(int pLen)
       {
           Random rdm = new Random(unchecked((int)DateTime.Now.Ticks));
           string strRdm = rdm.Next().ToString() + rdm.Next().ToString() + rdm.Next().ToString();
           strRdm = strRdm.Replace(".", "").PadRight(30,'0').Substring (0,pLen );
           return strRdm;
       }

	   /// <summary>
	   /// 判断字符串是否为空
	   /// </summary>
	   /// <param name="source"></param>
	   /// <returns></returns>
	   public static bool IsEmpty( string source )
	   {
		   bool isNull = true;

		   if ( source == null || source.Equals( null ) )
			   isNull = true;
		   else if ( source == "" || source.Trim() == "" )
			   isNull = true;
		   else
			   isNull = false;

		   return isNull;
	   }

	   /// <summary>
	   /// 判断字符串是否为数字
	   /// </summary>
	   /// <param name="InputStr"></param>
	   /// <returns></returns>
	   public static bool IsNumeric( string s )
	   {
		   bool result = false;

		   if ( CommonUtility.IsEmpty( s ) )
			   return false;

		   char[] tmp = s.ToCharArray();
		   foreach( char c in tmp )
		   {
			   switch ( c )
			   {
				   case '0':
					   result = true;
					   break;
				   case '1':
					   result = true;
					   break;
				   case '2':
					   result = true;
					   break;
				   case '3':
					   result = true;
					   break;
				   case '4':
					   result = true;
					   break;
				   case '5':
					   result = true;
					   break;
				   case '6':
					   result = true;
					   break;
				   case '7':
					   result = true;
					   break;
				   case '8':
					   result = true;
					   break;
				   case '9':
					   result = true;
					   break;
				   default:
					   return false;
			   }
		   }

		   return result;
	   }

       

	   /// <summary>
	   /// 四舍五入方法
	   /// </summary>
	   /// <param name="SourceNum"></param>
	   /// <param name="decimals"></param>
	   /// <returns></returns>
	   public static double Round( double SourceNum , int decimals )
	   {
		   double dResult = 0.0;

		   try
		   {
			   // 将原数字转换为字符串
			   string strSource = Convert.ToString( SourceNum );

			   // 小数点位置




			   int dotIndex = strSource.IndexOf( "." );
			   if ( dotIndex < 0 )
			   {
				   dResult = SourceNum;
				   return dResult;
			   }


			   // 整数部分
			   string strWholeNum = strSource.Substring( 0 , dotIndex );
			   // 小数部分
			   string strDecimal = strSource.Substring( dotIndex + 1 );


			   if ( decimals == 0 )
			   {
				   //  小数第一位的ASCII是否大于53?
				   if ( strDecimal[ 0 ] >= 53 )
					   dResult = Convert.ToDouble( strWholeNum ) + 1;
				   else
					   dResult = Convert.ToDouble( strWholeNum );
			   }
			   else
			   {
				   if ( strDecimal.Length > decimals )
				   {
					   // 要保留的小数部分
					   string strTmpDecimal = strDecimal.Substring( 0  , decimals );

					   //  要保留的小数部分的后一位的ASCII是否大于53?
					   if ( strDecimal[ decimals ] >= 53 )
						   dResult = Convert.ToDouble( strWholeNum + "." + Convert.ToString( Convert.ToInt32( strTmpDecimal ) + 1 ) );
					   else
						   dResult = Convert.ToDouble( strWholeNum + "." + strTmpDecimal );
				   }
				   else
					   dResult = Convert.ToDouble( strWholeNum + "." + strDecimal );
			   }
		   }
		   catch
		   {}

		   return dResult;
	   }



       /// <summary>
       /// 验证身份证号码
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
       public static bool CheckIDCard(string Id)
       {
           if (Id.Length == 18)
           {
               bool check = CheckIDCard18(Id);
               return check;
           }
           else if (Id.Length == 15)
           {
               bool check = CheckIDCard15(Id);
               return check;
           }
           else
           {
               return false;
           }
       }
       private static bool CheckIDCard18(string Id)
       {
           long n = 0;
           if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
           {
               return false;//数字验证
           }
           string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
           if (address.IndexOf(Id.Remove(2)) == -1)
           {
               return false;//省份验证
           }
           string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
           DateTime time = new DateTime();
           if (DateTime.TryParse(birth, out time) == false)
           {
               return false;//生日验证
           }
           string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
           string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
           char[] Ai = Id.Remove(17).ToCharArray();
           int sum = 0;
           for (int i = 0; i < 17; i++)
           {
               sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
           }
           int y = -1;
           Math.DivRem(sum, 11, out y);
           if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
           {
               return false;//校验码验证
           }
           return true;//符合GB11643-1999标准
       }
       private static bool CheckIDCard15(string Id)
       {
           long n = 0;
           if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
           {
               return false;//数字验证
           }
           string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
           if (address.IndexOf(Id.Remove(2)) == -1)
           {
               return false;//省份验证
           }
           string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
           DateTime time = new DateTime();
           if (DateTime.TryParse(birth, out time) == false)
           {
               return false;//生日验证
           }
           return true;//符合15位身份证标准
       }
       /// <summary>
       /// 根据身份证号获取生日
       /// </summary>
       /// <param name="IdCard"></param>
       /// <returns>
       public static string GetBrithdayFromIdCard(string IdCard)
       {
           string rtn = "1900-01-01";
           if (IdCard.Length == 15)
           {
               rtn = IdCard.Substring(6, 6).Insert(4, "-").Insert(2, "-");
           }
           else if (IdCard.Length == 18)
           {
               rtn = IdCard.Substring(6, 8).Insert(6, "-").Insert(4, "-");
           }
           return rtn;
       }
       /// <summary>
       /// 根据身份证获取性别
       /// </summary>
       /// <param name="IdCard"></param>
       /// <returns></returns>
       public static string GetSexFromIdCard(string IdCard)
       {
           string rtn;
           string tmp = "";
           if (IdCard.Length == 15)
           {
               tmp = IdCard.Substring(IdCard.Length - 3);
           }
           else if (IdCard.Length == 18)
           {
               tmp = IdCard.Substring(IdCard.Length - 4);
               tmp = tmp.Substring(0, 3);
           }
           int sx = int.Parse(tmp);
           int outNum;
           Math.DivRem(sx, 2, out outNum);
           if (outNum == 0)
           {
               rtn = "女";
           }
           else
           {
               rtn = "男";
           }
           return rtn;
       }


       public static Dictionary<String, String> splitParameters(string paraStr)
       {

           Dictionary<String, String> parameters = new Dictionary<string, string>();

           if (!String.Empty.Equals(paraStr))
           {
               string[] array = paraStr.Trim().Split('&');

               foreach (string temp in array)
               {
                   if (!String.Empty.Equals(temp))
                   {
                       string ttemp = temp.Trim();
                       int index = ttemp.IndexOf("=");
                       if (index > 0)
                       {
                           String key = ttemp.Substring(0, index);
                           String value = ttemp.Substring(index + 1);
                           if (String.Empty.Equals(key) || String.Empty.Equals(value)) continue;
                           parameters.Add(key.Trim(), value.Trim());
                       }
                   }
               }
           }

           return parameters;
       }



       /// <summary>
       /// 检查当前请求中是否包含指定参数
       /// </summary>
       /// <param name="ParameterName"></param>
       /// <param name="SpecificPage"></param>
       /// <returns></returns>
       public static bool IsFormParameterExist(string name, HttpRequest request)
       {
           bool isExist = false;
           string[] allKeys = request.Form.AllKeys;
           foreach (string key in allKeys)
           {
               if (key == name || key.Equals(name))
               {
                   isExist = true;
                   break;
               }
           }

           return isExist;
       }

       /// <summary>
       /// 检查当前请求中是否包含指定参数
       /// </summary>
       /// <param name="ParameterName"></param>
       /// <param name="SpecificPage"></param>
       /// <returns></returns>
       public static bool IsParameterExist(string Name, System.Web.UI.Page SpecificPage)
       {
           bool isExist = false;
           string[] allKeys = SpecificPage.Request.QueryString.AllKeys;
           foreach (string key in allKeys)
           {
               if (key == Name || key.Equals(Name))
               {
                   isExist = true;
                   break;
               }
           }

           return isExist;
       }

       /// <summary>
       /// 根据UserAccount获取用户省Code
       /// </summary>
       /// <param name="UserAccount"></param>
       /// <returns></returns>
       public static string GetProvinceCodeByUserAccount(string UserAccount)
       {
           string provinceCode = "";
           int position = -1;

           position = UserAccount.IndexOf("@");
           if (position < 0)
           {
               // 全国中心卡用户



               provinceCode = "ZX";
               return provinceCode;
           }

           // 有些省宽带帐号格式如下:XXXX@163.GD
           // 需要考虑对该种帐号的处理
           position = UserAccount.LastIndexOf("@");
           provinceCode = UserAccount.Substring(position + 1);

           position = provinceCode.IndexOf(".");
           if (position < 0)
           {
               provinceCode = "";
               return provinceCode;
           }

           provinceCode = provinceCode.Substring(0, position);
           provinceCode = provinceCode.ToUpper();

           // 全国中心卡用户



           if (provinceCode == "CHINAVNET")
           {
               provinceCode = "ZX";
           }

           return provinceCode;
       }

       #region 验证|邮箱、手机、电话、URL、验证码

       /// <summary>
       /// 用正则表达式验证
       /// 匹配返回true，不匹配返回false
       /// </summary>
       /// <param name="Sourse"></param>
       /// <param name="Regular"></param>
       /// <returns></returns>
       public static bool ValidateStr(string Sourse, string Regular)
       {
           if (null == Sourse)
           {
               return false;
           }
           return Regex.IsMatch(Sourse, Regular);
       }

       /// <summary>
       /// 验证邮箱
       /// </summary>
       /// <param name="Email"></param>
       /// <returns></returns>
       public static bool ValidateEmail(String Email)
       {
           String str = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
           return ValidateStr(Email, str);
       }

       /// <summary>
       /// 验证电话号码
       /// </summary>
       /// <param name="PhoneNum"></param>
       /// <returns></returns>
       public static bool ValidateTelephone(String PhoneNum)
       {
           String str = @"^(\d{3,4}\d{7,8})$";
           return ValidateStr(PhoneNum, str);
       }

       /// <summary>
       /// 验证手机号
       /// </summary>
       /// <param name="MobileNum"></param>
       /// <returns></returns>
       public static bool ValidateMobile(String MobileNum)
       {
           String str = @"^((13[0-9])|(15[^4,\D])|(18[0,5-9]))\d{8}$";
           return ValidateStr(MobileNum, str);
       }

       /// <summary>
       /// URL验证
       /// </summary>
       /// <param name="Url"></param>
       /// <returns></returns>
       public static bool ValidateUrl(String Url)
       {
           String str = @"^http(s?)\:\/\/";
           return ValidateStr(Url, str);
       }

       /// <summary>
       /// 验证码有效时间
       /// </summary>
       public static int ValidatorAvailableMinute
       {
           get
           {
               try
               {
                   return int.Parse(ConfigurationManager.AppSettings["ValidatorAvailableMinute"]);
               }
               catch
               {
                   return 5;
               }
           }
       }

       /// <summary>
       /// 验证验证码是否正确
       /// 作者：周涛        时间：2009-08-31
       /// </summary>
       /// <returns></returns>
       public static bool ValidateValidateCode(string Code, HttpContext context)
       {
           bool result = false;

           try
           {
               string validStr = HttpUtility.HtmlDecode(Code);
               validStr = CryptographyUtil.Encrypt(validStr);
               if (context.Request.Cookies["PASSPORT_USER_VALIDATOR"] == null)
               {
                   result = false;
               }
               else
               {
                   if (context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"] == null || context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"] == null)
                   {
                       result = false;
                   }
                   if (validStr == context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"].ToString())
                   {
                       if (DateTime.Now > DateTime.Parse(CryptographyUtil.Decrypt(context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"].ToString())))
                           result = false;
                       else
                           result = true;
                   }
                   else
                   {
                       result = false;
                   }
               }
           }
           catch
           {
               result = false;
           }

           return result;
       }

       #endregion

    }

    /// <summary>
    /// 关于Xml的处理
    /// </summary>
    public class VNetXml
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VNetXml()
        {

        }

        /// <summary>
        /// 得到一个元素中指定名称的值
        /// </summary>
        /// <param name="Node">元素</param>
        /// <param name="TagName">节点名</param>
        /// <returns>返回节点值</returns>
        public static string GetElementsTextByTagName(XmlElement Node, string TagName)
        {
            string Text = "";

            XmlNodeList elemList = Node.GetElementsByTagName(TagName);

            if (elemList.Count > 0)
            {
                Text = elemList[0].InnerText;
            }
            return Text;
        }
        /// <summary>
        /// 向一个结点添加一个子结点,并返回该子结点
        /// </summary>
        /// <param name="XMLDoc">XML文档对象</param>
        /// <param name="Node">指定的结点</param>
        /// <param name="ChildName">子结点名称</param>
        /// <param name="ChildText">子结点内容</param>
        /// <returns>刚添加的子结点</returns>
        public static XmlElement XMLAppendChild(XmlDocument XmlDoc, XmlNode Node, String ChildName, String ChildText)
        {
            XmlElement obj;
            obj = XmlDoc.CreateElement(ChildName);
            obj.InnerXml = ChildText;
            Node.AppendChild(obj);
            return obj;
        }





















        /// <summary>
        /// 向一个结点设置一个子结点,并返回该子结点，有此节点，则设置 没有此节点，则添加
        /// </summary>
        /// <param name="XMLDoc">XML文档对象</param>
        /// <param name="Node">指定的结点</param>
        /// <param name="ChildName">子结点名称</param>
        /// <param name="ChildText">子结点内容</param>
        /// <returns>新设置的子结点</returns>
        public static XmlElement XMLSetChild(XmlDocument XmlDoc, XmlNode Node, String ChildName, String ChildText)
        {
            XmlElement obj;

            if (Node.HasChildNodes)
            {
                for (int i = 0; i < Node.ChildNodes.Count; i++)
                {
                    if (ChildName == Node.ChildNodes[i].Name)
                    {
                        Node.ChildNodes[i].InnerXml = ChildText;
                        obj = (XmlElement)(Node.ChildNodes[i]);
                        return obj;
                    }
                }
            }

            obj = XmlDoc.CreateElement(ChildName);
            obj.InnerXml = ChildText;
            Node.AppendChild(obj);
            return obj;
        }

    }
    

    







    //EndClass
}

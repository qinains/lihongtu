/*********************************************************************************************************
 *   源文件: PageUtility.cs
 *     描述: 页面相关工具类 - 主要应用于接口子系统,用于检查请求参数、Cookie等数据是否存在
 *   所属包: Microsoft.DVAP.Interface.Utility
 * 开发平台: Windows XP + Microsoft SQL Server 2000
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2002
 *     作者: 高科
 * 联系方式: gaok@lianchuang.com
 *	   公司: 联创科技(南京)股份有限公司
 * 创建日期: 2005-04-25
 *********************************************************************************************************/
using System;
using System.Web;
using System.Configuration;

namespace Linkage.BestTone.Interface.Utility
{
	/// <summary>
	/// Summary description for PageUtility.
	/// </summary>
	public class PageUtility
	{
		public PageUtility()
		{
		}


		/// <summary>
		/// 设置Cookie
		/// </summary>
		/// <param name="UserTokenValue"></param>
		/// <param name="CookieName"></param>
		/// <param name="SpecificPage"></param>
		public static void SetCookie( string CookieValue , string CookieName , System.Web.UI.Page SpecificPage )
		{
			HttpCookie userCookie = new HttpCookie( CookieName );
			string domain =ConfigurationSettings.AppSettings["CIPDomain"];
			userCookie.Domain = domain;
			userCookie.Value = CookieValue;
			SpecificPage.Response.SetCookie( userCookie );
		}

        /// <summary>
        /// 添加cookie值
        /// @cookieName：cookie名称
        /// @cookieValue：cookie值
        /// @expireTime：过期时间(小时)
        /// </summary>
        public static void SetCookie(String cookieName, String cookieValue)
        {
            //先移除对应的cookie项目，如果不存在不会引发一场
            SetCookie(cookieName, cookieValue, 0);
        }

        /// <summary>
        /// 添加cookie值
        /// @cookieName：cookie名称
        /// @cookieValue：cookie值
        /// @expireTime：过期时间(小时)
        /// </summary>
        public static void SetCookie(String cookieName, String cookieValue, double expireTime)
        {
            //先移除对应的cookie项目，如果不存在不会引发一场
            HttpContext.Current.Response.Cookies.Remove(cookieName);

            HttpCookie cookie = new HttpCookie(cookieName);
            string domain = ConfigurationSettings.AppSettings["CIPDomain"];
            cookie.Domain = domain;
            cookie.Value = cookieValue;
            if (expireTime > 0)
            {
                cookie.Expires = DateTime.Now.AddHours(expireTime);
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取cookie值
        /// @cookieName：cookie名称
        /// </summary>
        public static String GetCookie(String cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
                return cookie.Value;
            else
                return null;
        }

		/// <summary>
		/// 注销Cookie
		/// </summary>
		/// <param name="CookieName"></param>
		/// <param name="SpecificPage"></param>
		public static void ExpireCookie( string CookieName , System.Web.UI.Page SpecificPage )
		{
			if ( PageUtility.IsCookieExist( CookieName , SpecificPage ) )
			{
				HttpCookie userCookie = new HttpCookie( CookieName );
                string domain = ConfigurationSettings.AppSettings["CIPDomain"];
				userCookie.Domain = domain;
				userCookie.Value = "";
				userCookie.Expires = DateTime.Now.AddSeconds( -1 );
				SpecificPage.Response.SetCookie( userCookie );
			}
		}

		private const string ProvinceCode = "ZX";	

		/// <summary>
		/// 拼接返回地址
		/// </summary>
		/// <param name="Url"></param>
		/// <param name="ResponseName"></param>
		/// <param name="ResponseValue"></param>
		/// <returns></returns>
		public static string GetRedirectUrl( string Url , string ResponseName , string ResponseValue )
		{		
			string HttpUrl = "";

			if( Url.IndexOf( '?' ) < 0 )
				HttpUrl = Url + "?Source=chinavnet&" + ResponseName + "=" + ResponseValue;
			else
				HttpUrl = Url + "&Source=chinavnet&" + ResponseName + "=" + ResponseValue;

			return HttpUrl;	
		}

		/// <summary>
		/// 拼接返回地址
		/// </summary>
		/// <param name="Url"></param>
		/// <param name="ResponseName"></param>
		/// <param name="ResponseValue"></param>
		/// <returns></returns>
		public static string GetRedirectUrlForPro( string Url , string ResponseName , string ResponseValue )
		{		
			string HttpUrl = "";

			if( Url.IndexOf( '?' ) < 0 )
				HttpUrl = Url + "?" + ResponseName + "=" + ResponseValue;
			else
				HttpUrl = Url + "&" + ResponseName + "=" + ResponseValue;

			return HttpUrl;	
		}


		public static string GetTransitionPageUrl( string ResponseName , string ResponseValue )
		{
			return "PayResult.aspx?ParaName=" + ResponseName + "&ParaValue=" + ResponseValue;
		}
		
		public static string GetTransitionPageUrlForPro( string ResponseName , string ResponseValue )
		{
			return "PayResultForPro.aspx?ParaName=" + ResponseName + "&ParaValue=" + ResponseValue;
		}

		/// <summary>
		/// 根据Request获取网页所属的省中心的Code
		/// </summary>
		/// <param name="Request"></param>
		/// <returns></returns>
		public static string GetProvinceCode( HttpRequest Request )
		{
			string ProvinceURLTemplate = ConfigurationSettings.AppSettings["ProvinceURLTemplate"];

			string SPRequestURL = Request.Url.AbsoluteUri;
			int Index = ProvinceURLTemplate.IndexOf( "**" );
			if( Index < 0 )
			{
				return  ProvinceCode;
			}
			if( SPRequestURL.Length < ( Index + 2 ) )
			{
				return ProvinceCode;
			}

			string PCode = SPRequestURL.Substring( Index , 2 );
			if( CommonUtility.IsEmpty( PCode ) || PCode.Length != 2 )
			{
				return ProvinceCode;
			}
			else
			{
				return PCode.ToUpper();
			}
		}

		/// <summary>
		/// 检查当前请求中是否包含指定参数
		/// </summary>
		/// <param name="ParameterName"></param>
		/// <param name="SpecificPage"></param>
		/// <returns></returns>
		public static bool IsParameterExist( string Name , System.Web.UI.Page SpecificPage )
		{
			bool isExist = false;
			string[] allKeys = SpecificPage.Request.QueryString.AllKeys;
			foreach( string key in allKeys )
			{
				if ( key == Name || key.Equals( Name ) )
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
		public static bool IsFormParameterExist( string Name , System.Web.UI.Page SpecificPage )
		{
			bool isExist = false;
			string[] allKeys = SpecificPage.Request.Form.AllKeys;
			foreach( string key in allKeys )
			{
				if ( key == Name || key.Equals( Name ) )
				{
					isExist = true;
					break;
				}
			}

			return isExist;
		}

		/// <summary>
		/// 检查当前请求中是否包含指定Cookie
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="SpecificPage"></param>
		/// <returns></returns>
		public static bool IsCookieExist( string Name , System.Web.UI.Page SpecificPage )
		{
			bool isExist = false;
			string[] allKeys = SpecificPage.Request.Cookies.AllKeys;
			foreach( string key in allKeys )
			{
				if ( key == Name || key.Equals( Name ) )
				{
					isExist = true;
					break;
				}
			}

			return isExist;
		}

		/// <summary>
		/// 检查当前请求中是否包含指定Cookie
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="SpecificContext"></param>
		/// <returns></returns>
		public static bool IsCookieExist( string Name , System.Web.HttpContext SpecificContext )
		{
			bool isExist = false;
			string[] allKeys = SpecificContext.Request.Cookies.AllKeys;
			foreach( string key in allKeys )
			{
				if ( key == Name || key.Equals( Name ) )
				{
					isExist = true;
					break;
				}
			}

			return isExist;
		}

		/// <summary>
		/// 生成登录页面的验证码
		/// </summary>
		private static string[] CharsArray = {"a","b","c","d","e","f","g","h","i","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z"};
		public static string GetRandomString()
		{
			string[] chars = GetRandomChar( 2 );
			string nums = GenerateRandomNumber( 4 );
			string sChar = "";
			int ichars = 0;
			for ( int i = 0 ; i < 2 ; i++ )
			{
				if ( chars[ i ] != "" )
				{
					sChar += chars[ i ];
					ichars ++;
				}
			}
			if ( ichars > 0 )
			{
				int[] iPoss = new int[ ichars ];
				iPoss = GetRandomNumber( ichars , 0 , 3 , 15151515 );
				for ( int j = 0 ; j < iPoss.Length ; j++ )
				{
					nums = nums.Substring( 0 , iPoss[ j ] )
						+ sChar.Substring( j , 1 ) 
						+ nums.Substring( iPoss[ j ] + 1 , nums.Length - iPoss[ j ] - 1 );
				}
			}
			nums = nums.Replace( "o" , "x" );
			nums = nums.Replace( "0" , "5" );
			return nums;
		}

		#region
		/*
		 * 目标：生成数字和字符混杂的4位字符串
		 * 步骤：
		 * 1.先从0-36中随机取出2个数字，这2个数中有几个小于25的数就从字符数组取几个字符
		 * (字符数cn<= 2)
		 * 2.从0-255中随机取出4-cn个数字，取每个数字的个位数
		 * 3.从0-3随机取出cn个数字，用来存放字符
		 * 4.数字倒序存放在剩下的位置
		 */

		//Use GetRandomNumber，Seed：17171717
		private static string[] GetRandomChar( int length )
		{
			int[] iRandom = new int[ length ];
			string[] letters = new string[ length ];
			iRandom = GetRandomNumber( length , 0 , 36 , 17171717 );
			for ( int i = 0 ; i < length ; i++ )
			{
				if ( iRandom[ i ] < 24 )
					letters[ i ] = CharsArray[ iRandom[ i ] ];
				else
					letters[ i ] = "";
			}
			return letters;
		}


		/*
		 * use the ticks of now to divide with seed and get the mod 
		 * then use the mod as the first seed to get the first random number,
		 * then use the first random number mutiply the first seed to get the second random number
		 */
		private static int[] GetRandomNumber( int length , int minValue , int maxValue , int seed )
		{
			Random rnd = new Random( unchecked( ( int ) DateTime.Now.Ticks % seed ) );
			int[] iResult = new int[ length ];
			int iFirst = rnd.Next( minValue , maxValue );
			for ( int i = 0 ; i < length ; i++ )
			{
				iResult[ i ] = iFirst;
				rnd = new Random( unchecked ( ( int ) ( iFirst * DateTime.Now.Ticks % seed ) ) );
				iFirst = rnd.Next( minValue , maxValue );
			}
			return iResult;
		}


		//use the RNGCryptoServiceProvider class to get a random bytes array of assigned length
		private static byte[] GetRandByte( int length )
		{
			System.Security.Cryptography.RNGCryptoServiceProvider rndGenerator = new System.Security.Cryptography.RNGCryptoServiceProvider();
			byte[] btResult = new byte[ length ];
			rndGenerator.GetBytes( btResult );
			return btResult;
		}


		//Use GetRandomNumber，Seed：13131313,打乱顺序后随机抽取
		private static string GenerateRandomNumber( int numLength )
		{
			int length = 20;
			string[] strNums = new string[ length ];
			byte[] bArray = new byte[ length ];
			string sNum = "";

			//1.get a rand byte array
			bArray = GetRandByte( length );
			//2.Convert the byte-type number array to string array
			for ( int i = 0 ; i < bArray.Length ; i++ )
			{
				strNums[ i ] = bArray[i].ToString();
				strNums[ i ] = strNums[i].PadLeft( 2 , '0' );
			}

			//3.Get a serial random number between 0 and 99 with assigned length,
			//then change string array sequence by random number
			int[] ichange = new int[ length ];
			ichange = GetRandomNumber( length , 0 , 99 , 13131313 );
			string tmp = "";
			int itmp ;
			for ( int ii = 0 ; ii < strNums.Length ; ii++ )
			{
				itmp = ichange[ ii ] % length;
				tmp = strNums[ itmp ];
				strNums[ itmp ] = strNums[ length - 1 - itmp ];
				strNums[ length - 1 - itmp ] = tmp;
			}
			
			//4.Get 2 random number between 0 and 99,
			//then use a temp variant "imid" to get the result of random-number mod with length,
			//use a temp string to join the two member of the string array
			//that is the very thing we want,It"s ok now !:)
			int[] iOut = new int[ numLength ];
			iOut = GetRandomNumber( numLength , 0 , 99 , 13131313 );
			int imid;
			for ( int jj = 0 ; jj < iOut.Length ; jj++ )
			{
				imid = iOut[ jj ] % length;
				sNum = strNums[ imid ].Substring( strNums[ imid ].Length - 1 , 1 ) + sNum;
			}

			return sNum;
		}
		#endregion
	}
}

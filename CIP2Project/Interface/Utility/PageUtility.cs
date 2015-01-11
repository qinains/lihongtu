/*********************************************************************************************************
 *   Դ�ļ�: PageUtility.cs
 *     ����: ҳ����ع����� - ��ҪӦ���ڽӿ���ϵͳ,���ڼ�����������Cookie�������Ƿ����
 *   ������: Microsoft.DVAP.Interface.Utility
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2000
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2002
 *     ����: �߿�
 * ��ϵ��ʽ: gaok@lianchuang.com
 *	   ��˾: �����Ƽ�(�Ͼ�)�ɷ����޹�˾
 * ��������: 2005-04-25
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
		/// ����Cookie
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
        /// ���cookieֵ
        /// @cookieName��cookie����
        /// @cookieValue��cookieֵ
        /// @expireTime������ʱ��(Сʱ)
        /// </summary>
        public static void SetCookie(String cookieName, String cookieValue)
        {
            //���Ƴ���Ӧ��cookie��Ŀ����������ڲ�������һ��
            SetCookie(cookieName, cookieValue, 0);
        }

        /// <summary>
        /// ���cookieֵ
        /// @cookieName��cookie����
        /// @cookieValue��cookieֵ
        /// @expireTime������ʱ��(Сʱ)
        /// </summary>
        public static void SetCookie(String cookieName, String cookieValue, double expireTime)
        {
            //���Ƴ���Ӧ��cookie��Ŀ����������ڲ�������һ��
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
        /// ��ȡcookieֵ
        /// @cookieName��cookie����
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
		/// ע��Cookie
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
		/// ƴ�ӷ��ص�ַ
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
		/// ƴ�ӷ��ص�ַ
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
		/// ����Request��ȡ��ҳ������ʡ���ĵ�Code
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
		/// ��鵱ǰ�������Ƿ����ָ������
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
		/// ��鵱ǰ�������Ƿ����ָ������
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
		/// ��鵱ǰ�������Ƿ����ָ��Cookie
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
		/// ��鵱ǰ�������Ƿ����ָ��Cookie
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
		/// ���ɵ�¼ҳ�����֤��
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
		 * Ŀ�꣺�������ֺ��ַ����ӵ�4λ�ַ���
		 * ���裺
		 * 1.�ȴ�0-36�����ȡ��2�����֣���2�������м���С��25�����ʹ��ַ�����ȡ�����ַ�
		 * (�ַ���cn<= 2)
		 * 2.��0-255�����ȡ��4-cn�����֣�ȡÿ�����ֵĸ�λ��
		 * 3.��0-3���ȡ��cn�����֣���������ַ�
		 * 4.���ֵ�������ʣ�µ�λ��
		 */

		//Use GetRandomNumber��Seed��17171717
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


		//Use GetRandomNumber��Seed��13131313,����˳��������ȡ
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

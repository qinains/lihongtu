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
		/// 检查当前请求中是否包含指定参数
		/// </summary>
		/// <param name="ParameterName"></param>
		/// <param name="SpecificPage"></param>
		/// <returns></returns>
//		public static bool IsFormParameterExist(string name, HttpRequest request)
//		{
//			bool isExist = false;
//			string[] allKeys = request.Form.AllKeys;
//			foreach (string key in allKeys)
//			{
//				if (key == name || key.Equals(name))
//				{
//					isExist = true;
//					break;
//				}
//			}
//
//			return isExist;
//		}


		/// <summary>
		/// 检查当前请求中是否包含指定参数
		/// </summary>
		/// <param name="ParameterName"></param>
		/// <param name="SpecificPage"></param>
		/// <returns></returns>
//		public static bool IsParameterExist(string Name, System.Web.UI.Page SpecificPage)
//		{
//			bool isExist = false;
//			string[] allKeys = SpecificPage.Request.QueryString.AllKeys;
//			foreach (string key in allKeys)
//			{
//				if (key == Name || key.Equals(Name))
//				{
//					isExist = true;
//					break;
//				}
//			}
//
//			return isExist;
//		}

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



		/// <summary>
		/// 用正则表达式验证
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
			return !Regex.IsMatch(Sourse, Regular);
		}
	}// End Class

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

	}//EndClass
}

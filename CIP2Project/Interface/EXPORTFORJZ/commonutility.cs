//==============================================================================================================
//
// Class Name: CommonUtility
// Description: �ṩ���õļ򵥹��÷���


// Author: Է��
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
		/// ��xml�ַ�����ȡֵ
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
		/// ��ǰʱ���(��׼��ʽʱ���ַ���)
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
		/// ����ָ�����ȵ�������ִ���

		/// �������ֵ��25
		/// </summary>
		/// <param name="pLen">ָ�����ȣ����ֵ��25</param>
		/// <returns></returns>
		public static string RandomStrOnFixLen(int pLen)
		{
			Random rdm = new Random(unchecked((int)DateTime.Now.Ticks));
			string strRdm = rdm.Next().ToString() + rdm.Next().ToString() + rdm.Next().ToString();
			strRdm = strRdm.Replace(".", "").PadRight(30,'0').Substring (0,pLen );
			return strRdm;
		}

		/// <summary>
		/// �ж��ַ����Ƿ�Ϊ��

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
		/// �ж��ַ����Ƿ�Ϊ����
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
		/// �������뷽��
		/// </summary>
		/// <param name="SourceNum"></param>
		/// <param name="decimals"></param>
		/// <returns></returns>
		public static double Round( double SourceNum , int decimals )
		{
			double dResult = 0.0;

			try
			{
				// ��ԭ����ת��Ϊ�ַ���
				string strSource = Convert.ToString( SourceNum );

				// С����λ��


				int dotIndex = strSource.IndexOf( "." );
				if ( dotIndex < 0 )
				{
					dResult = SourceNum;
					return dResult;
				}


				// ��������
				string strWholeNum = strSource.Substring( 0 , dotIndex );
				// С������
				string strDecimal = strSource.Substring( dotIndex + 1 );


				if ( decimals == 0 )
				{
					//  С����һλ��ASCII�Ƿ����53?
					if ( strDecimal[ 0 ] >= 53 )
						dResult = Convert.ToDouble( strWholeNum ) + 1;
					else
						dResult = Convert.ToDouble( strWholeNum );
				}
				else
				{
					if ( strDecimal.Length > decimals )
					{
						// Ҫ������С������
						string strTmpDecimal = strDecimal.Substring( 0  , decimals );

						//  Ҫ������С�����ֵĺ�һλ��ASCII�Ƿ����53?
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
		/// ��鵱ǰ�������Ƿ����ָ������
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
		/// ��鵱ǰ�������Ƿ����ָ������
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
		/// ����UserAccount��ȡ�û�ʡCode
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
				// ȫ�����Ŀ��û�

				provinceCode = "ZX";
				return provinceCode;
			}

			// ��Щʡ����ʺŸ�ʽ����:XXXX@163.GD
			// ��Ҫ���ǶԸ����ʺŵĴ���
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

			// ȫ�����Ŀ��û�

			if (provinceCode == "CHINAVNET")
			{
				provinceCode = "ZX";
			}

			return provinceCode;
		}



		/// <summary>
		/// ��������ʽ��֤
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
	/// ����Xml�Ĵ���
	/// </summary>
	public class VNetXml
	{
		/// <summary>
		/// ���캯��
		/// </summary>
		public VNetXml()
		{
		}

		/// <summary>
		/// �õ�һ��Ԫ����ָ�����Ƶ�ֵ
		/// </summary>
		/// <param name="Node">Ԫ��</param>
		/// <param name="TagName">�ڵ���</param>
		/// <returns>���ؽڵ�ֵ</returns>
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
		/// ��һ��������һ���ӽ��,�����ظ��ӽ��
		/// </summary>
		/// <param name="XMLDoc">XML�ĵ�����</param>
		/// <param name="Node">ָ���Ľ��</param>
		/// <param name="ChildName">�ӽ������</param>
		/// <param name="ChildText">�ӽ������</param>
		/// <returns>����ӵ��ӽ��</returns>
		public static XmlElement XMLAppendChild(XmlDocument XmlDoc, XmlNode Node, String ChildName, String ChildText)
		{
			XmlElement obj;
			obj = XmlDoc.CreateElement(ChildName);
			obj.InnerXml = ChildText;
			Node.AppendChild(obj);
			return obj;
		}
		/// <summary>
		/// ��һ���������һ���ӽ��,�����ظ��ӽ�㣬�д˽ڵ㣬������ û�д˽ڵ㣬�����
		/// </summary>
		/// <param name="XMLDoc">XML�ĵ�����</param>
		/// <param name="Node">ָ���Ľ��</param>
		/// <param name="ChildName">�ӽ������</param>
		/// <param name="ChildText">�ӽ������</param>
		/// <returns>�����õ��ӽ��</returns>
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

using System;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using  Proxy.UnifyInterfaceForUCenter;
using Linkage.BestTone.Interface.Utility;

namespace exportforjz
{
	/// <summary>
	/// Class1 的摘要说明。
	/// </summary>
	class Class1
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: 在此处添加代码以启动应用程序
			//


			int t=Read();
			Console.WriteLine(t.ToString());
			StreamWriter sw = new StreamWriter(File.OpenWrite(@"d:\zjlog081016.txt"),Encoding.Default);
			sw.BaseStream.Seek(0,SeekOrigin.End);
			sw.WriteLine("Total Rows = "+t.ToString());
			sw.Close();
			Console.ReadLine();
		}


		public static int Read()
		{
			int count=0;
			char[] ch={'\t'};
			

			int r=-1;
			
			StreamWriter sw = new StreamWriter(File.OpenWrite(@"d:\zjlog081016.txt"),Encoding.Default);
			sw.BaseStream.Seek(0,SeekOrigin.Begin);
			

			StreamReader sr = new StreamReader(File.OpenRead("zj081016.txt"),Encoding.Default);
			sr.BaseStream.Seek(0,SeekOrigin.Begin);
			while(sr.Peek()>-1)
			{
				StringBuilder sb = new StringBuilder();

				string line =sr.ReadLine();
				string[] arr = line.Split(ch);			
				r = CustInfoNotify(arr[0].Trim(),arr[1].Trim(),"","0");
				sb.Append(arr[0]+"\t");
				sb.Append(arr[1]+"\t");
				sb.Append(r.ToString()+"\r\n");
				sw.Write(sb.ToString());
				
				Console.Write(sb.ToString());
				count++;	
				line="";
			
			}
			sr.Close();
			sw.Close();
			return count;
		}

		public static int  CustInfoNotify(string CustID, string UserAccount, string ExtendField, string DealType)
		{
			int Result =-2;
			string ErrMsg = "";

			DataSet ds = null;
			SqlCommand cmd = new SqlCommand();

			try
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "up_BT_V2_Interface_QueryCustInfo";

				SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
				parCustID.Value = CustID;
				cmd.Parameters.Add(parCustID);

				SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
				parUserAccount.Value = UserAccount;
				cmd.Parameters.Add(parUserAccount);

				ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);


				string CustInfoXML = "";
				XmlDocument xmldoc;
				XmlNode xmlnode;
				XmlElement xmlelem;
				XmlElement xmlelem2 =null;
				XmlElement xmlelem3;
				XmlText xmltext;
				xmldoc = new XmlDocument();
				//加入XML的声明段落
				xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
				xmldoc.AppendChild(xmlnode);
				//加入一个根元素
				xmlelem = xmldoc.CreateElement("", "ROOT", "");
				xmldoc.AppendChild(xmlelem);

				//加入另外一个元素
				xmlelem2 = xmldoc.CreateElement("BasicUserInfo");
				xmlelem2 = xmldoc.CreateElement("", "BasicUserInfo", "");

				xmlelem3 = xmldoc.CreateElement("", "birthday", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["Birthday"].ToString());
				xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);

				xmlelem3 = xmldoc.CreateElement("", "cardClass", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CustLevel"].ToString());
				xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);

				xmlelem3 = xmldoc.CreateElement("", "certificateCode", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CertificateCode"].ToString());
				xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "certificateType", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CertificateType"].ToString());
				xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "credit", "");
				xmltext = xmldoc.CreateTextNode("");
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "custContactTel", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CustContactTel"].ToString());
				xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "custID", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CustID"].ToString());
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "custType", "");
				string custType = "1"; 
				//客户类型， 1：个人客户，2：政企客户
				if (UserAccount.Substring(3, 1) == "1" || UserAccount.Substring(3, 1) == "0" )
				{
					custType = "2";
				}

				
				xmltext = xmldoc.CreateTextNode(custType);
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "enterpriseID", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["EnterpriseID"].ToString());
				xmltext.InnerText = CommonUtility.IsEmpty(xmltext.InnerText) ? "" : xmltext.InnerText;
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "extendField", "");
				xmltext = xmldoc.CreateTextNode("");
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "realName", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["RealName"].ToString());
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "registration", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["RegistrationDate"].ToString());
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);


				xmlelem3 = xmldoc.CreateElement("", "sex", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["Sex"].ToString());
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);

				xmlelem3 = xmldoc.CreateElement("", "status", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["Status"].ToString());
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);

				xmlelem3 = xmldoc.CreateElement("", "uProvinceID", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["UProvinceID"].ToString());
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);

				xmlelem3 = xmldoc.CreateElement("", "userAccount", "");
				if(custType=="2")
					xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["CustID"].ToString());
				else
					xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["UserAccount"].ToString());

				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);

				xmlelem3 = xmldoc.CreateElement("", "userType", "");
				xmltext = xmldoc.CreateTextNode(ds.Tables[0].Rows[0]["UserType"].ToString());
				xmlelem3.AppendChild(xmltext);
				xmlelem2.AppendChild(xmlelem3);
				xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);

				//保存创建好的XML文档

				// xmldoc.Save(@".\BasicUserInfo.xml");
				CustInfoXML = xmldoc.OuterXml;
				CustInfoXML = CustInfoXML.Substring(CustInfoXML.IndexOf("<ROOT>"));
				CustInfoXML = @"<?xml version='1.0' encoding='gb2312'?>" + CustInfoXML;



				Proxy.UnifyInterfaceForUCenter.UnifyInterfaceForUCenter obj = new UnifyInterfaceForUCenter();
				//BaiscUserInfo CustInfoObj = new BaiscUserInfo();
				//CustInfoObj.birthday = ds.Tables[0].Rows[0]["Birthday"].ToString();
				//CustInfoObj.cardClass = ds.Tables[0].Rows[0]["CustLevel"].ToString();
				//CustInfoObj.certificateCode = ds.Tables[0].Rows[0]["CertificateCode"].ToString();
				//CustInfoObj.certificateType = ds.Tables[0].Rows[0]["CertificateType"].ToString();
				//CustInfoObj.credit = "";
				//CustInfoObj.custContactTel = ds.Tables[0].Rows[0]["CustContactTel"].ToString();
				//CustInfoObj.custId = ds.Tables[0].Rows[0]["CustID"].ToString();
				//CustInfoObj.custLevel = ds.Tables[0].Rows[0]["CustLevel"].ToString();
				//CustInfoObj.enterpriseId = ds.Tables[0].Rows[0]["EnterpriseID"].ToString();
				//CustInfoObj.extendField = "";
				//CustInfoObj.realName = ds.Tables[0].Rows[0]["RealName"].ToString();
				//CustInfoObj.registration = ds.Tables[0].Rows[0]["RegistrationDate"].ToString();
				//CustInfoObj.sex = ds.Tables[0].Rows[0]["Sex"].ToString();
				//CustInfoObj.status = ds.Tables[0].Rows[0]["Status"].ToString();
				//CustInfoObj.UProvinceID = ds.Tables[0].Rows[0]["UProvinceID"].ToString();
				//CustInfoObj.userAccount = ds.Tables[0].Rows[0]["UserAccount"].ToString();
				//CustInfoObj.userType = ds.Tables[0].Rows[0]["UserType"].ToString();
				//string sd = CustInfoObj.

				//obj.Url = ConfigurationManager.AppSettings["UnifyInterUrl"];
				obj.Url="http://bksvc.besttone.com.cn/WebService/services";
				string strResult = obj.newCardCustomerInfoExport("35000000", CustInfoXML, DealType);

				XmlDocument xmlObj = new XmlDocument();
				xmlObj.LoadXml(strResult);
				Result = int.Parse(xmlObj.GetElementsByTagName("result")[0].InnerText);
				ErrMsg = xmlObj.GetElementsByTagName("errorDescription")[0].InnerText;
				//Result = ResultObj.Result;
				//ErrMsg = ResultObj.ErrorDescription;

			}
			catch (Exception ex)
			{
				ErrMsg="errr:"+ex.Message.ToString();
				Result = -1;
			}
			finally
			{
				
				cmd = new SqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "up_BT_V2_Interface_InsertCustInfoNotifyFailRecord";

				SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
				parCustID.Value = CustID;
				cmd.Parameters.Add(parCustID);

				SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
				parUserAccount.Value = UserAccount;
				cmd.Parameters.Add(parUserAccount);

				SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
				parResult.Value = Result;
				cmd.Parameters.Add(parResult);

				SqlParameter parErrMsg = new SqlParameter("@ErrMsg", SqlDbType.VarChar, 256);
				parErrMsg.Value = ErrMsg;
				cmd.Parameters.Add(parErrMsg);

				DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
			
			}

			return Result;

		}

	}
}

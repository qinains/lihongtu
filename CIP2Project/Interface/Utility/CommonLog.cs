//==============================================================================================================
//
// Class Name: CommonLog
// Description: �ṩ���������ݱ����txt�ļ��Ĺ���
// Author: Է��
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-04
//
//==============================================================================================================
using System;
using System.IO;
using System.Text;

namespace Linkage.BestTone.Interface.Utility
{
	/// <summary>
	///  
	/// </summary>
	public class CommonLog
	{
		private static CommonLog comLog = null;
		private UTF8Encoding Utf8 = new UTF8Encoding();

       
       

		/// <summary>
		/// ���캯��
		/// </summary>
		private CommonLog()
		{

		}

		/// <summary>
		/// ����CommonLogʵ��
		/// </summary>
		/// <returns>����CommonLog����</returns>
		public static CommonLog getInstance()
		{
			if ( comLog == null )
				comLog = new CommonLog();
			return comLog;
		}
		
        /// <summary>
        /// ��������ļ�
        /// </summary>
        private FileStream CreateOrOpenFile(string dPath, string fName)
		{
			if ( dPath == "" || dPath == null || dPath.Trim() == "" ||
				fName == "" || fName == null || fName.Trim() == "" )
				return null;

			//Create Directory;
			try
			{
				if ( !Directory.Exists(dPath) )
					Directory.CreateDirectory(dPath);
			}
			catch(Exception Ex)
			{
				throw Ex;
			}
			
			//Create Output Stream Object;
			FileStream writer = null;
			try
			{
				writer = new FileStream(dPath+"\\"+fName,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.ReadWrite);
			}
			catch(Exception Ex)
			{
				throw Ex;
			}
				
			return writer;
		}

		/// <summary>
		/// д����
		/// </summary>
		/// <param name="writer">FileStream����</param>
		/// <param name="data">����</param>
		/// <returns>true-�ɹ�;false-ʧ��</returns>
		private bool Save(FileStream writer, string data)
		{
			if ( writer == null || writer.Equals(null) )
				return false;
			byte[] b = null;
			long len = 0;
			//b = Utf8.GetBytes(data);
            Encoding gbk = Encoding.GetEncoding("GBK");
            b = gbk.GetBytes(data);
			len = writer.Length;
			try
			{
				writer.Lock(0,len);
				writer.Seek(0,SeekOrigin.End);
				writer.Write(b,0,b.Length);
				writer.Unlock(0,len);
				writer.Flush();
			}
			catch(IOException e)
			{
				throw e;
			}
			catch(Exception Ex)
			{
				throw Ex;
			}
			finally
			{
				try
				{
					writer.Close();
				}
				catch(Exception Ex)
				{
					throw Ex;
				}
			}

			return true;
		}

		/// <summary>
		/// �������ݵ��ļ�
		/// </summary>
		/// <param name="dPath">Ŀ¼·��</param>
		/// <param name="fName">�ļ���</param>
		/// <param name="sData">����</param>
		/// <returns>true-�ɹ�;false-ʧ��</returns>
		public bool SaveToLog(string dPath, string fName, string sData)
		{
			lock(this)
			{
				if ( this.Save(this.CreateOrOpenFile(dPath,fName),sData) )
					return true;
			}

			return false;
		}
	}// End Class
}

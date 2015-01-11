//==============================================================================================================
//
// Class Name: CommonLog
// Description: 提供将请求数据保存成txt文件的功能
// Author: 苑峰
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
		/// 构造函数
		/// </summary>
		private CommonLog()
		{

		}

		/// <summary>
		/// 创建CommonLog实例
		/// </summary>
		/// <returns>返回CommonLog对象</returns>
		public static CommonLog getInstance()
		{
			if ( comLog == null )
				comLog = new CommonLog();
			return comLog;
		}
		
        /// <summary>
        /// 创建或打开文件
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
		/// 写数据
		/// </summary>
		/// <param name="writer">FileStream对象</param>
		/// <param name="data">数据</param>
		/// <returns>true-成功;false-失败</returns>
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
		/// 保存数据到文件
		/// </summary>
		/// <param name="dPath">目录路径</param>
		/// <param name="fName">文件名</param>
		/// <param name="sData">数据</param>
		/// <returns>true-成功;false-失败</returns>
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

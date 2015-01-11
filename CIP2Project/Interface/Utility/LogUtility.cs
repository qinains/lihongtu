//==============================================================================================================
//
// Class Name: LogUtility
// Description: 提供写文本日志方法
// Author: 苑峰
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-013
//
//==============================================================================================================
using System;
using System.Text;
using System.Diagnostics;
using System.Configuration;

namespace Linkage.BestTone.Interface.Utility
{
	/// <summary>
    /// 文本日志管理类
	/// </summary>
	public class BTUCenterInterfaceLog
	{
        public BTUCenterInterfaceLog()
		{

		}

		/// <summary>
		/// 其他日志
		/// </summary>
        public static void CenterForBizTourLog(string InterfaceName, StringBuilder Msg)
		{
			try
			{
				string logFileName = DateTime.Now.ToString( "yyyyMMdd" ) + "log.txt";

                // 指定日志路径
                string logPath = ConfigurationManager.AppSettings["CenterForBizTourLogPath"];
                if (logPath.EndsWith(@"\"))
                    logPath += @"CenterForBizTour\" + InterfaceName + @"\";
                else
                    logPath += @"\CenterForBizTour\" + InterfaceName + @"\";

                CommonLog.getInstance().SaveToLog(logPath, logFileName, Msg.ToString());
			}
			catch
			{ }
		}

        /// <summary>
        /// Crm交互日志
        /// </summary>
        public static void CenterForCRM(string InterfaceName, StringBuilder Msg)
        {
            try
            {
                string logFileName = DateTime.Now.ToString("yyyyMMdd") + "log.txt";

                // 指定日志路径
                string logPath = ConfigurationManager.AppSettings["CenterForBizTourLogPath"];
                if (logPath.EndsWith(@"\"))
                    logPath += @"CenterForCRM\" + InterfaceName + @"\";
                else
                    logPath += @"\CenterForCRM\" + InterfaceName + @"\";

                CommonLog.getInstance().SaveToLog(logPath, logFileName, Msg.ToString());
            }
            catch
            { }
        }


    }// End Class
}

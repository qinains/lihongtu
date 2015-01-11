//==============================================================================================================
//
// Class Name: LogUtility
// Description: �ṩд�ı���־����
// Author: Է��
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
    /// �ı���־������
	/// </summary>
	public class BTUCenterInterfaceLog
	{
        public BTUCenterInterfaceLog()
		{

		}

		/// <summary>
		/// ������־
		/// </summary>
        public static void CenterForBizTourLog(string InterfaceName, StringBuilder Msg)
		{
			try
			{
				string logFileName = DateTime.Now.ToString( "yyyyMMdd" ) + "log.txt";

                // ָ����־·��
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
        /// Crm������־
        /// </summary>
        public static void CenterForCRM(string InterfaceName, StringBuilder Msg)
        {
            try
            {
                string logFileName = DateTime.Now.ToString("yyyyMMdd") + "log.txt";

                // ָ����־·��
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

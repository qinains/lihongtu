/*********************************************************************************************************
 * 描述: 客户信息平台
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 李宏图
 * 联系方式: 
 *     公司: 号百信息服务有限公司
 * 创建日期: 2012-08-10
 *********************************************************************************************************/

using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Linkage.BestTone.Interface.Rule
{
    public class Logs
    {

        /// <summary>
        /// 日志发送记录处理
        /// </summary>
        /// <param name="detail">日志</param>
        public static void logSave(string detail)
        {

            StreamWriter sw = null;
            DateTime date = DateTime.Now;
            string FileName = date.Year + "-" + date.Month;
            try
            {
                #region 检测日志目录是否存在
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
                #endregion
              
                FileName = HttpContext.Current.Server.MapPath("~/Logs/") + FileName + "-qzone.txt";
                     
                #region 检测日志文件是否存在
                if (!File.Exists(FileName))
                    sw = File.CreateText(FileName);
                else
                {
                    sw = File.AppendText(FileName);
                }
                #endregion

                #region 向log文件中写数相关日志
                sw.WriteLine("IP        :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "\r");
                sw.WriteLine("Time      :" + date + "\r");
                sw.WriteLine("URL       :" + HttpContext.Current.Request.Url + "\r");
                sw.WriteLine("Details   :" + detail + "\r");
                sw.WriteLine("------------------------------------------------------------");
                //sw.WriteLine("");
                sw.Flush();
                #endregion
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
    }
}

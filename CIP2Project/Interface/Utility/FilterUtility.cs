using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text.RegularExpressions;

namespace Linkage.BestTone.Interface.Utility
{
    /// <summary>
    /// FilterHelper 的摘要说明
    /// </summary>
    public class FilterUtility
    {
        /// <summary>
        /// html标签
        /// </summary>
        private static string[] htmlRegex = {
            "<(html|head|body|form|title|link|base|meta|script|style)[^>]*>|</(html|head|body|form|title|link|base|meta|script|style)>",
            "<(iframe|frame|frameset|noframes)[^>]*>|</(iframe|frame|frameset|noframes)>",
            "<(div|p|span|center|br|hr)[^>]*>|</(div|p|span|center|br|hr)>",
            "<(table|tbody|tfoot|tr|td|th)[^>]*>|</(table|tbody|tfoot|tr|td|th)>",
            "<(dl|dt|dd|ul|li|ol)[^>]*>|</(dl|dt|dd|ul|li|ol)>",
            "<(label|input|button|select|option|textarea|img)[^>]*>|</(label|input|button|select|option|textarea|img)>",
            "<(font|strong|h)[^>]*>|</(font|strong|h)>",
            "<(.[^>]*)>|/>"
        };

        /// <summary>
        /// sql关键字
        /// </summary>
        private static string[] sqlRegex ={
            @"(select\s)|(insert\s)|(delete\s)|(update\s[\s\S].*\sset)",
            @"(create\s)|(drop\s)|(alter\s)",
            @"(\sand\s)|(\slike\s)",
            @"(exec\s)|(declare\s)|(truncate\s)|(\sbackup\s)|(xp_cmdshell\s)"
        };

        /// <summary>
        /// 特殊字符
        /// </summary>
        private static string[] specialRegex ={
            @"([\r\n])[\s]+|-->|<!--.*",
            @"<script[^>]*?>.*?</script>",
            @"href *= *[\s\S]*script *:"
        };

        private FilterUtility()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 检查是否有非法标签字符
        /// </summary>
        public static bool IsHasExceptionTags(String inputStr)
        {
            bool result = false;

            try
            {
                result = IsHasHtmlTagsException(inputStr);
                if (result)
                    return result;

                result = IsHasSpecialTagsException(inputStr);
                if (result)
                    return result;

                result = IsHasSQLTagsException(inputStr);
                if (result)
                    return result;
            }
            catch (Exception ex)
            { }

            return result;
        }

        /// <summary>
        /// 过滤非法标签字符
        /// </summary>
        public static String FilterExceptionTags(String inputStr)
        {
            String outputStr = String.Empty;
            if (String.IsNullOrEmpty(inputStr))
                return String.Empty;
            try
            {
                outputStr = inputStr;
                outputStr = FilterHtmlTags(outputStr);
                outputStr = FilterSpecialTags(outputStr);
                outputStr = FilterSQLTags(outputStr);
            }
            catch (Exception ex)
            { }

            return outputStr;
        }

        #region protected methods

        protected static bool IsHasHtmlTagsException(String inputStr)
        {
            bool result = false;

            foreach (String pattern in htmlRegex)
            {
                result = Regex.IsMatch(inputStr, pattern, RegexOptions.IgnoreCase);
                if (result)
                    return result;
            }

            return result;
        }
        protected static bool IsHasSpecialTagsException(String inputStr)
        {
            bool result = false;
            foreach (String pattern in specialRegex)
            {
                result = Regex.IsMatch(inputStr, pattern, RegexOptions.IgnoreCase);
                if (result)
                    return result;
            }

            return result;
        }
        protected static bool IsHasSQLTagsException(String inputStr)
        {
            bool result = false;

            foreach (String pattern in sqlRegex)
            {
                result = Regex.IsMatch(inputStr, pattern, RegexOptions.IgnoreCase);
                if (result)
                    return result;
            }

            return result;
        }

        /// <summary>
        /// 过滤一些html标签
        /// </summary>
        protected static String FilterHtmlTags(String inputStr)
        {
            String outputStr = String.Empty;
            if (String.IsNullOrEmpty(inputStr))
                return String.Empty;

            outputStr = inputStr;

            foreach (String pattern in htmlRegex)
            {
                outputStr = Regex.Replace(outputStr, pattern, "", RegexOptions.IgnoreCase);
            }

            return outputStr;
        }
        
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        protected static String FilterSpecialTags(String inputStr)
        {
            String outputStr = String.Empty;
            if (String.IsNullOrEmpty(inputStr))
                return String.Empty;

            outputStr = inputStr;

            foreach (String pattern in specialRegex)
            {
                outputStr = Regex.Replace(outputStr, pattern, "", RegexOptions.IgnoreCase);
            }

            return outputStr;
        }

        /// <summary>
        /// 过滤一些sql关键字
        /// </summary>
        protected static String FilterSQLTags(String inputStr)
        {
            String outputStr = String.Empty;
            if (String.IsNullOrEmpty(inputStr))
                return String.Empty;

            outputStr = inputStr;

            foreach (String pattern in sqlRegex)
            {
                outputStr = Regex.Replace(outputStr, pattern, "", RegexOptions.IgnoreCase);
            }

            return outputStr;
        }

        #endregion

    }
}

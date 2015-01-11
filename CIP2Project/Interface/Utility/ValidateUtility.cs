using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Configuration;
using System.Web;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Utility
{
    /// <summary>
    /// ������֤��
    /// </summary>
    public class ValidateUtility
    {
        /// <summary>
        /// ������ʽ��֤
        /// ƥ�䷵��true����ƥ�䷵��false
        /// </summary>
        public static bool ValidateByRegular(string Sourse, string Regular)
        {
            if (null == Sourse)
            {
                return false;
            }
            return Regex.IsMatch(Sourse, Regular);
        }

        /// <summary>
        /// ��֤����
        /// </summary>
        public static bool ValidateEmail(String Email)
        {
            String str = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return ValidateByRegular(Email, str);
        }

        /// <summary>
        /// ��֤�绰����
        /// </summary>
        public static bool ValidateTelephone(String PhoneNum)
        {
            String str = @"^(\d{3,4}\d{7,8})$";
            return ValidateByRegular(PhoneNum, str);
        }

        /// <summary>
        /// ��֤�ֻ���
        /// </summary>
        public static bool ValidateMobile(String MobileNum)
        {
            String str = @"^((13[0-9])|(15[^4,\D])|(18[0,5-9]))\d{8}$";
            return ValidateByRegular(MobileNum, str);
        }

        /// <summary>
        /// URL��֤
        /// </summary>
        public static bool ValidateUrl(String Url)
        {
            String str = @"^http(s?)\:\/\/";
            return ValidateByRegular(Url, str);
        }

        /// <summary>
        /// ��֤����Чʱ��
        /// </summary>
        public static int ValidatorAvailableMinute
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["ValidatorAvailableMinute"]);
                }
                catch
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// ��֤��֤���Ƿ���ȷ
        /// ���ߣ�����        ʱ�䣺2009-08-31
        /// </summary>
        public static bool ValidateValidateCode(string Code, HttpContext context)
        {
            bool result = false;

            try
            {
                string validStr = HttpUtility.HtmlDecode(Code);
                validStr = CryptographyUtil.Encrypt(validStr);
                if (context.Request.Cookies["PASSPORT_USER_VALIDATOR"] == null)
                {
                    result = false;
                }
                else
                {
                    if (context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"] == null || context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"] == null)
                    {
                        result = false;
                    }
                    if (validStr == context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"].ToString())
                    {
                        if (DateTime.Now > DateTime.Parse(CryptographyUtil.Decrypt(context.Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"].ToString())))
                            result = false;
                        else
                            result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

    }
}

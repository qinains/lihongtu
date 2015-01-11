//==============================================================================================================
//
// Class Name: Cryptography
// Description: 提供UPG、UPS需要的加密、解密功能


// Author: 苑峰
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-03-31
//
//==============================================================================================================
using System;
using System.Text;
using System.Security.Cryptography;

namespace Linkage.BestTone.Interface.Utility
{
    public class CryptographyUtil
    {
        private static readonly string HASHALGORITHM = "SHA1";
        private static readonly byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static readonly byte[] KEY ={ 218, 239, 227, 22, 31, 53, 120, 224, 223, 223, 171, 210, 140, 158, 47, 86, 122, 39, 238, 95, 47, 138, 44, 155 };
        private static byte[] BestPayIV = { 50, 51, 52, 53, 54, 55, 56, 57 };
        #region 验证码

        /// <summary>
        /// 生成验证码,算法:Base64(3DES(SHA1(内容)))
        /// </summary>
        public static string GenerateAuthenticator(string s, string key)
        {
            byte[] bKey = null;
            byte[] bHash = null;
            byte[] bEncrypt = null;
            string sEncrypt = "";

            try
            {
                // 1.SHA1加密
                bHash = Hash(s);
                // 2.3DES加密
                bKey = HexStringToByteArray(key);
                if (Encrypt(bKey, bHash, out bEncrypt)) {
                    sEncrypt = ToBase64String(bEncrypt);
                }
            }
            catch (Exception)
            {
                sEncrypt = "";
            }
          
            return sEncrypt;
        }

        /// <summary>
        /// 生成验证码,算法:Base64(3DES(SHA1(内容)))
        /// </summary>
        public static string GenerateAuthenticator(string s1, string s2, string s3, string s4, string s5,
            string s6, string s7, string s8, string s9, string s10, string s11, string s12, string s13,
            string s14, string s15, string s16, string s17, string s18, string s19, string s20, string key)
        {
            string authSource =
                s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9 + s10 + 
                s11 + s12 + s13 + s14 + s15 + s16 + s17 + s18 + s19 + s20;

            return GenerateAuthenticator(authSource, key);
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        public static bool ValidateAuthenticator(string s, string authenticator, string key)
        {
            bool isValid = false;

            try
            {
                // 1.生成新验证码
                string newAuthenticator = GenerateAuthenticator(s, key);

                // 2.比较新旧验证码
                if (newAuthenticator == authenticator || newAuthenticator.Equals(authenticator))
                    isValid = true;
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <returns></returns>
        public static bool ValidateAuthenticator(string s1, string s2, string s3, string s4, string s5, string s6, 
            string s7, string s8, string s9, string s10, string s11, string s12, string s13, string s14, string s15, 
            string s16, string s17, string s18, string s19, string s20, string authenticator, string key)
        {
            bool isValid = false;

            try
            {
                string authSource =
                    s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9 + s10 +
                    s11 + s12 + s13 + s14 + s15 + s16 + s17 + s18 + s19 + s20;

                // 1.生成新验证码
                string newAuthenticator = GenerateAuthenticator(authSource, key);

                // 2.比较新旧验证码

                if (newAuthenticator == authenticator || newAuthenticator.Equals(authenticator))
                    isValid = true;
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;
        }

        #endregion

        /// <summary>
        /// SHA1加密算法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] Hash( string s )
        {
            byte[] input = null, output = null;

            input = Encoding.UTF8.GetBytes(s);
            output = ((HashAlgorithm)CryptoConfig.CreateFromName(HASHALGORITHM)).ComputeHash(input);

            return output;
        }

        public static byte[] MD5Encrypt(String strSource)
        {
            byte[] input = null, output = null;
            input = Encoding.UTF8.GetBytes(strSource);
            output = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(input);

            return output;
        }


        //字节数组转字符串
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        #region Encrypt

        /// <summary>
        /// 3DES加密算法
        /// </summary>
        private static bool Encrypt( byte[] key, byte[] input, out byte[] output )
        {
            output = null;

            try
            {
                TripleDESCryptoServiceProvider trippleDesProvider = new TripleDESCryptoServiceProvider();
                ICryptoTransform encryptObj = trippleDesProvider.CreateEncryptor(key, IV);
                output = encryptObj.TransformFinalBlock(input, 0, input.Length);
                trippleDesProvider.Clear();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 3DES加密算法
        /// </summary>
        private static bool EncryptBestPay(byte[] key, byte[] input, out byte[] output)
        {
            output = null;

            try
            {
                TripleDESCryptoServiceProvider trippleDesProvider = new TripleDESCryptoServiceProvider();

                trippleDesProvider.Key = key;
                trippleDesProvider.IV = IV;
                trippleDesProvider.Mode = CipherMode.CBC;
                trippleDesProvider.Padding = PaddingMode.Zeros;
                ICryptoTransform encryptObj = trippleDesProvider.CreateEncryptor(key, IV);
                output = encryptObj.TransformFinalBlock(input, 0, input.Length);
                trippleDesProvider.Clear();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }



        public static string XXTeaEncrypt(String plainText, String appSecret)
        {
            byte[] msg = Encoding.UTF8.GetBytes(plainText);
            byte[] key = Encoding.UTF8.GetBytes(appSecret);
            return byteToHexStr(XXTEA.Encrypt(msg, key));
        }


        public static string XXTeaDecrypt(String cipherText, String appSecret)
        {
            byte[] encryptText = CryptographyUtil.HexStringToByteArray(cipherText);
            byte[] key = Encoding.UTF8.GetBytes(appSecret);
            return Encoding.UTF8.GetString(XXTEA.Decrypt(encryptText, key));
        }



        public static string HMAC_SHA1(String data,String appSecret)
        {
            System.Security.Cryptography.HMACSHA1 hmacsha1 = new System.Security.Cryptography.HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(appSecret);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return byteToHexStr(hashBytes); 
        }

        /// <summary>
        /// 3DES加密算法(Base64(Encrypt()))
        /// </summary>
        public static string EncryptBestPay(string source, string key)
        {
            string returnValue = "";

            byte[] input = null;
            byte[] output = null;
            byte[] bkey = null;

            try
            {
                input = Encoding.UTF8.GetBytes(source);
                bkey = CryptographyUtil.HexStringToByteArray(key);

                if (CryptographyUtil.EncryptBestPay(bkey, input, out output))
                {
                    returnValue = CryptographyUtil.ToBase64String(output);
                }
            }
            catch (Exception)
            {
                returnValue = "";
            }

            return returnValue;
        }


        public static string Encrypt3DES(string a_strString, string a_strKey)   
        {   
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = Encoding.UTF8.GetBytes(a_strKey);               
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = Encoding.UTF8.GetBytes(a_strString);   
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));   
        }



        public static string Decrypt3DES(string a_strString, string a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = Encoding.UTF8.GetBytes(a_strKey); 
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            string result = "";
            try
            {
                byte[] Buffer = Convert.FromBase64String(a_strString);
                result = Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {

            }
            return result;

        }


        /// <summary>
        /// 3DES加密算法(Base64(Encrypt()))
        /// </summary>
        public static string Encrypt(string source, string key)
        {
            string returnValue = "";

            byte[] input = null;
            byte[] output = null;
            byte[] bkey = null;

            try
            {
                input = Encoding.UTF8.GetBytes(source);
                bkey = CryptographyUtil.HexStringToByteArray(key);
                

                if (CryptographyUtil.Encrypt(bkey, input, out output))
                {
                    returnValue = CryptographyUtil.ToBase64String(output);
                }
            }
            catch (Exception)
            {
                returnValue = "";
            }

            return returnValue;
        }

        /// <summary>
        /// 3DES加密算法
        /// </summary>
        public static string Encrypt(string source)
        {
            string returnValue = "";

            byte[] input = null;
            byte[] output = null;

            try
            {
                input = Encoding.UTF8.GetBytes(source);
                if (CryptographyUtil.Encrypt(KEY, input, out output))
                {
                    returnValue = CryptographyUtil.ToBase64String(output);
                }
            }
            catch (Exception)
            {
                returnValue = "";
            }

            return returnValue;
        }

        #endregion

        #region Decrypt

        /// <summary>
        /// 3DES解密算法
        /// </summary>
        private static bool Decrypt(byte[] key, byte[] input, out byte[] output)
        {
            output = null;

            try
            {
                TripleDESCryptoServiceProvider trippleDesProvider = new TripleDESCryptoServiceProvider();
                ICryptoTransform decryptObj = trippleDesProvider.CreateDecryptor(key, IV);
                output = decryptObj.TransformFinalBlock(input, 0, input.Length);
                trippleDesProvider.Clear();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 3DES解密算法
        /// </summary>
        public static string Decrypt(string source, string key)
        {
            string returnValue = "";

            byte[] input = null;
            byte[] output = null;
            byte[] bkey = null;

            try
            {
                input = CryptographyUtil.FromBase64String(source);
                bkey = CryptographyUtil.HexStringToByteArray(key);

                if (CryptographyUtil.Decrypt(bkey, input, out output))
                {
                    returnValue = Encoding.UTF8.GetString(output);
                }
            }
            catch (Exception)
            {
                returnValue = "";
            }

            return returnValue;
        }

        /// <summary>
        /// 3DES解密算法
        /// </summary>
        public static string Decrypt(string source)
        {
            string returnValue = "";

            byte[] input = null;
            byte[] output = null;

            try
            {
                input = CryptographyUtil.FromBase64String(source);
                if (CryptographyUtil.Decrypt(KEY, input, out output))
                {
                    returnValue = Encoding.UTF8.GetString(output);
                }
            }
            catch (Exception ex )
            {
                returnValue = ex.Message;
                returnValue = "";
            }

            return returnValue;
        }

        #endregion

        /// <summary>
        /// Base64编码
        /// </summary>
        public static string ToBase64String( byte[] input )
        {
            return Convert.ToBase64String(input);
        }

        /// <summary>
        /// Base64反编码
        /// </summary>
        public static byte[] FromBase64String(string source)
        {
            return Convert.FromBase64String(source);
        }


        public static string GetASEkey(int length)
        {
            byte[] buffer = new byte[length / 2];
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            provider.GetBytes(buffer);
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", buffer[i]));
            }
            return builder.ToString();
        }


        /// <summary>
        /// 16进制字符串转换为byte[]
        /// </summary>
        private static byte[] HexStringToByteArray(string s)
        {
            Byte[] buf = new byte[s.Length / 2];
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(chr2hex(s.Substring(i * 2, 1)) * 0x10 + chr2hex(s.Substring(i * 2 + 1, 1)));
            }
            return buf;
        }
        private static byte chr2hex(string s)
        {
            switch (s)
            {
                case "0":
                    return 0x00;
                case "1":
                    return 0x01;
                case "2":
                    return 0x02;
                case "3":
                    return 0x03;
                case "4":
                    return 0x04;
                case "5":
                    return 0x05;
                case "6":
                    return 0x06;
                case "7":
                    return 0x07;
                case "8":
                    return 0x08;
                case "9":
                    return 0x09;
                case "A":
                    return 0x0a;
                case "B":
                    return 0x0b;
                case "C":
                    return 0x0c;
                case "D":
                    return 0x0d;
                case "E":
                    return 0x0e;
                case "F":
                    return 0x0f;
            }
            return 0x00;
        }




    }// End Class
}

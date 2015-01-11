using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Linkage.BestTone.Interface.Utility
{
    class DESede
    {
        private ICryptoTransform DESEncrypt;
        private ICryptoTransform DESDecrypt;
        public DESede(String a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = HexStringToByteArray(a_strKey);
      
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            //DES.Key = HexStringToByteArray(a_strKey);

            DESEncrypt = DES.CreateEncryptor();
            DESDecrypt = DES.CreateDecryptor();

        }

        public String encryptMode(String a_strString)
        {
            byte[] Buffer = HexStringToByteArray(a_strString); ;// Encoding.UTF8.GetBytes(a_strString);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));  
        }

        public String decryptMode(String a_strString)
        {
            byte[] Buffer = Convert.FromBase64String(a_strString);
            return  Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

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
    }
}

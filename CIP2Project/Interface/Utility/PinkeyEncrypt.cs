using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Utility

{
    public class PinkeyEncrypt
    {
        private DESede desede;
        public PinkeyEncrypt(String keyS)
        {
            try
            {
                keyS = "3134393836323037" + "3134393836323036" + "3134393836323035";
                this.desede = new DESede(keyS);
            }
            catch (System.Exception ex)
            {
                
            }
        }


        public PinkeyEncrypt()
        {
            try
            {
                String keyS = "3134393836323037";
                keyS = keyS + keyS + keyS;
                this.desede = new DESede(keyS);
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        public String encrypt(String pinkey, String cardOrAccountNo)
        {
            try
            {
                int Result = validParas(pinkey, cardOrAccountNo);
                if(Result == 0)
                {
                    String s = "0000" + truncAccountNo(cardOrAccountNo);
                    String fp = "06" + pinkey + "FFFFFFFF";
                    String xorFlatPin = xor(s, fp);

                    byte[] b0 = HexStringToByteArray(xorFlatPin);

                    byte[] b1 = System.Text.Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt3DES(System.Text.Encoding.UTF8.GetString(b0), pinkey));

                    return ByteArrayToHexString(b1).ToUpper().Substring(0, 16);
                }else{

                    return null;
                }


            }
            catch (System.Exception ex)
            {
                return null;
            }
        }


        private string ByteArrayToHexString(byte[] bits)
        {
            string str = BitConverter.ToString(bits).Replace("-", "");

            return str;


        }

        private int validParas(String pinkey,String cardOrAccountNo)
        {
            int Result = 0;
            if( (!CommonUtility.IsNumeric(pinkey)) || (pinkey.Length!=6) )
            {
                // 密码格式错误 
                Result = -1;
                return Result;
            }

            if (CommonUtility.IsNumeric(cardOrAccountNo) && cardOrAccountNo.Length==16)
            {
                Result = 0;
            }
            return Result;
            
        }

        private String truncAccountNo(String accountNo)
        {
            return accountNo.Substring(accountNo.Length - 13,
                    accountNo.Length - 1);
        }


        private String xor(String accountNo, String flatTxnPasswd)
        {
            byte[] buf1 = System.Text.Encoding.UTF8.GetBytes(accountNo);
            byte[] buf2 = System.Text.Encoding.UTF8.GetBytes(flatTxnPasswd);
            byte[] buf3 = new byte[16];
            int n = 0;
            for (int i = 0; i < 16; i += 2)
            {
                int b = buf1[i] - 48 << 4 | buf1[(i + 1)] - 48;
                int c = 0;
                if ((buf2[i] > 57) && (buf2[(i + 1)] > 57))
                    c = buf2[i] - 55 << 4 | buf2[(i + 1)] - 55;
                else if ((buf2[i] > 57) && (buf2[(i + 1)] < 57))
                    c = buf2[i] - 55 << 4 | buf2[(i + 1)] - 48;
                else if ((buf2[i] < 57) && (buf2[(i + 1)] > 57))
                    c = buf2[i] - 48 << 4 | buf2[(i + 1)] - 55;
                else
                {
                    c = buf2[i] - 48 << 4 | buf2[(i + 1)] - 48;
                }
                int x = b ^ c;
                int y = x;
                int left = (x >> 4) + 48;
                if (left > 57)
                {
                    left += 7;
                }
                int right = (y & 0xF) + 48;
                if (right > 57)
                {
                    right += 7;
                }
                buf3[(n++)] = (byte)left;
                buf3[(n++)] = (byte)right;
            }
            return System.Text.Encoding.UTF8.GetString(buf3);
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

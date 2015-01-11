using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Utility
{
    public class HMACSHA196
    {

        private const String name = "hmac-sha1-96";
        private const int bsize = 12;
        private System.Security.Cryptography.HMAC mentalis_mac;
        private System.Security.Cryptography.CryptoStream cs;
        //private Mac mac;
        public int getBlockSize() { return bsize; }
        public void init(byte[] key)
        {
            if (key.Length > 20)
            {
                byte[] tmp = new byte[20];
                Array.Copy(key, 0, tmp, 0, 20);
                key = tmp;
            }
            //    SecretKeySpec skey=new SecretKeySpec(key, "HmacSHA1");
            //    mac=Mac.getInstance("HmacSHA1");
            //    mac.init(skey);
            mentalis_mac = new System.Security.Cryptography.HMACMD5(key);
            cs = new System.Security.Cryptography.CryptoStream(System.IO.Stream.Null, mentalis_mac, System.Security.Cryptography.CryptoStreamMode.Write);
        }
        private byte[] tmp = new byte[4];
        public void update(int i)
        {
            tmp[0] = (byte)(i >> 24);
            tmp[1] = (byte)(i >> 16);
            tmp[2] = (byte)(i >> 8);
            tmp[3] = (byte)i;
            update(tmp, 0, 4);
        }
        public void update(byte[] foo, int s, int l)
        {
            //mac.update(foo, s, l);  
            cs.Write(foo, s, l);
        }
        private byte[] buf = new byte[12];
        public byte[] doFinal()
        {
            //    System.arraycopy(mac.doFinal(), 0, buf, 0, 12);
            //    return buf;
            cs.Close();
            Array.Copy(mentalis_mac.Hash, 0, buf, 0, 12);
            return buf;
        }
        public String getName()
        {
            return name;
        }
    }
}

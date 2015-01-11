using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Utility
{
    //http://sharpssh2.codeplex.com/SourceControl/changeset/view/37212#SharpSSH/SharpSSH/Crypto/MAC/HMACSHA1.cs
    public class HMACSHA1:MAC
    {
        private const String name = "hmac-sha1";
        private const int bsize = 20;
        private System.Security.Cryptography.HMAC mentalis_mac;
        private System.Security.Cryptography.CryptoStream cs;
        //private Mac mac;
        public int getBlockSize() { return bsize; }

        public void init(byte[] key)
        {
            if (key.Length > bsize)
            {
                byte[] tmp = new byte[bsize];
                Array.Copy(key, 0, tmp, 0, bsize);
                key = tmp;
            }
            //SecretKeySpec skey = new SecretKeySpec(key, "HmacSHA1");
            //mac = Mac.getInstance("HmacSHA1");
            //mac.init(skey);
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

        public byte[] doFinal()
        {
            Console.WriteLine("Sha1");
            //return mac.doFinal();
            cs.Close();
            byte[] result = mentalis_mac.Hash;
            byte[] key = mentalis_mac.Key;
            mentalis_mac.Clear();
            init(key);

            return result;
        }

        public String getName()
        {
            return name;
        }

    }
}

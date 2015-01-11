using UnifyPlatform.utils.Cryptography;

static void Main(string[] args)
{
	//xxtea º”√‹
  	String plaintext = "123456123456";
            	String key = "07bHrYkaFYBD4Bxrhdbm9KhnT5jmvFho";
            	String encrypt = XXTEA.XXteaEncrypt(plaintext,key);
            	Console.WriteLine(encrypt);
	
	//Ω‚√‹
	String decrypt = XXTEA.XXteaDecrypt(encrypt,key);
	Console.WriteLine(decrypt);

	//hmac_sha1«©√˚
      	Console.WriteLine(HMACSHA1.sign("test685AFB90888D059D1BE1234874AA6004C2D61D8040181F948428839502C43DF6E2A508B0FE9851D81575744B4EBA421D0E5B42ABB29D8BE3D949C2B4723C0AD3A0AD665606BA9F9292AD9839E86EDF5AEBEA2121CB6C685C5B0ADB62", "cn21"));
            	Console.Read();

}
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;
using Linkage.BestTone.Interface.Rule;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections;
using System.Xml;

using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Net;
using System.Timers;
using System.Runtime.InteropServices;
using log4net;
using System.Reflection;
using System.Text.RegularExpressions;
using UnifyPlatform.utils.Cryptography;


[assembly: log4net.Config.DOMConfigurator(ConfigFile = @"D:\logs\log4net\log4net.config", Watch = true)] 
//[assembly: log4net.Config.XmlConfigurator()]
namespace ConsoleApp
{

    class Program
    {
        private static readonly byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static readonly string HASHALGORITHM = "SHA1";
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog log = LogManager.GetLogger("File");

        [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExA")]
        public static extern int GetDiskFreeSpaceEx(string lpRootPathName, out   long lpFreeBytesAvailable, out   long lpTotalNumberOfBytes, out   long lpTotalNumberOfFreeBytes);   
        static void testLog4net()
        {
            Console.WriteLine("======");
            log.Info("asdfsdfsdfsdfsdf");
            log.Error("sdfsdfsdffs");
            Console.WriteLine("======");
        }


        /// 是否为日期型字符串  
        /// </summary>  
        /// <param name="StrSource">日期字符串(2008-05-08)</param>  
        /// <returns></returns>  
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]" +
                                                            @"|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|" +
                                                            @"1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?" +
                                                            @"2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468]" +
                                                            @"[048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        public static void HashStr()
        {

            //string newHC = CryptographyUtil.GenerateAuthenticator("35433333$122089282$sfM68NoFz6CnE6cliOJtj98OEOtq1eLS41SX0p2cbxnIA96fR8DZv/Af3MfL2BwJAOsv22T817I=", "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534");
            //Console.WriteLine(newHC);
            //Console.WriteLine(newHC);
            //Console.WriteLine(newHC);
            String abc = Encoding.UTF8.GetString(Hasha("35433333$122089282$sfM68NoFz6CnE6cliOJtj98OEOtq1eLS41SX0p2cbxnIA96fR8DZv/Af3MfL2BwJAOsv22T817I="));
            Console.WriteLine(abc);
            Console.Read();
        }
        public static byte[] Hash(string s)
        {
            byte[] input = null, output = null;

            input = Encoding.UTF8.GetBytes(s);
            output = ((HashAlgorithm)CryptoConfig.CreateFromName(HASHALGORITHM)).ComputeHash(input);

            return output;
        }

        public static void jiemi()
        {
            //i+rAdlXXTzL7BasX6O8CCvDnUQtAI3Ku
            try
            {
                String result = CryptographyUtil.Decrypt("i+rAdlXXTzL7BasX6O8CCvDnUQtAI3Ku", "F05F7D4722AE7C3A61ADE9475A89928BA623BF99E96E4F0");
                Console.WriteLine(result);
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Read();

            }
            
        }

        public static void destest()
        {
            String Inputsource = "123456789";
            //byte[] testInputSource = Encoding.UTF8.GetBytes(Inputsource);
            String keysource = "F05F7D4722AE7C3A61ADE9475A89928BA623BF99E96E4F0";
            //byte[] testkey = Encoding.UTF8.GetBytes(keysource);

            //75A89928BA623BF99E96E4F0";
            byte[] outtype = null;

            // 1.SHA1加密
            byte[] bHash = Hash(Inputsource);
            // 2.3DES加密
            byte[] bKey = HexStringToByteArray(keysource);

            Encrypt(bKey, bHash, out  outtype);

            //Console.WriteLine(Encoding.UTF8.GetString(outtype));
            //Console.WriteLine(Encoding.UTF8.GetString(outtype));
            //Console.WriteLine(Encoding.UTF8.GetString(outtype));

            //string newHC = CryptographyUtil.GenerateAuthenticator("123456789", keysource);

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


        /// <summary>
        /// SHA1加密算法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] Hasha(string s)
        {
            byte[] input = null, output = null;

            input = Encoding.UTF8.GetBytes(s);
            output = ((HashAlgorithm)CryptoConfig.CreateFromName(HASHALGORITHM)).ComputeHash(input);

            return output;
        }

            /// <summary>
        /// 3DES加密算法
        /// </summary>
        public  static bool Encrypt( byte[] key, byte[] input, out byte[] output )
        {
            output = null;

            try
            {
                TripleDESCryptoServiceProvider trippleDesProvider = new TripleDESCryptoServiceProvider();
                ICryptoTransform encryptObj = trippleDesProvider.CreateEncryptor(key, IV);
                output = encryptObj.TransformFinalBlock(input, 0, input.Length);
                trippleDesProvider.Clear();
                for (int i = 0; i < output.Length; i++)
                {
                    Console.WriteLine("{0:x}",output[i]);

                }

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        
     

      public static String sign(Dictionary<String, String> paramValues, String secret)  {
        
            StringBuilder sb = new StringBuilder();
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(paramValues);
            myList.Sort(delegate(KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
               {
                   return s2.Value.CompareTo(s1.Value);
               });
            sb.Append(secret);
            foreach (KeyValuePair<string, string> pair in myList)
            {
                sb.Append(pair.Key).Append(pair.Value);
            }
            sb.Append(secret);
            byte[] sha1Digest = getSHA1Digest(sb.ToString());  
            return byte2hex(sha1Digest);
       
    }    


        public static String OpenBesttoneAccountV2(String Request)
        {
            String ReturnCode = "";
            String Descriptioin = "";
            StringBuilder Response = new StringBuilder();
            Response.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            #region
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Request);
            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            String version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            String SPID = SPIDNode.Attributes["value"].Value;

            XmlNode CustIDNode = xmlDoc.SelectNodes("/root/param/CUSTID")[0];
            String CustID = CustIDNode.Attributes["value"].Value;

            String EC = xmlDoc.SelectNodes("/root/param/EC")[0].InnerText;
            String HC = xmlDoc.SelectNodes("/root/param/HC")[0].InnerText;
            String AuthenCode = xmlDoc.SelectNodes("/root/param/AuthenCode")[0].InnerText;

            #endregion

            int Result = 0;
            String ErrMsg = "";

            StringBuilder strLog = new StringBuilder();
            try
            {

                SPInfoManager spInfo = new SPInfoManager();
                //Object SPData = spInfo.GetSPData(this.Context, "SPData");
                string ScoreSystemSecret = "F05F7D4722AE7C34A61ADE9475A89928BA623BF99E96E4F0";  //spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

                string newHC = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + EC, ScoreSystemSecret);

                strLog.AppendFormat("HC:{0};newHC:{1}\r\n" , HC, newHC);
                string realName = "";
                string mobile = "";
                string contactTel = "";
                string email = "";
                string sex = "";
                string cerType = "";
                string cerNum = "";

                if (newHC.Equals(HC))
                {
                    try
                    {
                        string PlanTextStr = CryptographyUtil.Decrypt(EC.ToString(), ScoreSystemSecret);
                        strLog.AppendFormat("planTextStr:{0}\r\n", PlanTextStr);
                        string[] alSourceStr = PlanTextStr.Split('$');
                        realName = alSourceStr[0];
                        strLog.AppendFormat("realName:{0}\r\n", realName);
                        mobile = alSourceStr[1];
                        strLog.AppendFormat("mobile:{0}\r\n", mobile);
                        contactTel = alSourceStr[2];
                        strLog.AppendFormat("contactTel:{0}\r\n", contactTel);
                        email = alSourceStr[3];
                        strLog.AppendFormat("email:{0}\r\n", email);
                        sex = alSourceStr[4];
                        strLog.AppendFormat("sex:{0}\r\n", sex);
                        cerType = alSourceStr[5];
                        strLog.AppendFormat("cerType:{0}\r\n", cerType);
                        cerNum = alSourceStr[6];
                        strLog.AppendFormat("cerNum:{0}\r\n", cerNum);
                    }
                    catch (System.Exception e)
                    {
                        ReturnCode = "-7020";
                        Descriptioin = "解密错误" + e.ToString();
                        Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                        return Response.ToString();
                    }
                }
                else
                {
                    ReturnCode = "-7021";
                    Descriptioin = "hashcode校验不通过!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();
                }


                strLog.AppendFormat("Begin 开通号码百事通账户\r\n");
                strLog.AppendFormat("SPID:{0},CustID:{1},mobile:{2},realName:{3},contactTel:{4},email:{5},sex:{6},cerType:{7},cerNum:{8}\r\n", SPID, CustID, mobile, realName, contactTel, email, sex, cerType, cerNum);

                #region 数据合法性判断


                if (CommonUtility.IsEmpty(AuthenCode))
                {
                    ReturnCode = "-7014";
                    Descriptioin = "手机校验码不能为空!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();
                }

                //验证码校验
                Result = PhoneBO.SelSendSMSMassage(CustID, mobile, AuthenCode, out ErrMsg);
                if (Result != 0)
                {
                    ReturnCode = "-7014";
                    Descriptioin = "验证码验证失败：" + ErrMsg;
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }

                if (CommonUtility.IsEmpty(SPID))
                {
                    ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                    Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();
                }

              
         
                if (CommonUtility.IsEmpty(CustID))
                {
                    //Result.Result = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Code;
                    //Result.ErrMsg = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Msg;
                    //return Result;
                    ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_InValidCustID_Code);
                    Descriptioin = ErrorDefinition.BT_IError_Result_InValidCustID_Msg;
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();



                }
                if (CommonUtility.IsEmpty(mobile))
                {
                    //Result.Result = -7015;
                    //Result.ErrMsg = "账户名不能为空!";
                    //return Result;
                    ReturnCode = "-7015";
                    Descriptioin = "账户名不能为空!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }
                if (!Utils.isMobilePhone(mobile))
                {
                    //Result.Result = -7015;
                    //Result.ErrMsg = "无效的手机号码!";
                    //return Result;
                    ReturnCode = "-7016";
                    Descriptioin = "无效的手机号码!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }

                if (CommonUtility.IsEmpty(realName))
                {
                    //Result.Result = -7016;
                    //Result.ErrMsg = "用户名不能为空!";
                    //return Result;
                    ReturnCode = "-7017";
                    Descriptioin = "用户名不能为空!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }
                if (CommonUtility.IsEmpty(sex))
                {
                    //Result.Result = -7017;
                    //Result.ErrMsg = "性别不能为空!";
                    //return Result;
                    ReturnCode = "-7018";
                    Descriptioin = "性别不能为空!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }
                if (!Utils.IsNumeric(sex))
                {
                    //Result.Result = -7017;
                    //Result.ErrMsg = "性别只能为数字!";
                    //return Result;
                    ReturnCode = "-7019";
                    Descriptioin = "性别只能为数字!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }
                if ("0".Equals(sex) || "1".Equals(sex) || "2".Equals(sex))
                {
                }
                else
                {
                    //Result.Result = -7017;
                    //Result.ErrMsg = "性别只能为0和1,2!";
                    //return Result;
                    ReturnCode = "-7020";
                    Descriptioin = "性别只能为0和1,2!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }

                if (CommonUtility.IsEmpty(cerType))
                {
                    //Result.Result = -7018;
                    //Result.ErrMsg = "证件类型不能为空!";
                    //return Result;
                    ReturnCode = "-7021";
                    Descriptioin = "证件类型不能为空!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();
                }

                if (cerType.Equals("1") || cerType.Equals("2") || cerType.Equals("3") || cerType.Equals("4") || cerType.Equals("5") || cerType.Equals("6") || cerType.Equals("7") || cerType.Equals("8") || cerType.Equals("9") || cerType.Equals("10") || cerType.Equals("X"))
                { }
                else
                {
                    //Result.Result = -7018;
                    //Result.ErrMsg = "非法证件类型!";
                    //return Result;
                    ReturnCode = "-7022";
                    Descriptioin = "非法证件类型!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();

                }

                if (CommonUtility.IsEmpty(cerNum))
                {
                    //Result.Result = -7019;
                    //Result.ErrMsg = "证件号不能为空!";
                    //return Result;
                    ReturnCode = "-7023";
                    Descriptioin = "证件号不能为空!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    return Response.ToString();
                }


                if ("X".Equals(cerType))
                {
                    if ("99999".Equals(cerNum))
                    {
                    }
                    else
                    {
                        //Result.Result = -7019;
                        //Result.ErrMsg = "证件类型为其他类型(X)，则证件号必须是99999!";
                        //return Result;
                        ReturnCode = "-7024";
                        Descriptioin = "证件类型为其他类型(X)，则证件号必须是99999!";
                        Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                        return Response.ToString();
                    }
                }

                if ("1".Equals(cerType))
                {
                    if (!CommonUtility.CheckIDCard(cerNum))
                    {
                        //Result.Result = -7020;
                        //Result.ErrMsg = "身份证不合法!";
                        //return Result;
                        ReturnCode = "-7025";
                        Descriptioin = "身份证不合法!";
                        Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                        return Response.ToString();
                    }
                }

                #endregion

                String TransactionID = BesttoneAccountHelper.CreateTransactionID();
                BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
                strLog.AppendFormat("select * from besttoneaccount where custid={0}\r\n", CustID);
                BesttoneAccount besttoneAccountEntity = _besttoneAccount_dao.QueryByCustID(CustID);
                String ResponseCode = "";
                AccountItem ai = new AccountItem();
                if (besttoneAccountEntity == null)    // 未绑定
                {
                    strLog.AppendFormat("0 records return\r\n");
                    strLog.AppendFormat("未绑定\r\n");
                    int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(mobile, out ai, out ResponseCode, out ErrMsg);
                    strLog.AppendFormat("BesttoneAccountHelper.BesttoneAccountInfoQuery QueryBesttoneAccountResult:{0},ErrMsg:{1}\r\n ", QueryBesttoneAccountResult, ErrMsg);
                    if (QueryBesttoneAccountResult == 0)
                    {
                        if ("200010".Equals(ResponseCode))   // 未开户
                        {
                            strLog.AppendFormat("未绑定且未开户\r\n");
                            strLog.AppendFormat("准备去开户了\r\n");
                            strLog.AppendFormat("开户前日志,参数 SPID:{0},TransactionID:{1},CustID:{2},mobile:{3}", SPID, TransactionID, CustID, mobile);
                            UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  ErrMsg);  //日志
                            strLog.AppendFormat("开户前日志完成\r\n");
                            strLog.AppendFormat("开户......\r\n");
                            strLog.AppendFormat("开户参数:mobile:{0},realName:{1},contactTel:{2},email:{3},sex:{4},cerType:{5},cerNum:{6},TransactionID:{7}", mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID);
                            Result = BesttoneAccountHelper.RegisterBesttoneAccount(mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID, out ErrMsg);
                            strLog.AppendFormat("开户后返回的状态 Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
                            //绑定操作
                            strLog.AppendFormat("开完户准备进行绑定,将{0}绑定至{1}\r\n", mobile, CustID);
                            UserRegistry.CreateBesttoneAccount(SPID, CustID, mobile, out ErrMsg);
                            strLog.AppendFormat("绑定后结果ErrMsg:{0}\r\n", ErrMsg);
                            UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  ErrMsg); //日志
                            strLog.AppendFormat("开户后日志\r\n");
                            UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, cerNum, out ErrMsg);
                            strLog.AppendFormat("开户完成\r\n");
                        }
                        else
                        {  // 已开户
                            //绑定操作
                            if ("000000".Equals(ResponseCode))
                            {
                                strLog.AppendFormat("未绑定且 已开户\r\n");
                                strLog.AppendFormat("仅绑定.......\r\n");
                                UserRegistry.CreateBesttoneAccount(SPID, CustID, mobile, out ErrMsg);
                                strLog.AppendFormat("将{0}绑定到{1}\r\n", mobile, CustID);
                                UserRegistry.OnlyBindingBesttoneAccount(SPID, TransactionID, CustID, mobile, out  ErrMsg);
                                strLog.AppendFormat("记录绑定日志表,流水号:{0}", TransactionID);
                                strLog.AppendFormat("绑定后结果ErrMsg:{0}\r\n", ErrMsg);
                            }
                            else
                            {
                                //Result.Result = -25679;
                                //Result.ErrMsg = ResponseCode;
                                //return Result;
                                ReturnCode = "-7026";
                                Descriptioin = ResponseCode;
                                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                                return Response.ToString();
                            }
                        }
                    }
                    else
                    {
                        //Result.Result = QueryBesttoneAccountResult;
                        //return Result;
                        ReturnCode = "-7027";
                        Descriptioin = "查询账户出错!";
                        Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                        return Response.ToString();
                    }
                }
                else
                {
                    // 账户是否绑定到其他人身上  
                    strLog.AppendFormat("1 record return.\r\n");
                    strLog.AppendFormat("该CustID:{1}上已经有绑定的账户号{1}\r\n", CustID, mobile);
                    strLog.AppendFormat("检查改账户号{0}上是否绑定在别的CustID{1}上\r\n", mobile, CustID);
                    BesttoneAccount besttoneCunsInfo = _besttoneAccount_dao.QueryByBestAccount(mobile);
                    if (!besttoneCunsInfo.CustID.Equals(CustID))  // 绑定到了其他人身上
                    {
                        strLog.AppendFormat("{0}绑定到了其他人身上,此人的CustID:{1}\r\n", mobile, CustID);
                        Result = -12300;
                        ErrMsg = mobile + "手机号已为其他客户(" + CustID + ")开通了号码百事通账户，您可以登录系统，进入您的账户中心，用另一手机号码开通号码百事通账户，也可以咨询客服人员帮助排查问题。";
                    }
                    strLog.AppendFormat("{0}没有绑定到其他人身上,此人的CustID:{1}\r\n", mobile, CustID);
                    strLog.AppendFormat("去翼支付查询该账户号{0}是否存在\r\n", mobile);
                    int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(mobile, out ai, out ResponseCode, out ErrMsg);
                    if (QueryBesttoneAccountResult != 0)   // 未开户
                    {
                        strLog.AppendFormat("翼支付查询返回说该账户号{0}不存在\r\n", mobile);
                        strLog.AppendFormat("准备去为{0}开户........\r\n", mobile);
                        strLog.AppendFormat("开户前日志,参数 SPID:{0},TransactionID:{1},CustID:{2},mobile:{3}", SPID, TransactionID, CustID, mobile);
                        UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  ErrMsg);  //日志
                        strLog.AppendFormat("开户前日志完成\r\n");
                        strLog.AppendFormat("开户........\r\n");
                        strLog.AppendFormat("开户参数:mobile:{0},realName:{1},contactTel:{2},email:{3},sex:{4},cerType:{5},cerNum:{6},TransactionID:{7}\r\n", mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID);
                        Result = BesttoneAccountHelper.RegisterBesttoneAccount(mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID, out ErrMsg);
                        strLog.AppendFormat("开户完成，返回结果:Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
                        //绑定操作
                        strLog.AppendFormat("绑定{0}到{1}\r\n", mobile, CustID);
                        UserRegistry.CreateBesttoneAccount(SPID, CustID, mobile, out ErrMsg);
                        strLog.AppendFormat("绑定完成,返回结果 ErrMsg:{0}\r\n", ErrMsg);
                        strLog.AppendFormat("开户后日志\r\n");
                        UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  ErrMsg); //日志
                        strLog.AppendFormat("开户后日志完成 ErrMsg:{0}\r\n", ErrMsg);
                        UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, cerNum, out ErrMsg);
                    }
                }
                strLog.AppendFormat("End 开通号码百事通账户 Result:{0},ErrMsg{1}\r\n", Result, ErrMsg);
                //-99999 失败 0 成功
            }
            catch (Exception e)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code);
                ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + e.Message;
                Descriptioin = ErrMsg;
            }
            finally
            {
                
            }
            Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            return Response.ToString();
        }



        public static string byte2hex(byte[] bytes)
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



        public static byte[] getSHA1Digest(String data)
        {
            byte[] input = null, output = null;
            input = Encoding.UTF8.GetBytes(data);
            output = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(input);
            return output;
        }



        static int ParseSPTokenRequest(string SourceStr)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            string ErrMsg = "";
            string SPID = "";
            string CustID = "";
            string HeadFooter = "";
            string ReturnURL = "";
            string TimeStamp = "";
            string Digest = "";
            try
            {
                string[] alSourceStr = SourceStr.Split('$');
                SPID = alSourceStr[0].ToString();
                
                //SPInfoManager spInfo = new SPInfoManager();
                //Object SPData = spInfo.GetSPData(context, "SPData");
                //35000019    ->B985D1B0D0CF57B674C7F4754CB96C042D79B0E7EB57D537
                string ScoreSystemSecret = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";    //FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534     //B985D1B0D0CF57B674C7F4754CB96C042D79B0E7EB57D537   //225AC1A9A923D3830230AD1F1030334A832D68CF4D94FDCF
                //FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534
                string EncryptSourceStr = alSourceStr[1].ToString();
                string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
                string[] alRequest = RequestStr.Split('$');

                //加密顺序：URLEncoding(SPID + "$" + Base64(Encrypt(CustId + "$"  + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + Digest)))
                //Digest = Base64(Encrypt(Hash(CustId + "$"+ReturnURL +"$"+ HeadFooter "$"+TimeStamp)))
                CustID = alRequest[0].ToString();
                ReturnURL = alRequest[1].ToString();
                HeadFooter = alRequest[2].ToString();
                TimeStamp = alRequest[3].ToString();
                Digest = alRequest[4].ToString();
                //校验摘要 Digest 信息
                string NewDigest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + ReturnURL + "$" + HeadFooter + "$" + TimeStamp, ScoreSystemSecret);
                if (Digest != NewDigest)
                {
                    Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                    ErrMsg = "无效的Digest";
                    return Result;
                }

                Result = 0;
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return Result;
        }


        static void testTimer()
        {

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(TimeEvent);
            // 设置引发时间的时间间隔　此处设置为１秒（１０００毫秒）
            aTimer.AutoReset = true;
            aTimer.Interval = -1;
            aTimer.Enabled = true;
            aTimer.Start();        
        }

        static void TimeEvent(object source, ElapsedEventArgs e)
        {

            Console.WriteLine("hello!");
            Console.WriteLine("hello!");
            Console.WriteLine("hello!");
            Console.WriteLine("hello!");
           
        }

        static void testIp()
        {
            //IPAddress[] hostipspool = Dns.GetHostAddresses("");
            //Console.WriteLine(hostipspool[0].Address);
            //Console.WriteLine(hostipspool[0].Address);
            //Console.WriteLine(hostipspool[0].Address);
            //Console.WriteLine(hostipspool[0].Address);
            System.Net.IPHostEntry IpEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            //Console.WriteLine(IpEntry.AddressList[2].ToString());
            //Console.WriteLine(IpEntry.AddressList[2].ToString());
            //Console.WriteLine(IpEntry.AddressList[2].ToString());
            //Console.WriteLine(IpEntry.AddressList[2].ToString());
            String ipaddress = "";
            for (int i = 0; i != IpEntry.AddressList.Length; i++)
            {
                
                if (!IpEntry.AddressList[i].IsIPv6LinkLocal)
                {
                    ipaddress = IpEntry.AddressList[i].ToString();
                    //MessageBox.Show(IpEntry.AddressList[i].ToString());
                }
            }
            Console.WriteLine(ipaddress);
            Console.WriteLine(ipaddress);
            Console.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString());
            Console.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString());
            Console.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString());
        }

        static void bestpayencrypt()
        {
            BestPayEncryptService bpes = new BestPayEncryptService();
            string e_oldPassWord = "";
            string e_newPassWord = "";
            string e_confirmPassWord = "";
            string oldPassWord = "690123";
            string newPassWord = "690123";
            string confirmPassWord = "690123";
            string ErrMsg = "";
            AccountItem ai = new AccountItem();

            int QueryBesttoneAccountResult = BesttoneAccountHelper.QueryBesttoneAccount("18918790558", out ai, out ErrMsg);

            e_oldPassWord = bpes.encryptNoKey(oldPassWord, ai.AccountNo);
            e_newPassWord = bpes.encryptNoKey(newPassWord, ai.AccountNo);
            e_confirmPassWord = bpes.encryptNoKey(confirmPassWord, ai.AccountNo);

            Console.WriteLine(e_oldPassWord);
            Console.WriteLine(e_newPassWord);
            Console.WriteLine(e_confirmPassWord);
            Console.Read();

        }

        static void testExpr()
        {
            string regMobile = @"^1[3458]\d{9}$";
                string regEmail = @"^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$";
                string regCard = @"^(\d{9}|\d{16})$";
                Regex reg = new Regex(regEmail);
                if(reg.IsMatch("lihongtu@@besttone.com.cn"))
                {
                    Console.WriteLine("ok");
                    Console.Read();
                }else
                {
                    Console.WriteLine("not match");
                    Console.Read();
                }
        }

        public static Int32 VerifyCardNo(String CardNo, out String ErrMsg)
        {
            //号码百事通卡位数为16位，编码规则：前4位为8888，第5、6位为00，第7-15位为序列号，第16位为随机验证码。
            Int32 Result = 0;
            ErrMsg = "";
            Regex regCardNo = new Regex(@"888800\d{9}\w{1}$");
            if (!regCardNo.IsMatch(CardNo))
            {
                Result = -1;
                ErrMsg = "您输入的卡号不正确，请核对后再次输入，谢谢！";
            }
            return Result;
        }



        static void testJiaquanzhenXml()
        {
            StringBuilder xml = new StringBuilder();

            xml.Append("<SchraubErgebniss>");
            xml.Append("<Datei>");
            xml.Append("<Werkstueck>");
            xml.Append("<Block>");
            xml.Append("<AnzahlIst>1</AnzahlIst>");
            xml.Append("<Verschraubung>");
            xml.Append("<Nummer>0</Nummer>");
            xml.Append("</Verschraubung>");
            xml.Append("</Block>");
            xml.Append("<Block>");
            xml.Append("<AnzahlIst>2</AnzahlIst>");
            xml.Append("<Verschraubung>");
            xml.Append("<Nummer>1</Nummer>");
            xml.Append("</Verschraubung>");
            xml.Append("</Block>");
            xml.Append("</Werkstueck>");
            xml.Append("</Datei>");
            xml.Append("</SchraubErgebniss>");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.ToString());

            XmlNodeList datalist = xmlDoc.SelectNodes("/SchraubErgebniss/Datei/Werkstueck/Block");

            foreach (XmlNode node in datalist)
            {
                XmlNodeList vlist =  node.SelectNodes("Verschraubung");
          
                foreach (XmlNode vn in vlist)
                {
                    String Number = vn.SelectSingleNode("Nummer").InnerText;
                 
                }
            }

        }

        static void testKuohao()
        {
            //   Regex regCardNo = new Regex(@"888800\d{9}\w{1}$");
            String testStr = "[100]";
            Regex r = new Regex(@"\[\d*\]$");
            if (r.IsMatch(testStr))
            {
                testStr = testStr.Replace("[","");
                testStr = testStr.Replace("]","");
                //Console.WriteLine("yes");
            }
            else {
                Console.WriteLine("no");
            }

            Console.ReadLine();
        }

        static void testXml()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<root>");
            xml.Append("<callinfo>");
            xml.Append("<version value=\"1.0\"/>");
            xml.Append("<SPID value=\"35433333\"/>");
            xml.Append("</callinfo>");
            xml.Append("<srchcond>");
            xml.Append("<fromIndex value=\"12\"/>");
            xml.Append("<rowCount value=\"[20]\"/>");
            xml.Append("<conds>");
            xml.Append("<CustID value=\"号百客户ID\"/>");
            xml.Append("<BesttoneAccount value=\"号码百事通账户\"/>");
            xml.Append("<RechargeTransactionID value=\"充值流水号\"/>");
            xml.Append("<OrderSeq value=\"订单号\"/>");
            xml.Append("<fromDatetime value=\"2013-01-12\"/>");
            xml.Append("<toDatetime value=\"2013-02-23\"/>");
            xml.Append("<rechargeSRC value=\"1\"/>");
            xml.Append("<status value=\"2\"/>");
            xml.Append("<needInvoice value=\"1\"/>");
            xml.Append("</conds>");
            xml.Append("<sortFields>");
            xml.Append("<field value=\"fromDatetime\" desc=\"1\"/>");
            xml.Append("<field value=\"toDatetime\" desc=\"1\"/>");
            xml.Append("</sortFields>");
            xml.Append("</srchcond>");
            xml.Append("</root>");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.ToString());

            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            String version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            String SPID = SPIDNode.Attributes["value"].Value;

            XmlNode fromIndexNode = xmlDoc.SelectNodes("/root/srchcond/fromIndex")[0];
            String fromIndex = fromIndexNode.Attributes["value"].Value;

            XmlNode rowCountNode = xmlDoc.SelectNodes("/root/srchcond/rowCount")[0];
            String rowCount = rowCountNode.Attributes["value"].Value;

            XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CustID")[0];
            String CustID = CustIDNode.Attributes["value"].Value;

            XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
            String BesttoneAccount = BesttoneAccountNode.Attributes["value"].Value;

            XmlNode RechargeTransactionIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/RechargeTransactionID")[0];
            String RechargeTransactionID = RechargeTransactionIDNode.Attributes["value"].Value;

            XmlNode OrderSeqNode = xmlDoc.SelectNodes("/root/srchcond/conds/OrderSeq")[0];
            String OrderSeq = OrderSeqNode.Attributes["value"].Value;

            XmlNode fromDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/fromDatetime")[0];
            String fromDatetime = fromDatetimeNode.Attributes["value"].Value;

            XmlNode toDatetimeNode = xmlDoc.SelectNodes("/root/srchcond/conds/toDatetime")[0];
            String toDatetime = toDatetimeNode.Attributes["value"].Value;

            XmlNode rechargeSRCNode = xmlDoc.SelectNodes("/root/srchcond/conds/rechargeSRC")[0];
            String rechargeSRC = rechargeSRCNode.Attributes["value"].Value;

            XmlNode statusNode = xmlDoc.SelectNodes("/root/srchcond/conds/status")[0];
            String status = statusNode.Attributes["value"].Value;

            XmlNode needInvoiceNode = xmlDoc.SelectNodes("/root/srchcond/conds/needInvoice")[0];
            String needInvoice = needInvoiceNode.Attributes["value"].Value;


            XmlNode dataNode = xmlDoc.SelectNodes("/root/srchcond/sortFields")[0];
            XmlNodeList dataNodeList = xmlDoc.SelectNodes("/root/srchcond/sortFields/field");
            String value = "";
            foreach (XmlNode node in dataNodeList)
            {
                value = node.Attributes["value"].Value;
            }


        }
        
        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        public static String EnvelopSql(String OriginalSql, int from, int to)
        {
            String select_fields = GetValue(OriginalSql, "select", "from");  // 获得select 和from之间的字段
            String orderby_fields =OriginalSql.Substring(OriginalSql.IndexOf("order by")+"order by ".Length); //获得order by 后面的字段

            String rownum_field = " Row_Number() over  (Order by " + orderby_fields + " ) as RowId";
            //在 OriginalSql Select ...后面插入rownum_field 
            Regex r = r = new Regex(select_fields);
            if (r.IsMatch(OriginalSql))
            {
                OriginalSql = r.Replace(OriginalSql, rownum_field + "," + select_fields);
            }
            r = new Regex("order by " + orderby_fields);
            if (r.IsMatch(OriginalSql))
            {
                OriginalSql = r.Replace(OriginalSql, "");
            }
            String outter_sql = "select *  from (" + OriginalSql + ")  U  where 1=1 and U.RowId between " + from + " and " + to;
            return outter_sql;
        }

        static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min)) + min);
        }



        public static AuthenUamReturn AnalysisUamAuthenReturnXML(String UamAuthenReturnXMLText)
        {
            AuthenUamReturn uamReturn = new AuthenUamReturn();
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.InnerXml = UamAuthenReturnXMLText;
            XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    XmlNodeList nodelist = element.ChildNodes;
                    foreach (XmlElement childnodelist in nodelist)
                    {

                        switch (childnodelist.Name.ToLower())
                        {
                            case "actioncode":
                                uamReturn.TcpCont.ActionCode = childnodelist.InnerText;
                                break;
                            case "transactionid":
                                uamReturn.TcpCont.TransactionID = childnodelist.InnerText;
                                break;
                            case "rsptime":
                                uamReturn.TcpCont.RspTime = childnodelist.InnerText;
                                break;
                            case "response":
                                //得到该节点的子节点

                                XmlNodeList nodelistResponse = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistResponseEle in nodelistResponse)
                                {
                                    switch (nodelistResponseEle.Name.ToLower())
                                    {
                                        case "rsptype":
                                            uamReturn.TcpCont.Response.RspType = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspcode":
                                            uamReturn.TcpCont.Response.RspCode = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspdesc":
                                            uamReturn.TcpCont.Response.RspDesc = nodelistResponseEle.InnerText;
                                            break;
                                    }
                                }
                                break;
                        }

                    }
                }


                if (element.Name.ToLower() == "svccont")
                {
                    XmlNodeList nodelist = element.ChildNodes;
                    foreach (XmlElement childnodelist in nodelist)
                    {
                        if (childnodelist.Name.Equals("AuthenticationQryResp"))
                        {
                            XmlNodeList nodelistAuthenticationQryResp = childnodelist.ChildNodes;
                            foreach (XmlElement nodelistAuthenticationQryRespEle in nodelistAuthenticationQryResp)
                            {

                                switch (nodelistAuthenticationQryRespEle.Name.ToLower())
                                {
                                    case "rsptype":
                                        uamReturn.SvcCont.AuthenticationQryResp.RspType = nodelistAuthenticationQryRespEle.InnerText;
                                        break;
                                    case "rspcode":
                                        uamReturn.SvcCont.AuthenticationQryResp.RspCode = nodelistAuthenticationQryRespEle.InnerText;
                                        break;
                                    case "rspdesc":
                                        uamReturn.SvcCont.AuthenticationQryResp.RspDesc = nodelistAuthenticationQryRespEle.InnerText;
                                        break;
                                    case "authenticationkey":
                                        uamReturn.SvcCont.AuthenticationQryResp.AuthenticationKey = nodelistAuthenticationQryRespEle.InnerText;
                                        break;

                                }

                            }


                        }
                    }


                }

            }
            return uamReturn;
        }

        //https://webpaywg.bestpay.com.cn/payWeb.do
        //this._ORDERSEQ = orderSeq;
        //this._ORDERREQTRANSEQ = transactionID;
        //this._ORDERDATE = reqTime.ToString("yyyyMMddHHmmss");
        //this._ORDERAMOUNT = balance.ToString();
        //this._PRODUCTAMOUNT = balance.ToString();

        ////MAC签名
        //String mac = String.Format("MERCHANTID={0}&ORDERSEQ={1}&ORDERDATE={2}&ORDERAMOUNT={3}", this.MERCHANTID, this._ORDERSEQ, this._ORDERDATE, this._ORDERAMOUNT);
        //mac = BesttoneAccountHelper.MACSign(mac);

        //this._MAC = mac;
        static void wangyin_koukuan()
        {
            StringBuilder strLog = new StringBuilder();

            String _ORDERAMOUNT = "50000";
            String _PRODUCTAMOUNT = "50000";
            String MERCHANTID = "3100888914";
            String _ORDERREQTRANSEQ = BesttoneAccountHelper.CreateTransactionID();
            String _ORDERSEQ = BesttoneAccountHelper.CreateOrderSeq();
            DateTime _ORDERDATE = DateTime.Now;

            //3DES卡加密
            //String cardInfo = String.Format("cardNo={0}&password={1}", cardNo, cardPwd);
            //strLog.AppendFormat("CardNo:{0},", cardInfo);

            //cardInfo = BesttoneAccountHelper.DESEncrypt(cardInfo, "64B800B582F376AA0000000000000000");
            //strLog.AppendFormat("CardNoEncrypt:{0},", cardInfo);

            //MD5签名
            String mac = String.Format("MERCHANTID={0}&ORDERSEQ={1}&ORDERDATE={2}&ORDERAMOUNT={3}", MERCHANTID, _ORDERSEQ, _ORDERDATE.ToString("yyyyMMddHHmmss"), _ORDERAMOUNT);
            mac = BesttoneAccountHelper.MACSign(mac);
            //mac = BitConverter.ToString(MD5Encrypt(mac)).Replace("-", "");
            strLog.AppendFormat("MacEncrypt:{0}\r\n", mac);

            //<input type="hidden" name="MERCHANTID" value="<%=MERCHANTID %>" /> 
            //<input type="hidden" name="SUBMERCHANTID" value="<%=SUBMERCHANTID %>" /> 
            //<input type="hidden" name="ORDERSEQ" value="<%=ORDERSEQ %>"/>
            //<input type="hidden" name="ORDERREQTRANSEQ" value="<%=ORDERREQTRANSEQ %>"/> 
            //<input type="hidden" name="ORDERDATE" value="<%=ORDERDATE %>"/>
            //<input type="hidden" name="ORDERAMOUNT" value="<%=ORDERAMOUNT %>"/> 
            //<input type="hidden" name="PRODUCTAMOUNT" value="<%=PRODUCTAMOUNT %>" /> 
            //<input type="hidden" name="ATTACHAMOUNT" value="<%=ATTACHAMOUNT %>"/> 
            //<input type="hidden" name="CURTYPE" value="RMB" /> 
            //<input type="hidden" name="ENCODETYPE" value="<%=ENCODETYPE %>"/>
            //<input type="hidden" name="MERCHANTURL" value="<%=MERCHANTURL %>" /> 
            //<input type="hidden" name="BACKMERCHANTURL" value="<%=BACKMERCHANTURL %>" /> 
            //<input type="hidden" name="ATTACH" value="无" /> 
            //<input type="hidden" name="BUSICODE" value="0001" /> 
            //<input type="hidden" name="TMNUM" value="无" /> 
            //<input type="hidden" name="CUSTOMERID" value="" /> 
            //<input type="hidden" name="PRODUCTID" value="" /> 
            //<input type="hidden" name="PRODUCTDESC" value="" />
            //<input type="hidden" name="MAC" value="<%=MAC %>"/>


            NameValueCollection collection = new NameValueCollection();
            collection.Add("MERCHANTID", MERCHANTID);
            collection.Add("SUBMERCHANTID", "");
            collection.Add("ORDERSEQ", _ORDERSEQ);
            collection.Add("ORDERREQTRANSEQ", _ORDERREQTRANSEQ);
            collection.Add("ORDERDATE", _ORDERDATE.ToString("yyyyMMddHHmmss"));
            collection.Add("ORDERAMOUNT", _ORDERAMOUNT);
            collection.Add("PRODUCTAMOUNT", _PRODUCTAMOUNT);
            collection.Add("ATTACHAMOUNT", "0");
            collection.Add("CURTYPE", "RMB");

            collection.Add("ENCODETYPE", "1");
            collection.Add("MERCHANTURL", "http://customer.besttone.com.cn/UserPortal/UserAccount/BankRechargeSuccess.aspx");
            collection.Add("BACKMERCHANTURL", "http://customer.besttone.com.cn/UserPortal/UserAccount//BankRechargeBack.aspx");
            collection.Add("ATTACH", "无");
            collection.Add("BUSICODE", "0001");
            collection.Add("TMNUM", "无");
            collection.Add("CUSTOMERID", "");
            collection.Add("PRODUCTID", "");
            collection.Add("PRODUCTDESC", "");
            collection.Add("MAC", mac);

            WebClient client = new WebClient();
            byte[] info = client.UploadValues("https://webpaywg.bestpay.com.cn/payWeb.do", collection);
            String responseXml = System.Text.Encoding.UTF8.GetString(info);

            strLog.AppendFormat("[返回报文]:{0}\r\n", responseXml);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);



            String uptranSeq = xmlDoc.SelectNodes("/result/UPTRANSEQ")[0].InnerText;
            String tranDate = xmlDoc.SelectNodes("/result/TRANDATE")[0].InnerText;
            String responseCode = xmlDoc.SelectNodes("/result/RETURNCODE")[0].InnerText;
            String responseMsg = xmlDoc.SelectNodes("/result/RETNINFO")[0].InnerText;
            //String balance = xmlDoc.SelectNodes("/result/BALANCE")[0].InnerText;
            //String rechargeAmount = xmlDoc.SelectNodes("/result/RECHARGEAMOUNT")[0].InnerText;
            String sign = xmlDoc.SelectNodes("/result/SIGN")[0].InnerText;

            String out_UptranSeq = uptranSeq;
        
        }

        static void card_koukuan()
        {
            StringBuilder strLog = new StringBuilder();
            String cardNo = "8888000000847490";
            String cardPwd = "643099";
            String cardType = "2";
            String tranAmount = "50000";
            String transactionID = BesttoneAccountHelper.CreateTransactionID();
            String orderSeq = BesttoneAccountHelper.CreateOrderSeq();
            DateTime reqTime = DateTime.Now;

            //3DES卡加密
            String cardInfo = String.Format("cardNo={0}&password={1}", cardNo, cardPwd);
            strLog.AppendFormat("CardNo:{0},", cardInfo);
            
            cardInfo = BesttoneAccountHelper.DESEncrypt(cardInfo, "64B800B582F376AA0000000000000000");
            strLog.AppendFormat("CardNoEncrypt:{0},", cardInfo);

            //MD5签名
            String mac = String.Format("COMMCODE={0}&COMMPWD={1}&ORDID={2}&ORDPAYID={3}&REQTIME={4}&TRANSAMT={5}&KEY={6}", "3100888915",
                "656135", orderSeq, transactionID, reqTime.ToString("yyyyMMddHHmmss"), tranAmount, "64B800B582F376AA");
            strLog.AppendFormat("Mac:{0},", mac);
            mac = BitConverter.ToString(MD5Encrypt(mac)).Replace("-", "");
            strLog.AppendFormat("MacEncrypt:{0}\r\n", mac);

            NameValueCollection collection = new NameValueCollection();
            collection.Add("COMMCODE", "3100888915");
            collection.Add("SUBCOMMCODE", "");
            collection.Add("COMMPWD", "656135");
            collection.Add("CARDTYPE", cardType);
            collection.Add("ORDID", orderSeq);
            collection.Add("ORDPAYID", transactionID);
            collection.Add("TRANSAMT", "50000");
            collection.Add("CARDINFOENC", cardInfo);
            collection.Add("REQTIME", reqTime.ToString("yyyyMMddHHmmss"));
            collection.Add("ORDERVALIDITYTIME", "20340513111500");
            collection.Add("PRODUCTDESC", "");
            collection.Add("ATTACH", "");
            collection.Add("CUSTOMERIP", "228.112.116.118");
            collection.Add("CUSTOMERACCOUNT", "tylzhuang@gmail.com");
            collection.Add("CUSTOMERTELE", "");
            collection.Add("MAC", mac);
            //https://webtest.bestpay.com.cn/mobileTerminal.do
            //https://webpaywg.bestpay.com.cn/mobileTerminal.do
            WebClient client = new WebClient();
            byte[] info = client.UploadValues("https://webtest.bestpay.com.cn/mobileTerminal.do", collection);
            String responseXml = System.Text.Encoding.UTF8.GetString(info);

            strLog.AppendFormat("[返回报文]:{0}\r\n", responseXml);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);

     

            String uptranSeq = xmlDoc.SelectNodes("/result/UPTRANSEQ")[0].InnerText;
            String tranDate = xmlDoc.SelectNodes("/result/TRANDATE")[0].InnerText;
            String responseCode = xmlDoc.SelectNodes("/result/RETURNCODE")[0].InnerText;
            String responseMsg = xmlDoc.SelectNodes("/result/RETNINFO")[0].InnerText;
            //String balance = xmlDoc.SelectNodes("/result/BALANCE")[0].InnerText;
            //String rechargeAmount = xmlDoc.SelectNodes("/result/RECHARGEAMOUNT")[0].InnerText;
            String sign = xmlDoc.SelectNodes("/result/SIGN")[0].InnerText;

            String out_UptranSeq = uptranSeq;

        
        }

        //public static Dictionary<String, String> splitParameters(string paraStr)
        //{

        //    Dictionary<String, String> parameters = new Dictionary<string, string>();
            
        //    if (!String.Empty.Equals(paraStr))
        //    {
        //        string[] array = paraStr.Trim().Split('&');  
          
        //        foreach (string temp in array)
        //        {
        //            if (!String.Empty.Equals(temp))
        //            {
        //                string ttemp = temp.Trim();
        //                int index = ttemp.IndexOf("=");  
        //                if (index > 0)
        //                {
        //                    String key = ttemp.Substring(0, index);  
        //                    String value = ttemp.Substring(index + 1);
        //                    if (String.Empty.Equals(key) || String.Empty.Equals(value)) continue;
        //                    parameters.Add(key.Trim(), value.Trim());
        //                }
        //            }
        //        }
        //    }

        //    return parameters;
        //}

        static void Main(string[] args)
        {

            //UDBMBOSS _UDBMBoss = new UDBMBOSS();
            //string unifyPlatform_appId = UDBConstDefinition.DefaultInstance.UnifyPlatformAppId; //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appId"];
            //string unifyPlatform_appSecretKey = UDBConstDefinition.DefaultInstance.UnifyPlatformAppSecret;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_appSecretKey"];

            //string p_version = UDBConstDefinition.DefaultInstance.UnifyPlatformVersion;  //System.Configuration.ConfigurationManager.AppSettings["unifyPlatform_version"];
            //string p_clientType = UDBConstDefinition.DefaultInstance.UnifyPlatformClientType;
            //int Result = _UDBMBoss.UnifyPlatformGetUserInfo(unifyPlatform_appId, unifyPlatform_appSecretKey, p_version, p_clientType, accessToken, "116.228.55.13", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0; QQBrowser/7.7.24445.400)", out accountInfo, out ErrMsg);




            //Dictionary<String, String> OrderByMap = new Dictionary<string, string>();
            //OrderByMap.Add("a.completetime", "1");
            //StringBuilder sql = new StringBuilder();
            //sql.Append("select a.*,b.realname from (");
            //sql.Append("select ro.paytransactionid,ro.orderseq,ro.completetime,ro.orderamount,ro.productamount,ro.attachamount,");
            //sql.Append("ro.custid,ro.targetaccount,ro.status,ro.uptranseq,");
            //sql.Append("ro.returncode,ro.returndesc,ro.needinvoice,card.cardno,ro.orderdesc,ro.rechargetype ");
            //sql.Append("from rechargeorder ro left join  cardrechargerecord card on ro.orderseq = card.orderseq ) a,custinfo b with(nolock)");
            //sql.Append("where a.custid = b.custid ");


            //#region 拼排序语句
            //StringBuilder orderby = new StringBuilder();
            //String t_orderby = "";
            //if (OrderByMap.Keys.Count > 0)
            //{
            //    orderby.Append(" order by ");
            //    foreach (KeyValuePair<String, String> om in OrderByMap)
            //    {
            //        String OrderName = om.Key;
            //        String OrderMethod = om.Value;
            //        orderby.Append(OrderName);
            //        if ("1".Equals(OrderMethod))
            //        {
            //            orderby.Append(" asc ");
            //        }
            //        else if ("2".Equals(OrderMethod))
            //        {
            //            orderby.Append(" desc ");
            //        }
            //        orderby.Append(",");
            //    }
            //}

            //if (!String.IsNullOrEmpty(orderby.ToString()))
            //{
            //    t_orderby = orderby.ToString().Substring(0, orderby.ToString().Length - 1);   //去掉最后一个逗号
            //}
            //sql.Append(t_orderby);
            //#endregion
            


            //Console.WriteLine(sql.ToString());
            //Console.WriteLine(sql.ToString());

            //String abc = EnvelopSql(sql.ToString(), 1, 10);
            //Console.WriteLine(abc);
            //Console.WriteLine(abc);    
            //Console.ReadLine();

            //Dictionary<String, String> OrderByMap = new Dictionary<String, String>();
            //OrderByMap.Add("completetime", "2");
            //OrderByMap.Add("orderseq","1");

            //StringBuilder orderby = new StringBuilder();
            //String t_orderby = "";
            //if (OrderByMap.Keys.Count > 0)
            //{
            //    orderby.Append(" order by ");
            //    foreach (KeyValuePair<String, String> om in OrderByMap)
            //    {
            //        String OrderName = om.Key;
            //        String OrderMethod = om.Value;
            //        orderby.Append(OrderName);
            //        if ("1".Equals(OrderMethod))
            //        {
            //            orderby.Append(" asc ");
            //        }
            //        else if ("2".Equals(OrderMethod))
            //        {
            //            orderby.Append(" desc ");
            //        }
            //        else
            //        {
            //            orderby.Append(" asc ");
            //        }
            //        orderby.Append(",");
            //    }
            //}

            //if (!String.IsNullOrEmpty(orderby.ToString()))
            //{
            //    t_orderby = orderby.ToString().Substring(0, orderby.ToString().Length - 1);   //去掉最后一个逗号
            //}
            //Console.WriteLine(t_orderby);
            //Console.ReadLine();

            //String abc = "asdfa desc ,dfadfadsf asc,";
            //Console.WriteLine(abc.Substring(0,abc.Length-1));
            //Console.ReadLine();

            //string abc = GetValue("select asdfas,esfd,wwer from asdfasdf where a=c  order by a,b,d desc","select "," ");

            //Regex r = new Regex(abc);
            //if (r.IsMatch("select asdfas,esfd,wwer from asdfasdf where a=c  order by a,b,d desc"))
            //{
            //    String www = r.Replace("select asdfas,esfd,wwer from asdfasdf where a=c  order by a,b,d desc", " qqqq," + abc);
            //    Console.WriteLine( www);//输出：
            //}


            //Console.WriteLine(abc);

            
            //select * from (select Row_Number() over     
            //(Order by targetaccount asc,orderdate desc) as RowId ,* from RechargeOrder ) U 
            //where U.RowId between 10 and 20


            //String sql = " select orderseq,orderdate,curtype,custid from RechargeOrder where 1=1   order by  targetaccount asc,orderdate desc";

            //select a.*,b.realname from (
            //select ro.paytransactionid,ro.orderseq,ro.completetime,ro.orderamount,ro.productamount,ro.attachamount,
            //ro.custid,ro.targetaccount,ro.status,ro.uptranseq,
            //ro.returncode,ro.returndesc,card.cardno,ro.orderdesc,ro.rechargetype 
            //from rechargeorder ro left join  cardrechargerecord card on ro.orderseq = card.orderseq ) a,custinfo b with(nolock)
            //where a.custid = b.custid

            //StringBuilder sql = new StringBuilder();
            //sql.Append("select a.*,b.realname from (");
            //sql.Append("select ro.paytransactionid,ro.orderseq,ro.completetime,ro.orderamount,ro.productamount,ro.attachamount,");
            //sql.Append("ro.custid,ro.targetaccount,ro.status,ro.uptranseq,");
            //sql.Append("ro.returncode,ro.returndesc,card.cardno,ro.orderdesc,ro.rechargetype ");
            //sql.Append("from rechargeorder ro left join  cardrechargerecord card on ro.orderseq = card.orderseq ) a,custinfo b with(nolock)");
            //sql.Append("where a.custid = b.custid  and convert(varchar(10),a.completetime,112)>='20130301' and convert(varchar(10),a.completetime,112)<='20130330'  order by completetime desc");
            //String mysql = EnvelopSql(sql.ToString(),10,20);
            //Console.WriteLine(mysql);
            //Console.ReadLine();
            //Console.ReadLine();
            //bool flag = IsDate("2008-02-12");
            //String mydate = Convert.ToDateTime("2008-02-12").ToString("yyyy-MM-dd");
            //Console.WriteLine(flag);
            //Console.WriteLine(mydate);
            //Console.ReadLine();
            //testKuohao();
            //testJiaquanzhenXml();
            //testXml();
            //String CardNo = "888800123456789";
            //String ErrMsg = "";
            //Int32 Result = VerifyCardNo(CardNo, out ErrMsg);
            //Console.WriteLine(Result);
            //Console.WriteLine(ErrMsg);
            //Console.Read();

            //testExpr();
            //bestpayencrypt();

            //jiemi();
            //StringBuilder Request = new StringBuilder();

            //Request.Append("<?xml version='1.0' encoding='UTF-8' ?><root><callinfo><version value='1.0'/><SPID value='35000012'/></callinfo><param><CUSTID value='1111'/><AuthenCode>验证码</AuthenCode><EC>HWuyYaZhYEkC0wbEzCxmJUQIiUTxG8KI+fEnPSXeXQw=</EC><HC>4mTlZyQvfF1x8hPPJXWbS59YhDmAbiwy</HC></param></root>");

            //String Result = OpenBesttoneAccountV2(Request.ToString());

            //HashStr();
            //destest();
            //testIp();
            //testTimer();
            //testLog4net();

           //35433334$7qNXA38cYVMO583mq94/PJS+YjiS0yL/xzy67SbvOywtFuDbiSXehzu9wk9q3WtpRrliOoHRncNT2dWTZhl9wNMglvt3cQMYsK06l3LabDucqp/ZzUgPzUA2lE1piVz43TG+SpyBkFZMlUcyk0Hhczk4m5hjJEwK7W++p4ADxxs=
           //string token = "35433334$7qNXA38cYVMO583mq94/PJS+YjiS0yL/xzy67SbvOywtFuDbiSXehzu9wk9q3WtpRrliOoHRncNT2dWTZhl9wNMglvt3cQMYsK06l3LabDucqp/ZzUgPzUA2lE1piVz43TG+SpyBkFZMlUcyk0Hhczk4m5hjJEwK7W++p4ADxxs=";
           //           token = "35433334$7qNXA38cYVMO583mq94/PGQ4DRcGp4iKtFekor56+z4f3Pi3UX2lTsjzQTeIbKWQ9/rC3Q7IBgtEQofiZNVJlT4kE07WFvGvG2zcQjVS+1GN42JTYz7YcumN6lYS7V154uIxEijWp/PHduAslp/3fZ/3/4l4vPdP";
           //           token = "35433334$7qNXA38cYVMO583mq94/PJS+YjiS0yL/xzy67SbvOywtFuDbiSXehzu9wk9q3WtpRrliOoHRncNT2dWTZhl9wNMglvt3cQMYsK06l3LabDucqp/ZzUgPzUA2lE1piVz43TG+SpyBkFZMlUcyk0Hhczk4m5hjJEwK7W++p4ADxxs=";
           //token = "35000050%24y4DZ3jV6dE7owxNyyDjuMfkxdSP2AYjMWXdqG%2FUAw7nzJFUgC4YmIlPY%2FNVHD%2FLBF9EORN6JyskP%0AWWQspUFX39cXlxbZHuC0DfzNu2qJa05dugr4Glsm2Enl5%2Fu5zXQ%2F";
           //token = "NHBuV0doeVByd09ZVU00RFJrU0RMV05aKzkycVpBcWxZTlJKNnlkb0NXb2FrR01sbkVhaHZZOU42cWJLVjFtR1QrZytNZGsrODQ1L243UkVMdjJpc05HSjE5Z0ttLzNrQVQyUkVTM3Y4MkJpdk5CS1JlbjVjRVl2SGNndmttMDIwQWZCK3d4M1ZmRWNyeTRLYVYycXpYdGlvTFFONWVoZDFxdDJRWXNoWThVPQ%3d%3d";
           //token = HttpUtility.UrlDecode(token);
            //string token = "35433334$oABGqTTAfStTYElygrq5EftR02OPa3yu4ytE+HJWQ0jpHPy/zTzQ9asxXg7lIKv+dqwLOuDpBfSlthQqlYtttKmYv3f91i4rEUsY1cOMwuRkKdX1AmLlwva91JEtqHj++/WWIwLyobQZ/1HRMXQ4l6PdPS3EiUItstiO7YHPidL6yd5jZYulA+rIt+DbPLCm5KemcCGBL6XftaWwy2VOEA==";

            //token = "35433333$fZdMYLIvw8JgKx9wxxh7xhwQct7ENP7l8RUUayXZuWEMvFgzat+LwV2Jth1Qg6Clj04rpXi8H56oMGvuHOrdqZGu/ZSZ000Be1jDaWhVNFD9luuLIFHjqbKVqKap7sSc5HsHsU1luU4oAj5fist+T2b5Xa96SftTakZuRLzpz0k=";
            //token = "35000019%24bYWd1Vh5KH%2BmMptzFxEyPPCdWX8J%2BpiFeLmlOWROg3nZ3bW7PsRApumdte36ce6x7YmNlSRNjxndnO5XAGdR7zP7Hb1IG5ZXUfeW510Bb2Ek2AeGmBfnXeJPDjUi6EBFPabvpx14DrY%3D";
            //token = "35000019%24bYWd1Vh5KH8cOeh903fE1ljaNnRxNC2al0zgqPAc4o4eGC6AEz7kLsrT2Ju7oN0COhy1s0b38J%2bBAMiq3FyVBg8w%2bkmqeyA6H8FqlyoiyPgrk1uWohO2Bq184Bre%2fxJ8q3j9tmaQ58RURaX1lzCvHWRWlppCbkc3r4ag99gXKRC5UhM8KAwiWh0Xq3zkHV8QPr0GaooS8K5otFCAmpO%2bvg%2bX3xUynYS%2bd6dJweKmKtgZ04OtFGiPrQ%3d%3d";
            //token = "35000019$t8C9ziz2utUR8sKEMqU3ahj+iwFXi5culEJS5t6vA7plo0sQF57o8t3Jc7p72tcvI3V26W8sATR+tC1xqMRcwq1wgG3B5ZyycVhKfZ/pHqw=";
         
            //token = "35433333%24t8C9ziz2utU1xf6psiFvm2iXk6Cd0Rmp6TjQeUMjoYQhrxODw8IGfSiRVo4HsJqAKou7qhBW3gUZ3Zz%2fIfiiAeYXIIkM7%2fmPKv5zyLrAzc0%3d";
            //token = "35000019$t8C9ziz2utUR8sKEMqU3ahj+iwFXi5culEJS5t6vA7plo0sQF57o8t3Jc7p72tcvI3V26W8sATR+tC1xqMRcwq1wgG3B5ZyycVhKfZ/pHqw=";
           // string s_token = HttpUtility.UrlDecode(token);

            //String token = HttpUtility.UrlDecode("35433333%24dqS%2BhL04fl53JX5nAN7zsMtH8iUrZAg6OAvGImW0XvlceX36EB%2Flki%2BTx6GQAbC%2F7fwXuoU4M68G%0ACOcdPBCsXRIvwwjzkK8f%2BvZXOuZU0mi9e9vUetVZJX%2BqHtMjHCF5TTMBYPJXCGI%2FTlffjANibtVz%0APV2hJq2OxKz4Sq%2FIPBo%2BNqMdmuyT4A%3D%3D");

            //String token = "CsTflCMXcbrDaG3n82ui6FNN1yPKxcZBQ+D20Mw65X+p0dJk3vDzItYCIbPADHbLWs5stu0VFZ5HpXA+sbOfWZC3Y0LBUflj";
            //String abc = CryptographyUtil.Decrypt(token, "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534");
            //Console.WriteLine(abc);
            //Console.Read();
            //token = HttpUtility.UrlDecode("35433334%248gjsdINysksb0fI2PKjjloOu2JtcJtncVgJ6TW4uU84S7M51v2H7%2B5F9X%2Ba%2Ff9L7PGnIJFIWMNLQ%0A0711IcR9RM5ozrYHc%2FBEW2BxxoG2P1n5PIVQb2LE83JLqrJznRTIdeDMiee55WKAugMAVWp%2FUbDs%0A8g9rTKyvP1227VoYcVEj16ujWayfudcEsYGfjlBZoK8NkGdyAMo0EE7hXYoO3g%3D%3D");
            //int ret = ParseSPTokenRequest(token);

            //String abc = "http://sso.118114.cn/SSO/loginBackV2.action?url=aHR0cDovL3d3dy4xMTgxMTQuY24v";

            //35100007$5k5XivYmx1ztD3Qy3IVnIrsSEPtEg0AyxHBf3K6crgAe3yKMpmNjK9QTXurwR73YFG/3MmluvtCtL9N3Qx9GcL3PnAQDzph9AOLGafEgzxH1S8Q2BfSywrZuB2t5BZgHS85aeq3IqdhcHfONAoi5bw==
            //35100007$5k5XivYmx1wbaACEOze7brk3WMgRpqOMMKvfsG+3+edO7uJfKgtBGfmKcSYZ8BIMdPxlasMGzlNpc21E0VkUSkYm5xre84nRXuEXKvMmsL+aN4Rkod9qOPpqkt/QTC3ts3huNSRCVZSg4+nVm0CQ+hiP1Dgxa99Y
            //35100007%245k5XivYmx1ztD3Qy3IVnIrsSEPtEg0AyxHBf3K6crgAe3yKMpmNjK9QTXurwR73YFG%2F3MmluvtCt%0AL9N3Qx9GcL3PnAQDzph9AOLGafEgzxH1S8Q2BfSywrZuB2t5BZgHS85aeq3IqdhcHfONAoi5bw%3D%3D
            //string SourceStr = "35100007$5k5XivYmx1wbaACEOze7brk3WMgRpqOMMKvfsG+3+edO7uJfKgtBGfmKcSYZ8BIMdPxlasMGzlNpc21E0VkUSkYm5xre84nRdeVFCHHg6blPDhW4Jrqdadbnf0zahDOIyT8w+HkRnhw2DWosWUAP3L6Zuql/NsKj";
            //string SPID = "";
            //string UAProvinceID = "";
            //string SourceType = "";
            //string ReturnURL = "";
            //string ErrMsg = "";
            //int ret = ParseLoginRequest(token, out SPID, out UAProvinceID, out SourceType, out  ReturnURL, out  ErrMsg);

            //Console.WriteLine(SPID);
            //Console.WriteLine(ReturnURL);
            //Console.ReadLine();            

            //isIDCard();
            //testYY();
            //ResetPayPassword();
            //CancelBesttoneAccount();
            //testXiaofei();
            //testYourDate();
            //testMyDate();
            //testDATE();
            //Test43(Test44());
            //Test43();
            //Test9();
            //Test11();
            //queryCustinfo();
            //Test13();
            //isDate();
            charge();
            //QueryBesttoneAccount();
            //jiami();
            //testSms();
            //queryCharge();   // 
            //doQueryAllTxn();    
            //queryXiaoFie2();   //当日纪录查询
            //queryXiaoFei();    //查历史纪录
            //queryBalance();
            //QueryAccountBalance();
            //QueryCardBalance();

            //des();
            //testdes();
            //getPassWord();
            //abcd();
            //modifyPassword();
            //queryCustinfo();
            //modifyPassword();
            //modifyPasswordV2();
            //testtest();
            //modifyPassword();
            //efg();
            //RegisterBesttoneAccount();
            //NotifyBesttoneAccountInfo();
            //GetDiskFree();
            //ase();
            //testSms();
            //testInt();
            //String OutPhone = "";
            //bool ret = PhoneNumValid("14718070342", out OutPhone);
            //Console.WriteLine(OutPhone);
            //Console.WriteLine(OutPhone);
            //Console.WriteLine(OutPhone);
            //AccountRecharge();

            //String tid =Convert.ToString( LongRandom(1000000000, 9999999999, new Random()));
            //Console.WriteLine(tid);
            //Console.ReadLine();

            //String rawurl = "http://116.228.55.13:8081/SSO/SelectAssertion1.aspx?UATicket=1234567890&returnURL=http://www.118114.cn";

            //String myurl = HttpUtility.UrlEncode(rawurl);

            //Console.WriteLine(myurl);

            //String QueryString = rawurl.Substring(rawurl.IndexOf("UATicket="));

            //String[] Parameters = QueryString.Split('&');

            //foreach(String p in Parameters)
            //{
            //    String[] pv = p.Split('=');
            //    Console.WriteLine(pv[0]+"="+pv[1]);
            //}

            //Console.ReadLine();

            //String abc = CryptographyUtil.Encrypt("8086");
            //String efg = CryptographyUtil.Decrypt("Zd5pRxkCaEo=");
            //Console.WriteLine(efg);
            //Console.Read();
            
            //CryptographyUtil.Decrypt();

            //Console.WriteLine(abc);
            //Console.ReadLine();

            //DateTime dt1 = DateTime.Now;

            //DateTime dt3 = dt1.AddMinutes(2);

            //DateTime dt2 = DateTime.Now;

            //String abc = DateDiff(dt3,dt2);

            //Console.WriteLine(abc);
            //Console.WriteLine(abc);



            //string  str1="12:12";  
            //string  str2="14:14";  
            //DateTime  dt1=Convert.ToDateTime(str1);  
            //DateTime  dt2=Convert.ToDateTime(str2);  
            //DateTime  dt3=DateTime.Now;  
            //if(DateTime.Compare(dt1,dt2)>0)//大于  
            //{  
            //    Console.WriteLine("str1>str2");
                
            //}  
            //else if(DateTime.Compare(dt1,dt2)<0)//小于  
            //{
            //    Console.WriteLine("str1<str2");  
            //}  
            //else if(DateTime.Compare(dt1,dt2)==0)//相等  
            //{
            //    Console.WriteLine("str1==str2");  
            //}
            //{1ab24c}
            //Console.Read();
            //Random random = new Random();
            
            //object[] arr = new object[10] { 1,2,3,4,5,'a','b','c','d','e'};
            //int i, j;
            //object temp;

            //for (int k = 0; k < 20; k++)
            //{
            //    for (i = 0; i < 10; i++)
            //    {
            //        int r = random.Next(1, 10);
            //        j = r % (10 - i) + i;
            //        temp = arr[i];
            //        arr[i] = arr[j];
            //        arr[j] = temp;
            //    }
            //    for (i = 0; i < 10; i++)
            //    {
            //        Console.Write(arr[i]);
            //    }
            //    Console.WriteLine();
            //}

            //Console.ReadLine();
            //WebConfigurationManager.ConnectionString["Movies"].ConnectionString; 
            //try
            //{

            //    for (int i = 0; i < 10; i++)
            //    {
            //        ShortMessage[] in0 = new ShortMessage[1];
            //        ShortMessage sms = new ShortMessage();
            //        in0[0] = sms;
            //        sms.srcPhoneNumber = "18918790556";
            //        sms.destPhoneNumber = "123";
            //        sms.linkid = "";
            //        sms.msgContent = "rz";
            //        IPushPortClientService push = new IPushPortClientService();
            //        DateTime start = DateTime.Now;

            //        push.notifyRecivedShortMessages(in0);

            //        DateTime end = DateTime.Now;

            //        System.TimeSpan delta = end.Subtract(start);
            //        Console.WriteLine(delta.Milliseconds);
            //    }
            //     Console.Read();
            //}
            //catch (Exception e)
            //{ 
                
            //}

            //String mon = string.Format("{0:MM}",DateTime.Now);
            //Console.Write(mon);
            //Console.Read();

            //String connstr = DBUtility.BestToneCenterConStr;
            //SqlConnection conn = new SqlConnection(connstr);
            //conn.Open();
            //String cmdText = "select mesage from SmsTemplate where id = 2";
            //SqlCommand cmd = new SqlCommand(cmdText,conn);
            //SqlDataReader reader = cmd.ExecuteReader();
            //String msg = String.Empty;
            //while (reader.Read())
            //{
            //    msg = (string)reader["mesage"];
            //}



            //object[] arr_obj = new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            

            //Random r = new Random();
            //String AuthenCode = "";
            //AuthenCode += r.Next(100000, 999999).ToString();
            //Console.Write(AuthenCode);
            //Console.Read();

            //String resultmsg = HttpMethods.HttpGet("http://116.228.55.13:8113/facadeHome.do?actions=facadeHome&method=sendCouponToShare&wt=json&from=ios&registerCustId=134664179");

            //Console.WriteLine(resultmsg);

            //Console.Read();


            //SqlConnection con = new SqlConnection("server=.;uid=sa;password=123456;database=CIP2;Max Pool Size=512;");
            //SqlCommand cmd = new SqlCommand("select realname from custinfo where custid='117663773'", con);
            //using (con)
            //{
            //    con.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        Console.WriteLine((string)reader["realname"]);
            //    }
                 
            //}

            //Console.Read();    

            //card_koukuan();
            //wangyin_koukuan();

            //String plaintext = "123456123456";
            //String key = "07bHrYkaFYBD4Bxrhdbm9KhnT5jmvFho";

            //String ddd = "B8C86637437597C310F0869E8F27CBCD9E4A482ABB1CB272DF590DFC49356F7F62601182D8F2A9067FA8B4E467E467ACFE712AC94E9B309755FF5CA05288C6FE74681D7CF618056ACEDBA2B75CDF35855DFBDEC54A761068D08BE3324EC231DD41F6E0DE8A7B6F00";

            //String encrypt = CryptographyUtil.XXTeaEncrypt(ddd, key);
            //string encrypt = XXTEA.XXteaDecrypt(ddd,key);

           //Dictionary<String, String> hhh =  splitParameters(encrypt);

            //Console.WriteLine(encrypt);


            //Console.WriteLine(encrypt);
            //Console.WriteLine(CryptographyUtil.XXTeaDecrypt(encrypt, key));

            //Console.WriteLine(UnifyPlatform.utils.Cryptography.HMACSHA1.sign("test685AFB90888D059D1BE1234874AA6004C2D61D8040181F948428839502C43DF6E2A508B0FE9851D81575744B4EBA421D0E5B42ABB29D8BE3D949C2B4723C0AD3A0AD665606BA9F9292AD9839E86EDF5AEBEA2121CB6C685C5B0ADB62", "cn21"));
            //Console.Read();
            //Console.ReadLine();
            //String dot = "."; 
            //String bar ="-";
            //String star = "*";
            //String line = "_";
            //String a = "!";
            //String b = "'";
            //String c = "(";
            //String d = ")";

            //String url = "http://www.kjt.com*/中文/./-/_/!/(/)";
            //String encodeUrl = HttpUtility.UrlEncode(url);
            //String decodeUrl = HttpUtility.UrlDecode(encodeUrl);
            
            //Console.WriteLine(encodeUrl);
            //Console.Read();
            //Dictionary<String, String> paras = splitParameters("appId=118114&returnURL=http://114yg.cn");
            //Console.WriteLine(paras);
        }
        private static Dictionary<String, String> splitParameters(string paraStr)
        {
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendFormat("splitParameters:{0}\r\n", paraStr);
            Dictionary<String, String> parameters = new Dictionary<string, string>();

            if (!String.Empty.Equals(paraStr))
            {
                string[] array = paraStr.Trim().Split('&');

                foreach (string temp in array)
                {
                    if (!String.Empty.Equals(temp))
                    {
                        string ttemp = temp.Trim();
                        int index = ttemp.IndexOf("=");
                        if (index > 0)
                        {
                            String key = ttemp.Substring(0, index);
                            String value = ttemp.Substring(index + 1);
                            if (String.Empty.Equals(key) || String.Empty.Equals(value)) continue;
                            if (!parameters.ContainsKey(key))
                            {
                                parameters.Add(key.Trim(), value.Trim());
                            }

                        }
                    }
                }
            }

            return parameters;
        }

        class Book
        {
            public string Publisher;
            public string Title;
            public int Year;

            public Book(string title, string publisher, int year)
            {
                Title = title;
                Publisher = publisher;
                Year = year;
            }
        }

        

        public static void GenShareCode()
        {
            String connstr = DBUtility.BestToneCenterConStr;
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            String cmdText = "select mesage from SmsTemplate where id = 2";
            object[] arr_obj = new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            object[] arr_temp = arr_obj;
            StringBuilder sharecode = new StringBuilder();

            for (int x = 0; x < 2000000; x++)
            {
                sharecode.Length = 0;
                shuffle_object(arr_obj);
                arr_temp = arr_obj;
                shuffle_object(arr_temp);
                arr_obj = arr_temp;
                shuffle_object(arr_obj);
                //if (count_num(arr_obj) != 3)
                //{
                //    continue;
                //}
                for (int w = 0; w < 5; w++)
                {
                    sharecode.Append(arr_obj[w]);
                    //Console.Write(arr_obj[w]);
                }
                cmdText = "insert into sharecode(sharecode,flag) values ('" + sharecode.ToString() + "','0')";
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.ExecuteNonQuery();
                //Console.WriteLine();
            }
            conn.Close();
            Console.ReadLine();
        }


        public static int count_num(object[] arr_obj)
        {
            int c = 0;
            for (int f = 0; f < 6; f++)
            {
                int i;
                bool b = int.TryParse(arr_obj[f].ToString(), out i);
                //是数字则返回true;
                if (b)
                {
                    c++;
                }
            }

            return c;
        }

        public static void shuffle_object(object[] arr_obj)
        {
            Random random = new Random();
            int j;
            object temp;
            for (int i = 0; i < 10; i++)
            {
                int r = random.Next(1, 10);
                j = r % (10 - i) + i;
                temp = arr_obj[i];
                arr_obj[i] = arr_obj[j];
                arr_obj[j] = temp;
            }

            
        }

        public static void shuffle_int(int[] arr_int)
        {
            Random random = new Random();
            int j, temp;
            for (int i = 0; i < 10; i++)
            {
                int r = random.Next(1, 10);
                j = r % (10 - i) + i;
                temp = arr_int[i];
                arr_int[i] = arr_int[j];
                arr_int[j] = temp;
            }
        }

        public static void shuffle_chr(char[] arr_chr)
        {
            Random random = new Random();
            int j;
            char temp;
            for (int i = 0; i < 26; i++)
            {
                int r = random.Next(1, 26);
                j = r % (26 - i) + i;
                temp = arr_chr[i];
                arr_chr[i] = arr_chr[j];
                arr_chr[j] = temp;
            }
        }


        public void jifenshangchengcrmbaowenjixi()
        {

            //StringBuilder sb = new StringBuilder();
            //sb.Append("<ContractRoot>");
            //sb.Append("<TcpCont>");        
            //sb.Append("<TransactionID>1000000020201309041451187503</TransactionID>");        
            //sb.Append("<ActionCode>1</ActionCode>");         
            //sb.Append("<RspTime>20130904145119</RspTime>");         
            //sb.Append("<Response>");          
            //sb.Append("<RspType>0</RspType>");          
            //sb.Append("<RspCode>0000</RspCode>");        
            //sb.Append("<RspDesc>认证成功</RspDesc>");        
            //sb.Append("</Response>");         
            //sb.Append("</TcpCont>");        
            //sb.Append("<SvcCont>");         
            //sb.Append("<AuthenticationQryResp>");         
            //sb.Append("<RspType>0</RspType>");         
            //sb.Append("<RspCode>0000</RspCode>");           
            //sb.Append("<RspDesc>认证成功</RspDesc>");         
            //sb.Append("<AuthenticationKey>b3e3f3558acf7b43c0b838ed1e0e7cae</AuthenticationKey>");         
            //sb.Append("</AuthenticationQryResp>");         
            //sb.Append("</SvcCont>");
            //sb.Append("</ContractRoot>");

            //AuthenUamReturn tr = AnalysisUamAuthenReturnXML(sb.ToString());
            //sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            //sb.Append("<ContractRoot>");
            //sb.Append("<TcpCont>");
            //sb.Append("<ActionCode>1</ActionCode>");
            //sb.Append("<TransactionID>1000000020201309060919245107</TransactionID>");
            //sb.Append("<RspTime>20130906091925</RspTime>");
            //sb.Append("<Response>");
            //sb.Append("<RspType>0</RspType>");
            //sb.Append("<RspCode>0000</RspCode>");
            //sb.Append("<RspDesc>成功</RspDesc>");
            //sb.Append("</Response>");
            //sb.Append("</TcpCont>");
            //sb.Append("<SvcCont>");
            //sb.Append("<QryInfoRsp>");
            //sb.Append(" <InfoTypeID>31</InfoTypeID>");
            //sb.Append(" <InfoCont>");
            //sb.Append("<CustInfo>");
            //sb.Append(" <BelongInfo>");
            //sb.Append("  <ProvinceCode>609001</ProvinceCode>");
            //sb.Append(" <ProvinceName>北京</ProvinceName>");
            //sb.Append(" <CityCode>010</CityCode>");
            //sb.Append(" <CityName>北京市</CityName>");
            //sb.Append(" </BelongInfo>");
            //sb.Append(" <PartyCodeInfo>");
            //sb.Append("  <CodeType>15</CodeType>");
            //sb.Append("  <CodeValue>2010565347810000</CodeValue>");
            //sb.Append("  <CityCode>010</CityCode>");
            //sb.Append(" </PartyCodeInfo>");
            //sb.Append(" <IdentityInfo>");
            //sb.Append("  <IdentNum>130525199205222710</IdentNum>");
            //sb.Append("  <IdentType>1</IdentType>");
            //sb.Append("  </IdentityInfo>");
            //sb.Append("   <CustName>王中山</CustName>");
            //sb.Append("  <CustBrand/>");
            //sb.Append("  <CustGroup>12</CustGroup>");
            //sb.Append("  <CustServiceLevel>14</CustServiceLevel>");
            //sb.Append("  <CustAddress>北京市通州区葛布店北里19号楼111号</CustAddress>");
            //sb.Append("  </CustInfo>");
            //sb.Append(" <PointInfo>");
            //sb.Append("  <PointType>1</PointType>");
            //sb.Append("  <PointValueSum>5200</PointValueSum>");
            //sb.Append("  <PointValue>206</PointValue>");
            //sb.Append(" <PointTime>30000101</PointTime>");
            //sb.Append("  <PointValueEndOfYear>0</PointValueEndOfYear>");
            //sb.Append(" <PointItems>");
            //sb.Append("  <PointItemID>1</PointItemID>");
            //sb.Append("  <PointItemName>消费积分</PointItemName>");
            //sb.Append("  <PointItemValue>2603</PointItemValue>");
            //sb.Append(" <PointItemTime>30000101</PointItemTime>");
            //sb.Append("  </PointItems>");
            //sb.Append(" <PointItems>");
            //sb.Append("  <PointItemID>2</PointItemID>");
            //sb.Append("  <PointItemName>网龄积分</PointItemName>");
            //sb.Append(" <PointItemValue>200</PointItemValue>");
            //sb.Append(" <PointItemTime>30000101</PointItemTime>");
            //sb.Append(" </PointItems>");
            //sb.Append("  <PointItems>");
            //sb.Append("  <PointItemID>3</PointItemID>");
            //sb.Append("  <PointItemName> 奖励积分</PointItemName>");
            //sb.Append("  <PointItemValue>2603</PointItemValue>");
            //sb.Append("  <PointItemTime>30000101</PointItemTime>");
            //sb.Append("  </PointItems>");
            //sb.Append(" <PointItems>");
            //sb.Append("   <PointItemID>4</PointItemID>");
            //sb.Append("  <PointItemName>其它积分</PointItemName>");
            //sb.Append("  <PointItemValue>0</PointItemValue>");
            //sb.Append("  <PointItemTime>30000101</PointItemTime>");
            //sb.Append("  </PointItems>");
            //sb.Append("  </PointInfo>");
            //sb.Append("  </InfoCont>");
            //sb.Append("  </QryInfoRsp>");
            //sb.Append("  </SvcCont>");
            //sb.Append("  </ContractRoot>");

            //QryCustInfoV2Return qq = AnalysisQryCustInfoV2XML(sb.ToString());
            //string abc = GenerateOuterIDXmlV3("123", "123", "123", "123", "123", "123", "123", "123", qq);
            //Console.WriteLine(abc);
            //Console.ReadLine();
        }


        public void zhangzhipengjiami()
        {
            //Dictionary<String, String> dd = new Dictionary<string, string>();
            //dd.Add("appKey", "sh_car_service");
            //dd.Add("method", "car.searchCarMember");
            //dd.Add("v", "1.0");
            //dd.Add("messageFormat", "json");
            //dd.Add("phoneNumber", "1233333");

            //String lht = sign(dd, "abcdeabcdeabcdeabcdeabcde");

            //Console.WriteLine(lht);

            //List<string> list = new List<string>();
            //list = DecryptEmailURL("http://Customer.besttone.com.cn/UserPortal/authen.aspx?AuthenStr=bkF5a1FMTmFaOXZCUEk3ZXNuTXY3RUowcW5vOXRnQWUwc1N4SEtvVDlxelk0ZWQxUEtIT2ZGRU1xL216VGlTLzBJT0JsaGxXOXJ6VDZDM3ZheGxraXA4Qjh6VkVHWGVEb0p1RTlxckIwMStaWW51TDN0UUdyZz09");
        }


        public void zizhuce()
        {

            //StringBuilder requestXml = new StringBuilder();
            //String responseXml = String.Empty;

            #region 拼接请求xml字符串

            //String appKey = "ED150A183B8DE9A3E040A8C030B452AD";
            //String appSecret = "ED150A183B8EE9A3E040A8C030B452AD";
            //String apiName = "mobile";
            //String apiMethod = "getMobileSelfReg";

            //requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            //requestXml.Append("<reqXML version=\"1.0\">");
            //添加参数
            //requestXml.AppendFormat("<appKey>{0}</appKey>", appKey);
            //requestXml.AppendFormat("<apiName>{0}</apiName>", apiName);
            //requestXml.AppendFormat("<apiMethod>{0}</apiMethod>", apiMethod);
            //requestXml.AppendFormat("<timestamp>{0}</timestamp>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //requestXml.Append("<params>");
            //3100040168146224     460036171914508   698A019E0E878A29A2C8
            //requestXml.AppendFormat("<param  name=\"imsi\"  value='{0}' />", "460030167269401");           //手机IMSI号

            //requestXml.Append("</params>");
            //requestXml.Append("</reqXML>");


            #endregion
            //String serverAddress = "http://open.118114.cn/";


            //String url = serverAddress + "api?reqXml=" + HttpUtility.UrlEncode(requestXml.ToString(), Encoding.UTF8) + "&sign=" + getMD5Str(requestXml.ToString() + appSecret);

            //String ResponseXml = HttpClient.PostXmlData("http://open.118114.cn/", requestXml.ToString(), Encoding.UTF8, Encoding.UTF8);

            //String ResponseXml = HttpMethods.HttpGet(url);
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(ResponseXml);
            //XmlNode dataNode = xmlDoc.SelectNodes("/data")[0];
            //String mobile = String.Empty;
        }


        public void EmailCallBack()
        {
            List<string> list = new List<string>();
            String URL = "http://Customer.besttone.com.cn/UserPortal/authenV2.aspx?AuthenStr=NHBuV0doeVByd09ZVU00RFJrU0RMV05aKzkycVpBcWxZTlJKNnlkb0NXb2FrR01sbkVhaHZZOU42cWJLVjFtR1QrZytNZGsrODQ1L243UkVMdjJpc05HSjE5Z0ttLzNrQVQyUkVTM3Y4MkJpdk5CS1JlbjVjRVl2SGNndmttMDIwQWZCK3d4M1ZmRWNyeTRLYVYycXpYdGlvTFFONWVoZDFxdDJRWXNoWThVPQ%3d%3d";

            string[] arrTemp = URL.Split('=');
            URL = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(HttpUtility.UrlDecode(arrTemp[1]))));
            string[] arrParam = URL.Split('$');

            string key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";
            string Digest = CryptographyUtil.GenerateAuthenticator(arrParam[0] + "$" + arrParam[1] + "$" + arrParam[2] + "$" + arrParam[3] + "$" + arrParam[4], key);
            if (Digest.Equals(arrParam[5]))  //arrParam[5] 是 Digest
            {
                for (int i = 0; i < arrParam.Length - 1; i++)
                {
                    list.Add(arrParam[i]);
                }
            }
            else
            {
                list = null;
            }
        }


        /// <summary>
        /// 客户信息平台的接收邮箱认证解密地址
        /// 作者：周涛      时间：2009-9-09
        /// </summary>
        public static List<string> DecryptEmailURL(string URL)
        {
            List<string> list = new List<string>();
            try
            {
                string[] arrTemp = URL.Split('=');
                URL = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(HttpUtility.UrlDecode(arrTemp[1]))));
                string[] arrParam = URL.Split('$');
                SPInfoManager spInfo = new SPInfoManager();
                //Object SPData = spInfo.GetSPData(context, "SPData");
                //string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
                //string Digest = CryptographyUtil.GenerateAuthenticator(arrParam[0] + "$" + arrParam[1] + "$" + arrParam[2], key);
                //if (Digest.Equals(arrParam[3]))
                //{
                //    for (int i = 0; i < arrParam.Length - 1; i++)
                //    {
                //        list.Add(arrParam[i]);
                //    }
                //}
                //else
                //{
                //    list = null;
                //}
            }
            catch (System.Exception ex)
            {
                list = null;
            }

            return list;
        }

        public static String getMD5Str(String source)
        {
            byte[] abc = CryptographyUtil.MD5Encrypt(source);
            String ddd = byteToHexStr(abc);
            return ddd;
        }

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

        public static byte[] MD5Encrypt(String strSource)
        {
            byte[] input = null, output = null;
            input = Encoding.UTF8.GetBytes(strSource);
            output = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(input);

            return output;
        }


        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        //private static String getMD5Str(String source)
        //{
        //    //MessageDigest messageDigest = null;
        //    try
        //    {
        //        //messageDigest = MessageDigest.getInstance("MD5");
        //        //byte[] byteArray = messageDigest.digest(source.getBytes("UTF-8"));

        //        byte[] byteArray = CryptographyUtil.MD5Encrypt(source);
        //        StringBuilder md5StrBuff = new StringBuilder();
        //        for (int i = 0; i < byteArray.Length ; i++)
        //        {
        //            if (Integer.toHexString(0xFF & byteArray[i]).length() == 1)
        //            {
        //                md5StrBuff.append("0").append(Integer.toHexString(0xFF & byteArray[i]));
        //            }
        //            else
        //            {
        //                md5StrBuff.append(Integer.toHexString(0xFF & byteArray[i]));
        //            }
        //        }
        //        return md5StrBuff.toString();
        //    }
        //    catch (Exception e)
        //    {
        //        throw new RuntimeException("fail to get MD5 String", e);
        //    }
        //}

        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString() + "天"
                    + ts.Hours.ToString() + "小时"
                    + ts.Minutes.ToString() + "分钟"
                    + ts.Seconds.ToString() + "秒";
            }
            catch
            {

            }
            return dateDiff;
        }   





        static string GenerateOuterIDXmlV3(string OuterID, string ProvinceID, string LoginTicket, string CustAddress, string ResType, string RspCode, string RspDesc, string AuthenticationKey, QryCustInfoV2Return qryCustInfoReturn)
        {
            string Result = "";


            StringBuilder xmlDoc = new StringBuilder();
            try
            {

                XmlElement XmlRoot = null;
                XmlDocument XmlDom = new XmlDocument();
                Result = "<Root></Root>";
                XmlDom.LoadXml(Result);
                XmlRoot = XmlDom.DocumentElement;

                VNetXml.XMLSetChild(XmlDom, XmlRoot, "OuterID", OuterID);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "ProvinceID", ProvinceID);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "LoginTicket", LoginTicket);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "CustAddress", CustAddress);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "ResType", ResType);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "RspCode", RspCode);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "RspDesc", RspDesc);
                VNetXml.XMLSetChild(XmlDom, XmlRoot, "AuthenticationKey", AuthenticationKey);


                Result = @"<?xml version='1.0' encoding='gb2312' standalone='yes'?>" + XmlRoot.OuterXml;


                xmlDoc.Append("<ContractRoot>");
                xmlDoc.Append("<SvcCont>");
                if (qryCustInfoReturn != null)
                {
                    xmlDoc.Append("<QryInfoRsp>");
                    xmlDoc.AppendFormat("<InfoTypeID>{0}</InfoTypeID>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoTypeID);
                    xmlDoc.Append("<InfoCont>");
                    xmlDoc.Append("<CustInfo>");
                    xmlDoc.Append("<BelongInfo>");
                    xmlDoc.AppendFormat("<ProvinceCode>{0}</ProvinceCode>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceCode);
                    xmlDoc.AppendFormat("<ProvinceName>{0}</ProvinceName>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceName);
                    xmlDoc.AppendFormat("<CityCode>{0}</CityCode>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode);
                    xmlDoc.AppendFormat("<CityName>{0}</CityName>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityName);
                    xmlDoc.Append("</BelongInfo>");
                    xmlDoc.Append("<PartyCodeInfo>");
                    xmlDoc.AppendFormat("<CodeType>{0}</CodeType>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeType);
                    xmlDoc.AppendFormat("<CodeValue>{0}</CodeValue>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue);
                    xmlDoc.AppendFormat("<CityCode>{0}</CityCode>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CityCode);
                    xmlDoc.Append("</PartyCodeInfo>");
                    xmlDoc.Append("<IdentityInfo>");
                    xmlDoc.AppendFormat("<IdentNum>{0}</IdentNum>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum);
                    xmlDoc.AppendFormat("<IdentType>{0}</IdentType>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType);
                    xmlDoc.Append("</IdentityInfo>");
                    xmlDoc.AppendFormat("<CustName>{0}</CustName>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName);
                    xmlDoc.AppendFormat("<CustBrand>{0}</CustBrand>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustBrand);
                    xmlDoc.AppendFormat("<CustGroup>{0}</CustGroup>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup);
                    xmlDoc.AppendFormat("<CustServiceLevel>{0}</CustServiceLevel>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel);
                    xmlDoc.AppendFormat("<CustAddress>{0}</CustAddress>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress);
                    xmlDoc.Append("</CustInfo>");
                    //积分信息
                    xmlDoc.Append("<PointInfo>");
                    xmlDoc.AppendFormat("<PointType>{0}</PointType>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointType);
                    xmlDoc.AppendFormat("<PointValueSum>{0}</PointValueSum>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValueSum);
                    xmlDoc.AppendFormat("<PointValue>{0}</PointValue>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValue);
                    xmlDoc.AppendFormat("<PointTime>{0}</PointTime>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointTime);
                    xmlDoc.AppendFormat("<PointValueEndOfYear>{0}</PointValueEndOfYear>", qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValueEndOfYear);

                    foreach (QryCustInfoV2Return.SvcContResult.QryInfoRspResult.InfoContResult.PointInfoResult.PointItem item in qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointItems)
                    {
                        xmlDoc.Append("<PointItems>");
                        xmlDoc.AppendFormat("<PointItemID>{0}</PointItemID>", item.PointItemID);
                        xmlDoc.AppendFormat("<PointItemName>{0}</PointItemName>", item.PointItemName);
                        xmlDoc.AppendFormat("<PointItemValue>{0}</PointItemValue>", item.PointItemValue);
                        xmlDoc.AppendFormat("<PointItemTime>{0}</PointItemTime>", item.PointItemTime);
                        xmlDoc.Append("</PointItems>");
                    }
                    xmlDoc.Append("</PointInfo>");
                    xmlDoc.Append("</InfoCont>");
                    xmlDoc.Append("</QryInfoRsp>");
                }
                xmlDoc.Append("</SvcCont>");
                xmlDoc.Append("</ContractRoot>");
                Result = Result + xmlDoc.ToString();
            }
            catch (Exception e)
            {

            }
            finally
            {

            }


            return Result;
        }

        /// <summary>
        /// 解析向Crm查询客户信息时返回的XML
        /// </summary>
        /// <param name="QryUserStatusXMLText"></param>
        /// <returns></returns>
        static  QryCustInfoV2Return AnalysisQryCustInfoV2XML(string QryCustInfoXMLText)
        {
            QryCustInfoV2Return qryReturn = new QryCustInfoV2Return();
            System.Xml.XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.InnerXml = QryCustInfoXMLText;

            XmlNodeList nodeTemplet = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in nodeTemplet)
            {
                if (element.Name.ToLower() == "tcpcont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "actioncode":
                                qryReturn.TcpCont.ActionCode = childnodelist.InnerText;
                                break;
                            case "transactionid":
                                qryReturn.TcpCont.TransactionID = childnodelist.InnerText;
                                break;
                            case "rsptime":
                                qryReturn.TcpCont.RspTime = childnodelist.InnerText;
                                break;
                            case "response":
                                //得到该节点的子节点

                                XmlNodeList nodelistResponse = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistResponseEle in nodelistResponse)
                                {
                                    switch (nodelistResponseEle.Name.ToLower())
                                    {
                                        case "rsptype":
                                            qryReturn.TcpCont.Response.RspType = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspcode":
                                            qryReturn.TcpCont.Response.RspCode = nodelistResponseEle.InnerText;
                                            break;
                                        case "rspdesc":
                                            qryReturn.TcpCont.Response.RspDesc = nodelistResponseEle.InnerText;
                                            break;
                                    }
                                }
                                break;

                        }
                    }
                }
                if (element.Name.ToLower() == "svccont")
                {
                    //得到该节点的子节点

                    XmlNodeList nodelist = element.ChildNodes;

                    foreach (XmlElement childnodelist in nodelist)
                    {
                        switch (childnodelist.Name.ToLower())
                        {
                            case "qryinforsp":
                                //得到该节点的子节点

                                XmlNodeList nodelistQryInfoRsp = childnodelist.ChildNodes;

                                foreach (XmlElement nodelistQryInfoRspEle in nodelistQryInfoRsp)
                                {
                                    switch (nodelistQryInfoRspEle.Name.ToLower())
                                    {
                                        case "infotypeid":
                                            qryReturn.SvcCont.QryInfoRsp.InfoTypeID = nodelistQryInfoRspEle.InnerText;
                                            break;
                                        case "infocont":
                                            //得到该节点的子节点

                                            XmlNodeList nodelistInfoCont = nodelistQryInfoRspEle.ChildNodes;

                                            foreach (XmlElement nodelistInfoContEle in nodelistInfoCont)
                                            {
                                                switch (nodelistInfoContEle.Name.ToLower())
                                                {
                                                    case "custinfo":
                                                        //得到该节点的子节点

                                                        XmlNodeList nodelistCustInfo = nodelistInfoContEle.ChildNodes;
                                                        foreach (XmlElement nodelistCustInfoEle in nodelistCustInfo)
                                                        {
                                                            switch (nodelistCustInfoEle.Name.ToLower())
                                                            {

                                                                case "belonginfo":
                                                                    XmlNodeList nodelistBelongInfo = nodelistCustInfoEle.ChildNodes;
                                                                    foreach (XmlElement nodelistBelongInfoEle in nodelistBelongInfo)
                                                                    {
                                                                        switch (nodelistBelongInfoEle.Name.ToLower())
                                                                        {
                                                                            case "provincecode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceCode = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "provincename":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.ProvinceName = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "citycode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            case "cityname":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityName = nodelistBelongInfoEle.InnerText;
                                                                                break;
                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;

                                                                case "partycodeinfo":
                                                                    XmlNodeList nodelistPartyCodeInfo = nodelistCustInfoEle.ChildNodes;
                                                                    foreach (XmlElement nodelistPartyCodeInfoEle in nodelistPartyCodeInfo)
                                                                    {
                                                                        switch (nodelistPartyCodeInfoEle.Name.ToLower())
                                                                        {
                                                                            case "codetype":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeType = nodelistPartyCodeInfoEle.InnerText;
                                                                                break;
                                                                            case "codevalue":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue = nodelistPartyCodeInfoEle.InnerText;
                                                                                break;
                                                                            case "citycode":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CityCode = nodelistPartyCodeInfoEle.InnerText;
                                                                                break;
                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                                case "identityinfo":
                                                                    XmlNodeList IdentityInfoNodeList = nodelistCustInfoEle.ChildNodes;
                                                                    foreach (XmlElement IdentityInfoEle in IdentityInfoNodeList)
                                                                    {
                                                                        switch (IdentityInfoEle.Name.ToLower())
                                                                        {
                                                                            case "identtype":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType = IdentityInfoEle.InnerText;
                                                                                break;
                                                                            case "identnum":
                                                                                qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum = IdentityInfoEle.InnerText;
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                                case "custname":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName = nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custbrand":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustBrand = nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custgroup":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup = nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custservicelevel":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel = nodelistCustInfoEle.InnerText;
                                                                    break;
                                                                case "custaddress":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress = nodelistCustInfoEle.InnerText;
                                                                    break;

                                                            }
                                                        }
                                                        break;
                                                    case "pointinfo":
                                                        XmlNodeList nodelistpointInfo = nodelistInfoContEle.ChildNodes;
                                                        List<QryCustInfoV2Return.SvcContResult.QryInfoRspResult.InfoContResult.PointInfoResult.PointItem> pointItems = new List<QryCustInfoV2Return.SvcContResult.QryInfoRspResult.InfoContResult.PointInfoResult.PointItem>();
                                                        QryCustInfoV2Return.SvcContResult.QryInfoRspResult.InfoContResult.PointInfoResult.PointItem pointitem = null;
                                                        foreach (XmlElement nodelistpointInfoEle in nodelistpointInfo)
                                                        {
                                                            
                                                            switch (nodelistpointInfoEle.Name.ToLower())
                                                            {
                                                                case "pointtype":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointType = nodelistpointInfoEle.InnerText;
                                                                    break;
                                                                case "pointValue":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValue = nodelistpointInfoEle.InnerText;
                                                                    break;
                                                                case "pointvaluesum":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValueSum = nodelistpointInfoEle.InnerText;
                                                                    break;
                                                                case "pointtime":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointTime = nodelistpointInfoEle.InnerText;
                                                                    break;
                                                                case "pointvalueendofyear":
                                                                    qryReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointValueEndOfYear = nodelistpointInfoEle.InnerText;
                                                                    break;
                                                                case "pointitems":
                                                                    XmlNodeList nodelistpointitems = nodelistpointInfoEle.ChildNodes;
                                                                    pointitem = new QryCustInfoV2Return.SvcContResult.QryInfoRspResult.InfoContResult.PointInfoResult.PointItem();
                                                                    foreach (XmlElement nodelistpointitemsEle in nodelistpointitems)
                                                                    {
                                                                        
                                                                        if ("pointitemid".Equals(nodelistpointitemsEle.Name.ToLower()))
                                                                        {
                                                                            pointitem.PointItemID = nodelistpointitemsEle.InnerText;
                                                                        }
                                                                        else if ("pointitemname".Equals(nodelistpointitemsEle.Name.ToLower()))
                                                                        {
                                                                            pointitem.PointItemName = nodelistpointitemsEle.InnerText;
                                                                        }
                                                                        else if ("pointitemvalue".Equals(nodelistpointitemsEle.Name.ToLower()))
                                                                        {
                                                                            pointitem.PointItemValue = nodelistpointitemsEle.InnerText;
                                                                        }
                                                                        else if ("pointitemtime".Equals(nodelistpointitemsEle.Name.ToLower()))
                                                                        {
                                                                            pointitem.PointItemTime = nodelistpointitemsEle.InnerText;
                                                                        }
                                                                       
                                                                    }
                                                                    pointItems.Add(pointitem);
                                                                    break;
                                                            }
                                                           
                                                        }
                                                        qryReturn.SvcCont.QryInfoRsp.InfoCont.PointInfo.PointItems = pointItems;
                                                        break;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return qryReturn;
        }




        static int QueryBestPayAllTxn(string besttoneAccount, string txnType, string txnChannel, int maxReturnRecord, int startRecord, out IList<TxnItem> AllTxnList, out string ErrMsg)
        {

            int QueryBestPayAllTxnResult = 0;

            AllTxnList = new List<TxnItem>();

            //当日
            IList<TxnItem> CurrentDayTxnList = new List<TxnItem>();
            Int32 CurrentResult = BesttoneAccountHelper.QueryAllTypeTxn(besttoneAccount, txnType, txnChannel, out CurrentDayTxnList, out ErrMsg);


            //历史  3个月前
            IList<TxnItem> HistoryDayTxnList = new List<TxnItem>();
            DateTime today = DateTime.Now;
            DateTime lastday = today.AddDays(-90);

            DateTime startDate = Convert.ToDateTime(lastday.ToString("yyyy-MM-dd"));
            DateTime endDate = Convert.ToDateTime(today.ToString("yyyy-MM-dd"));

            Int32 HistoryResult = BesttoneAccountHelper.QueryHistoryTxn(startDate, endDate, besttoneAccount, txnType, txnChannel, maxReturnRecord, startRecord, out HistoryDayTxnList, out ErrMsg);

            if (CurrentResult == 0)
            {
                if (CurrentDayTxnList != null && CurrentDayTxnList.Count > 0)
                {
                    foreach (TxnItem ti in CurrentDayTxnList)
                    {
                        AllTxnList.Add(ti);
                    }
                }
            }
            else
            {
                QueryBestPayAllTxnResult = -1;
            }

            if (HistoryResult == 0)
            {
                if (HistoryDayTxnList != null && HistoryDayTxnList.Count > 0)
                {
                    foreach (TxnItem ti in HistoryDayTxnList)
                    {
                        AllTxnList.Add(ti);
                    }
                }


            }
            else
            {
                QueryBestPayAllTxnResult = -1;
            }

            return QueryBestPayAllTxnResult;
        }


        static void AccountRecharge()
        {
            String TransactionID = CreateTransactionID();
            String besttoneAccount = "18985582998";
            DateTime reqTime = new DateTime();
            long currentBalance = 0;
            String ErrMsg = "";

            int result = BesttoneAccountHelper.AccountRecharge(TransactionID, besttoneAccount, 20000, reqTime, out currentBalance, out ErrMsg);
        
        }

        static void testInt()
        {
            String ts = "00";
            int i = int.Parse(ts);
            Console.WriteLine(Convert.ToString(i));
            Console.WriteLine(Convert.ToString(i));
            Console.WriteLine(Convert.ToString(i));
        }

        static void GetDiskFree()
        {
            long a, b, c;
            GetDiskFreeSpaceEx("C:\\", out   a, out   b, out   c);
            Console.WriteLine(("C盘当前可用空间   " + long.Parse(a.ToString()) / 1024 / 1024/1024).ToString() + "   Gbyte");
            Console.WriteLine("======");
            Console.WriteLine("======");
            Console.WriteLine("======");
            Console.WriteLine("======");

        }

        static void ase()
        {
            String key_64 = CryptographyUtil.GetASEkey(64);
            String key_128 = CryptographyUtil.GetASEkey(128);

            Console.WriteLine(key_64);
            Console.WriteLine(key_128);
            Console.WriteLine("");

        }

        static void testSms()
        {

            //lihz  pighead  18901610666
            SmsClient sc = new SmsClient();
            //sc.sendSingleSms("02120906109", "18918790558", "笑一笑十年少，一声笑吓跑烦恼，二声笑忧愁绕道，三声笑快乐来扰，四声笑幸福报到");
            sc.sendSingleSms("35433333", "02120906109", "18918790558"
                , "客户信息平台:1、三台应用服务器 Windows 2003，2、三台数据库服务器 SQL Server（2台做集群，1台镜像），3、核心技术：.net、SQL Server、使用框架：MVC3 & EF4（ASP.NET MVC ApplicationUsing Entity Framework Code First）  接口规范：Web service/HTTP;4、注册认证：支持用户名、手机号、邮箱作为身份认证方式。实现号百及商旅平台之间的单点登录；支持189用户登录认证；支持电信客户与积分商城之间的单点登录（集团UDB规范）；正研究实现QQ、腾讯账号的认证机制。发信人-朱运坤");
        }

        static void querymy()
        {

           
        }

        
        public static int ParseLoginRequest(string SourceStr,  out string SPID, out string UAProvinceID,  out string SourceType, out string ReturnURL, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            SPID = "";
            UAProvinceID = "";
            SourceType = "";
            ReturnURL = "";
            string TimeStamp = "";
            string Digest = "";

            try
            {
                string[] alSourceStr = SourceStr.Split('$');
                SPID = alSourceStr[0].ToString();

                string ScoreSystemSecret = "161F869B808CA8DA62E02A4F91DCE95757A1DA297CAE388C";
                string EncryptSourceStr = alSourceStr[1].ToString();

                string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
                string[] alRequest = RequestStr.Split('$');
                //加密方式：Base64(Encrypt(UAProvinceID + “$” + SourceType+ “$”ReturnURL + “$”+ TimeStamp + “$”+ Digest))
                //Digest = Base64(Hash(UAProvinceID  + “$”+ SourceType + “$” + ReturnURL + “$”+ TimeStamp))
                UAProvinceID = alRequest[0].ToString();
                SourceType = alRequest[1].ToString();
                ReturnURL = alRequest[2].ToString();
                TimeStamp = alRequest[3].ToString();
                Digest = alRequest[4].ToString();
                //校验摘要 Digest 信息
                string NewDigest = UAProvinceID + "$" + SourceType + "$" + ReturnURL + "$" + TimeStamp;
                NewDigest = CryptographyUtil.GenerateAuthenticator(NewDigest, ScoreSystemSecret);
                if (Digest != NewDigest)
                {
                    Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                    ErrMsg = "无效的Digest";
                    return Result;
                }

                Result = 0;
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;
        }


        static string getYear(string date)
        {
            return date.Substring(0,4);
        }

        static string getMonth(string date)
        {
            return date.Substring(4, 2);
        }

        static string getDay(string date)
        {
            return date.Substring(6, 2);
        }


        static void testYourDate()
        {
            DateTime d1 = DateTime.Now;
            DateTime dBegin = d1.AddDays(-90);
            string begin_year = dBegin.Year.ToString();
            string begin_month = dBegin.Month.ToString();
            string begin_day = dBegin.Day.ToString();
            Console.WriteLine(Convert.ToDateTime(begin_year + "-" + begin_month + "-" + begin_day));
            Console.WriteLine(Convert.ToDateTime(begin_year + "-" + begin_month + "-" + begin_day));

            Console.WriteLine(Convert.ToDateTime(begin_year + "-" + begin_month + "-" + begin_day));
        }


        static void testMyDate()
        {
            Console.WriteLine(getYear("20080808"));
            Console.WriteLine(getMonth("20080808"));
            Console.WriteLine(getDay("20080808"));
            Console.WriteLine(getYear("20080808") +"-"+ getMonth("20080808") +"-"+ getDay("20080808"));
            Console.WriteLine(getYear("20080808") + "-" + getMonth("20080808") + "-" + getDay("20080808"));
            Console.WriteLine(getYear("20080808") + "-" + getMonth("20080808") + "-" + getDay("20080808"));
        }

        static void testDATE()
        {
            DateTime date = DateTime.Now;
            Console.WriteLine(date.Year);
            Console.WriteLine(date.Month);
            Console.WriteLine(date.Day);
            Console.WriteLine(date.ToString());
            Console.WriteLine(date.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine(date.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine(date.ToString());

            string date1 = "20080801";
            String date2 = String.Format("{0:d}", date1);
            Console.WriteLine(date2);
            Console.WriteLine(date2);
            Console.WriteLine(date2);

            DateTime dt = Convert.ToDateTime(date1);

            Console.WriteLine(dt.ToString("yyyy-MM-dd"));
            Console.WriteLine(dt.ToString("yyyy-MM-dd"));
            Console.WriteLine(dt.ToString("yyyy-MM-dd"));

            //Console.WriteLine(String.Format("{0yyyy-MM-dd","20121012}"));
            //Console.WriteLine(String.Format("{0yyyy-MM-dd", "20121012}"));
            //Console.WriteLine(String.Format("{0yyyy-MM-dd", "20121012}"));
        }


        static void testXiaofei()
        {
            String besttoneAccount = "18917921662";   //18930054353  18918790558
            String txnType = "121020";   // 121020充值   131010消费  131030退费
            //131010消费
            //131030退费

            String txnChannel = "02";

            IList<TxnItem> AllTxnList = new List<TxnItem>();
            String ErrMsg = "";
            //Int32 Result = BesttoneAccountHelper.QueryAllTypeTxn(besttoneAccount, txnType, txnChannel, out txnItemList, out ErrMsg);

            //DateTime startDate = Convert.ToDateTime("2012-01-01");
            //DateTime endDate = Convert.ToDateTime("2012-03-30");



            Int32 maxReturnRecord = 30;
            Int32 startRecord = 1;

            //Int32 Result = BesttoneAccountHelper.QueryHistoryTxn(startDate, endDate, besttoneAccount, txnType, txnChannel, maxReturnRecord, startRecord, out txnItemList, out ErrMsg);

            int ret_ret = QueryBestPayAllTxn(besttoneAccount, txnType, txnChannel, maxReturnRecord, startRecord,out AllTxnList,out ErrMsg);


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }



        static void doQueryAllTxn()
        {
            String besttoneAccount = "13316099926";
            String txnType = "";
            String txnChannel = "2";
            int maxReturnRecord = 100;
            int startRecord = 1;
            IList<TxnItem> AllTxnList = new List<TxnItem>();
            String ErrMsg = "";
            int result = QueryBestPayAllTxnV2(besttoneAccount, txnType, txnChannel, maxReturnRecord, startRecord, out AllTxnList, out ErrMsg);

           

        }

        static int QueryBestPayAllTxnV2(string besttoneAccount, string txnType, string txnChannel, int maxReturnRecord, int startRecord, out IList<TxnItem> AllTxnList, out string ErrMsg)
        {

            int QueryBestPayAllTxnResult = 0;

            AllTxnList = new List<TxnItem>();

            //当日
            IList<TxnItem> CurrentDayTxnList = new List<TxnItem>();
            Int32 CurrentResult = BesttoneAccountHelper.QueryAllTypeTxn(besttoneAccount, txnType, txnChannel, out CurrentDayTxnList, out ErrMsg);


            //历史  3个月前
            IList<TxnItem> HistoryDayTxnList = new List<TxnItem>();
            DateTime today = DateTime.Now;
            DateTime lastday = today.AddDays(-90);

            DateTime startDate = Convert.ToDateTime(lastday.ToString("yyyy-MM-dd"));
            DateTime endDate = Convert.ToDateTime(today.ToString("yyyy-MM-dd"));

            Int32 HistoryResult = BesttoneAccountHelper.QueryHistoryTxn(startDate, endDate, besttoneAccount, txnType, txnChannel, maxReturnRecord, startRecord, out HistoryDayTxnList, out ErrMsg);

            if (CurrentResult == 0)
            {
                if (CurrentDayTxnList != null && CurrentDayTxnList.Count > 0)
                {
                    foreach (TxnItem ti in CurrentDayTxnList)
                    {
                        AllTxnList.Add(ti);
                    }
                }
            }
            else
            {
                QueryBestPayAllTxnResult = -1;
            }

            if (HistoryResult == 0)
            {
                if (HistoryDayTxnList != null && HistoryDayTxnList.Count > 0)
                {
                    foreach (TxnItem ti in HistoryDayTxnList)
                    {
                        AllTxnList.Add(ti);
                    }
                }


            }
            else
            {
                QueryBestPayAllTxnResult = -1;
            }

            return QueryBestPayAllTxnResult;
        }

        static void CancelBesttoneAccount()
        {
            //need queryCustinfo
            String CustomerNo = "";
            //13817369457
            //18918790720
            //18930024055
            //18634850159
            //18930726862
            //13661832385
            String ProductNo = "18909720010";    //13701626560  18964041499    //13917014145
            String CustomerName = "lihongtu";
            String IDType = "X";
            String IDNo = "99999";
            String ErrMsg = "";
            //int ret0 = BesttoneAccountHelper.RegisterBesttoneAccount(phoneNumber, realName, contactTel, contactMail, sex, certtyp, certnum, TransactionID, out ErrMsg);
            CustInfo custinfo = new CustInfo();
            int ret0 = BesttoneAccountHelper.QueryCustInfo("18909720010", out custinfo, out ErrMsg);
            int ret1 = BesttoneAccountHelper.CancelBesttoneAccount(CustomerNo, ProductNo, custinfo.CustomerName, custinfo.IdType, custinfo.IdNo, out  ErrMsg);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }



        static void NotifyBesttoneAccountInfo()
        {
            string ProductNo = "18918790558";
            string newname = "笑巴郎";
            string newidtype = "1";
            string newidno = "310107197409092014";
            string newemail = "leohongtu@yahoo.com.cn";
 
            string ErrMsg = "";

            int ret = BesttoneAccountHelper.NotifyBesttoneAccountInfo(ProductNo, newname, newidtype, newidno, newemail, out ErrMsg);


        }

        static void ResetPayPassword()
        {
            String CustomerNo = "";
            String ProductNo = "18616313005";   //18964041499
            String CustomerName = "lihongtu";
            String IDType = "1";
            String IDNo = "310107197409092014";
            String ErrMsg = "";


       
            CustInfo custinfo = new CustInfo();
            int ret0 = BesttoneAccountHelper.QueryCustInfo("18616313005", out custinfo, out ErrMsg);
            if(ret0==0)
            {
                IDType = custinfo.IdType;
                IDNo = custinfo.IdNo;
                CustomerName = custinfo.CustomerName;
            }
            int ret1 = BesttoneAccountHelper.ResetBesttoneAccountPayPassword(ProductNo, IDType, IDNo, CustomerName, out ErrMsg);
        }

        


        static void efg()
        {
            String ErrMsg = "";
            AccountItem accountInfo = new AccountItem();
            int ret = BesttoneAccountHelper.QueryBesttoneAccount("15318790558", out accountInfo, out ErrMsg);


        }

        static void abcd()
        {
            string NewDigest = CryptographyUtil.GenerateAuthenticator("26257759" + "$" + "http://192.168.37.178:8080/userCenterAction.do?actions=redirectSuccess" + "$" + "yes" + "$" + "2012-09-25 19:32:30", "3A753D9FD1B2B2ADCCF4E27E372DD854BB260FB0BF2037CF");
            Console.WriteLine(NewDigest);
            Console.WriteLine(NewDigest);
            Console.WriteLine(NewDigest);
        
        }

        static void getPassWord()
        {
            String passWord = "";
            String Errmsg = "";
            Int32 ret = BesttoneAccountHelper.GetSymmetryPassWord(out passWord,out Errmsg);




        }


        static void isDate()
        {
            string birthday = "1980-13-34";
            if (Utils.IsDate(birthday))
            {
                Console.Write("yes");
            }
            else {
                Console.Write("no");
            }
        
        }

        static void isIDCard()
        {
            string idcard = "310000193709285826";   //   650104196901230031

            bool ret = CommonUtility.CheckIDCard(idcard);

            if (ret)
            {
                Console.WriteLine("yes");
            }
            else {
                Console.WriteLine("no");
            }
        }


        // 客户信息查询接口 lihongtu
        public static Int32 QueryCustInfo(String ProductNo, out CustInfo custinfo, out String ErrMsg)
        {

            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            String TransactionID = CreateTransactionID();
            StringBuilder requestXml = new StringBuilder();
            String responseXml = String.Empty;
            custinfo = new CustInfo();
            try
            {
                #region 拼接请求xml字符串
                //100101	客户查询
                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"客户查询\" WEBSVRCODE=\"100101\" APPFROM=\"100101|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //添加参数

                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + ProductNo);

                //


                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);  //002310000000000
                //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");  //002310000000000
                //001310000000000   
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion
                IDispatchControl serviceProxy = new IDispatchControl();
                //请求接口
                //log(String.Format("客户信息查询请求:{0}", requestXml));
                responseXml = serviceProxy.dispatchCommand("100101|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                //log(String.Format("客户信息查询返回:{0}", responseXml));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXml);

                String responseCode = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE").InnerText;
                ErrMsg = xmlDoc.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT").InnerText;

                if (responseCode == "000000")
                {
                    XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];
                    custinfo.CustomerNo = dataNode.Attributes["CUSTOMER_NO"].Value;
                    custinfo.ProductNo = dataNode.Attributes["PRODUCT_NO"].Value;
                    custinfo.CustomerName = dataNode.Attributes["CUSTOMER_NAME"].Value;
                    custinfo.IdType = dataNode.Attributes["ID_TYPE"].Value;
                    custinfo.IdNo = dataNode.Attributes["ID_NO"].Value;
                    Result = 0;
                    ErrMsg = String.Empty;
                }

            }
            catch (System.Exception ex)
            {
                //log("客户信息查询:" + ex.ToString());
            }
            return Result;
        }

        static void queryCustinfo()
        {
            //13808358066

            String Errmsg = "";
            CustInfo custinfo = new CustInfo();
            int ret = QueryCustInfo("13808358066", out custinfo, out Errmsg);
        }


        static void testtest()
        {
            String abc = "18918790558";

            String t = abc.PadLeft(16, '0');
            Console.WriteLine(abc.PadLeft(16, '0'));



        }


        static void modifyPasswordV2()
        {
            IDispatchControl serviceProxy = new IDispatchControl();
            StringBuilder requestXml = new StringBuilder(); ;

           
            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			requestXml.Append("	<PayPlatRequestParameter>");
			requestXml.Append("		<CTRL-INFO WEBSVRNAME=\"密码修改\" WEBSVRCODE=\"300030\" APPFROM=\"APP101\"");
			requestXml.Append("			KEEP=\"\" />");
			requestXml.Append("		<PARAMETERS>");
			requestXml.Append("			<PRODUCTNO>8631021000557154</PRODUCTNO>");
			requestXml.Append("			<ACCOUNTTYPE>1</ACCOUNTTYPE>");
			requestXml.Append("			<OLDPASSWD>337E5C247FB2F34A</OLDPASSWD>");
			requestXml.Append("			<NEWPASSWD>F2590806A2FE8311</NEWPASSWD>");
			requestXml.Append("			<ACCEPTORGCODE>001310000000000</ACCEPTORGCODE>");
			requestXml.Append("			<ACCEPTSEQNO>0001</ACCEPTSEQNO>	");
			requestXml.Append("			<ACCEPTAREACODE>440000</ACCEPTAREACODE>");
			requestXml.Append("			<ACCEPTCITYCODE>440100</ACCEPTCITYCODE>");
			requestXml.Append("			<ACCEPTCHANNEL>07</ACCEPTCHANNEL>");
			requestXml.Append("			<FEEFLAG>1</FEEFLAG>");
			requestXml.Append("			<FEEAMT>0</FEEAMT>");
			requestXml.Append("			<INPUTUID>20001</INPUTUID>");
			requestXml.Append("			<INPUTTIME>20110318111111</INPUTTIME>");
			requestXml.Append("			<CHECKUID>10001</CHECKUID>");
			requestXml.Append("			<CHECKTIME>20110318111111</CHECKTIME>");
			requestXml.Append("		</PARAMETERS>");
			requestXml.Append("	</PayPlatRequestParameter>");
            String responseXml = serviceProxy.dispatchCommand("300030|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());

            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void modifyPassword()
        {   //457715 
            BestPayEncryptService bpes = new BestPayEncryptService();
            String ErrMsg = "";
            String SymmetryPassWord = "3633353432353838";
            //int GetSymmetryPassWordResult = BesttoneAccountHelper.GetSymmetryPassWord(out SymmetryPassWord, out ErrMsg);


            AccountItem ai = new AccountItem();

            int QueryBesttoneAccountResult = BesttoneAccountHelper.QueryBesttoneAccount("18916765826", out ai, out ErrMsg);


          
            String e_oldPassWord = bpes.encryptNoKey("056169", ai.AccountNo);
            String e_newPassWord = bpes.encryptNoKey("123456", ai.AccountNo);
            String e_confirmPassWord = bpes.encryptNoKey("123456", ai.AccountNo);
            
            
            //log(String.Format("e_oldPassWord{0},e_newPassWord{1},e_confirmPassWord{2}", e_oldPassWord, e_newPassWord, e_confirmPassWord));
            int ModifyBestPayPasswordResult = BesttoneAccountHelper.ModifyBestPayPassword(ai.AccountNo, e_oldPassWord, e_newPassWord, e_confirmPassWord, out ErrMsg);
            Console.WriteLine(ErrMsg);
            Console.WriteLine(ErrMsg);
            Console.WriteLine(ErrMsg);
            Console.WriteLine(ErrMsg);
        }

 
        public static void des()
        {
            String pinkey = "627304";
            String cardOrAccountNo = "0000013385812347";
            String orderNo = "123456";
            PinkeyEncrypt bBEncrypt = new PinkeyEncrypt("3734363834373232");

            String ret = bBEncrypt.encrypt(pinkey, cardOrAccountNo);

            Console.WriteLine(ret);
            Console.Read();
        }
        
        static void Test()
        {
            String SPID = "35433333", ProvinceID="", SysID = "15999999", AuthenName = "13323719295", AuthenType = "2", Password = "280745", AreaID = "371", ExtendField = "";

            String ErrMsg, CustID, UserAccount, CustType, OutID, RealName, UserName, NickName;
            Int32 Result = BTForBusinessSystemInterfaceRules.UserAuthV2(SPID, AuthenName, AuthenType, Password,HttpContext.Current , ProvinceID, "", "",
            out  ErrMsg, out  CustID, out  UserAccount, out  CustType, out  OutID, out  ProvinceID, out  RealName, out  UserName, out  NickName);
        }

        static void Test2()
        {
            try
            {
                string url = "http://xxmc.myzone.cn/localservice/CRMForBTUCenterSoap?wsdl";

                //17999999
                String SysID = "15999999", AuthenName = "13323719295", AuthenType = "2", CustType = "30", Password = "280745", AreaID = "371", ExtendField = "";

                String ErrMsg = String.Empty;
                Int32 CrmResult;

                if (SysID == "15999999")
                {
                    BTUCenter.Proxy.JX.CRMUserAuthResult resultObj = new BTUCenter.Proxy.JX.CRMUserAuthResult();
                    BTUCenter.Proxy.JX.JXCRMForBTUCenter obj = new BTUCenter.Proxy.JX.JXCRMForBTUCenter();
                    obj.Url = url;

                    resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                    if (resultObj.ErrorDescription == null)
                        ErrMsg = "";
                    else
                        ErrMsg = "" + resultObj.ErrorDescription;
                    CrmResult = resultObj.Result;


                }
                else if (SysID == "13999999")
                {
                    BTUCenter.Proxy.AH.CRMUserAuthResult resultObj = new BTUCenter.Proxy.AH.CRMUserAuthResult();

                    BTUCenter.Proxy.AH.CRMForBTUCenter obj = new BTUCenter.Proxy.AH.CRMForBTUCenter();
                    obj.Url = url;

                    resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                    ErrMsg = resultObj.ErrorDescription;
                    CrmResult = resultObj.Result;

                }
                else if (SysID == "14999999")
                {
                    BTUCenter.Proxy.FJ.IntegralApplyWSFactory obj = new BTUCenter.Proxy.FJ.IntegralApplyWSFactory();
                    BTUCenter.Proxy.FJ.CRMUserAuthResponse resultObj = new BTUCenter.Proxy.FJ.CRMUserAuthResponse();

                    obj.Url = url;
                    resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, AreaID, Password, ExtendField);
                    ErrMsg = resultObj.errorDescription;
                    CrmResult = resultObj.result;
                }
                else if (SysID == "22999999")
                {
                    BTUCenter.Proxy.HN.EaiWebService obj = new BTUCenter.Proxy.HN.EaiWebService();
                    BTUCenter.Proxy.HN.CRMUserAuthResult resultObj = new BTUCenter.Proxy.HN.CRMUserAuthResult();
                    obj.Url = url;
                    resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);
                    // resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, AreaID, Password, ExtendField);
                    ErrMsg = resultObj.errorDescription;
                    CrmResult = resultObj.result;

                }
                else
                {
                    BTUCenter.Proxy.CRMUserAuthResult resultObj = new BTUCenter.Proxy.CRMUserAuthResult();

                    BTUCenter.Proxy.CRMForBTUCenter obj = new BTUCenter.Proxy.CRMForBTUCenter();

                    obj.Url = url;

                    resultObj = obj.CRMUserAuth("3501", AuthenName, AuthenType, CustType, Password, AreaID, ExtendField);

                    ErrMsg = resultObj.ErrorDescription;
                    CrmResult = resultObj.Result;
                }
            }
            catch (Exception ex)
            {

            }
        }

        static void Test3()
        {
            String SPID = "03000001", AuthenName = "lihongtu", Password = "189302";
            String Ticket = "", CustID = "", CustType = "", AuthenType = "", UserName = "", UserAccount = "", RealName = "", NickName = "", OuterID = "", SysID = "", AreaID = "", OutProvinceID = "";
            String ProvinceID = "", IsNeedLogin = "", IsQuery = "", PwdType = "";

            String ErrMsg = String.Empty;
            Int32 Result = BTForBusinessSystemInterfaceRules.UserAuthV3(SPID, AuthenName, Password, PwdType, out CustID, out CustType, out AuthenType,
                out UserName, out UserAccount, out RealName, out NickName, out SysID, out OuterID, out OutProvinceID, out AreaID, out ErrMsg);
            
        }

        static void Test4()
        {
            String spid = "35000000";
            String authencode = "403509";
            String CustID = "8600210100051000", Email = "wudan1020@yahoo.cn", returnUrl = "http://customer.besttone.com.cn/UserPortal/sso/login.aspx", timeTamp = "2012-05-29 13:37:33"; //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";
            String Digest = CryptographyUtil.GenerateAuthenticator(spid + "$" + CustID + "$" + Email + "$" + returnUrl + "$" + authencode + "$" + timeTamp, key);
            String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(spid + "$" + CustID + "$" + Email + "$" + returnUrl + "$" + authencode + "$" + timeTamp + "$" + Digest)));

            String url = "http://www.baidu.com?urlParam=" + HttpUtility.UrlEncode(AuthenStrValue);
        }

        static void Test41()
        {
            String str = "MkpYWEcrbUp6NUk4QXVWWENTbnhtek1BV0lsYWRtdjV4NkYrWHNyQjZHVTAvRCtlR2srUmhuQmh3SHZmYXNxbEJ6TkpaZVpaazJ1MnV1TGhnZFFYMFRkcG5SWlVTdkFXSU52SWtmVXJ3eTBtVTROTk9aNy9YL29YbkJuaUpLZ3dLWlJRT2NSa3B1N055YkZDUmFybnZyS3lZUldCYVNpc0JTNXNlYmlsbHZhWVM3YTRCRFFYOUFPZndBRHVoY3VDNUJjenRRVWt0OWNOdy9NWklrWWNLMGtiMlM4Sm1oK0E%3d";
            String urlParam = HttpUtility.UrlDecode(str);
            //解析并获取参数
            String DecryptParam = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(urlParam)));
            String[] paramArray = DecryptParam.Split('$');
            String spid = paramArray[0];
            String custid = paramArray[1];
            String email = paramArray[2];
            String returnUrl = String.IsNullOrEmpty(paramArray[3]) ? ConstHelper.DefaultInstance.BesttoneLoginPage : paramArray[3];
            String authenCode = paramArray[4];
            String timeTamp = paramArray[5];
            String digest = paramArray[6];

            //对参数进行验证
            //SPInfoManager spInfo = new SPInfoManager();
            //Object SPData = spInfo.GetSPData(this.Context, "SPData");
            //String key = spInfo.GetPropertyBySPID(spid, "SecretKey", SPData);
            String key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";
            String NewDigest = CryptographyUtil.GenerateAuthenticator(spid + "$" + custid + "$" + email + "$" + returnUrl + "$" + authenCode + "$" + timeTamp, key);
            //看是否过期
            DateTime sendMailTime = Convert.ToDateTime(timeTamp);
            Int32 expiredHour = ConstHelper.DefaultInstance.ResetPwdExpiredHour;

            String ErrMsg = String.Empty;
            Int32 result = SetMail.CheckEmaklSend(custid, email, authenCode, out ErrMsg);
        }

        static void Test42()
        {
            String str = "MkpYWEcrbUp6NUk4QXVWWENTbnhtek1BV0lsYWRtdjV4NkYrWHNyQjZHVTAvRCtlR2srUmhuQmh3SHZmYXNxbGd4Qm5semlPWFdaSTUxeHIwTVZVQm42UTJaOTY2RWNuN2hQcytsck4yVXpXMzQwQ0Z5eC9kWGtRMUEwQzFXTGhpMnExMExXUWVTbWtrVktLTldNTVoveXF4MFM0OWthblgyZ0hnbk9sQVFRQmtRcVdsSFRON1BETEI1UHN4czlrZVFhUVVpblN1SSt4N25FN1BWOU9pdz09";
            String urlParam = HttpUtility.UrlDecode(str);
            //解析并获取参数
            String DecryptParam = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(urlParam)));
            String[] paramArray = DecryptParam.Split('$');
            String spid = paramArray[0];
            String custid = paramArray[1];
            String email = paramArray[2];
            String returnUrl = String.IsNullOrEmpty(paramArray[3]) ? ConstHelper.DefaultInstance.BesttoneLoginPage : paramArray[3];
            String authenCode = paramArray[4];
            String timeTamp = paramArray[5];
            String digest = paramArray[6];

            String ErrMsg = "";
            SetMail.InsertEmailSendHistory2(custid, email, authenCode, out ErrMsg);
        }

        static void Test43()
        {
            //String key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";
            String key = "3A753D9FD1B2B2ADCCF4E27E372DD854BB260FB0BF2037CF";
            String str = "35433333$k88JssIAtjIQXjhoCqyvAPIPIJn1AuHuGYDnn3hgPIIVrF9TyHzUxqzLxgk4WKq9tzCkRSt9R8QoyPqXiEMYCPQOlqC3yfe2";
            String[] tempArray = str.Split('$');
            String temp = CryptographyUtil.Decrypt(tempArray[1], key);
        }

        static String Test44()
        {
            String key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";
            String TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String custID = "117663756", result = "0", errMsg = "";
            string Digest = CryptographyUtil.GenerateAuthenticator(TimeStamp + "$" + custID + "$" + result + "$" + errMsg, key);
            String encryptCustIDValue =CryptographyUtil.Encrypt(TimeStamp + "$" + custID + "$" + result + "$" + errMsg + "$" + Digest, key);
            return encryptCustIDValue;
        }

        static void Test5()
        {
            String ErrMsg = "";
            SetMail.InsertEmailByResetPwd("123", "2", "http://www.baidu.com", "123123", 1, "wudan1020@yahoo.cn", DateTime.Now, "描述", "中国电信号码百事通：找回密码", 1, 1, out ErrMsg);
        }

        static void Test7()
        {
            String str = "b2dCRkMvUit3amgxSFNFRFd3QmdGaG40dWdZRmlzKzJpTVdtS2ltQlhPYXM2WnFTenhUOUpzZHlMa1lRb2tYbUl2MHZxZXdNR2F4dndoaXE4eXJwdGdEQTR2akJieSs5QXpPcTU5b2RwdFY3YUdjVUNDWEpucGt4a1NyTGdlQVpOc2UwaHBPNlAwV3gxcXVpZTVnZ3d3PT0%3d";
            String urlParam = HttpUtility.UrlDecode(str);

        }

        static void Test8()
        {
            String spid = "35000022";
            String ip = "192.168.155.13";
            String ErrMsg = String.Empty;
            //CommonBizRules.CheckIPLimit(spid, ip, this, out ErrMsg);
            //long startNumber = CommonBizRules.GetIPAddressIPNumber("192.168.255.255");
            //long endNumber =CommonBizRules.GetIPAddressIPNumber("")
        }

        static void Test9()
        {
            String key = "3C67B5657DF383DFE5FDBC449FFC850B8EB79459AA369011";
            //String source = "kmqTK/VaNXGJRVwt377B8R2C2wzWTJX9l8B8zqsZ4WHlRAeyTCeY4SDB2KTG4FNwk4T/baQYgzVDXbsuf2OG3O75n9dKKz0LtiXV3ffdgg6dMqqBcvKHrKHBbzqOZZHccBropdg YK8Gd674RCUaWIyBDHzdcMh3";
            String source = "0$BJ35000000000000010000000000000078d5cfbb1bf54f878cb46a91d0e006bb$2012-07-03 10:51:57$s4Ng7QySY袔t苬qsjrvyfs1s=";
            String str = CryptographyUtil.Encrypt(source, key);
        }

        static void Test10()
        {
            String Authenticator = "Iwi5Pt3R/IjQBWzYesDcNplR/7FuG4Y+";
            String SrcSsDeviceNo = "3500000000408201";
            String AuthSsDeviceNo = "3500000000408201";
            String UDBTicket = "SH3500000000000001000000000000005a78733822fa4d3e9a0d9b50d2037658";
            String TimeStamp = "2012-07-04 10:45:07";

            Hashtable hs = new Hashtable();
            hs.Add("Authenticator", Authenticator);
            hs.Add("SrcSsDeviceNo", SrcSsDeviceNo);
            hs.Add("AuthSsDeviceNo", AuthSsDeviceNo);
            hs.Add("UDBTicket", UDBTicket);
            hs.Add("TimeStamp", TimeStamp);

            String returnXml = WebServiceCommon.QuerySoapWebService("http://zx.passport.189.cn/UDBAPPInterface/UDBAPPSYS/UDBAppSys.asmx", "AccountInfoCheck", hs).OuterXml;

            //ConsoleApp.cn._189.passport.zx.UDBAppSys service = new ConsoleApp.cn._189.passport.zx.UDBAppSys();
            //cn._189.passport.zx.AccountInfoCheckResult result = service.AccountInfoCheck(Authenticator, SrcSsDeviceNo, AuthSsDeviceNo, UDBTicket, TimeStamp);
        }

        static void Test11()
        {
            String UserID = "18918790558", PUserID = "02009571215", Alias = "lihongtu";
            String CustID, OuterID, Status, CustType, CustLevel, RealName, UserName, NickName, CertificateCode, CertificateType, Sex, Email, EnterpriseID, ProvinceID, AreaID, RegistrationSource;
            UDBAccountInfo accountInfo = new UDBAccountInfo();
            accountInfo.UserID = UserID;
            accountInfo.PUserID = PUserID;
            accountInfo.Alias = Alias;
            accountInfo.ProvinceID = "02";
            accountInfo.NumFlag = "2";
            accountInfo.SourceSPID = "35000011";

            String ErrMsg = String.Empty;
            //Int32 Result = UserRegistry.getUserRegistryUDB(accountInfo, out CustID, out ErrMsg); 
        }

        static void Test12()
        {
            String SrcSsDeviceNo = "3500000000408201";
            String UDBKey = "3C67B5657DF383DFE5FDBC449FFC850B8EB79459AA369011";

            String str1 = "3500000000408201%24A2ifLX%2fI714Cfc8Wun03uheoIChIJXHG5NXzVbnIFDDoasCtbufwDvpMkOoPpm6mMkh23MAPqbdC%0d%0akV5%2fY3rk79YFkQCUlTFMCmZQus7epRHMffvbZKgNK3Y%2f3RpMph9I4n3A%2bveV6MgPKCvcxXey4oBo%0d%0aZ2LIwBcFMZV3gF77vdf3y9hbh7qsbqimgjKmcODbDkVfoZV3thFtIMS6qKdi674c2kJpslGl";
            String str2 = "3500000000408201%24C4PVg6dxDzDfSsoJFFguZlhmT1Gsb02l1zYMyP7QBSYnayUjjT6Az9%2b%2bcgTcKqTUcoHVoBKFTfwx4FXkIEK7JpcCXoItGPHrI92Zabw2M4xHYx%2bp7Lgzbu1cJBbMu0GI";

            String str1UrlDecode = HttpUtility.UrlDecode(str1);
            String[] tempArray = str1UrlDecode.Split('$');
            String desDecode = CryptographyUtil.Decrypt(tempArray[1], UDBKey);

            String digest = CryptographyUtil.ToBase64String(CryptographyUtil.Hash(SrcSsDeviceNo + "2012-08-02 08:59:40" + "http://customer.besttone.com.cn/UserPortal/SSO/LoginUDB.aspx?SPID=35433333&ReturnUrl=http%3A%2F%2Fgo.118114.cn%2F"));
        }

        static void Test13()
        {
            //AccountItem ai = new AccountItem();
            String ErrMsg = "";
            //int Result = BesttoneAccountHelper.QueryBesttoneAccount("18918790558", out ai, out ErrMsg);
            //Convert.ToDateTime（2010-03-08 10:21:24.378）。
            DateTime startDate = Convert.ToDateTime("2012-01-01");
            DateTime endDate = Convert.ToDateTime("2012-09-21");
            String besttoneAccount = "18918790558";
            Int32 maxReturnRecord = 100;
           Int32 startRecord = 1;
            IList<TxnItem> txnItemList = new List<TxnItem>();
            //int Result = BesttoneAccountHelper.QueryBusinessTxnHistory(startDate, endDate, besttoneAccount, maxReturnRecord, startRecord, out  txnItemList, out ErrMsg);
            
            //DateTime currentDay, String besttoneAccount,out IList<TxnItem> txnItemList, out String ErrMsg
            //int result = BesttoneAccountHelper.QueryRechargeTxnCurrentDay(DateTime.Now, besttoneAccount, out txnItemList,out ErrMsg);
            
            //BesttoneAccountHelper.QueryAllTxnHistory()
            //DateTime startDate, DateTime endDate, String besttoneAccount, Int32 maxReturnRecord,Int32 startRecord, 
            //    out IList<TxnItem> txnItemList, out String ErrMsg

            //for (int i = 0; i < txnItemList.Count;i++ )
            //{
                //TxnItem ti = txnItemList.
            //}
            Console.WriteLine("");
            Console.WriteLine("");
            //foreach(TxnItem ti in txnItemList )
            //{
            //    Console.WriteLine(ti.AcceptDate + "|" + ti.AcceptSeqNO + "|" + ti.CancelFlag + "|" + ti.MerchantName + "|" + ti.TxnAmount + "|" + ti.TxnChannel + "|" + ti.TxnDscpt + "|" + ti.TxnType);

            
            
            //}
            //AccountItem accountInfo = new AccountItem();
            //double banlance = 0;
            //int Result = BesttoneAccountHelper.QueryBesttoneAccount(besttoneAccount, out accountInfo, out ErrMsg);
            //int Result = BesttoneAccountHelper.QueryAccountBalance(besttoneAccount, out banlance, out ErrMsg);
            //Int32 Result = BesttoneAccountHelper.QueryTxnHistory(startDate, endDate, besttoneAccount, "131010", "02", maxReturnRecord, startRecord, out txnItemList, out ErrMsg);
                //DateTime startDate, DateTime endDate, String besttoneAccount, String txnType, String txnChannel, Int32 maxReturnRecord, Int32 startRecord, 
                //                                            out IList<TxnItem> txnItemList, out String ErrMsg
            




            Console.WriteLine("");
            Console.WriteLine("");

        }

        static String CreateTransactionID()
        {
            String date = DateTime.Now.ToString("yyMMddHHmmss");

            //6位随机数
            Random r = new Random(Guid.NewGuid().GetHashCode());
            String TransactionID = "35" + date + r.Next(100000, 999999).ToString();

            return TransactionID;
        }


        //BesttoneAccountHelper.QueryBesttoneAccount
        static void QueryBesttoneAccount()
        {
            //陈先生   18953197107
            //陈先生   18953197116
            //BesttoneAccountInfoQuery
            AccountItem ai = new AccountItem();
            String ErrMsg = "";
            int Result = BesttoneAccountHelper.QueryBesttoneAccount("15301588430", out ai, out ErrMsg);  //13661832385  13594847333
        }

            DateTime startTime = DateTime.Now.AddDays(-10);
            DateTime endTime = DateTime.Now.AddDays(5);
            IList<TxnItem> txnItemList;
            
            //全部交易记录查询
            //Result = BesttoneAccountHelper.QueryAllTxnHistory(startTime, endTime, "18918790558", 10, 0, out txnItemList, out ErrMsg);

        static void Test15()
        {
            
            IDispatchControl serviceProxy = new IDispatchControl();

            String responseXml = String.Empty;
            Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
            String ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
            IList<TxnItem> txnItemList = new List<TxnItem>();
          
            StringBuilder requestXml = new StringBuilder();

            String TransactionID = CreateTransactionID();
            String besttoneAccount = "18917921662";
            String txnType = "121020";   //  121020(资金账户省平台（现金）充值)   (121010)资金帐户网银充值   131010(资金帐户消费)   (131030退费)
            String txnChannel = "02"; 



                #region 拼接请求xml字符串

                requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                requestXml.Append("<PayPlatRequestParameter>");
                requestXml.Append("<CTRL-INFO WEBSVRNAME=\"用户历史交易查询\" WEBSVRCODE=\"100105\" APPFROM=\"100105|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
                requestXml.Append("<PARAMETERS>");

                //添加参数
                requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", besttoneAccount);
                requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", txnType);
                requestXml.AppendFormat("<TXNCHANNEL>{0}</TXNCHANNEL>", txnChannel);
                requestXml.AppendFormat("<FROMDATE>{0}</FROMDATE>", "20120801");
                requestXml.AppendFormat("<TODATE>{0}</TODATE>", "20120912");
                requestXml.AppendFormat("<MaxRecord>{0}</MaxRecord>", "100");
                requestXml.AppendFormat("<StartRecord>{0}</StartRecord>", "1");
                requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "002310000000000");
                requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", "12345678901234567890");
                requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                requestXml.Append("</PARAMETERS>");
                requestXml.Append("</PayPlatRequestParameter>");

                #endregion

                //请求接口
                responseXml = serviceProxy.dispatchCommand("100105|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
                Console.WriteLine(responseXml);
                Console.WriteLine(responseXml);
                Console.WriteLine(responseXml);
        }

        static void PasswordPay()
        {
            IDispatchControl serviceProxy = new IDispatchControl();
            StringBuilder requestXml = new StringBuilder();
            String TransactionID = CreateTransactionID();
            String responseXml = String.Empty;
            String besttoneAccount = "18918790558";
            #region 拼接请求xml字符串

            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            requestXml.Append("<PayPlatRequestParameter>");
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"有密支付\" WEBSVRCODE=\"200503\" APPFROM=\"200503|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
            requestXml.Append("<PARAMETERS>");

            //添加参数
            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", besttoneAccount);
            requestXml.AppendFormat("<BUSINESSTYPE>{0}</BUSINESSTYPE>", "1201");   //1200 网银充值 1201 现金充值
            requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", "131010");
            requestXml.AppendFormat("<TXNPASSWD>{0}</TXNPASSWD>", "300");
            requestXml.AppendFormat("<PUBKEYINDEX>{0}</PUBKEYINDEX>", "1");  //FEEFLAG
            requestXml.AppendFormat("<TXNAMOUNT>{0}</TXNAMOUNT>", "300");
            requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "0");
            requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");   //
            requestXml.AppendFormat("<TRANSFERORGCODE>{0}</TRANSFERORGCODE>", "");
            requestXml.AppendFormat("<PAYORGCODE>{0}</PAYORGCODE>", "");
            requestXml.AppendFormat("<SUPPLYORGCODE>{0}</SUPPLYORGCODE>", "113310000000000");
            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "002310000000000");
            requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
            requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", "310000");
            requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", "310000");
            requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02 ");
            requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", "12345678901234567890");
            requestXml.AppendFormat("<ACCEPTTRANSDATE>{0}</ACCEPTTRANSDATE>", DateTime.Now.ToString("yyyyMMdd"));
            requestXml.AppendFormat("<ACCEPTTRANSTIME>{0}</ACCEPTTRANSTIME>", DateTime.Now.ToString("HHmmss"));
            requestXml.AppendFormat("<SUPPLYSEQNO>{0}</SUPPLYSEQNO>", DateTime.Now.ToString("HHmmss"));
            
            requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

            requestXml.Append("</PARAMETERS>");
            requestXml.Append("</PayPlatRequestParameter>");

            #endregion
            responseXml = serviceProxy.dispatchCommand("200503|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
            Console.WriteLine(responseXml);
            Console.WriteLine(responseXml);
            Console.WriteLine(responseXml);
        }

        static void charge()
        {
            IDispatchControl serviceProxy = new IDispatchControl();
            StringBuilder requestXml = new StringBuilder();
            String TransactionID = CreateTransactionID();
            String responseXml = String.Empty;
            String besttoneAccount = "yy18905394026";
            #region 拼接请求xml字符串

            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            requestXml.Append("<PayPlatRequestParameter>");
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"充值(营业厅)\" WEBSVRCODE=\"200401\" APPFROM=\"200401|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
            requestXml.Append("<PARAMETERS>");

            //添加参数
            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", besttoneAccount);

            requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1");          //1资金账户

            requestXml.AppendFormat("<BUSINESSTYPE>{0}</BUSINESSTYPE>", "1201 ");   //1200 网银充值 1201 现金充值

            requestXml.AppendFormat("<TXNTYPE>{0}</TXNTYPE>", "121020");

            requestXml.AppendFormat("<TXNAMOUNT>{0}</TXNAMOUNT>", "10000");


            requestXml.AppendFormat("<FEEFLAG>{0}</FEEFLAG>", "1");  //FEEFLAG


            requestXml.AppendFormat("<FEEAMT>{0}</FEEAMT>", "0");   //

            requestXml.AppendFormat("<TRANSFERORGCODE>{0}</TRANSFERORGCODE>", "");

            requestXml.AppendFormat("<PAYORGCODE>{0}</PAYORGCODE>", "");

            requestXml.AppendFormat("<SUPPLYORGCODE>{0}</SUPPLYORGCODE>", "113310000000000");

            requestXml.AppendFormat("<TERMINALSEQNO>{0}</TERMINALSEQNO>", "123456789");

          
           
            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "002310000000000");
            requestXml.AppendFormat("<ACCEPTUID>{0}</ACCEPTUID>", "");
            requestXml.AppendFormat("<ACCEPTAREACODE>{0}</ACCEPTAREACODE>", "310000");
            requestXml.AppendFormat("<ACCEPTCITYCODE>{0}</ACCEPTCITYCODE>", "310000");
            requestXml.AppendFormat("<ACCEPTCHANNEL>{0}</ACCEPTCHANNEL>", "02 ");
            


            requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", "12345678901234567890");
            requestXml.AppendFormat("<ACCEPTTRANSDATE>{0}</ACCEPTTRANSDATE>", DateTime.Now.ToString("yyyyMMdd"));
            requestXml.AppendFormat("<ACCEPTTRANSTIME>{0}</ACCEPTTRANSTIME>", DateTime.Now.ToString("HHmmss"));


            requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));

            requestXml.Append("</PARAMETERS>");
            requestXml.Append("</PayPlatRequestParameter>");

            #endregion
            responseXml = serviceProxy.dispatchCommand("200401|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
            Console.WriteLine(responseXml);
            Console.WriteLine(responseXml);
            Console.WriteLine(responseXml);

        }


        static void QueryTxnHistory()
        { 
            
        
        }


        static void queryXiaoFei()
        {
            String besttoneAccount = "18918790558";   //18930054353   13817872169   18964068379 (程玉坤)
            String txnType = "";   // 121020充值   131010消费  131030退费    121080充值  131090正式 
            //131010消费
            //131030退费

            String txnChannel = "02";

            IList<TxnItem> txnItemList = new List<TxnItem>();
            String ErrMsg = "";
            //Int32 Result = BesttoneAccountHelper.QueryAllTypeTxn(besttoneAccount, txnType, txnChannel, out txnItemList, out ErrMsg);

            DateTime startDate = Convert.ToDateTime("2013-05-01"); 
            DateTime endDate = Convert.ToDateTime("2013-07-18");
   
   

            Int32 maxReturnRecord = -1;
            Int32 startRecord = 1;

            Int32 Result = BesttoneAccountHelper.QueryHistoryTxn(startDate, endDate, besttoneAccount, txnType, txnChannel, maxReturnRecord, startRecord, out txnItemList, out ErrMsg);

       
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }


        static void queryXiaoFie2()
        {
            String besttoneAccount = "18918790558";  //18930054353   18019200662
            String txnType = "";   // 121020充值
            //131010消费
            //131030退费
            // "" 所有 
            String txnChannel = "02";

            IList<TxnItem> txnItemList = new List<TxnItem>();
            String ErrMsg = "";
            Int32 Result = BesttoneAccountHelper.QueryAllTypeTxn(besttoneAccount, txnType, txnChannel, out txnItemList, out ErrMsg);


            foreach (TxnItem ti in txnItemList)
            {
                Console.WriteLine(ti.AcceptDate);
                Console.WriteLine(ti.AcceptSeqNO);
                Console.WriteLine(ti.AcceptTime);
                Console.WriteLine(ti.CancelFlag);
                Console.WriteLine(ti.MerchantName);
                Console.WriteLine(ti.TxnAmount);
                Console.WriteLine(ti.TxnChannel);
                Console.WriteLine(ti.TxnDscpt);
                Console.WriteLine(ti.TxnType);

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            //System.Data.DataSet XMLDataSet = new DataSet(); // youyong 
            //StringReader sr = new StringReader("xml");     // youyong
            //{
            //st.ReadXml(sr);
            //    XMLDataSet.ReadXml(sr);   // youyong
            //datalist.DataSource = XMLDataSet.Tables["aaa"]; // youyou
            //} 

            //XMLDataSet.


        }


        static void queryCharge()
        {
            String besttoneAccount = "18019200662";
            String txnType = "";   // 121020充值
                                         //131010消费
                                         //131030退费

            String txnChannel = "02";
                                                            
            IList<TxnItem> txnItemList = new List<TxnItem>(); 
            String ErrMsg = "";
            Int32 Result = BesttoneAccountHelper.QueryAllTypeTxn(besttoneAccount, txnType, txnChannel, out txnItemList, out ErrMsg);


            foreach (TxnItem ti in txnItemList)
            {
                Console.WriteLine(ti.AcceptDate);
                Console.WriteLine(ti.AcceptSeqNO);
                Console.WriteLine(ti.AcceptTime);
                Console.WriteLine(ti.CancelFlag);
                Console.WriteLine(ti.MerchantName);
                Console.WriteLine(ti.TxnAmount);
                Console.WriteLine(ti.TxnChannel);
                Console.WriteLine(ti.TxnDscpt);
                Console.WriteLine(ti.TxnType);
            
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            //System.Data.DataSet XMLDataSet = new DataSet(); // youyong 
            //StringReader sr = new StringReader("xml");     // youyong
            //{
                //st.ReadXml(sr);
            //    XMLDataSet.ReadXml(sr);   // youyong
                //datalist.DataSource = XMLDataSet.Tables["aaa"]; // youyou
            //} 

            //XMLDataSet.

            
        }


        static void queryBalance()
        {
            string phoneNum = "18620863009";   //18930054343
            AccountItem ai = new AccountItem();
            string ErrMsg = "";
            int QueryBesttoneAccountResult = BesttoneAccountHelper.QueryBesttoneAccount(phoneNum, out ai, out ErrMsg);
        }

        static void QueryCardBalance()
        {
            String cardno = "8811000000510363";
            long balance = 0;
            String ErrMsg = "";

            BesttoneAccountHelper.QueryCardBalance(cardno, "Y", out balance, out ErrMsg);

            Console.WriteLine(balance);
            Console.WriteLine(balance);
            Console.WriteLine(balance);
            Console.WriteLine(balance);
        }


        static void QueryAccountBalance()
        {
            String besttoneAccount = "18918790558";    //13817369457   13917639071
            long balance = 0;
            String ErrMsg = "";
            BesttoneAccountHelper.QueryAccountBalance(besttoneAccount,out balance,out ErrMsg);
        }




        static void testAssertion()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<CAPRoot>");
            sb.Append("<SessionHeader>");
            sb.Append("<ActionCode>1</ActionCode>");
            sb.Append("<TransactionID>35000201211228921198171</TransactionID>");
            sb.Append("<RspTime>20121122085909</RspTime>");
            sb.Append("<DigitalSign />");
            sb.Append("<Response>");
            sb.Append("<RspType>0</RspType>");
            sb.Append("<RspCode>0000</RspCode>");
            sb.Append("<RspDesc>oper success</RspDesc>");
            sb.Append("</Response>");
            sb.Append("</SessionHeader>");
            sb.Append("<SessionBody>");
            sb.Append("<AssertionQueryResp>");
            sb.Append("<Assertion>");
            sb.Append("<AssertionID>3833866</AssertionID>");
            sb.Append("<Issuer>");
            sb.Append("<UAID>15</UAID>");
            sb.Append("<UA_URL>http://218.65.103.196/UAWeb/</UA_URL>");
            sb.Append("<NotBefore>2012-11-22 09:00:54</NotBefore>");
            sb.Append("<NotOnOrAfter>2012-11-22 09:30:54</NotOnOrAfter>");
            sb.Append("</Issuer>");
            sb.Append("<IssueInstant>2012-11-22 08:59:09</IssueInstant>");
            sb.Append("<AudienceID>35000</AudienceID>");
            sb.Append("<AssertionStatement>");
            sb.Append("<AuthInstant>2012-11-22 09:00:54</AuthInstant>");
            sb.Append("<AuthMethod>000</AuthMethod>");
            sb.Append("<AccountType>2000001</AccountType>");
            sb.Append("<AccountID>6417886</AccountID>");
            sb.Append("<PWDType>01</PWDType>");
            sb.Append("<AccountList>");
            sb.Append("<AccountInfo>");
            sb.Append("<AccountType>0000000</AccountType>");
            sb.Append("<AccountID>2790115778690000</AccountID>");
            sb.Append("<ServiceID></ServiceID>");
            sb.Append("<CityCode>0790</CityCode>");
            sb.Append("<PWDAttrList>");
            sb.Append("<PWDAttr>");
            sb.Append("<AttrName>accountType</AttrName>");
            sb.Append("<AttrValue>0000000</AttrValue>");
            sb.Append("</PWDAttr>");
            sb.Append("<PWDAttr>");
            sb.Append("<AttrName>CityCode</AttrName>");
            sb.Append("<AttrValue>0790</AttrValue>");
            sb.Append("</PWDAttr>");
            sb.Append("</PWDAttrList>");
            sb.Append("</AccountInfo>");
            sb.Append("</AccountList>");
            sb.Append("</AssertionStatement>");
            sb.Append("</Assertion>");
            sb.Append("</AssertionQueryResp>");
            sb.Append("</SessionBody>");
            sb.Append("</CAPRoot>");


        }





























        static void jiami()
        {
            /**
            RSACryptoServiceProvider rsaProvider;
            rsaProvider = new RSACryptoServiceProvider(1024);

            String PublicKey = rsaProvider.ToXmlString(false);
            String PrivateKey = rsaProvider.ToXmlString(true);

            Console.WriteLine(PublicKey);
            Console.WriteLine(PrivateKey);


            Console.WriteLine(PublicKey);
            Console.WriteLine(PrivateKey); 
            
            Console.WriteLine(PublicKey);
            Console.WriteLine(PrivateKey);
            **/
            //string source = "cardNo=8888000000051151&password=553509";
            //string key = "G7AXS7874305BV590000000000000000";

            //string result = CryptographyUtil.EncryptBestPay(source,key);

            String result = CryptographyUtil.Encrypt("某某$18019200662$18019200662$$2$1$111111111111111$", "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534");
            //String result = CryptographyUtil.Encrypt3DES("123456789", "F05F7D4722AE7C3A61ADE9475A89928BA623BF99E96E4F0");
            //F05F7D4722AE7C34A61ADE9475A89928BA623BF99E96E4F0
            //F05F7D4722AE7C3A61ADE9475A89928BA623BF99E96E4F0
            //CryptographyUtil.Encrypt("");
            //F05F7D4722AE7C34A61ADE9475A89928BA623BF99E96E4F0
            Console.WriteLine(result);
            Console.WriteLine(result);
            Console.WriteLine(result);
            Console.WriteLine(result);
        }


        static bool PhoneNumValid( string PhoneNum, out string OutPhone)
        {
            bool result = true;
            OutPhone = "";
            /*
                        //string dStyle = @"^0\d{2,3}[1-9]\d{6,7}$";
                        string dStyle = @"^(\d{3,4}\d{7,8})$";
                        if (CommonUtility.ValidateStr(PhoneNum, dStyle))
                        {
                            result = false;
                        }
                        if (PhoneNum.Length != 11 & PhoneNum.Length != 12)
                        {
                            result = false;
                        }
                        */
            string dStyle = @"^(\d{3,4}\d{7,8})$";
            if (!CommonUtility.ValidateStr(PhoneNum, dStyle))
            {
                result = false;
            }
            string leftStr = PhoneNum.Substring(0, 1);
            if (leftStr == "0")
            {
                string leftStr2 = PhoneNum.Substring(1, 1);
                string leftStr3 = PhoneNum.Substring(2, 1);
                switch (leftStr2)
                {
                    case "1":
                        if (leftStr3 == "0")
                        {
                            if (PhoneNum.Length == 11)
                            {
                                OutPhone = PhoneNum;
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (PhoneNum.Length == 12)
                            {
                                OutPhone = PhoneNum.Substring(1, 11);
                                string LeftPhone = OutPhone.Substring(0, 1);
                                if (LeftPhone == "0")
                                {
                             
                        
                                }
                            }
                            else
                            {
                                result = false;
                            }

                        }
                        break;
                    case "2":
                        if (PhoneNum.Length == 11)
                        {
                            OutPhone = PhoneNum;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    default:
                        if (PhoneNum.Length == 12 || PhoneNum.Length == 11)
                        {
                            OutPhone = PhoneNum;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                }
            }
            else if (leftStr == "1")
            {
                if (PhoneNum.Length != 11)
                {
                    result = false;
                }
                OutPhone = PhoneNum;
            }
            else
            {
                result = false;
            }

            return result;
        }


 


        private static void testYY()
        {

            string[] custinfos = new string[] { 
            
                 "15045099780|邓|23080519810911041X",
                "13996309898|李闯|500112199001020856",
                "15021180005|袁凯|310227198412240036",
                "15159092017|王丽|350824198707304181",
                "15155323290|张宇|340802198212210829",
                "15305691632|唐琼|341124197904170027",
                "15305776116|方敏|330325690628021",
                "15305779172|林健|330327197608240411",
                "15360552990|杨露|450981198611131120",
                "15326049027|彭岩|150428198412140024",
                "15327262212|印杰|422427197011113697",
                "15339629218|郑钰|342401198509040748",
                "15507556713|杨彬|430981198608165158",
                "15800639682|陆玲|310115199112277228",
                "15806025397|林帅|35012119881013171x",
                "13809578883|游荣|352231197408060031",
                "13801758067|金峥|310227195504180225",
                "13818447542|沈强|310229198703173411",
                "13856050507|章虎|340102198101010535",
                "13857155096|陈哲|330323197410201622",
                "13877576867|莫秋|452501196208250014",
                "13901695529|陈劼|310106198703180814",
                "13912782321|金凤|320826198102250229",
                "13917355688|徐敏|310104196501104424",
                "13917371974|金超|34088119891205034X",
                "13952047166|朱静|32010519800511062x",
                "13956738136|孙涛|342125197810190219",
                "13983547588|dd|512227197312010019",
                "13986240688|严菲|420103197901020813",
                "13303465123|李理|110108196611016394",
                "13311176302|汪冰|341021198811053008",
                "13314058952|刘帅|211002198601280116",
                "13328228434|陈稼|350181197803035879",
                "13030897398|邓聪|350403198602187018",
                "13052187227|瞿谊|310224197502092811",
                "13321388495|吕锋|620503198502091415",
                "13383025502|金松|130683199108157610",
                "13386178872|徐洁|310112198701142125",
                "13480257063|王静|431024198309231826",
                "13488807675|周博|110108198711164932",
                "13512132456|毛玥|130403197704052143",
                "13581500782|李丹|110101199203254026",
                "13605130612|赵东|320704198203155016",
                "13598055386|侯萍|410122196101170023",
                "13621609981|张倩|310109198205143024",
                "13635537260|张琼|422103198102178223",
                "13625017150|林芬|350822197907043927",
                "13770783728|吕斌|320721198603160013",
                "15855124251|王建|340123198002174333",
                "15882256476|蒋慧|230403198302260829",
                "15907113808|范磬|42070319861001340X",
                "15943794111|张雷|220581198505231174",
                "15951856625|康燕|220523198410030122",
                "15956765681|张永|341224198812069896",
                "15960222576|徐勉|350302198505190010",
                "15979319130|章莹|362502198709296081",
                "15991663358|秦振|610123197409050536",
                "18019270142|石兴|620523198310055319",
                "18049805196|陈鑫|31011019821206125x",
                "18051505130|朱莉|341022198507050925",
                "18066570715|潘伟|610525198703242810",
                "18012626765|张斌|320524197910232471",
                "18016391539|孙磊|310102197103083631",
                "18269113255|辰辰|452127199210100918",
                "18276812883|黄菊|452224198407123525",
                "18616381633|朱震|310110198111273317",
                "18616811570|严晨|310110198207310557",
                "18623585626|袁梦|500105199306021223",
                "18672920583|黄鉴|45098119860506171x",
                "18680315321|张虎|370826198812054731",
                "18706254007|周琦|320525199304090261",
                "18901109127|何平|110102196410083015",
                "18607636677|赵丹|220602198306091527",
                "18902881068|康征|440782197802058017",
                "18905527610|孙辉|340302198901021217",
                "18907278819|宋可|42130219910107421x",
                "18908511389|张惠|520103197001221226",
                "18909860929|赵阳|210204198404280734",
                "18916610098|沈巍|310228197808270022",
                "18918561632|孙峰|310101197412091612",
                "18918790590|张倩|310109198205143024",
                "18920028433|陈宇|120102198107041411",
                "18920213860|张毅|320321199102102870",
                "18920320525|李佳|120103198110214519",
                "18920801163|高睿|120221198207040023",
                "18930799369|梁桥|610103197911082490",
                "18930952965|李云|222403197108200641",
                "18922835970|伍辉|42212919700920297X",
                "18939925257|周婷|310103198512037100",
                "18949841257|邓超|342426199003080058",
                "18955208186|王建|340302197103250416",
                "18970866733|樊星|360103198109274148",
                "18971398347|吴娜|420105198204254224",
                "18972007200|高菲|420502198604281363",
                "18972560903|罗毅|420502198511031111",
                "18975860648|李立|430105198410103517",
                "18977140788|周悦|450105198311010036",
                "18985316566|朱珠|522527197903260026",
                "18988294598|陈强|330327198905170039",
                "18995110085|吴爽|642102197710040025",
                "18995192989|夏燕|51070319820810134X",
                "18996640101|王敏|512225196203309532",
                "18990316599|殷波|511126198307184710",
                "18990802252|蒋军|51132319860712587X",
                "18996640269|谭睿|51222519760419411X",
                "18987334941|李洪飞|532930199109031710",
                "18994080810|陈晓东|320106196903232028",
                "18996640188|王凤鸣|511224197808030013",
                "18988532597|赵子鸣|612422198307294413",
                "18979273760|刘云云|360425198601056737",
                "18979820597|余春辉|360202198701205511",
                "18981057484|汤学周|510623198204016439",
                "18982905626|邱洪英|511027197704244808",
                "18985141017|谢本亮|210302197810110931",
                "18973260790|周应军|430304196803011029",
                "18974814532|齐赞成|430124197606200030",
                "18971639232|邓全华|429006198303307028",
                "18971810122|吴浩泉|422325198002230038",
                "18955385589|社联办|342423198502240040",
                "18956002520|仰曙明|340104198306070032",
                "18958211955|甘永文|362524198010181035",
                "18964063430|肖晨江|320583198508016716",
                "18964521380|桑南鹏|310110198009138813",
                "18964555720|陆宇超|310105199006161237",
                "18964625418|章震祥|310101195804221632",
                "18964860718|王家乐|310104198607180854",
                "18952266757|王茂森|320381196709274452",
                "18955166573|窦树霞|340102195706304022",
                "18945315678|史慧明|232321198610122320",
                "18930085979|孙盛晖|310102198101091229",
                "18930580367|赵兴华|310111194712011214",
                "18934793451|叶秀玲|450322198702104045",
                "18939877825|张志国|321302198204150416",
                "18920945988|杨奎生|120225197104260817",
                "18919068237|刘彦彦|622425199001157622",
                "18919817199|畅年盛|110108196601236414",
                "18918588779|管仁勤|41071119640215002X",
                "18918790558|李宏图|310107197409092014",
                "18916657515|郭云光|512225197006219559",
                "18916776787|张冬峰|41128119780913351X",
                "18917653917|乐丽芳|31011019580330044X",
                "18910528700|王学强|110106198103075116",
                "18911352207|李光哲|222405198009080637",
                "18916037673|庄永祥|310106195408040014",
                "18916204759|仇宏明|341122198710275818",
                "18909062855|郭佳嘉|510902198409039014",
                "18907182801|吴才涛|420102197711010335",
                "18907186431|甘传红|420106196805074912",
                "18903409234|侯宝珠|142322198509053011",
                "18903710416|王德政|411322198704162031",
                "18611706297|王海涛|110101198203061553",
                "18901339366|符美琳|460032197912300021",
                "18901678090|林志华|310109198112252539",
                "18902843655|贺双飞|432522198406240316",
                "18721632022|朱哲梅|34242519850623672X",
                "18786666041|廖晓燕|522701198804050025",
                "18807424299|陈先利|430121199006262246",
                "18659218922|洪金买|350583195603201014",
                "18668295525|徐盈盈|330282198310172843",
                "18287563202|丁学会|533022198312280330",
                "18605246732|朱国京|321183198611064416",
                "18017278539|孙佳洁|310115198711102986",
                "18017490967|陈佳祥|310102195306020017",
                "18019120798|肖丰平|432424196709212871",
                "18019200662|张jf|111111111111111",
                "18068032517|路明正|320322198212225611",
                "18070767800|蔡肖梅|452824197712310042",
                "18073113117|沈友军|430421198701203274",
                "18075993577|吴祥辉|431230197709270019",
                "18096642839|刘洪侠|342101198203058420",
                "18121079773|卞迺斌|310102196805124851",
                "18255929993|严雪花|222406198911140047",
                "18259202892|陈南阳|350221197210193029",
                "18053229106|高晋东|37082919901231253X",
                "18055190206|刘登冬|340111197511151033",
                "18056767156|裴同辉|341281198608052058",
                "18056875068|荣张华|342622196308010095",
                "18056918137|檀应楼|340827197711256015",
                "18057678011|孔雪芝|331021198611082785",
                "18058870806|徐天然|330302198909257361",
                "18059298234|黄凌健|350823199110203730",
                "18059632211|范伟民|350204196108314014",
                "18019310301|孔凡安|370104194809282931",
                "18019467325|黄鸿志|362329198906300034",
                "18021070657|周月凌|310108198209210531",
                "18026020077|陈崇捷|445202198911140618",
                "18028229887|陈洪平|510524198210214635",
                "18036115580|胡年花|341182198512073226",
                "18046424243|陈燕娥|350322198308234825",
                "18001757563|李红征|340104198203122039",
                "18007522755|李焕春|445222198410014512",
                "18009960958|胡晓明|652827197201020314",
                "15984616690|陈庆德|510722196402288814",
                "15978991825|黄仕产|500384199006290313",
                "15900519774|徐雪莹|310105199001310088",
                "15855779199|吴莉莉|340302197806161225",
                "15859719323|叶坤茂|350583197812107111",
                "13774802773|方琼霞|350583198706291829",
                "13632081833|黄小姐|440782198406266863",
                "13675150707|宋佳颖|42011119750102732X",
                "13681912941|张介洪|310225198809112216",
                "13706120299|尤云梅|320402198303253140",
                "13714062900|翟先生|410423197107189515",
                "13725938128|吴常利|410901197306062786",
                "13732649871|崔小辉|320323198409284013",
                "13760475674|朱志轩|341124201002253818",
                "13636966064|陈永辉|520221199010242114",
                "13645662879|张翔鸥|340803197811152124",
                "13621917731|乔张奇|310110198209200052",
                "13600859793|乔长虹|210504197808220027",
                "13600951797|黄宁峰|352101196611020336",
                "13611609979|石丽华|310107195904292866",
                "13611644077|包玲红|310107196312132425",
                "13611770418|季宇锋|31010319780418321x",
                "13585975270|魏朝辉|61010319700419371x",
                "13587691321|徐曼萍|330302194907190062",
                "13589561515|宋礼鹏|422127198109190912",
                "13519857898|陈锡伟|440104196101192516",
                "13520085228|张为伟|342623198705053414",
                "13520213856|胡章成|342901198304147434",
                "13524400070|朴花星|231026197111221826",
                "13524546308|xyz|430922198902144614",
                "13553241404|赖晓辉|441425198008184392",
                "13559221418|陈根金|350524199107237411",
                "13505516064|陈永霞|342901198211126423",
                "13509393177|庄春枝|350102196802171510",
                "13386345687|李东卫|371202197409177718",
                "13388941433|杨仁飞|500233199202070152",
                "13424198750|赵新刚|610324198404282815",
                "13438438466|万建国|429001198403306936",
                "13384022500|郭同飞|410883198609223018",
                "13384627314|周芳新|230227196804242824",
                "13385218187|彭伟明|42900619870402271X",
                "13321902582|宁振云|460200196204095333",
                "13323187321|郑经义|13112119890603501X",
                "13101401049|李创华|440402198511209272",
                "13331633054|金成男|222405196002090673",
                "13333545029|王东平|142401198012224536",
                "13335391589|张建龙|610322198603293311",
                "13339011608|郑良水|340111197812253519",
                "13353752551|马晓晖|410482199106119039",
                "13376016616|孙春元|32108319770404713x",
                "13379101690|刘晓华|61012319820121126x",
                "13380540291|林嘉恒|441781198204100018",
                "13315992533|葛祥方|371312198611015811",
                "13316090486|陈学亮|362426198410230091",
                "13318739178|蓝仲弦|440105195910253990",
                "13306055130|颜耿忠|350583196208110715",
                "13306323520|张道宽|370481198408060016",
                "13307719905|莫名理|450111197707150919",
                "13309089261|陈卫斌|510502197105141451",
                "13984830546|陆石民|432524197710062655",
                "13959065310|陈君丽|420822198210175543",
                "13960219761|李剑明|350583198704103716",
                "13960461641|侯小p|350583198909049266",
                "13962520444|胡发林|320524197404166811",
                "13977119938|赵西宁|610103196506082839",
                "13918181662|余先锋|422130198203024353",
                "13924302090|叶卫红|440703196904076021",
                "13950815526|陈少芬|35068119831128052X",
                "13916279604|张婷华|31011519841023562X",
                "13917248421|林佳伟|310101198411120519",
                "13908789186|梁晓萍|532301197108140049",
                "13910169216|褚高卓|450124197906258799",
                "13882128710|何光彬|51052219850604357x",
                "13882847641|邓淇峰|513030198105080017",
                "13883635035|王曼人|510214198102185129",
                "13896214856|甘承琼|512227197202190020",
                "13899859618|蒋开明|652324197509253839",
                "13860633722|陈秀清|350121196811285041",
                "13866385112|贾瑞霞|230709198010060125",
                "13856244160|张金兰|340702690228106",
                "13818889178|祝佳欣|310105198407233612",
                "13830338646|郑敬尧|622701198307243073",
                "13805603789|罗莉莉|342501198206130549",
                "13816256708|李晓武|342622198209230796",
                "13816672864|韩香兰|410381198702022069",
                "15832825689|周庆刚|133022195704063230",
                "15340300969|周志建|513525195511151039",
                "15345200000|王正广|320830197710053210",
                "15372999218|贾金康|330724199708166919",
                "15385719527|丁宝建|342201198312134731",
                "15385948737|卢文志|342401199212114472",
                "15388931151|孙钦斌|371327198705282755",
                "15391408348|张传敏|532625198509170515",
                "15391676686|夏望炉|422326198101266593",
                "15396036266|陈娅玲|350723199101112161",
                "15308112702|王福海|510704198510281739",
                "15319101085|谢宏启|612127197702210030",
                "15326011717|李俊霞|152524198712250024",
                "15305776827|王松光|330325196702226512",
                "15156689599|刘群生|341223198405181315",
                "15172577994|郑彦文|610427199211261910",
                "15256530834|王凤霞|34080219830510112X",
                "15294003645|秦国兵|622225198106292112",
                "15301588277|易志坚|320103197205012058",
                "15303487250|胡彦辉|142331198108144218",
                "15060738331|赖国书|350823198703175312",
                "13668122389|cole|510102197203055328",
                "15956965018|刘金秋慧|340303198610010629",
                "18944714999|C网预开通|310106196007072028",
                "13671682003|louis|310106197911141612",
                "13565108155|克热木阿吉|653021198311240418",
                "18985940189|liweibin|520201197607240010",
                "13816855743|shaoxiaqin|310103195504233220",
                "13991889617|niesanyuan|610104196307246219",           
            };
            string ErrMsg = "";
            for (int i = 0; i < custinfos.Length; i++)
            {
                string[] custinfo = custinfos[i].Split('|');
                if (CommonUtility.CheckIDCard(  custinfo[2]))
                {
                    int ret = BesttoneAccountHelper.NotifyBesttoneAccountInfo(custinfo[0], custinfo[1], "1", custinfo[2], custinfo[0] + "@189.cn", out ErrMsg);
                    Console.WriteLine("ret="+ret);
                    
                }
            }


        }




        private static CustAuthenInfoRecord[] getCustAuthenInfoRecord(string CustID, out int Result, out string ErrMsg)
        {
            CustAuthenInfoRecord[] CustAuthenInfoRecords = null;
            Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_V3_Interface_getCustAuthenInfo";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int RowCount = ds.Tables[0].Rows.Count;
                            CustAuthenInfoRecords = new CustAuthenInfoRecord[RowCount];
                            CustAuthenInfoRecord CustAuthenInfoRecord = null;
                            for (int i = 0; i < RowCount; i++)
                            {
                                CustAuthenInfoRecord = new CustAuthenInfoRecord();
                                CustAuthenInfoRecord.AuthenName = ds.Tables[0].Rows[i]["AuthenName"].ToString().Trim();
                                CustAuthenInfoRecord.AuthenType = ds.Tables[0].Rows[i]["AuthenType"].ToString().Trim();
                                CustAuthenInfoRecords[i] = CustAuthenInfoRecord;
                            }

                            Result = ErrorDefinition.IError_Result_Success_Code;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = "获取客户验证名出错，" + e.Message;
            }

            return CustAuthenInfoRecords;
        }











        static void RegisterBesttoneAccount()
        {
            String phoneNumber = "15305512985";
            String realName = "郝方辉";
            String contactTel = "15305512985";
            String contactMail = "15305512985@189.cn";
            String sex="1";
            String certtyp="1";
            String certnum = "370631197601218033";
            String TransactionID= "1234567890"; 
            String ErrMsg = "";
            int ret = BesttoneAccountHelper.RegisterBesttoneAccount(phoneNumber, realName, contactTel, contactMail, sex, certtyp, certnum, TransactionID,out ErrMsg);
    
            
        }
    }
}

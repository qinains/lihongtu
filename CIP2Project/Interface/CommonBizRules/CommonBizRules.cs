using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;
using System.Web;


using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;


namespace Linkage.BestTone.Interface.Rule
{
    public class CommonBizRules
    {


        public static bool HasBesttoneAccount(HttpContext context, string PhoneNum, out string CustID,out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            bool result = true;
            CustID = "";
            try
            {
                String sql = " select * from BesttoneAccount where 1=1 and BestPayAccount='" + PhoneNum + "'";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;
                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null && ds.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CustID = row["CustID"].ToString();
                        if (String.IsNullOrEmpty(CustID))
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }
            return result;
        }

        /// <summary>
        /// ��֤�绰������Ч��
        /// </summary>
        public static bool PhoneNumValid(HttpContext context,string PhoneNum,out string OutPhone)
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
            if(leftStr=="0")
            {
                string leftStr2 = PhoneNum.Substring(1, 1);
                string leftStr3 = PhoneNum.Substring(2, 1);
                switch (leftStr2)
                {
                    case "1":
                        if (leftStr3 == "0")
                        { 
                            if(PhoneNum.Length==11)
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
                            if(PhoneNum.Length==12)
                            {
                                OutPhone = PhoneNum.Substring(1, 11);
                                string LeftPhone = OutPhone.Substring(0, 1);
                                if (LeftPhone == "0")
                                {
                                    PhoneAreaInfoManager Pam = new PhoneAreaInfoManager();
                                    //HttpContext context
                                    Object SPData = Pam.GetPhoneAreaData(context);

                                    string areaname = Pam.GetProvinceIDByPhoneNumber(OutPhone, SPData);
                                    if (areaname == "")
                                    {
                                        result = false;
                                    }
                                    else
                                        result = true;
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
                    default :
                        if (PhoneNum.Length == 12 || PhoneNum.Length == 11 )
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
            else if(leftStr=="1")
            {
                if(PhoneNum.Length!=11)
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

        /// <summary>
        /// v3�汾��SPIDֱ�Ӵ��͸���������
        /// </summary>
        /// <param name="PhoneNum"></param>
        /// <param name="Message"></param>
        /// <param name="SPID"></param>
        public static void SendMessageV3(string PhoneNum, string Message, string SPID)
        {
            string ErrMsg = "";
            string Result = "-19999";
            try
            {
                SmsClient sc = new SmsClient();
                //sc.sendSingleSms("02120906193", PhoneNum, Message);
                sc.sendSingleSms(SPID, "02120906193", PhoneNum, Message);
                Result = "0";
                ErrMsg = "���ͳɹ�";
            }
            catch (Exception e)
            {
                ErrMsg = e.ToString();
            }
            finally
            {
                string IsWriteLog = ConfigurationManager.AppSettings["IsWriteLog"];
                try
                {
                    if (IsWriteLog == "0")
                    {
                        CommonBizRules.WriteDataLog(SPID, "", "", int.Parse(Result), ErrMsg, PhoneNum, "SendMessage");
                    }
                }
                catch { }
            }

        }


        /// <summary>
        /// ֪ͨ��������
        /// Author Lihongtu
        /// </summary>
        /// <param name="PhoneNum"></param>
        /// <param name="Message"></param>
        /// <param name="SPID"></param>
        public static void SendMessageV2(string PhoneNum, string Message, string SPID)
        {
            string ErrMsg = "";
            string Result = "-19999";
            try
            {
                SmsClient sc = new SmsClient();
                sc.sendSingleSms("02120906193", PhoneNum, Message);
                Result = "0";
                ErrMsg = "���ͳɹ�";
            }
            catch (Exception e)
            {
                ErrMsg = e.ToString();
            }
            finally
            {
                string IsWriteLog = ConfigurationManager.AppSettings["IsWriteLog"];
                try
                {
                    if (IsWriteLog == "0")
                    {
                        CommonBizRules.WriteDataLog("35000000", "", "", int.Parse(Result), ErrMsg, PhoneNum, "SendMessage");
                    }
                }
                catch { }
            }

        }

        /// <summary>
        /// ֪ͨ��������
        /// </summary>
        public static void SendMessage(string PhoneNum, string Message, string SPID)
        {
            string ErrMsg = "";
            string Result = "-19999";
            try
            {
                UnifyInterfaceProxy obj = new UnifyInterfaceProxy();
                string UnifyInterUrl = ConfigurationManager.AppSettings["UnifyInterUrl"];
                obj.Url = UnifyInterUrl;
                Result = obj.insertSendDatas(PhoneNum, Message, SPID);
                XmlDocument xmlD = new XmlDocument();
                xmlD.LoadXml(Result);
                ErrMsg = xmlD.GetElementsByTagName("errorDescription")[0].InnerText;
                Result = xmlD.GetElementsByTagName("result")[0].InnerText;
                //���ʧ��������
            }
            catch (Exception ex)
            {
                string df = ex.Message;
            }
            finally
            {
                string IsWriteLog = ConfigurationManager.AppSettings["IsWriteLog"];
                try
                {
                    if (IsWriteLog == "0")
                    {
                        CommonBizRules.WriteDataLog("35000000", "", "", int.Parse(Result), ErrMsg, PhoneNum, "SendMessage");
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// ��xml�ַ����л�ȡֵ
        /// </summary>
        public static string GetValueFromXmlStr(string XmlStr, string NodeName)
        {
            string nodeValue = "";

            try
            {
                XmlDocument xmlReader = new XmlDocument();
                xmlReader.LoadXml(XmlStr);

                XmlNodeList nodeList = null;

                nodeList = xmlReader.GetElementsByTagName(NodeName);
                nodeValue = (nodeList.Count != 0) ? nodeList[0].InnerText : "";
            }
            catch (Exception ex)
            {
                nodeValue = "";
            }

            return nodeValue;
        }

        /// <summary>
        /// ת��IP����
        /// ���ߣ�Է��      ʱ�䣺2009-8-11
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public static long GetIPAddressIPNumber(string IPAddress)
        {

            long IPAddressValue = 0;
            long Temp_IPAddress_First = 0;
            long Temp_IPAddress_Second = 0;
            long Temp_IPAddress_Third = 0;
            long Temp_IPAddress_Forth = 0;

            string[] tmp = IPAddress.Split('.');

            Temp_IPAddress_First = Convert.ToInt64(tmp[0]);
            Temp_IPAddress_Second = Convert.ToInt64(tmp[1]);
            Temp_IPAddress_Third = Convert.ToInt64(tmp[2]);
            Temp_IPAddress_Forth = Convert.ToInt64(tmp[3]);

            IPAddressValue =
                Temp_IPAddress_First * ConstDefinition.IPAddress_First +
                Temp_IPAddress_Second * ConstDefinition.IPAddress_Second +
                Temp_IPAddress_Third * ConstDefinition.IPAddress_Third +
                Temp_IPAddress_Forth * ConstDefinition.IPAddress_Forth;

            return IPAddressValue;
        }

        /// <summary>
        /// IP�Ƿ��������
        /// ���ߣ�Է��      ʱ�䣺2009-8-11
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public static int CheckIPLimit(string SPID, string IP, HttpContext context, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            DataTable dt = null;
            try
            {
                string IsIPLimit = System.Configuration.ConfigurationManager.AppSettings["IsIPLimit"];
                //���������򷵻�����
                if (IsIPLimit == "1")
                {
                    Result = 0;
                    return Result;
                }
                //���ݴ���IP��ȡIPNumber
                long IPNumber = CommonBizRules.GetIPAddressIPNumber(IP);
                //�ӻ����л�ȡ����
                SPInfoManager spInfo = new SPInfoManager();
                SPIPListData SPIPListData = (SPIPListData)spInfo.GetSPData(context, "SPIPListData");

                dt = SPIPListData.Tables[SPIPListData.TableName];
                long StartIPIPNumber = 0;
                long EndIPIPNumber = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (SPID == dt.Rows[i][SPIPListData.Field_SPID].ToString())
                    {
                        StartIPIPNumber = long.Parse(dt.Rows[i][SPIPListData.Field_StartIPNumber].ToString());
                        EndIPIPNumber = long.Parse(dt.Rows[i][SPIPListData.Field_EndIPNumber].ToString());
                        //����ɣ��������б�����ɹ�
                        if (IPNumber >= StartIPIPNumber && IPNumber <= EndIPIPNumber)
                        {
                            Result = 0;
                            return Result;
                        }
                    }
                }

                Result = ErrorDefinition.BT_IError_Result_BizIPLimit_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;

        }


        /// <summary>
        ///   �ӿڷ���Ȩ������
        ///  Author Lihongtu
        ///  Createtime 2013-04-12
        /// </summary>
        /// <param name="SPID"></param>
        /// <param name="InterfaceName"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static int SPInterfaceGrant(String SPID, String InterfaceName, HttpContext context, out String ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = SPID +":accessd denied for "+InterfaceName+"method!";
           
            try
            {
                String sql = " select * from SPInterfaceGrant where 1=1 and SPID='"+SPID+"'";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;
                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);
                if (ds != null && ds.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        String _spid = row["SPID"].ToString();
                        String _interfaceName = row["interfaceName"].ToString();
                        if (_spid.Equals(SPID) && _interfaceName.Equals(InterfaceName))
                        {
                            Result = 0;
                            ErrMsg = "";
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;
        
        }



        /// <summary>
        /// �ӿڷ���Ȩ���ж�
        /// ���ߣ�Է��      ʱ�䣺2009-8-11
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public static int CheckInterfaceLimit(string SPID, string InterfaceName, HttpContext context, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            DataTable dt = null;
            try
            {
                string IsInterfaceLimit = System.Configuration.ConfigurationManager.AppSettings["IsInterfaceLimit"];
                //���������򷵻�����
                if(IsInterfaceLimit=="1")
                {
                    Result = 0;
                    return Result;
                }
                //�ӻ����л�ȡ����
                SPInfoManager spInfo = new SPInfoManager();
                SPInterfaceLimitData SPInterfaceLimitData = (SPInterfaceLimitData)spInfo.GetSPData(context, "SPInterfaceLimitData");

                dt = SPInterfaceLimitData.Tables[SPInterfaceLimitData.TableName];
                string tmpInterfaceName = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (SPID == dt.Rows[i][SPInterfaceLimitData.Field_SPID].ToString())
                    {
                        tmpInterfaceName = dt.Rows[i][SPInterfaceLimitData.Field_InterfaceName].ToString().Trim();
                        //����ɣ��������б�����ɹ�
                        if (tmpInterfaceName == InterfaceName)
                        {
                            Result = 0;
                            return Result;
                        }
                    }
                }

                Result = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code;
                ErrMsg = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;

            }
            catch (Exception e)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = e.Message;
            }

            return Result;

        }

        /// <summary>
        /// ҳ�������
        /// ���ߣ�Է��      ʱ�䣺2009-8-17
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public static void ErrorHappenedRedircet(int Result, string ErrMsg, string FunctionName, HttpContext context)
        {
            string HostUrl = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];
            StringBuilder sbUrlValue = new StringBuilder();
            sbUrlValue.Append(HostUrl);
            sbUrlValue.Append(@"/ErrorInfo.aspx?ErrorInfo=");
            sbUrlValue.Append(HttpUtility.UrlEncode(ErrMsg));
            sbUrlValue.Append("&Result=");
            sbUrlValue.Append(Result.ToString());
            sbUrlValue.Append("&FunctionName=");
            sbUrlValue.Append(HttpUtility.UrlEncode(FunctionName));

            context.Response.Redirect(sbUrlValue.ToString(), false);
        }

        /// <summary>
        /// ����ɹ�ҳ����ת
        /// ���ߣ�Է��      ʱ�䣺2009-8-17
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        public static void SuccessRedirect(string ReturnUrl, string Description, HttpContext context)
        {
            string HostUrl = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];

            StringBuilder sbUrlValue = new StringBuilder();
            sbUrlValue.Append(HostUrl);
            sbUrlValue.Append(@"/Success.aspx?Description=");
            sbUrlValue.Append(HttpUtility.UrlEncode(Description));
            sbUrlValue.Append(@"&ReturnUrl=");
            sbUrlValue.Append(HttpUtility.UrlEncode(ReturnUrl));

            context.Response.Redirect(sbUrlValue.ToString());
        }

        /// <summary>
        /// �������URL�Ƿ��SPID,ReturnUrl����
        /// ���ߣ�����      ʱ�䣺2009-8-20
        /// �޸ģ�   
        /// </summary>
        public static bool IsUrlParams(string Url)
        {
            bool temp = false;
            bool params1 = Url.Contains("?");
            bool params2 = Url.Contains("SPID");
            bool params3 = Url.Contains("ReturnUrl");

            if (params1 && params2 && params3)
            {
                temp = true;
            }

            return temp;

        }

        /// <summary>
        /// �ж��ַ����Ƿ�Ϊ��
        /// ���ߣ�������      ʱ�䣺2009-8-24
        /// </summary>
        public static string DealData(string source)
        {
            if (source == null || source == string.Empty)
                source = "";

            return source;
        }

        /// <summary>
        /// �ͻ���Ϣƽ̨�Ľ���������֤���ܵ�ַ
        /// ���ߣ�����      ʱ�䣺2009-9-09
        /// </summary>
        public static string EncryptEmailURl(string CustID, string Email, HttpContext context)
        {
            StringBuilder URL = new StringBuilder();
            URL.Append(ConfigurationManager.AppSettings["EmailAuthenURL"].ToString());
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(context, "SPData");
            string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
            string Digest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + Email + "$" + datetime, key);
            string AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(CustID + "$" +
                Email + "$" + datetime + "$" + Digest)));
            URL.Append("=");
            URL.Append(HttpUtility.UrlEncode(AuthenStrValue));
            return URL.ToString();  
        }

        /// <summary>
        /// ����ҵ��ϵͳ�ṩ��URL���ܲ������ʼ�
        /// author lihongtu 2014-01-08
        /// </summary>
        public static String EncryptEmailWithNoReturnUrl(String SPID, String CustID, String Email, HttpContext context)
        {
            StringBuilder URL = new StringBuilder();
            String mailCallBackUrl = ConfigurationManager.AppSettings["EmailAuthenNoURL"].ToString(); //// ����ָ��authenv2.aspx
            if (String.IsNullOrEmpty(mailCallBackUrl))
            {
                mailCallBackUrl = "http://Customer.besttone.com.cn/UserPortal/authenV3.aspx?AuthenStr";
            }
            URL.Append(mailCallBackUrl);
            String timeTamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";  //spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            String Digest = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + Email  + "$" + timeTamp, key);
            String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(SPID + "$" + CustID + "$" + Email  + "$" + timeTamp + "$" + Digest)));
            URL.Append("=");
            URL.Append(HttpUtility.UrlEncode(AuthenStrValue));
            return URL.ToString();

        }


        /// <summary>
        /// ����ҵ��ϵͳ�ṩ��URL���ܲ������ʼ�
        /// author lihongtu 2013-12-20
        /// </summary>
        public static String EncryptEmailURlV2(String SPID, String CustID, String Email, String ReturnUrl, HttpContext context)
        {
            StringBuilder URL = new StringBuilder();
            String mailCallBackUrl = ConfigurationManager.AppSettings["EmailAuthenURLV2"].ToString(); //// ����ָ��authenv2.aspx
            if (String.IsNullOrEmpty(mailCallBackUrl))
            {
                mailCallBackUrl = "http://Customer.besttone.com.cn/UserPortal/authenV2.aspx?AuthenStr";
            }
            URL.Append(mailCallBackUrl);  
            String timeTamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //SPInfoManager spInfo = new SPInfoManager();
            //Object SPData = spInfo.GetSPData(context, "SPData");
            String key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";  //spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            String Digest = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + Email + "$" + ReturnUrl  + "$" + timeTamp, key);
            String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(SPID + "$" + CustID + "$" +
                Email + "$" + ReturnUrl +  "$" + timeTamp + "$" + Digest)));
            URL.Append("=");
            URL.Append(HttpUtility.UrlEncode(AuthenStrValue));
            return URL.ToString();  

        }


        /// <summary>
        /// ����ҵ��ϵͳ�ṩ��URL���ܲ������ʼ�
        /// </summary>
        public static String EncryptEmailURl_Client(String SPID, String CustID, String Email, String AuthenCode, HttpContext context)
        {
            
            String timeTamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(context, "SPData");
            String key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            String Digest = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + Email  + "$" + AuthenCode + "$" + timeTamp, key);
            String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(SPID + "$" + CustID + "$" + Email  + "$" + AuthenCode + "$" + timeTamp + "$" + Digest)));
            return AuthenStrValue;
        }

        /// <summary>
        /// ����ҵ��ϵͳ�ṩ��URL���ܲ������ʼ�
        /// </summary>
        public static String EncryptEmailURl(String SPID, String CustID, String Email, String Url,String AuthenCode, HttpContext context)
        {
            //StringBuilder strUrl = new StringBuilder();
            //strUrl.Append(Url);
            String timeTamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(context, "SPData");
            String key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
            String Digest = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + Email + "$" + Url + "$" + AuthenCode + "$" + timeTamp, key);
            String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(SPID + "$" + CustID + "$" +
                Email + "$" + Url + "$" + AuthenCode + "$" + timeTamp + "$" + Digest)));
            return AuthenStrValue;
            //strUrl.Append("=");
            //strUrl.Append(HttpUtility.UrlEncode(AuthenStrValue));
            //return strUrl.ToString();
        }

        /// <summary>
        /// �ͻ���Ϣƽ̨�Ľ���������֤���ܵ�ַ
        /// ���ߣ�����      ʱ�䣺2009-9-09
        /// </summary>
        public static List<string> DecryptEmailURL(string URL, HttpContext context)
        {
            List<string> list = new List<string>();
            try
            {
                string[] arrTemp = URL.Split('=');
                URL = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(HttpUtility.UrlDecode(arrTemp[1]))));
                string[] arrParam = URL.Split('$');
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(context, "SPData");
                string key = spInfo.GetPropertyBySPID("35000000", "SecretKey", SPData);
                string Digest = CryptographyUtil.GenerateAuthenticator(arrParam[0] + "$" + arrParam[1] + "$" + arrParam[2], key);
                if (Digest.Equals(arrParam[3]))
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
            catch (System.Exception ex)
            {
                list = null;
            }

            return list;
        }


        /// <summary>
        /// �ͻ���Ϣƽ̨�Ľ���������֤���ܵ�ַ
        /// ���ߣ����ͼ      ʱ�䣺2012-12-20
        /// </summary>
        public static List<string> DecryptEmailWithNoReturnUrl(string SPID, string URL, HttpContext context)
        {
            List<string> list = new List<string>();
            //String Digest = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + Email + "$" + ReturnUrl + "$" + timeTamp, key);
            //String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(SPID + "$" + CustID + "$" +
            //    Email + "$" + ReturnUrl + "$" + timeTamp + "$" + Digest)));

            try
            {
                string[] arrTemp = URL.Split('=');
                URL = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(HttpUtility.UrlDecode(arrTemp[1]))));
                string[] arrParam = URL.Split('$');
                //SPInfoManager spInfo = new SPInfoManager();
                //Object SPData = spInfo.GetSPData(context, "SPData");
                string key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";  //spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                string Digest = CryptographyUtil.GenerateAuthenticator(arrParam[0] + "$" + arrParam[1] + "$" + arrParam[2] + "$" + arrParam[3] , key);
                if (Digest.Equals(arrParam[4]))  //arrParam[5] �� Digest
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
            catch (System.Exception ex)
            {
                list = null;
            }

            return list;
        }


        /// <summary>
        /// �ͻ���Ϣƽ̨�Ľ���������֤���ܵ�ַ
        /// ���ߣ����ͼ      ʱ�䣺2012-12-20
        /// </summary>
        public static List<string> DecryptEmailURLV2(string SPID,string URL, HttpContext context)
        {
            List<string> list = new List<string>();
            //String Digest = CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + Email + "$" + ReturnUrl + "$" + timeTamp, key);
            //String AuthenStrValue = CryptographyUtil.ToBase64String(Encoding.UTF8.GetBytes(CryptographyUtil.Encrypt(SPID + "$" + CustID + "$" +
            //    Email + "$" + ReturnUrl + "$" + timeTamp + "$" + Digest)));

            try
            {
                string[] arrTemp = URL.Split('=');
                URL = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(HttpUtility.UrlDecode(arrTemp[1]))));
                string[] arrParam = URL.Split('$');
                //SPInfoManager spInfo = new SPInfoManager();
                //Object SPData = spInfo.GetSPData(context, "SPData");
                string key = "FEA53F0FECBF8F2FFA4825DBD0816FAB416A1F8B8FB7C534";  //spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                string Digest = CryptographyUtil.GenerateAuthenticator(arrParam[0] + "$" + arrParam[1] + "$" + arrParam[2] + "$" + arrParam[3] + "$" + arrParam[4], key);
                if (Digest.Equals(arrParam[5]))  //arrParam[5] �� Digest
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
            catch (System.Exception ex)
            {
                list = null;
            }

            return list;
        }


        /// <summary>
        /// ���ݲ�ͬ��ҵ��ϵͳ�ṩ��url��url�������н���
        /// ����list<string>һ����:CustID��Email��Time��Digest
        /// </summary>
        public static List<String> DecryptEmailURL(String SPID, String CustID, String Email, String Url, HttpContext context)
        {
            List<String> list = new List<String>();
            try
            {
                String urlParameter = Url.Split('=')[1];
                String decryptParameter = CryptographyUtil.Decrypt(Encoding.UTF8.GetString(CryptographyUtil.FromBase64String(HttpUtility.UrlDecode(urlParameter))));
                String[] parArray = decryptParameter.Split('$');
                //��ȡ��ӦSPID��key
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(context, "SPData");
                String key = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                string Digest = CryptographyUtil.GenerateAuthenticator(parArray[0] + "$" + parArray[1] + "$" + parArray[2], key);
                if (Digest.Equals(parArray[3]))
                {
                    for (int i = 0; i < parArray.Length - 1; i++)
                    {
                        list.Add(parArray[i]);
                    }
                }
                else
                {
                    list = null;
                }

            }
            catch (Exception ex)
            {
                list = null;
            }

            return list;
        }

        /// <summary>
        /// ����ʡ�����ȡʡ�Ĺ�����
        /// </summary>
        public static string GetReginCodeByProvinceID(string ProvinceID, HttpContext context)
        {
            string Result = "";
            ProvinceInfoManager proInfo = new ProvinceInfoManager();
            Object ProvinceData = proInfo.GetProvinceData(context);
            Result = proInfo.GetPropertyByProvinceID(ProvinceID, "RegionProvince", ProvinceData);
            return Result;
        }

        /// <summary>
        /// ����SPID ��ȡSP�ⲿϵͳID
        /// </summary>
        public static string GetSPOuterIDBySPID(string SPID, HttpContext context)
        {
            string Result = "";
            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(context, "SPData");
            Result = spInfo.GetPropertyBySPID(SPID, "SPOuterID", SPData);
            return Result;
        }

        /// <summary>
        /// ����ticket������ʱ���(17λ)+�����(5λ)
        /// </summary>
        public static String CreateTicket()
        {
            //17λʱ���
            String date = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //����5λ�����
            //���������guid��hashCodeֵ�����ӣ�Ĭ��Random����ʱ���Ϊ���ӣ������ںܶ�ʱ���ڴ����ظ�
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return date + r.Next(10000, 99999).ToString();
        }

        /// <summary>
        /// ������ˮ��:ʱ��(yyyyMMdd)+�����(10λ)
        /// </summary>
        public static String CreateTransactionID()
        {
            //8λʱ��
            String date = DateTime.Now.ToString("yyyyMMdd");

            //10λ�����
            Random r = new Random(Guid.NewGuid().GetHashCode());
            String TransactionID = date + r.Next(10000000, 99999999).ToString();
            r = new Random(Guid.NewGuid().GetHashCode());
            TransactionID += r.Next(10, 99);

            return TransactionID;
        }

        #region д���ݿ���־

        /// <summary>
        /// ��¼ע����Դip��ַ
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="UserName"></param>
        /// <param name="SPID"></param>
        /// <param name="IPAddress"></param>
        public static void WriteTraceIpLog(String CustID,String UserName,String SPID,String IPAddress,String OperType)
        {
            //��¼ע����Դip��ַ
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBUtility.BestToneCenterConStr);
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("insert into {0} (custid,username,spid,registertime,ip,msg,opertype) ", "Register_" + string.Format("{0:MM}", DateTime.Now));
            sql.AppendFormat(" values ({0},'{1}','{2}',getdate(),'{3}','{4}','{5}') ", CustID, UserName, SPID, IPAddress, "", OperType);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// ��¼�û���¼��֤��־
        /// ���ߣ�����      ʱ�䣺2009-8-28
        /// </summary>
        public static void WriteDataCustAuthenLog(string SPID, string CustID, string ProvinceID, string AuthenType, string AuthenName,
            string LoginType, int Results, string Description)
        {
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = myCon;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_V3_Interface_WriteDataCustAuthenLog";

            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
            parCustID.Value = CustID;
            cmd.Parameters.Add(parCustID);

            SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
            parProvinceID.Value = ProvinceID;
            cmd.Parameters.Add(parProvinceID);

            SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
            parAuthenType.Value = AuthenType;
            cmd.Parameters.Add(parAuthenType);

            SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 48);
            parAuthenName.Value = AuthenName;
            cmd.Parameters.Add(parAuthenName);

            SqlParameter parLoginType = new SqlParameter("@LoginType", SqlDbType.VarChar, 2);
            parLoginType.Value = LoginType;
            cmd.Parameters.Add(parLoginType);

            SqlParameter parResults = new SqlParameter("@Results", SqlDbType.Int, 4);
            parResults.Value = Results;
            cmd.Parameters.Add(parResults);

            SqlParameter parDescription = new SqlParameter("@Description", SqlDbType.VarChar, 256);
            parDescription.Value = Description;
            cmd.Parameters.Add(parDescription);

            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);


        }

        /// <summary>
        /// ��¼�ӿڵ������ݿ���־
        /// </summary>
        public static void WriteCallInterfaceLog_DB(String IP,String SPID, String InterfaceName, String InParameters, String OutParameters, Int32 CallResult, String ErrMsg)
        {
            try
            {
                //�ж��Ƿ��������ݿ��¼��־����
                Boolean enabled = ConstHelper.DefaultInstance.DBLogEnabled;
                if (!enabled)
                    return;

                //��ȡ��д���ݿ���־�Ľӿ�
                String outInerfaceList = ConstHelper.DefaultInstance.UnWriteDBLogInterface;
                if (("," + outInerfaceList + ",").IndexOf("," + InterfaceName + ",") >= 0)
                    return;

                CallInterfaceLog entity = new CallInterfaceLog();
                entity.IP = IP;
                entity.SPID = SPID;
                entity.InterfaceName = InterfaceName;
                entity.InParameters = InParameters;
                entity.OutParameters = OutParameters;
                entity.CallResult = CallResult.ToString();
                entity.ErrMsg = ErrMsg;
                entity.CreateTime = DateTime.Now;
                CallInterfaceLogBO.InsertLog(entity);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// ��ò��û�д洢���̡�д���ݿ���־
        /// </summary>
        public static void WriteDataLog(string SPID, string CustID, string UserAccount, int Result, string ErrorDescription,
           string PhoneNum, string InterfaceName)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.up_BT_V2_Interface_WriteDataLog";

            SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
            parSPID.Value = (SPID == null) ? "" : SPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
            parCustID.Value = (CustID == null) ? "" : CustID;
            cmd.Parameters.Add(parCustID);


            SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
            parUserAccount.Value = (UserAccount == null) ? "" : UserAccount;
            cmd.Parameters.Add(parUserAccount);

            SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
            parResult.Value = Result;
            cmd.Parameters.Add(parResult);

            SqlParameter parErrorDescription = new SqlParameter("@ErrorDescription", SqlDbType.VarChar, 256);
            parErrorDescription.Value = (ErrorDescription == null) ? "" : ErrorDescription;
            cmd.Parameters.Add(parErrorDescription);

            SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
            parPhoneNum.Value = (PhoneNum == null) ? "" : PhoneNum;
            cmd.Parameters.Add(parPhoneNum);

            SqlParameter parInterfaceName = new SqlParameter("@InterfaceName", SqlDbType.VarChar, 48);
            parInterfaceName.Value = (InterfaceName == null) ? "" : InterfaceName;
            cmd.Parameters.Add(parInterfaceName);


            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

        }

        /// <summary>
        /// ����ʱ���ã�������WriteDataCustAuthenLog����¼�û���¼��֤��־
        /// </summary>
        public static void InsertCustAuthenLog(string CustID, string AuthenType, string AuthenName, string LoginType, int Results, string SPID, DateTime DealTime, string Description)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_Customer_OV3_Interface_InsertCustAuthenLog";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
                parAuthenType.Value = AuthenType;
                cmd.Parameters.Add(parAuthenType);


                SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 30);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parAuthenName);

                SqlParameter parLoginType = new SqlParameter("@LoginType", SqlDbType.VarChar, 1);
                parLoginType.Value = LoginType;
                cmd.Parameters.Add(parLoginType);

                SqlParameter parResults = new SqlParameter("@Results", SqlDbType.Int);
                parAuthenName.Value = AuthenName;
                cmd.Parameters.Add(parResults);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parDealTime = new SqlParameter("@DealTime", SqlDbType.DateTime);
                parDealTime.Value = DealTime;
                cmd.Parameters.Add(parDealTime);

                SqlParameter parDescription = new SqlParameter("@Description", SqlDbType.VarChar, 40);
                parDescription.Value = Description;
                cmd.Parameters.Add(parDescription);


                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);
            }
            catch { }
        }

        /// <summary>
        /// ����ʱ���á�
        /// </summary>
        public static void WriteDataSSOCRMLog( string SSOSPID,string SSOCustID,string SSOOuterID,string CRMCustID,string CRMOuterID, string ProvinceID, string AuthenType, string AuthenName,
                int Results, string Description)
        {
            SqlConnection myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = myCon;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "up_Customer_OV3_Interface_InsertSSOCRMLog";

            SqlParameter parSPID = new SqlParameter("@SSOSPID", SqlDbType.VarChar, 8);
            parSPID.Value = SSOSPID;
            cmd.Parameters.Add(parSPID);

            SqlParameter paSSOrCustID = new SqlParameter("@SSOCustID", SqlDbType.VarChar, 16);
            paSSOrCustID.Value = SSOCustID;
            cmd.Parameters.Add(paSSOrCustID);

            SqlParameter parSSOOuterID = new SqlParameter("@SSOOuterID", SqlDbType.VarChar, 16);
            parSSOOuterID.Value = SSOOuterID;
            cmd.Parameters.Add(parSSOOuterID);

            SqlParameter parCRMCustID = new SqlParameter("@CRMCustID", SqlDbType.VarChar, 16);
            parCRMCustID.Value = CRMCustID;
            cmd.Parameters.Add(parCRMCustID);

            SqlParameter parCRMOuterID = new SqlParameter("@CRMOuterID", SqlDbType.VarChar, 16);
            parCRMOuterID.Value = CRMOuterID;
            cmd.Parameters.Add(parCRMOuterID);

            SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
            parProvinceID.Value = ProvinceID;
            cmd.Parameters.Add(parProvinceID);

              SqlParameter parResults = new SqlParameter("@Results", SqlDbType.Int, 4);
            parResults.Value = Results;
            cmd.Parameters.Add(parResults);          

            SqlParameter parAuthenType = new SqlParameter("@AuthenType", SqlDbType.VarChar, 2);
            parAuthenType.Value = AuthenType;
            cmd.Parameters.Add(parAuthenType);

            SqlParameter parAuthenName = new SqlParameter("@AuthenName", SqlDbType.VarChar, 30);
            parAuthenName.Value = AuthenName;
            cmd.Parameters.Add(parAuthenName);         

            SqlParameter parDescription = new SqlParameter("@Description", SqlDbType.VarChar, 256);
            parDescription.Value = Description;
            cmd.Parameters.Add(parDescription);

            DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);


        }

        #endregion

    }
}

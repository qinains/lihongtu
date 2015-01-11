/*********************************************************************************************************
 *     ����: UserToken����
 * ����ƽ̨: Windows XP + Microsoft SQL Server 2005
 * ��������: C#
 * ��������: Microsoft Visual Studio.Net 2005
 *     ����: Է��
 * ��ϵ��ʽ: 
 *     ��˾: �����Ƽ�(�Ͼ�)�ɷ����޹�˾
 * ��������: 2009-08-12
 **********/


using System;
using System.Collections.Generic;
using System.Text;

using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

namespace Linkage.BestTone.Interface.Rule
{
    
    public class UserToken
    {


        /// <summary>
        /// ���ߣ����ͼ      ʱ�䣺2013-01-23
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="CustID"></param>
        /// <param name="RealName"></param>
        /// <param name="UserName"></param>
        /// <param name="NickName"></param>
        /// <param name="OuterID"></param>
        /// <param name="CustType"></param>
        /// <param name="LoginAuthenName"></param>
        /// <param name="LoginAuthenType"></param>
        /// <param name="key"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public string GenerateJFUserToken(string ProvinceID,string CustID, string RealName, string UserName, string NickName, string OuterID, string CustType, string LoginAuthenName, string LoginAuthenType, string key, out string ErrMsg)
        {
            string UserTokenVaule = "";
            ErrMsg = "";

            try
            {
                string TokenStr = System.Configuration.ConfigurationManager.AppSettings["TokenStr"];
                string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //Digest = Base64(3Des(SHA1��CustID + RealName+ UserName+NickName+ Timestamp+OuterID+CustType+TokenStr��))
                StringBuilder sbDigest = new StringBuilder();
                sbDigest.Append(ProvinceID);
                sbDigest.Append("$");
                sbDigest.Append(CustID);
                sbDigest.Append("$");
                sbDigest.Append(RealName);
                sbDigest.Append("$");
                sbDigest.Append(UserName);
                sbDigest.Append("$");
                sbDigest.Append(NickName);
                sbDigest.Append("$");
                sbDigest.Append(TimeStamp);
                sbDigest.Append("$");
                sbDigest.Append(OuterID);
                sbDigest.Append("$");
                sbDigest.Append(CustType);
                sbDigest.Append("$");
                sbDigest.Append(LoginAuthenName);
                sbDigest.Append("$");
                sbDigest.Append(LoginAuthenType);
                sbDigest.Append("$");
                sbDigest.Append(TokenStr);

                string Digest = CryptographyUtil.GenerateAuthenticator(sbDigest.ToString(), key);

                //Base64��3DES��CustID +"$"+ RealName+ "$"+ UserName +"$"+NickName +��$��+Timestamp+"$"+OuterID +"$"+CustType+"$"+TokenStr+ "$"+  Digest����
                StringBuilder sbUsertokenValue = new StringBuilder();
                sbUsertokenValue.Append(ProvinceID);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(CustID);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(RealName);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(UserName);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(NickName);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(TimeStamp);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(OuterID);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(CustType);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(LoginAuthenName);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(LoginAuthenType);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(TokenStr);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(Digest);

                UserTokenVaule = CryptographyUtil.Encrypt(sbUsertokenValue.ToString(), key);
            }
            catch (System.Exception ex)
            {
                UserTokenVaule = "";
                ErrMsg = ex.Message;
            }

            return UserTokenVaule;
        }


        /// <summary>
        /// ����UserToken
        /// ���ߣ�Է��      ʱ�䣺2009-8-13
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="RealName"></param>
        /// <param name="NickName"></param>
        /// <param name="OuterID"></param>
        /// <param name="key"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public string GenerateUserToken(string CustID, string RealName, string UserName, string NickName, string OuterID, string CustType, string LoginAuthenName, string LoginAuthenType,string key, out string ErrMsg)
        {
            string UserTokenVaule = "";
            ErrMsg = "";

            try
            {
                string TokenStr = System.Configuration.ConfigurationManager.AppSettings["TokenStr"];
               string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
               //Digest = Base64(3Des(SHA1��CustID + RealName+ UserName+NickName+ Timestamp+OuterID+CustType+TokenStr��))
               StringBuilder sbDigest = new StringBuilder();
               sbDigest.Append(CustID);
               sbDigest.Append(RealName);
               sbDigest.Append(UserName);
               sbDigest.Append(NickName);
               sbDigest.Append(TimeStamp);
               sbDigest.Append(OuterID);
               sbDigest.Append(CustType);
               sbDigest.Append(LoginAuthenName);
               sbDigest.Append(LoginAuthenType);
               sbDigest.Append(TokenStr);

               string Digest = CryptographyUtil.GenerateAuthenticator(sbDigest.ToString(), key);

               //Base64��3DES��CustID +"$"+ RealName+ "$"+ UserName +"$"+NickName +��$��+Timestamp+"$"+OuterID +"$"+CustType+"$"+TokenStr+ "$"+  Digest����
               StringBuilder sbUsertokenValue = new StringBuilder();

               sbUsertokenValue.Append(CustID);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(RealName);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(UserName);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(NickName);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(TimeStamp);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(OuterID);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(CustType);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(LoginAuthenName);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(LoginAuthenType);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(TokenStr);
               sbUsertokenValue.Append("$");
               sbUsertokenValue.Append(Digest);

               UserTokenVaule = CryptographyUtil.Encrypt(sbUsertokenValue.ToString(), key);
            }
            catch (System.Exception ex)
            {
                UserTokenVaule = "";
                ErrMsg = ex.Message;
            }

            return UserTokenVaule;
        }


        /// <summary>
        /// ����UserToken
        /// ���ߣ�lihongtu  ʱ�䣺2012-9-13
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="ReturnUrl"></param>
        /// <param name="HeadFooter"></param>
        /// <param name="TimeStamp"></param>
        /// <param name="key"></param>  
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public string GenerateBestAccountMainUserTokenM(string CustID, string ReturnUrl, string TimeStamp, string key, out string ErrMsg)
        {
            string UserTokenVaule = "";
            ErrMsg = "";

            try
            {
                //Digest = Base64(3Des(SHA1��CustID + RealName+ UserName+NickName+ Timestamp+OuterID+CustType+TokenStr��))
                StringBuilder sbDigest = new StringBuilder();
                sbDigest.Append(CustID).Append("$");
                sbDigest.Append(ReturnUrl).Append("$");
                //sbDigest.Append(HeadFooter).Append("$");
                sbDigest.Append(TimeStamp);


                string Digest = CryptographyUtil.GenerateAuthenticator(sbDigest.ToString(), key);

                //Base64��3DES��CustID + "$" + ReturnUrl + "$" + "$" + TimeStamp+ "$"+  Digest����
                StringBuilder sbUsertokenValue = new StringBuilder();
                sbUsertokenValue.Append(CustID);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(ReturnUrl);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(TimeStamp);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(Digest);
                UserTokenVaule = CryptographyUtil.Encrypt(sbUsertokenValue.ToString(), key);
            }
            catch (System.Exception ex)
            {
                UserTokenVaule = "";
                ErrMsg = ex.Message;
            }
            return UserTokenVaule;
        }



        /// <summary>
        /// ����UserToken
        /// ���ߣ�lihongtu  ʱ�䣺2012-9-13
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="ReturnUrl"></param>
        /// <param name="HeadFooter"></param>
        /// <param name="TimeStamp"></param>
        /// <param name="key"></param>  
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public string GenerateBestAccountMainUserToken(string CustID, string ReturnUrl, string HeadFooter, string TimeStamp ,string key, out string ErrMsg)
        {
            string UserTokenVaule = "";
            ErrMsg = "";
            StringBuilder sbLog = new StringBuilder();
            try
            {
                sbLog.Append("GenerateBestAccountMainUserToken\r\n");
                sbLog.Append("key="+key+"\r\n");
                //Digest = Base64(3Des(SHA1��CustID + RealName+ UserName+NickName+ Timestamp+OuterID+CustType+TokenStr��))
                StringBuilder sbDigest = new StringBuilder();
                sbDigest.Append(CustID).Append("$");
                sbDigest.Append(ReturnUrl).Append("$");
                sbDigest.Append(HeadFooter).Append("$");
                sbDigest.Append(TimeStamp);
                sbLog.Append("sbDigest:"+sbDigest+"\r\n");
                string Digest = CryptographyUtil.GenerateAuthenticator(sbDigest.ToString(), key);
                sbLog.Append("Digest:" + Digest+"\r\n");
                //Base64��3DES��CustID + "$" + ReturnUrl + "$" + HeadFooter + "$" + TimeStamp+ "$"+  Digest����
                StringBuilder sbUsertokenValue = new StringBuilder();

                sbUsertokenValue.Append(CustID);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(ReturnUrl);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(HeadFooter);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(TimeStamp);
                sbUsertokenValue.Append("$");
                sbUsertokenValue.Append(Digest);
                sbLog.Append("sbUsertokenValue=" + sbUsertokenValue+"\r\n");
                UserTokenVaule = CryptographyUtil.Encrypt(sbUsertokenValue.ToString(), key);
                sbLog.Append("CryptographyUtil.Encrypt=" + UserTokenVaule);
            }
            catch (System.Exception ex)
            {
                UserTokenVaule = "";
                ErrMsg = ex.Message;
                ErrMsg += sbLog.ToString();
            }

            return UserTokenVaule;
        }

        /// <summary>
        /// ����UserToken
        /// ���ߣ����ͼ      ʱ�䣺2011-12-09
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        /// <param name="UserTokenVaule"></param>
        /// <param name="key"></param>
        /// <param name="CustID"></param>
        /// <param name="RealName"></param>
        /// <param name="NickName"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int ParsejfUserToken(string UserTokenVaule, string key, out string ProvinceID, out string AuthenType, out string AuthenName, out string Password, out string ReturnURL, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrMsg = "";

            ProvinceID = "";
            AuthenType = "";
            AuthenName = "";
            Password = "";
            ReturnURL = "";


            try
            {
                //���� UserToken
                UserTokenVaule = CryptographyUtil.Decrypt(UserTokenVaule, key);
                if (UserTokenVaule == "")
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "����ʧ��.";
                    return Result;
                }

                //��ȡ����.
                //Base64��3DES��ProvinceID +"$"+ AuthenType+ "$"+ AuthenName +"$"+Password +��$��+ReturnURL+"$"+Timestamp + "$"+  Digest����
                string[] arrUserTokenVaule = UserTokenVaule.Split('$');
                ProvinceID = arrUserTokenVaule[0];
                AuthenType = arrUserTokenVaule[1];
                AuthenName = arrUserTokenVaule[2];
                Password = arrUserTokenVaule[3];
                ReturnURL = arrUserTokenVaule[4];
                string TimeStamp = arrUserTokenVaule[5];
               
                // TokenStr �������ļ��ж�ȡ
                string TokenStr = System.Configuration.ConfigurationManager.AppSettings["TokenStr"];
                string Digest = arrUserTokenVaule[6];

                //Digest = Base64(3Des(SHA1��CustID + RealName+ UserName+NickName+ Timestamp+OuterID+CustType+TokenStr��))
                StringBuilder sbDigest = new StringBuilder();
                sbDigest.Append(ProvinceID);
                sbDigest.Append(AuthenType);
                sbDigest.Append(AuthenName);
                sbDigest.Append(Password);
                sbDigest.Append(ReturnURL);
                sbDigest.Append(TimeStamp);
                

                string tmpDigest = CryptographyUtil.GenerateAuthenticator(sbDigest.ToString(), key);

                if (tmpDigest != Digest)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "����ʧ��";
                    return Result;
                }
                //��ȡCookie����ʱ��(��λ��Сʱ)
                int CookieExpireTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CookieExpireTime"]);
                if (DateTime.Parse(TimeStamp).AddHours(CookieExpireTime) < DateTime.Now)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "Cookie�ѹ���";
                    return Result;
                }
                Result = 0;
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }


        /// <summary>
        ///  ���ߣ����ͼ      ʱ�䣺2013-01-23
        /// </summary>
        /// <param name="UserTokenValue"></param>
        /// <param name="key"></param>
        /// <param name="ProvinceID"></param>
        /// <param name="CustID"></param>
        /// <param name="RealName"></param>
        /// <param name="UserName"></param>
        /// <param name="OuterID"></param>
        /// <param name="CustType"></param>
        /// <param name="LoginAuthenName"></param>
        /// <param name="LoginAuthenType"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int ParseScoreUserToken(string token, string key, out string ProvinceID, out string CustID, out string RealName, out string UserName,out string NickName, out string OuterID, out string CustType, out string LoginAuthenName, out string LoginAuthenType, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrMsg = "";
            ProvinceID = "";
            CustID = "";
            RealName = "";
            UserName = "";
            NickName = "";
            OuterID = "";
            CustType = "";
            LoginAuthenName = "";
            LoginAuthenType = "";
            ErrMsg = "";

            try
            {
                //���� UserToken
                token = CryptographyUtil.Decrypt(token, key);
                if (token == "")
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "����ʧ��.";
                    return Result;
                }

                //��ȡ����.
                //Base64��3DES��ProvinceID+"$"+CustID +"$"+ RealName+ "$"+ UserName +"$"+NickName +��$��+Timestamp+"$"+OuterID +"$"+TokenStr+ "$"+  Digest����
                string[] arrUserTokenVaule = token.Split('$');
                ProvinceID = arrUserTokenVaule[0];
                CustID = arrUserTokenVaule[1];
                RealName = arrUserTokenVaule[2];
                UserName = arrUserTokenVaule[3];
                NickName = arrUserTokenVaule[4];
                string TimeStamp = arrUserTokenVaule[5];
                OuterID = arrUserTokenVaule[6];
                CustType = arrUserTokenVaule[7];
                LoginAuthenName = arrUserTokenVaule[8];
                LoginAuthenType = arrUserTokenVaule[9];

                // TokenStr �������ļ��ж�ȡ
                string TokenStr = System.Configuration.ConfigurationManager.AppSettings["TokenStr"];
                string Digest = arrUserTokenVaule[10];

                //Digest = Base64(3Des(SHA1��CustID + RealName+ UserName+NickName+ Timestamp+OuterID+CustType+TokenStr��))
                StringBuilder sbDigest = new StringBuilder();
                sbDigest.Append(ProvinceID);
                sbDigest.Append("$");
                sbDigest.Append(CustID);
                sbDigest.Append("$");
                sbDigest.Append(RealName);
                sbDigest.Append("$");
                sbDigest.Append(UserName);
                sbDigest.Append("$");
                sbDigest.Append(NickName);
                sbDigest.Append("$");
                sbDigest.Append(TimeStamp);
                sbDigest.Append("$");
                sbDigest.Append(OuterID);
                sbDigest.Append("$");
                sbDigest.Append(CustType);
                sbDigest.Append("$");
                sbDigest.Append(LoginAuthenName);
                sbDigest.Append("$");
                sbDigest.Append(LoginAuthenType);
                sbDigest.Append("$");
                sbDigest.Append(TokenStr);

                string tmpDigest = CryptographyUtil.GenerateAuthenticator(sbDigest.ToString(), key);

                if (tmpDigest != Digest)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "����ʧ��";
                    return Result;
                }
                //��ȡCookie����ʱ��(��λ��Сʱ)
                int CookieExpireTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CookieExpireTime"]);
                if (DateTime.Parse(TimeStamp).AddHours(CookieExpireTime) < DateTime.Now)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "Cookie�ѹ���";
                    return Result;
                }
                Result = 0;
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// ����UserToken
        /// ���ߣ�Է��      ʱ�䣺2009-8-13
        /// �޸ģ�          ʱ�䣺
        /// </summary>
        /// <param name="UserTokenVaule"></param>
        /// <param name="key"></param>
        /// <param name="CustID"></param>
        /// <param name="RealName"></param>
        /// <param name="NickName"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int ParseUserToken(string UserTokenVaule, string key, out string CustID, out string RealName, out string UserName, out string NickName, out string OuterID, out string CustType,out string LoginAuthenName, out string LoginAuthenType, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrMsg = "";

            CustID = "";  
            RealName = "";
            UserName = "";
            NickName = "";
            OuterID = "";
            CustType = "";
            LoginAuthenName="";
            LoginAuthenType = "";
            ErrMsg = "";

            try
            {
                //���� UserToken
                UserTokenVaule = CryptographyUtil.Decrypt(UserTokenVaule, key);
                if(UserTokenVaule=="")
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "����ʧ��.";
                    return Result;
                }

                //��ȡ����.
                //Base64��3DES��CustID +"$"+ RealName+ "$"+ UserName +"$"+NickName +��$��+Timestamp+"$"+OuterID +"$"+TokenStr+ "$"+  Digest����
                string[] arrUserTokenVaule = UserTokenVaule.Split('$');
                CustID = arrUserTokenVaule[0];
                RealName = arrUserTokenVaule[1];
                UserName = arrUserTokenVaule[2];
                NickName = arrUserTokenVaule[3];
                string TimeStamp = arrUserTokenVaule[4];
                OuterID = arrUserTokenVaule[5];
                CustType = arrUserTokenVaule[6];
                LoginAuthenName=arrUserTokenVaule[7];
                LoginAuthenType = arrUserTokenVaule[8];

                // TokenStr �������ļ��ж�ȡ
                string TokenStr = System.Configuration.ConfigurationManager.AppSettings["TokenStr"];
                string Digest = arrUserTokenVaule[10];

                //Digest = Base64(3Des(SHA1��CustID + RealName+ UserName+NickName+ Timestamp+OuterID+CustType+TokenStr��))
                StringBuilder sbDigest = new StringBuilder();
                sbDigest.Append(CustID);
                sbDigest.Append(RealName);
                sbDigest.Append(UserName);
                sbDigest.Append(NickName);
                sbDigest.Append(TimeStamp);
                sbDigest.Append(OuterID);
                sbDigest.Append(CustType);
                sbDigest.Append(LoginAuthenName);
                sbDigest.Append(LoginAuthenType);
                sbDigest.Append(TokenStr);

                string tmpDigest = CryptographyUtil.GenerateAuthenticator(sbDigest.ToString(), key);

                if (tmpDigest != Digest)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "����ʧ��";
                    return Result;
                }
                //��ȡCookie����ʱ��(��λ��Сʱ)
                int CookieExpireTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CookieExpireTime"]);
                if (DateTime.Parse(TimeStamp).AddHours(CookieExpireTime)<DateTime.Now)
                {
                    Result = ErrorDefinition.BT_IError_Result_BusinessError_Code;
                    ErrMsg = "Cookie�ѹ���";
                    return Result;
                }
                Result = 0;
            }
            catch (System.Exception ex)
            {
                Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }
 

    }
}

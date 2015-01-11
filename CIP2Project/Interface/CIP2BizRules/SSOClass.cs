using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web;
using System.Configuration;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

namespace Linkage.BestTone.Interface.Rule
{
    public class SSOClass
    {
        /// <summary>
        ///  �����������
        /// </summary>
        public static int ParseLoginRequest(string SourceStr, HttpContext context,out string SPID, out string UAProvinceID,
            out string SourceType, out string ReturnURL, out string ErrMsg)
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

                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(context, "SPData");
                string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                string EncryptSourceStr = alSourceStr[1].ToString();

                string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
                string[] alRequest = RequestStr.Split('$');
                //���ܷ�ʽ��Base64(Encrypt(UAProvinceID + ��$�� + SourceType+ ��$��ReturnURL + ��$��+ TimeStamp + ��$��+ Digest))
                //Digest = Base64(Hash(UAProvinceID  + ��$��+ SourceType + ��$�� + ReturnURL + ��$��+ TimeStamp))
                UAProvinceID = alRequest[0].ToString();
                SourceType = alRequest[1].ToString();
                ReturnURL = alRequest[2].ToString();
                TimeStamp = alRequest[3].ToString();
                Digest = alRequest[4].ToString();
                //У��ժҪ Digest ��Ϣ
                string NewDigest = UAProvinceID + "$" + SourceType + "$" + ReturnURL + "$" + TimeStamp;
                NewDigest = CryptographyUtil.GenerateAuthenticator(NewDigest, ScoreSystemSecret);
                if (Digest != NewDigest)
                {
                    Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                    ErrMsg = "��Ч��Digest";
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

        /// <summary>
        /// ���������̳ǵ�¼(login2.aspx)���������
        /// ����ǰlogin.aspx����AuthenName��Password
        /// </summary>
        public static int ParseJFLoginRequest(string SourceStr, HttpContext context, out string SPID, out string UAProvinceID,
            out string AuthenType, out string AuthenName, out string Password, out string ReturnURL, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            SPID = "";
            UAProvinceID = "";
            AuthenType = "";
            AuthenName = "";
            Password = "";
            ReturnURL = "";
            string TimeStamp = "";
            string Digest = "";

            try
            {
                string[] alSourceStr = SourceStr.Split('$');
                SPID = alSourceStr[0].ToString();

                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(context, "SPData");
                string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                // string ScoreSystemSecret = System.Configuration.ConfigurationManager.AppSettings["ScoreSystemSecret"]
                string EncryptSourceStr = alSourceStr[1].ToString();

                string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
                string[] alRequest = RequestStr.Split('$');
                //���ܷ�ʽ��Base64(Encrypt(UAProvinceID + ��$�� + SourceType+ ��$��ReturnURL + ��$��+ TimeStamp + ��$��+ Digest))
                //Digest = Base64(Hash(UAProvinceID  + ��$��+ SourceType + ��$�� + ReturnURL + ��$��+ TimeStamp))
                
                UAProvinceID = alRequest[0].ToString();
                AuthenType = alRequest[1].ToString();
                AuthenName = alRequest[2].ToString();
                Password = alRequest[3].ToString();
                ReturnURL = alRequest[4].ToString();
                TimeStamp = alRequest[5].ToString();
                Digest = alRequest[6].ToString();
                
                //У��ժҪ Digest ��Ϣ
                string NewDigest = UAProvinceID + "$" + AuthenType +  "$"+ AuthenName + "$" + Password + "$" + ReturnURL + "$" + TimeStamp;
                NewDigest = CryptographyUtil.GenerateAuthenticator(NewDigest, ScoreSystemSecret);
                if (Digest != NewDigest)
                {
                    Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                    ErrMsg = "��Ч��Digest";
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


        /// <summary>
        /// ������ͨ�������ͨ�˺�SPTokenRequest
        /// </summary>

        public static int ParseBesttoneAccountPageRequestM(string SourceStr, HttpContext context, out string SPID, out string CustID,
             out string ReturnURL, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            SPID = "";
            CustID = "";
            ReturnURL = "";
            string TimeStamp = "";
            string Digest = "";
            try
            {
                string[] alSourceStr = SourceStr.Split('$');
                SPID = alSourceStr[0].ToString();
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(context, "SPData");
                string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                string EncryptSourceStr = alSourceStr[1].ToString();
                string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
                string[] alRequest = RequestStr.Split('$');

                //����˳��URLEncoding(SPID + "$" + Base64(Encrypt(CustId + "$"  + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + Digest)))
                //Digest = Base64(Encrypt(Hash(CustId + "$"+ReturnURL +"$"+ HeadFooter "$"+TimeStamp)))
                CustID = alRequest[0].ToString();
                ReturnURL = alRequest[1].ToString();
                TimeStamp = alRequest[2].ToString();
                Digest = alRequest[3].ToString();
                //У��ժҪ Digest ��Ϣ
                string NewDigest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + ReturnURL  + "$" + TimeStamp, ScoreSystemSecret);
                if (Digest != NewDigest)
                {
                    Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                    ErrMsg = "��Ч��Digest";
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

        /// <summary>
        /// ������ͨ�������ͨ�˺�SPTokenRequest
        /// </summary>

        public static int ParseBesttoneAccountPageRequest(string SourceStr, HttpContext context, out string SPID, out string CustID,
            out string HeadFooter,out string ReturnURL, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            SPID = "";
            CustID = "";
            HeadFooter = "";
            ReturnURL = "";
            string TimeStamp = "";
            string Digest = "";
            try
            {
                string[] alSourceStr = SourceStr.Split('$');
                SPID = alSourceStr[0].ToString();
                SPInfoManager spInfo = new SPInfoManager();
                Object SPData = spInfo.GetSPData(context, "SPData");
                string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                string EncryptSourceStr = alSourceStr[1].ToString();
                string RequestStr = CryptographyUtil.Decrypt(EncryptSourceStr.ToString(), ScoreSystemSecret);
                string[] alRequest = RequestStr.Split('$');

                //����˳��URLEncoding(SPID + "$" + Base64(Encrypt(CustId + "$"  + ReturnURL + "$" + HeadFooter + "$" + TimeStamp + "$" + Digest)))
                //Digest = Base64(Encrypt(Hash(CustId + "$"+ReturnURL +"$"+ HeadFooter "$"+TimeStamp)))
                CustID = alRequest[0].ToString();
                ReturnURL = alRequest[1].ToString();
                HeadFooter = alRequest[2].ToString();
                TimeStamp = alRequest[3].ToString();
                Digest = alRequest[4].ToString();
                //У��ժҪ Digest ��Ϣ
                string NewDigest = CryptographyUtil.GenerateAuthenticator(CustID + "$" + ReturnURL + "$" + HeadFooter + "$" + TimeStamp, ScoreSystemSecret);
                if (Digest != NewDigest)
                {
                    Result = ErrorDefinition.IError_Result_InValidAuthenticator_Code;
                    ErrMsg = "��Ч��Digest";
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




        /// <summary>
        /// ���ɷ����ַ���
        /// </summary>
        public static string GenerateResStr(string SPID, string CustID, int Result, string ErrMsg,HttpContext context)
        {
            string ResStr = "";

            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(context, "SPData");
            string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);
                        
            // Digest = Base64(Hash(ExpireTime +��$�� ��CustID + ��$��+Result + ��$��+ErrMsg))
            int SSOExpire = int.Parse(ConfigurationSettings.AppSettings["SSOExpire"]);
            string ExpireTime = DateTime.Now.AddSeconds(SSOExpire).ToString("yyyy-MM-dd HH:mm:ss");
            
            StringBuilder sbDigest = new StringBuilder();
            sbDigest.Append(ExpireTime);
            sbDigest.Append("$");
            sbDigest.Append(CustID);
            sbDigest.Append("$");
            sbDigest.Append(Result.ToString());
            sbDigest.Append("$");
            sbDigest.Append(ErrMsg);

            string Digest = sbDigest.ToString();
            Digest = CryptographyUtil.GenerateAuthenticator(Digest, ScoreSystemSecret);
           
            StringBuilder sbRes = new StringBuilder();
            sbRes.Append(Result.ToString());
            sbDigest.Append("$");
            sbRes.Append(CustID);
            sbDigest.Append("$");
            sbRes.Append(ExpireTime);
            sbDigest.Append("$");
            sbRes.Append(ErrMsg);
            sbDigest.Append("$");
            sbRes.Append(Digest);
            ResStr = sbRes.ToString();
            ResStr = CryptographyUtil.Encrypt(ResStr, ScoreSystemSecret);
            //SPTokenResponseValue = URLEncoding(SPID + ��$��+ Base64(Encrypt (Result + ��$�� + CustID + ��$�� + ExpireTime + ��$��+ErrMsg + ��$��+ Digest)))
            ResStr = HttpUtility.UrlEncode(SPID + "$" + ResStr);

            return ResStr;
        }

    }
}

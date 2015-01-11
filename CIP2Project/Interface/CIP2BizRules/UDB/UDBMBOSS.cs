using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using System.Net;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using BTUCenter.Proxy;
using Newtonsoft.Json;
namespace Linkage.BestTone.Interface.Rule
{
    public class UDBMBOSS
    {
        /// <summary>
        /// 根据UDBTicket查询用户信息
        /// </summary>
        public Int32 AccountInfoQuery(String SrcSsDeviceNo, String AuthSsDeviceNo, String UDBTicket, String key, out UDBAccountInfo accountInfo, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            accountInfo = new UDBAccountInfo();
            StringBuilder strMsg = new StringBuilder();
            strMsg.AppendFormat("【AccountInfoQuery,DateTime:{3}】SrcSsDeviceNo:{0},AuthSsDeviceNo:{1},UDBTicket:{2}", SrcSsDeviceNo, AuthSsDeviceNo, UDBTicket, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                String timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String digest = CryptographyUtil.GenerateAuthenticator(SrcSsDeviceNo + AuthSsDeviceNo + UDBTicket + timeStamp, key);
                strMsg.AppendFormat(",timeStamp:{0},digest:{1}", timeStamp, digest);

                UDBAppSys serviceProxy = new UDBAppSys();
                serviceProxy.Url = UDBConstDefinition.DefaultInstance.BJSOAPUrl;
                AccountInfoCheckResult accountInfoResult = serviceProxy.AccountInfoCheck(digest, SrcSsDeviceNo, AuthSsDeviceNo, UDBTicket, timeStamp);

                if (accountInfoResult != null)
                {
                    Result = accountInfoResult.ResultCode;

                    accountInfo.UserID = accountInfoResult.UserID;
                    accountInfo.UserIDType = accountInfoResult.UserIDType;
                    accountInfo.PUserID = accountInfoResult.PUserID;
                    accountInfo.Alias = accountInfoResult.Alias;
                    accountInfo.BindingAccessNo = accountInfoResult.BindingAccessNo;
                    accountInfo.ThirdSsUserID = accountInfoResult.ThirdSsUserID;
                    accountInfo.UserIDStatus = accountInfoResult.UserIDStatus;
                    accountInfo.UserIDSsStatus = accountInfoResult.UserIDSsStatus;
                    accountInfo.UserPayType = accountInfoResult.UserPayType;
                    accountInfo.PrePaySystemNo = accountInfoResult.PrePaySystemNo;
                    accountInfo.UserType = accountInfoResult.UserType.ToString();
                    String temp_Description = accountInfoResult.Description;
                    ErrMsg = temp_Description;

                    strMsg.AppendFormat("【查询结果】Result:{0},UserID:{1},UserIDType:{2},PUserID:{3},Alias:{4},BindingAccessNo:{5},ThirdSsUserID:{6},UserIDStatus:{7},UserIDSsStatus:{8},UserPayType:{9},PrePaySystemNo:{10},UserType:{11},temp_Description:{12}",
                        Result, accountInfo.UserID, accountInfo.UserIDType, accountInfo.PUserID, accountInfo.Alias, accountInfo.BindingAccessNo, accountInfo.ThirdSsUserID, accountInfo.UserIDStatus, accountInfo.UserIDSsStatus, accountInfo.UserPayType, accountInfo.PrePaySystemNo, accountInfo.UserType, temp_Description);

                    if (Result == 0 || Result == 5)
                    {
                        //<Ex><PID>07</PID><NF>2</NF></Ex>
                        Int32 startIndex = temp_Description.IndexOf("<PID>");
                        Int32 endIndex = temp_Description.IndexOf("</PID>");
                        accountInfo.ProvinceID = temp_Description.Substring(startIndex + 5, endIndex - startIndex - 5);
                        startIndex = temp_Description.IndexOf("<NF>");
                        endIndex = temp_Description.IndexOf("</NF>");
                        accountInfo.NumFlag = temp_Description.Substring(startIndex + 4, endIndex - startIndex - 4);
                    }
                }
                else
                {
                    strMsg.Append(",查询用户信息失败");
                }

                #region 隐藏

                ////定义调用webservice参数
                //Hashtable hs = new Hashtable();
                //hs.Add("Authenticator", digest);
                //hs.Add("SrcSsDeviceNo", SrcSsDeviceNo);
                //hs.Add("AuthSsDeviceNo", AuthSsDeviceNo);
                //hs.Add("UDBTicket", UDBTicket);
                //hs.Add("TimeStamp", timeStamp);
                //strMsg.AppendFormat(",url:{0}", UDBConstDefinition.DefaultInstance.BJSOAPUrl);

                //String method = UDBConstDefinition.DefaultInstance.UserWebServiceMethod;
                //if (method.ToUpper() == "GET")
                //{
                //    returnXml = WebServiceCommon.QueryGetWebService(UDBConstDefinition.DefaultInstance.BJSOAPUrl, "AccountInfoCheck", hs).OuterXml;
                //}
                //else if (method.ToUpper() == "POST")
                //{
                //    returnXml = WebServiceCommon.QueryPostWebService(UDBConstDefinition.DefaultInstance.BJSOAPUrl, "AccountInfoCheck", hs).OuterXml;
                //}
                //else
                //{
                //    returnXml = WebServiceCommon.QuerySoapWebService(UDBConstDefinition.DefaultInstance.BJSOAPUrl, "AccountInfoCheck", hs).OuterXml;
                //}

                //strMsg.AppendFormat(",returnXml:{0}", returnXml);
                ////解析返回的用户信息xml
                //Result = UDBBusiness.ParseAccountInfoQueryXml(returnXml, out accountInfo, out ErrMsg);

                #endregion

            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
                strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
            }
            finally
            {
                WriteLog(strMsg.ToString());
            }

            return Result;
        }


        /// <summary>
        /// 5.1.25	修改用户名接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="accessToken"></param>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        public void UnifyPlatformUpdateUserName(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, String accessToken,out int result, out String msg)
        {
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            string sign = String.Empty;
            result = -1;
            msg = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "userName=" + userName + "&accessToken=" + accessToken + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformUpdateUserNameUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String strResult = String.Empty;
                result_dic.TryGetValue("result",out strResult);
                result_dic.TryGetValue("msg", out msg);
            }
            catch (Exception e)
            {
                strMsg.AppendFormat("异常:{0}\r\n",e.ToString());
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyPlatformUpdateUserName");
            }

        }


        /// <summary>
        /// 5.1.24	用户名是否存在接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="result"></param>
        /// <param name="isExist"></param>
        /// <param name="msg"></param>
        public void UnifyPlatformIsUserNameExist(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName,  out int result, out int isExist, out String msg)
        {
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            string sign = String.Empty;
            isExist = 2;    // 2 已经存在  1 不存在  
            result = -1;
            msg = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "userName=" + userName + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformIsUserNameExistUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String strIsExists = String.Empty;
                String strResult = String.Empty;

                result_dic.TryGetValue("isExist", out strIsExists);
                result_dic.TryGetValue("result", out strResult);
                result_dic.TryGetValue("msg", out msg);

                if (!String.IsNullOrEmpty(strIsExists))
                {
                    isExist = Convert.ToInt32(strIsExists);
                }

                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }


            }
            catch (Exception e)
            {
                strMsg.AppendFormat("异常:{0}\r\n",e.ToString());
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyPlatformIsUserNameExist");
            }


        }

        /// <summary>
        /// 5.1.3	登录认证接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="account"></param>
        /// <param name="accessToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        public void UnifyPlatformUserAuth(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, String password, out UnifyAccountInfo account, out String accessToken, out string loginNum, out long expiresIn, out int result, out String msg)
        {
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            string sign = String.Empty;
            expiresIn = 0;
            result = -1;
            loginNum = String.Empty;
            accessToken = String.Empty;
            msg = String.Empty;
            account = new UnifyAccountInfo();
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "userName=" + userName + "&password=" + password + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformLoginUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String strResult = String.Empty;
                String strExpiresIn = String.Empty;

                result_dic.TryGetValue("result", out strResult);
                result_dic.TryGetValue("expiresIn", out strExpiresIn);
                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
                if (!String.IsNullOrEmpty(strExpiresIn))
                {
                    expiresIn = Convert.ToInt64(strExpiresIn);
                }
                String userId = String.Empty;

                result_dic.TryGetValue("msg", out msg);
                result_dic.TryGetValue("userId", out userId);
                result_dic.TryGetValue("accessToken", out accessToken);
                result_dic.TryGetValue("loginNum", out loginNum);
                String o_UserName = String.Empty;
                String zhUserName = String.Empty;
                String pUserId = String.Empty;
                String productUid = String.Empty;
                String userType = String.Empty;
                String status = String.Empty;
                String aliasName = String.Empty;
                String provinceId = String.Empty;
                String cityId = String.Empty;

                result_dic.TryGetValue("userName", out o_UserName);
                result_dic.TryGetValue("zhUserName", out zhUserName);
                result_dic.TryGetValue("pUserId", out pUserId);
                result_dic.TryGetValue("productUid", out productUid);
                result_dic.TryGetValue("userType", out userType);
                result_dic.TryGetValue("status", out status);
                result_dic.TryGetValue("aliasName", out aliasName);
                result_dic.TryGetValue("provinceId", out provinceId);
                result_dic.TryGetValue("cityId", out cityId);

               
                
                account.userName = o_UserName;
                account.zhUserName = zhUserName;
                account.pUserId = pUserId;
                account.productUid = productUid;
                try {
                    account.userId = Convert.ToInt64(userId);
                    account.userType = Convert.ToInt16(userType);
                    account.status = Convert.ToInt16(status);
                }catch(Exception et)
                {
                    strMsg.Append(et.ToString());
                }
                account.aliasName = aliasName;
                account.province = provinceId;
                account.city = cityId;
            }
            catch (Exception e)
            {
                strMsg.AppendFormat("异常:{0}\r\n", e.ToString());
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyPlatformUserAuth");
            }
            
        }


        /// <summary>
        /// 5.1.21	5.1.21	userName反查userId接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        public void GetUserInfoByName(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, out int result, out String userId, out String msg)
        {
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            result = -1;
            userId = String.Empty;
            msg = String.Empty;
            string sign = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "userName=" + userName + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);
                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformGetUserInfoByNameUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String strResult = String.Empty;
                result_dic.TryGetValue("result", out strResult);
                result_dic.TryGetValue("userId", out userId);
                result_dic.TryGetValue("msg", out msg);

                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
                else
                {
                    result = -1;
                }
            }
            catch (Exception e)
            {
                strMsg.AppendFormat("异常:{0}\r\n", e.ToString());
            }
            finally
            {
                WriteLog(strMsg.ToString(), "GetUserInfoByName");
            }

        }


        /// <summary>
        /// 5.1.24	用户名是否存在接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        public void IsUserNameExists(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, out int result, out String isExist,out String msg)
        {
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            result = -1;
            isExist = String.Empty;
            msg = String.Empty;
            string sign = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "userName=" + userName +  "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);
                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformIsUserNameExistUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String strResult = String.Empty;
                result_dic.TryGetValue("result", out strResult);
                result_dic.TryGetValue("isExist", out isExist);
                result_dic.TryGetValue("msg", out msg);

                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
                else
                {
                    result = -1;
                }
            }
            catch (Exception e)
            {
                strMsg.AppendFormat("异常:{0}\r\n", e.ToString());
            }
            finally
            {
                WriteLog(strMsg.ToString(), "IsUserNameExists");
            }

        }




        /// <summary>
        ///    5.1.18	用户隐式注册接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="sendSms"></param>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        public void UnifyPlatformRegisterAccount(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, String password, String sendSms, out String userId, out String o_UserName, out String accessToken, out long expiresIn,out int result, out String msg)
        {
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            string sign = String.Empty;
            expiresIn = 0;
            result = -1;
            accessToken = String.Empty;
            msg = String.Empty;
            userId = String.Empty;
            o_UserName = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                //string parameters = "userName=" + userName +  "&password="+password+  "&sendSms=" + sendSms + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                string parameters = "userName=" + userName + "&password=&sendSms=" + sendSms + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformRegisterUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String strResult = String.Empty;
                String strExpiresIn = String.Empty;

                result_dic.TryGetValue("result", out strResult);
                result_dic.TryGetValue("expiresIn", out strExpiresIn);
                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
                if (!String.IsNullOrEmpty(strExpiresIn))
                {
                    expiresIn = Convert.ToInt64(strExpiresIn);
                }
                result_dic.TryGetValue("msg", out msg);
                result_dic.TryGetValue("userId", out userId);
                result_dic.TryGetValue("accessToken", out accessToken);
                result_dic.TryGetValue("userName", out o_UserName);

            }
            catch (Exception e)
            {
                strMsg.AppendFormat("异常:{0}\r\n", e.ToString());
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyplatformRegisterAccount");
            }
        }


        /// <summary>
        /// 5.1.2	用户注册接口
        /// 综合平台注册接口(需要验证码)
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="captcha"></param>
        /// <param name="result"></param>
        /// <param name="o_UserName"></param>
        /// <param name="accessToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="msg"></param>
        public void UnifyPlatformRegister(String appId, String appSecret, String version, String clientType, String clientIp, String clientAgent, String userName, String password,  String captcha, out int result,out String o_UserName,out String accessToken,  out long expiresIn, out String msg)
        {
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            string sign = String.Empty;
            o_UserName = String.Empty;
            accessToken = String.Empty;
            expiresIn = 0;
            result = -1;
            msg = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "userName=" + userName + "&password=" + password + "&captcha=" + captcha + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformRegisterUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String strResult = String.Empty;
                String strExpiresIn = String.Empty;
                result_dic.TryGetValue("result", out strResult);
                result_dic.TryGetValue("expiresIn", out strExpiresIn);
                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
                if (!String.IsNullOrEmpty(strExpiresIn))
                {
                    expiresIn = Convert.ToInt64(strExpiresIn);   
                }
               result_dic.TryGetValue("msg", out msg);
               result_dic.TryGetValue("userName", out o_UserName);
               result_dic.TryGetValue("accessToken", out accessToken);
            }
            catch (Exception e)
            {
                strMsg.AppendFormat("异常:{0}\r\n", e.ToString());
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyplatformRegister");
            }
        }


        /// <summary>
        /// 注册时获取短信验证码
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="mobile"></param>
        /// <param name="type"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public Int32 UnifyPlatformCaptcha(String appId, String appSecret, String version, String clientType,  String clientIp, String clientAgent, String mobile, String type,  out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            string sign = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "mobile=" + mobile +  "&type="+type+  "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformCaptchaUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion
                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                String result = String.Empty;
                String msg = String.Empty;
                result_dic.TryGetValue("result", out result);
                result_dic.TryGetValue("msg", out msg);
                Result = Convert.ToInt32(result);
                ErrMsg = msg;
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
                strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyPlatformCaptcha");
            }
            return Result;
        }

        /// <summary>
        /// 5.1.10	修改用户信息接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="accessToken"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="accountInfo"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public Int32 UnifyPlatformUpdateUserInfo(String appId, String appSecret, String version, String clientType, String accessToken, String clientIp, String clientAgent, UnifyAccountInfo accountInfo, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            accountInfo = new UnifyAccountInfo();
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            strMsg.AppendFormat("【UnifyPlatformUpdateUserInfo,DateTime:{3}】accessToken:{0},clientIp:{1},clientAgent:{2}\r\n", accessToken, clientIp, clientAgent, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string sign = String.Empty;

            try
            {
                // get方式
                #region
                //resultJson = HttpMethods.HttpGet(UDBConstDefinition.DefaultInstance.UnifyPlatformGetUserInfoUrl + "?accessToken=" + accessToken + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent );
                //strMsg.AppendFormat("resultJson:{0}", resultJson);                
                #endregion
                // 或者用post方式
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "accessToken=" + accessToken + "&gender="+accountInfo.gender
                +  "&birthday="+accountInfo.birthday+"&province="+accountInfo.province+"&city="+accountInfo.city
                + "&address=" + accountInfo.address + "&mail=" + accountInfo.mail + "&qq="+accountInfo.qq
                + "&position=" + accountInfo.position + "&intro=" + accountInfo.intro + "&nickName="+accountInfo.nickName
                +  "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformUpdateUserInfoUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion

                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

                String msg = String.Empty;//  String 256  获取出错信息
                int result = 0;// Integer 10 
                String strResult = String.Empty;

                //取值
         
                result_dic.TryGetValue("msg", out msg);
                result_dic.TryGetValue("result", out strResult);
                result = Convert.ToInt16(strResult);
                Result = result;
                ErrMsg = msg;
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
                strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
            }
            finally
            {
                WriteLog(strMsg.ToString(),"UnifyPlatformUpdateUserInfo");
            }
            return Result;
        }




        public Int32 UnifyPlatformFindPwd(String appId, String appSecret, String version, String clientType, String accessToken, String password, String nPassword, String clientIp, String clientAgent, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
          
            strMsg.AppendFormat("【UnifyPlatformUpdatePwd,DateTime:{3}】accessToken:{0},clientIp:{1},clientAgent:{2},password:{3},nPassword:{4}\r\n", accessToken, clientIp, clientAgent, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), password, nPassword);
            string sign = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "accessToken=" + accessToken + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent + "&password=" + password + "&nPassword=" + nPassword;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformFindPwdUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion

                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

                #region
                string msg = String.Empty;
                string result = String.Empty;
                result_dic.TryGetValue("msg", out ErrMsg);
                result_dic.TryGetValue("result", out result);
                Result = Convert.ToInt32(result);
                #endregion
            }
            catch (Exception e)
            {
                ErrMsg += e.Message;
                strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyPlatformUpdatePwd");
            }
            return Result;
        }


        /// <summary>
        /// 5.1.6	修改密码接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="version"></param>
        /// <param name="clientType"></param>
        /// <param name="accessToken"></param>
        /// <param name="password"></param>
        /// <param name="nPassword"></param>
        /// <param name="clientIp"></param>
        /// <param name="clientAgent"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public Int32 UnifyPlatformUpdatePwd(String appId, String appSecret, String version, String clientType, String accessToken, String password, String nPassword, String clientIp, String clientAgent,out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            strMsg.AppendFormat("【UnifyPlatformUpdatePwd,DateTime:{3}】accessToken:{0},clientIp:{1},clientAgent:{2},password:{3},nPassword:{4}\r\n", accessToken, clientIp, clientAgent, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),password,nPassword);
            string sign = String.Empty;
            try
            {
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "accessToken=" + accessToken + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent+ "&password=" + password + "&nPassword=" + nPassword;
                strMsg.AppendFormat("parameters:={0}\r\n",parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n",paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n",sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformUpdatePwdUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n",jsonResult);
                #endregion

                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

                #region
                string msg = String.Empty;
                string result = String.Empty;
                result_dic.TryGetValue("msg", out ErrMsg);
                result_dic.TryGetValue("result", out result);
                Result = Convert.ToInt32(result);
                #endregion
            }
            catch (Exception e)
            {
                ErrMsg += e.Message;
                strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
            }
            finally
            {
                WriteLog(strMsg.ToString(), "UnifyPlatformUpdatePwd");
            }
            return Result;
        }


        public Int32 UnifyPlatformLoginByImsi(String appId, String appSecret, String version, String clientType, String imsi, String clientIp, String clientAgent, out UnifyAccountInfo accountInfo, out String loginNum, out String accessToken, out long expiresIn,out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            accountInfo = new UnifyAccountInfo();
            loginNum = String.Empty;
            accessToken = String.Empty;
            expiresIn = 0;

            StringBuilder strMsg = new StringBuilder();
            strMsg.AppendFormat("时间:{0}\r\n",DateTime.Now.ToString("u"));
            strMsg.AppendFormat("参数,appId:{0},appSecret:{1},version:{2},clientType:{3},imsi:{4},clientIp:{5},clientAgent:{6}\r\n",appId,appSecret,version,clientType,imsi,clientIp,clientAgent);
            String jsonResult = String.Empty;
            string sign = String.Empty;
            try
            {

                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "imsi=" + imsi + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n", parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n", paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n", sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformLoginByIMSIUrl, "POST", postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n", jsonResult);
                #endregion

                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

                long userId = 0;//    Long 18 
                String strUserId = String.Empty;
                String pUserId = String.Empty;//  String 12  UDB 账号的ID
                String productUid = String.Empty;// String 30  基地账号原userId  可以为空 和appId有关
                int userType = 0;//  Integer 4   账号类型  暂未使用
                String strUserType = String.Empty;
                int status = 0;// Integer 4 账号状态
                String strStatus = String.Empty;
                String userName = String.Empty;// String 30
                String zhUserName = String.Empty;// String 30 综合平台UserName 
                String mobileName = String.Empty;// String 11 主手机号
                String emailName = String.Empty;// String60 主油箱名
                String mobileList = String.Empty;// String 300 绑定的手机号列表  181000001,181....
                String emailList = String.Empty;// String 300 绑定的邮箱列表  
                String aliasName = String.Empty;// String 30  主手机号别名
                String nickName = String.Empty;// String 30 昵称
                int gender = 0;// Integer  1 性别
                String strGender = String.Empty;
                String province = String.Empty;//  String 30 所属省 
                String city = String.Empty;//  String 60 所属市
                String birthday = String.Empty;//  Striing 8   yyyy-MM-dd
                String address = String.Empty;//  String 300 联系地址
                String mail = String.Empty;// String 60  联系邮箱
                String qq = String.Empty;//  String 50 qq账号
                String position = String.Empty;//  String 职位
                String intro = String.Empty;// String 300 个人介绍
                String userIconUrl1 = String.Empty;//  String 256 用户头像链接  150*150
                String userIconUrl2 = String.Empty;//  String 256 用户头像链接  50*50
                String msg = String.Empty;//  String 256  获取出错信息
                int result = 0;// Integer 10 
                String strResult = String.Empty;

                //取值
                result_dic.TryGetValue("userId", out strUserId);
                result_dic.TryGetValue("pUserId", out pUserId);
                result_dic.TryGetValue("productUid", out productUid);
                result_dic.TryGetValue("userType", out strUserType);
                result_dic.TryGetValue("status", out strStatus);
                result_dic.TryGetValue("userName", out userName);
                result_dic.TryGetValue("zhUserName", out zhUserName);
                result_dic.TryGetValue("mobileName", out mobileName);
                result_dic.TryGetValue("emailName", out emailName);
                result_dic.TryGetValue("mobileList", out mobileList);
                result_dic.TryGetValue("emailList", out emailList);
                result_dic.TryGetValue("aliasName", out aliasName);
                result_dic.TryGetValue("nickName", out nickName);
                result_dic.TryGetValue("gender", out strGender);
                result_dic.TryGetValue("province", out province);
                result_dic.TryGetValue("city", out city);
                result_dic.TryGetValue("birthday", out birthday);
                result_dic.TryGetValue("address", out address);
                result_dic.TryGetValue("mail", out mail);
                result_dic.TryGetValue("qq", out qq);
                result_dic.TryGetValue("position", out position);
                result_dic.TryGetValue("intro", out intro);
                result_dic.TryGetValue("userIconUrl1", out userIconUrl1);
                result_dic.TryGetValue("userIconUrl2", out userIconUrl2);
                result_dic.TryGetValue("msg", out msg);
                result_dic.TryGetValue("result", out strResult);

                result_dic.TryGetValue("accessToken",out accessToken);
                result_dic.TryGetValue("loginNum",out loginNum);
                string t_exp = String.Empty;
                result_dic.TryGetValue("expiresIn", out t_exp);
                if (!String.IsNullOrEmpty(t_exp))
                {
                    expiresIn = Convert.ToInt64(t_exp);
                }
                if (!String.IsNullOrEmpty(strUserId))
                {
                    userId = Convert.ToInt64(strUserId);
                }
                if (!String.IsNullOrEmpty(strUserType))
                {
                    userType = Convert.ToInt32(strUserType);
                }

                if (!String.IsNullOrEmpty(strStatus))
                {
                    status = Convert.ToInt32(strStatus);
                }

                if (!String.IsNullOrEmpty(strGender))
                {
                    gender = Convert.ToInt32(strGender);
                }
                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
                Result = result;
                ErrMsg = msg;
                if (result == 0)
                {
                    accountInfo.address = address;
                    accountInfo.aliasName = aliasName;
                    accountInfo.birthday = birthday;
                    accountInfo.city = city;
                    accountInfo.emailList = emailList;
                    accountInfo.emailName = emailName;
                    accountInfo.gender = gender;
                    accountInfo.intro = intro;
                    accountInfo.mail = mail;
                    accountInfo.mobileList = mobileList;
                    accountInfo.mobileName = mobileName;

                    accountInfo.nickName = nickName;
                    accountInfo.productUid = productUid;
                    accountInfo.province = province;
                    accountInfo.pUserId = pUserId;
                    accountInfo.qq = qq;
                    accountInfo.position = position;

                    accountInfo.status = status;
                    accountInfo.userIconUrl1 = userIconUrl1;
                    accountInfo.userIconUrl2 = userIconUrl2;
                    accountInfo.userId = userId;
                    accountInfo.userName = userName;
                    accountInfo.userType = userType;
                    accountInfo.zhUserName = zhUserName;
                }

            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
                strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
            }
            finally
            {
                WriteLog(strMsg.ToString(),"UnifyPlatformLoginByImsi");
            }
            return Result;
        }

        /// <summary>
        /// 根据accessToken查询用户信息
        /// </summary>
        public Int32 UnifyPlatformGetUserInfo(String appId, String appSecret, String version, String clientType, String accessToken, String clientIp, String clientAgent, out UnifyAccountInfo accountInfo, out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            accountInfo = new UnifyAccountInfo();
            StringBuilder strMsg = new StringBuilder();
            String jsonResult = String.Empty;
            strMsg.AppendFormat("【UnifyPlatformGetUserInfo,DateTime:{3}】accessToken:{0},clientIp:{1},clientAgent:{2}\r\n", accessToken, clientIp, clientAgent, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string sign = String.Empty;

            try
            {
                // get方式
                #region
                //resultJson = HttpMethods.HttpGet(UDBConstDefinition.DefaultInstance.UnifyPlatformGetUserInfoUrl + "?accessToken=" + accessToken + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent );
                //strMsg.AppendFormat("resultJson:{0}", resultJson);                
                #endregion
                // 或者用post方式
                #region
                string paras = String.Empty;
                string format = "json";
                string parameters = "accessToken=" + accessToken + "&clientIp=" + clientIp + "&clientAgent=" + clientAgent;
                strMsg.AppendFormat("parameters:={0}\r\n",parameters);
                paras = CryptographyUtil.XXTeaEncrypt(parameters, appSecret);
                strMsg.AppendFormat("paras:={0}\r\n",paras);
                sign = CryptographyUtil.HMAC_SHA1(appId + clientType + format + version + paras, appSecret);
                strMsg.AppendFormat("sign:={0}\r\n",sign);
                NameValueCollection postData = new NameValueCollection();
                postData.Add("appId", appId);
                postData.Add("version", version);
                postData.Add("clientType", clientType);
                postData.Add("paras", paras);
                postData.Add("sign", sign);
                postData.Add("format", format);

                WebClient webclient = new WebClient();
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webclient.UploadValues(UDBConstDefinition.DefaultInstance.UnifyPlatformGetUserInfoUrl, "POST",postData);
                jsonResult = System.Text.Encoding.UTF8.GetString(responseData);
                strMsg.AppendFormat("jsonResult:{0}\r\n",jsonResult);
                #endregion

                Dictionary<string, string> result_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

                long userId =0 ;//    Long 18 
                String strUserId = String.Empty;
                String pUserId =String.Empty  ;//  String 12  UDB 账号的ID
                String productUid = String.Empty ;// String 30  基地账号原userId  可以为空 和appId有关
                int userType =0 ;//  Integer 4   账号类型  暂未使用
                String strUserType  = String.Empty;
                int status = 0;// Integer 4 账号状态
                String strStatus = String.Empty;
                String userName =String.Empty ;// String 30
                String zhUserName =String.Empty ;// String 30 综合平台UserName 
                String mobileName =String.Empty ;// String 11 主手机号
                String emailName =String.Empty;// String60 主油箱名
                String mobileList =String.Empty;// String 300 绑定的手机号列表  181000001,181....
                String emailList =String.Empty ;// String 300 绑定的邮箱列表  
                String aliasName =String.Empty ;// String 30  主手机号别名
                String nickName =String.Empty ;// String 30 昵称
                int gender = 0;// Integer  1 性别
                String strGender = String.Empty;
                String province =String.Empty;//  String 30 所属省 
                String city = String.Empty;//  String 60 所属市
                String birthday = String.Empty;//  Striing 8   yyyy-MM-dd
                String address =String.Empty ;//  String 300 联系地址
                String mail=String.Empty;// String 60  联系邮箱
                String qq = String.Empty;//  String 50 qq账号
                String position = String.Empty;//  String 职位
                String intro = String.Empty ;// String 300 个人介绍
                String userIconUrl1 = String.Empty;//  String 256 用户头像链接  150*150
                String userIconUrl2 = String.Empty;//  String 256 用户头像链接  50*50
                String msg = String.Empty                    ;//  String 256  获取出错信息
                int result = 0;// Integer 10 
                String strResult = String.Empty;

                //取值
                result_dic.TryGetValue("userId", out strUserId);
                result_dic.TryGetValue("pUserId", out pUserId);
                result_dic.TryGetValue("productUid", out productUid);
                result_dic.TryGetValue("userType", out strUserType);
                result_dic.TryGetValue("status", out strStatus);
                result_dic.TryGetValue("userName", out userName);
                result_dic.TryGetValue("zhUserName", out zhUserName);
                result_dic.TryGetValue("mobileName", out mobileName);
                result_dic.TryGetValue("emailName", out emailName);
                result_dic.TryGetValue("mobileList", out mobileList);
                result_dic.TryGetValue("emailList", out emailList);
                result_dic.TryGetValue("aliasName", out aliasName);
                result_dic.TryGetValue("nickName", out nickName);
                result_dic.TryGetValue("gender", out strGender);
                result_dic.TryGetValue("province", out province);
                result_dic.TryGetValue("city", out city);
                result_dic.TryGetValue("birthday", out birthday);
                result_dic.TryGetValue("address", out address);
                result_dic.TryGetValue("mail", out mail);
                result_dic.TryGetValue("qq", out qq);
                result_dic.TryGetValue("position", out position);
                result_dic.TryGetValue("intro", out intro);
                result_dic.TryGetValue("userIconUrl1", out userIconUrl1);
                result_dic.TryGetValue("userIconUrl2", out userIconUrl2);
                result_dic.TryGetValue("msg", out msg);
                result_dic.TryGetValue("result", out strResult);

                

                if (!String.IsNullOrEmpty(strUserId))
                {
                    userId = Convert.ToInt64(strUserId);
                }

                if (!String.IsNullOrEmpty(strUserType))
                {
                    userType = Convert.ToInt32(strUserType);
                }

                if (!String.IsNullOrEmpty(strStatus))
                {
                    status = Convert.ToInt32(strStatus);
                }

                if (!String.IsNullOrEmpty(strGender))
                {
                    gender = Convert.ToInt32(strGender);
                }
                if (!String.IsNullOrEmpty(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
                Result = result;
                ErrMsg = msg;
                if (result == 0)
                {
                    accountInfo.address = address;
                    accountInfo.aliasName = aliasName;
                    accountInfo.birthday = birthday;
                    accountInfo.city = city;
                    accountInfo.emailList = emailList;
                    accountInfo.emailName = emailName;
                    accountInfo.gender = gender;
                    accountInfo.intro = intro;
                    accountInfo.mail = mail;
                    accountInfo.mobileList = mobileList;
                    accountInfo.mobileName = mobileName;

                    accountInfo.nickName = nickName;
                    accountInfo.productUid = productUid;
                    accountInfo.province = province;
                    accountInfo.pUserId = pUserId;
                    accountInfo.qq = qq;
                    accountInfo.position = position;
       
                    accountInfo.status = status;
                    accountInfo.userIconUrl1 = userIconUrl1;
                    accountInfo.userIconUrl2 = userIconUrl2;
                    accountInfo.userId = userId;
                    accountInfo.userName = userName;
                    accountInfo.userType = userType;
                    accountInfo.zhUserName = zhUserName;
                }

 

                #region 隐藏

          
                #endregion

            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
                strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
            }
            finally
            {
                WriteLog(strMsg.ToString());
            }

            return Result;
        }


        //public Int32 AccountInfoCheck(String SrcSsDeviceNo, String AuthSsDeviceNo, String UDBTicket, String key, out UDBAccountInfo accountInfo, out String ErrMsg)
        //{
        //    Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        //    accountInfo = new UDBAccountInfo();
        //    ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
        //    StringBuilder strMsg = new StringBuilder();
        //    strMsg.AppendFormat("【AccountInfoQuery,DateTime:{3}】SrcSsDeviceNo:{0},AuthSsDeviceNo:{1},UDBTicket:{2}", SrcSsDeviceNo, AuthSsDeviceNo, UDBTicket, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //    try
        //    {
        //        String timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //        String digest = CryptographyUtil.GenerateAuthenticator(SrcSsDeviceNo + AuthSsDeviceNo + UDBTicket + timeStamp, key);
        //        strMsg.AppendFormat(",timeStamp:{0},digest:{1}", timeStamp, digest);

        //        UDBAppSys udbClient = new UDBAppSys();
        //        AccountInfoCheckResult udbResult = udbClient.AccountInfoCheck(digest, SrcSsDeviceNo, AuthSsDeviceNo, UDBTicket, timeStamp);
        //        if (udbResult != null)
        //        {
        //            accountInfo.Alias = udbResult.Alias;
        //            accountInfo.BindingAccessNo = udbResult.BindingAccessNo;
        //            accountInfo.Description = udbResult.Description;
        //            accountInfo.PrePaySystemNo = udbResult.PrePaySystemNo;
        //            accountInfo.PUserID = udbResult.PUserID;

        //            accountInfo.ThirdSsUserID = udbResult.ThirdSsUserID;
        //            accountInfo.UserID = udbResult.UserID;
        //            accountInfo.UserIDSsStatus = udbResult.UserIDSsStatus;
        //            accountInfo.UserIDStatus = udbResult.UserIDStatus;
        //            accountInfo.UserIDType = udbResult.UserIDType;
        //            accountInfo.UserPayType = udbResult.UserPayType;
        //            accountInfo.UserType = Convert.ToString(udbResult.UserType);



        //            if (udbResult.ResultCode == 0 || udbResult.ResultCode == 5)
        //            {
        //                //<Ex><PID>07</PID><NF>2</NF></Ex>
        //                Int32 startIndex = udbResult.Description.IndexOf("<PID>");
        //                Int32 endIndex = udbResult.Description.IndexOf("</PID>");
        //                accountInfo.ProvinceID = udbResult.Description.Substring(startIndex + 5, endIndex - startIndex - 5);
        //                startIndex = udbResult.Description.IndexOf("<NF>");
        //                endIndex = udbResult.Description.IndexOf("</NF>");
        //                accountInfo.NumFlag = udbResult.Description.Substring(startIndex + 4, endIndex - startIndex - 4);
        //                Result = 0;
        //            }
        //            else
        //            {
        //                ErrMsg = udbResult.Description;
        //            }

        //        }

        //        strMsg.AppendFormat(",Alias:{0},BindingAccessNo:{1},Description:{2},PrePaySystemNo:{3},PUserID:{4},ResultCode:{5},ThirdSsUserID:{6},UserID:{7}", udbResult.Alias, udbResult.BindingAccessNo, udbResult.Description, udbResult.PrePaySystemNo, udbResult.PUserID, udbResult.ResultCode, udbResult.ThirdSsUserID, udbResult.UserID);

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrMsg += ex.Message;
        //        strMsg.AppendFormat(",ErrMsg:{0}", ErrMsg);
        //    }
        //    finally
        //    {
        //        WriteLog(strMsg.ToString());
        //    }

        //    return Result;
        //}




        /// <summary>
        /// 写日志功能
        /// </summary>
        protected void WriteLog(String str,String method)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog(method, msg);
        }
        



        /// <summary>
        /// 写日志功能
        /// </summary>
        protected void WriteLog(String str)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            msg.Append(str);
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            BTUCenterInterfaceLog.CenterForBizTourLog("UnifyPlatformGetAccountInfo", msg);
        }
    }
}

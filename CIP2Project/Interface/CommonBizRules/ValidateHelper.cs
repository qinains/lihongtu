using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Data;

namespace Linkage.BestTone.Interface.Rule
{
    /// <summary>
    /// 一些业务逻辑的验证
    /// </summary>
    public class ValidateHelper
    {

        /// <summary>
        /// 验证电话号码有效性
        /// </summary>
        public static bool PhoneNumValid(HttpContext context, string PhoneNum, out string OutPhone)
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

        /// <summary>
        /// 接口访问时对SPID进行验证判断
        /// </summary>
        public static Int32 ValidateInterfaceSPID(String SPID,String InterfaceName,String EncryptStr,out String ErrMsg)
        {
            Int32 Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            try
            {
                String interfaceList = "," + ConstHelper.DefaultInstance.UnValidateSPIDInterface + ",";
                String spidList = "," + SPID + ",";

                if (interfaceList.IndexOf(InterfaceName) >= 0 || spidList.IndexOf(SPID) >= 0)
                {
                    Result = 0;
                    ErrMsg = "";
                    return Result;
                }

                Result = ValidateSPIDData(SPID, EncryptStr, out ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// 根据SPID对业务平台加密的数据进行验证
        /// </summary>
        public static Int32 ValidateSPIDData(String SPID,String encryptStr,out String ErrMsg)
        {
            Int32 result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = ErrorDefinition.BT_IError_Result_UnknowError_Msg;
            try
            {
                //根据SPID获取key
                SPInfoManager spinfo = new SPInfoManager();
                Object SPData = spinfo.GetSPData(HttpContext.Current, "SPData");
                String key = spinfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

                //根据key无法解密则数据或key不正确
                String decryptStr = CryptographyUtil.Decrypt(encryptStr, key);
                if (String.IsNullOrEmpty(decryptStr))
                {
                    ErrMsg = "数据解密出错";
                    return result;
                }

                //数组最少为2维：原始数据+Digest
                String[] tempArray = decryptStr.Split('$');
                Int32 len = tempArray.Length;
                if (len <= 2)
                {
                    ErrMsg = "数据格式有误";
                    return result;
                }

                String Digest = tempArray[len - 1];
                StringBuilder tempStr = new StringBuilder();
                Int32 i = 0;
                foreach (String temp in tempArray)
                {
                    if (i == len - 1)
                        break;
                    tempStr.Append(temp + "$");
                    i++;
                }
                
                String newDigest = CryptographyUtil.Encrypt(tempStr.ToString().TrimEnd('$'), key);
                if (newDigest.Equals(Digest))
                    result = 0;
            }
            catch (Exception ex)
            {
                ErrMsg += ex.Message;
            }

            return result;
        }

        /// <summary>
        /// IP访问权限判断
        /// 作者：苑峰      时间：2009-8-11
        /// 修改：          时间：
        /// </summary>
        public static int CheckIPLimit(string SPID, string IP, HttpContext context, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            DataTable dt = null;
            try
            {
                string IsIPLimit = System.Configuration.ConfigurationManager.AppSettings["IsIPLimit"];
                //若不启用则返回允许
                if (IsIPLimit == "1")
                {
                    Result = 0;
                    return Result;
                }
                //根据传入IP获取IPNumber
                long IPNumber = CommonBizRules.GetIPAddressIPNumber(IP);
                //从缓存中获取数据
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
                        //如果ＩＰ在限制列表中则成功
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
        /// 接口访问权限判断
        /// 作者：苑峰      时间：2009-8-11
        /// 修改：          时间：
        /// </summary>
        public static int CheckInterfaceLimit(string SPID, string InterfaceName, HttpContext context, out string ErrMsg)
        {
            int Result = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrMsg = "";
            DataTable dt = null;
            try
            {
                string IsInterfaceLimit = System.Configuration.ConfigurationManager.AppSettings["IsInterfaceLimit"];
                //若不启用则返回允许
                if (IsInterfaceLimit == "1")
                {
                    Result = 0;
                    return Result;
                }
                //从缓存中获取数据
                SPInfoManager spInfo = new SPInfoManager();
                SPInterfaceLimitData SPInterfaceLimitData = (SPInterfaceLimitData)spInfo.GetSPData(context, "SPInterfaceLimitData");

                dt = SPInterfaceLimitData.Tables[SPInterfaceLimitData.TableName];
                string tmpInterfaceName = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (SPID == dt.Rows[i][SPInterfaceLimitData.Field_SPID].ToString())
                    {
                        tmpInterfaceName = dt.Rows[i][SPInterfaceLimitData.Field_InterfaceName].ToString().Trim();
                        //如果ＩＰ在限制列表中则成功
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

    }
}

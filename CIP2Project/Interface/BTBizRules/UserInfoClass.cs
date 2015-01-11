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
using Linkage.BestTone.Interface.Rule;

namespace Linkage.BestTone.Interface.Rule
{
    public class UserInfoClass
    {
        public UserInfoClass()
        { }

        public UserInfoClass(UserInfo UserDetailInfo)
        {
            this.userType = UserDetailInfo.UserType;
            this.userAccount = UserDetailInfo.UserAccount;
            this.password = UserDetailInfo.Password;
            this.custID = UserDetailInfo.CustID;
            this.uProvinceID = UserDetailInfo.UProvinceID;
            this.areaCode = UserDetailInfo.AreaCode;

            this.Status = UserDetailInfo.Status;
            this.RealName = UserDetailInfo.RealName;
            this.CertificateCode = UserDetailInfo.CertificateCode;
            this.CertificateType = UserDetailInfo.CertificateType;
            this.Birthday = UserDetailInfo.Birthday;
            this.Sex = UserDetailInfo.Sex;
            this.CustLevel = UserDetailInfo.CustLevel;
            this.EduLevel = UserDetailInfo.EduLevel;
            this.Favorite = UserDetailInfo.Favorite;
            this.IncomeLevel = UserDetailInfo.IncomeLevel;
            this.Email = UserDetailInfo.Email;
            this.PaymentAccountType = UserDetailInfo.PaymentAccountType;
            this.PaymentAccount = UserDetailInfo.PaymentAccount;
            this.PaymentAccountPassword = UserDetailInfo.PaymentAccountPassword;
            this.CustContactTel = UserDetailInfo.CustContactTel;
            this.IsPost = UserDetailInfo.IsPost;
            this.boundPhoneRecords = UserDetailInfo.BoundPhoneRecords;
            this.addressRecords = UserDetailInfo.AddressRecords;
            this.assessmentInfoRecords = UserDetailInfo.AssessmentInfos;
            this.extendField = UserDetailInfo.ExtendField;
            //this.userName = UserDetailInfo.UserName;
            
        }

        /// <summary>
        /// 用户信息基本性校验（长度，是否为空）
        /// </summary>
        /// <param name="UserDetailInfo"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int UserInfoCommonCheck(out string ErrMsg, HttpContext Context)
        {
            int Result = 0;
            ErrMsg = "";

            string OutPhone = "";
            try
            {
               

                if (CommonUtility.IsEmpty(userType))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + ",不能为空";
                    return Result;
                }


                if (ConstDefinition.Span_UserType.IndexOf(userType) < 0)
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + "，无该用户类型";
                    return Result;
                }

                if (!CommonUtility.IsEmpty(userAccount))
                {
                    if (userAccount.Length < ConstDefinition.Length_Min_UserAccount)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                        return Result;
                    }

                    if (userAccount.Length > ConstDefinition.Length_Max_UserAccount)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidUserAccount_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidUserAccount_Msg + "，长度有误";
                        return Result;
                    }


                }

                if (!CommonUtility.IsEmpty(incomeLevel))
                {
                    if (ConstDefinition.Span_IncomeLevel.IndexOf(incomeLevel) < 0)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidIncomeLevel_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidIncomeLevel_Msg + "，不能为空";
                        return Result;
                    }

                }

                if (!CommonUtility.IsEmpty(eduLevel))
                {
                    if (ConstDefinition.Span_EduLevel.IndexOf(eduLevel) < 0)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidEduLevel_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidEduLevel_Msg + ",无该教育水平";
                        return Result;
                    }

                }

                //if (CommonUtility.IsEmpty(uProvinceID))
                //{

                //    Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                //    ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",uProvinceID不能为空";
                //    return Result;
                //}

                //if (uProvinceID.Length != ConstDefinition.Length_ProvinceID)
                //{
                //    Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                //    ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",uProvinceID长度有误";
                //    return Result;
                //}


                if (CommonUtility.IsEmpty(areaCode))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidAreaCode_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidAreaCode_Msg + ",区号不能为空";
                    return Result;
                }

                if (areaCode.Length == 2)
                    areaCode = "0" + areaCode;

                //if (areaCode.Length != ConstDefinition.Length_AreaCode)
                //{
                //    Result = ErrorDefinition.BT_IError_Result_InValidAreaCode_Code;
                //    ErrMsg = ErrorDefinition.BT_IError_Result_InValidAreaCode_Msg + ",区号长度有误";
                //    return Result;
                //}

                //PhoneAreaInfoManager areaObject = new PhoneAreaInfoManager();
                //object areaDataObject = areaObject.GetPhoneAreaData(Context);
                //uProvinceID = areaObject.GetPropertyByPhoneAreaCode(areaCode, "AreaName", areaDataObject);
                //if (CommonUtility.IsEmpty(uProvinceID))
                //{
                //    Result = ErrorDefinition.BT_IError_Result_InValidAreaCode_Code;
                //    ErrMsg = ErrorDefinition.BT_IError_Result_InValidAreaCode_Msg + ",该区号不存在";
                //    return Result;
                //}

                if (CommonUtility.IsEmpty(status))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + ",状态不能为空";
                    return Result;
                }
                if (ConstDefinition.Span_UserStatus.IndexOf(status) < 0)
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + ",无此状态类型";
                    return Result;
                }

                if (CommonUtility.IsEmpty(custLevel))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidCustLevel_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustLevel_Msg + ",客户级别不能为空";
                    return Result;
                }

                if (ConstDefinition.Span_CustLevel.IndexOf(custLevel) < 0)
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidCustLevel_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustLevel_Msg + ",无此客户级别";
                    return Result;
                }

                if (CommonUtility.IsEmpty(sex))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidSex_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidSex_Msg + ",不能为空";
                    return Result;
                }

                if (ConstDefinition.Span_Sex.IndexOf(sex) < 0)
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidSex_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidSex_Msg + "，无此性别定义";
                    return Result;
                }

                if (CommonUtility.IsEmpty(custContactTel))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidCustContactTel_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidCustContactTel_Msg + "，不能为空";
                    return Result;
                }

                if (!CommonBizRules.PhoneNumValid(Context,custContactTel, out OutPhone))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    ErrMsg = "联系电话无效";
                    return Result;
                }
                custContactTel = OutPhone;
                if (!CommonUtility.IsEmpty(certificateType))
                {
                    if (ConstDefinition.Span_CertificateType.IndexOf(certificateType) < 0)
                    {

                        Result = ErrorDefinition.BT_IError_Result_InValidCertificateType_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidCertificateType_Msg + ",无此证件类型";
                        return Result;
                    }

                    if (certificateCode.Length > 20)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidCertificateCode_Msg + ",证件长度长度有误";
                        return Result;

                    }
                }

                if (!CommonUtility.IsEmpty(password))
                {
                    if (password.Length < ConstDefinition.Length_Min_Password)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + ",密码不能小于6位";
                        return Result;
                    }
                }

                if (!CommonUtility.IsEmpty(paymentAccountType))
                {
                    if (ConstDefinition.Span_PaymentAccountType.IndexOf(paymentAccountType) < 0)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidPaymentAccountType_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidPaymentAccountType_Msg + "，无该类型";
                        return Result;
                    }
                }

                if (!CommonUtility.IsEmpty(isPost))
                {
                    if (ConstDefinition.Span_IsPost.IndexOf(isPost) < 0)
                    {
                        Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                        ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，是否邮寄参数错误";
                        return Result;
                    }
                }

                //ProvinceInfoManager proObject = new ProvinceInfoManager();
                //object proDataObject = proObject.GetProvinceData(Context);
                //if (proObject.GetPropertyByProvinceID(uProvinceID, "ProvinceCode", proDataObject) == "")
                //{
                //    Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                //    ErrMsg = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + ",无效的用户所属省";
                //    return Result;
                //}

                if (boundPhoneRecords != null)
                {
                    int MaxBoundPhone = int.Parse(ConfigurationManager.AppSettings["MaxBoundPhone"]);
                    if (boundPhoneRecords.Length > 5)
                    {
                        Result = ErrorDefinition.BT_IError_Result_BoundPhoneExceed_Code;
                        ErrMsg = "绑定电话超过个数限制,每个用户只能绑定" + MaxBoundPhone.ToString() + "个电话。";
                        return Result;
                    }

                    if (boundPhoneRecords.Length > 0)
                    {
                        string phone = "";
                        for (int i = 0; i < boundPhoneRecords.Length; i++)
                        {
                            if (!CommonBizRules.PhoneNumValid(Context, boundPhoneRecords[i].Phone, out phone))
                            {
                                Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                                ErrMsg = "第" + (i + 1).ToString() + "条绑定电话无效";
                                return Result;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;

            }
            return Result;

        }

        //密码鉴权认证
        public static int ValidUserPassword(string CustID, string EncryptedPassword)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            string ErrMsg = "";
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try{

                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_OV3_Interface_ValidUserPassword";

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CustID;
                cmd.Parameters.Add(parCustID);


                SqlParameter parEncryptedPassword = new SqlParameter("@EncryptedPassword", SqlDbType.VarChar, 50);
                parEncryptedPassword.Value = EncryptedPassword;
                cmd.Parameters.Add(parEncryptedPassword);


                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);
            
                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());



            }catch(Exception ex){

                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }
            return Result;
        }

        //密码验证用
        public static int CheckUserPassword(string ProvinceID, string SPID, string CustID, string UserAccount,string PhoneNum, string EncryptedPassword, out string ErrMsg, out UserInfo UserDetailInfo)
        {

            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            UserDetailInfo = null;
            //SubscriptionRecords = null;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_OV3_Interface_CheckUserPassword";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);


                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = (CustID == null ? "" : CustID);
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = (PhoneNum == null ? "" : PhoneNum);
                cmd.Parameters.Add(parPhoneNum);


                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = (UserAccount == null ? "" : UserAccount);
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parEncryptedPassword = new SqlParameter("@EncryptedPassword", SqlDbType.VarChar, 50);
                parEncryptedPassword.Value = (EncryptedPassword == null ? "" : EncryptedPassword);
                cmd.Parameters.Add(parEncryptedPassword);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);


                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                if (Result != 0)
                    return Result;

                if (ds == null)
                {
                    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                    ErrMsg = "无用户记录";
                    return Result;
                }

                //if (ds.Tables.Count != 1)
                //{
                //    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                //    ErrMsg = "无用户记录";
                //    return Result;
                //}

                UserDetailInfo = new UserInfo();

                //DataTable tb = new DataTable();
                //tb = ds.Tables[0];
                //if (tb != null)
                //{
                //    if (tb.Rows.Count > 0)
                //    {
                //        UserDetailInfo.UserType = tb.Rows[0]["UserType"].ToString().Trim();
                //        UserDetailInfo.UserAccount = tb.Rows[0]["UserAccount"].ToString().Trim();
                //        UserDetailInfo.Password = "";
                //        UserDetailInfo.CustID = tb.Rows[0]["CustID"].ToString().Trim();
                //        UserDetailInfo.UProvinceID = tb.Rows[0]["UProvinceID"].ToString().Trim();
                //        UserDetailInfo.AreaCode = tb.Rows[0]["AreaCode"].ToString().Trim();
                //        UserDetailInfo.Status = tb.Rows[0]["Status"].ToString().Trim();
                //        UserDetailInfo.RealName = tb.Rows[0]["RealName"].ToString().Trim();
                //        UserDetailInfo.CertificateCode = tb.Rows[0]["CertificateCode"].ToString().Trim();
                //        UserDetailInfo.CertificateType = tb.Rows[0]["CertificateType"].ToString().Trim();
                //        UserDetailInfo.Birthday = DateTime.Parse(tb.Rows[0]["Birthday"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                //        UserDetailInfo.Sex = tb.Rows[0]["Sex"].ToString().Trim();
                //        UserDetailInfo.CustLevel = tb.Rows[0]["CustLevel"].ToString().Trim();
                //        UserDetailInfo.EduLevel = tb.Rows[0]["EduLevel"].ToString().Trim();
                //        UserDetailInfo.Favorite = tb.Rows[0]["Favorite"].ToString().Trim();
                //        UserDetailInfo.IncomeLevel = tb.Rows[0]["IncomeLevel"].ToString().Trim();
                //        UserDetailInfo.Email = tb.Rows[0]["Email"].ToString().Trim();
                //        UserDetailInfo.PaymentAccountType = tb.Rows[0]["PaymentAccountType"].ToString().Trim();
                //        UserDetailInfo.PaymentAccount = tb.Rows[0]["PaymentAccount"].ToString().Trim();
                //        UserDetailInfo.PaymentAccountPassword = "";//tb.Rows[0]["PaymentAccountPassword"].ToString().Trim();
                //        //UserDetailInfo.CustContactTel = tb.Rows[0]["CustContactTel"].ToString().Trim();
                //        UserDetailInfo.CustContactTel = tb.Rows[0]["Phone"].ToString().Trim();
                //    }
                //}
            }
            catch (Exception ex)
            {

                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }
            return Result;
        }


        /// <summary>
        /// 客户信息查询接口
        /// 作者：周涛   时间：2009-8-14
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="CustID"></param>
        /// <param name="CertificateCode"></param>
        /// <param name="CertificateType"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="UserDetailInfo"></param>
        /// <returns></returns>
        public static int UserInfoQuery(string ProvinceID, string SPID, string UserAccount, string CustID,
            string PhoneNum, string Password, out string ErrMsg, out UserInfo UserDetailInfo, out SubscriptionRecord[] SubscriptionRecords)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            UserDetailInfo = null;
            SubscriptionRecords = null;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_OV3_Interface_UserInfoQuery";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = (UserAccount == null ? "" : UserAccount);
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = (CustID == null ? "" : CustID);
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = (PhoneNum == null ? "" : PhoneNum);
                cmd.Parameters.Add(parPhoneNum);


                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 6);
                parPassword.Value = Password;
                cmd.Parameters.Add(parPassword);

                SqlParameter parEncryptedPassword = new SqlParameter("@EncryptedPassword", SqlDbType.VarChar, 50);
                parEncryptedPassword.Value = (CommonUtility.IsEmpty(Password) ? "" : CryptographyUtil.Encrypt(Password));
                cmd.Parameters.Add(parEncryptedPassword);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                //生成四个表
                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                //特殊错误描述代表激活码验证成功
                if (Result != 0 || ErrMsg == "000")
                    return Result;

                if (ds == null)
                {
                    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                    ErrMsg = "无用户记录";
                    return Result;
                }

                UserDetailInfo = new UserInfo();

                string UserName = "";
                string MobilePhone = "";

                DataTable tb = new DataTable();
                tb = ds.Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        string UserType1=tb.Rows[0]["CustType"].ToString().Trim();
                        #region 判断是否是特殊的SPID

                        string OldSPID = ConfigurationManager.AppSettings["OldType_SPID"];

                        int SIP = OldSPID.IndexOf(SPID);

                        if (SIP >= 0)
                        {
                            // userType = "";
                            switch (UserType1)
                            {
                                case "14":
                                    UserType1 = "01";
                                    break;
                                case "20":
                                    UserType1 = "02";
                                    break;
                                case "12":
                                    UserType1 = "03";
                                    break;
                                case "90":
                                    UserType1 = "09";
                                    break;
                                case "30":
                                    UserType1 = "11";
                                    break;
                                case "42":
                                    UserType1 = "00";
                                    break;
                                default:
                                    UserType1 = "90";
                                    break;
                            }

                        }

                        #endregion
                        UserDetailInfo.UserType = UserType1;
                        UserDetailInfo.UserAccount = tb.Rows[0]["CardID"].ToString().Trim();
                        UserDetailInfo.Password = "";
                        UserDetailInfo.CustID = tb.Rows[0]["CustID"].ToString().Trim();
                        UserDetailInfo.UProvinceID = tb.Rows[0]["ProvinceID"].ToString().Trim();
                        UserDetailInfo.AreaCode = tb.Rows[0]["AreaID"].ToString().Trim();
                        UserDetailInfo.Status = tb.Rows[0]["Status"].ToString().Trim();
                        UserDetailInfo.RealName = tb.Rows[0]["RealName"].ToString().Trim();
                        //UserDetailInfo.UserName = tb.Rows[0]["UserName"].ToString().Trim();
                        UserName = tb.Rows[0]["UserName"].ToString().Trim();
                        UserDetailInfo.CertificateCode = tb.Rows[0]["CertificateCode"].ToString().Trim();
                        UserDetailInfo.CertificateType = tb.Rows[0]["CertificateType"].ToString().Trim();
                        if (!CommonUtility.IsEmpty(tb.Rows[0]["Birthday"].ToString()))
                        {
                            UserDetailInfo.Birthday = DateTime.Parse(tb.Rows[0]["Birthday"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                        }
                        else
                        {
                            UserDetailInfo.Birthday = "";
                        }
                        UserDetailInfo.Sex = tb.Rows[0]["Sex"].ToString().Trim();
                        UserDetailInfo.CustLevel = tb.Rows[0]["CustLevel"].ToString().Trim();
                        UserDetailInfo.EduLevel = tb.Rows[0]["EduLevel"].ToString().Trim();
                        UserDetailInfo.Favorite = tb.Rows[0]["Favorite"].ToString().Trim();
                        UserDetailInfo.IncomeLevel = tb.Rows[0]["IncomeLevel"].ToString().Trim();
                        UserDetailInfo.Email = tb.Rows[0]["Email"].ToString().Trim();
                        UserDetailInfo.EnterpriseID = tb.Rows[0]["EnterpriseID"].ToString().Trim();
                        UserDetailInfo.PaymentAccountType = "";
                        UserDetailInfo.PaymentAccount = "";
                        UserDetailInfo.PaymentAccountPassword = "";//tb.Rows[0]["PaymentAccountPassword"].ToString().Trim();
                        UserDetailInfo.CustContactTel = tb.Rows[0]["Phone"].ToString().Trim();
                        UserDetailInfo.IsPost = "";

                        if(!tb.Rows[0]["PhoneClass"].ToString().Equals("1"))
                        {
                            MobilePhone = tb.Rows[0]["Phone"].ToString().Trim();
                        }
                    }
                }

                //绑定电话
                tb = ds.Tables[1];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        UserDetailInfo.BoundPhoneRecords = new BoundPhoneRecord[tb.Rows.Count];
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            BoundPhoneRecord tmpBoundPhone = new BoundPhoneRecord();
                            tmpBoundPhone.Phone = tb.Rows[i]["Phone"].ToString().Trim();
                            UserDetailInfo.BoundPhoneRecords[i] = tmpBoundPhone;
                        }
                    }
                }

                //联系信息
                tb = ds.Tables[2];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        UserDetailInfo.AddressRecords = new AddressRecord[tb.Rows.Count];
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            AddressRecord tmpAddress = new AddressRecord();
                            tmpAddress.Address = tb.Rows[i]["Address"].ToString().Trim();
                            tmpAddress.ContactTel = tb.Rows[i]["Phone"].ToString().Trim();
                            tmpAddress.LinkMan = tb.Rows[i]["RealName"].ToString().Trim();
                            tmpAddress.Zipcode = tb.Rows[i]["Zipcode"].ToString().Trim();
                            tmpAddress.Type = tb.Rows[i]["Type"].ToString().Trim();
                            UserDetailInfo.AddressRecords[i] = tmpAddress;
                        }
                    }
                }

                //评估信息---已经不存在
                UserDetailInfo.AssessmentInfos = null;
                //tb = ds.Tables[3];
                //if (tb != null)
                //{
                //    if (tb.Rows.Count > 0)
                //    {
                //        UserDetailInfo.AssessmentInfos = new AssessmentInfoRecord[tb.Rows.Count];
                //        for (int i = 0; i < tb.Rows.Count; i++)
                //        {
                //            AssessmentInfoRecord tmpAssessMent = new AssessmentInfoRecord();
                //            tmpAssessMent.Credit = long.Parse(tb.Rows[i]["Credit"].ToString().Trim());
                //            tmpAssessMent.CreditLevel = tb.Rows[i]["CreditLevel"].ToString().Trim();
                //            tmpAssessMent.Score = long.Parse(tb.Rows[i]["Score"].ToString().Trim());
                //            tmpAssessMent.AvailableScore = long.Parse(tb.Rows[i]["AvailableScore"].ToString().Trim());
                //            UserDetailInfo.AssessmentInfos[i] = tmpAssessMent;
                //        }
                //    }
                //}

                //订购记录--已经不存在
                SubscriptionRecords = null;
                //tb = ds.Tables[4];
                //if (tb != null)
                //{
                //    if (tb.Rows.Count > 0)
                //    {
                //        SubscriptionRecords = new SubscriptionRecord[tb.Rows.Count];
                //        for (int i = 0; i < tb.Rows.Count; i++)
                //        {
                //            SubscriptionRecord tmpSub = new SubscriptionRecord();
                //            tmpSub.CustID = tb.Rows[i]["CustID"].ToString().Trim();
                //            tmpSub.EndTime = DateTime.Parse(tb.Rows[i]["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                //            tmpSub.ServiceID = tb.Rows[i]["ServiceID"].ToString().Trim();
                //            tmpSub.ServiceName = tb.Rows[i]["ServiceName"].ToString().Trim();
                //            tmpSub.StartTime = DateTime.Parse(tb.Rows[i]["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                //            tmpSub.SubscribeStyle = tb.Rows[i]["SubscribeStyle"].ToString().Trim();
                //            tmpSub.TransactionID = tb.Rows[i]["TransactionID"].ToString().Trim();
                //            tmpSub.UserAccount = tb.Rows[i]["UserAccount"].ToString().Trim();
                //            SubscriptionRecords[i] = tmpSub;
                //        }

                //    }
                //}


                tb = ds.Tables[3];
                //认证方式信息
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        //for (int i = 0; i < tb.Rows.Count; i++)
                        //{
                        //    if (tb.Rows[i]["AuthenType"].ToString().Trim() == "1") //用户名
                        //        UserName = tb.Rows[i]["AuthenName"].ToString().Trim();
                        //    else if (tb.Rows[i]["AuthenType"].ToString().Trim() == "1") //手机
                        //        MobilePhone = tb.Rows[i]["AuthenName"].ToString().Trim();
                        //}
                    }
                }
                StringBuilder sm = new StringBuilder();
                //<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><UserExtendInfo><MobilePhone></MobilePhone><UserName>moomoofarm</UserName></UserExtendInfo></Root>
                sm.Append(@"<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><UserExtendInfo><MobilePhone>");
                sm.Append(MobilePhone);
                sm.Append(@"</MobilePhone><UserName>");
                sm.Append(UserName);
                sm.Append(@"</UserName></UserExtendInfo></Root>");
                UserDetailInfo.ExtendField = sm.ToString();

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }
        public static int UserInfoQueryV3(string ProvinceID, string SPID, string UserAccount, string CustID,
             string PhoneNum, string Password, out string ErrMsg, out UserInfo UserDetailInfo, out SubscriptionRecord[] SubscriptionRecords)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            UserDetailInfo = null;
            SubscriptionRecords = null;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_OV3_Interface_UserInfoQuery_3";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = (UserAccount == null ? "" : UserAccount);
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = (CustID == null ? "" : CustID);
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = (PhoneNum == null ? "" : PhoneNum);
                cmd.Parameters.Add(parPhoneNum);


                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 6);
                parPassword.Value = Password;
                cmd.Parameters.Add(parPassword);

                SqlParameter parEncryptedPassword = new SqlParameter("@EncryptedPassword", SqlDbType.VarChar, 50);
                parEncryptedPassword.Value = (CommonUtility.IsEmpty(Password) ? "" : CryptographyUtil.Encrypt(Password));
                cmd.Parameters.Add(parEncryptedPassword);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                //生成四个表
                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                //特殊错误描述代表激活码验证成功
                if (Result != 0 || ErrMsg == "000")
                    return Result;

                if (ds == null)
                {
                    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                    ErrMsg = "无用户记录";
                    return Result;
                }

                UserDetailInfo = new UserInfo();

                string UserName = "";
                string MobilePhone = "";

                DataTable tb = new DataTable();
                tb = ds.Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        string UserType1 = tb.Rows[0]["CustType"].ToString().Trim();
                        #region 判断是否是特殊的SPID

                        string OldSPID = ConfigurationManager.AppSettings["OldType_SPID"];

                        int SIP = OldSPID.IndexOf(SPID);

                        if (SIP >= 0)
                        {
                            // userType = "";
                            switch (UserType1)
                            {
                                case "14":
                                    UserType1 = "01";
                                    break;
                                case "20":
                                    UserType1 = "02";
                                    break;
                                case "12":
                                    UserType1 = "03";
                                    break;
                                case "90":
                                    UserType1 = "09";
                                    break;
                                case "30":
                                    UserType1 = "11";
                                    break;
                                case "42":
                                    UserType1 = "00";
                                    break;
                                default:
                                    UserType1 = "90";
                                    break;
                            }

                        }

                        #endregion
                        UserDetailInfo.UserType = UserType1;
                        UserDetailInfo.UserAccount = tb.Rows[0]["CardID"].ToString().Trim();
                        UserDetailInfo.Password = "";
                        UserDetailInfo.CustID = tb.Rows[0]["CustID"].ToString().Trim();
                        UserDetailInfo.UProvinceID = tb.Rows[0]["ProvinceID"].ToString().Trim();
                        UserDetailInfo.AreaCode = tb.Rows[0]["AreaID"].ToString().Trim();
                        UserDetailInfo.Status = tb.Rows[0]["Status"].ToString().Trim();
                        UserDetailInfo.RealName = tb.Rows[0]["RealName"].ToString().Trim();
                        //UserDetailInfo.UserName = tb.Rows[0]["UserName"].ToString().Trim();
                        UserDetailInfo.OuterID = tb.Rows[0]["OuterID"].ToString().Trim();
                        UserName = tb.Rows[0]["UserName"].ToString().Trim();
                        UserDetailInfo.CertificateCode = tb.Rows[0]["CertificateCode"].ToString().Trim();
                        UserDetailInfo.CertificateType = tb.Rows[0]["CertificateType"].ToString().Trim();
                        if (!CommonUtility.IsEmpty(tb.Rows[0]["Birthday"].ToString()))
                        {
                            UserDetailInfo.Birthday = DateTime.Parse(tb.Rows[0]["Birthday"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                        }
                        else
                        {
                            UserDetailInfo.Birthday = "";
                        }
                        UserDetailInfo.Sex = tb.Rows[0]["Sex"].ToString().Trim();
                        UserDetailInfo.CustLevel = tb.Rows[0]["CustLevel"].ToString().Trim();
                        UserDetailInfo.EduLevel = tb.Rows[0]["EduLevel"].ToString().Trim();
                        UserDetailInfo.Favorite = tb.Rows[0]["Favorite"].ToString().Trim();
                        UserDetailInfo.IncomeLevel = tb.Rows[0]["IncomeLevel"].ToString().Trim();
                        UserDetailInfo.Email = tb.Rows[0]["Email"].ToString().Trim();
                        UserDetailInfo.EnterpriseID = tb.Rows[0]["EnterpriseID"].ToString().Trim();
                        UserDetailInfo.PaymentAccountType = "";
                        UserDetailInfo.PaymentAccount = "";
                        UserDetailInfo.PaymentAccountPassword = "";//tb.Rows[0]["PaymentAccountPassword"].ToString().Trim();
                        UserDetailInfo.CustContactTel = tb.Rows[0]["Phone"].ToString().Trim();
                        UserDetailInfo.IsPost = "";

                        if (!tb.Rows[0]["PhoneClass"].ToString().Equals("1"))
                        {
                            MobilePhone = tb.Rows[0]["Phone"].ToString().Trim();
                        }
                    }
                }

                //绑定电话
                tb = ds.Tables[1];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        UserDetailInfo.BoundPhoneRecords = new BoundPhoneRecord[tb.Rows.Count];
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            BoundPhoneRecord tmpBoundPhone = new BoundPhoneRecord();
                            tmpBoundPhone.Phone = tb.Rows[i]["Phone"].ToString().Trim();
                            UserDetailInfo.BoundPhoneRecords[i] = tmpBoundPhone;
                        }
                    }
                }

                //联系信息
                tb = ds.Tables[2];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        UserDetailInfo.AddressRecords = new AddressRecord[tb.Rows.Count];
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            AddressRecord tmpAddress = new AddressRecord();
                            tmpAddress.Address = tb.Rows[i]["Address"].ToString().Trim();
                            tmpAddress.ContactTel = tb.Rows[i]["Phone"].ToString().Trim();
                            tmpAddress.LinkMan = tb.Rows[i]["RealName"].ToString().Trim();
                            tmpAddress.Zipcode = tb.Rows[i]["Zipcode"].ToString().Trim();
                            tmpAddress.Type = tb.Rows[i]["Type"].ToString().Trim();
                            UserDetailInfo.AddressRecords[i] = tmpAddress;
                        }
                    }
                }

                //评估信息---已经不存在
                UserDetailInfo.AssessmentInfos = null;
                //tb = ds.Tables[3];
                //if (tb != null)
                //{
                //    if (tb.Rows.Count > 0)
                //    {
                //        UserDetailInfo.AssessmentInfos = new AssessmentInfoRecord[tb.Rows.Count];
                //        for (int i = 0; i < tb.Rows.Count; i++)
                //        {
                //            AssessmentInfoRecord tmpAssessMent = new AssessmentInfoRecord();
                //            tmpAssessMent.Credit = long.Parse(tb.Rows[i]["Credit"].ToString().Trim());
                //            tmpAssessMent.CreditLevel = tb.Rows[i]["CreditLevel"].ToString().Trim();
                //            tmpAssessMent.Score = long.Parse(tb.Rows[i]["Score"].ToString().Trim());
                //            tmpAssessMent.AvailableScore = long.Parse(tb.Rows[i]["AvailableScore"].ToString().Trim());
                //            UserDetailInfo.AssessmentInfos[i] = tmpAssessMent;
                //        }
                //    }
                //}

                //订购记录--已经不存在
                SubscriptionRecords = null;
                //tb = ds.Tables[4];
                //if (tb != null)
                //{
                //    if (tb.Rows.Count > 0)
                //    {
                //        SubscriptionRecords = new SubscriptionRecord[tb.Rows.Count];
                //        for (int i = 0; i < tb.Rows.Count; i++)
                //        {
                //            SubscriptionRecord tmpSub = new SubscriptionRecord();
                //            tmpSub.CustID = tb.Rows[i]["CustID"].ToString().Trim();
                //            tmpSub.EndTime = DateTime.Parse(tb.Rows[i]["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                //            tmpSub.ServiceID = tb.Rows[i]["ServiceID"].ToString().Trim();
                //            tmpSub.ServiceName = tb.Rows[i]["ServiceName"].ToString().Trim();
                //            tmpSub.StartTime = DateTime.Parse(tb.Rows[i]["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                //            tmpSub.SubscribeStyle = tb.Rows[i]["SubscribeStyle"].ToString().Trim();
                //            tmpSub.TransactionID = tb.Rows[i]["TransactionID"].ToString().Trim();
                //            tmpSub.UserAccount = tb.Rows[i]["UserAccount"].ToString().Trim();
                //            SubscriptionRecords[i] = tmpSub;
                //        }

                //    }
                //}


                tb = ds.Tables[3];
                //认证方式信息
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        //for (int i = 0; i < tb.Rows.Count; i++)
                        //{
                        //    if (tb.Rows[i]["AuthenType"].ToString().Trim() == "1") //用户名
                        //        UserName = tb.Rows[i]["AuthenName"].ToString().Trim();
                        //    else if (tb.Rows[i]["AuthenType"].ToString().Trim() == "1") //手机
                        //        MobilePhone = tb.Rows[i]["AuthenName"].ToString().Trim();
                        //}
                    }
                }
                StringBuilder sm = new StringBuilder();
                //<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><UserExtendInfo><MobilePhone></MobilePhone><UserName>moomoofarm</UserName></UserExtendInfo></Root>
                sm.Append(@"<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><UserExtendInfo><MobilePhone>");
                sm.Append(MobilePhone);
                sm.Append(@"</MobilePhone><UserName>");
                sm.Append(UserName);
                sm.Append(@"</UserName></UserExtendInfo></Root>");
                UserDetailInfo.ExtendField = sm.ToString();

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }
        /// <summary>
        /// 客户信息查询接口
        /// 作者：周涛   时间：2009-8-14
        /// </summary>
        /// <param name="ProvinceID"></param>
        /// <param name="SPID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="CustID"></param>
        /// <param name="CertificateCode"></param>
        /// <param name="CertificateType"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="UserDetailInfo"></param>
        /// <returns></returns>
        public static int UserInfoQueryV2(string ProvinceID, string SPID, string UserAccount, string CustID,
            string PhoneNum, string Password, out string ErrMsg, out UserInfo UserDetailInfo, out SubscriptionRecord[] SubscriptionRecords)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            UserDetailInfo = null;
            SubscriptionRecords = null;

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "up_Customer_OV3_Interface_UserInfoQuery_2";

                SqlParameter parProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 2);
                parProvinceID.Value = ProvinceID;
                cmd.Parameters.Add(parProvinceID);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = (UserAccount == null ? "" : UserAccount);
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = (CustID == null ? "" : CustID);
                cmd.Parameters.Add(parCustID);

                SqlParameter parPhoneNum = new SqlParameter("@PhoneNum", SqlDbType.VarChar, 20);
                parPhoneNum.Value = (PhoneNum == null ? "" : PhoneNum);
                cmd.Parameters.Add(parPhoneNum);


                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 6);
                parPassword.Value = Password;
                cmd.Parameters.Add(parPassword);

                SqlParameter parEncryptedPassword = new SqlParameter("@EncryptedPassword", SqlDbType.VarChar, 50);
                parEncryptedPassword.Value = (CommonUtility.IsEmpty(Password) ? "" : CryptographyUtil.Encrypt(Password));
                cmd.Parameters.Add(parEncryptedPassword);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                UserDetailInfo = new UserInfo();

                //生成四个表
                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                //特殊错误描述代表激活码验证成功
                if (Result != 0 || ErrMsg == "000")
                    return Result;

                if (ds == null)
                {
                    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                    ErrMsg = "无用户记录";
                    return Result;
                }

        

                string UserName = "";
                string MobilePhone = "";

                DataTable tb = new DataTable();
                tb = ds.Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        string UserType1 = tb.Rows[0]["CustType"].ToString().Trim();
                        #region 判断是否是特殊的SPID

                        string OldSPID = ConfigurationManager.AppSettings["OldType_SPID"];

                        int SIP = OldSPID.IndexOf(SPID);

                        if (SIP >= 0)
                        {
                            // userType = "";
                            switch (UserType1)
                            {
                                case "14":
                                    UserType1 = "01";
                                    break;
                                case "20":
                                    UserType1 = "02";
                                    break;
                                case "12":
                                    UserType1 = "03";
                                    break;
                                case "90":
                                    UserType1 = "09";
                                    break;
                                case "30":
                                    UserType1 = "11";
                                    break;
                                case "42":
                                    UserType1 = "00";
                                    break;
                                default:
                                    UserType1 = "90";
                                    break;
                            }

                        }

                        #endregion
                        UserDetailInfo.UserType = UserType1;
                        UserDetailInfo.UserAccount = tb.Rows[0]["CardID"].ToString().Trim();
                        UserDetailInfo.Password = "";
                        UserDetailInfo.CustID = tb.Rows[0]["CustID"].ToString().Trim();
                        UserDetailInfo.UProvinceID = tb.Rows[0]["ProvinceID"].ToString().Trim();
                        UserDetailInfo.AreaCode = tb.Rows[0]["AreaID"].ToString().Trim();
                        UserDetailInfo.Status = tb.Rows[0]["Status"].ToString().Trim();
                        UserDetailInfo.RealName = tb.Rows[0]["RealName"].ToString().Trim();
                        //UserDetailInfo.UserName = tb.Rows[0]["UserName"].ToString().Trim();
                        UserName = tb.Rows[0]["UserName"].ToString().Trim();
                        UserDetailInfo.CertificateCode = tb.Rows[0]["CertificateCode"].ToString().Trim();
                        UserDetailInfo.CertificateType = tb.Rows[0]["CertificateType"].ToString().Trim();
                        if (!CommonUtility.IsEmpty(tb.Rows[0]["Birthday"].ToString()))
                        {
                            UserDetailInfo.Birthday = DateTime.Parse(tb.Rows[0]["Birthday"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                        }
                        else
                        {
                            UserDetailInfo.Birthday = "";
                        }
                        UserDetailInfo.Sex = tb.Rows[0]["Sex"].ToString().Trim();
                        UserDetailInfo.CustLevel = tb.Rows[0]["CustLevel"].ToString().Trim();
                        UserDetailInfo.EduLevel = tb.Rows[0]["EduLevel"].ToString().Trim();
                        UserDetailInfo.Favorite = tb.Rows[0]["Favorite"].ToString().Trim();
                        UserDetailInfo.IncomeLevel = tb.Rows[0]["IncomeLevel"].ToString().Trim();
                        UserDetailInfo.Email = tb.Rows[0]["Email"].ToString().Trim();
                        UserDetailInfo.EnterpriseID = tb.Rows[0]["EnterpriseID"].ToString().Trim();
                        UserDetailInfo.PaymentAccountType = "";
                        UserDetailInfo.PaymentAccount = "";
                        UserDetailInfo.PaymentAccountPassword = "";//tb.Rows[0]["PaymentAccountPassword"].ToString().Trim();
                        UserDetailInfo.CustContactTel = tb.Rows[0]["Phone"].ToString().Trim();
                        UserDetailInfo.IsPost = "";

                        if (!tb.Rows[0]["PhoneClass"].ToString().Equals("1"))
                        {
                            MobilePhone = tb.Rows[0]["Phone"].ToString().Trim();
                        }
                    }
                }

                //绑定电话
                tb = ds.Tables[1];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        UserDetailInfo.BoundPhoneRecords = new BoundPhoneRecord[tb.Rows.Count];
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            BoundPhoneRecord tmpBoundPhone = new BoundPhoneRecord();
                            tmpBoundPhone.Phone = tb.Rows[i]["Phone"].ToString().Trim();
                            UserDetailInfo.BoundPhoneRecords[i] = tmpBoundPhone;
                        }
                    }
                }

                //联系信息
                tb = ds.Tables[2];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        UserDetailInfo.AddressRecords = new AddressRecord[tb.Rows.Count];
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            AddressRecord tmpAddress = new AddressRecord();
                            tmpAddress.Address = tb.Rows[i]["Address"].ToString().Trim();
                            tmpAddress.ContactTel = tb.Rows[i]["Phone"].ToString().Trim();
                            tmpAddress.LinkMan = tb.Rows[i]["RealName"].ToString().Trim();
                            tmpAddress.Zipcode = tb.Rows[i]["Zipcode"].ToString().Trim();
                            tmpAddress.Type = tb.Rows[i]["Type"].ToString().Trim();
                            UserDetailInfo.AddressRecords[i] = tmpAddress;
                        }
                    }
                }

                //评估信息---已经不存在
                UserDetailInfo.AssessmentInfos = null;
                //tb = ds.Tables[3];
                //if (tb != null)
                //{
                //    if (tb.Rows.Count > 0)
                //    {
                //        UserDetailInfo.AssessmentInfos = new AssessmentInfoRecord[tb.Rows.Count];
                //        for (int i = 0; i < tb.Rows.Count; i++)
                //        {
                //            AssessmentInfoRecord tmpAssessMent = new AssessmentInfoRecord();
                //            tmpAssessMent.Credit = long.Parse(tb.Rows[i]["Credit"].ToString().Trim());
                //            tmpAssessMent.CreditLevel = tb.Rows[i]["CreditLevel"].ToString().Trim();
                //            tmpAssessMent.Score = long.Parse(tb.Rows[i]["Score"].ToString().Trim());
                //            tmpAssessMent.AvailableScore = long.Parse(tb.Rows[i]["AvailableScore"].ToString().Trim());
                //            UserDetailInfo.AssessmentInfos[i] = tmpAssessMent;
                //        }
                //    }
                //}

                //订购记录--已经不存在
                SubscriptionRecords = null;
                //tb = ds.Tables[4];
                //if (tb != null)
                //{
                //    if (tb.Rows.Count > 0)
                //    {
                //        SubscriptionRecords = new SubscriptionRecord[tb.Rows.Count];
                //        for (int i = 0; i < tb.Rows.Count; i++)
                //        {
                //            SubscriptionRecord tmpSub = new SubscriptionRecord();
                //            tmpSub.CustID = tb.Rows[i]["CustID"].ToString().Trim();
                //            tmpSub.EndTime = DateTime.Parse(tb.Rows[i]["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                //            tmpSub.ServiceID = tb.Rows[i]["ServiceID"].ToString().Trim();
                //            tmpSub.ServiceName = tb.Rows[i]["ServiceName"].ToString().Trim();
                //            tmpSub.StartTime = DateTime.Parse(tb.Rows[i]["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                //            tmpSub.SubscribeStyle = tb.Rows[i]["SubscribeStyle"].ToString().Trim();
                //            tmpSub.TransactionID = tb.Rows[i]["TransactionID"].ToString().Trim();
                //            tmpSub.UserAccount = tb.Rows[i]["UserAccount"].ToString().Trim();
                //            SubscriptionRecords[i] = tmpSub;
                //        }

                //    }
                //}


                tb = ds.Tables[3];
                //认证方式信息
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        //for (int i = 0; i < tb.Rows.Count; i++)
                        //{
                        //    if (tb.Rows[i]["AuthenType"].ToString().Trim() == "1") //用户名
                        //        UserName = tb.Rows[i]["AuthenName"].ToString().Trim();
                        //    else if (tb.Rows[i]["AuthenType"].ToString().Trim() == "1") //手机
                        //        MobilePhone = tb.Rows[i]["AuthenName"].ToString().Trim();
                        //}
                    }
                }
                StringBuilder sm = new StringBuilder();
                //<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><UserExtendInfo><MobilePhone></MobilePhone><UserName>moomoofarm</UserName></UserExtendInfo></Root>
                sm.Append(@"<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><UserExtendInfo><MobilePhone>");
                sm.Append(MobilePhone);
                sm.Append(@"</MobilePhone><UserName>");
                sm.Append(UserName);
                sm.Append(@"</UserName></UserExtendInfo></Root>");
                UserDetailInfo.ExtendField = sm.ToString();

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }


        //根据UserAccount 得到客户信息
        public static UserInfo getUserInfoByUserAccount(out string ErrMsg, string UserAccount)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            UserInfo UserDetailInfo = new UserInfo();
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();


            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandText = "up_Customer_OV3_Interface_GetUserInfoByUserAccount";
                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = UserAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                DataSet ds = DBUtility.FillData(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();

                if (Result != 0 || ErrMsg == "000")
                    return UserDetailInfo;

                if (ds == null)
                {
                    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                    ErrMsg = "无用户记录";
                    return UserDetailInfo;
                }
                if (ds.Tables.Count < 1)
                {
                    Result = ErrorDefinition.IError_Result_UserNotExist_Code;
                    ErrMsg = "无用户记录";
                    return UserDetailInfo;
                }

                //UserDetailInfo = new UserInfo();

                DataTable tb = new DataTable();
                tb = ds.Tables[0];

                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        UserDetailInfo.UserType = tb.Rows[0]["UserType"].ToString().Trim();
                        UserDetailInfo.UserAccount = tb.Rows[0]["UserAccount"].ToString().Trim();
                        UserDetailInfo.Password = "";
                        UserDetailInfo.CustID = tb.Rows[0]["CustID"].ToString().Trim();
                        UserDetailInfo.UProvinceID = tb.Rows[0]["UProvinceID"].ToString().Trim();
                        UserDetailInfo.AreaCode = tb.Rows[0]["AreaCode"].ToString().Trim();
                        UserDetailInfo.Status = tb.Rows[0]["Status"].ToString().Trim();
                        UserDetailInfo.RealName = tb.Rows[0]["RealName"].ToString().Trim();
                        UserDetailInfo.CertificateCode = tb.Rows[0]["CertificateCode"].ToString().Trim();
                        UserDetailInfo.CertificateType = tb.Rows[0]["CertificateType"].ToString().Trim();
                        UserDetailInfo.Birthday = DateTime.Parse(tb.Rows[0]["Birthday"].ToString()).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                        UserDetailInfo.Sex = tb.Rows[0]["Sex"].ToString().Trim();
                        UserDetailInfo.CustLevel = tb.Rows[0]["CustLevel"].ToString().Trim();
                        UserDetailInfo.EduLevel = tb.Rows[0]["EduLevel"].ToString().Trim();
                        UserDetailInfo.Favorite = tb.Rows[0]["Favorite"].ToString().Trim();
                        UserDetailInfo.IncomeLevel = tb.Rows[0]["IncomeLevel"].ToString().Trim();
                        UserDetailInfo.Email = tb.Rows[0]["Email"].ToString().Trim();
                        UserDetailInfo.PaymentAccountType = tb.Rows[0]["PaymentAccountType"].ToString().Trim();
                        UserDetailInfo.PaymentAccount = tb.Rows[0]["PaymentAccount"].ToString().Trim();
                        UserDetailInfo.PaymentAccountPassword = "";//tb.Rows[0]["PaymentAccountPassword"].ToString().Trim();
                        
                        UserDetailInfo.CustContactTel = tb.Rows[0]["Phone"].ToString().Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return UserDetailInfo;

        }



        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="RegistrationStyle"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int ModifyUserInfo(string SPID,out string ErrMsg)
        {
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";
            
            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "up_BT_V2_Interface_ModifyUserCommonInfo";
                cmd.CommandText = "[up_Customer_OV3_Interface_ModifyUserCommonInfo]";


                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = userType;
                cmd.Parameters.Add(parUserType);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = userAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 50);
                parPassword.Value = CryptographyUtil.Encrypt(password);
                cmd.Parameters.Add(parPassword);




                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = custID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = uProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 3);
                parAreaCode.Value = areaCode;
                cmd.Parameters.Add(parAreaCode);


                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Value = status;
                cmd.Parameters.Add(parStatus);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = realName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 20);
                parCertificateCode.Value = certificateCode;
                cmd.Parameters.Add(parCertificateCode);


                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar, 2);
                parCertificateType.Value = certificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 19);
                parBirthday.Value = birthday;
                cmd.Parameters.Add(parBirthday);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Value = custLevel;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parEduLevel = new SqlParameter("@EduLevel", SqlDbType.VarChar, 2);
                parEduLevel.Value = eduLevel;
                cmd.Parameters.Add(parEduLevel);

                SqlParameter parFavorite = new SqlParameter("@Favorite", SqlDbType.VarChar, 256);
                parFavorite.Value = favorite;
                cmd.Parameters.Add(parFavorite);

                SqlParameter parIncomeLevel = new SqlParameter("@IncomeLevel", SqlDbType.VarChar, 2);
                parIncomeLevel.Value = incomeLevel;
                cmd.Parameters.Add(parIncomeLevel);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Value = email;
                cmd.Parameters.Add(parEmail);

                SqlParameter parPaymentAccountType = new SqlParameter("@PaymentAccountType", SqlDbType.VarChar, 2);
                parPaymentAccountType.Value = paymentAccountType;
                cmd.Parameters.Add(parPaymentAccountType);

                SqlParameter parPaymentAccount = new SqlParameter("@PaymentAccount", SqlDbType.VarChar, 50);
                parPaymentAccount.Value = paymentAccount;
                cmd.Parameters.Add(parPaymentAccount);

                SqlParameter parPaymentAccountPassword = new SqlParameter("@PaymentAccountPassword", SqlDbType.VarChar, 50);
                parPaymentAccountPassword.Value = paymentAccountPassword;
                cmd.Parameters.Add(parPaymentAccountPassword);

                SqlParameter parContactTel = new SqlParameter("@CustContactTel", SqlDbType.VarChar, 20);
                parContactTel.Value = custContactTel;
                cmd.Parameters.Add(parContactTel);

                //SqlParameter parEnterPriseID = new SqlParameter("@EnterPriseID", SqlDbType.VarChar, 50);  remarked by lht
                //parEnterPriseID.Value = enterpriseID;
                //cmd.Parameters.Add(parEnterPriseID);

                //SqlParameter parRegistrationSource = new SqlParameter("@RegistrationSource", SqlDbType.VarChar, 2); remarked by lht
                //parRegistrationSource.Value = RegistrationStyle;
                //cmd.Parameters.Add(parRegistrationSource);

                SqlParameter parIsPost = new SqlParameter("@IsPost", SqlDbType.VarChar, 2);
                parIsPost.Value = (isPost == null ? "" : isPost);
                cmd.Parameters.Add(parIsPost);

                SqlParameter parBoundPhoneRecords = new SqlParameter("@BoundPhoneRecords ", SqlDbType.Text);
                string xmlBoundPhoneRecords = this.GenerateXmlForBoundPhoneRecords();

                if (xmlBoundPhoneRecords == "")
                    parBoundPhoneRecords.Value = DBNull.Value;
                else
                    parBoundPhoneRecords.Value = xmlBoundPhoneRecords;

                cmd.Parameters.Add(parBoundPhoneRecords);

                SqlParameter parAddressRecords = new SqlParameter("@AddressRecords", SqlDbType.Text);

                string xmlAddressRecords = this.GenerateXmlForAddressRecords();
                if (xmlAddressRecords == "")
                    parAddressRecords.Value = DBNull.Value;
                else
                    parAddressRecords.Value = xmlAddressRecords;
                cmd.Parameters.Add(parAddressRecords);

                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                //SqlParameter parOCustID = new SqlParameter("@oCustID ", SqlDbType.VarChar, 16);
                //parOCustID.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parOCustID);

                //SqlParameter parOUserAccount = new SqlParameter("@oUserAccount ", SqlDbType.VarChar, 16);
                //parOUserAccount.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parOUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                //custID = parOCustID.Value.ToString();
                //userAccount = parOUserAccount.Value.ToString();
            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// 用户注册
        /// 修改：周涛      时间：2009-09-01
        /// </summary>
        /// <param name="RegistrationStyle"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public int UserInfoRegistry(HttpContext SpecificContext, string RegistrationStyle, string SPID, out string ErrMsg)
        {

            //如果phone不为空时，手机为认证手机，其它号码均为一般电话
            int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
            ErrMsg = "";

            string userName = CommonBizRules.GetValueFromXmlStr(ExtendField, "UserName");
            string Phone = CommonBizRules.GetValueFromXmlStr(ExtendField, "MobilePhone");

            string UserNameSPID = System.Configuration.ConfigurationManager.AppSettings["UserName_SPID"];

            int SIP = UserNameSPID.IndexOf(SPID);
            if (SIP < 0)
            {
                if (CommonUtility.IsEmpty(userName))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    ErrMsg = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，用户名不能为空";
                    return Result;
                }
            }

            if(!CommonUtility.IsEmpty(userName))
            {
                //判断用户名是否存在
                if (CustBasicInfo.IsExistUser(userName) != 0)
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    ErrMsg = "用户名已经存在";
                    return Result;
                }
                
            }




            bool tmp = false;
            if (!CommonUtility.IsEmpty(Phone))
            {
                string phone1 = "";
                if (!CommonBizRules.PhoneNumValid(SpecificContext,Phone,out phone1))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    ErrMsg = "绑定电话无效";
                    return Result;
                }
                Phone = phone1;

                BasicInfoV2Record[] asicInfoV2Records = PhoneBO.GetQueryByPhone(SPID, Phone, out Result, out ErrMsg);
                if(asicInfoV2Records != null)
                {
                    if(asicInfoV2Records.Length > 0)
                    {
                        foreach (BasicInfoV2Record basicinfo in asicInfoV2Records)
                        {
                            if(basicinfo.PhoneClass != "1")
                            {
                                tmp = true;
                                break;
                            }

                        }
                    }
                }
                
                if(tmp)
                {
                    Result = -30005;
                    ErrMsg = "该手机号码已经被其它客户绑定";
                    return Result;
                }


            }

            //移百手机注册
            //Phone  作为认证电话
            //if(RegistrationStyle == "05")
            //{
                
            //}


            if (RegistrationStyle == "05")
            {
                if (CommonUtility.IsEmpty(Phone))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                    ErrMsg = "手机号不能为空";
                    return Result;

                }

                Result = PhoneBO.PhoneSel("", Phone, out ErrMsg);
                if(Result != 0)
                {
                    ErrMsg = "该手机号码已经被其他用户绑定";
                    return Result;
                }

            }

            if (RegistrationStyle == "06")
            {
                if (CommonUtility.IsEmpty(userName))
                {
                    Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                    ErrMsg = "用户名不能为空";
                    return Result;

                }
            }

            if(userAccount != "")
            {
                if(userAccount.Length != 9 && userAccount.Length != 16)
                {
                    result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                    ErrMsg = "商旅卡长度不正确";
                    return Result;
                }
            }
       

            SqlConnection myCon = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
                cmd.Connection = myCon;
                cmd.CommandType = CommandType.StoredProcedure;
                //if (RegistrationStyle == "05" || RegistrationStyle == "06")
                //    cmd.CommandText = "up_BT_V2_Interface_PersonalUserNameUserRegistry";
                //else
                //    cmd.CommandText = "up_BT_V2_Interface_InsertUserCommonInfo";
                cmd.CommandText = "up_Customer_OV3_Interface_UserInfoRegistry";

                //SqlParameter parRegistrationStyle = new SqlParameter("@RegistrationStyle", SqlDbType.VarChar, 2);
                //parRegistrationStyle.Value = RegistrationStyle;
                //cmd.Parameters.Add(cmd);

                SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
                parSPID.Value = SPID;
                cmd.Parameters.Add(parSPID);

                SqlParameter parUserType = new SqlParameter("@UserType", SqlDbType.VarChar, 2);
                parUserType.Value = userType;
                cmd.Parameters.Add(parUserType);

                SqlParameter parUserAccount = new SqlParameter("@UserAccount", SqlDbType.VarChar, 16);
                parUserAccount.Value = CommonUtility.IsEmpty(userAccount) ? "" : userAccount;
                cmd.Parameters.Add(parUserAccount);

                SqlParameter parPassword = new SqlParameter("@Password", SqlDbType.VarChar, 6);
                parPassword.Value = CommonUtility.IsEmpty(password) ? "" : password;
                cmd.Parameters.Add(parPassword);

                SqlParameter parEncryptPassword = new SqlParameter("@EncryptPassword", SqlDbType.VarChar, 50);
                parEncryptPassword.Value = CryptographyUtil.Encrypt(password);
                cmd.Parameters.Add(parEncryptPassword);

                SqlParameter parCustID = new SqlParameter("@CustID", SqlDbType.VarChar, 16);
                parCustID.Value = CommonUtility.IsEmpty(CustID) ? "" : CustID;
                cmd.Parameters.Add(parCustID);

                SqlParameter parUProvinceID = new SqlParameter("@UProvinceID", SqlDbType.VarChar, 2);
                parUProvinceID.Value = uProvinceID;
                cmd.Parameters.Add(parUProvinceID);

                SqlParameter parAreaCode = new SqlParameter("@AreaCode", SqlDbType.VarChar, 6);
                parAreaCode.Value = areaCode;
                cmd.Parameters.Add(parAreaCode);


                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                parStatus.Value = status;
                cmd.Parameters.Add(parStatus);

                SqlParameter parRealName = new SqlParameter("@RealName", SqlDbType.VarChar, 30);
                parRealName.Value = realName;
                cmd.Parameters.Add(parRealName);

                SqlParameter parCertificateCode = new SqlParameter("@CertificateCode", SqlDbType.VarChar, 30);
                parCertificateCode.Value = CommonUtility.IsEmpty(certificateCode) ? "" : certificateCode;
                cmd.Parameters.Add(parCertificateCode);


                SqlParameter parCertificateType = new SqlParameter("@CertificateType", SqlDbType.VarChar,2);
                parCertificateType.Value = CommonUtility.IsEmpty(CertificateType) ? "" : CertificateType;
                cmd.Parameters.Add(parCertificateType);

                SqlParameter parBirthday = new SqlParameter("@Birthday", SqlDbType.VarChar, 19);
                parBirthday.Value = CommonUtility.IsEmpty(Birthday) ? "" : Birthday;
                cmd.Parameters.Add(parBirthday);

                SqlParameter parSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
                parSex.Value = CommonUtility.IsEmpty(Sex) ? "" : Sex;
                cmd.Parameters.Add(parSex);

                SqlParameter parCustLevel = new SqlParameter("@CustLevel", SqlDbType.VarChar, 1);
                parCustLevel.Value = CommonUtility.IsEmpty(CustLevel) ? "" : CustLevel;
                cmd.Parameters.Add(parCustLevel);

                SqlParameter parEduLevel = new SqlParameter("@EduLevel", SqlDbType.VarChar, 2);
                parEduLevel.Value = CommonUtility.IsEmpty(EduLevel) ? "" : EduLevel;
                cmd.Parameters.Add(parEduLevel);

                if(CommonUtility.IsEmpty(favorite))
                {
                    SqlParameter parFavorite = new SqlParameter("@Favorite", SqlDbType.VarChar, 256);
                    parFavorite.Value = DBNull.Value;
                    cmd.Parameters.Add(parFavorite);
                }
                else
                {
                    SqlParameter parFavorite = new SqlParameter("@Favorite", SqlDbType.VarChar, 256);
                    parFavorite.Value = favorite;
                    cmd.Parameters.Add(parFavorite);
                }

                SqlParameter parIncomeLevel = new SqlParameter("@IncomeLevel", SqlDbType.VarChar, 2);
                parIncomeLevel.Value = CommonUtility.IsEmpty(IncomeLevel) ? "" : IncomeLevel;
                cmd.Parameters.Add(parIncomeLevel);

                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 100);
                parEmail.Value = CommonUtility.IsEmpty(Email) ? "" : Email;
                cmd.Parameters.Add(parEmail);


                //SqlParameter parPaymentAccountType = new SqlParameter("@PaymentAccountType", SqlDbType.VarChar, 2);
                //parPaymentAccountType.Value = CommonUtility.IsEmpty(PaymentAccountType) ? "" : PaymentAccountType;
                //cmd.Parameters.Add(parPaymentAccountType);


                ////系统改造，原PaymentAccount字段含义废弃， 用PaymentAccount参数传送用户名
                //SqlParameter parPaymentAccount = new SqlParameter("@PaymentAccount", SqlDbType.VarChar, 50);
                //parPaymentAccount.Value = CommonUtility.IsEmpty(Phone) ? "" : Phone;
                //cmd.Parameters.Add(parPaymentAccount);


                ////系统改造，原EnterPriseID字段含义废弃， 用EnterPriseID参数传送手机号
                //SqlParameter parEnterPriseID = new SqlParameter("@EnterPriseID", SqlDbType.VarChar, 30);
                //parEnterPriseID.Value = CommonUtility.IsEmpty(userName) ? "" : userName;
                //cmd.Parameters.Add(parEnterPriseID);

                //SqlParameter parPaymentAccountPassword = new SqlParameter("@PaymentAccountPassword", SqlDbType.VarChar, 50);
                //parPaymentAccountPassword.Value = CryptographyUtil.Encrypt(paymentAccountPassword);
                //cmd.Parameters.Add(parPaymentAccountPassword);

                //SqlParameter parContactTel = new SqlParameter("@CustContactTel", SqlDbType.VarChar, 20);
                //parContactTel.Value = CommonUtility.IsEmpty(custContactTel) ? "" : custContactTel;
                //cmd.Parameters.Add(parContactTel);


                SqlParameter parRegistrationSource = new SqlParameter("@RegistrationSource", SqlDbType.VarChar, 2);
                parRegistrationSource.Value = RegistrationStyle;
                cmd.Parameters.Add(parRegistrationSource);

                SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 30);
                parUserName.Value = CommonUtility.IsEmpty(userName) ? "" : userName;
                cmd.Parameters.Add(parUserName);

                //SqlParameter parIsPost = new SqlParameter("@IsPost", SqlDbType.VarChar, 2);
                //parIsPost.Value = CommonUtility.IsEmpty(isPost) ? "" : isPost;
                //cmd.Parameters.Add(parIsPost);

                //SqlParameter parBoundPhoneRecords = new SqlParameter("@BoundPhoneRecords ", SqlDbType.Text);
                //string xmlBoundPhoneRecords = this.GenerateXmlForBoundPhoneRecords();

                //if (xmlBoundPhoneRecords == "")
                //    parBoundPhoneRecords.Value = DBNull.Value;
                //else
                //    parBoundPhoneRecords.Value = xmlBoundPhoneRecords;

                //cmd.Parameters.Add(parBoundPhoneRecords);

                //SqlParameter parAddressRecords = new SqlParameter("@AddressRecords", SqlDbType.Text);

                //string xmlAddressRecords = this.GenerateXmlForAddressRecords();
                //if (xmlAddressRecords == "")
                //    parAddressRecords.Value = DBNull.Value;
                //else
                //    parAddressRecords.Value = xmlAddressRecords;
                //cmd.Parameters.Add(parAddressRecords);



                SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
                parResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parResult);

                SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
                parErrMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parErrMsg);

                SqlParameter parOCustID = new SqlParameter("@oCustID ", SqlDbType.VarChar, 16);
                parOCustID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOCustID);

                SqlParameter parOUserAccount = new SqlParameter("@oUserAccount ", SqlDbType.VarChar, 16);
                parOUserAccount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parOUserAccount);

                DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

                Result = int.Parse(parResult.Value.ToString());
                ErrMsg = parErrMsg.Value.ToString();
                custID = parOCustID.Value.ToString();
                userAccount = parOUserAccount.Value.ToString();

                if(Result != 0)
                {
                    return Result;
                }

                bool temp = false;

                //绑定电话（一般电话）
                if (boundPhoneRecords != null)
                {
                    if (boundPhoneRecords.Length > 0)
                    {
                        for (int i = 0; i < boundPhoneRecords.Length; i++)
                        {
                            Result = PhoneBO.PhoneSet(SPID, custID, boundPhoneRecords[i].Phone, "1", "2", out ErrMsg);

                            if(boundPhoneRecords[i].Phone.ToString().Equals(CustContactTel))
                            {
                                temp = true;
                            }
                        }
                    }
                }

                //如果CustContactTel和boundPhoneRecords不同，插入绑定电话表(一般电话)
                if(!CommonUtility.IsEmpty(CustContactTel))
                {
                    if (!temp)
                    {
                        Result = PhoneBO.PhoneSet(SPID, custID, CustContactTel, "1", "2", out ErrMsg);
                    }
                }


                //如果Phone不为空，绑定电话（认证电话）
                if (!CommonUtility.IsEmpty(Phone))
                {
                    Result = PhoneBO.PhoneSet(SPID, custID, Phone, "2", "2", out ErrMsg);
                }
                int Address_Result;
                string Address_ErrMsg = "";
                //客户地址插入
                if(addressRecords != null)
                {
                    if(addressRecords.Length > 0)
                    {
                        AddressInfoRecord[] AddressInfoRecords = new AddressInfoRecord[addressRecords.Length];
                        for (int i = 0; i < addressRecords.Length; i++)
                        {
                            AddressInfoRecords[i] = new AddressInfoRecord();
                            AddressInfoRecords[i].Address = addressRecords[i].Address;
                            AddressInfoRecords[i].Zipcode = addressRecords[i].Zipcode;
                            AddressInfoRecords[i].Type = addressRecords[i].Type;
                            AddressInfoRecords[i].DealType = "0";
                        }

                        Address_Result =AddressInfoBO.UploadAddressInfo(SPID, AddressInfoRecords, custID, out Address_ErrMsg);
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
                ErrMsg = ex.Message;
            }

            return Result;
        }

        /// <summary>
        /// 生成绑定电话的xml字符串
        /// </summary>
        /// <param name="BoundPhoneRecords"></param>
        /// <returns></returns>
        /// 
        public string GenerateXmlForBoundPhoneRecords()
        {
            string Result = "";

            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem;
            XmlElement xmlelem2;
            XmlElement xmlelem3;
            XmlText xmltext;
            try
            {
                xmldoc = new XmlDocument();
                //加入XML的声明段落
                xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);
                //加入一个根元素
                xmlelem = xmldoc.CreateElement("", "ROOT", "");
                xmldoc.AppendChild(xmlelem);

                for (int i = 0; i < boundPhoneRecords.Length; i++)
                {
                    //加入另外一个元素
                    xmlelem2 = xmldoc.CreateElement("BoundPhoneRecord");
                    xmlelem2 = xmldoc.CreateElement("", "BoundPhoneRecord", "");

                    xmlelem3 = xmlelem3 = xmldoc.CreateElement("", "Phone", "");
                    xmltext = xmldoc.CreateTextNode(boundPhoneRecords[i].Phone.ToString());
                    xmlelem3.AppendChild(xmltext);
                    xmlelem2.AppendChild(xmlelem3);
                    xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
                }
                //保存创建好的XML文档

                //xmldoc.Save("c:\\BountPhoneRecord.xml");
                Result = xmldoc.OuterXml;

            }
            catch { }

            return Result;

        }

        /// <summary>
        /// 生成地址记录xml字符串
        /// </summary>
        /// <returns></returns>
        public string GenerateXmlForAddressRecords()
        {
            string Result = "";

            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem;
            XmlElement xmlelem2;
            XmlElement xmlelem3;
            XmlText xmltext;
            try
            {
                xmldoc = new XmlDocument();

                //加入XML的声明段落
                xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);
                //加入一个根元素
                xmlelem = xmldoc.CreateElement("", "ROOT", "");
                xmldoc.AppendChild(xmlelem);

                for (int i = 0; i < addressRecords.Length; i++)
                {
                    //加入另外一个元素
                    xmlelem2 = xmldoc.CreateElement("AddressRecord");
                    xmlelem2 = xmldoc.CreateElement("", "AddressRecord", "");

                    if (!CommonUtility.IsEmpty(addressRecords[i].Address))
                    {
                        xmlelem3 = xmlelem3 = xmldoc.CreateElement("", "Address", "");
                        xmltext = xmldoc.CreateTextNode(addressRecords[i].Address.ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                    }

                    if (!CommonUtility.IsEmpty(addressRecords[i].ContactTel))
                    {
                        xmlelem3 = xmldoc.CreateElement("", "ContectTel", "");

                        xmltext = xmldoc.CreateTextNode(addressRecords[i].ContactTel.ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                    }

                    if (!CommonUtility.IsEmpty(addressRecords[i].LinkMan))
                    {
                        xmlelem3 = xmlelem3 = xmldoc.CreateElement("", "LinkMan", "");
                        xmltext = xmldoc.CreateTextNode(addressRecords[i].LinkMan.ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                    }
                    if (!CommonUtility.IsEmpty(addressRecords[i].Zipcode))
                    {
                        xmlelem3 = xmlelem3 = xmldoc.CreateElement("", "Zipcode", "");
                        xmltext = xmldoc.CreateTextNode(addressRecords[i].Zipcode.ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                    }

                    if (!CommonUtility.IsEmpty(addressRecords[i].Type))
                    {
                        xmlelem3 = xmlelem3 = xmldoc.CreateElement("", "Type", "");
                        xmltext = xmldoc.CreateTextNode(addressRecords[i].Type.ToString());
                        xmlelem3.AppendChild(xmltext);
                        xmlelem2.AppendChild(xmlelem3);
                    }

                    xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
                }
                //保存创建好的XML文档

                //xmldoc.Save("c:\\AddressRecords.xml");

                Result = xmldoc.OuterXml;


                Result = Result.Substring(Result.IndexOf("<ROOT>"));
                Result = @"<?xml version='1.0' encoding='gb2312'?>" + Result;
            }
            catch { }

            return Result;

        }

        ///// <summary>
        ///// 执行绑定电话存储过程
        ///// </summary>
        ///// <param name="cmd"></param>
        ///// <param name="StoredProcedureStr"></param>
        //private void ExBoundPhone(ref SqlCommand cmd, string StoredProcedureStr)
        //{
        //    cmd.CommandText = StoredProcedureStr;
        //    cmd.Parameters.Clear();

        //    SqlParameter parPhoneNum = new SqlParameter("PhoneNum", SqlDbType.VarChar, 20);
        //    cmd.Parameters.Add(parPhoneNum);

        //    SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
        //    cmd.Parameters.Add(parCustID);
        //    parCustID.Value = custID;

        //    for (int i = 0; i < BoundPhoneRecords.Length; i++)
        //    {
        //        parPhoneNum.Value = BoundPhoneRecords[i].Phone;

        //        cmd.ExecuteNonQuery();
        //    }

        //}


        ///// <summary>
        ///// 执行地址信息存储过程
        ///// </summary>
        ///// <param name="cmd"></param>
        //private void ExAddressInfoCmd(ref SqlCommand cmd, string StoredProcedureStr)
        //{
        //    cmd.Parameters.Clear();
        //    cmd.CommandText = StoredProcedureStr;

        //    SqlParameter parUserAccount = new SqlParameter("UserAccount", SqlDbType.VarChar, 16);
        //    parUserAccount.Value = userAccount;
        //    cmd.Parameters.Add(parUserAccount);

        //    SqlParameter parCustID = new SqlParameter("CustID", SqlDbType.VarChar, 16);
        //    cmd.Parameters.Add(parCustID);

        //    SqlParameter parLinkMan = new SqlParameter("LinkMan", SqlDbType.VarChar, 20);
        //    cmd.Parameters.Add(parLinkMan);

        //    SqlParameter parContactTel = new SqlParameter("ContactTel", SqlDbType.VarChar, 20);
        //    cmd.Parameters.Add(parContactTel);

        //    SqlParameter parAddress = new SqlParameter("Address", SqlDbType.VarChar, 200);
        //    cmd.Parameters.Add(parAddress);

        //    SqlParameter parZipcode = new SqlParameter("Zipcode", SqlDbType.VarChar, 6);
        //    cmd.Parameters.Add(parZipcode);

        //    for (int i = 0; i < this.addressRecords.Length; i++)
        //    {
        //        parLinkMan.Value = addressRecords[i].LinkMan;
        //        parContactTel.Value = addressRecords[i].ContactTel;
        //        parAddress.Value = addressRecords[i].Address;
        //        parZipcode.Value = addressRecords[i].Zipcode;

        //        cmd.ExecuteNonQuery();
        //    }

        //}

        public static void WriteLogForUserInfo(UserInfo UserDetailInfo, ref StringBuilder Msg)
        {
            if (UserDetailInfo != null)
            {
                Msg.Append("UserType - " + UserDetailInfo.UserType);
                Msg.Append(";UserAccount - " + UserDetailInfo.UserAccount);
                Msg.Append(";Password - " + UserDetailInfo.Password);
                Msg.Append(";CustID - " + UserDetailInfo.CustID);
                Msg.Append(";UProvinceID - " + UserDetailInfo.UProvinceID);
                Msg.Append(";AreaCode - " + UserDetailInfo.AreaCode);
                Msg.Append(";Status - " + UserDetailInfo.Status);
                Msg.Append(";RealName - " + UserDetailInfo.RealName);
                Msg.Append(";CertificateCode - " + UserDetailInfo.CertificateCode);
                Msg.Append(";CertificateType - " + UserDetailInfo.CertificateType);
                Msg.Append(";Birthday - " + UserDetailInfo.Birthday);
                Msg.Append(";Sex - " + UserDetailInfo.Sex);
                Msg.Append(";CustLevel - " + UserDetailInfo.CustLevel);
                Msg.Append(";EduLevel - " + UserDetailInfo.EduLevel);
                Msg.Append(";Favorite - " + UserDetailInfo.Favorite);
                Msg.Append(";IncomeLevel - " + UserDetailInfo.IncomeLevel);
                Msg.Append(";Email - " + UserDetailInfo.Email);
                Msg.Append(";PaymentAccountType - " + UserDetailInfo.PaymentAccountType);
                Msg.Append(";PaymentAccount - " + UserDetailInfo.PaymentAccount);
                Msg.Append(";PaymentAccountPassword - " + UserDetailInfo.PaymentAccountPassword);
                Msg.Append(";CustContactTel - " + UserDetailInfo.CustContactTel);
                Msg.Append(";EnterpriseID - " + UserDetailInfo.EnterpriseID);
                Msg.Append(";IsPost - " + UserDetailInfo.IsPost);
                Msg.Append(";ExtendField - " + UserDetailInfo.ExtendField + "\r\n");

                if (UserDetailInfo.AddressRecords != null)
                    if (UserDetailInfo.AddressRecords.Length > 0)
                    {
                        Msg.Append("AddressRecords: \r\n");
                        for (int i = 0; i < UserDetailInfo.AddressRecords.Length; i++)
                        {
                            Msg.Append("LinkMan - " + UserDetailInfo.AddressRecords[i].LinkMan);
                            Msg.Append(";ContactTel - " + UserDetailInfo.AddressRecords[i].ContactTel);
                            Msg.Append(";Address - " + UserDetailInfo.AddressRecords[i].Address);
                            Msg.Append(";Zipcode - " + UserDetailInfo.AddressRecords[i].Zipcode);
                            Msg.Append(";Type - " + UserDetailInfo.AddressRecords[i].Type);
                            Msg.Append("\r\n");
                        }
                    }


                if (UserDetailInfo.BoundPhoneRecords != null)
                    if (UserDetailInfo.BoundPhoneRecords.Length > 0)
                    {
                        Msg.Append("BoundPhoneRecords: \r\n");
                        for (int i = 0; i < UserDetailInfo.BoundPhoneRecords.Length; i++)
                        {
                            Msg.Append("Phone - " + UserDetailInfo.BoundPhoneRecords[i].Phone);
                            Msg.Append("\r\n");
                        }

                    }

                if (UserDetailInfo.AssessmentInfos != null)
                    if (UserDetailInfo.AssessmentInfos.Length > 0)
                    {
                        Msg.Append("AssessmentInfos: \r\n");
                        for (int i = 0; i < UserDetailInfo.AssessmentInfos.Length; i++)
                        {
                            Msg.Append("Credit - " + UserDetailInfo.AssessmentInfos[i].Credit);
                            Msg.Append(";CreditLevel - " + UserDetailInfo.AssessmentInfos[i].CreditLevel);
                            Msg.Append(";Score - " + UserDetailInfo.AssessmentInfos[i].Score);
                            Msg.Append(";AvailableScore - " + UserDetailInfo.AssessmentInfos[i].AvailableScore);
                            Msg.Append("\r\n");
                        }
                    }
            }

        }

        #region 手机，用户名注册

        //public int UseRegistry(string RegistrationStyle, string SPID, out string ErrMsg)
        //{
        //    int Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        //    ErrMsg = "";

        //     SqlConnection myCon = null;
        //    SqlCommand cmd = new SqlCommand();
        //    try
        //    {
        //        myCon = new SqlConnection(DBUtility.BestToneCenterConStr);
        //        cmd.Connection = myCon;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "up_BT_V3_Interface_PersonalRegistry";

        //        SqlParameter parSPID = new SqlParameter("@SPID", SqlDbType.VarChar, 8);
        //        parSPID.Value = SPID;
        //        cmd.Parameters.Add(parSPID);

        //        SqlParameter parPassword = new SqlParameter("@UserPassword", SqlDbType.VarChar, 6);
        //        parPassword.Value = password;
        //        cmd.Parameters.Add(parPassword);

        //        SqlParameter parEncryptPassword = new SqlParameter("@EncryptPassword", SqlDbType.VarChar, 50);
        //        parEncryptPassword.Value = CryptographyUtil.Encrypt(password);
        //        cmd.Parameters.Add(parEncryptPassword);

        //        SqlParameter parUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 8);
        //        parUserName.Value = UserName;
        //        cmd.Parameters.Add(parUserName);

        //        SqlParameter parMobilePhone = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 8);
        //        parMobilePhone.Value = MobilePhone;
        //        cmd.Parameters.Add(parMobilePhone);

        //        SqlParameter parBoundPhoneRecords = new SqlParameter("@BoundPhoneRecords ", SqlDbType.Text);
        //        string xmlBoundPhoneRecords = this.GenerateXmlForBoundPhoneRecords();

        //        if (xmlBoundPhoneRecords == "")
        //            parBoundPhoneRecords.Value = DBNull.Value;
        //        else
        //            parBoundPhoneRecords.Value = xmlBoundPhoneRecords;

        //        cmd.Parameters.Add(parBoundPhoneRecords);

        //        SqlParameter parAddressRecords = new SqlParameter("@AddressRecords", SqlDbType.Text);

        //        string xmlAddressRecords = this.GenerateXmlForAddressRecords();
        //        if (xmlAddressRecords == "")
        //            parAddressRecords.Value = DBNull.Value;
        //        else
        //            parAddressRecords.Value = xmlAddressRecords;
        //        cmd.Parameters.Add(parAddressRecords);

        //        SqlParameter parResult = new SqlParameter("@Result", SqlDbType.Int);
        //        parResult.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(parResult);

        //        SqlParameter parErrMsg = new SqlParameter("@ErrMsg ", SqlDbType.VarChar, 256);
        //        parErrMsg.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(parErrMsg);

        //        SqlParameter parOCustID = new SqlParameter("@oCustID ", SqlDbType.VarChar, 16);
        //        parOCustID.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(parOCustID);

        //        SqlParameter parOUserAccount = new SqlParameter("@oUserAccount ", SqlDbType.VarChar, 16);
        //        parOUserAccount.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(parOUserAccount);

        //        DBUtility.Execute(cmd, DBUtility.BestToneCenterConStr);

        //        Result = int.Parse(parResult.Value.ToString());
        //        ErrMsg = parErrMsg.Value.ToString();
        //        custID = parOCustID.Value.ToString();
        //        userAccount = parOUserAccount.Value.ToString();


        //    }
        //    catch (Exception ex)
        //    {
        //        Result = ErrorDefinition.BT_IError_Result_System_UnknowError_Code;
        //        ErrMsg = ex.Message;
        //    }

        //    return Result;

        //}

        #endregion


        #region 成员变量
        private int result = ErrorDefinition.IError_Result_UnknowError_Code;

        public int Result
        {
            get { return result; }
        }

        private string errorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;

        public string ErrorDescription
        {
            get { return errorDescription; }
        }


        private string userType = "";

        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        private string userAccount = "";

        public string UserAccount
        {
            get { return userAccount; }
            set { userAccount = value; }
        }

        private string password = "";

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string custID = "";

        public string CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        private string uProvinceID = "";

        public string UProvinceID
        {
            get { return uProvinceID; }
            set { uProvinceID = value; }
        }

        private string areaCode = "";

        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }

        private BoundPhoneRecord[] boundPhoneRecords = null;

        public BoundPhoneRecord[] BoundPhoneRecords
        {
            get { return boundPhoneRecords; }
            set { boundPhoneRecords = value; }
        }

        private AddressRecord[] addressRecords = null;

        public AddressRecord[] AddressRecords
        {
            get { return addressRecords; }
            set { addressRecords = value; }
        }

        private AssessmentInfoRecord[] assessmentInfoRecords = null;

        public AssessmentInfoRecord[] AssessmentInfoRecords
        {
            get { return assessmentInfoRecords; }
            set { assessmentInfoRecords = value; }

        }

        //


        private string status = "";

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string realName = "";

        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }

        private string userName = "";

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string certificateCode = "";

        public string CertificateCode
        {
            get { return certificateCode; }
            set { certificateCode = value; }
        }

        private string certificateType = "";

        public string CertificateType
        {
            get { return certificateType; }
            set { certificateType = value; }
        }

        private string birthday = "";

        public string Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        private string sex = "";

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        private string custLevel = "";

        public string CustLevel
        {
            get { return custLevel; }
            set { custLevel = value; }
        }

        private string eduLevel = "";

        public string EduLevel
        {
            get { return eduLevel; }
            set { eduLevel = value; }
        }

        private string favorite = "";

        public string Favorite
        {
            get { return favorite; }
            set { favorite = value; }
        }

        private string incomeLevel = "";

        public string IncomeLevel
        {
            get { return incomeLevel; }
            set { incomeLevel = value; }
        }

        private string email = "";

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string paymentAccountType = "";

        public string PaymentAccountType
        {
            get { return paymentAccountType; }
            set { paymentAccountType = value; }
        }

        private string paymentAccount = "";

        public string PaymentAccount
        {
            get { return paymentAccount; }
            set { paymentAccount = value; }
        }

        private string paymentAccountPassword = "";

        public string PaymentAccountPassword
        {
            get { return paymentAccountPassword; }
            set { paymentAccountPassword = value; }
        }

        private string custContactTel = "";

        public string CustContactTel
        {
            get { return custContactTel; }
            set { custContactTel = value; }
        }

        public string enterpriseID = "";
        public string EnterpriseID
        {
            get { return enterpriseID; }
            set { enterpriseID = value; }
        }

        public string isPost = "";
        public string IsPost
        {
            get { return isPost; }
            set { isPost = value; }
        }

        private string extendField = "";
        public string ExtendField
        {
            get { return extendField; }
            set { extendField = value; }
        }
        #endregion


       

    }
}

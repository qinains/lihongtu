using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;

using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;

/// <summary>
/// BTUCenterForCRM 的摘要说明
/// </summary>
[WebService(Namespace = "http://Customer.Besttone.cn")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class BTUCenterForCRM : System.Web.Services.WebService
{

    public BTUCenterForCRM()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }
    
    #region 漫游认证接口
    public class RoamAuthResult
    {
        public int Result;
        public string CustID;
        public string UserAccount;
        public string CustType;
        public string ErrorDescription;
        public string ExtendField;
    }

   // [WebMethod(Description = "漫游密码认证接口")]
    public RoamAuthResult RoamAuth(string SystemsID, string AuthenName, string AuthenType, string Password, string ExtendField)
    {
        RoamAuthResult Result = new RoamAuthResult();
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        Result.CustType = "";
       
        string ProvinceID = "";

        try
        {
            #region 数据校验
            if (CommonUtility.IsEmpty(SystemsID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "，不能为空";
                return Result;
            }

            if (SystemsID.Length != ConstDefinition.Length_SYSID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSYSID_Msg + "，长度有误";
                return Result;
            }


            if (CommonUtility.IsEmpty(AuthenName))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(AuthenType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
                return Result;
            }

            //if (Span_AuthenType)
            //{

            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，不能为空";
            //    return Result;
            //}

            if (CommonUtility.IsEmpty(Password))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidPassword_Msg + "，不能为空";
                return Result;
            }

            #endregion

            //string CustType = "";
            //Result.Result = BTForBusinessSystemInterfaceRules.UserAuthV2(SystemsID, AuthenName, AuthenType, Password, this.Context,
            //    out Result.ErrorDescription, out Result.CustID, out Result.UserAccount, out Result.CustType,out outerid, out ProvinceID);

        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("漫游认证接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";SystemsID - " + SystemsID);
            msg.Append(";AuthenName - " + AuthenName);
            msg.Append(";AuthenType - " + AuthenType);
            msg.Append(";Password - " + Password);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; CustID - " + Result.CustID);
            msg.Append("; UserAccount - " + Result.UserAccount);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("UserAuthV2", msg);
            #endregion

            //写数据库日志
            try
            {
                CommonBizRules.WriteDataLog(SystemsID, AuthenName, AuthenType, Result.Result,
                    Result.ErrorDescription, "", "UserAuthV2");
            }
            catch { }

        }
        return Result;

    }

    #endregion

    #region 数据同步接口
    public class CustInfoUploadResult
    {
        public int Result;
        public string CustID;
        public string ID;
        public string ErrorDescription;
        public string ExtendField;
    }

    [WebMethod(Description = "数据同步接口")]
    public CustInfoUploadResult CustInfoUpload(string ProvinceID, string ID, string CustType, string CustAccount, string CustLevel,
         string RealName, string ContactTel, string Address, string ZipCode, string CertificateCode,
         string CertificateType, string AreaID, string Sex, string Email, string dealType,AuthenRecord[] AuthenRecords, string ExtendField)
    {
        CustInfoUploadResult Result = new CustInfoUploadResult();
        string SPID = "";
        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ID = ID;
        Result.ExtendField = "";
        Result.CustID = "";
        string strXML = "";

        

        try
        {
            #region 数据校验

            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = "ProvinceID不能为空";
                return Result;
            }


            if (CommonUtility.IsEmpty(CustType))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code;
                Result.ErrorDescription = "CUstType不能为空";
                return Result;
            }


            if (CommonUtility.IsEmpty(CustLevel))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustLevel_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustLevel_Msg + "，不能为空";
                return Result;
            }
            if (CustType.Length != 2)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code;
                Result.ErrorDescription = "CustType无效，长度不为2";
                return Result;
            }

            if (CommonUtility.IsEmpty(RealName))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidPassword_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                return Result;
            }


            if (!CommonUtility.IsEmpty(ExtendField))
            {
                SPID = CommonBizRules.GetValueFromXmlStr(ExtendField, "SPID");
            }

            switch(CustType)
            {
                case "01":
                    CustType = "14";
                    break;
                case "02":
                    CustType = "20";
                    break;
                case "03":
                    CustType = "12";
                    break;
                case "09":
                    CustType = "90";
                    break;
                case "11":
                    CustType = "30";
                    break;
                case "00":
                    CustType = "42";
                    break;
            }


            Result.Result = BTForCrmBizRules.CustInfoUpload(ProvinceID, ID, CustType, CustAccount, CustLevel,
              RealName, ContactTel, Address, ZipCode, CertificateCode,
              CertificateType, AreaID, Sex, Email, dealType, ExtendField, AuthenRecords,SPID, out Result.CustID,out Result.ErrorDescription);

            if (Result.Result == 0)
            {
                //通知积分系统
                CIP2BizRules.InsertCustInfoNotify(Result.CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], "", dealType, out Result.ErrorDescription);
              
            }
           strXML = BTBizRules.GenerateResultXml(Result.Result.ToString(), Result.ErrorDescription);
            #endregion
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "," + ex.Message;
            
        }
        finally 
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("Crm客户同步接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";ProvinceID - " + ProvinceID);
            msg.Append(";ID - " + ID);
            msg.Append(";CustType - " + CustType);
             msg.Append(";CustAccount - " + CustAccount);
             msg.Append(";CustLevel - " + CustLevel);
             msg.Append(";RealName - " + RealName);
             msg.Append(";ContactTel - " + ContactTel);
             msg.Append(";Address - " + Address);
             msg.Append(";ZipCode - " + ZipCode);
             msg.Append(";CertificateCode - " + CertificateCode);
             msg.Append(";CertificateType - " + CertificateType);
             msg.Append(";AreaID - " + AreaID);
             msg.Append(";Sex - " + Sex);
             msg.Append(";Email - " + Email);
             msg.Append(";dealType - " + dealType);
             msg.Append(";ExtendField - " + ExtendField);
             msg.Append(";strXML - " + strXML);
            
            if(AuthenRecords != null )
            {
                if (AuthenRecords.Length > 0)
                {
                    for (int i = 0; i < AuthenRecords.Length; i++)
                    {
                        msg.Append(";AuthenType - " + AuthenRecords[i].AuthenType);
                        msg.Append(";AuthenName - " + AuthenRecords[i].AuthenName);
                    }
                }
            }
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; CustID - " + Result.CustID);
            msg.Append("; ID - " + Result.ID);
            msg.Append("; ErrorDescription - " + Result.ErrorDescription);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForCRM("CustInfoUpload", msg);
            #endregion        
        }

        return Result;

    }
    #endregion

    #region 客户状态变更接口

    public class CustStatusChangeResult
    { 
        public string ProvinceID;
        public int Result;
        public string CRMID;
        public string ErrorDescription;
        public string ExtendField;

    
    }

    [WebMethod(Description = "客户状态变更接口")]
    public CustStatusChangeResult CustStatusChange(string ProvinceID, string CRMID, string CustType, string CRMAccount, string Status,
        string Description, string ExtendField)
    {
        CustStatusChangeResult Result = new CustStatusChangeResult();
        string SPID = "";
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        Result.CRMID = "";
        Result.ProvinceID = "";
        try
        {
            if (CommonUtility.IsEmpty(ProvinceID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(CRMID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，CRMID不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(CustType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + "，CustType不能为空";
                return Result;
            }

            if (CommonUtility.IsEmpty(Status))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidStatus_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidStatus_Msg + "，不能为空";
                return Result;
            }
            if (!CommonUtility.IsEmpty(ExtendField))
            {
                SPID = CommonBizRules.GetValueFromXmlStr(ExtendField, "SPID");
            }
            //if (CommonUtility.IsEmpty(CRMAccount))
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，CRMAccount不能为空";
            //    return Result;
            //}

            Result.Result = BTForCrmBizRules.CustStatusChange(ProvinceID, CRMID, CustType, CRMAccount, Status,SPID, out Result.ErrorDescription, out Result.ProvinceID, out Result.CRMID);
            Result.ProvinceID = ProvinceID;

        }
        catch(Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "," + ex.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("客户状态变更接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";ProvinceID - " + ProvinceID);
            msg.Append(";CRMID - " + CRMID);
            msg.Append(";CRMAccount - " + CRMAccount);
            msg.Append(";CustType - " + CustType);
            msg.Append(";Status - " + Status);
            msg.Append(";Description - " + Description);
            msg.Append(";ExtendField - " + ExtendField);
        

            msg.Append("\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; ProvinceID - " + Result.ProvinceID);
            msg.Append("; CRMID - " + Result.CRMID);
            msg.Append("; ErrorDescription - " + Result.ErrorDescription);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForCRM("CustStatusChange", msg);
            #endregion
        }
        return Result;
    
    }
    #endregion

    #region 客户合并接口

    public class IncorporateAccountResult
    {
        public string ProvinceID;
        public int Result;
        public string ErrorDescription;
        public string CRMID;
        public string CustID;
        public string SavedCustType;
        public string ExtendField;

    }


    [WebMethod(Description = "客户合并接口")]
    public IncorporateAccountResult IncorporateAccount(string ProvinceID, string IncorporatedCustType, string IncorporatedCRMID,
           string IncorporatedAccount, string SavedCRMID, string SavedAccount, string SavedCustType, string ExtendField)
    {
        string SPID = "";
        IncorporateAccountResult Result = new IncorporateAccountResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code; 
        Result.ProvinceID = "";
        Result.ErrorDescription = "";
        Result.CRMID = "";
        Result.CustID = "";
        Result.SavedCustType = "";
        Result.ExtendField = "";
        try
        {
            #region 验证
            if (CommonUtility.IsEmpty(ProvinceID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(IncorporatedCustType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，IncorporatedCustType不能为空";
                return Result;
            }

            if(CommonUtility.IsEmpty(IncorporatedCRMID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，IncorporatedCRMID不能为空";
                return Result;
            }

            //if (CommonUtility.IsEmpty(IncorporatedAccount))
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，IncorporatedAccount不能为空";
            //    return Result;
            //}

            if (CommonUtility.IsEmpty(SavedCRMID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，SavedCRMID不能为空";
                return Result;
            }

            if (!CommonUtility.IsEmpty(ExtendField))
            {
                SPID = CommonBizRules.GetValueFromXmlStr(ExtendField, "SPID");
            }
            //if (CommonUtility.IsEmpty(SavedAccount))
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，SavedAccount不能为空";
            //    return Result;
            //}

            if (CommonUtility.IsEmpty(SavedCustType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，SavedCustType不能为空";
                return Result;
            }
            if (ConstDefinition.Span_UserType.IndexOf(IncorporatedCustType) < 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + "，无该用户类型";
                return Result;
            }
            if (ConstDefinition.Span_UserType.IndexOf(SavedCustType) < 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidUserType_Code;
                Result.ErrorDescription  = ErrorDefinition.BT_IError_Result_InValidUserType_Msg + "，无该用户类型";
                return Result;
            }
            //string tmp = "01,02,03,09,11";
            //if (tmp.IndexOf(IncorporatedCustType)<0)
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，IncorporatedCustType取值不在范围内";
            //    return Result;
            //}
            //if (tmp.IndexOf(SavedCustType) < 0)
            //{
            //    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
            //    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，SavedCustType取值不在范围内";
            //    return Result;
            //}
            #endregion

            Result.Result = BTForCrmBizRules.IncorporateAccount(ProvinceID, IncorporatedCustType, IncorporatedCRMID, IncorporatedAccount, SavedCRMID, SavedAccount,
                SavedCustType, SPID,out Result.ProvinceID, out Result.ErrorDescription, out Result.CRMID, out Result.CustID, out Result.SavedCustType);
           

        }
        catch(Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "," + ex.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("CRM客户合并接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";ProvinceID - " + ProvinceID);
            msg.Append(";IncorporatedCustType - " + IncorporatedCustType);
            msg.Append(";IncorporatedCRMID - " + IncorporatedCRMID);
            msg.Append(";IncorporatedAccount - " + IncorporatedAccount);
            msg.Append(";SavedCRMID - " + SavedCRMID);
            msg.Append(";SavedAccount - " + SavedAccount);
            msg.Append(";SavedCustType - " + SavedCustType);
            msg.Append(";ExtendField - " + ExtendField);


            msg.Append("\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; ProvinceID - " + Result.ProvinceID);
            msg.Append("; CRMID - " + Result.CRMID);
            msg.Append("; CustID - " + Result.CustID);
            msg.Append("; SavedCustType - " + Result.SavedCustType);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForCRM("IncorporateAccount", msg);
            #endregion
        }

        return Result;
    }


    #endregion

    #region 认证信息同步接口
    public class UserAuthenStyleUploadResult
    {
        public int Result;
        public string CustID;
        public string ID;
        public string ErrorDescription;
        public string ExtendField;
    }

        [WebMethod(Description = "认证信息同步接口")]
    public UserAuthenStyleUploadResult UserAuthenStyleUpload(string ProvinceID, string ID,
         string AreaID, AuthenRecord[] AuthenRecords, string ExtendField)
    {
        UserAuthenStyleUploadResult Result = new UserAuthenStyleUploadResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = "";
        Result.ID = ID;
        Result.ExtendField = "";
        Result.CustID = "";
        //string strXML = "";
        string SPID = "";

        

        try
        {
            #region 数据校验

            if (CommonUtility.IsEmpty(ProvinceID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = "ProvinceID不能为空";
                return Result;
            }


            if (CommonUtility.IsEmpty(AreaID))
            {

                Result.Result = ErrorDefinition.BT_IError_Result_InValidAreaCode_Code;
                Result.ErrorDescription =ErrorDefinition.BT_IError_Result_InValidAreaCode_Msg+",AreaID不能为空";
                return Result;
            }



            if (CommonUtility.IsEmpty(ID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";
                return Result;
            }

            if (!CommonUtility.IsEmpty(ExtendField))
            {
                SPID = CommonBizRules.GetValueFromXmlStr(ExtendField, "SPID");
            }
            #endregion


            Result.Result = BTForCrmBizRules.UserAuthenStyleUpload(ProvinceID, ID, AreaID,ExtendField, AuthenRecords,SPID, out Result.CustID, out Result.ErrorDescription);

         
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "," + ex.Message;

        }
        finally 
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("Crm客户认证信息同步接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";ProvinceID - " + ProvinceID);
            msg.Append(";ID - " + ID);        
             msg.Append(";AreaID - " + AreaID);          
            
            if(AuthenRecords != null )
            {
                if (AuthenRecords.Length > 0)
                {
                    for (int i = 0; i < AuthenRecords.Length; i++)
                    {
                        msg.Append(";AuthenType - " + AuthenRecords[i].AuthenType);
                        msg.Append(";AuthenName - " + AuthenRecords[i].AuthenName);
                    }
                }
            }
            msg.Append(";ExtendField - " + ExtendField);
            msg.Append("\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; CustID - " + Result.CustID);
            msg.Append("; ID - " + Result.ID);
            msg.Append("; ErrorDescription - " + Result.ErrorDescription);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForCRM("UserAuthenStyleUpload", msg);
            #endregion
        
        }

        return Result;

    }
    #endregion

    #region 认证方式变更接口

    public class ChangeUserAuthenStyleResult
    {
        public int Result;        
        public string OriginalID;
        public string NewID;
        public string ErrorDescription;
        public string ExtendField;

    }


    [WebMethod(Description = "认证方式变更接口")]
    public ChangeUserAuthenStyleResult ChangeUserAuthenStyle(string ProvinceID, string OriginalID, string NewID,
        string DealType, AuthenRecord[] AuthenRecords, string ExtendField)
    {
        ChangeUserAuthenStyleResult Result = new ChangeUserAuthenStyleResult();
        Result.Result = ErrorDefinition.BT_IError_Result_UnknowError_Code;
        Result.NewID = "";
        Result.ErrorDescription = "";
        Result.OriginalID = "";
        Result.ErrorDescription = "";
        Result.ExtendField = "";
        string SPID = "";
        try
        {

            #region 验证
            if (CommonUtility.IsEmpty(ProvinceID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，不能为空";
                return Result;
            }

            if (ProvinceID.Length != ConstDefinition.Length_ProvinceID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg + "，长度有误";
                return Result;
            }

            if (CommonUtility.IsEmpty(OriginalID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，OriginalID不能为空";
                return Result;
            }
           
            if (CommonUtility.IsEmpty(DealType))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，DealType不能为空";
                return Result;
            }
            string tmp = "1;2;3";
            if (tmp.IndexOf(DealType)<0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，DealType类型不正确！";
                return Result;
            }
             if(DealType=="1")
             {
                if (CommonUtility.IsEmpty(NewID))
                {
                    Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                    Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，NewID不能为空";
                    return Result;
                }
            }
            if (AuthenRecords==null)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，AuthenRecords不能为空";
                return Result;
            }
            if (AuthenRecords.Length <= 0)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidParameter_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidParameter_Msg + "，AuthenRecords数组长度为小于0";
                return Result;
            }

            if (!CommonUtility.IsEmpty(ExtendField))
            {
                SPID = CommonBizRules.GetValueFromXmlStr(ExtendField, "SPID");
            }
            
          
            #endregion

            Result.Result = BTForCrmBizRules.ChangeUserAuthenStyle(ProvinceID,OriginalID, NewID, DealType, AuthenRecords,SPID, out Result.NewID, out Result.OriginalID, out Result.ErrorDescription);

        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.IError_Result_System_UnknowError_Code;
            Result.ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + "," + ex.Message;
        }
        finally
        {
            #region WriteLog
            StringBuilder msg = new StringBuilder();
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n\r\n");
            msg.Append("认证方式变更接口 " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";ProvinceID - " + ProvinceID);
            msg.Append(";OriginalID - " + OriginalID);
            msg.Append(";NewID - " + NewID);
            msg.Append(";DealType - " + DealType);
            if (AuthenRecords != null)
            {
                if (AuthenRecords.Length > 0)
                {
                    for (int i = 0; i < AuthenRecords.Length; i++)
                    {
                        msg.Append(";AuthenType - " + AuthenRecords[i].AuthenType);
                        msg.Append(";AuthenName - " + AuthenRecords[i].AuthenName);
                    }
                }
            }
            msg.Append(";ExtendField - " + ExtendField);


            msg.Append("\r\n");
            msg.Append("处理结果 - " + Result.Result);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);

            msg.Append("; NewID - " + Result.NewID);
            msg.Append("; OriginalID - " + Result.OriginalID);
            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForCRM("ChangeUserAuthenStyle", msg);
            #endregion
        }

        return Result;
    }


    #endregion

}


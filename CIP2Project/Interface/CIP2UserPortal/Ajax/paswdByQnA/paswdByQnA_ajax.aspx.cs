using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Rule;

public partial class Ajax_paswdByQnA_paswdByQnA_ajax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 1)
            {
                aaa();
            }
            else if (Convert.ToInt32(Request.QueryString["typeId"].ToString()) == 2)

            { bbb(); }

        }
    }
    #region 获取参数并调用方法，Response.Write(接受参数)
    public void aaa()
    {

        string AuthenName = HttpUtility.HtmlDecode(Request.QueryString["name"].ToString());
        string QuestionID = HttpUtility.HtmlDecode(Request.QueryString["questionID"].ToString());
        string Answer = HttpUtility.HtmlDecode(Request.QueryString["answer"].ToString());
        
        
        ///调用你的方法


        if (!ValidateValidateCode())
        {
            Response.Write("1");
            return;
        }



        string CustID = "";
        string outerid = "";

        string UserAccount = "";
        string ProvinceID = "";

        string CustType = "";
        string RealName = "";
        string UserName = "";
        string NickName = "";
        int Result1=0;
        int Result2=0;
        string ErrorDescription;
        string str;


        try
        {
            #region 数据校验


            if (CommonUtility.IsEmpty(AuthenName))
            {

                Result1 = ErrorDefinition.BT_IError_Result_InValidRealName_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidRealName_Msg + "，不能为空";
                //return Result;
            }

           

            #endregion



            Result1 = BTForBusinessSystemInterfaceRules.UserAuthV2("35000000", AuthenName, "1", "",
                this.Context, ProvinceID,"","",
                out ErrorDescription, out CustID, out UserAccount, out CustType, out outerid, out ProvinceID, out  RealName, out  UserName, out  NickName);


            //Result1 = BTForBusinessSystemInterfaceRules.UserAuthV2("35000000", AuthenName, "1", "",
            //    this.Context,ProvinceID,
            //    out ErrorDescription


        }
        catch (Exception e)
        {
            Result1 = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }

        if (Result1 != -21501)
        {
           str = "CustID:\"" + CustID + "\"," + "CustType:\"" + CustType + "\"," + "Result1:\"" + Result1 + "\"," + "Result2:\"" + Result2 + "\"";
           Response.Write(str);
           return;
        }

        try
        {



            if (CommonUtility.IsEmpty(CustID))
            {
                Result2 = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "，不能为空";

            }
            if (CommonUtility.IsEmpty(QuestionID))
            {
                Result2 = ErrorDefinition.IError_Result_UnknowError_Code;
                ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg + "，QuestionID不能为空";

            }
            if (CommonUtility.IsEmpty(Answer))
            {
                Result2 = ErrorDefinition.IError_Result_UnknowError_Code;
                ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg + "，Answer不能为空";

            }


            Result2 = BTForBusinessSystemInterfaceRules.PwdQuestionAuth(CustID, int.Parse(QuestionID), Answer, out ErrorDescription);

        }
        catch (Exception err)
        {
            Result2 = ErrorDefinition.IError_Result_UnknowError_Code;
            ErrorDescription = err.Message.ToString(); ;
        }


        str = "CustID:\"" + CustID + "\"," + "CustType:\"" + CustType + "\"," + "Result1:\"" + Result1 + "\"," + "Result2:\"" + Result2 + "\"";

        Response.Write(str);
    }
    #endregion
    #region 获取参数并调用方法，Response.Write(接受参数)
    public void bbb()
    {
        string CustID = HttpUtility.HtmlDecode(Request.QueryString["custID"].ToString());
        string AuthenNumber = HttpUtility.HtmlDecode(Request.QueryString["name"].ToString());
        string CustType = HttpUtility.HtmlDecode(Request.QueryString["custType"].ToString());
        string Pwd = HttpUtility.HtmlDecode(Request.QueryString["pwd"].ToString());
        string PwdType = "2";
        string SPID = "35000000";
        int Result1=0;
        int Result2 = 0;
        string ErrorDescription = "";
        string ExtendField = "";
        string ErrMsg = "";
        string IPAddress = "";


        try
        {
            //数据合法性判断

            if (CommonUtility.IsEmpty(CustID))
            {
                Result1 = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "不能为空";
                
            }

            if (CustID.Length > ConstDefinition.Length_CustID)
            {
                Result1 = ErrorDefinition.BT_IError_Result_InValidCustID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidCustID_Msg + "长度有误";
               
            }

            if (CommonUtility.IsEmpty(Pwd))
            {
                Result1 = ErrorDefinition.BT_IError_Result_InValidProvinceID_Code;
                ErrorDescription = ErrorDefinition.BT_IError_Result_InValidProvinceID_Msg;
              
            }



            //数据库操作

            Result1 = PassWordBO.SetPassword(SPID, CustID, Pwd, PwdType, ExtendField, out ErrorDescription);

        }
        catch (Exception e)
        {
            Result1 = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }
        try
        {
            if (Context.Request.ServerVariables["HTTP_VIA"] != null) // using proxy
            {
                IPAddress = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();  // Return real client IP.
            }
            else// not using proxy or can't get the Client IP
            {
                IPAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
            }
            Result2 = FindPwd.InsertFindPwdLog(CustID, CustType, "0", "1", AuthenNumber, Result1, SPID, IPAddress, "", out  ErrMsg);
        }
        catch (Exception e)
        {
            Result2 = ErrorDefinition.IError_Result_System_UnknowError_Code;
            ErrorDescription = ErrorDefinition.IError_Result_System_UnknowError_Msg + e.Message;
        }

        Response.Write(Result1);
     
          }
    #endregion
    #region 验证校验
    /// <summary>
    /// 验证验证码是否正确
    /// </summary>
    /// <returns></returns>
    private bool ValidateValidateCode()
    {
        bool result = false;

        try
        {
            string validStr = HttpUtility.HtmlDecode(Request.QueryString["code"].ToString().Trim().ToUpper());
            validStr = CryptographyUtil.Encrypt(validStr);
            if (Request.Cookies["PASSPORT_USER_VALIDATOR"] == null)
            {
                result = false;
            }
            else
            {
                if (Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"] == null || Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"] == null)
                {
                    result = false;
                }
                if (validStr == Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ValidatorStr"].ToString())
                {
                    if (DateTime.Now > DateTime.Parse(CryptographyUtil.Decrypt(Request.Cookies["PASSPORT_USER_VALIDATOR"].Values["ExpireTime"].ToString())))
                        result = false;
                    else
                        result = true;
                }
                else
                {
                    result = false;
                }
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }
    #endregion

}

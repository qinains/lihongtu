using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;
using System.Xml;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;

/// <summary>
/// BesttoneAccountService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class BesttoneAccountService : System.Web.Services.WebService
{

    public BesttoneAccountService()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    public class GetAuthenCodeResult
    {
        public int Result;
        public string CustID;
        public string Phone;
        public string AuthenCode;
        public string ExtendField;
        public string ErrorDescription;
    }
    [WebMethod(Description = "获取验证码接口")]
    public GetAuthenCodeResult GetAuthenCode(string SPID, string phone, string ExtendField)
    {
        GetAuthenCodeResult Result = new GetAuthenCodeResult();

        Result.Result = ErrorDefinition.IError_Result_UnknowError_Code;
        Result.ErrorDescription = ErrorDefinition.IError_Result_UnknowError_Msg;
        Result.ExtendField = "";

        try
        {
            #region 数据校验
            //数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "不能为空";
                return Result;
            }

            if (SPID.Length != ConstDefinition.Length_SPID)
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidSPID_Code;
                Result.ErrorDescription = ErrorDefinition.BT_IError_Result_InValidSPID_Msg + "长度有误";
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetAuthenCode", this.Context, out Result.ErrorDescription);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (!ValidateUtility.ValidateMobile(phone))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription = "手机号码格式有误";
                return Result;
            }
            String custid = PhoneBO.IsAuthenPhone(phone, SPID, out Result.ErrorDescription);
            if (String.IsNullOrEmpty(custid))
            {
                Result.Result = ErrorDefinition.BT_IError_Result_InValidPhoneNum_Code;
                Result.ErrorDescription += ",验证手机有误";
                return Result;
            }

            #endregion

            if (custid != "")
            {
                Random r = new Random();
                string AuthenCode = "";
                AuthenCode += r.Next(10000, 99999).ToString();
                //这里要根据custid 去查找该客户的认证手机,比较认证手机是否和phone一致
                //在另外一个webservice接口中根据phone获得custid，根据custid获得password
                // CryptographyUtil.Decrypt(password);
                //CommonBizRules.SendMessageV2(phone, "您的验证码是:" + AuthenCode, "35000000");
                CommonBizRules.SendMessageV3(phone, "您的验证码是:" + AuthenCode, SPID);
                //将AuthenCode插入数据库
                PhoneBO.InsertPhoneSendMassage(custid, "验证码信息内容", AuthenCode, phone, DateTime.Now, "描述未知", 1, 0, "1", out Result.ErrorDescription);
                Result.AuthenCode = AuthenCode;
                Result.CustID = custid;
                Result.Result = 0;
                Result.ErrorDescription = "您的验证码已经下发到手机，请查看!";
            }
            else
            {
                Result.Result = -1728;
                Result.ErrorDescription = "该手机号码不是认证电话!";
                //CommonBizRules.SendMessage(phone, Result.ErrorDescription, "35000000");
                //PhoneBO.InsertPhoneSendMassage(custid, "验证码信息内容", "非认证电话", phone, DateTime.Now, "描述未知", 1, 0, "1", out ErrMsg);
            }

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
            msg.Append("获取手机验证码： " + DateTime.Now.ToString("u") + "\r\n");
            msg.Append(";IP - " + HttpContext.Current.Request.UserHostAddress);
            msg.Append(";SPID - " + SPID);
            msg.Append(";手机号 - " + phone);
            msg.Append("\r\n");

            msg.Append("处理结果 - " + Result.Result);
            msg.Append("验证码：" + Result.AuthenCode);
            msg.Append("; 错误描述 - " + Result.ErrorDescription);
            msg.Append("; CustID - " + Result.CustID);

            msg.Append("; ExtendField - " + Result.ExtendField + "\r\n");
            msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            BTUCenterInterfaceLog.CenterForBizTourLog("GiveMeCheckCode", msg);
            #endregion
        }
        return Result;

    }


    public class OpenBesttoneAccountV2Result
    {
        public String returnCode;
        public String msg;
    }
 
    /// <summary>
    /// 开户
    /// </summary>
    /// <param name="Request"></param>
    /// <returns></returns>
    /// 
    [WebMethod(Description = "开通号码百事通账户-xml")]
    public OpenBesttoneAccountV2Result OpenBesttoneAccountV2(String Request)
    {

        OpenBesttoneAccountV2Result openBesttoneAccountV2Result = new OpenBesttoneAccountV2Result();
        String ReturnCode = "";
        String Descriptioin = ""; 
        int Result = 0;
        String ErrMsg = "";


        #region
        XmlDocument xmlDoc = new XmlDocument();
        String version = "";
        String SPID = "";
        String CustID = "";
        String EC = "";
        String HC = "";
        try
        {
            xmlDoc.LoadXml(Request);
            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode CustIDNode = xmlDoc.SelectNodes("/root/param/CUSTID")[0];
            CustID = CustIDNode.Attributes["value"].Value;

            EC = xmlDoc.SelectNodes("/root/param/EC")[0].InnerText;
            HC = xmlDoc.SelectNodes("/root/param/HC")[0].InnerText;
        }
        catch (Exception e)
        { 
            ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code);
            ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + e.Message;
            Descriptioin = ErrMsg+":xml格式错误";
            //Response.AppendFormat("<result returnCode = '{0}' msg = '{1}' />", ReturnCode, Descriptioin);
            //return Response.ToString();
            openBesttoneAccountV2Result.returnCode = ReturnCode;
            openBesttoneAccountV2Result.msg = Descriptioin;
            return openBesttoneAccountV2Result;
        }
        //String AuthenCode = xmlDoc.SelectNodes("/root/param/AuthenCode")[0].InnerText;
        #endregion

        StringBuilder strLog = new StringBuilder();
        try
        {

            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

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
                    if (String.IsNullOrEmpty(PlanTextStr))
                    {
                        ReturnCode = "-7020";
                        Descriptioin = "解密失败！" ;
                        openBesttoneAccountV2Result.returnCode = ReturnCode;
                        openBesttoneAccountV2Result.msg = Descriptioin;
                        return openBesttoneAccountV2Result;

                    }
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
                    Descriptioin = "解密错误"+e.ToString();
                    openBesttoneAccountV2Result.returnCode = ReturnCode;
                    openBesttoneAccountV2Result.msg = Descriptioin;
                    return openBesttoneAccountV2Result;
                }
            }
            else
            {
                ReturnCode = "-7021";
                Descriptioin = "hashcode校验不通过!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }


            strLog.AppendFormat("Begin 开通号码百事通账户\r\n");
            strLog.AppendFormat("SPID:{0},CustID:{1},mobile:{2},realName:{3},contactTel:{4},email:{5},sex:{6},cerType:{7},cerNum:{8}\r\n", SPID, CustID, mobile, realName, contactTel, email, sex, cerType, cerNum);

            #region 数据合法性判断


            //if (CommonUtility.IsEmpty(AuthenCode))
            //{
            //    ReturnCode = "-7014";
            //    Descriptioin = "手机校验码不能为空!";
            //    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            //    return Response.ToString();
            //}

            //验证码校验
            //Result = PhoneBO.SelSendSMSMassage(CustID, mobile, AuthenCode, out ErrMsg);
            //if (Result != 0)
            //{
            //    ReturnCode = "-7014";
            //    Descriptioin = "验证码验证失败：" + ErrMsg;
            //    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            //    return Response.ToString();
          
            //}

            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:'{0}',ErrMsg:'{1}'\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;

            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "OpenBesttoneAccountV2", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_InValidCustID_Code);
                Descriptioin = "CustID不能为空:"+ErrorDefinition.BT_IError_Result_InValidCustID_Msg;
                //Response.AppendFormat("<result returnCode = '{0}' msg = '{1}' />", ReturnCode, Descriptioin);
                //return Response.ToString();
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }

            if (CommonUtility.IsEmpty(mobile))
            {
                ReturnCode = "-7015";
                Descriptioin = "账户名不能为空!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;

            }
       
            if (!Utils.isMobilePhoneV2(mobile))
            {
                ReturnCode = "-7016";
                Descriptioin = "无效的手机号码!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }

            if (CommonUtility.IsEmpty(realName))
            {
                ReturnCode = "-7017";
                Descriptioin = "用户名不能为空!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;

            }

            if (CommonUtility.IsEmpty(sex))
            {
                ReturnCode = "-7018";
                Descriptioin = "性别不能为空!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;

            }
            if (!Utils.IsNumeric(sex))
            {
                ReturnCode = "-7019";
                Descriptioin = "性别只能为数字!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }

            
            if ("0".Equals(sex) || "1".Equals(sex) || "2".Equals(sex))
            {
            }
            else
            {
                ReturnCode = "-7020";
                Descriptioin = "性别只能为0和1,2!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }

            if (CommonUtility.IsEmpty(cerType))
            {
                ReturnCode = "-7021";
                Descriptioin = "证件类型不能为空!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }

            if (cerType.Equals("1") || cerType.Equals("2") || cerType.Equals("3") || cerType.Equals("4") || cerType.Equals("5") || cerType.Equals("6") || cerType.Equals("7") || cerType.Equals("8") || cerType.Equals("9") || cerType.Equals("10") || cerType.Equals("X"))
            { }
            else
            {
                ReturnCode = "-7022";
                Descriptioin = "非法证件类型!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;

            }

            if (CommonUtility.IsEmpty(cerNum))
            {
                ReturnCode = "-7023";
                Descriptioin = "证件号不能为空!";
                openBesttoneAccountV2Result.returnCode = ReturnCode;
                openBesttoneAccountV2Result.msg = Descriptioin;
                return openBesttoneAccountV2Result;
            }


            if ("X".Equals(cerType))
            {
                if ("99999".Equals(cerNum))
                {
                }
                else
                {
                    ReturnCode = "-7024";
                    Descriptioin = "证件类型为其他类型(X)，则证件号必须是99999!";
                    openBesttoneAccountV2Result.returnCode = ReturnCode;
                    openBesttoneAccountV2Result.msg = Descriptioin;
                    return openBesttoneAccountV2Result;
                }
            }

            if ("1".Equals(cerType))
            {
                if (!CommonUtility.CheckIDCard(cerNum))
                {
                    ReturnCode = "-7025";
                    Descriptioin = "身份证不合法!";
                    openBesttoneAccountV2Result.returnCode = ReturnCode;
                    openBesttoneAccountV2Result.msg = Descriptioin;
                    return openBesttoneAccountV2Result;
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
                int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(mobile, out ai,  out ResponseCode, out ErrMsg);
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
                        ReturnCode = "0";
                        Descriptioin = "开户成功";
                        openBesttoneAccountV2Result.returnCode = ReturnCode;
                        openBesttoneAccountV2Result.msg = Descriptioin;
                        return openBesttoneAccountV2Result;
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
                            ReturnCode = "1";
                            Descriptioin = "该用户已开过户，仅做绑定关系";
                            openBesttoneAccountV2Result.returnCode = ReturnCode;
                            openBesttoneAccountV2Result.msg = Descriptioin;
                            return openBesttoneAccountV2Result;
                        }
                        else
                        {
                            ReturnCode = "-7026";
                            Descriptioin = ResponseCode;
                            openBesttoneAccountV2Result.returnCode = ReturnCode;
                            openBesttoneAccountV2Result.msg = Descriptioin;
                            return openBesttoneAccountV2Result;
                        }
                    }
                }
                else
                {
                    ReturnCode = "-7027";
                    Descriptioin = "查询账户出错!";
                    openBesttoneAccountV2Result.returnCode = ReturnCode;
                    openBesttoneAccountV2Result.msg = Descriptioin;
                    return openBesttoneAccountV2Result;
                }
            }
            else
            {
                // 账户已经绑定到其他人身上  
                strLog.AppendFormat("1 record return.\r\n");
                strLog.AppendFormat("该CustID:{0}上已经有绑定的账户号{1}\r\n", CustID, mobile);

                Result = -12300;
                ErrMsg = mobile + "手机号已为其他客户(" + CustID + ")开通了号码百事通账户，您可以登录系统，进入您的账户中心，用另一手机号码开通号码百事通账户，也可以咨询客服人员帮助排查问题。";
                openBesttoneAccountV2Result.returnCode = "-12300";
                openBesttoneAccountV2Result.msg = ErrMsg;
                return openBesttoneAccountV2Result;
 
            }
            strLog.AppendFormat("End 开通号码百事通账户 Result:{0},ErrMsg{1}\r\n", Result, ErrMsg);
            //-99999 失败 0 成功
        }
        catch (Exception e)
        {
            ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code);
            ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + e.Message;
            openBesttoneAccountV2Result.returnCode = ReturnCode;
            openBesttoneAccountV2Result.msg = ErrMsg+e.ToString();
            return openBesttoneAccountV2Result;
        }
        finally
        {
            log(strLog.ToString(),"OpenBesttoneAccountV2");
        }
        openBesttoneAccountV2Result.returnCode = ReturnCode;
        openBesttoneAccountV2Result.msg = Descriptioin;
        return openBesttoneAccountV2Result;
    }





    public class OpenBesttoneAccountResult
    {
        public Int32 Result;
        public String ErrMsg;
    
    }
    [WebMethod(Description = "开通号码百事通账户")]
    public OpenBesttoneAccountResult OpenBesttoneAccount(String SPID, String CustID, String EC,String HC)
    {
        OpenBesttoneAccountResult Result = new OpenBesttoneAccountResult();
        Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        StringBuilder strLog = new StringBuilder();
        try
        {

            SPInfoManager spInfo = new SPInfoManager();
            Object SPData = spInfo.GetSPData(this.Context, "SPData");
            string ScoreSystemSecret = spInfo.GetPropertyBySPID(SPID, "SecretKey", SPData);

            string newHC= CryptographyUtil.GenerateAuthenticator(SPID + "$" + CustID + "$" + EC, ScoreSystemSecret);
            
            strLog.AppendFormat("HC:{0};newHC:{1}\r\n"+HC,newHC);
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
                    strLog.AppendFormat("planTextStr:{0}\r\n",PlanTextStr);
                    string[] alSourceStr = PlanTextStr.Split('$');
                    realName = alSourceStr[0];
                    strLog.AppendFormat("realName:{0}\r\n", realName);
                    mobile = alSourceStr[1];
                    strLog.AppendFormat("mobile:{0}\r\n",mobile);
                    contactTel = alSourceStr[2];
                    strLog.AppendFormat("contactTel:{0}\r\n",contactTel);
                    email = alSourceStr[3];
                    strLog.AppendFormat("email:{0}\r\n",email);
                    sex = alSourceStr[4];
                    strLog.AppendFormat("sex:{0}\r\n",sex);
                    cerType = alSourceStr[5];
                    strLog.AppendFormat("cerType:{0}\r\n",cerType);
                    cerNum = alSourceStr[6];
                    strLog.AppendFormat("cerNum:{0}\r\n",cerNum);
                }
                catch (System.Exception e)
                {
                    Result.Result = -7020;
                    Result.ErrMsg = "解密错误!"+e.ToString();
                    return Result;
                }

            }
            else {
                Result.Result = -7020;
                Result.ErrMsg = "hashcode校验不通过!";
                return Result;
            }

            
            strLog.AppendFormat("Begin 开通号码百事通账户\r\n");
            strLog.AppendFormat("SPID:{0},CustID:{1},mobile:{2},realName:{3},contactTel:{4},email:{5},sex:{6},cerType:{7},cerNum:{8}\r\n", SPID, CustID, mobile, realName, contactTel, email, sex, cerType, cerNum);

            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "OpenBesttoneAccount", this.Context, out Result.ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Msg;
                return Result;
            }
            if (CommonUtility.IsEmpty(mobile))
            {
                Result.Result = -7015;
                Result.ErrMsg = "账户名不能为空!";
                return Result;
            }
            if (!Utils.isMobilePhone(mobile))
            {
                Result.Result = -7015;
                Result.ErrMsg = "无效的手机号码!";
                return Result;
            }

            if (CommonUtility.IsEmpty(realName))
            {
                Result.Result = -7016;
                Result.ErrMsg = "用户名不能为空!";
                return Result;
            }
            if (CommonUtility.IsEmpty(sex))
            {
                Result.Result = -7017;
                Result.ErrMsg = "性别不能为空!";
                return Result;
            }
            if (!Utils.IsNumeric(sex))
            {
                Result.Result = -7017;
                Result.ErrMsg = "性别只能为数字!";
                return Result;
            }
            if ("0".Equals(sex) || "1".Equals(sex) || "2".Equals(sex))
            {
            }
            else
            {
                Result.Result = -7017;
                Result.ErrMsg = "性别只能为0和1,2!";
                return Result;
            }

            if (CommonUtility.IsEmpty(cerType))
            {
                Result.Result = -7018;
                Result.ErrMsg = "证件类型不能为空!";
                return Result;
            }

            if (cerType.Equals("1") || cerType.Equals("2") || cerType.Equals("3") || cerType.Equals("4") || cerType.Equals("5") || cerType.Equals("6") || cerType.Equals("7") || cerType.Equals("8") || cerType.Equals("9") || cerType.Equals("10") || cerType.Equals("X"))
            { }
            else
            {
                Result.Result = -7018;
                Result.ErrMsg = "非法证件类型!";
                return Result;
            }

            if (CommonUtility.IsEmpty(cerNum))
            {
                Result.Result = -7019;
                Result.ErrMsg = "证件号不能为空!";
                return Result;
            }


            if ("X".Equals(cerType))
            {
                if ("99999".Equals(cerNum))
                {
                }
                else
                {
                    Result.Result = -7019;
                    Result.ErrMsg = "证件类型为其他类型(X)，则证件号必须是99999!";
                    return Result;
                }
            }

            if ("1".Equals(cerType))
            {
                if (!CommonUtility.CheckIDCard(cerNum))
                {
                    Result.Result = -7020;
                    Result.ErrMsg = "身份证不合法!";
                    return Result;
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
                int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(mobile, out ai, out ResponseCode, out Result.ErrMsg);
                strLog.AppendFormat("BesttoneAccountHelper.BesttoneAccountInfoQuery QueryBesttoneAccountResult:{0},ErrMsg:{1}\r\n ", QueryBesttoneAccountResult, Result.ErrMsg);
                if (QueryBesttoneAccountResult == 0)
                {
                    if ("200010".Equals(ResponseCode))   // 未开户
                    {
                        strLog.AppendFormat("未绑定且未开户\r\n");
                        strLog.AppendFormat("准备去开户了\r\n");
                        strLog.AppendFormat("开户前日志,参数 SPID:{0},TransactionID:{1},CustID:{2},mobile:{3}", SPID, TransactionID, CustID, mobile);
                        UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  Result.ErrMsg);  //日志
                        strLog.AppendFormat("开户前日志完成\r\n");
                        strLog.AppendFormat("开户......\r\n");
                        strLog.AppendFormat("开户参数:mobile:{0},realName:{1},contactTel:{2},email:{3},sex:{4},cerType:{5},cerNum:{6},TransactionID:{7}", mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID);
                        Result.Result = BesttoneAccountHelper.RegisterBesttoneAccount(mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID, out Result.ErrMsg);
                        strLog.AppendFormat("开户后返回的状态 Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
                        //绑定操作
                        strLog.AppendFormat("开完户准备进行绑定,将{0}绑定至{1}\r\n", mobile, CustID);
                        UserRegistry.CreateBesttoneAccount(SPID, CustID, mobile, out Result.ErrMsg);
                        strLog.AppendFormat("绑定后结果ErrMsg:{0}\r\n", Result.ErrMsg);
                        UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  Result.ErrMsg); //日志
                        strLog.AppendFormat("开户后日志\r\n");
                        UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, cerNum, out Result.ErrMsg);
                        strLog.AppendFormat("开户完成\r\n");
                    }
                    else
                    {  // 已开户
                        //绑定操作
                        if ("000000".Equals(ResponseCode))
                        {
                            strLog.AppendFormat("未绑定且 已开户\r\n");
                            strLog.AppendFormat("仅绑定.......\r\n");
                            UserRegistry.CreateBesttoneAccount(SPID, CustID, mobile, out Result.ErrMsg);
                            strLog.AppendFormat("将{0}绑定到{1}\r\n", mobile, CustID);
                            UserRegistry.OnlyBindingBesttoneAccount(SPID, TransactionID, CustID, mobile, out  Result.ErrMsg);
                            strLog.AppendFormat("记录绑定日志表,流水号:{0}", TransactionID);
                            strLog.AppendFormat("绑定后结果ErrMsg:{0}\r\n", Result.ErrMsg);
                        }
                        else
                        {
                            Result.Result = -25679;
                            Result.ErrMsg = ResponseCode;
                            return Result;
                        }
                    }
                }
                else
                {

                    Result.Result = QueryBesttoneAccountResult;
                    return Result;
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
                    Result.Result = -12300;
                    Result.ErrMsg = mobile + "手机号已为其他客户(" + CustID + ")开通了号码百事通账户，您可以登录系统，进入您的账户中心，用另一手机号码开通号码百事通账户，也可以咨询客服人员帮助排查问题。";
                }
                strLog.AppendFormat("{0}没有绑定到其他人身上,此人的CustID:{1}\r\n", mobile, CustID);
                strLog.AppendFormat("去翼支付查询该账户号{0}是否存在\r\n", mobile);
                int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(mobile, out ai, out ResponseCode, out Result.ErrMsg);
                if (QueryBesttoneAccountResult != 0)   // 未开户
                {
                    strLog.AppendFormat("翼支付查询返回说该账户号{0}不存在\r\n", mobile);
                    strLog.AppendFormat("准备去为{0}开户........\r\n", mobile);
                    strLog.AppendFormat("开户前日志,参数 SPID:{0},TransactionID:{1},CustID:{2},mobile:{3}", SPID, TransactionID, CustID, mobile);
                    UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  Result.ErrMsg);  //日志
                    strLog.AppendFormat("开户前日志完成\r\n");
                    strLog.AppendFormat("开户........\r\n");
                    strLog.AppendFormat("开户参数:mobile:{0},realName:{1},contactTel:{2},email:{3},sex:{4},cerType:{5},cerNum:{6},TransactionID:{7}\r\n", mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID);
                    Result.Result = BesttoneAccountHelper.RegisterBesttoneAccount(mobile, realName, contactTel, email, sex, cerType, cerNum, TransactionID, out Result.ErrMsg);
                    strLog.AppendFormat("开户完成，返回结果:Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
                    //绑定操作
                    strLog.AppendFormat("绑定{0}到{1}\r\n", mobile, CustID);
                    UserRegistry.CreateBesttoneAccount(SPID, CustID, mobile, out Result.ErrMsg);
                    strLog.AppendFormat("绑定完成,返回结果 ErrMsg:{0}\r\n", Result.ErrMsg);
                    strLog.AppendFormat("开户后日志\r\n");
                    UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, mobile, out  Result.ErrMsg); //日志
                    strLog.AppendFormat("开户后日志完成 ErrMsg:{0}\r\n", Result.ErrMsg);
                    UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, realName, cerNum, out Result.ErrMsg);
                }
            }
            strLog.AppendFormat("End 开通号码百事通账户 Result:{0},ErrMsg{1}\r\n", Result.Result, Result.ErrMsg);
            //-99999 失败 0 成功

        }
        catch (Exception e)
        {
            Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + e.Message;
        }
        finally
        {
            log(strLog.ToString());
        }
        return Result;
    }


    public class CustInfoQueryResult
    {
        public string returnCode;
        public string msg;
        public CustInfoQueryData data;
    }
    public class CustInfoQueryData
    {
        public string Id;
        public string CustId;
        public string BesttoneAccount;
        public string RealName;
        public string CertificateType;
        public string CertificateCode;

    }
    [WebMethod(Description = "号码百事通账户客户信息查询-xml")]
    public CustInfoQueryResult GetCustInfo(String Request)
    {
        CustInfoQueryResult custInfoQueryResult = new CustInfoQueryResult();
        String ReturnCode = "0";
        String Descriptioin = "成功";
        StringBuilder Response = new StringBuilder();
        Response.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        #region
        XmlDocument xmlDoc = new XmlDocument();
        String version = "";
        String SPID = "";
        String CustID = "";
        String BA = "";
        try
        {
            xmlDoc.LoadXml(Request);
            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CUSTID")[0];
            CustID = CustIDNode.Attributes["value"].Value;

            XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
            BA = BesttoneAccountNode.Attributes["value"].Value;
        }
        catch (Exception e)
        {
            ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
            Descriptioin = "客户信息询失败!-xml格式错误:"+e.ToString();
            //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            custInfoQueryResult.returnCode = ReturnCode;
            custInfoQueryResult.msg = Descriptioin;
            return custInfoQueryResult;
        }

      

        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                custInfoQueryResult.returnCode = ReturnCode;
                custInfoQueryResult.msg = Descriptioin;
                return custInfoQueryResult;
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                custInfoQueryResult.returnCode = ReturnCode;
                custInfoQueryResult.msg = Descriptioin;
                return custInfoQueryResult;

            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetCustInfo", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                custInfoQueryResult.returnCode = ReturnCode;
                custInfoQueryResult.msg = Descriptioin;
                return custInfoQueryResult;
            }

            if (CommonUtility.IsEmpty(CustID) && CommonUtility.IsEmpty(BA))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "CustID和BesttoneAccount不能同时为空!";
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                custInfoQueryResult.returnCode = ReturnCode;
                custInfoQueryResult.msg = Descriptioin;
                return custInfoQueryResult;
            }
            BesttoneAccount account = null;
            BesttoneAccountDAO dao = new BesttoneAccountDAO();

            if (!CommonUtility.IsEmpty(CustID))
            {
                account = dao.QueryByCustID(CustID);
            }

            if (!CommonUtility.IsEmpty(BA))
            {
                account = dao.QueryByBestAccount(BA);
            }

            if (account == null)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户不存在!";
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                custInfoQueryResult.returnCode = ReturnCode;
                custInfoQueryResult.msg = Descriptioin;
                return custInfoQueryResult;
            }
            Linkage.BestTone.Interface.Rule.CustInfo custInfo = new Linkage.BestTone.Interface.Rule.CustInfo();
            int QueryCustInfoResult = BesttoneAccountHelper.QueryCustInfo(account.BestPayAccount, out custInfo, out ErrMsg);
            if (QueryCustInfoResult == 0)
            {
                custInfoQueryResult.returnCode = ReturnCode;
                custInfoQueryResult.msg = Descriptioin;
                CustInfoQueryData data = new CustInfoQueryData();

                //Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                //Response.AppendFormat("<Data ID = \"\">");
                //Response.AppendFormat("<CUSTID>{0}</CUSTID>", account.CustID);
                //Response.AppendFormat("<BESTTONEACCOUNT>{0}</BESTTONEACCOUNT>", custInfo.ProductNo);
                //Response.AppendFormat("<REALNAME>{0}</REALNAME>", custInfo.CustomerName);
                //Response.AppendFormat("<CERTIFICATETYPE>{0}</CERTIFICATETYPE>", custInfo.IdType);
                //Response.AppendFormat("<CERTIFICATECODE>{0}</CERTIFICATECODE>", custInfo.IdNo);
                //Response.AppendFormat("</Data>");
                //Response.AppendFormat("</Result>");
                data.Id = "";
                data.CustId = account.CustID;
                data.BesttoneAccount = custInfo.ProductNo;
                data.RealName = custInfo.CustomerName;
                data.CertificateType = custInfo.IdType;
                data.CertificateCode = custInfo.IdNo;
                custInfoQueryResult.data = data;
            }
            #endregion
        }
        catch (Exception e)
        {
            ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
            Descriptioin = "客户信息询失败!";
            //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            custInfoQueryResult.returnCode = ReturnCode;
            custInfoQueryResult.msg = Descriptioin+e.ToString();
        }
        //return Response.ToString();
        return custInfoQueryResult;
    }

    //public class CustInfoQueryResult
    //{
    //    public Int32 Result;
    //    public String ErrMsg;
    //    public String CustID;
    //    public String RealName;
    //    public String CertificateType;
    //    public String CertificateCode;
    //}
    //[WebMethod(Description = "号码百事通账户客户信息查询")]
    //public CustInfoQueryResult CustInfoQuery(string SPID, string ProductNo)
    //{
    //    CustInfoQueryResult Result = new CustInfoQueryResult();
    //    Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    //    Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
    //    StringBuilder strLog = new StringBuilder();

    //    try
    //    {
    //        strLog.AppendFormat("Begin 号码百事通账户客户信息查询:\r\n");
    //        #region 数据合法性判断
    //        if (CommonUtility.IsEmpty(SPID))
    //        {
    //            Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
    //            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
    //            return Result;
    //        }
    //        //IP是否允许访问
    //        Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
    //        strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
    //        strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
    //        if (Result.Result != 0)
    //        {
    //            return Result;
    //        }
    //        //接口访问权限判断
    //        Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "CustInfoQuery", this.Context, out Result.ErrMsg);
    //        strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
    //        if (Result.Result != 0)
    //        {
    //            return Result;
    //        }
    //        if (CommonUtility.IsEmpty(ProductNo))
    //        {
    //            Result.Result = -7015;
    //            Result.ErrMsg = "账户名不能为空!";
    //            return Result;
    //        }
            
    //        #endregion
    //        Linkage.BestTone.Interface.Rule.CustInfo custInfo = new Linkage.BestTone.Interface.Rule.CustInfo();
    //        strLog.AppendFormat("去翼支付查询{0}的客户信息\r\n", ProductNo);
    //        int QueryCustInfoResult = BesttoneAccountHelper.QueryCustInfo(ProductNo, out custInfo, out Result.ErrMsg);
    //        strLog.AppendFormat("查询后返回结果:QueryCustinoResult:{0},ErrMsg:{1}\r\n", QueryCustInfoResult, Result.ErrMsg);
    //        strLog.AppendFormat("账户信息:CustomerNo:{0}, ProductNo:{1}, custinfo.CustomerName:{2}, custinfo.IdType:{3}, custinfo.IdNo:{4}\r\n", custInfo.CustomerNo, custInfo.ProductNo, custInfo.CustomerName, custInfo.IdType, custInfo.IdNo);
    //        Result.CertificateCode = custInfo.IdNo;
    //        Result.CertificateType = custInfo.IdType;
    //        Result.RealName = custInfo.CustomerName;

    //        BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
    //        Linkage.BestTone.Interface.Rule.BesttoneAccount obj_BesttoneAccount = _besttoneAccount_dao.QueryByBestAccount(ProductNo);
    //        if (obj_BesttoneAccount != null)
    //        {
    //            Result.CustID = obj_BesttoneAccount.CustID;
    //        }
            
    //        strLog.AppendFormat("End 号码百事通账户客户信息查询:\r\n");
    //    }
    //    catch (Exception ex)
    //    {
    //        Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
    //        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + ex.Message;
    //    }
    //    finally
    //    {
    //        log(strLog.ToString());
    //    }
    //    return Result;
    //}


    #region 注销账户
    public class CancelBesttoneAccountResult
    {
        public Int32 Result;
        public String ErrMsg;
    }

    [WebMethod(Description = "注销号码百事通账户")]
    public CancelBesttoneAccountResult CancelBesttoneAccount(string SPID, string BesttoneAccount)
    {
        CancelBesttoneAccountResult Result = new CancelBesttoneAccountResult();
        Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        StringBuilder strLog = new StringBuilder();
       
        try
        {
            strLog.AppendFormat("Begin 注销号码百事通账户:\r\n");
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return Result;
            }
            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }
            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "CancelBesttoneAccount", this.Context, out Result.ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }
            if (CommonUtility.IsEmpty(BesttoneAccount))
            {
                Result.Result = -7015;
                Result.ErrMsg = "账户名不能为空!";
                return Result;
            }
            if (!Utils.isMobilePhone(BesttoneAccount))
            {
                Result.Result = -7015;
                Result.ErrMsg = "无效的手机号码!";
                return Result;
            }
            #endregion
            Linkage.BestTone.Interface.Rule.CustInfo custInfo = new Linkage.BestTone.Interface.Rule.CustInfo();
            strLog.AppendFormat("去翼支付查询{0}的客户信息\r\n",BesttoneAccount);
            int QueryCustInfoResult = BesttoneAccountHelper.QueryCustInfo(BesttoneAccount, out custInfo, out Result.ErrMsg);
            strLog.AppendFormat("查询后返回结果:QueryCustinoResult:{0},ErrMsg:{1}\r\n",QueryCustInfoResult,Result.ErrMsg);
            strLog.AppendFormat("账户信息:CustomerNo:{0}, ProductNo:{1}, custinfo.CustomerName:{2}, custinfo.IdType:{3}, custinfo.IdNo:{4}\r\n", custInfo.CustomerNo, custInfo.ProductNo, custInfo.CustomerName, custInfo.IdType, custInfo.IdNo);
            strLog.AppendFormat("销户...");
            int CancelBesttoneAccountResult = BesttoneAccountHelper.CancelBesttoneAccount(custInfo.CustomerNo, custInfo.ProductNo, custInfo.CustomerName, custInfo.IdType, custInfo.IdNo, out  Result.ErrMsg);
            strLog.AppendFormat("销户返回结果:CancelBesttoneAccountResult{0},ErrMsg:{1}", CancelBesttoneAccountResult,Result.ErrMsg);
            String ResponseCode = "";
            AccountItem ai = new AccountItem();
            int QueryBesttoneAccountResult = BesttoneAccountHelper.BesttoneAccountInfoQuery(BesttoneAccount, out ai, out ResponseCode, out Result.ErrMsg);
            if (QueryBesttoneAccountResult != 0) {
                BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
                bool ret = _besttoneAccount_dao.Delete(BesttoneAccount);
                if (ret)
                {
                    strLog.AppendFormat("解绑成功\r\n");
                }
                else {
                    strLog.AppendFormat("解绑失败\r\n");
                }
            }
            strLog.AppendFormat("End 注销号码百事通账户:\r\n");
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + ex.Message;
        }
        finally {
            log(strLog.ToString());
        }
        return Result;
    }

    #endregion


    [WebMethod(Description = "重置支付密码")]
    public String ResetPayPassWord(String Request)
    {
        String ReturnCode = "";
        String Descriptioin = "";
        StringBuilder Response = new StringBuilder();
        Response.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        #region

        XmlDocument xmlDoc = new XmlDocument();
        String version = "";
        String SPID = "";
        String BA = "";
        try
        {
            xmlDoc.LoadXml(Request);
            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
            BA = BesttoneAccountNode.Attributes["value"].Value;
        }
        catch (Exception e)
        {
            ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
            Descriptioin = "重置密码失败!-xml格式错误:" + e.ToString();
            Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
        }

        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();

            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetAccountInfo", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

        
            BesttoneAccount account = null;
            BesttoneAccountDAO dao = new BesttoneAccountDAO();
        
            if (!CommonUtility.IsEmpty(BA))
            {
                account = dao.QueryByBestAccount(BA);
            }

            if (account == null)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户不存在!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

            

            Linkage.BestTone.Interface.Rule.CustInfo custInfo = new Linkage.BestTone.Interface.Rule.CustInfo();
            int QueryCustInfoResult = BesttoneAccountHelper.QueryCustInfo(account.BestPayAccount, out custInfo, out ErrMsg);
            if (QueryCustInfoResult == 0)
            {
                int ResetBesttoneAccountPayPasswordResult = BesttoneAccountHelper.ResetBesttoneAccountPayPassword(account.BestPayAccount, custInfo.IdType, custInfo.IdNo, custInfo.CustomerName, out ErrMsg);
                if (ResetBesttoneAccountPayPasswordResult == 0)
                {
                    Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                    Response.AppendFormat("</Result>");
                }
                else
                {
                    ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                    Descriptioin = "重置密码失败!";
                    Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                
                }
            }
            else {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "无此账户!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            }
            #endregion
        }
        catch (Exception e)
        {
            ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
            Descriptioin = "重置密码失败!"+e.ToString();
            Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
        }
        return Response.ToString();
    }

    public class ResetPayPassWordResult
    {
        public string returnCode;
        public string msg;
    }

    [WebMethod(Description = "重置支付密码-xml")]
    public ResetPayPassWordResult ResetPayPassWordV2(String Request)
    {
        ResetPayPassWordResult resetPayPassWordResult = new ResetPayPassWordResult();
        String ReturnCode = "";
        String Descriptioin = "";
        StringBuilder Response = new StringBuilder();
        Response.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        #region

        XmlDocument xmlDoc = new XmlDocument();
        String version = "";
        String SPID = "";
        String BA = "";
        try
        {
            xmlDoc.LoadXml(Request);
            XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
            version = versionNode.Attributes["value"].Value;

            XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
            SPID = SPIDNode.Attributes["value"].Value;

            XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
            BA = BesttoneAccountNode.Attributes["value"].Value;
        }
        catch (Exception e)
        {
            ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
            Descriptioin = "重置密码失败!-xml格式错误:" + e.ToString();
            //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            resetPayPassWordResult.returnCode = ReturnCode;
            resetPayPassWordResult.msg = Descriptioin;
            return resetPayPassWordResult;
        }

        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                ///Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                resetPayPassWordResult.returnCode = ReturnCode;
                resetPayPassWordResult.msg = Descriptioin;
                return resetPayPassWordResult;
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                resetPayPassWordResult.returnCode = ReturnCode;
                resetPayPassWordResult.msg = Descriptioin;
                return resetPayPassWordResult;
            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "ResetPayPassWordV2", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                resetPayPassWordResult.returnCode = ReturnCode;
                resetPayPassWordResult.msg = Descriptioin;
                return resetPayPassWordResult;
            }


            BesttoneAccount account = null;
            BesttoneAccountDAO dao = new BesttoneAccountDAO();

            if (!CommonUtility.IsEmpty(BA))
            {
                account = dao.QueryByBestAccount(BA);
            }

            if (account == null)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户不存在!";
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                resetPayPassWordResult.returnCode = ReturnCode;
                resetPayPassWordResult.msg = Descriptioin;
                return resetPayPassWordResult;
            }



            Linkage.BestTone.Interface.Rule.CustInfo custInfo = new Linkage.BestTone.Interface.Rule.CustInfo();
            int QueryCustInfoResult = BesttoneAccountHelper.QueryCustInfo(account.BestPayAccount, out custInfo, out ErrMsg);
            if (QueryCustInfoResult == 0)
            {
                int ResetBesttoneAccountPayPasswordResult = BesttoneAccountHelper.ResetBesttoneAccountPayPassword(account.BestPayAccount, custInfo.IdType, custInfo.IdNo, custInfo.CustomerName, out ErrMsg);
                if (ResetBesttoneAccountPayPasswordResult == 0)
                {
                    //Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                    //Response.AppendFormat("</Result>");
                    resetPayPassWordResult.returnCode = "0";
                    resetPayPassWordResult.msg = "成功";
                    return resetPayPassWordResult;
                }
                else
                {
                    ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                    Descriptioin = "重置密码失败!";
                    //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                    resetPayPassWordResult.returnCode = ReturnCode;
                    resetPayPassWordResult.msg = Descriptioin;
                    return resetPayPassWordResult;
                }
            }
            else
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "无此账户!";
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                resetPayPassWordResult.returnCode = ReturnCode;
                resetPayPassWordResult.msg = Descriptioin;
                return resetPayPassWordResult;
            }
            #endregion
        }
        catch (Exception e)
        {
            ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
            Descriptioin = "重置密码失败!" + e.ToString();
            //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            resetPayPassWordResult.returnCode = ReturnCode;
            resetPayPassWordResult.msg = Descriptioin;
            return resetPayPassWordResult;
        }
        return resetPayPassWordResult;
        //return Response.ToString();
    }

    //#region 重置支付密码

    //public class ResetPayPassWordResult
    //{
    //    public Int32 Result;
    //    public String ErrMsg;
    //}

    //[WebMethod(Description = "重置支付密码")]
    //public ResetPayPassWordResult ResetPayPassWordV1(String SPID,String BesttoneAccount)
    //{
    //    ResetPayPassWordResult Result = new ResetPayPassWordResult();
    //    Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
    //    Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg; 
    //    String CustomerName = "";
    //    String IDType = "";
    //    String IDNo = "";

    //    StringBuilder strLog = new StringBuilder();

    //    try
    //    {

    //        strLog.AppendFormat("Begin 重置支付密码:\r\n");
    //        #region 数据合法性判断
    //        if (CommonUtility.IsEmpty(SPID))
    //        {
    //            Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
    //            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
    //            return Result;
    //        }
    //        //IP是否允许访问
    //        Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
    //        strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
    //        strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
    //        if (Result.Result != 0)
    //        {
    //            return Result;
    //        }
    //        //接口访问权限判断
    //        Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "ResetPayPassWord", this.Context, out Result.ErrMsg);
    //        strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result.Result, Result.ErrMsg);
    //        if (Result.Result != 0)
    //        {
    //            return Result;
    //        }
    //        if (CommonUtility.IsEmpty(BesttoneAccount))
    //        {
    //            Result.Result = -7015;
    //            Result.ErrMsg = "账户名不能为空!";
    //            return Result;
    //        }
    //        if (!Utils.isMobilePhone(BesttoneAccount))
    //        {
    //            Result.Result = -7016;
    //            Result.ErrMsg = "无效的手机号码!";
    //            return Result;
    //        }
    //        #endregion

    //        Linkage.BestTone.Interface.Rule.CustInfo custinfo = new Linkage.BestTone.Interface.Rule.CustInfo();
    //        strLog.AppendFormat("去翼支付查询{0}的客户信息\r\n", BesttoneAccount);
    //        String QueryCustInfoErrorInfo = "";
    //        int QueryCustInfoResult = BesttoneAccountHelper.QueryCustInfo(BesttoneAccount, out custinfo, out QueryCustInfoErrorInfo);
    //        strLog.AppendFormat("查询后返回结果:QueryCustinoResult:{0},ErrMsg:{1}\r\n", QueryCustInfoResult, Result.ErrMsg);
    //        strLog.AppendFormat("账户信息:CustomerNo:{0}, BesttoneAccount:{1}, custinfo.CustomerName:{2}, custinfo.IdType:{3}, custinfo.IdNo:{4}\r\n", custinfo.CustomerNo, custinfo.ProductNo, custinfo.CustomerName, custinfo.IdType, custinfo.IdNo);
    //        if (QueryCustInfoResult == 0)
    //        {
    //            strLog.Append("查询返回0，继续下一步重置密码\r\n");
    //            IDType = custinfo.IdType;
    //            IDNo = custinfo.IdNo;
    //            CustomerName = custinfo.CustomerName;
    //            String ResetErrorInfo = "";
    //            int ResetBesttoneAccountPayPasswordResult = BesttoneAccountHelper.ResetBesttoneAccountPayPassword(BesttoneAccount, IDType, IDNo, CustomerName, out ResetErrorInfo);
    //            strLog.AppendFormat("重置密码结果:{0},{1}\r\n", ResetBesttoneAccountPayPasswordResult, ResetErrorInfo);
    //            if (ResetBesttoneAccountPayPasswordResult == 0)
    //            {
    //                Result.Result = 0;
    //                Result.ErrMsg = "密码重置成功！" + ResetErrorInfo;
    //            }
    //            else
    //            {
    //                Result.Result = -20101;
    //                Result.ErrMsg = "密码重置失败！" + ResetErrorInfo;
    //            }
    //        }
    //        else
    //        {
    //            Result.Result = -20102;
    //            Result.ErrMsg = "账户不存在！" + QueryCustInfoErrorInfo;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
    //        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + e.Message;
   
    //    }
    //    finally
    //    {
    //        log(strLog.ToString());
    //    }

    //    return Result;
    //}

    //#endregion

    public class GetAccountInfoResult
    {
        public String ReturnCode;
        public String Msg;
        public AccountInfoData data;

    }


    public class AccountInfoData
    {
        public String Id;
        public String ReturnCode;
        public String Msg;
        public String CustID;
        public String BesttoneAccount;
        public String AccountType;
        public String AccountStatus;
        public long AccountBalance;
        public long PredayBalance;
        public long PremonthBalance;
        public long AvailableBalance;
        public long UnavailableBalance;
        public long AvailableCash;
        public long UnailableCash;
        public String CardNum;
        public String CardType;
    }

    /// <summary>
    /// 二、	账户信息查询接口
    /// </summary>
    /// <param name="Request"></param>
    /// <returns></returns>
    [WebMethod(Description = "账户信息查询接口")]
    public GetAccountInfoResult GetAccountInfo(String Request)
    {
        GetAccountInfoResult getAccountInfoResult = new GetAccountInfoResult();
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

        XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CUSTID")[0];
        String CustID = CustIDNode.Attributes["value"].Value;

        XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
        String BA = BesttoneAccountNode.Attributes["value"].Value;
        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getAccountInfoResult.ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                getAccountInfoResult.Msg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return getAccountInfoResult;
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getAccountInfoResult.ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                getAccountInfoResult.Msg = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                return getAccountInfoResult;
            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetAccountInfo", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getAccountInfoResult.ReturnCode = ReturnCode;
                getAccountInfoResult.Msg = Descriptioin;
                return getAccountInfoResult;
            }

            if (CommonUtility.IsEmpty(CustID) && CommonUtility.IsEmpty(BA))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "CustID和BesttoneAccount不能同时为空!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getAccountInfoResult.ReturnCode = ReturnCode;
                getAccountInfoResult.Msg = Descriptioin;
                return getAccountInfoResult;
            }
            BesttoneAccount account = null;
            BesttoneAccountDAO dao = new BesttoneAccountDAO();

            if (!CommonUtility.IsEmpty(CustID))
            {
                account = dao.QueryByCustID(CustID);            
            }

            if (!CommonUtility.IsEmpty(BA))
            {
                account = dao.QueryByBestAccount(BA);
            }

            if (account == null)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户不存在!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getAccountInfoResult.ReturnCode = ReturnCode;
                getAccountInfoResult.Msg = Descriptioin;
                return getAccountInfoResult;
            }
          
            //查询账户余额
            AccountItem item;
            Result = BesttoneAccountHelper.QueryBesttoneAccount(account.BestPayAccount, out item, out ErrMsg);
            if (Result == 0)
            {
                AccountInfoData data = new AccountInfoData();
                getAccountInfoResult.ReturnCode = "0";
                getAccountInfoResult.Msg = "成.功";
                data.Id = "";
                data.CustID = account.CustID;
                data.BesttoneAccount = account.BestPayAccount;
                data.AccountType = item.AccountType;
                data.AccountStatus = item.AccountStatus;
                data.AccountBalance = item.AccountBalance;
                data.PredayBalance = item.PredayBalance;
                data.PremonthBalance = item.PreMonthBalance;
                data.AvailableBalance = item.AvailableBalance;
                data.UnavailableBalance = item.UnAvailableBalance;
                data.AvailableCash = item.AvailableLecash;
                data.CardNum = item.CardNum;
                data.CardType = item.CardType;
                getAccountInfoResult.data = data;
                //Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                //Response.AppendFormat("<Data ID = \"\">");
                //Response.AppendFormat("<CUSTID>{0}</CUSTID>", account.CustID);
                //Response.AppendFormat("<BESTTONEACCOUNT>{0}</BESTTONEACCOUNT>", account.BestPayAccount);
                //Response.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", item.AccountType);

                //Response.AppendFormat("<ACCOUNTSTATUS>{0}</ACCOUNTSTATUS>", item.AccountStatus);
                //Response.AppendFormat("<ACCOUNTBALANCE>{0}</ACCOUNTBALANCE>", item.AvailableBalance);
                //Response.AppendFormat("<PREDAYBALANCE>{0}</PREDAYBALANCE>", item.PredayBalance);
                //Response.AppendFormat("<PREMONTHBALANCE>{0}</PREMONTHBALANCE>", item.PreMonthBalance);
                //Response.AppendFormat("<AVAILABLEBALANCE>{0}</AVAILABLEBALANCE>", item.AvailableBalance);
                //Response.AppendFormat("<UNAVAILABLEBALANCE>{0}</UNAVAILABLEBALANCE>", item.UnAvailableBalance);
                //Response.AppendFormat("<AVAILABLECASH>{0}</AVAILABLECASH>", item.AvailableLecash);
                //Response.AppendFormat("<CARDNUM>{0}</CARDNUM>", item.CardNum);
                //Response.AppendFormat("<CARDTYPE>{0}</CARDTYPE>", item.CardType);
                //Response.AppendFormat("</Data>");
                //Response.AppendFormat("</Result>");
               
            }
            else {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户查询失败!";
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                getAccountInfoResult.ReturnCode = ReturnCode;
                getAccountInfoResult.Msg = Descriptioin;
            }
            #endregion
        }
        catch (Exception e)
        {
            getAccountInfoResult.ReturnCode = "-9001";
            getAccountInfoResult.Msg = e.ToString();
        }
        //return Response.ToString();
        return getAccountInfoResult;
    }


    public class AccountInfoQueryV2Result
    {
        public Int32 Result;
        public String ErrMsg;
        public String CustID;
        public String Account;

        public String AccountNo;
        public String AccountName;
        public String AccountType;
        public String AccountStatus;
        public String AccountBalance;
        public String PredayBalance;
        public String PreMonthBalance;
        public String AvailableBalance;
        public String UnAvailableBalance;
        public String AvailableLecash;
        public String CardNum;
        public String CardType;
    
    }
    [WebMethod(Description = "账户信息查询V2")]
    public AccountInfoQueryV2Result AccountInfoV2Query(String SPID, String BesttoneAccount)
    {
        AccountInfoQueryV2Result Result = new AccountInfoQueryV2Result();
        Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        Result.Account = BesttoneAccount;
      
        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AccountInfoV2Query", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(BesttoneAccount))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Msg;
                return Result;
            }

            #endregion

            BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
            BesttoneAccount entity = _besttoneAccount_dao.QueryByBestAccount(BesttoneAccount);
            if (entity == null)
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Msg;
            }
            else
            {
                
                Result.CustID = entity.CustID;
                //查询账户余额
                AccountItem item;
                Result.Result = BesttoneAccountHelper.QueryBesttoneAccount(entity.BestPayAccount, out item, out Result.ErrMsg);
                if (Result.Result == 0)
                {
                    Result.AccountNo = item.AccountNo;
                    Result.AccountName = item.AccountName;
                    Result.AccountType = item.AccountType;
                    Result.AccountStatus = item.AccountStatus;
                    Result.AccountBalance = item.AccountBalance.ToString();
                    Result.PredayBalance = item.PredayBalance.ToString();
                    Result.PreMonthBalance = item.PreMonthBalance.ToString();
                    Result.AvailableBalance = item.AvailableBalance.ToString();
                    Result.UnAvailableBalance = item.UnAvailableBalance.ToString();
                    Result.AvailableLecash = item.AvailableLecash.ToString();
                    Result.CardNum = item.CardNum;
                    Result.CardType = item.CardType;
                }
            }

        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + ex.Message;
          
        }
        finally
        {

        }
        return Result;
    }

    #region 账户查询

    public class AccountInfoQueryResult
    {
        public Int32 Result;
        public String ErrMsg;
        public String CustID;
        public String Account;

        public String AccountNo;
        public String AccountName;
        public String AccountType;
        public String AccountStatus;
        public String AccountBalance;
        public String PredayBalance;
        public String PreMonthBalance;
        public String AvailableBalance;
        public String UnAvailableBalance;
        public String AvailableLecash;
        public String CardNum;
        public String CardType;
    }
    /// <summary>
    /// 账户信息查询
    /// </summary>
    [WebMethod(Description = "账户信息查询")]
    public AccountInfoQueryResult AccountInfoQuery(String SPID, String CustID)
    {
        AccountInfoQueryResult Result = new AccountInfoQueryResult();
        Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        Result.CustID = CustID;
        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AccountInfoQuery", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Msg;
                return Result;
            }

            #endregion

            BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
            BesttoneAccount entity = _besttoneAccount_dao.QueryByCustID(CustID);
            if (entity == null)
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Msg;
            }
            else
            {
                Result.Account = entity.BestPayAccount;
                //查询账户余额
                AccountItem item;
                Result.Result = BesttoneAccountHelper.QueryBesttoneAccount(entity.BestPayAccount, out item, out Result.ErrMsg);
                if (Result.Result == 0)
                {
                    Result.AccountNo = item.AccountNo;
                    Result.AccountName = item.AccountName;
                    Result.AccountType = item.AccountType;
                    Result.AccountStatus = item.AccountStatus;
                    Result.AccountBalance = item.AccountBalance.ToString();
                    Result.PredayBalance = item.PredayBalance.ToString();
                    Result.PreMonthBalance = item.PreMonthBalance.ToString();
                    Result.AvailableBalance = item.AvailableBalance.ToString();
                    Result.UnAvailableBalance = item.UnAvailableBalance.ToString();
                    Result.AvailableLecash = item.AvailableLecash.ToString();
                    Result.CardNum = item.CardNum;
                    Result.CardType = item.CardType;
                }
            }

        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + ex.Message;
        }
        finally
        {

        }
        return Result;
    }

    [WebMethod(Description = "账户余额查询")]
    public String GetAccountBalance(String Request)
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

        XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CUSTID")[0];
        String CustID = CustIDNode.Attributes["value"].Value;

        XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
        String BA = BesttoneAccountNode.Attributes["value"].Value;
        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();

            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetAccountBalance", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

            if (CommonUtility.IsEmpty(CustID) && CommonUtility.IsEmpty(BA))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "CustID和BesttoneAccount不能同时为空!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }
            BesttoneAccount account = null;
            BesttoneAccountDAO dao = new BesttoneAccountDAO();

            if (!CommonUtility.IsEmpty(CustID))
            {
                account = dao.QueryByCustID(CustID);
            }

            if (!CommonUtility.IsEmpty(BA))
            {
                account = dao.QueryByBestAccount(BA);
            }

            if (account == null)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户不存在!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

            //查询账户余额
            long balance = 0;
            Int32 QueryAccountBalanceResult = BesttoneAccountHelper.QueryAccountBalance(account.BestPayAccount, out balance, out ErrMsg);

            if (QueryAccountBalanceResult == 0)
            {
                Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                Response.AppendFormat("<Data ID = \"\">");
                Response.AppendFormat("<CUSTID>{0}</CUSTID>", account.CustID);
                Response.AppendFormat("<BESTTONEACCOUNT>{0}</BESTTONEACCOUNT>", account.BestPayAccount);
                Response.AppendFormat("<BALANCE>{0}</BALANCE>", balance);
                Response.AppendFormat("</Data>");
                Response.AppendFormat("</Result>");

            }
            else
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户查询失败!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            }
            #endregion
        }
        catch (Exception e)
        {

        }
        finally {
            log(strLog.ToString());
        }
        return Response.ToString();
    }


    public class GetAccountBalanceResult
    {
        public string returnCode;
        public string msg;
        public GetAccountBalanceData data;
    }

    public class GetAccountBalanceData
    {
        public string Id;
        public string CustID;
        public string BesttoneAccount;
        public long Balance;
    
    }



    [WebMethod(Description = "账户余额查询-xml")]
    public GetAccountBalanceResult GetAccountBalanceV2(String Request)
    {
        GetAccountBalanceResult getAccountBalanceResult = new GetAccountBalanceResult();
        String ReturnCode = "0";
        String Descriptioin = "成功";
        StringBuilder Response = new StringBuilder();
        Response.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        #region
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Request);
        XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
        String version = versionNode.Attributes["value"].Value;

        XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
        String SPID = SPIDNode.Attributes["value"].Value;

        XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CUSTID")[0];
        String CustID = CustIDNode.Attributes["value"].Value;

        XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
        String BA = BesttoneAccountNode.Attributes["value"].Value;
        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("请求参数xml:{0}\r\n",Request);
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                getAccountBalanceResult.returnCode = ReturnCode;
                getAccountBalanceResult.msg = Descriptioin;
                return getAccountBalanceResult;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                getAccountBalanceResult.returnCode = ReturnCode;
                getAccountBalanceResult.msg = Descriptioin;
                return getAccountBalanceResult;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();

            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetAccountBalance", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                getAccountBalanceResult.returnCode = ReturnCode;
                getAccountBalanceResult.msg = Descriptioin;
                return getAccountBalanceResult;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
            }

            if (CommonUtility.IsEmpty(CustID) && CommonUtility.IsEmpty(BA))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "CustID和BesttoneAccount不能同时为空!";
                getAccountBalanceResult.returnCode = ReturnCode;
                getAccountBalanceResult.msg = Descriptioin;
                return getAccountBalanceResult;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
            }
            BesttoneAccount account = null;
            BesttoneAccountDAO dao = new BesttoneAccountDAO();

            if (!CommonUtility.IsEmpty(CustID))
            {
                account = dao.QueryByCustID(CustID);
            }

            if (!CommonUtility.IsEmpty(BA))
            {
                account = dao.QueryByBestAccount(BA);
            }

            if (account == null)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户不存在!";
                getAccountBalanceResult.returnCode = ReturnCode;
                getAccountBalanceResult.msg = Descriptioin;
                return getAccountBalanceResult;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
            }

            //查询账户余额
            long balance = 0;
            Int32 QueryAccountBalanceResult = BesttoneAccountHelper.QueryAccountBalance(account.BestPayAccount, out balance, out ErrMsg);

            if (QueryAccountBalanceResult == 0)
            {
                GetAccountBalanceData data = new GetAccountBalanceData();
                //Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                //Response.AppendFormat("<Data ID = \"\">");
                getAccountBalanceResult.returnCode = "0";
                getAccountBalanceResult.msg = "成功";
                data.Id = "";
                data.CustID = account.CustID;
                data.CustID = account.BestPayAccount;
                data.Balance = balance;
                getAccountBalanceResult.data = data;
                //Response.AppendFormat("<CUSTID>{0}</CUSTID>", account.CustID);
                //Response.AppendFormat("<BESTTONEACCOUNT>{0}</BESTTONEACCOUNT>", account.BestPayAccount);
                //Response.AppendFormat("<BALANCE>{0}</BALANCE>", balance);
                //Response.AppendFormat("</Data>");
                //Response.AppendFormat("</Result>");

            }
            else
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户查询失败!";
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                getAccountBalanceResult.returnCode = ReturnCode;
                getAccountBalanceResult.msg = Descriptioin;
            }
            #endregion
        }
        catch (Exception e)
        {
            getAccountBalanceResult.returnCode = "-9002";
            getAccountBalanceResult.msg = e.ToString() ;
        }
        //return Response.ToString();
        return getAccountBalanceResult;
    }


    [WebMethod(Description = "卡余额查询")]
    public String GetCardBalance(String Request)
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

        XmlNode CardTypeNode = xmlDoc.SelectNodes("/root/srchcond/conds/CardType")[0];
        String CardType = CardTypeNode.Attributes["value"].Value;

        XmlNode CardNoNode = xmlDoc.SelectNodes("/root/srchcond/conds/CardNo")[0];
        String CardNo = CardNoNode.Attributes["value"].Value;

        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();

            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetCardBalance", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                return Response.ToString();
            }

            long balance = 0;
            int QueryCardBalanceResult = BesttoneAccountHelper.QueryCardBalance(CardNo, CardType, out balance, out ErrMsg);
            if (QueryCardBalanceResult == 0)
            {
                Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                Response.AppendFormat("<Data ID = \"\">");
                Response.AppendFormat("<BALANCE>{0}</BALANCE>", balance);
                Response.AppendFormat("</Data>");
                Response.AppendFormat("</Result>");
            }
            else
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户查询失败!";
                Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
            }
            #endregion
        }
        catch (Exception e)
        {

        }
        return Response.ToString();
    }

    public class GetCardBalanceResult
    {
        public string returnCode;
        public string msg;
        public GetCardBalanceData data;
    
    }

    public class GetCardBalanceData
    {
        public string Id;
        public long Balance;
    }

    [WebMethod(Description = "卡余额查询-xml")]
    public GetCardBalanceResult GetCardBalanceV2(String Request)
    {
        GetCardBalanceResult getCardBalanceResult = new GetCardBalanceResult();

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

        XmlNode CardTypeNode = xmlDoc.SelectNodes("/root/srchcond/conds/CardType")[0];
        String CardType = CardTypeNode.Attributes["value"].Value;

        XmlNode CardNoNode = xmlDoc.SelectNodes("/root/srchcond/conds/CardNo")[0];
        String CardNo = CardNoNode.Attributes["value"].Value;

        #endregion

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();
        try
        {
            #region 条件校验
            if (CommonUtility.IsEmpty(SPID))
            {
                ReturnCode = Convert.ToString(ErrorDefinition.CIP_IError_Result_SPIDInValid_Code);
                Descriptioin = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getCardBalanceResult.returnCode = ReturnCode;
                getCardBalanceResult.msg = Descriptioin;
                return getCardBalanceResult;
            }

            //IP是否允许访问
            Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out ErrMsg);
            strLog.AppendFormat("请求方ip:{0}\r\n", HttpContext.Current.Request.UserHostAddress);
            strLog.AppendFormat("CheckIPLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                //return Result;
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizIPLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizIPLimit_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getCardBalanceResult.returnCode = ReturnCode;
                getCardBalanceResult.msg = Descriptioin;
                return getCardBalanceResult;
            }

            //接口访问权限判断
            Result = CommonBizRules.CheckInterfaceLimit(SPID, "GetCardBalance", this.Context, out ErrMsg);
            strLog.AppendFormat("CheckInterfaceLimit Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
            if (Result != 0)
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Msg;
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                //return Response.ToString();
                getCardBalanceResult.returnCode = ReturnCode;
                getCardBalanceResult.msg = Descriptioin;
                return getCardBalanceResult;
            }

            long balance = 0;
            int QueryCardBalanceResult = BesttoneAccountHelper.QueryCardBalance(CardNo, CardType, out balance, out ErrMsg);
            if (QueryCardBalanceResult == 0)
            {
                getCardBalanceResult.returnCode ="0";
                getCardBalanceResult.msg = "成功";
                GetCardBalanceData data = new GetCardBalanceData();
                data.Id = "";
                data.Balance = balance;
                getCardBalanceResult.data = data;
                //Response.AppendFormat("<Result returnCode = \"0\" msg = \"成功\">");
                //Response.AppendFormat("<Data ID = \"\">");
                //Response.AppendFormat("<BALANCE>{0}</BALANCE>", balance);
                //Response.AppendFormat("</Data>");
                //Response.AppendFormat("</Result>");
            }
            else
            {
                ReturnCode = Convert.ToString(ErrorDefinition.BT_IError_Result_BizInterfaceLimit_Code);
                Descriptioin = "账户查询失败!";
                //Response.AppendFormat("<result returnCode = {0} msg = {1} />", ReturnCode, Descriptioin);
                getCardBalanceResult.returnCode = ReturnCode;
                getCardBalanceResult.msg = Descriptioin;
            }
            #endregion
        }
        catch (Exception e)
        {
            getCardBalanceResult.returnCode = "-9003";
            getCardBalanceResult.msg = e.ToString();
        }
        return getCardBalanceResult;
        //return Response.ToString();
    }



    public class RechargeResult
    {
        public String ReturnCode;
        public String Msg;
        public String Rechargeamount;
        public String Rechargeamountlimit;
        public String OnceRechargeLimit;
        public String CurrentAmountLimit;
        public String AccountBalance;
    }

    /// <summary>
    /// 充值接口
    /// </summary>
    /// 

    [WebMethod(Description = "充值接口-xml")]
    public RechargeResult Recharge(String Request)
    {
        RechargeResult rechargeResult = new RechargeResult();

        int Result = 0;
        String ErrMsg = "";

        StringBuilder strLog = new StringBuilder();

        StringBuilder Response = new StringBuilder();
        Response.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
    
        #region 解析xml
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Request);
        XmlNode versionNode = xmlDoc.SelectNodes("/root/callinfo/version")[0];
        String version = versionNode.Attributes["value"].Value;

        XmlNode SPIDNode = xmlDoc.SelectNodes("/root/callinfo/SPID")[0];
        String SPID = SPIDNode.Attributes["value"].Value;

        XmlNode CardTypeNode = xmlDoc.SelectNodes("/root/srchcond/conds/CardType")[0];
        String CardType = CardTypeNode.Attributes["value"].Value;

        XmlNode CustIDNode = xmlDoc.SelectNodes("/root/srchcond/conds/CustID")[0];
        String CustID = CustIDNode.Attributes["value"].Value;

        XmlNode BesttoneAccountNode = xmlDoc.SelectNodes("/root/srchcond/conds/BesttoneAccount")[0];
        String BesttoneAccount = CustIDNode.Attributes["value"].Value;

        XmlNode CardNoNode = xmlDoc.SelectNodes("/root/srchcond/conds/CardNo")[0];
        String CardNo = CardNoNode.Attributes["value"].Value;

        XmlNode CardPasswordNode = xmlDoc.SelectNodes("/root/srchcond/conds/CardPassword")[0];
        String CardPassword = CardPasswordNode.Attributes["value"].Value;

        #endregion

        #region 请求数据校验

        if (String.IsNullOrEmpty(CustID) && String.IsNullOrEmpty(BesttoneAccount))
        { 
            //返回错误
            rechargeResult.ReturnCode = "-10";
            rechargeResult.Msg = "CUSTID 和BesttoneAccount 不能同时为空! ";
            return rechargeResult;
        }

        #endregion

        #region 权限校验
        #endregion

        long accountBalance = 0;        //账户余额
        long cardBalance = 0;           //卡余额
        //String accountType = BesttoneAccountHelper.ConvertAccountType(cardType);            //转换卡类型
        /*********************************************查询账户信息*****************************************************/
        BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
        BesttoneAccount account_entity = null;
        //订单充值操作类
        RechargeOrderDAO _rechargeOrder_dao = new RechargeOrderDAO();
        //卡扣款记录操作类
        CardRechargeRecordDAO _cardRechargeRecord_dao = new CardRechargeRecordDAO();
        //充值操作类
        AccountRechargeRecordDAO _accountRechargeRecord_dao = new AccountRechargeRecordDAO();


        if (!String.IsNullOrEmpty(CustID))
        {
            account_entity = _besttoneAccount_dao.QueryByCustID(CustID);
        }

        if (!String.IsNullOrEmpty(BesttoneAccount))
        {
            account_entity = _besttoneAccount_dao.QueryByBestAccount(BesttoneAccount);
       }

        #region 卡余额查询

        //查询卡余额
        Result = BesttoneAccountHelper.QueryCardBalance(CardNo, CardType, out cardBalance, out ErrMsg);
        strLog.AppendFormat("[查询卡余额]:Result:{0},Balance:{1}\r\n", Result, cardBalance);

        //查询失败
        //if (Result != 0)
        //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + Result + "\",\"info\":\"查询余额失败\"}]";
        //if (Result != 0)
        //    return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='" + Result + "' Msg='" + 查询余额失败 + "'  />";

        if (Result != 0)
        {
            rechargeResult.ReturnCode = "-11";
            rechargeResult.Msg = "查询余额失败";
            return rechargeResult;
        }

        //卡余额为0
        //if (cardBalance == 0)
        //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"200020\",\"info\":\"卡内余额为0\"}]";

        if (cardBalance == 0)
        {
            rechargeResult.ReturnCode = "200020";
            rechargeResult.Msg = "卡内余额为0";
            return rechargeResult;
        }
        
        //if (Result != 0)
        //    return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='200020' Msg='" + 卡内余额为0 + "'  />";

        #endregion

        #region 账户充值金额上限校验

        long OnceRechargeLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedOne;              //单笔充值金额上限
        long RechargeAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountRechargeLimitedDay;            //账户单日充值额度上限
        long CurrentAmountLimit = BesttoneAccountConstDefinition.DefaultInstance.AccountBalanceLimited;                 //账户余额上限
        if (OnceRechargeLimit > 0)
        {
            //检测用户单笔充值金额是否超限(10000元)
            //if (cardBalance > OnceRechargeLimit)
            //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100003\",\"rechargeamount\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "\",\"rechargeamountlimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit) + "\",\"info\":\"卡内余额为0\"}]";
            //if (Result != 0)
            //    return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='100003' Rechargeamount='" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "' Rechargeamountlimit='" + BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit) + "' />";

            if (Result != 0)
            {
                rechargeResult.ReturnCode = "100003";
                rechargeResult.Msg = "单笔充值金额是否超限(10000元)";
                rechargeResult.Rechargeamount = BesttoneAccountHelper.ConvertAmountToYuan(cardBalance);
                rechargeResult.Rechargeamountlimit = BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit);
                return rechargeResult;
            }
        
        }
        if (RechargeAmountLimit > 0)
        {
            //检测用户当日充值是否超限(当日充值金额不能超过50000元)
            long hadRechargeAmount = _rechargeOrder_dao.QueryCurrentRechargeAmount(account_entity.BestPayAccount);
            //if ((hadRechargeAmount + cardBalance) > RechargeAmountLimit)
            //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100001\",\"rechargeamount\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(hadRechargeAmount) + "\",\"rechargeamountlimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(RechargeAmountLimit) + "\",\"info\":\"卡内余额为0\"}]";
            //if (Result != 0)
            //    return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='100001' Rechargeamount='" + BesttoneAccountHelper.ConvertAmountToYuan(hadRechargeAmount) + "' Rechargeamountlimit='" + BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit) + "' />";
            rechargeResult.ReturnCode = "100001";
            rechargeResult.Msg = "当日充值金额不能超过50000元";
            rechargeResult.Rechargeamount = BesttoneAccountHelper.ConvertAmountToYuan(cardBalance);
            rechargeResult.Rechargeamountlimit = BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit);
            return rechargeResult;
        
        }

        if (CurrentAmountLimit > 0)
        {
            //检测用户帐户余额(个人账户余额不能超过100000元)
            Result = BesttoneAccountHelper.QueryAccountBalance(account_entity.BestPayAccount, out accountBalance, out ErrMsg);
            //if (Result != 0)
            //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"" + Result + "\",\"info\":\"账户信息查询失败\"}]";

            //if (Result != 0)
            //    return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='" + Result + "' Msg='账户信息查询失败'  />";

            //if ((accountBalance + cardBalance) > CurrentAmountLimit)
            //    return "[{\"result\":\"false\",\"step\":\"query\",\"errorcode\":\"100002\",\"accountbalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\",\"CurrentAmountLimit\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(CurrentAmountLimit) + "\",\"info\":\"卡内余额为0\"}]";
            //accountBalance = 0;

            //if ((accountBalance + cardBalance) > CurrentAmountLimit)
            //    return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='100002' Accountbalance='" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "' CurrentAmountLimit='" + BesttoneAccountHelper.ConvertAmountToYuan(CurrentAmountLimit) + "'  />";

            if ((accountBalance + cardBalance) > CurrentAmountLimit)
            {
                rechargeResult.ReturnCode = "100001";
                rechargeResult.Msg = "当日充值金额不能超过50000元";
                rechargeResult.Rechargeamount = BesttoneAccountHelper.ConvertAmountToYuan(cardBalance);
                rechargeResult.Rechargeamountlimit = BesttoneAccountHelper.ConvertAmountToYuan(OnceRechargeLimit);
                return rechargeResult;
            }

        }

        #endregion

        #region 卡扣款

        String transactionID = BesttoneAccountHelper.CreateTransactionID();
        String orderSeq = BesttoneAccountHelper.CreateOrderSeq();
        DateTime reqTime = DateTime.Now;

        RechargeOrder _recharge_order;              //充值订单
        CardRechargeRecord cardrecharge_entity;     //卡扣款流水记录

        //初始化充值订单
        _recharge_order = new RechargeOrder(orderSeq, transactionID, reqTime.ToString("yyyyMMdd"), "RMB", cardBalance, cardBalance, 0, "消费卡向账户充值扣款",
            account_entity.CustID, account_entity.BestPayAccount, CardType, SPID, reqTime, new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), 1, 0, "", "", "", "0");  //2013-04-13 add 最后一个字段 0 代表是否需要开票
 
        //初始化卡扣款流水
        cardrecharge_entity = new CardRechargeRecord(transactionID, orderSeq, reqTime.ToString("yyyyMMdd"), "RMB", cardBalance, "消费卡向账户充值扣款", CardNo, CardPassword, CardType,
            account_entity.BestPayAccount, 0, reqTime, new DateTime(1900, 1, 1), "", "", "", "", "");

        /***********************************************************开始扣款*******************************************************/
        String uptranSeq = String.Empty;                        //交易流水号，支付平台返回的，后期对账用
        Result = BesttoneAccountHelper.CardDeductionBalance(transactionID, orderSeq, CardNo, CardPassword, CardType, cardBalance, reqTime, "", out uptranSeq, out ErrMsg);
        strLog.AppendFormat("[卡扣款]:Result:{0},ErrMsg:{1}\r\n", Result, ErrMsg);
        _recharge_order.UptranSeq = uptranSeq;
        cardrecharge_entity.UptranSeq = uptranSeq;
        if (Result != 0)
        {
            //修改订单信息
            _recharge_order.Status = 4;
            _recharge_order.PayTime = DateTime.Now;
            _recharge_order.ReturnCode = Result.ToString();
            _recharge_order.ReturnDesc = ErrMsg;
            _rechargeOrder_dao.Insert(_recharge_order);

            //修改卡扣款记录信息
            cardrecharge_entity.Status = 2;
            cardrecharge_entity.PayTime = DateTime.Now;
            cardrecharge_entity.ReturnCode = Result.ToString();
            cardrecharge_entity.ReturnDesc = ErrMsg;
            _cardRechargeRecord_dao.Insert(cardrecharge_entity);
            //return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='" + Result + "' Msg='卡扣款失败'   />";

            //return "[{\"result\":\"false\",\"step\":\"deduction\",\"errorcode\":\"" + Result + "\",\"info\":\"卡扣款失败\"}]";
            rechargeResult.ReturnCode = Convert.ToString(Result);
            rechargeResult.Msg = "卡扣款失败!";

            return rechargeResult;
        }
        else
        {
            //修改订单信息
            _recharge_order.Status = 2;
            _recharge_order.PayTime = DateTime.Now;
            _recharge_order.ReturnCode = "0000";
            _recharge_order.ReturnDesc = "扣款成功";
            _rechargeOrder_dao.Insert(_recharge_order);

            //修改卡充值记录信息
            cardrecharge_entity.Status = 1;
            cardrecharge_entity.PayTime = DateTime.Now;
            cardrecharge_entity.ReturnCode = "0000";
            cardrecharge_entity.ReturnDesc = "扣款成功";
            _cardRechargeRecord_dao.Insert(cardrecharge_entity);
        }

        #endregion

        #region 充值

        transactionID = BesttoneAccountHelper.CreateTransactionID();        //充值流水记录
        DateTime rechargeTime = DateTime.Now;                               //充值请求时间
        String returnMsg = String.Empty;
        bool resultBoolean = false;

        //初始化充值流水类
        AccountRechargeRecord rechargeRecord_entity = new AccountRechargeRecord(transactionID, rechargeTime.ToString("yyyyMMdd"), orderSeq,
            cardBalance, CardType, "消费卡充值", rechargeTime, new DateTime(1900, 1, 1), 0, "", "");

        try
        {
            #region 开始充值

            //调用接口给账户充值
            Result = BesttoneAccountHelper.AccountRecharge(transactionID, account_entity.BestPayAccount, cardBalance, rechargeTime, out accountBalance, out ErrMsg);
            //调用接口给账户充值—测试
            //Result = BesttoneAccountHelper.AccountRecharge(transactionID, account_entity.BestPayAccount, 1, rechargeTime, out accountBalance, out ErrMsg);
            strLog.AppendFormat("[账户充值]:TransactionID:{0},Result:{1},ErrMsg:{2}\r\n", transactionID, Result, ErrMsg);
            if (Result == 0)
            {
                //修改订单信息
                _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                _recharge_order.Status = 3;
                _recharge_order.RechargeCount = 1;
                _recharge_order.CompleteTime = DateTime.Now;
                _recharge_order.ReturnCode = "0000";
                _recharge_order.ReturnDesc = "充值成功";
                resultBoolean = _rechargeOrder_dao.Update(_recharge_order);

                //修改充值流水记录信息
                rechargeRecord_entity.Status = 1;
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = "0000";
                rechargeRecord_entity.ReturnDesc = "充值成功";

                strLog.AppendFormat("[更新订单状态]ResultBoolean:{0}\r\n", resultBoolean);
                returnMsg = "[{\"result\":\"true\",\"info\":\"账户充值成功\",\"deductionBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "\",\"accountBalance\":\"" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "\"}]";
                //return "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='" + Result + "' Msg='卡扣款失败'   />";
                //returnMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='" + 0 + "' Msg='账户充值成功'  deductionBalance='" + BesttoneAccountHelper.ConvertAmountToYuan(cardBalance) + "' accountBalance='" + BesttoneAccountHelper.ConvertAmountToYuan(accountBalance) + "' />";
                rechargeResult.Msg = "账户充值成功";
                rechargeResult.ReturnCode = "0";
                rechargeResult.Rechargeamount = BesttoneAccountHelper.ConvertAmountToYuan(accountBalance);
                return rechargeResult;
            }
            else
            {
                //修改订单信息
                _recharge_order = _rechargeOrder_dao.QueryByOrderSeq(orderSeq);
                _recharge_order.RechargeCount = 1;
                _recharge_order.ReturnCode = Result.ToString();
                _recharge_order.ReturnDesc = ErrMsg;
                resultBoolean = _rechargeOrder_dao.Update(_recharge_order);

                //修改充值流水记录信息
                rechargeRecord_entity.Status = 2;
                rechargeRecord_entity.CompleteTime = DateTime.Now;
                rechargeRecord_entity.ReturnCode = Result.ToString();
                rechargeRecord_entity.ReturnDesc = ErrMsg;

                returnMsg = "[{\"result\":\"false\",\"step\":\"recharge\",\"errorcode\":\"" + Result + "\",\"info\":\"账户充值失败\"}]";
                //returnMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\"><Result ReturnCode='" + Result + "' Msg='账户充值失败'   />";
                rechargeResult.ReturnCode = "" + Result+"";
                rechargeResult.Msg = "账户充值失败";
                return rechargeResult;
            }

            #endregion
        }
        catch (Exception ex)
        {
            rechargeRecord_entity.ReturnDesc += ex.Message;
            throw ex;
        }
        finally
        {
            _accountRechargeRecord_dao.Insert(rechargeRecord_entity);
        }

        #endregion
        return rechargeResult;
    }


    public class AccountBalanceQueryV2Result
    {
        public Int32 Result;
        public String ErrMsg;
        public String CustID;
        public String Account;
        public String Balance;
    }
    [WebMethod(Description = "账户余额查询V2")]
    public AccountBalanceQueryV2Result AccountBalanceQueryV2(String SPID, String BesttoneAccount)
    {
        AccountBalanceQueryV2Result Result = new AccountBalanceQueryV2Result();
        Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        Result.Account = BesttoneAccount;
        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AccountBalanceQueryV2", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(BesttoneAccount))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Msg;
                return Result;
            }

            #endregion

            BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
            BesttoneAccount entity = _besttoneAccount_dao.QueryByBestAccount(BesttoneAccount);
            if (entity == null)
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Msg;
            }
            else
            {
                long balance = 0;
                //查询账户余额
                Result.Result = BesttoneAccountHelper.QueryAccountBalance(entity.BestPayAccount, out balance, out Result.ErrMsg);
                Result.Balance = balance.ToString();
                Result.CustID = entity.CustID;
            }
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + ex.Message;
        }
        finally
        {
            //记录日志
        }
        return Result;
    }




    public class AccountBalanceQueryResult
    {
        public Int32 Result;
        public String ErrMsg;
        public String CustID;
        public String Account;
        public String Balance;
    }
    /// <summary>
    /// 账户余额查询
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "账户余额查询")]
    public AccountBalanceQueryResult AccountBalanceQuery(String SPID, String CustID)
    {
        AccountBalanceQueryResult Result = new AccountBalanceQueryResult();
        Result.Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "AccountInfoQuery", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            if (CommonUtility.IsEmpty(CustID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_User_UserIDInValid_Msg;
                return Result;
            }

            #endregion

            BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
            BesttoneAccount entity = _besttoneAccount_dao.QueryByCustID(CustID);
            if (entity == null)
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAccount_UnRegister_Msg;
            }
            else
            {
                long balance = 0;
                //查询账户余额
                Result.Result = BesttoneAccountHelper.QueryAccountBalance(entity.BestPayAccount, out balance, out Result.ErrMsg);
                Result.Balance = balance.ToString();
            }
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + ex.Message;
        }
        finally
        {
            //记录日志
        }
        return Result;
    }


    public class CardBalanceQueryResult
    {
        public Int32 Result;
        public String ErrMsg;
        public String CardNo;
        public String CardType;
        public long Balance;
    }
    /// <summary>
    /// 卡余额查询
    /// </summary>
    [WebMethod(Description="卡余额查询")]
    public CardBalanceQueryResult CardBalanceQuery(String SPID, String CardNo, String CardType)
    {
        CardBalanceQueryResult Result = new CardBalanceQueryResult();
        Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
        Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg;
        Result.CardNo = CardNo;
        Result.CardType = CardType;
        try
        {
            #region 数据合法性判断
            if (CommonUtility.IsEmpty(SPID))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_SPIDInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_SPIDInValid_Msg;
                return Result;
            }

            //IP是否允许访问
            Result.Result = CommonBizRules.CheckIPLimit(SPID, HttpContext.Current.Request.UserHostAddress, this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }

            //接口访问权限判断
            Result.Result = CommonBizRules.CheckInterfaceLimit(SPID, "CardBalanceQuery", this.Context, out Result.ErrMsg);
            if (Result.Result != 0)
            {
                return Result;
            }
            //卡不能为空
            if (CommonUtility.IsEmpty(CardNo))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAccount_CardNoInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAccount_CardNoInValid_Msg;
                return Result;
            }
            //卡类型也不能为空
            if (CommonUtility.IsEmpty(CardType))
            {
                Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAccount_CardTypeInValid_Code;
                Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAccount_CardTypeInValid_Msg;
                return Result;
            }

            #endregion

            //查询卡余额
            Result.Result = BesttoneAccountHelper.QueryCardBalance(CardNo, CardType, out Result.Balance, out Result.ErrMsg);
        }
        catch (Exception ex)
        {
            Result.Result = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Code;
            Result.ErrMsg = ErrorDefinition.CIP_IError_Result_BesttoneAcountException_Msg + "," + ex.Message;
        }
        finally
        { 
            
        }

        return Result;
    }

    #endregion







    protected void log(string strLog, string logPath)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(strLog);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneAccountService"+"_"+logPath, msg);
    }

    protected void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("BesttoneAccountService", msg);
    }
}


using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using log4net;
using BTUCenter.Proxy;

public partial class UserAccount_OpenBesttoneAcccount : System.Web.UI.Page
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(UserAccount_OpenBesttoneAcccount));
    private static IDispatchControl serviceProxy = new IDispatchControl();




    /// <summary>
    /// 生成20位数字随机流水号:35+yyMMddHHmmss+随机数(6位)
    /// </summary>
    public static String CreateTransactionID()
    {
        String date = DateTime.Now.ToString("yyyyMMddHHmmss");

        //6位随机数
        Random r = new Random(Guid.NewGuid().GetHashCode());
        String TransactionID = "00" + date + r.Next(1000, 9999).ToString();

        return TransactionID;
    }

    /// <summary>
    /// 号码百事通账户查询
    /// </summary>
    public Int32 QueryBesttoneAccount(String besttoneAccount, out AccountItem accountInfo, out String responseCode, out String ErrMsg)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.AppendFormat("号码百事通账户查询入口参数:besttoneAccount:{0}\r\n", besttoneAccount);
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        accountInfo = null;
        responseCode = String.Empty;
        StringBuilder requestXml = new StringBuilder();
        String responseXml = String.Empty;
        //流水号
        String TransactionID = CreateTransactionID();
        try
        {
            #region 拼接请求xml字符串
            requestXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            requestXml.Append("<PayPlatRequestParameter>");
            requestXml.Append("<CTRL-INFO WEBSVRNAME=\"账户查询\" WEBSVRCODE=\"100100\" APPFROM=\"100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1\" KEEP=\"" + TransactionID + "\" />");
            requestXml.Append("<PARAMETERS>");

            //添加参数
            requestXml.AppendFormat("<PRODUCTNO>{0}</PRODUCTNO>", "yy" + besttoneAccount);
            requestXml.AppendFormat("<ACCOUNTTYPE>{0}</ACCOUNTTYPE>", "1");
            requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);
            //requestXml.AppendFormat("<ACCEPTORGCODE>{0}</ACCEPTORGCODE>", "001310000000000");
            requestXml.AppendFormat("<ACCEPTSEQNO>{0}</ACCEPTSEQNO>", TransactionID);
            requestXml.AppendFormat("<INPUTTIME>{0}</INPUTTIME>", DateTime.Now.ToString("yyyyMMddHHmmss"));
            requestXml.Append("</PARAMETERS>");
            requestXml.Append("</PayPlatRequestParameter>");
            #endregion
            //请求接口
            responseXml = serviceProxy.dispatchCommand("100100|310000-TEST1-127.0.0.1|1.0|127.0.0.1", requestXml.ToString());
            strLog.AppendFormat("responseXml={0}\r\n", responseXml);

            #region 解析接口返回参数

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXml);

            responseCode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECODE")[0].InnerText;
            ErrMsg = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESPONSECONTENT")[0].InnerText;
            if (responseCode == "000000" )
            {
                Result = 0;
                accountInfo = new AccountItem();
                XmlNode dataNode = xmlDoc.SelectNodes("/PayPlatResponseParameter/RESULTDATESET/DATAS")[0];
                accountInfo.AccountNo = dataNode.Attributes["ACCOUNTNO"].Value;
                accountInfo.AccountName = dataNode.Attributes["ACCOUNTNAME"].Value;
                accountInfo.AccountType = dataNode.Attributes["ACCOUNTTYPE"].Value;
                accountInfo.AccountStatus = dataNode.Attributes["ACCOUNTSTATUS"].Value;
                accountInfo.AccountBalance = Convert.ToInt64(dataNode.Attributes["ACCOUNTBALANCE"].Value);
                accountInfo.PredayBalance = Convert.ToInt64(dataNode.Attributes["PREDAYBALANCE"].Value);
                accountInfo.PreMonthBalance = Convert.ToInt64(dataNode.Attributes["PREMONTHBALANCE"].Value);
                accountInfo.AvailableBalance = Convert.ToInt64(dataNode.Attributes["AVAILABLEBALANCE"].Value);
                accountInfo.UnAvailableBalance = Convert.ToInt64(dataNode.Attributes["UNAVAILABLEBALANCE"].Value);
                accountInfo.AvailableLecash = Convert.ToInt64(dataNode.Attributes["AVAILABLECASH"].Value);
                accountInfo.CardNum = dataNode.Attributes["CARDNUM"].Value;
                accountInfo.CardType = dataNode.Attributes["CARDTYPE"].Value;
            }

            #endregion

        }
        catch (Exception ex)
        {
            strLog.AppendFormat("异常:{0}\r\n", ex.ToString());

        }
        finally
        {
            logger.Info("客户端调用：responseXml=" + responseXml);
        }
        return Result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="SPID"></param>
    /// <param name="CustID"></param>
    /// <param name="Phone"></param>
    /// <param name="CheckPhoneCode"></param>
    /// <param name="wt"></param>
    /// <returns></returns>
    public String OpenBesttoneAccount(String SPID, String CustID, String Phone, String IDCard, String RealName, String ContactTel, String Email, String Sex,String AuthenCode, String wt)
    {
        //返回参数
        String ErrMsg = String.Empty;
        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
     
        StringBuilder ResponseMsg = new StringBuilder();


        if (CommonUtility.IsEmpty(SPID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "995");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "SPID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "995");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "SPID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(CustID))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "996");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "CustID不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "996");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "CustID不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(Phone))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "997");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "Phone不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "997");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "Phone不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }


        if (CommonUtility.IsEmpty(IDCard))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "998");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "IDCard不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "998");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "IDCard不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        if (CommonUtility.IsEmpty(RealName))
        {
            // 返回错误信息
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "999");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "RealName不能为空！");
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "999");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "RealName不能为空！");
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();
        }

        //验证码校验
        Result = PhoneBO.SelSendSMSMassage(CustID, Phone, AuthenCode, out ErrMsg);
        if (Result != 0)
        {
            // 验证码未校验通过  return 
            ResponseMsg.Length = 0;
            if ("json".Equals(wt))
            {
                ResponseMsg.Append("{");
                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1000");
                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ErrMsg);
                ResponseMsg.Append("}");
            }
            else
            {
                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                ResponseMsg.Append("<PayPlatRequestParameter>");
                ResponseMsg.Append("<PARAMETERS>");
                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1000");
                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ErrMsg);
                ResponseMsg.Append("</PARAMETERS>");
                ResponseMsg.Append("</PayPlatRequestParameter>");
            }
            return ResponseMsg.ToString();

        }
        else
        {
            String t_custid = PhoneBO.IsAuthenPhone(Phone, SPID, out ErrMsg);
            // t_custid 可以为空，但不能是别人的custid,可以为空是说明此客户无认证电话
            if (!String.IsNullOrEmpty(t_custid))
            {
                if (CustID != t_custid)
                { 
                    //是别人的手机号，不能用来开户  return 
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1000");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", Phone + "(" + CustID + ")是别人的手机号(" + t_custid + ")，不能用来开户！");
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1000");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "是别人的手机号，不能用来开户！");
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                }
            }


            if (!CommonUtility.CheckIDCard(IDCard))
            {
                //身份证不合法! return  
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1001");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "身份证不合法！");
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1001");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "身份证不合法！");
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();

            }

            try
            {
                String TransactionID = CreateTransactionID();
                BesttoneAccountDAO _besttoneAccount_dao = new BesttoneAccountDAO();
                //strLog.AppendFormat("select * from besttoneaccount where custid={0}\r\n", CustID);
                BesttoneAccount besttoneAccountEntity = _besttoneAccount_dao.QueryByCustID(CustID);


                AccountItem ai = new AccountItem();
                String QueryBAResponseCode = "";

                if (besttoneAccountEntity == null)   // 未绑定
                {
                    //去翼支付查
                    int QueryBesttoneAccountResult = QueryBesttoneAccount(Phone, out ai, out QueryBAResponseCode, out ErrMsg);

                    //if (QueryBesttoneAccountResult == 0)
                    //{
                        if ("200010".Equals(QueryBAResponseCode))   // 未开户
                        {
                            UserRegistry.BeforeCreateBesttoneAccount(SPID, TransactionID, CustID, Phone, out  ErrMsg);  //日志
                            RegisterBesttoneAccount(Phone, RealName, ContactTel, Email, Sex, "1", IDCard, TransactionID, out ErrMsg);
                            UserRegistry.CreateBesttoneAccount(SPID, CustID, Phone, out ErrMsg);   //建立绑定关系
                            UserRegistry.AfterCreateBesttoneAccount(SPID, TransactionID, CustID, Phone, out  ErrMsg); //日志
                            UserRegistry.WriteBackBestToneAccountToCustInfo(SPID, CustID, RealName, IDCard, out ErrMsg);
                            ResponseMsg.Length = 0;
                            if ("json".Equals(wt))
                            {
                                ResponseMsg.Append("{");
                                ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                                ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "开户成功，绑定成功！");
                                ResponseMsg.Append("}");
                            }
                            else
                            {
                                ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                ResponseMsg.Append("<PayPlatRequestParameter>");
                                ResponseMsg.Append("<PARAMETERS>");
                                ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                                ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "开户成功，绑定成功！");
                                ResponseMsg.Append("</PARAMETERS>");
                                ResponseMsg.Append("</PayPlatRequestParameter>");
                            }
                            return ResponseMsg.ToString();
                        }
                        else
                        {   //可能在开过户
                            if ("000000".Equals(QueryBAResponseCode))
                            {
                                //绑定操作
                                UserRegistry.CreateBesttoneAccount(SPID, CustID, Phone, out ErrMsg);
                                UserRegistry.OnlyBindingBesttoneAccount(SPID, TransactionID, CustID, Phone, out  ErrMsg);

                                ResponseMsg.Length = 0;
                                if ("json".Equals(wt))
                                {
                                    ResponseMsg.Append("{");
                                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "0");
                                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "仅仅绑定成功！");
                                    ResponseMsg.Append("}");
                                }
                                else
                                {
                                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                    ResponseMsg.Append("<PayPlatRequestParameter>");
                                    ResponseMsg.Append("<PARAMETERS>");
                                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "0");
                                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "仅仅绑定成功！");
                                    ResponseMsg.Append("</PARAMETERS>");
                                    ResponseMsg.Append("</PayPlatRequestParameter>");
                                }
                                return ResponseMsg.ToString();
                            }
                            else
                            {
                                //账户状态可能存在异常  return 
                           
                                ResponseMsg.Length = 0;
                                if ("json".Equals(wt))
                                {
                                    ResponseMsg.Append("{");
                                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1002");
                                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "账户状态可能存在异常！");
                                    ResponseMsg.Append("}");
                                }
                                else
                                {
                                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                    ResponseMsg.Append("<PayPlatRequestParameter>");
                                    ResponseMsg.Append("<PARAMETERS>");
                                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1002");
                                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "账户状态可能存在异常！");
                                    ResponseMsg.Append("</PARAMETERS>");
                                    ResponseMsg.Append("</PayPlatRequestParameter>");
                                }
                                return ResponseMsg.ToString();
                            }
                        }
                    //}
                    //else
                    //{
                        ////账户查询过程中发生异常  return 
                        //ResponseMsg.Length = 0;
                        //if ("json".Equals(wt))
                        //{
                        //    ResponseMsg.Append("{");
                        //    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1003");
                        //    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "账户查询过程中发生异常！");
                        //    ResponseMsg.Append("}");
                        //}
                        //else
                        //{
                        //    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        //    ResponseMsg.Append("<PayPlatRequestParameter>");
                        //    ResponseMsg.Append("<PARAMETERS>");
                        //    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1003");
                        //    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "账户查询过程中发生异常！");
                        //    ResponseMsg.Append("</PARAMETERS>");
                        //    ResponseMsg.Append("</PayPlatRequestParameter>");
                        //}
                        //return ResponseMsg.ToString();
                    //}

                }
                else
                {
                    //该手机号码已经开过户，账户所绑定的custid不管是不是自己的，都不允许再开户
                    ResponseMsg.Length = 0;
                    if ("json".Equals(wt))
                    {
                        ResponseMsg.Append("{");
                        ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1004");
                        ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", "该手机号码已经开过户！");
                        ResponseMsg.Append("}");
                    }
                    else
                    {
                        ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                        ResponseMsg.Append("<PayPlatRequestParameter>");
                        ResponseMsg.Append("<PARAMETERS>");
                        ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1004");
                        ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", "该手机号码已经开过户！");
                        ResponseMsg.Append("</PARAMETERS>");
                        ResponseMsg.Append("</PayPlatRequestParameter>");
                    }
                    return ResponseMsg.ToString();
                }

            }
            catch (Exception ecp)
            {
                ResponseMsg.Length = 0;
                if ("json".Equals(wt))
                {
                    ResponseMsg.Append("{");
                    ResponseMsg.AppendFormat("\"errcode\":\"{0}\",", "1005");
                    ResponseMsg.AppendFormat("\"errmsg\":\"{0}\"", ecp.ToString());
                    ResponseMsg.Append("}");
                }
                else
                {
                    ResponseMsg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    ResponseMsg.Append("<PayPlatRequestParameter>");
                    ResponseMsg.Append("<PARAMETERS>");
                    ResponseMsg.AppendFormat("<ErrCode>{0}</ErrCode>", "1005");
                    ResponseMsg.AppendFormat("<ErrMsg>{0}</ErrMsg>", ecp.ToString());
                    ResponseMsg.Append("</PARAMETERS>");
                    ResponseMsg.Append("</PayPlatRequestParameter>");
                }
                return ResponseMsg.ToString();
            }

        }

        // 开户的前置条件
        //1.必须先登录（说明是合法号百客户,有CUSTID）
        //2.手机验证码校验  （如果是接口，仅校验手机验证码，如果是页面，则需校验页面验证码？）
        //3.检查手机号码是否是别人的登录账号 (登录后获得的custid和custphone 的custid比对)
        //4.验证身份证号是否合法
        //5.检查手机号码是否是别人的支付账户 （是否已经存在绑定关系，如果是，是否是绑定在自己的custid下）
        //6.去翼支付检查该手机号码是否开过户（直接调翼支付账户查询）

        //开户

        //IF 校验码未通过
        //    重定向到错误页面（如果是接口，则返回错误提示）
        //ELSE
        //      IF 根据PHONE 获得的CUSTID不是 登录CUSTID 
        //           非法开户，重定向到错误页面 （如果是接口，则返回错误提示）  --
        //      ELSE
        //            IF PHONE 存在于账户绑定关系
        //    IF 账户对应的CUSTID 与登录后获得的CUSTID 不匹配   -- 说明该手机号码已经被别的客户开成账户了
        //           该手机号码已经被别的客户开成账户了,重定向到错误页面，如果是接口，则返回错误提示信息
        //                ELSE
        //                      该手机已经开过户，不需要再开户
        //    END
        //           ELSE
        //                     IF 翼支付没能能查到该手机的账户信息 --说明的确未开户
        //                              验证身份证号是否合法
        //                               开户 （调用翼支付开户接口）
        //                                插入绑定关系表         	
        //                     END
        //           END
        //       END
        //END
        return ResponseMsg.ToString();
    }

    protected void log(String str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("OpenBesttoneAccountHttp", msg);
    }
    /// <summary>
    /// 开户  phoneNumber -> PRODUCTNO
    /// </summary>
    public static Int32 RegisterBesttoneAccount(String phoneNumber, String realName, String contactTel, String contactMail, String sex, String certtype, String certnum, String TransactionID, out String ErrMsg)
    {

        Int32 Result = ErrorDefinition.CIP_IError_Result_UnknowError_Code;
        ErrMsg = ErrorDefinition.CIP_IError_Result_UnknowError_Msg;
        string command = "100201|310000-TEST1-127.0.0.1|1.0|127.0.0.1";
        string ResultXML = "";
        string CARDNO = "8600021008207231";
        string CARDTYPE = "1";
        if (String.IsNullOrEmpty(realName))
        {
            realName = phoneNumber;
        }
        string NAME = realName;    //M  真实姓名，如张三
        string LOGINID = "";         //O
        string PRODUCTNO = "yy" + phoneNumber;   //M  号码百事通账户（手机号）
        string ISREALNAME = "3";        //M 是否已实名认证  3未经验证
        string REGISTERTYPE = "Y";       //M 注册类型 1手机号
        string ISAPPLYCERT = "1";       // M 是否申请个人证书 1是
        string CUSTOMLEVEL = "1";        //M 客户级别  1普通
        string CUSTOMTYPE = "1";          //M 客户类别  1个人 


        string REGCELLPHONENUM = "";      //M 联系手机 如18900000000 PRODUCTNO
        if (String.IsNullOrEmpty(contactTel))
        {
            REGCELLPHONENUM = phoneNumber;
        }
        else
        {
            REGCELLPHONENUM = contactTel;
        }

        string REGEMAIL = contactMail;              //M 联系邮箱 
 
        REGEMAIL = "";

        string LOGINPASSWORD = "";    //O 登录密码    见11.5，暂不填

        string SEX = sex;     //O
        string CERTTYPE = certtype;    //证件类型 M X其他
        string CERTNUM = certnum;      //证件号码 M 9999
        string FAMILYTEL = "";       //家庭联系电话  O
        string OFFICETEL = "";       //其他电话 办公室电话 0

        string CONTRACTADD = "";        //联系地址 O
        string COMPANYADD = "";         //单位地址 O
        string COMPANYZIP = "";         //单位邮编 O
        string COMPANYCODE = "";         //单位代码 O
        //string ACCEPTORGCODE = ACCEPTORGCODE;     //ACCEPTORGCODE
        string ACCEPTUID = "";       //受理人 O

        string ACCEPTCHANNEL = "02";    //受理渠道 M  02 WEB
        string ACCEPTSEQNO = TransactionID;      //受理流水 M 20位长度 调用方生成
        string FEEFLAG = "1";   //服务收费标志 M 1不收费
        string FEEAMT = "0";      //服务收费金额 M 以分为单位 0

        string INPUTUID = "";    //操作人员代码 O
        string INPUTTIME = "";   //操作时间 M yyyyMMddHHmmss格式
        string CHECKUID = "";    //授权员工标识 O
        string CHECKTIME = "";   //授权时间 O yyyyMMddHHmmss格式

        try
        {
            Result = GetRequestXML(CARDNO, CARDTYPE, NAME, LOGINID, PRODUCTNO, ISREALNAME, REGISTERTYPE,
            ISAPPLYCERT, CUSTOMLEVEL, CUSTOMTYPE, REGCELLPHONENUM, REGEMAIL, LOGINPASSWORD, SEX, CERTTYPE, CERTNUM, FAMILYTEL,
            OFFICETEL, CONTRACTADD, COMPANYADD, COMPANYZIP,
            COMPANYCODE, ACCEPTUID, ACCEPTCHANNEL,
            ACCEPTSEQNO, FEEFLAG, FEEAMT, INPUTUID, INPUTTIME, CHECKUID, CHECKTIME, TransactionID,
            out ResultXML, out  ErrMsg);
      
            string ret = serviceProxy.dispatchCommand(command, ResultXML);
            System.Xml.XmlDocument xd = new XmlDocument();
            xd.LoadXml(ret);
            XmlNode xmlNode1 = xd.SelectSingleNode("/PayPlatResponseParameter/RESPONSECODE");
            XmlNode xmlNode2 = xd.SelectSingleNode("/PayPlatResponseParameter/RESPONSECONTENT");
            if ("000000".Equals(xmlNode1.InnerText))
            {
                Result = 0;
                ErrMsg = xmlNode2.InnerText;
            }
            else
            {
                Result = -99999;
                ErrMsg = xmlNode2.InnerText;
            }

        }
        catch (Exception ex)
        {
           
        }
        return Result;
    }




    /// <summary>
    /// 分装开户请求参数
    /// <PayPlatRequestParameter>
    /// <CTRL-INFO 
    /// WEBSVRNAME="服务名称" 
    /// WEBSVRCODE="服务编码" 
    /// APPFROM="请求本服务的应用标识码"
    /// 	KEEP="本次服务的标识流水" />
    /// <PARAMETERS>
    /// ....  红色的是要从前面带过来 必填 除了性别
    /// </PARAMETERS>
    /// </PayPlatRequestParameter>
    /// </summary>
    public static int GetRequestXML(string CARDNO, string CARDTYPE, string NAME, string LOGINID, string PRODUCTNO, string ISREALNAME, string REGISTERTYPE,
        string ISAPPLYCERT, string CUSTOMLEVEL, string CUSTOMTYPE, string REGCELLPHONENUM, string REGEMAIL, string LOGINPASSWORD, string SEX, string CERTTYPE, string CERTNUM, string FAMILYTEL,
        string OFFICETEL, string CONTRACTADD, string COMPANYADD, string COMPANYZIP,
        string COMPANYCODE, string ACCEPTUID, string ACCEPTCHANNEL,
        string ACCEPTSEQNO, string FEEFLAG, string FEEAMT, string INPUTUID, string INPUTTIME, string CHECKUID, string CHECKTIME, string KEEP,
        out string ResultXML, out string ErrMsg)
    {

        int Result = 0;
        ResultXML = "";
        ErrMsg = "";


        XmlDocument xmldoc;
        XmlNode xmlnode;
        XmlElement xmlelem_ROOT;
        XmlElement xmlelem_CTRLINFO;
        XmlElement xmlelem_PARAMETERS;


        XmlElement xmlelem_CARDNO;
        XmlElement xmlelem_CARDTYPE;
        XmlElement xmlelem_NAME;
        XmlElement xmlelem_LOGINID;
        XmlElement xmlelem_PRODUCTNO;
        XmlElement xmlelem_ISREALNAME;
        XmlElement xmlelem_REGISTERTYPE; //
        XmlElement xmlelem_ISAPPLYCERT;  //
        XmlElement xmlelem_CUSTOMLEVEL;  //
        XmlElement xmlelem_CUSTOMTYPE;   //

        XmlElement xmlelem_REGCELLPHONENUM; //
        XmlElement xmlelem_REGEMAIL;  //
        XmlElement xmlelem_LOGINPASSWORD; //

        XmlElement xmlelem_SEX;  // 
        XmlElement xmlelem_CERTTYPE;  // 
        XmlElement xmlelem_CERTNUM;   //
        XmlElement xmlelem_FAMILYTEL;  //
        XmlElement xmlelem_OFFICETEL;   //
        XmlElement xmlelem_APANAGE;    // 
        XmlElement xmlelem_AREACODE;   //
        XmlElement xmlelem_CITYCODE;   //
        XmlElement xmlelem_CONTRACTADD; //
        XmlElement xmlelem_COMPANYADD;  // 
        XmlElement xmlelem_COMPANYZIP;   // 
        XmlElement xmlelem_COMPANYCODE;   // 
        XmlElement xmlelem_ACCEPTORGCODE;  // 
        XmlElement xmlelem_ACCEPTUID;  //
        XmlElement xmlelem_ACCEPTAREACODE;  // 
        XmlElement xmlelem_ACCEPTCITYCODE;  //
        XmlElement xmlelem_ACCEPTCHANNEL;  //
        XmlElement xmlelem_ACCEPTSEQNO;  // 
        XmlElement xmlelem_FEEFLAG;  // 
        XmlElement xmlelem_FEEAMT;  // 
        XmlElement xmlelem_INPUTUID;  // 
        XmlElement xmlelem_INPUTTIME;  // 
        XmlElement xmlelem_CHECKUID;  // 
        XmlElement xmlelem_CHECKTIME; // 


        XmlText xmltext;


        xmldoc = new XmlDocument();
        //加入XML的声明段落

        xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");

        xmldoc.AppendChild(xmlnode);
        //加入一个根元素
        xmlelem_ROOT = xmldoc.CreateElement("", "PayPlatRequestParameter", "");
        xmldoc.AppendChild(xmlelem_ROOT);

        ///////////////////////////////////////////

        xmlelem_CTRLINFO = xmldoc.CreateElement("", "CTRL-INFO", "");
        xmldoc.ChildNodes.Item(1).AppendChild(xmlelem_CTRLINFO);
        xmlelem_CTRLINFO.SetAttribute("WEBSVRNAME", "开户（全部账户）");  //服务名称，参考附录10.2
        xmlelem_CTRLINFO.SetAttribute("WEBSVRCODE", "100201");   //服务编码，参考附录10.2
        xmlelem_CTRLINFO.SetAttribute("APPFROM", "100201|310000-TEST1-127.0.0.1|1.0|127.0.0.1");  //20 位   请求此服务的客户端标识编码 格式：省份-应用系统名称-版本号-IP 省份编码参考：9.9
        xmlelem_CTRLINFO.SetAttribute("KEEP", KEEP);    //2012 09 16 20 21 52

        xmlelem_PARAMETERS = xmldoc.CreateElement("PARAMETERS");
        xmldoc.ChildNodes.Item(1).AppendChild(xmlelem_PARAMETERS);




        //CARDNO
        //xmlelem_CARDNO = xmldoc.CreateElement("CARDNO");
        //xmltext = xmldoc.CreateTextNode(CARDNO);
        //xmlelem_CARDNO.AppendChild(xmltext);
        //xmlelem_PARAMETERS.AppendChild(xmlelem_CARDNO);


        //CARDTYPE
        //xmlelem_CARDTYPE = xmldoc.CreateElement("CARDTYPE");
        //xmltext = xmldoc.CreateTextNode(CARDTYPE);
        //xmlelem_CARDTYPE.AppendChild(xmltext);
        //xmlelem_PARAMETERS.AppendChild(xmlelem_CARDTYPE);


        //NAME
        xmlelem_NAME = xmldoc.CreateElement("NAME");
        xmltext = xmldoc.CreateTextNode(NAME);
        xmlelem_NAME.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_NAME);


        //LOGINID
        xmlelem_LOGINID = xmldoc.CreateElement("LOGINID");
        xmltext = xmldoc.CreateTextNode(LOGINID);
        xmlelem_LOGINID.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_LOGINID);

        //PRODUCTNO
        xmlelem_PRODUCTNO = xmldoc.CreateElement("PRODUCTNO");
        xmltext = xmldoc.CreateTextNode(PRODUCTNO);
        xmlelem_PRODUCTNO.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_PRODUCTNO);

        //ISREALNAME
        xmlelem_ISREALNAME = xmldoc.CreateElement("ISREALNAME");
        xmltext = xmldoc.CreateTextNode(ISREALNAME);
        xmlelem_ISREALNAME.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ISREALNAME);

        //xmlelem_REGISTERTYPE
        xmlelem_REGISTERTYPE = xmldoc.CreateElement("REGISTERTYPE");
        xmltext = xmldoc.CreateTextNode(REGISTERTYPE);
        xmlelem_REGISTERTYPE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_REGISTERTYPE);

        //xmlelem_ISAPPLYCERT
        xmlelem_ISAPPLYCERT = xmldoc.CreateElement("ISAPPLYCERT");
        xmltext = xmldoc.CreateTextNode(ISAPPLYCERT);
        xmlelem_ISAPPLYCERT.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ISAPPLYCERT);

        //xmlelem_CUSTOMLEVEL
        xmlelem_CUSTOMLEVEL = xmldoc.CreateElement("CUSTOMLEVEL");
        xmltext = xmldoc.CreateTextNode(CUSTOMLEVEL);
        xmlelem_CUSTOMLEVEL.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CUSTOMLEVEL);

        //xmlelem_CUSTOMTYPE
        xmlelem_CUSTOMTYPE = xmldoc.CreateElement("CUSTOMTYPE");
        xmltext = xmldoc.CreateTextNode(CUSTOMTYPE);
        xmlelem_CUSTOMTYPE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CUSTOMTYPE);


        //xmlelem_REGCELLPHONENUM
        xmlelem_REGCELLPHONENUM = xmldoc.CreateElement("REGCELLPHONENUM");
        xmltext = xmldoc.CreateTextNode(REGCELLPHONENUM);
        xmlelem_REGCELLPHONENUM.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_REGCELLPHONENUM);

        //xmlelem_REGEMAIL
        xmlelem_REGEMAIL = xmldoc.CreateElement("REGEMAIL");
        xmltext = xmldoc.CreateTextNode(REGEMAIL);
        xmlelem_REGEMAIL.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_REGEMAIL);

        //xmlelem_LOGINPASSWORD
        xmlelem_LOGINPASSWORD = xmldoc.CreateElement("LOGINPASSWORD");
        xmltext = xmldoc.CreateTextNode(LOGINPASSWORD);
        xmlelem_LOGINPASSWORD.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_LOGINPASSWORD);

        //xmlelem_SEX
        xmlelem_SEX = xmldoc.CreateElement("SEX");
        xmltext = xmldoc.CreateTextNode(SEX);
        xmlelem_SEX.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_SEX);

        //xmlelem_CERTTYPE
        xmlelem_CERTTYPE = xmldoc.CreateElement("CERTTYPE");
        xmltext = xmldoc.CreateTextNode(CERTTYPE);
        xmlelem_CERTTYPE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CERTTYPE);

        //xmlelem_CERTNUM
        xmlelem_CERTNUM = xmldoc.CreateElement("CERTNUM");
        xmltext = xmldoc.CreateTextNode(CERTNUM);
        xmlelem_CERTNUM.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CERTNUM);

        //xmlelem_FAMILYTEL
        xmlelem_FAMILYTEL = xmldoc.CreateElement("FAMILYTEL");
        xmltext = xmldoc.CreateTextNode(FAMILYTEL);
        xmlelem_FAMILYTEL.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_FAMILYTEL);

        //xmlelem_OFFICETEL
        xmlelem_OFFICETEL = xmldoc.CreateElement("OFFICETEL");
        xmltext = xmldoc.CreateTextNode(OFFICETEL);
        xmlelem_OFFICETEL.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_OFFICETEL);


        //xmlelem_APANAGE
        xmlelem_APANAGE = xmldoc.CreateElement("APANAGE");

        //xmltext = xmldoc.CreateTextNode("113310000000000");
        xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.APANAGE);
        //BesttoneAccountConstDefinition.DefaultInstance.APANAGE;
        xmlelem_APANAGE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_APANAGE);


        //xmlelem_AREACODE
        xmlelem_AREACODE = xmldoc.CreateElement("AREACODE");
        xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.AREACODE);
        xmlelem_AREACODE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_AREACODE);

        //xmlelem_CITYCODE
        xmlelem_CITYCODE = xmldoc.CreateElement("CITYCODE");
        xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.CITYCODE);
        xmlelem_CITYCODE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CITYCODE);

        //xmlelem_CONTRACTADD
        xmlelem_CONTRACTADD = xmldoc.CreateElement("CONTRACTADD");
        xmltext = xmldoc.CreateTextNode(CONTRACTADD);
        xmlelem_CONTRACTADD.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CONTRACTADD);


        //xmlelem_COMPANYADD
        xmlelem_COMPANYADD = xmldoc.CreateElement("COMPANYADD");
        xmltext = xmldoc.CreateTextNode(COMPANYADD);
        xmlelem_COMPANYADD.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_COMPANYADD);


        //xmlelem_COMPANYZIP
        xmlelem_COMPANYZIP = xmldoc.CreateElement("COMPANYZIP");
        xmltext = xmldoc.CreateTextNode(COMPANYZIP);
        xmlelem_COMPANYZIP.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_COMPANYZIP);

        //xmlelem_COMPANYCODE
        xmlelem_COMPANYCODE = xmldoc.CreateElement("COMPANYCODE");
        xmltext = xmldoc.CreateTextNode(COMPANYCODE);
        xmlelem_COMPANYCODE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_COMPANYCODE);

        //ACCEPTORGCODE	受理机构代码 001310000000000
        //SUPPLYORGCODE	出单机构 113310000000000
        //xmlelem_ACCEPTORGCODE
        xmlelem_ACCEPTORGCODE = xmldoc.CreateElement("ACCEPTORGCODE");
        //xmltext = xmldoc.CreateTextNode("001310000000000");
        xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE);

        //BesttoneAccountConstDefinition.DefaultInstance.ACCEPTORGCODE;
        xmlelem_ACCEPTORGCODE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTORGCODE);

        //xmlelem_ACCEPTUID
        xmlelem_ACCEPTUID = xmldoc.CreateElement("ACCEPTUID");
        xmltext = xmldoc.CreateTextNode(ACCEPTUID);
        xmlelem_ACCEPTUID.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTUID);

        //xmlelem_ACCEPTAREACODE
        xmlelem_ACCEPTAREACODE = xmldoc.CreateElement("ACCEPTAREACODE");
        xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.ACCEPTAREACODE);
        xmlelem_ACCEPTAREACODE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTAREACODE);

        //xmlelem_ACCEPTCITYCODE
        xmlelem_ACCEPTCITYCODE = xmldoc.CreateElement("ACCEPTCITYCODE");
        xmltext = xmldoc.CreateTextNode(BesttoneAccountConstDefinition.DefaultInstance.ACCEPTCITYCODE);
        xmlelem_ACCEPTCITYCODE.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTCITYCODE);


        //xmlelem_ACCEPTCHANNEL
        xmlelem_ACCEPTCHANNEL = xmldoc.CreateElement("ACCEPTCHANNEL");
        xmltext = xmldoc.CreateTextNode(ACCEPTCHANNEL);
        xmlelem_ACCEPTCHANNEL.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTCHANNEL);

        //xmlelem_ACCEPTSEQNO
        xmlelem_ACCEPTSEQNO = xmldoc.CreateElement("ACCEPTSEQNO");
        xmltext = xmldoc.CreateTextNode(ACCEPTSEQNO);
        xmlelem_ACCEPTSEQNO.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_ACCEPTSEQNO);

        //xmlelem_FEEFLAG
        xmlelem_FEEFLAG = xmldoc.CreateElement("FEEFLAG");
        xmltext = xmldoc.CreateTextNode(FEEFLAG);
        xmlelem_FEEFLAG.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_FEEFLAG);


        //xmlelem_FEEAMT
        xmlelem_FEEAMT = xmldoc.CreateElement("FEEAMT");
        xmltext = xmldoc.CreateTextNode(FEEAMT);
        xmlelem_FEEAMT.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_FEEAMT);

        //xmlelem_INPUTUID
        xmlelem_INPUTUID = xmldoc.CreateElement("INPUTUID");
        xmltext = xmldoc.CreateTextNode(INPUTUID);
        xmlelem_INPUTUID.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_INPUTUID);


        //xmlelem_INPUTTIME
        xmlelem_INPUTTIME = xmldoc.CreateElement("INPUTTIME");
        xmltext = xmldoc.CreateTextNode(DateTime.Now.ToString("yyyyMMddHHmmss"));
        xmlelem_INPUTTIME.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_INPUTTIME);

        //xmlelem_CHECKUID
        xmlelem_CHECKUID = xmldoc.CreateElement("CHECKUID");
        xmltext = xmldoc.CreateTextNode(CHECKUID);
        xmlelem_CHECKUID.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CHECKUID);

        //xmlelem_CHECKTIME
        xmlelem_CHECKTIME = xmldoc.CreateElement("CHECKTIME");
        xmltext = xmldoc.CreateTextNode(DateTime.Now.ToString("yyyyMMddHHmmss"));
        xmlelem_CHECKTIME.AppendChild(xmltext);
        xmlelem_PARAMETERS.AppendChild(xmlelem_CHECKTIME);


        ResultXML = xmldoc.OuterXml;


        return Result;
    }


    protected void Page_Load(object sender, EventArgs e)
    {


                //请求参数
                String CustID = Request["CustID"];
                String SPID = Request["SPID"];
                String Phone = Request["Phone"];
                String IDCard = Request["IDCard"];
                String AuthenCode = Request["AuthenCode"];
                String RealName = Request["RealName"];
                String ContactTel = Request["ContactTel"];
                String Email = Request["Email"];
                String Sex = Request["Sex"];
                String wt = Request["wt"];   // json or xml
                String ResponseText = OpenBesttoneAccount(SPID, CustID, Phone,IDCard,RealName,ContactTel,Email,Sex, AuthenCode, wt);
                if (!"json".Equals(wt))
                {
                    Response.ContentType = "xml/text";
                }

        
                Response.Write(ResponseText);
                Response.Flush();
                Response.End();

    }
}

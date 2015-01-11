using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Linkage.BestTone.Interface.Rule;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
/// <summary>
/// IPushPortClientService 的摘要说明
/// </summary>
[WebServiceBinding(ConformsTo = WsiProfiles.None)]
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Web.Services.WebServiceAttribute(Namespace = "www.fsti.com")]
[System.Web.Services.WebServiceBindingAttribute(Name = "PushPortClientSoapBinding", Namespace = "www.fsti.com")]
[System.Xml.Serialization.SoapIncludeAttribute(typeof(ShortMessage))]
[System.Xml.Serialization.SoapIncludeAttribute(typeof(DeliverState))]

public class IPushPortClientService : System.Web.Services.WebService
{

    public IPushPortClientService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }



    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "www.fsti.com", ResponseNamespace = "www.fsti.com")]
    public void notifyRecivedShortMessages(ShortMessage[] in0)
    {

        DateTime starttime = DateTime.Now;
        DateTime endtime = DateTime.Now;
        System.TimeSpan delta = endtime.Subtract(starttime);
         //in0[0].deliverTime 
         //in0[0].srcPhoneNumber
         //in0[0].msgContent      RZ(设置认证手机) CZ(重置密码)
         //in0[0].destPhoneNumber

        String NeedSendDownSMS = System.Configuration.ConfigurationManager.AppSettings["NeedSendDownSMS"];

        StringBuilder strLog = new StringBuilder();
        strLog.Append("===="+starttime+"=======");
        strLog.AppendFormat("主叫手机号码:{0};被叫号码:{1};上行短信内容:{2}\r\n",in0[0].srcPhoneNumber,in0[0].destPhoneNumber,in0[0].msgContent);
        
        Int32 Result=0;
        String ErrMsg="";
        String SPID = "";
        //11811411   10690007311811    11811412   10690007311812
        try
        {
            if ("11811411".Equals(in0[0].destPhoneNumber) || "10690007311811".Equals(in0[0].destPhoneNumber))
            {
                SPID = "35433333";
            }
            else
            {
                SPID = "35433334";
            }
            String msg = "验证码:";
            String CustID = "";
            String type = String.Empty;
            type = in0[0].msgContent.ToLower();   // 类型 cz 重置密码   ,rz 认证手机   ,kt 开通账户

            if ("cz".Equals(type))
            {
                msg = "您正在通过手机重置密码，验证码为:";
                CustID = PhoneBO.IsAuthenPhone(in0[0].srcPhoneNumber, SPID, out ErrMsg);
                if (!String.IsNullOrEmpty(CustID))
                {
                    Random random = new Random();
                    String AuthenCode = random.Next(111111, 999999).ToString();
                    starttime = DateTime.Now;

                    SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                    SqlCommand cmd = new SqlCommand("select mesage from SmsTemplate where id=2",conn);
                    using (conn)
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            msg = (string)reader["mesage"];
                        }
                    }
                    StringBuilder msgtemplate = new StringBuilder();
                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = "您正在通过手机重置密码，验证码为:{0},有效期2分钟。";
                    }
                    msgtemplate.AppendFormat(msg, AuthenCode);
                    if (String.IsNullOrEmpty(NeedSendDownSMS))
                    {
                        CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msgtemplate.ToString(), SPID);
                    }
                    Result = PhoneBO.InsertPhoneSendMassage(CustID, msgtemplate.ToString(), AuthenCode, in0[0].srcPhoneNumber, DateTime.Now, in0[0].msgContent, 1, 0, "1", out ErrMsg);
                    endtime = DateTime.Now;
                    delta = endtime.Subtract(starttime);
                    strLog.AppendFormat("下发短信消耗时间:{0}\r\n", delta.Milliseconds);

                }
                else
                {
                    SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                    SqlCommand cmd = new SqlCommand("select mesage from SmsTemplate where id=3",conn);
                    using (conn)
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            msg = (string)reader["mesage"];
                        }
                    }
                    StringBuilder msgtemplate = new StringBuilder();
                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = "亲，您正在通过手机获取验证码来重置密码,但是{0}不是号百客户的认证手机,不能通过该号码找回密码。";
                    }
                    msgtemplate.AppendFormat(msg, in0[0].srcPhoneNumber);
                    if (String.IsNullOrEmpty(NeedSendDownSMS))
                    {
                        CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msgtemplate.ToString(), SPID);
                    }
                    //CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, "亲，您正在通过手机获取验证码来重置密码,但是" + in0[0].srcPhoneNumber + "不是号百客户的认证手机,不能通过该号码找回密码。", SPID);

                }

            }

            if ("rz".Equals(type))
            {
                strLog.AppendFormat("CustID:{0}\r\n", CustID);
                Random random = new Random();
                String AuthenCode = random.Next(111111, 999999).ToString();
                //int k = PhoneBO.PhoneSelV2("", in0[0].srcPhoneNumber, out ErrMsg);    // 验证电话是否可以做认证电话(这里的电话包括手机和电话)
                //int k = 0;
                starttime = DateTime.Now;
                int k = PhoneBO.PhoneSel(CustID, in0[0].srcPhoneNumber, out ErrMsg);   // 验证电话是否可以做认证电话(这里的电话包括手机和电话) 以及发送次数控制
                endtime = DateTime.Now;
                delta = endtime.Subtract(starttime);
                strLog.AppendFormat("验证手机是否是认证手机以及发送短信次数控制:{0};{1}\r\n", k, ErrMsg);
                strLog.AppendFormat("消耗查询电话时间:{0}\r\n", delta.Milliseconds);
                if (k == 0)
                {
               
                    msg = "欢迎注册号码百事通会员，验证码为:";
            
                    strLog.AppendFormat("验证码类型:{0}\r\n", msg);
                    starttime = DateTime.Now;

                    SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                    SqlCommand cmd = new SqlCommand("select mesage from SmsTemplate where id=4",conn);
                    using (conn)
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            msg = (string)reader["mesage"];
                        }
                    }
                    StringBuilder msgtemplate = new StringBuilder();
                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = "亲，欢迎注册号码百事通会员，验证码为:{0}，有效期2分钟。";
                    }
                    msgtemplate.AppendFormat(msg, AuthenCode);
                    if (String.IsNullOrEmpty(NeedSendDownSMS))
                    {
                        CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msgtemplate.ToString(), SPID);
                    } 
                    //CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msg + AuthenCode + "，有效期2分钟。", SPID);
                    Result = PhoneBO.InsertPhoneSendMassage(CustID, msg + AuthenCode + "，有效期2分钟。", AuthenCode, in0[0].srcPhoneNumber, DateTime.Now, in0[0].msgContent, 1, 0, "1", out ErrMsg);
                    endtime = DateTime.Now;
                    delta = endtime.Subtract(starttime);
                    strLog.AppendFormat("下发短信消耗时间:{0}\r\n", delta.Milliseconds);
                }
                else
                {
                    SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                    SqlCommand cmd = new SqlCommand("select mesage from SmsTemplate where id=5",conn);
                    using (conn)
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            msg = (string)reader["mesage"];
                        }
                    }
                    StringBuilder msgtemplate = new StringBuilder();
                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = "亲，您的手机号码:{0}已经是号码百事通客户了,无须再次为该号码设置认证手机。";
                    }
                    msgtemplate.AppendFormat(msg, in0[0].srcPhoneNumber);
                    if (String.IsNullOrEmpty(NeedSendDownSMS))
                    {
                        CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msgtemplate.ToString(), SPID);
                    } 
                    
                    //   CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, "亲，您的手机号码" + in0[0].srcPhoneNumber + "已经是号码百事通客户了,无须再次为该号码设置认证手机。", SPID);
                }


            }


            if ("kt".Equals(type))
            {
                strLog.AppendFormat("开通账户CustID:{0}\r\n", CustID);
                Random random = new Random();
                String AuthenCode = random.Next(111111, 999999).ToString();

                starttime = DateTime.Now;
                Result = PhoneBO.IsBesttoneAccountBind(in0[0].srcPhoneNumber, out ErrMsg);   // 验证电话是否可以做账户
                endtime = DateTime.Now;
                delta = endtime.Subtract(starttime);
                strLog.AppendFormat("验证手机是否是可以做账户号码:{0};{1}\r\n", Result, ErrMsg);
                strLog.AppendFormat("消耗IsBesttoneAccountBind时间:{0}\r\n", delta.Milliseconds);
                if (Result == 0)
                {
                    msg = "欢迎注册开通号码百事通账户，验证码为:";

                    strLog.AppendFormat("验证码类型:{0}\r\n", msg);
                    starttime = DateTime.Now;

                    SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                    SqlCommand cmd = new SqlCommand("select mesage from SmsTemplate where id=6",conn);
                    using (conn)
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            msg = (string)reader["mesage"];
                        }
                    }
                    StringBuilder msgtemplate = new StringBuilder();
                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = "亲，欢迎注册开通号码百事通账户，验证码为:{0}，有效期2分钟。";
                    }
                    msgtemplate.AppendFormat(msg, AuthenCode);
                    if (String.IsNullOrEmpty(NeedSendDownSMS))
                    {
                        CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msgtemplate.ToString(), SPID);
                    } 
                    //CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msg + AuthenCode + "，有效期2分钟。", SPID);
                    Result = PhoneBO.InsertPhoneSendMassage(CustID, msg + AuthenCode + "，有效期2分钟。", AuthenCode, in0[0].srcPhoneNumber, DateTime.Now, in0[0].msgContent, 1, 0, "1", out ErrMsg);
                    endtime = DateTime.Now;
                    delta = endtime.Subtract(starttime);
                    strLog.AppendFormat("下发短信消耗时间:{0}\r\n", delta.Milliseconds);
                }
                else
                {

                    SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
                    SqlCommand cmd = new SqlCommand("select mesage from SmsTemplate where id=7",conn);
                    using (conn)
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            msg = (string)reader["mesage"];
                        }
                    }
                    StringBuilder msgtemplate = new StringBuilder();
                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = "亲，您的手机号码:{0}已经开通过号码百事通账户了,无须再次为该号码开通账户。";
                    }
                    msgtemplate.AppendFormat(msg, in0[0].srcPhoneNumber);
                    if (String.IsNullOrEmpty(NeedSendDownSMS))
                    {
                        CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, msgtemplate.ToString(), SPID);
                    } 
                    //CommonBizRules.SendMessageV3(in0[0].srcPhoneNumber, "亲，您的手机号码" + in0[0].srcPhoneNumber + "已经开通过号码百事通账户了,无须再次为该号码开通账户。", SPID);
                }

            }

  
        }
        catch (Exception e)
        {
            strLog.AppendFormat("发生异常:{0}",e.Message);
        }
        finally
        {
            BTUCenterInterfaceLog.CenterForBizTourLog("PushPortClientService", strLog);  
        }
        //CommonBizRules.SendMessageV3("18930036387", "哈哈哈哈", "35433333");
    }

}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.SoapTypeAttribute(Namespace = "www.fsti.com")]
public partial class DeliverState
{

    private string deliverStateField;

    private long queryHandlerField;

    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
    public string deliverState
    {
        get
        {
            return this.deliverStateField;
        }
        set
        {
            this.deliverStateField = value;
        }
    }

    /// <remarks/>
    public long queryHandler
    {
        get
        {
            return this.queryHandlerField;
        }
        set
        {
            this.queryHandlerField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.SoapTypeAttribute(Namespace = "www.fsti.com")]
public partial class ShortMessage
{

    private string deliverTimeField;

    private string destPhoneNumberField;

    private string linkidField;

    private string msgContentField;

    private string reserveField;

    private string srcPhoneNumberField;

    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
    public string deliverTime
    {
        get
        {
            return this.deliverTimeField;
        }
        set
        {
            this.deliverTimeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
    public string destPhoneNumber
    {
        get
        {
            return this.destPhoneNumberField;
        }
        set
        {
            this.destPhoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
    public string linkid
    {
        get
        {
            return this.linkidField;
        }
        set
        {
            this.linkidField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
    public string msgContent
    {
        get
        {
            return this.msgContentField;
        }
        set
        {
            this.msgContentField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
    public string reserve
    {
        get
        {
            return this.reserveField;
        }
        set
        {
            this.reserveField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
    public string srcPhoneNumber
    {
        get
        {
            return this.srcPhoneNumberField;
        }
        set
        {
            this.srcPhoneNumberField = value;
        }
    }
}
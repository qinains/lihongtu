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
using Linkage.BestTone.Interface.Rule;
using System.Data.SqlClient;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.BTException;
using System.Xml;
using System.Xml.Serialization;

public partial class SSO_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        //string returnurl=UserRegistry("111", "" + this.tx1.Value + "", "1", "1", "1", "222");
        //Response.Redirect(returnurl);
    }

   

    protected void Button2_Click(object sender, EventArgs e)
    {
        XMLExchange xMLExchange = new XMLExchange();
        string RealName,UserName,NickName,OutID,CustType,CustID,RealName1="";
        string ErrMsg = "";
        int Result = -1;
        string rStr = "<ContractRoot><TcpCont><ActionCode>1</ActionCode><TransactionID>1000000020201006182031463789</TransactionID><RspTime>20100618110036</RspTime><Response><RspType>0</RspType><RspCode>0000</RspCode><RspDesc>成功</RspDesc></Response></TcpCont><SvcCont><QryInfoRsp><InfoTypeID>31</InfoTypeID><InfoCont><CustInfo><BelongInfo><ProvinceCode>609001</ProvinceCode><ProvinceName>北京</ProvinceName><CityCode>010</CityCode><CityName>北京市</CityName></BelongInfo><PartyCodeInfo><CodeType>15</CodeType><CodeValue>200003412729</CodeValue><CityCode>010</CityCode></PartyCodeInfo><IdentityInfo><IdentType>1</IdentType><IdentNum>110221198109288314</IdentNum></IdentityInfo><CustName>杨悦</CustName><CustBrand>14</CustBrand><CustGroup>12</CustGroup><CustServiceLevel>14</CustServiceLevel><CustAddress>北京市昌平区东关南里33号楼1单元6号</CustAddress></CustInfo><PointInfo><PointType>1</PointType><PointValueSum>2188</PointValueSum><PointValue>2176</PointValue><PointTime>30000101</PointTime><PointItems><PointItemID>1</PointItemID><PointItemName>消费积分</PointItemName><PointItemValue>520</PointItemValue><PointItemTime>30000101</PointItemTime></PointItems><PointItems><PointItemID>3</PointItemID><PointItemName>奖励积分</PointItemName><PointItemValue>65</PointItemValue><PointItemTime>30000101</PointItemTime></PointItems><PointItems><PointItemID>4</PointItemID><PointItemName>其它积分</PointItemName><PointItemValue>1591</PointItemValue><PointItemTime>30000101</PointItemTime></PointItems><PointItems><PointItemID>5</PointItemID><PointItemName>机场贵宾厅服务免费总次数</PointItemName><PointItemValue>0</PointItemValue><PointItemTime>30000101</PointItemTime></PointItems><PointItems><PointItemID>6</PointItemID><PointItemName>机场贵宾厅服务剩余免费总次数</PointItemName><PointItemValue>0</PointItemValue><PointItemTime>30000101</PointItemTime></PointItems><PointItems><PointItemID>7</PointItemID><PointItemName>火车站贵宾厅服务免费总次数</PointItemName><PointItemValue>0</PointItemValue><PointItemTime>30000101</PointItemTime></PointItems><PointItems><PointItemID>8</PointItemID><PointItemName>火车站贵宾厅服务剩余免费总次数</PointItemName><PointItemValue>0</PointItemValue><PointItemTime>30000101</PointItemTime></PointItems></PointInfo></InfoCont></QryInfoRsp></SvcCont></ContractRoot>";
        try{
             QryCustInfoReturn qryCustInfoReturn = xMLExchange.AnalysisQryCustInfoXML(rStr);

                    if (qryCustInfoReturn.TcpCont.Response.RspType == "0")
                    {

                        RealName = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustName;
                        UserName = "";
                        NickName = "";
                        OutID = qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.PartyCodeInfo.CodeValue;

                        //10	政企客户 10
                        //11	家庭客户 20
                        //12	个人客户 30
                        //99	其它客户 90

                        switch (qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustGroup)
                        {
                            case "10":
                                CustType = "10";
                                break;
                            case "11":
                                CustType = "20";
                                break;
                            case "12":
                                CustType = "30";
                                break;
                            case "99":
                                CustType = "90";
                                break;
                            default:
                                CustType = "90";
                                break;
                        }


                        CustID = "";


                        /*0	  统一客户标识码 9
                        *1	  身份证         0
                        *2	  军官证         2   
                        *3	  护照           3
                        *4	  港澳台通行证   6
                        *5	  部队干部离休证 9
                        *6	  工商营业执照   9
                        *7	  单位证明       9
                        *9	  驾驶证         9
                        *10	  学生证         9
                        *11	  教师证         9
                        *12	  户口本/居住证  9
                        *13	  老人证         9
                        *14	  士兵证         1
                        *15	  组织机构代码证 9
                        *17	  工作证         9
                        *18	  暂住证         9
                        *19	  电信识别编码   9
                        *20	  集团客户标识码 9
                        *21	  VIP卡          9
                        *99	  其它           9
                        *
                        0－身份证
                        1－士兵证
                        2－军官证
                        3－护照
                        4－保留
                        5－台胞证
                        6－港澳通行证
                        7－国际海员证
                        9－其它
                        10-部队干部离休证
                        11-工商营业执照
                        12-单位证明
                        13-驾驶证
                        14-学生证
                        15-教师证
                        16-户口本/居住证
                        17-老人证
                        18-组织机构代码证
                        19-工作证
                        20-暂住证
                        21-电信识别编码
                        22-集团客户标识码
                        23-VIP卡
                        24-警官证

                        */
                        string IdentType = "";


                        switch (qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType)
                        {
                            case "0":
                                IdentType = "25";
                                break;
                            case "1":
                                IdentType = "0";
                                break;
                            case "2":
                                IdentType = "2";
                                break;
                            case "3":
                                IdentType = "3";
                                break;
                            case "4":
                                IdentType = "6";
                                break;
                            case "5":
                                IdentType = "10";
                                break;
                            case "6":
                                IdentType = "11";
                                break;
                            case "7":
                                IdentType = "12";
                                break;
                            case "9":
                                IdentType = "13";
                                break;
                            case "10":
                                IdentType = "14";
                                break;
                            case "11":
                                IdentType = "15";
                                break;
                            case "12":
                                IdentType = "16";
                                break;
                            case "13":
                                IdentType = "17";
                                break;
                            case "14":
                                IdentType = "1";
                                break;
                            case "15":
                                IdentType = "18";
                                break;
                            case "17":
                                IdentType = "19";
                                break;
                            case "18":
                                IdentType = "20";
                                break;
                            case "19":
                                IdentType = "21";
                                break;
                            case "20":
                                IdentType = "22";
                                break;
                            case "21":
                                IdentType = "23";
                                break;
                            case "22":
                                IdentType = "24";
                                break;
                            default:
                                IdentType = "9";
                                break;
                        }


                        if (RealName1 != "")
                            RealName = RealName1;

                        //全国CRM用户注册到号百
                        Result = UserRegistry.getUserRegistryCrm("",
                            qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                            //"021",
                            CustType,//客户类型
                            IdentType,
                            qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                            RealName,
                            qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel,
                            "2",//未知
                            OutID,
                            "",
                            qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                            out CustID,
                           out  ErrMsg);

                        if (Result != 0)
                        {
                            //err_code.InnerHtml = ErrMsg;
                           
                        }
                        CIP2BizRules.InsertCustInfoNotify(CustID, "2", "", "", "0", out ErrMsg);

                    }
                    else
                    {

                        ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                                             + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                                             + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;

                        //return -1;
                        int rspcode = -1;
                        try
                        {
                            rspcode = int.Parse(qryCustInfoReturn.TcpCont.Response.RspCode);
                        }
                        catch { }
                      

                    }
                }
                catch (Exception ex1)
                {
                    ErrMsg = ex1.Message.ToString();
                    Result = -29999;
                }
              
       
    }

    /// <summary>
    /// CAP02001票据解读函数,省ua认证|取断言地址
    /// </summary>
    /// <param name="UATicket">断言</param>       
    /// <returns>BilByCompilingResult</returns>
    public MBOSSClass.BilByCompilingResult BilByCompiling(string UATicketXML)
        {
            MBOSSClass.BilByCompilingResult Result = new MBOSSClass.BilByCompilingResult();
        //    string DigitalSign = GetValueFromXML(UATicketXML, "DigitalSign");

       //     Result.Result = VerifySignByPublicKey(UATicketXML, PublicKeyFile, DigitalSign, out Result.ErrMsg);

            Result.Assertion = GetValueFromXML(UATicketXML, "Assertion");
            Result.AccountID = GetValueFromXML(UATicketXML, "AccountID");
            Result.AccountType = GetValueFromXML(UATicketXML, "AccountType");
            Result.AudienceID = GetValueFromXML(UATicketXML, "AudienceID");
            Result.AuthInstant = GetValueFromXML(UATicketXML, "AuthInstant");
            Result.AuthMethod = GetValueFromXML(UATicketXML, "AuthMethod");
            Result.IssueInstant = GetValueFromXML(UATicketXML, "IssueInstant");
            Result.NotBefore = GetValueFromXML(UATicketXML, "NotBefore");
            Result.NotOnOrAfter = GetValueFromXML(UATicketXML, "NotOnOrAfter");
            Result.UA_URL = GetValueFromXML(UATicketXML, "UA_URL");
            Result.UAID = GetValueFromXML(UATicketXML, "UAID");

            MBOSSClass.AuthenRecord[] AccountInfos = GetAccountInfoFromXML(UATicketXML);
            Result.AccountInfos = AccountInfos;
       
            switch (Result.AccountType)
            {
                case "2000001":                   
                    Result.AccountType = "9";
                    break;
                case "2000002":                   
                    Result.AccountType = "11";
                    break;
                case "2000003":
                    Result.AccountType = "10";
                  
                    break;
                case "2000004":
                    Result.AccountType = "7";
                    break;  
                case "0000000":
                    Result.AccountType = "99";
                    break;
                case "0000001":
                    Result.AccountType = "5";
                    break;               
                default:
                    Result.AccountType = "-1";
                    break;
            }

         
         
            return Result;
        }

        #region 网厅返回给UA系统的可接受的帐号列表解析
        /// <summary>
        /// 网厅返回给UA系统的可接受的帐号列表解析
        /// </summary>
        /// <param name="XmlInfo">返回的帐号列表（XML）</param>
        /// <returns></returns>
    public static MBOSSClass.AuthenRecord[] GetAccountInfoFromXML(string XmlInfo)
        {

            string XMLValue = "";
            XmlNodeList nodeList = null;
            XmlNodeList nodeList1 = null;
            MBOSSClass.AuthenRecord[] ais = new MBOSSClass.AuthenRecord[0];
            try
            {

                XmlDocument xmlReader = new XmlDocument();
                xmlReader.LoadXml(XmlInfo);

                nodeList = xmlReader.GetElementsByTagName("AccountList");
                nodeList1 = nodeList[0].SelectNodes("AccountInfo");
                ais = new MBOSSClass.AuthenRecord[nodeList1.Count];
                for (int i = 0; i < nodeList1.Count; i++)
                {
                    MBOSSClass.AuthenRecord ai = new MBOSSClass.AuthenRecord();
                    ai.AuthenName = nodeList1[0].SelectNodes("AccountID")[0].InnerText == null ? "" : nodeList1[i].SelectNodes("AccountID")[0].InnerText;
                    ai.AuthenType = nodeList1[0].SelectNodes("AccountType")[0].InnerText == null ? "" : nodeList1[i].SelectNodes("AccountType")[0].InnerText;

                    try
                    {
                        ai.areaid = "";
                        XmlNodeList node3 = nodeList1[i].SelectNodes("PWDAttrList/PWDAttr");
                        for (int r = 0; r < node3.Count; r++)
                        {
                            //     string ss = node3[r].SelectNodes("AttrName")[0].InnerText;
                            if (node3[r].SelectNodes("AttrName")[0].InnerText == "CityCode")
                            {
                                ai.areaid = node3[r].SelectNodes("AttrValue")[0].InnerText == null ? "" : node3[r].SelectNodes("AttrValue")[0].InnerText;
                            }
                        }


                    }
                    catch
                    {
                        ai.areaid = "";
                    }

                    ai.ExtendField = "";
                    switch (ai.AuthenType)
                    {
                        //case "2000001":
                        //    ai.AuthenType = "9";
                        //    break;
                        //case "2000002":
                        //    ai.AuthenType = "11";
                        //    break;
                        //case "2000003":
                        //    ai.AuthenType = "10";
                        //    break;
                        //case "2000004":
                        //    ai.AuthenType = "7";
                        //    break;
                        case "0000000":
                            ai.AuthenType = "0";
                            break;
                        default:
                            ai.AuthenType = "-1";
                            break;
                    }

                    ais[i] = ai;

                }
                // nodeValue = (nodeList1.Count != 0) ? nodeList1[0].OuterXml : "";
            }
            catch (Exception)
            { ais = new MBOSSClass.AuthenRecord[0]; }

            return ais;
        }
        #endregion

   


    protected void Button1_Click1(object sender, EventArgs e)
    {
        //string a = "11111";
        //int k = SetMail.SelSendEmailMassage("", "237881953@qq.com", "4321", out a);
       // Response.Write(k);

    }




    protected void Button3_Click(object sender, EventArgs e)
    {
       
      
    }

        public static string GetValueFromXML(string XmlInfo, string NodeName)
        {
            string nodeValue = "";

            try
            {
                XmlDocument xmlReader = new XmlDocument();
                xmlReader.LoadXml(XmlInfo);

                XmlNodeList nodeList = null;

                nodeList = xmlReader.GetElementsByTagName(NodeName);
                nodeValue = (nodeList.Count != 0) ? nodeList[0].InnerText : "";
            }
            catch (Exception)
            {
                nodeValue = "";
            }

            return nodeValue;
        }
        
}
public class BilByCompilingResult
{
    public string Assertion;//断言标识
    public string UAID;//断言颁发的UA的标识
    public string UA_URL;//断言颁发UA的URL
    public string NotBefore;//断言使用时间，不在此时间之前
    public string NotOnOrAfter;//断言使用时间，不在此时间之后
    public string IssueInstant;//断言颁发时间
    public string AudienceID;//断言接受者标识
    public string AuthInstant;//认证时间
    public string AuthMethod;//认证方式
    public string AccountType;//帐号类型
    public string AccountID;//帐号标识
    public AuthenRecord[] AccountInfos;//用户在业务系统中可接受的帐号列表

    public int Result;//返回值
    public string ErrMsg;//返回信息

}
public class AuthenRecord
{
    public string AuthenType;
    public string AuthenName;
    public string areaid;
    public string ExtendField;
}
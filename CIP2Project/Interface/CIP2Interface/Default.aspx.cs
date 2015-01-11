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
using Linkage.BestTone.Interface.BTException;
using Linkage.BestTone.Interface.Utility;
using System.Text;


using System.Collections.Generic;


using System.Data.SqlClient;
using System.Xml;
using BTUCenter.Proxy;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string rStr = "<ContractRoot><TcpCont><ActionCode>1</ActionCode><TransactionID>1000000020201005136604177019</TransactionID><RspTime>20100513094916</RspTime><Response><RspType>9</RspType><RspCode>9016</RspCode><RspDesc>调用落地方异常：An error occurred whilst performing a socket operation: getsockopt</RspDesc></Response></TcpCont></ContractRoot>";
        string CustType = "";
        string RealName="";
        string UserName="";
        string NickName="";
        string OutID="";
        XMLExchange xMLExchange = new XMLExchange();
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


                    string CustID = "";


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
                    *0－身份证
                    *1－士兵证
                    *2－军官证
                    *3－护照
                    *4－保留
                    *5－台胞证
                    *6－港澳通行证
                    *7－国际海员证
                    *9－其他
                    */
                    string IdentType = "";


                    switch (qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentType)
                    {
                        case "1":
                            IdentType = "0";
                            break;
                        case "4":
                            IdentType = "6";
                            break;
                        case "14":
                            IdentType = "1";
                            break;
                        case "2":
                            IdentType = "2";
                            break;
                        case "3":
                            IdentType = "3";
                            break;
                        default:
                            IdentType = "9";
                            break;
                    }




                    //全国CRM用户注册到号百
                  //int  Result = UserRegistry.getUserRegistryCrm(UAProvinceID,
                  //      qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.BelongInfo.CityCode,
                  //      //"021",
                  //      CustType,//客户类型
                  //      IdentType,
                  //      qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.IdentityInfo.IdentNum,
                  //      RealName,
                  //      qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustServiceLevel,
                  //      "2",//未知
                  //      OutID,
                  //      System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"],
                  //      qryCustInfoReturn.SvcCont.QryInfoRsp.InfoCont.CustInfo.CustAddress,
                  //      out CustID,
                  //     out  ErrMsg);

                    //if (Result != 0)
                    //{
                    //    //err_code.InnerHtml = ErrMsg;
                    //    return Result;
                    //}
                    string ErrMsg = "";
                    CIP2BizRules.InsertCustInfoNotify(CustID, "2", System.Configuration.ConfigurationManager.AppSettings["ScoreBesttoneSPID"], "", "0", out ErrMsg);

                }
                else
                {

                    string ErrMsg = "错误类型及编码:" + qryCustInfoReturn.TcpCont.Response.RspType
                                         + "应答代码:" + qryCustInfoReturn.TcpCont.Response.RspCode
                                         + "描述:" + qryCustInfoReturn.TcpCont.Response.RspDesc;

                 //   return -1;

                }
          
         
        //string phone="";
        //this.Label1.Text=CommonBizRules.PhoneNumValid(this.Context, this.TextBox1.Text.Trim(), out phone)+"=="+phone;
        
    }
}

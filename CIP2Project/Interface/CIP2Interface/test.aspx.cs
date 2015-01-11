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
using System.Data.Sql;
using System.Data.SqlClient;
using Linkage.BestTone.Interface.Utility;
using Linkage.BestTone.Interface.Rule;

//using BTUCenter.Proxy;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(DateTime.Now.ToString("yyyy-mm-dd hh:mm", DateTimeFormatInfo.InvariantInfo));
        //Response.Write(DateTime.Now.ToString()+DateTime.Now.Millisecond.ToString());
        int i = 0;
    }
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
            if(leftStr=="0")
            {
                string leftStr2 = PhoneNum.Substring(1, 1);
                string leftStr3 = PhoneNum.Substring(2, 1);
                switch (leftStr2)
                {
                    case "1":
                        if (leftStr3 == "0")
                        { 
                            if(PhoneNum.Length==11)
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
                            if(PhoneNum.Length==12)
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
                    default :
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
            else if(leftStr=="1")
            {
                if(PhoneNum.Length!=11)
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        //Response.Write(DateTime.Now.ToString() +":"+ DateTime.Now.Millisecond.ToString());
        //return;
        string phone="";
        bool b = test.PhoneNumValid(this.Context, "07918816511", out phone);
        return;

        string connStr = System.Configuration.ConfigurationManager.AppSettings["BestToneCenterConStr"].ToString();
        SqlConnection conn = new SqlConnection(DBUtility.BestToneCenterConStr);
        try
        {
            //conn.Open();
            //if (conn.State == ConnectionState.Open)
            //    Label1.Text = "Open";
            //else
            //    Label1.Text = "closed";

            //Random rd = new Random();
            //string NewPassword = rd.Next(100000, 999999).ToString();
            //Response.Write(NewPassword);

            DateTime dd = DateTime.Now;
            DateTime ee = dd.AddMilliseconds(123);
           // long ff = ee.Ticks - dd.Ticks;

           //int ff = ee.CompareTo(dd,);
           // Response.Write(ff.ToString());
        }
        catch (Exception ex)
        {
            Label1.Text += ex.Message;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //testCRMUserAuthResult resultObj = new testCRMUserAuthResult();
        //testSoap obj = new testSoap();
        //obj.Url = "http://bksvc-test.besttone.com.cn/yuan/services/CRMForBTUCenterSoap12";

        //resultObj = obj.CRMUserAuth("3501", "1", "1", "1", "1", "1", "1");

        //string ErrMsg = resultObj.ErrorDescription;
        //int Result = resultObj.Result;
        int a = 0;

        


        AddressInfoRecord[] addressInfoRecords = new AddressInfoRecord[4];
        addressInfoRecords[0] = new AddressInfoRecord();
        addressInfoRecords[0].Address = "aaaaaa";
        addressInfoRecords[0].Type = "01";
        addressInfoRecords[0].Zipcode = "210031";
        addressInfoRecords[0].ExtendField = "";
        addressInfoRecords[0].DealType = "0";

        addressInfoRecords[1] = new AddressInfoRecord();
        addressInfoRecords[1].Address = "bbbbb";
        addressInfoRecords[1].Type = "01";
        addressInfoRecords[1].Zipcode = "210031";
        addressInfoRecords[1].ExtendField = "";
        addressInfoRecords[1].DealType = "0";

        addressInfoRecords[2] = new AddressInfoRecord();
        addressInfoRecords[2].Address = "cccccc";
        addressInfoRecords[2].Type = "01";
        addressInfoRecords[2].Zipcode = "210031";
        addressInfoRecords[2].ExtendField = "";
        addressInfoRecords[2].DealType = "0";

        addressInfoRecords[3] = new AddressInfoRecord();
        addressInfoRecords[3].Address = "ddddd";
        addressInfoRecords[3].Type = "01";
        addressInfoRecords[3].Zipcode = "210031";
        addressInfoRecords[3].ExtendField = "";
        addressInfoRecords[3].DealType = "0";


        CIPInterfaceForBizSystem face = new CIPInterfaceForBizSystem();
        //face.QueryByPhone("11211111", "15050686791", "");
        face.CustBasicInfoQuery("01010101", "417345346", "");
        //face.CustExtendInfoQuery("11211111", "8600010191502000", "");
        //face.CustProvinceRelationQuery("11211111", "02", "02", "");
        //face.AddressInfoQuery("11211111", "8600010191502000", "");
        //face.FrequentUserUploadQuery("12111111", "8600010191502000",addressInfoRecords, "");
        //face.FrequentUserQuery("12111111", "8600010191502000", "");
        //face.UserRegistryV2("12112222", "41", "", "111111", "20", "130183", "hello", "13029999900", "1419991600", "0", "", "", "1", "");
        //face.SetPwd("12112222", "8600010191502000", "111a11", "1", "");
        //face.SetPwd("12112222", "8600010191502000", "sdfgreert", "2", "");
        face.AddressInfoUpload("11111111", "10051", addressInfoRecords, "");
        AddressRecord[] addresses = new AddressRecord[2];
        addresses[0] = new AddressRecord();
        addresses[0].Address = "上海南汇";
        addresses[0].Type = "02";
        addresses[0].Zipcode = "210031";

        addresses[1] = new AddressRecord();
        addresses[1].Address = "江苏南京";
        addresses[1].Type = "02";
        addresses[1].Zipcode = "210032";

        BoundPhoneRecord[] boundPhoneRecords = new BoundPhoneRecord[1];
        boundPhoneRecords[0] = new BoundPhoneRecord();
        boundPhoneRecords[0].Phone = "15813999999";

        UserInfo userInfo = new UserInfo();
        userInfo.UserType = "20";
        userInfo.UserAccount = "";
        userInfo.Password = "111111"; ;
        userInfo.CustID = "";
        userInfo.UProvinceID = "20";
        userInfo.AreaCode = "020";
        userInfo.Status = "00";
        userInfo.RealName = "xiaotong";
        userInfo.CertificateType = "";
        userInfo.CertificateCode = "";
        userInfo.Birthday = "1999-09-29";
        userInfo.Sex = "1";
        userInfo.CustLevel = "4";
        userInfo.EduLevel = "5";
        userInfo.Favorite = "";
        userInfo.IncomeLevel = "3";
        userInfo.Email = "xiaotong@153.com";
        userInfo.CustContactTel = "15813999999";
        userInfo.AddressRecords = addresses;
        userInfo.BoundPhoneRecords = boundPhoneRecords;
        userInfo.ExtendField = "<?xml version='1.0' encoding='gb2312' standalone='yes'?><Root><UserName>xaotong</UserName><MobilePhone>15888866644</MobilePhone></Root>";

        BestToneUCenterForBizTour best = new BestToneUCenterForBizTour();
       // best.UserInfoQuery("20", "12342343", "203100004", "", "", DateTime.Now.ToString());
        //best.UserRegistry("20", "35000001", "05", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userInfo);
        
    }


    protected void Button3_Click(object sender, EventArgs e)
    {
//        BTUCenterForCRM crm = new BTUCenterForCRM();
//        Linkage.BestTone.Interface.Rule.AuthenRecord[] res = new Linkage.BestTone.Interface.Rule.AuthenRecord[1];
//        Linkage.BestTone.Interface.Rule.AuthenRecord re = new Linkage.BestTone.Interface.Rule.AuthenRecord();
//        res[0] = re;


//        crm.CustInfoUpload("14", "5044876", "09", "8631591101012099", "1", "新华通讯社福建分社",
//"", "福州市华林路新华通讯社福建分社", "", "133582760310000001", "4", "591", "2", "", "1", res, "");

        string strMSG = "";
        //CIP2BizRules.InsertCustInfoNotify("10058", "0", "35888888", "yyyyy", "0", out strMSG);
        CIP2BizRules.newCardCustomerInfoExport("92", "10058", "0", "yyyyy", "0", out strMSG, "");
        ////BestToneUCenterForBizTour face = new BestToneUCenterForBizTour();
        ////face.PhoneBind("21", "35000000", "2009-10-10", "8600100680036000", "54545", "13857881111");
        //BestToneUCenterForBizTour ff = new BestToneUCenterForBizTour();
        //ff.PhoneBind("21","35000000","2009-10-10","8600100680036000","54545","13857881111");
    }
}

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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Linkage.BestTone.Interface.Rule;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string Ticket = CommonBizRules.CreateTicket();
        //string sDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //Random r = new Random();
        //Ticket = sDate + r.Next(10000, 99999).ToString();
        Response.Write(Ticket);
        return;
       // Result.Result = CIPTicketManager.insertCIPTicket(Ticket, SPID, Result.CustID, RealName, NickName, UserName, outerid, Result.ErrorDescription, AuthenName, AuthenType, out Result.ErrorDescription);
        //string dd= CommonBizRules.GetSPOuterIDBySPID("35000001", this.Context);

        string dd = CommonBizRules.GetReginCodeByProvinceID("02", this.Context);
        //ProvinceInfoManager proInfo = new ProvinceInfoManager();
        //object ProData = proInfo.GetProvinceData(this.Context);
        //ProvinceData ds = (ProvinceData)ProData;

        //PhoneAreaInfoManager areaInfo = new PhoneAreaInfoManager();
        //object areaData = areaInfo.GetPhoneAreaData(this.Context);
        //PhoneAreaData pad = (PhoneAreaData)areaData;

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string str = "132423";
        string[] alUsername = str.Split('-');
        TextBox1.Text = alUsername[0];




        //string aaa = TextBox1.Text.Trim();
        //string ErrMsg = "";
        //int Result = CommonBizRules.CheckInterfaceLimit("35111111", aaa, this.Context, out ErrMsg);

        //Response.Write(Result.ToString() + "," + ErrMsg);
        //string CacheName = TextBox1.Text.Trim();
        //string PropertyName = TextBox2.Text.Trim();
        //SPInfoManager spInfo = new SPInfoManager();
        //Object SPData = spInfo.GetSPData(this.Context, CacheName);

        ////byte[] dd = spInfo.GetCAInfo("35111111", 0, SPData);

        //X509Certificate2 x509 = new X509Certificate2(dd, "12345678");
        //DSACryptoServiceProvider dsa = (DSACryptoServiceProvider)x509.PrivateKey;

        ////byte[] OriginalByte = System.Text.Encoding.UTF8.GetBytes(btCer);

        //byte[] s = dsa.SignData(dd);
        //string dd = spInfo.GetPropertyBySPID("35111111", PropertyName, SPData);
        //Response.Write(dd);

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        CerFileOP objCer = new CerFileOP();
        byte[] btCer = objCer.ReadFile(@"D:\我的工作\号百\号百三期\MBOSS单点登录\ua联调\cert\jfsc.p12");
        X509Certificate2 x509 = new X509Certificate2(btCer, "12345678");
        DSACryptoServiceProvider dsa = (DSACryptoServiceProvider)x509.PrivateKey;

        //byte[] OriginalByte = System.Text.Encoding.UTF8.GetBytes(btCer);

        byte[] s = dsa.SignData(btCer);

    }
}

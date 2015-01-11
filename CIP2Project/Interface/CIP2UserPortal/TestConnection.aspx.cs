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
using Linkage.BestTone.Interface.Rule.Young.Entity;
using System.Data.SqlClient;
using System.Data;
public partial class TestConnection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
       // System.Threading.Thread[] threadArray = new System.Threading.Thread[100];

        int cnt = Convert.ToInt32(ConfigurationManager.AppSettings["loopcnt"]);

        for (Int32 i = 0; i < cnt; i++)
        {
            TestMethod();
            //threadArray[i] = new System.Threading.Thread(new System.Threading.ThreadStart(TestMethod));
            //threadArray[i].Start();
        }
    }

    void TestMethod()
    {
        Int32 Result = -1;
        String ErrMsg = String.Empty;
        try
        {
            //for (Int32 i = 0; i < 50; i++)
            //{
                //根据电话取得相关的客户信息（客户电话表CustPhone）
                BasicInfoV2Record[] custinfoList = PhoneBO.GetQueryByPhone("35000000", "18918790558", out Result, out ErrMsg);
            //}
        }
        catch (Exception ex)
        {
            ErrMsg += ex.Message;
        }
        finally
        {
            log(ErrMsg);
        }
    }

    void log(string str)
    {
        System.Text.StringBuilder msg = new System.Text.StringBuilder();
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        msg.Append(str);
        msg.Append("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
        BTUCenterInterfaceLog.CenterForBizTourLog("MyTest", msg);
    }

    
}

using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;


/// <summary>
/// CIP2AuthInterface 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.mbossuac.com.cn/ua")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CIP2AuthInterface : System.Web.Services.WebService
{

    public CIP2AuthInterface()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }




    [WebMethod]
    public string SelectAssertion(string request)
    {
        string result = "";




        return result;
    }
    [WebMethod]
    public string authReq(string request)
    {
        string result = "";




        return result;
    }
}


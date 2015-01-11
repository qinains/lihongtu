using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;


/// <summary>
/// CRMForBTUCenter 的摘要说明



/// </summary>
[WebService(Namespace = "http://CRMInterface.Customer.Besttone.cn")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CRMForBTUCenter : System.Web.Services.WebService
{

    public CRMForBTUCenter()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    public class CRMUserAuthResult
    { 
        public int Result;
        public string ErrorDescription;
        public string ExtendField;    
    }

    [WebMethod(Description = "")]
    public CRMUserAuthResult CRMUserAuth(string SystemsID, string AuthenName, string AuthenType, string CustType, string Password, string AreaCode,
        string ExtendField)
    {
        CRMUserAuthResult result = new CRMUserAuthResult();
        return result;
       
    }

}


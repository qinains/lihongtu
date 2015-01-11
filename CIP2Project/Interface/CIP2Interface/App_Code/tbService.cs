using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Net;

/// <summary>
/// tbService 的摘要说明
/// </summary>
[WebService(Namespace = "http://BestToneUserCenter.vnet.cn")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class tbService : System.Web.Services.WebService
{

    public tbService()
    {
       
     //   IPAddress addr = IP(Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].Address);
    }

    [WebMethod(Description = "AddressList")]
    public string HelloWorld()
    {
        //IPAddress addr = IP(Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].Address);
        //string  l = addr.Address.ToString();
        //string s=addr.AddressFamily.ToString();
        //string s1=addr.ScopeId.ToString();
        //string s2 = addr.IsIPv6LinkLocal.ToString();
        //string s3 = addr.IsIPv6SiteLocal.ToString();
        //return l+"=="+s+"=="+s1+"=="+s2+"=="+s3 ;
        string addr = this.Context.Request.UserHostAddress;
        string host = Context.Request.UserHostName;
        return addr+"=="+host;

    }
    //public static IPAddress IP(long HostName)
    //{
    //    System.Net.IPAddress addr;
    //    //   获得本机局域网IP地址     
    //    addr = new System.Net.IPAddress(HostName);
    //    return addr;
    //}
}


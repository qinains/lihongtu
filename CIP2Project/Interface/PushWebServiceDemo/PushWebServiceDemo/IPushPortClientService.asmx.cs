using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PushWebServiceDemo
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "www.fsti.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None, Name = "PushPortClientSoapBinding", Namespace = "www.fsti.com")]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class IPushPortClientService : System.Web.Services.WebService
    {

        #region 短信上行
        [WebMethod]
        [System.Web.Services.Protocols.SoapRpcMethod(Action = "", RequestNamespace = "www.fsti.com", Binding = "PushPortClientSoapBinding")]
        public void notifyRecivedShortMessages(ServiceReference.ShortMessage[] in0)
        {


          //此处实现内部处理逻辑，短信内容对象均在 in0对象中

        }
        #endregion
    }
}

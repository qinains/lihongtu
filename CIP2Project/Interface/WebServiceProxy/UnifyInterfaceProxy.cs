using System.Diagnostics;
using System.Xml.Serialization;
using System;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;

namespace BTUCenter.Proxy
{
    //------------------------------------------------------------------------------
    // <autogenerated>
    //     This code was generated by a tool.
    //     Runtime Version: 1.1.4322.2407
    //
    //     Changes to this file may cause incorrect behavior and will be lost if 
    //     the code is regenerated.
    // </autogenerated>
    //------------------------------------------------------------------------------

    // 
    // ��Դ������ wsdl, Version=1.1.4322.2407 �Զ����ɡ�
    // 
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "Interface.Customer.Besttone.cn", Namespace = "http://Interface.Customer.Besttone.cn")]
    public class UnifyInterfaceProxy : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        /// <remarks/>
        public UnifyInterfaceProxy()
        {
            this.Url = "http://192.168.18.21:8080/WebService/services/ForLCServer";
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://com.tele.server.forlc", ResponseNamespace = "http://com.tele.server.forlc")]
        [return: System.Xml.Serialization.SoapElementAttribute("insertSendDatasReturn")]
        public string insertSendDatas(string phoneNum, string description, string spId)
        {
            object[] results = this.Invoke("insertSendDatas", new object[] {
                    phoneNum,
                    description,
                    spId});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegininsertSendDatas(string phoneNum, string description, string spId, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("insertSendDatas", new object[] {
                    phoneNum,
                    description,
                    spId}, callback, asyncState);
        }

        /// <remarks/>
        public string EndinsertSendDatas(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://com.tele.server.forlc", ResponseNamespace = "http://com.tele.server.forlc")]
        [return: System.Xml.Serialization.SoapElementAttribute("selectSMSHistoryReturn")]
        public string selectSMSHistory(string phoneNum, string spId)
        {
            object[] results = this.Invoke("selectSMSHistory", new object[] {
                    phoneNum,
                    spId});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginselectSMSHistory(string phoneNum, string spId, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("selectSMSHistory", new object[] {
                    phoneNum,
                    spId}, callback, asyncState);
        }

        /// <remarks/>
        public string EndselectSMSHistory(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
    }

}


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Configuration;

namespace BTUCenter.Proxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "IDispatchControlHttpBinding", Namespace = "http://control.ppcore.haobai.huateng.com")]
    public partial class IDispatchControl : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback dispatchCommandOperationCompleted;

        /// <remarks/>
        public IDispatchControl()
        {
            this.Url = System.Configuration.ConfigurationManager.AppSettings["bestpay_webservice_uri"]; //"http://132.129.11.185:7000/provfront/services/businessService";
        }

        /// <remarks/>
        public event dispatchCommandCompletedEventHandler dispatchCommandCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://control.ppcore.haobai.huateng.com", ResponseNamespace = "http://control.ppcore.haobai.huateng.com", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("out", IsNullable = true)]
        public string dispatchCommand([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string in0, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string in1)
        {
            object[] results = this.Invoke("dispatchCommand", new object[] {
                    in0,
                    in1});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegindispatchCommand(string in0, string in1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("dispatchCommand", new object[] {
                    in0,
                    in1}, callback, asyncState);
        }

        /// <remarks/>
        public string EnddispatchCommand(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void dispatchCommandAsync(string in0, string in1)
        {
            this.dispatchCommandAsync(in0, in1, null);
        }

        /// <remarks/>
        public void dispatchCommandAsync(string in0, string in1, object userState)
        {
            if ((this.dispatchCommandOperationCompleted == null))
            {
                this.dispatchCommandOperationCompleted = new System.Threading.SendOrPostCallback(this.OndispatchCommandOperationCompleted);
            }
            this.InvokeAsync("dispatchCommand", new object[] {
                    in0,
                    in1}, this.dispatchCommandOperationCompleted, userState);
        }

        private void OndispatchCommandOperationCompleted(object arg)
        {
            if ((this.dispatchCommandCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.dispatchCommandCompleted(this, new dispatchCommandCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void dispatchCommandCompletedEventHandler(object sender, dispatchCommandCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class dispatchCommandCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal dispatchCommandCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}
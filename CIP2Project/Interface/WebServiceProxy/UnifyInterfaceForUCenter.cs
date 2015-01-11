    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;

namespace BTUCenter.Proxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DbServerSoapBinding", Namespace = "http://com.tele.server.db")]
    public partial class UnifyInterfaceForUCenter : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback newCardCustomerInfoExportOperationCompleted;

        private System.Threading.SendOrPostCallback enterpriseInfoUplodOperationCompleted;

        private System.Threading.SendOrPostCallback ordersImplOperationCompleted;

        private System.Threading.SendOrPostCallback changeCardOperationCompleted;

        private System.Threading.SendOrPostCallback ordersBindOperationCompleted;

        /// <remarks/>
        public UnifyInterfaceForUCenter()
        {
            this.Url = "http://localhost:8080/WebService/services/DbServer";
        }

        /// <remarks/>
        public event newCardCustomerInfoExportCompletedEventHandler newCardCustomerInfoExportCompleted;

        /// <remarks/>
        public event enterpriseInfoUplodCompletedEventHandler enterpriseInfoUplodCompleted;

        /// <remarks/>
        public event ordersImplCompletedEventHandler ordersImplCompleted;

        /// <remarks/>
        public event changeCardCompletedEventHandler changeCardCompleted;

        /// <remarks/>
        public event ordersBindCompletedEventHandler ordersBindCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://com.tele.server.db", ResponseNamespace = "http://com.tele.server.db")]
        [return: System.Xml.Serialization.SoapElementAttribute("newCardCustomerInfoExportReturn")]
        public string newCardCustomerInfoExport(string spId, string info, string dealType)
        {
            object[] results = this.Invoke("newCardCustomerInfoExport", new object[] {
                    spId,
                    info,
                    dealType});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginnewCardCustomerInfoExport(string spId, string info, string dealType, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("newCardCustomerInfoExport", new object[] {
                    spId,
                    info,
                    dealType}, callback, asyncState);
        }

        /// <remarks/>
        public string EndnewCardCustomerInfoExport(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void newCardCustomerInfoExportAsync(string spId, string info, string dealType)
        {
            this.newCardCustomerInfoExportAsync(spId, info, dealType, null);
        }

        /// <remarks/>
        public void newCardCustomerInfoExportAsync(string spId, string info, string dealType, object userState)
        {
            if ((this.newCardCustomerInfoExportOperationCompleted == null))
            {
                this.newCardCustomerInfoExportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnnewCardCustomerInfoExportOperationCompleted);
            }
            this.InvokeAsync("newCardCustomerInfoExport", new object[] {
                    spId,
                    info,
                    dealType}, this.newCardCustomerInfoExportOperationCompleted, userState);
        }

        private void OnnewCardCustomerInfoExportOperationCompleted(object arg)
        {
            if ((this.newCardCustomerInfoExportCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.newCardCustomerInfoExportCompleted(this, new newCardCustomerInfoExportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://com.tele.server.db", ResponseNamespace = "http://com.tele.server.db")]
        [return: System.Xml.Serialization.SoapElementAttribute("enterpriseInfoUplodReturn")]
        public string enterpriseInfoUplod(string spId, string corporationId, string custId, string userAccount, string corporationName, string corporationType)
        {
            object[] results = this.Invoke("enterpriseInfoUplod", new object[] {
                    spId,
                    corporationId,
                    custId,
                    userAccount,
                    corporationName,
                    corporationType});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginenterpriseInfoUplod(string spId, string corporationId, string custId, string userAccount, string corporationName, string corporationType, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("enterpriseInfoUplod", new object[] {
                    spId,
                    corporationId,
                    custId,
                    userAccount,
                    corporationName,
                    corporationType}, callback, asyncState);
        }

        /// <remarks/>
        public string EndenterpriseInfoUplod(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void enterpriseInfoUplodAsync(string spId, string corporationId, string custId, string userAccount, string corporationName, string corporationType)
        {
            this.enterpriseInfoUplodAsync(spId, corporationId, custId, userAccount, corporationName, corporationType, null);
        }

        /// <remarks/>
        public void enterpriseInfoUplodAsync(string spId, string corporationId, string custId, string userAccount, string corporationName, string corporationType, object userState)
        {
            if ((this.enterpriseInfoUplodOperationCompleted == null))
            {
                this.enterpriseInfoUplodOperationCompleted = new System.Threading.SendOrPostCallback(this.OnenterpriseInfoUplodOperationCompleted);
            }
            this.InvokeAsync("enterpriseInfoUplod", new object[] {
                    spId,
                    corporationId,
                    custId,
                    userAccount,
                    corporationName,
                    corporationType}, this.enterpriseInfoUplodOperationCompleted, userState);
        }

        private void OnenterpriseInfoUplodOperationCompleted(object arg)
        {
            if ((this.enterpriseInfoUplodCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.enterpriseInfoUplodCompleted(this, new enterpriseInfoUplodCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://com.tele.server.db", ResponseNamespace = "http://com.tele.server.db")]
        [return: System.Xml.Serialization.SoapElementAttribute("ordersImplReturn")]
        public string ordersImpl(string xmlString, string spId)
        {
            object[] results = this.Invoke("ordersImpl", new object[] {
                    xmlString,
                    spId});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginordersImpl(string xmlString, string spId, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ordersImpl", new object[] {
                    xmlString,
                    spId}, callback, asyncState);
        }

        /// <remarks/>
        public string EndordersImpl(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ordersImplAsync(string xmlString, string spId)
        {
            this.ordersImplAsync(xmlString, spId, null);
        }

        /// <remarks/>
        public void ordersImplAsync(string xmlString, string spId, object userState)
        {
            if ((this.ordersImplOperationCompleted == null))
            {
                this.ordersImplOperationCompleted = new System.Threading.SendOrPostCallback(this.OnordersImplOperationCompleted);
            }
            this.InvokeAsync("ordersImpl", new object[] {
                    xmlString,
                    spId}, this.ordersImplOperationCompleted, userState);
        }

        private void OnordersImplOperationCompleted(object arg)
        {
            if ((this.ordersImplCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ordersImplCompleted(this, new ordersImplCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://com.tele.server.db", ResponseNamespace = "http://com.tele.server.db")]
        [return: System.Xml.Serialization.SoapElementAttribute("changeCardReturn")]
        public string changeCard(string custId, string spId, string workNo)
        {
            object[] results = this.Invoke("changeCard", new object[] {
                    custId,
                    spId,
                    workNo});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginchangeCard(string custId, string spId, string workNo, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("changeCard", new object[] {
                    custId,
                    spId,
                    workNo}, callback, asyncState);
        }

        /// <remarks/>
        public string EndchangeCard(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void changeCardAsync(string custId, string spId, string workNo)
        {
            this.changeCardAsync(custId, spId, workNo, null);
        }

        /// <remarks/>
        public void changeCardAsync(string custId, string spId, string workNo, object userState)
        {
            if ((this.changeCardOperationCompleted == null))
            {
                this.changeCardOperationCompleted = new System.Threading.SendOrPostCallback(this.OnchangeCardOperationCompleted);
            }
            this.InvokeAsync("changeCard", new object[] {
                    custId,
                    spId,
                    workNo}, this.changeCardOperationCompleted, userState);
        }

        private void OnchangeCardOperationCompleted(object arg)
        {
            if ((this.changeCardCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.changeCardCompleted(this, new changeCardCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://com.tele.server.db", ResponseNamespace = "http://com.tele.server.db")]
        [return: System.Xml.Serialization.SoapElementAttribute("ordersBindReturn")]
        public string ordersBind(string spId, string custId, string userAccount, string orderSeq, string workNo)
        {
            object[] results = this.Invoke("ordersBind", new object[] {
                    spId,
                    custId,
                    userAccount,
                    orderSeq,
                    workNo});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginordersBind(string spId, string custId, string userAccount, string orderSeq, string workNo, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ordersBind", new object[] {
                    spId,
                    custId,
                    userAccount,
                    orderSeq,
                    workNo}, callback, asyncState);
        }

        /// <remarks/>
        public string EndordersBind(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void ordersBindAsync(string spId, string custId, string userAccount, string orderSeq, string workNo)
        {
            this.ordersBindAsync(spId, custId, userAccount, orderSeq, workNo, null);
        }

        /// <remarks/>
        public void ordersBindAsync(string spId, string custId, string userAccount, string orderSeq, string workNo, object userState)
        {
            if ((this.ordersBindOperationCompleted == null))
            {
                this.ordersBindOperationCompleted = new System.Threading.SendOrPostCallback(this.OnordersBindOperationCompleted);
            }
            this.InvokeAsync("ordersBind", new object[] {
                    spId,
                    custId,
                    userAccount,
                    orderSeq,
                    workNo}, this.ordersBindOperationCompleted, userState);
        }

        private void OnordersBindOperationCompleted(object arg)
        {
            if ((this.ordersBindCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ordersBindCompleted(this, new ordersBindCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void newCardCustomerInfoExportCompletedEventHandler(object sender, newCardCustomerInfoExportCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class newCardCustomerInfoExportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal newCardCustomerInfoExportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void enterpriseInfoUplodCompletedEventHandler(object sender, enterpriseInfoUplodCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class enterpriseInfoUplodCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal enterpriseInfoUplodCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ordersImplCompletedEventHandler(object sender, ordersImplCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ordersImplCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ordersImplCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void changeCardCompletedEventHandler(object sender, changeCardCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class changeCardCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal changeCardCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ordersBindCompletedEventHandler(object sender, ordersBindCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ordersBindCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ordersBindCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
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

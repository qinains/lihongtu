﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5477
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// 此源代码由 wsdl 自动生成, Version=2.0.50727.42。
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="IPushPortClientServiceSoap", Namespace="www.fsti.com")]
[System.Xml.Serialization.SoapIncludeAttribute(typeof(ShortMessage))]
public partial class IPushPortClientService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
    
    private System.Threading.SendOrPostCallback notifyRecivedShortMessagesOperationCompleted;
    
    /// <remarks/>
    public IPushPortClientService() {
        this.Url = "http://116.228.55.13:8081/CIP2Interface/IPushPortClientService.asmx";
        //this.Url = "http://interface.customer.besttone.cn/BestTone2UCenterInterface/CIP2Interface/IPushPortClientService.asmx";
    }
    
    /// <remarks/>
    public event HelloWorldCompletedEventHandler HelloWorldCompleted;
    
    /// <remarks/>
    public event notifyRecivedShortMessagesCompletedEventHandler notifyRecivedShortMessagesCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("www.fsti.com/HelloWorld", RequestNamespace="www.fsti.com", ResponseNamespace="www.fsti.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string HelloWorld() {
        object[] results = this.Invoke("HelloWorld", new object[0]);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginHelloWorld(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("HelloWorld", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public string EndHelloWorld(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void HelloWorldAsync() {
        this.HelloWorldAsync(null);
    }
    
    /// <remarks/>
    public void HelloWorldAsync(object userState) {
        if ((this.HelloWorldOperationCompleted == null)) {
            this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
        }
        this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
    }
    
    private void OnHelloWorldOperationCompleted(object arg) {
        if ((this.HelloWorldCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="www.fsti.com", ResponseNamespace="www.fsti.com")]
    public void notifyRecivedShortMessages(ShortMessage[] in0) {
        this.Invoke("notifyRecivedShortMessages", new object[] {
                    in0});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginnotifyRecivedShortMessages(ShortMessage[] in0, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("notifyRecivedShortMessages", new object[] {
                    in0}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndnotifyRecivedShortMessages(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void notifyRecivedShortMessagesAsync(ShortMessage[] in0) {
        this.notifyRecivedShortMessagesAsync(in0, null);
    }
    
    /// <remarks/>
    public void notifyRecivedShortMessagesAsync(ShortMessage[] in0, object userState) {
        if ((this.notifyRecivedShortMessagesOperationCompleted == null)) {
            this.notifyRecivedShortMessagesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnnotifyRecivedShortMessagesOperationCompleted);
        }
        this.InvokeAsync("notifyRecivedShortMessages", new object[] {
                    in0}, this.notifyRecivedShortMessagesOperationCompleted, userState);
    }
    
    private void OnnotifyRecivedShortMessagesOperationCompleted(object arg) {
        if ((this.notifyRecivedShortMessagesCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.notifyRecivedShortMessagesCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.SoapTypeAttribute(Namespace="www.fsti.com")]
public partial class ShortMessage {
    
    private string deliverTimeField;
    
    private string destPhoneNumberField;
    
    private string linkidField;
    
    private string msgContentField;
    
    private string reserveField;
    
    private string srcPhoneNumberField;
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string deliverTime {
        get {
            return this.deliverTimeField;
        }
        set {
            this.deliverTimeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string destPhoneNumber {
        get {
            return this.destPhoneNumberField;
        }
        set {
            this.destPhoneNumberField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string linkid {
        get {
            return this.linkidField;
        }
        set {
            this.linkidField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string msgContent {
        get {
            return this.msgContentField;
        }
        set {
            this.msgContentField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string reserve {
        get {
            return this.reserveField;
        }
        set {
            this.reserveField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string srcPhoneNumber {
        get {
            return this.srcPhoneNumberField;
        }
        set {
            this.srcPhoneNumberField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public string Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((string)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
public delegate void notifyRecivedShortMessagesCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
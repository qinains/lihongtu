﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// 此源代码由 wsdl, Version=1.1.4322.573 自动生成。

// 
using System.Diagnostics;
using System.Xml.Serialization;
using System;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;
namespace BTUCenter.Proxy.GS
{
   
/// <remarks/>
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="UIPHttpBinding", Namespace="http://Interface.Customer.Besttone.cn")]
public class UIP : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    /// <remarks/>
    public UIP() {
        this.Url = "http://10.74.17.13/GSUIP";
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://Interface.Customer.Besttone.cn", ResponseNamespace="http://Interface.Customer.Besttone.cn", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    [return: System.Xml.Serialization.XmlElementAttribute("out")]
    public CRMUserAuthResponse CRMUserAuth(CRMUserAuthRequest in0) {
        object[] results = this.Invoke("CRMUserAuth", new object[] {
                    in0});
        return ((CRMUserAuthResponse)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginCRMUserAuth(CRMUserAuthRequest in0, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("CRMUserAuth", new object[] {
                    in0}, callback, asyncState);
    }
    
    /// <remarks/>
    public CRMUserAuthResponse EndCRMUserAuth(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((CRMUserAuthResponse)(results[0]));
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Interface.Customer.Besttone.cn")]
public class CRMUserAuthRequest {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string areaCode;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string authenName;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string authenType;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string custType;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string extendField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string password;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string systemsID;
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Interface.Customer.Besttone.cn")]
public class CRMUserAuthResponse {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string errorDescription;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public string extendField;
    
    /// <remarks/>
    public int result;
}
}
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
/// <summary>
/// IUaServerimplServiceSoapBinding 的摘要说明
/// </summary>
/// 

[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
[System.Web.Services.WebServiceBindingAttribute(Name = "UaServerimplServiceSoapBinding", Namespace = "http://www.mbossuac.com.cn/ua")]
public interface IUaServerimplServiceSoapBinding
{
   
        //
        // TODO: 在此处添加构造函数逻辑
        //

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://www.mbossuac.com.cn/ua", ResponseNamespace="http://www.mbossuac.com.cn/ua", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("response", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        string AccountInfoQuery([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string request);

    
}

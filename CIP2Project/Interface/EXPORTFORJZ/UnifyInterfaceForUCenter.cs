using System.Diagnostics;
using System.Xml.Serialization;
using System;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;
using System.Xml;

namespace Proxy.UnifyInterfaceForUCenter
{

	/// <remarks/>
	/// <remarks/>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name="DbServerSoapBinding", Namespace="http://com.tele.server.db")]
	public class UnifyInterfaceForUCenter : System.Web.Services.Protocols.SoapHttpClientProtocol 
	{
    
		/// <remarks/>
		public UnifyInterfaceForUCenter()
		{
			this.Url = "http://192.168.18.20:8080/WebService/services/DbServer";
		}
    
		/// <remarks/>
		[System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://com.tele.server.db", ResponseNamespace="http://com.tele.server.db")]
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
		[System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://com.tele.server.db", ResponseNamespace="http://com.tele.server.db")]
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
		[System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://com.tele.server.db", ResponseNamespace="http://com.tele.server.db")]
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
		[System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://com.tele.server.db", ResponseNamespace="http://com.tele.server.db")]
		[return: System.Xml.Serialization.SoapElementAttribute("changeCardReturn")]
		public string changeCard(string custId, string spId) 
		{
			object[] results = this.Invoke("changeCard", new object[] {
																		  custId,
																		  spId});
			return ((string)(results[0]));
		}
    
		/// <remarks/>
		public System.IAsyncResult BeginchangeCard(string custId, string spId, System.AsyncCallback callback, object asyncState) 
		{
			return this.BeginInvoke("changeCard", new object[] {
																   custId,
																   spId}, callback, asyncState);
		}
    
		/// <remarks/>
		public string EndchangeCard(System.IAsyncResult asyncResult) 
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}
	}


}


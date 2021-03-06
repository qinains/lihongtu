﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5456
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
namespace BTUCenter.Proxy
{
    // 
    // 此源代码由 wsdl 自动生成, Version=2.0.50727.1432。
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "UDBAppSysSoap", Namespace = "http://udb.chinatelecom.com")]
    public partial class UDBAppSys : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback AccountBindingOperationCompleted;

        private System.Threading.SendOrPostCallback AccountSsStatusRefreshOperationCompleted;

        private System.Threading.SendOrPostCallback AccountInfoCheckOperationCompleted;

        private System.Threading.SendOrPostCallback PuserIDMappingQueryOperationCompleted;

        /// <remarks/>
        public UDBAppSys()
        {
            this.Url = "http://zx.passport.189.cn/UDBAPPInterface/UDBAPPSYS/UDBAppSys.asmx";
        }

        /// <remarks/>
        public event AccountBindingCompletedEventHandler AccountBindingCompleted;

        /// <remarks/>
        public event AccountSsStatusRefreshCompletedEventHandler AccountSsStatusRefreshCompleted;

        /// <remarks/>
        public event AccountInfoCheckCompletedEventHandler AccountInfoCheckCompleted;

        /// <remarks/>
        public event PuserIDMappingQueryCompletedEventHandler PuserIDMappingQueryCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://udb.chinatelecom.com/AccountBinding", RequestElementName = "AccountBindingRequest", RequestNamespace = "http://udb.chinatelecom.com", ResponseNamespace = "http://udb.chinatelecom.com", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public AccountBindingResult AccountBinding(string Authenticator, string SrcSsDeviceNo, string BindSsDeviceNo, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string UserID, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string PUserID, string ThirdSsUserID, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string SsPWEncryType, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string SsPassword, string BindType, string TimeStamp, string BindEffectMode, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string EffectTimeStamp)
        {
            object[] results = this.Invoke("AccountBinding", new object[] {
                    Authenticator,
                    SrcSsDeviceNo,
                    BindSsDeviceNo,
                    UserID,
                    PUserID,
                    ThirdSsUserID,
                    SsPWEncryType,
                    SsPassword,
                    BindType,
                    TimeStamp,
                    BindEffectMode,
                    EffectTimeStamp});
            return ((AccountBindingResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAccountBinding(string Authenticator, string SrcSsDeviceNo, string BindSsDeviceNo, string UserID, string PUserID, string ThirdSsUserID, string SsPWEncryType, string SsPassword, string BindType, string TimeStamp, string BindEffectMode, string EffectTimeStamp, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AccountBinding", new object[] {
                    Authenticator,
                    SrcSsDeviceNo,
                    BindSsDeviceNo,
                    UserID,
                    PUserID,
                    ThirdSsUserID,
                    SsPWEncryType,
                    SsPassword,
                    BindType,
                    TimeStamp,
                    BindEffectMode,
                    EffectTimeStamp}, callback, asyncState);
        }

        /// <remarks/>
        public AccountBindingResult EndAccountBinding(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AccountBindingResult)(results[0]));
        }

        /// <remarks/>
        public void AccountBindingAsync(string Authenticator, string SrcSsDeviceNo, string BindSsDeviceNo, string UserID, string PUserID, string ThirdSsUserID, string SsPWEncryType, string SsPassword, string BindType, string TimeStamp, string BindEffectMode, string EffectTimeStamp)
        {
            this.AccountBindingAsync(Authenticator, SrcSsDeviceNo, BindSsDeviceNo, UserID, PUserID, ThirdSsUserID, SsPWEncryType, SsPassword, BindType, TimeStamp, BindEffectMode, EffectTimeStamp, null);
        }

        /// <remarks/>
        public void AccountBindingAsync(string Authenticator, string SrcSsDeviceNo, string BindSsDeviceNo, string UserID, string PUserID, string ThirdSsUserID, string SsPWEncryType, string SsPassword, string BindType, string TimeStamp, string BindEffectMode, string EffectTimeStamp, object userState)
        {
            if ((this.AccountBindingOperationCompleted == null))
            {
                this.AccountBindingOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAccountBindingOperationCompleted);
            }
            this.InvokeAsync("AccountBinding", new object[] {
                    Authenticator,
                    SrcSsDeviceNo,
                    BindSsDeviceNo,
                    UserID,
                    PUserID,
                    ThirdSsUserID,
                    SsPWEncryType,
                    SsPassword,
                    BindType,
                    TimeStamp,
                    BindEffectMode,
                    EffectTimeStamp}, this.AccountBindingOperationCompleted, userState);
        }

        private void OnAccountBindingOperationCompleted(object arg)
        {
            if ((this.AccountBindingCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AccountBindingCompleted(this, new AccountBindingCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://udb.chinatelecom.com/AccountSsStatusRefresh", RequestElementName = "AccountSsStatusRefreshRequest", RequestNamespace = "http://udb.chinatelecom.com", ResponseNamespace = "http://udb.chinatelecom.com", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public AccountSsStatusRefreshResult AccountSsStatusRefresh(string SrcSsDeviceNo, string UpdateSsDeviceNo, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string UserID, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string PUserID, string UserIDSsStatus, string TimeStamp, string RefreshEffectMode, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string EffectTimeStamp)
        {
            object[] results = this.Invoke("AccountSsStatusRefresh", new object[] {
                    SrcSsDeviceNo,
                    UpdateSsDeviceNo,
                    UserID,
                    PUserID,
                    UserIDSsStatus,
                    TimeStamp,
                    RefreshEffectMode,
                    EffectTimeStamp});
            return ((AccountSsStatusRefreshResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAccountSsStatusRefresh(string SrcSsDeviceNo, string UpdateSsDeviceNo, string UserID, string PUserID, string UserIDSsStatus, string TimeStamp, string RefreshEffectMode, string EffectTimeStamp, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AccountSsStatusRefresh", new object[] {
                    SrcSsDeviceNo,
                    UpdateSsDeviceNo,
                    UserID,
                    PUserID,
                    UserIDSsStatus,
                    TimeStamp,
                    RefreshEffectMode,
                    EffectTimeStamp}, callback, asyncState);
        }

        /// <remarks/>
        public AccountSsStatusRefreshResult EndAccountSsStatusRefresh(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AccountSsStatusRefreshResult)(results[0]));
        }

        /// <remarks/>
        public void AccountSsStatusRefreshAsync(string SrcSsDeviceNo, string UpdateSsDeviceNo, string UserID, string PUserID, string UserIDSsStatus, string TimeStamp, string RefreshEffectMode, string EffectTimeStamp)
        {
            this.AccountSsStatusRefreshAsync(SrcSsDeviceNo, UpdateSsDeviceNo, UserID, PUserID, UserIDSsStatus, TimeStamp, RefreshEffectMode, EffectTimeStamp, null);
        }

        /// <remarks/>
        public void AccountSsStatusRefreshAsync(string SrcSsDeviceNo, string UpdateSsDeviceNo, string UserID, string PUserID, string UserIDSsStatus, string TimeStamp, string RefreshEffectMode, string EffectTimeStamp, object userState)
        {
            if ((this.AccountSsStatusRefreshOperationCompleted == null))
            {
                this.AccountSsStatusRefreshOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAccountSsStatusRefreshOperationCompleted);
            }
            this.InvokeAsync("AccountSsStatusRefresh", new object[] {
                    SrcSsDeviceNo,
                    UpdateSsDeviceNo,
                    UserID,
                    PUserID,
                    UserIDSsStatus,
                    TimeStamp,
                    RefreshEffectMode,
                    EffectTimeStamp}, this.AccountSsStatusRefreshOperationCompleted, userState);
        }

        private void OnAccountSsStatusRefreshOperationCompleted(object arg)
        {
            if ((this.AccountSsStatusRefreshCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AccountSsStatusRefreshCompleted(this, new AccountSsStatusRefreshCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://udb.chinatelecom.com/AccountInfoCheck", RequestElementName = "AccountInfoCheckRequest", RequestNamespace = "http://udb.chinatelecom.com", ResponseNamespace = "http://udb.chinatelecom.com", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public AccountInfoCheckResult AccountInfoCheck(string Authenticator, string SrcSsDeviceNo, string AuthSsDeviceNo, string UDBTicket, string TimeStamp)
        {
            object[] results = this.Invoke("AccountInfoCheck", new object[] {
                    Authenticator,
                    SrcSsDeviceNo,
                    AuthSsDeviceNo,
                    UDBTicket,
                    TimeStamp});
            return ((AccountInfoCheckResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAccountInfoCheck(string Authenticator, string SrcSsDeviceNo, string AuthSsDeviceNo, string UDBTicket, string TimeStamp, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AccountInfoCheck", new object[] {
                    Authenticator,
                    SrcSsDeviceNo,
                    AuthSsDeviceNo,
                    UDBTicket,
                    TimeStamp}, callback, asyncState);
        }

        /// <remarks/>
        public AccountInfoCheckResult EndAccountInfoCheck(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AccountInfoCheckResult)(results[0]));
        }

        /// <remarks/>
        public void AccountInfoCheckAsync(string Authenticator, string SrcSsDeviceNo, string AuthSsDeviceNo, string UDBTicket, string TimeStamp)
        {
            this.AccountInfoCheckAsync(Authenticator, SrcSsDeviceNo, AuthSsDeviceNo, UDBTicket, TimeStamp, null);
        }

        /// <remarks/>
        public void AccountInfoCheckAsync(string Authenticator, string SrcSsDeviceNo, string AuthSsDeviceNo, string UDBTicket, string TimeStamp, object userState)
        {
            if ((this.AccountInfoCheckOperationCompleted == null))
            {
                this.AccountInfoCheckOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAccountInfoCheckOperationCompleted);
            }
            this.InvokeAsync("AccountInfoCheck", new object[] {
                    Authenticator,
                    SrcSsDeviceNo,
                    AuthSsDeviceNo,
                    UDBTicket,
                    TimeStamp}, this.AccountInfoCheckOperationCompleted, userState);
        }

        private void OnAccountInfoCheckOperationCompleted(object arg)
        {
            if ((this.AccountInfoCheckCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AccountInfoCheckCompleted(this, new AccountInfoCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://udb.chinatelecom.com/PuserIDMappingQuery", RequestElementName = "PuserIDMappingQueryRequest", RequestNamespace = "http://udb.chinatelecom.com", ResponseNamespace = "http://udb.chinatelecom.com", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public PuserIDMappingQueryResult PuserIDMappingQuery(string Authenticator, string SrcDeviceNo, string PUserID, string QueryType, string TimeStamp)
        {
            object[] results = this.Invoke("PuserIDMappingQuery", new object[] {
                    Authenticator,
                    SrcDeviceNo,
                    PUserID,
                    QueryType,
                    TimeStamp});
            return ((PuserIDMappingQueryResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginPuserIDMappingQuery(string Authenticator, string SrcDeviceNo, string PUserID, string QueryType, string TimeStamp, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("PuserIDMappingQuery", new object[] {
                    Authenticator,
                    SrcDeviceNo,
                    PUserID,
                    QueryType,
                    TimeStamp}, callback, asyncState);
        }

        /// <remarks/>
        public PuserIDMappingQueryResult EndPuserIDMappingQuery(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((PuserIDMappingQueryResult)(results[0]));
        }

        /// <remarks/>
        public void PuserIDMappingQueryAsync(string Authenticator, string SrcDeviceNo, string PUserID, string QueryType, string TimeStamp)
        {
            this.PuserIDMappingQueryAsync(Authenticator, SrcDeviceNo, PUserID, QueryType, TimeStamp, null);
        }

        /// <remarks/>
        public void PuserIDMappingQueryAsync(string Authenticator, string SrcDeviceNo, string PUserID, string QueryType, string TimeStamp, object userState)
        {
            if ((this.PuserIDMappingQueryOperationCompleted == null))
            {
                this.PuserIDMappingQueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPuserIDMappingQueryOperationCompleted);
            }
            this.InvokeAsync("PuserIDMappingQuery", new object[] {
                    Authenticator,
                    SrcDeviceNo,
                    PUserID,
                    QueryType,
                    TimeStamp}, this.PuserIDMappingQueryOperationCompleted, userState);
        }

        private void OnPuserIDMappingQueryOperationCompleted(object arg)
        {
            if ((this.PuserIDMappingQueryCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PuserIDMappingQueryCompleted(this, new PuserIDMappingQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://udb.chinatelecom.com")]
    public partial class AccountBindingResult
    {

        private int resultCodeField;

        private string userIDField;

        private string pUserIDField;

        private string thirdSsUserIDField;

        private string descriptionField;

        /// <remarks/>
        public int ResultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string PUserID
        {
            get
            {
                return this.pUserIDField;
            }
            set
            {
                this.pUserIDField = value;
            }
        }

        /// <remarks/>
        public string ThirdSsUserID
        {
            get
            {
                return this.thirdSsUserIDField;
            }
            set
            {
                this.thirdSsUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://udb.chinatelecom.com")]
    public partial class PuserIDMappingQueryResult
    {

        private int resultCodeField;

        private string provinceIDField;

        private string pUserIDField;

        private string oriPUserIDField;

        private string errorDescriptionField;

        /// <remarks/>
        public int ResultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ProvinceID
        {
            get
            {
                return this.provinceIDField;
            }
            set
            {
                this.provinceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string PUserID
        {
            get
            {
                return this.pUserIDField;
            }
            set
            {
                this.pUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OriPUserID
        {
            get
            {
                return this.oriPUserIDField;
            }
            set
            {
                this.oriPUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ErrorDescription
        {
            get
            {
                return this.errorDescriptionField;
            }
            set
            {
                this.errorDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://udb.chinatelecom.com")]
    public partial class ReturnUserGroup
    {

        private string userIDField;

        private string userIDTypeField;

        /// <remarks/>
        public string UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        public string UserIDType
        {
            get
            {
                return this.userIDTypeField;
            }
            set
            {
                this.userIDTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://udb.chinatelecom.com")]
    public partial class AccountInfoCheckResult
    {

        private int resultCodeField;

        private int userTypeField;

        private ReturnUserGroup[] returnUserGroupListField;

        private string userIDField;

        private string userIDTypeField;

        private string pUserIDField;

        private string aliasField;

        private string bindingAccessNoField;

        private string thirdSsUserIDField;

        private string userIDStatusField;

        private string userIDSsStatusField;

        private string userPayTypeField;

        private string prePaySystemNoField;

        private string descriptionField;

        /// <remarks/>
        public int ResultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        public int UserType
        {
            get
            {
                return this.userTypeField;
            }
            set
            {
                this.userTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        public ReturnUserGroup[] ReturnUserGroupList
        {
            get
            {
                return this.returnUserGroupListField;
            }
            set
            {
                this.returnUserGroupListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string UserIDType
        {
            get
            {
                return this.userIDTypeField;
            }
            set
            {
                this.userIDTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string PUserID
        {
            get
            {
                return this.pUserIDField;
            }
            set
            {
                this.pUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Alias
        {
            get
            {
                return this.aliasField;
            }
            set
            {
                this.aliasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string BindingAccessNo
        {
            get
            {
                return this.bindingAccessNoField;
            }
            set
            {
                this.bindingAccessNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ThirdSsUserID
        {
            get
            {
                return this.thirdSsUserIDField;
            }
            set
            {
                this.thirdSsUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string UserIDStatus
        {
            get
            {
                return this.userIDStatusField;
            }
            set
            {
                this.userIDStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string UserIDSsStatus
        {
            get
            {
                return this.userIDSsStatusField;
            }
            set
            {
                this.userIDSsStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string UserPayType
        {
            get
            {
                return this.userPayTypeField;
            }
            set
            {
                this.userPayTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string PrePaySystemNo
        {
            get
            {
                return this.prePaySystemNoField;
            }
            set
            {
                this.prePaySystemNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://udb.chinatelecom.com")]
    public partial class AccountSsStatusRefreshResult
    {

        private int resultCodeField;

        private string userIDField;

        private string pUserIDField;

        private string descriptionField;

        /// <remarks/>
        public int ResultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string PUserID
        {
            get
            {
                return this.pUserIDField;
            }
            set
            {
                this.pUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void AccountBindingCompletedEventHandler(object sender, AccountBindingCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AccountBindingCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AccountBindingCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AccountBindingResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AccountBindingResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void AccountSsStatusRefreshCompletedEventHandler(object sender, AccountSsStatusRefreshCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AccountSsStatusRefreshCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AccountSsStatusRefreshCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AccountSsStatusRefreshResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AccountSsStatusRefreshResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void AccountInfoCheckCompletedEventHandler(object sender, AccountInfoCheckCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AccountInfoCheckCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AccountInfoCheckCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AccountInfoCheckResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AccountInfoCheckResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void PuserIDMappingQueryCompletedEventHandler(object sender, PuserIDMappingQueryCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PuserIDMappingQueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal PuserIDMappingQueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public PuserIDMappingQueryResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((PuserIDMappingQueryResult)(this.results[0]));
            }
        }
    }
}
<%@ page language="C#" autoeventwireup="true" inherits="CustInfo, App_Web_kpte7rfs" enableEventValidation="false" %>
function CustInfo(isLogin,welcomeName,encryptCustIDValue){ 
    this.IsLogin=isLogin;
    this.WelcomeName=welcomeName;
    this.EncryptCustIDValue=encryptCustIDValue;
}
function getCustInfo(){
    var custInfo=new CustInfo("<%=IsLogin%>","<%=WelcomeName%>","<%=EncryptCustIDValue%>");
    return custInfo;
}
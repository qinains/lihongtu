<%@ page language="C#" autoeventwireup="true" inherits="CustInfo, App_Web_custinfo.aspx.cdcab7d2" enableEventValidation="false" %>
function CustInfo(isLogin,welcomeName,outerID,encryptCustIDValue,phone){ 
    this.IsLogin=isLogin;
    this.WelcomeName=welcomeName;
    this.outerID =  outerID;
    this.EncryptCustIDValue=encryptCustIDValue;
    this.Phone = phone; 
}
function getCustInfo(){
    var custInfo=new CustInfo("<%=IsLogin%>","<%=WelcomeName%>",  "<%=outerID %>","<%=EncryptCustIDValue%>","<%=Phone%>");
    return custInfo;
}
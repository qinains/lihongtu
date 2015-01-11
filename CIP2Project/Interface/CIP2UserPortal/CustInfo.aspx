<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustInfo.aspx.cs" Inherits="CustInfo" %>
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
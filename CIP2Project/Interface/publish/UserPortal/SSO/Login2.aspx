﻿<%@ page language="C#" autoeventwireup="true" inherits="SSO_Login2, App_Web_login2.aspx.27254924" enableEventValidation="false" %>

<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
 <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>

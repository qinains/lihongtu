﻿<%@ page language="C#" autoeventwireup="true" inherits="SSO_SSOTransitCenter, App_Web_ssotransitcenter.aspx.27254924" enableEventValidation="false" %>

<%@ Register Src="../UserCtrl/ValidateCIPToken.ascx" TagName="ValidateCIPToken" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:ValidateCIPToken ID="TokenValidate" runat="server" />
    </div>
    </form>
</body>
</html>

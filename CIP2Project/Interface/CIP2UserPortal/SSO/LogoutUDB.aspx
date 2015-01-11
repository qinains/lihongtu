<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogoutUDB.aspx.cs" Inherits="SSO_LogoutUDB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript">
        window.setTimeout("RedirectFun()",1000);
        function RedirectFun(){
            window.location.href=document.getElementById("hdReturnUrl").value;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
    </div>
    </form>
</body>
</html>

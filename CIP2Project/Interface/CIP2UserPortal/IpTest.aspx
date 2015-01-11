<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IpTest.aspx.cs" Inherits="IpTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
<tr>
<td><asp:Label ID="Label7" runat="server" Text="你访问认证平台IP是："></asp:Label></td>
<td style="width: 400px"><asp:TextBox ID="txtIP" runat="server" Width="400px"></asp:TextBox></td>
</tr>
<tr>
<td><asp:Label ID="Label1" runat="server" Text="你访问认证平台URL是："></asp:Label></td>
<td style="width: 400px"><asp:TextBox ID="txtUrl" runat="server" Width="400px"></asp:TextBox></td>
</tr>
</table>
    </div>
    </form>
</body>
</html>

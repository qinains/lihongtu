<%@ page language="C#" autoeventwireup="true" inherits="GenerateBtCer, App_Web_generatebtcer.aspx.cdcab7d2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;
        <asp:Label ID="label9" runat="server" Style="z-index: 100; left: 191px; position: absolute;
            top: 84px" Text="SPID"></asp:Label>
        <asp:TextBox ID="txtSPID" runat="server" Style="z-index: 101; left: 263px; position: absolute;
            top: 84px">35111111</asp:TextBox>
        <asp:Label ID="Label2" runat="server" Style="z-index: 102; left: 192px; position: absolute;
            top: 130px" Text="FilePath"></asp:Label>
        <asp:TextBox ID="txtFilePath" runat="server" Style="z-index: 103; left: 262px; position: absolute;
            top: 130px" Width="580px">D:\我的工作\号百\号百三期\MBOSS单点登录\ua联调\cert\jfsc.p12</asp:TextBox>
        <asp:Label ID="Label3" runat="server" Style="z-index: 104; left: 190px; position: absolute;
            top: 214px" Text="Password"></asp:Label>
        <asp:Label ID="Label1" runat="server" Style="z-index: 105; left: 190px; position: absolute;
            top: 175px" Text="UserName"></asp:Label>
        <asp:TextBox ID="txtPassword" runat="server" Style="z-index: 106; left: 262px; position: absolute;
            top: 214px">12345678</asp:TextBox>
        <asp:TextBox ID="txtUserName" runat="server" Style="z-index: 107; left: 262px; position: absolute;
            top: 173px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Style="z-index: 108;
            left: 404px; position: absolute; top: 351px" Text="Button" />
        <asp:Label ID="Label5" runat="server" Style="z-index: 109; left: 190px; position: absolute;
            top: 256px" Text="CerType"></asp:Label>
        <asp:DropDownList ID="ddlCerType" runat="server" Style="z-index: 111; left: 328px;
            position: absolute; top: 256px" Width="75px">
            <asp:ListItem Value="0">公钥证书</asp:ListItem>
            <asp:ListItem Value="0">私钥证书</asp:ListItem>
        </asp:DropDownList>
    
    </div>
    </form>
</body>
</html>

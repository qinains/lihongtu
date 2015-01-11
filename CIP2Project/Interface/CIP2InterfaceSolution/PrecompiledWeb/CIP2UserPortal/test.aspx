<%@ page language="C#" autoeventwireup="true" inherits="test, App_Web_kpte7rfs" enableEventValidation="false" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type="text/C#" runat="server">
        public void Page_Load(object sender, System.EventArgs e) 
        {
            Response.Write(Request.ServerVariables.Get("Remote_Addr").ToString());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Style="z-index: 100;
            left: 230px; position: absolute; top: 275px" Text="生成" />
        <asp:Button ID="Button2" runat="server" OnClick="Button1_Click" Style="z-index: 101;
            left: 358px; position: absolute; top: 275px" Text="解析" />
        <asp:Label ID="Label2" runat="server" Style="z-index: 104; left: 178px; position: absolute;
            top: 140px" Text="NickName"></asp:Label>
        <asp:Label ID="Label3" runat="server" Style="z-index: 104; left: 177px; position: absolute;
            top: 94px" Text="RealName"></asp:Label>
        <asp:TextBox ID="txtNickName" runat="server" Style="z-index: 103; left: 272px; position: absolute;
            top: 139px"></asp:TextBox>
        <asp:TextBox ID="txtRealName" runat="server" Style="z-index: 103; left: 271px; position: absolute;
            top: 93px"></asp:TextBox>
        <asp:Label ID="Label1" runat="server" Style="z-index: 104; left: 176px; position: absolute;
            top: 51px">CustID</asp:Label>
        <asp:TextBox ID="txtCustID" runat="server" Style="z-index: 103; left: 270px; position: absolute;
            top: 50px"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Style="z-index: 104; left: 179px; position: absolute;
            top: 183px" Text="UserName"></asp:Label>
        <asp:TextBox ID="txtUserName" runat="server" Style="z-index: 103; left: 273px; position: absolute;
            top: 182px"></asp:TextBox>
        <asp:TextBox ID="txtNickName2" runat="server" Style="z-index: 103; left: 494px; position: absolute;
            top: 139px"></asp:TextBox>
        <asp:TextBox ID="txtRealName2" runat="server" Style="z-index: 103; left: 493px; position: absolute;
            top: 93px"></asp:TextBox>
        <asp:TextBox ID="txtCustID2" runat="server" Style="z-index: 103; left: 492px; position: absolute;
            top: 50px"></asp:TextBox>
        <asp:TextBox ID="txtUserName2" runat="server" Style="z-index: 103; left: 495px; position: absolute;
            top: 182px"></asp:TextBox>
    
    </div>
    </form>
</body>
</html>

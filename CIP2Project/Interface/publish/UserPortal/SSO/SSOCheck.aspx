<%@ page language="C#" autoeventwireup="true" inherits="SSO_SSOCheck, App_Web_ssocheck.aspx.27254924" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script language="javascript" type="text/javascript">
			function PostToUA()
			{
				document.form1.submit();
			}

		</script>
</head>
<body>
    <form id="form1"  name="form1" runat="server"  >
			
    <div>
        <asp:Label ID="Label1" runat="server" Text="02" Width="136px"></asp:Label><br />
        <br />
        <asp:TextBox ID="TextBox1" runat="server">02</asp:TextBox><br />
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox><br />
        <br />
        &nbsp;<br />
        <asp:Label ID="Label2" runat="server" Text="帐号类型："></asp:Label>
        <asp:TextBox ID="TextBox5" runat="server">0000000</asp:TextBox>
        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>&nbsp;<br />
        <br />
        <br />
        </div>
    </form>
</body>
</html>

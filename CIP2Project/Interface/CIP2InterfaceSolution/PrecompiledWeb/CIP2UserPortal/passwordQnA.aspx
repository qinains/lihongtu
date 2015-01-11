<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="passwordQnA, App_Web_waxseis2" title="Untitled Page" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="ca">
			<h3>设置密码提示问题</h3>
</div>
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
<div class="cb">
			<table class="tableA">
				<tr>
					<th><asp:Label ID="txtQuestion1" runat="server" Text="问题一"></asp:Label></th>
					<td><asp:DropDownList ID="ddlQuestion1" runat="server"></asp:DropDownList></td>
					<td><asp:TextBox ID="txtAnswer1" runat="server" CssClass="texti"></asp:TextBox></td>
				</tr>
				<tr>
					<th><asp:Label ID="Label2" runat="server" Text="问题二"></asp:Label></th>
					<td><asp:DropDownList ID="ddlQuestion2" runat="server"></asp:DropDownList></td>
					<td><asp:TextBox ID="txtAnswer2" runat="server" CssClass="texti"></asp:TextBox></td>
				</tr>
				<tr>
					<th><asp:Label ID="Label3" runat="server" Text="问题三"></asp:Label></th>
					<td><asp:DropDownList ID="ddlQuestion3" runat="server"></asp:DropDownList></td>
					<td><asp:TextBox ID="txtAnswer3" runat="server" CssClass="texti"></asp:TextBox></td>
				</tr>
				<tr>
					<td colspan="3" style="text-align:center;">
						<textarea cols="45" rows="5">用户可以在此页面里选择多条密码问题及答案。</textarea>
                        <asp:TextBox ID="txtHidSq1" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtHidSq2" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtHidSq3" runat="server" Visible="False"></asp:TextBox>
					</td>
				</tr>
			</table>

			<div class="subtn"><span class="btn btnA"><span><button type="submit" runat="server" id="btnlogin" onserverclick="btnlogin_ServerClick">确定</button></span></span></div>
</div>
</asp:Content>


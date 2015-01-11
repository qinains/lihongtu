<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="setMobile2.aspx.cs" Inherits="setMobile2" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="cb">
			<div class="notice true">
				<table><tr>
					<th><img src="images/true.gif" width="57" height="45" /></th>
					<td>
						<strong>你好！你的认证手机已经被激活！</strong><br />你的认证的手机为：<strong><%=Phone %></strong>
					</td>
				</tr></table>
			</div>
		</div>
</asp:Content>


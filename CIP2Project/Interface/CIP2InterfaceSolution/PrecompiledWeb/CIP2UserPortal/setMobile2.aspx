<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="setMobile2, App_Web_waxseis2" title="Untitled Page" enableEventValidation="false" %>
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


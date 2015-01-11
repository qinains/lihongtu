<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="bizCard3, App_Web_bizcard3.aspx.cdcab7d2" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
<div class="cb">
			<div class="notice true">
				<table><tr>
					<th><img src="images/true.gif" width="57" height="45" /></th>
					<td>
						<strong>你好！你的商旅卡已经申请成功！</strong><br />你的商旅卡号为：<label id="lbErrorInfo" runat="Server"></label>
					</td>
				</tr></table>
			</div>
		</div>
<div class="cb">		
<div class="subtn"><span class="btn btnA"><span>
<asp:Button ID="Button1" runat="server" Text="返回" OnClick="Button1_Click" />
</span></span></div>
		</div>
   
</asp:Content>

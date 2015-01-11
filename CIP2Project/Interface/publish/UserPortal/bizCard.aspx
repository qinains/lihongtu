<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="bizCard, App_Web_bizcard.aspx.cdcab7d2" title="Untitled Page" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
          <div class="ca">
			<h3>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="您已经申请过商旅卡了,你的商旅卡号为:"
                    Visible="False" Width="468px"></asp:Label>&nbsp;</h3>
              <h3>
                  申请商旅卡</h3>
		</div>
		<div class="cb">
			<div class="cardAgre">
			<form action="bizCard2.aspx"  method="post" id="form2" name="form2">
				<span class="btn btnA">
			<span><button type="button" runat="server" id="btnlogin2"  onserverclick="btnlogin2_ServerClick">返 回</button>
            </span></span>
		</form>
		</div>
	</div>
</asp:Content>

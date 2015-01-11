<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="voicePassword, App_Web_waxseis2" title="Untitled Page" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript" src="JS/setPwd/setPwd.js"></script>
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
<div class="ca">
			<h3>修改语音密码</h3>
		</div>
		<div class="cb">
			<table class="tableA">
				<tr>
					<th><label for="oldPasswd"><span class="mst">*</span><asp:Label ID="Label1" runat="server" Text="输入原密码"></asp:Label>：</label></th>
					<td style="width: 190px"><input type="password" value="" id="oldPasswd" class="texti" name="oldPasswd" maxlength="6" onblur="OldPwdBlur();" /><span id="err_oldPasswd" class="remark"></span></td>
				</tr>
				<tr>
					<th><label for="passwd"><span class="mst">*</span>输入新密码：</label></th>
					<td style="width: 190px"><input type="password" value="" id="passwd" class="texti" name="passwd" maxlength="6" onblur="NewPwdBlur('0');"/><span id="err_passwd" class="remark"></span></td>
				</tr>
				<tr>
					<th><label for="verifyPasswd"><span class="mst">*</span>确认新密码：</label></th>
					<td style="width: 190px"><input type="password" value="" id="verifyPasswd" class="texti" name="verifyPasswd" maxlength="6" onblur="VerifyPwdBlur();" /><span id="err_verifyPasswd" class="remark"></span></td>
				</tr>
				<tr>
					<th><label for="code"><span class="mst">*</span>输入验证码：</label></th>
					<td style="width: 190px">
						<input type="text" value="" id="code" class="texti" name="code"/>
					</td>
				</tr>
				<tr>
					<th><label for="code"></label></th>
					<td style="width: 190px">
					    <div id="err_code" class="remark" runat="server">  </div>
						<span class="code">                        
						<script language="javascript" type="text/javascript">
				            document.write("<img id='IMG2' src='ValidateToken.aspx' width='62' height='22'>");
				            function RefreshCode()
				            {
					            document.all.IMG2.src = 'ValidateToken.aspx?.tmp='+Math.random();
				            }
                        </script>
                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span>
                        </span>
					</td>
				</tr>
			</table>
			<div class="subtn"><span class="btn btnA"><span><asp:Button ID="btn_OK" runat="server" class="btn_OK" Text="确定" OnClick="btn_OK_Click"  /></span></span><span class="btn btnA"><span><input id="Button1" type="button" value="重填" onclick="clear1();" class="btn_OK"/></span></span></div>
			<div id="error" class="remark"  runat="server" style="text-align:center"></div>
		</div>
</asp:Content>


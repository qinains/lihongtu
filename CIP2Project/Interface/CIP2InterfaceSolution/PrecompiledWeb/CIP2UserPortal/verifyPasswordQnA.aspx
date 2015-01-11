<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="verifyPasswordQnA, App_Web_waxseis2" title="Untitled Page" enableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="cb">
			<div class="notice true">
				<table><tr>
					<th><img alt="" src="images/true.gif" width="57" height="45" /></th>
					<td>
						<strong>亲爱的用户，恭喜您！您已成功设置密码提示问题！</strong>
					</td>
				</tr></table>
			</div>
			<p class="note">3秒钟后如不自动返回，请<a href="passwordQnA.aspx">点击&gt;&gt;</a></p>
		</div>
		<script language="javascript" type="text/javascript">
            function dd () {}
            setInterval(dd, 5000)
            window.location.href = 'passwordQnA.aspx'
        </script>
</asp:Content>


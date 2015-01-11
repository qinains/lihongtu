<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="verifyEmail, App_Web_waxseis2" title="Untitled Page" enableEventValidation="false" %>

<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

    <script language="javascript" type="text/javascript" src="JS/setEmail/setEmail_JS.js"></script>

    <div class="ca">
        <h3>
            设置认证邮箱</h3>
    </div>
    <div class="cb">
        <form>
            <table class="tableA">
            <tr>
                    <th>
                        <label for="password">
                            登录密码：</label>
                    </th>
                    <td>
                        <input type="password" id="pwdTxt" class="texti" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label for="email">
                            输入邮箱帐号：</label>
                    </th>
                    <td>
                        <input type="text" value="" id="Emailtxt" class="texti"  runat="server"/><asp:Label ID="EmailClassLab" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label for="verifyMobile">
                            验证码：</label></th>
                    <td>
                        <input type="text" id="pageyzm" />

                        <script language="javascript" type="text/javascript">
				            document.write("<input type=hidden name=codeId id=codeId value=",Math.random(),">")
				            var tmp = document.getElementById("codeId").value;
				            document.write("<img id='IMG2' src='ValidateToken.aspx?yymm=",tmp,"' width='62' height='22'>");
				            function RefreshCode()
				            {
					            document.all.IMG2.src = 'ValidateToken.aspx?yymm='+Math.random();
				            }
                        </script>

                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span></td>
                </tr>
                <tr style="display: none" id="errtr">
                    <th>
                        <img src="images/false.gif" width="73" height="65" /></th>
                    <td>
                        <strong></strong>
                    </td>
                </tr>
            </table>
            <div class="subtn">
                <span class="btn btnA"><span>
                    <button type="button" onclick="selEmail();">
                        确定</button></span></span></div>
            <p id="msgp" class="note" style="display:none;color:Red;"></p>	
        </form>
    </div>
    <input type="button" id="pageyzmbutton" onclick="RefreshCode();" style="display:none;" />
        <input type="text" id="custidtxt" runat="server" style="display: none;" />
    <input type="button" id="urlRedirectButton" style="display: none;" runat="server"
        onserverclick="urlRedirectButton_ServerClick" />
</asp:Content>

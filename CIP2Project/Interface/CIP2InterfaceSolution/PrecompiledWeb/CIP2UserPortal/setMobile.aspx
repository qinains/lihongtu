<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="setMobile, App_Web_waxseis2" title="Untitled Page" enableEventValidation="false" %>

<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

    <script language="javascript" type="text/javascript" src="JS/setMobile/setMoblie_JS.js"></script>

    <div class="ca">
        <h3>
            设置认证手机</h3>
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
                        <label for="verifyMobile">
                            认证手机号码：</label>
                    </th>
                    <td>
                        <input type="text" id="verifyMobile" class="texti" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            手机验证码：</label></th>
                    <td>
                        <input type="text" id="tPasswd" />
                        <span class="btn btnA">
                            <button type="button" id="selButton" onclick="selmobile($('#num').attr('value'),1);"
                                class="btn btnA">
                                获取验证码</button></span>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            验证码：</label></th>
                    <td>
                        <input type="text" id="pageyzm" />

                        <script language="javascript" type="text/javascript">
				            document.write("<input type=hidden name=pageyzm id=pageyzm value=",Math.random(),">")
				            var tmp = document.getElementById("pageyzm").value;
				            document.write("<img id='IMG2' src='ValidateToken.aspx?yymm=",tmp,"' width='62' height='22'>");
				            function RefreshCode()
				            {
					            document.all.IMG2.src = 'ValidateToken.aspx?yymm='+Math.random();
				            }
                        </script>

                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span>
                    </td>
                </tr>
            </table>
            <div class="subtn">
                <span class="btn btnA"><span>
                    <button type="button" id="setButton" onclick="setmobile(); ">
                        确定</button></span></span></div>
            <p id="msgp" class="note" style="display: block;color:Red;">
            </p>
        </form>
    </div>
        <input type="button" id="pageyzmbutton" onclick="RefreshCode();" style="display:none;" />
    <input type="text" id="custidtxt" runat="server" style="display: none;" />
    <input name="num" id="num" size="3" value="0" style="display: none;" />
    <input type="button" id="urlRedirectButton" style="display: none;" runat="server"
        onserverclick="urlRedirectButton_ServerClick" />
</asp:Content>

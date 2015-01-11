<%@ page language="C#" autoeventwireup="true" inherits="FindPassWord_ResetPwdByEmail, App_Web_uglirgsr" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="~/CSS/account.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/Stylecss.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/msgStyle.css" type="text/css" rel="stylesheet" media="screen" />
    <%--<script language="javascript" type="text/javascript" src="../ModelJS/jquery.js"></script>--%>

    <script language="javascript" type="text/javascript" src="JS/jquery-latest.pack.js"></script>

    <script language="javascript" type="text/javascript" src="JS/ResetPwdByEmail.js"></script>

    <style type="text/css">
        .tableA th,.tableA td{padding-left:7px;padding-right:7px;padding-top:5px;padding-bottom:2px}
        .msg .msg-inline .show{display:none}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="Top1" runat="server" />
        <div>
            <div class="btContain clfx">
                <div id="main">
                    <div class="ca">
                        <h3>
                            重置密码</h3>
                    </div>
                    <asp:Panel ID="ResetPanel" runat="server">
                        <div class="cb">
                            <center>
                                请设置新的登录密码，这次一定要记牢哦！</center>
                            <table class="tableA">
                                <tr>
                                    <th>
                                        <label for="email">
                                            新密码：</label>
                                    </th>
                                    <td style="width: 189px">
                                        <input type="password" value="" id="txtPwd" class="texti" />
                                    </td>
                                    <td style="width: 150px">
                                        <div class="msg msg-inline show" id="pwdError_tip">
                                            <div class="msg-default msg-error">
                                                <i class="msg-icon"></i>
                                                <div class="msg-content" id="pwdError_msg">
                                                    不能为空
                                                </div>
                                            </div>
                                        </div>
                                        <%--<span id="nameMsgSpan" class="remark">用户名不能为空</span>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label for="email">
                                            再次输入新密码：</label>
                                    </th>
                                    <td style="width: 189px">
                                        <input type="password" value="" id="txtPwdSecond" class="texti" />
                                    </td>
                                    <td style="width: 150px">
                                        <div class="msg msg-inline show" id="pwdSecondError_tip">
                                            <div class="msg-default msg-error">
                                                <i class="msg-icon"></i>
                                                <div class="msg-content" id="pwdSecondError_msg">
                                                    不能为空
                                                </div>
                                            </div>
                                        </div>
                                        <%--<span id="emailMsgSpan" class="remark">邮箱不能为空</span>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label>
                                            验证码：</label></th>
                                    <td style="width: 189px">
                                        <input type="text" id="txtCode" class="texti" style="width: 60px" />

                                        <script language="javascript" type="text/javascript">
				                            document.write("<input type=hidden name=pageyzm id=pageyzm value=",Math.random(),">")
				                            var tmp = document.getElementById("pageyzm").value;
				                            document.write("<img id='IMG2' src='../ValidateToken.aspx?yymm=",tmp,"' width='62' height='22'>");
				                            function RefreshCode()
				                            {
					                            document.all.IMG2.src = '../ValidateToken.aspx?yymm='+Math.random();
				                            }
                                        </script>

                                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span>
                                    </td>
                                    <td style="width: 150px">
                                        <div class="msg msg-inline show" id="codeError_tip">
                                            <div class="msg-default msg-error">
                                                <i class="msg-icon"></i>
                                                <div class="msg-content" id="codeError_msg">
                                                    不能为空
                                                </div>
                                            </div>
                                        </div>
                                        <%--<span id="codeMsgSpan" class="remark">验证码不能为空</span>--%>
                                    </td>
                                </tr>
                            </table>
                            <div class="subtn" style="height:40px">
                                <span class="btn btnA"><span>
                                    <img id="loadImg" src="Images/Loading.gif" style="position:absolute; display:none" />
                                    <button type="button" id="btnSave">完 成</button>
                                    </span></span></div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="MsgPanel" runat="server">
                        <div class="cb">
                            <center>
                                <div style="height: 100px; padding-top: 30px;">
                                    激活邮件的链接有错误（重设密码链接只能用一次，并且2小时内有效）
                                </div>
                            </center>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <uc2:Foot ID="Foot1" runat="server" />
        <asp:HiddenField ID="hdCustID" runat="server" />
        <asp:HiddenField ID="hdEmail" runat="server" />
        <asp:HiddenField ID="hdAuthenCode" runat="server" />
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindByEmail.aspx.cs" Inherits="FindByEmail" %>

<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="~/CSS/account.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/Stylecss.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/msgStyle.css" type="text/css" rel="stylesheet" media="screen" />
    <%--<script language="javascript" type="text/javascript" src="../ModelJS/jquery.js"></script>--%>
    <script language="javascript" type="text/javascript" src="JS/jquery-latest.pack.js"></script>
    <script language="javascript" type="text/javascript" src="JS/FindPwdByEmail.js"></script>
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
                            认证邮箱找回密码</h3>
                    </div>
                    <div class="cb">
                        <center>
                            忘记了密码？请输入注册时填写的用户名以及对应邮箱，我们会把重设密码邮件发送到您的邮箱！</center>
                        <table class="tableA">
                            <%--<tr>
                                <th>
                                    <label for="email">
                                        用户名：</label>
                                </th>
                                <td style="width: 189px">
                                    <input type="text" value="" id="txtName" class="texti" />
                                </td>
                                <td style="width:150px">
                                    <div class="msg msg-inline show" id="nameError_tip">
                                        <div class="msg-default msg-error">
                                            <i class="msg-icon"></i>
                                            <div class="msg-content" id="nameError_msg">
                                                不能为空
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>--%>
                            <tr>
                                <th>
                                    <label for="email">
                                        认证邮箱：</label>
                                </th>
                                <td style="width: 189px">
                                    <input type="text" value="" id="txtEmail" class="texti" />
                                </td>
                                <td style="width:150px">
                                    <div class="msg msg-inline show" id="emailError_tip">
                                        <div class="msg-default msg-error">
                                            <i class="msg-icon"></i>
                                            <div class="msg-content" id="emailError_msg">
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
                                    <input type="text" id="txtCode" class="texti" style="width:60px" />
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
                                <td style="width:150px">
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
                            <%--<input type="button" value="完 成" id="btnSave" />--%></span></span></div>
                    </div>
                </div>
            </div>
        </div>
        <uc2:Foot ID="Foot1" runat="server" />
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
    </form>
</body>

</html>

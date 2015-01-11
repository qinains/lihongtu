<%@ page language="C#" autoeventwireup="true" inherits="FindPassWord_SendMailSuccess, App_Web_uglirgsr" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/account.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/Stylecss.css" type="text/css" rel="stylesheet" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="Top1" runat="server" />
        <div>
            <div class="btContain clfx">
                <div id="main">
                    <div class="ca">
                        <h3>
                            邮件发送成功</h3>
                    </div>
                    <div class="cb">
                        <center>
                            <div class="login_cont06">
                                您的申请已提交成功，请查看您的邮箱！
                                <br />
                                <asp:HyperLink ID="linkReSendEmail" runat="server" Style="color: Red">返回重新发送邮件</asp:HyperLink>
                                <%--<a href="FindByEmail.aspx" style="color: Red">返回重新发送邮件</a>--%>
                            </div>
                        </center>
                        <div class="subtn">
                            <span class="btn btnA"><span>
                                <button id="btnClose" onclick="javascript:window.close()">
                                    关闭此页</button></span></span></div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
        <uc2:Foot ID="Foot1" runat="server" />
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Success.aspx.cs" Inherits="FindPassWord_Success" %>

<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/account.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/Stylecss.css" type="text/css" rel="stylesheet" media="screen" />
     <script language="javascript" type="text/javascript" src="JS/jquery-latest.pack.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="Top1" runat="server" />
        <div>
            <div class="btContain clfx">
                <div id="main">
                    <div class="ca">
                        <h3>
                            操作成功</h3>
                    </div>
                    <div class="cb">
                        <center>
                            <div class="login_cont06">
                                密码修改成功，页面正在跳转，请耐心等待…
                                <br />
                                <img src="Images/large-loading.gif" /></div>
                                
                        </center>
                        <div class="subtn">
                            <span class="btn btnA"><span>
                                <button id="btnClose" onclick="javascript:window.close()">
                                    关闭此页</button></span></span></div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdRedirectUrl" runat="server" />
        <uc2:Foot ID="Foot1" runat="server" />
    </form>
    <script type="text/javascript">
        function Redirect(){
            var redirectUrl = $("#hdRedirectUrl").val();
            window.location.href=redirectUrl;
        }
        setTimeout("Redirect()",1000);
        
    </script>
</body>
</html>

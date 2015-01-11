<%@ page language="C#" autoeventwireup="true" inherits="Demo, App_Web_kpte7rfs" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script language="javascript" type="text/javascript" src="ModelJS/jquery.js"></script>
    <script type="text/javascript">
        $(document).ready(function(){
            $("#iframe1").attr("src",$("#hdUDBUrl").val());
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<iframe id="iframe1" runat="server" width="100%" height="800" src="http://Service.Passport.189.cn/Logon/UDBCommon/S/PassportLogin.aspx?PassportLoginRequest=3500000000408201%24A2ifLX%2fI714uAtuqKUlpf4bCwFCWcYrBzQUkREXhcIXiYlEbZxQ1QP6P6bczaDKIEbPqcb6ENJfx%0d%0apOfJMz2m6tB7nzxp%2fAmN0HtiZitU7AF8Ivmuf7BbUiaXM80MbB8xRntajMUw3rDfOB2UcXnqwJi%2b%0d%0ag9f%2frNLjrz%2fNrU8YXqV0b6Mb9DrKV%2fusGNfU4wI%2fDmrXvcJ6H8wguLsnRFXqbbsRHwAnSNvW"></iframe>
        --%>
        <iframe id="iframe1" runat="server" width="100%" height="800" src="http://Service.Passport.189.cn/Logon/UDBCommon/S/PassportLogin.aspx"></iframe>
        
    </div>
        <asp:HiddenField ID="hdUDBUrl" runat="server" />
    </form>
</body>
</html>

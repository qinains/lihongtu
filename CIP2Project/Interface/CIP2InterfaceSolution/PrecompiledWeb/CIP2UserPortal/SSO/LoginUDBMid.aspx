<%@ page language="C#" autoeventwireup="true" inherits="SSO_LoginUDBMid, App_Web_2mjfkmpd" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>

    <script type="text/javascript">
        $(document).ready(function(){
            var url =$("#hdReturnUrl").val();
            window.parent.location.href=url;
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 100px">
        </div>
        <div>
            <center>
                <img src="../images/large-loading.gif" />
            </center>
        </div>
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankRechargeSuccess.aspx.cs"
    Inherits="UserAccount_BankRechargeSuccess" %>

<%@ Register Src="../UserCtrl/CommonHead.ascx" TagName="CommonHead" TagPrefix="uc1" %>
<%@ Register Src="../UserCtrl/CommonFoot.ascx" TagName="CommonFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css">
    <style>
        .rechargeBox1{width:863px;border:1px solid #8fc4e0;margin:10px auto;padding:20px 0 20px 115px;background:#ecfaff;font-size:14px}
        .rechargeBox1 p{line-height:22px;margin-bottom:5px}
        .rechargeBox1 p strong{color:#41ab01}
        .rechargeBox1 p a{color:#39c;margin:0 5px}
    </style>
    
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    
</head>
<body>
    <uc1:CommonHead ID="CommonHead1" runat="server"></uc1:CommonHead>
    <form id="form1" runat="server">
        <div class="rechargeBox1">
            <p style="background: url(images/bg_rechargeok.png) no-repeat; padding-top: 45px;
                padding-left: 70px">
                <asp:Literal ID="lblMsg" runat="server"></asp:Literal>
            <p style="padding-left: 70px">
                <font></font></p>
        </div>
    </form>
    <uc2:CommonFoot ID="CommonFoot1" runat="server"></uc2:CommonFoot>
    <script type="text/javascript">
        $("form").eq(0).show();
        $(".rechargeDd").children("a").eq(0).addClass("hover");
        $(".rechargeDd").children("a").click(function(){
	        $(this).addClass("hover").siblings().removeClass("hover");
	        $("form").eq($(this).attr("pos")).show().siblings("form").hide();
        });
    </script>

</body>
</html>

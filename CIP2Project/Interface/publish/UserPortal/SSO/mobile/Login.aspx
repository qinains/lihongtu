<%@ page language="C#" autoeventwireup="true" inherits="SSO_mobile_Login, App_Web_login.aspx.81d98cab" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
	登录
</title>
<meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
<meta content="yes" name="apple-mobile-web-app-capable" />
<meta content="black" name="apple-mobile-web-app-status-bar-style" />
<meta content="telephone=no" name="format-detection" />
<link href="css/base.css"rel="stylesheet" type="text/css" />
<link href="css/global.css"rel="stylesheet" type="text/css" />
<link href="css/mobile.css"rel="stylesheet" type="text/css" />
<link href= "css/pay.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../JS/jquery-1.4.2.min.js"></script> 

   
</head>
<body>
    <div class="wapPage">
    <form id="form1" runat="server">
        <center>
       
            <div style="margin-top:30px;">
           <div class="loginBG"></div> 
                 <a href="..\..\CustInfoManager\M\LowSintRegisterM.aspx?SPTokenRequest=<%=SPTokenRequest %>">免费注册</a>
                <span id="errorHint" runat="server" style="color:Red"></span>
                <table class="tabLogin">
                    <tr>
                        <th height="40" style=" text-indent:5px;">
                            账号：</th>
                        <td>
                            <asp:TextBox ID="username" runat="server" style="width:250px; height:30px; font-size:16px; border:1px solid #ccc;" ></asp:TextBox>
                                            
                        </td>
                    </tr>
                    <tr>
                        <th height="40" style=" text-indent:5px;">
                            密码：</th>
                        <td>
                            <asp:TextBox ID="password" runat="server" TextMode="Password" style="width:250px; height:30px; font-size:16px; border:1px solid #ccc;"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td height="40" colspan="2" style="text-align:center; padding:30px 0 0 0;">
                            &nbsp;<asp:ImageButton ID="ImageBtnLogin" runat="server" ImageUrl="~/SSO/images/bt_login.gif" OnClick="ImageBtnLogin_Click" />
                            
                        </td>
                        
                    </tr>
                </table>
            </div>
        </center>
    </form>
   </div> 
</body>
</html>

<%Response.AddHeader("P3P", "CP=CAO PSA OUR");%>
<%@ page language="C#" autoeventwireup="true" inherits="yiqigou_login, App_Web_2mjfkmpd" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title></title>
<style type="text/css">
<!--
body {font-size:12px;margin:0;padding:0;}
p {margin:0;padding:0;}
.f14 {font-size:14px;}
.underline {text-decoration:underline;}
.loginbtn {background:url(images/btn_login.jpg) no-repeat;border:0;width:72px;height:27px;cursor:pointer;vertical-align:middle;}
.login_input {height:30px;line-height:34px;width:146px;background:url(images/login_input.jpg) 0 0 no-repeat;border:0;color:#a1a1a1;padding:2px 7px;vertical-align:middle;}
.sms_input {height:30px;line-height:34px;width:51px;background:url(images/sms_input.jpg) no-repeat;border:0;color:#a1a1a1;padding:2px 7px;vertical-align:middle;}
#errorHint{margin-left:70px;font-size:14px;color:#C30;height:30px;line-height:30px}
.loginbox {width:100%;}
.loginbox .content {}
.loginbox .content p {line-height:20px;margin-bottom:4px;vertical-align:middle;}
.loginbox .content .login {margin-top:28px;margin-left:57px;line-height:27px;}
.loginbox .content .errorprompt {background:url(images/icon_1.gif) 0px 3px no-repeat;text-indent:20px;margin:10px 0;line-height:18px;color:red;}
-->
</style>
   
<script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>    
<script language="javascript" type="text/javascript">
// <!CDATA[

                function RefreshCode()
	            {
		            document.all.IMG2.src = '../ValidateToken.aspx?.tmp='+Math.random();
	            }
				
				function check_input()
				{       var userName = $('#username').val();
		                if(userName==""){
		                    $("#errorHint").html("请输入账号");
		                    $("#username").focus();				
			                return false;
		                }else{
			                $("#errorHint").html("");
		                }
		                var regMobile=/^1([3][0-9]|[5][012356789]|[8][0123456789])\d{8}$/;		//手机号验证
		                var regEmail=/^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$/;		//邮箱验证
		                var regCard=/^(\d{9}|\d{16})$/;		//商旅卡验证
		                var regUserName=/^[a-zA-Z][a-zA-Z0-9]*([-._]?[a-zA-Z0-9]+)*$/;  // 用户名
		                $('#AuthenType').val('2');
		                if(regMobile.test(userName)){   //手机
			                $('#AuthenType').val('2');
		                }else if(regEmail.test(userName)){  // 邮箱
			                $('#AuthenType').val('4');
		                }else if(regCard.test(userName)){
			                 $('#AuthenType').val('3');  // 商旅卡
		                }else if(regUserName.test(userName)){
			                $('#AuthenType').val('1'); // 用户名
		                }
				        return true;
				}
				      			

// ]]>
</script>
</head>
<body>
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

<div class="loginbox">
			<div class="content">
			<form id="login" runat="server">
			    <p><font class="f14">用户名：</font><input name="username" id="username" type="text" value="" class="login_input"/></p>
			    <p><font class="f14" style="letter-spacing:3px;*letter-spacing:5px;">密 码</font>：<input name="password" type="password" value="" class="login_input"/> </p>
                <p><font class="f14">验证码：</font><input name="checkCode" id="checkCode" type="text" value="" class="sms_input"/> <img width="62" height="30" src="../ValidateToken.aspx" id="IMG2" alt=""/><a href="javascript:RefreshCode()">看不清？换一张</a></p>
				 <span id="errorHint"  runat="server"></span>
			    <p class="login"></p>
                <asp:Button ID="Submit1" class="loginbtn"  runat="server" OnClick="Submit1_Click"   OnClientClick="check_input();" />
               <input type="hidden" value ="" id="AuthenType" name="AuthenType" />
               
			</form>

			</div>
</div>
</body>
</html>

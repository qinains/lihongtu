<%@ page language="C#" autoeventwireup="true" inherits="SSO_AuthBindLogin, App_Web_2mjfkmpd" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>

    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    <style type="text/css">
    .authBox{width:600px;padding:20px 30px 20px 30px;background:#fff;color:#555}
    .authBox a{color:#618ADE}
    .authBox p{margin-bottom:5px}
    .authBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1;margin-bottom:30px}
    .authBox h2 span{color:#555;margin-right:5px}
    .authBox form{margin-bottom:30px}
    .authBox label{display:block;height:35px;margin-bottom:15px}
    .authBox label span{display:block;float:left;margin-right:10px;width:80px;text-align:right;font-size:14px;line-height:30px}
    .authBox label em{line-height:30px;color:red;margin-left:5px;margin-right:10px;float:left;display:inline}
    .authBox label p{line-height:30px;float:left;margin-left:5px}
    .authBox label p.hintCorrect{padding-left:20px;color:#390;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px 5px}
    .authBox label p.hintError{padding-left:20px;color:#C30;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px -21px}
    .authBox label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
    input#checkCode{width:50px;margin-right:5px}
    #sendCode{float:left;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer}
    .authBox label .checkbox{margin-left:90px;margin-right:10px}
    .authBox label a{line-height:30px;margin-left:5px}
    #auth{width:164px;height:38px;margin-left:90px;border:none;background:url(images/spriteLoginRegister.png) no-repeat 0 -398px;cursor:pointer;display:block;text-indent:-9999px}
    #successNote{margin-left:90px;margin-top:10px;padding-left:20px;line-height:30px;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px 5px;display:none}
    </style>   
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../js/jquery.inputHint.js"></script>  
    
</head>
<body>
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

 <div class="authBox">
	<h2><span>我已经有</span>号百通行证，<span>直接绑定</span></h2>
	<span id="errorHint"  runat="server"></span>
    <form id="form1" runat="Server" onsubmit="return checkLogin();">
        <label for="mobile"><span>账号：</span><input type="text" class="input" id="username" /><p>请输入您的账号</p></label>
        <label for="password"><span>密码：</span><input type="password" class="input" id="password" /><a href="../FindPwdSelect.aspx">忘记密码？</a></label>
        <asp:Button ID="auth" runat="server" Text="绑定" OnClick="auth_Click" />
        <input type="hidden" value ="" id="AuthenType" name="AuthenType" />
        <p id="successNote">账号绑定成功，3秒后跳转至首页。</p>
    </form>
</div>



 <script language="javascript" type="text/javascript">
				 
	       
				
				function checkUserNameEmpty()
				{
                    if($("#username").attr("value") == ""){
                        $("#username").focus();
                        $("#errorHint").html("");
                        $("#errorHint").html("请输入账户");
                        return false;
                    }else{
                      $("#errorHint").html("");
                    }
                    return true;
                }

				
				
				function checkUserName()
				{       var userName = $('#username').val();
		                if(userName==""){
		                    $("#errorHint").html("请输入账号");
		                    $("#username").focus();				
			                return false;
		                }else{
			                $("#errorHint").html("");
		                }
		                var regMobile=/^1([3][0-9]|[5][012356789]|[8][056789])\d{8}$/;		//手机号验证
		                var regEmail=/^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$/;		//邮箱验证
		                var regCard=/^(\d{9}|\d{16})$/;		//商旅卡验证
		                if(regMobile.test(userName)){   //手机
			                $('#AuthenType').val('2');
		                }else if(regEmail.test(userName)){  // 邮箱
			                $('#AuthenType').val('4');
		                }else if(regCard.test(userName)){
			                 $('#AuthenType').val('3');  // 商旅卡
		                }else{
			                $('#AuthenType').val('1'); // 用户名
		                }
				        return true;
				}
				      
                function checkPassword(){
                    if($("#password").attr("value") == ""){
                        $("#password").focus();
                        $("#errorHint").html("请输入密码");
                        return false;
                    }else{
                      $("#errorHint").html("");
                    }
                    return true;
                }

           


               function checkLogin(){ 
                   
                     if(checkUserNameEmpty()&&checkPassword()&&checkUserName()){
                        return true;
                     }else{
                       return false;
                     }
                }

				
			$("#username").inputHint({hint:"手机号/邮箱/用户名/商旅卡"});
	
				
				            
  </script>



</body>
</html>

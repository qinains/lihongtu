<%Response.AddHeader("P3P", "CP=CAO PSA OUR");%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PassportLogin.aspx.cs" Inherits="SSO_PassportLogin" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>号百欢迎您</title>
    
    <meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号码百事通,114,118114" />
    <meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." />
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../js/jquery.inputHint.js"></script>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css"/>
    <style type="text/css">
        .loginBox{width:330px;padding:20px 30px 20px 20px;border:1px solid #8fc4e0;background:#fff;color:#555}
        .loginBox a{color:#618ADE}
        .loginBox p{margin-bottom:5px}
        .loginBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1}
        #errorHint{margin-left:70px;font-size:14px;color:#C30;height:30px;line-height:30px}
        .loginBox form{margin-bottom:30px}
        .loginBox label{display:block;height:35px;margin-bottom:20px}
        .loginBox label span{display:block;float:left;margin-right:10px;width:60px;text-align:right;font-size:14px;line-height:30px}
        .loginBox label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
        input#checkCode{width:50px;margin-right:5px}
        .loginBox label .checkbox{margin-left:70px;margin-right:10px}
        .loginBox label img{display:inline-block;vertical-align:middle;float:left}
        .loginBox label a{line-height:30px;margin-left:5px}
        #checkUserName{width:108px;height:38px;margin-left:70px;border:none;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat 0 0;cursor:pointer;display:block;text-indent:-9999px}
        .otherLogin a{padding:4px 0 4px 20px;margin-right:20px;background-image:url(http://static.118114.cn/images/spriteLoginRegis.png);background-repeat:no-repeat}
        #loginSina{background-position:-207px -54px}
        #loginQQ{background-position:-207px -84px}
        #login189{background-position:-207px -113px}
   
    </style>
 
 
    
</head>
<body>
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
<div class="loginBox">
	<h2>会员登录</h2>
	 <span id="errorHint"  runat="server"></span>
     <form id="form1" runat="server" onsubmit="return checkLogin();">
    	<label for="username"><span>账号：</span><asp:TextBox ID="username" CssClass="input" runat="server"></asp:TextBox></label>
        <label for="password"><span>密码：</span> <asp:TextBox ID="password" CssClass="input" runat="server" TextMode="Password"></asp:TextBox> <a href="../FindPwdSelect.aspx" target="_blank">忘记密码</a></label>
        <label for="checkCode"><span>验证码：</span><input type="text" value="" id="checkCode" name="checkCode" class="input" maxlength="4" /><img width="62" height="30" src="../ValidateToken.aspx" id="IMG2" alt=""/><a href="javascript:RefreshCode()">看不清？换一张</a></label>
        <!-- <label for="autoLogin"><input type="checkbox" class="checkbox" id="autoLogin" hidefocus="true"/>下次自动登录</label> -->
        
        <asp:Button ID="checkUserName" runat="server" OnClick="Button1_Click" />
        <input type="hidden" value ="" id="AuthenType" name="AuthenType" />
    </form>
    <p>没有号百通行证？
    
   
        <asp:HyperLink ID="linkU1" runat="server">马上注册一个</asp:HyperLink>
    
    </p>
    <p>也可使用合作网站帐号登录：</p>
    <p class="otherLogin">
    <!-- 
		<asp:HyperLink id="loginSina" runat="server" Target="_top">
				  新浪微博
	    </asp:HyperLink>       
  		<asp:HyperLink id="loginQQ" runat="server" Target="_top">
				  QQ
	    </asp:HyperLink>     
     -->

      	<asp:HyperLink id="login189" runat="server" Target="_top">
				 天翼189
	    </asp:HyperLink> 
       
    </p>
</div>

  <script language="javascript" type="text/javascript">
				 
	            function RefreshCode()
	            {
		            document.all.IMG2.src = '../ValidateToken.aspx?.tmp='+Math.random();
	            }
				
				
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

                function checkCode(){
                    if($("#checkCode").attr("value") == ""){
                        $("#checkCode").focus();
                        $("#errorHint").html("请输入验证码");
                        return false;
                    }else{
                      $("#errorHint").html("");
                    }
                    return true;
                }


               function checkLogin(){ 
                   
                     if(checkUserNameEmpty()&&checkPassword()&&checkCode()&&checkUserName()){
                        return true;
                     }else{
                       return false;
                     }
                }

				
			$("#username").inputHint({hint:"手机号/邮箱/用户名/商旅卡"});
	
				
				            
  </script>

</body>
</html>

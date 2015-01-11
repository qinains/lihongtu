<%Response.AddHeader("P3P", "CP=CAO PSA OUR");%>
<%@ page language="C#" autoeventwireup="true" inherits="SSO_YiGou_Login, App_Web_yigou_login.aspx.27254924" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
     <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../js/jquery.inputHint.js"></script> 
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    
</head>
<body>
  <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

     <div class="loginBox">
                <div class="clearfix">
                    <h2 onclick="tabconAlt(0,'E',8);" id="tabE0" class="vazn">会员登录</h2>
                    <h2 onclick="tabconAlt(1,'E',8);" id="tabE1">天翼帐号登录</h2>
                </div>
                <div id="conE0">
                    <div id="errorHint" runat="server"></div>
                        <form id="form1" runat="server" onsubmit="return checkLogin();">
                            <label for="username"><span>账号：</span><asp:TextBox ID="username" CssClass="input" runat="server"></asp:TextBox> </label>
                            <label for="password"><span>密码：</span><asp:TextBox ID="password" CssClass="input" runat="server" TextMode="Password"></asp:TextBox><a href="../FindPwdSelect.aspx" target="_blank">忘记密码？</a></label>
                            <label for="checkCode"><span>验证码：</span><input type="text" value="" id="checkCode" name="checkCode" class="input" maxlength="4" /><img width="62" height="30" src="../ValidateToken.aspx" id="IMG2"><a href="javascript:RefreshCode()">看不清？换一张</a></label>
                          <%--  <label for="autoLogin"><input type="checkbox" class="checkbox" id="autoLogin" hidefocus="true" />下次自动登录</label>--%>
                            <asp:Button ID="login" runat="server" OnClick="login_Click"  /> 
                            <input type="hidden" value ="" id="AuthenType" name="AuthenType" />
                        </form>
                        <p>没有号百通行证？<asp:HyperLink ID="linkU1" runat="server">马上注册一个</asp:HyperLink></p>
                 </div>
                 <div id="conE1" style="display:none">
                    <div style="margin-top:30px;margin-bottom:17px">
			        <iframe frameborder="0"  style="filter:chroma (color=#ffffff)"  height="270px" width="100%" id="udbLoginFrame" src="<%=Login189Url %>"></iframe>
			        </div>   
   		        </div>
	        </div>

        <script type="text/javascript">
        
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
				{       
				        var userName = $('#username').val();
		                if(userName==""){
		                    $("#errorHint").html("请输入账号");
		                    $("#username").focus();				
			                return false;
		                }else{
			                $("#errorHint").html("");
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

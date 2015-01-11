<%Response.AddHeader("P3P", "CP=CAO PSA OUR");%>
<%@ page language="C#" autoeventwireup="true" inherits="SSO_YiYou_Login, App_Web_open189login.aspx.27254924" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>号百欢迎您</title>
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../js/jquery.inputHint.js"></script> 
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
  
    <% 
        if ("35433333".Equals(SPID))
        {
    %>
                   <style type="text/css">
                    html body{background:none}
                    .word{float:left;padding-top:39px;_padding-top:41px;color:#007bc1;height:23px;line-height:23px;font-size:25px;font-family:Microsoft Yahei,simhei}
                    .content{background:url(images/bg_login.jpg) no-repeat;margin-top:63px;height:445px}
                    .loginBox{width:330px;padding:20px 30px 20px 20px;border:1px solid #8fc4e0;background:#fff;color:#555;position:relative;height: 332px;}
                    .loginBox a{color:#618ADE}
                    .loginBox p{margin-bottom:5px}
                    .loginBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1}
		            .loginTabWrapper{position:relative;width:330px;height:281px}
		            .loginTabShow,.loginTabHide{position:absolute;top:0;left:0;opacity:1;filter:alpha(opacity=100);z-index:2}
		            .loginTabHide{opacity:0;filter:alpha(opacity=0);z-index:1}
                    #errorHint{margin-left:70px;font-size:14px;color:#C30;height:30px;line-height:30px}
                    .loginBox form{margin-bottom:30px}
                    .loginBox label{display:block;height:35px;margin-bottom:20px;position:relative}
                    .loginBox label span{display:block;float:left;margin-right:10px;width:60px;text-align:right;font-size:14px;line-height:30px}
                    .loginBox label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
                    input#checkCode{width:50px;margin-right:5px}
                    .loginBox label .checkbox{margin-left:70px;margin-right:10px}
                    .loginBox label img{display:inline-block;vertical-align:middle;float:left}
                    .loginBox label a{line-height:30px;margin-left:5px}
                    #login{width:108px;height:38px;margin-left:70px;border:none;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat 0 0;cursor:pointer;display:block;text-indent:-9999px}
                    .otherLogin a{padding:4px 0 4px 20px;margin-right:20px;background-image:url(http://static.118114.cn/images/spriteLoginRegis.png);background-repeat:no-repeat}
                    #loginSina{background-position:-207px -54px}
                    #loginQQ{background-position:-207px -84px}
                    #login189{background-position:-207px -113px}
                    .loginBox h2{float:left;padding:0 10px;clear:none;width:145px;height:35px;line-height:35px;background:#e4e4e4;font-weight:700;font-size:14px;color:#838383; cursor:pointer;text-align:center}
                    .loginBox h2.vazn{ background:#3bb0ff;color:#fff}

                </style>   
    <%
        }
        else
        { 
        
    %>
   
                  <style type="text/css">
                        html body{background:none}
                        .word{float:left;padding-top:39px;_padding-top:41px;color:#007bc1;height:23px;line-height:23px;font-size:25px;font-family:Microsoft Yahei,simhei}
                        .content{background:url(images/bg_login.jpg) no-repeat;margin-top:63px;height:445px}
                        .loginBox{width:330px;padding:20px 30px 20px 20px;border:1px solid #dedede;background:#fff;color:#555;position:relative;height: 332px;}
                        .loginBox a{color:#0A79D3}
                        .loginBox p{margin-bottom:5px}
                        .loginBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1}
		                .loginTabWrapper{position:relative;width:330px;height:281px}
		                .loginTabShow,.loginTabHide{position:absolute;top:0;left:0;opacity:1;filter:alpha(opacity=100);z-index:2}
		                .loginTabHide{opacity:0;filter:alpha(opacity=0);z-index:1}
                        #errorHint{margin-left:70px;font-size:14px;color:#C30;height:30px;line-height:30px}
                        .loginBox form{margin-bottom:30px}
                        .loginBox label{display:block;height:35px;margin-bottom:20px;position:relative}
                        .loginBox label span{display:block;float:left;margin-right:10px;width:60px;text-align:right;font-size:14px;line-height:30px}
                        .loginBox label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
                        input#checkCode{width:50px;margin-right:5px}
                        .loginBox label .checkbox{margin-left:70px;margin-right:10px}
                        .loginBox label img{display:inline-block;vertical-align:middle;float:left}
                        .loginBox label a{line-height:30px;margin-left:5px}
                        #login{width:108px;height:38px;margin-left:70px;border:none;background:url(images/ygLoginRegis.png) no-repeat 0 0;cursor:pointer;display:block;text-indent:-9999px}
                        .otherLogin a{padding:4px 0 4px 20px;margin-right:20px;background-image:url(images/ygLoginRegis.png);background-repeat:no-repeat}
                        #loginSina{background-position:-207px -54px}
                        #loginQQ{background-position:-207px -84px}
                        #login189{background-position:-207px -113px}
                        .loginBox h2{float:left;padding:0 10px;clear:none;width:145px;height:35px;line-height:35px;background:#e4e4e4;font-weight:700;font-size:14px;color:#838383; cursor:pointer;text-align:center}
                        .loginBox h2.vazn{ background:#F37C06;color:#fff}
                </style>   


  
    <%      
        }     
    %>
  

    <script language="javascript" type="text/javascript" src="../js/tab_yiyou.js"></script>
  

</head>
<body>
        <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
            <div class="loginBox" >
                <div class="clearfix">
                    <h2 onclick="tabconAlt(0,'E',8);" id="tabE0" >天翼帐号</h2>
                    <h2 onclick="tabconAlt(1,'E',8);" id="tabE1" >号码百事通用户</h2>
                </div>
                <div class="loginTabWrapper">
                <div id="conE0" class="loginTabHide">
                    <div style="margin-top:30px;margin-bottom:17px">
			        <iframe frameborder="0"  style="filter:chroma (color=#ffffff)"  height="270px" width="100%" id="udbLoginFrame" src="<%=Login189Url %>"></iframe>
			        </div>   
   		        </div>
                <div id="conE1" class="loginTabHide">
                    <div id="errorHint" runat="server"></div>
                        <form id="form1" runat="server" onsubmit="return checkLogin();">
                            <label for="username"><span>登录名：</span><asp:TextBox ID="username" CssClass="input" runat="server"></asp:TextBox> </label>
                            <label for="password"><span>密码：</span><asp:TextBox ID="password" CssClass="input" runat="server" TextMode="Password"></asp:TextBox><a href="../FindPwdSelect.aspx?SPID=<%=SPID%>" target="_blank">忘记密码？</a></label>
                            <label for="checkCode"><span>验证码：</span><input type="text" value="" id="checkCode" name="checkCode" class="input" maxlength="4" /><img width="62" height="30" src="../ValidateToken.aspx" id="IMG2"><a href="javascript:RefreshCode()">看不清？换一张</a></label>
                          <%--  <label for="autoLogin"><input type="checkbox" class="checkbox" id="autoLogin" hidefocus="true" />下次自动登录</label>--%>
                            <asp:Button ID="login" runat="server" OnClick="login_Click"  /> 
                            <input type="hidden" value ="" id="AuthenType" name="AuthenType" />
                        </form>
                        <p>没有号百通行证？<asp:HyperLink ID="linkU1" runat="server">马上注册一个</asp:HyperLink></p>
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
                
                $(document).ready(function(){
                    var defaultTab = "<%=LoginTabCookieValue%>";
                    
                    if(defaultTab=='UDBTab'){
                        $("#tabE0").addClass("vazn");
                        $("#conE0").removeClass("loginTabHide").addClass("loginTabShow");
                    }else{
                        $("#tabE1").addClass("vazn");
                        $("#conE1").removeClass("loginTabHide").addClass("loginTabShow");
                    }
                })
                
        </script>

</body>
</html>

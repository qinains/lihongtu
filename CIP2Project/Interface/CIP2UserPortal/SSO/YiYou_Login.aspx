<%Response.AddHeader("P3P", "CP=CAO PSA OUR");%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YiYou_Login.aspx.cs" Inherits="SSO_YiYou_Login" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>号百欢迎您</title>
     
     <script type="text/javascript" src="../JS/jquery-1.4.2.min.js"></script>
      <link href="css/global_unify.css" rel="stylesheet" type="text/css"/>
 
                   <style type="text/css">
                        <!--
                        .show{display:block;}
                        .hide{display:none;}
                        .loginBox{width:310px;height:354px;position:relative;background:#f2f2f2; border: 1px solid #dcdcdc;}
                        .loginTab .tab{width:50%; float:left; line-height:40px;text-indent:50px; font-size:14px;background:none}
                        .loginTab .tab_on{background:none}
                        .loginTab .tyLogo{background-image:url(images/tyLogo8.png);background-position:12px center;background-repeat:no-repeat;}
                        .loginTab .tab_off{background-color:#e2e2e2;}
                        .loginIframe{width:100%;height:314px;background:#f2f2f2;position:absolute;}

                        .pt30{padding-top:30px;}
                        .errorHint{height:24px;}
                        .errorHint div{width:240px;margin:0 auto;display:none;background:#FDEEE9;border: 1px solid #FFC7BF;color: #FE7C48;font-size: 12px;margin-bottom: 0;padding: 2px 0 1px 10px;}
                        .loginBox form{margin-bottom:30px}
                        .loginBox .inputItem{}
                        .loginBox label{display:block;margin:0 auto;width:250px;height:40px;position:relative;border:1px solid #dcdcdc;background:#fff}
                        .loginBox .mt1{margin-top:-1px;}
                        .loginBox label span{position:absolute;color:#999999;display:block;font-family:tahoma,arial,"Hiragino Sans GB","Microsoft Yahei";font-size:12px;height:40px;top:0;left:13px;line-height:40px;opacity:1;filter:alpha(opacity=100);text-align:left;}
                        .loginBox label .input{display:block;float:left;width:230px;height:16px;padding:12px 10px;color:#333;border:none;line-height:18px;font-size:14px}
                         input#checkCode{width:50px;margin-right:5px}
                        .loginBox label .checkbox{margin-left:70px;margin-right:10px}
                        .loginBox label img{display:inline-block;vertical-align:middle;float:left}
                        .loginBox label a{line-height:40px;margin-left:5px}
                        .otherLogin a{padding:4px 0 4px 20px;margin-right:20px;background-image:url(http://static.118114.cn/images/spriteLoginRegis.png);background-repeat:no-repeat}
                        .loginBox h2{float:left;padding:0 10px;clear:none;width:145px;height:35px;line-height:35px;background:#e4e4e4;font-weight:700;font-size:14px;color:#838383; cursor:pointer;text-align:center}
                        .loginBox h2.vazn{ background:#3bb0ff;color:#fff}
                        .findPass{width:250px;margin:10px auto;text-align:right}
                        #login{width:252px;height:38px;margin:10px auto;line-height:38px;border:none;background:#FC5741;cursor:pointer;display:block;color:#fff;font-size:14px;font-family: tahoma,arial,"Hiragino Sans GB","Microsoft Yahei";}
                        #login{font-family: tahoma,arial,"Hiragino Sans GB","Microsoft Yahei"}                        
                        .loginbutton{border-radius: 2px;}
                        -->
                </style>   
    
                <% 
                    if ("35433333".Equals(SPID))
                    {
                 %>
                        <style type="text/css">
                                #login{width:252px;height:38px;margin:10px auto;line-height:38px;border:none;background:#3FA0EB;cursor:pointer;display:block;color:#fff;font-size:14px;}
                                #login{font-weight:700}
                                #login:active{background:#0b84e1;}
                        </style>
                 <%    
                    }
                    else
                    { 
                 %>
                        <style type="text/css">
                                #login{width:252px;height:38px;margin:10px auto;line-height:38px;border:none;background:#FC5741;cursor:pointer;display:block;color:#fff;font-size:14px;}
                                #login{font-weight:700}
                                #login:active{background:#e24e3a;}
                        </style>
                <%    
                    }  
                 %> 
   

</head>
<body>
        <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
  <div class="loginBox">
  <div class="loginTab">
    <div id="tytab" class="tab tyLogo">天翼帐号</div>
    <div id="bttab" class="tab tab_off">
            
            <%
                if ("35433333".Equals(SPID))
                {
            %>
               翼游账号
            <%  
                }
                else
                { 
             %>
                普通帐号
             <%    
                } 
 
             %>
            
     
     </div>
  </div>
  <div class="loginIframe">
    <iframe frameborder="0"  style="filter:chroma (color=#ffffff)"  height="314" width="100%" id="udbLoginFrame" src="<%=Login189Url %>"  scrolling="no"></iframe>
  </div>
  <div class="loginIframe hide">
    <div class="pt30">
          
                <div  id="errorHint" runat="server"></div>
           
          
         <form id="form1" runat="server" onsubmit="return checkLogin();">
   
            <div class="inputItem"><label for="username"><span>登录名</span>
            
           <asp:TextBox ID="username" CssClass="input" runat="server"></asp:TextBox>
          
             </label></div>
            <div class="inputItem mt1"><label for="password"><span>密码</span><asp:TextBox ID="password" CssClass="input" runat="server" TextMode="Password"></asp:TextBox></label></div>
            <div class="inputItem mt1"><label for="checkCode"><span>验证码</span><input type="text" value="" id="checkCode" name="checkCode" class="input" maxlength="4" /><img width="62" height="40" src="../ValidateToken.aspx" id="IMG2"><a href="javascript:RefreshCode()">看不清？换一张</a></label></div>
            <asp:Button ID="login" runat="server" CssClass="loginbutton" OnClick="login_Click"  Text="登 录" /> 
            <input type="hidden" value ="" id="AuthenType" name="AuthenType" />
          
        </form>
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
        
        
        
                  function changeTab(that,index){
                    switch(index){
                    case 0:
                      $(that).removeClass("tab_off").siblings().addClass("tab_off");
                      $(".loginIframe").eq(0).show();
                      $(".loginIframe").eq(1).hide();
                      break;
                    case 1:
                      $(that).removeClass("tab_off").siblings().addClass("tab_off");
                      $(".loginIframe").eq(1).show();
                      $(".loginIframe").eq(0).hide();
                      break;
                    }
                  }
                  
                  $(".loginTab div").click(function(){
                    changeTab(this,$(this).index());
                  });
                  
                  function hideHint(that){
                    if($(that).val()){
                        $(that).siblings("span").hide();
                      }else{
                        $(that).siblings("span").show();
                      }
                  }
                  
                  $(".input")
                    .each(function(){
                        hideHint(this);
                    })
                    .focus(function(){
                      $(this).siblings("span").css("opacity","0.6");
                    })
                    .blur(function(){
                      $(this).siblings("span").css("opacity","1");
                    })
                    .keyup(function(){
                      hideHint(this);
                    });
                   
             
                  $(document).ready(function(){
                        var defaultTab = "<%=LoginTabCookieValue%>";
                        if(defaultTab=='UDBTab'){
                                  $("#tytab").removeClass("tab_off").addClass("tab_on");
                                  $("#bttab").removeClass("tab_on").addClass("tab_off");
                                  $(".loginIframe").eq(0).show();
                                  $(".loginIframe").eq(1).hide();
                        }else{
                                  $("#bttab").removeClass("tab_off").addClass("tab_on");
                                  $("#tytab").removeClass("tab_on").addClass("tab_off");
                                  $(".loginIframe").eq(1).show();
                                  $(".loginIframe").eq(0).hide();
                        }
                    })

                    
        </script>

</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="SSO_mobile_Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>注册开户</title>
   <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" /> 
    <meta content="yes" name="apple-mobile-web-app-capable" /> 
    <meta content="black" name="apple-mobile-web-app-status-bar-style" /> 
    <meta content="telephone=no" name="format-detection" /> 
    <link href="css/base.css" rel="stylesheet" type="text/css" />
    <link href="css/reg.css" rel="stylesheet" type="text/css" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />     
     
<style type="text/css">
    #sendCode{float:left;margin:0 0 0 3px;width:92px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer}
    #sendCode:hover{text-decoration:none}
</style>    
     
     
     
     
    <script type="text/javascript" src="../../JS/jquery-1.4.2.min.js"></script> 
   <script type="text/javascript">
        var hasSend = 0; 
        var canMobileUser = "1";
        $(function(){


            $("#mobile").blur(function(){
           
                    mobilePhoneExsit();
            });

	        $("#sendCode").click(function(){

		        if(hasSend==1){
			        return false	
		        }
		        SendCheckCode();
	        });

        });
        
        //手机是否可用
        function mobilePhoneExsit(){
		        var tmp = Math.random();
		        $.get("../../ExistAuthPhone.aspx", {
			        PhoneNum: $("#mobile").val(),
			        SPID: $("#HiddenField_SPID").val(),
			        RAM: tmp,
			        typeId: 1
		        },
		        ResultMobilePhone);
	        
        }
        
        //手机是否存在返回结果
        function ResultMobilePhone(Result)
        {
	        $("#phonestate").attr("value", Result);
	        if (Result == "0"){
		        $("#hintMobile").html("该手机号可以注册").addClass("hintCorrect");
		        canMobileUser = "0";
		        return true;
	        }else{
		        $("#hintMobile").html("该手机已经被其他用户绑定").addClass("hintError");
		        canMobileUser = "1";
		        return false;
	        }
        }
        
        function SendCheckCode()
        {
            if(hasSend==1){
			    return false	
		    }
            var res = $("#phonestate").val()
	 	    if (res == '0') {
			    var tmp = Math.random();
			    $.get("../../SendAuthCode.aspx", {
				    PhoneNum: $("#mobile").val(),
				    SPID: $("#HiddenField_SPID").val(),
				    RAM: tmp,
				    typeId: 1
			    },
			    ResultCheckCode);
		    }
        }
  
        function ResultCheckCode(Result)
         {
	        hasSend=1;
	        $("#phonestate").attr("value", Result);
	        if (Result == "0"){
		        $("#hintCode").html("手机验证码已经发送");
		        codeCountDown($("#sendCode"));
	        }else if (Result == "-30004"){
		        $("#hintCode").html("超出发送次数");
		        return false;
	        }
        }
        
    //验证码倒计时
    function codeCountDown($obj) {
	    var sec = 120;
	    $obj.html("<strong id='countdown'>" + sec + "</strong>秒后可重发").css({
		    "background": "#ececec"
	    });
	    function countDown() {
		    if (sec > 1) {
			    sec--;
			    $("#countdown").text(sec);
		    } else {
			    clearInterval(countDown_Timer);
			    $("#countdown").text("0");
			    $obj.html("发送验证码").css({
				    "background": "#d3effe"
			    });
			    hasSend=0;
		    }
	    }
	    countDown_Timer = setInterval(countDown, 1000);
    }

    //重复密码验证
    function checkPasswordAgain(){
	    if ($("#password").val() != $("#password2").val()){
		    $("#password2").attr("value", "");
		
		    return false;
	    }else
	    {
	       
	        return true;
	    }
    }


    function CheckInput() {
    	if(!checkPasswordAgain())
	    {
	        return false;
	    }
	    if(canMobileUser=="1")
	    {
	        return false;
	    }
	    
    }
   </script> 
</head>
<body>
 <div style="width:300px; margin: 0 auto; padding:0 10px;">
    <form id="form1" runat="server">

        <div>
           <input id="phonestate" type="hidden" name="phonestate" value="0" />
           <input type="hidden" id="hid_openAccount" name="hid_openAccount" value="1" />
           <asp:HiddenField ID="HiddenField_SPID" runat="server" /> 
        </div>
        
        <div style="background:url(images/userreg.gif) no-repeat center -70px; height: 80px; float:left; width:100%;"></div>  
        <div style="width:100%;float:left;color:Red;line-height:30px;">
         <span id="errorHint"  runat="server"></span>
        </div>      
        <ul class="userfile">
    			<li>手机号：</li>
                <li >验证码：</li>
                <li>设置密码：</li>
                <li>确认密码：</li>
                <li>真实姓名：</li>
                <li>身份证：</li>
        </ul>        
         
           <div class="registerBox" id="userBox">
              
                <label class="label" for="mobile">
                    <input type="text" id="mobile" name="mobile"  maxlength="11"  /><em>*</em>
                </label>
                
                <label class="label" >
                    <input type="text" id="checkCode" name="checkCode" style="width:4em; float:left;" /> 
                    <a id="sendCode">发送验证码</a><em>*</em>
                </label>
                <label class="label" for="password">
                    <input type="password" id="password" name="password" class="input"  /><em>*</em>
                </label>
                <label class="label" for="password2">
                    <input type="password" id="password2" name="password2" class="input" /><em>*</em>
                </label>
       
                
                <div id="accountInfo">
                <label class="label" for="realName">
                    <input type="text" id="realName" name="realName"   maxlength="11" /><em>*</em></label>
                <label class="label" for="certnum">
                    <input type="text" id="certnum" name="certnum" maxlength="20" /><em>*</em></label>
                </div> 
            </div>
           
        <div style=" float:left; width:100%;">
          <label class="label" for="acceptUserProtocol" style=" height:70px;">
              <div style="width:20px; float:left; padding:5px 0 0 0;">
                    <input type="checkbox" class="checkbox" id="Checkbox1" checked/>
              </div>
              <div style=" float:left; width:280px;">
                    我同意<a href="protocolUser.html" target="_blank">《号百通行证用户服务协议》</a>与<br/><a href="protocolAccount.html" target="_blank">《号码百事通账户自助交易服务协议》</a>
              </div>
          </label>
        </div>
        
        <div style=" float:left; width:100%;">
          <label class="label" for="registBesttoneAccount">
          <div style="width:20px; float:left; padding:5px 0 0 0;">
                <input type="checkbox" class="checkbox" id="Checkbox2" checked />
           </div>
           <div style=" float:left;">
                 同步创建<a id="A1" style="cursor:pointer">号码百事通账户</a>
           </div>
           </label>
        </div>          
          
           <div style=" float:left; width:100%;margin:10px 0 0 0"> &nbsp;
                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/SSO/images/reg.jpg"  OnClientClick="return CheckInput();" OnClick="ImageButton1_Click" />  
           </div> 
    </form>
   </div>
  

    <script type="text/javascript">
    

            //勾选开通账户
            $("#registBesttoneAccount").click(function(){
                if($(this).attr("checked")==true){
                    $("#accountInfo").slideDown();
                    $("#hid_openAccount").attr("value","1"); 
                    $("#BestToneAccount").attr("value",$("#mobile").val());    
                     $("#besttoneBox").slideDown();
                       
                }else{
                    $("#accountInfo").slideUp();
                    $("#hid_openAccount").attr("value","0"); 
                    $("#BestToneAccount").attr("value","");        
                    $("#besttoneBox").hide();
                }
            })

 

    </script>  
    
</body>
</html>

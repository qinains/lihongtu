<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterAllInOne.aspx.cs"
    Inherits="RegisterAllInOne" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>注册开户</title>
    <link rel="stylesheet" href="css/base.css" />
     
    <style type="text/css">
   .Yiyou_header,.content{width:980px;position:relative;margin:0 auto 10px} 
     .content{width:978px;border:1px solid #8fc4e0;background:#fff}
    .registerBox{width:928px;padding:20px 30px 0 20px;background:#fff;color:#555}
    .registerBox a{color:#618ADE}
    .registerBox a:hover{text-decoration:none} 
    .registerBox p{margin-bottom:5px}
    .registerBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1;margin-bottom:30px}
    .registerBox h2 span{color:#555;margin-right:5px}
    .registerBox form{margin-bottom:30px}
    .registerBox .label{display:block;width:650px;height:35px;margin-bottom:15px}
    .registerBox .label span{display:block;float:left;margin-right:10px;width:80px;text-align:right;font-size:14px;line-height:30px}
    .registerBox .label em{line-height:30px;color:red;margin-left:5px;margin-right:10px;float:left;display:block;width:5px;height:30px}
    .registerBox .label p{line-height:30px;float:left}
    .registerBox .label p.hintCorrect{padding-left:20px;color:#390;background:url(images/spriteLoginRegister.png) no-repeat -206px 5px}
    .registerBox .label p.hintError{padding-left:20px;color:#C30;background:url(images/spriteLoginRegister.png) no-repeat -206px -21px}
    .registerBox .label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
   	.registerBox .label i{font-style:normal;float:left;margin-right:13px;line-height:30px} 
    .registerBox .radio{float:left;margin-right:5px;margin-top:9px;*margin-top:2px}	
    input#checkCode{width:50px;margin-right:5px}
    #sendCode{float:left;margin:0;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer}
    #sendCode:hover{text-decoration:none}
    #code{display:none}
    .registerBox label .checkbox{margin-left:90px;margin-right:10px}
    .registerBox label a{line-height:30px}
    .registerBox textarea{width:500px;height:140px;padding-left:5px;line-height:22px;border:1px solid #ddd;overflow:auto;margin:0px 0 20px 90px;font-size:12px;white-space:pre}
    #register{width:108px;height:38px;margin-left:108px;margin-bottom:20px;border:none;background:url(images/spriteLoginRegister.png) no-repeat 0 -38px;cursor:pointer;display:block;text-indent:-9999px}
    #besttone{width:163px;height:38px;margin-left:90px;border:none;background:url(images/spriteLoginRegister.png) no-repeat 0 -474px;cursor:pointer;display:block;text-indent:-9999px}
    .conExplain{position:absolute;width:220px;top:30px;right:100px;line-height:22px;color:#666}
    .conExplain .blue{margin-bottom:10px;color:#007bc1}
    .selectValue{width:190px;height:150px;overflow-y:scroll;margin:0;border:1px solid #ccc;position:absolute;top:32px;left:100px;background:#fff;display:none}
    .selectValue a{display:block;height:30px;line-height:30px;padding-left:5px;text-align:left;margin:0;color:#39c;text-decoration:none}
    .selectValue a:hover{background:#39c;color:#fff;text-decoration:none}
    #accountInfo{border-top:1px dashed #ccc}
   .foot{width:980px;margin:10px auto;text-align:center;color:#78797a;line-height:23px}
.foot a{color:#acadaf;text-decoration: none;}
.foot a:hover,.foot a:visited{color:#acadaf;text-decoration: none;}

  
    
    </style>

    <script type="text/javascript" src="JS/jquery-1.4.2.min.js"></script>

  <script type="text/javascript">
var globAjax_Result = 1;
var canMobileUser = "1";
var hasSend=0;
$(function() {
	$("#phonestate").attr("value", "");
	$("#checkCode").blur(function() {
		if($(this).val()!=""){
		    checkCode();
		}
	});
	$("#mobile").blur(function() {
	    if($(this).val()!=""){
		    mobilePhoneExsit();
		}
	});
	$("#password").blur(function() {
	    if($(this).val()!=""){
		    checkPassword();
		}
	});
	$("#password2").blur(function() {
	    if($(this).val()!=""){
		    checkPasswordAgain();
	    } 
	});
	
	$("#email").blur(function(){
	    if($(this).val()!=""){
	        EmailExists();
	    }
	});
	
	$("#userName").blur(function(){
	    if($(this).val()!=""){
	        UserNameExists();
	    }
	});
	
	$("#certnum").blur(function(){
	    if($(this).val()!=""){
	        isIDCode($(this).val());
	    }
	});
	
	$("#certnum").change(function(){
	    if($(this).val()!=""){
	        isIDCode($(this).val());
	    }
	});	
	
	$("#sendCode").click(function(){
 		if(hasSend==1){
			return false	
		}
		//SendPhoneAuth2($('#num').attr('value'),1);
	});
	
    var bodyHeight=$(window).height()-200;
    $(".input").focus(function(){
	    var thisTop=$(this).offset().top;
	    if(thisTop>bodyHeight){
		    $("html,body").animate({"scrollTop":thisTop-50},{duration:1000,queue:false});
		    bodyHeight=bodyHeight+thisTop-200;
	    }
    });
});
//密码验证
function checkPassword(){
	if ($("#password").val() == ""){
		$("#hintPassword").html("请输入密码").addClass("hintError");
		return false;
	}else if ($("#password").val().length < 6){
		$("#hintPassword").html("密码长度不能少于6个字符").addClass("hintError");
		return false;
	}else if (checkspace($("#password").val())){
		$("#hintPassword").html("密码不能包含空格").addClass("hintError");
		return false;
	}else{
		$("#hintPassword").html("密码输入正确").addClass("hintCorrect");
		return true;
	}
}
//重复密码验证
function checkPasswordAgain(){
	if ($("#password").val() != $("#password2").val()){
		$("#password2").attr("value", "");
		$("#hintPassword2").html("密码不一致").addClass("hintError");
		return false;
	}else
	{
	    $("#hintPassword2").html("").removeClass("hintError");
	    return true;
	}
}
function CheckInput() {
	if(canMobileUser=="1")
	{
	    return false;
	}
	
	//if(!MobilePhoneHasBindBestAccount())
	//{
    //    return false;	
	//}
	
	if(! checkPassword() )
	{
	    return false;
	}
	if(!checkPasswordAgain())
	{
	    return false;
	}
    if(! checkEmail() )
   {
        return false;
   } 
    if(!checkUserName())
   {
        return false;
   } 
    

	if (!$("#acceptUserProtocol").attr("checked")){
		return false	
	}
	if( $("#registBesttoneAccount").attr("checked")){
	    var tmp = Math.random();
        $.get("ExistBesttoneAccount_ajax.aspx",{
        PhoneNum:$("#BestToneAccount").val(),
        SPID:$("#HiddenField_SPID").val(),
        RAM:tmp,
        typeId:1}, ResultBestTonePhone);
        
        if($("#realName").val()=="")
        {
            $("#hint_realName").addClass("hintError");
            return false; 
        }else
        {
            $("#hint_realName").removeClass("hintError");
        }    
        

        
        if($("#certnum").val()=="")
        {
            $("#hint_certnum").addClass("hintError");
           return false; 
        }else
        {
            if(!isIDCode($("#certnum").val() ))
            {
                return false;
            }else
            {
                $("#hint_certnum").removeClass("hintError");
                return true;
            } 
        }      
	}
	return true;
}

//验证邮箱  可以为空
function checkEmail(){
            if($("#email").val() != "")
            {
                    var regMail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
                    if(!regMail.test($("#email").val()))
                    {
                        $("#hint_email").addClass("hintError");
                        return false;
                    }
                    else
                    {
                        $("#hint_email").removeClass("hintError").addClass("hintCorrect");
                        return true;
                    }
            }else
           {
                return true;
           } 
}

//验证用户名 可以为空
function checkUserName(){
   if($("#userName").val()!="")
   {
        var regUserName=/^[a-zA-Z][a-zA-Z0-9]*([-._]?[a-zA-Z0-9]+)*$/;
        if(!regUserName.test($("#userName").val()))
        {
            $("#hint_userName").addClass("hintError");
            return false; 
        }
        else
        {
            $("#hint_userName").removeClass("hintError").addClass("hintCorrect");
            return true; 
        }
   }else{
        return true;
   } 
}

//手机格式验证
function mobilePhone() {
	var regMobile = /^1([3][0-9]|[5][0123456789]|[8][0123456789])\d{8}$/;
	if (!regMobile.test($("#mobile").val())) {
		$("#hintMobile").html("请输入正确格式的手机号码").addClass("hintError");
		return false;
	} else {
		$("#hintMobile").html("输入正确").addClass("hintCorrect").removeClass("hintError");
	}
	return true;
}
//验证码验证
function checkCode(){
	if ($("#phonestate").val() == "0"){
		if ($("#checkCode").val() == ""){
			$("#hintCode").html("请输入手机验证码").addClass("hintError");
			$("#checkCode").focus();
			return false;
		}
	}	
}

//手机号码是否已经被别的用户开过户
function MobilePhoneHasBindBestAccount(){
    if(mobilePhone()){
        var tmp = Math.random();
        $.get("ExistBesttoneAccount_ajax.aspx",{
            PhoneNum: $("#mobile").val(),
 			SPID: $("#HiddenField_SPID").val(),
			RAM: tmp,
			typeId: 1           
        },ResultMobilePhoneHasBindBestAccount);
    }
}

function ResultMobilePhoneHasBindBestAccount(){
	$("#phonestate").attr("value", Result);
	if (Result == "0"){
		$("#hintMobile").html("该手机号可以注册").addClass("hintCorrect");
		canMobileUser = "0";
		return true;
	}else{
		$("#hintMobile").html("该手机已经被其他用户绑定了号码百事通账户").addClass("hintError");
		canMobileUser = "1";
		return false;
	}
}

//手机是否可用-是否已经是别的客户的认证手机，或者说已经被注册过
function mobilePhoneExsit(){
	if(mobilePhone()){
		var tmp = Math.random();
		$.get("ExistAuthPhone.aspx", {
			PhoneNum: $("#mobile").val(),
			SPID: $("#HiddenField_SPID").val(),
			RAM: tmp,
			typeId: 1
		},
		ResultMobilePhone);
	}
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

//检查用户名是否已经存在
function UserNameExists()
{
       if( $("#userName").val()!="" && checkUserName())
       {
                var tmp = Math.random();
		        $.get("ExistUsername_ajax.aspx", {
			        UserName: $("#userName").val(),
			        SPID: $("#HiddenField_SPID").val(),
			        RAM: tmp,
			        typeId: 1
		        },
		        ResultUserNameExists);           
       } 

}

function ResultUserNameExists(Result)
{
    if(Result=="0")
    {
          $("#hint_usename").html("该用户名可以注册").addClass("hintCorrect");
          return true; 
    }
    else
    {
         $("#hint_usename").html("该用户名已经被注册").addClass("hintError");
         return false; 
    } 
}

//检查邮箱是否已经存在
function EmailExists()
{
       if(  $("#email").val()!="" &&  checkEmail())
       {
                var tmp = Math.random();
		        $.get("EmailAuth_ajax.aspx", {
			        Mail: $("#email").val(),
			        SPID: $("#HiddenField_SPID").val(),
			        RAM: tmp,
			        typeId: 1
		        },
		        ResultEmailExists);         
       } 
}

function ResultEmailExists(Result)
{
    if(Result=="0")
    {
        $("#hint_email").html("该邮箱可以注册").addClass("hintCorrect"); 
        return true; 
    }
    else
    {
        $("#hint_email").html("该邮箱已经被注册").addClass("hintError");
        return false;
    }   

}

//发送验证码
function SendPhoneAuth1(){
  $("#num").attr("value",1);
	if (canMobileUser=="0"  ){
		var res = $("#phonestate").val()
	 	if (res == '0') {
			var tmp = Math.random();
			$.get("PutAuthenCodeForRegisterAllInOne.aspx", {
				PhoneNum: $("#mobile").val(),
				SPID: $("#HiddenField_SPID").val(),
				count:$("#num").attr("value"),
				RAM: tmp,
				typeId: 1
			},
			ResultCheckCode1);
		}
	}
}

function SendPhoneAuth2(s,t){
   $("#num").attr("value",parseInt(s)+t); 
   var count =   parseInt($("#num").val());
   if(count>2){
        alert('发送次数超过2次！'); 
        return false;  
   }
   
	if (canMobileUser=="0"  ){
		var res = $("#phonestate").val();
	 	if (res == '0') {
			var tmp = Math.random();
			$.get("PutAuthenCodeForRegisterAllInOne.aspx", {
				PhoneNum: $("#mobile").val(),
				SPID: $("#HiddenField_SPID").val(),
				count:$("#num").attr("value"),
				RAM: tmp,
				typeId: 1
			},
			ResultCheckCode1);
		}
	}
}

//验证码发送返回结果
function ResultCheckCode1(Result100)
 {
	hasSend=1;
	$("#phonestate").attr("value", Result100);
	if (Result100 == "0"){
		$("#hintCode").html("手机验证码已经发送").addClass("hintCorrect");
		codeCountDown($("#sendCode"));
	}else if (Result100 == "-30004"){
		$("#hintCode").html("超出发送次数").addClass("hintError");
		return false;
	}
}

  function checkspace(str) 
  { 
            if(str.indexOf(" ")!=-1)
            {
                return true; 
            }
            return false;
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

//身份证验证
function isIDCode(num) {
    var len = num.length, re;
    if (len == 15){
        re = new RegExp(/^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$/); 
    }else if (len == 18){ 
        re = new RegExp(/^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})(\d|X)$/); 
    }else {
            $("#hint_certnum").html("身份证号非法!").addClass("hintError");
            return false;
    }

    var a = num.match(re);
    if (a != null) {
        if (len == 15) {
            var D = new Date("19" + a[2] + "/" + a[3] + "/" + a[4]);
            var B = D.getYear() == a[2] && (D.getMonth() + 1) == a[3] && D.getDate() == a[4];
        }
        else {
            var D = new Date(a[2] + "/" + a[3] + "/" + a[4]);
            var B = D.getFullYear() == a[2] && (D.getMonth() + 1) == a[3] && D.getDate() == a[4];

            //check code verify
            var w = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
            var x = "10X98765432";
            var cc = 0;
            for (var i = 0; i < 17; i++) {
                cc = cc + (num.charAt(i) * w[i]);
            }
            cc = cc % 11;
            if (a[6] != x.charAt(cc)) { $("#hint_certnum").html("身份证号非法!").addClass("hintError");  return false; }
        }

        if (!B) {  $("#hint_certnum").html("身份证号非法!").addClass("hintError");    return false; }
    }
    else {
        $("#hint_certnum").html("身份证号非法!").addClass("hintError"); 
        return false;
    }
    $("#hint_certnum").html("身份证号合法!").addClass("hintCorrect").removeClass("hintError");  
    return true;
}


</script>

</head>
<body>

 <asp:Panel ID="PanelForYiYouHeader" runat="server">
     <div class="Yiyou_header">
        <img src="images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
    </div>
 </asp:Panel>

 <asp:Panel ID="PanelForYiGouHeader" runat="server">
     <div class="Yiyou_header">
        <img src="images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
     </div>  
 </asp:Panel>

    <div class="content">
        <form id="form1" runat="server">
            <div class="registerBox" id="userBox">
                <h2>
                   <span>注册成为</span>号码百事通会员</h2>
                <label class="label" for="mobile">
                    <span>手机号：</span><input type="text" id="mobile" name="mobile" class="input" maxlength="11"  /><input id="phonestate" type="hidden" name="phonestate" value="" /><em>*</em><p id="hintMobile">
                        请输入11位手机号码</p>
                </label>
                <label class="label" >
                    <span>验证码：</span><input type="text" id="checkCode" name="checkCode" class="input" /><a id="sendCode">发送验证码</a><em>*</em><p id="hintCode" runat="server">请输入收到的验证码</p>
                </label>
                <label class="label" for="password">
                    <span>设置密码：</span><input type="password" id="password" name="password" class="input"  /><em>*</em><p
                        id="hintPassword">
                        6-12位，可以由英文、数字或符号组成，不能含空格</p>
                </label>
                <label class="label" for="password2">
                    <span>确认密码：</span><input type="password" id="password2" name="password2" class="input" /><em>*</em><p
                        id="hintPassword2">
                        请重复输入密码</p>
                </label>
                
                <label class="label" for="userName">
                    <span>用户名：</span><input type="text" id="userName" name="userName" class="input" /><em></em><p id="hint_userName">可用于登录，由字母和数字组成，必须字母开头</p>
                </label>                
                
                 <label class="label" for="email">
                    <span>邮箱：</span><input type="text" id="email" name="email" class="input" /><em></em><p id="hint_email">可用于登录、找回密码。格式：xxx@abc.com</p>
                </label>                
                <input type="hidden" id="hid_openAccount" name="hid_openAccount" value="0" />
                <input name="num" id="num" size="3" value="0" style="display: none;" />
                <asp:HiddenField ID="HiddenField_SPID" runat="server" />
                <asp:HiddenField ID="HiddenField_URL" runat="server" />
                <asp:HiddenField ID="myCustID" Value="" runat="server" />
                <asp:HiddenField ID="hidCheckMobile" runat="server" />
                <asp:HiddenField ID="myReturnUrl" Value="" runat="server" />
                <label class="label" for="acceptUserProtocol"><input type="checkbox" class="checkbox" id="acceptUserProtocol" checked/>我同意<a href="protocolUser.html" target="_blank">《号百通行证用户服务协议》</a></label> 
                <label class="label" for="registBesttoneAccount"><input type="checkbox" class="checkbox" id="registBesttoneAccount"  />我同意<a href="protocolAccount.html" target="_blank">《号码百事通账户自助交易服务协议》</a>并同步创建<a id="whatBesttone" style="cursor:pointer">号码百事通账户</a></label>
                <div id="accountInfo" class="none">
                        <p style="font-size:14px;padding-left:90px;color:#390;height:30px;line-height:30px">以下信息创建后不可随意更改，请填写真实信息。</p>
                        <label class="label" for="realName">
                            <span>真实姓名：</span><input type="text" id="realName" name="realName" class="input"
                                maxlength="11" /><em>*</em><p id="hint_realName">请填写真实姓名 </p></label>

                        <label class="label" for="certnum">
                            <span>身份证：</span><input type="text" id="certnum" name="certnum" class="input" maxlength="20" /><em>*</em><p id="hint_certnum">请填写正确的身份证号，以确保能够正常使用账户功能。</p></label>
                 </div>  
            </div>
            <asp:Button ID="register" runat="server"  OnClientClick="return CheckInput();"  OnClick="register_Click" />
        </form>
    </div>


    <asp:Panel ID="PanelForYiYouFooter" runat="server">
        <script type="text/javascript" src="JS/register_foot.js"></script>
    </asp:Panel>

    <asp:Panel ID="PanelForYiGouFooter" runat="server">
         <div class="foot">
            <p>中国电信集团：<a href="">189邮箱</a> | <a href="">天翼宽带</a> | <a href="">号百商旅</a> | <a href="">号百导航</a> | <a href="">天翼手机网</a> | <a href="">翼支付</a> | <a href="">爱音乐</a> | <a href="">天翼视讯</a> | <a href="">协同通讯</a> | <a href="">物联网</a> | <a href="">天翼空间</a> | <a href="">天翼阅读</a> | <a href="">爱游戏</a> | <a href="">爱动漫</a></p>
            违法和不良信息举报 <a href="">service@118114.cn</a>   copyright© 2007-2011 号百商旅电子商务有限公司版权所有<br />   
            增值电信业务经营许可证：沪B2-20110026　沪ICP备11017770号  上海工商标识编号：20110624111724289
        </div>  
    
   
    </asp:Panel>




    <script type="text/javascript">
    
            //解释号百账户
            $("#whatBesttone").hover(function(){
	            var html="<div style='position:absolute;width:350px;padding:5px;border:1px solid #39c;background:#F6F6F6;left:"+($(this).offset().left+100)+"px;top:"+$(this).offset().top+"px' id='explainBesttone'>号码百事通账户是针对<strong>翼游旅行网</strong>和<strong>翼购商城</strong>的专享账户，可购买网站上机票、旅游线路、景点门票、各地特产和日用商品等各项产品。<p class='clearfix'><span class='fl'>合作伙伴：天翼电子商务有限公司</span><span style='color:#39c;float:right'>客服热线：4008-011-888</span></p></div>";
	            $("body").append(html);
            },function(){
	            $("#explainBesttone").remove();
            })
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

            function ResultBestTonePhone(Result)
                {
                    globAjax_Result = Result;
                    if(Result != 0) {
                       $("#accountInfo").html('<p style="font-size:14px;margin-left:90px;color:#c00;height:30px;line-height:30px">您的手机号码已开通号码百事通账户，无法同步创建号码百事通账户。</p>'); 
                       $("#registBesttoneAccount").removeAttr("checked");
                    }
                }       


    </script>
</body>
</html>

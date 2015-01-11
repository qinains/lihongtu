<%Response.AddHeader("P3P", "CP=CAO PSA OUR");%>
<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_RegisterInLowstint, App_Web_registerinlowstint.aspx.8268bb4f" enableEventValidation="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>会员注册</title>
    <link rel="stylesheet" href="../css/base.css" />
   
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
     input#checkCode{width:50px;margin-right:5px} 
    .conExplain{position:absolute;width:220px;top:30px;right:100px;line-height:22px;color:#666}
    .conExplain .blue{margin-bottom:10px;color:#007bc1}
    .selectValue{width:190px;height:150px;overflow-y:scroll;margin:0;border:1px solid #ccc;position:absolute;top:32px;left:100px;background:#fff;display:none}
    .selectValue a{display:block;height:30px;line-height:30px;padding-left:5px;text-align:left;margin:0;color:#39c;text-decoration:none}
    .selectValue a:hover{background:#39c;color:#fff;text-decoration:none}
    #accountInfo{border-top:1px dashed #ccc}
   .foot{width:980px;margin:10px auto;text-align:center;color:#78797a;line-height:23px}
    .foot a{color:#acadaf;text-decoration: none;}
    .foot a:hover,.foot a:visited{color:#acadaf;text-decoration: none;}
    
   
 	#passwordStrengthDiv{margin-top:6px;}

	.is0{background:url(images/progressImg1.png) no-repeat 0 0;width:138px;height:7px;}
	.is10{background-position:0 -7px;}
	.is20{background-position:0 -14px;}
	.is30{background-position:0 -21px;}
	.is40{background-position:0 -28px;}
	.is50{background-position:0 -35px;}
	.is60{background-position:0 -42px;}
	.is70{background-position:0 -49px;}
	.is80{background-position:0 -56px;}
	.is90{background-position:0 -63px;}
	.is100{background-position:0 -70px;} 
    
    </style> 
    
   <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
   <script language="javascript" type="text/javascript" src="../JS/jquery.passwordStrength.js"></script> 
   <script language="javascript" type="text/javascript">
  
    $(document).ready(function(){
   		
   		var $pwd =$("#password");
		$pwd.passwordStrength();
		
        $("#userName").blur(function(){

	        if($(this).val()!=""){
	            checkUserName();
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

     })
  
        
  
          function checkUserName(){
              $("#hint_userName").html("");
               if($("#userName").val()!="")
               {
                   
                    if ($("#userName").val().length < 5 || $("#userName").val().length >25){
                        $("#hint_userName").html("长度必须在 5-25").removeClass("hintCorrect").addClass("hintError");
                        $("#userName").focus();
                        return false; 
                    }else{
                        $("#hint_userName").html(""); 
                    }  
                   
                    if(checkspace($("#userName").val())){
                      
                        $("#hint_userName").html("不能有空格").removeClass("hintCorrect").addClass("hintError");
                        $("#userName").focus();
                        return false; 
                    }else{
                        $("#hint_userName").html("");  
                    } 
                    if($("#userName").val().indexOf('@')!=-1){
                       
                        $("#hint_userName").html("不能有@符号").removeClass("hintCorrect").addClass("hintError");
                        $("#userName").focus();
                        return false;
                    }else{
                        $("#hint_userName").html("");  
                    }
                    var regUserName=/^[a-zA-Z][a-zA-Z0-9]*([-._]?[a-zA-Z0-9]+)*$/;     //由字母和数字组成，必须字母开头
                    if(!regUserName.test($("#userName").val()))
                    {
                       
                        $("#hint_userName").html("必须由字母和数字组成，字母开头").removeClass("hintCorrect").addClass("hintError") ;
                        $("#userName").focus();
                        return false; 
                    }else{
                        $("#hint_userName").html("");  
                    }
                    UserNameExists();  //用户名判重
               }else{
                      $("#hint_userName").html(""); 
                      $("#hint_userName").html("用户名不能为空").removeClass("hintCorrect").addClass("hintError");
                      $("#userName").focus();
                      return false;
                    
               } 
        }
  
  
  
         //检查用户名是否已经存在
        function UserNameExists()
        {
            
               if( $("#userName").val()!="" )
               {
                        var tmp = Math.random();
		                $.get("../ExistUsername_ajax.aspx", {
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
                  $("#hint_userName").removeClass("hintError");
                  $("#hint_userName").html("该用户名可以注册").addClass("hintCorrect") ;  
                  return true; 
            }
            else
            {
                $("#hint_userName").removeClass("hintCorrect");
                $("#hint_userName").html("该用户名已经被注册").addClass("hintError") ; 
                $("#userName").focus();
                return false; 
            } 
        }
  
  
        function checkspace(str)  { 
            if(str.indexOf(" ")!=-1)
            {
                return true; 
            }
            return false;
        }
          //密码验证
        function checkPassword(){
            $("#hintPassword").html("");
	        if ($("#password").val() == ""){
		        $("#hintPassword").html("请输入密码").removeClass("hintCorrect").addClass("hintError");
		        return false;
	        }else if ($("#password").val().length < 6){
		        $("#hintPassword").html("密码长度不能少于6个字符").removeClass("hintCorrect").addClass("hintError");
		        return false;
	        }else if (checkspace($("#password").val())){
		        $("#hintPassword").html("密码不能包含空格").removeClass("hintCorrect").addClass("hintError");
		        return false;
	        }else{
		        $("#hintPassword").html("密码输入正确").removeClass("hintError").addClass("hintCorrect");
		        return true;
	        }
        }
        
  
         //重复密码验证
        function checkPasswordAgain(){
            $("#hintPassword2").html("");
	        if ($("#password").val() != $("#password2").val()){
		        $("#password2").attr("value", "");
		        $("#hintPassword2").html("密码不一致").removeClass("hintCorrect").addClass("hintError");
		        return false;
	        }else
	        {
	            $("#hintPassword2").html("").removeClass("hintError");
	            return true;
	        }
        }
  
        function CheckCheckCode(){
            if($("#checkCode").val()!=""){
                 return true;
            }else{
                 return false; 
            }
        }
  
  	    function RefreshCode(){
            document.all.IMG2.src = '../ValidateToken.aspx?.tmp='+Math.random();
        }
        
        
        
        function CheckInput() {
      
        
              if(! checkPassword()){
                 $("#password").focus(); 
                  return false;
                  // 这里应个提示 
              }
 
              if(!checkPasswordAgain()){
                  $("#password2").focus(); 
                   return false;
                  // 这里应个提示 
              }
             
              if(!CheckCheckCode()){
                  $("#checkCode").focus(); 
                  return false;
                  // 这里应个提示 
              } 
              return true;
        }

        
   </script>
</head>
<body>

    <asp:Panel ID="PanelForYiYouHeader" runat="server">
         <div class="Yiyou_header">
            <img src="../images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="../images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
        </div>
     </asp:Panel>

     <asp:Panel ID="PanelForYiGouHeader" runat="server">
         <div class="Yiyou_header">
            <img src="../images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="../images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
         </div>  
     </asp:Panel>
     
   <div class="content">     
     
     <form id="form1" runat="server">
              <div class="registerBox" id="userBox">
                <h2><span>注册成为</span>号码百事通会员</h2>

                <label class="label" for="userName">
                    <span>用户名：</span><input type="text" id="userName" name="userName" class="input" /><em>*</em><p id="hint_userName">可用于登录，由字母和数字组成，必须字母开头</p>
                </label>         

                <label class="label" for="password">
                    <span>设置密码：</span><input type="password" id="password" name="password" class="input"  /><em>*</em><p id="hintPassword">6-12位，可以由英文、数字或符号组成，不能含空格</p> 
                </label>
             
                <div style="margin: 0px 0px 18px 90px; " id="passwordStrengthDiv" class="is0"></div>
                
                <label class="label" for="password2">
                    <span>确认密码：</span><input type="password" id="password2" name="password2" class="input" /><em>*</em><p
                        id="hintPassword2">
                        请重复输入密码</p>
                </label>

                <label class="label" for="checkCode">
                    <span>验证码：</span><input type="text" value="" id="checkCode" name="checkCode" class="input" maxlength="4" /><em>*</em>
                     <img width="62" height="30" src="../ValidateToken.aspx" id="IMG2" alt=""/><a href="javascript:RefreshCode()">看不清？换一张</a>
                     
                </label>
                <span id="errorHint"  runat="server"></span>
                <asp:HiddenField ID="HiddenField_SPID" runat="server" />
                </div>
                <asp:Button ID="register" runat="server" Text="注册"    OnClientClick="return CheckInput();"    OnClick="register_Click" />

    </form>
   
  </div> 
  
      <asp:Panel ID="PanelForYiYouFooter" runat="server">
        <script type="text/javascript" src="../JS/register_foot.js"></script>
    </asp:Panel>

    <asp:Panel ID="PanelForYiGouFooter" runat="server">
         <div class="foot">
            <p>中国电信集团：<a href="">189邮箱</a> | <a href="">天翼宽带</a> | <a href="">号百商旅</a> | <a href="">号百导航</a> | <a href="">天翼手机网</a> | <a href="">翼支付</a> | <a href="">爱音乐</a> | <a href="">天翼视讯</a> | <a href="">协同通讯</a> | <a href="">物联网</a> | <a href="">天翼空间</a> | <a href="">天翼阅读</a> | <a href="">爱游戏</a> | <a href="">爱动漫</a></p>
            违法和不良信息举报 <a href="">service@118114.cn</a>   copyright© 2007-2011 号百商旅电子商务有限公司版权所有<br />   
            增值电信业务经营许可证：沪B2-20110026　沪ICP备11017770号  上海工商标识编号：20110624111724289
        </div>  
    </asp:Panel>
    
</body>
</html>

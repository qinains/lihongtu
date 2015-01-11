<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_M_LowSintRegisterM, App_Web_lowsintregisterm.aspx.7caca394" enableEventValidation="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" /> 
    <meta content="yes" name="apple-mobile-web-app-capable" /> 
    <meta content="black" name="apple-mobile-web-app-status-bar-style" /> 
    <meta content="telephone=no" name="format-detection" />     
   
  
    <link rel="stylesheet" type="text/css" href="css2014/global.css"/>   
     <link rel="stylesheet" type="text/css" href="css2014/lihongtu.css"/>    
     <script type="text/javascript" src="../../JS/jquery-1.4.2.min.js"></script>
     
     <script type="text/javascript">
     
        $(document).ready(function(){
            $(".chushi").focus(function(){
			    $(this).val("").removeClass("chushi");
		    });
		    
            $("#UserName").blur(function(){
	            if($(this).val()!=""){
	                checkUserName();
	            }
	        });
            
           
	        $("#Password").blur(function() {
	            if($(this).val()!=""){
		            checkPassword();
		        }
	        });
	
	        $("#Password2").blur(function() {
	            if($(this).val()!=""){
		            checkPasswordAgain();
	            } 
	        });            
            
        })
     
     
         function showError(str,callback){
	            var html="<div style='width:100%;position:absolute;top:0;left:0;z-index:9999;height:"+$(window).height()+"px' id='J-mask'></div>";
	                  html+="<div id='J-error' style='width:200px;padding:0 10px;line-height:40px;background:rgba(0,0,0,0.7);color:#fff;text-align:center;position:absolute;top:"+($(window).scrollTop()+$(window).height()/2-20)+"px;left:50%;margin-left:-110px;;z-index:10000;-moz-border-radius:5px;-webkit-border-radius:5px;border-radius:5px'>";
	                  html+="<p>"+str+"</p></div>";
	                  $("body").append(html);
	                  setTimeout(function(){
		              $("#J-mask").remove();
		              $("#J-error").remove();
		              if(callback){
			                callback();
		              }
	                },2000)
         }
     
        //检查用户名是否已经存在
        function UserNameExists()
        {
               if( $("#UserName").val()!="" )
               {
                        var tmp = Math.random();
                        $.get("../../ExistUsername_ajax.aspx", {
                            UserName: $("#UserName").val(),
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
                return true; 
            }
            else
            {
                showError("该用户名已经被注册");
                $("#UserName").focus();
                return false; 
            } 
        }     
     
     
               function checkUserName(){
          
               if($("#UserName").val()!=""  && $("#UserName").val()!="由字母和数字组成，必须字母开头")
               {
                   
                    if ($("#UserName").val().length < 5 || $("#UserName").val().length >25){
                        showError("用户名长度必须在 5-25");
                        $("#UserName").focus();
                        return false; 
                    }
                   
                    if(checkspace($("#UserName").val())){
                        showError("用户名不能有空格");
                        $("#UserName").focus();
                        return false; 
                    }
                    if($("#UserName").val().indexOf('@')!=-1){
                        showError("用户名不能有@符号");
                        $("#UserName").focus();
                        return false;
                    }
                    var regUserName=/^[a-zA-Z][a-zA-Z0-9]*([-._]?[a-zA-Z0-9]+)*$/;     //由字母和数字组成，必须字母开头
                    if(!regUserName.test($("#UserName").val()))
                    {
                        showError("用户名必须由字母和数字组成，字母开头");
                        $("#UserName").focus();
                        return false; 
                    }
                    UserNameExists();  //用户名判重
               }else{
                      showError("用户名不能为空");
                      $("#UserName").focus();
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
	        if ($("#Password").val() == ""){
		         showError("请输入密码");
		        return false;
	        }else if ($("#Password").val().length < 6){
	            showError("密码长度不能少于6个字符"); 
		        return false;
	        }else if (checkspace($("#Password").val())){
	            showError("密码不能包含空格"); 
		        return false;
	        }else{
	            return true;
	        }
        }
        
  
         //重复密码验证
        function checkPasswordAgain(){
	        if ($("#Password").val() != $("#Password2").val()){
		        $("#Password2").attr("value", "");
		        showError("密码不一致"); 
		        return false;
	        }else
	        {
	            return true;
	        }
        }     
        
        function CheckInput() {
            
             if($("#UserName").val()==""  || $("#UserName").val()=="由字母和数字组成，必须字母开头"){
                showError("请输入用户名!"); 
                $("#UserName").focus();
                return false;
             }
        
              if(! checkPassword()){
                 $("#Password").focus(); 
                  return false;
              }
 
              if(!checkPasswordAgain()){
                  $("#Password2").focus(); 
                   return false;
              }
              return true;
        }     
        
        </script>
     
     
    <title>注册</title>
</head>
<body>
    <form id="form1" runat="server">
         <div class="top">
          <a href="#" class="back"></a>
          <p>注册</p>
        </div>
        <div class="wrap" style=" padding:0 5px; background:#fafafa;">
          <div>
            <ul>
              <li>
                <div class="file">用户名：</div>
                <div class="input">
                  <input name="UserName" type="text" id="UserName" value="由字母和数字组成，必须字母开头" class="chushi" />
                </div>
              </li>
              <li>
                <div class="file">密 码：</div>
                <div class="input">
                  <input name="Password" type="password" id="Password" value="6-12位，不含空格" class="chushi" />
                </div>
              </li>
              <li>
                <div class="file">确认密码：</div>
                <div class="input">
                  <input name="Password2" type="password" id="Password2" value="请重复输入密码" class="chushi" />
                </div>
              </li>
            </ul>
          </div>
          <div style=" margin:30px 0; text-align:center">
              
              <asp:Button ID="BtnSubmit"  CssClass="bt_bo"  runat="server" Text="提 交"  OnClientClick="return CheckInput();" OnClick="BtnSubmit_Click" />
           </div>
        </div>
        <asp:HiddenField ID="HiddenField_SPID" runat="server" />
        <div id="errorHint" style="display:none" runat="server"></div>
    </form>
</body>
</html>

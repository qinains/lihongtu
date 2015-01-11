// JScript 文件


//hintCorrect

        $(document).ready(function(){
            //清空状态
            //$("#acceptDeal").attr("checked",'true');
            $("#registBesttoneAccount").attr("checked",'true');
            $("#phonestate").attr("value","");
            
            $("#checkCode").blur(function(){
                if($("#phonestate").attr("value") == "0")
                {
                        if($("#checkCode").attr("value") == "")
                        {
                            $("#hintCode").html("请输入手机验证码");
                            $("#hintCode").attr("class","hintError");
                        }
                        else
                        {
                            $("#hintCode").html("");
                        }
                        
                 }
            });
          
             //验证手机号码
            $("#mobile").blur(function(){
                if($("#mobile").attr("value") != "")
                {
                    var reg = /^1([3][0-9]|[5][012356789]|[8][056789])\d{8}$/;
                    if(!reg.test($("#mobile").attr("value")))
                    {
                        $("#hintMobile").html("请输入正确的手机号码");
                        $("#hintMobile").attr("class","hintError");
                    }
                    else
                    {
                        //判断手机是否已经被绑定 
                        var tmp = Math.random();
                        $.get("ExistAuthPhone.aspx",{
                        PhoneNum:$("#mobile").attr("value"),
                        SPID:$("#HiddenField_SPID").attr("value"),
                        RAM:tmp,
                        typeId:1}, ResultMobilePhone);
                    
                        //$("#hintMobile").html("");  // 
                    }
                }
            });         
          
          
         
            //判断密码为空和密码是否少于6位字符,密码中是否包含空格
            $("#password").blur(function(){
                if($("#password").attr("value") == "")
                {
                    $("#hintPassword").html("请输入密码");
                    $("#hintPassword").attr("class","hintError");
                }
                else if($("#password").attr("value").length < 6)
                {
                    $("#hintPassword").html("密码长度不能少于6个字符");        
                    $("#hintPassword").attr("class","hintError");
                }
                else if(checkspace($("#password").attr("value")))
                {
                    $("#hintPassword").html("密码不能包含空格"); 
                    $("#hintPassword").attr("class","hintError");
                }
                else
                {
                    $("#hintPassword").html("");
                }                
            });

            //判断重复密码
            $("#password2").blur(function(){
                if($("#password").attr("value") != $("#password2").attr("value"))
                {
                    $("#password").attr("value","");
                    $("#password2").attr("value","");
                    $("#hintPassword2").html("密码不一致");
                    $("#hintPassword2").attr("class","hintError");
                }
                else
                {
                    $("#hintPassword2").html("");
                }
            });
            
        });
        
        //用户验证ajax返回值
        function ResultMobilePhone(Result)
        {
            $("#phonestate").attr("value",Result);
            if(Result == 0)
            {
                $("#hintMobile").html("该手机号可以注册");
                $("#hintMobile").attr("class","hintCorrect");
            }
            else
            {
               $("#hintMobile").html("该手机已经被其他用户绑定"); 
               $("#hintMobile").attr("class","hintError");
            }
        }        
        

        //输入验证
        function CheckInput()
        {
       
     
            if($("#registBesttoneAccount").attr("checked")==true)
            {
                $("#hid_openAccount").attr("value","1");
            }else{
                $("#hid_openAccount").attr("value","0");
            }

            
            if($("#mobile").attr("value") == "")
            {
                $("#mobile").focus();
                $("#hintMobile").html("请输入手机号码");
                $("#hintMobile").attr("class","hintError");
                return false;
            }
            
            
            else if($("#password").attr("value") == "")
            {
                $("#password").focus();
                $("#hintPassword").html("请输入密码");
                $("#hintPassword").attr("class","hintError");
                return false;
            }
            else if($("#password").attr("value").length < 6)
            {
                $("#password").focus();
                $("#hintPassword").html("密码长度不能少于6个字符");
                $("#hintPassword").attr("class","hintError");
                return false;
            }
            else if(checkspace($("#password").attr("value")))
            {
                $("#password").focus();
                $("#hintPassword").html("密码不能包含空格"); 
                $("#hintPassword").attr("class","hintError");
                return false;
            }
            else if($("#password").attr("value") != $("#password2").attr("value"))
            {
                $("#password").attr("value","");
                $("#password2").attr("value","");
                $("#password").focus();
                $("#hintPassword2").html("密码不一致");
                $("#hintPassword2").attr("class","hintError");
                return false;
            }
            
            if($("#mobile").attr("value") != "")
            {

                var reg = /^(1[0-9][0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;
                if(!reg.test($("#mobile").attr("value")))
                {
                    $("#mobile").focus();
                    $("#hintMobile").html("请输入正确的手机号码");
                    $("#hintMobile").attr("class","hintError");
                    return false;
                }
                
                if($("#phonestate").attr("value") == "0")   // 只有验证了手机号码没有被绑定的情况下才有条件发验证码
                {
                    if($("#checkCode").attr("value") == "")
                    {
                        $("#hintCode").html("请输入手机验证码");
                        $("#hintCode").attr("class","hintError");
                        $("#checkCode").focus();
                        return false;
                    }
                    

                }
                if($("#phonestate").attr("value") != 0)
                {
                    $("#hintMobile").html("该手机号码已经被其他用户绑定");
                    $("#hintMobile").attr("class","hintError");
                    $("#mobile").focus();
                    return false;
                }

            }
         return true;
        }
        
        //手机认证
        //判断手机号码是否为空
        function IsPhone()
        {
            if($("#mobile").attr("value") == "")
            {
                $("#hintMobile").html("请输入手机号码");
                return false;
            }
            return true;
        }
        
        //发送手机验证码ajax
        function SendPhoneAuth()
        {
        
            if($("#mobile").attr("value") != "")
            {
                var reg = /^1([3][0-9]|[5][012356789]|[8][056789])\d{8}$/;
                if(!reg.test($("#mobile").attr("value")))
                {
                    $("#hintMobile").html("请输入正确的手机号码");
                    $("#hintMobile").attr("class","hintError");
                }
                else
                {
                    //判断手机是否已经被绑定 
                    var tmp = Math.random();
                    $.get("ExistAuthPhone.aspx",{
                    PhoneNum:$("#mobile").attr("value"),
                    SPID:$("#HiddenField_SPID").attr("value"),
                    RAM:tmp,
                    typeId:1}, ResultMobilePhone);
                
                    //$("#hintMobile").html("");  // 
                }
            }       
        
            var res = $("#phonestate").attr("value")
            if(res=='0'){
            
               //$("#identifying_code").show();
                //$("#identifying_codeState").attr("value", "open");
                var tmp = Math.random();
                //alert(tmp);
                $.get("SendAuthCode.aspx",{
                    PhoneNum:$("#mobile").attr("value"),
                    SPID:$("#HiddenField_SPID").attr("value"),
                    RAM:tmp,
                    typeId:1}, ResultCheckCode);            
            }
 
        }
        //ajax手机验证码返回值
        function ResultCheckCode(Result)
        {       
            $("#phonestate").attr("value",Result);
            if(Result == 0)
            {
                $("#hintCode").html("手机验证码已经发送");
                $("#hintCode").attr("class","hintCorrect");
            }
			else if(Result == "-30004")
            {
                $("#hintCode").html("超出发送次数");
                $("#hintCode").attr("class","hintError");
            }
            
        }
        
     
        function ChangePhoneState()
        {
            $("#phonestate").attr("value","");
            //alert($("#phonestate").attr("value"));
        }
        

      
        function checkspace(str) 
        { 
            if(str.indexOf(" ")!=-1)
            {
                return true; 
            }
            return false;
        }

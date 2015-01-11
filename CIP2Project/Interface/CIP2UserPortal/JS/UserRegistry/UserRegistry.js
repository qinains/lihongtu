// JScript 文件  --- WEB页面注册

        $(document).ready(function(){
            //清空状态
            $("#userstate").attr("value","");
            $("#phonestate").attr("value","");
            //判断用户名为空
            $("#username").blur(function(){
            if($("#username").attr("value") == "")
            {
                $("#err_username").html("请输入用户名");
            }
            else
            {
                //判断用户名是否已经存在 
                var tmp = Math.random();
                $.get("ExistUserName_ajax.aspx",{
                UserName:$("#username").attr("value"),
                RAM:tmp,
                typeId:1}, ResultUser);
                //$("#err_username").html("");
            }
            });
            //判断真实姓名
            $("#fullname").blur(function(){
            if($("#fullname").attr("value") == "")
            {
                $("#err_fullname").html("请输入真实姓名");
            }
            else
            {
                $("#err_fullname").html("");
            }
            });
            //判断密码为空和密码是否少于6位字符,密码中是否包含空格
            $("#password").blur(function(){
                if($("#password").attr("value") == "")
                {
                    $("#err_password").html("请输入密码");
                }
                else if($("#password").attr("value").length < 6)
                {
                    $("#err_password").html("密码长度不能少于6个字符");        
                }
                else if(checkspace($("#password").attr("value")))
                {
                    $("#err_password").html("密码不能包含空格"); 
                }
                else
                {
                    $("#err_password").html("");
                }                
            });

            //判断重复密码
            $("#password_OK").blur(function(){
                if($("#password").attr("value") != $("#password_OK").attr("value"))
                {
                    $("#password").attr("value","");
                    $("#password_OK").attr("value","");
                    $("#err_password_OK").html("密码不一致");
                }
                else
                {
                    $("#err_password_OK").html("");
                }
            });
            //验证手机号码
            $("#telephone").blur(function(){
                if($("#telephone").attr("value") != "")
                {
                    var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;
                    if(!reg.test($("#telephone").attr("value")))
                    {
                        $("#err_telephone").html("请输入正确的手机号码");
                    }
                    else
                    {
                        $("#err_telephone").html("");
                    }
                }
            });
            //验证邮箱
            $("#email").blur(function(){
                 if($("#email").attr("value") != "")
                 {
                    var regMail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
                    if(!regMail.test($("#email").attr("value")))
                    {
                        $("#err_email").html("请输入正确的邮件地址");
                    }
                    else
                    {
                        $("#err_email").html("");
                    }
                 }
            });
            
            //验证日期
            $("#birthday").blur(function(){
                if($("#birthday").attr("value") != "")
                {
                    if(!IsDate($("#birthday").attr("value")))
                    {
                        $("#err_birthday").html("日期格式不正确");
                    }
                    else
                    {
                         $("#err_birthday").html("");
                    }
                }
            });
            
            
        });
        //用户验证ajax返回值
        function ResultUser(Result)
        {
            $("#userstate").attr("value",Result);
            if(Result == 0)
            {
                $("#err_username").html("");
            }
            else
            {
               $("#err_username").html("该用户名已经存在"); 
            }
        }
        //输入验证
        function CheckInput()
        {
            if($("#username").attr("value") == "")
            {
                $("#username").focus();
                $("#err_username").html("请输入用户名");
                return false;
            }
            else if( $("#userstate").attr("value") != 0)
            {
                $("#username").focus();
                $("#err_username").html("该用户名已经存在");
                return false;
            }
            else if($("#fullname").attr("value") == "")
            {
                $("#fullname").focus();
                $("#err_fullname").html("请输入真实姓名");
                return false;
            }
            else if($("#password").attr("value") == "")
            {
                $("#password").focus();
                $("#err_password").html("请输入密码");
                return false;
            }
            else if($("#password").attr("value").length < 6)
            {
                $("#password").focus();
                $("#err_password").html("密码长度不能少于6个字符");
                return false;
            }
            else if(checkspace($("#password").attr("value")))
            {
                $("#password").focus();
                $("#err_password").html("密码不能包含空格"); 
                return false;
            }
            else if($("#password").attr("value") != $("#password_OK").attr("value"))
            {
                $("#password").attr("value","");
                $("#password_OK").attr("value","");
                $("#password").focus();
                $("#err_password_OK").html("密码不一致");
                return false;
            }
            
            if($("#telephone").attr("value") != "")
            {
//                var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;
 var reg = /^(1[0-9][0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;
                if(!reg.test($("#telephone").attr("value")))
                {
                    $("#telephone").focus();
                    $("#err_telephone").html("请输入正确的手机号码");
                    return false;
                }
                
                if($("#phonestate").attr("value") == "0")
                {
                    if($("#phone_code").attr("value") == "")
                    {
                        $("#err_phone_code").html("请输入手机验证码");
                        $("#phone_code").focus();
                        return false;
                    }
                    else if($("#page_code").attr("value") == "")
                    {
                        $("#err_page_code").html("请输入页面验证码");
                        $("#page_code").focus();
                        return false;
                    }

                }
                if($("#phonestate").attr("value") != 0)
                {
                    $("#err_telephone").html("该手机号码已经被其他用户绑定");
                    $("#telephone").focus();
                    return false;
                }

            }
            
            if($("#email").attr("value") != "")
            {
                var regMail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
                if(!regMail.test($("#email").attr("value")))
                {
                    $("#email").focus();
                    $("#err_email").html("请输入正确的邮件地址");
                    return false;
                }
            }
            
            if($("#birthday").attr("value") != "")
            {
            //alert($("#birthday").attr("value"));
                if(!IsDate($("#birthday").attr("value")))
                {
                    $("#birthday").focus();
                    $("#err_birthday").html("日期格式不正确");
                    return false;
                }
            }
            
            return true;
        }
        
        //手机认证
        //判断手机号码是否为空
        function IsPhone()
        {
            if($("#telephone").attr("value") == "")
            {
                $("#err_telephone").html("请输入手机号码");
                return false;
            }
            return true;
        }
        
        //发送手机验证码ajax
        function SendPhoneAuth()
        {
            $("#identifying_code").show();
            $("#identifying_codeState").attr("value", "open");
            var tmp = Math.random();
            //alert(tmp);
            $.get("PhoneAuth_ajax.aspx",{
                PhoneNum:$("#telephone").attr("value"),
                SPID:$("#HiddenField_SPID").attr("value"),
                RAM:tmp,
                typeId:1}, ResultPhone);
        }
        //ajax手机验证码返回值
        function ResultPhone(Result)
        {
            $("#phonestate").attr("value",Result);
            //alert($("#phonestate").attr("value"));
            if(Result == 0)
            {
                $("#err_telephone").html("手机验证码已经发送");
            }
			else if(Result == "-30004")
            {
                $("#err_telephone").html("超出发送次数");
            }
            else
            {
                $("#err_telephone").html("该手机号码已经被其他用户绑定");
            }
        }
        
        //验证码验证
        function codeBlur()
        {
            if($("#phonestate").attr("value") == "0")
            {
                    if($("#phone_code").attr("value") == "")
                    {
                        $("#err_phone_code").html("请输入手机验证码");
                    }
                    else
                    {
                        $("#err_phone_code").html("");
                    }
                    if($("#page_code").attr("value") == "")
                    {
                        $("#err_page_code").html("请输入页面验证码");
                    }
                    else
                    {
                        $("#err_page_code").html("");
                    }
             }

        }
        //邮件认证
        //判断邮箱是否为空
        function IsEmail()
        {
            if($("#email").attr("value") == "")
            {
                $("#err_email").html("请输入邮箱");
                return false;
            }
            return true;
        }
        
        //发送邮件认证码ajax
        function SendEmailAuth()
        {
            $.get("EmailAuth_ajax.aspx",{
                Mail:$("#email").attr("value"),
                SPID:$("#HiddenField_SPID").attr("value"),
                typeId:1}, ResultEmail);
        }
        //ajax邮箱验证码返回值
        function ResultEmail(Result)
        {
            $("#emailstate").attr("value",Result);
            //alert( $("#emailstate").attr("value"));
            if(Result == 0)
            {
                $("#err_email").html("邮箱验证码已经发送");
            }
            else 
            {
                $("#err_email").html("该邮箱已经被其他用户绑定");
            }
        }
        
     
        function ChangePhoneState()
        {
            $("#phonestate").attr("value","");
            //alert($("#phonestate").attr("value"));
        }
        
        function ChangeMailState()
        {
            $("#emailstate").attr("value","");
        }
        
        //判断日期格式是否正确
        function IsDate(mystring)
        {  
            var reg = /^(\d{4})-(\d{2})-(\d{2})$/;  
            var str = mystring;
            if (str=="")   return   true;  
            if (!reg.test(str))
            {  
                return false;  
            }  
            return true;  
      } 
      
    function checkspace(str) 
    { 
        if(str.indexOf(" ")!=-1)
        {
            return true; 
        }
        return false;
    }


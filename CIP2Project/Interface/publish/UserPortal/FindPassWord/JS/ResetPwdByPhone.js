$(document).ready(function(){
    $("#txtPwd").blur(function(){
        var pwd = $("#txtPwd").val();
        if(pwd == ""){
            $("#pwdError_tip").css("display","block");
        }else{
            $("#pwdError_tip").css("display","none");
        }
    })
    
    $("#txtPwdSecond").blur(function(){
        var PwdSecond = $("#txtPwdSecond").val();
        if(PwdSecond == ""){
            $("#pwdSecondError_tip").css("display","block");
        }else{
            $("#pwdSecondError_tip").css("display","none");
        }
    })
    
    $("#txtCode").blur(function(){
        var code = $("#txtCode").val();
        if(code == ""){
            $("#codeError_msg").text("不能为空");
            $("#codeError_tip").css("display","block");
        }else{
            $("#codeError_tip").css("display","none");
        }
    })
    
    $("#btnSave").click(function(){
        var pwd = $("#txtPwd").val();
        var PwdSecond = $("#txtPwdSecond").val();
        var code = $("#txtCode").val();
        if(pwd == ""){
            $("#pwdError_tip").css("display","block");
        }else{
            $("#pwdError_tip").css("display","none");
        }
        if(PwdSecond == ""){
            $("#pwdSecondError_tip").css("display","block");
        }else{
            $("#pwdSecondError_tip").css("display","none");
        }
        if(PwdSecond!=""){
            if(pwd!=PwdSecond){
                $("#pwdSecondError_tip").css("display","block");
                $("#pwdSecondError_msg").text("密码输入有误");
                return false;
            }else{
                $("#pwdSecondError_tip").css("display","none");
                $("#pwdSecondError_msg").text("不能为空");
            }
        }
        
        if(code == ""){
            $("#codeError_msg").text("不能为空");
            $("#codeError_tip").css("display","block");
        }else{
            $("#codeError_tip").css("display","none");
        }
        
        if(pwd=="" || PwdSecond=="" || code==""){
            return false;
        }
        
        var time = new Date();
        var type = "ResetPwdByPhone";       //密码类型
        var CustID = $("#hdCustID").val();
        var ReturnUrl = $("#hdReturnUrl").val();
        var pwdType = $("#hdPwdType").val();
        
        //如果是语音密码设置的话需要验证语音密码
        if(pwdType=="1"){
            var length= pwd.length;
            if(isNaN(pwd) || length !=6){
                alert("语音密码只能是6位数字");
                return false;
            }
        }

        $.ajax({
            type:"post",
            dataType: "json",//返回json格式的数据
            url:"../HttpHandler/ResetPwdHandler.ashx",            
            data:{CustID:CustID,PassWord:pwd,Code:code,Type:type,PwdType:pwdType,time:time.getTime()},
            beforeSend: function(XMLHttpRequest) {
                $("#loadImg").css("display", "block");
                $("#btnSave").css("display","none");
            },
            complete: function(XMLHttpRequest, textStatus) {
                $("#loadImg").css("display", "none");
                $("#btnSave").css("display","block");
            },
            success:function(data,testStatus){
                $.each(data,function(index,item){
                    if(item["result"]=="false"){
                        if(item["msg"]=="验证码错误"){
                            $("#codeError_msg").text("验证码错误");
                            $("#codeError_tip").css("display","block");
                        }else{
                            $("#codeError_tip").css("display","none");
                            $("#codeError_msg").text("不能为空");
                        }
                        RefreshCode();
                    }else{
                        //alert("密码修改成功");
                        window.location.href="Success.aspx?ReturnUrl="+encodeURI(ReturnUrl);
                    }
                })
            },
            error:function(text){
                //alert(text);
            }
        })
    })
})

        
        
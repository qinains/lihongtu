$(document).ready(function(){
//    $("#txtName").blur(function(){
//        var username = $("#txtName").val();
//        if(username == ""){
//            $("#nameError_tip").css("display","block");
//        }else{
//            $("#nameError_tip").css("display","none");
//        }
//    })
    
    $("#txtEmail").blur(function(){
        var email = $("#txtEmail").val();
        if(email == ""){
            $("#emailError_tip").css("display","block");
        }else{
            $("#emailError_tip").css("display","none");
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
        //var username = $("#txtName").val();
        var email = $("#txtEmail").val();
        var code = $("#txtCode").val();
//        if(username == ""){
//            $("#nameError_tip").css("display","block");
//        }else{
//            $("#nameError_tip").css("display","none");
//        }
        if(email == ""){
            $("#emailError_tip").css("display","block");
        }else{
            $("#emailError_tip").css("display","none");
        }
        if(code == ""){
            $("#codeError_msg").text("不能为空");
            $("#codeError_tip").css("display","block");
        }else{
            $("#codeError_tip").css("display","none");
        }
        
//        if(username=="" || email=="" || code==""){
//            return false;
//        }
        if(email=="" || code==""){
            return false;
        }
        var returnUrl =$("#hdReturnUrl").val();
        var time = new Date();
        $.ajax({
            type:"post",
            dataType: "json",//返回json格式的数据
            url:"../HttpHandler/FindPwdByEmailHandler.ashx",            
            data:{Email:email,Code:code,ReturnUrl:returnUrl,time:time.getTime()},
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
                    if(item["result"]=="true"){
                        window.location.href='SendMailSuccess.aspx?ReturnUrl='+encodeURI(returnUrl);
                    }else if(item["result"]=="CodeError"){
                        $("#codeError_msg").text("验证码错误");
                        $("#codeError_tip").css("display","block");
                    }else if(item["result"]=="EmailError"){
                        $("#emailError_msg").text("认证邮箱有误，请重新输入");
                        $("#emailError_tip").css("display","block");
                    }else{
                        alert(item["msg"]);
                    }
                    RefreshCode();
                })
            },
            error:function(text){
                alert(text);
            }
        })
        
        
    })
})

        
        

$(document).ready(function(){
    var objUsername=$("#username");
    var objRealname=$("#realname");
    var objPassword=$("#password");
    var objPassword_OK=$("#password_OK");
    var objCertno=$("#certno");
    var objProvince=$("#stext");
    var objArea=$("#resulttxt");
    
    $("#realname,#username,#password,#password_OK,#certno").bind("blur",function(){
        /*=============用户名================*/
        if($(this).attr("id")=="username"){
            $("#usernameError_tip").css("display","block");
            if(objUsername.val()==""){
                username_error = 1;
                $("#usernameError_msg").text("不能为空");
                $("#usernameImg").css("display","block");
                $("#usernameCheckImg").css("display","none");
                $("#usernameOKImg").css("display","none");
            }else{
                username_error = 0;
                /*===============检验用户名是否重复=======================*/
                var time = new Date();
                $.ajax({
                    type:"POST",
                    dataType:"json",
                    url:"HttpHandler/CheckUserNameHandler.ashx",
                    data:{username:objUsername.val(),time:time.getTime()},
                    beforeSend:function(XMLHttpRequest){
                        $("#usernameImg").css("display","none");
                        $("#usernameOKImg").css("display","none");
                        $("#usernameCheckImg").css("display","block");
                    },
                    complete:function(XMLHttpRequest, textStatus){
                        $("#usernameCheckImg").css("display","none");
                    },
                    success:function(data,testStatus){
                        $.each(data,function(index,item){
                            $("#usernameError_msg").text(item["info"]);
                            if(item["result"]=="true"){
                                $("#usernameOKImg").css("display","block");
                            }else{
                                $("#usernameImg").css("display","block");
                            }
                        })
                    },
                    error:function(){
                        
                    }
                })
            }
        }else{
            IsEmptyValidate($(this));
                
            if($(this).attr("id")=="password"){
                //密码规则验证
                
            }else if($(this).attr("id")=="password_OK"){
                if(objPassword.val()!="" && objPassword_OK.val()!="" && objPassword.val()!=objPassword_OK.val()){
                    $("#password_OKError_tip").css("display","block");
                    $("#password_OKError_msg").text("密码有误");
                }else{
                    
                }
            }
        }
        
    })
    
    /*================保存按钮==================*/
    $("#btnSave").bind("click",validateFun);
    
    var validateFun = function validateFun(){
        var error_Flag = 0;
        $("#usernameError_tip").css("display","block");
        if(objUsername.val()==""){
            $("#usernameError_msg").text("不能为空");
            $("#usernameImg").css("display","block");
            $("#usernameCheckImg").css("display","none");
            $("#usernameOKImg").css("display","none");
            error_Flag = 1;
        }else{
            /*===============检验用户名是否重复=======================*/
            var time = new Date();
            $.ajax({
                type:"POST",
                dataType:"json",
                url:"HttpHandler/CheckUserNameHandler.ashx",
                data:{username:objUsername.val(),time:time.getTime()},
                beforeSend:function(XMLHttpRequest){
                    $("#usernameImg").css("display","none");
                    $("#usernameOKImg").css("display","none");
                    $("#usernameCheckImg").css("display","block");
                },
                complete:function(XMLHttpRequest, textStatus){
                    $("#usernameCheckImg").css("display","none");
                },
                success:function(data,testStatus){
                    $.each(data,function(index,item){
                        $("#usernameError_msg").text(item["info"]);
                        if(item["result"]=="true"){
                            $("#usernameOKImg").css("display","block");
                        }else{
                            error_Flag = 1;
                            $("#usernameImg").css("display","block");
                        }
                    })
                },
                error:function(){
                    
                }
            })
        }
        
        if(IsEmptyValidate(objRealname)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objPassword)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objPassword_OK)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objCertno)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objProvince)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objArea)=="1"){
            error_Flag = 1;
        }
        
        if(objPassword.val()!="" && objPassword_OK.val()!="" && objPassword.val()!=objPassword_OK.val()){
            $("#password_OKError_tip").css("display","block");
            $("#password_OKError_msg").text("密码有误");
            error_Flag = 1;
        }
        if(error_Flag == 1){
            alert("有错误");
        }else{
            alert("OK");
        }
    }
})


/*==============非空验证===============*/
function IsEmptyValidate(obj){
    var tipobj=$("#"+obj.attr("id")+"Error_tip");
    var msgobj=$("#"+obj.attr("id")+"Error_msg");
    //证件非空验证
    if(obj.attr("id") == "certno"){
        var certificateType = $("#hdCertificateState").val();
        if(certificateType == "1" && $("#certno").val() == ""){
            $("#certnoError_tip").css("display","block");
            return 1;
        }else{
            $("#certnoError_tip").css("display","none");
            return 0;
        }
    }else if(obj.attr("id")=="stext"){
        if(obj.val()=="-999" || obj.val() ==""){
            tipobj.css("display","block");
            msgobj.text("请选择");
            return 1;
        }else{
            tipobj.css("display","none");
            msgobj.text("");
            return 0;
        }
    }else if(obj.attr("id")=="resulttxt"){
        if(obj.val()=="-999" || obj.val() ==""){
            tipobj.css("display","block");
            msgobj.text("请选择");
            return 1;
        }else{
            tipobj.css("display","none");
            msgobj.text("");
            return 0;
        }
    }else{
        if(obj.val()==""){
            tipobj.css("display","block");
            msgobj.text("不能为空");
            return 1;
        }else{
            tipobj.css("display","none");
            msgobj.text("");
            return 0;
        }
    }
}

/*==============密码验证===============*/
function PassWordValidate(password){
    
}
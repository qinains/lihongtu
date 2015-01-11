
$(document).ready(function(){
    var objUsername=$("#username");
    var objPassword=$("#password");
    var objPassword_OK=$("#password_OK");
    var objProvince=$("#stext");
    var objArea=$("#resulttxt");
    var objPhone=$("#telephone");
    var objEmail=$("#email");
    var objPhoneCode=$("#phone_code");
    var objPageCode=$("#page_code");
    var IsPhoneAuthen=$("#hdIsPhoneAuthen");
    var IsEmailAuthen=$("#hdIsEmailAuthen");
    var hdRnadomCustID=$("#hdRandomCustID");
    
    
    $("#username,#password,#password_OK").bind("blur",function(){
        /*=============用户名================*/
        if($(this).attr("id")=="username"){
            $("#usernameError_tip").css("display","block");
            if(objUsername.val()==""){
                $("#usernameError_msg").text("不能为空");
                $("#usernameImg").css("display","block");
                $("#usernameCheckImg").css("display","none");
                $("#usernameOKImg").css("display","none");
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
    $("#btnSave").bind("click",function(){
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
                            validateFun();
                        }else{
                            $("#usernameImg").css("display","block");
                        }
                    })
                },
                error:function(){
                    
                }
            })
        }
    });
    
    var validateFun = function validateFun(){
        var error_Flag = 0;
        
        if(IsEmptyValidate(objPassword)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objPassword_OK)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objProvince)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objArea)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objPhoneCode)=="1"){
            error_Flag = 1;
        }
        if(IsEmptyValidate(objPageCode)=="1"){
            error_Flag = 1;
        }
        
        if(objPassword.val()!="" && objPassword_OK.val()!="" && objPassword.val()!=objPassword_OK.val()){
            $("#password_OKError_tip").css("display","block");
            $("#password_OKError_msg").text("密码有误");
            error_Flag = 1;
        }
        if(error_Flag == 1){
            return false;
        }else{
            saveFun();
        }
        
    }
    /*=================开始异步保存数据====================*/
    var saveFun = function saveFun(){
        var time = new Date();
        $.ajax({
            type:"post",
            url:"HttpHandler/SaveRegisterInfoHandler.ashx",
            dataType:"json",
            data:{UserName:objUsername.val(),PassWord:objPassword.val(),ProvinceID:objProvince.val(),AreaID:objArea.val(),Email:objEmail.val(),
                    EmailState:IsEmailAuthen.val(),PhoneNum:objPhone.val(),PhoneState:IsPhoneAuthen.val(),PhoneCode:objPhoneCode.val(),PageCode:objPageCode.val(),time:time.getTime()},
            beforeSend: function(XMLHttpRequest) {
                $("#loadImg").css("display", "block");
                $("#btnSave").css("display","none");
            },
            complete: function(XMLHttpRequest, textStatus) {
                $("#loadImg").css("display", "none");
                $("#btnSave").css("display","block");
            },
            success:function(data,textStatus){
                $.each(data,function(index,item){
                    if(item["result"]=="false"){
                        alert(item["info"]);
                    }else{
                        alert(item["info"]);
                    }
                })
            },
            error:function(data){
                alert("保存失败");
            }
        })
    }
})


/*==============非空验证===============*/
function IsEmptyValidate(obj){
    var tipobj=$("#"+obj.attr("id")+"Error_tip");
    var msgobj=$("#"+obj.attr("id")+"Error_msg");
    //省
    if(obj.attr("id")=="stext"){
        if(obj.val()=="-999" || obj.val() ==""){
            tipobj.css("display","block");
            msgobj.text("请选择");
            return 1;
        }else{
            tipobj.css("display","none");
            msgobj.text("");
            return 0;
        }//地区市
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
    }else if(obj.attr("id")=="phone_code"){
        var IsAuthen = $("#hdIsPhoneAuthen").val();
        if(IsAuthen=="0"){
            if(obj.val()==""){
                tipobj.css("display","block");
                msgobj.text("不能为空");
                return 1;
            }else{
                tipobj.css("display","none");
                msgobj.text("");
                return 0;
            }
        }else{
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